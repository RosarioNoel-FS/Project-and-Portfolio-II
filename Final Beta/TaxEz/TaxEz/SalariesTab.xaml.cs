using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using TaxEz.Models;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.IO;

namespace TaxEz
{
    public partial class SalariesTab : ContentPage
    {
        ObservableCollection<TaxData> groupedJobList = new ObservableCollection<TaxData>(new List<TaxData>());

        //TODO use a dictionary to reduce the file reads
        //key - job name, value - TaxData with grouped job info
        Dictionary<string, TaxData> groupedJobMap = new Dictionary<string, TaxData>();

        //TODO only read from file once when tab is created
        //on each new submission, if job not in dictionary, add, otherwise sum job info with existing

        public SalariesTab()
        {
            InitializeComponent();
            listview.ItemsSource = groupedJobList;

            MessagingCenter.Subscribe<TaxData>(this, "NewSubmission", (sender) =>
            {
                GetInfoAndDisplay();
            });

            GetInfoAndDisplay();
        }

        //create list containg all job names
        private List<string> GetListOfJobs()
        {
            List<string> jobNames = new List<string>();

            string line;
            using (StreamReader sr = new StreamReader(Tabs.CurrentUser.UserIncomeFile))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    string[] split = line.Split(';');
                    string jobname = split[0];

                    if (!jobNames.Contains(split[0]))
                    {
                        jobNames.Add(jobname);
                    }
                }
                return jobNames;
            }
        }

        //get gross income and hours worked
        private void GetInfoAndDisplay()
        {
            //replace list view entires
            groupedJobList.Clear();

            List<string> jobList = GetListOfJobs();

            foreach (string job in jobList)
            {
                //reset tracked values
                int hoursWorkedTotal = 0;
                decimal GrossTotal = 0;
                //============================================================================
                using (StreamReader sr = new StreamReader(Tabs.CurrentUser.UserIncomeFile))
                {
                    string line;
                    //iterate through file
                    while ((line = sr.ReadLine()) != null)
                    {
                        //initial values 
                        string[] split = line.Split(';');
                        string jobnameStr = split[0];
                        string grossStr = split[1];
                        string hoursStr = split[2];
                        //=====================================================================
                        //validate date
                        decimal grossDec = decimal.Parse(grossStr);
                        int hoursInt = int.Parse(hoursStr);
                        //=====================================================================
                        //compare and track
                        if (job == jobnameStr)
                        {
                            hoursWorkedTotal += hoursInt;
                            GrossTotal += grossDec;
                        }
                    }
                }

                decimal payRate;
                if (hoursWorkedTotal == 0)
                {
                    //don't divide
                    payRate = GrossTotal;
                }
                else
                {
                    //divide
                    payRate = GrossTotal / hoursWorkedTotal;
                }

                TaxData currentJob = new TaxData();
                currentJob.JobName = job;
                currentJob.PreTaxAmount = GrossTotal;
                currentJob.HoursWorked = hoursWorkedTotal;
                currentJob.PayRate = payRate;

                //============================================================================
                groupedJobList.Add(currentJob);
            }
        }
    }

}


