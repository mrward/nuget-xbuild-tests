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
	public class ProjectTests
	{
		[Test]
		public void GetTargetFrameworkMonitorPropertyValueFromImportedTargetsFile ()
		{
			string xml =
@"<Project ToolsVersion='4.0' xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
	<Import Project='$(MSBuildBinPath)\Microsoft.CSharp.targets' />
</Project>";
			Project project = new Project (XmlReader.Create (new StringReader (xml)));

			string value = project.GetPropertyValue ("TargetFrameworkMoniker");

			Assert.AreEqual (".NETFramework,Version=v4.0", value);
		}

		[Test]
		public void ReevaluateIfNecessaryWhenNoChangesMade ()
		{
			string xml = @"<Project xmlns='http://schemas.microsoft.com/developer/msbuild/2003'/>";
			Project project = new Project (XmlReader.Create (new StringReader (xml)));

			Assert.DoesNotThrow (() => project.ReevaluateIfNecessary ());
		}

		[Test]
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