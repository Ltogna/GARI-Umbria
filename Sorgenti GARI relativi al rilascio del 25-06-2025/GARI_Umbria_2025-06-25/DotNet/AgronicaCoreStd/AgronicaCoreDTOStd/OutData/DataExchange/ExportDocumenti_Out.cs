using AgronicaCoreModelsSTD.anagrafiche;
using System;
using System.Collections.Generic;
using System.Text;

namespace OutData.DataExchange
{
    public class ExportDocumenti_Out
    {
        public int Id_Documento { get; set; }
        public int Id_Tipologia { get; set; }
        public string Descrizione { get; set; }
        public DateTime Data_Scadenza { get; set; }
        public string FileName { get; set; }
        public byte[] FileByte { get; set; }
        public bool Cancellato { get; set; }
    }

}
