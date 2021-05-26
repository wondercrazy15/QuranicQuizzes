using System;
using Android.App;
using QuranicQuizzes.Droid.Helpers;
using QuranicQuizzes.Interfaces;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(AndroidMethods))]
namespace QuranicQuizzes.Droid.Helpers
{
    public class AndroidMethods : IAndroidMethods
    {
        public void CloseApp()
        {
            var activity = (Activity)Forms.Context;
            activity.FinishAffinity();
            //Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
        }
        
    }
}
