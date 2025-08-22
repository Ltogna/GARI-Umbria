using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgronicaCoreAPI.models.entities
{
    public class ImpiantoEntity
    {
        public int codice;
        public int appezzamentoCod;
        public int centroAziendaleCod;
        public string partitaIva;
        public string descrizione;
        public string utilizzoTerrenoClassType;
        public int utilizzoTerrenoCod;
        public int gruppoFinalitaCod;
        public double superficie;
        public string cartografia;
        public string StaticMapBase64String;
        public DateTime inizioValidita;
        public DateTime fineValidita;
        public bool coverCrops;
    }
}
