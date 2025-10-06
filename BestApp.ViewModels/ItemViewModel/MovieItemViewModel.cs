using BestApp.Abstraction.General.AppService.Dto;
using BestApp.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestApp.ViewModels.ItemViewModel
{
    public class MovieItemViewModel : Bindable
    {
        public MovieDto MovieDto { get; }
        public MovieItemViewModel(MovieDto movieDto)
        {
            MovieDto = movieDto;
        }

        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Cost { get; set; }        
    }
}
