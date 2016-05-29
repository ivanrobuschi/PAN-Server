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
            var sut = new PANserver();
            testExpectedPanList.Clear();
            testExpectedMaskList.Clear();
            testExpectedPanList.Add("1234567890123456");
            testExpectedPanList.Add("2345678901234567");
            testExpectedMaskList.Add("123456UHGTRA3456");
            testExpectedMaskList.Add("234567BDJXHW4567");
            sut.CreateLists(testFileName);
            testExpectedPanList.Should().BeEquivalentTo(sut.panList);
            testExpectedMaskList.Should().BeEquivalentTo(sut.maskList);
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
            var sut = new PANserver();
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
            var sut = new PANserver();
            sut.panList.Add("1234567890123456");
            sut.panList.Add("2345678901234567");
            sut.maskList.Add("123456UHGTRA3456");
            sut.maskList.Add("234567BDJXHW4567");
            sut.SaveLists(testFileName);
            sut.CreateLists(testFileName);
            sut.panList.Should().BeEquivalentTo(sut.panList);
            sut.maskList.Should().BeEquivalentTo(sut.maskList);

            //metodi: panExists, createMask, addData
        }
    }
}