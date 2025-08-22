using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgronicaCoreAPI.models.entities
{
    public class EsercizioEntity
    {
        public int codice;
        public string descrizione;
        public DateTime inizioValidita;
        public DateTime fineValidita;

        public int impiantoCod;
        public int appezzamentoCod;
        public int centroAziendaleCod;
        public string partitaIva;

        public double resaPrevista;
    }
}
