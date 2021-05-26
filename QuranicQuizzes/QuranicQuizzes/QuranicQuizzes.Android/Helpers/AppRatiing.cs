using System;
using Android.Content;
using Android.Content.PM;
using QuranicQuizzes.Droid.Helpers;
using QuranicQuizzes.Interfaces;
[assembly: Xamarin.Forms.Dependency(typeof(AppRatiing))]
namespace QuranicQuizzes.Droid.Helpers
{
    public class AppRatiing : IAppRating
    {
        public void RateApp()
        {
            var activity = Android.App.Application.Context;
            //string url = $"market://details?id={(activity as Context)?.PackageName}";
            Android.Net.Uri uri = Android.Net.Uri.Parse($"market://details?id={(activity as Context)?.PackageName}");
            //var intent = new Intent(Intent.ActionView, uri);
            try
            {
                activity.PackageManager.GetPackageInfo("com.QuranicQuizzes", PackageInfoFlags.Activities);
                Intent intent = new Intent(Intent.ActionView, uri);
                intent.SetFlags(ActivityFlags.NewTask);
                activity.StartActivity(intent);
            }
            catch (PackageManager.NameNotFoundException ex)
            {
                // this won't happen. But catching just in case the user has downloaded the app without having Google Play installed.
                Console.WriteLine(ex.Message);
            }
            catch (ActivityNotFoundException)
            {
                // if Google Play fails to load, open the App link on the browser 
                var playStoreUrl = "https://play.google.com/store/apps/details?id=com.yourapplicationpackagename"; //Add here the url of your application on the store
                uri = Android.Net.Uri.Parse(playStoreUrl);
                var browserIntent = new Intent(Intent.ActionView, uri);
                browserIntent.AddFlags(ActivityFlags.NewTask | ActivityFlags.ResetTaskIfNeeded);
                activity.StartActivity(browserIntent);
            }
        }
    }
}