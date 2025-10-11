using DryIoc;

namespace BestApp.X.Droid
{
    internal static class ContainerLocator
    {
        public static IContainer Container { get; } = MainActivity.Instance.Container;

        public static T Resolve<T>() => Container.Resolve<T>();
    }
}
