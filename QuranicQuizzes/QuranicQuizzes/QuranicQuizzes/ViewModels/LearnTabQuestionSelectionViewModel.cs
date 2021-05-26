using System;
using System.Collections.Generic;
using Acr.UserDialogs;
using Microsoft.AppCenter.Crashes;
using Prism.Commands;
using Prism.Navigation;
using QuranicQuizzes.Interfaces;
using QuranicQuizzes.Models;
using QuranicQuizzes.Views;
using Xamarin.Forms;

namespace QuranicQuizzes.ViewModels
{
    public class LearnTabQuestionSelectionViewModel : ViewModelBase
    {
        INavigationService _navigationService;
        IClientAPI _clientAPI;
        public Command<object> StudyCommmand { get; set; }
        public Command<object> GenrateCommmand { get; set; }
        public LearnTabQuestionSelectionViewModel(INavigationService navigationService, IClientAPI clientAPI) : base(navigationService)
        {
            _navigationService = navigationService;
            _clientAPI = clientAPI;
            //StudyCommmand = new Command<object>(StudyCommmandClick, (x) => CanNavigate);
            GenrateCommmand = new Command<object>(GenrateCommmandClick, (x) => CanNavigate);

        }

        private async void GenrateCommmandClick(object obj)
        {
            try
            {
                UserDialogs.Instance.ShowLoading("Please wait...", MaskType.Gradient);
                var parameters = new NavigationParameters();
                parameters.Add("PlayTab", Quizzesdata);
                parameters.Add("Categories", Category);
                //parameters.Add("SubCategories", LvSelectedSubCategoryName);
                if (Quizzesdata.IsLearnTabFree)
                {
                    await _navigationService.NavigateAsync(nameof(LearnTabFlipPage), parameters);
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

        bool _canNavigate = true;
        public bool CanNavigate
        {
            get { return _canNavigate; }
            set
            {
                _canNavigate = value;
                RaisePropertyChanged(nameof(CanNavigate));
                // OnPropertyChanged();
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
                //CheckSession SessionInfo = await _clientAPI.GetCheckSession(Quizzesdata.Id, GlobalConst.isCourse, 0);
                Category = parameters["Categories"] as Categories;
                Coureses = parameters["Coureses"] as Coures;

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
        //Close Quizze click
        public DelegateCommand CloseQuizze => new DelegateCommand(async () =>
        {
            try
            {
                var parameters = new NavigationParameters();
                parameters.Add("PlayTab", Quizzesdata);
                parameters.Add("Categories", Category);
                parameters.Add("Coureses", Coureses);
                //parameters.Add("SubCategories", LvSelectedSubCategoryName);

                await _navigationService.GoBackAsync(parameters);
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

    }
}