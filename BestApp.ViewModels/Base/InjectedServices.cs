using BestApp.Abstraction.Main.PlatformServices;
using BestApp.Abstraction.Main.UI;
using BestApp.MVVM.Navigation;
using BestApp.MVVM.ViewModels;
using DryIoc;

namespace BestApp.ViewModels.Base
{
    public class InjectedServices : PageInjectedServices
    {        
        public InjectedServices(IPageNavigationService navigationService, IContainer container) : base(navigationService, container)
        {
            
        }        
        public IPopupAlert PopupAlertService=> Container.Resolve<IPopupAlert>();
        public IPlatformErrorService PlatformError => Container.Resolve<IPlatformErrorService>();
        public IDeviceThreadService DeviceThreadService => Container.Resolve<IDeviceThreadService>();
    }
}
