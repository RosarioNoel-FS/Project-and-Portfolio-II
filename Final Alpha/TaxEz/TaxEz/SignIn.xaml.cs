using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Forms;
using TaxEz.Models;

namespace TaxEz
{
    public partial class SignIn : ContentPage
    {
        public SignIn()
        {
            InitializeComponent();
            LogInButton.Clicked += LogInButton_Clicked;
            MakeNewAccountButton.Clicked += MakeNewAccountButton_Clicked;
            ForgotUserPassButton.Clicked += ForgotUserPassButton_Clicked;
        }

        private async void ForgotUserPassButton_Clicked(object sender, EventArgs e)
        {
            if (EntryUsernameSignIn.Text == null)
            {
                await DisplayAlert("Forgot Username/Password", "Hint: Username", "OK");
            }
            else if (EntryPasswordSignIn.Text == null)
            {
                await DisplayAlert("Forgot Username/Password", "Hint: Password", "OK");
            }
        }

        private async void MakeNewAccountButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SignUp());
            Navigation.RemovePage(this);
        }

        private void clearFields()
        {
            EntryUsernameSignIn.Text = null;
            EntryPasswordSignIn.Text = null;
        }

        private async void LogInButton_Clicked(object sender, EventArgs e)
        {
            //user data file path
            string userDataFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), $"{EntryUsernameSignIn.Text}.txt");
            //check to see if username exists
            if (!File.Exists(userDataFile))
            {
                bool answer = await DisplayAlert("Invalid Username", "This user does not exist. Would you like to sign up?", "Try Again", "Sign Up");
                if (answer)
                {
                    clearFields();
                }
                else
                {
                    await Navigation.PushAsync(new SignUp());
                    Navigation.RemovePage(this);
                }
            }
            //if user exists
            else
            {
                string password;
                string state;
                using (StreamReader sr = new StreamReader(userDataFile))
                {
                    //read lines until you get to the password and state
                    sr.ReadLine();
                    sr.ReadLine();
                    sr.ReadLine();
                    sr.ReadLine();
                    password = sr.ReadLine();
                    state = sr.ReadLine();
                }

                //check if password is correct for this user
                if (EntryPasswordSignIn.Text != password)
                {
                    await DisplayAlert("Invalid Login", "Username/password combination is not correct.", "OK");
                    clearFields();
                }
                //if user signed in successfully
                else
                {
                    Tabs.CurrentUser = new User()
                    {
                        Username = EntryUsernameSignIn.Text,
                        State = state
                    };

                    Application.Current.MainPage = new Tabs();
                }
            }
        }
    }
}
