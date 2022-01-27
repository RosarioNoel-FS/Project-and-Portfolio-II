using System;
using System.Collections.Generic;
using TaxEz.Models;

using Xamarin.Forms;

namespace TaxEz
{
    public partial class Tabs : TabbedPage
    {
        public static User CurrentUser { get; set; }

        public Tabs()
        {
            InitializeComponent();

            //create tab navigation
            SubmitTab submitTab = new SubmitTab();
            submitTab.Title = "Submit";

            HistoryTab historyTab = new HistoryTab();
            historyTab.Title = "History";

            //SalariesTab salariesTab = new SalariesTab();
            //salariesTab.Title = "Salaries";

            //Add Tabs to MainPage View
            Children.Add(submitTab);
            Children.Add(historyTab);
            //Children.Add(salariesTab);
        }
    }
}
