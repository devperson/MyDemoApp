using Base.Abstractions.Diagnostic;
using Base.Aspect;
using Com.Otaliastudios.Transcoder;
using Com.Otaliastudios.Transcoder.Strategy;
using Com.Otaliastudios.Transcoder.Strategy.Size;
using Java.Lang;
using Java.Util.Concurrent;

namespace BestApp.Impl.Droid.PlatformServices.Video;

[LogMethods]
public class VideoTranscoder : Java.Lang.Object, ITranscoderListener
{
    private string _assetPath;
    private readonly ILoggingService logger;
    private IFuture runningOperation = null;
    private string TAG = "VideoTranscoder: ";

    public VideoTranscoder(ILoggingService logger)
    {
        this.logger = logger;
    }

    public string OutputPath { get; set; }

    public async Task TranscodeVideoAsync(string sourceVideoPath, string outputFilePath)
    {
        _assetPath = sourceVideoPath;
        OutputPath = outputFilePath;
        var length = new FileInfo(sourceVideoPath).Length;
        logger.Log($"{TAG}Start  Original file size: " + length);

        await ExportTaskAsync();

        var length2 = new FileInfo(outputFilePath).Length;
        logger.Log($"{TAG}Compressed file size: " + length2);
    }

    public void CancelCurrentTranscoding()
    {
        runningOperation.Cancel(true);
    }

    TaskCompletionSource<bool> completionSource;
    private Task ExportTaskAsync()
    {
        try
        {
            completionSource = new TaskCompletionSource<bool>();
            var videoStrategy = new DefaultVideoStrategy.Builder()
                   .AddResizer(new ExactResizer(720, 1280))
                   .BitRate(4096)
                   .FrameRate(40)
                   .KeyFrameInterval(3f)
                   .Build();

            DefaultAudioStrategy audioStrategy = new DefaultAudioStrategy.Builder().Build();
            //.SampleRate(48000)//44100)
            //.BitRate(96)
            //.Build();

            runningOperation = Transcoder.Into(this.OutputPath)
                .AddDataSource(_assetPath)
                .SetVideoTrackStrategy(DefaultVideoStrategies.For720x1280())
                .SetAudioTrackStrategy(audioStrategy)
                .SetListener(this)
                .Transcode();
        }
        catch (System.Exception ex)
        {
            logger.TrackError(ex);
        }

        return completionSource.Task;
    }


    #region ITranscoderListener Methods

    public void OnTranscodeCanceled()
    {
        logger.LogWarning($"{TAG}Video transcoding canceled");
        completionSource.TrySetResult(false);
    }

    public void OnTranscodeCompleted(int successCode)
    {
        logger.LogWarning($"{TAG}Video transcoding completed");
        completionSource.TrySetResult(true);
    }

    public void OnTranscodeFailed(Throwable exception)
    {
        logger.LogWarning($"{TAG}Video transcoding failed: {exception}");
        completionSource.TrySetResult(false);
    }

    public void OnTranscodeProgress(double progress)
    {
        logger.Log($"{TAG}Transcoding progress: {progress}");
    }

    #endregion

}