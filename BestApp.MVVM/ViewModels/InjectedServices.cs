using BestApp.Abstraction.Common.Events;
using BestApp.MVVM.Navigation;
using Common.Abstrtactions;
using DryIoc;

namespace BestApp.MVVM.ViewModels
{
    public class PageInjectedServices
    {        
        public PageInjectedServices(IPageNavigationService navigationService, IContainer container)
        {
            NavigationService = navigationService;
            Container = container;
        }
        public IContainer Container { get; }
        public IPageNavigationService NavigationService { get; }

        public IMessagesCenter EventAggregator => Container.Resolve<IMessagesCenter>();
        public ILoggingService LoggingService => Container.Resolve<ILoggingService>();
    }
}
