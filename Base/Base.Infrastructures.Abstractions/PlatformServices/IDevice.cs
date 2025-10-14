using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Abstractions.PlatformServices;
public interface IDevice
{
    event EventHandler NetworkChanged;
    DeviceInfo DeviceInfo { get; }

    string GetDeviceId();
    string GetDeviceCountryCode();

    bool KeepScreenOn { get; set; }

    bool HasInternetConnection { get; }

    //void LongHaptic();

    //void SetBadgeCount(int count);

    //void PlaySound(SoundType soundType);

    //void HideChatEditorKeyboard();
    bool IsFirstLaunch();
    Task OpenInBrowser(string url);
    Task<bool> SendEmail(string to);
    Task<bool> SendEmail(string to, string subject, string body);
}

public class DeviceInfo
{
    public string Model { get; set; }
    public string Manufacturer { get; set; }
    public string Name { get; set; }
    public string VersionString { get; set; }
    public string Platform { get; set; }
    public string Version { get; set; }
    public string Idiom { get; set; }
    public string DeviceType { get; set; }
    public string UniqueId { get; set; }
    public double DisplayWidth { get; set; }
    public double DisplayHeight { get; set; }

    public const string iOS = "iOS";
    public const string Android = "Android";

}

public interface IBadge
{
    void SetBadgeCount(int count);
}

public interface IKeybordService
{
    void HideChatEditorKeyboard();
    void HideKeyboard();
}

public interface IDeviceLocale
{
    string GetDeviceCountryCode();
}

public enum SoundType
{
    MessageSent,
    MessageReceived,
}
