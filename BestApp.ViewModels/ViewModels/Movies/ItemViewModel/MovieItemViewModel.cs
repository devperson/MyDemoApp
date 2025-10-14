using Base.MVVM.ViewModels;
using BestApp.Abstraction.Main.AppService.Dto;

namespace BestApp.ViewModels.Movies.ItemViewModel
{
    public class MovieItemViewModel : Bindable
    {        
        public MovieItemViewModel()
        {
            
        }

        public MovieItemViewModel(MovieDto movieDto)
        {
            Id = movieDto.Id;
            Name = movieDto.Name;
            Overview = movieDto.Overview;
            PosterUrl = movieDto.PosterUrl;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Overview { get; set; }
        public string PosterUrl { get; set; }
    }
}
