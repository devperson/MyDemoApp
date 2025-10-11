using Base.Infrastructures.Abstractions.Repository;
using Base.Infrastructures.Abstractions.REST;

namespace BestApp.Impl.Cross.Infasructures
{
    internal class MyInfrastructureService : InfrastructureService
    {
        public MyInfrastructureService(Lazy<ILocalDbInitilizer> localDbInitilizer, Lazy<IRestQueueService> restQueueService) : base(localDbInitilizer, restQueueService)
        {
        }

        public override Task Start()
        {
            return base.Start();
        }

        public override Task Pause()
        {
            return base.Pause();
        }

        public override Task Resume()
        {
            return base.Resume();
        }

        public override Task Stop()
        {
            return base.Stop();
        }
    }
}
