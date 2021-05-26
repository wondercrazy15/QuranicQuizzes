using System;
using System.Collections.Generic;
using Microsoft.AppCenter.Crashes;
using QuranicQuizzes.Interfaces;
using QuranicQuizzes.ViewModels;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace QuranicQuizzes.Views
{
    public partial class WebPopupPage : PopupPage
    {
        private WebPopupPageViewModel vm;
        string SourceURL;
        public WebPopupPage(string notesHeader, string sourceURL = "")
        {
            try
            {
                InitializeComponent();
                vm = BindingContext as WebPopupPageViewModel;
                SourceURL = sourceURL;
                vm.Name = notesHeader;
                webNote.Source = SourceURL;
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
            
            //webNote.HeightRequest = Scroll.ContentSize.Height;
        }
        private void BtnClose_Clicked(object sender, EventArgs e)
        {
            popUpDissmiss();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            
        }

        protected override bool OnBackButtonPressed()
        {
            popUpDissmiss();
            return base.OnBackButtonPressed();
        }
        public async void popUpDissmiss()
        {
            try 
            {
                await PopupNavigation.Instance.PopAllAsync();
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
    }
}
