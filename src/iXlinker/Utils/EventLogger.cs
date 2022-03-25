using System;
using Serilog;

namespace iXlinker.Utils
{
    public class EventLogger
    {
        private Serilog.ILogger _logger;

        public static Serilog.Events.LogEventLevel VerbosityLevel { get; set; }

        private EventLogger()
        {
            _logger
                = new Serilog.LoggerConfiguration()
                    .MinimumLevel.Verbose()
                    .WriteTo.Console(restrictedToMinimumLevel: VerbosityLevel)
                    .WriteTo.File((AppDomain.CurrentDomain.BaseDirectory + "\\iXlinkerLog.txt").Replace("\\\\","\\"), restrictedToMinimumLevel: VerbosityLevel, fileSizeLimitBytes: 10000000)
                    .CreateLogger();
        }


        public Serilog.ILogger Logger
        {
            get { return _logger; }
        }

        private static EventLogger instance;

        private const string exportableMessagePrefix = ">>>>>>";
        public void Error(
                          string description,
                          string project = "",
                          string file = "",
                          int line = 0,
                          int column = 0,
                          string code = "")
        {
            Logger.Error($"{exportableMessagePrefix}\"E\",\"{code}\",\"{description}\",\"{project}\",\"{file}\",\"{line}\",\"{column}\"");
            Environment.Exit(1);
        }


        public void Warning(
                          string description,
                          string project = "",
                          string file = "",
                          int line = 0,
                          int column = 0,
                          string code = "")
        {
            Logger.Warning($"{exportableMessagePrefix}\"W\",\"{code}\",\"{description}\",\"{project}\",\"{file}\",\"{line}\",\"{column}\"");
        }


        public void Error(Exception ex, string message)
        {
            Logger.Error(ex, message);
        }

        public void Debug(string message)
        {
            Logger.Debug(message);
        }

        public void Information(string message)
        {
            Logger.Information(message);
        }

        public void Verbose(string message)
        {
            Logger.Verbose(message);
        }

        public static EventLogger Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EventLogger();
                }

                return instance;

            }
        }

        public void Done()
        {
            Logger.Information($"{exportableMessagePrefix}I, Done");
        }

    }
}
