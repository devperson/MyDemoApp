using Base.Abstractions;

namespace IntegrationTest.Impl
{
    internal class ConstImpl : IConstants
    {
        public string ServerUrlHost { get; set; } = "https://api.themoviedb.org/3/";
    }
}
