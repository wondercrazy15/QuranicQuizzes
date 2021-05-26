using System;
using System.Collections.Generic;
using Acr.UserDialogs;
using Microsoft.AppCenter.Crashes;
using Prism.Commands;
using Prism.Navigation;
using QuranicQuizzes.Interfaces;
using QuranicQuizzes.Models;
using Xamarin.Forms;

namespace QuranicQuizzes.ViewModels
{
    public class LearnTabFlipPageViewModel : ViewModelBase
    {
        INavigationService _navigationService;
        IClientAPI _clientAPI;
        public Command<object> StudyCommmand { get; set; }

        public LearnTabFlipPageViewModel(INavigationService navigationService, IClientAPI clientAPI) : base(navigationService)
        {
            _navigationService = navigationService;
            _clientAPI = clientAPI;
            //StudyCommmand = new Command<object>(StudyCommmandClick, (x) => CanNavigate);
        }

        public DelegateCommand CloseTap => new DelegateCommand(async () =>
        {
            var parameters = new NavigationParameters();
            parameters.Add("PlayTab", Quizzesdata);
            parameters.Add("Categories", Category);
            parameters.Add("Coureses", Coureses);
            await _navigationService.NavigateAsync("../../../../QuizzesByCategoriesPage", parameters);

        });


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

    }
}
