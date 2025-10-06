using BestApp.Abstraction.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationTest.Impl
{
    internal class ConstImpl : IConstants
    {
        public string ServerUrlHost { get; set; } = "https://api.themoviedb.org/3/";
    }
}
