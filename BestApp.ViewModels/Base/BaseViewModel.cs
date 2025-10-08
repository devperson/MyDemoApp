using BestApp.Abstraction.Main.UI.Navigation;
using IDestructible = BestApp.Abstraction.Main.UI.Navigation.IDestructible;
using INavigationParameters = BestApp.Abstraction.Main.UI.Navigation.INavigationParameters;

namespace BestApp.ViewModels.Base
{
    public class BaseViewModel : Bindable, IDestructible, IInitialize
    {
        public event EventHandler Destroyed;
        public event EventHandler Initialized;
        protected bool isDestroyed;

        public BaseViewModel()
        {
        }

        public virtual void Destroy()
        {
            isDestroyed = true;
            Destroyed?.Invoke(this, EventArgs.Empty);
        }

        public virtual void Initialize(INavigationParameters parameters)
        {
            Initialized?.Invoke(this, EventArgs.Empty);
        }




    }
}
