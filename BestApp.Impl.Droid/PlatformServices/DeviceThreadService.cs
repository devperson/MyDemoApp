using BestApp.Abstraction.Main.PlatformServices;
using Microsoft.Maui.ApplicationModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestApp.Impl.Droid.PlatformServices;

public class DeviceThreadService : IDeviceThreadService
{
    public void BeginInvokeOnMainThread(Action action)
    {
        MainThread.BeginInvokeOnMainThread(action);
    }
}
