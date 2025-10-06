using BestApp.Abstraction.General.AppService.Dto;
using BestApp.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestApp.ViewModels.Movies.ItemViewModel
{
    public class MovieItemViewModel : Bindable
    {        
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
