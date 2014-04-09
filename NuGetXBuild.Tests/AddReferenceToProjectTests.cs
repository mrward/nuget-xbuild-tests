using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using NUnit.Framework;

namespace NuGetXBuild.Tests
{
	[TestFixture]
	public class AddReferenceToProjectTests
	{
		[Test]
		public void AddNUnitReferenceWithHintPathToProject ()
		{
			string xml = @"<Project xmlns='http://schemas.microsoft.com/developer/msbuild/2003'/>";
			var project = new Microsoft.Build.Evaluation.Project (XmlReader.Create (new StringReader (xml)));

			string relativePath = "packages/NUnit.2.6.3/lib/nunit.framework.dll";
			project.AddItem ("Reference",
				"NUnit.Framework",
				new[] {
					new KeyValuePair<string, string>("HintPath", relativePath)
				});

			var referenceItem = project.GetItems ("Reference").SingleOrDefault ();

			Assert.IsNotNull (referenceItem);
			Assert.AreEqual ("NUnit.Framework", referenceItem.EvaluatedInclude);
			Assert.AreEqual (relativePath, referenceItem.GetMetadataValue ("HintPath"));
		}
	}
}

