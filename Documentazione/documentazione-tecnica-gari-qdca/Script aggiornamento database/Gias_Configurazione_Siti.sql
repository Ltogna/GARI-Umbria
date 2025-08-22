/*  SCRIPT CONFIGURAZIONE_SITI 158 APERTO il 16/05/2025  */


:setvar SuperServer GIAS_Super_Server
:setvar ServerDB GIAS_Server
:setvar VersioneConfigurazioneSiti 158

:setvar PercorsoLink ''  --'/MASTERGIAS' --percorso nel server dei siti, di default stringa vuota, altrimenti ad esempio /MASTERGIAS
:setvar PercorsoCartellaConfigSiti 'C:\GIASLAN\' --'C:\GIASLAN\'  'D:\AGRONICA\'
:setvar LanToWebSiteBasePath  'http://localhost'
:setvar AgronicaCore_FileNameLOG  'AgronicaCoreLog.txt'

:setvar EliminaChiaviServerSeSulSuperserver 'false' 

:setvar enumStampe2010  '5,6,7,9,10,11,12,20,23,37,39,40,42,43,44,46,55,56,57,61,62,63,64,65,67,68,73,99,102,103,104,148,155,156,157,158,159,163,164,165,166,167,168,169,170,149,1,2,3,4,13,14,15,16,19,25,27,29,115,149,150,152,74,172,173,175,176,177,179,180,181,182,183,189,106,107,190,109,191,192,21,22,153,160,193,194,195,196,197,198,199,200,201,203,204,205,206,207,208,209,210,212,213,214,216,217,218,219,220,221,222,223,224,225,226,227,18,151,228,51,26,229,231,232,74,-74,75,76,81,77,78,79,80,171,127,130,131,134,132,133,233,234,235,236,237,238,239,240,241,242,243,244,245,246,247,248,249,250,254,255,256,89,-1' 

/* Lista di database separati da virgola, dove impostare il flag "GIS_EscludiFiltroCodiceFiscaleTecnico" a false  */
/* Per aggiornare questa lista andare in fondo al file dove c'è la dichiarazione della variabile @ServerDBCFTecFalse */


/* ########################################################################### */
/*                                                                             */
/*  A T T E N Z I O N E :                                                      */
/*                                                                             */
/*                                                                             */
/*                                                                             */
/*       ConfigurazioneSiti                                                    */
/*                                                                             */
/*                                                                             */
/*                                                                             */
/*       CTRL+F FOR:                                                           */
/*       -- > --LAST SuperServer                                               */
/*       -- > --LAST ServerDB                                                  */
/*                                                                             */
/* ########################################################################### */
/*  

    --ATTENZIONE, per gestire le chiavi sul superserver occorre avere i siti 
    compilati dal 29 aprile 2014, altrimenti il core che va a leggere 
    non filtra le chiavi perchè usa la piva superuser.
    Occorre eliminare il prima possibile le colonne sito_cod e 
    pivasuperuser dalle tabelle 
    
    CONFIGURAZIONE SITI SERVER E SUPERSERVER
    -Aggiungo chiavi al superserver
    -Elimina chiavi sul gias server se presenti sul superserver
    -aggiunge chiavi non presenti (anche quelle del 329)

---------------------------------------------------------------------------------------------------------
    
VERSIONI:

-- 158:
    - 

-- 157:
    - Aggiunta chiave super_server NumeroMassimoRigheEstraibiliFiltroRicerca con default 99999, per limitare le righe estraibili dal Filtro di Ricerca (new!)

-- 156:
    - Ripristinate chiavi Sincro_ImportAgrea_Password, Sincro_ImportAgrea_Username, Sincro_ImportAgrea_Link, Sincro_ImportAgrea_FlagPasswordCriptata

-- 155:
    - Aggiunte a enumStampe2010 l'excel automatico GiasLan (89,-1)

-- 154:
    - Aggiunta la chiave Filtro_SchedaCampagna_ColturaleBiologico_BS (server) per usare la nuova pagina di lancio BS per la stampa 'Scheda Colturale BIO'

-- 153:
    - Rimossi enum stampe 251, 252, 253, erano destinati a 3 stampe aca che poi non sono mai state fatte 
    - Aggiunti enum stampe 254,255,256, rispettivamente per PdC stampa analisi fitofarmaci, PdC etichetta di prova e PdC rapporto di prova.
    - Chiave ServerAddresses (per indicare gli id dei server sotto bilanciatore per la pulizia della cache)
    - Rimosso update fisso per GiasOnline_WS_Meteo_Meteo e GiasOnline_WS_Meteo_Meteo_2 (in alcune installazioni deve aver un valore diverso, quindi è stata lasciata solo la voce in insert nel super server)

-- 152:
    - Aggiunta la chiave SSOLogin_UserCreationConfig per specificare il profilo da attribuire ai nuovi utenti in fase
      di login tramite SSO. Sviluppo per CreditAgricole.
    - Modificata chiave WS Artea
    - Cambiato mittente mail errori sql da sqldataprovider@agronica.it a service@agronicagroup.it

-- 151:
    - Aggiunta la chiave 'Configurazioni_WS_Mappe_2024' (server)
    - Aggiunte chiavi Verifica_Sottoscrizione_Servizio_QDC e Tipo_Verifica_Sottoscrizione_Servizio_QDC

-- 150:
    - Aggiunta riferimento al web service di aggiornamento della tabella Codifica_Specie_Vegetali_Agea_2025_2020 in Configurazione UMA
    - Aggiunta la chiave 'Configurazioni_WS_Mappe_2024' (server)
    - Aggiunta chiave 'UserPwdConnectionString_toCrypt' (super server)
    - Aggiunta la chiave 'useNewZooMenu' (server) per forzare il redirect del Menù Zootecnia alla nuova pagina Angular
    - Aggiunte a enumStampe2010 le stampe per ACA

-- 149:
    - Aggiunta la chiave 'password_smtp_isEncrypted' (server e super server)
    - Aggiunta la chiave 'PasswordArteaWS_isEncrypted' (server)
    - Aggiunta la chiave 'cr' (server e super server)
    - Tolti campi non più necessari

-- 148:
    - Aggiunta la chiave 'MinutiValiditaTokenRecuperoPassword' per la gestione del token di reset password del DB SuperServer

-- 147:
    - aggiornata chiave googlemaps per visibilita = 1

-- 146:
    - Impostata Visibilita=1 per chiavi che devono essere accessibili da GiasNG (sia Server che SuperServer)
        [servizioAutorizzazioneHubMeteo, AgroProfilazione_MaxUtentiCaricatiDefault, BloccaCoreWS_NG, 
         personalizzazioniRegioneUmbria, Blocco_Inserimento_PIVA_Impresa, ListaStiliPersonalizzati_NG, 
         LinkAgronicaGiasNG, MenuAgendaNG, OperazioniAgendaNG]

-- 145:
    - Rimodificato a 1 il default di "AbilitaHashPassword" in modo che tutte le nuove installazioni partano con la password criptata
    - Aggiunta chiave ConfigChatGPT sul db server 

-- 144:
    - Aggiunto a enumStampe2010 lo Statistometro
    - Aggiunte chiavi sul super_server LinkAPIHubAgea e APIKeyHubAgea

-- 143:
    - Aggiunto cod 248 a enumStampe2010 per Piano Colturale Catasto Griglia
    - Aggiunta chiave AgroProfilazione_MaxUtentiCaricatiDefault per indicare il limite massimo di utenti sotto il quale entrando nella profilazione NG vengono caricati di default tutti gli utenti

-- 142:
    - Aggiunta chiave AgroFascicolo_WS_PadrePivaSuperUser

-- 141:
    - Cambiato valore chiave AgroFascicolo_WS_Coordinamento per puntamento nuovo Web Service fascicoli

-- 140:
    - Aggiunge nuova chiave server servizioAutorizzazioneHubMeteo con parametri login HubMeteo (x widget meteo)

-- 139:
    - Aggiunti enum per stampe Orogel
    - Nuova chiave ParametriElasticSearch per configurazione invio log ad elasticSearch

-- 138:
    - Rimosso update fisso per chiave DataProviderLogConfig in modo da attivare la scrittura su tabella solo da qualcuno (adeguato anche valore in insert con tutte le ultime proprietà)

-- 137:
    - Aggiunta chive per nuovo sito AgronicaNetCoreAPI
    - Aggiunti gli enum per le stampe orogel

-- 136:
    -  Rimosso update da chiave "GiasOnline_WS_Mappe_2023" (ora le varie piattaforme si configurano a mano)
    -  Chiave ElencoManualiJson per impostare elenco manuali dinamico
    -  Aggiunta chiave Log_Segnalazioni_Speciali
    -  Aggiunta chiave Log_Segnalazioni_Speciali_Mail_A
    -  Aggiunta chiave Coldiretti_ElasticSearchUrl

-- 135:
    -  Aggiunti gli enum delle stampe OP

-- 134:
    - Aggiunta chiave per uso nuova funzione inserimento Utenti_Visbilita_Appoggio

-- 133:
    - Chiave personalizzazioniRegioneUmbria per personalizzazioni grafiche Regione Umbria (super-server)
    - Aggiornato link WS Artea se già presente in http

-- 132:
    - Chiave TestUMA per collaudi su richieste future

-- 131:
    - Aggiunto codice 231 e 232, rispettivamente Report_OrdiniVivaio e Report_PianoColturalePreventivoVivaio, a enumStampe2010

-- 130:
    - Aggiunta config per ListaStiliPersonalizzati_2022 e ListaStiliPersonalizzati_NG (super-server)

-- 129:
    -  Modificata chiave Configurazioni_GoogleEarthEngine

-- 128:
    -  Modificata chiave Configurazioni_GoogleEarthEngine

-- 127:
    -  Aggiunta nuova chiave Server Azienda_Timesheet_Tecnici per indicare la PIVA dell'azienda in cui inserire i TimeSheet dei tecnici che devono indicare ferie/permessi/riunioni/visite (sviluppo nato per il CAI, ma generalizzabile)

-- 126:
    -  Aggiunta nuova chiave Server pathFileRaster_GoogleEarthEngine
    -  Aggiunta nuova chiave server Prefisso_PIVA_Generica_Da_Assegnare per personalizzazione prefisso PIVA generata automaticamente nella creazione imprese
    -  Aggiunta nuova chiave server Blocco_Inserimento_PIVA_Impresa per blocco inserimento manuale PIVA in creazione impresa 

-- 125:
    -  Aggiunta nuova chiave superserver AgronicaSpecialNavigationCFG per configurazioni della Login con tunnel (Demetra in primis)

-- 124:
    -  Aggiunta nuova chiave LinkAgronicaDomandaIrrigua per nuovo sito Domanda Irrigua

-- 123:
    - Aggiunta chiave AuthDispatcher_Configurations per l'endpoint di lettura signed URL per Google Earth Engine ed autenticazioni GEE e Google Cloud
    - Aggiunta chiave GiasOnline_WS_Mappe_2023: nuova configurazione per lettura NDVI ed altri indici da GEE.
    - Aggiunta chiave Configurazioni_GoogleEarthEngine: Configurazioni per le elaborazioni di GEE

-- 122:
    - impostato il flag "GIS_EscludiFiltroCodiceFiscaleTecnico" a false in base a lista di db nella variabile "ServerDBCFTecFalse"

-- 121:
    - Aggiunta chiave 'pathFileRaster' per configurazione file raster

-- 120:
    - Aggiunta chiave Google_GeocodingBaseUrl per CalcolaBilancioApportiAmmessi in PC

-- 119:
    - Aggiunta chiave LinkGiasBase (SUPERSERVER / SERVER) per parametrizzazione percorso link GiasBase

-- 118:
    - Aggiunta chiave MenuGisNG per gestione redirect a pagina GIS Vecchia (false) o nuova (true)
    - Aggiunta chiave MenuAnagrafeNG per gestione redirect a pagina Anagrafica Vecchia (false) o nuova (true)
    - Nuova chiave LinkProfitosan_2023
    - Aggiunta stampa 26 a enumStampe2010 (ReportConserveItalia XLS)
    - Aggiunta chiave VersioneLogin

-- 117:
    - Abbassato livello di log del dataprovider a 1 (only file) per flodding mail service@agronica.it

-- 116:
    - Fix chiavi meteo ws (aggiunto anche update su db server)

-- 115:
    - Aggiunta stampa 51 a enumStampe2010 (Impegno Produzione Soci)

-- 114:
    - Aggiornati i link del Meteo per passare in https

-- 113:
    - Aggiornati i link del Profitosan
    - Aggiunte chiavi MenuAgendaNG e OperazioniAgendaNG per redirect vari

-- 112:
     - Aggiunta nuova chiave VersioneHeader per switchare header vecchia a header nuova + sidebar (DBSUPERSERVER) [2022 = Nuova versione / 2021 = vecchia versione]
       Default lasciato a stringa vuota = stabilito con costante a codice (2021) 

     - Aggiunta nuova chiave UsaCoreAPI per decidere se effettuare login alle CoreAPI 
       Default False

-- 111:
    - Impostata chiave Blocca_SeminaConCambioSpecie_SePresentiOperazioni_SuApp sempre = 1 (per bloccare le semine con cambio anagrafica con operazioni precedenti registrate assieme ad altri appezzamenti)
    - Aggiunta chiave GiasOnline_Core_API
    - Bypass temporaneo su default Flag_Nuovo_Controllo_Riduzione_Diserbo per Collis (in cui è stato manualmente spento)

-- 110:
    - Aggiunta nuova chiave VersioneMaster per switchare da css Agronica a css Xonne (DBSUPERSERVER) [2022 = Nuova versione / 2021 = vecchia versione]
      Default lasciato a stringa vuota = stabilito con costante a codice (2021)

-- 109:
    - Impostata chiave Flag_Nuovo_Controllo_Riduzione_Diserbo sempre = 1 per poter switchare tra vecchio / nuovo controllo DoseConsentitaDiserbo

-- 108:
    - Aggiunta stampa 228 a enumStampe2010 (Estrazione catasto affitti)

-- 107:
    - Cambiato valore default LinkArteaWS da "http://.." a "https://.."
    - Aggiunta Chiave sito Agronica UMA (LinkAgronicaUMA)
    - Aggiunto PercorsoLink a chiave LinkAgronicaGiasNG

-- 106:
    - Impostata la chiave IrrigazioneBS come default a true per aprire la nuova pagina dell'Irrigazione

-- 105:
    - Aggiunta chiave per Contratti Affitto
    - Modificata aggiunta chiave DocContabile2021DaAgenda con Valore = 1 in modo che per le NUOVE installazioni la pagina di default
      da lanciare per il carico di magazzino dalla pagina dei trattamenti sia quella dei NUOVI documenti contabili

-- 104:
    - aggiunta stampa 23 a enumStampe2010 (ExportGiasToSap per Conserve Italia)

-- 103:
    - Aggiunta chiave UploadMultiploAllegati_Documentale per abilitare / disabilitare l'uso del nuovo componente kendoUpload nel documentale

-- 102:
    - Aggiunta chiave CompressioneRispostaAjax per abilitare / disabilitare compressione risposte

-- 101:
    - Aggiunta chiave Export_MovimentiMagazzino_Utilizzo_Recode

-- 100:
    - Aggiunta chiave LinkAgronicaGiasNG

-- 99:
    - Aggiunta chiave Flag_Nuovo_Controllo_Riduzione_Diserbo per poter switchare tra vecchio / nuovo controllo DoseConsentitaDiserbo

-- 98:
    - inserito aggiornamento chiavi GiasOnline_WS_Meteo_Meteo e GiasOnline_WS_Meteo_Meteo_2
      a nuovo indirizzo web service meteo
    - fix valorizzazione pivasuperuser su Tracciabilita_ApiUrlBase_SDF e Tracciabilita_ApiUrlResource_SDF
      (con regolarizzazione del pregresso)

-- 97:
    - Aggiunte chiavi per TaskData API SDF
      Tracciabilita_ApiKey_SDF              -> Chiave Auth API
      Tracciabilita_ApiUrlBase_SDF          -> Base Url 
      Tracciabilita_ApiUrlResource_SDF      -> Api Resource

-- 96:
    - Aggiunte chiavi per controlli su Lavorazioni e Ordini di lavorazione

-- 95:
    - Aggiunta chiave per manuale del modulo UMA

-- 94:
    - Aggiunte chiavi per parametri caricamento FTP tracciabilità
      Esporta_Tracciabilita_FTPMode (true/false) -> abilita l'upload ftp dopo esportazione (usato solo in modalita 0)
      Esporta_Tracciabilita_FTPUrl               -> url ftp
      Esporta_Tracciabilita_FTPPort              -> port ftp
      Esporta_Tracciabilita_FTPUsername          -> username
      Esporta_Tracciabilita_FTPPassword          -> password

-- 93:
    - Aggiunte chiavi per abilitare e configurare esportazione tracciabilità
      Esporta_Tracciabilita (true/false) -> abilitare un nuovo pulsante sulla GUI per lanciare l'export della traccibilità
      Esporta_Tracciabilita_Modalita(0/1) -> 0 - destinazione disco del web server
                                             1 - Richiamo Web Service Esterno
      Esporta_Tracciabilita_DestinazionePath -> Path dove esportare il file se modalità = 0
      Esporta_Tracciabilita_LinkWSEsterno -> Url per richiamare WS esterno se modalità = 1
      Esporta_Tracciabilita_WSEsternoParametri -> Stringa di configurazione json per parametri chiamate ws esterno
    - Aggiunta chiave DocContabile2021DaAgenda su DB_Server per controllare la pagina da lanciare per il carico di magazzino dalla pagina dei trattamenti

-- 92:
    - enumStampe2010: aggiunto codice per l' export "Centri" (codice 44)

-- 91:
    - enumStampe2010: aggiunti codici per le stampe "riepilogo impiego superfici" mono e multi azienda (codici rispettivamente 18 e 151)

-- 90:
    - Aggiunta chiave GIS_StaticMapCFG

-- 89:
    - Aggiunta Chiave WS Gestione azienda
    - Aggiunta Chiave Log_Scheda_Campagna_Abilitato x abilitare / disabilitare log tempi in Stampa Scheda Campagna
    - Fix default LinkWSImportaMagazzino, Sincro_ImportDDTSeled_Data_Inizio_Filtro e Sincro_ImportDDTSeled_Data_Fine_Filtro
    - Aggiunta chiave googlemaps in SuperServer

-- 88:
    - Aggiunte chiavi generali: LinkWSImportaMagazzino, PathFileINI_ConnessionePredefinita_Server, PathFileINI_ConnessionePredefinita_Utenti, timeout_smtp 
    - Aggiunte chiavi specifiche per: Sincro, WS Importa Gias e WS Magazzino


-- 87:
    - Aggiunta chiave GiasOnline_WS_Irriframe su DB server per controllare le richieste da GIAS a Irriframe: Contenuto valore -> stringa JSON { "persistent": true/false, "baseUrl": "https://www.irriframe.it/irriframeapi/api" } 

-- 86:
    - enumStampe2010: aggiunti codici per le stampe del conferimento, in particolare Conf_Esportazione_CertificatiPomodoro_XLS = 223 

-- 85:
    - enumStampe2010: aggiunti codici per le stampe UMA 226 e 227

-- 84:
    - enumStampe2010: aggiunto codice 222 (nuovo certificato pomodoro)

-- 83:
    - GIS_EscludiFiltroCodiceFiscaleTecnico: cambiato default a true
    - enumStampe2010 Aggiunti codici per le stampe UMA 224 e 225
    - Fix lettura chiave super_server ClientSMTP per copia su db_server

-- 82:
    - Aggiunta chiave "RichiestaIscrizioneGias" su DB Server per abilitare/disabilitare funzionalità di domanda di registrazione a Gias. Default a 0
    - Nuova Chiave Blocca_SeminaConCambioSpecie_SePresentiOperazioni_SuApp su DB Server

-- 81:
    - Aggiunta chiave MailFrom_smtp anche in Configurazione_Siti del DB Server
    - Aggiunta gestione che riporta su MailFrom del DB Server il valore di MailFrom del DB SuperServer se esiste già la chiave nel DB Server ma è vuota
    - Aggiunta gestione che riporta su ClientSMTP del DB Server il valore di ClientSMTP del DB SuperServer se esiste già la chiave nel DB Server ma è vuota

-- 80:
    - Aggiunte chiavi "ClientSMTP" e "ClientSMTP_Porta" sul super_server con valore vuoto. Erano già presenti sul server
    - Cambiato il default su db server per la chiave "ClientSMTP" da 'SRVMAIL01' a stringa vuota
    - Nuova chiave "JohnDeereConfig in db Server
    - Nuova Chiave DataProviderLogConfig

-- 79:
    - MenuAgendaBS, MenuBS_2017, MenuAnagrafeBS, OperazioniAgendaBS, PianoConcimazioneBS, PlanningBS, RilieviBS, SeminaBS, Gis2017: cambiato il default a true (update valore)
    - Modificato valore chiave LinkPianoConcimazione_2017 (da PianoConcimazione_2017 a AgronicaPianoConcimazione_2017) per l'inserimento del record nuovo su Super_Server
    - Aggiunta chiave LinkPianoConcimazione_2017 su Gias_Server
    - modificato il declare @PivaSuperUser as char(25) in varchar(25)
    - eliminati gli eventuali spazi generati con il declare @PivaSuperUser as char(25)

-- 78
    - enumStampe2010: aggiunto codice 157 (esportazione listini excel)

-- 77
    - Aggiunta chiave Configurazione_MVVE

-- 76: 
    - modificato a 0 il default di "AbilitaHashPassword", diversamente chi usa il LAN non riuscirebbe a fare il login dopo al primo accesso diretto a online.  Fosforo è già a posto

-- 75: 
    - aggiunta chiave su db super_server "MailFrom_smtp" che deve essere concorde con i domini gestiti dall'smtp configurato

-- 74: 
    - aggiunta chiave per SPID, separazione certificati: 
    - aggiunta chiave su db server "AbilitaHashPassword" per scegliere se memorizzare o meno le password degli utenti come hash

-- 73:
    - modificata chiave per SPID_CLAIMS_ARRAY

-- 72:
    - aggiunte chivi con parametri SAML (SPID et al.) su super_server (con parametri vuoti)
    - enum_stampe: aggiunto 40 (stampa pizzoli anche se forse non è più utilizzata, messo per coerenza)
    - aggiunta chiave IrrigazioneBS (default false) 
    - aggiunta chiave per Flex2B: G2F2B_Cfg

-- 71:
    - aggiunta chiave ExportAnalisiPDC_FiltroCapitolatoCodice
    - enumStampe2010: aggiunto codice 217 (Conf_Esportazione_BolleFF_XLS )
    - enumStampe2010: aggiunto codice 218 (Filtro_StampeBiologico )

-- 70:
    - enumStampe2010: aggiunto codice 216 forse il bilancio fertilizzazioni dettagliato (rif Drudi)

-- 69:
    - update MailAssistenza su assistenza@agronica.it sia nel gias_super_server sia nel gias_server

-- 68: 
    - enumStampe2010: aggiunto codice 46 esportatore universale agenda

-- 67:
    - enumStampe2010: aggiunto codice 213 Riepilogo Prodotti Utilizzati

-- 66:
    - aggiunta chiave RicetteOrdiniDiLavoro2018
    - enumStampe2010: aggiunto codice 214

-- 65:
    - enumStampe2010: aggiunti codici 181, 182 stampa massiva reg trattamenti e reg fertilizzazioni

-- 64:
    - aggiunta chiave GiasOnline_WS_Audit_IAgroAPI_Audit
    - modificato default gis2017
    - aggiunta chiave GisEliminazioneImpiantoEliminaAppezzamento
    - aggiunte chiavi x FRUTTAGEL: JDE_ORACLE_DB_DTA e JDE_ORACLE_DB_CTL

-- 63:
    - enumStampe2010: aggiunto codice per EsportazionePomodoroIndustriaOINordItalia = 212

-- 62:
    - enumStampe2010: aggiunto codice per Report_Vendita_PDF (210) 

-- 61:
    - Superserver: aggiunta paginaIndex_PnlPersonalizzatoBasso
    - Flag_Stampe_Nuovi_Arrotondamenti: cambiato il default a true
    - MenuAgendaBS, MenuBS_2017, MenuAnagrafeBS, OperazioniAgendaBS, PianoConcimazioneBS: cambiato il default a true

-- 60:
    - enumStampe2010: aggiunto codice per PianoColturale (39) 
    - enumStampe2010: aggiunto codice per PianoColturaleCatasto (166) 

-- 59:
    - enumStampe2010: aggiunto codice per stampa MVV (209) 

-- 58:
    - aggiunta chiave AgroFascicolo_WS_AGREA
    - aggiunta chiave AgroFascicolo_WS_Coordinamento
    - aggiunta chiave SeminaBS (default false)
    - aggiunta chiave ScaricoArteaWS_Config
    - aggiunta chiave GIS_cmbElementoGrafico_GenerazionePoligoni_DefaultValue

-- 57:
    - enumStampe2010: aggiunti codici per riepiloghi accise (206, 207, 208) 
    - aggiunta chiave RaccoltaBS (default false)
 
-- 56:
    - aggiunta chiave DocumentoRicevutoLight (default false)
    - aggiunta chiave ClientSMTP_Porta (default "")

-- 55:
    - enumStampe2010: eliminato LibroConferimentiPDF (38) [occorre lanciare ancora quello delle stampe2003]

-- 54:
    - enumStampe2010: aggiunti LibroConferimentiPDF (38) e LibroConferimenti (XLS) 148
    - aggiunta chiave LibroConferimenti_NumRighePagina (numero di righe per pagina sul report libro conferimenti pdf, utilizzato da Agrisfera)

-- 53
    - cambio default per enum_PC_Anteprima_ListaWS
    - cambio default per enum_PC_Anteprima_wizardComportamentoWS
    - enumStampe2010: aggiunto RegistroTrattamentiVeneto_StdCondizionalita (205)

-- 52
    - Aggiunta chiave Connessione_AgroGSB (stringa connessione super server)

-- 51 
    - enumStampe2010: aggiunto SchedaCatastoeUtilizzi (204)

-- 50
    - Aggiunta chiave per web service ws_mappe_2013
    - enumStampe2010: aggiunto Registro Aziendale Unico (203)

-- 49
    - aggiunta la chiave minutiValiditaLoginMemorizzato (240, ovvero 4 ore)
    - Svuotamento forzato di versione Kendo su SuperServer perchè era stata impostata fissa la 2017.3.913
    - aggiunta la chiave enablessl_smtp per l'invio delle mail (necessario ad esempio per Gmail)
    - aggiunto enumstampe Esportazione_AnagraficaContatti = 99

-- 48
    - chiave di attivazione verifica GDPR (GDPR_Attivo)

-- 47
    - aggiunta la chiave PathFileExportConferimentiCSV 

-- 46
    - aggiunto il report 25 (Scheda Tracciabilità Vegetale) all'enumStampe2010

-- 45
    - Aggiunta chiave per WS AgroFascicolo

-- 44
    - enumstampe: aggiunto 201 Scheda Interventi Agronomici

-- 43
    - aggiunto il report 37 (dati grafica) all'enumStampe2010

-- 42
    - Aggiunto Flag_Stampe_Nuovi_Arrotondamenti (x il momento con default a false)
    - Aggiunto login_MaxNumeroTentativiAccesso (x impostare il numero massimo di tentativi in fase di accesso)

-- 41
    - Aggiunti FF_AutofatturaLiquidazioneSoci (197), FF_RiepilogoLiquidazioneSoci (198) e FF_PagatiSuCampionato (199) in enum_Stampe

-- 40
    - Aggiunti FF_FatturaLiquidazioneSoci (195) e FF_PagatiSuConferito (196) in enum_Stampe

-- 39
    - Chiave per attivazione nuova pagina GIS

-- 38
    - Chiavi per RilieviBS

-- 37
    - Chiavi per Importazione web service ARTEA

-- 36
    - LinkHomePageGlobale: imposta un'home page personalizzata e diversa dalla nostra, che scatta quando non si passa dalla parte client di index.aspx, poichè si proviene da token

-- 35
    - modificato valore default enum_PC_Anteprima_ListaWS
    - enumStampe2010: eliminato codice 46 (export universale agenda) che era stato aggiunto x sbaglio

-- 34
    - Aggiunta config per GiasOnline_WS_AgronicaWebApiProfilatore (super-server)
    - Aggiunta config per AgronicaWebApiProfilatore_Token (super-server)

-- 33
    - Aggiunto nuovo codice enumStampe2010

-- 32 

    - Aggiunta config per ListaLoghiPersonalizzati (super-server)
    - Aggiunta config per ListaStiliPersonalizzati (super-server)
    - Aggiunta config per VersioneKendo (super-server)

-- 31
    - Aggiunta config per enum_PC_Anteprima_wizardComportamentoWS
    - Aggiunta config per enum_PC_Anteprima_ListaWS
    - Aggiunta config nuovo menu MenuBS_2017

-- 30
    - Aggiunta configurazione per LoginLogAccessoTutti
    - Aggiunta config per stampa buono di campionamento
    - Aggiunta config per assistenza HTML

-- 29
    - Aggiunta configurazione per VerificaAnagraficaImpreseProfilate

-- 28
    - aggiunto GiasOnline_WS_SpecieVegetali_IAgroAPI_SpecieVegetali e GiasOnline_WS_SpecieVegetali_IAgroAPI_SpecieVegetali_2

-- 27
    - Aggiunta configurazione per Sincro_Regolamento_Condizionalita

-- 26
    - Aggiunta Configurazione per PlanningBS

-- 25
    - aggiunta configurazione LanToWebSiteBasePath e AgronicaCore_FileNameLOG

-- 24
    - Corretti link WS che puntavano a IP fisso

-- 23
    - aggiunto BloccoAppezzamentiIgnora

-- 22
    - aggiunto GiasOnline_WS_Fertilizzanti_IAgroAPI_Fertilizzanti e GiasOnline_WS_Fertilizzanti_IAgroAPI_Fertilizzanti_2

-- 21
    - aggiunta chiave OperazioniAgendaBS

-- 20
    - aggiunto GiasOnline_WS_PianoConcimazione_AgroAPI_PianoConcimazione e GiasOnline_WS_PianoConcimazione_AgroAPI_PianoConcimazione_2

-- 19
    - aggiunto LinkPianoConcimazione_2017 (nuovo sito)
    - aggiunto PianoConcimazioneBS (per scegliere se andare nel nuovo sito)

-- 18
    - aggiunto RipartoCatasto_Disabilita_VerificaIntersezione
    - aggiunto codice stampe excel agenti provvigioni (160)

-- 17
    - aggiunto CodRisUm_Personalizzazione_Doc_Contabili_1

-- 16
    - Spostate le stampe del Biologico su Stampe 2010:
        Scheda Materie Prime Biologico (21)
        Scheda Vendite Biologico (22)
        Scheda Preparati Biologico (153)

-- 15
    - aggiunta stampa Scheda Campagna Multi Lombardia (192)

-- 14
    - aggiunta chiave MenuAgendaBS
    - aggiunta chiave MenuAnagrafeBS
    - aggiunto codice stampa bilancio fertilizzazioni (109)
    - aggiunto codice stampa Provincia Autonoma di Trento (191)

-- 13

    - aggiunta chiave ConnessioneMaps_Server
    - aggiunta chiave deltaE
    - aggiunta chiave deltaN
    Servono per Agro_GoogleMaps nell'Agenda

      
-- 12

    - aggiunta chiave Sincro_Codice_Chiave_Cliente
    - aggiunta chiave Sincro_StringaConnessioneExcel
    - aggiunta chiave Sincro_LinkWSImportaGIAS
    - aggiunta chiave Sincro_ImportAnagrafica_BloccaAppezzamenti
    - aggiunta chiave Sincro_ImportAnagrafica_Impianto_DpiCod
    - aggiunta chiave Sincro_ImportAnagrafica_Impianto_RegConcCod
    
    - introdotto parametro x enumStampe2010
                
-- 11
    - aggiunta chiave FiltroGruppiUtente

-- 10 
    aggiunte chiavi:
    - chiavi x import JDE Fruttagel (LABCQ Larino): JDE_ORACLECodiceStabilimento, JDE_ORACLENomeStabilimento, GIS_EscludiFiltroCodiceFiscaleTecnico, FIRMAVERIFICAPATH_EXE

-- 9 
    aggiunte chiavi:
    - RaccoltaNew
    - chiavi x export JDE Fruttagel: JDE_Database, JDE_ORACLEStringaConnessione, JDE_SQLStringaConnessione

    
-- 8 del 07/01/2016
    aggiunte chiavi:
    Stampe_Massive_Filtro
    StartRicevuto_Prog
    lenRicevuto_Prog


-- 7  del   01/09/2015 
      aggiunta chiave per Core WS: 
      GiasOnline_WS_Core_AgroWS_Core
      
-- 6  del   22/06/2015 
      aggiunta chiave 
      FiltroneBootstrap		default = false
      MonitoraggioCE_FW4   default = false
        
-- 3  del   02/07/2014    
      aggiunta Path_AgroVerificatore_Log      
      
-- 2  del   02/05/2014    
      aggiunta user_smtp                			                                        	      	
      aggiunta password_smtp  

-- nuova versione
    aggiunta chiave LinkWS_Importa_GIAS http://localhost/WS_Importa_GIAS/ImportaWS.asmx
    aggiunta chiave LinkGiasOnline_Root che contiene l'indirizzo del sito
    aggiunta chiave LinkGiasOnline_Root che contiene l'indirizzo del sito
    aggiunta chiave FiltroneBootstrap x puntere al nuovo filtrone
                            
*/


