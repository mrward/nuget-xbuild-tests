using System.Linq;
using System.Xml;
using Microsoft.Build.Evaluation;
using NUnit.Framework;
using System.IO;

namespace NuGetXBuild.Tests
{
	[TestFixture]
	public class GetTargetFrameworkMonikerTests
	{
		[Test]
		public void GetTargetFrameworkMonikerPropertyValueFromImportedTargetsFile ()
		{
			string xml =
				@"<Project ToolsVersion='4.0' xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
	<Import Project='$(MSBuildBinPath)\Microsoft.CSharp.targets' />
</Project>";
			Project project = new Project (XmlReader.Create (new StringReader (xml)));

			string value = project.GetPropertyValue ("TargetFrameworkMoniker");

			Assert.AreEqual (".NETFramework,Version=v4.0", value);
		}
	}
}

