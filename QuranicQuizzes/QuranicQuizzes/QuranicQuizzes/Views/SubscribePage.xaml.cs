using System;
using System.Collections.Generic;
using Microsoft.AppCenter.Analytics;
using QuranicQuizzes.Helpers;
using Xamarin.Forms;

namespace QuranicQuizzes.Views
{
    public partial class SubscribePage : ContentPage
    {
        public SubscribePage()
        {
            InitializeComponent();
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    InstitutionAccount.IsVisible = false;
                    break;
                case Device.Android:
                    InstitutionAccount.IsVisible = true;
                    break;
            }
            
            btnDropDownpickerIndividualAccount.Clicked += BtnDropDownpickerIndividualAccount_Clicked;
            btnDropDownpickerFamilyAccount.Clicked += BtnDropDownpickerFamilyAccount_Clicked;
        }

        private void BtnDropDownpickerFamilyAccount_Clicked(object sender, EventArgs e)
        {
            pickerFamilyAccount.Focus();
        }
        
        private void BtnDropDownpickerIndividualAccount_Clicked(object sender, EventArgs e)
        {
            pickerIndividualAccount.Focus();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
    }
}
