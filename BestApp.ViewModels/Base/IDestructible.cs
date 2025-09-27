using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestApp.ViewModels.Base
{
    /// <summary>
    /// Interface for objects that require cleanup of resources prior to Disposal
    /// </summary>
    public interface IDestructible
    {
        /// <summary>
        /// This method allows cleanup of any resources used by your View/ViewModel 
        /// </summary>
        void Destroy();
    }
}
