using BestApp.Abstraction.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestApp.Abstraction.Main.Infasructures.REST
{
    public interface IMovieRestService
    {
        Task<List<Movie>> GetMovieRestlist();
    }
}
