using BestApp.Abstraction.General.AppService.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestApp.Abstraction.General.AppService
{
    public interface IMovieService 
    {        
        Task<Some<MovieDto>> Add(string name, string overview, string posterUrl);
        Task<Some<List<MovieDto>>> GetList(int count = -1, int skip = 0, bool remoteList = false);
    }
}
