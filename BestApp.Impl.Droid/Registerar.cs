using BestApp.Abstraction.General.Platform;
using BestApp.Abstraction.General.UI;
using BestApp.Impl.Droid.Platform;
using BestApp.Impl.Droid.UI;
using Common.Abstrtactions;
using DryIoc;

namespace BestApp.Impl.Droid
{
    public static class Registerar
    {
        public static void RegisterTypes(IContainer container)
        {
            container.Register<IPreferencesService, PreferencesService>(Reuse.Singleton);
            container.Register<IDirectoryService, DirectoryService>(Reuse.Singleton);
            container.Register<IPopupAlert, MockPopup>(Reuse.Singleton);
            container.Register<IPlatformErrorService, PlatformErrorService>(Reuse.Singleton);            
        }
    }
}
