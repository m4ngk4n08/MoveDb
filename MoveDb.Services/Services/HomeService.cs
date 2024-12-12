using Microsoft.Extensions.Options;
using System.Text.Json;
using MoveDb.Services.Data;
using MoveDb.Services.IServices;
using MoveDb.Services.Data.Entities;
using MoveDb.Services.Services.IServices;
using System.Text.RegularExpressions;
using System.Text.Json.Serialization;
using System.Dynamic;
using Titanium.Web.Proxy.Http;
using Microsoft.Extensions.Logging;
using System.Web;
using Titanium.Web.Proxy.Http.Responses;
using Newtonsoft.Json.Linq;

namespace MoveDb.Services.Services {
    public class HomeService : IHomeService {
        private readonly IApiService _apiService;
        private readonly APISettings _apiSettings;

        public HomeService(
            IApiService apiService,
            IOptionsSnapshot<AppSettings> optionAccessor)
        {
            _apiService = apiService;
            _apiSettings = optionAccessor.Value.APISettings;
        }

        public async Task<IEnumerable<Result>> Get()
        {
            try
            {
                var genreDict = new GenreDict();
                var genres = genreDict.Genres;
                var movieTrendRequest = _apiService.APIRequests(_apiSettings.TMDBAPISettings.BaseAppSettings.URL.MovieTrendDay);
                var parsedJson = JsonSerializer.Deserialize<ResponseContent>(movieTrendRequest.Result);
                var results = new List<Result>();

                if (parsedJson != null)
                {
                    foreach (var result in parsedJson.Results)
                    {
                        var bannerMovieTrendRequest = _apiService.APIRequests(_apiSettings.TMDBAPISettings.BaseAppSettings.URL.Banner.Replace("{}", Convert.ToString(result.Id)));

                        var parsedImageResponse = JsonSerializer.Deserialize<ImageRoot>(bannerMovieTrendRequest.Result);

                        if (parsedImageResponse?.Posters != null && parsedImageResponse.Posters.Any())
                        {
                            var backdropImagePath = string.Empty;

                            if (parsedImageResponse.Posters[0].Width > 800)
                            {
                                backdropImagePath = string.Format("{0}{1}", _apiSettings.TMDBAPISettings.BaseAppSettings.URL.ImageUrl, result.BackdropPath ?? string.Empty);
                            }
                            else
                            {
                                backdropImagePath = string.Format("{0}{1}", _apiSettings.TMDBAPISettings.BaseAppSettings.URL.ImageUrl, parsedImageResponse?.Posters[0].FilePath ?? string.Empty);

                            }

                            result.BackdropPath = backdropImagePath;
                            results.Add(result);
                        }

                        // TODO: Duration, IMDB rate, Genre
                        var genreString = new List<string>();
                        if (result.GenreIds != null)
                        {
                            result.GenreIds.ForEach(genreIds =>
                            {
                                var hasGenre = genres.Where(j => j.Key.Equals(genreIds)).Select(j => j.Value).FirstOrDefault();

                                if (!string.IsNullOrWhiteSpace(hasGenre)) genreString.Add(hasGenre);

                            });
                        }

                        result.Genres = genreString;
                        result.Name = result.OriginalName?.Trim()
                                    ?? result.OriginalTitle?.Trim()
                                    ?? result.Name;
                    }

                    parsedJson.Results = results;
                    return parsedJson.Results;
                }

                throw new ArgumentException("Get Movies is not returning a value!");
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.ToString());
            }
        }

        public async Task<ResultResponse> GetMovieDetails(int id)
        {
            var movieUrl = _apiSettings.MovieUrls.VidSrc;
            var requestMovieDetails = await _apiService
                .APIRequests(_apiSettings.TMDBAPISettings.BaseAppSettings.URL.MovieDetailByIdUrl.Replace("{id}", id.ToString()));

            var response = JsonSerializer.Deserialize<ResultResponse>(requestMovieDetails);

            if (response != null)
            {
                var resultResponse = new ResultResponse
                {
                    Id = response.Id,
                    Name = response.OriginalTitle,
                    Genres = response.Genres,
                    VoteAverage = response.VoteAverage,
                    Url = movieUrl.Replace("{id}", id.ToString()),
                    PosterPath = string.Format("{0}{1}", _apiSettings.TMDBAPISettings.BaseAppSettings.URL.ImageUrl, response.PosterPath),
                    Runtime = response.Runtime,
                    ReleaseDate = response.ReleaseDate,
                };

                return resultResponse;
            }
            else
            {
                throw new ArgumentException("Movie details is not returning a value!");
            }
        }

