using System.Collections.Generic;
using NUnit.Framework;
using FluentAssertions;
using System;
using NSubstitute;

namespace PANserver
{
    class PANserverTest
    {
        IPANArchiveManager _panArchiveManager;

        [SetUp]
        public void SetUp()
        {
            _panArchiveManager = Substitute.For<IPANArchiveManager>();
        }

        [TestCase("1234567890123456", true)]
        [TestCase("123456UHGTRA3456", false)]
        [TestCase("123456£22145qwerty7980", false)]
        [TestCase("123456789076767547890", false)]
        [TestCase("1234", false)]
        [TestCase("1234ac", false)]
        public void AcceptPANShouldAccept16NumberStringAsParameter(string inputPAN, bool expectedResult)
        {
            var sut = new PANserver(_panArchiveManager);
            sut.AcceptPAN(inputPAN).Should().Be(expectedResult);
        }

        //[TestCase("1234567890123456", true)]
        //[TestCase("123abc", false)]
        //public void GivenPANAlredyExistsShouldCheckInFileIfGivenPANAlredyExists(string givenPAN, bool expectedResult)
        //{
        //    //This method needs a manually created test file to work
        //    //var reader = new StreamReader(testFileName);
        //    //string testFirstLinePAN = reader.ReadLine().Substring(0, 15);
        //    var sut = new PANserver(testFileName);
        //    sut.GivenPANAlredyExists(givenPAN).Should().Be(expectedResult);
        //}

        [TestCase("123456UHGTRA3456", "1234567890123456")]
        [TestCase("234567BDJXHW4567", "2345678901234567")]
        [TestCase("0898656442278686", "")]
        public void GetPANShouldReturnPANFromMask(string mask, string expectedPAN)
        {
            _panArchiveManager.SearchPAN(mask).Returns(expectedPAN);

            var sut = new PANserver(_panArchiveManager);
            string actualPAN = sut.GetPAN(mask);
            actualPAN.Should().Be(expectedPAN);
        }

        [TestCase("0123456789")]
        [TestCase("ABCDE")]
        public void GetPANShouldReturnAnErrorIfWrongMaskLength(string wrongMask)
        {
            var sut = new PANserver(_panArchiveManager);
            sut.GetPAN(wrongMask).Should().Be(sut.invalidMaskErrorMSG);
        }

        [TestCase("1234567890123456", "123456UHGTRA3456")]
        [TestCase("2345678901234567", "234567BDJXHW4567")]
        [TestCase("345123567ABCDEFG", "PAN invalido")]
        public void GetMaskShouldReturnMaskFromPan(string PAN, string expectedMask)
        {
            _panArchiveManager.SearchMask(PAN).Returns(expectedMask);

            var sut = new PANserver(_panArchiveManager);
            string actualMask = sut.GetMask(PAN);
            actualMask.Should().Be(expectedMask);
        }

        [Test]
        public void GetMaskShouldReturnANewMaskWhenPanIsNotFound()
        {
            var pan = "1234567890123456";
            _panArchiveManager.SearchMask(pan).Returns((string)null);

            var sut = new PANserver(_panArchiveManager);
            var newMask = sut.GetMask(pan);
            newMask.Length.Should().Be(16);
            _panArchiveManager.Received().AddPanAndMask(pan, newMask);
        }

    }
}