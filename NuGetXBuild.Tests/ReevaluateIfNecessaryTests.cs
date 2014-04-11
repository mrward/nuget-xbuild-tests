using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
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
			var project = new Microsoft.Build.Evaluation.Project (XmlReader.Create (new StringReader (xml)));

			Assert.DoesNotThrow (() => project.ReevaluateIfNecessary ());
		}
	}
}

