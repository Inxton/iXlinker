using CommandLine;
using iXlinker.Utils;
using iXlinkerDtos;
using Serilog;
using System.Collections.Generic;
using TsprojFile.Scan;

namespace iXlinker
{
    class Program
    {
        internal static void Main(string[] args)

        {


            Parser.Default.ParseArguments<CommandLineOptions>(args)
                   .WithParsed(o =>
                   {
                       EventLogger.VerbosityLevel = CommandLineOptions.GetVerbosity(o.Verbosity);
                       EventLogger.Instance.Logger.Information($"iXlinker started");

                       var linker = new ScanTcProjFile();
                       linker.RuniXlinker(o.TsProjectFile, o.ActiveTargetPlatform, o.PlcProjectFile, o.GenerateMappings == "yes" ? true : false, o.DevenvPath,ushort.Parse(o.MaxEthercatFrameIndex));                       
                   })
                   .WithNotParsed(o => 
                   {
                       EventLogger.Instance.Logger.Information("Not parsed");
                   });            
        }
    }

    internal class CommandLineOptions
    {
        [Option('t', "tsproj", Required = true, HelpText = "TwinCAT project file [.tsproj].")]
        public string TsProjectFile { get { return this.tsProjectFile; } set { if (!string.IsNullOrEmpty(value)){ this.tsProjectFile = ReplaceDoubleBackslash(value); } } }

        [Option('p', "platform", Default = "Release|TwinCAT RT (x64)", HelpText = "Active target platform")]
        public string ActiveTargetPlatform { get { return this.activeTargetPlatform; } set { if (!string.IsNullOrEmpty(value)) { this.activeTargetPlatform = value; } } }

        [Option('c', "plcproj", HelpText = "Plc project file [.plcproj]. If not specified and TwinCAT project contains only one PLC project, this one is used.")]
        public string PlcProjectFile { get { return this.plcProjectFile; } set { if (!string.IsNullOrEmpty(value)) { this.plcProjectFile = ReplaceDoubleBackslash(value); } } }

        [Option('g', "generate", Default = "yes", HelpText = "Generate mappings")]
        public string GenerateMappings { get { return this.generateMappings; } set { if (!string.IsNullOrEmpty(value)) { this.generateMappings = TrueOrYes(value); } } }

        [Option('d', "devenv", Default = @"C:\Program Files (x86)\Microsoft Visual Studio\2019\Professional\Common7\IDE\devenv.com", HelpText = "Path to devenv.com")]
        public string DevenvPath { get { return this.devenvPath; } set { if (!string.IsNullOrEmpty(value)) { this.devenvPath = ReplaceDoubleBackslash(value); } } }

        [Option('n', "maxframes", Default = "0", HelpText = "Highest index of the ethercat frame")]
        public string  MaxEthercatFrameIndex { get { return this.maxEthercatFrameIndex.ToString(); } set { if (!string.IsNullOrEmpty(value) && ushort.TryParse(value, out this.maxEthercatFrameIndex)) { } } }

        [Option('v', "verbosity", Default = "Information", HelpText = "Verbosity")]
        public string Verbosity { get { return this.verbosity; } set { if (!string.IsNullOrEmpty(value)) { this.verbosity = value; } } }

        private string tsProjectFile;
        private string activeTargetPlatform;
        private string plcProjectFile;
        private string generateMappings;
        private string devenvPath;
        private ushort maxEthercatFrameIndex;
        private string verbosity;
        private string ReplaceDoubleBackslash(string path)
        {
            return path.Replace("\\\\", "\\");
        }
        private string TrueOrYes(string value)
        {
            string retval = "false";
            if (value.ToLower().Contains("yes") | value.ToLower().Contains("true")) { retval = "yes";}
            return retval;
        }

        internal static Serilog.Events.LogEventLevel GetVerbosity(string sVerbosity)
        {
            switch (sVerbosity.ToLower())
            {
                case "verbose":
                    return Serilog.Events.LogEventLevel.Verbose;
                case "debug":
                    return Serilog.Events.LogEventLevel.Debug;
                case "information":
                    return Serilog.Events.LogEventLevel.Information;
                case "warning":
                    return Serilog.Events.LogEventLevel.Warning;
                case "error":
                    return Serilog.Events.LogEventLevel.Error;
                case "fatal":
                    return Serilog.Events.LogEventLevel.Fatal;
                default:
                    return Serilog.Events.LogEventLevel.Information;
            }
        }


    }
}
