using System.Collections.Generic;
using System.Threading.Tasks;
using BaseForms.Interfaces.IWebServices;
using BaseForms.Models.Movie;
using BaseForms.ViewModels;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;

namespace BaseForms.Core.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
      
        public MainViewModel(IMvxNavigationService navigationService, IMovieWebService webMovieService) : base(navigationService, webMovieService)
        {
            
        }

        public override Task Initialize()
        {
            return base.Initialize();
        }

        public async override void ViewAppeared()
        {

            this.IsBusy = true;
            var response = await this.webMovieService.Movie();
            this.IsBusy = false;
            if (response.IsCorrect)
            {
                Movies = response.Movies;
            }
            else
            {
                //mostrar el mensaje de error
            }

        }

        private List<ResultMovie> movies;
        public List<ResultMovie> Movies
        {
            get
            {
                return this.movies;
            }

            set
            {
                this.movies = value;
                this.RaisePropertyChanged();

            }
        }
        private ResultMovie selectedMovie;
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
                this.OpenModal(value);
            }
        }

        private void OpenModal(ResultMovie value)
        {
            object parameter = value;
            navigationService.Navigate<AboutViewModel, object>(parameter);

        }
    }
}
