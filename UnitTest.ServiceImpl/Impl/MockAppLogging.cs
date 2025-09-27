using Common.Abstrtactions;

namespace UnitTest.ServiceImpl.Impl
{
    public class MockAppLogging : ILoggingService
    {
        public void Log(string message)
        {
            Console.WriteLine(message);
        }

        public void LogWarning(string message)
        {
            Console.WriteLine(message);
        }

        public void TrackError(Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }
}
