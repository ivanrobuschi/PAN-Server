using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PANserver
{
    public class PANArchiveManager : IPANArchiveManager
    {

        PANserverEntities _db;
        public PANArchiveManager(PANserverEntities PANdb)
        {
            _db = PANdb;
        }

        public string SearchPAN(string mask)
        {
            PANs PANdata = _db.PANs.SingleOrDefault(x => x.MaskedPAN == mask);
            if (PANdata != null)
            {
                return PANdata.PAN.Trim();
            }

            return null;
        }

        public string SearchMask(string PAN)
        {
            PANs PANdata = _db.PANs.SingleOrDefault(x => x.PAN == PAN);
            if (PANdata != null)
            {
                return PANdata.MaskedPAN.Trim();
            }

            return null;
        }

        public void AddPanAndMask(string PAN, string mask)
        {
            PANs NewRecord = new PANs();
            if ((SearchMask(PAN) == null) || (SearchPAN(mask) == null))
            {
                NewRecord.PAN = PAN;
                NewRecord.MaskedPAN = mask;
                _db.PANs.Add(NewRecord);
                _db.SaveChanges();
            }
        }

        //RemovePanAndMask method to add
    }
}
