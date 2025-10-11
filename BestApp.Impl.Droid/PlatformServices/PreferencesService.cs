using Base.Abstractions.Platform;
using Microsoft.Maui.Storage;

namespace BestApp.Impl.Droid.PlatformServices
{
    public class PreferencesService : IPreferencesService
    {
        public bool Get(string key, bool defaultValue)
        {
            if (!Preferences.ContainsKey(key))
                return defaultValue;

            return Preferences.Get(key, defaultValue);
        }

        public string Get(string key, string defaultValue)
        {
            return Preferences.Get(key, defaultValue);
        }

        public DateTime Get(string key, DateTime defaultValue)
        {
            return Preferences.Get(key, defaultValue);
        }

        public int Get(string key, int defaultValue)
        {
            if (!Preferences.ContainsKey(key))
                return defaultValue;

            return Preferences.Get(key, defaultValue);
        }

        public void Set(string key, int value)
        {
            Preferences.Set(key, value);
        }

        public long Get(string key, long defaultValue)
        {
            if (!Preferences.ContainsKey(key))
                return defaultValue;

            return Preferences.Get(key, defaultValue);
        }

        public void Set(string key, long value)
        {
            Preferences.Set(key, value);
        }

        public void Set(string key, bool value)
        {
            Preferences.Set(key, value);
        }

        public void Set(string key, string value)
        {
            Preferences.Set(key, value);
        }

        public void Set(string key, DateTime value)
        {
            Preferences.Set(key, value);
        }

        public void Remove(string key)
        {
            Preferences.Remove(key);
        }
    }
}
