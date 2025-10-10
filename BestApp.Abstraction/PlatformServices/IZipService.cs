using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestApp.Abstraction.Main.PlatformServices;
public interface IZipService
{
    void CreateFromDirectory(string dir, string filePath);
    void ExtractToDirectory(string filePath, string dir, bool overwrite);
    Task CreateFromFileAsync(string filePath, string zipPath);
}
