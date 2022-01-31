using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
namespace TaxEz.Models
{
    public static class APIHanler
    {
        private static JToken federalTaxBrackets;
        private static JToken stateTaxBrackets;

        private async static Task<JToken> CallAPI(string apiEndpoint)
        {
            using (var httpClientHandler = new HttpClientHandler())
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) =>
                {
                    return true;
                };
                using (var httpClient = new HttpClient(httpClientHandler))
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJBUElfS0VZX01BTkFHRVIiLCJodHRwOi8vdGF4ZWUuaW8vdXNlcl9pZCI6IjYxZDVjM2NkNDgzODU2NDc2YWE5OWVkMSIsImh0dHA6Ly90YXhlZS5pby9zY29wZXMiOlsiYXBpIl0sImlhdCI6MTY0MTM5OTI0NX0.d9ZiG1SgrYORLeGoJelubzMg2r7-cFyZg1K7WWsiRhk");
                    //Make API call
                    var apiReturn = await httpClient.GetAsync(apiEndpoint);
                    //get content from API
                    string apiReturnStr = await apiReturn.Content.ReadAsStringAsync();
                    //parse content as a Jobject
                    var apiJson = JObject.Parse(apiReturnStr);
                    return apiJson["single"]["income_tax_brackets"];
                }
            }
        }

        //get federal tax rate from Json
        private static decimal GetTaxAmountForFederal(JToken incometaxBracket, decimal grossIncome)
        {
            if (incometaxBracket == null)
            {
                return 0m;
            }
            decimal amountToTax = 0m;
            decimal taxRate = 0m;
            //amount taxed from previous brackets
            decimal prevTaxed = 0m;
            foreach (var bracket in incometaxBracket)
            {
                decimal bracketValue = decimal.Parse(bracket["bracket"].ToString());
                //compare user submission to tack brackets in order to find appropriate tax bracket placement
                if (grossIncome < bracketValue)
                {   //break when submission is in the correct tax bracket
                    break;
                }
                amountToTax = grossIncome - bracketValue;
                taxRate = decimal.Parse(bracket["marginal_rate"].ToString()) / 100m;
                prevTaxed = decimal.Parse(bracket["amount"].ToString());
            }
            decimal totalTaxedAmount = amountToTax * taxRate + prevTaxed;
            return totalTaxedAmount;
        }

        //get state tax rate from Json
        private static decimal GetTaxAmountForState(JToken incometaxBracket, decimal grossIncome)
        {
            if (incometaxBracket == null)
            {
                return 0m;
            }
            decimal leftOverGross = grossIncome;
            decimal totalTax = 0m;
            JArray brackets = (JArray)incometaxBracket;
            for (int i = brackets.Count - 1; i >= 0; i--)
            {
                decimal bracket = decimal.Parse(brackets[i]["bracket"].ToString());
                if (leftOverGross > bracket)
                {
                    decimal thisBracketGross = leftOverGross - bracket;
                    decimal taxRate = decimal.Parse(brackets[i]["marginal_rate"].ToString()) / 100m;
                    decimal thisBracketTax = thisBracketGross * taxRate;
                    totalTax += thisBracketTax;
                    leftOverGross = bracket;
                }
            }
            return totalTax;
        }

        //Get take home amount
        public static async Task<decimal> GetTaxAmount(decimal grossIncome, string state)
        {
            //API pulls
            if (federalTaxBrackets == null)
            {
                federalTaxBrackets = await CallAPI("https://taxee.io/api/v2/federal/2020");
            }
            if (stateTaxBrackets == null)
            {
                stateTaxBrackets = await CallAPI($"https://taxee.io/api/v2/state/2020/{state}");
            }
            //State and Federal tax rates
            decimal federalTaxes = GetTaxAmountForFederal(federalTaxBrackets, grossIncome);
            decimal stateTaxes = GetTaxAmountForState(stateTaxBrackets, grossIncome);
            //total amount taxed
            return federalTaxes + stateTaxes;
        }

        public static void resetCache()
        {
            stateTaxBrackets = null;
        }

       
    }
}
