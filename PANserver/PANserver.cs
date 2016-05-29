using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace PANserver
{
    public class PANserver
    {
        public List<string> panList = new List<string>();
        public List<string> maskList = new List<string>();
        string defaultFileName = @"C:\Temp\PANs.txt";
        //string defaultFileName = @"C:\Temp\PANsTest.txt";
        string invalidPANerrorMSG = "PAN invalido";
        string invalidMaskErrorMSG = "PAN mascherato invalido";
        //string PANDoesntExistErrorMSG = "Il PAN dato non esiste";

        public PANserver()
        {
            defaultFileName = AppDomain.CurrentDomain.BaseDirectory + "\\PANsTest.txt";
        }

        public bool AcceptPAN(string inputPAN)
        {
            bool validPAN = true;
            long parsedPan;
            validPAN = long.TryParse(inputPAN, out parsedPan);
            if ((inputPAN.Length!=16)||(validPAN == false))
            {
                validPAN = false;
            }
            return validPAN;
        }

        public bool GivenPANAlredyExists(string inputPAN, string fileName)
        {
            var reader = new StreamReader(fileName);
            bool exists = false;

            while ((!reader.EndOfStream)&&(exists == false))
            {
                string currentPAN = reader.ReadLine();
                exists = (currentPAN.Substring(0, 16) == inputPAN);
            }
            reader.Close();
            return exists;
        }

        public void CreateLists(string fileName)
        {
            var reader = new StreamReader(fileName);

            panList.Clear();
            maskList.Clear();

            while ((!reader.EndOfStream))
            {
                string currentRow= reader.ReadLine();
                panList.Add(currentRow.Substring(0, 16));
                maskList.Add(currentRow.Substring(17, 16));
            }
            reader.Close();
        }

        public string GetPAN(string mask, string fileName)
        {
            if (mask.Length != 16)
            {
                return invalidMaskErrorMSG;
            }
            else
            {
                CreateLists(fileName);
                int index = maskList.IndexOf(mask);
                if (index < 0)
                {
                    return "";
                }
                else
                {
                    return panList[index];
                }
            }
        }

        public string GetMask(string PAN, string fileName)
        {
            if (!AcceptPAN(PAN))
            {
                return invalidPANerrorMSG;
            }
            else
            {
                CreateLists(fileName);
                string mask;
                int index = panList.IndexOf(PAN);
                if (index < 0)
                {
                    mask = CreateMask(PAN);
                    SaveLists(fileName);
                }
                else
                {
                    mask = maskList[index];
                }
                return mask;
            }
        }

        public string CreateMask(string PAN)
        {
            string maskedPAN = PAN.Substring(0, 6);
            const string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string previousCharString = "";
            var random = new Random();
            for(int i = 6; i <= 11; i++)
            {
                int letterIndex = random.Next(26);
                string currentCharString = alphabet[letterIndex].ToString();
                if (letterIndex.Equals(previousCharString))
                {
                    currentCharString = alphabet[letterIndex + 1].ToString();
                }
                previousCharString = currentCharString;
                maskedPAN += currentCharString;
            }
            maskedPAN += PAN.Substring(12, 4);
            panList.Add(PAN);
            maskList.Add(maskedPAN);
            return maskedPAN;
        }

        public void SaveLists(string fileName)
        {
            var writer = new StreamWriter(fileName);

            for (int i = 0; i < panList.Count(); i++)
            {
                string fileLine = panList[i] + " " + maskList[i];
                writer.WriteLine(fileLine);
            }
            writer.Close();
        }
    }
}