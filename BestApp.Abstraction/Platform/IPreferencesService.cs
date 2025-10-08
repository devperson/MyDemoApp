using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestApp.Abstraction.Main.Platform
{
    public interface IPreferencesService
    {
        bool Get(string key, bool defaultValue);
        void Set(string key, bool value);

        string Get(string key, string defaultValue);
        void Set(string key, string value);

        DateTime Get(string key, DateTime defaultValue);
        void Set(string key, DateTime value);

        int Get(string key, int defaultValue);
        void Set(string key, int value);

        long Get(string key, long defaultValue);
        void Set(string key, long value);

        void Remove(string key);
    }
}