        public async Task<List<ResultResponse>> GetMovieSuggestion(string message)
        {
            try
            {
                var requestBody = BuildGeminiResponse(message);

                //var geminiResponseBody = await _apiService.GeminiApiRequest(requestBody);

                var geminiResponseBody = @"{
  ""candidates"": [
    {
      ""content"": {
        ""parts"": [
          {
            ""text"": ""\""Summer Wars\""\n\""Sword Art Online: The Movie - Ordinal Scale\""\n\""Digimon Adventure tri.\""\n\""Accel World: Infinite Burst\""\n\""No Game No Life: Zero\""\n\""Code Geass: Akito the Exiled\""""
          }
        ],
        ""role"": ""model""
      },
      ""finishReason"": ""STOP"",
      ""index"": 0,
      ""safetyRatings"": [
        {
          ""category"": ""HARM_CATEGORY_SEXUALLY_EXPLICIT"",
          ""probability"": ""NEGLIGIBLE""
        },
        {
          ""category"": ""HARM_CATEGORY_HATE_SPEECH"",
          ""probability"": ""NEGLIGIBLE""
        },
        {
          ""category"": ""HARM_CATEGORY_HARASSMENT"",
          ""probability"": ""NEGLIGIBLE""
        },
        {
          ""category"": ""HARM_CATEGORY_DANGEROUS_CONTENT"",
          ""probability"": ""NEGLIGIBLE""
        }
      ]
    }
  ],
  ""usageMetadata"": {
    ""promptTokenCount"": 48,
    ""candidatesTokenCount"": 53,
    ""totalTokenCount"": 101
  },
  ""modelVersion"": ""gemini-1.0-pro""
}
";

                var constructMovieTitles = await ConstructMovieTitles(geminiResponseBody);
                return constructMovieTitles;

            }
            catch (Exception ex)
            {
                throw new ArgumentException("There's an error on the Get Movie Suggestions.", ex);
                throw;
            }
        }

        private async Task<List<ResultResponse>> ConstructMovieTitles(string? geminiResponseBody)
        {
            if (!string.IsNullOrEmpty(geminiResponseBody))
            {
                var geminiResponse = JsonSerializer.Deserialize<GeminiResponse>(geminiResponseBody, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = null, // Ensure no default policy
                    DefaultIgnoreCondition = JsonIgnoreCondition.Never // Include all properties
                });

                var response = new List<ResultResponse>();

                if (geminiResponse?.Candidates[0] != null)
                {
                    var pattern = @"\p{L}+";
                    var titles = geminiResponse?.Candidates[0].Content.Parts[0].Text;
                    foreach (var cleanTitle in titles.Split('"'))
                    {
                        if (Regex.IsMatch(cleanTitle, pattern))
                        {
                            var fetchMovieDetails = await FetchMovieDetailsAsync(cleanTitle);
                            response.Add(fetchMovieDetails);
                        }
                    }
                }
                return response;
            }

            return new List<ResultResponse>();
        }

        private GemeniRequest BuildGeminiResponse(string message)
        {
            message = string.Format("Provide 6 movie that are similar to {0}. I want you to provide the titles only no other context, separate it by quotation.", message);
            return new GemeniRequest
            {
                Content = new List<Content>
                {
                    new Content
                    {
                        Role = "",
                        Parts = new List<Part>
                        {
                            new Part { Text = message }
                        }
                    }
                },
                GenerationConfig = new GenerationConfig
                {
                    Temperature = 0.9,
                    TopK = 50,
                    TopP = 0.95,
                    MaxOutputTokens = 4096,
                    StopSequences = new List<string>()
                },
                SafetySettings = new List<object>()
            };
        }

        private async Task<ResultResponse> FetchMovieDetailsAsync(string title)
        {
            try
            {
                //TODO: Get Movie Suggestion Link
                var details = await _apiService.APIRequests(_apiSettings.TMDBAPISettings.BaseAppSettings.URL.MovieDetailByNameUrl.Replace("{title}", title));
                var url = _apiSettings.MovieUrls.VidSrc;

                var responseContent = JsonSerializer.Deserialize<ResponseContent>(details);
                if (responseContent != null && responseContent.Results != null && responseContent.Results.Any())
                {
                    if (!string.IsNullOrWhiteSpace(responseContent.Results[0].PosterPath))
                    {
                        var id = responseContent?.Results[0].Id.ToString();
                        var resultResponse = new ResultResponse
                        {
                            Id = Convert.ToInt32(id),
                            Name = responseContent.Results[0].OriginalTitle,
                            VoteAverage = responseContent.Results[0].VoteAverage,
                            Url = url.Replace("{id}", responseContent.Results[0].Id.ToString()),
                            PosterPath = string.Format("{0}{1}", _apiSettings.TMDBAPISettings.BaseAppSettings.URL.ImageUrl, responseContent.Results[0].PosterPath),
                            Overview = responseContent.Results[0].Overview
                        };

                        return resultResponse;
                    }
                }
                return new ResultResponse();
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Error fetching details for movie: {title}", ex);
            }
        }

    }
}
