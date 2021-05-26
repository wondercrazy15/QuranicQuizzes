using System;
using System.Collections.Generic;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace QuranicQuizzes.Views
{
    public partial class QuizzesBySubCategoriesPage : ContentPage
    {
        public QuizzesBySubCategoriesPage()
        {
            InitializeComponent();
            SubCategoriesList.HeightRequest = DeviceDisplay.MainDisplayInfo.Height;
        }
    }
}
