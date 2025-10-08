using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestApp.Abstraction.Main.Platform
{
    public interface IDirectoryService
    {
        string GetCacheDir();
        string GetAppDataDir();                
    }
}
