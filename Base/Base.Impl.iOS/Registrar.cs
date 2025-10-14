using Base.Abstractions.PlatformServices;
using Base.Abstractions.UI;
using Base.Impl.iOS.UI;
using DryIoc;
using Base.Impl.PlatformServices;
using Base.Impl.iOS.PlatformServices;

namespace Base.Impl.iOS
{
    public static class Registrar
    {
        public static IContainer appContainer;
        public static void RegisterTypes(IContainer container)
        {
            appContainer = container;

            container.Register<IDirectoryService, DirectoryService>(Reuse.Singleton);            
            container.Register<IPlatformErrorService, PlatformErrorService>(Reuse.Singleton);
        }
    }
}
