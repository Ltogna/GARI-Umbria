using AgronicaCoreModelsSTD.anagrafiche;
using System;
using System.Collections.Generic;
using System.Text;

namespace AgronicaCoreModelsSTD.Indici_rischio
{
    public class IndiciProduttivitaAI
    {
        public int id { get; set; }
        public decimal Produttivita { get; set; }
        public decimal plv { get; set; }
        public decimal IndiceErosione { get; set; }
        public decimal IndiceCO2 { get; set; }
        public decimal IndiceRischioMeteoAggregato { get; set; }
        public decimal IndiceRischioGelata { get; set; }
        public decimal IndiceRischioVentoForte { get; set; }
        public decimal IndiceRischioSiccita { get; set; }
        public decimal IndiceRischioGrandine { get; set; }
        public decimal IndiceRischioAllagamento { get; set; }
        public IntervalloTemporale validita { get; set; }
        public bool flag_cancellazione { get; set; }

        public IndiciProduttivitaAI()
        {
            validita = new IntervalloTemporale();
            flag_cancellazione = false;
        }
    }

    public class Reg_Impianti_XIndiciProduttivitaAI
    {        
        public Impianto.PK impiantoPK { get; set; }
        public List<IndiciProduttivitaAI> indici { get; set; }
        public IntervalloTemporale validita { get; set; }
        public bool flag_cancellazione { get; set; }

        public Reg_Impianti_XIndiciProduttivitaAI()
        {
            indici = new List<IndiciProduttivitaAI>();
            validita = new IntervalloTemporale();
            flag_cancellazione = false;
        }

    }
}
