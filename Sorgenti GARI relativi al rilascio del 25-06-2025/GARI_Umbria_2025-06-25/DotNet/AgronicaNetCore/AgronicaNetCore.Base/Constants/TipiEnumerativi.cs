using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgronicaNetCore.Base.Constants
{
    public class TipiEnumerativi
    {
        #region Profilazione
        public enum Enum_Impostazioni_Utenti
        {

            // =====================================================================================
            // DOCUMENTAZIONE: \\rubino2\DOCUMENTAZIONE\GIAS --- Utenti\Utenti_Impostazioni.docx
            // =====================================================================================

            // -------------------------------------------------------------------------------------
            // Impostazioni GENERICHE
            // -------------------------------------------------------------------------------------

            // La visibilità di queste impostazioni non viene indicata rigidamente nel nome,
            // ma documentata tramite summary.

            /// <summary>
            ///         ''' Visibilità: CENTRO, IMPRESA, SUPERUSER
            ///         ''' </summary>
            ScriviAppezza_RipartoCatastoDaEntita = 1079,

            /// <summary>
            ///         ''' Visibilità: IMPRESA
            ///         ''' </summary>
            DocContabili_GestioneWorkFlow = 1063,

            /// <summary>
            ///         ''' Visibilità: IMPRESA
            ///         ''' </summary>
            Default_GruppoMerce_CategoriaProdotto = 1064,

            /// <summary>
            ///         ''' Visibilità: IMPRESA e SUPERUSER
            ///         ''' </summary>
            Raccolta_Con_Carico_Magazzino = 1065,


            /// <summary>
            ///         ''' Visibilità: SUPERUSER
            ///         ''' </summary>
            Degrado_Visibilita_Obbligatorieta = 1066,

            /// <summary>
            ///         ''' Visibilità: IMPRESA e SUPERUSER
            ///         ''' </summary>
            Conferimento_Da_Raccolta = 1068,

            /// <summary>
            ///         ''' Visibilità: IMPRESA e SUPERUSER
            ///         ''' </summary>
            StampaDocAcquisto_CodiceSDI = 1069,

            /// <summary>
            ///         ''' Visibilità: IMPRESA e SUPERUSER
            ///         ''' </summary>
            Indirizzi_Obbligatorieta_Valorizzazione_Gerarchia_Geografica = 1070,

            /// <summary>
            ///         ''' Visibilità: Utente
            ///         ''' </summary>
            StampaCampagna_Default_Vedi_AvversitaQta = 1071,

            /// <summary>
            ///         ''' Visibilità: IMPRESA e SUPERUSER
            ///         ''' </summary>
            Mostra_DDT_MenuAgenda = 1072,

            /// <summary>
            ///         ''' Visibilità: IMPRESA e SUPERUSER
            ///         ''' </summary>
            GruppoMerce_Controllo = 1073,

            /// <summary>
            ///         ''' Visibilità: IMPRESA e SUPERUSER
            ///         ''' </summary>
            CdC_Wbs_Controllo = 1074,

            /// <summary>
            ///         ''' Visibilità: IMPRESA e SUPERUSER Specifica per Iniziative Biometano
            ///         ''' </summary>
            IB_RisorseUmane_SettoreDes_Controllo = 1075,

            /// <summary>
            ///         ''' Visibilità: IMPRESA e SUPERUSER
            ///         ''' </summary>
            Collega_Solo_Ordini_Inviati = 1076,

            /// <summary>
            ///         ''' Visibilità: IMPRESA e SUPERUSER Specifica per Iniziative Biometano
            ///         ''' </summary>
            IB_Funzione_Controlli_PreInvio = 1077,

            /// <summary>
            ///         ''' Visibilità: IMPRESA e SUPERUSER
            ///         ''' </summary>
            MenuAgenda_Visibilita_Doc_Contabili = 1078,

            /// <summary>
            ///         ''' Visibilità: SUPERUSER
            ///         ''' </summary>
            Workflow_GruppiUtente_PermessiStato = 1080,

            /// <summary>
            ///         ''' Visibilità: IMPRESA e SUPERUSER
            ///         ''' </summary>
            Imputazione_Impianti_Raccolta_Conf = 1085,

            /// <summary>
            ///         ''' Visibilità: IMPRESA e SUPERUSER
            ///         ''' </summary>
            Scarico_Da_Raccolta = 1086,

            /// <summary>
            ///         ''' Visibilità: IMPRESA e SUPERUSER
            ///         ''' </summary>
            Aggiorna_Peso_Raccolta_Da_Conf = 1087,

            /// <summary>
            ///         ''' Visibilità: IMPRESA e SUPERUSER
            ///         ''' </summary>
            StampaArticolo62ElemCod = 1092,

            /// <summary>
            ///         ''' Visibilità: IMPRESA e SUPERUSER
            ///         ''' </summary>
            ImpedisciCreazioneCarichiMultiriga = 1094,

            // -------------------------------------------------------------------------------------
            // Impostazioni SUPERUSER
            // -------------------------------------------------------------------------------------

            SUPERUSER_COD_DISTRIBUZIONE_ACQUA = 1,
            SUPERUSER_COD_IMPIANTO_DISTINTA = 2,
            SUPERUSER_COD_BLOCCO_PARTICELLE = 10,
            SUPERUSER_COD_ANALISI_COSTI = 12,
            SUPERUSER_Blocca_Impianti_Smart = 73,
            SUPERUSER_Smart_NuovoImpianto_OrganismoReferente = 74,
            SUPERUSER_Smart_NuovoImpianto_DestinazioneUso = 75,
            SUPERUSER_Smart_NuovaImpresa_ImpresaPadre = 76,
            SUPERUSER_Smart_NuovoImpianto_DefaultOrganismoReferente = 77,
            SUPERUSER_COD_FILE_IMPORTAZIONE_DATI = 500,
            SUPERUSER_COD_FILE_ESPORTAZIONE_DATI = 501,
            SUPERUSER_COD_FILE_ESPORTAZIONE_DATI_2 = 502,
            SUPERUSER_COD_IP_BILANCIA = 600,
            SUPERUSER_COD_TARA_BILANCIA = 601,
            SUPERUSER_COD_FILTRO_MATERIE_PRIME = 666,
            SUPERUSER_COD_COOP_REFERENTE_OBBLIGATORIA = 670,
            SUPERUSER_COD_ORG_REFERENTE_OBBLIGATORIO = 670,
            SUPERUSER_COD_ORG_REFERENTE_DA_PADRE = 671,
            SUPERUSER_COD_IMPOSTAZIONE_IMBALLAGGI = 700,
            SUPERUSER_COD_IMPOSTAZIONE_CONTENITORI = 701,
            SUPERUSER_COD_CONTABILITA_MULTIPLA = 710,
            SUPERUSER_COD_IVA_DEFAULT = 712,
            SUPERUSER_COD_CAUSALE_TRASPORTO_DEFAULT = 713,
            SUPERUSER_COD_NOTE_DEFAULT_BOLLA_EMESSA = 714,
            SUPERUSER_COD_NOTE_DEFAULT_FATTURA_EMESSA = 715,
            SUPERUSER_COD_LISTINI_PRODUZIONE = 720,
            UTENTE_RapportoContabileDefault = 722,
            SUPERUSER_COD_MODALITA_TRASPORTO_DEFAULT = 723,
            SUPERUSER_COD_Aspetto_Beni_Default = 724,
            SuperUser_LayOut_Peso_DDT = 727,
            SuperUser_LayOut_Prezzo_DDT = 728,
            SuperUser_LayOut_Riscontrato_DDT = 729,
            SUPERUSER_COD_GESTIONE_AGENTI = 734,
            SUPERUSER_COD_GESTIONE_SEZIONALI = 746,
            SUPERUSER_COD_GEST_AUTO_CONSISTENZE_ENOLOGICHE = 750,
            SUPERUSER_COD_ZONA_VITICOLA = 751,
            SUPERUSER_COD_GESTIONE_REG_VINIFICAZIONE = 756,
            SuperUser_StampaCapacitaEffettivaVascaRegCantina = 757,
            SuperUser_StampaIndentificativoVascaRegCantina = 758,
            SUPERUSER_COD_VISUAL_CODARTICOLO_DOCUMENTI = 764,
            SuperUser_ContributoConai = 766,
            SUPERUSER_COD_PERMETTI_OPERAZIONI_SU_IMPIANTI_BLOCCATI = 767,
            SUPERUSER_COD_PERMETTI_MODIFICA_RICETTE_CON_OPERAZIONI_REGISTRATE = 768,
            SuperUser_NoteIntegrative1DDT = 770,
            SuperUser_NoteIntegrative2DDT = 771,
            SuperUser_NoteIntegrative3DDT = 772,
            SuperUser_Articolo62DDT = 773,
            SuperUser_NoteIntegrative1DDTCorrispettivi = 774,
            SuperUser_NoteIntegrative2DDTCorrispettivi = 775,
            SuperUser_NoteIntegrative3DDTCorrispettivi = 776,
            SuperUser_Articolo62DDTCorrispettivi = 777,
            SuperUser_NoteIntegrative1Ordini = 778,
            SuperUser_NoteIntegrative2Ordini = 779,
            SuperUser_NoteIntegrative3Ordini = 780,
            SuperUser_Articolo62Ordini = 781,
            SuperUser_NoteIntegrative1Fatture = 782,
            SuperUser_NoteIntegrative2Fatture = 783,
            SuperUser_NoteIntegrative3Fatture = 784,
            SuperUser_Articolo62Fatture = 785,
            SuperUser_GestioneVisualNumVascaRegImbott = 791,
            SUPERUSER_COD_TIPO_DOC_ACCETTAZIONE_DEFAULT = 795,
            SUPERUSER_COD_CHKCOGE_MANUALE_DEFAULT = 797,
            SuperUser_StampaLottoTrasformazioneRegCantina = 801,
            SUPERUSER_COD_GESTIONE_CAPOAREA = 807,
            SuperUser_ApriFiltroFattura = 810,
            SuperUser_ApriFiltroDDT = 811,
            SuperUser_StPersDDTAccetta = 812,
            SuperUser_StRifOrdine = 813,
            SUPERUSER_COD_ALGORITMO_COSTI_ACCESSORI = 814,
            SUPERUSER_COD_SQPNI_FILTROSPECIE = 815,
            SUPERUSER_COD_SQPNI_FILTROMACRO_USI = 816,
            SUPERUSER_COD_SQPNI_FILTRODESTINAZIONE_USO = 817,
            SUPERUSER_COD_PERSON_RILIEVO_FASI_FENOLOGICHE = 819,
            SUPERUSER_COD_PERSON_RILIEVO_AVVERSITA = 820,
            SUPERUSER_COD_PERSON_RILIEVO_ERBE_INFESTANTI = 821,
            SUPERUSER_COD_PERSON_RILIEVO_INDICI_MATURITA = 822,
            SUPERUSER_COD_PERSON_RILIEVO_INDICI_RESE_RACCOLTA = 832,
            SUPERUSER_COD_PERSON_RILIEVO_DANNI_ALLA_RACCOLTA = 833,
            SUPERUSER_COD_TOLLERANZA_PESO_GIACENZE_DISTINTA = 825,
            SUPERUSER_COD_NUOVA_MODALITA_ARROTONDAMENTO_LAN = 826,
            SUPERUSER_GiasAPP_NUOVA_RICETTA = 827,
            SUPERUSER_GiasAPP_NUOVO_INTERVENTO = 828,
            SUPERUSER_GiasAPP_INTERVENTI_DA_FARE = 829,
            SUPERUSER_GiasAPP_SCARICO_ORE = 830,
            SUPERUSER_UTILIZZO_ORARI_INIZIO_FINE = 831,
            SUPERUSER_COD_RICETTE_CREA_UNA_OPERAZIONE_PER_OGNI_IMPIANTO = 834,
            SUPERUSER_GiasAPP_MAX_AZIENDE = 835,
            SUPERUSER_ArrotondamentoIVA4Dec_DataAttivazione = 836,
            SUPERUSER_Consenti_ContattoCod_Duplicato = 838,
            SUPERUSER_PRODOTTOCOD_MAX_LENGTH = 839,
            SUPERUSER_ContattoCod_Max_Lenght = 840,
            SUPERUSER_StampaAlcolTotale = 841,
            SUPERUSER_ConsentiNumDocDuplicati = 842,
            SUPERUSER_JoinCacPivaSuperUser = 843,
            SUPERUSER_SoloLottiDisponibiliInOrdineVendita = 844,
            SUPERUSER_PrezzoNulloInListino = 845,
            SUPERUSER_TipoValorizzazioneCostiCdG = 846,
            SUPERUSER_Applica_Listini_Non_Associati = 847,
            SUPERUSER_FF_GEST_MATERIALE_VIVAISTICO = 848,
            SUPERUSER_DOCUMENTALE_SALVA_ALLEGATO_SU_DB = 849,
            SUPERUSER_GiasAPP_ENTRATAUSCITA = 850,
            SUPERUSER_GiasAPP_LAMIAPOSIZIONE = 851,
            SUPERUSER_GiasAPP_VISITE = 852,
            SUPERUSER_GiasAPP_FREQUENZARILIEVO_MINUTI = 853,
            SUPERUSER_GiasAPP_FREQUENZASINCRO_MINUTI = 854,
            SUPERUSER_Licenza_Giorni_Franchigia = 855,
            SUPERUSER_INTESTAZIONE_STAMPA_CENTROAZIENDALEPARTENZA = 877,
            SUPERUSER_GESTIONE_SPOSTAMENTO_ANIMALI = 878,
            SUPERUSER_Gestione_GHG = 872,
            SUPERUSER_EDIT_LOTTO = 990,
            SUPERUSER_OPERATORE_ACCETTAZIONE = 998,
            SUPERUSER_Consenti_ProdottoCod_Duplicato = 999,
            SuperUser_StampaLitriDDTFatture = 1050,
            SuperUser_CessionariAggiuntivi = 1051,
            SuperUser_RifDoc_EnteConsorzio = 1052,
            SuperUser_PesiColli_Riscontrati = 1053,
            SuperUser_TipoDestinazione_Default = 1054,
            SUPERUSER_LIVELLO_GESTIONE_CONTABILITA = 1058,
            SUPERUSER_ACCETTAZIONE_CON_GERARCHIA = 1059,
            SUPERUSER_DocContabili_SceltaImputazione = 1061,
            IMPRESA_DocContabili_Cod_Tipo_Imputazione = 1062,
            SUPERUSER_Creazione_Prodotti = 1081,
            SUPERUSER_Documentale_GestioneWorkFlow = 1082,
            SUPERUSER_Documentale_GestioneChecklist = 1083,
            SUPERUSER_NR_ORE_VISITA = 1084,
            SUPERUSER_ZOO_IN_VISITA = 1088,
            SUPERUSER_STATO_VISITA = 1089,
            GIS_PARAMETRI_SETUP = 1090,
            SUPERUSER_CARICO_ZOO_GRIGLIA = 1091,
            /// <summary>
            ///         ''' Visibilità: IMPRESA e SUPERUSER
            ///         ''' </summary>
            SUPERUSER_ELABORAZIONE_DSS_DA_IMPIANTI = 1093,
            SUPERUSER_ELABORAZIONE_DSS_FORECAST = 1095,
            SUPERUSER_COD_IMPORTAZIONE_AGREA = 3001,
            SUPERUSER_COD_IMPORTAZIONE_ANAGRAFEEMILIAROMAGNA = 3002,
            SUPERUSER_COD_IMPORTAZIONE_AVEPA = 3003,

            // -------------------------------------------------------------------------------------
            // Impostazioni UTENTE
            // -------------------------------------------------------------------------------------

            UTENTE_COD_FILTRO_GRUPPI_VEGETALI = 3,
            UTENTE_COD_FILTRO_VARIETA = 4,
            UTENTE_COD_DEFAULT_SINGOLE_GRUPPI_AVVERSITA = 5   // valore: 88=gruppi 77=singole
,
            UTENTE_COD_FILTRO_SPECIE_VEGETALI = 6,
            UTENTE_COD_FILTRO_LAVORAZIONI = 7    // aka. filtro operazioni
,
            UTENTE_COD_FILTRO_GRUPPI_OPERAZIONI = 8,
            UTENTE_COD_MAGAZZINO_RIFERIMENTO = 9,
            UTENTE_COD_DEFAULT_SINGOLE_GRUPPI_INFESTANTI = 11   // valore: 88=gruppi 77=singole
,
            UTENTE_COD_DEFAULT_DPI = 13   // valore: 0=nessun DPI 1=con DPI
,
            UTENTE_COD_DEFAULT_BLOCCO_SALVA_NOCONFORME = 14   // valore: 0=nessun blocco 1=blocco salvataggio
,
            UTENTE_COD_DEFAULT_QTA_PRODOTTO = 15   // valore: 88=totale 77=ettaro
,
            UTENTE_COD_DEFAULT_QTA_ACQUA = 16        // valore: 88=totale 77=ettaro
,
            UTENTE_COD_DEFAULT_GESTIONE_MAGAZZINO = 18     // 0=nessun magazzino  1=con magazzino
,
            UTENTE_COD_DEFAULT_FILTRO_PRODOTTI = 19     // 0 = nessuno - 1 = coltura - 2 = coltura/ avversità/ infestante
,
            UTENTE_COD_DEFAULT_FILTRO_PRODOTTI_RICETTE = 176     // 0 = nessuno - 1 = coltura - 2 = coltura/ avversità/ infestante
,
            UTENTE_COD_ColonneVisibili_TabellaImpianti_Semina = 24,
            UTENTE_COD_ColonneVisibili_TabellaImpiantiAgenda = 26,
            UTENTE_COD_ColonneVisibili_PAN_PianoColturale = 57,
            UTENTE_COD_FILTRO_STAMPA_RICETTA_NUMERO_RICETTA = 27,
            UTENTE_COD_FILTRO_STAMPA_RICETTA_MACCHINE = 28,
            UTENTE_COD_FILTRO_STAMPA_RICETTA_OPERATORI = 29,
            UTENTE_COD_FILTRO_STAMPA_RICETTA_TECNICO_AUTORIZZANTE = 30,
            UTENTE_COD_FILTRO_STAMPA_RICETTA_FIRMA_AGRICOLTORE = 31,
            UTENTE_COD_FILTRO_STAMPA_RICETTA_DATA_ULTIMA_MANUTENZIONE = 44,
            UTENTE_COD_FILTRO_STAMPA_SCHEDA_CAMPAGNA_ARROTONDA_ACQUA = 32,
            UTENTE_COD_FILTRO_STAMPA_SCHEDA_CAMPAGNA_MACCHINE = 33,
            UTENTE_COD_FILTRO_STAMPA_SCHEDA_CAMPAGNA_OPERATORI = 34,
            UTENTE_COD_FILTRO_STAMPA_SCHEDA_CAMPAGNA_TECNICO_AUTORIZZANTE = 35,
            UTENTE_COD_FILTRO_STAMPA_SCHEDA_CAMPAGNA_DATA_FIRMA = 36,
            UTENTE_COD_FILTRO_STAMPA_SCHEDA_CAMPAGNA_NUMERO_RICETTA = 42,
            UTENTE_COD_FILTRO_STAMPA_SCHEDA_CAMPAGNA_DATA_ULTIMA_MANUTENZIONE = 45,
            UTENTE_COD_FILTRO_STAMPA_SCHEDA_CAMPAGNA_FASE_EPOCA_ETICHETTA = 117,
            UTENTE_COD_FILTRO_STAMPA_SCHEDA_CAMPAGNA_FILTRA_FASCICOLO = 112,
            UTENTE_COD_FILTRO_STAMPA_SCHEDA_CAMPAGNA_VISUALCAMPO_SOLOFRONTESPIZIO = 177,
            UTENTE_COD_SCHEDA_CAMPAGNA_NUM_APPEZZAMENTO = 109,
            UTENTE_COD_FILTRO_STAMPA_SCHEDA_GLOBAL_ARROTONDA_ACQUA = 37,
            UTENTE_COD_FILTRO_STAMPA_SCHEDA_GLOBAL_MACCHINE = 38,
            UTENTE_COD_FILTRO_STAMPA_SCHEDA_GLOBAL_OPERATORI = 39,
            UTENTE_COD_FILTRO_STAMPA_SCHEDA_GLOBAL_TECNICO_AUTORIZZANTE = 40,
            UTENTE_COD_FILTRO_STAMPA_SCHEDA_GLOBAL_DATA_FIRMA = 41,
            UTENTE_COD_FILTRO_STAMPA_SCHEDA_GLOBAL_NUMERO_RICETTA = 43,
            UTENTE_COD_FILTRO_STAMPA_SCHEDA_GLOBAL_DATA_ULTIMA_MANUTENZIONE = 46,
            UTENTE_COD_FILTRO_STAMPA_SCHEDA_GLOBAL_FASE_EPOCA_ETICHETTA = 118,

            // blocchi di etichetta inserimento
            UTENTE_COD_BLOCCA_DOSEMASSIMA_ETICHETTA = 47,
            UTENTE_COD_BLOCCA_DOSEMINIMA_ETICHETTA = 48,
            UTENTE_COD_BLOCCA_ACQUAMASSIMA_ETICHETTA = 49,
            UTENTE_COD_BLOCCA_ACQUAMINIMA_ETICHETTA = 50,
            UTENTE_COD_BLOCCA_NUMERO_MASSIMO_TRATTAMENTI_ETICHETTA = 51,
            UTENTE_COD_BLOCCA_FINO_FIORITURA_ETICHETTA = 52,
            UTENTE_COD_BLOCCA_CARENZA_NON_RISPETTATA = 53,
            UTENTE_COD_BLOCCA_BUFFERZONE_ETICHETTA = 169,

            // Livello di Controllo del DPI --> 1 livello base 2 livello avanzato
            UTENTE_COD_LIVELLO_CHK_DPI = 54,
            UTENTE_COD_TUTTI_I_CENTRI = 55,
            UTENTE_COD_CHILI_LITRI = 56,
            SUPERUSER_DEFAULT_NOTE_CAMPIONI = 57,
            UTENTE_MODALITA_NERO = 58,
            UTENTE_GESTIONE_MAGAZZINO_2 = 59    // 0=nessuna gestione  1= carica magazzini super user 2= carica magazzini impresa padre
,
            UTENTE_OPERAZIONI_QDC_PREFERITE = 60 // Lav_cod separati da |
,
            UTENTE_COD_ColonneVisibili_Planning = 61,
            UTENTE_COD_ColParticelleVisibili_Planning = 91,
            SUPERUSER_Codice_Campione_Progressivo_Inizio_Anno = 62,
            SUPERUSER_Label_Codice_Campione = 63,
            UTENTE_NumAppezza_Progr_Modalita = 64,
            UTENTE_MultiModificaImpianti_FiltroProprieta = 65,
            UTENTE_COD_ColonneVisibili_PDC_da_campagna = 66,
            UTENTE_COD_ColonneVisibili_PDC_acquisti = 67,
            UTENTE_COD_DEFAULT_DPI_PREDEFINITO = 68   // valore: es 34/1 emilia romagna 2012
,
            UTENTE_COD_DEFAULT_RiferimentoRicetteOperazioni = 69 // 0= non salva il riferimento in ricettaxagenda, 1 o non impostato= salva il riferimento
,
            UTENTE_StampaPDF_RicFiscali_ConSenzaPreview = 70 // 0= con preview,1=senza preview
,
            UTENTE_OPERAZIONI_TIPI_GRUPPI_OPERAZIONI_VISIBILI_MENU_AGENDA = 71 // Tipo gruppo separati da | (E,P,C,Z,E6,E10) tipo E separato in E6, E!= per magazzino e contabili))
,
            UTENTE_Nome_Stampante_Ricevuta_A5 = 72 // Tipo gruppo separati da | (E,P,C,Z,E6,E10) tipo E separato in E6, E!= per magazzino e contabili))
,
            UTENTE_COD_FILTRO_STAMPA_RICETTA_ARROTONDA_ACQUA = 78,
            UTENTE_AlberoAnagrafica_visualizzaRiferimentoAlfanumericoImpianto = 79,
            UTENTE_AlberoAnagrafica_ordinaDataUltimoImpianto = 80,
            UTENTE_COD_BLOCCA_SE_SENZA_MAGAZZINO = 81,
            UTENTE_COD_BLOCCA_SE_SUPERA_GIACENZE = 82,
            UTENTE_COD_BLOCCA_SE_SENZA_DISCIPLINARE = 168,
            UTENTE_COD_BLOCCA_SE_DATA_FATTURA_SUPERA_ALLEGATI = 83,
            SUPERUSER_Label_Codice_Analisi = 84,
            SUPERUSER_Label_Codice_Griglia = 85,
            UTENTE_COD_ColonneVisibili_PDC_AltreAnalisi = 86,
            SUPERUSER_Stampa_Rapida_WorkFlow = 87,
            UTENTE_LINK_PREFERITE = 88,
            UTENTE_LINK_PREFERITE_MENUBS2017 = 171,
            UTENTE_COD_AZIENDA_PREDEFINITA_ALL_AVVIO = 182,
            SUPERUSER_Codice_Campione_Zani = 92,
            UTENTE_LINK_PREFERITE_MENU_ONLINE = 119,

            // CATEGORIE MAGAZZINO
            UTENTE_COD_CATEGORIAMAGAZZINO_DEFAULT = 711,
            UTENTE_COD_CATEGORIAMAGAZZINO_COSTI_DEFAULT = 721,
            UTENTE_COD_FILTRO_CATEGORIAMAGAZZINO = 89   // Elem_cod separati da |
,
            UTENTE_COD_FILTRO_CATEGORIAMAGAZZINO_UDM_DEFAULT = 90   // Elem_cod_Udm_Cod separati da |
,
            UTENTE_COD_FILTRO_CATEGORIAMAGAZZINO_GIACENZE = 180   // Elem_cod_Valore separati da |
,
            UTENTE_COD_FILTRO_CATEGORIAMAGAZZINO_LOTTI = 181   // Elem_cod_Valore separati da |
,
            UTENTE_COD_FILTRO_CATEGORIAMAGAZZINO_GIACENZE_APP = 183   // Per APP Elem_cod_Valore separati da |
,
            UTENTE_COD_BLOCCA_SE_SUPERA_LIMITIMAS = 93  // Blocca se vengono superati i limiti di azoto, fosforo, potassio e magnesio dell'impianto
,
            UTENTE_COD_DEFAULT_ProdottiTossiciPatentinoMovimenti = 94 // 0=permetti 1=avvisa 2=blocca
,
            UTENTE_COD_BLOCCO_TARATURA_ATOMIZZATORE_SCADUTA = 212 // 0=permetti 1=avvisa 2=blocca
,
            UTENTE_COD_BLOCCA_MIX_POLVERULENTI_NONPOLVERULENTI = 213 // 0=no 1=sì
,
            UTENTE_COD_BLOCCA_MIX_POLVERULENTI_NONPOLVERULENTI_SALVATAGGIO = 214 // 0=no 1=sì
,
            UTENTE_COD_BLOCCA_INTERVALLOTRATTAMENTI_ETICHETTA = 95,
            UTENTE_Menu_Agenda_Selezione_Tipo_Operazioni = 96,
            UTENTE_Menu_Agenda_Selezione_GruppoOperazioni = 97,
            UTENTE_MENU_NUMERO_ULTIME_AZIENDE_SELEZIONATE_DA_VISUALIZZARE = 211,
            SUPERUSER_Codice_Campione_TERREMERSE = 98,
            UTENTE_COD_ColonneVisibili_Analisi_PDC = 99,

            // Nitrati Piano di concimazione
            UTENTE_Nitrati_PC_Analisi = 100     // Analisi_Entita_Cod separati da |
,
            UTENTE_STAMPE_PREFERITE = 101 // cod_report, enum_CodificaStampe, tabella StampeReport  separati da |
,
            UTENTE_Nitrati_RegolamentoPC_Default = 102,
            UTENTE_FILTRONE_PDC = 103,
            SUPERUSER_ComportamentoComboFF = 104,
            UTENTE_InizioFineAnnataAgraria = 105,
            UTENTE_Planning_Date = 106,
            UTENTE_Planning_NValidazioneNome = 107,
            UTENTE_Ribaltamento_CreaCampi = 108,
            UTENTE_COD_UTILIZZA_SUP_APP_AGENDA = 151,
            UTENTE_COD_RACCOLTA_TIPO = 110,
            UTENTE_COD_RACCOLTA_TIPOLOGIA_PRODOTTO = 111,
            UTENTE_TABACCO_EXPORT_ALL = 113,
            UTENTE_TABACCO_EXPORT_APPEZZA = 114,
            UTENTE_TABACCO_EXPORT_CORONA = 115,
            UTENTE_TABACCO_EXPORT_GRADO = 116,
            UTENTE_TABACCO_EXPORT_BUCHI = 121,
            UTENTE_Attiva_Configurazione_Pratica = 172,
            UTENTE_COD_BLOCCA_RACCOLTA_CARENZA_NON_RISPETTATA = 120,
            UTENTE_COD_BLOCCA_SEMINA_SE_SENZA_QTA = 122,

            // blocchi  etichetta/dpi salvataggio
            UTENTE_COD_DEFAULT_BLOCCO_INSERIMENTO_NONCONFORME = 125,
            UTENTE_COD_BLOCCA_DOSEMASSIMA_ETICHETTA_SALVATAGGIO = 126,
            UTENTE_COD_BLOCCA_DOSEMINIMA_ETICHETTA_SALVATAGGIO = 127,
            UTENTE_COD_BLOCCA_ACQUAMASSIMA_ETICHETTA_SALVATAGGIO = 128,
            UTENTE_COD_BLOCCA_ACQUAMINIMA_ETICHETTA_SALVATAGGIO = 129,
            UTENTE_COD_BLOCCA_NUMERO_MASSIMO_TRATTAMENTI_ETICHETTA_SALVATAGGIO = 130,
            UTENTE_COD_BLOCCA_FINO_FIORITURA_ETICHETTA_SALVATAGGIO = 131,
            UTENTE_COD_BLOCCA_CARENZA_NON_RISPETTATA_SALVATAGGIO = 132,
            UTENTE_COD_BLOCCA_INTERVALLOTRATTAMENTI_ETICHETTA_SALVATAGGIO = 133,
            UTENTE_COD_BLOCCA_AVVERSITANODPI_SALVATAGGIO = 134,
            UTENTE_COD_BLOCCA_PRODOTTONOAVVERSITA_SALVATAGGIO = 135,
            UTENTE_COD_BLOCCA_DOSENONDISPONIBILE_SALVATAGGIO = 136,
            UTENTE_COD_BLOCCA_DOSEUDMNONCONFORME_SALVATAGGIO = 137,
            UTENTE_COD_BLOCCA_DATADPIMIN_SALVATAGGIO = 138,
            UTENTE_COD_BLOCCA_DATADPIMAX_SALVATAGGIO = 139,
            UTENTE_COD_BLOCCA_IMPIANTINONCOERENTIDPI_SALVATAGGIO = 140,
            UTENTE_COD_BLOCCA_DOSEDPIDISERBOMAX_SALVATAGGIO = 141,
            UTENTE_COD_BLOCCA_DOSEDPIDISERBOANNOMAX_SALVATAGGIO = 142,
            UTENTE_COD_BLOCCA_NUMERO_MASSIMO_TRATTAMENTI_DPI_SALVATAGGIO = 143,
            UTENTE_COD_BLOCCA_NUMERO_MINIMO_TRATTAMENTI_DPI_SALVATAGGIO = 174,
            UTENTE_COD_BLOCCA_DOSERAMEANNOMAX_SALVATAGGIO = 144,
            UTENTE_COD_BLOCCA_DOSERAME5ANNIMAX_SALVATAGGIO = 145,
            UTENTE_COD_BLOCCA_EPOCADPINONCONFORME_SALVATAGGIO = 146,
            UTENTE_COD_BLOCCA_BUFFERZONE_ETICHETTA_SALVATAGGIO = 170,
            UTENTE_COD_DEFAULT_BLOCCO_INSERIMENTO_NOTE = 163,
            UTENTE_COD_DEFAULT_BLOCCO_INSERIMENTO_MACCHINE = 164,
            UTENTE_COD_DEFAULT_BLOCCO_INSERIMENTO_OPERATORE = 165,
            UTENTE_COD_BLOCCA_ACQUAMASSIMA_DPI_SALVATAGGIO = 173,
            UTENTE_COD_BLOCCA_DOSEMASSIMA_ANNO_ETICHETTA_SALVATAGGIO = 206,
            UTENTE_COD_BLOCCA_PRODOTTO_RELAZIONE_FORMULATO_SALVATAGGIO = 209,
            UTENTE_COD_BLOCCA_PRODOTTO_ETA_IMPIANTO_SALVATAGGIO = 210,
            SUPERUSER_GESTIONE_MAGAZZINO_ABILITATA = 208,

            // Impostazioni Campagna e GLobal
            UTENTE_COD_FILTRO_STAMPA_SCHEDA_CAMPAGNA_CODICE_PRODUTTORE = 147,
            UTENTE_COD_FILTRO_STAMPA_SCHEDA_CAMPAGNA_TECNICO_RIFERIMENTO = 148,
            UTENTE_COD_FILTRO_STAMPA_SCHEDA_GLOBAL_CODICE_PRODUTTORE = 149,
            UTENTE_COD_FILTRO_STAMPA_SCHEDA_GLOBAL_TECNICO_RIFERIMENTO = 150,
            UTENTE_COD_FILTRO_STAMPA_SCHEDA_CAMPAGNA_DOSE_ETICHETTA = 161,
            UTENTE_COD_FILTRO_STAMPA_SCHEDA_GLOBAL_DOSE_ETICHETTA = 162,
            UTENTE_COD_FILTRO_STAMPA_SCHEDA_GLOBAL_NASCONDI_CAMPO = 166,
            UTENTE_COD_FILTRO_STAMPA_SCHEDA_CAMPAGNA_NASCONDI_CAMPO = 167,

            // 'impostazioni BIO
            UTENTE_COD_FILTRO_STAMPA_SCHEDA_BIO_MACCHINE = 152,
            UTENTE_COD_FILTRO_STAMPA_SCHEDA_BIO_OPERATORI = 153,
            UTENTE_COD_FILTRO_STAMPA_SCHEDA_BIO_TECNICO_AUTORIZZANTE = 154,
            UTENTE_COD_FILTRO_STAMPA_SCHEDA_BIO_DATA_FIRMA = 155,
            UTENTE_COD_FILTRO_STAMPA_SCHEDA_BIO_DATA_ULTIMA_MANUTENZIONE = 156,
            UTENTE_COD_FILTRO_STAMPA_SCHEDA_BIO_CAMPO = 157,
            UTENTE_COD_FILTRO_STAMPA_SCHEDA_BIO_NUM_APPEZZAMENTO = 158,
            UTENTE_COD_FILTRO_STAMPA_SCHEDA_BIO_NUM_APPEZZAMENTO_BIO = 159,
            UTENTE_COD_SCHEDA_CAMPAGNA_BIO_NUM_APPEZZAMENTO = 160,
            UTENTE_COD_FILTRO_STAMPA_PIANOCONCIMAZIONE_CAMPAGNA_DATA_FIRMA = 205,
            UTENTE_COD_SEMINA_TIPO = 175,
            UTENTE_WS_SCARICO_FASCICOLO = 178 // enum_WS_Esterni separati da |
,
            UTENTE_PRATICHE_DA_ATTIVARE_SCARICO_FASCICOLO = 179 // servizi separati da |
,
            UTENTE_WS_GESIONE_AZIENDE = 189,
            UTENTE_COD_FORMULATI_STATO = 184,
            Utente_SchedaColtBio_VisualizzaLottoSemine = 185,
            Utente_SchedaColtBio_VisualizzaLottoRaccolte = 186,
            SuperUser_FiltroSQL_MateriePrime = 187,
            SuperUser_Gestione_Esercizi = 188,
            SuperUser_Albero_Anagrafiche = 190,
            UTENTE_GESTORE_AZIENDE = 191,
            SuperUser_Ereditatore = 192,
            SuperUser_KPIN_BlockName = 193,
            SuperUser_Visualizza_Codici_Anagrafici = 194,
            Codice_Univoco_Appezzamento = 195,
            Codice_Univoco_Impianto = 196,
            Codice_Univoco_Progetto = 197,
            SuperUser_Impostazioni_PDC = 198,
            SuperUser_Gestione_PDC_Analisi = 199,
            Utente_Lingua_Stampa = 200,
            UTENTE_COD_FINALITA = 201,
            UTENTE_COD_REGOLAMENTO = 202,
            UTENTE_NUOVA_PARTICELLA = 203,
            UTENTE_COD_RACCOLTA_TIPO_NUOVO_IMPIANTO = 204,
            UTENTE_OPERAZIONI_ZOO_PREFERITE = 207,
            Visualizza_UdmAggiuntiva_xSup = 215,
            SUPERUSER_NuovoImpianto_DefaultTecnico = 216,
            // =================================================

            UTENTE_DAA = 753,
            Utente_ChkAccettazioneDaDiversi = 761,
            UTENTE_COD_TIPO_ALLERTA_PREZZO_0_VENDITA = 798,
            UTENTE_COD_TIPO_FATTURA_DEFAULT = 799,
            UTENTE_COD_PERSONALIZZAZIONI_GRIGLIE_KENDO = 818,
            UTENTE_COD_PREFERITI_MENU = 823,
            UTENTE_COD_MODALITA_STAMPA = 824,
            UTENTE_COD_PREFERITI_REPORT_VENDITE = 837,

            /// costanti per il proxy
            UTENTE_COD_PROXY = 1000,
            UTENTE_COD_USERNAME_PROXY = 1001,
            UTENTE_COD_PASSWORD_PROXY = 1002,
            UTENTE_COD_HOST_PROXY = 1003,
            UTENTE_UDM_Area_COD = 1010,
            UTENTE_CATASTO_PercentoSogliaDifferenzeEvidenziate = 1011,
            UTENTE_GIS = 1012,

            // Impostazioni CDG
            UTENTE_CONTROLLO_SCARICO_CDG = 1055,
            UTENTE_MODALITA_SCARICO_PRODOTTI_CDG = 1055,
            UTENTE_STATI_PANEL_BAR = 1056,
            UTENTE_FILTRI_RICERCA_DOC_CONTABILI = 1057,
            UTENTE_FILTRI_RICERCA_DOCUMENTALE = 1067,
            UTENTE_FILTRI_RICERCA_FILTRONE_VISITE = 1060,
            UTENTE_Operazione_Predefinita_Da_Impianto = 856,
            SUPERUSER_Livello_Applicazione_Listini = 857,
            UTENTE_COD_PREFERITI_REPORT_ACQUISTI = 859,
            SUPERUSER_GiasAPP_IMPOSTAZIONI = 860,
            SUPERUSER_GiasAPP_DOCUMENTI = 861,
            SUPERUSER_GiasAPP_RILIEVI = 862,
            SUPERUSER_GiasAPP_GIS = 863,
            SUPERUSER_GiasAPP_InCab = 864,
            SUPERUSER_Conferimento_Ripartizione_Impianti = 865,
            SUPERUSER_Conferimento_Ripartizione_Obbligatoria = 866,
            SUPERUSER_GiasAPP_Permessi = 867,
            SUPERUSER_Mod_Ricerca_Impresa = 868,
            UTENTE_COD_PREFERITI_REPORT_ANALISI_PROGETTI = 870,
            UTENTE_COD_PREFERITI_VISTE_INVESTIMENTO_CATASTO = 871,
            UTENTE_GIASAPP_LAVORAZIONI_IN_CAMPO = 873,
            UTENTE_GIASAPP_GESTIONE_POSIZIONE = 874,
            UTENTE_COD_PREFERITI_REPORT_SOSTENIBILITA = 875,
            UTENTE_COD_PREFERITI_REPORT_STATISTICHE_QDC = 876,
            UTENTE_COD_PREFERITI_REPORT_RIEPILOGO_ANALISI_PDC_ZOO = 879,
            UTENTE_COD_PREFERITI_REPORT_CONSISTENZE_ZOO = 880,
            UTENTE_COD_PLANNING_INVIA_A_ESOLVER = 881,
            UTENTE_COD_PREFERITI_REPORT_TRATTAMENTI_ZOO = 882,
            UTENTE_COD_PREFERITI_REPORT_SENZA_TRATTAMENTI_ZOO = 883,
            UTENTE_COD_PREFERITI_REPORT_STAZIONAMENTO_ZOO = 884,
            UTENTE_COD_PREFERITI_REPORT_SCARICO_ZOO = 885,
            SUPERUSER_IMPEDISCI_ELIMINAZIONE_CONTATTI_E_RISORSE_UMANE = 886,
            Operazioni_Contab_Collegate_QdC = 888,
            SUPERUSER_IMPEDISCI_INS_MOD_BROGLIACCIO = 889,
            UTENTE_COD_PREFERITI_REPORT_PIANO_COLTURALE_CATASTO_GRID = 890,
            SUPERUSER_Mod_Filtro_Ricerca = 891,
            SUPERUSER_Mod_Analisi_Terreno = 892
        }
        #endregion

        #region GIS
        public enum Enum_Gis_LayerElementiGrafici_std
        {
            ANALISI_MAPPE_SATELLITARI = -2,
            WMS = -1,
            APPEZZAMENTI = 1,
            Fabbricati = 2,
            CATASTO = 3,
            TESTO = 4,
            TEMALIBERO = 5,
            VULNERABILITA = 6,
            ZONAZIONEPAC = 7,
            AREEOMOGENEE = 8,
            UTILIZZODEISUOLI = 9,
            SVILUPPORURALE = 10,
            INVISIBILE = 11,
            ETTARI_EQUIVALENTI = 12,
            CAMPIONAMENTI = 13,
            CAMPIONIANALISI = 17,
            CAMPI = 18,
            IMPIANTI = 19,
            Impianti_Pianificati_Entita = 33,
            Pianificazioni_Testata = 34,
            Dettagli_Precision_Farming = 50,
            Mappe_Prescrizione = 51,
            Op_Agenda = 54,
            Precision = 55,
            Dettaglio_Ricetta = 60,
            AGENDA_RILIEVI = 63,
            Trattori = 66,
            Centri_Aziendali = 67,
            Fasce_di_rispetto = 86,
            Tecnici_in_campo = 87,
            Corpi_idrici_superficiali = 88,
            Stazioni_Meteo = 89,
            MUZ = 90,
            Indici_Rischio_Produttivita = 91,
            Indice_Erosione = 92,
            Indice_CO2 = 93,
            Indice_Rischio_Meteo = 94
        }
        #endregion

        #region Anagrafe

        public static class Enum_Origine
        {
            public static string ConferimentoWMS = "ConferimentoWMS";
        }

        /// <summary>
        /// Codici cliente presenti in anagrafe.
        /// </summary>
        public enum Enum_Anagrafe_CodiciCliente
        {
            //ApoConerpo = 2004,
            //Apofruit = 2013,
            //AgriBologna = 2069,
            //Casalasco = 2064,
            //CantinaSoave = 2045,
            //CoFruTa = 2051,
            //Costea = 2016,
            //Demetra = 2201,
            //Ferrarini = 2066,
            //FruitModena = 2042,
            //GranfruttaZani = 2029,
            //Italfrutta = 2005,
            Orogel = 2039,
            //OPTA = 2046,
            //Ruggeri = 2044,
            //SBTF = 2065,
            //Valdoca = 2036,
        }

        public enum Enum_CodiciAnagrafe
        {
            Codice_Ausl = 4,
            Centro_Sede_Legale = 101,
            Centro_Sede_Aziendale = 102,
            Centro_Stabilimento = 103,
            TipoAttivita = 1000,
            DataFineControllo = 1001,
            CodiceCentro_Precedente = 1002,
            CodiceCentro_Attuale = 1003 // codice operatore bio
,
            PartitaIva_Fittizia = 1004,
            Associazione_CODEX = 1005,
            Conformita_Biologico = 1006,
            Conformita_FilieraControllata = 1007,
            OTE = 1009,
            CodiceCUAA = 1010,
            DataFineImpiegoPNC = 1014,
            OrientamentoProduttivo = 1015,
            TitoloPossesso = 1016,
            MetodoDiProduzione = 1018,
            pers_LegaleRappresentante = 1019,
            pers_Conduttore = 1020,
            pers_Delegato = 1021,
            pers_Detentore = 1022,
            ImpresaPAT = 1278,
            Codice_Socio = 1033,
            Codice_Certificazione = 1341,
            Impianto_LimiteN = 1050,
            Impianto_LimiteN_Organico = 1316,
            Impianto_LimiteP = 1051,
            Impianto_LimiteK = 1052,
            Impianto_LimiteMg = 1053,
            Impianto_Ibrido = 1054,
            Impianto_CodiceB_Maschio = 1055,
            Impianto_CodiceB_Femmina = 1056,
            Impianto_Genetica_Maschio = 1057,
            Impianto_Genetica_Femmina = 1058,
            Impianto_OffType_Maschio = 1059,
            Impianto_OffType_Femmina = 1060,
            Impianto_TraFila_Maschio = 1061,
            Impianto_TraFila_Femmina = 1062,
            Impianto_SuFila_Maschio = 1063,
            Impianto_SuFila_Femmina = 1064,
            Impianto_Interbina = 1065,
            Impianto_Germinabilita = 1066,
            Impianto_PianoSemina = 1071,
            Impianto_LottoFitosanitarioOmogeneo = 1072,
            Impianto_GroverCode = 1076,
            Appezzamento_CodiceContratto = 1073,
            Impianto_Cooperativa = 1074,
            Appezzamento_ConfiniRischio = 1075,
            Organismo_Referente = 1074,
            GestioneDocumentiAllegati = 1077,
            CodiceRigaRiferimentoQuadroP = 1078,
            Coltura_Precedente_1 = 1079,
            Coltura_Precedente_2 = 1080,
            Coltura_Precedente_3 = 1081,
            Coltura_Precedente_4 = 1082,
            Impianto_Taglio_Tuberi_Patate = 1083,
            Impianto_Parti_Tuberi_Patate = 1084,
            Codice_Appezza_Biologico = 1085,
            Codice_Libro_Soci = 1086,
            Data_Iscrizione_Libro_Soci = 1087,
            Tecnico = 1088,
            Codice_Cliente = 1089,
            Codice_Fornitore = 1090,
            Codice_Fornitore_2 = 1091,
            Codice_Fornitore_3 = 1092,
            Organismo_di_Controllo = 1205,
            Mandato = 1206,
            Codice_Capitolato_Privato = 1093,
            Codice_Residuo = 1340,
            Codice_Certificazione_Prodotto = 1344,
            Riferimento_Alfanumerico_Appezzamento = 1104,
            Magazzino_Conferimento = 1122,
            Codice_Unione_OP = 1125,
            Certificazione = 1130,
            Codice_GlobalGap = 1261,
            Codice_GlobalGap2 = 1314,
            Tribunale_di_registrazione = 1275,
            Regolamento_Aziendale_Default = 1294,
            Disciplinare_Aziendale_Default = 1295,
            Impianto_IAF_ImpegniAggiuntiviFacoltativi = 1296,
            Codice_Destinazione = 1297,
            Programmazione_Impianto_Pratica_Cod = 1298,
            INDICODE_EDI_Euritmo_Codice_punto_consegna_NAD = 4013,
            INDICODE_EDI_Euritmo_Codice_magazzino_emissione_ordine_NAB = 4014,
            INDICODE_EDI_Euritmo_Codice_fornitore_NAS_CodForn = 4015,
            INDICODE_EDI_Euritmo_Tipo_Codice_fornitore_NAS_QCodForn = 4016,
            GS1_CompanyPrefix = 1276,
            BNDOO_BancaDatiOperatoriOrtofrutticoli = 1277,
            GestioneContabile_DataInizio = 1284,
            GestioneContabile_ConsideraSaldiIniziali = 1285,
            Tipo_Gestione_Impresa = 1286,
            Zespri_Codice_kPIN = 1287,
            Zespri_Block_Name = 1288,
            Zepri_Grower_Number = 1317,
            Zepri_Identificativo_Flex2B = 1329,
            Default_Sincro_DDT_Terremerse = 1289,
            CampoSperimentaleBZ = 3000,
            SetAside = 3001,
            IntercalareAzione3 = 3002,
            Azione09_SiepieBoschetti = 3003,
            Azione10_Laghetti = 3004,
            AzioneG = 3005,
            ForestazioneL2080 = 3006,
            Agricampeggio = 3007,
            Lago = 3008,
            Alberatura = 3009,
            AzioneF3 = 3010,
            AzioneD1 = 3011,
            AzioneF1 = 3012,
            VivaioOrnamentali = 3013,
            Azione_Agroambientale = 3014,
            Tara_improduttiva = 3015,
            Prato = 3016,
            Affitto = 3017,
            Misura_2H = 3018,
            Orto = 3019,
            Biomassa = 3020,
            Colture_a_perdere_per_fauna_selvatica = 3021,
            Frutteto = 3022,
            Erbaio = 3023,
            Bosco = 3024,
            NessunaMappaturaConGias = 3078,
            UtilizzoImpianto_DaDefinire = 3103,
            Riferimento_Alfanumerico_Campo = 1279,
            Coltura_Appezzamento_Precedente_1 = 1280,
            Coltura_Appezzamento_Precedente_2 = 1281,
            Coltura_Appezzamento_Precedente_3 = 1282,
            Coltura_Appezzamento_Precedente_4 = 1283,
            Coltura_Campo_Precedente_1 = 1290,
            Coltura_Campo_Precedente_2 = 1291,
            Coltura_Campo_Precedente_3 = 1292,
            Coltura_Campo_Precedente_4 = 1293,
            Capitolato_Privato = 1093,
            Gruppo_Varietale_1 = 1094,
            Gruppo_Varietale_2 = 1095,
            Gruppo_Varietale_3 = 1096,
            Gruppo_Varietale_4 = 1097,
            Gruppo_Varietale_5 = 1098,
            Gruppo_Varietale_6 = 1099,
            Gruppo_Varietale_7 = 1100,
            Gruppo_Varietale_8 = 1101,
            Gruppo_Varietale_9 = 1102,
            Gruppo_Varietale_10 = 1103,
            Dettaglio_Specie_Personalizzato = 1108,
            Centro_Aziendale_Esterno_Collegato = 1337,
            UTE = 1338,
            PivaSuperUser_Origine_Dato = 1107,
            Codice_ICQ = 1109,
            Impianto_Codice_Programmazione = 1110,
            CodiceREA = 1112,
            CodiceISO = 1111,
            NumIscrAlboSocCoop = 1113,
            NumRegImprese = 1119,
            CodiceStabilimento = 1114 // sul fabbricato
,
            CodiceConferente = 1115,
            CodiceProduttore = 1116,
            CodiceCooperativa = 1117,
            Codice_Zona = 1118,
            Flag_Smart_Impianto = 1131,
            Flag_Smart_Appezzamento = 1132,
            Codice_Specie_Agea = 1133,
            Codice_Cultivar_Agea = 1134,
            Isola = 1320,
            codice_RUOP = 1321,
            Codice_Sito_Vivaio = 1323,
            Contratto_Produzione = 1324,
            Sup_Contratto = 1325,
            Filiera = 1326,
            Riferimento_Trasferimento_Dati = 1327,
            Data_Inizio_Portinnesto = 1328 // dal 05/21 la data d'inzio del portinnesto viene salvata sul nuovo campo Reg_Impianti.Data_Inizio_Portinnesto (anzichè  in reg_impianti_codici)
,
            CodiceCliente_FRUTTAGEL = 2025,
            TracciaEsportazioneSIGPA_Fabbricati_XFertilizzanti = 5007,
            TracciaEsportazioneSIGPA_Fabbricati_XFormulati = 5017,
            TracciaEsportazioneSIGPA_ChiusuraGiacenze_XFertilizzanti = 5008,
            TracciaEsportazioneSIGPA_ChiusuraGiacenze_XFormulati = 5018,
            CodiceCliente_GranfruttaZani = 2029,
            CodiceCliente_Apofruit = 2013,
            CodiceCliente_Orogel = 2039,
            CodiceCliente_Apot = 2042,
            CodiceCliente_Tecnoterr = 2063,
            CodiceCliente_Casalasco = 2064,
            acciseDAA_CodiceAccisa_Mittente = 4003,
            acciseDAA_CodiceAccisa_UfficioDoganale = 4005,
            acciseDAA_CodiceAccisa_Destinatario = 4006,
            acciseDAA_CodiceAccisa_codiceUA = 4008,
            acciseDAA_Accise_Conto_Garanzia = 4009,
            acciseDAA_Codice_Accise_Prefisso = 4010,
            acciseDAA_VIDIMA_RegistroElettronica = 4011,
            Contatto_OrganismoDiControllo_BIO = 4018,

            // CBI corporate interbancario
            CBI_CodiceSIA = 1264,
            Codice_Centro = 1265,

            // spesometro
            CodiceAteco2007 = 1266,
            codiceFiscaleProduttoreSoftware = 1267,
            Spesometro_intermediario_CodFisc = 1268,
            Spesometro_intermediario_AlboCAF = 1269,
            SISPAC_Codice_Anagrafico = 4007,
            // ----CODICI APOFRUIT SIAGR
            // inizio 1135

            // impresa
            // V01CON00_VCCOD = 1213 'Codice Conferente
            // V01CON00_VCCOD = Codice_Socio 'Codice Socio (Conferente) uso quello presente in gias, visualizzato SOCIO
            V01CON00_VCDNA = 1135 // Data di nascita
,
            V01CON00_VCLNA = 1136 // Località di nascita
,
            V01CON00_VCZON = 1137 // Zona
,
            V01CON00_VCSUA = 1138 // Superficie agricola
,
            V01CON00_VCQTE = 1139 // Codice socio Precedente ex Qta tassa altre coop
,
            V01CON00_VCGRU = 1140 // Gruppo trasportatori
,
            V01CON00_VCTRA = 1141 // Codice trasportatore
,
            V01CON00_VCNIS = 1142 // Numero inscrizione
,
            V01CON00_VCDIS = 1143 // Data iscrizione
,
            V01CON00_VCAIC = 1144 // Anno inizio conferimento
,
            V01CON00_VCIFA = 1145 // Flag Fattura/Autofattura
,
            V01CON00_VCLIS = 1146 // Codice Listino
,
            V01CON00_VCMEZ = 1147 // Flag Mezzadria
,
            V01CON00_VCVAL = 1240 // % meszzadro
,
            V01CON00_VCPRP = 1148 // Codice proprietario
,
            V01CON00_VCAT1 = 1149 // Attributo 1
,
            V01CON00_VCAT2 = 1150 // Attributo 2
,
            V01CON00_VCAT3 = 1151 // Attributo 3
,
            V01CON00_VCAT4 = 1152 // Attributo 4
,
            V01CON00_VCAT5 = 1153 // Attributo 5
,
            V01CON00_VCAT6 = 1154 // Attributo 6
,
            V01CON00_VCAT7 = 1155 // Attributo 7
,
            V01CON00_VCAT8 = 1156 // Attributo 8
,
            V01CON00_VCAT9 = 1157 // Attributo 9
,
            V01CON00_VCAT0 = 1158 // Attributo 10
,
            V01CON00_VCDUM = 1175 // Data ultima variazione
,
            V01CON00_VCANN = 1160 // Flag annullamento
,
            V01CON00_VTPCON = 1161 // Tipo conferimento
,
            V01CON00_VCCOIS = 1162 // Centro di conferimento
,
            Stabilimento = 1163 // Stabilimento di conferimento (FARLO PUBBLICO)
,
            V01CON00_VANAN = 1164 // Anni anzianità
,
            V01CON00_VANUL = 1165 // Anno ultima liquidazione
,
            V01CON00_VANULI = 1166 // Anno ultimo invecchiamento
,
            V01CON00_VLIATR = 1167 // Linea addebito trasporto
,
            V01CON00_VLIRTR = 1168 // Linea rimborso trasporto
,
            V01CON00_VCFOR = 1169 // Codice Gruppo
,

            // impresadettagli
            CATCON0F_CCRAP = 1170 // Codice Rappresentante
,
            CATCON0F_CCEST = 1171 // Superficie totale
,
            CATCON0F_CCRIF = 1172 // Rif. nucleo familiare
,
            CATCON0F_CCSES = 1173 // Sesso
,
            CATCON0F_CCDTN = 1174 // Data notifica
,
            // CATCON0F_CCDTV = 1175 'Data variazione/Taratura Atomiz. non lo sal,co, lo uso per la prima importaz e basta

            // centro
            CATANA0F_CAFON = 1262 // Numero Fondo
,
            CATANA0F_CAZON = 1176 // zona/frazione
,
            CATANA0F_CAZOP = 1177 // produttivita zona/frazione
,
            CATANA0F_CACON = 1241 // Note/Confinanti
,
            CATANA0F_CARES = 1242 // Responsabile Centro
,
            CATANA0F_CAFPR = 1243 // Flag Proprietario
,
            CATANA0F_CATCO = 1178 // Tipo Conduzione
,
            CATANA0F_CASUT = 1244 // Sup. Totale
,
            CATANA0F_CASUP = 1245 // Sup. Pianura
,
            CATANA0F_CASUC = 1246 // Sup. Collina
,
            CATANA0F_CATAR = 1247 // Sup. Tara
,
            CATANA0F_CASUI = 1248 // Sup. Incolta
,
            CATANA0F_CASUF = 1249 // Sup. Frutteto
,
            CATANA0F_CASUO = 1250 // Sup. Orticola
,
            CATANA0F_CASUV = 1251 // Sup. Vigneto
,
            CATANA0F_CASUS = 1252 // Sup. Seminativo
,
            CATANA0F_CASUA = 1253 // Sup. Altri
,
            CATANA0F_CAALT = 1254 // Altitudine
,
            CATANA0F_CAFLI = 1255 // Flag Irriguo
,
            CATANA0F_CAFLH = 1256 // Flag Acqua
,
            CATANA0F_CAFLR = 1257 // Reperibilità acqua
,
            CATANA0F_CAREA = 1258 // Reddito agrario
,
            CATANA0F_CARED = 1259 // Reddito Dominicale
,
            CATANA0F_CAPCA = 1260 // Perc. Abbattimento
,
            CATANA0F_CAA01 = 1179 // tecnico di riferimento zona
,
            CATANA0F_CAA02 = 1180 // azienda eurepgap
,
            CATANA0F_CAA03 = 1181 // sup totale per scaglioni
,
            CATANA0F_CAA04 = 1182 // sup ortofrutta per scaglioni
,
            CATANA0F_CAA05 = 1183 // tecnico di riferimento RER
,
            CATANA0F_CAA06 = 1184 // Prospettiva Aziendale
,
            CATANA0F_CAA07 = 1185 // Aumento-Diminuzione prodotti
,
            CATANA0F_CAA08 = 1186 // Fascicolo Aziendale
,

            // impianto
            CATIMP0F_CINPR = 1214 // Numero Progressivo Impianto
,
            CATIMP0F_CIFPC = 1187 // flag pianura collina
,
            CATIMP0F_CIFIR = 1188 // flag irriguo
,
            CATIMP0F_CISER = 1189 // flag serra
,
            // CATIMP0F_CILIN = 1190 'codice lotta intagrata, (uso capitolato)
            CATIMP0F_CIRAC = 1191 // raccolta manuale/meccanica
,
            // CATIMP0F_CIPIN = 1192 'Portinnesto
            // CATIMP0F_CIALL = 1193 'Allevamento
            CATIMP0F_CIIRR = 1194 // Irrigazione
,
            // CATIMP0F_CITPR = 1195 ' tpo produzione-regolamento bio
            // CATIMP0F_CINAP = 1195 'numero appezzamento per consociazione (utilizzato enum_CodiciAnagrafe.Codice_Appezza_Biologico)
            // CATIMP0F_CIQTP = 1196 'raccolta ottimale
            // CATIMP0F_CIQTC = 1197 'raccolta corretta
            CATIMP0F_CIC02 = 1198 // Fumigazione
,
            Contributi = 1199 // contributi
,
            // CATIMP0F_CITCO = 1200 'Tipo copertura
            CATIMP0F_CIDAL = 1201 // Data ammissione LI
,
            CATIMP0F_CIDEL = 1202 // Data esclusione LI
,
            CATIMP0F_CITIM = 1203 // Impianto consociato - successione
,
            CATIMP0F_CISEQ = 1204 // Sequenza successione - consociazione
,
            CATIMP0F_CISPE_CIVAR = 1205 // salvo la specie e varieta (loro codice) 
,
            CATIMP0F_VarietaContratto = 1206 // Segnalo se varieta a contratto 0/1 campo VVAT4
,

            // impresa note codificate
            // V01NOC00_NCPRO_9000 = 1207 'codice ente certificazione bio
            V01NOC00_NCPRO_9001 = 1208 // Numero etichetta
,
            // V01NOC00_NCPRO_9002 = 1209 'Cellulare del socio
            // V01NOC00_NCPRO_9003 = 1210 'codice operatore bio
            V01NOC00_NCPRO_9004 = 1211 // passaggio al codice
,
            V01NOC00_NCPRO_9005 = 1212 // Coordinate gps
,
            V01NOC00_NCPRO_9006 = 1261 // CGN
,
            // impresa note generiche
            V01NOC00_NCPRO_1 = 1215,
            V01NOC00_NCPRO_2 = 1216,
            V01NOC00_NCPRO_3 = 1217,
            V01NOC00_NCPRO_4 = 1218,
            V01NOC00_NCPRO_5 = 1219,
            V01NOC00_NCPRO_6 = 1220,
            V01NOC00_NCPRO_7 = 1221,
            V01NOC00_NCPRO_8 = 1222,
            V01NOC00_NCPRO_9 = 1223,
            V01NOC00_NCPRO_10 = 1224,
            V01NOC00_NCPRO_11 = 1225,
            V01NOC00_NCPRO_12 = 1226,
            V01NOC00_NCPRO_13 = 1227,
            V01NOC00_NCPRO_14 = 1218,
            V01NOC00_NCPRO_15 = 1229,
            V01NOC00_NCPRO_16 = 1230,
            V01NOC00_NCPRO_17 = 1231,
            V01NOC00_NCPRO_18 = 1232,
            V01NOC00_NCPRO_19 = 1233,
            V01NOC00_NCPRO_20 = 1234,
            V01NOC00_NCPRO_21 = 1235,
            V01NOC00_NCPRO_22 = 1236,
            V01NOC00_NCPRO_23 = 1237,
            V01NOC00_NCPRO_24 = 1238,
            V01NOC00_NCPRO_25 = 1239,


            // ----FINE CODICI APOFRUIT SIAGRultimo 1262
            // 

            ORGANISMO_DI_CONTROLLO_BIO = 1263 // organismo controllo salvato sui codici del centro, collegato a tabella BIO_Dati_OrganismiControllo
,
            Fabbricato_Forno_Combustibile = 1270,
            Fabbricato_Forno_Fiamma = 1271,
            Fabbricato_Forno_Cantiere = 1272,
            Fabbricato_Forno_Umidificazione = 1273,
            Fabbricato_Forno_Tipo = 1274,
            OrganismoRefIntestatarioQDC = 1299,
            Codice_Impianto = 1300,
            Distinta_Chiusa = 1301,
            Codice_Impianto_Ribaltato = 1311,
            Algoritmo_Codifica = 1312,
            ScontoContattoDefault = 4000,
            ListinoPrezziAcquistoDefault = 4001,
            ListinoPrezziVenditaDefault = 4002,
            CodiceAccisa = 4003,
            ModalitaPagamentoDefault = 4004,
            Codice_Ufficio_Doganale = 4005,
            Codice_Magazzino_Fiscale = 4006,
            Codice_Accise_UA = 4008,
            Codice_Accise_Conto_Garanzia = 4009,
            IBANDefault = 4012,
            NumeroIscrizioneAlboAutotrasportatori = 4017,
            PEC = 4019,
            SDI = 4020,

            // --- Per Fattura Elettronica
            CapitaleSociale = 1123,

            /// <see cref="enumTipoSocieta"/>
            TipoSocieta = 1302      // Spa, Sapa, Srl, Altro
,
            UfficioRea = 1304,
            NumeroRea = 1305,

            /// <see cref="enumNumeroSoci"/>
            NumeroSoci = 1306,

            /// <see cref="enumStatoLiquidazione"/>
            StatoLiquidazione = 1307,
            DataAttivazioneEFattura = 1309,
            DataUltimaRicezioneEFattura = 1310,

            /// <see cref="enumTipoContattoFattura"/>
            TipoContattoFattura = 1303,
            PecContatto = 4019,
            CodiceSDI = 4020,
            RappresentanteFiscale = 4021,

            // codici aggiunti per dichiarazione intenti in e-fatt
            DichiarazioneIntentoNumeroProtocollo = 4022,
            DichiarazioneIntentoDataRicezione = 4023,
            ReferenteConferimento = 4024,
            Visibile_da_App = 1308,
            UfficioICQRF = 1313,
            Gestione_Vettore_Default = 1315,
            CodiceParticella = 1318,
            Finalita_Concimazione_Impianto = 1319,
            Centro_CodiceStabilimento = 1322,
            Zespri_Fasi_Fase = 1330,
            Zespri_Fasi_Tipo = 1331,
            Zespri_Fasi_Grower = 1332,
            Num_Piante_Femmine = 1333,
            Num_Piante_Maschi = 1334,
            Impresa_Pubblica = 1335,
            TipologiaDIInnestoTrapianto = 1336,
            Potenziale_Metanigeno = 1339,
            GiasAPP_Dati_Impresa = 1342,
            GiasAPP_Dati_Centro = 1343,
            Terreno_Inutilizzato = 1345,
            Terreno_Degradato = 1346,
            Low_ILUC = 1347,
            Certificate_Number = 1348,
            CodiceAgenzia = 1349,
            CodiceZona = 1350,
            Fabbricato_Uso_da_Terzi = 1351,
            Lavorazione = 1352,
            Specifica = 1353,
            Sau_Tot_Azienda_ha = 1354,
            Ultima_Verifica_Ispettiva = 1355,

            // Nuovo tracciato agea
            Agea_EffluentiZootecnici = 1356,
            Agea_TipoDiSemina = 1357
        }
        #endregion

        public enum enum_Agenda_Causali
        {

            // Profili Utenti
            PROFILI_UTENTI = 900,

            // Anagrafe
            IMPRESA = 1050,
            STRUTTURA = 1100,
            APPEZZAMENTO = 1200,
            IMPIANTO = 1300,
            CATASTO = 1500,
            STALLA = 1600,

            // Operazioni Colturali
            TRATTAMENTO = 2050,
            RILIEVO_CAMPO = 2100,
            RILIEVO_RACCOLTA = 2200,
            LAVORAZIONE = 2300,
            COSTI_ACCESSORI = 2600,

            // Operazioni Zootecniche
            ANIMALE = 3001,
            ANALISI_LATTE = 3100,
            ALIMENTAZIONE = 3200,
            LETTIERE = 3300,
            MUNGITURA = 3400,
            MACELLAZIONE = 3450,
            RILIEVI_PRODUZIONI = 3500,
            EVENTI = 3550,
            VISUALIZZAZIONE_CONSISTENZE = 3600,
            CARICO_CONSISTENZE = 3700,
            SCARICO_CONSISTENZE = 3750,

            // Operazioni Contabili
            REGISTRAZIONI = 4000,
            CONFERIMENTO = 4100,
            CONFERIMENTO_DIVERSI = 4200,

            // Cartografia
            CARTOGRAFIA = 5001,

            // Contatti
            CONTATTO = 6001,
            RAPPORTO_CONTABILE = 6100,
            CORRISPETTIVO = 6200,
            MOVIMENTO_CONTABILE = 6300,
            MOVIMENTO_NON_CONTABILE = 6400,
            STATISTICHE = 6500,
            CLIENTE = 6600,
            FORNITORE = 6610,
            DIPENDENTE = 6620,
            TERZISTA = 6630,
            LEGALE = 6640,
            IMPUTAZIONE_MANODOPERA = 6800  // Utilizzo di manodopera
,
            IMPUTAZIONE_TERZISTI = 6850  // Utilizzo dei Terzi
,

            // Magazzini
            MAGAZZINO = 7001,
            VISUALIZZAZIONE_GIACENZE = 7100,
            VISUALIZZAZIONE_INVESTIMENTO = 7200,
            CARICO = 7300,
            SCARICO = 7350,
            TRASFERIMENTO = 7380,
            IMPUTAZIONE_UTILIZZO_PRODOTTI = 7400,
            PRODOTTI_AZIENDALI = 7800,
            ACCETTAZIONE_BENI = 7900,

            // Parco Macchine
            ANAGRAFE_PARCOMACCHINE = 8001,
            IMPUTAZIONE_PARCOMACCHINE = 8100  // Utilizzo del Parco Macchine
,

            // Progetti
            PROGETTO = 9001,
            Progetto_Produzione_Agricola = 9100,
            PROGETTO_ZOOTECNICO = 9150,
            PROGETTO_TECNICO = 9200,
            CONTRATTO_COLTURALE = 9300,
            CONTRATTO_CONFERIMENTO_COLTURALE_AZIENDA_AGRICOLA = 9301,
            ORDINE = 9320,
            PROGRAMMA_PRODUZIONE = 9325,
            CENTROCOSTO = 9350,
            IMPUTAZIONE_COSTISTANDARD = 9400,

            // Linee Produzione
            LINEA_PRODUZIONE = 10001,
            LINEA_VEGETALE = 10100,
            LINEA_ANIMALE = 10200
        }

        public enum Enum_TipoEntita
        {
            Impresa = 1,
            Centro = 2,
            Campo = 3,
            Appezzamento = 4,
            Impianto = 5,
            Particella = 6,
            Macchina = 7,
            Contatto = 8,
            AnalisiTerreno = 9,
            PianoConcimazione = 10,
            PUA = 11,
            Fabbricato = 12,
            Ricetta_Destinazione = 13,
            OperazioneDiAgenda = 14,
            Uma_Carburanti_Richiesta = 15,
            Ricetta = 16,
            Particella_Impresa = 17
        }

        public enum Enum_IndirizzoTipo
        {
            // Persona giuridica
            // Codice            Tipo indirizzo
            // 1	                Sede operativa
            // 101	            Sede legale
            // 102	            Sede aziendale
            // 103	            Stabilimento
            // 201                Stabile Organizzazione
            SedeOperativa = 1,
            SedeLegale = 101,
            SedeAziendale = 102,
            Stabilimento = 103,
            StabileOrganizzazione = 201,

            // Persona fisica
            // Codice            Tipo indirizzo
            // 2	                Domicilio
            // 3	                Residenza
            // 4	                Residenza Estiva
            // 5	                Luogo di nascita
            Domicilio = 2,
            Residenza = 3,
            ResidenzaEstiva = 4,
            LuogoNascita = 5
        }

        public enum Enum_TipoControllo
        {
            UNDEFINED = 0,
            CASELLA_TESTO = 1,
            AREA_TESTO = 2,
            MENU_DISCESA = 3,
            CASELLA_SPUNTA = 4, // boolean switch / checkbox
            CALENDARIO = 5,
            ALLEGATO = 6,
            LINK = 7,
            PASSWORD = 8,
            MULTISELECT_ESTESA_SERVER = 9,
            PULSANTE_SCELTA = 10,
            IMMAGINE = 11,
            DDL_ESTESA_CLIENT = 12,
            GIS_VIEWER = 13,
            DDL_ESTESA_SERVER_LIGHT = 14,
            NUMERO_INTERO = 15,
            NUMERO_DECIMALE = 16,
            MULTISELECT = 17
        }

        public enum enum_TipoOperazioneDB
    {
            Lettura = 0,
            Scrittura = 1,
            Modifica = 2,
            Cancellazione = 3,
            Trasferimento = 4,
            Copia = 10
    }

        public enum Enum_UnitaMisura
        {
            KG = 2,// 1 
            Grammi = 3,//0,001 kg
            Quintali = 4,//100 kg
            Milligrammi = 2032,
            Tonnellate = 304,

            Millilitri = 101,
            CentimetriCubi = 104,
            Litri = 29,
            Metri_Cubi = 19,       //1000 litri
            Ettolitro = 2121,


            Grammi__HA = 20,
            KG__HA = 88,
            UNITA__HA = 89,
            METRI3__HA = 90,
            Tonnellate__HA = 2098,
            NumUnita__HA = 176,
            QUINTALI__HA = 2120,
            Tonnellate__HA_Spighe = 2112,

            Litro__HA = 22,
            Millilitri__Ha = 163,


            CC__HL = 21,
            Grammi__HL = 23,
            Milligrammi__HL = 126,

            Millilitri__HL = 164,
            Litri__HL = 173,
            Millilitri__Litro = 2016,
            Chilogrammi__HL = 175,
            Grammi__Litro = 2003,
            Milligrammi__Litro = 5001006,


            Num_Piante = 92,
            Unita_Seme = 93,
            Confezioni = 1003,

            Ettaro = 2123,

            Metri = 25,
            MetriQuadri = 28,
            Ore = 141,
            Giorni = 1002,

            Numero_Trappole = 11,
            Numero_Inneschi = 38,

            Numero_Diffusori_HA = 119,

            Numero = 38,

            Anno = 2019,
            StagioneColturale = 2024,
            CicloColturale = 2025,

            Montegradi = 5001040,

            Millimetri = 18,
            Chilometri = 305,
            Miglia = 306,

            UNITA = 5001053,

            Numero_Diffusori = 5001052,

            Percentuale = 1,
            Grammi__dL = 5001055,
            femtoLitri = 5001056,

        }

        public enum Enum_TipoAgricoltura
        {
            Convenzionale = 1,
            InConversione = 2,
            Biologica = 3

        }

        public enum Enum_Cod_Regolamento
        {
            Non_Specificato = 0,
            Regolamento_Nessuno = 1,
            Regolamento_Bio = 4,
            Regolamento_ProduzioneIntegrata = 10
        }

        public enum Enum_CAC_Codifica_InfoAggiuntive_ArgomentoCod
        {
            CapitolatoPrivato = 1,
            DettaglioSpeciePersonalizzato = 2,
            CAA_Agrea = 3,
            CAA_Anagrafe = 4,
            Piano_Semina = 5,
            Revisione_Reportistica = 6,
            Residuo = 7,
            Certificazione_Prodotto = 8,
            CodificaRazzeTRACESNT = 9,
            Lavorazione = 10,
            Specifica = 11,
        }

        public enum Enum_Rapporti_Contabili_Standard
        {
            Legale_Rappresentante = -1,
            Cliente = -2,
            Fornitore = -3,
            Dipendente = -4,
            Terzista = -5,
            Tecnico = -6,
            Centro_Revisione_Manutenzione_Macchine = -7,
            Laboratorio_Analisi = -8,
            Socio = -9,
            Trasportatore = -10,
            Tecnico_Responsabile = -12,
            Agente = -13,
            Referente_Aziendale = -14,
            Consulente = -15,
            Spedizioniere = -16,
            Vivaio = -17,
            Conferente = -18,
            Operatore_Lab_Controllo_Qualita = -19,
            Capo_Area = -20,
            Avventizio = -21,
            Smaltitore = -22,
            Fornitore_Ortofrutta = -24,
            Organismo_Referente = -25,
            Rappresentante_Fiscale = -26,
            Coadiuvante_Familiare = -27,
            Organismo_Di_Controllo = -28,
            Riferimento_Trasferimento_Dati = -29,
            Fornitore_Agrofarmaci = -30,
            Allevatore = -31,
            Macello = -32,
            Veterinario = -33,
            Referente_Conferimento = -34,
            Ditta_Sementiera = -35,
            Trattorista = -36,
        }
        public enum enum_GruppoVegetale
        {
            NonDefinito = 0,
            Arboree = 1,
            Erbacee = 2,
            OrtoFloroVivaismo = 3
        }

        public enum Enum_GruppoOperazioni
        {
            // TIPO C
            Rilievi_in_Campo = 1,
            Rilievi_alla_Raccolta = 2,
            Trattamenti = 3,
            Lavorazioni = 4,
            Altre_Operazioni_Colturali_NonGestite = 5,
            Linee_Produzione_Vegetale = 100,

            // TIPO E
            Registrazioni_Contabili = 6,
            Gestione_beni_ammortizzabili = 7,
            Entrate_ed_uscite = 8,
            Scadenze = 9,
            Registrazioni = 10,
            Utilità = 11,

            // TIPO Z
            Analisi_del_latte = 12,
            Anagrafe_animali = 13,
            Rilievi_produzioni = 14,
            Gestione_mungitura = 15,
            Gestione_lettiere = 16,
            Gestione_alimentazione = 17,
            Altri_lavori_di_stalla = 18,
            Linee_Produzione_Animale = 101,

            // TIPO P
            Gestione_Macchine_e_Attrezzature = 19,

            // TIPO V
            Audit_Monitoraggi = 20,
        }

        #region "Filtro Ricerca NEW"

        public enum Enum_TipoMostra_FiltroRicerca
        {
            Aziende = 1,
            CentriAziendali = 2,
            Campi = 3,
            Appezzamenti = 4,
            Impianti = 5,
            Esercizi = 6,
            PianoColturale = 7,
            PianoColturaleBudget = 8,
            Fabbricati = 9,
            Movimenti = 10
        }

        public enum Enum_Entita_FiltroRicerca
        {
            Azienda = 1,
            CentroAziendale = 2,
            Campo = 3,
            Appezzamento = 4,
            Impianto = 5,
            Esercizio = 6,
            Fabbricato = 7
        }

        public enum Enum_FiltroDestinazioneUso_FiltroRicerca
        {
            Tutto = 0,
            SoloDestinazioniUso = 1,
            EscludiDestinazioniUso = 2
        }

        public enum Enum_FiltroPoligoni_FiltroRicerca
        {
            Tutto = 0,
            ConPoligoni = 1,
            SenzaPoligoni = 2
        }

        public enum Enum_FiltroRipartoCatasto_FiltroRicerca
        {
            Tutto = 0,
            ConRiparto = 1,
            SenzaRiparto = 2
        }

        public enum Enum_ColonnaData_FiltroRicerca
        {
            ValiditaInizio = 0,
            ValiditaFine = 1,
            IntervalloValidita = 2,
            DataCreazioneAnagrafica = 3,

            /// <summary>
            /// Disponibile solo nel BE, viene usato per propagare il filtro 'data operazione' su app-imp-ese
            /// </summary>
            EsercizioFiltroMovimenti = 99
        }

        public enum Enum_TipoConfronto_FiltroRicerca
        {
            Maggiore = 0,
            Minore = 1,
            MaggioreUguale = 2,
            MinoreUguale = 3,
            CompresoFra = 4,

            /// <summary>
            /// Disponibile solo nel BE, viene usato per propagare il filtro 'data operazione' su app-imp-ese
            /// </summary>
            EsercizioFiltroMovimenti = 99
        }

        public enum Enum_ModalitaFiltroData_FiltroRicerca
        {
            Manuale = 0,
            AnnataAgraria = 1,
            Oggi = 2,
        }

        public enum Enum_FiltroOperatoreLogico_FiltroRicerca
        {
            OR_AlmenoUnaCondizioneTrue = 0,
            AND_TutteLeCondizioniTrue = 1,
        }

        public enum Enum_FiltroOperazioniAgenda_FiltroRicerca
        {
            Tutto = 0,
            ConOperazioni = 1,
            SenzaOperazioni = 2,
        }

        #endregion

        public enum EnumCodiciDefault
        {
            Semina = 1,
            Fioritura = 2,
            Raccolta = 3,
            Resa = 4,
            DosiHa = 17,
            DosiHaUdm = 18,
            UnitaCalore = 5,
            Gg = 6,
            Cal1 = 7,
            Cal2 = 8,
            Cal3 = 9,
            Cal4 = 10,
            Cal5 = 11,
            Cal6 = 12,
            TipoMaturazione = 13,
            SogliaMinima = 14,
            ResaStabilimento = 15,
            PesoSgocciolato = 16,
        }

        public enum Enum_Zone
        {
            Pianura = -31,
            Montagna = -30,
            Collina = -29,
            ZVN = -17,
            ZPS = -3,
            SIC = -2,
        }

        public enum ContributionType
        {
            ACA = 1
        }

        public class TIPI_DESTINAZIONE
        {
            public const int TIPO_DESTINAZIONE_IMPIANTO = 0;
            public const int TIPO_DESTINAZIONE_MAGAZZINO = 20;
            public const int TIPO_DESTINAZIONE_VASCA = 13;
            public const int MAGAZZINO = 20;
            public const int STALLA = 15;
            public const int CONSISTENZA = 18;
            public const int VASCA_ENOLOGICA = 13;
            public const int CELLA_FRIGORIFERA = 16;
            public const int SILOS = 17;
            public const int TIPO_DESTINAZIONE_RAGGRUPPAMENTO_STALLA = 21;
            public const int ESSICCATOIO = 222;
            public const int TIPO_DESTINAZIONE_ANIMALE = 1;
        }

        public enum enum_Gestione_Giacenze
        {
            SoloMovimentati = 0, // default
            SoloPresenti = 1,
            TuttiProdotti = 2
        }

        public enum enum_Gestione_Lotti
        {
            Nessuna = 0, // default
            Obbligatoria = 1,
            Facoltativa = 2
        }


        /// <summary>
        /// Codici non più usati all'iterno dek GIAS
        /// </summary>
        public static readonly int[] CodiciSkip = new int[]
        {
            (int)Enum_CodiciAnagrafe.CodiceCUAA,
            (int)Enum_CodiciAnagrafe.TitoloPossesso,
            (int)Enum_CodiciAnagrafe.Algoritmo_Codifica,
            (int)Enum_CodiciAnagrafe.Disciplinare_Aziendale_Default,
            1012,  //Reg. CEE n. 2081/00
            (int)Enum_CodiciAnagrafe.Coltura_Campo_Precedente_1,
            (int)Enum_CodiciAnagrafe.Coltura_Campo_Precedente_2,
            (int)Enum_CodiciAnagrafe.Coltura_Campo_Precedente_3,
            (int)Enum_CodiciAnagrafe.Coltura_Campo_Precedente_4,
            (int)Enum_CodiciAnagrafe.PivaSuperUser_Origine_Dato,
            (int)Enum_CodiciAnagrafe.Impianto_IAF_ImpegniAggiuntiviFacoltativi,
            (int)Enum_CodiciAnagrafe.Regolamento_Aziendale_Default,
            (int)Enum_CodiciAnagrafe.Centro_Sede_Legale,
            (int)Enum_CodiciAnagrafe.Centro_Sede_Aziendale,
            (int)Enum_CodiciAnagrafe.Centro_Stabilimento,
            1120, //Ispettorato Centrale Repressione Frodi Ufficio Periferico (solo per tipo attività PVVI)
            1121, //Autorità Competente del Luogo di Spedizione (solo per tipo attività PVVI)
        };

        public enum enum_Esportazioni_Sistema_Cod
        {
            Enogis = 1,
            Artea = 2,
            BDN = 3,
            XFarm = 4,
            OnPlantImport = 5,
            OnPlantExport = 6,
            HubIoT = 7,
            Nogmo = 8,
            Fex = 9,
            AsipoImport = 10,
            AsipoOrdiniMVExport = 11,
            AsipoOrdiniMVGetStato = 12,
            Recap_Ricette_CAI = 20,
            OrogelBI = 21,

            Demetra_Import_Analisi = 22,
            Demetra_Export_Analisi = 23,

            Demetra_Import_Attivita = 24,
            Demetra_Export_Attivita = 25,

            Demetra_Import_Fabbricati = 26,
            Demetra_Export_Fabbricati = 27,

            Demetra_Import_LavoratoriQDC = 28,
            Demetra_Export_LavoratoriQDC = 29,

            Demetra_Import_Fornitori = 30,
            Demetra_Export_Fornitori = 31,
                
            Demetra_Import_Macchine = 32,
            Demetra_Export_Macchine = 33,

            Demetra_Import_MovimentiMag = 34,
            Demetra_Export_MovimentiMag = 35,

            Demetra_Import_DataPublish = 36,
            Demetra_Export_DataPublish = 37,

            Horta = 38,

            CAI_Export_Visite = 39,

            Horta_Orzo = 40,

            Agea_Import_Pendenze = 41,

            OnPlantImport_Conferimenti = 42
        }

    }
}