USE $(SuperServer)
GO


/* --------------------------------------------------------------------- */
/* --------------- Configurazione_siti chiavi comuni --------------------*/
/* --------------------------------------------------------------------- */
print ('Migra tabella: Configurazione_siti')

if not exists (select 1 from Configurazione_Siti where chiave = 'LinkWS_Importa_GIAS')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (N'', 6, 'LinkWS_Importa_GIAS', 'http://localhost/WS_Importa_GIAS/ImportaWS.asmx')

if not exists (select 1 from Configurazione_Siti where chiave = 'GiasOnline_WS_GiasWSAggiorna_AgroWS_GiasWSAggiorna')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (N'', 6, 'GiasOnline_WS_GiasWSAggiorna_AgroWS_GiasWSAggiorna', 'http://www.agronica.it/AgronicaWebService/Gias_service.asmx')

if not exists (select 1 from Configurazione_Siti where chiave = 'GiasOnline_WS_GiasWSAggiorna_AgroWS_GiasWSAggiorna_2')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (N'', 6, 'GiasOnline_WS_GiasWSAggiorna_AgroWS_GiasWSAggiorna_2', 'http://www2.agronica.it/AgronicaWebService/Gias_service.asmx')

UPDATE [dbo].[Configurazione_Siti] 
SET [Valore] = 'http://www2.agronica.it/AgronicaWebService/Gias_service.asmx'
WHERE [Sito_Cod] = 6 AND [Chiave] = 'GiasOnline_WS_GiasWSAggiorna_AgroWS_GiasWSAggiorna_2' AND [Valore] = 'http://212.210.214.19/AgronicaWebService/Gias_service.asmx'

if not exists (select 1 from Configurazione_Siti where chiave = '7zLib')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (N'', 0, N'7zLib', N'C:\Program Files\7-Zip\7z.dll')

if not exists (select 1 from Configurazione_Siti where chiave = 'GiasOnline_WS_CapitolatoCliente_AgroWS_CapitolatoCliente')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (N'', 6, N'GiasOnline_WS_CapitolatoCliente_AgroWS_CapitolatoCliente', N'http://www.agronica.it/AgronicaWebService/AgroWS_CapitolatoCliente.asmx')

if not exists (select 1 from Configurazione_Siti where chiave = 'GiasOnline_WS_CapitolatoCliente_AgroWS_CapitolatoCliente_2')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (N'', 6, N'GiasOnline_WS_CapitolatoCliente_AgroWS_CapitolatoCliente_2', N'http://www2.agronica.it/AgronicaWebService/AgroWS_CapitolatoCliente.asmx')

UPDATE [dbo].[Configurazione_Siti] 
SET [Valore] = 'http://www2.agronica.it/AgronicaWebService/AgroWS_CapitolatoCliente.asmx'
WHERE [Sito_Cod] = 6 AND [Chiave] = 'GiasOnline_WS_CapitolatoCliente_AgroWS_CapitolatoCliente_2' AND [Valore] = 'http://212.210.214.19/AgronicaWebService/AgroWS_CapitolatoCliente.asmx'

if not exists (select 1 from Configurazione_Siti where chiave = 'GiasOnline_WS_Disciplinari_AgroWS_Disciplinari')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (N'', 6, N'GiasOnline_WS_Disciplinari_AgroWS_Disciplinari', N'http://www.agronica.it/AgronicaWebService/AgroWS_Disciplinari.asmx')

if not exists (select 1 from Configurazione_Siti where chiave = 'GiasOnline_WS_Disciplinari_AgroWS_Disciplinari_2')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (N'', 6, N'GiasOnline_WS_Disciplinari_AgroWS_Disciplinari_2', N'http://www2.agronica.it/AgronicaWebService/AgroWS_Disciplinari.asmx')

UPDATE [dbo].[Configurazione_Siti] 
SET [Valore] = 'http://www2.agronica.it/AgronicaWebService/AgroWS_Disciplinari.asmx'
WHERE [Sito_Cod] = 6 AND [Chiave] = 'GiasOnline_WS_Disciplinari_AgroWS_Disciplinari_2' AND [Valore] = 'http://212.210.214.19/AgronicaWebService/AgroWS_Disciplinari.asmx'

if not exists (select 1 from Configurazione_Siti where chiave = 'GiasOnline_WS_Fitofarmaci_AgroWS_Fitofarmaci')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (N'', 6, N'GiasOnline_WS_Fitofarmaci_AgroWS_Fitofarmaci', N'http://www.agronica.it/AgronicaWebService/AgroWS_Fitofarmaci.asmx')

if not exists (select 1 from Configurazione_Siti where chiave = 'GiasOnline_WS_Fitofarmaci_AgroWS_Fitofarmaci_2')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (N'', 6, N'GiasOnline_WS_Fitofarmaci_AgroWS_Fitofarmaci_2', N'http://www2.agronica.it/AgronicaWebService/AgroWS_Fitofarmaci.asmx')

UPDATE [dbo].[Configurazione_Siti] 
SET [Valore] = 'http://www2.agronica.it/AgronicaWebService/AgroWS_Fitofarmaci.asmx'
WHERE [Sito_Cod] = 6 AND [Chiave] = 'GiasOnline_WS_Fitofarmaci_AgroWS_Fitofarmaci_2' AND [Valore] = 'http://212.210.214.19/AgronicaWebService/AgroWS_Fitofarmaci.asmx'

if not exists (select 1 from Configurazione_Siti where chiave = 'GiasOnline_WS_PianoConcimazione_AgroAPI_PianoConcimazione')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (N'', 6, N'GiasOnline_WS_PianoConcimazione_AgroAPI_PianoConcimazione', N'http://www.agronica.it/AgronicaWebService/AgroAPI_PianoConcimazione.svc')

if not exists (select 1 from Configurazione_Siti where chiave = 'GiasOnline_WS_PianoConcimazione_AgroAPI_PianoConcimazione_2')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (N'', 6, N'GiasOnline_WS_PianoConcimazione_AgroAPI_PianoConcimazione_2', N'http://www2.agronica.it/AgronicaWebService/AgroAPI_PianoConcimazione.svc')

