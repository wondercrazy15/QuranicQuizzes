using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace QuranicQuizzes.Views
{
    public partial class TermsPage : ContentPage
    {
        public TermsPage()
        {
            InitializeComponent();
            webview.Source = "https://quranicquizzes.com/Legal/Terms?IsApp=true";
        }
    }
}
