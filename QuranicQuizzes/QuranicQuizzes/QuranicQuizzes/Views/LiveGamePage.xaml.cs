using System;
using System.Collections.Generic;
using QuranicQuizzes.Helpers;
using Xamarin.Forms;

namespace QuranicQuizzes.Views
{
    public partial class LiveGamePage : ContentPage
    {
        public LiveGamePage()
        {
            InitializeComponent();
            webview.Source = "http://quranicquizzes.com/Play/LiveGamesMobile?"+GlobalConst.ApiUrlKey;
            //webview.Uri = "http://quranicquizzes.com/Play/LiveGamesMobile?" + GlobalConst.ApiUrlKey;
        }
        protected override bool OnBackButtonPressed()
        {
          
            return base.OnBackButtonPressed();
        }

        //Close Live game
        void Handle_CloseTapped(object sender, System.EventArgs e)
        {
            this.Navigation.RemovePage(this.Navigation.NavigationStack[this.Navigation.NavigationStack.Count - 1]);
        }
    }
}
