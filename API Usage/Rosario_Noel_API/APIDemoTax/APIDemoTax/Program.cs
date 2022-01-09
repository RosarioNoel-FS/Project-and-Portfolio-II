using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace APIDemoTax
{
    class MainClass
    {
        //httpClient is what is used to send a request to recieve an end point
        HttpClient client = new HttpClient();

        static async Task Main(string[] args)
        {   //create instance of class to call method
            MainClass mainClass = new MainClass();
            await mainClass.GetToDoItems();
        }

        private async Task GetToDoItems()
        {
            //Post Request
            //income tax
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJBUElfS0VZX01BTkFHRVIiLCJodHRwOi8vdGF4ZWUuaW8vdXNlcl9pZCI6IjYxZDVjM2NkNDgzODU2NDc2YWE5OWVkMSIsImh0dHA6Ly90YXhlZS5pby9zY29wZXMiOlsiYXBpIl0sImlhdCI6MTY0MTM5OTI0NX0.d9ZiG1SgrYORLeGoJelubzMg2r7-cFyZg1K7WWsiRhk");
            //how to add perams for a post request

            //add params into a dictionary
            Dictionary<string, string> paramDictionary = new Dictionary<string, string> { { "year", "2020" }, { "pay_rate", "1000" }, { "filing_status", "single" }, { "state", "NY" } };
            //encoding params so it can be used for the request
            FormUrlEncodedContent encodedContent = new FormUrlEncodedContent(paramDictionary);

            //make request
            HttpResponseMessage response = await client.PostAsync("https://taxee.io/api/v2/calculate/2020", encodedContent);

            //returns conten AKA json from server //still needs to be converted as a json
            string Jstring = await response.Content.ReadAsStringAsync();

            JObject jsonObject = JObject.Parse(Jstring);

            //{"annual":{"fica":{"amount":1530},"federal":{"amount":760},"state":{"amount":499.75}}}


            Console.WriteLine(jsonObject["annual"]["federal"]["amount"]); //This will return just the federal amount of "760"
                                                                          //we access into annuel and then access into federal and then access into amount

            //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            //Get
            //federal tax api

            //send request await response 
            HttpResponseMessage federalreturn = await client.GetAsync("https://taxee.io/api/v2/federal/2020");

            string federalTaxBracketsString = await federalreturn.Content.ReadAsStringAsync();

            JObject federalTaxJson = JObject.Parse(federalTaxBracketsString);

            //Console.WriteLine(federalTaxJson);

            Console.WriteLine(federalTaxJson["single"]["income_tax_brackets"]);

            //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

            //Get
            //State Tax api

            //send request await response 
            HttpResponseMessage statereturn = await client.GetAsync("https://taxee.io/api/v2/state/2020/NY");

            string stateTaxBracketsString = await statereturn.Content.ReadAsStringAsync();

            JObject stateTaxJson = JObject.Parse(stateTaxBracketsString);

            //Console.WriteLine(stateTaxJson);

            //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        }
    }
}