using System.Collections;
using System.ComponentModel;

namespace BestApp.MVVM.Navigation
{
    public class NavigationParameters : IEnumerable<KeyValuePair<string, object>>, INavigationParameters
    {
        private readonly List<KeyValuePair<string, object>> _entries = new List<KeyValuePair<string, object>>();

        public NavigationParameters()
        {

        }

        /// <summary>
        /// Searches Parameter collection and returns value if Collection contains key.
        /// Otherwise returns null.
        /// </summary>
        /// <param name="key">The key for the value to be returned.</param>
        /// <returns>The value of the parameter referenced by the key; otherwise <c>null</c>.</returns>
        public object this[string key]
        {
            get
            {
                foreach (var entry in _entries)
                {
                    if (string.Compare(entry.Key, key, StringComparison.Ordinal) == 0)
                    {
                        return entry.Value;
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// Constructs a list of parameters.
        /// </summary>
        /// <param name="query">Query string to be parsed.</param>
        public NavigationParameters(string query)
        {
            if (!string.IsNullOrWhiteSpace(query))
            {
                int num = query.Length;
                for (int i = query.Length > 0 && query[0] == '?' ? 1 : 0; i < num; i++)
                {
                    int startIndex = i;
                    int num4 = -1;
                    while (i < num)
                    {
                        char ch = query[i];
                        if (ch == '=')
                        {
                            if (num4 < 0)
                            {
                                num4 = i;
                            }
                        }
                        else if (ch == '&')
                        {
                            break;
                        }
                        i++;
                    }
                    string key = null;
                    string value;
                    if (num4 >= 0)
                    {
                        key = query.Substring(startIndex, num4 - startIndex);
                        value = query.Substring(num4 + 1, i - num4 - 1);
                    }
                    else
                    {
                        value = query.Substring(startIndex, i - startIndex);
                    }

                    if (key != null)
                        Add(Uri.UnescapeDataString(key), Uri.UnescapeDataString(value));
                }
            }
        }

        /// <summary>
        /// Adds the key and value to the parameters collection.
        /// </summary>
        /// <param name="key">The key to reference this value in the parameters collection.</param>
        /// <param name="value">The value of the parameter to store.</param>
        public void Add(string key, object value) =>
            _entries.Add(new KeyValuePair<string, object>(key, value));



        /// <summary>
        /// Gets an enumerator for the KeyValuePairs in parameter collection.
        /// </summary>
        /// <returns>Enumerator.</returns>
        public IEnumerator<KeyValuePair<string, object>> GetEnumerator() =>
            _entries.GetEnumerator();

        /// <summary>
        /// Returns the value of the member referenced by key.
        /// </summary>
        /// <typeparam name="T">The type of object to be returned.</typeparam>
        /// <param name="key">The key for the value to be returned.</param>
        /// <returns>Returns a matching parameter of <typeparamref name="T"/> if one exists in the Collection.</returns>
        public T GetValue<T>(string key) =>
            (T)GetValue(key, typeof(T));

        /// <summary>
        /// Searches <paramref name="parameters"/> for value referenced by <paramref name="key"/>
        /// </summary>
        /// <param name="parameters">A collection of parameters to search</param>
        /// <param name="key">The key of the parameter to find</param>
        /// <param name="type">The type of the parameter to return</param>
        /// <returns>A matching value of <paramref name="type"/> if it exists</returns>
        /// <exception cref="InvalidCastException">Unable to convert the value of Type</exception>        
        public object GetValue(string key, Type type)
        {
            foreach (var kvp in _entries)
            {
                if (string.Compare(kvp.Key, key, StringComparison.Ordinal) == 0)
                {
                    if (TryGetValueInternal(kvp, type, out var value))
                        return value;

                    throw new InvalidCastException($"Unable to convert the value of Type '{kvp.Value.GetType().FullName}' to '{type.FullName}' for the key '{key}' ");
                }
            }

            return GetDefault(type);
        }

        private bool TryGetValueInternal(KeyValuePair<string, object> kvp, Type type, out object value)
        {
            value = GetDefault(type);
            var success = false;
            if (kvp.Value == null)
            {
                success = true;
            }
            else if (kvp.Value.GetType() == type)
            {
                success = true;
                value = kvp.Value;
            }
            else if (type.IsAssignableFrom(kvp.Value.GetType()))
            {
                success = true;
                value = kvp.Value;
            }
            else if (type.IsEnum)
            {
                var valueAsString = kvp.Value.ToString();
                if (Enum.IsDefined(type, valueAsString))
                {
                    success = true;
                    value = Enum.Parse(type, valueAsString);
                }
                else if (int.TryParse(valueAsString, out var numericValue))
                {
                    success = true;
                    value = Enum.ToObject(type, numericValue);
                }
            }

            if (!success && type.GetInterface("System.IConvertible") != null)
            {
                success = true;
                value = Convert.ChangeType(kvp.Value, type);
            }

            return success;
        }


        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();


        // <summary>
        /// Checks to see if key exists in parameter collection
        /// </summary>
        /// <param name="parameters">IEnumerable to search</param>
        /// <param name="key">The key to search the <paramref name="parameters"/> for existence</param>
        /// <returns><c>true</c> if key exists; <c>false</c> otherwise</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ContainsKey(string key) =>
            _entries.Any(x => string.Compare(x.Key, key, StringComparison.Ordinal) == 0);

        private object GetDefault(Type type)
        {
            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }
            return null;
        }
    }
}
