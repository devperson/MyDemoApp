using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestApp.Abstraction.General.UI.Navigation
{
    public interface INavigationParameters : IEnumerable<KeyValuePair<string, object>>
    {
        /// <summary>
        /// Determines whether the <see cref="IParameters"/> contains the specified <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key to search the parameters for existence.</param>
        /// <returns>true if the <see cref="IParameters"/> contains a parameter with the specified key; otherwise, false.</returns>
        bool ContainsKey(string key);

        /// <summary>
        /// Gets the parameter associated with the specified <paramref name="key"/>.
        /// </summary>
        /// <typeparam name="T">The type of the parameter to get.</typeparam>
        /// <param name="key">The key of the parameter to find.</param>
        /// <returns>A matching value of <typeparamref name="T"/> if it exists.</returns>
        T GetValue<T>(string key);
    }
}
