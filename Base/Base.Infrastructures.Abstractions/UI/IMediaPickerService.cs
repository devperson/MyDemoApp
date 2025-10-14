using Base.Abstractions.PlatformServices;

namespace Base.Abstractions.UI;

public interface IMediaPickerService
{
    Task<MediaResult> TakePhotoAsync(PhotoOptions photoOptions);
    Task<MediaResult> GetPhotoAsync(PhotoOptions photoOptions);
    Task<MediaResult> GetVideoAsync();
    Task<MediaResult> TakeVideoAsync();
    Task<MediaResult> ShrinkPhoto(string path);
}

public class MediaResult
{
    public Func<Stream> GetFileStream { get; set; }
    public string MimeType { get; set; }
    //public string Extension { get; set; }
    public string FileName { get; set; }
    public string FilePath { get; set; }
    public MediaType Type { get; set; }
    public Status Status { get; set; } = Status.Succces;
    public string MessageError { get; set; }
    public ThubnailInfo VideoThumbnailInfo { get; set; }

    public bool IsPortrait { get; set; } = true;
}

public enum Status
{
    Succces,
    Error,
    Canceled
}
public enum MediaType
{
    Image,
    Video
}

public class PhotoOptions
{
    public int MaxSize { get; set; } = 500;
    public bool ShrinkPhoto { get; set; } = true;
    public bool WithFilePath { get; set; } = true;
    public float Quality { get; set; } = 97f;
}
