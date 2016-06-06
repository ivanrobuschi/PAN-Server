using System.Collections.Generic;
using NUnit.Framework;
using FluentAssertions;
using System;

namespace PANserver
{
    class PANserverTest
    {
        //List<string> testMaskList = new List<string>();
        //List<string> testPanList = new List<string>();
        //string testFileName = @"C:\Temp\PANsTest.txt";
        string testFileName;
        List<string> testExpectedPanList = new List<string>();
        List<string> testExpectedMaskList = new List<string>();

        [SetUp]
        public void SetUp()
        {
            testFileName = AppDomain.CurrentDomain.BaseDirectory + "\\PANsTest.txt";
        }

        [TestCase("1234567890123456", true)]
        [TestCase("123456UHGTRA3456", false)]
        [TestCase("123456£22145qwerty7980", false)]
        [TestCase("123456789076767547890", false)]
        [TestCase("1234", false)]
        [TestCase("1234ac", false)]
        public void AcceptPANShouldAccept16NumberStringAsParameter(string inputPAN, bool expectedResult)
        {
            var sut = new PANserver(testFileName);
            sut.AcceptPAN(inputPAN).Should().Be(expectedResult);
        }

        [TestCase("1234567890123456", true)]
        [TestCase("123abc", false)]
        public void GivenPANAlredyExistsShouldCheckInFileIfGivenPANAlredyExists(string givenPAN, bool expectedResult)
        {
            //This method needs a manually created test file to work
            //var reader = new StreamReader(testFileName);
            //string testFirstLinePAN = reader.ReadLine().Substring(0, 15);
            var sut = new PANserver(testFileName);
            sut.GivenPANAlredyExists(givenPAN).Should().Be(expectedResult);
        }

        [Test]
        public void CreateListsShouldCreatePANsAndMasksListsFromFile()
        {
            var server = new PANserver(testFileName);
            var sut = new PANArchiveManager();
            testExpectedPanList.Clear();
            testExpectedMaskList.Clear();
            testExpectedPanList.Add("1234567890123456");
            testExpectedPanList.Add("2345678901234567");
            testExpectedMaskList.Add("123456UHGTRA3456");
            testExpectedMaskList.Add("234567BDJXHW4567");
            sut.CreateLists(testFileName, out server.panList, out server.maskList);
            testExpectedPanList.Should().BeEquivalentTo(server.panList);
            testExpectedMaskList.Should().BeEquivalentTo(server.maskList);
        }

        [TestCase("123456UHGTRA3456", "1234567890123456")]
        [TestCase("234567BDJXHW4567", "2345678901234567")]
        [TestCase("0898656442278686", "")]
        public void GetPANShouldReturnPANFromMask(string mask, string expectedPAN)
        {
            var sut = new PANserver(testFileName);
            string actualPAN = sut.GetPAN(mask);
            actualPAN.Should().Be(expectedPAN);
        }

        [TestCase("0123456789")]
        [TestCase("ABCDE")]
        public void GetPANShouldReturnAnErrorIfWrongMaskLength(string wrongMask)
        {
            var sut = new PANserver(testFileName);
            sut.GetPAN(wrongMask).Should().Be(sut.invalidMaskErrorMSG);
        }

        [TestCase("1234567890123456", "123456UHGTRA3456")]
        [TestCase("2345678901234567", "234567BDJXHW4567")]
        [TestCase("345123567ABCDEFG", "PAN invalido")]
        public void GetMaskShouldReturnMaskFromPan(string PAN, string expectedMask)
        {
            var sut = new PANserver(testFileName);
            string actualMask = sut.GetMask(PAN);
            actualMask.Should().Be(expectedMask);
        }
    }
}