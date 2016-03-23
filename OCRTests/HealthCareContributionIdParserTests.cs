using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using OCR;

namespace OCRTests
{
    /// <summary>
    /// Summary description for HealthCareContributionIdParserTests
    /// </summary>
    [TestClass]
    public class HealthCareContributionIdParserTests
    {
       

        [TestMethod]
        [TestCategory("UnitTest")]
        [DeploymentItem(@"..\..\..\..\OCR-TPC\OCRTests\Resources\Parsedtext.txt")]
        public void Parse_Success()
        {
            string id = HealthCareContributionIdParser.Parse(File.ReadAllText("Parsedtext.txt"));
            Assert.IsTrue(id == "30 0026409 59");
        }



        [TestMethod]
        [TestCategory("UnitTest")]
        [ExpectedException(typeof(ArgumentException))]
        public void Parse_Fail()
        {
            string id = HealthCareContributionIdParser.Parse("lol");
        }
    }
}
