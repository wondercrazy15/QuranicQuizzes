using System;
using System.Collections.Generic;
using Microsoft.AppCenter.Crashes;
using Prism.Commands;
using Prism.Navigation;
using QuranicQuizzes.Helpers;

namespace QuranicQuizzes.ViewModels
{
    //StartGame PageViewModel
    public class StartGamePageViewModel : ViewModelBase
    {
        
        private string _sourceUrl { get; set; }
        public string SourceUrl
        {
            get
            {
                return _sourceUrl;
            }
            set
            {
                if (_sourceUrl != value)
                {
                    _sourceUrl = value;
                    RaisePropertyChanged(nameof(SourceUrl));
                   
                }
            }
        }

       

        public StartGamePageViewModel(INavigationService navigationService) : base(navigationService)
        {

        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            try
            {
                if (parameters != null)
                {
                    string qid = parameters["qid"] as string;
                    SourceUrl = "http://quranicquizzes.com/Play/LiveGamesMobile/" + qid + "?" + GlobalConst.ApiUrlKey;

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

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

          
        }
    }
}
