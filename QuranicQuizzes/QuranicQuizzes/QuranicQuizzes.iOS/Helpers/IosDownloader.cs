using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using QuranicQuizzes.Interfaces;
using QuranicQuizzes.iOS.Helpers;
using Xamarin.Forms;

[assembly: Dependency(typeof(IosDownloader))]
namespace QuranicQuizzes.iOS.Helpers
{
    public class IosDownloader : IDownloader
    {
        public event EventHandler<DownloadEventArgs> OnFileDownloaded;

        public bool DownloadFile(string url, string folder)
        {
            //string pathToNewFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), folder);
            //Directory.CreateDirectory(pathToNewFolder);
            try
            {
                WebClient webClient = new WebClient();
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);

                var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                var directoryname = Path.Combine(documents, folder);
                if (!System.IO.Directory.Exists(directoryname))
                {
                    System.IO.Directory.CreateDirectory(directoryname);
                }
                string pathToNewFile = Path.Combine(directoryname, Path.GetFileName(url));
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