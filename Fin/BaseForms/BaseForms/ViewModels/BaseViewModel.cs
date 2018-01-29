using System;
using BaseForms.Interfaces.IWebServices;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;

namespace BaseForms.ViewModels
{
    public class BaseViewModel : MvxViewModel<object>
    {
        protected readonly IMvxNavigationService navigationService;
        protected readonly IMovieWebService webMovieService;
        protected bool isBusy;
        protected string title;
        public BaseViewModel(IMvxNavigationService navigationService,IMovieWebService webMovieService)
        {
            this.navigationService = navigationService;
            this.webMovieService = webMovieService;
        }

        public bool IsBusy
        {
            get
            {
                return this.isBusy;
            }
            set
            {
                this.isBusy = value;
                this.RaisePropertyChanged(() => this.IsBusy);
            }
        }

        public string Title
        {
            get
            {
                return this.title;
            }
            set
            {
                this.title = value;
                this.RaisePropertyChanged(() => this.Title);
            }
        }



        public override void Prepare(object parameter)
        {

        }
    }
}
