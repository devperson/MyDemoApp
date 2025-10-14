using Base.Abstractions.Diagnostic;
using Base.Abstractions.PlatformServices;
using DryIoc;

namespace Base.Impl
{
    public class SentryErrorTrackingService : IErrorTrackingService
    {
        public event EventHandler<Exception> OnServiceError;
        private int logFileIndex = 0;
        private string logDirectoryForUnhandled;
        private readonly Lazy<IContainer> container;
        private readonly Lazy<IDirectoryService> directoryService;

        public SentryErrorTrackingService(Lazy<IContainer> container, Lazy<IDirectoryService> directoryService)
        {
            this.container = container;
            this.directoryService = directoryService;

            InitializeSentry();            
        }



        public void TrackError(Exception ex, byte[] attachment = null, Dictionary<string, string> addionalData = null)
        {
            try
            {
                if (attachment != null)
                {
                    SentrySdk.CaptureException(ex, scope =>
                    {
                        scope.AddAttachment(attachment, "logs.zip", AttachmentType.Default, "application/x-zip-compressed");
                    });
                }
                else
                {
                    SentrySdk.CaptureException(ex);
                }
            }
            catch (Exception error)
            {
                OnServiceError?.Invoke(this, error);
            }
        }


        private void InitializeSentry()
        {
            SentrySdk.Init(options =>
            {
                OnInit(options);

                //options.Debug = true; // Enable Sentry internal logging
                //options.DiagnosticLevel = SentryLevel.Debug;

                options.SetBeforeSend((ev, hint) =>
                {
                    //if (ev.Exception is AggregateException aggEx)
                    //{
                    //    if (aggEx.InnerExceptions.Any(ex =>
                    //    {
                    //        var fullName = ex.GetType().FullName?.ToLower() ?? "";
                    //        if (fullName.Contains("illegalstateexception") || fullName.Contains("runtimeexception"))
                    //        {
                    //            return false;
                    //        }
                    //        return fullName is "java.lang.illegalstateexception" or "java.lang.runtimeexception";
                    //    }))
                    //    {
                    //        // Ignore the event
                    //        return null;
                    //    }
                    //}
                    //else
                    //{
                    //    var fullName = ev.Exception?.GetType().FullName?.ToLower() ?? "";
                    //    if (fullName.Contains("illegalstateexception") || fullName.Contains("runtimeexception"))
                    //    {
                    //        return null;
                    //    }
                    //}

                    //BeforeSend is fired for every event (crash, handled exception, log, or message)
                    //Unhandled crashes typically do not contain attachments.
                    //also this way all exception (tracked and unhandled) will have app log attached 
                    if (hint.Attachments.Count == 0)
                    {
                        //WARNING this service intended to be used in ILoggingService thus do not add ILoggingService in constructor to not have references to each other
                        var logger = container.Value.Resolve<ILoggingService>();
                        if (logger != null)
                        {
                            logger.LogUnhandledError(ev.Exception);//log error into app logs     
                            Task.Delay(300).Wait();//give time for logger

                            var newLogPath = CreatePathForAttachment();
                            var logBytes = logger.GetLastSessionLogBytes();
                            if (logBytes != null)
                            {
                                File.WriteAllBytes(newLogPath, logBytes);
                                hint.AddAttachment(newLogPath, AttachmentType.Default, "application/x-zip-compressed");
                            }
                        }
                    }

                    return ev;
                });
            });
        }

        protected virtual void OnInit(SentryOptions options)
        {
            
        }

        private string CreatePathForAttachment()
        {
            if (logDirectoryForUnhandled == null)
            {
                logDirectoryForUnhandled = Path.Combine(directoryService.Value.GetCacheDir(), $"sentryTempFolder");
                if (!Directory.Exists(logDirectoryForUnhandled))
                {
                    Directory.CreateDirectory(logDirectoryForUnhandled);
                }
            }

            while (true)
            {
                try
                {
                    var newPath = Path.Combine(logDirectoryForUnhandled, $"applog{logFileIndex}.zip");
                    if (File.Exists(newPath))
                        File.Delete(newPath);
                    return newPath;
                }
                catch
                {
                    logFileIndex++;
                }
            }
        }
    }
}