UPDATE [dbo].[Configurazione_Siti] 
SET [Valore] = 'http://www2.agronica.it/AgronicaWebService/AgroAPI_PianoConcimazione.svc'
WHERE [Sito_Cod] = 6 AND [Chiave] = 'GiasOnline_WS_PianoConcimazione_AgroAPI_PianoConcimazione_2' AND [Valore] = 'http://212.210.214.19/AgronicaWebService/AgroAPI_PianoConcimazione.svc'

if not exists (select 1 from Configurazione_Siti where chiave = 'GiasOnline_WS_Fertilizzanti_IAgroAPI_Fertilizzanti')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (N'', 6, N'GiasOnline_WS_Fertilizzanti_IAgroAPI_Fertilizzanti', N'http://www.agronica.it/AgronicaWebService/AgroAPI_Fertilizzanti.svc')

if not exists (select 1 from Configurazione_Siti where chiave = 'GiasOnline_WS_Fertilizzanti_IAgroAPI_Fertilizzanti_2')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (N'', 6, N'GiasOnline_WS_Fertilizzanti_IAgroAPI_Fertilizzanti_2', N'http://www2.agronica.it/AgronicaWebService/AgroAPI_Fertilizzanti.svc')

UPDATE [dbo].[Configurazione_Siti] 
SET [Valore] = 'http://www2.agronica.it/AgronicaWebService/AgroAPI_Fertilizzanti.svc'
WHERE [Sito_Cod] = 6 AND [Chiave] = 'GiasOnline_WS_Fertilizzanti_IAgroAPI_Fertilizzanti_2' AND [Valore] = 'http://212.210.214.19/AgronicaWebService/AgroAPI_Fertilizzanti.svc'


if not exists (select 1 from Configurazione_Siti where chiave = 'GiasOnline_WS_SpecieVegetali_IAgroAPI_SpecieVegetali')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (N'', 6, N'GiasOnline_WS_SpecieVegetali_IAgroAPI_SpecieVegetali', N'http://www.agronica.it/AgronicaWebService/AgroAPI_SpecieVegetali.svc')

if not exists (select 1 from Configurazione_Siti where chiave = 'GiasOnline_WS_SpecieVegetali_IAgroAPI_SpecieVegetali_2')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (N'', 6, N'GiasOnline_WS_SpecieVegetali_IAgroAPI_SpecieVegetali_2', N'http://www2.agronica.it/AgronicaWebService/AgroAPI_SpecieVegetali.svc')

UPDATE [dbo].[Configurazione_Siti] 
SET [Valore] = 'http://www2.agronica.it/AgronicaWebService/AgroAPI_SpecieVegetali.svc'
WHERE [Sito_Cod] = 6 AND [Chiave] = 'GiasOnline_WS_SpecieVegetali_IAgroAPI_SpecieVegetali_2' AND [Valore] = 'http://212.210.214.19/AgronicaWebService/AgroAPI_SpecieVegetali.svc'



if not exists (select 1 from Configurazione_Siti where chiave = 'GiasOnline_WS_GiasWSAggiorna_AgroWS_GiasWSAggiorna')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (N'', 6, N'GiasOnline_WS_GiasWSAggiorna_AgroWS_GiasWSAggiorna', N'http://www.agronica.it/AgronicaWebService/Gias_service.asmx')

if not exists (select 1 from Configurazione_Siti where chiave = 'GiasOnline_WS_GiasWSAggiorna_AgroWS_GiasWSAggiorna_2')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (N'', 6, N'GiasOnline_WS_GiasWSAggiorna_AgroWS_GiasWSAggiorna_2', N'http://www2.agronica.it/AgronicaWebService/Gias_service.asmx')

UPDATE [dbo].[Configurazione_Siti] 
SET [Valore] = 'http://www2.agronica.it/AgronicaWebService/Gias_service.asmx'
WHERE [Sito_Cod] = 6 AND [Chiave] = 'GiasOnline_WS_GiasWSAggiorna_AgroWS_GiasWSAggiorna_2' AND [Valore] = 'http://212.210.214.19/AgronicaWebService/Gias_service.asmx'

if not exists (select 1 from Configurazione_Siti where chiave = 'GiasOnline_WS_Mappe_Gias_Service')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (N'', 6, N'GiasOnline_WS_Mappe_Gias_Service', N'http://www2.agronica.it/ws_gias_2004/gias_service.asmx')

UPDATE [dbo].[Configurazione_Siti] 
SET [Valore] = 'http://www2.agronica.it/ws_gias_2004/gias_service.asmx'
WHERE [Sito_Cod] = 6 AND [Chiave] = 'GiasOnline_WS_Mappe_Gias_Service' AND [Valore] = 'http://212.210.214.19/ws_gias_2004/gias_service.asmx'

if not exists (select 1 from Configurazione_Siti where chiave = 'GiasOnline_WS_Meteo_Meteo')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (N'', 6, N'GiasOnline_WS_Meteo_Meteo', N'https://meteo.netagronica.it/AgronicaMeteoWebService/Meteo.asmx')

if not exists (select 1 from Configurazione_Siti where chiave = 'GiasOnline_WS_Meteo_Meteo_2')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (N'', 6, N'GiasOnline_WS_Meteo_Meteo_2', N'https://meteo.netagronica.it/AgronicaMeteoWebService/Meteo.asmx')

if not exists (select 1 from Configurazione_Siti where chiave = 'LinkAgronicaAgenda2010')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (N'', 6, N'LinkAgronicaAgenda2010', $(PercorsoLink) + N'/AgronicaAgenda_2010/GestioneRichieste.aspx')

if not exists (select 1 from Configurazione_Siti where chiave = 'LinkAgronicaAnalisi')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (N'', 6, N'LinkAgronicaAnalisi', $(PercorsoLink) + N'/AgronicaAnalisi/GestioneRichieste.aspx')

if not exists (select 1 from Configurazione_Siti where chiave = 'LinkAgronicaAnalisi_2010')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (N'', 6, N'LinkAgronicaAnalisi_2010', $(PercorsoLink) + N'/AgronicaAnalisi_2010/GestioneRichieste.aspx')

if not exists (select 1 from Configurazione_Siti where chiave = 'LinkAgronicaLabControlloQualita')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (N'', 6, N'LinkAgronicaLabControlloQualita', $(PercorsoLink) + N'/AgronicaLabControlloQualita/GestioneRichieste.aspx')

if not exists (select 1 from Configurazione_Siti where chiave = 'LinkAgronicaAudit')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (N'', 6, N'LinkAgronicaAudit', $(PercorsoLink) + N'/AgronicaAudit/GestioneRichieste.aspx')

if not exists (select 1 from Configurazione_Siti where chiave = 'LinkAgronicaBio')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (N'', 6, N'LinkAgronicaBio', $(PercorsoLink) + N'/AgronicaBio/GestioneRichieste.aspx')

if not exists (select 1 from Configurazione_Siti where chiave = 'LinkAgronicaCantine')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (N'', 6, N'LinkAgronicaCantine', $(PercorsoLink) + N'/AgronicaCantine')

if not exists (select 1 from Configurazione_Siti where chiave = 'LinkAgronicaCheckCOOP')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (N'', 6, N'LinkAgronicaCheckCOOP', $(PercorsoLink) + N'/AgronicaCheckCOOP/GestioneRichieste.aspx')

if not exists (select 1 from Configurazione_Siti where chiave = 'LinkAgronicaDiagrammi')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (N'', 6, N'LinkAgronicaDiagrammi', $(PercorsoLink) + N'/AgronicaDiagrammi')

if not exists (select 1 from Configurazione_Siti where chiave = 'LinkAgronicaGlobalGap')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (N'', 6, N'LinkAgronicaGlobalGap', $(PercorsoLink) + N'/AgronicaGlobalGap/GestioneRichieste.aspx')
        
if not exists (select 1 from Configurazione_Siti where chiave = 'LinkAgronicaGrafici')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (N'', 6, N'LinkAgronicaGrafici', $(PercorsoLink) + N'/AgronicaGrafici/')

if not exists (select 1 from Configurazione_Siti where chiave = 'LinkAgronicaMeteo')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (N'', 6, N'LinkAgronicaMeteo', $(PercorsoLink) + N'/AgronicaMeteo/GestioneRichieste.aspx')

if not exists (select 1 from Configurazione_Siti where chiave = 'LinkAgronicaPianiCampionamento')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (N'', 6, N'LinkAgronicaPianiCampionamento', $(PercorsoLink) + N'/AgronicaPianiCampionamento/GestioneRichieste.aspx')

if not exists (select 1 from Configurazione_Siti where chiave = 'LinkAgronicaPianiSemina')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (N'', 6, N'LinkAgronicaPianiSemina', $(PercorsoLink) + N'/AgronicaPianiSemina/GestioneRichieste.aspx')

if not exists (select 1 from Configurazione_Siti where chiave = 'LinkAgronicaPlanning')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (N'', 6, N'LinkAgronicaPlanning', $(PercorsoLink) + N'/AgronicaPlanning/GestioneRichieste.aspx')

if not exists (select 1 from Configurazione_Siti where chiave = 'LinkAgronicaProfilazione')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (N'', 6, N'LinkAgronicaProfilazione', $(PercorsoLink) + N'/AgronicaProfilazione/GestioneRichieste.aspx')

if not exists (select 1 from Configurazione_Siti where chiave = 'LinkAgronicaPua')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (N'', 6, N'LinkAgronicaPua', $(PercorsoLink) + N'/AgronicaPUA/GestioneRichieste.aspx')

if not exists (select 1 from Configurazione_Siti where chiave = 'LinkAgronicaSicurezzaLavoro')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (N'', 6, N'LinkAgronicaSicurezzaLavoro', $(PercorsoLink) + N'/AgronicaSicurezzaLavoro/GestioneRichieste.aspx')

if not exists (select 1 from Configurazione_Siti where chiave = 'LinkAgronicaSincronizzatore')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (N'', 6, N'LinkAgronicaSincronizzatore', $(PercorsoLink) + N'/AgronicaSincronizzatoreWeb/GestioneRichieste.aspx')

if not exists (select 1 from Configurazione_Siti where chiave = 'LinkAgronicaStampe')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (N'', 6, N'LinkAgronicaStampe', $(PercorsoLink) + N'/AgronicaStampe/GestioneRichieste.aspx')

if not exists (select 1 from Configurazione_Siti where chiave = 'LinkAgronicaStampe_2010')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (N'', 6, N'LinkAgronicaStampe_2010', $(PercorsoLink) + N'/AgronicaStampe_2010/GestioneRichieste.aspx')

if not exists (select 1 from Configurazione_Siti where chiave = 'LinkAgronicaView')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (N'', 6, N'LinkAgronicaView', $(PercorsoLink) + N'/AgronicaView/GestioneRichieste.aspx')

if not exists (select 1 from Configurazione_Siti where chiave = 'LinkGiasOnline')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (N'', 6, N'LinkGiasOnline', $(PercorsoLink) + N'/GiasOnline/GestioneRichieste.aspx')

if not exists (select 1 from Configurazione_Siti where chiave = 'LinkGiasOnline_2010')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (N'', 6, N'LinkGiasOnline_2010', $(PercorsoLink) + N'/GiasOnline_2010/GestioneRichieste.aspx')

if not exists (select 1 from Configurazione_Siti where chiave = 'LinkManualeGiasOnline')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (N'', 6, N'LinkManualeGiasOnline', $(PercorsoLink) + N'/GiasOnline/Manuali/Manuale_GiasOnline_01.pdf')

if not exists (select 1 from Configurazione_Siti where chiave = 'LinkPianoConcimazione')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (N'', 6, N'LinkPianoConcimazione', $(PercorsoLink) + N'/PianoConcimazione/GestioneRichieste.aspx')
        
if not exists (select 1 from Configurazione_Siti where chiave = 'LinkPianoConcimazione_2017')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (N'', 6, N'LinkPianoConcimazione_2017', $(PercorsoLink) + N'/AgronicaPianoConcimazione_2017/GestioneRichieste.aspx')

if not exists (select 1 from Configurazione_Siti where chiave = 'LinkProfitosan')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (N'', 6, N'LinkProfitosan', N'https://www2.profitosan.it')

if not exists (select 1 from Configurazione_Siti where chiave = 'LinkProfitosan_WS')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (N'', 6, N'LinkProfitosan_WS', N'https://www2.profitosan.it/ws_profitosan/ProfitosanApp/www/')

if not exists (select 1 from Configurazione_Siti where chiave = 'LinkVisualizzatoreDPI')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (N'', 6, N'LinkVisualizzatoreDPI', N'http://www.net-agree.com/dpi')

if not exists (select 1 from Configurazione_Siti where chiave = 'MailAssistenza')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (N'', 6, N'MailAssistenza', N'assistenza@agronica.it')

UPDATE [Configurazione_Siti]
SET valore = N'assistenza@agronica.it'
where chiave= N'MailAssistenza'
 
if not exists (select 1 from Configurazione_Siti where chiave = 'MailBancheDati')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (N'', 6, N'MailBancheDati', N'banchedati@agronica.it')

if not exists (select 1 from Configurazione_Siti where chiave = 'Assistenza')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (N'', 6, N'Assistenza', N'<a href="mailto:assistenza@agronica.it">assistenza@agronica.it</a>')

if not exists (select 1 from Configurazione_Siti where chiave = 'Ritaglio_Servizio')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (N'', 6, N'Ritaglio_Servizio', N'http://www2.agronica.it/ws_gias_2004/gias_service.asmx')

UPDATE [dbo].[Configurazione_Siti] 
SET [Valore] = 'http://www2.agronica.it/ws_gias_2004/gias_service.asmx'
WHERE [Sito_Cod] = 6 AND [Chiave] = 'Ritaglio_Servizio' AND [Valore] = 'http://212.210.214.19/ws_gias_2004/gias_service.asmx'

if not exists (select 1 from Configurazione_Siti where chiave = 'Ritaglio_Sito')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (N'', 6, N'Ritaglio_Sito', N'http://www.agronica.it/AgroMaps/xMapFinder2/Container.aspx')

if not exists (select 1 from Configurazione_Siti where chiave = 'StarterKit_Pwd')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (N'', 6, N'StarterKit_Pwd', N'E0GCAGE4GCAGDBGCAGE5GC2GDEGD8GDAGC2GE1GC5GD7GDCGD7GCFGD4')

if not exists (select 1 from Configurazione_Siti where chiave = 'Tunnel_Pwd')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (N'', 6, N'Tunnel_Pwd', N'D4GC2GD4GCBGD1GC4GA6G2019GA1G161GA0G90GA5G2018GA3G161GCFGC2GD5GC3GD5GCC')

if not exists (select 1 from Configurazione_Siti where chiave = 'Versione_Pwd')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (N'', 6, N'Versione_Pwd', N'D4GD4GD5GD8GDAGCAGE3GCAGD5GCEGDCGD0')

if not exists (select 1 from Configurazione_Siti where chiave = 'VersioneCodifica')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (N'', 6, N'VersioneCodifica', N'NEW')

if not exists (select 1 from Configurazione_Siti where chiave = 'WebService_GiasAlarm')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (N'', 6, N'WebService_GiasAlarm', N'http://www.agronica.it/GiasAlarmWebservice/GIASAlarm_Web.asmx')

if not exists (select 1 from Configurazione_Siti where chiave = 'WebServiceMappe_RepositoryGIS')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (N'', 6, N'WebServiceMappe_RepositoryGIS', N'http://www.agronica.it/Mappe/')

if not exists (select 1 from Configurazione_Siti where chiave = 'WS_Timeout')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (N'', 6, N'WS_Timeout', N'10000')

if not exists (select 1 from Configurazione_Siti where chiave = 'user_smtp')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (N'', 6, N'user_smtp', N'')

if not exists (select 1 from Configurazione_Siti where chiave = 'password_smtp')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (N'', 6, N'password_smtp', N'')

if not exists (select 1 from Configurazione_Siti where chiave = 'enablessl_smtp')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (N'', 6, N'enablessl_smtp', N'')

if not exists (select 1 from Configurazione_Siti where chiave = 'FiltroneBootstrap')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (N'', 6, N'FiltroneBootstrap', N'false')

if not exists (select 1 from Configurazione_Siti where chiave = 'Stampe_Massive_Filtro')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (N'', 6, N'Stampe_Massive_Filtro', N'false')

if not exists (select 1 from Configurazione_Siti where chiave = 'ListaLoghiPersonalizzati')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) 
VALUES (N'', 6, N'ListaLoghiPersonalizzati', N'')

if not exists (select 1 from Configurazione_Siti where chiave = 'ListaStiliPersonalizzati')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) 
VALUES (N'', 6, N'ListaStiliPersonalizzati', N'')

if not exists (select 1 from Configurazione_Siti where chiave = 'ListaStiliPersonalizzati_2022')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) 
VALUES (N'', 6, N'ListaStiliPersonalizzati_2022', N'')

if not exists (select 1 from Configurazione_Siti where chiave = 'ListaStiliPersonalizzati_NG')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) 
VALUES (N'', 6, N'ListaStiliPersonalizzati_NG', N'')

if not exists (select 1 from Configurazione_Siti where chiave = 'VersioneKendo')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) 
VALUES (N'', 6, N'VersioneKendo', N'')

-- Svuotamento forzato record
UPDATE Configurazione_Siti SET [Valore] = N''
WHERE [PivaSuperUser] = '' AND [Sito_Cod] = 6 AND [Chiave] = 'VersioneKendo' AND [Valore] = '2017.3.913'
    
if not exists (select 1 from Configurazione_Siti where chiave = 'GiasOnline_WS_AgronicaWebApiProfilatore')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) 
VALUES (N'', 6, N'GiasOnline_WS_AgronicaWebApiProfilatore', N'http://localhost/AgronicaWebApiProfilatore/ProfilatoreUtenze.svc')	

if not exists (select 1 from Configurazione_Siti where chiave = 'AgronicaWebApiProfilatore_Token')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) 
VALUES (N'', 6, N'AgronicaWebApiProfilatore_Token', N'')	

if not exists (select 1 from Configurazione_Siti where chiave = 'LinkHomePageGlobale')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) 
VALUES (N'', 6, N'LinkHomePageGlobale', N'')


if not exists (select 1 from Configurazione_Siti where chiave = 'login_MaxNumeroTentativiAccesso')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) 
VALUES (N'', 6, N'login_MaxNumeroTentativiAccesso', N'-1')

if not exists (select 1 from Configurazione_Siti where chiave = 'paginaIndex_PnlPersonalizzatoBasso')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) 
VALUES (N'', 6, N'paginaIndex_PnlPersonalizzatoBasso', N'')


if not exists (select 1 from Configurazione_Siti where chiave = 'Antivirus_Documentale')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) 
VALUES (N'', 6, 'Antivirus_Documentale', '[{"antivirus_on":"0","antivirus_id":"1","antivirus_key":"74773157-a8b6-487f-8759-cc5ebb854ff8"}]')



--Parametri SAML / SPID
/*
paginaIndex_LoginSPID
paginaIndex_LoginCFG
SPID_AssertionConsumerServiceURL
SPID_CERTIFICATE_NAME
SPID_CERTIFICATE_NAME_CALLBACK
SPID_DOMAIN_VALUE
SPID_IDP
SPID_LoginURL
SPID_ENVIROMENT
SPID_CLAIMS_ARRAY

*/


if not exists (select 1 from Configurazione_Siti where chiave = 'paginaIndex_LoginSPID')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) 
VALUES (N'', 6, N'paginaIndex_LoginSPID', N'false')

if not exists (select 1 from Configurazione_Siti where chiave = 'paginaIndex_LoginSPIDCFG')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) 
VALUES (N'', 6, N'paginaIndex_LoginSPIDCFG', N'{"PathLogo": "", "Testo": ""}')

if not exists (select 1 from Configurazione_Siti where chiave = 'SPID_AgroSamlConfig')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) 
VALUES (N'', 6, N'SPID_AgroSamlConfig', N'{ "ComparisonType": 0, "SignAssertion": true, "VerifyResponse": true }')

if not exists (select 1 from Configurazione_Siti where chiave = 'SPID_AssertionConsumerServiceURL')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) 
VALUES (N'', 6, N'SPID_AssertionConsumerServiceURL', N'http://localhost:52551/AgronicaAgenda/index.aspx')

