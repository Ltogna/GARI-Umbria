using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgronicaCoreAPI.models.entities
{
    public class RisorseUmaneEntity
    {
        public int codice;
        public DateTime validitaFrom;
        public DateTime validitaTo;
        public string settore;
        public string attivita;
        public int rapportoContabileCod;
        public string codiceContatto;
        public string partitaIvaContatto;
    }
}
