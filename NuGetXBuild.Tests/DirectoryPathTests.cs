using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using NUnit.Framework;

namespace NuGetXBuild.Tests
{
	[TestFixture]
	public class DirectoryPathTests
	{
		[Test]
		public void DirectoryPath ()
		{
			string xml = @"<Project ToolsVersion='4.0' xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
	<ItemGroup>
		<Reference Include='Microsoft.Build' />
	</ItemGroup>
</Project>";
			string fileName = CreateProjectXmlFile (xml, "DirectoryPathTest.csproj");

			var globalProperties = new Dictionary<string, string> ();
			Directory.SetCurrentDirectory (System.Environment.GetFolderPath (Environment.SpecialFolder.ApplicationData));
			var project = new Microsoft.Build.Evaluation.Project (fileName, globalProperties, null);

			string path = project.DirectoryPath;

			string expectedDirectoryPath = Path.GetDirectoryName (fileName);
			Assert.AreEqual (expectedDirectoryPath, path);
		}

		string CreateProjectXmlFile (string xml, string fileName)
		{
			string directory = Path.GetDirectoryName (typeof(ProjectConstructorTests).Assembly.Location);
			string fullPath = Path.Combine (directory, fileName);

			File.WriteAllText (fullPath, xml);

			return fullPath;
		}
	}
}

