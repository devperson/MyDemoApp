using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Abstractions.Platform;

public interface IResizeImageService
{
    ImageResizeResult ResizeImage(byte[] imageData, string originalContentType, int maxWidth, int maxHeight, float quality = 97f, int rotation = 0, bool shouldSetUniqueName = false);
    ImageResizeResult ResizeNativeImage(object image, string originalContentType, int maxWidth, int maxHeight, int rotation = 0, float quality = 97f, bool shouldSetUniqueName = false);
    int GetRequiredRotation(object fileResult);
    int GetRequiredRotation(string filePath);
}

public class ImageResizeResult
{
    public bool IsResized { get; set; } = true;
    public object NativeImage { get; set; }
    public byte[] Image { get; set; }
    public string ContentType { get; set; } = string.Empty;
    public Size ImageSize { get; set; }
    public string FileExtension => ContentType.Contains("png") ? ".png" : ".jpg";
    public string FilePath { get; set; }
    public bool IsPortrait
    {
        get
        {
            if (ImageSize == Size.Empty)
                return true;

            if (ImageSize.Height > ImageSize.Width)
                return true;
            else
                return false;
        }
    }
}