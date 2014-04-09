using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using NUnit.Framework;

namespace NuGetXBuild.Tests
{
	[TestFixture]
	public class RemoveReferenceFromProjectTests
	{
		[Test]
		public void RemoveReferenceFromProject ()
		{
			string xml =
@"<Project ToolsVersion='4.0' xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
	<ItemGroup>
		<Reference Include='Microsoft.Build' />
	</ItemGroup>
</Project>";
			var project = new Microsoft.Build.Evaluation.Project (XmlReader.Create (new StringReader (xml)));
			var referenceItem = project.GetItems ("Reference").Single ();

			project.RemoveItem (referenceItem);

			int count = project.GetItems ("Reference").Count;
			Assert.AreEqual (0, count);
		}
	}
}