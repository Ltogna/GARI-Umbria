using AgronicaCoreModelsSTD.attivita;
using AgronicaCoreModelsSTD.attivita.dettagli;

namespace AgronicaCoreDTOStd.InData.Agenda
{
    public class GiacenzeXProdotto
    { 

        public Lavorazione Operazione { get; set; }

        public int Categoria_Magazzino { get; set; }

        public DettaglioTrattamento dettaglioTrattamento { get; set; }

        public DettaglioFertilizzazione dettaglioFertilizzazione { get; set; }

        public DettaglioSemina dettaglioSemina { get; set; }
    }
}