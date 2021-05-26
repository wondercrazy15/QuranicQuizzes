using System;
using Prism.Navigation;
using QuranicQuizzes.Interfaces;

namespace QuranicQuizzes.ViewModels
{
    public class WebPopupPageViewModel : ViewModelBase
    {
        INavigationService _navigationService;
        IClientAPI _clientAPI;

        public WebPopupPageViewModel(INavigationService navigationService, IClientAPI clientAPI) : base(navigationService)
        {
           
        }

        public string _Name;
        public string Name
        {
            get
            {
                return _Name;
            }

            set
            {
                SetProperty(ref _Name, value);
            }
        }


        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

        }

    }
}
