using System;
using Android.Widget;
using QuranicQuizzes.Droid.Helpers;
using QuranicQuizzes.Interfaces;

[assembly: Xamarin.Forms.Dependency(typeof(Toast_Android))]
namespace QuranicQuizzes.Droid.Helpers
{
    public class Toast_Android : IToast
    {
        public void Show(string message)
        {
            Toast.MakeText(Android.App.Application.Context, message, ToastLength.Short).Show();
        }
    }
}
