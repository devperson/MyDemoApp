using BestApp.Abstraction.Main.AppService.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestApp.Abstraction.Main.AppService
{
    public interface IMoviesService 
    {
        Task<Some<List<MovieDto>>> GetListAsync(int count = -1, int skip = 0, bool remoteList = false);
        Task<Some<MovieDto>> AddAsync(string name, string Overview, string posterUrl);
        Task<Some<MovieDto>> UpdateAsync(MovieDto dtoModel);
        Task<Some<int>> RemoveAsync(MovieDto dtoModel);
    }
}