if not exists (select 1 from Configurazione_Siti where chiave = 'SPID_CERTIFICATE_NAME')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) 
VALUES (N'', 6, N'SPID_CERTIFICATE_NAME', N'agro-signing')

if not exists (select 1 from Configurazione_Siti where chiave = 'SPID_CERTIFICATE_NAME_CALLBACK')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) 
VALUES (N'', 6, N'SPID_CERTIFICATE_NAME_CALLBACK', N'agro-signing')

if not exists (select 1 from Configurazione_Siti where chiave = 'SPID_DOMAIN_VALUE')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) 
VALUES (N'', 6, N'SPID_DOMAIN_VALUE', N'https://localhost/Shibboleth')

if not exists (select 1 from Configurazione_Siti where chiave = 'SPID_IDP')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) 
VALUES (N'', 6, N'SPID_IDP', N'{ "entityId": "", "organizationName": "", "organizationDisplayName": "", "organizationUrl": "", "singleSignOnServiceUrl": "", "singleLogoutServiceUrl": "", "subjectNameIdRemoveText": "" }')

if not exists (select 1 from Configurazione_Siti where chiave = 'SPID_LoginURL')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) 
VALUES (N'', 6, N'SPID_LoginURL', N'')

if not exists (select 1 from Configurazione_Siti where chiave = 'SPID_LogoutURL')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) 
VALUES (N'', 6, N'SPID_LogoutURL', N'')

if not exists (select 1 from Configurazione_Siti where chiave = 'SPID_WindowsFindBy')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) 
VALUES (N'', 6, N'SPID_WindowsFindBy', N'')

if not exists (select 1 from Configurazione_Siti where chiave = 'SPID_WindowsFindBy_Callback')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) 
VALUES (N'', 6, N'SPID_WindowsFindBy_Callback', N'')

if not exists (select 1 from Configurazione_Siti where chiave = 'SPID_ENVIROMENT')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) 
VALUES (N'', 6, N'SPID_ENVIROMENT', N'')

if not exists (select 1 from Configurazione_Siti where chiave = 'SPID_CLAIMS_ARRAY')
    INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) 
    VALUES (N'', 6, N'SPID_CLAIMS_ARRAY', N'{ "TipoDiSAMLClaimPerRiconoscereUtente": 1, "ListaClaims": [ 
        { "SAMLKey": "nome", "GiasKey": "nome"}, 
        { "SAMLKey": "cognome", "GiasKey": "cognome"}, 
        { "SAMLKey": "CodiceFiscale", "GiasKey": "CodiceFiscale"}
    ]}')

if not exists (select 1 from Configurazione_Siti where chiave = 'MailFrom_smtp')
    INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) 
    VALUES (N'', 6, N'MailFrom_smtp', N'')

if not exists (select 1 from Configurazione_Siti where chiave = 'ClientSMTP')
    INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore])
    VALUES (N'', 6, N'ClientSMTP', N'')

if not exists (select 1 from Configurazione_Siti where chiave = 'ClientSMTP_Porta')
    INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore])
    VALUES (N'', 6, N'ClientSMTP_Porta', N'')

if not exists (select 1 from Configurazione_Siti where chiave = 'googlemaps')
    INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore])
    VALUES (N'', 6, N'googlemaps', N'')

if not exists (select 1 from Configurazione_Siti where chiave = 'VersioneMaster')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (N'', 6, N'VersioneMaster', N'')

if not exists (select 1 from Configurazione_Siti where chiave = 'VersioneHeader')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (N'', 6, N'VersioneHeader', N'')

if not exists (select 1 from Configurazione_Siti where chiave = 'VersioneLogin')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (N'', 6, N'VersioneLogin', N'')

if not exists (select 1 from Configurazione_Siti where chiave = 'UsaCoreAPI')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (N'', 6, N'UsaCoreAPI', N'false')

if not exists (select 1 from Configurazione_Siti where chiave = 'LinkProfitosan_2023')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (N'', 6, N'LinkProfitosan_2023', N'https://app.profitosan.it/')

if not exists (select 1 from Configurazione_Siti where chiave = 'LinkGiasBase')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (N'', 6, N'LinkGiasBase', $(PercorsoLink) + N'/GiasBase')

IF NOT EXISTS(Select 1 from dbo.Configurazione_Siti where Chiave = 'AgronicaSpecialNavigationCFG')
INSERT INTO [Configurazione_Siti] 
([PivaSuperUser], [Sito_Cod], [Chiave], [Valore])
VALUES ('', 6, 'AgronicaSpecialNavigationCFG','{ "AgronicaSpecialNavigationTokenValue" : "1"}')

-- Chiave personalizzazioniRegioneUmbria per personalizzazioni grafiche Regione Umbria
if not exists (select 1 from Configurazione_Siti where chiave = 'personalizzazioniRegioneUmbria')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (N'', 6, N'personalizzazioniRegioneUmbria', N'')

-- Chiave LinkAPIHubAgea per export QdC verso Agea
if not exists (select 1 from Configurazione_Siti where chiave = 'LinkAPIHubAgea')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) 
    VALUES (N'', 0, N'LinkAPIHubAgea', N'')

-- Chiave APIKeyHubAgea per export QdC verso Agea
if not exists (select 1 from Configurazione_Siti where chiave = 'APIKeyHubAgea')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) 
    VALUES (N'', 0, N'APIKeyHubAgea', N'')

IF NOT EXISTS(Select 1 from Configurazione_Siti where Chiave = 'MinutiValiditaTokenRecuperoPassword')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore])
    VALUES (N'', 6, 'MinutiValiditaTokenRecuperoPassword','30')

IF NOT EXISTS(Select 1 from Configurazione_Siti where Chiave = 'password_smtp_isEncrypted')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore])
    VALUES (N'', 6, 'password_smtp_isEncrypted','0')

IF NOT EXISTS(Select 1 from Configurazione_Siti where Chiave = 'cr')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore])
    VALUES (N'', 6, 'cr', NEWID())

IF NOT EXISTS(Select 1 from Configurazione_Siti where Chiave = 'UserPwdConnectionString_toCrypt')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore])
    VALUES (N'', 6, 'UserPwdConnectionString_toCrypt', N'false')

IF NOT EXISTS(Select 1 from Configurazione_Siti where Chiave = 'NumeroMassimoRigheEstraibiliFiltroRicerca')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore])
    VALUES (N'', 6, 'NumeroMassimoRigheEstraibiliFiltroRicerca', N'99999')

--LAST SuperServer (!! INSERIRE SOPRA !!)



-- LASCIARE COME ULTIMA ISTRUZIONE:  Chiavi visibili lato client
UPDATE [dbo].[Configurazione_Siti] SET Visibilita = 1
WHERE chiave IN 
('servizioAutorizzazioneHubMeteo',
'AgroProfilazione_MaxUtentiCaricatiDefault',
'BloccaCoreWS_NG',
'personalizzazioniRegioneUmbria',
'Blocco_Inserimento_PIVA_Impresa',
'ListaStiliPersonalizzati_NG',
'LinkAgronicaGiasNG',
'MenuAgendaNG',
'OperazioniAgendaNG',
'googlemaps')

-- ATTENZIONE! lasciare il go per ultimo!!!!
GO



USE $(ServerDB)
GO


/* ########################################################################################## */
/* ##########      $(ServerDB) Tabelle      ################################################# */
/* ########################################################################################## */

/* ----------------------------------------------------------------------------------------------------------- */
/* ---------------------- Configurazione_Siti  ELIMINO LE CHIAVI CHE SONO IN SUPERSERVER  --------------------- */
/* ----------------------------------------------------------------------------------------------------------- */
if $(EliminaChiaviServerSeSulSuperserver)='true'
delete 
FROM         Configurazione_Siti
WHERE     (Chiave IN
                          (SELECT     Chiave COLLATE Latin1_General_CI_AS AS Expr1
                            FROM          $(SuperServer).dbo.Configurazione_Siti AS configurazione_siti_1))
GO

/* ----------------------------------------------------------------- */
/* ------ Configurazione_Siti (Eliminazione vecchie chiavi)  ------- */
/* ----------------------------------------------------------------- */
print ('migra tabella: Configurazione_Siti (Eliminazione vecchie chiavi)')

-- Queste sono vecchie chiavi che erano state create ma mai lette e non servono più, 
-- quindi le cancelliamo (per il momento lasciamo il relativo insert commenatto 
-- nel caso debbano resuscitare [saranno anche da spostare più sotto])

IF EXISTS (SELECT 1 FROM Configurazione_Siti WHERE chiave = 'Sincro_ImportAgrea_Tutte')
BEGIN
    DELETE FROM Configurazione_Siti WHERE chiave = 'Sincro_ImportAgrea_Tutte'
    --INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
    --        (@PivaSuperUser, 16, N'Sincro_ImportAgrea_Tutte', '1')
END

IF EXISTS (SELECT 1 FROM Configurazione_Siti WHERE chiave = 'Sincro_ImportAnagrafeER_Tutte')
BEGIN
    DELETE FROM Configurazione_Siti WHERE chiave = 'Sincro_ImportAnagrafeER_Tutte'
    --INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
    --        (@PivaSuperUser, 16, N'Sincro_ImportAnagrafeER_Tutte', '1')
END

IF EXISTS (SELECT 1 FROM Configurazione_Siti WHERE chiave = 'Sincro_ImportAnagrafeER_Password_Cia')
BEGIN
    DELETE FROM Configurazione_Siti WHERE chiave = 'Sincro_ImportAnagrafeER_Password_Cia'
    --INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
    --        (@PivaSuperUser, 16, N'Sincro_ImportAnagrafeER_Password_Cia', '!CHESIACAARINA*99')
END

IF EXISTS (SELECT 1 FROM Configurazione_Siti WHERE chiave = 'Sincro_ImportAgrea_Password_Cia')
BEGIN
    DELETE FROM Configurazione_Siti WHERE chiave = 'Sincro_ImportAgrea_Password_Cia'
    --INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
    --        (@PivaSuperUser, 16, N'Sincro_ImportAgrea_Password_Cia', '2010*2011')
END

IF EXISTS (SELECT 1 FROM Configurazione_Siti WHERE chiave = 'Sincro_ImportAgrea_Password_Coldiretti')
BEGIN
    DELETE FROM Configurazione_Siti WHERE chiave = 'Sincro_ImportAgrea_Password_Coldiretti'
    --INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
    --        (@PivaSuperUser, 16, N'Sincro_ImportAgrea_Password_Coldiretti', '4909cd38171c62cf4627308ea9d7f56bd6b44009')
END

IF EXISTS (SELECT 1 FROM Configurazione_Siti WHERE chiave = 'Sincro_ImportAnagrafeER_Password_LegaCoop')
BEGIN
    DELETE FROM Configurazione_Siti WHERE chiave = 'Sincro_ImportAnagrafeER_Password_LegaCoop'
    --INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
    --        (@PivaSuperUser, 16, N'Sincro_ImportAnagrafeER_Password_LegaCoop', 'CAALEGACOOP.16')
END

IF EXISTS (SELECT 1 FROM Configurazione_Siti WHERE chiave = 'Sincro_ImportAgrea_Password_Confagricoltura')
BEGIN
    DELETE FROM Configurazione_Siti WHERE chiave = 'Sincro_ImportAgrea_Password_Confagricoltura'
    --INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
    --        (@PivaSuperUser, 16, N'Sincro_ImportAgrea_Password_Confagricoltura', 'cf3c9d742c056d6a6bc212ab15bee938ee873798')
END

IF EXISTS (SELECT 1 FROM Configurazione_Siti WHERE chiave = 'Sincro_ImportAgrea_Password_LegaCoop')
BEGIN
    DELETE FROM Configurazione_Siti WHERE chiave = 'Sincro_ImportAgrea_Password_LegaCoop'
    --INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
    --        (@PivaSuperUser, 16, N'Sincro_ImportAgrea_Password_LegaCoop', 'ciaox!11')
END

IF EXISTS (SELECT 1 FROM Configurazione_Siti WHERE chiave = 'Sincro_ImportAnagrafeER_Password_Coldiretti')
BEGIN
    DELETE FROM Configurazione_Siti WHERE chiave = 'Sincro_ImportAnagrafeER_Password_Coldiretti'
    --INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
    --        (@PivaSuperUser, 16, N'Sincro_ImportAnagrafeER_Password_Coldiretti', 'COLDIRETTI_005')
END

IF EXISTS (SELECT 1 FROM Configurazione_Siti WHERE chiave = 'Sincro_ImportAnagrafeER_Password_Confagricoltura')
BEGIN
    DELETE FROM Configurazione_Siti WHERE chiave = 'Sincro_ImportAnagrafeER_Password_Confagricoltura'
    --INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
    --        (@PivaSuperUser, 16, N'Sincro_ImportAnagrafeER_Password_Confagricoltura', 'GALLICCHIO.1')
END

IF EXISTS (SELECT 1 FROM Configurazione_Siti WHERE chiave = 'Sincro_ImportAnagrafeER_Link')
BEGIN
    DELETE FROM Configurazione_Siti WHERE chiave = 'Sincro_ImportAnagrafeER_Link'
    --INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
    --        (@PivaSuperUser, 16, N'Sincro_ImportAnagrafeER_Link', 'https://agri.regione.emilia-romagna.it/MandatoWS/services/Fascicolo')
END

IF EXISTS (SELECT 1 FROM Configurazione_Siti WHERE chiave = 'Sincro_ImportAnagrafeER_Password')
BEGIN
    DELETE FROM Configurazione_Siti WHERE chiave = 'Sincro_ImportAnagrafeER_Password'
    --INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
    --        (@PivaSuperUser, 16, N'Sincro_ImportAnagrafeER_Password', '')
END

IF EXISTS (SELECT 1 FROM Configurazione_Siti WHERE chiave = 'Sincro_ImportAnagrafeER_Username')
BEGIN
    DELETE FROM Configurazione_Siti WHERE chiave = 'Sincro_ImportAnagrafeER_Username'
    --INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
    --        (@PivaSuperUser, 16, N'Sincro_ImportAnagrafeER_Username', '')
END

IF EXISTS (SELECT 1 FROM Configurazione_Siti WHERE chiave = 'Sincro_ImportAnagrafeER_Tipo')
BEGIN
    DELETE FROM Configurazione_Siti WHERE chiave = 'Sincro_ImportAnagrafeER_Tipo'
    --INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
    --        (@PivaSuperUser, 16, N'Sincro_ImportAnagrafeER_Tipo', 'ws')
END

IF EXISTS (SELECT 1 FROM Configurazione_Siti WHERE chiave = 'Sincro_ImportAnagrafeER_Username_Cia')
BEGIN
    DELETE FROM Configurazione_Siti WHERE chiave = 'Sincro_ImportAnagrafeER_Username_Cia'
    --INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
    --        (@PivaSuperUser, 16, N'Sincro_ImportAnagrafeER_Username_Cia', 'WS_CAACIA')
END

IF EXISTS (SELECT 1 FROM Configurazione_Siti WHERE chiave = 'Sincro_ImportAnagrafeER_Username_Coldiretti')
BEGIN
    DELETE FROM Configurazione_Siti WHERE chiave = 'Sincro_ImportAnagrafeER_Username_Coldiretti'
    --INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
    --        (@PivaSuperUser, 16, N'Sincro_ImportAnagrafeER_Username_Coldiretti', 'WS_CAACOLDIRETTI')
END

IF EXISTS (SELECT 1 FROM Configurazione_Siti WHERE chiave = 'Sincro_ImportAnagrafeER_Username_Confagricoltura')
BEGIN
    DELETE FROM Configurazione_Siti WHERE chiave = 'Sincro_ImportAnagrafeER_Username_Confagricoltura'
    --INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
    --        (@PivaSuperUser, 16, N'Sincro_ImportAnagrafeER_Username_Confagricoltura', 'WS_CAACONFAGRI')
END


IF EXISTS (SELECT 1 FROM Configurazione_Siti WHERE chiave = 'Sincro_ImportAnagrafeER_Username_LegaCoop')
BEGIN
    DELETE FROM Configurazione_Siti WHERE chiave = 'Sincro_ImportAnagrafeER_Username_LegaCoop'
    --INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
    --        (@PivaSuperUser, 16, N'Sincro_ImportAnagrafeER_Username_LegaCoop', 'WS_CAALEGACOOP')
END

IF EXISTS (SELECT 1 FROM Configurazione_Siti WHERE chiave = 'Sincro_ImportAgrea_Username_Cia')
BEGIN
    DELETE FROM Configurazione_Siti WHERE chiave = 'Sincro_ImportAgrea_Username_Cia'
    --INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
    --        (@PivaSuperUser, 16, N'Sincro_ImportAgrea_Username_Cia', 'ws_cia')
END

IF EXISTS (SELECT 1 FROM Configurazione_Siti WHERE chiave = 'Sincro_ImportAgrea_Username_Coldiretti')
BEGIN
    DELETE FROM Configurazione_Siti WHERE chiave = 'Sincro_ImportAgrea_Username_Coldiretti'
    --INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
    --        (@PivaSuperUser, 16, N'Sincro_ImportAgrea_Username_Coldiretti', 'ws_coldiretti')
END

IF EXISTS (SELECT 1 FROM Configurazione_Siti WHERE chiave = 'Sincro_ImportAgrea_Username_Confagricoltura')
BEGIN
    DELETE FROM Configurazione_Siti WHERE chiave = 'Sincro_ImportAgrea_Username_Confagricoltura'
    --INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
    --        (@PivaSuperUser, 16, N'Sincro_ImportAgrea_Username_Confagricoltura', 'ws_confagri')
END

IF EXISTS (SELECT 1 FROM Configurazione_Siti WHERE chiave = 'Sincro_ImportAgrea_Username_LegaCoop')
BEGIN
    DELETE FROM Configurazione_Siti WHERE chiave = 'Sincro_ImportAgrea_Username_LegaCoop'
    --INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
    --        (@PivaSuperUser, 16, N'Sincro_ImportAgrea_Username_LegaCoop', 'ws_legacoop')
END

GO

/* ----------------------------------------------------------------- */
/* ---------------------- Configurazione_Siti  --------------------- */
/* ----------------------------------------------------------------- */
print ('migra tabella: Configurazione_Siti')

declare @PivaSuperUser as varchar(25)
set @PivaSuperUser = (SELECT  top 1 [USER] FROM utentiximprese)




if not exists (select 1 from Configurazione_Siti where chiave = 'CERTDEST_NOME')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 0, N'CERTDEST_NOME', N'CN=BERKELIO.agronica.it')

if not exists (select 1 from Configurazione_Siti where chiave = 'CERTSIG_NOME')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 0, N'CERTSIG_NOME', N'CN=BERKELIO.agronica.it')

if not exists (select 1 from Configurazione_Siti where chiave = 'CodificaFILE')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 0, N'CodificaFILE', N'ASCII')

if not exists (select 1 from Configurazione_Siti where chiave = 'CRIPTAFILE')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 0, N'CRIPTAFILE', N'false')

if not exists (select 1 from Configurazione_Siti where chiave = 'DatiNonTrovati')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 0, N'DatiNonTrovati', N'Non sono stati trovati movimenti associati ai parametri di ricerca indicati')

if not exists (select 1 from Configurazione_Siti where chiave = 'DEPEURO')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 0, N'DEPEURO', N'200')

if not exists (select 1 from Configurazione_Siti where chiave = 'DEPEURORIPORTO')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 0, N'DEPEURORIPORTO', N'0')

if not exists (select 1 from Configurazione_Siti where chiave = 'estensioneFile')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 0, N'estensioneFile', N'.P7M')

if not exists (select 1 from Configurazione_Siti where chiave = 'FIRMAVERIFICAPATH')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 0, N'FIRMAVERIFICAPATH', N'C:\Users\costa\Documents\FirmaVerifica2.2.0.0\dafirmare\')

