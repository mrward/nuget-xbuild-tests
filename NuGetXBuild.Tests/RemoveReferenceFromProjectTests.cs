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
			Project project = new Project (XmlReader.Create (new StringReader (xml)));
			ProjectItem referenceItem = project.GetItems ("Reference").Single ();

			project.RemoveItem (referenceItem);

			int count = project.GetItems ("Reference").Count;
			Assert.AreEqual (0, count);
		}
	}
}