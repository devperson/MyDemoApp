using Base.Abstractions.Diagnostic;
using Base.Abstractions.Messaging;
using Base.Abstractions.PlatformServices;
using BestApp.Abstraction.Main.Infasructures.Events;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.ApplicationModel.Communication;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Networking;
using Newtonsoft.Json;
using System.Reflection;
using DeviceInfo = Base.Abstractions.PlatformServices.DeviceInfo;
using EssentialDeviceInfo = Microsoft.Maui.Devices.DeviceInfo;


namespace Base.Impl.PlatformServices;
public class DeviceInfoService : IDevice
{
    private string deviceId = null;    
    private readonly Lazy<IBadge> badge;
    private readonly Lazy<IKeybordService> keyboardService;
    private readonly Lazy<IPreferencesService> preferencesService;
    private readonly Lazy<ILoggingService> loggingService;
    private readonly Lazy<IDeviceLocale> deviceLocale;

    public event EventHandler NetworkChanged;

    public DeviceInfoService(Lazy<IMessagesCenter> eventAggregator,                             
                             Lazy<IBadge> badge,
                             Lazy<IKeybordService> keyboardService,
                             Lazy<IPreferencesService> preferencesService,
                             Lazy<ILoggingService> loggingService,
                             Lazy<IDeviceLocale> deviceLocale)
    {
        Connectivity.ConnectivityChanged += (s, e) =>
        {
            var hasConnection = e.NetworkAccess == NetworkAccess.Internet;
            eventAggregator.Value.GetEvent<ConnectionChangedEvent>().Publish(hasConnection);
            NetworkChanged?.Invoke(this, EventArgs.Empty);
        };        
        this.badge = badge;
        this.keyboardService = keyboardService;
        this.preferencesService = preferencesService;
        this.loggingService = loggingService;
        this.deviceLocale = deviceLocale;
    }


    public string GetDeviceId()
    {
        var deviceInfo = new DeviceInfo
        {
            Manufacturer = EssentialDeviceInfo.Manufacturer,
            VersionString = EssentialDeviceInfo.VersionString,
            Platform = EssentialDeviceInfo.Platform.ToString(),
            Version = EssentialDeviceInfo.Version.ToString(),
            Idiom = EssentialDeviceInfo.Idiom.ToString(),
            DeviceType = EssentialDeviceInfo.DeviceType.ToString(),
            UniqueId = DeviceUniqueId,
        };

        if (EssentialDeviceInfo.DeviceType == DeviceType.Virtual)
        {
            deviceInfo.Model = "Emulator";
        }
        else
        {
            //get device id from device info. Note it doesn't contain devcie name as this can be easily changed
            deviceInfo.Model = EssentialDeviceInfo.Model;
        }
        var jsonData = JsonConvert.SerializeObject(deviceInfo);
        return jsonData;
    }

    public void LongHaptic()
    {
        HapticFeedback.Perform(HapticFeedbackType.LongPress);
    }

    public void SetBadgeCount(int count)
    {
        badge.Value.SetBadgeCount(count);
    }


    public async Task<bool> SendEmail(string to)
    {
        try
        {
            await Email.ComposeAsync(new EmailMessage()
            {
                To = new List<string>() { to }
            });
            return true;
        }
        catch (Exception ex)
        {
            loggingService.Value.TrackError(ex);
            return false;
        }
    }

    public async Task<bool> SendEmail(string to, string subject, string body)
    {
        try
        {
            await Email.ComposeAsync(new EmailMessage(subject, body, [to]));
            return true;
        }
        catch (Exception ex)
        {
            loggingService.Value.TrackError(ex);
            return false;
        }
    }

    public bool IsFirstLaunch()
    {
        return VersionTracking.IsFirstLaunchForCurrentVersion;
    }

