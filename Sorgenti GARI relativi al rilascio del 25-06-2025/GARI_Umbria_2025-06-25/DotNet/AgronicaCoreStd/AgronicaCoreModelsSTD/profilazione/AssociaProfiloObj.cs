using System;
using System.Collections.Generic;
using System.Text;

namespace AgronicaCoreModelsSTD.profilazione
{
    public class AssociaProfiloObj
    {
        public List<UtentePermessi> Utenti { get; set; }
        public TipologiaUtente Profilo { get; set; }
        public bool AssociaImpostazioni { get; set; }
    }

    public  class CopyProfileObj
    {
        public TipologiaUtente Original {get; set;}
        public TipologiaUtente CopyTemplate { get; set; }
        public bool AlsoCopySettings { get; set; } = false;
    }
}
