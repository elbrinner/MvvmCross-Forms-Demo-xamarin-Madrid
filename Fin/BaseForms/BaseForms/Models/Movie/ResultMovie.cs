using System;
using BaseForms.Constants;
using BaseForms.Converters.Json;
using Newtonsoft.Json;

namespace BaseForms.Models.Movie
{
    public class ResultMovie 
    {
        [JsonProperty("vote_count")]
        public int VoteCount { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonConverter(typeof(BoolConverter))]
        [JsonProperty("video")]
        public bool Video { get; set; }

        [JsonProperty("vote_average")]
        public double VoteAverage { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("original_title")]
        public string OriginalTitle { get; set; }

        [JsonProperty("overview")]
        public string Overview { get; set; }

        [JsonProperty("backdrop_path")]
        public string Backdrop { get; set; }

        [JsonProperty("original_language")]
        public string OriginalLanguage { get; set; }

        public string ImgSmall => string.Format("{0}{1}", ConfigConstants.imgSmall, Backdrop);

        public string ImgBig => string.Format("{0}{1}", ConfigConstants.imgBig, Backdrop);

        /*
         * Propiedades sin tratar
        public double popularity { get; set; }
        public string poster_path { get; set; }
        public List<object> genre_ids { get; set; }
        public string backdrop_path { get; set; }
        public bool adult { get; set; }
        public string release_date { get; set; }
        */

    }
}
