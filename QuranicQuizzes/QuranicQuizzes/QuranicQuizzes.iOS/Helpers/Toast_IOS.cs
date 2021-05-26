using System;
using Foundation;
using GlobalToast;
using QuranicQuizzes.Interfaces;
using QuranicQuizzes.iOS.Helpers;

[assembly: Xamarin.Forms.Dependency(typeof(Toast_IOS))]
namespace QuranicQuizzes.iOS.Helpers
{
    public class Toast_IOS : IToast
    {
        public void Show(string message)
        {
            var toast = Toast.MakeToast(message).Show();

            //ShowAlert(message, LONG_DELAY);
            NSTimer.CreateScheduledTimer(2, false, delegate
            {
                toast.Dismiss();
                //Toast.ShowToast(message).Dismiss();
            });
        }
    }
}