if not exists (select 1 from Configurazione_Siti where chiave = 'FIRMAVERIFICAPATH_EXE')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 0, N'FIRMAVERIFICAPATH_EXE', N'C:\Users\costa\Documents\FirmaVerifica2.2.0.0\dafirmare\FirmaVerifica.exe')


if not exists (select 1 from Configurazione_Siti where chiave = 'FTP_Address')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 0, N'FTP_Address', N'ftp://localhost/TestAcciseNonCancellare/')

if not exists (select 1 from Configurazione_Siti where chiave = 'FTP_Filtri')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 0, N'FTP_Filtri', N'.J')

if not exists (select 1 from Configurazione_Siti where chiave = 'FTP_INVIA')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 0, N'FTP_INVIA', N'false')

if not exists (select 1 from Configurazione_Siti where chiave = 'FTP_Password')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 0, N'FTP_Password', N'')

if not exists (select 1 from Configurazione_Siti where chiave = 'FTP_USEBINARY')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 0, N'FTP_USEBINARY', N'true')

if not exists (select 1 from Configurazione_Siti where chiave = 'FTP_Username')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 0, N'FTP_Username', N'FTPGuest')

if not exists (select 1 from Configurazione_Siti where chiave = 'gzcpio32')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 0, N'gzcpio32', N'd:\www_manager\ManagerAcciseDAA\Allegati\gzcpio32.exe')

if not exists (select 1 from Configurazione_Siti where chiave = 'IMPEGNACAUZ')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 0, N'IMPEGNACAUZ', N'0,1')

if not exists (select 1 from Configurazione_Siti where chiave = 'LABELSTATODOC')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 0, N'LABELSTATODOC', N' --- Stato del documento eDAA:')

if not exists (select 1 from Configurazione_Siti where chiave = 'LanToAgenda2010')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 0, N'LanToAgenda2010', N'true')

if not exists (select 1 from Configurazione_Siti where chiave = 'LanToAgenda2010_BrowserMode')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 0, N'LanToAgenda2010_BrowserMode', N'2')

if not exists (select 1 from Configurazione_Siti where chiave = 'LanToCartografia2010')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 0, N'LanToCartografia2010', N'true')

if not exists (select 1 from Configurazione_Siti where chiave = 'LanToCartografia2010_BrowserMode')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 0, N'LanToCartografia2010_BrowserMode', N'2')

if not exists (select 1 from Configurazione_Siti where chiave = 'LanToPlanning')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 0, N'LanToPlanning', N'true')

if not exists (select 1 from Configurazione_Siti where chiave = 'LanToPlanning_BrowserMode')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 0, N'LanToPlanning_BrowserMode', N'2')

if not exists (select 1 from Configurazione_Siti where chiave = 'LanToStampe2010_BrowserMode')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 0, N'LanToStampe2010_BrowserMode', N'2')

if not exists (select 1 from Configurazione_Siti where chiave = 'LanToWebSiteBasePath')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 0, N'LanToWebSiteBasePath', $(LanToWebSiteBasePath))

if not exists (select 1 from Configurazione_Siti where chiave = 'LenARC')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 0, N'LenARC', N'21')

if not exists (select 1 from Configurazione_Siti where chiave = 'lenARC_Prog')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 0, N'lenARC_Prog', N'4')

if not exists (select 1 from Configurazione_Siti where chiave = 'lenConfTipoRisp')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 0, N'lenConfTipoRisp', N'1')

if not exists (select 1 from Configurazione_Siti where chiave = 'msgConfAnnulla')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 0, N'msgConfAnnulla', N'D')

if not exists (select 1 from Configurazione_Siti where chiave = 'msgConfCambioDest')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 0, N'msgConfCambioDest', N'E')

if not exists (select 1 from Configurazione_Siti where chiave = 'MSGERRCAUZIONE')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 0, N'MSGERRCAUZIONE', N'Errore nel calcolo della cauzione. Verificare la correttezza dei dati inseriti nella configurazione: <br/> <ul><li>Esiste un prodotto configurato </li><li>esiste una configurazione per la nazione del destinatario</li><li>gli importi sono stati imputa')

if not exists (select 1 from Configurazione_Siti where chiave = 'msgIE815ok')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 0, N'msgIE815ok', N'A')

if not exists (select 1 from Configurazione_Siti where chiave = 'Path_Directory_Loghi_Cliente')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 0, N'Path_Directory_Loghi_Cliente', $(PercorsoCartellaConfigSiti) + N'LOGHI\')

if not exists (select 1 from Configurazione_Siti where chiave = 's2s_Sample')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 0, N's2s_Sample', N'D:\www_manager\ManagerAcciseDAA\Allegati\s2s_sample')

if not exists (select 1 from Configurazione_Siti where chiave = 'SISPACPATH')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 0, N'SISPACPATH', N'%userprofile%\\Documents\\SISPAC\\')

if not exists (select 1 from Configurazione_Siti where chiave = 'StartARC')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 0, N'StartARC', N'153')

if not exists (select 1 from Configurazione_Siti where chiave = 'StartARC_Prog')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 0, N'StartARC_Prog', N'148')

if not exists (select 1 from Configurazione_Siti where chiave = 'startConfTipoRisp')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 0, N'startConfTipoRisp', N'147')

if not exists (select 1 from Configurazione_Siti where chiave = 'URLPROVA')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 0, N'URLPROVA', N'https://telematicoprova.agenziadogane.it/TelematicoInvioFileWEB/GestireInvioFileServlet?UC=1&SC=1&ST=1')

if not exists (select 1 from Configurazione_Siti where chiave = 'URLREALE')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 0, N'URLREALE', N'https://telematico.agenziadogane.it/TelematicoInvioFileWEB/GestireInvioFileServlet?UC=1&SC=1&ST=1')

if not exists (select 1 from Configurazione_Siti where chiave = 'Agenda2010')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 6, N'Agenda2010', N'true')

if not exists (select 1 from Configurazione_Siti where chiave = 'AgendaVisualizzazioneDefault')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 6, N'AgendaVisualizzazioneDefault', N'2')

if not exists (select 1 from Configurazione_Siti where chiave = 'AgronicaCore_DirectoryLOG')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 6, N'AgronicaCore_DirectoryLOG', $(PercorsoCartellaConfigSiti) + N'LOG')

if not exists (select 1 from Configurazione_Siti where chiave = 'AgronicaCore_FileNameLOG')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 6, N'AgronicaCore_FileNameLOG', $(AgronicaCore_FileNameLOG))

if not exists (select 1 from Configurazione_Siti where chiave = 'AgronicaCore_Flag_CancellazioneLogica')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 6, N'AgronicaCore_Flag_CancellazioneLogica', N'0')

if not exists (select 1 from Configurazione_Siti where chiave = 'AgronicaCore_Flag_Visibilita')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 6, N'AgronicaCore_Flag_Visibilita', N'1')

if not exists (select 1 from Configurazione_Siti where chiave = 'Analisi2010')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 6, N'Analisi2010', N'true')

if not exists (select 1 from Configurazione_Siti where chiave = 'AttivaStampePersonalizzate')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 6, N'AttivaStampePersonalizzate', N'true')

if not exists (select 1 from Configurazione_Siti where chiave = 'Cartografia_LarghezzaMappaMillimetri')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 6, N'Cartografia_LarghezzaMappaMillimetri', N'138')

if not exists (select 1 from Configurazione_Siti where chiave = 'Cartografia_NomeMappaBase')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 6, N'Cartografia_NomeMappaBase', N'!RegioneER')

if not exists (select 1 from Configurazione_Siti where chiave = 'Cartografia_SoloMappeUtente')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 6, N'Cartografia_SoloMappeUtente', N'true')

if not exists (select 1 from Configurazione_Siti where chiave = 'Codici_Analisi_PDC_x_Fruttagel')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 6, N'Codici_Analisi_PDC_x_Fruttagel', N'false')

if not exists (select 1 from Configurazione_Siti where chiave = 'ColoreNodoBloccatoAlberoImprese')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 6, N'ColoreNodoBloccatoAlberoImprese', N'Red')

if not exists (select 1 from Configurazione_Siti where chiave = 'ColoreNodoScadutoAlberoImprese')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 6, N'ColoreNodoScadutoAlberoImprese', N'DimGray')

if not exists (select 1 from Configurazione_Siti where chiave = 'Connessione_ONLINE_DPI')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 6, N'Connessione_ONLINE_DPI', N'cnGIAS_DPI')

if not exists (select 1 from Configurazione_Siti where chiave = 'Connessione_ONLINE_Server')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 6, N'Connessione_ONLINE_Server', N'')

if not exists (select 1 from Configurazione_Siti where chiave = 'Connessione_ONLINE_Utenti')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 6, N'Connessione_ONLINE_Utenti', N'')

if not exists (select 1 from Configurazione_Siti where chiave = 'DescrizioneServer')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 6, N'DescrizioneServer', N'GranFrutta Zani')

if not exists (select 1 from Configurazione_Siti where chiave = 'DettagliAppezzamentiAlberoImprese')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 6, N'DettagliAppezzamentiAlberoImprese', N'true')

if not exists (select 1 from Configurazione_Siti where chiave = 'DettagliImpiantiAlberoImprese')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 6, N'DettagliImpiantiAlberoImprese', N'true')

if not exists (select 1 from Configurazione_Siti where chiave = 'enumStampe2010')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 6, N'enumStampe2010', N'5,6,7,67,68,99,103,149,158,159,163,63,61,62,55,56,57,64,65,102,104')
UPDATE [dbo].[Configurazione_Siti]	
SET 	Valore =	 $(enumStampe2010)
WHERE chiave = 'enumStampe2010'

if not exists (select 1 from Configurazione_Siti where chiave = 'EspandiNodoCatasto')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 6, N'EspandiNodoCatasto', N'false')

if not exists (select 1 from Configurazione_Siti where chiave = 'Flag_DisciplinareAttivo')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 6, N'Flag_DisciplinareAttivo', N'true')

if not exists (select 1 from Configurazione_Siti where chiave = 'Flag_DisciplinarePrivato')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 6, N'Flag_DisciplinarePrivato', N'false')

if not exists (select 1 from Configurazione_Siti where chiave = 'Flag_GestioneAnalisiAttivo')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 6, N'Flag_GestioneAnalisiAttivo', N'true')

if not exists (select 1 from Configurazione_Siti where chiave = 'Flag_Messaggio')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 6, N'Flag_Messaggio', N'true')

if not exists (select 1 from Configurazione_Siti where chiave = 'Flag_Mirror')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 6, N'Flag_Mirror', N'SYSTEM_FRAMEWORK')

if not exists (select 1 from Configurazione_Siti where chiave = 'Flag_SoglieAttive')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 6, N'Flag_SoglieAttive', N'true')

if not exists (select 1 from Configurazione_Siti where chiave = 'Flag_WS_Fitofarmaci_Remoto')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 6, N'Flag_WS_Fitofarmaci_Remoto', N'true')

if not exists (select 1 from Configurazione_Siti where chiave = 'Fuso')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 6, N'Fuso', N'32')

if not exists (select 1 from Configurazione_Siti where chiave = 'GestioneAllegati_Repository')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 6, N'GestioneAllegati_Repository', $(PercorsoCartellaConfigSiti) + N'AgronicaStampe_Allegati\')

if not exists (select 1 from Configurazione_Siti where chiave = 'GestioneCartografia_Repository')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 6, N'GestioneCartografia_Repository', $(PercorsoCartellaConfigSiti) + N'Mappe\')

if not exists (select 1 from Configurazione_Siti where chiave = 'GestioneEsportazioni_Repository')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 6, N'GestioneEsportazioni_Repository', $(PercorsoCartellaConfigSiti) + N'File_Esportazioni\')

if not exists (select 1 from Configurazione_Siti where chiave = 'GestioneImportazioni_Repository')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 6, N'GestioneImportazioni_Repository', $(PercorsoCartellaConfigSiti) + N'File_Importazioni\')

if not exists (select 1 from Configurazione_Siti where chiave = 'IconaCentroConCartografiaAlberoImprese')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 6, N'IconaCentroConCartografiaAlberoImprese', N'true')

if not exists (select 1 from Configurazione_Siti where chiave = 'LinkCartografiaMappe')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 6, N'LinkCartografiaMappe', N'http://SRVAPP01/Mappe')

if not exists (select 1 from Configurazione_Siti where chiave = 'LinkSementieri')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 6, N'LinkSementieri', N'')

if not exists (select 1 from Configurazione_Siti where chiave = 'LivelloEspansoAlberoImprese')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 6, N'LivelloEspansoAlberoImprese', N'5')

if not exists (select 1 from Configurazione_Siti where chiave = 'NomeStampante')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 6, N'NomeStampante', N'')

if not exists (select 1 from Configurazione_Siti where chiave = 'PathDirectoryLOG')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 6, N'PathDirectoryLOG', $(PercorsoCartellaConfigSiti) + N'LOG')

if not exists (select 1 from Configurazione_Siti where chiave = 'PathFileTemporanei')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 6, N'PathFileTemporanei', N'')

if not exists (select 1 from Configurazione_Siti where chiave = 'Prefix4SP')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 6, N'Prefix4SP', N'dbo.')

if not exists (select 1 from Configurazione_Siti where chiave = 'Sincronizzatore2010')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 6, N'Sincronizzatore2010', N'true')

if not exists (select 1 from Configurazione_Siti where chiave = 'SitoRichiesto')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 6, N'SitoRichiesto', N'giasonline')

if not exists (select 1 from Configurazione_Siti where chiave = 'Stampe2010')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 6, N'Stampe2010', N'true')

if not exists (select 1 from Configurazione_Siti where chiave = 'StarGate_CodicePredefinito')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 6, N'StarGate_CodicePredefinito', N'1')

if not exists (select 1 from Configurazione_Siti where chiave = 'StarGate_Connessione')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 6, N'StarGate_Connessione', N'cnSTARGATE')

if not exists (select 1 from Configurazione_Siti where chiave = 'StarGate_ConsentiGateBypass')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 6, N'StarGate_ConsentiGateBypass', N'true')

if not exists (select 1 from Configurazione_Siti where chiave = 'StarGate_LinkBypassBloccato')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 6, N'StarGate_LinkBypassBloccato', N'http://www.giasonline.it')

if not exists (select 1 from Configurazione_Siti where chiave = 'StarGate_PathFileINI')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 6, N'StarGate_PathFileINI', N'c:/agroconnessioni/connessioni.ini')

if not exists (select 1 from Configurazione_Siti where chiave = 'StarGate_Versione')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 6, N'StarGate_Versione', N'OLD')

if not exists (select 1 from Configurazione_Siti where chiave = 'StarGate_Whynot')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 6, N'StarGate_Whynot', N'true')

if not exists (select 1 from Configurazione_Siti where chiave = 'Str_TabelleTemp_RegolaConfronto')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 6, N'Str_TabelleTemp_RegolaConfronto', N'')

if not exists (select 1 from Configurazione_Siti where chiave = 'TabelleTemp_Mode')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 6, N'TabelleTemp_Mode', N'1')

if not exists (select 1 from Configurazione_Siti where chiave = 'SpesometroProduttoreSoftware')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 16, N'SpesometroProduttoreSoftware', N'03487210407')

if not exists (select 1 from Configurazione_Siti where chiave = 'ClientSMTP')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 17, N'ClientSMTP', N'')

if not exists (select 1 from Configurazione_Siti where chiave = 'ClientSMTP_Porta')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 17, N'ClientSMTP_Porta', N'')

if not exists (select 1 from Configurazione_Siti where chiave = 'Nome_Codice_Griglia')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 17, N'Nome_Codice_Griglia', N'Codice Griglia')

if not exists (select 1 from Configurazione_Siti where chiave = 'ForzaMakeValid')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 20, N'ForzaMakeValid', N'false')

if not exists (select 1 from Configurazione_Siti where chiave = 'VerificaInizialeDatiGIS')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 20, N'VerificaInizialeDatiGIS', N'true')

if not exists (select 1 from Configurazione_Siti where chiave = 'BloccoAppezzamentiScaricatiSuPalm')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 23, N'BloccoAppezzamentiScaricatiSuPalm', N'True')

if not exists (select 1 from Configurazione_Siti where chiave = 'UsaNuovaGraficaPalm')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 23, N'UsaNuovaGraficaPalm', N'true')

if not exists (select 1 from Configurazione_Siti where chiave = 'UsaNuovaGraficaPalm_GIS_SistemiRiferimentoCartografia')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 23, N'UsaNuovaGraficaPalm_GIS_SistemiRiferimentoCartografia', N'4')

if not exists (select 1 from Configurazione_Siti where chiave = 'Lista_Stampanti')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 24, N'Lista_Stampanti', N'')

if not exists (select 1 from Configurazione_Siti where chiave = 'Mode_AgroWinSrvc_PrintUtility_PtP')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 24, N'Mode_AgroWinSrvc_PrintUtility_PtP', N'1')

if not exists (select 1 from Configurazione_Siti where chiave = 'PathComandi_AgroWinSrvc_PrintUtility')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 24, N'PathComandi_AgroWinSrvc_PrintUtility', N'')

if not exists (select 1 from Configurazione_Siti where chiave = 'PathDati_AgroWinSrvc_PrintUtility')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 24, N'PathDati_AgroWinSrvc_PrintUtility', N'')
       
if not exists (select 1 from Configurazione_Siti where chiave = 'user_smtp')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 24, N'user_smtp', N'')

if not exists (select 1 from Configurazione_Siti where chiave = 'password_smtp')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 24, N'password_smtp', N'')

if not exists (select 1 from Configurazione_Siti where chiave = 'enablessl_smtp')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 24, N'enablessl_smtp', N'false')
            
if not exists (select 1 from Configurazione_Siti where chiave = 'LinkGiasOnline_Root')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 24, N'LinkGiasOnline_Root', N'')
           
if not exists (select 1 from Configurazione_Siti where chiave = 'Path_AgroVerificatore_Log')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 24, N'Path_AgroVerificatore_Log', N'')
       
IF NOT EXISTS (SELECT 1 FROM Configurazione_Siti WHERE Chiave = 'LanToSincro2010_BrowserMode' ) 
 INSERT INTO [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore])
 VALUES (@PivaSuperUser, 0, 'LanToSincro2010_BrowserMode', '2')

if not exists (select 1 from Configurazione_Siti where chiave = 'FiltroneBootstrap')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) 
VALUES (@PivaSuperUser, 6, N'FiltroneBootstrap', N'false')

if not exists (select 1 from Configurazione_Siti where chiave = 'MonitoraggioCE_FW4')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) 
VALUES (@PivaSuperUser, 0, N'MonitoraggioCE_FW4', N'false')

if not exists (select 1 from Configurazione_Siti where chiave = 'GiasOnline_WS_Core_AgroWS_Core')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) 
VALUES (@PivaSuperUser, 0, N'GiasOnline_WS_Core_AgroWS_Core', N'/AgronicaCoreWS/')

if not exists (select 1 from Configurazione_Siti where chiave = 'StartRicevuto_Prog')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 0, N'StartRicevuto_Prog', N'86')
       
if not exists (select 1 from Configurazione_Siti where chiave = 'lenRicevuto_Prog')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 0, N'lenRicevuto_Prog', N'10')
    
if not exists (select 1 from Configurazione_Siti where chiave = 'RaccoltaNew')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 6, N'RaccoltaNew', N'true')
        
if not exists (select 1 from Configurazione_Siti where chiave = 'JDE_Database')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 0, N'JDE_Database', N'')
        
if not exists (select 1 from Configurazione_Siti where chiave = 'JDE_SQLStringaConnessione')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 0, N'JDE_SQLStringaConnessione', N'')
        
if not exists (select 1 from Configurazione_Siti where chiave = 'JDE_ORACLEStringaConnessione')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 0, N'JDE_ORACLEStringaConnessione', N'')
        
if not exists (select 1 from Configurazione_Siti where chiave = 'JDE_ORACLECodiceStabilimento')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 0, N'JDE_ORACLECodiceStabilimento', N'')
        
