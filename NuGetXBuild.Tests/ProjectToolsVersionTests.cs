using System;
using NUnit.Framework;
using System.Xml;
using System.IO;

namespace NuGetXBuild.Tests
{
	[TestFixture]
	public class ProjectToolsVersionTests
	{
		[Test]
		public void GetToolsVersion ()
		{
			string xml =
@"<Project ToolsVersion='4.0' xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
</Project>";
			var project = new Microsoft.Build.Evaluation.Project (XmlReader.Create (new StringReader (xml)));
			string value = project.ToolsVersion;

			Assert.AreEqual ("4.0", value);
		}
	}
}

