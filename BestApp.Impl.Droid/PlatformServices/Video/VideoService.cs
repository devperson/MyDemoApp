using Android.Content;
using Android.Media;
using Base.Abstractions.Platform;
using BestApp.Abstraction.Main.PlatformServices;
using Common.Abstrtactions;
using Microsoft.Maui.ApplicationModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestApp.Impl.Droid.PlatformServices.Video;

public class VideoService : IVideoService
{
    private readonly Lazy<ResizeImageService> imageResizeService;
    private readonly ILoggingService loggingService;
    VideoTranscoder transcoder;

    public VideoService(Lazy<ResizeImageService> imageResizeService, ILoggingService loggingService)
    {
        this.imageResizeService = imageResizeService;
        this.loggingService = loggingService;
        transcoder = new VideoTranscoder(loggingService);
    }

    public Task<ThubnailInfo> GetThumbnail(string videoFilePath)
    {
        return Task.Run(() =>
        {
            MediaMetadataRetriever retriever = new MediaMetadataRetriever();
            retriever.SetDataSource(videoFilePath);
            var bitmap = retriever.GetFrameAtTime(1);
            if (bitmap != null)
            {
                //get create resized image with unique name so next thumbnail will not override the current one
                var resizeResult = imageResizeService.Value.ResizeNativeImage(bitmap, null, 500, 500, 0, 97f, shouldSetUniqueName: true);
                return new ThubnailInfo() { FilePath = resizeResult.FilePath, ImageSize = resizeResult.ImageSize };
            }
            else
            {
                return new ThubnailInfo();
            }
        });
    }

    SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
    public async Task<string> CompressVideo(string inputPath, CancellationToken cancellationToken)
    {
        //use semaphor to prevent to run multiple call. Do not allow making concurrent multiple transcoding
        await semaphore.WaitAsync();

        string outputPath = string.Empty;
        try
        {
            string personalPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var fileName = Path.GetFileNameWithoutExtension(inputPath);
            outputPath = Path.Combine(personalPath, $"{fileName}_compressed_video.mp4");

            if (File.Exists(outputPath))
            {
                //we already have this video compressed
                loggingService.LogWarning($"VideoService: Skip compressing the video because app have it in the folder, inputPath:{inputPath}");
                return outputPath;
            }

            //subscribe to cancelation
            using (cancellationToken.Register(transcoder.CancelCurrentTranscoding))
            {
                //start trasncoding
                await transcoder.TranscodeVideoAsync(inputPath, outputPath);
            }

            if (cancellationToken.IsCancellationRequested)
                throw new TaskCanceledException();
        }
        finally
        {
            SecureRelease(semaphore);
        }

        return outputPath;
    }

    private void SecureRelease(SemaphoreSlim semaphor)
    {
        try
        {
            semaphor.Release();
        }
        catch (Exception ex)
        {
            loggingService.TrackError(ex);
        }
    }


    public static async void PlayVideoInExternalPlayer(Context context, string urlString)
    {
        var url = Android.Net.Uri.Parse(urlString);
        var intent = new Intent(Intent.ActionView, url);
        intent.SetDataAndType(url, "video/*");
        var chooserIntent = Intent.CreateChooser(intent, "Open with...");

        if (chooserIntent.ResolveActivity(context.PackageManager) != null)
        {
            context.StartActivity(chooserIntent);
        }
        else
        {
            await Browser.OpenAsync(urlString, BrowserLaunchMode.SystemPreferred);
        }
    }
}
