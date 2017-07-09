#tool "nuget:?package=NUnit.ConsoleRunner"
#tool "nuget:?package=GitVersion.CommandLine"

var solution = "../TextTableParser.sln";
var version = "";
var packagesPath = "../NugetPackages";
var apiKey = Argument<string>("ApiKey");

Task("Restore")
    .Does(() =>
{
    NuGetRestore(solution);
});

Task("SetVersion")
    .Does(() =>
{
    var gitVersion = GitVersion(new GitVersionSettings {
        UpdateAssemblyInfo = true
    });
    version = gitVersion.NuGetVersion;
});

Task("Build")
    .IsDependentOn("Restore")
    .Does(() =>
{
    DotNetBuild(solution );
});

Task("RunTests")
    .IsDependentOn("Build")
    .Does(() =>
{
    NUnit3("../Bin/*.Tests.dll");
});

Task("Pack")
    .IsDependentOn("RunTests")
    .Does(() =>
{
    var nuGetPackSettings = new NuGetPackSettings {
                                     Id                      = "TextTableParser",
                                     Version                 = version ,
                                     Title                   = "",
                                     Authors                 = new[] {"John Doe"},
                                     Owners                  = new[] {"Contoso"},
                                     Description             = "A set of classes for text file to table parsing and to convert text table to DTO of your type",
                                     Summary                 = "Classes for table manipulating",
                                     ProjectUrl              = new Uri("https://github.com/SergeyO/TextTableParser/"),
                                     Copyright               = "SergeyO 2017",
                                     Tags                    = new [] {"Text", "Parse", "CSV", "TSV"},
                                     RequireLicenseAcceptance= false,
                                     Symbols                 = false,
                                     NoPackageAnalysis       = true,
                                     OutputDirectory         = packagesPath
                                 };

     NuGetPack("../TextTableParser/TextTableParser.csproj", nuGetPackSettings);
});

Task("Publish")
    .IsDependentOn("SetVersion")
    .IsDependentOn("Pack")
    .Does(() =>
{
    foreach (var package in GetFiles(packagesPath + "/*.nupkg")) {
        Information("Publishing " + package + "...");
        NuGetPush(package, new NuGetPushSettings {
            ApiKey = apiKey
        });
    }
});

Task("Default")
    .IsDependentOn("Publish")
    .Does(() => {});

RunTarget("Default");