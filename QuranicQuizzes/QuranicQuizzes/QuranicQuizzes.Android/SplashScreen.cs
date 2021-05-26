
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Microsoft.AppCenter.Crashes;

namespace QuranicQuizzes.Droid
{
    [Activity(Label = "QuranicQuizzes", HardwareAccelerated = true, MainLauncher = true, Theme = "@style/Splash", NoHistory = true, LaunchMode = Android.Content.PM.LaunchMode.SingleTop)]
    public class SplashScreen : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                Intent intent = new Intent(this, typeof(MainActivity));
                if (Intent.Extras != null)
                {
                    var d1 = Intent.GetStringExtra("Notification");
                    intent.PutExtras(Intent.Extras);
                    
                }
                intent.SetFlags(ActivityFlags.SingleTop);
                StartActivity(intent);
            }
            catch (Exception ex)
            {
                var properties = new Dictionary<string, string>
                {
                    { "Messge", ex.Message },
                    { "StackTrace", ex.StackTrace }
                };

                Crashes.TrackError(ex, properties);
            }
            
           
        }
      
    }
}
