using BestApp.Abstraction.Main.Platform;
using BestApp.Abstraction.Main.UI;
using BestApp.Impl.iOS.Platform;
using BestApp.Impl.iOS.UI;
using BestApp.Impl.Platform;
using DryIoc;

namespace BestApp.Impl.iOS
{
    public static class Registerar
    {
        public static void RegisterTypes(IContainer container)
        {            
            container.Register<IDirectoryService, DirectoryService>(Reuse.Singleton);
            container.Register<IPopupAlert, MockPopup>(Reuse.Singleton);
            container.Register<IPlatformErrorService, PlatformErrorService>(Reuse.Singleton);
        }
    }
}
