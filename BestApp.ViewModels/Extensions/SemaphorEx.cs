namespace BestApp.ViewModels.Extensions
{
    public static class SemaphorEx
    {
        public static Exception SecureRelease(this SemaphoreSlim semaphore)
        {
            try
            {
                semaphore.Release();
                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }
    }
}
