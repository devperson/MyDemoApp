using DryIoc;

namespace BestApp.X.iOS;

internal static class ContainerLocator
{
    public static IContainer Container { get; } = AppDelegate.Instance.Container;

    public static T Resolve<T>() => Container.Resolve<T>();
}
