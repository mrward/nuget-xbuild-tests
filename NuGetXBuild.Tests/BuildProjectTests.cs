using System;
using NUnit.Framework;
using System.IO;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Execution;
using System.Collections.Generic;
using System.Xml;
using System.Linq;
using Microsoft.Build.Framework;

namespace NuGetXBuild.Tests
{
	[TestFixture]
	public class BuildProjectTests
	{
		[Test]
		public void BuildProjectWithoutSpecifyingTargetsShouldBuildProject ()
		{
			RemoveOldBuildOutputIfExists ();

			// Create project
			string projectFullPath = GetProjectPath ();
			CreateProject (projectFullPath);

			// Build project.
			var projectProperties = new Dictionary<string, string> ();
			using (var projectCollection = new ProjectCollection (ToolsetDefinitionLocations.Registry | ToolsetDefinitionLocations.ConfigurationFile)) {
				var targetsToBuild = new string[0];
				BuildRequestData requestData = new BuildRequestData (projectFullPath, projectProperties, "4.0", targetsToBuild, null);
				var parameters = new BuildParameters (projectCollection) {
					NodeExeLocation = typeof(BuildProjectTests).Assembly.Location,
					ToolsetDefinitionLocations = projectCollection.ToolsetLocations
				};
				BuildResult result = BuildManager.DefaultBuildManager.Build (parameters, requestData);
			
				Assert.AreEqual (result.OverallResult, BuildResultCode.Success);
			}

			string outputFileName = GetBuildOutputFileName ();
			Assert.IsTrue (File.Exists (outputFileName));
		}

		void RemoveOldBuildOutputIfExists ()
		{
			string outputFileName = GetBuildOutputFileName ();
			if (File.Exists (outputFileName)) {
				File.Delete (outputFileName);
			}
		}

		string GetProjectPath ()
		{
			return Path.Combine (GetProjectDirectory (), "TestBuild.csproj");
		}

		string GetProjectDirectory ()
		{
			return Path.Combine (Path.GetDirectoryName (typeof(BuildProjectTests).Assembly.Location), "BuildProjectTest");
		}

		string GetBuildDirectory ()
		{
			return Path.Combine (GetProjectDirectory (), "bin", "Debug");
		}

		string GetBuildOutputFileName ()
		{
			return Path.Combine (GetBuildDirectory (), "TestBuild.dll");
		}

		void CreateProject (string projectFullPath)
		{
			string xml = 
@"<?xml version=""1.0"" encoding=""utf-8""?>
<Project DefaultTargets=""Build"" ToolsVersion=""4.0"" xmlns=""http://schemas.microsoft.com/developer/msbuild/2003"">
  <PropertyGroup>
    <Configuration Condition="" '$(Configuration)' == '' "">Debug</Configuration>
    <Platform Condition="" '$(Platform)' == '' "">AnyCPU</Platform>
    <ProjectGuid>{0C6DB678-81B5-4A23-9FC9-98C813A91BB7}</ProjectGuid>
    <OutputType>Library</OutputType>
    <RootNamespace>TestBuild</RootNamespace>
    <AssemblyName>TestBuild</AssemblyName>
  </PropertyGroup>
  <PropertyGroup Condition="" '$(Configuration)|$(Platform)' == 'Debug|AnyCPU' "">
    <DebugSymbols>true</DebugSymbols>
    <DebugType>full</DebugType>
    <Optimize>false</Optimize>
    <OutputPath>bin\Debug</OutputPath>
    <DefineConstants>DEBUG;</DefineConstants>
    <ErrorReport>prompt</ErrorReport>
    <WarningLevel>4</WarningLevel>
    <ConsolePause>false</ConsolePause>
  </PropertyGroup>
  <PropertyGroup Condition="" '$(Configuration)|$(Platform)' == 'Release|AnyCPU' "">
    <DebugType>full</DebugType>
    <Optimize>true</Optimize>
    <OutputPath>bin\Release</OutputPath>
    <ErrorReport>prompt</ErrorReport>
    <WarningLevel>4</WarningLevel>
    <ConsolePause>false</ConsolePause>
  </PropertyGroup>
  <ItemGroup>
    <Reference Include=""System"" />
  </ItemGroup>
  <ItemGroup>
    <Compile Include=""AssemblyInfo.cs"" />
  </ItemGroup>
  <Import Project=""$(MSBuildBinPath)\Microsoft.CSharp.targets"" />
</Project>";

			Directory.CreateDirectory (GetProjectDirectory ());
			File.WriteAllText (projectFullPath, xml);

			string assemblyInfo = 
				@"using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: AssemblyTitle (""TestBuild"")]
[assembly: AssemblyDescription ("""")]
[assembly: AssemblyConfiguration ("""")]
[assembly: AssemblyCompany ("""")]
[assembly: AssemblyProduct ("""")]
[assembly: AssemblyCopyright (""Xamarin Inc. (http://xamarin.com)"")]
[assembly: AssemblyTrademark ("""")]
[assembly: AssemblyCulture ("""")]
[assembly: AssemblyVersion (""1.0.*"")]
";

			File.WriteAllText (Path.Combine (GetProjectDirectory (), "AssemblyInfo.cs"), assemblyInfo);
		}

		[Test]
		public void BuildProjectSpecifyingTargetShouldBuildProject ()
		{
			RemoveOldBuildOutputIfExists ();

			// Create project
			string projectFullPath = GetProjectPath ();
			CreateProject (projectFullPath);

			// Build project.
			var projectProperties = new Dictionary<string, string> ();
			using (var projectCollection = new ProjectCollection (ToolsetDefinitionLocations.Registry | ToolsetDefinitionLocations.ConfigurationFile)) {
				var targetsToBuild = new string[] { "Build" };
				var rootElement = Microsoft.Build.Construction.ProjectRootElement.Create (
					XmlReader.Create (projectFullPath));
				rootElement.FullPath = projectFullPath;
				var projectInstance = new ProjectInstance (rootElement);
				BuildRequestData requestData = new BuildRequestData (projectInstance, targetsToBuild);
				var parameters = new BuildParameters (projectCollection) {
					Loggers = new ILogger[] { new Microsoft.Build.Logging.ConsoleLogger () },
					NodeExeLocation = typeof(BuildProjectTests).Assembly.Location,
					ToolsetDefinitionLocations = projectCollection.ToolsetLocations
				};
				BuildResult result = BuildManager.DefaultBuildManager.Build (parameters, requestData);

				foreach (var key in result.ResultsByTarget.Keys) {
					var targetResult = result.ResultsByTarget[key];
					Console.WriteLine (key);
					Console.WriteLine (targetResult.ResultCode);
					if (targetResult.Exception != null) {
						Console.WriteLine (targetResult.Exception);
					}
				}
				Assert.AreEqual (result.OverallResult, BuildResultCode.Success);
			}

			string outputFileName = GetBuildOutputFileName ();
			Assert.IsTrue (File.Exists (outputFileName));
		}
	}
}

