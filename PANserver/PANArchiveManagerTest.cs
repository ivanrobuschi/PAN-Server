using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using FluentAssertions;

namespace PANserver
{
    class PANArchiveManagerTest
    {
        string testFileName;
        List<string> testExpectedPanList = new List<string>();
        List<string> testExpectedMaskList = new List<string>();

        [SetUp]
        public void SetUp()
        {
            testFileName = AppDomain.CurrentDomain.BaseDirectory + "\\PANsTest.txt";
        }

        [Test]
        public void SaveListsShouldWritePANsAndMasksListsToFile()
        {
            //This test needs that sut.CreateLists is working correctly
            var server = new PANserver();
            var sut = new PANArchiveManager();
            server.panList.Add("1234567890123456");
            server.panList.Add("2345678901234567");
            server.maskList.Add("123456UHGTRA3456");
            server.maskList.Add("234567BDJXHW4567");
            testExpectedPanList.Clear();
            testExpectedMaskList.Clear();
            testExpectedPanList.Add("1234567890123456");
            testExpectedPanList.Add("2345678901234567");
            testExpectedMaskList.Add("123456UHGTRA3456");
            testExpectedMaskList.Add("234567BDJXHW4567");
            sut.SaveLists(testFileName, server.panList, server.maskList);
            sut.CreateLists(testFileName, out server.panList, out server.maskList);
            server.panList.Should().BeEquivalentTo(testExpectedPanList);
            server.maskList.Should().BeEquivalentTo(testExpectedMaskList);

            //metodi: panExists, createMask, addData
        }
    }
}
