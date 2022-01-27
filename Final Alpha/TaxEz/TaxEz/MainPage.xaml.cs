using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TaxEz
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            SignInButton.Clicked += SignInButton_Clicked;
            SignUpButton.Clicked += SignUpButton_Clicked;
        }

        private async void SignUpButton_Clicked(object sender, EventArgs e)
        {
            //navigate to sign up screen
            await Navigation.PushAsync(new SignUp());
        }

        private async void SignInButton_Clicked(object sender, EventArgs e)
        {
            //navigate to sign up screen
            await Navigation.PushAsync(new SignIn());
        }
    }
}
