using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.MVVM.Helper
{
    public class ClickUtil
    {
        public const int OneClickDelay = 1000;
        private bool isInCall = false;
        private object syncLock = new object();
        public async Task ExecuteOnlyOnceAsync(Func<Task> inFunction)
        {
            lock (syncLock)
            {
                if (isInCall)
                    return;
                isInCall = true;
            }

            try
            {
                await inFunction();
            }
            finally
            {
                lock (syncLock)
                {
                    isInCall = false;
                }
            }
        }



        public void ExecuteOnlyOnce(Action inFunction)
        {
            lock (syncLock)
            {
                if (isInCall)
                    return;
                isInCall = true;
            }

            try
            {
                inFunction();
            }
            finally
            {
                lock (syncLock)
                {
                    isInCall = false;
                }
            }
        }

        private long mLastClickTime;

        /// <summary>
        /// Gets false if user double-clicking
        /// </summary>
        /// <returns></returns>
        public bool IsOneClickEvent()
        {
            var clickTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            if (clickTime - mLastClickTime < OneClickDelay)
                return false;

            mLastClickTime = clickTime;

            return true;
        }
    }
}
