using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace QuranicQuizzes.Views
{
    public partial class MyStudentPage : ContentPage
    {
        public MyStudentPage()
        {
            InitializeComponent();
            webview.Source = "http://quranicquizzes.com/Members/Students?IsApp=true";
        }
    }
}
