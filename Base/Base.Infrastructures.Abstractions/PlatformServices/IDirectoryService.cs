using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Abstractions.PlatformServices
{
    public interface IDirectoryService
    {
        string GetCacheDir();
        string GetAppDataDir();                
    }
}
