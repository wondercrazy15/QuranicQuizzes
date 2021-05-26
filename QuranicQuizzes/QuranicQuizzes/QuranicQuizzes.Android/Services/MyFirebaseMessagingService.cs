using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Media;
using Android.OS;
using Android.Support.V4.App;
using Firebase.Messaging;

namespace QuranicQuizzes.Droid.Services
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class MyFirebaseMessagingService : FirebaseMessagingService
    {
        public static string CHANNEL_ID = "My_Channel";
        //APICall a = new APICall();
        public string NotificationId { get; private set; }

        public override void OnMessageReceived(RemoteMessage message)
        {
            base.OnMessageReceived(message);
            createNotification("Notifications", message, this);
        }

        //async Task<bool> CheckURL(string links)
        //{
        //    var Data = await a.URLCheck(links);
        //    return Data;
        //}
        void createNotification(string title, RemoteMessage message, Android.Content.Context context)
        {
            string value = "";
            
            RemoteMessage.Notification response = message.GetNotification();
            if (response != null)
            {
                Random random = new Random();
                int notifyId = random.Next(9999 - 1000) + 1000;
                var notificationManager = NotificationManager.FromContext(this);
             

                //we use the pending intent, passing our ui intent over which will get called
                //when the notification is tapped.
                // Passed parameters to MainActivity.cs
                string temp = "Notification";
                var uiIntent = new Intent(this, typeof(MainActivity));
                uiIntent.PutExtra("Notification", temp);
                
                uiIntent.AddFlags(ActivityFlags.ClearTop);

                var pendingIntent = PendingIntent.GetActivity(this, notifyId, uiIntent, PendingIntentFlags.UpdateCurrent);
                NotificationCompat.Builder builder = new  NotificationCompat.Builder(this, "Notification");
                builder.SetPriority((int)NotificationPriority.High)
                                        .SetSmallIcon(Resource.Drawable.Quran)
                                         .SetContentTitle(response.Title)
                                         .SetContentText(response.Body)
                                         //Set the notification sound
                                         .SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Notification))
                                         .SetStyle(new NotificationCompat.BigTextStyle().BigText(response.Body))
                                         //Auto cancel will remove the notification once the user touches it
                                         .SetAutoCancel(true)
                                         .SetContentIntent(pendingIntent);
                if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
                {
                    builder.SetSmallIcon(Resource.Drawable.Quran);
                    builder.SetColor(Resource.Color.primary_dark_material_dark);
                    NotificationChannel mChannel = new NotificationChannel("QuranicQuizzesNotification", "QuranicQuizzesNotification",
                        NotificationImportance.High);
                    //mChannel.SetShowBadge(true);
                    mChannel.SetShowBadge(true);
                    notificationManager.CreateNotificationChannel(mChannel);
                }
                else
                {
                    Bitmap icon = BitmapFactory.DecodeResource(context.Resources,
                                              Resource.Drawable.Quran);
                    builder.SetLargeIcon(icon);
                    builder.SetSmallIcon(Resource.Drawable.Quran);

                }
                //Show the notification
                notificationManager.Notify(notifyId, builder.Build());
            }
        }

        public string SubstringAfter(string source, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return source;
            }
            CompareInfo compareInfo = CultureInfo.InvariantCulture.CompareInfo;
            int index = compareInfo.IndexOf(source, value, CompareOptions.Ordinal);
            if (index < 0)
            {
                //No such substring
                return string.Empty;
            }
            return source.Substring(index + value.Length);
        }

    }
}
