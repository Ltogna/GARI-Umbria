using AgronicaCoreModelsSTD.anagrafiche;
using System;

namespace AgronicaCoreDTOStd.InData.Zoo
{
    public class LeggiPrescrizioni
    {
        public LeggiPrescrizioni()
        {
            Piva = "";
            Sa_Cod = 0;
            Sta_Num = 0;
            Tipo_Cod = 0;
            validita = new IntervalloTemporale();
        }
        public int? Ricetta_Cod { get; set; }

        public string Piva { get; set; }
        
        public int? Sa_Cod { get; set; }

        public int? Sta_Num { get; set; }

        /// <summary>
        /// TODO
        /// Tipo di prescrizione da enum_TipoPrescrizione
        /// </summary>
        public int? Tipo_Cod { get; set; }

        public IntervalloTemporale validita { get; set; }

        public LeggiPrescrizioni(int ricettaCod)
        {
            Ricetta_Cod = ricettaCod;
        }

        public LeggiPrescrizioni(int ricettaCod, int tipoCod)
        {
            Ricetta_Cod = ricettaCod;
            Tipo_Cod = tipoCod;
        }
    }
}
