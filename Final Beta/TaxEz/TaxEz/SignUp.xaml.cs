using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Forms;
using TaxEz.Models;

namespace TaxEz
{
    public partial class SignUp : ContentPage
    {
        public SignUp()
        {
            InitializeComponent();
            SignUpButton.Clicked += SignUpButton_Clicked;
            AlreadyHaveAccountButton.Clicked += AlreadyHaveAccountButton_Clicked;
        }

        private async void AlreadyHaveAccountButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SignIn());
            Navigation.RemovePage(this);
        }

        private void clearFields()
        {
            EntryEmailSignUp.Text = null;
            EntryFirstNameSignUp.Text = null;
            EntryLastNameSignUp.Text = null;
            EntryUsernameSignUp.Text = null;
            EntryPasswordSignUp.Text = null;
            EntryPasswordTwoSignUp.Text = null;
        }

        private async void SignUpButton_Clicked(object sender, EventArgs e)
        {
            //file to store user data
            string userDataFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), $"{EntryUsernameSignUp.Text}.txt");
            //file to check existing email
            string emailFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), $"{EntryEmailSignUp.Text}.txt");

            //Front end Validation ==========================================================================================================================
            //check if any fields are empty
            if (string.IsNullOrWhiteSpace(EntryEmailSignUp.Text)
            || string.IsNullOrWhiteSpace(EntryFirstNameSignUp.Text)
            || string.IsNullOrWhiteSpace(EntryLastNameSignUp.Text)
            || string.IsNullOrWhiteSpace(EntryPasswordSignUp.Text)
            || string.IsNullOrWhiteSpace(EntryPasswordTwoSignUp.Text)
            || string.IsNullOrWhiteSpace(EntryUsernameSignUp.Text)
            )
            {
                await DisplayAlert("Empty Entries", "Do not leave any of the entries empty.", "OK");
                clearFields();
            }
            //check if email is valid
            else if (!EntryEmailSignUp.Text.Contains("@"))
            {
                await DisplayAlert("Invalid Email", "No valid email was provided.", "OK");
                clearFields();
            }
            //check if the passwords match
            else if (EntryPasswordSignUp.Text != EntryPasswordTwoSignUp.Text)
            {
                await DisplayAlert("Password Mismatch", "The passwords are not the same.", "OK");
                clearFields();
            }
            //Back end Validation ===========================================================================================================================
            //check if email already exists
            else if (File.Exists(emailFile))
            {
                await DisplayAlert("Email Already In Use", "This email is taken. Please enter another one.", "OK");
                clearFields();
            }
            //check if the username already exists
            else if (File.Exists(userDataFile))
            {
                await DisplayAlert("Username Already In Use", "This username is taken. Please enter another one.", "OK");
                clearFields();
            }
            //everything is valid
            else
            {
                //write user data to file
                using (StreamWriter sw = new StreamWriter(userDataFile))
                {
                    sw.WriteLine(EntryFirstNameSignUp.Text);
                    sw.WriteLine(EntryLastNameSignUp.Text);
                    sw.WriteLine(EntryUsernameSignUp.Text);
                    sw.WriteLine(EntryEmailSignUp.Text);
                    sw.WriteLine(EntryPasswordSignUp.Text);
                }
                //write email to separate file
                using (StreamWriter sw2 = new StreamWriter(emailFile))
                {
                    sw2.WriteLine(EntryEmailSignUp.Text);
                }

                await DisplayAlert("Sign Up Completed", "Thank you for signing up!", "OK");

                Tabs.CurrentUser = new User()
                {
                    Username = EntryUsernameSignUp.Text
                };

                Application.Current.MainPage = new FinancialSetUp();
            }
        }
    }
}
