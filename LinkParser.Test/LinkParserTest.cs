﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LinkParser.Test
{

	[TestFixture]
	[Category("LinkParserTest Tests")]
	class LinkParserTest
	{
		List<string> sampleMatches;

		[OneTimeSetUp]
		public void TestSetup()
		{
			string resource_data = Properties.Resources.sample;
			sampleMatches = resource_data.Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries).ToList();
		}

		[Test]
		public void ShouldParseFile()
		{
			List<string> matches = DataExtractor.ExtractData((Properties.Resources.rawSample));
			CollectionAssert.AreEquivalent(sampleMatches, matches);
		}
	}
}
