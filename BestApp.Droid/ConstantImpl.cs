using Base.Abstractions;

namespace BestApp.X
{
    internal class ConstantImpl : IConstants
    {
        public string ServerUrlHost { get; set; } = "https://api.themoviedb.org/3/";
    }
}
