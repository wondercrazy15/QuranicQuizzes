using System;
using System.Collections.Generic;
using Microsoft.AppCenter.Crashes;
using QuranicQuizzes.Interfaces;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace QuranicQuizzes.Views
{
    public partial class NotePopupPage : PopupPage
    {
        string SourceURL;
        public NotePopupPage(string notes, string sourceURL="")
        {
            InitializeComponent();
            noteData.Text = notes;
            SourceURL = sourceURL;
        }
        private void BtnClose_Clicked(object sender, EventArgs e)
        {
            popUpDissmiss();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            webNote.Source = SourceURL;
            
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
