using Base.Infrastructures.Abstractions.Repository;
using Base.Infrastructures.Abstractions.REST;
using DryIoc;

namespace BestApp.Impl.Cross.Infasructures
{
    internal class MyInfrastructureService : InfrastructureService
    {
        private readonly Lazy<ILocalDbInitilizer> localDbInitilizer;

        public MyInfrastructureService(IContainer container, Lazy<ILocalDbInitilizer> localDbInitilizer) : base(container)
        {
            this.localDbInitilizer = localDbInitilizer;
        }

        public override async Task Start()
        {
            await base.Start();
            await localDbInitilizer.Value.Init();            
        }

        public override Task Pause()
        {
            return base.Pause();
        }

        public override Task Resume()
        {
            return base.Resume();
        }

        public override async Task Stop()
        {
            await base.Stop();
            await localDbInitilizer.Value.Release();
        }
    }
}
