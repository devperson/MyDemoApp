using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Abstractions.PlatformServices;

public interface IVideoService
{
    Task<ThubnailInfo> GetThumbnail(string videoFilePath);
    Task<string> CompressVideo(string inputPath, CancellationToken cancellationToken);
}

public class ThubnailInfo
{
    public bool Success => !string.IsNullOrEmpty(FilePath);
    public Size ImageSize { get; set; }
    public string FilePath { get; set; }
    public bool IsPortrait
    {
        get
        {
            if (ImageSize.Width > 0)
            {
                return ImageSize.Height > ImageSize.Width;
            }

            return true;
        }
    }
}
