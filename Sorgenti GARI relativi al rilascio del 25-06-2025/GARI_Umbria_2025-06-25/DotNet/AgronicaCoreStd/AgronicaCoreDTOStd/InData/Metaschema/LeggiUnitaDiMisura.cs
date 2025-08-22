using AgronicaCoreModelsSTD.anagrafiche;
using AgronicaCoreModelsSTD.attivita;
using AgronicaCoreModelsSTD.metaschema;
using AgronicaCoreModelsSTD.attivita.risorse;
using AgronicaCoreModelsSTD.metaschema.utilizzi;
using AgronicaCoreModelsSTD.metaschema.avversita;
using System;
using AgronicaCoreModelsSTD.attivita.dettagli;

namespace AgronicaCoreDTOStd.InData.Metaschema
{
    public class LeggiUnitaDiMisura
    {
        public Lavorazione lavorazione { get; set; }

        public Attivita.Tipo_Attivita tipo_Attivita { get; set; }

        public Attivita.Tipo_Ricetta tipo_Ricetta { get; set; }

        public DettaglioTrattamento dettaglioTrattamento { get; set; }

        public DettaglioFertilizzazione dettaglioFertilizzazione { get; set; }

        public DettaglioSemina dettaglioSemina { get; set; }

        public DoseEtichetta doseEtichetta { get; set; }

        public UnitaDiMisura unitaDiMisura { get; set; }
    }
}