using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace QuranicQuizzes.Views
{
    public partial class AboutUSPage : ContentPage
    {
        public AboutUSPage()
        {
            InitializeComponent();
            webview.Source = "https://quranicquizzes.com/Home/AboutUs?IsApp=true";
        }
    }
}
