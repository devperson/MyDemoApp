using Base.Abstractions.AppService;
using Base.Abstractions.Diagnostic;
using Base.Aspect;
using Base.Infrastructures.Abstractions.Repository;
using BestApp.Abstraction.Domain.Entities;
using BestApp.Abstraction.Main.AppService;
using BestApp.Abstraction.Main.AppService.Dto;
using BestApp.Abstraction.Main.Infasructures.REST;
using MapsterMapper;

namespace BestApp.Impl.Cross.AppService
{
    [LogMethods]
    internal class MoviesService : IMoviesService
    {
        private readonly Lazy<IRepository<Movie>> movieRepository;
        private readonly Lazy<IMovieRestService> movieRestService;
        private readonly Lazy<ILoggingService> loggingService;
        private readonly Lazy<IMapper> mapper;

        public MoviesService(Lazy<IRepository<Movie>> movieRepository,
                            Lazy<IMovieRestService> movieRestService,
                            Lazy<ILoggingService> loggingService,
                            Lazy<IMapper> mapper)
        {
            this.movieRepository = movieRepository;
            this.movieRestService = movieRestService;
            this.loggingService = loggingService;
            this.mapper = mapper;
        }

        public Task<Some<List<MovieDto>>> GetListAsync(int count = -1, int skip = 0, bool remoteList = false)
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

                    if (canLoadLocal)
                    {
                        var dtoList = localList?.Select(s => mapper.Value.Map<MovieDto>(s)).ToList();
                        return new Some<List<MovieDto>>(dtoList);
                    }
                    else
                    {
                        //download all list
                        var remoteList = await movieRestService.Value.GetMovieRestlist();
                        await movieRepository.Value.ClearAsync($"{nameof(MoviesService)}: Delete all items requested when syncing");
                        await movieRepository.Value.AddAllAsync(remoteList);

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

        public Task<Some<MovieDto>> AddAsync(string name, string overview, string posterUrl)
        {
            return Task.Run(async () =>
            {
                try
                {
                    var movie = Movie.Create(name, overview, posterUrl);
                    await this.movieRepository.Value.AddAsync(movie);

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

        public Task<Some<MovieDto>> UpdateAsync(MovieDto dtoModel)
        {
            return Task.Run(async () =>
            {
                try
                {
                    var movie = mapper.Value.Map<Movie>(dtoModel);
                    await this.movieRepository.Value.UpdateAsync(movie);

                    var outDto = mapper.Value.Map<MovieDto>(movie);
                    return new Some<MovieDto>(outDto);
                }
                catch (Exception ex)
                {
                    loggingService.Value.TrackError(ex);
                    return new Some<MovieDto>(ex);
                }
            });
        }

       
        public Task<Some<int>> RemoveAsync(MovieDto dtoModel)
        {
            return Task.Run(async () =>
            {
                try
                {
                    var movie = mapper.Value.Map<Movie>(dtoModel);
                    var res = await this.movieRepository.Value.RemoveAsync(movie);

                    return new Some<int>(res);
                }
                catch (Exception ex)
                {
                    loggingService.Value.TrackError(ex);
                    return new Some<int>(ex);
                }
            });
        }
    }
}
