using BestApp.Abstraction.Domain.Entities;
using BestApp.Abstraction.Main.AppService;
using BestApp.Abstraction.Main.AppService.Dto;
using BestApp.Abstraction.Main.Infasructures;
using BestApp.Abstraction.Main.Infasructures.REST;
using Common.Abstrtactions;
using Logging.Aspects;
using MapsterMapper;
using static SQLite.SQLite3;

namespace BestApp.Impl.Cross.AppService
{
    [LogMethods]
    internal class MovieService : IMovieService
    {
        private readonly Lazy<IRepository<Movie>> movieRepository;
        private readonly Lazy<IMovieRestService> movieRestService;
        private readonly Lazy<ILoggingService> loggingService;
        private readonly Lazy<IMapper> mapper;

        public MovieService(Lazy<IRepository<Movie>> movieRepository,
                            Lazy<IMovieRestService> movieRestService,
                            Lazy<ILoggingService> loggingService,
                            Lazy<IMapper> mapper)
        {
            this.movieRepository = movieRepository;
            this.movieRestService = movieRestService;
            this.loggingService = loggingService;
            this.mapper = mapper;
        }

        public Task<Some<MovieDto>> Add(string name, string overview, string posterUrl)
        {
            return Task.Run(async () =>
            {
                try
                {
                    var movie = Movie.Create(name, overview, posterUrl);
                    await this.movieRepository.Value.Add(movie);

                    var dtoMovie = mapper.Value.Map<MovieDto>(movie);
                    return new Some<MovieDto>(dtoMovie);
                }
                catch (Exception ex)
                {
                    loggingService.Value.TrackError(ex);
                    return new Some<MovieDto>(ex);
                }
            });
        }

        public Task<Some<MovieDto>> Update(string name, string overview, string posterUrl)
        {
            return Task.Run(async () =>
            {
                try
                {
                    var movie = Movie.Create(name, overview, posterUrl);
                    await this.movieRepository.Value.Update(movie);

                    var dtoMovie = mapper.Value.Map<MovieDto>(movie);
                    return new Some<MovieDto>(dtoMovie);
                }
                catch (Exception ex)
                {
                    loggingService.Value.TrackError(ex);
                    return new Some<MovieDto>(ex);
                }
            });
        }

        public Task<Some<List<MovieDto>>> GetList(int count = -1, int skip = 0, bool remoteList = false)
        {
            return Task.Run(async () =>
            {
                try
                {
                    bool canLoadLocal = true;
                    List<Movie> localList = null;
                    if (remoteList)
                    {
                        canLoadLocal = false;
                    }
                    else
                    {
                        localList = await this.movieRepository.Value.GetList();
                        canLoadLocal = localList.Count > 0;
                    }
                    
                    if(canLoadLocal)
                    {
                        var dtoList = localList?.Select(s => mapper.Value.Map<MovieDto>(s)).ToList();
                        return new Some<List<MovieDto>>(dtoList);
                    }
                    else
                    {
                        //download all list
                        var remoteList = await movieRestService.Value.GetMovieRestlist();
                        await movieRepository.Value.Clear($"{nameof(MovieService)}: Delete all items requested when syncing");
                        await movieRepository.Value.AddAll(remoteList);

                        //return dto list
                        var dtoList = remoteList.Select(s => mapper.Value.Map<MovieDto>(s)).ToList();
                        return new Some<List<MovieDto>>(dtoList);
                    }
                }
                catch (Exception ex)
                {
                    loggingService.Value.TrackError(ex);
                    return new Some<List<MovieDto>>(ex);
                }
            });
        }
    }
}
