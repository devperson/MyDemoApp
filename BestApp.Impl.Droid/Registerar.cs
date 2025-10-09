using BestApp.Abstraction.Main.Platform;
using BestApp.Abstraction.Main.UI;
using BestApp.Impl.Droid.Platform;
using BestApp.Impl.Droid.UI;
using BestApp.Impl.Platform;
using Common.Abstrtactions;
using DryIoc;
using Mapster;

namespace BestApp.Impl.Droid
{
    public static class Registerar
    {
        public static void RegisterTypes(IContainer container, TypeAdapterConfig mapperConfig)
        {
            container.Register<IPreferencesService, PreferencesService>(Reuse.Singleton);
            container.Register<IDirectoryService, DirectoryService>(Reuse.Singleton);
            container.Register<IPopupAlert, MockPopup>(Reuse.Singleton);
            container.Register<IPlatformErrorService, PlatformErrorService>(Reuse.Singleton);            
        }
    }
}
