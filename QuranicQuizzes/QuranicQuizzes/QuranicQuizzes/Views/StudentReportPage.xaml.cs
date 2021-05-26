using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace QuranicQuizzes.Views
{
    public partial class StudentReportPage : ContentPage
    {
        public StudentReportPage()
        {
            InitializeComponent();
            webview.Source = "http://quranicquizzes.com/Members/Reports?IsApp=true";

       
        }
    }
}
