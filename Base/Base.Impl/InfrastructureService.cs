using Base.Aspect;
using Base.Infrastructures.Abstractions.Repository;
using Base.Infrastructures.Abstractions.REST;
using BestApp.Abstraction.Main.Infasructures;
using BestApp.Impl.Cross.Infasructures.REST;
using DryIoc;

namespace BestApp.Impl.Cross.Infasructures
{
    /// <summary>
    /// Provides centralized control over the app’s core infrastructure lifecycle.
    ///
    /// This service coordinates initialization, suspension, resumption, and cleanup of 
    /// background systems such as local database initialization and queued REST operations.
    /// It acts as a bridge between the application’s lifecycle events (startup, backgrounding,
    /// resume, and shutdown) and the lower-level infrastructure components.
    ///
    /// <para>
    /// Usage:
    /// <list type="bullet">
    ///   <item>
    ///     <description><see cref="Start"/> — Call when the main application starts to initialize the local database and essential services.</description>
    ///   </item>
    ///   <item>
    ///     <description><see cref="Pause"/> — Call when the app transitions to background to pause network queue processing.</description>
    ///   </item>
    ///   <item>
    ///     <description><see cref="Resume"/> — Call when the app returns to the foreground to resume network operations.</description>
    ///   </item>
    ///   <item>
    ///     <description><see cref="Stop"/> — Call when the app stops or the user logs out to gracefully release resources.</description>
    ///   </item>
    /// </list>
    /// </para>
    ///
    /// Dependencies:
    /// <list type="bullet">
    ///   <item><description><see cref="ILocalDbInitilizer"/> — Handles setup and teardown of the local database.</description></item>
    ///   <item><description><see cref="IRestQueueService"/> — Manages queued REST requests, allowing pausing and resuming when app state changes.</description></item>
    /// </list>
    /// </summary>
    [LogMethods]
    public class InfrastructureService : IInfrastructureServices
    {        
        private readonly Lazy<RequestQueueList> restQueueService;

        public InfrastructureService(IContainer container)
        {           
            this.restQueueService = new Lazy<RequestQueueList>(() => container.Resolve<RequestQueueList>()); 
        }

        /// <summary>
        /// Initializes infrastructure components when the application starts.
        /// </summary>
        public virtual Task Start()
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Pauses background infrastructure (e.g., queued REST requests) when the app goes to background.
        /// </summary>
        public virtual Task Pause()
        {
            restQueueService.Value.Pause();
            return Task.CompletedTask;
        }

        /// <summary>
        /// Resumes paused background infrastructure when the app returns to the foreground.
        /// </summary>
        public virtual Task Resume()
        {
            restQueueService.Value.Resume();
            return Task.CompletedTask;
        }

        /// <summary>
        /// Gracefully stops and releases resources when the app shuts down or the user logs out.
        /// </summary>
        public virtual Task Stop()
        {            
            restQueueService.Value.Release();

            return Task.CompletedTask;
        }
    }
}
