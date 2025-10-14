using Base.Abstractions.PlatformServices;
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
        
        public IDeviceThreadService DeviceThreadService => Container.Resolve<IDeviceThreadService>();
    }
}
