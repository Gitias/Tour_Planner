namespace Tourplanner.Shared.Logging
{
    using log4net;

    public class Log4NetLogger : ILogger
    {
        public Log4NetLogger(ILog log)
        {
            this.log = log;
        }

        public void Debug(string message)
        {
            log.Debug(message);
        }

        public void Error(string message)
        {
            log.Error(message);
        }

        public void Fatal(string message)
        {
            log.Fatal(message);
        }

        public void Info(string message)
        {
            log.Info(message);
        }

        public void Warn(string message)
        {
            log.Warn(message);
        }

        private readonly ILog log;
    }
}
