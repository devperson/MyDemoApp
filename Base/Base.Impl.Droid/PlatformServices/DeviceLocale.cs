using Base.Abstractions.PlatformServices;
using Java.Util;

namespace Base.Impl.Droid.PlatformServices
{
    public class DeviceLocale : IDeviceLocale
    {
        public string GetDeviceCountryCode()
        {
            var regionCode = Locale.Default.Country;
            return regionCode;
        }
    }
}
