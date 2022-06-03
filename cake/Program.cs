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
using Cake.Core.Diagnostics;

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
    public bool SkipTests => false;
    public string MsBuildConfiguration { get; set; }
    public string WorkDirName => Environment.WorkingDirectory.GetDirectoryName();
    public string RootDir => Path.GetFullPath(Path.Combine(Environment.WorkingDirectory.FullPath, ".."));
    public string Solution => Path.GetFullPath(Path.Combine(Environment.WorkingDirectory.FullPath, @"..\iXlinker.sln"));
    public string SlnfCLI => Path.GetFullPath(Path.Combine(Environment.WorkingDirectory.FullPath, @"..\iXlinkerCLI.slnf"));
    public string SlnfExt => Path.GetFullPath(Path.Combine(Environment.WorkingDirectory.FullPath, @"..\iXlinkerExt.slnf"));
    public string CliProject => Path.GetFullPath(Path.Combine(Environment.WorkingDirectory.FullPath, @"..\src\iXlinker\iXlinker.csproj"));
    public string ExtProject => Path.GetFullPath(Path.Combine(Environment.WorkingDirectory.FullPath, @"..\src\iXlinkerExt\iXlinkerExt.csproj"));
    public string ExtInstProject => Path.GetFullPath(Path.Combine(Environment.WorkingDirectory.FullPath, @"..\src\iXlinkerExtInstaller\iXlinkerExtInstaller.csproj"));
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
        context.MSBuild(context.ExtProject, new MSBuildSettings() { Configuration = context.MsBuildConfiguration, PlatformTarget = PlatformTarget.MSIL, MSBuildPlatform = MSBuildPlatform.Automatic });
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

[TaskName("BuildExtInstaller")]
[IsDependentOn(typeof(CopyExtensionTask))]
public sealed class BuildExtInstallerTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        context.DotNetBuild(context.ExtInstProject, new DotNetBuildSettings() { Configuration = context.MsBuildConfiguration });
    }
}

[TaskName("PublishExtInstaller")]
[IsDependentOn(typeof(BuildExtInstallerTask))]
public sealed class PublishExtInstallerTask : FrostingTask<BuildContext>
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
        context.PublishSettings.EnableCompressionInSingleFile = false;

        context.DotNetPublish(context.ExtInstProject, context.PublishSettings);
    }
}

[TaskName("BuildCli")]
[IsDependentOn(typeof(PublishExtInstallerTask))]
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
    public override bool ShouldRun(BuildContext context)
    {
        return context.SkipTests.Equals(false);
    }
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
        context.PublishSettings.EnableCompressionInSingleFile = false;

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
            new Cake.Common.Tools.DotNet.Pack.DotNetPackSettings() 
            { 
                OutputDirectory = context.NugetDir, 
                MSBuildSettings = new DotNetMSBuildSettings() 
                { 
                    Version = context.Version 
                }                 
            };
       
        context.DotNetPack(context.CliProject, nuSettings);
    }
}

[TaskName("PublishNugetPackage")]
[IsDependentOn(typeof(PackNugetPackageTask))]
public sealed class PublishNugetPackageTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        foreach (var nugetFile in Directory.EnumerateFiles(context.NugetDir, "*.nupkg").Select(p => new FileInfo(p)))
        {
            context.Log.Information($"Signing {nugetFile.FullName}");

            var arguments = new Cake.Core.IO.ProcessArgumentBuilder();
            arguments.Append("nuget")
                     .Append("sign")
                     .Append(nugetFile.FullName)
                     .Append("--certificate-path")
                     .AppendQuotedSecret(System.Environment.GetEnvironmentVariable("cp"))
                     .Append("--certificate-password")
                     .AppendSecret(System.Environment.GetEnvironmentVariable("cpw"))
                     .Append("--timestamper")
                     .Append("http://timestamp.digicert.com/")
                     .Append("--overwrite");

            context.ProcessRunner.Start("dotnet.exe", new Cake.Core.IO.ProcessSettings() { Arguments = arguments, Silent = true }).WaitForExit();

            context.DotNetNuGetPush(nugetFile.FullName, new Cake.Common.Tools.DotNet.NuGet.Push.DotNetNuGetPushSettings()
            {
                Source = "https://api.nuget.org/v3/index.json",
                ApiKey = System.Environment.GetEnvironmentVariable("gh-inxton-ixlinker-nuget-api"),
                // SymbolApiKey = System.Environment.GetEnvironmentVariable("gh-inxton-ixlinker-nuget-api")

            });
        }


        // context.PublishNuGets();

    }
}

[TaskName("Default")]
[IsDependentOn(typeof(PublishNugetPackageTask))]
public class DefaultTask : FrostingTask
{
}