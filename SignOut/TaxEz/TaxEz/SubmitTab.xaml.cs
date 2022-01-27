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

        private void SubmitButton_Clicked(object sender, EventArgs e)
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
        }
    }
}
