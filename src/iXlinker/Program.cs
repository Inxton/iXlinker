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
        public string TsProjectFile { get; set; }

        [Option('c', "plcproj", Required = true, HelpText = "Plc project file [.plcproj].")]
        public string PlcProjectFile { get; set; }

        [Option('p', "platform", Default = "Release|TwinCAT RT (x64)", HelpText = "Active target platform")]
        public string ActiveTargetPlatform { get; set; }

        [Option('g', "generate", Default = "yes", HelpText = "Generate mappings")]
        public string GenerateMappings { get; set; }

        [Option('d', "devenv", Default = @"C:\Program Files (x86)\Microsoft Visual Studio\2019\Professional\Common7\IDE\devenv.com", HelpText = "Path to devenv.com")]
        public string DevenvPath { get; set; }
    }
    
}
