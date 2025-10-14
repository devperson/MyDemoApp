using Base.Abstractions.PlatformServices;

namespace Base.Impl.iOS.PlatformServices
{
    public class DeviceLocale : IDeviceLocale
    {
        public string GetDeviceCountryCode()
        {
            return NSLocale.CurrentLocale.RegionCode;
        }
    }
}
