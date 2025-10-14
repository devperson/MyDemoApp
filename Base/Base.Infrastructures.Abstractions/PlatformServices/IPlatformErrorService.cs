using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Abstractions.PlatformServices
{
    public interface IPlatformErrorService
    {
        /// <summary>
        /// The error that happens very rarely and related to native bugs
        /// </summary>
        /// <returns></returns>
        bool IsHttpRareError(Exception ex);
    }
}
