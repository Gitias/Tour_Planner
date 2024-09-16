namespace Tourplanner.Shared.Logging
{
    using log4net;
    using log4net.Config;
    using Microsoft.Extensions.Configuration;
    using System.IO;

    public class Log4NetFactory : ILoggerFactory
    {
        public Log4NetFactory(IConfiguration configuration)
        {
            configPath = configuration["Logging:Log4Net:ConfigFile"]
                ?? throw new ArgumentNullException(nameof(configuration));
        }
        public ILogger CreateLogger<TContext>()
        {
            if(!File.Exists(configPath))
            {
                throw new FileNotFoundException("Log4Net configuration file not found", configPath);
            }

            XmlConfigurator.Configure(new FileInfo(configPath));

            return new Log4NetLogger(LogManager.GetLogger(typeof(TContext)));
        }

        private readonly string configPath;
    }
}
