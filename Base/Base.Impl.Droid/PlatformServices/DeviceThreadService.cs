using Base.Abstractions.PlatformServices;
using Microsoft.Maui.ApplicationModel;

namespace Base.Impl.Droid.PlatformServices;

public class DeviceThreadService : IDeviceThreadService
{
    public void BeginInvokeOnMainThread(Action action)
    {
        MainThread.BeginInvokeOnMainThread(action);
    }
}
