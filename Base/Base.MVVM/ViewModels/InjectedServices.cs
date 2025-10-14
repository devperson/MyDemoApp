using Base.MVVM.Navigation;
using DryIoc;
using Base.Abstractions.Diagnostic;
using Base.Abstractions.Messaging;
using Base.Abstractions.PlatformServices;
using Base.Abstractions.UI;

namespace Base.MVVM.ViewModels
{
    public class InjectedServices
    {        
        public InjectedServices(IPageNavigationService navigationService, IContainer container)
        {
            NavigationService = navigationService;
            Container = container;
        }
        public IContainer Container { get; }
        public IPageNavigationService NavigationService { get; }

        public IMessagesCenter EventAggregator => Container.Resolve<IMessagesCenter>();
        public ILoggingService LoggingService => Container.Resolve<ILoggingService>();
        public ISnackbarService SnackBarService => Container.Resolve<ISnackbarService>();
        public IPlatformErrorService PlatformError => Container.Resolve<IPlatformErrorService>();
    }
}
