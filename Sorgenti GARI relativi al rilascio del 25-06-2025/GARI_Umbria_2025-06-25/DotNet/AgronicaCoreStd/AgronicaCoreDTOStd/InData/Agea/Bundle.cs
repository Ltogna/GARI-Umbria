using System;

namespace AgronicaCoreDTOStd.InData.Agea
{
    public class Bundle
    {
        public string cuaa { get; set; }

        public int campaignYear { get; set; }

        public string creationUser { get; set; }

        public string farmDescription { get; set; }

        /// <summary>
        /// Contiene il Supply in formato JSON
        /// </summary>
        public string data { get; set; }
    }
}
