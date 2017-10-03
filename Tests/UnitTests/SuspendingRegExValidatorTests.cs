using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using BizTalkComponents.Utils;
using Winterdom.BizTalk.PipelineTesting;

namespace BizTalkComponents.PipelineComponents.ValidateCustom.Tests.UnitTests
{
    [TestClass]
    
    public class SuspendingRegExValidatorTests
    {
        [TestMethod]
        //[ExpectedException(typeof(Exception))]
        public void SuspendingRegExTestBadXpath()
        {

            var pipeline = PipelineFactory.CreateEmptyReceivePipeline();
            var component = new SuspendingRegExValidator
                {
                ///*[local-name()='MDM_KUNDE_TELEFON_RESPONSE']/*[local-name()='Success']/*[local-name()='ResultSets']/*[local-name()='Table1']/@*[local-name()='RETURKODE']
                Xpath = "/*[local-name()='MDM_KUNDE_TELEFON_RESPONSE']/Success/ResultSets/Table1[@RETURKODE]",
                Regex = @"^(?>\s*)\d*(?>\s*$)"

            };

            pipeline.AddComponent(component, PipelineStage.Decode);
            string filePath = @"C:\Github\ValidateCustom\Tests\UnitTests\TestData\Response_DB2.xml";
            

            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    var message = MessageHelper.CreateFromStream(sr.BaseStream);
                    var output = pipeline.Execute(message);

                }
            }

        }

        [TestMethod]
        public void PassingTest()
        {

            var pipeline = PipelineFactory.CreateEmptyReceivePipeline();
            var component = new SuspendingRegExValidator
            {

                Xpath = "root/test",
                Regex = "(TestValue)"

            };

            pipeline.AddComponent(component, PipelineStage.Decode);

            var message = MessageHelper.Create("<root><test>TestValue</test></root>");

            var output = pipeline.Execute(message);

            Assert.AreEqual(1, output.Count);

        }


        [TestMethod]
        public void SuspendingTestRegEx()
        {
            //Only allows digits switch testvalue to digits and the test will pass.
            var pipeline = PipelineFactory.CreateEmptyReceivePipeline();
            var component = new SuspendingRegExValidator
            {

                Xpath = "root/test/level1",
                Regex = @"^(?>\s*)\d*(?>\s*$)"

            };

            pipeline.AddComponent(component, PipelineStage.Decode);

            var message = MessageHelper.Create("<root><test><level1>TestValue</level1></test></root>");

            var output = pipeline.Execute(message);
            //Assert.AreEqual(1, output.Count);

        }


        [TestMethod]
        public void PassingRegExTest()
        {

            var pipeline = PipelineFactory.CreateEmptyReceivePipeline();
            var component = new SuspendingRegExValidator
            {
                Xpath = "/*[local-name()='test']",
                Regex = "(123)"
            };
            string filePath = @"C:\Github\ValidateCustom\Tests\UnitTests\TestData\simpletest.xml";
            pipeline.AddComponent(component, PipelineStage.Decode);

            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    var message = MessageHelper.CreateFromStream(sr.BaseStream);
                    var output = pipeline.Execute(message);

                    Assert.AreEqual(1, output.Count);
                }
            }

        }
    }
}
