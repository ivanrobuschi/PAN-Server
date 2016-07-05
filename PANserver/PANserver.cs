using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace PANserver
{
    public class PANserver
    {
        public List<string> panList = new List<string>(); //To be removed
        public List<string> maskList = new List<string>(); //To be removed

        IPANArchiveManager _panArchiveManager;
        IMaskGenerator _maskGen;
        public string invalidPANerrorMSG = "PAN invalido";
        public string invalidMaskErrorMSG = "PAN mascherato invalido";

        public PANserver(IPANArchiveManager panArchiveManager, IMaskGenerator maskGen)
        {
            _panArchiveManager = panArchiveManager;
            _maskGen = maskGen;
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

        //public bool GivenPANAlredyExists(string inputPAN)
        //{
        //    var reader = new StreamReader(fileName);
        //    bool exists = false;

        //    while ((!reader.EndOfStream)&&(exists == false))
        //    {
        //        string currentPAN = reader.ReadLine();
        //        exists = (currentPAN.Substring(0, 16) == inputPAN);
        //    }
        //    reader.Close();
        //    return exists;
        //}

        

        public string GetPAN(string mask)
        {
            if (mask.Length != 16)
            {
                return invalidMaskErrorMSG;
            }
            else
            {
                return _panArchiveManager.SearchPAN(mask);
            }
        }

        public string GetMask(string PAN)
        {
            if (!AcceptPAN(PAN))
            {
                return invalidPANerrorMSG;
            }
            else
            {
                string mask = _panArchiveManager.SearchMask(PAN);
                if (mask == null)
                {
                    mask = _maskGen.CreateMask(PAN);
                    _panArchiveManager.AddPanAndMask(PAN, mask);
                }
                return mask;
            }
        }
    }
}