///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

var artifactsDir = Directory("./artifacts");
var publishDir =  Directory("./publish");
var targetRuntime = "win-x64";
var targetFramework = "net7.0-windows";

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
	CleanDirectory(publishDir);
	CleanDirectory("test-results");
	CleanDirectories("./*/bin");
	CleanDirectories("./*/obj");
});

Task("Restore-NuGet-Packages")
	.Does(() => 
{
	DotNetRestore(new DotNetRestoreSettings {
		IgnoreFailedSources = true,
		Verbosity = DotNetVerbosity.Minimal,
		Runtime = targetRuntime
	});
});

Task("Build")
	.IsDependentOn("Clean")
	.IsDependentOn("Restore-NuGet-Packages")
	.Does(() =>
{
	DotNetBuild(solution, new DotNetBuildSettings
	{
		Configuration = configuration,
		NoRestore = true,
	});
});

Task("Pack-Contract")
	.IsDependentOn("Build")
	.Does(() =>
{
	var packSettings = new DotNetPackSettings
	{
		Configuration = configuration,
		NoBuild = true,
		OutputDirectory = artifactsDir
	};
	DotNetPack(contractProject, packSettings);
});

Task("Pack-Wrapper")
	.IsDependentOn("Build")
	.Does(() =>
{
	var publishSettings = new DotNetPublishSettings
	{
		 NoRestore = false,
		 OutputDirectory = publishDir,
		 Configuration = configuration,
		 Framework = targetFramework,
		 Runtime = targetRuntime,
		 SelfContained = true,
		 MSBuildSettings = new DotNetMSBuildSettings()
	};
	publishSettings.MSBuildSettings.Properties.Add("Platform", new [] {"x64"});
	DotNetPublish(wrapperProject, publishSettings);

	var packSettings = new DotNetPackSettings
	{
		NoBuild = true,
		OutputDirectory = artifactsDir,
		MSBuildSettings = new DotNetMSBuildSettings(),
	};
	packSettings.MSBuildSettings.Properties.Add("NuspecFile", new [] {"Griffeye.VlcWrapper.nuspec"});
	DotNetPack(wrapperProject, packSettings);

});

Task("Run-Unit-Tests")
	.IsDependentOn("Build")
	.Does(() =>
{
	var projects = GetFiles("./**/*.Tests.csproj");
	foreach(var project in projects)
	{
		DotNetTest(
			project.ToString(),
			new DotNetTestSettings()
			{
				Configuration = "Release",
				Loggers = new[] { "trx" },
				ResultsDirectory = "test-results",
				NoBuild = true
			});
	}
});

Task("Default")
	.IsDependentOn("Run-Unit-Tests")
	.IsDependentOn("Pack-Wrapper")
	.IsDependentOn("Pack-Contract");

RunTarget(target);
