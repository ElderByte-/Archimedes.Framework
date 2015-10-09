using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;

namespace Archimedes.Framework.SampleApp
{
    public class LoggerConfiguration
    {
        private readonly string _logFolder;
        private readonly string _logFile;


        public LoggerConfiguration(string appDataFolder, string logLevel) :
            this(appDataFolder, LogManager.GetRepository().LevelMap[logLevel])
        {

        }

        public LoggerConfiguration(string appDataFolder, Level logLevel)
        {
            _logFolder = appDataFolder + "\\Logs";
            _logFile = _logFolder + "\\events.log";

            Setup(logLevel);
        }

        /// <summary>
        /// Configures the log4net environment
        /// </summary>
        private void Setup(Level logLevel)
        {
            Directory.CreateDirectory(_logFolder);


            var hierarchy = (Hierarchy)LogManager.GetRepository();

            var patternLayout = new PatternLayout("%date [%thread] %-5level - %message%newline");
            patternLayout.ActivateOptions();

            // Log to a file
            var roller = new RollingFileAppender();
            roller.AppendToFile = false;
            roller.File = _logFile;
            roller.Layout = patternLayout;
            roller.MaxSizeRollBackups = 5;
            roller.RollingStyle = RollingFileAppender.RollingMode.Size;
            roller.StaticLogFileName = true;
            roller.ActivateOptions();
            hierarchy.Root.AddAppender(roller);

            // Log to the visual studio console
            var appender = new TraceAppender();
            appender.ActivateOptions();
            appender.Layout = patternLayout;
            hierarchy.Root.AddAppender(appender);

            hierarchy.Root.Level = logLevel;
            hierarchy.Configured = true;
        }
    }

    
}
