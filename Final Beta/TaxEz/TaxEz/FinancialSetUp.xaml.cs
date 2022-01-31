using System;
using System.Collections.Generic;
using System.IO;
using TaxEz.Models;
using Newtonsoft.Json.Linq;

using Xamarin.Forms;

namespace TaxEz
{
    public partial class FinancialSetUp : ContentPage
    {
        public FinancialSetUp()
        {
            InitializeComponent();
            SubmitButton.Clicked += SubmitButton_Clicked;
        }

        private async void SubmitButton_Clicked(object sender, EventArgs e)
        {
            if(StatePicker.SelectedItem == null || EntryGrossTotal.Text == null)
            {
                await DisplayAlert("Empty Entries", "Do not leave any of the entries empty.", "OK");
            }
            else
            {
                string state = StatePicker.SelectedItem.ToString();
                Tabs.CurrentUser.State = state;

                string userDataFile = Tabs.CurrentUser.UserDataFile;
                using (StreamWriter sw = File.AppendText(userDataFile))
                {
                    sw.WriteLine(state);
                }

                //TODO make sure that user input is actually valid
                //info being written to file
                decimal grossIncome = decimal.Parse(EntryGrossTotal.Text);
                int hoursWorked = 0;
                string job = "Initial";

                string userIncomeFile = Tabs.CurrentUser.UserIncomeFile;
                using (StreamWriter sw = new StreamWriter(userIncomeFile))
                {
                    sw.WriteLine($"{job};{grossIncome};{hoursWorked}");
                }
                await DisplayAlert("Successful Submission", "Your financial set up was submited successfully", "OK");
                //TODO reword message

                Application.Current.MainPage = new NavigationPage(new Tabs());
            }

           
        }
    }
}