    public Task OpenInBrowser(string url)
    {
        return Browser.OpenAsync(url);
    }

//    public async void PlaySound(SoundType soundType)
//    {
//        if (AssemblyCache == null)
//            AssemblyCache = typeof(KYChat.Shared.Implementation.Extensions).GetTypeInfo().Assembly;

//        if (soundType == SoundType.MessageSent)
//        {
//            string realResource = GetRealResource("iphone_send_sms.mp3");
//            using (var audioStream = AssemblyCache.GetManifestResourceStream(realResource))
//            {
//                //TODO: android need to use player without dispose because current plugin
//                //calls GC collector to finilize every time when audio ends which force somehow to reconnect mesibo connection
//                if (DeviceInfo.Platform == DeviceInfo.iOS)
//                {
//#if IOS_NET
//                        var messageSentPlayer = new KYChat.NET.iOS.Implementation.Platform.SimpleAudioPlayerImplementation1();
//#else
//                    var messageSentPlayer = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();
//#endif
//                    messageSentPlayer.Load(audioStream);
//                    await Task.Delay(300);
//                    messageSentPlayer.Play();
//                }
//                else
//                {
//                    CrossSimpleAudioPlayer.Current.Load(audioStream);
//                    CrossSimpleAudioPlayer.Current.Play();
//                }

//            }
//        }
//        if (soundType == SoundType.MessageReceived)
//        {
//            string realResource = GetRealResource("iphone_text.mp3");
//            using (var audioStream = AssemblyCache.GetManifestResourceStream(realResource))
//            {
//                if (DeviceInfo.Platform == DeviceInfo.iOS)
//                {
//#if IOS_NET
//                        var messageRecievedPlayer = new KYChat.NET.iOS.Implementation.Platform.SimpleAudioPlayerImplementation1();
//#else
//                    var messageRecievedPlayer = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();
//#endif
//                    messageRecievedPlayer.Load(audioStream);
//                    await Task.Delay(300);
//                    messageRecievedPlayer.Play();
//                }
//                else
//                {
//                    CrossSimpleAudioPlayer.Current.Load(audioStream);
//                    CrossSimpleAudioPlayer.Current.Play();
//                }
//            }
//        }
//    }

    DeviceInfo _deviceInfo;
    public DeviceInfo DeviceInfo
    {
        get
        {
            if (_deviceInfo == null)
            {
                _deviceInfo = new DeviceInfo
                {
                    Model = EssentialDeviceInfo.Model,
                    Manufacturer = EssentialDeviceInfo.Manufacturer,
                    Name = EssentialDeviceInfo.Name,
                    VersionString = EssentialDeviceInfo.VersionString,
                    Platform = EssentialDeviceInfo.Platform.ToString(),
                    Version = EssentialDeviceInfo.Version.ToString(),
                    Idiom = EssentialDeviceInfo.Idiom.ToString(),
                    DeviceType = EssentialDeviceInfo.DeviceType.ToString()
                };

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    _deviceInfo.DisplayWidth = DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density;
                    _deviceInfo.DisplayHeight = DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density;
                });
            }

            return _deviceInfo;
        }
    }



    public bool KeepScreenOn
    {
        get => DeviceDisplay.KeepScreenOn;
        set => DeviceDisplay.KeepScreenOn = value;
    }

    public bool HasInternetConnection
    {
        get
        {
            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.Internet || current == NetworkAccess.ConstrainedInternet)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public static Assembly AssemblyCache = null;
    private static string GetRealResource(string resource)
    {
        return AssemblyCache.GetManifestResourceNames().FirstOrDefault((string x) => x.EndsWith(resource, StringComparison.CurrentCultureIgnoreCase));
    }

    //public void HideChatEditorKeyboard()
    //{
    //    keyboardService.Value.HideChatEditorKeyboard();
    //}

    public string GetDeviceCountryCode()
    {
        return deviceLocale.Value.GetDeviceCountryCode();
    }

    /// <summary>
    /// Currently it is not possible to get device id that will always identify this device. 
    /// So create temp id that identifies this device while user activly uses the app.
    /// Note this id will change if user will delete the app and install it again
    /// </summary>
    private string uniqueDeviceIdKey = "uniqueDeviceId";
    public string DeviceUniqueId
    {
        get
        {
            var id = preferencesService.Value.Get(uniqueDeviceIdKey, string.Empty);
            if (string.IsNullOrEmpty(id))
            {
                id = Guid.NewGuid().ToString();
                preferencesService.Value.Set(uniqueDeviceIdKey, id);
            }

            return id;
        }
    }
}
