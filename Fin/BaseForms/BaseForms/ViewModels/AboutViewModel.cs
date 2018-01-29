using System;
using BaseForms.Interfaces.IWebServices;
using BaseForms.Models.Movie;
using BaseForms.ViewModels;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;

namespace BaseForms.Core.ViewModels
{

    public class AboutViewModel : BaseViewModel
    {
       

        private IMvxCommand closeCommand;
        public IMvxCommand CloseCommand => closeCommand = closeCommand ?? new MvxCommand(DoCloseHandler);

        private ResultMovie selectedMovie;

        public AboutViewModel(IMvxNavigationService navigationService, IMovieWebService webMovieService) : base(navigationService, webMovieService)
        {
        }

        public ResultMovie SelectedMovie
        {
            get
            {
                return this.selectedMovie;
            }

            set
            {
                this.selectedMovie = value;
                this.RaisePropertyChanged();
            }
        }

        private void DoCloseHandler()
        {
            Close(this);
        }

        public override void Prepare(object parameter)
        {
            this.SelectedMovie = (ResultMovie)Convert.ChangeType(parameter, typeof(ResultMovie));
            this.Title = this.SelectedMovie.Title;
        }
    }
}
