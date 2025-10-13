using Base.Abstractions.Diagnostic;
using Base.Abstractions.Messaging;
using Base.Abstractions.REST;
using Base.Abstractions.REST.Exceptions;
using Base.Aspect;
using Base.Infrastructures.Abstractions.REST;
using BestApp.Abstraction.Domain.Entities;
using BestApp.Abstraction.Main.Infasructures.REST;
using Newtonsoft.Json;

namespace BestApp.Impl.Cross.Infasructures.REST
{
    [LogMethods]
    internal class MovieRestService : RestService, IMovieRestService
    {
        public MovieRestService(Lazy<ILoggingService> loggingService, 
                                Lazy<IAuthTokenService> authTokenService, 
                                Lazy<IRestClient> restClient, 
                                Lazy<IMessagesCenter> eventAggregator, 
                                RequestQueueList requestQueues) : base(loggingService, authTokenService, restClient, eventAggregator, requestQueues)
        {
        }

        private const string baseImageHost = "https://image.tmdb.org/t/p/w300/";
        private readonly Uri ImageBaseUrl = new Uri(baseImageHost);
        

        public async Task<List<Movie>> GetMovieRestlist()
        {
            var result = await Get<MovieListResponse>(new RestRequest()
            {
                ApiEndpoint = "movie/popular?api_key=424f4be6472e955cadf36e104d8762d7",
                WithBearer = false
            });

            if(result.Success)
            {
                var list = result.Movies.Select(s =>
                {
                    var posterUrl = string.Empty;
                    if (s.PosterPath.StartsWith("/"))
                    {
                        var path = s.PosterPath.Substring(1);
                        posterUrl = new Uri(ImageBaseUrl, path).ToString();
                    }

                    var movie = new Movie()
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Overview = s.Overview,
                        PosterUrl = posterUrl
                    };
                    return movie;
                }).ToList();

                return list;
            }
            else
            {
                throw new RestApiException(result.ErrorCode);
            }
        }
    }

    internal class MovieListResponse : ResponseBase
    {
        [JsonProperty("page")]
        public int Page { get; set; }
        [JsonProperty("total_pages")]
        public int TotalPages { get; set; }
        [JsonProperty("total_results")]
        public int TotalResults { get; set; }
        [JsonProperty("results")]
        public List<MovieRestModel> Movies { get; set; }
    }

    internal class MovieRestModel
    {
        public int Id { get; set; }
        [JsonProperty("title")]
        public string Name { get; set; }
        [JsonProperty("poster_path")]
        public string PosterPath { get; set; }
        public string Overview { get; set; }
    }
}
