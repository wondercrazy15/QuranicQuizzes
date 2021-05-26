using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace QuranicQuizzes.Views
{
    //FAQPage
    public partial class FAQPage : ContentPage
    {
        public FAQPage()
        {
            InitializeComponent();
            webview.Source = "https://quranicquizzes.com/Home/FAQs?IsApp=true";
        }
    }
}
