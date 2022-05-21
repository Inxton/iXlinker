using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cake.Common;
using Cake.Common.IO;
using Cake.Common.Tools.DotNet;
using Cake.Common.Tools.DotNet.Build;
using Cake.Common.Tools.DotNetCore.Publish;
using Cake.Common.Tools.DotNetCore.Test;
using Cake.Common.Tools.DotNet.MSBuild;
using Cake.Core;
using Cake.Frosting;
using Cake.Common.Tools.MSBuild;
public static class Program
{
    public static int Main(string[] args)
    {
        return new CakeHost()
            .UseContext<BuildContext>()
            .Run(args);
    }
}

public class BuildContext : FrostingContext
{
    public string MsBuildConfiguration { get; set; }
    public string WorkDirName => Environment.WorkingDirectory.GetDirectoryName();
    public string RootDir => Path.GetFullPath(Path.Combine(Environment.WorkingDirectory.FullPath, ".."));
    public string Solution => Path.GetFullPath(Path.Combine(Environment.WorkingDirectory.FullPath, @"..\iXlinker.sln"));
    public string SlnfCLI => Path.GetFullPath(Path.Combine(Environment.WorkingDirectory.FullPath, @"..\iXlinkerCLI.slnf"));
    public string SlnfExt => Path.GetFullPath(Path.Combine(Environment.WorkingDirectory.FullPath, @"..\iXlinkerExt.slnf"));
    public string CliProject => Path.GetFullPath(Path.Combine(Environment.WorkingDirectory.FullPath, @"..\src\iXlinker\iXlinker.csproj"));
    public string ExtProject => Path.GetFullPath(Path.Combine(Environment.WorkingDirectory.FullPath, @"..\src\iXlinkerExt\iXlinkerExt.csproj"));
    public string PublishDir => Path.GetFullPath(Path.Combine(Environment.WorkingDirectory.FullPath, @"..\src\iXlinker\_publish"));
    public string NugetDir => Path.GetFullPath(Path.Combine(Environment.WorkingDirectory.FullPath, @"..\_nuget"));
    public IEnumerable<string> TestProjects => Directory.EnumerateFiles(RootDir + "\\tests", "*Tests.csproj", SearchOption.AllDirectories);
    public string Version => GitVersionInformation.SemVer;

    private DotNetCorePublishSettings publishSettings;
    public DotNetCorePublishSettings PublishSettings { get => publishSettings ?? (publishSettings = new DotNetCorePublishSettings()); set => publishSettings = value; }

    public BuildContext(ICakeContext context)
        : base(context)
    {
        MsBuildConfiguration = context.Argument("configuration", "Release");
    }
}


[TaskName("Clean")]
public sealed class CleanTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        IEnumerable<string> folders = Directory.EnumerateDirectories(context.RootDir, "bin", SearchOption.AllDirectories).Where(p => !p.Contains(context.WorkDirName))
                    .Concat(Directory.EnumerateDirectories(context.RootDir, "obj", SearchOption.AllDirectories).Where(p => !p.Contains(context.WorkDirName)));
        foreach (string folder in folders)
        {
            context.CleanDirectory(folder);
        }
        context.CleanDirectory(context.PublishDir);
        context.CleanDirectory(context.NugetDir);
    }
}

[TaskName("Restore")]
[IsDependentOn(typeof(CleanTask))]
public sealed class RestoreTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        context.DotNetRestore(context.Solution, new Cake.Common.Tools.DotNet.Restore.DotNetRestoreSettings() { Runtime = "any" });
    }
}

[TaskName("BuildExt")]
[IsDependentOn(typeof(RestoreTask))]
public sealed class BuildExtTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        context.MSBuild(context.ExtProject, new MSBuildSettings() { Configuration = context.MsBuildConfiguration , PlatformTarget = PlatformTarget.MSIL , MSBuildPlatform= MSBuildPlatform.Automatic});
    }
}

[TaskName("CopyExtension")]
[IsDependentOn(typeof(BuildExtTask))]
public sealed class CopyExtensionTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        context.CopyDirectory(context.RootDir + @"\src\iXlinkerExt\bin\" + context.MsBuildConfiguration + @"\", context.PublishDir);
    }
}

[TaskName("BuildCli")]
[IsDependentOn(typeof(CopyExtensionTask))]
public sealed class BuildCliTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        context.DotNetBuild(context.SlnfCLI, new DotNetBuildSettings()
        {
            Configuration = context.MsBuildConfiguration,
            MSBuildSettings = new DotNetMSBuildSettings() { Version = context.Version }
        });
    }
}

[TaskName("TestsRun")]
[IsDependentOn(typeof(BuildCliTask))]
public sealed class TestsRunTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        foreach (string TestProject in context.TestProjects)
        {
            context.DotNetTest(TestProject, new DotNetCoreTestSettings() { Configuration = context.MsBuildConfiguration, Verbosity = DotNetVerbosity.Normal });
        }
    }
}

[TaskName("PublishExecutable")]
[IsDependentOn(typeof(TestsRunTask))]
public sealed class PublishExecutableTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        context.PublishSettings.Configuration = context.MsBuildConfiguration;
        context.PublishSettings.OutputDirectory = context.PublishDir;
        context.PublishSettings.Framework = "net5.0";
        context.PublishSettings.Runtime = "win-x64";
        context.PublishSettings.SelfContained = false;
        context.PublishSettings.PublishSingleFile = true;
        context.PublishSettings.PublishReadyToRun = true;
        context.PublishSettings.PublishTrimmed = false;
        context.PublishSettings.EnableCompressionInSingleFile = true;

        context.DotNetPublish(context.CliProject, context.PublishSettings);
    }
}

[TaskName("PackNugetPackage")]
[IsDependentOn(typeof(PublishExecutableTask))]
public sealed class PackNugetPackageTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        System.Console.WriteLine(GitVersionInformation.SemVer);
        Cake.Common.Tools.DotNet.Pack.DotNetPackSettings nuSettings =
            new Cake.Common.Tools.DotNet.Pack.DotNetPackSettings() { OutputDirectory = context.NugetDir, MSBuildSettings = new DotNetMSBuildSettings() { Version = context.Version }};
       
        context.DotNetPack(context.CliProject, nuSettings);
    }
}

[TaskName("PublishNugetPackage")]
[IsDependentOn(typeof(PackNugetPackageTask))]
public sealed class PublishNugetPackageTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        //context.PublishNuGets();
  
    }
}

[TaskName("Default")]
[IsDependentOn(typeof(PublishNugetPackageTask))]
public class DefaultTask : FrostingTask
{
}