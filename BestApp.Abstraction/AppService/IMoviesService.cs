using Base.Abstractions.AppService;
using BestApp.Abstraction.Main.AppService.Dto;

namespace BestApp.Abstraction.Main.AppService
{
    public interface IMoviesService 
    {
        Task<Some<List<MovieDto>>> GetListAsync(int count = -1, int skip = 0, bool remoteList = false);
        Task<Some<MovieDto>> GetById(int id);
        Task<Some<int>> AddAsync(string name, string Overview, string posterUrl);
        Task<Some<int>> UpdateAsync(MovieDto dtoModel);
        Task<Some<int>> RemoveAsync(int id);
    }
}
