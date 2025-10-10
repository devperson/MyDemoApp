using BestApp.Abstraction.Main.PlatformServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.InfrasImpl.Impl
{
    internal class PreferenceService : IPreferencesService
    {
        public Dictionary<string, bool> boolData = new Dictionary<string, bool>();
        public Dictionary<string, string> strngData = new Dictionary<string, string>();
        public Dictionary<string, int> intData = new Dictionary<string, int>();
        public Dictionary<string, long> longData = new Dictionary<string, long>();

        public bool Get(string key, bool defaultValue)
        {
            if (boolData.ContainsKey(key))
                return boolData[key];

            return defaultValue;
        }

        public string Get(string key, string defaultValue)
        {
            if (strngData.ContainsKey(key))
                return strngData[key];

            return defaultValue;
        }

        public DateTime Get(string key, DateTime defaultValue)
        {
            if (strngData.ContainsKey(key))
                return DateTime.Parse(strngData[key]);

            return defaultValue;
        }

        public int Get(string key, int defaultValue)
        {
            if (intData.ContainsKey(key))
                return intData[key];

            return defaultValue;
        }

        public long Get(string key, long defaultValue)
        {
            if (longData.ContainsKey(key))
                return longData[key];

            return defaultValue;
        }

        public void Remove(string key)
        {
            if (intData.ContainsKey(key))
                intData.Remove(key);
            if (boolData.ContainsKey(key))
                boolData.Remove(key);
            if (strngData.ContainsKey(key))
                strngData.Remove(key);
        }

        public void Set(string key, bool value)
        {
            if (boolData.ContainsKey(key))
                boolData[key] = value;
            else
                boolData.Add(key, value);
        }

        public void Set(string key, string value)
        {
            if (strngData.ContainsKey(key))
                strngData[key] = value;
            else
                strngData.Add(key, value);
        }

        public void Set(string key, DateTime value)
        {
            strngData[key] = value.ToString();
        }

        public void Set(string key, int value)
        {
            if (intData.ContainsKey(key))
                intData[key] = value;
            else
                intData.Add(key, value);
        }

        public void Set(string key, long value)
        {
            if (intData.ContainsKey(key))
                longData[key] = value;
            else
                longData.Add(key, value);
        }
    }
}
