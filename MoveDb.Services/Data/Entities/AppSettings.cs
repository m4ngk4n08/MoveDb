using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

namespace MoveDb.Services.Data.Entities {
    public class AppSettings : IOptions<AppSettings> {

        public APISettings APISettings { get; set; }

        AppSettings IOptions<AppSettings>.Value => this;
    }

    public class APISettings {
        public TMDBAPISettings TMDBAPISettings { get; set; }

        public GeminiApi GeminiApi { get; set; }

        public MovieUrls MovieUrls { get; set; }
    }

    public class GeminiApi
    {
        public string APIKeyGemini { get; set; }

        public string APIUrlGemini { get; set; }
    }

    public class MovieUrls
    {
        public string VidSrc { get; set; }
    }

    public class TMDBAPISettings {
        public BaseAppSettings BaseAppSettings { get; set; }
    }

    public class URL
    {
        public string MovieTrendDay { get; set; }
        public string MovieTrendWeek { get; set; }
        public string Banner { get; set; }
        public string ImageUrl { get; set; }
        public string MovieDetailByNameUrl { get; set; }
        public string MovieDetailByIdUrl { get; set; }
    }

    public class BaseAppSettings {

        [Required]
        public string BearerToken { get; set; }

        [Required]
        [Url]
        public URL URL { get; set; }
    }
}
