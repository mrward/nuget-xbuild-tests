using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using Microsoft.Build.Evaluation;
using NUnit.Framework;

namespace NuGetXBuild.Tests
{
	[TestFixture]
	public class GetReferenceProjectItemsTests
	{
		[Test]
		[Ignore("This works on MAC")]
		public void GetReferenceProjectItemUsingGetItems ()
		{
			string xml =
				@"<Project ToolsVersion='4.0' xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
	<ItemGroup>
		<Reference Include='Microsoft.Build' />
	</ItemGroup>
</Project>";
			Project project = new Project (XmlReader.Create (new StringReader (xml)));

			ProjectItem referenceItem = project.GetItems ("Reference").Single ();

			Assert.AreEqual ("Microsoft.Build", referenceItem.EvaluatedInclude);
		}

		// Works on Windows but not on Mac
		[Test]
		public void GetReferenceProjectItemUsingGetItemsWhenProjectLoadedFromDisk ()
		{
			string xml =
				@"<Project ToolsVersion='4.0' xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
	<ItemGroup>
		<Reference Include='Microsoft.Build' />
	</ItemGroup>
</Project>";

			string fileName = CreateProjectXmlFile (xml, "GetReferenceProjectItemUsingGetItemsWhenProjectLoadedFromDisk.csproj");
			Project project = new Project (fileName);

			File.Delete (fileName);

			ProjectItem referenceItem = project.GetItems ("Reference").SingleOrDefault ();

			Assert.AreEqual ("Microsoft.Build", referenceItem.EvaluatedInclude);
		}

		string CreateProjectXmlFile (string xml, string fileName)
		{
			string directory = Path.GetDirectoryName (typeof(GetReferenceProjectItemsTests).Assembly.Location);
			string fullPath = Path.Combine (directory, fileName);

			File.WriteAllText (fullPath, xml);

			return fullPath;
		}

		[Test]
		[Ignore("This works on MAC")]
		public void GetReferenceProjectItemUsingGetItemsWhenProjectHasManyItems ()
		{
			string xml =
				@"<?xml version=""1.0"" encoding=""utf-8""?>
				<Project ToolsVersion=""4.0"" xmlns=""http://schemas.microsoft.com/developer/msbuild/2003"" DefaultTargets=""Build"">
  <PropertyGroup>
    <ProjectGuid>{44563C28-FDC5-4025-AF51-3718447F60C4}</ProjectGuid>
			<Configuration Condition="" '$(Configuration)' == '' "">Debug</Configuration>
				<Platform Condition="" '$(Platform)' == '' "">AnyCPU</Platform>
    <OutputType>Exe</OutputType>
    <RootNamespace>test</RootNamespace>
    <AssemblyName>test</AssemblyName>
    <TargetFrameworkVersion>v4.5</TargetFrameworkVersion>
    <AppDesignerFolder>Properties</AppDesignerFolder>
  </PropertyGroup>
				<PropertyGroup Condition="" '$(Platform)' == 'AnyCPU' "">
    <PlatformTarget>AnyCPU</PlatformTarget>
    <Prefer32Bit>True</Prefer32Bit>
  </PropertyGroup>
				<PropertyGroup Condition="" '$(Configuration)' == 'Debug' "">
    <OutputPath>bin\Debug\</OutputPath>
    <DebugSymbols>True</DebugSymbols>
    <DebugType>Full</DebugType>
    <Optimize>False</Optimize>
    <CheckForOverflowUnderflow>True</CheckForOverflowUnderflow>
    <DefineConstants>DEBUG;TRACE</DefineConstants>
  </PropertyGroup>
			<PropertyGroup Condition="" '$(Configuration)' == 'Release' "">
    <OutputPath>bin\Release\</OutputPath>
    <DebugSymbols>False</DebugSymbols>
    <DebugType>None</DebugType>
    <Optimize>True</Optimize>
    <CheckForOverflowUnderflow>False</CheckForOverflowUnderflow>
    <DefineConstants>TRACE</DefineConstants>
  </PropertyGroup>
  <ItemGroup>
				<Reference Include=""Newtonsoft.Json"">
      <HintPath>packages\Newtonsoft.Json.5.0.6\lib\net45\Newtonsoft.Json.dll</HintPath>
    </Reference>
				<Reference Include=""nunit.framework"">
      <HintPath>packages\NUnit.2.6.2\lib\nunit.framework.dll</HintPath>
    </Reference>
				<Reference Include=""System"" />
				<Reference Include=""System.Core"">
      <RequiredTargetFramework>3.5</RequiredTargetFramework>
    </Reference>
				<Reference Include=""System.Data"" />
				<Reference Include=""System.Data.DataSetExtensions"">
      <RequiredTargetFramework>3.5</RequiredTargetFramework>
    </Reference>
				<Reference Include=""System.Xml"" />
				<Reference Include=""System.Xml.Linq"">
      <RequiredTargetFramework>3.5</RequiredTargetFramework>
    </Reference>
  </ItemGroup>
  <ItemGroup>
				<Compile Include=""Program.cs"" />
				<Compile Include=""Properties\AssemblyInfo.cs"" />
  </ItemGroup>
  <ItemGroup>
				<None Include=""app.config"" />
				<None Include=""packages.config"" />
				<None Include=""Scripts\jquery-2.0.3-vsdoc.js"" />
				<None Include=""Scripts\jquery-2.0.3.js"" />
				<None Include=""Scripts\jquery-2.0.3.min.js"" />
				<None Include=""Scripts\jquery-2.0.3.min.map"" />
  </ItemGroup>
				<Import Project=""$(MSBuildToolsPath)\Microsoft.CSharp.targets"" />
				</Project>";

			Project project = new Project (XmlReader.Create (new StringReader (xml)));
			project = new Project (@"/Users/matt/Projects/test/test.csproj");
			ProjectItem referenceItem = project.GetItems ("Reference").Single (i => i.EvaluatedInclude == "Newtonsoft.Json");

			Assert.AreEqual ("Newtonsoft.Json", referenceItem.EvaluatedInclude);
		}
	}
}