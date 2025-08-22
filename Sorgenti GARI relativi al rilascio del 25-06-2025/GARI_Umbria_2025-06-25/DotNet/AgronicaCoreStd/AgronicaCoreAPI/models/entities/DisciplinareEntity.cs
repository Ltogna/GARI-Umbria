using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgronicaCoreAPI.models.entities
{
    public class DisciplinareEntity
    {
        public string codice;
        public string descrizione;
        public int regConcimazioneCod;
        public string regConcimazioneDesc;
        public int disciplinarePubblicoPrivato;
        public int flagProtetto;
        public int idTr;
        public DateTime inizioValidita;
        public DateTime fineValidita;
    }
}
