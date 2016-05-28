using Microsoft.VisualStudio.TestTools.UnitTesting;
using OCR;
using System;
using System.Collections.Generic;
using System.IO;

namespace OCRTests
{

    [TestClass]
    public class ContributionPeriodsParserTests
    {
        [TestMethod]
        [TestCategory("UnitTest")]
        public void Parse_Success_EmptyText()
        {
            string textToParse = "";
            var parser = new ContributionPeriodsParser();

            List<ContributionPeriod> result = parser.Parse(textToParse);
            Assert.IsTrue(result.Count == 0);
        }


        [TestMethod]
        [TestCategory("UnitTest")]
        [DeploymentItem(@"..\..\..\..\OCR-TPC\OCRTests\Resources\Parsedtext.txt")]
        public void Parse_Success()
        {
            string textToParse = File.ReadAllText("Parsedtext.txt");
            var parser = new ContributionPeriodsParser();

            List<ContributionPeriod> result = parser.Parse(textToParse);
            Assert.IsTrue(result.Count == 5);
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public void ParseContributionPeriod_Success()
        {
            var parser = new ContributionPeriodsParser();
            ContributionPeriod expected = new ContributionPeriod(new DateTime(2011, 1, 1), new DateTime(2011, 12, 1), 99920.66, true);
            ContributionPeriod obtained = parser.ParseContributionPeriod("Enero 2011 a Diciembre 2011 99.920,66");

            Assert.AreEqual(obtained, expected);
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public void ParseContributionPeriod_Fail()
        {
            var parser = new ContributionPeriodsParser();
            ContributionPeriod obtained = parser.ParseContributionPeriod("Enero 2011   Diciembre 2011 99.920,66");
            Assert.IsNull(obtained);
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public void ParseSpanishMonth_Success()
        {
            var parser = new ContributionPeriodsParser();
            Assert.AreEqual(parser.ParseSpanishMonth("EnEro"), 1);
            Assert.AreEqual(parser.ParseSpanishMonth("febrero"), 2);
            Assert.AreEqual(parser.ParseSpanishMonth("DICIEMBRE"), 12);

        }

        [TestMethod]
        [TestCategory("UnitTest")]
        [ExpectedException(typeof(ArgumentException))]
        public void ParseSpanishMonth_Fails()
        {
            var parser = new ContributionPeriodsParser();
            parser.ParseSpanishMonth("trolazo");
        }

    }
}