if not exists (select 1 from Configurazione_Siti where chiave = 'JDE_ORACLENomeStabilimento')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 0, N'JDE_ORACLENomeStabilimento', N'')

if not exists (select 1 from Configurazione_Siti where chiave = 'JDE_ORACLE_DB_DTA')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 0, N'JDE_ORACLE_DB_DTA', N'')

if not exists (select 1 from Configurazione_Siti where chiave = 'JDE_ORACLE_DB_CTL')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 0, N'JDE_ORACLE_DB_CTL', N'')
            
if not exists (select 1 from Configurazione_Siti where chiave = 'GIS_EscludiFiltroCodiceFiscaleTecnico')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 6, N'GIS_EscludiFiltroCodiceFiscaleTecnico', N'true')

if not exists (select 1 from Configurazione_Siti where chiave = 'FiltroGruppiUtente')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) 
VALUES (@PivaSuperUser, 6, N'FiltroGruppiUtente', N'false')		   
           
if not exists (select 1 from Configurazione_Siti where chiave = 'Sincro_Codice_Chiave_Cliente')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) 
VALUES (@PivaSuperUser, 16, N'Sincro_Codice_Chiave_Cliente', N'')	

if not exists (select 1 from Configurazione_Siti where chiave = 'Sincro_StringaConnessioneExcel')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) 
VALUES (@PivaSuperUser, 16, N'Sincro_StringaConnessioneExcel', N'')

if not exists (select 1 from Configurazione_Siti where chiave = 'Sincro_LinkWSImportaGIAS')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) 
VALUES (@PivaSuperUser, 16, N'Sincro_LinkWSImportaGIAS', N'')
           
if not exists (select 1 from Configurazione_Siti where chiave = 'Sincro_ImportAnagrafica_BloccaAppezzamenti')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) 
VALUES (@PivaSuperUser, 16, N'Sincro_ImportAnagrafica_BloccaAppezzamenti', N'false')			   
           
if not exists (select 1 from Configurazione_Siti where chiave = 'Sincro_ImportAnagrafica_Impianto_DpiCod')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) 
VALUES (@PivaSuperUser, 16, N'Sincro_ImportAnagrafica_Impianto_DpiCod', N'0')	

if not exists (select 1 from Configurazione_Siti where chiave = 'Sincro_ImportAnagrafica_Impianto_RegConcCod')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) 
VALUES (@PivaSuperUser, 16, N'Sincro_ImportAnagrafica_Impianto_RegConcCod', N'0')


if not exists (select 1 from Configurazione_Siti where chiave = 'ConnessioneMaps_Server')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) 
VALUES (@PivaSuperUser, 6, N'ConnessioneMaps_Server', N'cnAgronicaMaps')

if not exists (select 1 from Configurazione_Siti where chiave = 'deltaE')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) 
VALUES (@PivaSuperUser, 6, N'deltaE', N'70.0')

if not exists (select 1 from Configurazione_Siti where chiave = 'deltaN')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) 
VALUES (@PivaSuperUser, 6, N'deltaN', N'108.0')

if not exists (select 1 from Configurazione_Siti where chiave = 'MenuAgendaBS')
    INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) 
    VALUES (@PivaSuperUser, 6, N'MenuAgendaBS', N'true')
else
    UPDATE Configurazione_Siti set valore='true' where chiave = 'MenuAgendaBS'

if not exists (select 1 from Configurazione_Siti where chiave = 'MenuBS_2017')
    INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) 
    VALUES (@PivaSuperUser, 6, N'MenuBS_2017', N'true')
else
    UPDATE Configurazione_Siti set valore='true' where chiave = 'MenuBS_2017'

if not exists (select 1 from Configurazione_Siti where chiave = 'MenuAnagrafeBS')
    INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) 
    VALUES (@PivaSuperUser, 6, N'MenuAnagrafeBS', N'true')
else
    UPDATE Configurazione_Siti set valore='true' where chiave = 'MenuAnagrafeBS'

if not exists (select 1 from Configurazione_Siti where chiave = 'OperazioniAgendaBS')
    INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) 
    VALUES (@PivaSuperUser, 6, N'OperazioniAgendaBS', N'true')
else
    UPDATE Configurazione_Siti set valore='true' where chiave = 'OperazioniAgendaBS'

if not exists (select 1 from Configurazione_Siti where chiave = 'PianoConcimazioneBS')
    INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) 
    VALUES (@PivaSuperUser, 6, N'PianoConcimazioneBS', N'true')
else
    UPDATE Configurazione_Siti set valore='true' where chiave = 'PianoConcimazioneBS'

if not exists (select 1 from Configurazione_Siti where chiave = 'CodRisUm_Personalizzazione_Doc_Contabili_1')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) 
VALUES (@PivaSuperUser, 6, N'CodRisUm_Personalizzazione_Doc_Contabili_1', N'')

if not exists (select 1 from Configurazione_Siti where chiave = 'RipartoCatasto_Disabilita_VerificaIntersezione')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) 
VALUES (@PivaSuperUser, 6, N'RipartoCatasto_Disabilita_VerificaIntersezione', N'false')

if not exists (select 1 from Configurazione_Siti where chiave = 'BloccoAppezzamentiIgnora')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) 
VALUES (@PivaSuperUser, 23, N'BloccoAppezzamentiIgnora', N'false')

--VERIFICA CHE NON CI SIANO LINK A WS CON IP FISSI
UPDATE [dbo].[Configurazione_Siti] 
SET [Valore] = 'http://www2.agronica.it/AgronicaWebService/Gias_service.asmx'
WHERE [Sito_Cod] = 6 AND [Chiave] = 'GiasOnline_WS_GiasWSAggiorna_AgroWS_GiasWSAggiorna_2' AND [Valore] = 'http://212.210.214.19/AgronicaWebService/Gias_service.asmx'

UPDATE [dbo].[Configurazione_Siti] 
SET [Valore] = 'http://www2.agronica.it/AgronicaWebService/AgroWS_CapitolatoCliente.asmx'
WHERE [Sito_Cod] = 6 AND [Chiave] = 'GiasOnline_WS_CapitolatoCliente_AgroWS_CapitolatoCliente_2' AND [Valore] = 'http://212.210.214.19/AgronicaWebService/AgroWS_CapitolatoCliente.asmx'

UPDATE [dbo].[Configurazione_Siti] 
SET [Valore] = 'http://www2.agronica.it/AgronicaWebService/AgroWS_Disciplinari.asmx'
WHERE [Sito_Cod] = 6 AND [Chiave] = 'GiasOnline_WS_Disciplinari_AgroWS_Disciplinari_2' AND [Valore] = 'http://212.210.214.19/AgronicaWebService/AgroWS_Disciplinari.asmx'

UPDATE [dbo].[Configurazione_Siti] 
SET [Valore] = 'http://www2.agronica.it/AgronicaWebService/AgroWS_Fitofarmaci.asmx'
WHERE [Sito_Cod] = 6 AND [Chiave] = 'GiasOnline_WS_Fitofarmaci_AgroWS_Fitofarmaci_2' AND [Valore] = 'http://212.210.214.19/AgronicaWebService/AgroWS_Fitofarmaci.asmx'

UPDATE [dbo].[Configurazione_Siti] 
SET [Valore] = 'http://www2.agronica.it/AgronicaWebService/AgroAPI_PianoConcimazione.svc'
WHERE [Sito_Cod] = 6 AND [Chiave] = 'GiasOnline_WS_PianoConcimazione_AgroAPI_PianoConcimazione_2' AND [Valore] = 'http://212.210.214.19/AgronicaWebService/AgroAPI_PianoConcimazione.svc'

UPDATE [dbo].[Configurazione_Siti] 
SET [Valore] = 'http://www2.agronica.it/AgronicaWebService/AgroAPI_Fertilizzanti.svc'
WHERE [Sito_Cod] = 6 AND [Chiave] = 'GiasOnline_WS_Fertilizzanti_IAgroAPI_Fertilizzanti_2' AND [Valore] = 'http://212.210.214.19/AgronicaWebService/AgroAPI_Fertilizzanti.svc'

UPDATE [dbo].[Configurazione_Siti] 
SET [Valore] = 'http://www2.agronica.it/AgronicaWebService/Gias_service.asmx'
WHERE [Sito_Cod] = 6 AND [Chiave] = 'GiasOnline_WS_GiasWSAggiorna_AgroWS_GiasWSAggiorna_2' AND [Valore] = 'http://212.210.214.19/AgronicaWebService/Gias_service.asmx'

UPDATE [dbo].[Configurazione_Siti] 
SET [Valore] = 'http://www2.agronica.it/ws_gias_2004/gias_service.asmx'
WHERE [Sito_Cod] = 6 AND [Chiave] = 'GiasOnline_WS_Mappe_Gias_Service' AND [Valore] = 'http://212.210.214.19/ws_gias_2004/gias_service.asmx'



UPDATE [dbo].[Configurazione_Siti] 
SET [Valore] = 'http://www2.agronica.it/ws_gias_2004/gias_service.asmx'
WHERE [Sito_Cod] = 6 AND [Chiave] = 'Ritaglio_Servizio' AND [Valore] = 'http://212.210.214.19/ws_gias_2004/gias_service.asmx'

if not exists (select 1 from Configurazione_Siti where chiave = 'PlanningBS')
    INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) 
    VALUES (@PivaSuperUser, 6, N'PlanningBS', N'true')
else
    UPDATE Configurazione_Siti set valore='true' where chiave = 'PlanningBS'

if not exists (select 1 from Configurazione_Siti where chiave = 'RilieviBS')
    INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) 
    VALUES (@PivaSuperUser, 6, N'RilieviBS', N'true')
else
    UPDATE Configurazione_Siti set valore='true' where chiave = 'RilieviBS'

if not exists (select 1 from Configurazione_Siti where chiave = 'Sincro_Regolamento_Condizionalita')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) 
VALUES (@PivaSuperUser, 16, N'Sincro_Regolamento_Condizionalita', N'')

if not exists (select 1 from Configurazione_Siti where chiave = 'VerificaAnagraficaImpreseProfilate')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) 
VALUES (@PivaSuperUser, 6, N'VerificaAnagraficaImpreseProfilate', N'false')

if not exists (select 1 from Configurazione_Siti where chiave = 'LoginLogAccessoTutti')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) 
VALUES (@PivaSuperUser, 6, N'LoginLogAccessoTutti', N'false')

if not exists (select 1 from Configurazione_Siti where chiave = 'enum_PC_Anteprima_wizardComportamentoWS')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) 
VALUES (@PivaSuperUser, 6, N'enum_PC_Anteprima_wizardComportamentoWS', N'{ "ChiamaWS": 0, "ChiamaWS_UMBRIA": 0, "Imposta_Pratica" : false, "Servizi_Regione": 0 }')

if not exists (select 1 from Configurazione_Siti where chiave = 'enum_PC_Anteprima_ListaWS')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) 
VALUES (@PivaSuperUser, 6, N'enum_PC_Anteprima_ListaWS', N'[]')

if not exists (select 1 from Configurazione_Siti where chiave = 'LinkArteaWS')
    INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) 
    VALUES (@PivaSuperUser, 16, N'LinkArteaWS', N'https://www1.artea.toscana.it/ws_ArteaAnagrafiche/ArteaService.asmx')
ELSE
    UPDATE [dbo].[Configurazione_Siti] SET Valore = N'https://www1.artea.toscana.it/ws_ArteaAnagrafiche/ArteaService.asmx' WHERE Chiave = N'LinkArteaWS'

if not exists (select 1 from Configurazione_Siti where chiave = 'PasswordArteaWS')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) 
VALUES (@PivaSuperUser, 16, N'PasswordArteaWS', N'1wsagronica1')

if not exists (select 1 from Configurazione_Siti where chiave = 'UsernameArteaWS')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) 
VALUES (@PivaSuperUser, 16, N'UsernameArteaWS', N'WSAGRONICA')


if not exists (select 1 from Configurazione_Siti where chiave = 'Gis2017')
    INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) 
    VALUES (@PivaSuperUser, 6, N'Gis2017', N'true')
else
    UPDATE Configurazione_Siti set valore='true' where chiave = 'Gis2017'

if not exists (select 1 from Configurazione_Siti where chiave = 'Flag_Stampe_Nuovi_Arrotondamenti')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) 
VALUES (@PivaSuperUser, 24, N'Flag_Stampe_Nuovi_Arrotondamenti', N'true')


if not exists (select 1 from Configurazione_Siti where chiave = 'AgroFascicolo_WS')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 0, N'AgroFascicolo_WS', N'http://www.agronica.it/WS_AgroFascicoloBA/FascicoloManager.svc')


if not exists (select 1 from Configurazione_Siti where chiave = 'PathFileExportConferimentiCSV')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) 
VALUES (@PivaSuperUser, 16, N'PathFileExportConferimentiCSV', N'C:\GIASLAN\File_Esportazioni\ExportConferimentiCSV')


if not exists (select 1 from Configurazione_Siti where chiave = 'GDPR_Attivo')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) 
VALUES (@PivaSuperUser, 6, N'GDPR_Attivo', N'true')

if not exists (select 1 from Configurazione_Siti where chiave = 'minutiValiditaLoginMemorizzato')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) 
VALUES (@PivaSuperUser, 6, N'minutiValiditaLoginMemorizzato', N'240')

if not exists (select 1 from Configurazione_Siti where chiave = 'GiasOnline_WS_Mappe_2013')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) 
VALUES (@PivaSuperUser, 6, N'GiasOnline_WS_Mappe_2013', N'https://agrosat.agronica.it/ws_mappe_2013/')

if not exists (select 1 from Configurazione_Siti where chiave = 'Connessione_AgroGSB')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) 
VALUES (@PivaSuperUser, 6, N'Connessione_AgroGSB', N'')

if not exists (select 1 from Configurazione_Siti where chiave = 'LibroConferimenti_NumRighePagina')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) 
VALUES (@PivaSuperUser, 24, N'LibroConferimenti_NumRighePagina', N'50')

if not exists (select 1 from Configurazione_Siti where chiave = 'DocumentoRicevutoLight')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) 
VALUES (@PivaSuperUser, 15, N'DocumentoRicevutoLight', N'false')

if not exists (select 1 from Configurazione_Siti where chiave = 'RaccoltaBS')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) 
VALUES (@PivaSuperUser, 6, N'RaccoltaBS', N'false')

if not exists (select 1 from Configurazione_Siti where chiave = 'SeminaBS')
    INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) 
    VALUES (@PivaSuperUser, 6, N'SeminaBS', N'true')
else
    UPDATE Configurazione_Siti set valore='true' where chiave = 'SeminaBS'

if not exists (select 1 from Configurazione_Siti where chiave = 'ScaricoArteaWS_Config')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) 
VALUES (@PivaSuperUser, 6, N'ScaricoArteaWS_Config', N'{ "ScaricaCatasto": false, "ScaricaRipartoCatasto": false}')

if not exists (select 1 from Configurazione_Siti where chiave = 'GIS_cmbElementoGrafico_GenerazionePoligoni_DefaultValue')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) 
VALUES (@PivaSuperUser, 6, N'GIS_cmbElementoGrafico_GenerazionePoligoni_DefaultValue', N'3')

if not exists (select 1 from Configurazione_Siti where chiave = 'AgroFascicolo_WS_AGREA')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 0, N'AgroFascicolo_WS_AGREA', N'http://www.agronica.it/WS_AgroFascicoloBA/FascicoloManager.svc')

if not exists (select 1 from Configurazione_Siti where chiave = 'AgroFascicolo_WS_Coordinamento')
Begin
        INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 0, N'AgroFascicolo_WS_Coordinamento', N'https://gariumbria.regione.umbria.it/WS_AgroFascicoloBA/FascicoloManager.svc')
End
else
Begin
        if (@PivaSuperUser <> '03761180961')
        Begin
                UPDATE Configurazione_Siti
                SET Valore = N'https://gariumbria.regione.umbria.it/WS_AgroFascicoloBA/FascicoloManager.svc'
                WHERE Chiave = N'AgroFascicolo_WS_Coordinamento'
        End
End

if not exists (select 1 from Configurazione_Siti where chiave = 'GiasOnline_WS_Audit_IAgroAPI_Audit')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) 
VALUES (@PivaSuperUser, 6, N'GiasOnline_WS_Audit_IAgroAPI_Audit', N'http://www.agronica.it/AgronicaWebService/AgroAPI_Audit.svc')

if not exists (select 1 from Configurazione_Siti where chiave = 'GisEliminazioneImpiantoEliminaAppezzamento')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) 
VALUES (@PivaSuperUser, 6, N'GisEliminazioneImpiantoEliminaAppezzamento', N'false')

if not exists (select 1 from Configurazione_Siti where chiave = 'RicetteOrdiniDiLavoro2018')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) 
VALUES (@PivaSuperUser, 6, N'RicetteOrdiniDiLavoro2018', N'false')

UPDATE [Configurazione_Siti]
SET valore = N'assistenza@agronica.it'
where chiave= N'MailAssistenza'

if not exists (select 1 from Configurazione_Siti where chiave = 'ExportAnalisiPDC_FiltroCapitolatoCodice')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) 
VALUES (@PivaSuperUser, 0, N'ExportAnalisiPDC_FiltroCapitolatoCodice', N'')

if not exists (select 1 from Configurazione_Siti where chiave = 'IrrigazioneBS')
    INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) 
    VALUES (@PivaSuperUser, 6, N'IrrigazioneBS', N'true')
else
    UPDATE [dbo].[Configurazione_Siti] 
    set Valore = N'true'
    where sito_cod = 6 and chiave = N'IrrigazioneBS'

if not exists (select 1 from Configurazione_Siti where chiave = 'G2F2B_Cfg')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) 
VALUES (@PivaSuperUser, 6, N'G2F2B_Cfg', N'{ "url": "", "username": "", "password": "", "passwordHash": "", "serializationCultureInfo": "it-IT" }')

if not exists (select 1 from Configurazione_Siti where chiave = 'AbilitaHashPassword')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) 
VALUES (@PivaSuperUser, 6, N'AbilitaHashPassword', N'1')

if not exists (select 1 from Configurazione_Siti where chiave = 'Configurazione_MVVE')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES (@PivaSuperUser, 6, N'Configurazione_MVVE', N'')


if not exists (select 1 from Configurazione_Siti where chiave = 'LinkPianoConcimazione_2017')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 6, N'LinkPianoConcimazione_2017', $(PercorsoLink) + N'/AgronicaPianoConcimazione_2017/GestioneRichieste.aspx')

if not exists (select 1 from Configurazione_Siti where chiave = 'JohnDeereConfig')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 6, N'JohnDeereConfig', '')

