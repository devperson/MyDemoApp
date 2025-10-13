using Base.MVVM.Navigation;
using DryIoc;
using Base.Abstractions.Diagnostic;
using Base.Abstractions.Messaging;

namespace Base.MVVM.ViewModels
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
