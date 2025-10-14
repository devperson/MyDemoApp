using Base.Abstractions.PlatformServices;
using Microsoft.Maui.ApplicationModel;
using System;

namespace Base.Impl.PlatformServices;

public class DeviceThreadService : IDeviceThreadService
{
    public void BeginInvokeOnMainThread(Action action)
    {
        MainThread.BeginInvokeOnMainThread(action);
    }
}
