using AVFoundation;
using Foundation;
using System;
using System.IO;
using System.Threading.Tasks;
using UIKit;
using System.Threading;
using Base.Abstractions.PlatformServices;
using Base.Abstractions.Diagnostic;

namespace Base.Impl.iOS.PlatformServices;

public class VideoService : IVideoService
{
    private readonly Lazy<IResizeImageService> resizeImageService;
    private readonly Lazy<ILoggingService> loggingService;

    public VideoService(Lazy<IResizeImageService> resizeImageService, Lazy<ILoggingService> loggingService)
    {
        this.resizeImageService = resizeImageService;
        this.loggingService = loggingService;
    }

    public Task<ThubnailInfo> GetThumbnail(string videoFilePath)
    {
        CoreMedia.CMTime actualTime;
        NSError outError;
        using (var asset = AVAsset.FromUrl(NSUrl.FromFilename(videoFilePath)))
        using (var imageGen = new AVAssetImageGenerator(asset))
        {
            imageGen.AppliesPreferredTrackTransform = true;
            using (var imageRef = imageGen.CopyCGImageAtTime(new CoreMedia.CMTime(1, 1), out actualTime, out outError))
            {
                if (imageRef == null)
                    return null;
                var image = UIImage.FromImage(imageRef);

                var resizedImage = resizeImageService.Value.ResizeNativeImage(image, string.Empty, 500, 500, shouldSetUniqueName: true);
                return Task.Run(() =>
                {
                    return new ThubnailInfo { FilePath = resizedImage.FilePath, ImageSize = resizedImage.ImageSize };
                });
            }
        }
    }

    public async Task<string> CompressVideo(string inputPath, CancellationToken cancellationToken)
    {
        string personalPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        var fileName = Path.GetFileNameWithoutExtension(inputPath);
        string outputPath = Path.Combine(personalPath, $"{fileName}_compressed_video.mp4");
        if (File.Exists(outputPath))
        {
#if !iosShareExt
            //we already have this video compressed
            loggingService.Value.LogWarning($"VideoService: Skip compressing the video because app have it in the folder, inputPath:{inputPath}");
#endif
            return outputPath;
        }

        //inputPath = CopyVideo(inputPath);
        var task = new TaskCompletionSource<string>();

        NSUrl myFileUrl = NSUrl.FromFilename(inputPath);
        var export = new AVAssetExportSession(AVAsset.FromUrl(myFileUrl), AVAssetExportSession.PresetMediumQuality);

        export.OutputUrl = NSUrl.FromFilename(outputPath);
        export.OutputFileType = AVFileTypes.Mpeg4.GetConstant();
        export.ShouldOptimizeForNetworkUse = true;
        string result = string.Empty;
        //register cancelation
        using (cancellationToken.Register(export.CancelExport))
        {
            //start export
            export.ExportAsynchronously(() =>
            {
                if (export.Status == AVAssetExportSessionStatus.Completed)
                {
                    task.TrySetResult(outputPath);
                }
                else
                {
                    task.TrySetResult(string.Empty);
                }
            });

            result = await task.Task;
        }

        return result;
    }


    public string CopyVideo(string inputPath)
    {
        string personalPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        string extension = Path.GetExtension(inputPath);
        string copyInputPath = Path.Combine(personalPath, $"original_Video{extension}");
        using (FileStream fs = new FileStream(copyInputPath, FileMode.Create))
        using (var inputStream = File.OpenRead(inputPath))
        {
            inputStream.CopyTo(fs);
            inputStream.Flush();
            return copyInputPath;
        }
    }
}