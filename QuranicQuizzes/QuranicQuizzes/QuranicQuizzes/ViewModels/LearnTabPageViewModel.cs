using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using Acr.UserDialogs;
using MediaManager;
using Microsoft.AppCenter.Crashes;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Plugin.SimpleAudioPlayer;
using Prism.Commands;
using Prism.Navigation;
using QuranicQuizzes.Helpers;
using QuranicQuizzes.Interfaces;
using QuranicQuizzes.Models;
using QuranicQuizzes.Views;
using Xamarin.Forms;

namespace QuranicQuizzes.ViewModels
{
    public class LearnTabPageViewModel : ViewModelBase, INotifyPropertyChanged
    {
        /// <summary>
        /// LearnTabPage viewmodel
        /// </summary>
        ISimpleAudioPlayer player;
        INavigationService _navigationService;
        IClientAPI _clientAPI;
        IDownloader downloader = DependencyService.Get<IDownloader>();
        public LearnTabPageViewModel(INavigationService navigationService, IClientAPI clientAPI) : base(navigationService)
        {
            _navigationService = navigationService;
            _clientAPI = clientAPI;

        }
        //Download permission
        public async void  PermissionData()
        {
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);
                if (status != PermissionStatus.Granted)
                {
                    var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Plugin.Permissions.Abstractions.Permission.Storage });
                    status = results[Plugin.Permissions.Abstractions.Permission.Storage];

                }
                    //Query permission
                    var res = downloader.DownloadFile(Quizzesdata.LearnTabSound, "QuranicQuizzes");
                    if (res)
                    {
                        IsPlay = false;
                        IsDownload = true;
                    }
                    else
                    {
                    IsDownload = false;
                    IsPlay = true;
                    }
                
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                var properties = new Dictionary<string, string>
                {
                    { "Messge", ex.Message },
                    { "StackTrace", ex.StackTrace }
                };

                Crashes.TrackError(ex, properties);
            }
        }

        private bool _isBusy { get; set; }
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                RaisePropertyChanged(nameof(IsBusy));
            }
        }

        public bool _IsSoundTab;
        public bool IsSoundTab
        {
            get
            {
                return _IsSoundTab;
            }
            set
            {
                SetProperty(ref _IsSoundTab, value);
            }
        }
        
        private bool _IsDownload { get; set; }
        public bool IsDownload
        {
            get => _IsDownload;
            set
            {
                _IsDownload = value;
                RaisePropertyChanged(nameof(IsDownload));
            }
        }

        private string _SourceURL { get; set; }
        public string SourceURL
        {
            get => _SourceURL;
            set
            {
                _SourceURL = value;
                RaisePropertyChanged(nameof(SourceURL));
            }
        }

        private bool _isPlay { get; set; }
        public bool IsPlay
        {
            get => _isPlay;
            set
            {
                _isPlay = value;
                RaisePropertyChanged(nameof(IsPlay));
            }
        }

        private bool _isResume { get; set; }
        public bool IsResume
        {
            get => _isResume;
            set
            {
                _isResume = value;
                RaisePropertyChanged(nameof(IsResume));
            }
        }

        public string _IconColor;
        public string IconColor
        {
            get
            {
                return _IconColor;
            }

            set
            {
                _IconColor = value;
                RaisePropertyChanged(nameof(IconColor));
            }
        }

        public string _name;
        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
                RaisePropertyChanged(nameof(Name));
            }
        }

        Quizze Quizzesdata = new Quizze();
        CheckSession SessionInfo = new CheckSession();
        Categories Category = new Categories();
        Coures Coureses = new Coures();

        public override async void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            try
            {
                Quizzesdata = parameters["PlayTab"] as Quizze;
                SourceURL = "http://quranicquizzes.com/" + "Quizzes/LearnApp/" + Quizzesdata.Id + "?" + GlobalConst.ApiUrlKey + "&iscourse=" + GlobalConst.isCourse;
                CheckSession SessionInfo = await _clientAPI.GetCheckSession(Quizzesdata.Id, GlobalConst.isCourse, 0);
                Category = parameters["Categories"] as Categories;
                Coureses = parameters["Coureses"] as Coures;
                IsPlay = false;
                Name = Quizzesdata.Name;
                if (string.IsNullOrEmpty(Quizzesdata.LearnTabSound))
                {
                    IsSoundTab = false;
                }
                else
                {
                    var player = CrossSimpleAudioPlayer.Current;
                    this.player = player;
                    //string url = "some-url-string";
                    WebClient wc = new WebClient();
                    Stream fileStream = wc.OpenRead(Quizzesdata.LearnTabSound);
                    player.Load(fileStream);

                    IsSoundTab = true;
                    IsPlay = true;
                    
                    if (Category.CategoryName.Equals("1. Al-Quran"))
                    {
                        IsDownload = true;
                        PermissionData();
                    }
                }
                
                
                //string url = "some-url-string";

                //if (Quizzesdata != null)
                //{
                //    Name = Quizzesdata.Name;
                //}
                //http://quranicquizzes.com/Quizzes/LearnApp/311?ak=9yT6MRqbdBYbEvQQ7tweG&isCourse=false
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                var properties = new Dictionary<string, string>
                {
                    { "Messge", ex.Message },
                    { "StackTrace", ex.StackTrace }
                };

                Crashes.TrackError(ex, properties);
            }
        }

        public DelegateCommand Back => new DelegateCommand(async () =>
        {
            try
            {
                if (IsSoundTab)
                {
                    if (player != null)
                    {
                        count = 1;
                        if (player.IsPlaying)
                        {
                            player.Stop();
                        }
                    }
                }
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
            
            //await CrossMediaManager.Current.Stop();
        });

        //Download Click
        public DelegateCommand DownloadCommand => new DelegateCommand(async () =>
        {
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);
                if (status != PermissionStatus.Granted)
                {
                    
                    var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Plugin.Permissions.Abstractions.Permission.Storage });
                    status = results[Plugin.Permissions.Abstractions.Permission.Storage];

                }

                if (status == PermissionStatus.Granted)
                {
                    //Query permission
                    
                    var res=downloader.DownloadFile(Quizzesdata.LearnTabSound, "QuranicQuizzes");
                    if (res)
                    {
                        ShowMessage("Download Completed");
                        IsPlay = true;
                    }
                    else
                    {
                        ShowMessage("File already downloaded");
                        IsDownload = false;
                    }
                    
                }
                else if (status != PermissionStatus.Unknown)
                {
                    //location denied
                }
                
                //var destination = Path.Combine(
                //System.Environment.GetFolderPath(
                //    System.Environment.SpecialFolder.ApplicationData),
                //    "music.mp3");

                //await new WebClient().DownloadFileTaskAsync(
                //    new Uri(Quizzesdata.LearnTabSound),
                //    destination);
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
            
        });

        //Check sound compele or not
        public async void CheckSoundComplete()
        {
            try
            {
                var currPosition = player.CurrentPosition;
                if (currPosition == 0)
                {
                    IsPlay = true;
                    IsResume = false;
                    count = 1;

                }

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

        //Play sound click
        int count = 1;
        public DelegateCommand PlaySoundCommand => new DelegateCommand(async () =>
        {
            try
            {
                if (count == 1)
                {
                    //player = CrossSimpleAudioPlayer.Current;
                    ////string url = "some-url-string";
                    //WebClient wc = new WebClient();
                    //Stream fileStream = wc.OpenRead(Quizzesdata.LearnTabSound);
                    //player.Load(fileStream);

                    count++;
                    Device.StartTimer(TimeSpan.FromSeconds(5), () =>
                    {
                        Device.BeginInvokeOnMainThread(() => CheckSoundComplete());
                        if (count == 1)
                            return false;
                        else
                            return true;
                    });
                }
                player.Play();
                //player.Loop = true;
                IsPlay = false;
                IsResume = true;
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

            
            //if (CrossMediaManager.Current.IsStopped())
            //{

            //    IsPlay = false;
            //    IsResume = true;
            //    await CrossMediaManager.Current.Play(Quizzesdata.LearnTabSound);

            //}
            //else
            //{
            //    if (Ispause)
            //    {
            //        IsPlay = false;
            //        IsResume = true;
            //        Ispause = false;
            //        await CrossMediaManager.Current.PlayPause();
            //    }
            //}
            //var d = player.Duration;
            //player.Play();
        }); 

        //Pause sound click
        public DelegateCommand PauseSoundCommand => new DelegateCommand(async () =>
        {
            try
            {
                var d = player.Duration;
                if (player.IsPlaying)
                {
                    player.Pause();

                }
                IsPlay = true;
                IsResume = false;

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
            //if (CrossMediaManager.Current.IsPlaying())
            //{
            //    await CrossMediaManager.Current.Pause();
            //    Ispause = true;
            //    IsPlay = true;
            //    IsResume = false;
            //}
            //else
            //{
            //    IsPlay = false;
            //    IsResume = true;
            //    await CrossMediaManager.Current.Play(Quizzesdata.LearnTabSound);
            //}
        });

        //public DelegateCommand ResumeSoundCommand => new DelegateCommand(async () =>
        //{

        //});

        //FlashCard
        public DelegateCommand FlashCardClick => new DelegateCommand(async () =>
        {
            try
            {
                UserDialogs.Instance.ShowLoading("Please wait...", MaskType.Gradient);
                var parameters = new NavigationParameters();
                parameters.Add("PlayTab", Quizzesdata);
                parameters.Add("Categories", Category);
                if (Quizzesdata.IsLearnTabFree)
                {
                    await _navigationService.NavigateAsync(nameof(LearnTabQuestionSelectionPage), parameters);
                }
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                var properties = new Dictionary<string, string>
                {
                    { "Messge", ex.Message },
                    { "StackTrace", ex.StackTrace }
                };

                Crashes.TrackError(ex, properties);
            }
        });

        //Play Quizz
        public DelegateCommand PlayCommand => new DelegateCommand(async () =>
        {
            try
            {
                UserDialogs.Instance.ShowLoading("Please wait...", MaskType.Gradient);
                var parameters = new NavigationParameters();
                parameters.Add("PlayTab", Quizzesdata);
                parameters.Add("Categories", Category);
                parameters.Add("SourceURL", SourceURL);
                parameters.Add("Coureses", Coureses);
                if (Quizzesdata != null)
                {
                    if (!Quizzesdata.LockQuiz)
                    {
                        UserDialogs.Instance.ShowLoading("Please wait...", MaskType.Gradient);
                        GlobalConst.isLearnTab = true;
                        //CheckSession SessionData = await _clientAPI.GetCheckSession(data.Id,GlobalConst.isCourse, 0);
                        //parameters.Add("SessionInfo", SessionData);

                        if (Quizzesdata.IsLearnTabFree)
                        {
                            await _navigationService.NavigateAsync(nameof(PlayTabPage), parameters);
                        }
                    }
                    else
                    {
                        await _navigationService.NavigateAsync(nameof(SubscribePage));
                        UserDialogs.Instance.HideLoading();
                    }
                }
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                var properties = new Dictionary<string, string>
                {
                    { "Messge", ex.Message },
                    { "StackTrace", ex.StackTrace }
                };
                Crashes.TrackError(ex, properties);
            }
        });


    }
}
