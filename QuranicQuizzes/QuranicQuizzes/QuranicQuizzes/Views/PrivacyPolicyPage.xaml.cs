using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace QuranicQuizzes.Views
{
    public partial class PrivacyPolicyPage : ContentPage
    {
        public PrivacyPolicyPage()
        {
            InitializeComponent();
            webview.Source = "https://quranicquizzes.com/Legal/Privacy?IsApp=true";
        }
    }
}
