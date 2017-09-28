#tool "nuget:?package=xunit.runner.console"
#tool "nuget:?package=ReportUnit"
var report = Directory("./reports");
var xunitReport = report + Directory("xunit");
var target = Argument("target", "Build");

Task("Default")
  .IsDependentOn("xUnit");

Task("Clean")   
    .Does(() =>
{
    CleanDirectories("../Source/**/bin");
    CleanDirectories("../Source/**/obj");
    CreateDirectory(xunitReport);
    CleanDirectories(xunitReport);
});

Task("Build")
  .IsDependentOn("Clean")
  .Does(() =>
{
  MSBuild("./src/CakeDemo.sln");
});

Task("xUnit")
  .IsDependentOn("Build")
    .Does(() =>
{
  XUnit2("./src/CakeDemo.Tests/bin/Debug/CakeDemo.Tests.dll", new XUnit2Settings
  {
       XmlReport = true,
      OutputDirectory = xunitReport
  });
})
.Finally(() => 
  {
    ReportUnit(xunitReport);
  });


RunTarget(target);