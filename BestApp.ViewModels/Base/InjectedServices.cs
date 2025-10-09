using BestApp.Abstraction.Common.Events;
using BestApp.Abstraction.Main.Platform;
using BestApp.Abstraction.Main.UI;
using BestApp.Abstraction.Main.UI.Navigation;
using Common.Abstrtactions;
using DryIoc;

namespace BestApp.ViewModels.Base
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
        public IPopupAlert PopupAlertService=> Container.Resolve<IPopupAlert>();
        public IPlatformErrorService PlatformError => Container.Resolve<IPlatformErrorService>();        
    }
}
