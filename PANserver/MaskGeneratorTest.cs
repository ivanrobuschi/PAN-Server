using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using FluentAssertions;

namespace PANserver
{
    class MaskGeneratorTest
    {
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

    }
}
