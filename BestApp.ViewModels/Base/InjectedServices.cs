using BestApp.Abstraction.General.Platform;
using BestApp.Abstraction.General.UI;
using Common.Abstrtactions;
using DryIoc;

namespace BestApp.ViewModels.Base
{
    public class InjectedServices
    {
        private Lazy<IContainer> container;
        public InjectedServices(IPageNavigationService navigationService, IContainer container)
        {
            NavigationService = navigationService;
            Container = container;
        }
        public IContainer Container { get; }
        public IPageNavigationService NavigationService { get; }

        public IEventAggregator EventAggregator => Container.Resolve<IEventAggregator>();
        public ILoggingService LoggingService => Container.Resolve<ILoggingService>();
        public IPopupAlert PopupAlertService=> Container.Resolve<IPopupAlert>();
        public IPlatformErrorService PlatformError => Container.Resolve<IPlatformErrorService>();
    }
}
