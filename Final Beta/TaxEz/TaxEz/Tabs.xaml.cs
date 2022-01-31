using System;
using System.Collections.Generic;
using TaxEz.Models;

using Xamarin.Forms;

namespace TaxEz
{
    public partial class Tabs : TabbedPage
    {
        //all tabs can access this because it is static
        public static User CurrentUser { get; set; }

        public Tabs()
        {
            InitializeComponent();

            //add signout button to toolbar
            ToolbarItem signOut = new ToolbarItem()
            {   
                Text = "Sign Out",
                Order = ToolbarItemOrder.Primary,
                Priority = 0,
            };
            ToolbarItems.Add(signOut);

            signOut.Command = new Command((sender)  =>
            {

                SignOutAlert();
                
                
            });

            //create tabs
            SubmitTab submitTab = new SubmitTab();
            submitTab.Title = "Submit";

            HistoryTab historyTab = new HistoryTab();
            historyTab.Title = "History";

            SalariesTab salariesTab = new SalariesTab();
            salariesTab.Title = "Salaries";

            //Add Tabs to MainPage View
            Children.Add(submitTab);
            Children.Add(historyTab);
            Children.Add(salariesTab);
        }

        private async void SignOutAlert()
        {
            bool signOut = await DisplayAlert("Sign Out", "Are you sure you would like to sign out?", "Yes", "No");

            if(signOut)
            {
                //nav to main page
                Application.Current.MainPage = new NavigationPage(new MainPage());
                //reset user data
                CurrentUser = null;
                APIHanler.resetCache();
            }


        }
    }
}
