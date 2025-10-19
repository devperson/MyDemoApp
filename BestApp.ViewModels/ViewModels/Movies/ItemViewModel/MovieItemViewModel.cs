using Base.MVVM.ViewModels;
using BestApp.Abstraction.Main.AppService.Dto;

namespace BestApp.ViewModels.Movies.ItemViewModel
{
    public class MovieItemViewModel : Bindable
    {
        private readonly MovieDto movieDto;

        public MovieItemViewModel()
        {
            
        }

        public MovieItemViewModel(MovieDto movieDto)
        {
            Id = movieDto.Id;
            Name = movieDto.Name;
            Overview = movieDto.Overview;
            PosterUrl = movieDto.PosterUrl;
            this.movieDto = movieDto;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Overview { get; set; }
        public string PosterUrl { get; set; }

        public MovieDto ToDto()
        {
            movieDto.Id = Id;
            movieDto.Name = Name;
            movieDto.Overview = Overview;
            movieDto.PosterUrl = PosterUrl;

            return movieDto;
        }
    }
}
