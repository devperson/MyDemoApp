using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Abstractions.Platform
{
    public interface IDirectoryService
    {
        string GetCacheDir();
        string GetAppDataDir();                
    }
}
