using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using TaxEz.Models;
using System.Threading.Tasks;

using Xamarin.Forms;
using System.ComponentModel;

namespace TaxEz
{
    public partial class HistoryTab : ContentPage
    {
        ObservableCollection<TaxData> taxHistoryList = new ObservableCollection<TaxData>(new List<TaxData>());

        public decimal TotalGrossIncome { get; set; }
        public decimal TotalTakeHomeAmount { get; set; }
        private decimal TotalTaxAmount { get; set; }

        public HistoryTab()
        {
            InitializeComponent();

            listView.ItemsSource = taxHistoryList;
            //when a submission occurs UI is updated and taxdata is added to the observable collection
            MessagingCenter.Subscribe<TaxData>(this, "NewSubmission", async (sender) =>
            {
                await AddNewTaxEntry(sender.PreTaxAmount);
            });

            LoadIncomeHistoryFromFile();
        }
        //empty task is = void
        //handles the gross income submission and assigns appropriate values to the UI by adding entry to the ObservableCollection
        private async Task AddNewTaxEntry(decimal grossIncome)
        {
            //adding submission to the total gross
            TotalGrossIncome += grossIncome;
            //retrieve taxed amount based on the total gross income
            decimal totalTax = await APIHanler.GetTaxAmount(TotalGrossIncome, Tabs.CurrentUser.State);
            //Take the new taxed total and subtract the previous taxed total (Put Away in view)
            decimal thisEntryTax = totalTax - TotalTaxAmount;

            TotalTaxAmount = totalTax;

            TaxData entry = new TaxData()
            {
                PreTaxAmount = grossIncome,
                Deducted = thisEntryTax,
                TakeHomeAmount = grossIncome - thisEntryTax
            };
            taxHistoryList.Add(entry);

            TotalTakeHomeAmount = TotalGrossIncome - TotalTaxAmount;

            LabelGrossTotal.Text = $"Total Gross Income: {TotalGrossIncome:C}";
            LabelTakeHomeTotal.Text = $"Total Take Home: {TotalTakeHomeAmount:C}";
        }
        //combined with the AddNewTaxEntry every line from the file (submissions) are applied with tax and results are loaded

        //takes care of financial set up

        //read file to get old submissions and add
        private async void LoadIncomeHistoryFromFile()
        {
            using (StreamReader sr = new StreamReader(Tabs.CurrentUser.UserIncomeFile))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] data = line.Split(';');
                    decimal grossIncome = decimal.Parse(data[1]);
                    await AddNewTaxEntry(grossIncome);
                }
            }
        }//add takes place in message center < load outsode of message center
    }
}
