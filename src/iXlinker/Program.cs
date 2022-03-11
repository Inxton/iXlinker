using CommandLine;
using iXlinkerDtos;
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
                       var linker = new ScanTcProjFile();
                       linker.RuniXlinker(o.TsProjectFile, o.ActiveTargetPlatform, o.PlcProjectFile, o.GenerateMappings == "yes" ? true : false, o.DevenvPath);                       
                   })
                   .WithNotParsed(o => 
                   {
                       System.Console.WriteLine("Not parsed");
                   });            
        }
    }

    class CommandLineOptions
    {
        [Option('t', "tsproj", Required = true, HelpText = "TwinCAT project file [.tsproj].")]
        public string TsProjectFile { get { return this.tsProjectFile; } set { if (!string.IsNullOrEmpty(value)){ this.tsProjectFile = ReplaceDoubleBackslash(value); } } }

        [Option('p', "platform", Default = "Release|TwinCAT RT (x64)", HelpText = "Active target platform")]
        public string ActiveTargetPlatform { get; set; }

        [Option('c', "plcproj", Required = true, HelpText = "Plc project file [.plcproj].")]
        public string PlcProjectFile { get { return this.plcProjectFile; } set { if (!string.IsNullOrEmpty(value)) { this.plcProjectFile = ReplaceDoubleBackslash(value); } } }

        [Option('g', "generate", Default = "yes", HelpText = "Generate mappings")]
        public string GenerateMappings { get { return this.generateMappings; } set { if (!string.IsNullOrEmpty(value)) { this.generateMappings = TrueOrYes(value); } } }

        [Option('d', "devenv", Default = @"C:\Program Files (x86)\Microsoft Visual Studio\2019\Professional\Common7\IDE\devenv.com", HelpText = "Path to devenv.com")]
        public string DevenvPath { get { return this.devenvPath; } set { if (!string.IsNullOrEmpty(value)) { this.devenvPath = ReplaceDoubleBackslash(value); } } }

        private string tsProjectFile;
        private string plcProjectFile;
        private string generateMappings;
        private string devenvPath;
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


    }
}
