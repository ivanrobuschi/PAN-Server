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
        public string invalidPANerrorMSG = "PAN invalido";
        public string invalidMaskErrorMSG = "PAN mascherato invalido";
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

        

        public string GetPAN(string mask, string fileName)
        {
            var archive = new PANArchiveManager();
            if (mask.Length != 16)
            {
                return invalidMaskErrorMSG;
            }
            else
            {
                archive.CreateLists(fileName, out panList, out maskList);
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
            var archive = new PANArchiveManager();
            var maskGen = new MaskGenerator();
            if (!AcceptPAN(PAN))
            {
                return invalidPANerrorMSG;
            }
            else
            {
                archive.CreateLists(fileName, out panList, out maskList);
                string mask;
                int index = panList.IndexOf(PAN);
                if (index < 0)
                {
                    mask = maskGen.CreateMask(PAN);
                    panList.Add(PAN);
                    maskList.Add(mask);
                    archive.SaveLists(fileName, panList, maskList);
                }
                else
                {
                    mask = maskList[index];
                }
                return mask;
            }
        }
    }
}