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
	public class ReevaluateIfNecessaryTests
	{
		[Test]
		public void ReevaluateIfNecessaryWhenNoChangesMade ()
		{
			string xml = @"<Project xmlns='http://schemas.microsoft.com/developer/msbuild/2003'/>";
			Project project = new Project (XmlReader.Create (new StringReader (xml)));

			Assert.DoesNotThrow (() => project.ReevaluateIfNecessary ());
		}
	}
}