if not exists (	select 1 from Configurazione_Siti where chiave = 'DataProviderLogConfig')
INSERT [dbo].[Configurazione_Siti] ( [PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES
        (@PivaSuperUser, 6, N'DataProviderLogConfig', '{"LivelloLOG":1,"Mittente_Mail":"service@agronicagroup.it","Destinatari_Mail":"testhack@agronica.it","UtilizzaCache":"True","SlidingExpirationInMinuti":60,"LimiteElementiClausoleIn":50}')
-- Rimuovo l'update fisso, in modo da poter testare l'utilizzo della scrittura su tabella (LivelloLog =2) 
-- solo su alcuni db/ambienti prima di metterla da tutti
-- A quel punto sarà possibile correggere la configurazione di questo update ed attivarlo da tutti con update massivo

--UPDATE [dbo].[Configurazione_Siti] 
--    set Valore =									  '{"LivelloLOG":1,"Mittente_Mail":"service@agronicagroup.it","Destinatari_Mail":"testhack@agronica.it","UtilizzaCache":"True","SlidingExpirationInMinuti":60,"LimiteElementiClausoleIn":50}'
--    where sito_cod = 6 and chiave = 'DataProviderLogConfig'

-- recupero valore chiave MailFrom_smtp dal superServer
declare @valore_MailFrom_smtp_Gias_Super_Server varchar(4000)
select @valore_MailFrom_smtp_Gias_Super_Server = valore from  $(SuperServer).dbo.Configurazione_Siti where chiave = 'MailFrom_smtp'	and Sito_Cod = 6

if not exists (select 1 from Configurazione_Siti where chiave = 'MailFrom_smtp')
    begin 
        -- se non esiste nel DB_SERVER la inserisco col valore che trovo nel Gias_Super_Server
        INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 6, N'MailFrom_smtp', @valore_MailFrom_smtp_Gias_Super_Server)
    end 
else
    begin
        -- se esiste già nel DB_SERVER ed è vuota faccio update con valore rispettiva chiave del Gias_Super_Server
        declare @valore_MailFrom_smtp_DB_Server varchar(4000)
        select @valore_MailFrom_smtp_DB_Server = valore from  dbo.Configurazione_Siti where chiave = 'MailFrom_smtp'	and Sito_Cod = 6
        if @valore_MailFrom_smtp_DB_Server = '' 
            begin
                UPDATE [dbo].[Configurazione_Siti] Set [Valore] = @valore_MailFrom_smtp_Gias_Super_Server
                WHERE Chiave = 'MailFrom_smtp' AND Sito_Cod = 6
            end
    end 

-- recupero valore chiave ClientSMTP dal superServer
declare @valore_ClientSMTP_Gias_Super_Server varchar(4000)
select @valore_ClientSMTP_Gias_Super_Server = valore from  $(SuperServer).dbo.Configurazione_Siti where chiave = 'ClientSMTP' and (Sito_Cod = 0 Or Sito_Cod = 6)

-- se esiste già nel DB_SERVER ed è vuota faccio update con valore rispettiva chiave del Gias_Super_Server
declare @valore_ClientSMTP_DB_Server varchar(4000)
select @valore_ClientSMTP_DB_Server = valore from  dbo.Configurazione_Siti where chiave = 'ClientSMTP'	and Sito_Cod = 17
if @valore_ClientSMTP_DB_Server = '' 
    begin
        UPDATE [dbo].[Configurazione_Siti] Set [Valore] = @valore_ClientSMTP_Gias_Super_Server
        WHERE Chiave = 'ClientSMTP' AND Sito_Cod = 17
    end


if not exists (select 1 from Configurazione_Siti where chiave = 'RichiestaIscrizioneGias')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 6, N'RichiestaIscrizioneGias', '0')


if not exists (select 1 from Configurazione_Siti where chiave = 'Blocca_SeminaConCambioSpecie_SePresentiOperazioni_SuApp')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 6, N'Blocca_SeminaConCambioSpecie_SePresentiOperazioni_SuApp', '1')
else
    UPDATE Configurazione_Siti set valore='1' where chiave = 'Blocca_SeminaConCambioSpecie_SePresentiOperazioni_SuApp'


if not exists (select 1 from Configurazione_Siti where chiave = 'GiasOnline_WS_Irriframe')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 6, N'GiasOnline_WS_Irriframe', '{ "persistent": false, "baseUrl": "https://www.irriframe.it/irriframeapi/api" }')


if not exists (select 1 from Configurazione_Siti where chiave = 'UploadMultiploAllegati_Documentale')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 6, N'UploadMultiploAllegati_Documentale', N'true')

-- ANNA 20/08/21

if not exists (select 1 from Configurazione_Siti where chiave = 'Sincro_BA_Anno_Piano_Colturale')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 16, N'Sincro_BA_Anno_Piano_Colturale', '0')


if not exists (select 1 from Configurazione_Siti where chiave = 'Sincro_BA_UffZona_TipoArchivioCliente')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 16, N'Sincro_BA_UffZona_TipoArchivioCliente', '1')


if not exists (select 1 from Configurazione_Siti where chiave = 'Sincro_ImportDDTSeled_TabelleTemp_Mode')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 16, N'Sincro_ImportDDTSeled_TabelleTemp_Mode', '2')


if not exists (select 1 from Configurazione_Siti where chiave = 'WSIG_Demetra_NumMin_Caratteri_FerDes_Richiesti')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 16, N'WSIG_Demetra_NumMin_Caratteri_FerDes_Richiesti', '4')


if not exists (select 1 from Configurazione_Siti where chiave = 'WSIG_Demetra_NumMin_Caratteri_FrDes_Richiesti')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 16, N'WSIG_Demetra_NumMin_Caratteri_FrDes_Richiesti', '4')


if not exists (select 1 from Configurazione_Siti where chiave = 'Sincro_BA_UffZona_Carattere_ASCII_Separatore')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 16, N'Sincro_BA_UffZona_Carattere_ASCII_Separatore', '59')


if not exists (select 1 from Configurazione_Siti where chiave = 'Sincro_ImportAnagSELED_Carattere_ASCII_Separatore')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 16, N'Sincro_ImportAnagSELED_Carattere_ASCII_Separatore', '59')


if not exists (select 1 from Configurazione_Siti where chiave = 'Sincro_BA_UffZona_ID_CAA')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 16, N'Sincro_BA_UffZona_ID_CAA', '103')


if not exists (select 1 from Configurazione_Siti where chiave = 'Sincro_ImportDDTSeled_Data_Inizio_Filtro')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 16, N'Sincro_ImportDDTSeled_Data_Inizio_Filtro', '01/11/2020')


if not exists (select 1 from Configurazione_Siti where chiave = 'Sincro_ImportDDTSeled_Data_Fine_Filtro')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 16, N'Sincro_ImportDDTSeled_Data_Fine_Filtro', '31/12/2100')


if not exists (select 1 from Configurazione_Siti where chiave = 'WSIG_Demetra_DataOperazioneBlocco')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 16, N'WSIG_Demetra_DataOperazioneBlocco', '31/12/2100')



if not exists (select 1 from Configurazione_Siti where chiave = 'LinkWSImportaMagazzino')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 6, N'LinkWSImportaMagazzino', 'http://localhost/WS_Importa_Magazzino_2010/WSImportaMagazzino.asmx')


if not exists (select 1 from Configurazione_Siti where chiave = 'Sincro_ImportDDTSeled_mail_destinatario_banchedati')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 16, N'Sincro_ImportDDTSeled_mail_destinatario_banchedati', 'AlertMagazzino@agronica.it')


if not exists (select 1 from Configurazione_Siti where chiave = 'Sincro_ImportDDTSeled_mail_destinatario_log')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 16, N'Sincro_ImportDDTSeled_mail_destinatario_log', 'AlertMagazzino@agronica.it')


if not exists (select 1 from Configurazione_Siti where chiave = 'Sincro_MAG_mail_destinatario_banchedati')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 16, N'Sincro_MAG_mail_destinatario_banchedati', 'AlertMagazzino@agronica.it')


if not exists (select 1 from Configurazione_Siti where chiave = 'Sincro_MAG_mail_destinatario_log')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 16, N'Sincro_MAG_mail_destinatario_log', 'AlertMagazzino@agronica.it')


if not exists (select 1 from Configurazione_Siti where chiave = 'Sincro_ImportDDTSeled_TabelleTemp_RegolaConfronto')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 16, N'Sincro_ImportDDTSeled_TabelleTemp_RegolaConfronto', 'COLLATE Latin1_General_CI_AS')


if not exists (select 1 from Configurazione_Siti where chiave = 'Sincro_dbwin_Str_Collate_RegolaConfronto')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 16, N'Sincro_dbwin_Str_Collate_RegolaConfronto', 'COLLATE SQL_Latin1_General_CP850_CI_AS')


if not exists (select 1 from Configurazione_Siti where chiave = 'Sincro_SincroAccessOP_Mode')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 16, N'Sincro_SincroAccessOP_Mode', 'cuaa/massiva')


if not exists (select 1 from Configurazione_Siti where chiave = 'WSIG_Flag_AttivaCreazioneAutoCentroFabbricato')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 16, N'WSIG_Flag_AttivaCreazioneAutoCentroFabbricato', 'false')


if not exists (select 1 from Configurazione_Siti where chiave = 'WsMag_FlagSalvaFileXmlPrivato')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 16, N'WsMag_FlagSalvaFileXmlPrivato', 'false')


if not exists (select 1 from Configurazione_Siti where chiave = 'WSIG_FlagSalvaFileXmlPrivato')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 16, N'WSIG_FlagSalvaFileXmlPrivato', 'false')


if not exists (select 1 from Configurazione_Siti where chiave = 'Sincro_dbwin_Flag_UsaOpenQuery')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 16, N'Sincro_dbwin_Flag_UsaOpenQuery', 'false')


if not exists (select 1 from Configurazione_Siti where chiave = 'Sincro_ImportDDTSeled_FlagBloccoOperazione')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 16, N'Sincro_ImportDDTSeled_FlagBloccoOperazione', 'false')


if not exists (select 1 from Configurazione_Siti where chiave = 'Sincro_MAG_flag_invio_mail')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 16, N'Sincro_MAG_flag_invio_mail', 'false')


if not exists (select 1 from Configurazione_Siti where chiave = 'Sincro_MAG_Flag_Verifica_CodificaProdotti')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 16, N'Sincro_MAG_Flag_Verifica_CodificaProdotti', 'false')


if not exists (select 1 from Configurazione_Siti where chiave = 'Sincro_MAG_FlagBloccoOperazione')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 16, N'Sincro_MAG_FlagBloccoOperazione', 'false')


if not exists (select 1 from Configurazione_Siti where chiave = 'Sincro_MAG_FlagControllaUdmEtichetta')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 16, N'Sincro_MAG_FlagControllaUdmEtichetta', 'false')


if not exists (select 1 from Configurazione_Siti where chiave = 'Sincro_MAG_FlagScartaRevocati')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 16, N'Sincro_MAG_FlagScartaRevocati', 'false')


if not exists (select 1 from Configurazione_Siti where chiave = 'Sincro_MAG_FlagSovrascriviOperazione')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 16, N'Sincro_MAG_FlagSovrascriviOperazione', 'false')


if not exists (select 1 from Configurazione_Siti where chiave = 'Sincro_BA_ImportFascicoloBA_Link')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 16, N'Sincro_BA_ImportFascicoloBA_Link', 'http://esb.coldiretti.it:80/FascicoloMMWeb/sca/FascicoloWS')


if not exists (select 1 from Configurazione_Siti where chiave = 'Sincro_BA_ImportAnagrafeBA_Link')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 16, N'Sincro_BA_ImportAnagrafeBA_Link', 'http://esb.coldiretti.it:80/SSAMMWeb/sca/SSAWS')


if not exists (select 1 from Configurazione_Siti where chiave = 'Sincro_BA_UffZona_ServerWebServices')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 16, N'Sincro_BA_UffZona_ServerWebServices', 'localhost')


if not exists (select 1 from Configurazione_Siti where chiave = 'Sincro_StringaConnessioneExcelRiga1Colonne')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 16, N'Sincro_StringaConnessioneExcelRiga1Colonne', 'PROVIDER=Microsoft.ACE.OLEDB.12.0;Extended Properties=''Excel 8.0;HDR=Yes;IMEX=1'';data source=')


if not exists (select 1 from Configurazione_Siti where chiave = 'Sincro_dbwin_OptionStringaConnSql')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 16, N'Sincro_dbwin_OptionStringaConnSql', 'Trusted_Connection=False;')


if not exists (select 1 from Configurazione_Siti where chiave = 'PathFileINI_ConnessionePredefinita_Server')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 0, N'PathFileINI_ConnessionePredefinita_Server', '')


if not exists (select 1 from Configurazione_Siti where chiave = 'PathFileINI_ConnessionePredefinita_Utenti')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 0, N'PathFileINI_ConnessionePredefinita_Utenti', '')


if not exists (select 1 from Configurazione_Siti where chiave = 'Sincro_dbwin_mail_destinatario')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 16, N'Sincro_dbwin_mail_destinatario', '')


if not exists (select 1 from Configurazione_Siti where chiave = 'Sincro_dbwin_DB_IstanzaSQL')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 16, N'Sincro_dbwin_DB_IstanzaSQL', '')


if not exists (select 1 from Configurazione_Siti where chiave = 'Sincro_dbwin_mail_mittente')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 16, N'Sincro_dbwin_mail_mittente', '')


if not exists (select 1 from Configurazione_Siti where chiave = 'Sincro_dbwin_DB_Nome')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 16, N'Sincro_dbwin_DB_Nome', '')


if not exists (select 1 from Configurazione_Siti where chiave = 'Sincro_dbwin_DB_Password')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 16, N'Sincro_dbwin_DB_Password', '')


if not exists (select 1 from Configurazione_Siti where chiave = 'Sincro_dbwin_Stringa_Connessione_Informix')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 16, N'Sincro_dbwin_Stringa_Connessione_Informix', '')


if not exists (select 1 from Configurazione_Siti where chiave = 'Sincro_dbwin_DB_Username')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 16, N'Sincro_dbwin_DB_Username', '')


if not exists (select 1 from Configurazione_Siti where chiave = 'Sincro_ImportAnagSELED_Piva_Padre')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 16, N'Sincro_ImportAnagSELED_Piva_Padre', '')


if not exists (select 1 from Configurazione_Siti where chiave = 'Sincro_ImportDDTSeled_Codifiche_IstanzaSQL')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 16, N'Sincro_ImportDDTSeled_Codifiche_IstanzaSQL', '')


if not exists (select 1 from Configurazione_Siti where chiave = 'Sincro_ImportDDTSeled_Codifiche_Nome_Database')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 16, N'Sincro_ImportDDTSeled_Codifiche_Nome_Database', '')


if not exists (select 1 from Configurazione_Siti where chiave = 'Sincro_ImportDDTSeled_Codifiche_Password_Database')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 16, N'Sincro_ImportDDTSeled_Codifiche_Password_Database', '')


if not exists (select 1 from Configurazione_Siti where chiave = 'Sincro_ImportDDTSeled_Codifiche_Username_Database')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 16, N'Sincro_ImportDDTSeled_Codifiche_Username_Database', '')


if not exists (select 1 from Configurazione_Siti where chiave = 'Sincro_ImportDDTSeled_Filtro_Aggiuntivo_CodificheArticoli')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 16, N'Sincro_ImportDDTSeled_Filtro_Aggiuntivo_CodificheArticoli', '')


if not exists (select 1 from Configurazione_Siti where chiave = 'Sincro_ImportDDTSeled_Filtro_Aggiuntivo_DatiDDT')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 16, N'Sincro_ImportDDTSeled_Filtro_Aggiuntivo_DatiDDT', '')


if not exists (select 1 from Configurazione_Siti where chiave = 'Sincro_ImportDDTSeled_Filtro_Aggiuntivo_ElencoDDT')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 16, N'Sincro_ImportDDTSeled_Filtro_Aggiuntivo_ElencoDDT', '')


if not exists (select 1 from Configurazione_Siti where chiave = 'Sincro_ImportDDTSeled_Filtro_SQL_CATEGPROD')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 16, N'Sincro_ImportDDTSeled_Filtro_SQL_CATEGPROD', '')


if not exists (select 1 from Configurazione_Siti where chiave = 'Sincro_ImportDDTSeled_Filtro_SQL_CAUSTRASP')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 16, N'Sincro_ImportDDTSeled_Filtro_SQL_CAUSTRASP', '')


if not exists (select 1 from Configurazione_Siti where chiave = 'Sincro_ImportDDTSeled_Filtro_SQL_PARTITAIVA')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 16, N'Sincro_ImportDDTSeled_Filtro_SQL_PARTITAIVA', '')


if not exists (select 1 from Configurazione_Siti where chiave = 'Sincro_ImportDDTSeled_Filtro_SQL_TIPODOC')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 16, N'Sincro_ImportDDTSeled_Filtro_SQL_TIPODOC', '')


if not exists (select 1 from Configurazione_Siti where chiave = 'Sincro_ImportDDTSeled_DB_IstanzaSQL')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 16, N'Sincro_ImportDDTSeled_DB_IstanzaSQL', '')


if not exists (select 1 from Configurazione_Siti where chiave = 'Sincro_ImportDDTSeled_mail_mittente')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 16, N'Sincro_ImportDDTSeled_mail_mittente', '')


if not exists (select 1 from Configurazione_Siti where chiave = 'Sincro_ImportDDTSeled_DB_Nome')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 16, N'Sincro_ImportDDTSeled_DB_Nome', '')


if not exists (select 1 from Configurazione_Siti where chiave = 'Sincro_ImportDDTSeled_DB_Password')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 16, N'Sincro_ImportDDTSeled_DB_Password', '')


if not exists (select 1 from Configurazione_Siti where chiave = 'Sincro_ImportDDTSeled_Piva_Fornitore')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 16, N'Sincro_ImportDDTSeled_Piva_Fornitore', '')


if not exists (select 1 from Configurazione_Siti where chiave = 'Sincro_ImportDDTSeled_DB_Username')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 16, N'Sincro_ImportDDTSeled_DB_Username', '')


if not exists (select 1 from Configurazione_Siti where chiave = 'Sincro_MAG_LinkWSLeggiMagazzinoREMOTO')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 16, N'Sincro_MAG_LinkWSLeggiMagazzinoREMOTO', '')


if not exists (select 1 from Configurazione_Siti where chiave = 'Sincro_MAG_mail_mittente')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 16, N'Sincro_MAG_mail_mittente', '')


if not exists (select 1 from Configurazione_Siti where chiave = 'Sincro_MAG_PivaSuperUserREMOTO')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 16, N'Sincro_MAG_PivaSuperUserREMOTO', '')


if not exists (select 1 from Configurazione_Siti where chiave = 'Sincro_StringaConnessioneSIAGR')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 16, N'Sincro_StringaConnessioneSIAGR', '')


if not exists (select 1 from Configurazione_Siti where chiave = 'timeout_smtp')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 24, N'timeout_smtp', '')


if not exists (select 1 from Configurazione_Siti where chiave = 'GiasOnline_WS_GestioneAzienda')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 16, N'GiasOnline_WS_GestioneAzienda', '')

if not exists (select 1 from Configurazione_Siti where chiave = 'Log_Scheda_Campagna_Abilitato')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 6, N'Log_Scheda_Campagna_Abilitato', N'False')


if not exists (select 1 from Configurazione_Siti where chiave = 'GIS_StaticMapCFG')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 6, N'GIS_StaticMapCFG', N'{ "StaticMapAttive": false, "DebugImgPathPerDump": "", "MapsApikey": "", "SignPrivateKey": "", "FillColor": "", "Color": "", "TipoMappaSfondo": "satellite", "SizeX": 400, "SizeY": 400,  "LayerAbilitati": [1, 19], "TimeoutRequestInSeconds": 5  }')

-- INIZIO CHIAVI DI CONFIGURAZIONE PER ESPORTAZIONE TRACCIABILITà

if not exists (select 1 from Configurazione_Siti where chiave = 'Esporta_Tracciabilita')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser,6,N'Esporta_Tracciabilita',N'false')
--Valori ammessi:
--0 -> esporta su disco
--1 -> richiama web service esterno
if not exists (select 1 from Configurazione_Siti where chiave = 'Esporta_Tracciabilita_Modalita')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser,6,N'Esporta_Tracciabilita_Modalita',N'0')

if not exists (select 1 from Configurazione_Siti where chiave = 'Esporta_Tracciabilita_DestinazionePath')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES
        (@PivaSuperUser,6,N'Esporta_Tracciabilita_DestinazionePath',N'')

if not exists (select 1 from Configurazione_Siti where chiave = 'Esporta_Tracciabilita_LinkWSEsterno')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES
        (@PivaSuperUser,6,N'Esporta_Tracciabilita_LinkWSEsterno',N'')

if not exists (select 1 from Configurazione_Siti where chiave = 'Esporta_Tracciabilita_WSEsternoParametri')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES
        (@PivaSuperUser,6,N'Esporta_Tracciabilita_WSEsternoParametri',N'')

if not exists (select 1 from Configurazione_Siti where chiave = 'Esporta_Tracciabilita_FTPMode')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser,6,N'Esporta_Tracciabilita_FTPMode',N'true')

