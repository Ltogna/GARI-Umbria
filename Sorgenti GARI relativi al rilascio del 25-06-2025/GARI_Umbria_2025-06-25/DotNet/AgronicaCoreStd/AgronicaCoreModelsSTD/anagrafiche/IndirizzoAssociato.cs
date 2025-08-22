using System;
using System.Collections.Generic;
using System.Text;

namespace AgronicaCoreModelsSTD.anagrafiche
{
    public class IndirizzoAssociato
    {

        public Indirizzo indirizzo { get; set; }

        public int tipo_Indirizzo { get; set; }
        public bool flag_cancellazione { get; set; }

        public IndirizzoAssociato()
        {
            flag_cancellazione = false;
        }

    }
}
