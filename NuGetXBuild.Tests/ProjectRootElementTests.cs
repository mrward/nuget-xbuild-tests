using System;
using NUnit.Framework;
using System.IO;
using Microsoft.Build.Construction;
using System.Linq;
using System.Xml;

namespace NuGetXBuild.Tests
{
	[TestFixture]
	public class ProjectRootElementTests
	{
		[Test]
		[Ignore("This test does not work on Windows - ProjectRootElement.Create (path) does not take anything inside the root element")]
		public void CreateUsingFileName ()
		{
			string xml =
				@"<Project ToolsVersion='4.0' xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
<ItemGroup>
	<Reference Include='Microsoft.Build' />
</ItemGroup>
</Project>";

			string fileName = CreateProjectXmlFile (xml, "ProjectRootElementTest-CreateUsingFileName.csproj");
			ProjectRootElement projectElement = ProjectRootElement.Create (fileName);

			File.Delete (fileName);

			ProjectItemElement referenceItem = projectElement.Items.FirstOrDefault (i => i.Include == "Microsoft.Build");

			Assert.IsTrue (projectElement.Items.Count > 0);
			Assert.AreEqual ("Microsoft.Build", referenceItem.Include);
		}

		string CreateProjectXmlFile (string xml, string fileName)
		{
			string directory = Path.GetDirectoryName (typeof(ProjectRootElementTests).Assembly.Location);
			string fullPath = Path.Combine (directory, fileName);

			File.WriteAllText (fullPath, xml);

			return fullPath;
		}

		[Test]
		public void CreateUsingXmlReader ()
		{
			string xml =
				@"<Project ToolsVersion='4.0' xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
<ItemGroup>
	<Reference Include='Microsoft.Build' />
</ItemGroup>
</Project>";

			ProjectRootElement projectElement = ProjectRootElement.Create (XmlReader.Create (new StringReader (xml)));

			ProjectItemElement referenceItem = projectElement.Items.FirstOrDefault (i => i.Include == "Microsoft.Build");

			Assert.IsTrue (projectElement.Items.Count > 0);
			Assert.AreEqual ("Microsoft.Build", referenceItem.Include);
		}
	}
}

