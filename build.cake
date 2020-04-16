///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

var artifactsDir = Directory("./artifacts");
var publishDir = artifactsDir + Directory("publish");
var targetRuntime = "win-x64";
var targetFramework = "netcoreapp3.1";

var solution = "./VideoPlayer.sln";
var contractProject = "./Griffeye.VideoPlayerContract/Griffeye.VideoPlayerContract.csproj";
var wrapperProject = "./Griffeye.VlcWrapper/Griffeye.VlcWrapper.csproj";

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() => 
{
    CleanDirectory(artifactsDir);
    CleanDirectories("./*/bin");
    CleanDirectories("./*/obj");
});

Task("Restore-NuGet-Packages")
    .Does(() => 
{
    DotNetCoreRestore(new DotNetCoreRestoreSettings {
        IgnoreFailedSources = true,
		Verbosity = DotNetCoreVerbosity.Minimal,
    });
});

Task("Build")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
{
    DotNetCoreBuild(solution, new DotNetCoreBuildSettings
    {
        Configuration = configuration,
        NoRestore = true
    });

});

Task("Pack-Contract")
    .IsDependentOn("Build")
    .Does(() =>
{
    var packSettings = new DotNetCorePackSettings
    {
        Configuration = configuration,
        NoBuild = true,
        OutputDirectory = artifactsDir
    };
    DotNetCorePack(contractProject, packSettings);
});

Task("Pack-Wrapper")
    .IsDependentOn("Build")
    .Does(() =>
{
    var publishSettings = new DotNetCorePublishSettings
    {
         NoRestore = true,
         OutputDirectory = publishDir,
         Configuration = configuration,
         Framework = "netcoreapp3.1",
         Runtime = "win-x64",
         MSBuildSettings = new DotNetCoreMSBuildSettings()

    };
    publishSettings.MSBuildSettings.Properties.Add("Platform", new [] {"x64"});
    DotNetCorePublish(wrapperProject,publishSettings);

    var packSettings = new DotNetCorePackSettings
    {
        NoBuild = true,
        OutputDirectory = artifactsDir,
        MSBuildSettings = new DotNetCoreMSBuildSettings(),
        
    };
    packSettings.MSBuildSettings.Properties.Add("NuspecFile", new [] {"Griffeye.VlcWrapper.nuspec"});
    DotNetCorePack(wrapperProject, packSettings);

});

Task("Run-Unit-Tests")
    .IsDependentOn("Build")
    .Does(() =>
{
    var projects = GetFiles("./**/*.Tests.csproj");
    foreach(var project in projects)
    {
        DotNetCoreTest(
            project.ToString(),
            new DotNetCoreTestSettings()
            {
                Configuration = "Release",
                Logger = "trx",
                ResultsDirectory = artifactsDir,
                NoBuild = true
            });
    }
});

Task("Default")
    .IsDependentOn("Run-Unit-Tests")
    .IsDependentOn("Pack-Wrapper")
    .IsDependentOn("Pack-Contract");

RunTarget(target);