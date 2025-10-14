using Base.Abstractions.PlatformServices;
using Base.Abstractions.UI;
using Base.Impl.iOS.Platform;
using Base.Impl.iOS.UI;
using DryIoc;
using Base.Impl.PlatformServices;

namespace Base.Impl.iOS
{
    public static class Registrar
    {
        public static void RegisterTypes(IContainer container)
        {            
            container.Register<IDirectoryService, DirectoryService>(Reuse.Singleton);
            container.Register<IPopupAlert, MockPopup>(Reuse.Singleton);
            container.Register<IPlatformErrorService, PlatformErrorService>(Reuse.Singleton);
        }
    }
}
