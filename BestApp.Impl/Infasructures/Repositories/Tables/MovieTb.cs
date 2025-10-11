using Base.Abstractions.Repository;
using SQLite;

namespace BestApp.Impl.Cross.Infasructures.Repositories.Tables
{
    internal class MovieTb : ITable
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Overview { get; set; }
        public string PosterUrl { get; set; }
    }
}
