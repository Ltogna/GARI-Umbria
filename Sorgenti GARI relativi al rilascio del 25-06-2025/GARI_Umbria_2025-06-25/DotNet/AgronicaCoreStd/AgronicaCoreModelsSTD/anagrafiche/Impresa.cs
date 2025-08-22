using AgronicaCoreModelsSTD.analisi;
using AgronicaCoreModelsSTD.baseClass;
using AgronicaCoreModelsSTD.metaschema;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AgronicaCoreModelsSTD.anagrafiche
{
    public class RifImpresa
    {
        public string partitaIva { get; set; }

        // Costruttore vuoto per consentire deserializzazioni della classe
        public RifImpresa()
        {
            partitaIva = "";
        }

        public RifImpresa(string _piva)
        {
            partitaIva = _piva;
        }
    }

    public class Impresa 
    {
        public string partitaIva { get; set; }
        /// <summary>
        /// Codice Univoco Azienda Agricola (come da standard AGEA)
        /// </summary>
        public string CUAA { get; set; }
        public string ragioneSociale { get; set; }

        public IntervalloTemporale validita { get; set; }

        public FormeGiuridiche forma_Giuridica { get; set; }

        public int tipo_Impresa { get; set; }

        public List<CentroAziendale> centriAziendali { get; set; }

        public List<CodiciAnagrafeValori> codici { get; set; }

        public List<ImpresaPadre> impresaPadre { get; set; }

        public List<IndirizzoAssociato> indirizzi { get; set; }

        public Contatto tecnicoReferente { get; set; }        

        public RisorseUmane organismo_di_Controllo { get; set; }        

        public List<Contatto> contatti { get; set; }

        /// <summary>
        /// Rappresenta il contatto dell'impresa sotto l'impresa superuser
        /// </summary>
        public Contatto contatto_superuser { get; set; }
        public List<BaseCodeDescr> certificazione { get; set; }
        public BaseCodeDescr gruppoRaccolta { get; set; }
        public bool flag_cancellazione { get; set; }

        // passato dall'app per evitare duplicati
        public string guid { get; set; }

        public BaseCodiceDescr disciplinareAziendalePredefinito { get; set; }

        public Impresa()
        {
            flag_cancellazione = false;
        }

        public RifImpresa getRiferimento()
        {
            return new RifImpresa
            (
                this.partitaIva
            );
        }

    }

}
