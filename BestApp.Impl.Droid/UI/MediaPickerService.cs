using BestApp.Abstraction.Main.PlatformServices;
using BestApp.Abstraction.Main.UI;
using Common.Abstrtactions;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Media;
using Microsoft.Maui.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeviceInfo = Microsoft.Maui.Devices.DeviceInfo;

namespace BestApp.Impl.Droid.UI;

public class MediaPickerService : IMediaPickerService
{
    public string TAG = "MediaPickerService";
    public const int MaxImageSize = 500;
    private readonly Lazy<IResizeImageService> resizeImageService;
    private readonly Lazy<IDevice> device;
    private readonly Lazy<IVideoService> videoService;
    private readonly Lazy<ILoggingService> loggingService;
    private string storageError = "The Storage permission was not granted. It is required to grant the Storage permission to choose media files.";
    public MediaPickerService(Lazy<IResizeImageService> resizeImageService, Lazy<IDevice> device, Lazy<IVideoService> videoService, Lazy<ILoggingService> loggingService)
    {
        this.resizeImageService = resizeImageService;
        this.device = device;
        this.videoService = videoService;
        this.loggingService = loggingService;
    }

    public async Task<MediaResult> TakePhotoAsync(PhotoOptions photoOptions)
    {
        try
        {
            var photo = await MediaPicker.CapturePhotoAsync();
            if (photo == null)
                return null;

            if (photoOptions.ShrinkPhoto)
            {
                return await ShrinkPhoto(photo, photoOptions.MaxSize, photoOptions.Quality, photoOptions.WithFilePath);
            }
            else
            {
                if (photoOptions.WithFilePath && DeviceInfo.Platform == DevicePlatform.iOS)
                {
                    //Xamarin.Essentials CapturePhotoAsync doesn't contain image file path for ios platform so create it
                    var filePath = await CreatePath(photo, permament: true);
                    var newPhotoFile = new FileResult(filePath, photo.ContentType);
                    newPhotoFile.FileName = photo.FileName;
                    return ConvertToMediaResult(newPhotoFile);
                }
                else
                {
                    return ConvertToMediaResult(photo);
                }
            }

        }
        catch (PermissionException ex)
        {
            loggingService.Value.LogError(ex, $"{TAG}Failed to capture Photo, exception details");

            var result = new MediaResult();
            result.Status = Status.Error;
            if (DeviceInfo.Platform == DevicePlatform.Android)
            {
                result.MessageError = storageError;
            }
            else
            {
                result.MessageError = ex.ToString();
            }
            return result;
        }
    }


    public async Task<MediaResult> GetPhotoAsync(PhotoOptions photoOptions)
    {
        try
        {
            var photo = await MediaPicker.PickPhotoAsync();
            if (photoOptions.ShrinkPhoto)
            {
                return await ShrinkPhoto(photo, photoOptions.MaxSize, photoOptions.Quality, photoOptions.WithFilePath);
            }
            else
            {
                if (photo == null)
                {
                    var result = new MediaResult();
                    result.Status = Status.Canceled;
                    return result;
                }
                else
                {
                    return ConvertToMediaResult(photo);
                }
            }

        }
        catch (PermissionException ex)
        {
            loggingService.Value.LogError(ex, $"{TAG}Failed to get Photo, exception details");

            var result = new MediaResult();
            result.Status = Status.Error;
            if (DeviceInfo.Platform == DevicePlatform.Android)
            {
                result.MessageError = storageError;
            }
            else
            {
                result.MessageError = ex.Message;
            }
            return result;
        }
    }



    public async Task<MediaResult> GetVideoAsync()
    {
        try
        {
            //todo: add compress video https://github.com/Xamarians/VideoCompressionSample
            var video = await MediaPicker.PickVideoAsync();
            if (video != null)
            {
                var thumbnail = await videoService.Value.GetThumbnail(video.FullPath);

                var result = new MediaResult();
                result.MimeType = video.ContentType;
                //result.GetFileStream = () => stream;
                result.FileName = video.FileName;
                result.Type = MediaType.Video;
                result.VideoThumbnailInfo = thumbnail;

                if (DeviceInfo.Platform == DevicePlatform.iOS)
                {
                    //ios creates temporary path which will be deleted after app restart
                    //create permament file path
                    var path = await CreatePath(video, permament: true, isVideo: true);
                    result.FilePath = path;
                }
                else
                {
                    result.FilePath = video.FullPath;
                }

                return result;
            }
            else
            {
                var result = new MediaResult();
                result.Status = Status.Canceled;
                return result;
            }
        }
        catch (PermissionException ex)
        {
            loggingService.Value.LogError(ex, $"{TAG}Failed to select Video, exception details");

            var result = new MediaResult();
            result.Status = Status.Error;
            if (DeviceInfo.Platform == DevicePlatform.Android)
            {
                result.MessageError = storageError;
            }
            else
            {
                result.MessageError = ex.Message;
            }
            return result;
        }
    }

