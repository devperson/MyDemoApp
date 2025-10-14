using Base.Abstractions.PlatformServices;
using System.IO.Compression;

namespace Base.Impl.Droid.PlatformServices;

public class ZipService : IZipService
{
    public void CreateFromDirectory(string dir, string filePath)
    {
        ZipFile.CreateFromDirectory(dir, filePath);
    }

    public void ExtractToDirectory(string filePath, string dir, bool overwrite)
    {
        ZipFile.ExtractToDirectory(filePath, dir, overwrite);
    }

    public Task CreateFromFileAsync(string filePath, string zipPath)
    {
        return Task.Run(() =>
        {
            using (var zip = ZipFile.Open(zipPath, ZipArchiveMode.Create))
            {
                var entryName = Path.GetFileName(filePath);
                zip.CreateEntryFromFile(filePath, entryName);
            }
        });
    }
}
