using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgronicaCoreAPI.models.entities
{
    public class IndiciMaturitaSpecieVegetaliEntity
    {
        public int indiceMaturitaCod;
        public int specieCod;
        public int REG_COD;
        public string classe;
        public DateTime? dataAggiornamento;
        public bool raccolta;
    }
}
