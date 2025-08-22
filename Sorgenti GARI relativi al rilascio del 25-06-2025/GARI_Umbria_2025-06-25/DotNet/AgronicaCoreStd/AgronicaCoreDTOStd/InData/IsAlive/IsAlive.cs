using System;
using System.Collections.Generic;
using System.Text;

namespace AgronicaCoreDTOStd.InData.IsAlive
{
    /// <summary>
    /// Classe params input per IsAlive()
    /// </summary>
    public class CheckIsAliveIN
    {
        /// <summary>
        /// Elenco di siti di cui verificare la raggiungibilità
        /// </summary>
        public string[] sites { get; set; }
        /// <summary>
        /// Se true, allega anche la versione dei siti richiesti
        /// </summary>
        public bool showVersion { get; set; }
        /// <summary>
        /// Se true, allega anche l'url chiamato
        /// </summary>
        public bool showUrl { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ReachableSiteIN
    {
        /// <summary>
        /// Stringa identificativa del sito (passata come param)
        /// </summary>
        public string key { get; set; }
        /// <summary>
        /// Link del sito
        /// </summary>
        public string value { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public ReachableSiteIN(string key, string value)
        {
            this.key = key;
            this.value = value;
        }
    }

}
