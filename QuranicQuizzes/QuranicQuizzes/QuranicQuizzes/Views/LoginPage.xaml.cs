using System;
using System.Collections.Generic;
using Microsoft.AppCenter.Crashes;
using QuranicQuizzes.CustomControls;
using QuranicQuizzes.Interfaces;
using QuranicQuizzes.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace QuranicQuizzes.Views
{
    public partial class LoginPage : ContentPage
    {
        private LoginPageViewModel vm;

        public LoginPage()
        {
            try
            {
                InitializeComponent();
                vm = BindingContext as LoginPageViewModel;
                On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
                EdtEmail.ReturnCommand = new Command(() => EdtPassword.Focus());
                EdtPassword.ReturnCommand = new Command(() => frameLogin.Focus());
            }
            catch (Exception ex)
            {
                var properties = new Dictionary<string, string>
                {
                    { "Messge", ex.Message },
                    { "StackTrace", ex.StackTrace }
                };

                Crashes.TrackError(ex, properties);
            }
            
        }

        //Tooltip hidden when tap outside screen
        void Handle_Tapped(object sender, System.EventArgs e)
        {
            try
            {
                //TooltipEffect.SetHasTooltip(mainLayout.Children, false);
                foreach (var c in mainLayout.Children)
                {
                    var name=c.FindByName<Image>("btn3");
                    
                    if (TooltipEffect.GetHasTooltip(name))
                    {
                        TooltipEffect.SetHasTooltip(name, false);
                        TooltipEffect.SetHasTooltip(name, true);
                    }
                }
            }
            catch (Exception ex)
            {
                DependencyService.Get<IToast>().Show("Opps,Something wrong..!!");
                var properties = new Dictionary<string, string>
                {
                    { "Messge", ex.Message },
                    { "StackTrace", ex.StackTrace }
                };

                Crashes.TrackError(ex, properties);
            }
        }

        
        protected override bool OnBackButtonPressed()
        {
            //vm.Back();
            return base.OnBackButtonPressed();
        }
    }
}
