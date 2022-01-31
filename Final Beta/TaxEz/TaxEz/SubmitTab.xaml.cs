using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using TaxEz.Models;

using Xamarin.Forms;

namespace TaxEz
{
    public partial class SubmitTab : ContentPage
    {

        public SubmitTab()
        {
            InitializeComponent();
            SubmitButton.Clicked += SubmitButton_Clicked;
        }

        private async void SubmitButton_Clicked(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(EntryGrossSubmission.Text)
            || string.IsNullOrWhiteSpace(EntryHoursWorked.Text)
            || string.IsNullOrWhiteSpace(EntryJobName.Text)
            )
            {
                await DisplayAlert("Empty Entries", "Do not leave any of the entries empty.", "OK");
                EntryGrossSubmission.Text = null;
                EntryHoursWorked.Text = null;
                EntryJobName.Text = null;
            }
            else
            {
                string jobName = EntryJobName.Text;
                decimal grossIncome = decimal.Parse(EntryGrossSubmission.Text);
                int hoursWorked = int.Parse(EntryHoursWorked.Text);
                using (StreamWriter sw = File.AppendText(Tabs.CurrentUser.UserIncomeFile))
                {
                    sw.WriteLine($"{jobName};{grossIncome};{hoursWorked}");
                }
                TaxData taxData = new TaxData()
                {
                    JobName = jobName,
                    PreTaxAmount = grossIncome,
                    HoursWorked = hoursWorked
                };
                MessagingCenter.Send(taxData, "NewSubmission");
                await DisplayAlert("Successful Submission", "Your submission was submited successfully", "OK");
                EntryHoursWorked.Text = null;
                EntryGrossSubmission.Text = null;
                EntryJobName.Text = null;
            }
           
            //TODO clear fields and reword message
        }
    }
}
