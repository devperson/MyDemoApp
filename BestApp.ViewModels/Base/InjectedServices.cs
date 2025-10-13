using Base.Abstractions.Platform;
using Base.Abstractions.UI;
using Base.MVVM.Navigation;
using Base.MVVM.ViewModels;
using DryIoc;

namespace BestApp.ViewModels.Base
{
    public class PageInjectedServices : InjectedServices
    {        
        public PageInjectedServices(IPageNavigationService navigationService, IContainer container) : base(navigationService, container)
        {
            
        }        
        public IPopupAlert PopupAlertService=> Container.Resolve<IPopupAlert>();
        public IPlatformErrorService PlatformError => Container.Resolve<IPlatformErrorService>();
        public IDeviceThreadService DeviceThreadService => Container.Resolve<IDeviceThreadService>();
    }
}