    public async Task<MediaResult> TakeVideoAsync()
    {
        try
        {
            var video = await MediaPicker.CaptureVideoAsync();
            if (video != null)
            {
                string videoPath = video.FullPath;
                var thumbnail = await videoService.Value.GetThumbnail(videoPath);

                var result = new MediaResult();
                result.MimeType = video.ContentType;
                //result.GetFileStream = () => stream;
                result.FileName = video.FileName;
                result.FilePath = videoPath;
                result.Type = MediaType.Video;
                result.VideoThumbnailInfo = thumbnail;

                return result;
            }
            else
            {
                var result = new MediaResult();
                result.Status = Status.Canceled;
                return result;
            }
        }
        catch (PermissionException ex)
        {
            loggingService.Value.LogError(ex, $"{TAG}Failed to take Video, exception details");

            var result = new MediaResult();
            result.Status = Status.Error;
            if (DeviceInfo.Platform == DevicePlatform.Android)
            {
                result.MessageError = storageError;
            }
            else
            {
                result.MessageError = ex.Message;
            }
            return result;
        }
    }

    private async Task<string> CreatePath(FileResult file, bool permament = false, bool isVideo = false)
    {
        var stream = await file.OpenReadAsync().ConfigureAwait(false);
        var fileResultPath = file.FileName;

        if (stream != null && stream != Stream.Null)
        {
            var destPath = string.Empty;
            if (permament)
            {
                string personalForlder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                var extension = Path.GetExtension(file.FileName);
                var mediaName = isVideo ? "movie" : "photo";
                destPath = Path.Combine(personalForlder, $"copied_{mediaName}_{Guid.NewGuid()}{extension}");
            }
            else
            {
                destPath = Path.Combine(FileSystem.CacheDirectory, file.FileName);
            }
            using (var fileStream = new FileStream(destPath, FileMode.CreateNew))
            {
                await stream.CopyToAsync(fileStream).ConfigureAwait(false);
                await stream.FlushAsync().ConfigureAwait(false);
            }

            fileResultPath = destPath;

            stream.Dispose();
        }
        else
        {
            throw new Exception("Failed to copy video file becasue FileResult doesn't have file stream");
        }

        return fileResultPath;
    }


    public async Task<MediaResult> ShrinkPhoto(FileResult photo, int maxSize, float quality, bool withFilePath = false)
    {
        if (photo != null)
        {
            var rotation = resizeImageService.Value.GetRequiredRotation(photo);

            using (var stream = await photo.OpenReadAsync())
            {
                //read all bytes from stream

                var imgBytes = new byte[stream.Length];
                stream.Read(imgBytes, 0, imgBytes.Length);
                stream.Close();

                File.Delete(photo.FullPath);

                //scale down the image so it will not be larger that 500px in height and width
                var result = await ShrinkPhoto(imgBytes, photo.FileName, photo.ContentType, quality, maxWidth: maxSize, maxHeight: maxSize, withFilePath: withFilePath, rotation: rotation);

                return result;
            }
        }
        else
        {
            var result = new MediaResult();
            result.Status = Status.Canceled;
            return result;
        }
    }

    public async Task<MediaResult> ShrinkPhoto(string path)
    {
        var bytes = File.ReadAllBytes(path);
        var result = await ShrinkPhoto(bytes, null, null, 97f);
        return result;
    }

    public async Task<MediaResult> ShrinkPhoto(byte[] bytes, string fileName, string contentType, float quality, int maxWidth = 500, int maxHeight = 500, bool withFilePath = false, int rotation = 0)
    {
        return await Task.Run(() =>
        {
            //var bytes = File.ReadAllBytes(filePath);                
            var resizeResult = resizeImageService.Value.ResizeImage(bytes, contentType, maxWidth, maxHeight, quality, rotation, shouldSetUniqueName: true);
            var resizedStream = new MemoryStream(resizeResult.Image);

            var result = new MediaResult();
            result.MimeType = resizeResult.ContentType;
            result.GetFileStream = () => resizedStream;

            result.IsPortrait = resizeResult.IsPortrait;

            if (withFilePath)
            {
                if (!string.IsNullOrEmpty(resizeResult.FilePath))
                {
                    result.FilePath = resizeResult.FilePath;
                    result.FileName = Path.GetFileName(resizeResult.FilePath);
                }
                else
                {
                    string documentsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                    string jpgFilename = Path.Combine(documentsDirectory, fileName);
                    File.WriteAllBytes(jpgFilename, resizeResult.Image);
                    result.FilePath = jpgFilename;
                    result.FileName = fileName;
                }
            }

            return result;
        });

    }

    private MediaResult ConvertToMediaResult(FileResult file)
    {
        var result = new MediaResult();
        result.MimeType = file.ContentType;
        result.FileName = file.FileName;
        result.FilePath = file.FullPath;
        result.Type = MediaType.Image;

        return result;
    }
}
