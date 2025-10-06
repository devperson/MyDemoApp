using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestApp.Abstraction.General.AppService.Dto
{
    public class MovieDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Overview { get; set; }
        public string PosterUrl { get; set; }
    }
}
