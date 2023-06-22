namespace CoreRS.Logger
{
    public interface ILoggerManager
    {
        void LogDebug(string message);
        void LogError(string message);
        void LogFatal(string message);
        void LogInfo(string message);
        void LogWarn(string message);
    }
}