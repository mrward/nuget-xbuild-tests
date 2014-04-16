using System.Collections.Generic;
using System.Linq;
using System.Xml;
using NUnit.Framework;
using System.IO;

namespace NuGetXBuild.Tests
{
	[TestFixture]
	public class GetTargetPathTests
	{
		[Test]
		public void GetDebugTargetPath ()
		{
			string xml =
@"<Project ToolsVersion='4.0' xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
	<PropertyGroup>
		<AssemblyName>test</AssemblyName>
		<OutputType>Exe</OutputType>
	</PropertyGroup>
	<PropertyGroup Condition="" '$(Configuration)' == 'Debug' "">
		<OutputPath>bin\Debug\</OutputPath>
	</PropertyGroup>
	<PropertyGroup Condition="" '$(Configuration)' == 'Release' "">
		<OutputPath>bin\Release\</OutputPath>
	</PropertyGroup>
	<Import Project='$(MSBuildBinPath)\Microsoft.CSharp.targets' />
</Project>";
			var properties = new Dictionary<string, string> ();
			properties.Add ("Configuration", "Debug");
			var project = new Microsoft.Build.Evaluation.Project (XmlReader.Create (new StringReader (xml)), properties, null);
			string value = project.GetPropertyValue ("TargetPath");

			string directory = Directory.GetCurrentDirectory ();
			string expectedValue = Path.Combine (directory, @"bin\Debug\test.exe");
			Assert.AreEqual (expectedValue, value);
		}

		[Test]
		public void GetReleaseTargetPath ()
		{
			string xml =
@"<Project ToolsVersion='4.0' xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
	<PropertyGroup>
		<AssemblyName>test</AssemblyName>
		<OutputType>Library</OutputType>
	</PropertyGroup>
	<PropertyGroup Condition="" '$(Configuration)' == 'Debug' "">
		<OutputPath>bin\Debug\</OutputPath>
	</PropertyGroup>
	<PropertyGroup Condition="" '$(Configuration)' == 'Release' "">
		<OutputPath>bin\Release\</OutputPath>
	</PropertyGroup>
	<Import Project='$(MSBuildBinPath)\Microsoft.CSharp.targets' />
</Project>";
			var properties = new Dictionary<string, string> ();
			properties.Add ("Configuration", "Release");
			var project = new Microsoft.Build.Evaluation.Project (XmlReader.Create (new StringReader (xml)), properties, null);
			string value = project.GetPropertyValue ("TargetPath");

			string directory = Directory.GetCurrentDirectory ();
			string expectedValue = Path.Combine (directory, @"bin\Release\test.dll");
			Assert.AreEqual (expectedValue, value);
		}
	}
}

