using Base.Abstractions.PlatformServices;
using Base.Impl;
using DryIoc;

namespace BestApp.Impl.Cross.Infasructures
{
    internal class MyErrorTrackingService : SentryErrorTrackingService
    {
        public MyErrorTrackingService(Lazy<IContainer> container, Lazy<IDirectoryService> directoryService) : base(container, directoryService)
        {
        }

        protected override void OnInit(SentryOptions options)
        {
            options.Dsn = "https://e2232a39fbb65cd0fd4beba2b0954c87@o4507288977080320.ingest.de.sentry.io/4510120095842384";
            options.Debug = false;
            options.TracesSampleRate = 0.1;
            options.AutoSessionTracking = true;
            options.Environment = "internal";
        }
    }
}
