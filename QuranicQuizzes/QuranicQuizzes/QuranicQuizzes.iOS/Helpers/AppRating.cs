using System;
using System.Collections.Generic;
using Foundation;
using Microsoft.AppCenter.Crashes;
using QuranicQuizzes.Interfaces;
using QuranicQuizzes.iOS.Helpers;
using StoreKit;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(AppRating))]
namespace QuranicQuizzes.iOS.Helpers
{
    public class AppRating : IAppRating
    {
        public void RateApp()
        {
            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 3))
                SKStoreReviewController.RequestReview();
            else
            {
                var storeUrl = "itms-apps://itunes.apple.com/app/1502388967";
                var url = storeUrl + "?action=write-review";

                try
                {
                    UIApplication.SharedApplication.OpenUrl(new NSUrl(url));
                }
                catch (Exception ex)
                {
                    // Here you could show an alert to the user telling that App Store was unable to launch
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
}