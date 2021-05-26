using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using QuranicQuizzes.Droid.Helpers;
using QuranicQuizzes.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(AndroidDownloader))]
namespace QuranicQuizzes.Droid.Helpers
{
    public class AndroidDownloader : IDownloader
    {
        public event EventHandler<DownloadEventArgs> OnFileDownloaded;

        public bool DownloadFile(string url, string folder)
        {
            string pathToNewFolder = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, folder);
            Directory.CreateDirectory(pathToNewFolder);

            try
            {
                WebClient webClient = new WebClient();
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
                string pathToNewFile = Path.Combine(pathToNewFolder, Path.GetFileName(url));
                if (File.Exists(pathToNewFile))
                {
                    return false;
                }
                else
                {
                    webClient.DownloadFileAsync(new Uri(url), pathToNewFile);
                    return false;
                }
                
            }
            catch (Exception ex)
            {
                if (OnFileDownloaded != null)
                    OnFileDownloaded.Invoke(this, new DownloadEventArgs(false));
            }
            return true;
        }

        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                if (OnFileDownloaded != null)
                    OnFileDownloaded.Invoke(this, new DownloadEventArgs(false));
            }
            else
            {
                if (OnFileDownloaded != null)
                    OnFileDownloaded.Invoke(this, new DownloadEventArgs(true));
            }
        }
    }
}