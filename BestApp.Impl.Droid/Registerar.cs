using BestApp.Abstraction.General.UI;
using BestApp.Impl.Droid.UI;
using Common.Abstrtactions;
using DryIoc;

namespace BestApp.Impl.Droid
{
    public static class Registerar
    {
        public static void RegisterTypes(IContainer container)
        {
            container.Register<ILoggingService, AppLogging>(Reuse.Singleton);
            container.Register<IPopupAlert, MockPopup>(Reuse.Singleton);
        }
    }
}
