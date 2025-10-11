using Base.MVVM.Navigation;
using IDestructible = Base.MVVM.Navigation.IDestructible;
using INavigationParameters = Base.MVVM.Navigation.INavigationParameters;

namespace Base.MVVM.ViewModels
{
    public abstract class BaseViewModel : Bindable, IDestructible, IInitialize
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
