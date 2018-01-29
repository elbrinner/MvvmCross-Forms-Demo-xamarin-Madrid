using System;
using System.Threading.Tasks;
using BaseForms.Models.Movie;

namespace BaseForms.Interfaces.IWebServices
{
    public interface IMovieWebService
    {
        Task<MovieResponse> Movie();
    }
}
