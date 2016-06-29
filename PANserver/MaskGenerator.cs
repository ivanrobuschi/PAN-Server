using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PANserver
{
    class MaskGenerator
    {
        public string CreateMask(string PAN)
        {
            string maskedPAN = PAN.Substring(0, 6);
            const string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string twicePreviousCharString = "";
            string previousCharString = "";
            var random = new Random();
            for (int i = 6; i <= 11; i++)
            {
                int letterIndex = random.Next(26);
                string currentCharString = alphabet[letterIndex].ToString();
                if ((currentCharString.Equals(previousCharString))&&(currentCharString.Equals(twicePreviousCharString)))
                {
                    letterIndex++;
                    if (letterIndex == 26) { letterIndex = 0; }
                    currentCharString = alphabet[letterIndex].ToString();
                }
                twicePreviousCharString = previousCharString;
                previousCharString = currentCharString;
                maskedPAN += currentCharString;
            }
            maskedPAN += PAN.Substring(12, 4);
            return maskedPAN;
        }
    }
}