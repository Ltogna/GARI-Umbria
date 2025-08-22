using AgronicaCoreModelsSTD.attivita.risorse;
using AgronicaCoreModelsSTD.metaschema.avversita;
using AgronicaCoreModelsSTD.metaschema;
using System;
using System.Collections.Generic;
using System.Text;
using AgronicaCoreModelsSTD.anagrafiche;
using AgronicaCoreModelsSTD.baseClass;
using Newtonsoft.Json;

namespace AgronicaCoreModelsSTD.attivita.dettagli
{
    public class DettaglioRegistroSomministrazioni : RisorsaProdotto
    {
        /// <summary>
        /// Numero somministrazione 
        /// </summary>
        public string codice { get; set; }

        /// <summary>
        /// Numero trattamento
        /// </summary>
        public string numTrattamento { get; set; }

        /// <summary>
        /// Movimenti_Dettagli.Pro_Cod da Farmaci.AIC
        /// </summary>
        public string codiceAIC { get; set; }

        /// <summary>
        /// Mov_Dettaglio_Tecnico.av_cod, Mov_Dettaglio_Tecnico.av_gru
        /// </summary>
        public AvversitaGruppo avversitaGruppo { get; set; }

        /// <summary>
        /// Movimenti_dettagli.tempocarenza
        /// </summary>
        public TempiSospensione[] sospensione { get; set; }

        /// <summary>
        /// Data inizio e fine del trattamento
        /// </summary>
        public IntervalloTemporale validita { get; set; }

        /// <summary>
        /// Data della prescrizione
        /// </summary>
        public DateTime dataPrescrizione { get; set; }

        /// <summary>
        /// Durata del trattamento in giorni
        /// </summary>
        public int durataTrattamento { get; set; }

        /// <summary>
        /// Numero del registro di scorta (da VetInfo)
        /// </summary>
        public string regSco_Numero { get; set; }

        public DettaglioRegistroSomministrazioni()
        {
            classType = costanti.ClassType.DettaglioRegistroSomministrazioni;
        }

        public new DettaglioRegistroSomministrazioni Clona()
        {
            try
            {

                string output = JsonConvert.SerializeObject(this);
                DettaglioRegistroSomministrazioni deserializedObject = JsonConvert.DeserializeObject<DettaglioRegistroSomministrazioni>(output);
                return deserializedObject;
            }
            catch (Exception ex)
            {
                throw new Exception("DettaglioRegistroSomministrazioni.Clona: " + ex.Message);
            }

        }

    }

    public class TempiSospensione
    {
        public int tempoSospensione { get; set; }
        public BaseCodeDescr Alimento { get; set; }
    }

}
