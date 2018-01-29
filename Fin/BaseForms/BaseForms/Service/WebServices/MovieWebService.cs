using System;
using System.Globalization;
using System.Threading.Tasks;
using BaseForms.Constants;
using BaseForms.Interfaces.IConnectors;
using BaseForms.Interfaces.IWebServices;
using BaseForms.Models.Movie;
using Newtonsoft.Json;

namespace BaseForms.Service.WebServices
{
    public class MovieWebService : IMovieWebService
    {
        IWebClientService conection;

        public MovieWebService(IWebClientService conection)
        {
            this.conection = conection;
        }

        public async Task<MovieResponse> Movie()
        {
            var url = new Uri(string.Format(CultureInfo.InvariantCulture, ConfigConstants.baseUrl));
            var result = await conection.Client().GetAsync(url);
            if (result.IsSuccessStatusCode)
            {
                string content = await result.Content.ReadAsStringAsync();
                if (content != null)
                {
                    var response = JsonConvert.DeserializeObject<MovieResponse>(content);
                    response.IsCorrect = true;
                    return response;
                }
            }
            return new MovieResponse() { Mensage = "Servicio no disponible" };
        }
    }
}
