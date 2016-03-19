using Microsoft.VisualStudio.TestTools.UnitTesting;
using OCR;
using System.Collections.Generic;

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

    }
}
