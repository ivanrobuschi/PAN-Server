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
        string testFileName = @"C:\Temp\PANsTest.txt";
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
            var sut = new PANserver();
            sut.AcceptPAN(inputPAN).Should().Be(expectedResult);
        }

        [TestCase("1234567890123456", true)]
        [TestCase("123abc", false)]
        public void GivenPANAlredyExistsShouldCheckInFileIfGivenPANAlredyExists(string givenPAN, bool expectedResult)
        {
            //This method needs a manually created test file to work
            //var reader = new StreamReader(testFileName);
            //string testFirstLinePAN = reader.ReadLine().Substring(0, 15);
            var sut = new PANserver();
            sut.GivenPANAlredyExists(givenPAN, testFileName).Should().Be(expectedResult);
        }

        [Test]
        public void CreateListsShouldCreatePANsAndMasksListsFromFile()
        {
            var server = new PANserver();
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
            var sut = new PANserver();
            string actualPAN = sut.GetPAN(mask, testFileName);
            actualPAN.Should().Be(expectedPAN);
        }

        [TestCase("0123456789")]
        [TestCase("ABCDE")]
        public void GetPANShouldReturnAnErrorIfWrongMaskLength(string wrongMask)
        {
            var sut = new PANserver();
            sut.GetPAN(wrongMask, testFileName).Should().Be(sut.invalidMaskErrorMSG);
        }

        [TestCase("1234567890123456", "123456UHGTRA3456")]
        [TestCase("2345678901234567", "234567BDJXHW4567")]
        [TestCase("345123567ABCDEFG", "PAN invalido")]
        public void GetMaskShouldReturnMaskFromPan(string PAN, string expectedMask)
        {
            var sut = new PANserver();
            string actualMask = sut.GetMask(PAN, testFileName);
            actualMask.Should().Be(expectedMask);
        }

        [TestCase("1234567890123456")]
        public void CreateMaskShouldCreateMaskedPanFromPan(string testPAN)
        {
            var sut = new MaskGenerator();
            string maskedPAN = sut.CreateMask(testPAN);
            testPAN.Substring(0, 6).Should().Be(maskedPAN.Substring(0, 6));
            testPAN.Substring(12, 4).Should().Be(maskedPAN.Substring(12, 4));
            for (int i = 6; i <= 11; i++)
            {
                int parsedChar;
                char currentChar = maskedPAN[i];
                string currentCharString = currentChar.ToString();
                bool isANumber = int.TryParse(currentCharString, out parsedChar);
                isANumber.Should().BeFalse();
            }
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