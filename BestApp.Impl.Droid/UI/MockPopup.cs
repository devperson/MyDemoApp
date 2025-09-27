using BestApp.Abstraction.General.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestApp.Impl.Droid.UI
{
    internal class MockPopup : IPopupAlert
    {
        public Task ShowError(string message)
        {
            return Task.CompletedTask;
        }

        public Task ShowInfo(string message)
        {
            return Task.CompletedTask;
        }

        public Task ShowSuccess(string message)
        {
            return Task.CompletedTask;
        }
    }
}
