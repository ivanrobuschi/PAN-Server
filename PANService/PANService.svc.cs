using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace PANService
{
    // NOTA: è possibile utilizzare il comando "Rinomina" del menu "Refactoring" per modificare il nome di classe "Service1" nel codice, nel file svc e nel file di configurazione contemporaneamente.
    // NOTA: per avviare il client di prova WCF per testare il servizio, selezionare Service1.svc o Service1.svc.cs in Esplora soluzioni e avviare il debug.
    public class PANService : IPANService
    {
        public string GetMask(string PAN)
        {
            var server = new PANserver.PANserver(new PANserver.PANArchiveManager(new PANserver.PANserverEntities()));
            string mask = server.GetMask(PAN);
            return mask;
        }

        public string GetPAN(string mask)
        {
            var server = new PANserver.PANserver(new PANserver.PANArchiveManager(new PANserver.PANserverEntities()));
            string PAN = server.GetPAN(mask);
            return PAN;
        }
    }
}
