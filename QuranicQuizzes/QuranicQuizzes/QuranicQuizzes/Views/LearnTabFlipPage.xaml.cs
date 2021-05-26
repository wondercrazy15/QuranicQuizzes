using System;
using System.Collections.Generic;
using QuranicQuizzes.ViewModels;
using Xamarin.Forms;

namespace QuranicQuizzes.Views
{
    public partial class LearnTabFlipPage : ContentPage
    {
        private LearnTabFlipPageViewModel vm;

        public LearnTabFlipPage()
        {
            InitializeComponent();
            vm = BindingContext as LearnTabFlipPageViewModel;
            TimerRunner();
        }

        private void flipItButton_OnClicked(object sender, EventArgs e)
        {
            XFFlipViewControl1.IsFlipped = !XFFlipViewControl1.IsFlipped;
        }

        private void moreButton_OnClicked(object sender, EventArgs e)
        {
            //Navigation.PushAsync(new DemoPage1());
        }

        private void TimerRunner()
        {
            //Device.StartTimer(TimeSpan.FromSeconds(1),
            //    () =>
            //    {
            //        frontViewTimeLabel.Text = $"{DateTime.Now.ToString("F")}";

            //        backViewTimeLabel.Text = $"{DateTime.Now.ToString("F")}";

            //        return true;
            //    });
        }
    }
}