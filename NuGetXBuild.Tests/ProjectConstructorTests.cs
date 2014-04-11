using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using NUnit.Framework;

namespace NuGetXBuild.Tests
{
	[TestFixture]
	public class ProjectConstructorTests
	{
		[Test]
		public void ProjectConstructor ()
		{
			string xml = @"<Project ToolsVersion='4.0' xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
	<ItemGroup>
		<Reference Include='Microsoft.Build' />
	</ItemGroup>
</Project>";
			string fileName = CreateProjectXmlFile (xml, "ProjectConstructorTest.csproj");

			var globalProperties = new Dictionary<string, string> ();
			var project = new Microsoft.Build.Evaluation.Project (fileName, globalProperties, null);

			int count = project.GetItems ("Reference").Count;
			Assert.AreEqual (1, count);
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