if not exists (select 1 from Configurazione_Siti where chiave = 'Esporta_Tracciabilita_FTPUrl')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser,6,N'Esporta_Tracciabilita_FTPUrl',N'')

if not exists (select 1 from Configurazione_Siti where chiave = 'Esporta_Tracciabilita_FTPPort')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser,6,N'Esporta_Tracciabilita_FTPPort',N'')

if not exists (select 1 from Configurazione_Siti where chiave = 'Esporta_Tracciabilita_FTPUsername')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser,6,N'Esporta_Tracciabilita_FTPUsername',N'')

if not exists (select 1 from Configurazione_Siti where chiave = 'Esporta_Tracciabilita_FTPPassword')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser,6,N'Esporta_Tracciabilita_FTPPassword',N'')

if not exists (select 1 from Configurazione_Siti where chiave = 'Tracciabilita_ApiKey_SDF')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser,6,N'Tracciabilita_ApiKey_SDF',N'')

if not exists (select 1 from Configurazione_Siti where chiave = 'Tracciabilita_ApiUrlBase_SDF')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser,6,N'Tracciabilita_ApiUrlBase_SDF',N'')

if exists (select 1 from Configurazione_Siti where chiave = 'Tracciabilita_ApiUrlBase_SDF' and PivaSuperUser<>@PivaSuperUser and Sito_Cod=6)
UPDATE [dbo].[Configurazione_Siti] set 
    [PivaSuperUser]=@PivaSuperUser 
where
    chiave = 'Tracciabilita_ApiUrlBase_SDF' and
    PivaSuperUser<>@PivaSuperUser and
    Sito_Cod=6

if not exists (select 1 from Configurazione_Siti where chiave = 'Tracciabilita_ApiUrlResource_SDF')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser,6,N'Tracciabilita_ApiUrlResource_SDF',N'')

if exists (select 1 from Configurazione_Siti where chiave = 'Tracciabilita_ApiUrlResource_SDF' and PivaSuperUser<>@PivaSuperUser and Sito_Cod=6)
UPDATE [dbo].[Configurazione_Siti] set 
    [PivaSuperUser]=@PivaSuperUser 
where
    chiave = 'Tracciabilita_ApiUrlResource_SDF' and
    PivaSuperUser<>@PivaSuperUser and
    Sito_Cod=6



-- FINE CHIAVI DI CONFIGURAZIONE PER ESPORTAZIONE TRACIABILITà

if not exists (select 1 from Configurazione_Siti where chiave = 'DocContabile2021DaAgenda')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 6, N'DocContabile2021DaAgenda', '1')

if not exists (select 1 from Configurazione_Siti where chiave = 'LinkManualeUmaCarburanti')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 6, N'LinkManualeUmaCarburanti', '')

-- Chiavi configurazione Lavorazioni e Ordini di lavorazione

if not exists (select 1 from Configurazione_Siti where chiave = 'Lavorazioni')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 0, N'Lavorazioni', N'{"CtrUnivIdenLav":0}')

if not exists (select 1 from Configurazione_Siti where chiave = 'OrdiniLavorazione')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 0, N'OrdiniLavorazione', N'{"CtrUnivIdenLav":0, "CtrLegameLav":0}')

if not exists (select 1 from Configurazione_Siti where chiave = 'Flag_Nuovo_Controllo_Riduzione_Diserbo')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 0, N'Flag_Nuovo_Controllo_Riduzione_Diserbo', '1')
else
    UPDATE Configurazione_Siti set valore='1' where chiave = 'Flag_Nuovo_Controllo_Riduzione_Diserbo'
    AND PivaSuperUser NOT IN ('03790110237')   -- Temporaneo Bypass per non sovrascrivere a 1 la chiave su Collis dove è stato manualmente spento

if not exists (select 1 from Configurazione_Siti where chiave = 'LinkAgronicaGiasNG')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 6, N'LinkAgronicaGiasNG', $(PercorsoLink) + N'/GiasNG/GestioneRichieste')

if not exists (select 1 from Configurazione_Siti where chiave = 'Export_MovimentiMagazzino_Utilizzo_Recode')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 6, N'Export_MovimentiMagazzino_Utilizzo_Recode', N'false')

if not exists (select 1 from Configurazione_Siti where chiave = 'CompressioneRispostaAjax')
INSERT [dbo].[Configurazione_Siti] ( [PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES
        (@PivaSuperUser, 6, N'CompressioneRispostaAjax', '{"AbilitaCompressioneRisposte":true,"DimensioneMinimaPerCompressioneInMB":10}')

-- Chiave configurazione Contratti Affitto
if not exists (select 1 from Configurazione_Siti where chiave = 'ContrattiAffitto')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 0, N'ContrattiAffitto', N'{"ScriviLogAggiornaImpreseParticelle":0}')


-- Chiave sito AgronicaUMA
if not exists (select 1 from Configurazione_Siti where chiave = 'LinkAgronicaUMA')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 6, N'LinkAgronicaUMA', $(PercorsoLink) + N'/AgronicaUMA/GestioneRichieste.aspx')

-- chiave sito AgronicaDomandaIrrigua
if not exists (select 1 from Configurazione_Siti where chiave = 'LinkAgronicaDomandaIrrigua')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 6, N'LinkAgronicaDomandaIrrigua', $(PercorsoLink) + N'/AgronicaDomandaIrrigua/GestioneRichieste.aspx')


if not exists (select 1 from Configurazione_Siti where chiave = 'GiasOnline_Core_API')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 6, N'GiasOnline_Core_API', $(PercorsoLink) + N'/AgronicaCoreAPI')



if not exists (select 1 from Configurazione_Siti where chiave = 'MenuAnagrafeNG')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 6, N'MenuAnagrafeNG', N'false')

if not exists (select 1 from Configurazione_Siti where chiave = 'MenuAgendaNG')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 6, N'MenuAgendaNG', N'false')

if not exists (select 1 from Configurazione_Siti where chiave = 'OperazioniAgendaNG')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 6, N'OperazioniAgendaNG', N'false')

if not exists (select 1 from Configurazione_Siti where chiave = 'MenuGisNG')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 6, N'MenuGisNG', N'false')

if not exists (select 1 from Configurazione_Siti where chiave = 'LinkGiasBase')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 6, N'LinkGiasBase', $(PercorsoLink) + N'/GiasBase')

if not exists (select 1 from Configurazione_Siti where chiave = 'Google_GeocodingBaseUrl')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 6, N'Google_GeocodingBaseUrl', N'https://maps.googleapis.com/maps/api/geocode/json?key=')

IF NOT EXISTS(Select 1 from dbo.Configurazione_Siti where Chiave = 'pathFileRaster')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 6, 'pathFileRaster', '{"BasePathAllegatiRaster": "GisRaster", "FormatiConsentiti": [".tif", ".tiff", ".ecw", ".jp2"]}'	)

IF NOT EXISTS(Select 1 from dbo.Configurazione_Siti where Chiave = 'AuthDispatcher_Configurations')
INSERT INTO [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore])
VALUES (@PivaSuperUser, 6, 'AuthDispatcher_Configurations','{"BaseUrl":"https://authdispatcher.netagronica.it/", "ApiKey":"1e99bea4-d1f7-4c45-826b-feac874834cb"}')

IF NOT EXISTS(Select 1 from dbo.Configurazione_Siti where Chiave = 'GiasOnline_WS_Mappe_2023')
INSERT INTO [Configurazione_Siti] 
([PivaSuperUser], [Sito_Cod], [Chiave], [Valore])
VALUES (@PivaSuperUser, 6, 'GiasOnline_WS_Mappe_2023','{"type" : "1", "baseUrl" : "https://storage.cloud.google.com/", "bucket" : "geewsmappe2023", "obj" : "geeWSELabMappe2023"}')


IF NOT EXISTS(Select 1 from dbo.Configurazione_Siti where Chiave = 'Configurazioni_GoogleEarthEngine')
INSERT INTO [Configurazione_Siti] 
([PivaSuperUser], [Sito_Cod], [Chiave], [Valore])
VALUES (@PivaSuperUser, 6, 'Configurazioni_GoogleEarthEngine','{ "BaseUrl_GEE" : "", "CollectionNames": ["NDVI", "NDMI", "NDWI"], "JSONAuth" : "", "SubmitGISAlgorithm": "SubmitGISAlgorithm", "GEOTIFFUploadOnGCP": "GEOTIFFUploadOnGCP", "GISAlgorithmOnGCP_GEESyncDispatcher": "GEESyncDispatcher", "PrescriptionsMapsOnVegIndexes": "PrescriptionsMapsOnVegIndexes", "QueriesGEEVegIndexesTimeSeries": "QueriesGEEVegIndexesTimeSeries" }')

IF NOT EXISTS(Select 1 from dbo.Configurazione_Siti where Chiave = 'pathFileRaster_GoogleEarthEngine')
INSERT INTO [Configurazione_Siti] 
([PivaSuperUser], [Sito_Cod], [Chiave], [Valore])
VALUES (@PivaSuperUser, 6, 'pathFileRaster_GoogleEarthEngine','{"type" : 1 , "baseUrl" : "https://storage.cloud.google.com/", "bucket" : "agronicageerepobucket", "obj" : "AgronicaGeeRepoObj"}')

IF NOT EXISTS (SELECT 1 FROM Configurazione_Siti WHERE chiave = 'Prefisso_PIVA_Generica_Da_Assegnare')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 6, N'Prefisso_PIVA_Generica_Da_Assegnare', N'')

IF NOT EXISTS (SELECT 1 FROM Configurazione_Siti WHERE chiave = 'Blocco_Inserimento_PIVA_Impresa')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 6, N'Blocco_Inserimento_PIVA_Impresa', N'false')

IF NOT EXISTS (SELECT 1 FROM Configurazione_Siti WHERE chiave = 'Azienda_Timesheet_Tecnici')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 6, N'Azienda_Timesheet_Tecnici', N'')

-- Chiave TestUMA per collaudi su richieste future

if not exists (select 1 from Configurazione_Siti where chiave = 'TestUMA')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 0, N'TestUMA', N'{"Abilitato":0, "Mese":0}')

if not exists (select 1 from Configurazione_Siti where chiave = 'Utenti_Visibilia_Appoggio_Da_Capostipiti')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 0, N'Utenti_Visibilia_Appoggio_Da_Capostipiti', N'0')

-- Chiave ElencoManualiJson per impostare elenco manuali dinamico

if not exists (select 1 from Configurazione_Siti where chiave = 'ElencoManualiJson')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 0, N'ElencoManualiJson', N'')

if not exists (select 1 from Configurazione_Siti where chiave = 'Log_Segnalazioni_Speciali')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 0, N'Log_Segnalazioni_Speciali', N'0')

if not exists (select 1 from Configurazione_Siti where chiave = 'Log_Segnalazioni_Speciali_Mail_A')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 0, N'Log_Segnalazioni_Speciali_Mail_A', N'aggiornamenti@agronica.it')

if not exists (select 1 from Configurazione_Siti where chiave = 'Coldiretti_ElasticSearchUrl')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 0, N'Coldiretti_ElasticSearchUrl', N'')

if not exists (select 1 from Configurazione_Siti where chiave = 'GiasOnline_NetCore_API')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 6, N'GiasOnline_NetCore_API', $(PercorsoLink) + N'/AgronicaNetCoreAPI')

if not exists (select 1 from Configurazione_Siti where chiave = 'ParametriElasticSearch')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 6, N'ParametriElasticSearch', N'{"Ambiente":""}')

if not exists (select 1 from Configurazione_Siti where chiave = 'servizioAutorizzazioneHubMeteo')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 0, N'servizioAutorizzazioneHubMeteo', N'{ "baseUrl": "https://hubmeteo.netagronica.it/MeteoAPI", "user": "GIAS", "password": "#M37304P1#" }')

if not exists (select 1 from Configurazione_Siti where chiave = 'AgroFascicolo_WS_PadrePivaSuperUser')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 0, N'AgroFascicolo_WS_PadrePivaSuperUser', N'false')

if not exists (select 1 from Configurazione_Siti where chiave = 'AgroProfilazione_MaxUtentiCaricatiDefault')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 0, N'AgroProfilazione_MaxUtentiCaricatiDefault', N'200')

if not exists (select 1 from Configurazione_Siti where chiave = 'ConfigChatGPT')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) VALUES 
        (@PivaSuperUser, 0, N'ConfigChatGPT', N'')

IF NOT EXISTS(Select 1 from Configurazione_Siti where Chiave = 'password_smtp_isEncrypted')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore])
    VALUES (@PivaSuperUser, 24, 'password_smtp_isEncrypted','0')

IF NOT EXISTS(Select 1 from Configurazione_Siti where Chiave = 'PasswordArteaWS_isEncrypted')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore])
    VALUES (@PivaSuperUser, 16, 'PasswordArteaWS_isEncrypted','0')

IF NOT EXISTS(Select 1 from Configurazione_Siti where Chiave = 'cr')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore])
    VALUES (@PivaSuperUser, 24, 'cr', NEWID())

IF NOT EXISTS (select * FROM Configurazione_Siti WHERE chiave = 'GiasOnline_WS_UMA_AgroWS_Codifica_Specie_Vegetali_Agea_2015_2020')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore])
    VALUES (@PivaSuperUser, 6, 'GiasOnline_WS_UMA_AgroWS_Codifica_Specie_Vegetali_Agea_2015_2020', 'http://www.agronica.it/AgronicaWebService/AgroWS_UMA_Codifica_Specie_Vegetali_Agea_2015_2020.asmx')

IF NOT EXISTS(Select 1 from Configurazione_Siti where Chiave = 'Configurazioni_WS_Mappe_2024')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore])
    VALUES (@PivaSuperUser, 16, 'Configurazioni_WS_Mappe_2024','{ "baseUrl" : "" , "auth_key" : { "type" : "" , "value" : "" }}')

IF NOT EXISTS(Select 1 from Configurazione_Siti where Chiave = 'useNewZooMenu')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore])
    VALUES (@PivaSuperUser, 0, 'useNewZooMenu','false')

IF NOT EXISTS(Select 1 from Configurazione_Siti where Chiave = 'Verifica_Sottoscrizione_Servizio_QDC')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore],[Visibilita])
    VALUES (@PivaSuperUser, 0, 'Verifica_Sottoscrizione_Servizio_QDC','false',1)

IF NOT EXISTS(Select 1 from Configurazione_Siti where Chiave = 'Tipo_Verifica_Sottoscrizione_Servizio_QDC')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore])
    VALUES (@PivaSuperUser, 0, 'Tipo_Verifica_Sottoscrizione_Servizio_QDC','')

IF NOT EXISTS(Select 1 from Configurazione_Siti where Chiave = 'Configurazioni_WS_Mappe_2024')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore])
    VALUES (@PivaSuperUser, 16, 'Configurazioni_WS_Mappe_2024','{ "baseUrl" : "" , "auth_key" : { "type" : "" , "value" : "" }}')

IF NOT EXISTS(Select 1 from Configurazione_Siti where Chiave = 'SSOLogin_UserCreationConfig')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore])
    VALUES (@PivaSuperUser, 0, 'SSOLogin_UserCreationConfig','{ "profile" : "0" }')

IF NOT EXISTS(Select 1 from Configurazione_Siti where Chiave = 'ServerAddresses')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore])
    VALUES (@PivaSuperUser, 0, 'ServerAddresses','')

IF NOT EXISTS(Select 1 from Configurazione_Siti where Chiave = 'Filtro_SchedaCampagna_ColturaleBiologico_BS')
INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore])
    VALUES (@PivaSuperUser, 0, 'Filtro_SchedaCampagna_ColturaleBiologico_BS','')

IF NOT EXISTS(SELECT 1 FROM Configurazione_Siti WHERE Chiave = 'Sincro_ImportAgrea_Password')
    INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) 
    VALUES (@PivaSuperUser, 16, N'Sincro_ImportAgrea_Password', '')

IF NOT EXISTS(SELECT 1 FROM Configurazione_Siti WHERE Chiave = 'Sincro_ImportAgrea_Username')
    INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) 
    VALUES (@PivaSuperUser, 16, N'Sincro_ImportAgrea_Username', '')

IF NOT EXISTS(SELECT 1 FROM Configurazione_Siti WHERE Chiave = 'Sincro_ImportAgrea_Link')
    INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) 
    VALUES (@PivaSuperUser, 16, N'Sincro_ImportAgrea_Link', 'http://agreagestione.regione.emilia-romagna.it/FornituraServiziWS/services/GetPianoColturale')

IF NOT EXISTS(SELECT 1 FROM Configurazione_Siti WHERE Chiave = 'Sincro_ImportAgrea_FlagPasswordCriptata')
    INSERT [dbo].[Configurazione_Siti] ([PivaSuperUser], [Sito_Cod], [Chiave], [Valore]) 
    VALUES (@PivaSuperUser, 16, N'Sincro_ImportAgrea_FlagPasswordCriptata', '0')

--LAST ServerDB (!! INSERIRE SOPRA !!)



-- LASCIARE COME TERZULTIMA ISTRUZIONE:  Chiavi visibili lato client
UPDATE [dbo].[Configurazione_Siti] SET Visibilita = 1
WHERE chiave IN 
('servizioAutorizzazioneHubMeteo',
'AgroProfilazione_MaxUtentiCaricatiDefault',
'BloccaCoreWS_NG',
'personalizzazioniRegioneUmbria',
'Blocco_Inserimento_PIVA_Impresa',
'ListaStiliPersonalizzati_NG',
'LinkAgronicaGiasNG',
'MenuAgendaNG',
'OperazioniAgendaNG',
'googlemaps',
'Verifica_Sottoscrizione_Servizio_QDC')


-- LASCIARE COME PENULTIMA ISTRUZIONE:  Update chiave GIS_EscludiFiltroCodiceFiscaleTecnico

/* Lista di database separati da virgola, dove impostare il flag "GIS_EscludiFiltroCodiceFiscaleTecnico" a false  */
DECLARE @ServerDBCFTecFalse nvarchar(1000) = 'AgronicaSementi2013_server'

DECLARE @ServerDBReplace nvarchar(100) = '$(ServerDB)'
SET @ServerDBReplace = Replace(Replace(@ServerDBReplace, '[',''), ']', '')

declare @valGIS_EscludiFiltroCodiceFiscaleTecnico varchar(1000)
set @valGIS_EscludiFiltroCodiceFiscaleTecnico = 'true'

if (@ServerDBCFTecFalse like '%'+@ServerDBReplace+'%' )
    set  @valGIS_EscludiFiltroCodiceFiscaleTecnico = 'false'

update s
    set valore = @valGIS_EscludiFiltroCodiceFiscaleTecnico
from configurazione_siti s
where chiave = 'GIS_EscludiFiltroCodiceFiscaleTecnico'


-- LASCIARE COME ULTIMA ISTRUZIONE:  Update per eliminare gli spazi generati con il declare char(25)
UPDATE [dbo].[Configurazione_Siti] 
SET PivaSuperUser= RTRIM ( PivaSuperUser )

-- ATTENZIONE! lasciare il go per ultimo!!!!
GO





/* ########################################################################################## */
/* ########## Versione Database ############################################################# */
/* ########################################################################################## */
print ('Versione $(VersioneConfigurazioneSiti)')

IF NOT EXISTS (SELECT 1 FROM [dbo].[Versione_Database] WHERE [Versione] = 'VersioneConfigurazioneSiti $(VersioneConfigurazioneSiti)' )
BEGIN
INSERT INTO Versione_Database  (Versione , Username_Creazione, Username_Modifica)
VALUES ('VersioneConfigurazioneSiti $(VersioneConfigurazioneSiti)', 'script VersioneConfigurazioneSiti $(VersioneConfigurazioneSiti)', 'script VersioneConfigurazioneSiti $(VersioneConfigurazioneSiti)')
END
ELSE
BEGIN
    print ('Aggiornamento $(ServerDB), VersioneConfigurazioneSiti $(VersioneConfigurazioneSiti): già eseguito ...')
END
GO
