Imports System.IO
Imports AgronicaCoreUtilityVersioni
Imports AgronicaCoreUtilityVersioni.Enumerativi

Namespace CoreVersione

    Public Class Versione
        Inherits System.Web.UI.Page

        Private _objChangeSito As Changelog_Agronica_Obj

#Region "Costruttori"

        Public Sub New()
            _objChangeSito = Nothing
        End Sub

        Public Sub New(ByVal projectName As String)
            _objChangeSito = New Changelog_Agronica_Obj(projectName)
        End Sub

#End Region

#Region "Crea Changelog"

        Public Function Create_Changelog(ByVal dirOutput As String) As String
            Dim pathFileJson As String = ""

            Try

                If _objChangeSito Is Nothing Then
                    Throw New NullReferenceException("objChangeSito non è stato inizializzato")
                End If
                If String.IsNullOrEmpty(dirOutput) Then
                    Throw New ArgumentNullException(NameOf(dirOutput))
                End If

                Dim fileName As String = Replace(_objChangeSito.progetto, " ", "") & ".json"

                'Chiamo il Versione.aspx
                Carica_Pannello()

                pathFileJson = Path.Combine(dirOutput, fileName)
                Dim res As Boolean = _objChangeSito.ScriviJson(pathFileJson)

                If Not res Then
                    pathFileJson = ""
                End If

            Catch ex As Exception
                pathFileJson = ""
                Throw New Exception(ex.Message)
            End Try

            Return pathFileJson
        End Function

#End Region

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Carica_Pannello()
        End Sub


        Private Sub Carica_Pannello()

            '§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§
            '                     CHI COMPILA
            '           SI PREOCCUPI DI VERIFICARE COSA SERVE:
            '                       NEI COM,
            '                       NEL MIGRA,
            '                       NEL WEB.CONFIG
            '§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§

            Riga_Versione("18 Giugno 2025 BIS", "138.3.5")

            Riga_Requisiti("AgronicaWebService2010",
                           "Per lettura indici Maturità e Danni Raccolta senza specie 
                           Per lettura Erbe Infestanti da WS", "14/04/2025")

            Riga_Requisiti("Migra",
                           "Per Porting da LAN",
                           Ver:="787")

            Riga_Requisiti("Aggancio",
                           "Aggiunte viste in lingua del modulo zootecnia", "130")

            Riga_Requisiti("Configurazione_Siti",
                           "Aggiunta chiave 'UserPwdConnectionString_toCrypt' (super server)", "150")

            Riga_Requisiti("AgronicaCoreAPI",
                           "Aggiunta possibilità di configurare la lista dei Widgets diversamente dal Preset",
                           "14/04/2025")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")

            Riga_Changelog(
                enum_Tipo_Changelog.Feature,
                area:="API NewAgri - OperazioniQdC",
                descrizione:="Aggiunto all'import delle concimazioni organiche l' N del Fertilizzante e il Regolamento_Cod",
                cliente:="Regione Umbria",
                idTicketAssistenza:=0,
                idTicketSviluppo:=180584,
                idTicketTesting:=0,
                autore:=DEV_LC
            )

            '---------------------------------------------------------------------------

            Riga_Versione("18 Giugno 2025", "138.3.4")

            Riga_Requisiti("AgronicaWebService2010",
                           "Per lettura indici Maturità e Danni Raccolta senza specie 
                           Per lettura Erbe Infestanti da WS", "14/04/2025")

            Riga_Requisiti("Migra",
                           "Per Porting da LAN",
                           Ver:="787")

            Riga_Requisiti("Aggancio",
                           "Aggiunte viste in lingua del modulo zootecnia", "130")

            Riga_Requisiti("Configurazione_Siti",
                           "Aggiunta chiave 'UserPwdConnectionString_toCrypt' (super server)", "150")

            Riga_Requisiti("AgronicaCoreAPI",
                           "Aggiunta possibilità di configurare la lista dei Widgets diversamente dal Preset",
                           "14/04/2025")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")

            Riga_Changelog(
                           enum_Tipo_Changelog.Feature,
                          "Controlli cancellazione Impianti x UMA",
                           "Introdotto controllo alla cancellazione di un impianto da EDIT COMPLETA: se risulta associato ad una pratica UMA (dal 2025 in poi), la cancellazione verrà impedita",
                           cliente:="Regione Umbria",
                           idTicketAssistenza:=0, idTicketSviluppo:=163342, idTicketTesting:=0,
                           autore:="Anna Funciello",
                           noteTest:="Provare a cancellare un impianto da EDIT COMPLETA utilizzato all'interno di una pratica UMA (dal 2025 in poi).
                           Verificare il blocco dell'operazione.",
                           noteTecniche:="")
                           
           Riga_Changelog(
                           enum_Tipo_Changelog.Feature,
                          "Consulta Sincro Dati App (modalità Demetra/NewAgri)",
                           "Modificate label 'Tipo Esportazione': rimossi valori D2G/G2D, sostituiti con generici Import/Export
                           Modificati valori colonna 'Descrizione': sostituiti Chiave Gias/Demetra con Chiave Interna/Esterna",
                           cliente:="Regione Umbria",
                           idTicketAssistenza:=0, idTicketSviluppo:=0, idTicketTesting:=0,
                           autore:="Anna Funciello",
                           noteTest:="",
                           noteTecniche:="")
            '---------------------------------------------------------------------------

            Riga_Versione("05 Giugno 2025", "138.3.3")

            Riga_Requisiti("AgronicaWebService2010",
                           "Per lettura indici Maturità e Danni Raccolta senza specie 
                           Per lettura Erbe Infestanti da WS", "14/04/2025")

            Riga_Requisiti("Migra",
                           "Per Porting da LAN",
                           Ver:="787")

            Riga_Requisiti("Aggancio",
                           "Aggiunte viste in lingua del modulo zootecnia", "130")

            Riga_Requisiti("Configurazione_Siti",
                           "Aggiunta chiave 'UserPwdConnectionString_toCrypt' (super server)", "150")

            Riga_Requisiti("AgronicaCoreAPI",
                           "Aggiunta possibilità di configurare la lista dei Widgets diversamente dal Preset",
                           "14/04/2025")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")

            Riga_Changelog(
                           enum_Tipo_Changelog.Bug,
                           "N_Distribuito (NewAgri)",
                           "Sistemata estrazione impianti da chiave AGEA",
                           cliente:="Regione Umbria",
                           idTicketAssistenza:=0, idTicketSviluppo:=163385, idTicketTesting:=0,
                           autore:=DEV_Funcy,
                           noteTest:="",
                           noteTecniche:="")
            '---------------------------------------------------------------------------

            Riga_Versione("04 Giugno 2025", "138.3.2")

            Riga_Requisiti("AgronicaWebService2010",
                           "Per lettura indici Maturità e Danni Raccolta senza specie 
                           Per lettura Erbe Infestanti da WS", "14/04/2025")

            Riga_Requisiti("Migra",
                           "Per Porting da LAN",
                           Ver:="787")

            Riga_Requisiti("Aggancio",
                           "Aggiunte viste in lingua del modulo zootecnia", "130")

            Riga_Requisiti("Configurazione_Siti",
                           "Aggiunta chiave 'UserPwdConnectionString_toCrypt' (super server)", "150")

            Riga_Requisiti("AgronicaCoreAPI",
                           "Aggiunta possibilità di configurare la lista dei Widgets diversamente dal Preset",
                           "14/04/2025")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")

            Riga_Changelog(
                           enum_Tipo_Changelog.Bug,
                           area:="QdCA - Diserbo",
                           descrizione:="Sistemato errore che appare quando si va in modifica di un prodotto già inserito",
                           cliente:="Propar",
                           idTicketAssistenza:=176703, idTicketSviluppo:=0, idTicketTesting:=0,
                           autore:=DEV_LC,
                           noteTest:="",
                           noteTecniche:="")

            '==================================================================

            Riga_Versione("03 Giugno 2025", "138.3.1")

            Riga_Requisiti("AgronicaWebService2010",
                           "Per lettura indici Maturità e Danni Raccolta senza specie 
                           Per lettura Erbe Infestanti da WS", "14/04/2025")

            Riga_Requisiti("Migra",
                           "Per Porting da LAN",
                           Ver:="787")

            Riga_Requisiti("Aggancio",
                           "Aggiunte viste in lingua del modulo zootecnia", "130")

            Riga_Requisiti("Configurazione_Siti",
                           "Aggiunta chiave 'UserPwdConnectionString_toCrypt' (super server)", "150")

            Riga_Requisiti("AgronicaCoreAPI",
                           "Aggiunta possibilità di configurare la lista dei Widgets diversamente dal Preset",
                           "14/04/2025")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")

            Riga_Changelog(
                           enum_Tipo_Changelog.Bug,
                           area:="Anagrafica Imprese",
                           descrizione:="Errore lettura popolamento ddl imprese padri",
                           cliente:="TUTTI",
                           idTicketAssistenza:=177308, idTicketSviluppo:=0, idTicketTesting:=0,
                           autore:=DEV_Drudi,
                           noteTest:="",
                           noteTecniche:="")

            '==================================================================

            Riga_Versione("30 Maggio 2025", "138.3.0")

            Riga_Requisiti("AgronicaWebService2010",
                           "Per lettura indici Maturità e Danni Raccolta senza specie 
                           Per lettura Erbe Infestanti da WS", "14/04/2025")

            Riga_Requisiti("Migra",
                           "Per Porting da LAN",
                           Ver:="787")

            Riga_Requisiti("Aggancio",
                           "Aggiunte viste in lingua del modulo zootecnia", "130")

            Riga_Requisiti("Configurazione_Siti",
                           "Aggiunta chiave 'UserPwdConnectionString_toCrypt' (super server)", "150")

            Riga_Requisiti("AgronicaCoreAPI",
                           "Aggiunta possibilità di configurare la lista dei Widgets diversamente dal Preset",
                           "14/04/2025")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")

            Riga_Changelog(
                           enum_Tipo_Changelog.Bug,
                           area:="Ricevimento concimazioni organiche in GIAS da New Agri",
                           descrizione:="Cambiato messaggio di errore nel caso in cui la superficie trattata sia maggiore della superficie anagrafica dell'impianto",
                           cliente:="Regione Umbria",
                           idTicketAssistenza:=0, idTicketSviluppo:=153538, idTicketTesting:=0,
                           autore:=DEV_LC,
                           noteTest:="Nessun test necessario",
                           noteTecniche:="")

            Riga_Changelog(
                           enum_Tipo_Changelog.Bug,
                           area:="Utenti e Permessi (new!)",
                           descrizione:="Corregge il salvataggio del campo codice fiscale al primo login del'utente e
                           durate la creazione degli utenti di tipo azienda",
                           cliente:="Coprob",
                           idTicketAssistenza:=178033, idTicketSviluppo:=0, idTicketTesting:=0,
                           autore:="Anny Bevilacqua")

            Riga_Changelog(
                           enum_Tipo_Changelog.Bug,
                           area:="API NewAgri Aggiorna_Impianti_N_Pua",
                           descrizione:="Corregge l'estrazione degli esercizi Demetra-NewAgri",
                           cliente:="NewAgri",
                           idTicketAssistenza:=0, idTicketSviluppo:=0, idTicketTesting:=153538,
                           autore:="Anny Bevilacqua")

            Riga_Changelog(
                           enum_Tipo_Changelog.Bug,
                           area:="Menu Visite (new!)",
                           descrizione:="Corretta la query sintetica delle Visite che riportava delle righe duplicate e aggiunte le colonne di Lat/Long relative alle visite",
                           cliente:="CAI",
                           idTicketAssistenza:=178233, idTicketSviluppo:=0, idTicketTesting:=0,
                           autore:="Dario Cabras")

            '==================================================================

            Riga_Versione("23 Maggio 2025", "138.2.0")

            Riga_Requisiti("AgronicaWebService2010",
                           "Per lettura indici Maturità e Danni Raccolta senza specie 
                           Per lettura Erbe Infestanti da WS", "14/04/2025")

            Riga_Requisiti("Migra",
                           "Per Porting da LAN",
                           Ver:="787")

            Riga_Requisiti("Aggancio",
                           "Aggiunte viste in lingua del modulo zootecnia", "130")

            Riga_Requisiti("Configurazione_Siti",
                           "Aggiunta chiave 'UserPwdConnectionString_toCrypt' (super server)", "150")

            Riga_Requisiti("AgronicaCoreAPI",
                           "Aggiunta possibilità di configurare la lista dei Widgets diversamente dal Preset",
                           "14/04/2025")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")

            Riga_Changelog(
                enum_Tipo_Changelog.Feature,
                area:="Reportistica Zootecnica",
                descrizione:="Aggiunta colonna che descrive la 'Data Disponibilità' nella griglia 'Controllo Trattamenti', ottenuta aggiungendo alla 'Data Ultimo Trattamento' il numero di giorni specificati dall'utente sui filtri soprastanti la griglia",
                cliente:="Inalca",
                idTicketAssistenza:=0,
                idTicketSviluppo:=176584,
                idTicketTesting:=0,
                autore:="Dario Cabras",
                noteTest:="Verificare la visualizzazione della nuova colonna in Reportistica Zootecnica -> Controllo Trattamenti",
                noteTecniche:=""
                )

            Riga_Changelog(
                           enum_Tipo_Changelog.Feature,
                          "Controlli cancellazione Impianti x UMA",
                           "Introdotto controllo alla cancellazione di un impianto: se risulta associato ad una pratica UMA (dal 2025 in poi), la cancellazione verrà impedita",
                           cliente:="Regione Umbria",
                           idTicketAssistenza:=0, idTicketSviluppo:=163342, idTicketTesting:=0,
                           autore:="Anna Funciello",
                           noteTest:="Provare a cancellare un impianto utilizzato all'interno di una pratica UMA (dal 2025 in poi).
                           Verificare il blocco dell'operazione.",
                           noteTecniche:="")

            Riga_Changelog(
                           enum_Tipo_Changelog.Feature,
                          "Monitor Log Interscambio (pagina Consulta Sincro Dati App)",
                           "Aggiunto messaggio di riepilogo Piani Colturali in coda per l'importazione
                           Il messaggio è visibile se:
                           - non si applica nessun filtro sul 'Tipo Dato' (= mostra tutti i log)
                           - tra i 'Tipo Dati' selezionati c'è la voce 'D2G - Piano Colturale'
                           NB. i filtri data NON hanno rilevanza",
                           cliente:="Coldiretti",
                           idTicketAssistenza:=0, idTicketSviluppo:=163176, idTicketTesting:=0,
                           autore:=DEV_Funcy,
                           noteTest:="",
                           noteTecniche:="")

            Riga_Changelog(
                           enum_Tipo_Changelog.Feature,
                           area:="Ricevimento concimazioni organiche in GIAS da New Agri",
                           descrizione:="Aggiunto controllo che la superficie trattata degli impianti ricevuti da NewAgri non sia maggiore della superficie anagrafica registrata su GIAS",
                           cliente:="Regione Umbria",
                           idTicketAssistenza:=0, idTicketSviluppo:=153538, idTicketTesting:=0,
                           autore:=DEV_LC,
                           noteTest:="Nessun test necessario",
                           noteTecniche:="")

            '==================================================================

            Riga_Versione("21 Maggio 2025", "138.1.1")

            Riga_Requisiti("AgronicaWebService2010",
                           "Per lettura indici Maturità e Danni Raccolta senza specie 
                           Per lettura Erbe Infestanti da WS", "14/04/2025")

            Riga_Requisiti("Migra",
                           "Per Porting da LAN",
                           Ver:="787")

            Riga_Requisiti("Aggancio",
                           "Aggiunte viste in lingua del modulo zootecnia", "130")

            Riga_Requisiti("Configurazione_Siti",
                           "Aggiunta chiave 'UserPwdConnectionString_toCrypt' (super server)", "150")

            Riga_Requisiti("AgronicaCoreAPI",
                           "Aggiunta possibilità di configurare la lista dei Widgets diversamente dal Preset",
                           "14/04/2025")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")
            Riga_Changelog(
                enum_Tipo_Changelog.Feature,
                area:="Ricevimento concimazioni organiche in GIAS da New Agri",
                descrizione:="- Aggiunto controllo che la superficie trattata degli impianti sia maggiore di 0" &
                "- Forzata l' importazione diretta nel Brogliaccio senza leggere l'impostazione utente" &
                "- Cambiati messaggi di errore",
                cliente:="Regione Umbria",
                idTicketAssistenza:=0,
                idTicketSviluppo:=153538,
                idTicketTesting:=0,
                autore:=DEV_LC,
                noteTest:="Nessun test necessario",
                noteTecniche:=""
                )

            Riga_Changelog(enum_Tipo_Changelog.Bug,
                           "Operazioni Zootecniche (new) ",
                           "Gestiti redirect anche verso la nuova pagina delle operazioni zootecniche quando si genera e/o si registra un modello 4",
                           cliente:="Inalca",
                           idTicketAssistenza:=175325, idTicketSviluppo:=0, idTicketTesting:=0,
                           autore:="Riccardo Drudi",
                           noteTest:="Cliccare sul bottone genera modello 4 o registra modello 4 e poi nella pagina premere il bottone indietro e verificare il redirect",
                           noteTecniche:="")

            '==================================================================

            Riga_Versione("16 Maggio 2025", "138.1.0")

            Riga_Requisiti("AgronicaWebService2010",
                           "Per lettura indici Maturità e Danni Raccolta senza specie 
                           Per lettura Erbe Infestanti da WS", "14/04/2025")

            Riga_Requisiti("Migra",
                           "Per Porting da LAN",
                           Ver:="787")

            Riga_Requisiti("Aggancio",
                           "Aggiunte viste in lingua del modulo zootecnia", "130")

            Riga_Requisiti("Configurazione_Siti",
                           "Aggiunta chiave 'UserPwdConnectionString_toCrypt' (super server)", "150")

            Riga_Requisiti("AgronicaCoreAPI",
                           "Aggiunta possibilità di configurare la lista dei Widgets diversamente dal Preset",
                           "14/04/2025")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")

            Riga_Changelog(
                enum_Tipo_Changelog.Bug,
                "GIS - CheckList",
                "Fix query estrazione log elaborazione Checklist",
                cliente:="ENI",
                idTicketAssistenza:=175670, idTicketSviluppo:=0, idTicketTesting:=0,
                autore:="Lorenzo Lavezzo", noteTest:=""
                )

            Riga_Changelog(
                enum_Tipo_Changelog.Bug,
                area:="GoogleEarth Engine",
                descrizione:="Corretto bug che impediva la ricezione del risultato elaborazioni da Google Cloud Platform",
                cliente:="CAI",
                idTicketAssistenza:=175749, idTicketSviluppo:=0, idTicketTesting:=0,
                autore:="Lorenzo Lavezzo",
                noteTecniche:="Corretto errato casting ed ottimizzato controllo GUID_Entita"
            )

            Riga_Changelog(
                enum_Tipo_Changelog.Feature,
                area:="API NewAgri",
                descrizione:="Aggiunge nuovo endpoint Import/Aggiorna_Impianti_N_Pua",
                cliente:="Regione Umbria",
                idTicketAssistenza:=0, idTicketSviluppo:=153538, idTicketTesting:=0,
                autore:="Anny Bevilacqua"
            )

            Riga_Changelog(
                enum_Tipo_Changelog.Bug,
                area:="Anagrafica catasto",
                descrizione:="Corretto bug che mandava in errore l'apertura in lettura/modifica della particella perché c'erano dei valori Null nella tabella ParticelleCatastalixMacrousi",
                cliente:="AGRISOL",
                idTicketAssistenza:=176004, idTicketSviluppo:=0, idTicketTesting:=0,
                autore:=DEV_Drudi,
                noteTecniche:="",
                noteTest:="Il ticket è già chiuso perché in prod dal cliente ho fatto query per mettere valori di default in quella tabella"
            )

            '==================================================================

            Riga_Versione("14 Maggio 2025", "138.0.1")

            Riga_Requisiti("AgronicaWebService2010",
                           "Per lettura indici Maturità e Danni Raccolta senza specie 
                           Per lettura Erbe Infestanti da WS", "14/04/2025")

            Riga_Requisiti("Migra",
                           "Per Porting da LAN",
                           Ver:="787")

            Riga_Requisiti("Aggancio",
                           "Aggiunte viste in lingua del modulo zootecnia", "130")

            Riga_Requisiti("Configurazione_Siti",
                           "Aggiunta chiave 'UserPwdConnectionString_toCrypt' (super server)", "150")

            Riga_Requisiti("AgronicaCoreAPI",
                           "Aggiunta possibilità di configurare la lista dei Widgets diversamente dal Preset",
                           "14/04/2025")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")

            Riga_Changelog(
                enum_Tipo_Changelog.Bug,
                "Concia del Seme - Caricamento Prodotti da Trattare",
                "Sistemato filtro Prodotti x Specie nell'operazione Concia del Seme",
                cliente:="CAI",
                idTicketAssistenza:=174968, idTicketSviluppo:=0, idTicketTesting:=0,
                autore:=DEV_Funcy, noteTest:="Caricare a magazzino il semente di due specie diverse (es. Cocomero e Cipolla)
                Nell'operazione di concia del seme i prodotti devono essere filtrati correttamente in base alla Specie selezionata"
                )
            Riga_Changelog(
                enum_Tipo_Changelog.Bug,
                "Modifica Motorino per separare la gestione carico/scarico zoo di aboca",
                "Gestito impostazione utenti per diversificare la gestione di aboca",
                cliente:="",
                idTicketAssistenza:=0, idTicketSviluppo:=0, idTicketTesting:=171565,
                autore:="Marco Lucchi"
                )

            Riga_Changelog(
                enum_Tipo_Changelog.Bug,
                "DSS Difesa",
                "Campo inizio evento, per gli eventi pioggia, passato come stringa per preservare il formato corretto a front-end.",
                cliente:="ERSAF",
                idTicketAssistenza:=0, idTicketSviluppo:=170681, idTicketTesting:=0,
                autore:="Andrea Novaga"
                )

            '==================================================================

            Riga_Versione("12 Maggio 2025", "138.0.0")

            Riga_Requisiti("AgronicaWebService2010",
                           "Per lettura indici Maturità e Danni Raccolta senza specie 
                           Per lettura Erbe Infestanti da WS", "14/04/2025")

            Riga_Requisiti("Migra",
                           "Per Porting da LAN",
                           Ver:="787")

            Riga_Requisiti("Aggancio",
                           "Aggiunte viste in lingua del modulo zootecnia", "130")

            Riga_Requisiti("Configurazione_Siti",
                           "Aggiunta chiave 'UserPwdConnectionString_toCrypt' (super server)", "150")

            Riga_Requisiti("AgronicaCoreAPI",
                           "Aggiunta possibilità di configurare la lista dei Widgets diversamente dal Preset",
                           "14/04/2025")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")

            Riga_Changelog(
                enum_Tipo_Changelog.Bug,
                "Invio Log Elastic Search - LOG APPLICATIVI (DataProvider)",
                "Rimosso possibile loop infinito in caso di errore di invio a ElasticSearch",
                cliente:="",
                idTicketAssistenza:=0, idTicketSviluppo:=0, idTicketTesting:=0,
                autore:=DEV_Funcy
                )

            Riga_Changelog(
                enum_Tipo_Changelog.Bug,
                "Widget Colture Principali Attive",
                "Aggiunto filtro per impianti attivi nell'anno solare (allineato funzionamento agli altri widget della dashboard)",
                cliente:="Cantina Valpantena",
                idTicketAssistenza:=173590, idTicketSviluppo:=0, idTicketTesting:=0,
                autore:=DEV_LC
                )

            Riga_Changelog(enum_Tipo_Changelog.Feature,
                           "Conferimento",
                           "Porting da LAN - introdotte logiche di scrittura per le nuove pagine",
                           cliente:="Porting LAN", idTicketAssistenza:=0, idTicketSviluppo:=164552, idTicketTesting:=0,
                           autore:="Marco Cecalupo")

            Riga_Changelog(
                enum_Tipo_Changelog.Bug,
                "Widgets",
                "sistemato errore generato in background al caricamento e alla modifica della lista dei widget",
                cliente:="Tutti",
                idTicketAssistenza:=0, idTicketSviluppo:=165875, idTicketTesting:=0,
                autore:="Dario Cabras",
                noteTest:="segnalato internamente"
                )

            Riga_Changelog(
                enum_Tipo_Changelog.Bug,
                "Operazione default da impianto",
                "Aggiunta possibilità di scegliere operazione di semina",
                cliente:="",
                idTicketAssistenza:=175049, idTicketSviluppo:=0, idTicketTesting:=0,
                autore:=DEV_Drudi
                )

            Riga_Changelog(enum_Tipo_Changelog.Feature,
                           "Anagrafica Imprese",
                           "Aggiunto CUAA padre (nella colonna della ragione sociale) nella griglia delle imprese",
                           cliente:="Coldiretti", idTicketAssistenza:=0, idTicketSviluppo:=174939, idTicketTesting:=0,
                           autore:=DEV_Drudi)

            Riga_Changelog(
                enum_Tipo_Changelog.Feature,
                "Lettura Impostazioni Imprese e SuperUser",
                "Modificata Lettura a Scalare di Imprese_Impostazioni per basarsi sulla tabella Guida_Impostazioni",
                cliente:="CAI e ERSAF",
                idTicketAssistenza:=164048, idTicketSviluppo:=163292, idTicketTesting:=0,
                autore:="Gianluca Amoroso",
                noteTecniche:="TFS 8053",
                noteTest:="")

            Riga_Changelog(enum_Tipo_Changelog.Feature,
                           "Menu Zoo NG",
                           "Aggiorna la funzione di blocco/sblocco operazioni per gestire le attività zootecniche",
                           cliente:="Inalca",
                           idTicketAssistenza:=0, idTicketSviluppo:=162708, idTicketTesting:=0,
                           autore:="Anny Bevilacqua",
                           noteTest:="")

            Riga_Changelog(enum_Tipo_Changelog.Feature,
                           "CDG Zoo",
                           " - Modifica Motorino CDG per Carico/Scarico Consistenze Zootecniche",
                           cliente:="Inalca", idTicketAssistenza:=0, idTicketSviluppo:=171565, idTicketTesting:=0,
                           autore:="Marco Lucchi")

            Riga_Changelog(enum_Tipo_Changelog.Feature,
                           "QdCA",
                           "Aggiunto controllo validità Macchina al salvataggio dell'Operazione",
                           noteTest:="Salvare una nuova Operazione QdCA indicando una Macchina, riaprire in modifica l'Operazione " &
                           "(cambiando la data scegliendone una fuori dal periodo di validità della macchina). Verificare che appaia il messaggio di blocco al salvatggio dell'Operazione.",
                           cliente:="Coldiretti", idTicketAssistenza:=0, idTicketSviluppo:=168148, idTicketTesting:=0,
                           autore:=DEV_LC)

            Riga_Changelog(enum_Tipo_Changelog.Feature,
                           "Importazione Documenti",
                           " - Inserito campo 'Data Documento' in documenti e colonne Data_Documento e Numero_Documento in Tabella APP_Documenti",
                           cliente:="Zani", idTicketAssistenza:=0, idTicketSviluppo:=163032, idTicketTesting:=0,
                           autore:="Marco Lucchi")

            Riga_Changelog(enum_Tipo_Changelog.Feature,
                           "Audit / Qualita e Tracciabilita",
                           "Aggiunta gestione chiamate per redirect a pagine Coop e Lab Controllo Qualita",
                           noteTest:="Da Dashboard selezionare le voci di menu Coop e Lab Controllo Qualita. Verificare che si apra la pagina corretta.",
                           cliente:="Fruttagel", idTicketAssistenza:=0, idTicketSviluppo:=172031, idTicketTesting:=0,
                           autore:=DEV_Casmatt)

            Riga_Changelog(
                enum_Tipo_Changelog.Feature,
                area:="Copia Sposta Impianti",
                descrizione:="Aggiunta traduzione per intestazione messaggio errori",
                cliente:="Orogel",
                idTicketAssistenza:=172744,
                idTicketSviluppo:=0,
                idTicketTesting:=0,
                autore:="Salvatore Zammataro",
                noteTest:="",
                noteTecniche:=""
                )

            Riga_Changelog(
                enum_Tipo_Changelog.Feature,
                area:="Ricevimento concimazioni organiche in GIAS",
                descrizione:="Sviluppato nuovo endpoint per il ricevimento delle concimazioni organiche in GIAS da NewAgri",
                cliente:="Regione Umbria",
                idTicketAssistenza:=0,
                idTicketSviluppo:=153538,
                idTicketTesting:=0,
                autore:=DEV_LC,
                noteTest:="Ri-testare anche il flusso Demetra verso Gias con varie Operazioni di campagna (trattamenti,fertilizzazioni) verificando che atterrino correttamente sul Brogliaccio GIAS." &
                "(Test necessario perché sono state toccate parti in comune con Demetra)",
                noteTecniche:=""
                )

            Riga_Changelog(
                enum_Tipo_Changelog.Feature,
                "DSS Difesa",
                "Gestione unificata per la pagina Indicatori DSS sia da Dashboard che da pagina DSS Difesa",
                cliente:="",
                idTicketAssistenza:=0, idTicketSviluppo:=0, idTicketTesting:=0,
                autore:="Gabriele Venturi",
                noteTest:="verificare che gli indicatori DSS in dashboard compaiano correttamente sia in modalità configurazione stazioni che da impianti",
                noteTecniche:=""
                )

            Riga_Changelog(
                enum_Tipo_Changelog.Feature,
                "DSS Difesa",
                "Gestione meteo per indicatori con gg forecast come numero intero e non come flag si/no",
                cliente:="",
                idTicketAssistenza:=0, idTicketSviluppo:=0, idTicketTesting:=0,
                autore:="Gabriele Venturi",
                noteTest:="Verificare che gli indicatori DSS siano visualizzati con i gg di previsionale come da configurazione",
                noteTecniche:=""
                )

            Riga_Changelog(
                enum_Tipo_Changelog.Feature,
                "DSS Difesa",
                "Risposta indicatori con elenco eventi pioggia",
                cliente:="",
                idTicketAssistenza:=0, idTicketSviluppo:=0, idTicketTesting:=0,
                autore:="Gabriele Venturi",
                noteTest:="",
                noteTecniche:="Arricchita la risposta da WS con elenco eventi pioggia per il periodo"
                )

            Riga_Changelog(
                enum_Tipo_Changelog.Feature,
                "DSS Difesa",
                "Risposta indicatori con informazione chiave impianto se possibile",
                cliente:="",
                idTicketAssistenza:=0, idTicketSviluppo:=0, idTicketTesting:=0,
                autore:="Gabriele Venturi",
                noteTest:="",
                noteTecniche:="Arricchita la risposta da WS con la chiave impianto per poter chiedere il trattamento fitosanitario"
                )

            Riga_Changelog(
                enum_Tipo_Changelog.Feature,
                "operazioni QdCA e Brogliaccio su Macchine",
                "Aggiornati controlli su modifica e eliminazione macchine, aggiunto controllo su data, allineati controlli Demetra a Gias",
                cliente:="Coldiretti",
                noteTest:="Creare una macchina, 
                inserire un movimento/ricetta o costo e testare che la modifica e eliminazione della macchina funzionino correttamente, 
                testare modifica inline e completa",
                idTicketAssistenza:=0, idTicketSviluppo:=168148, idTicketTesting:=0,
                autore:=DEV_Casmatt
                )


            '==================================================================

            Riga_Versione("05 Maggio 2025", "137.3.1")

            Riga_Requisiti("AgronicaWebService2010",
                           "Per lettura indici Maturità e Danni Raccolta senza specie 
                           Per lettura Erbe Infestanti da WS", "14/04/2025")

            Riga_Requisiti("Migra",
                           "Aggiunge i permessi per le tab di Protocolli Terapeutici e Indicazioni Terapeutiche nel Menu Zoo NG",
                           Ver:="785")

            Riga_Requisiti("Aggancio",
                           "Aggiunte viste in lingua del modulo zootecnia", "130")

            Riga_Requisiti("Configurazione_Siti",
                           "Aggiunta chiave 'UserPwdConnectionString_toCrypt' (super server)", "150")

            Riga_Requisiti("AgronicaCoreAPI",
                           "Aggiunta possibilità di configurare la lista dei Widgets diversamente dal Preset",
                           "14/04/2025")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")

            Riga_Changelog(
                enum_Tipo_Changelog.Bug,
                area:="Impostazioni Utente (new!)",
                descrizione:="Corregge salvataggio impostazioni 3,4,6 (filtri su gruppi, specie e varietà vegetali) in
                modo da aderire strettamente al comportamento mostrato nella vecchia profilazione",
                cliente:="CAI",
                idTicketAssistenza:=174154, idTicketSviluppo:=0, idTicketTesting:=0,
                autore:="Anny Bevilacqua",
                noteTecniche:="Ora rimuovendo i valori presenti in uno dei filtri, oltre a rimuovere i record inerenti
                nella tabella Utenti_Impostazioni_FiltroMono, verrà eliminato anche il record da Utenti_Impostazioni"
            )

            Riga_Changelog(
              enum_Tipo_Changelog.Bug,
              area:="Caricamento Specie Vegetali QdC x Visibilità Utente",
              descrizione:="Integrato controllo su visibilità utente quando si caricano le specie vegetali presenti a magazzino
                Sia da menu agenda che da edit operazione",
              cliente:="CAI - Consorzi Agrari d'Italia",
              idTicketAssistenza:=0, idTicketSviluppo:=166039, idTicketTesting:=0,
              autore:=DEV_Funcy,
              noteTest:="Caricare a magazzino dei sementi per due specie, esempio Vite e Ciliegio
                Verificare che nel QdC (sia menu che operazione) siano presenti entrambe le voci
                Nelle impostazioni superuser limitare visibilità alla Vite
                Nel QdC (sia menu che operazione) verificare che la voce Ciliegio non sia più visibile
                Consiglio di utilizzare specie non presenti in anagrafica impianti, per facilitare il test"
              )

            '==================================================================

            Riga_Versione("30 Aprile 2025", "137.3.0")

            Riga_Requisiti("AgronicaWebService2010",
                           "Per lettura indici Maturità e Danni Raccolta senza specie 
                           Per lettura Erbe Infestanti da WS", "14/04/2025")

            Riga_Requisiti("Migra",
                           "Aggiunge i permessi per le tab di Protocolli Terapeutici e Indicazioni Terapeutiche nel Menu Zoo NG",
                           Ver:="785")

            Riga_Requisiti("Aggancio",
                           "Aggiunte viste in lingua del modulo zootecnia", "130")

            Riga_Requisiti("Configurazione_Siti",
                           "Aggiunta chiave 'UserPwdConnectionString_toCrypt' (super server)", "150")

            Riga_Requisiti("AgronicaCoreAPI",
                           "Aggiunta possibilità di configurare la lista dei Widgets diversamente dal Preset",
                           "14/04/2025")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")

            Riga_Changelog(enum_Tipo_Changelog.Feature,
                           "Audit / Qualita e Tracciabilita",
                           "Aggiunta gestione chiamate per redirect a pagine Coop e Lab Controllo Qualita",
                           noteTest:="Da Dashboard selezionare le voci di menu Coop e Lab Controllo Qualita. Verificare che si apra la pagina corretta.",
                           cliente:="Fruttagel", idTicketAssistenza:=0, idTicketSviluppo:=172031, idTicketTesting:=0,
                           autore:=DEV_Casmatt)

            Riga_Changelog(
                enum_Tipo_Changelog.Feature,
                "Dashboard",
                "- Gestita traduzione widget 'Indicatore unico di rating' e rimosso primo punto elenco dalla sotto-sezione PLV " &
                "- modificato titolo widget 'Indice di rischio allagamento' in 'Indice di rischio pioggia forte' ",
                cliente:="Credit Agricole",
                idTicketAssistenza:=0, idTicketSviluppo:=169677, idTicketTesting:=0,
                autore:="Gianluca Amoroso"
                )

            Riga_Changelog(
                enum_Tipo_Changelog.Bug,
                area:="Impostazioni Utente (new!)",
                descrizione:="Corregge il caricamento delle categorie di magazzino per l'impostazione 183 (Giacenze APP)",
                cliente:="Inalca",
                idTicketAssistenza:=0,
                idTicketSviluppo:=0,
                idTicketTesting:=171976,
                autore:="Anny Bevilacqua"
            )

            '==================================================================

            Riga_Versione("24 Aprile 2025", "137.2.0")

            Riga_Requisiti("AgronicaWebService2010",
                           "Per lettura indici Maturità e Danni Raccolta senza specie 
                           Per lettura Erbe Infestanti da WS", "14/04/2025")

            Riga_Requisiti("Migra",
                           "Aggiunge i permessi per le tab di Protocolli Terapeutici e Indicazioni Terapeutiche nel Menu Zoo NG",
                           Ver:="785")

            Riga_Requisiti("Aggancio",
                           "Aggiunte viste in lingua del modulo zootecnia", "130")

            Riga_Requisiti("Configurazione_Siti",
                           "Aggiunta chiave 'UserPwdConnectionString_toCrypt' (super server)", "150")

            Riga_Requisiti("AgronicaCoreAPI",
                           "Aggiunta possibilità di configurare la lista dei Widgets diversamente dal Preset",
                           "14/04/2025")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")

            Riga_Changelog(
                enum_Tipo_Changelog.Bug,
                area:="Analisi del Terreno (new!)",
                descrizione:="Sistemata lettura di analisi del terreno associate a particelle con subalterno non definito, create con il vecchio modulo",
                cliente:="Regione Umbria",
                idTicketAssistenza:=172807,
                idTicketSviluppo:=0,
                idTicketTesting:=0,
                autore:=DEV_Funcy,
                noteTest:="Per i test:
                - creare/trovare una particella con subalterno non definito 
                - creare un'analisi del terreno tramite il vecchio modulo e associarla alla particella di cui sopra
                - verificare la corretta apertura dell'analisi dal modulo Analisi del Terreno (new!) 
                - salvare l'analisi dal nuovo modulo
                - verificare stesso giro partendo da creazione analisi sul modulo Analisi del Terreno (new!)",
                noteTecniche:=""
                )

            Riga_Changelog(
                enum_Tipo_Changelog.Bug,
                area:="Copia Sposta Impianti",
                descrizione:="Corretta composizione messaggio errore",
                cliente:="Orogel",
                idTicketAssistenza:=172744,
                idTicketSviluppo:=0,
                idTicketTesting:=0,
                autore:="Salvatore Zammataro",
                noteTest:="",
                noteTecniche:=""
                )

            Riga_Changelog(enum_Tipo_Changelog.Bug,
                "Anagrafiche",
                "Bloccata la cancellazione dei Centri, delle Stalle e dei Raggruppamenti se ci sono dei movimenti associati",
                cliente:="Inalca",
                idTicketAssistenza:=173104, idTicketSviluppo:=0, idTicketTesting:=0,
                autore:="Marco Rossi",
                noteTest:="Provare a cancellare un Centro una Stalla e un Raggruppamento e verificare che, se ci sono dei movimenti, si venga bloccati, mentre se sono completamente vuoti avvenga la cancellazione correttamente")

            Riga_Changelog(
                enum_Tipo_Changelog.Bug,
                "Modifica Multipla Piano Colturale",
                "Sistemato messaggio di avviso quando si cerca di cambiare il regolamento di un esercizio associato ad un Appezzamento Bio.",
                cliente:="",
                idTicketAssistenza:=0, idTicketSviluppo:=0, idTicketTesting:=171969,
                autore:=DEV_Funcy,
                noteTest:="Appezzamento con Metodo Produzione BIO > Esercizio con Regolamento BIO > Tentare di impostare il regolamento <> Bio. La modifica deve essere impedita con un messaggio coerente.
                Il ticket deve rimanere in stato HOLD fino al rilascio dello sviluppo sull'allineamento Vincolo esercizio (assegnato a casalboni) ")

            '==================================================================

            Riga_Versione("18 Aprile 2025", "137.1.0")

            Riga_Requisiti("AgronicaWebService2010",
                           "Per lettura indici Maturità e Danni Raccolta senza specie 
                           Per lettura Erbe Infestanti da WS", "14/04/2025")

            Riga_Requisiti("Migra",
                           "Aggiunge i permessi per le tab di Protocolli Terapeutici e Indicazioni Terapeutiche nel Menu Zoo NG",
                           Ver:="785")

            Riga_Requisiti("Aggancio",
                           "Aggiunte viste in lingua del modulo zootecnia", "130")

            Riga_Requisiti("Configurazione_Siti",
                           "Aggiunta chiave 'UserPwdConnectionString_toCrypt' (super server)", "150")

            Riga_Requisiti("AgronicaCoreAPI",
                           "Aggiunta possibilità di configurare la lista dei Widgets diversamente dal Preset",
                           "14/04/2025")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")

            Riga_Changelog(
                enum_Tipo_Changelog.Bug,
                area:="Anagrafica - Edit imprese",
                descrizione:="Il disciplinare di default viene ora caricato correttamente per gli ambienti configurati per utilizzare i disciplinari privati",
                cliente:="Consorzi Agrari d’Italia S.p.A / CAI Man_Ass",
                idTicketAssistenza:=172083, idTicketSviluppo:=0, idTicketTesting:=0,
                autore:="Massimo Mengarda",
                noteTest:="La lista dei disciplinare dovrebbe essere caricata correttamente e i valori dovrebbero essere ordinati per anno"
            )

            Riga_Changelog(
                enum_Tipo_Changelog.Bug,
                area:="Prelievi APP",
                descrizione:="Corretto bug che bloccava la sincronizzazione quando una matricola non veniva trovata nella stalla selezionata",
                cliente:="Inalca",
                idTicketAssistenza:=171964, idTicketSviluppo:=0, idTicketTesting:=0,
                autore:="Marco Rossi",
                noteTest:="Non testare già in produzione"
                )

            Riga_Changelog(
                enum_Tipo_Changelog.Bug,
                area:="Scrittura impianti da APP - Metodo Produzione Default",
                descrizione:="Aggiunto un valore default in mancanza del Metodo Produzione su APP.
                Viene letto il campo 'Disciplinare Default Azienda' (data anagrafica Azienda):
                - se = BIO --> Default Metodo Produzione Appezzamento 'Biologico'
                - in tutti gli altri casi --> Default Metodo Produzione Appezzamento 'Integrato'",
                cliente:="ENI RWANDA",
                idTicketAssistenza:=171929, idTicketSviluppo:=0, idTicketTesting:=0,
                autore:=DEV_Funcy,
                noteTest:="Testare creazione di un appezzamento da APP con varie combinazioni di 'Disciplinare Default Azienda' (nessuna selezione, 'Nessuno', 'Bio', disciplinare casuale) "
                )
            Riga_Changelog(
                enum_Tipo_Changelog.Bug,
                area:="Inizializzazione DLL Gdal",
                descrizione:="Corretta inizializzazione Librerie GDAL in base all'applicazione di utilizzo Web\background\etc...",
                cliente:="ENI ZIMBAWE",
                idTicketAssistenza:=0, idTicketSviluppo:=0, idTicketTesting:=0,
                autore:="Lorenzo Lavezzo",
                noteTest:="no test"
                )

            Riga_Changelog(
                enum_Tipo_Changelog.Feature,
                area:="Anagrafica Appezzamenti",
                descrizione:="Aggiunti metodi per scrittura pendenza e tessitura appezzamenti. Aggiornata lettura esercizi per grid impianti anagrafica ng, aggiunta lettura pendeza e tessitura",
                cliente:="Regione Umbria",
                idTicketAssistenza:=0,
                idTicketSviluppo:=163377,
                idTicketTesting:=0,
                autore:="Salvatore Zammataro",
                noteTest:="Testare la modifica di pendenza e tessitura di uno e più appezzamenti e tentare le operazioni con utenti con permessi diversi (sola lettura, lettura e scrittura), i permessi sono 578, 579 della tabella TB_Attivita",
                noteTecniche:=""
                )

            Riga_Changelog(
                enum_Tipo_Changelog.Bug,
                area:="Scrittura rilievi da APP con allegati",
                descrizione:="Fix per ridurre l'occupazione su db della tabella app_dati: vengono rimosse le informazioni sui documenti allegati in quanto ridondanti.",
                cliente:="ENI KENYA",
                idTicketAssistenza:=0, idTicketSviluppo:=172080, idTicketTesting:=0,
                autore:="Carlo Giovanardi",
                noteTest:="Verificare che i documenti allegati ai rilievi app arrivino correttamente su web")

            Riga_Changelog(
                 enum_Tipo_Changelog.Bug,
                 "Modifica Multipla Piano Colturale",
                 "Quando la modifica degli esercizi non andava a buon fine non compariva l'errore.",
                 cliente:="",
                 idTicketAssistenza:=0, idTicketSviluppo:=0, idTicketTesting:=171969,
                 autore:=DEV_Funcy,
                 noteTest:="")

            Riga_Changelog(
                enum_Tipo_Changelog.Bug,
                "GIS (new!)",
                "Corregge il check sui dati passati dal GIS per aprire i dati del Menu Agenda.",
                cliente:="",
                idTicketAssistenza:=0, idTicketSviluppo:=0, idTicketTesting:=166742,
                autore:="Anny Bevilacqua",
                noteTest:="Ora selezionando un poligono e cercando di aprire il tab dati per vedere le operazioni
                associate non dovrebbero esserci errori.")

            Riga_Changelog(
                enum_Tipo_Changelog.Bug,
                "Anagrafica Impianti",
                "Fix su copia/sposta Impianti",
                cliente:="",
                idTicketAssistenza:=172744, idTicketSviluppo:=0, idTicketTesting:=0,
                autore:=DEV_Drudi,
                noteTest:="")

            Riga_Changelog(
                enum_Tipo_Changelog.Feature,
                "QdCA",
                "- Mostro il messaggio di conferma inserimento se sono variati i quantitativi dei dosaggi, NPK, oppure efficienza di un Fertilizzante" &
                "rispetto alla Ricetta/Brogliaccio durante il ribaltamento in QdCA." &
                "- Mostro il messaggio di Fertilizzante mancante durante il ribaltamento in QDCA se non è stato indicato nella Ricetta/Brogliaccio proveniente da Demetra o dall'APP.",
                cliente:="Coldiretti",
                idTicketAssistenza:=0, idTicketSviluppo:=163881, idTicketTesting:=0,
                autore:=DEV_LC,
                noteTest:="")

            '==================================================================

            Riga_Versione("14 Aprile 2025", "137.0.0")

            Riga_Requisiti("AgronicaWebService2010",
                           "Per lettura indici Maturità e Danni Raccolta senza specie 
                           Per lettura Erbe Infestanti da WS", "14/04/2025")

            Riga_Requisiti("Migra",
                           "Aggiunge i permessi per le tab di Protocolli Terapeutici e Indicazioni Terapeutiche nel Menu Zoo NG",
                           Ver:="785")

            Riga_Requisiti("Aggancio",
                           "Aggiunte viste in lingua del modulo zootecnia", "130")

            Riga_Requisiti("Configurazione_Siti",
                           "Aggiunta chiave 'UserPwdConnectionString_toCrypt' (super server)", "150")

            Riga_Requisiti("AgronicaCoreAPI",
                           "Aggiunta possibilità di configurare la lista dei Widgets diversamente dal Preset",
                           "14/04/2025")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")

            Riga_Changelog(
                enum_Tipo_Changelog.Performance,
                "Report Orogel",
                "Create funzioni per il caricamento massivo di dati in funzione dell'ottimizzazione dei report Orogel; sono chiamate dalla soluzione delle stampe.",
                cliente:="Orogel",
                idTicketAssistenza:=0, idTicketSviluppo:=166946, idTicketTesting:=0,
                autore:="Tommaso Turci"
                )

            Riga_Changelog(
                enum_Tipo_Changelog.Feature,
                "Widget GHG",
                "Gestione calcolo GHG ponderato in base alla tabella Imprese_Parametri_GHG",
                cliente:="Banca Cambiano",
                idTicketAssistenza:=0, idTicketSviluppo:=167955, idTicketTesting:=0,
                autore:="Marco Lucchi",
                noteTest:="Provare ad inserire valori utilizzando la pagina, rimane cmq un test difficile da verificare",
                noteTecniche:="Se non vengono rilevate rese di impianto scatta il calcolo ghg utilizzando la tabella Imprese_Parametri_GHG"
                )

            Riga_Changelog(
                enum_Tipo_Changelog.Bug,
                "Chiamate CoreWS Zoo",
                " - Fix delle chiamate su CoreWS verso tabelle Zoo di MetaSchema che non filtravano correttamente; 
                 - Cambio posizione DAL di tabelle MetaSchema Zoo da CoreZoo a CoreMetaSchema. ",
                cliente:="Inalca",
                idTicketAssistenza:=0, idTicketSviluppo:=168561, idTicketTesting:=0,
                autore:="Lorenzo Di Varano",
                noteTest:="Verificare che le dropdown delle tabelle coinvolte vengano caricate correttamente: 
                    Generi, Specie, Razze, Categorie, Indirizzi Produttivi, Patologie, Anomalie, Causali Morte, Attributi Stalla, Tipi Fabbricati, Tipi Stalla."
                )

            Riga_Changelog(
                enum_Tipo_Changelog.Feature,
                "Widgets",
                "Aggiunta la possibilità di configurare la lista dei Widgets diversamente dal preset",
                cliente:="ENI Kenya",
                idTicketAssistenza:=0, idTicketSviluppo:=165875, idTicketTesting:=0,
                autore:="Dario Cabras"
                )

            Riga_Changelog(
                enum_Tipo_Changelog.Bug,
                "Importazioni GiasAPP valori Default",
                "Cambio default GiasAPP invio visite",
                cliente:="Fondazione Agritech/MUR",
                idTicketAssistenza:=0, idTicketSviluppo:=169584, idTicketTesting:=0,
                autore:="Mattia Casalboni",
                noteTest:="Da APP testare l'invio di visite con rilievo e controllare che vengano inviate direttamente a web 
                senza il bisogno di dover premere sul pulsante Carica Da APP dal menu web visite",
                noteTecniche:="L'impostazione 860 dell'ambiente di test deve avere VisiteRilievi: true, Importazioni: null"
                )

            Riga_Changelog(
                enum_Tipo_Changelog.Bug,
                "SQL Injection",
                "Aggiunto Agro_SQL_SaveText in ConfigurazioneBase2.EsisteTabellaQry",
                cliente:="Tutti",
                idTicketAssistenza:=0, idTicketSviluppo:=0, idTicketTesting:=0,
                autore:="Lorenzo Casanova",
                noteTest:="Nessun test necessario")

            Riga_Changelog(
                enum_Tipo_Changelog.Feature,
                "Modifica Multipla Piano Colturale",
                "Aggiunti nuovi endpoint -- guardare versione Agenda",
                cliente:="",
                idTicketAssistenza:=0, idTicketSviluppo:=163253, idTicketTesting:=0,
                autore:=DEV_Funcy,
                noteTest:="")

            Riga_Changelog(
                enum_Tipo_Changelog.Feature,
                "Gestione Visite con Rilievo su SincroDatiApp",
                "Gestita l'elaborazione delle visite con rilievo per il nuovo menu visite",
                cliente:="OROGEL",
                idTicketAssistenza:=0, idTicketSviluppo:=169588, idTicketTesting:=0,
                autore:="Mattia Casalboni",
                noteTest:="")

            Riga_Changelog(
                enum_Tipo_Changelog.Feature,
                "Rilievi Indici Coltura e Rilievi Danni",
                "Gestito il salvataggio dei Rilievi Indici Coltura e Rilievi Danni senza specie",
                cliente:="OROGEL",
                idTicketAssistenza:=0, idTicketSviluppo:=166946, idTicketTesting:=0,
                autore:="Lorenzo Casanova",
                noteTest:="")

            Riga_Changelog(
                enum_Tipo_Changelog.Feature,
                "Report Rilievi e Menu Visite (new!)",
                "Gestisce la presenza di rilievi non collegati a una coltura specifica. Mostra la voce 'Nessuna Specie' nei filtri laterali e in quelli su colonna'",
                cliente:="OROGEL",
                idTicketAssistenza:=0, idTicketSviluppo:=166946, idTicketTesting:=0,
                autore:="Dario Cabras",
                noteTest:="")

            '==================================================================

            Riga_Versione("04 Aprile 2025", "136.3.0")

            Riga_Requisiti("AgronicaWebService2010",
                           "Per calcolo tessitura da anagrafica appezzamento", "17/02/2025")

            Riga_Requisiti("Migra",
                           "Aggiunte tabelle in lingua del modulo zootecnia",
                           Ver:="781")

            Riga_Requisiti("Aggancio",
                           "Aggiunte viste in lingua del modulo zootecnia", "130")

            Riga_Requisiti("Configurazione_Siti",
                           "Aggiunta chiave 'UserPwdConnectionString_toCrypt' (super server)", "150")

            Riga_Requisiti("AgronicaCoreAPI",
                           "EndPoint per Widget Previsioni AI",
                           "17/02/2025")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")

            Riga_Changelog(
                enum_Tipo_Changelog.Bug,
                area:="Widget Meteo",
                descrizione:="Aggiornata lettura LatLng per widget meteo, aggiunta geolocalizzazione impresa in base all'indirizzo",
                cliente:="ERSAF",
                idTicketAssistenza:=168312, idTicketSviluppo:=0, idTicketTesting:=0,
                autore:="Zammataro Salvatore"
                )

            Riga_Changelog(
                enum_Tipo_Changelog.Bug,
                area:="Lettura impianti su APP",
                descrizione:="Fix a query per lettura impianti dove veniva troncato il dato cartografico se sorpassava i 2000 caratteri",
                cliente:="ENI RWANDA",
                idTicketAssistenza:=170104, idTicketSviluppo:=0, idTicketTesting:=0,
                autore:="Mattia Casalboni",
                noteTest:="Testare preferibilmente du DB ENI la sincronizzazione globale, soprattutto dove abbiamo poligoni di grandi dimensioni,
                provare anche a creare poligoni di grandi dimensioni e verificare che vengano inviati a web e sincronizzati sull'app correttamente"
                )

            '==================================================================

            Riga_Versione("01 Aprile 2025", "136.2.1")

            Riga_Requisiti("AgronicaWebService2010",
                           "Per calcolo tessitura da anagrafica appezzamento", "17/02/2025")

            Riga_Requisiti("Migra",
                           "Aggiunge i permessi per le tab di Protocolli Terapeutici e Indicazioni Terapeutiche nel Menu Zoo NG",
                           Ver:="785")

            Riga_Requisiti("Aggancio",
                           "Aggiunte viste in lingua del modulo zootecnia", "130")

            Riga_Requisiti("Configurazione_Siti",
                           "Aggiunta chiave 'UserPwdConnectionString_toCrypt' (super server)", "150")

            Riga_Requisiti("AgronicaCoreAPI",
                           "EndPoint per Widget Previsioni AI",
                           "17/02/2025")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")

            Riga_Changelog(
                enum_Tipo_Changelog.Feature,
                "Indicatori Dashboard",
                "Aggiornato metodo di lettura per gli indicatori Meteo",
                cliente:="ERSAF - Lombardia",
                idTicketAssistenza:=0, idTicketSviluppo:=168279, idTicketTesting:=0,
                autore:="Mattia Casalboni",
                noteTest:="Controllare che gli indicatori attivi vengano mostrati correttamente sulla dashboard",
                noteTecniche:=""
                )

            '==================================================================

            Riga_Versione("28 Marzo 2025", "136.2.0")

            Riga_Requisiti("AgronicaWebService2010",
                           "Per calcolo tessitura da anagrafica appezzamento", "17/02/2025")

            Riga_Requisiti("Migra",
                           "Aggiunge i permessi per le tab di Protocolli Terapeutici e Indicazioni Terapeutiche nel Menu Zoo NG",
                           Ver:="785")

            Riga_Requisiti("Aggancio",
                           "Aggiunte viste in lingua del modulo zootecnia", "130")

            Riga_Requisiti("Configurazione_Siti",
                           "Aggiunta chiave 'UserPwdConnectionString_toCrypt' (super server)", "150")

            Riga_Requisiti("AgronicaCoreAPI",
                           "EndPoint per Widget Previsioni AI",
                           "17/02/2025")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")

            Riga_Changelog(
                enum_Tipo_Changelog.Feature,
                "Dashboard",
                "- Modificata terza descrizione nel widget 'Indicatore unico di rating' " &
                "- Modificato titolo widget 'Produttività [q/Ha]' in 'Produttività [%]' " &
                "- Modificato titolo widget 'Indice di CO2' in 'Indice SOC [g/kg]' ",
                cliente:="Credit Agricole",
                idTicketAssistenza:=0, idTicketSviluppo:=169677, idTicketTesting:=0,
                autore:="Gianluca Amoroso"
                )

            Riga_Changelog(
                enum_Tipo_Changelog.Security,
                "Ricerca Doc Contabili",
                "Corretto errore in caso la ricerca dei documenti venga lanciata senza un parametro di default",
                cliente:="Coldiretti",
                idTicketAssistenza:=0, idTicketSviluppo:=163182, idTicketTesting:=0,
                autore:="Gianluca Amoroso", "TFS 8380", "Verificare che la ricerca dei documenti contabili continui a funzionare normalmente"
                )

            '==================================================================

            Riga_Versione("27 Marzo 2025", "136.1.3")

            Riga_Requisiti("AgronicaWebService2010",
                           "Per calcolo tessitura da anagrafica appezzamento", "17/02/2025")

            Riga_Requisiti("Migra",
                           "Aggiunge i permessi per le tab di Protocolli Terapeutici e Indicazioni Terapeutiche nel Menu Zoo NG",
                           Ver:="785")

            Riga_Requisiti("Aggancio",
                           "Aggiunte viste in lingua del modulo zootecnia", "130")

            Riga_Requisiti("Configurazione_Siti",
                           "Aggiunta chiave 'UserPwdConnectionString_toCrypt' (super server)", "150")

            Riga_Requisiti("AgronicaCoreAPI",
                           "EndPoint per Widget Previsioni AI",
                           "17/02/2025")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")

            Riga_Changelog(
                enum_Tipo_Changelog.Bug,
                "Cancellazione Brogliaccio da Menu Agenda NG",
                "Sistemato errore che impediva la cancellazione dei brogliacci",
                cliente:="Aboca",
                idTicketAssistenza:=168387, idTicketSviluppo:=0, idTicketTesting:=0,
                autore:=DEV_Funcy
                )

            '==================================================================

            Riga_Versione("26 Marzo 2025", "136.1.2")

            Riga_Requisiti("AgronicaWebService2010",
                           "Per calcolo tessitura da anagrafica appezzamento", "17/02/2025")

            Riga_Requisiti("Migra",
                           "Aggiunge i permessi per le tab di Protocolli Terapeutici e Indicazioni Terapeutiche nel Menu Zoo NG",
                           Ver:="785")

            Riga_Requisiti("Aggancio",
                           "Aggiunte viste in lingua del modulo zootecnia", "130")

            Riga_Requisiti("Configurazione_Siti",
                           "Aggiunta chiave 'UserPwdConnectionString_toCrypt' (super server)", "150")

            Riga_Requisiti("AgronicaCoreAPI",
                           "EndPoint per Widget Previsioni AI",
                           "17/02/2025")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")

            Riga_Changelog(
                enum_Tipo_Changelog.Bug,
                "Impostazioni superuser",
                "Corregge la funzione di lettura delle categorie magazzino per le impostazioni di filtro giacenze (180)
                e filtro lotti (181).",
                cliente:="Eni Kenia, Tutti",
                idTicketAssistenza:=168677, idTicketSviluppo:=0, idTicketTesting:=0,
                autore:="Anny Bevilacqua",
                noteTest:="Il problema capitava quando la lingua del superuser era impostata su inglese. Di fatto andava
                in errore una funzione e non venivano caricate le categorie magazzino nelle griglie per le impostazioni
                sopra citate.",
                noteTecniche:=""
            )

            Riga_Changelog(
                enum_Tipo_Changelog.Bug,
                "GIS Ng",
                "Aggiornati metodi per scrittura dei tema attivo e etichetta per layer: corretto controllo permessi",
                cliente:="ENI Costa d'Avorio",
                idTicketAssistenza:=168499, idTicketSviluppo:=0, idTicketTesting:=0,
                autore:="Zammataro Salvatore",
                noteTest:="Provare a modificare il tema attivo e etichetta per attributi layer, testare con utenti normali e superuser"
                )

            Riga_Changelog(
                enum_Tipo_Changelog.Bug,
                "GIS Ng",
                "Fix query estrazione entita per visualizzazione totale",
                cliente:="ENI Kenya",
                idTicketAssistenza:=169014, idTicketSviluppo:=0, idTicketTesting:=0,
                autore:="Lorenzo Lavezzo",
                noteTest:="Con l'utente francesco.polinelli , attivare il layer catasto (se non attivo) ed aggiornare la visualizzazione in modalità 'Visualizzaizone Totale'. Non deve dare errore"
                )

            Riga_Changelog(
                enum_Tipo_Changelog.Bug,
                "Ribaltamento da Ricetta/Brogliaccio in QdCA",
                "Sistemato errore alla conferma del Prodotto se è stata selezionata una Dose ad Etichetta con unità di misura diversa rispetto a quella nella Ricetta/Brogliaccio.",
                cliente:="Coldiretti",
                idTicketAssistenza:=0, idTicketSviluppo:=0, idTicketTesting:=169010,
                autore:="Lorenzo Casanova"
                )

            '==================================================================

            Riga_Versione("21 Marzo 2025", "136.1.0")

            Riga_Requisiti("AgronicaWebService2010",
                           "Per calcolo tessitura da anagrafica appezzamento", "17/02/2025")

            Riga_Requisiti("Migra",
                           "Aggiunge i permessi per le tab di Protocolli Terapeutici e Indicazioni Terapeutiche nel Menu Zoo NG",
                           Ver:="785")

            Riga_Requisiti("Aggancio",
                           "Aggiunte viste in lingua del modulo zootecnia", "130")

            Riga_Requisiti("Configurazione_Siti",
                           "Aggiunta chiave 'UserPwdConnectionString_toCrypt' (super server)", "150")

            Riga_Requisiti("AgronicaCoreAPI",
                           "EndPoint per Widget Previsioni AI",
                           "17/02/2025")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")

            Riga_Changelog(enum_Tipo_Changelog.Bug,
                           "Export QdC to Agea",
                           "Corretto formato dati in sezione 'Warehouses': corretto formato a 3 decimali per campo 'capacity'",
                           cliente:="Coldiretti",
                           idTicketAssistenza:=0, idTicketSviluppo:=163183, idTicketTesting:=0,
                           autore:="Andrea Novaga",
                           noteTest:="Nessun test - ancora WIP"
                )

            Riga_Changelog(
                enum_Tipo_Changelog.Feature,
                "Inizializzazione GIS per utente - p2",
                "Inserito ribaltamento Tiles per tematizzazione Layer in creazione utente",
                cliente:="Credit Agricole",
                idTicketAssistenza:=0, idTicketSviluppo:=165875, idTicketTesting:=0,
                autore:="Lorenzo Lavezzo",
                noteTest:="",
                noteTecniche:="Creare un nuovo utente che abbia i permessi gis, al primo accesso per i layer di analisi produttività deve essere presente l'icona della tematizzazione"
                )

            Riga_Changelog(
                enum_Tipo_Changelog.Bug,
                "Permessi installazione",
                "Corregge la funzione di salvataggio dei permessi di installazione",
                cliente:="Tutti",
                idTicketAssistenza:=0, idTicketSviluppo:=0, idTicketTesting:=166286,
                autore:="Anny Bevilacqua",
                noteTest:="- Evita di salvare modifiche sui permessi di accesso al menu principale e su quello di
                accesso alla pagina utenti e permessi NG.
                - Disabilitando e riabilitando tutti i permessi non comparirà più l'errore dovuto alla presenza di
                permessi con codice duplicato.",
                noteTecniche:=""
            )

            Riga_Changelog(
                enum_Tipo_Changelog.Bug,
                "Analisi del Terreno (new)",
                "Sistemata scrittura Agronica_Log_Analisi",
                cliente:="Tutti",
                idTicketAssistenza:=0, idTicketSviluppo:=0, idTicketTesting:=0,
                autore:=DEV_Funcy,
                noteTest:="Non testabile da assistenza",
                noteTecniche:=""
            )

            '==================================================================

            Riga_Versione("19 Marzo 2025", "136.0.1")

            Riga_Requisiti("AgronicaWebService2010",
                           "Per calcolo tessitura da anagrafica appezzamento", "17/02/2025")

            Riga_Requisiti("Migra",
                           "Aggiunge i permessi per le tab di Protocolli Terapeutici e Indicazioni Terapeutiche nel Menu Zoo NG",
                           Ver:="785")

            Riga_Requisiti("Aggancio",
                           "Aggiunte viste in lingua del modulo zootecnia", "130")

            Riga_Requisiti("Configurazione_Siti",
                           "Aggiunta chiave 'UserPwdConnectionString_toCrypt' (super server)", "150")

            Riga_Requisiti("AgronicaCoreAPI",
                           "EndPoint per Widget Previsioni AI",
                           "17/02/2025")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")

            Riga_Changelog(
                enum_Tipo_Changelog.Bug,
                "Widget Indici Produttività",
                "Eliminato moltiplicatore per 10 da tutti gli indici meteo per la visualizzazioe grafici",
                cliente:="Credit Agricole",
                idTicketAssistenza:=168098, idTicketSviluppo:=0, idTicketTesting:=0,
                autore:="Lorenzo Lavezzo",
                noteTest:="",
                noteTecniche:=""
                )

            Riga_Changelog(
                enum_Tipo_Changelog.Bug,
                "Sincronizzazione impianti APP",
                "Risolto problema di sincronizzazione impianti per APP",
                cliente:="Orogel",
                idTicketAssistenza:=168034, idTicketSviluppo:=0, idTicketTesting:=0,
                autore:="Mattia Casalboni",
                noteTest:="Su APP provare a sincronizzare diverse aziende e controllare che vengano sincronizzati gli impianti correttamente controllando anche la presenza sul piano colturale, effettuare il test su diversi ambienti"
                )

            Riga_Changelog(
                enum_Tipo_Changelog.Bug,
                "Specie visite APP",
                "Risolta sincronizzazione impianti per APP, risolve anche la visualizzazione dello switch specie impianti/tutte",
                cliente:="Orogel",
                idTicketAssistenza:=168036, idTicketSviluppo:=0, idTicketTesting:=0,
                autore:="Mattia Casalboni",
                noteTest:="Su APP, in creazione delle visite controllare che lo switch specie impianti/tutte sia visibile, sincronizzare la visita a web e verificarne il corretto funzionamento"
                )

            Riga_Changelog(
                enum_Tipo_Changelog.Bug,
                "GIS - Visualizzazione esiti elaborazione checklist ",
                "Corretta query estrazione esiti elaborazione checklist gis (ISCC) che considerava erroneamente elaborazioni di altre aziende",
                cliente:="ENI",
                idTicketAssistenza:=168242, idTicketSviluppo:=0, idTicketTesting:=0,
                autore:="Lorenzo Lavezzo",
                noteTest:="Test eseguibile in produzione, per le aziende ID121190025 e ID121130041 l'elaborazione del algoritmo GDAL non deve essere negativo ma positivo (perchè i check su tutti gli impianti sono positivi)"
                )

            Riga_Changelog(
                enum_Tipo_Changelog.Bug,
                "Lista Razze Animali",
                "Corretto bug che non caricava correttamente la lista delle razze (metteva un filtro fisso su Raz_Cod = 0)",
                "Inalca",
                idTicketAssistenza:=168422,
                idTicketSviluppo:=0,
                idTicketTesting:=0,
                autore:="Lorenzo Di Varano",
                noteTest:="Testare caricamento Dropdown razze in Anagrafica del Capo"
                )

            Riga_Changelog(
                enum_Tipo_Changelog.Bug,
                "Elaborazione Notifiche GEE",
                "Gestita casistica di loop-retry per le notifiche relative a poligoni non più presenti in GIAS che causavano il blocco delle Sync",
                cliente:="Consorzi Agrari Italia",
                idTicketAssistenza:=168098, idTicketSviluppo:=0, idTicketTesting:=0,
                autore:="Lorenzo Lavezzo",
                noteTest:="",
                noteTecniche:="Se viene cancellato un poligono che è oggetto di sync con GEE per le immagini satellitari, le notifiche di quel poligono non devono bloccare la sync ma essere forzate a OK per non interrompere la coda di elaborazione"
                )

            Riga_Changelog(
                enum_Tipo_Changelog.Feature,
                "QdCA",
                "Mostro il messaggio di conferma inserimento se sono variati i quantitativi dei dosaggi di un Formulato rispetto alla Ricetta/Brogliaccio durante il ribaltamento in QdCA",
                cliente:="Coldiretti",
                idTicketAssistenza:=0, idTicketSviluppo:=163881, idTicketTesting:=0,
                autore:="Lorenzo Casanova"
                )

            '==================================================================

            Riga_Versione("17 Marzo 2025", "136.0.0")

            Riga_Requisiti("AgronicaWebService2010",
                           "Per calcolo tessitura da anagrafica appezzamento", "17/02/2025")

            Riga_Requisiti("Migra",
                           "Aggiunge i permessi per le tab di Protocolli Terapeutici e Indicazioni Terapeutiche nel Menu Zoo NG",
                           Ver:="785")

            Riga_Requisiti("Aggancio",
                           "Aggiunte viste in lingua del modulo zootecnia", "130")

            Riga_Requisiti("Configurazione_Siti",
                           "Aggiunta chiave 'UserPwdConnectionString_toCrypt' (super server)", "150")

            Riga_Requisiti("AgronicaCoreAPI",
                           "EndPoint per Widget Previsioni AI",
                           "17/02/2025")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")

            Riga_Changelog(enum_Tipo_Changelog.Feature,
                           "Rilievo Indici Coltura",
                           "Ordinati i valori degli Indici in modo alfabetico",
                           cliente:="",
                           idTicketAssistenza:=0, idTicketSviluppo:=0, idTicketTesting:=0,
                           autore:="Lorenzo Casanova",
                           noteTest:="Non testabile")

            Riga_Changelog(
                enum_Tipo_Changelog.Feature,
                "Inizializzazione GIS per utente",
                "Inserito ribaltamento Tiles per tematizzazione Layer",
                cliente:="Credit Agricole",
                idTicketAssistenza:=0, idTicketSviluppo:=165875, idTicketTesting:=0,
                autore:="Lorenzo Lavezzo",
                noteTest:="",
                noteTecniche:="Al primo accesso al gis dell'utente, o all'abilitazione di un nuovo Layer preesistente. Verificare che compaia l'icona per la tematizzazione dello stesso."
                )

            Riga_Changelog(
                enum_Tipo_Changelog.Bug,
                "Filtro visibilità imprese area",
                "Per gli utenti con visibilità globale, ora vengono correttamente restituite tutte le imprese presenti senza filtri",
                cliente:="Regione Umbria",
                idTicketAssistenza:=166712, idTicketSviluppo:=0, idTicketTesting:=0,
                autore:=DEV_Casa,
                noteTest:="",
                noteTecniche:=""
                )

            Riga_Changelog(
                enum_Tipo_Changelog.Feature,
                "Menu Zoo NG",
                "Aggiunge i permessi per le tab di Protocolli Terapeutici e Indicazioni Terapeutiche nel Menu Zoo NG
                all'enumerativato enum_Security_Attivita",
                cliente:="Inalca",
                idTicketAssistenza:=0, idTicketSviluppo:=162708, idTicketTesting:=0,
                autore:="Anny Bevilacqua",
                noteTest:="",
                noteTecniche:=""
                )


            Riga_Changelog(
                enum_Tipo_Changelog.Bug,
                "Analisi del Terreno (new!)",
                "Griglia appezzamenti: nella colonna 'Utilizzo' verranno elencati tutti gli utilizzi degli impianti associati, ordinati per validità.
                NB: questa colonna NON è soggetta ai filtri di validità, quindi verranno elencati tutti gli utilizzi presenti.",
                cliente:="Coldiretti",
                idTicketAssistenza:=0, idTicketSviluppo:=0, idTicketTesting:=164947,
                autore:=DEV_Funcy,
                noteTest:="",
                noteTecniche:=""
                )

            Riga_Changelog(
                enum_Tipo_Changelog.Bug,
                "Menu Agenda NG",
                "Modifica la lettura del riferimento agli impianti selezionati quando si accede al Menu Agenda dal GIS
                tramite la selezione di uno o piu impianti",
                cliente:="",
                idTicketAssistenza:=0, idTicketSviluppo:=0, idTicketTesting:=166742,
                autore:="Anny Bevilacqua",
                noteTest:=""
            )

            '==================================================================

            Riga_Versione("11 Marzo 2025", "135.3.2")

            Riga_Requisiti("AgronicaWebService2010",
                           "Per calcolo tessitura da anagrafica appezzamento", "17/02/2025")

            Riga_Requisiti("Migra",
                           "Aggiunte tabelle in lingua del modulo zootecnia",
                           Ver:="781")

            Riga_Requisiti("Aggancio",
                           "Aggiunte viste in lingua del modulo zootecnia", "130")

            Riga_Requisiti("Configurazione_Siti",
                           "Aggiunta chiave 'UserPwdConnectionString_toCrypt' (super server)", "150")

            Riga_Requisiti("AgronicaCoreAPI",
                           "EndPoint per Widget Previsioni AI",
                           "17/02/2025")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")

            Riga_Changelog(
                enum_Tipo_Changelog.Feature,
                "Esportazione Registro Trattamenti Agea",
                "Modificata informazione per gli stock di magazzino: un solo elemento per ogni prodotto in magazzino (sia fitofarmaci che fertilizzanti), con giacenza a data inizio e data fine.",
                cliente:="Coldiretti",
                idTicketAssistenza:=0, idTicketSviluppo:=163183, idTicketTesting:=0,
                autore:="Andrea Novaga",
                noteTest:="Nessun test - ancora WIP"
                )

            '===================================================================

            Riga_Versione("10 Marzo 2025", "135.3.1")

            Riga_Requisiti("AgronicaWebService2010",
                           "Per calcolo tessitura da anagrafica appezzamento", "17/02/2025")

            Riga_Requisiti("Migra",
                           "Aggiunte tabelle in lingua del modulo zootecnia",
                           Ver:="781")

            Riga_Requisiti("Aggancio",
                           "Aggiunte viste in lingua del modulo zootecnia", "130")

            Riga_Requisiti("Configurazione_Siti",
                           "Aggiunta chiave 'UserPwdConnectionString_toCrypt' (super server)", "150")

            Riga_Requisiti("AgronicaCoreAPI",
                           "EndPoint per Widget Previsioni AI",
                           "17/02/2025")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")

            Riga_Changelog(
                enum_Tipo_Changelog.Bug,
                "Anagrafica Esercizi",
                "Aggiornati metodi per lettura e scrittura dei contributi ACA associati all'esercizio: corretto controllo permessi",
                cliente:="Regione Umbria",
                idTicketAssistenza:=0, idTicketSviluppo:=163377, idTicketTesting:=0,
                autore:="Zammataro Salvatore",
                noteTest:="Provare a salvare e modificare i contributi ACA dalla pagina di edit completo degli esercizi. Ripetere i test per utenti con tutte le combinazioni di permessi lettura/modifica per ContributiACA e Impianti (es: Modifica Impianto: sì & Modifica Contributi ACA: no)"
                )

            '===================================================================

            Riga_Versione("07 Marzo 2025", "135.3.0")

            Riga_Requisiti("AgronicaWebService2010",
                           "Per calcolo tessitura da anagrafica appezzamento", "17/02/2025")

            Riga_Requisiti("Migra",
                           "Aggiunte tabelle in lingua del modulo zootecnia",
                           Ver:="781")

            Riga_Requisiti("Aggancio",
                           "Aggiunte viste in lingua del modulo zootecnia", "130")

            Riga_Requisiti("Configurazione_Siti",
                           "Aggiunta chiave 'UserPwdConnectionString_toCrypt' (super server)", "150")

            Riga_Requisiti("AgronicaCoreAPI",
                           "EndPoint per Widget Previsioni AI",
                           "17/02/2025")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")

            Riga_Changelog(enum_Tipo_Changelog.Feature,
                           "Lettura N_Distribuito (NewAgri)",
                           "Estrazione N_Distribuito da concimazioni",
                           cliente:="Regione Umbria",
                           idTicketAssistenza:=0, idTicketSviluppo:=163385, idTicketTesting:=0,
                           autore:=DEV_Funcy,
                           noteTest:="Non testabile")

            Riga_Changelog(enum_Tipo_Changelog.Feature,
                           "Anagrafica NG",
                           "Migliorati messaggi di errore per cancellazione/modifica date entità.",
                           cliente:="",
                           idTicketAssistenza:=0, idTicketSviluppo:=0, idTicketTesting:=164130,
                           autore:=DEV_Funcy)

            Riga_Changelog(
                enum_Tipo_Changelog.Bug,
                "Widget Indici Produttività",
                "Indice Meteo Aggregato non più moltiplicato per 10",
                cliente:="Credit Agricole",
                idTicketAssistenza:=0, idTicketSviluppo:=0, idTicketTesting:=0,
                autore:="Lorenzo Lavezzo",
                noteTest:="",
                noteTecniche:=""
                )

            Riga_Changelog(
                enum_Tipo_Changelog.Bug,
                "Lettura Impianti APP",
                "Corretta condizione per lettura poligoni degli impianti",
                cliente:="ENI MOZAMBICO",
                idTicketAssistenza:=0, idTicketSviluppo:=0, idTicketTesting:=0,
                autore:="Mattia Casalboni",
                noteTest:="Su APP sincronizzare una o più aziende e controllare che i poligoni dei rispettivi impianti siano visibili con la ricerca locale",
                noteTecniche:=""
                )

            '===================================================================

            Riga_Versione("04 Marzo 2025", "135.2.1")

            Riga_Requisiti("AgronicaWebService2010",
                           "Per calcolo tessitura da anagrafica appezzamento", "17/02/2025")

            Riga_Requisiti("Migra",
                           "Aggiunte tabelle in lingua del modulo zootecnia",
                           Ver:="781")

            Riga_Requisiti("Aggancio",
                           "Aggiunte viste in lingua del modulo zootecnia", "130")

            Riga_Requisiti("Configurazione_Siti",
                           "Aggiunta chiave 'UserPwdConnectionString_toCrypt' (super server)", "150")

            Riga_Requisiti("AgronicaCoreAPI",
                           "EndPoint per Widget Previsioni AI",
                           "17/02/2025")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")

            Riga_Changelog(
                enum_Tipo_Changelog.Feature,
                "Anagrafica Esercizi",
                "Aggiornati metodi per lettura e scrittura dei contributi ACA associati all'esercizio",
                cliente:="Regione Umbria",
                idTicketAssistenza:=0, idTicketSviluppo:=163377, idTicketTesting:=0,
                autore:="Zammataro Salvatore",
                noteTest:="provare a salvare e modificare i contributi ACA dalla finestra Associazione Esercizi - Contributi"
                )

            Riga_Changelog(
                enum_Tipo_Changelog.Bug,
                "Spostamento animali da origine diversa",
                "Corretta gestione spostamento capi da app, ora quando un capo viene spostato da un box di origine diverso non si blocca ma sposta comunque l'animale dal box in cui vine trovato",
                cliente:="Inalca",
                idTicketAssistenza:=165209, idTicketSviluppo:=0, idTicketTesting:=0,
                autore:="Rossi Marco",
                noteTest:="Provare a spostare degli animali dall'app testando sia casi in cui l'animale si trova effettivamente nel box di partenza segnalato dall'app, sia casi in cui il box di partenza è diverso"
            )


            Riga_Changelog(
                enum_Tipo_Changelog.Bug,
                "Anagrafica Esercizi",
                "Corretta scrittura contributi aca",
                cliente:="Regione Umbria",
                idTicketAssistenza:=0, idTicketSviluppo:=0, idTicketTesting:=0,
                autore:="Zammataro Salvatore",
                noteTest:="",
                noteTecniche:="Gestito caso in cui la lista acaContributes sia Nothing"
                )

            Riga_Changelog(
                enum_Tipo_Changelog.Feature,
                "Esportazione QDCA verso Agea",
                "Aggiunti nuovi controlli in fase di esportazione verso AGEA (Id Attività TFS: 9124)",
                cliente:="Coldiretti",
                idTicketAssistenza:=0, idTicketSviluppo:=163183, idTicketTesting:=0,
                autore:="Lorenzo Casanova",
                noteTest:="",
                noteTecniche:=""
                )

            Riga_Changelog(
                enum_Tipo_Changelog.Feature,
                "Inizializzazione GIS per utente",
                "Inserito ribaltamento Tiles per tematizzazione Layer",
                cliente:="Credit Agricole",
                idTicketAssistenza:=0, idTicketSviluppo:=165875, idTicketTesting:=0,
                autore:="Lorenzo Lavezzo",
                noteTest:="",
                noteTecniche:="Al primo accesso al gis dell'utente, o all'abilitazione di un nuovo Layer preesistente. Verificare che compaia l'icona per la tematizzazione dello stesso."
                )

            '===================================================================

            Riga_Versione("28 Febbraio 2025", "135.2.0")

            Riga_Requisiti("AgronicaWebService2010",
                           "Per calcolo tessitura da anagrafica appezzamento", "17/02/2025")

            Riga_Requisiti("Migra",
                           "Aggiunte tabelle in lingua del modulo zootecnia",
                           Ver:="781")

            Riga_Requisiti("Aggancio",
                           "Aggiunte viste in lingua del modulo zootecnia", "130")

            Riga_Requisiti("Configurazione_Siti",
                           "Aggiunta chiave 'UserPwdConnectionString_toCrypt' (super server)", "150")

            Riga_Requisiti("AgronicaCoreAPI",
                           "EndPoint per Widget Previsioni AI",
                           "17/02/2025")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")

            Riga_Changelog(
                enum_Tipo_Changelog.Bug,
                "Profilazione NG",
                "Corretta lettura delle aziende visualizzabili da un utente",
                cliente:="Inalca",
                idTicketAssistenza:=164573, idTicketSviluppo:=0, idTicketTesting:=0,
                autore:="Anny Bevilacqua",
                noteTest:="Dal menu utenti provare a selezionare un utente a cambiare visibilitaà. Controllare che dopo
                il cambio tutte le nuove aziende visibili siano mostrate correttamente."
            )

            Riga_Changelog(
                enum_Tipo_Changelog.Bug,
                "Esercizi NG",
                "Accelerata lettura parametri Replica Gias inlettura esercizi",
                cliente:="Aboca",
                idTicketAssistenza:=165483, idTicketSviluppo:=0, idTicketTesting:=0,
                autore:="Salvatore Zammataro",
                noteTest:="Testare i tempi di esecuzione della lettura con un gran numero di esercizi creati su diversi centri"
            )

            Riga_Changelog(
                enum_Tipo_Changelog.Bug,
                "GIS NG",
                "Corretta lettura GIS che estraeva erronemanete particelle con possesso non più valido alla data di lettura",
                cliente:="Aboca",
                idTicketAssistenza:=164242, idTicketSviluppo:=0, idTicketTesting:=0,
                autore:="Lorenzo Lavezzo",
                noteTest:="Data una particella castatale presente nel GIS con possesso valido alla data, cambiare la data di fine validità del possesso in essere affinchè quest'ultimo non sia più valido alla data. Rileggendo le particelle nel GIS non deve più comparire il poligono della particella"
            )


            Riga_Changelog(
                enum_Tipo_Changelog.Bug,
                "Cancellazione catasto e poligoni associati",
                "Quando si cancella una particella catastale, viene rimosso anche il poligono associato sul GIS (se esiste)",
                cliente:="Aboca",
                idTicketAssistenza:=164312, idTicketSviluppo:=0, idTicketTesting:=0,
                autore:=DEV_Funcy,
                noteTest:="Un esempio di particella con poligono associato è su Aboca Test Interni (0_20_96). NB. Per poterla cancellare bisogna rimuovere l'associazione agli impianti. 
                Dalla griglia Catasto > Particella > Pulsante 3 punti > Impianti > Vengono mostrati tutti gli impianti associati")

            Riga_Changelog(
                enum_Tipo_Changelog.Bug,
                "Report Rilievi",
                "Gestione del filtro sull'azienda per il Report Rilievi e fix sulla query per le date di Validità",
                cliente:="ENI KENYA",
                idTicketAssistenza:=164605, idTicketSviluppo:=0, idTicketTesting:=0,
                autore:="Dario Cabras",
                noteTest:="Selezionare un'azienda sui filtri e verificare che i rilievi riportati siano associati a quella sola azienda")

            Riga_Changelog(enum_Tipo_Changelog.Feature,
                           "Servizi Quaderno Di Campagna - Bio",
                           " Il servizio Quaderno di Campagna Bio ora ha le stesse funzionalita del servizio Quaderno di Campagna - SQNPI, quindi blocco delle operazioni, 
                           verifica delle sportello in creazione e modifica delle operazioni, e controllo se l'azienda e' in verifica",
                           "Regione Umbria",
                           idTicketAssistenza:=0,
                           idTicketSviluppo:=163377,
                           idTicketTesting:=0,
                           autore:=DEV_Uhalid)

            Riga_Changelog(
                enum_Tipo_Changelog.Bug,
                "Utenti Permessi",
                "Corretta gestione del permesso id_operazione 9 per licenza scaduta",
                cliente:="Nogalba",
                idTicketAssistenza:=166075, idTicketSviluppo:=0, idTicketTesting:=0,
                autore:="Riccardo Drudi",
                noteTest:=""
            )

            '===================================================================

            Riga_Versione("21 Febbraio 2025", "135.1.0")

            Riga_Requisiti("AgronicaWebService2010",
                           "Per calcolo tessitura da anagrafica appezzamento", "17/02/2025")

            Riga_Requisiti("Migra",
                           "Aggiunte tabelle in lingua del modulo zootecnia",
                           Ver:="781")

            Riga_Requisiti("Aggancio",
                           "Aggiunte viste in lingua del modulo zootecnia", "130")

            Riga_Requisiti("Configurazione_Siti",
                           "Aggiunta chiave 'UserPwdConnectionString_toCrypt' (super server)", "150")

            Riga_Requisiti("AgronicaCoreAPI",
                           "EndPoint per Widget Previsioni AI",
                           "17/02/2025")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")

            Riga_Changelog(enum_Tipo_Changelog.Bug,
                           "Anagrafica Catasto",
                           "Corretta lettura e scrittura di particelle con il campo sezione valorizzato",
                           cliente:="Aboca",
                           idTicketAssistenza:=164320, idTicketSviluppo:=0, idTicketTesting:=0,
                           autore:="Drudi Riccardo", noteTest:="provare a salvare e modificare una particella catastale valorizzando il campo sezione (anche non valorizzandolo) ")

            Riga_Changelog(enum_Tipo_Changelog.Feature,
                           "Anagrafica Esercizi",
                           "Aggiunti metodi per lettura e scrittura dei contributi ACA associati all'esercizio",
                           cliente:="Regione Umbria",
                           idTicketAssistenza:=163377, idTicketSviluppo:=0, idTicketTesting:=0,
                           autore:="Zammataro Salvatore", noteTest:="provare a salvare e modificare i contributi ACA dalla pagina di edit completo dell'esercizio in anagrafica NG")

            Riga_Changelog(enum_Tipo_Changelog.Bug,
                           "Report Rilievi",
                           "Aggiunte colonne sul Report Rilievi e modificati alcuni titoli di colonna",
                           cliente:="ENI KENYA",
                           idTicketAssistenza:=0, idTicketSviluppo:=162807, idTicketTesting:=0,
                           autore:="Dario Cabras")

            Riga_Changelog(enum_Tipo_Changelog.Bug,
                           "Agro_Sequenze",
                           "Ripristino porzione di codice rimossa erroneamente",
                           "",
                           idTicketAssistenza:=0, idTicketSviluppo:=0, idTicketTesting:=0,
                           autore:=DEV_Funcy,
                           noteTest:="Provare a creare un nuovo layer GIS in un ambiente dove non esistono già layer personalizzati. (un ambiente dove il gis non viene utilizzato è la strada più semplice).
                           La creazione deve andare a buon fine")

            Riga_Changelog(enum_Tipo_Changelog.Bug,
                           "Analisi Terreno (new!)",
                           "Aggiunta una colonna sulla query degli esercizi per filtrare gli impianti per la loro data fine di validità",
                           cliente:="coldiretti",
                           idTicketAssistenza:=164947, idTicketSviluppo:=0, idTicketTesting:=0,
                           autore:="Dario Cabras")

            '==================================================================

            Riga_Versione("17 Febbraio 2025", "135.0.0")

            Riga_Requisiti("AgronicaWebService2010",
                           "Per calcolo tessitura da anagrafica appezzamento", "17/02/2025")

            Riga_Requisiti("Migra",
                           "Aggiunte tabelle in lingua del modulo zootecnia",
                           Ver:="781")

            Riga_Requisiti("Aggancio",
                           "Aggiunte viste in lingua del modulo zootecnia", "130")

            Riga_Requisiti("Configurazione_Siti",
                           "Aggiunta chiave 'UserPwdConnectionString_toCrypt' (super server)", "150")

            Riga_Requisiti("AgronicaCoreAPI",
                           "EndPoint per Widget Previsioni AI",
                           "17/02/2025")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")

            Riga_Changelog(enum_Tipo_Changelog.Feature,
                           "Anagrafe DAL - Reg_Impianti_Codici",
                           "Implementato metodo per l'estrazione dei codici ACA associati agli impianti",
                           cliente:="Regione Umbria",
                           idTicketAssistenza:=0, idTicketSviluppo:=163377, idTicketTesting:=0,
                           autore:="Marco Cecalupo")

            Riga_Changelog(enum_Tipo_Changelog.Feature,
                           "Refactor log",
                           "Centralizzazione delle chiamate 'Scrivi_LOG' con la gestione standardizzata con objParametri",
                           cliente:="Coldiretti",
                           idTicketAssistenza:=0, idTicketSviluppo:=163176, idTicketTesting:=0,
                           autore:="Marco Cecalupo")

            Riga_Changelog(enum_Tipo_Changelog.Feature,
                           "Menu Agenda NG",
                           "Aggiunge funzione per il controllo della presenza di allegati legati a un attivita colturale",
                           cliente:="",
                           idTicketAssistenza:=0, idTicketSviluppo:=0, idTicketTesting:=0,
                           autore:="Anny Bevilacqua")

            Riga_Changelog(enum_Tipo_Changelog.Feature,
                           "Anagrafica NG Macchine",
                           "Aggiunto controllo sul cambio tipologia della macchina",
                           cliente:="",
                           idTicketAssistenza:=0, idTicketSviluppo:=0, idTicketTesting:=0,
                           autore:="Salvatore Zammataro",
                           noteTest:="Se si tenta di cambiare la tipologia di una macchina e questa risulta associata and un Appezzamento, la modifica viene impedita")

            Riga_Changelog(enum_Tipo_Changelog.Feature,
                           "Anagrafiche NG",
                           "Sviluppati metodi per lettura cancellazione modifica associazioni AppezzamentiXParcoMacchine",
                           cliente:="",
                           idTicketAssistenza:=0, idTicketSviluppo:=0, idTicketTesting:=0,
                           autore:="Salvatore Zammataro")

            Riga_Changelog(enum_Tipo_Changelog.Performance,
                           "AgronicaCoreModello",
                           "Ottimizzazione caricamento coordinate GPS solo per le operazioni di campagna (e solo per visite e rilievi, che al momento sono le uniche che le prevedono)",
                           cliente:="Coldiretti",
                           idTicketAssistenza:=0, idTicketSviluppo:=0, idTicketTesting:=0,
                           autore:="Daniela Tura")

            Riga_Changelog(enum_Tipo_Changelog.Bug,
                           "WidgetIndiciProduttivitaWS",
                           "Fix query estrazione dashboard sintetica indici di rischio (avoid division by 0)",
                           cliente:="CreditAgricole",
                           idTicketAssistenza:=0, idTicketSviluppo:=0, idTicketTesting:=0,
                           autore:="Lorenzo Lavezzo",
                           noteTest:="no test")

            Riga_Changelog(enum_Tipo_Changelog.Bug,
                           "Sincro documenti APP",
                           "Fix username upload nei documenti inviati dall'app",
                           cliente:="CAI",
                           idTicketAssistenza:=163955, idTicketSviluppo:=0, idTicketTesting:=0,
                           autore:="Carlo Giovanardi",
                           noteTest:="inviare un documento dall'app e verificare che la colonna username upload nel documentale sia valorizzata")

            Riga_Changelog(enum_Tipo_Changelog.Bug,
                           "WS Nazioni.asmx/Leggi",
                           "Aggiunto valore codice Ag. Entrate Unico per specializzare i dati restituiti dal WS",
                           cliente:="Borgoluce",
                           idTicketAssistenza:=0, idTicketSviluppo:=0, idTicketTesting:=0,
                           autore:="Gianluca Amoroso",
                           noteTest:="Chiamata performa = 34373 - Legato alla segnalazione di Borgoluce per i valori 'undefined' nell'elenco delle Nazioni")

            Riga_Changelog(enum_Tipo_Changelog.Feature,
                           "Anagrafica Esercizi NG",
                           "Rimossa lettura codici esercizio di tipo Nr_Domanda_ACA",
                           cliente:="Regione Umbria",
                           idTicketAssistenza:=0, idTicketSviluppo:=0, idTicketTesting:=0,
                           autore:="Salvatore Zammataro",
                           noteTest:="Non è più possibile da interfaccia salvare o visualizzare codici esercizio di tipo Nr_Domanda_ACA")

            Riga_Changelog(enum_Tipo_Changelog.Feature,
                           "Lettura fitofarmaci",
                           "Sostituzione clausola xml path con string_agg in query lettura fitofarmaci",
                           cliente:="",
                           idTicketAssistenza:=0, idTicketSviluppo:=0, idTicketTesting:=0,
                           autore:="Marco Lucchi",
                           noteTest:="Testare con carico")

            Riga_Changelog(enum_Tipo_Changelog.Feature,
                           "Esercizi X Contributi",
                           "Creati metodi per lettura, scrittura, modifica e cancellazione delle associazioni Esercizio-Contributo",
                           cliente:="Regione Umbria",
                           idTicketAssistenza:=0, idTicketSviluppo:=0, idTicketTesting:=0,
                           autore:="Salvatore Zammataro",
                      noteTest:="Non è ancora presente un modo per testare tali funzioni, in quanto non è ancora disponibile l'interfaccia grafica per l'utente")

            Riga_Changelog(enum_Tipo_Changelog.Feature,
                           "Contributi",
                           "Creati metodi per lettura, modifica, cancellazione dei Contributi",
                           cliente:="Regione Umbria",
                           idTicketAssistenza:=0, idTicketSviluppo:=0, idTicketTesting:=0,
                           autore:="Salvatore Zammataro",
                           noteTest:="Non ancora testabile in quanto manca l'interfaccia grafica per l'utente")

            Riga_Changelog(enum_Tipo_Changelog.Feature,
                           "Dashboard Widget",
                           "Creato nuovo Widget per previsioni, dati gli esercizi aperti nella data corrente, costi e ricavi con AI.",
                           cliente:="",
                           idTicketAssistenza:=0, idTicketSviluppo:=163288, idTicketTesting:=0,
                           autore:="Dario Cabras")

            Riga_Changelog(enum_Tipo_Changelog.Feature,
                           "Report Rilievi",
                           "Creato nuova voce nel Menù 'Report Rilievi', con una griglia che riporta i Rilievi creati sotto l'azienda corrente",
                           cliente:="",
                           idTicketAssistenza:=0, idTicketSviluppo:=0, idTicketTesting:=0,
                           autore:="Dario Cabras")

            Riga_Changelog(enum_Tipo_Changelog.Feature,
                           "Permesso per modificare operazioni bloccate da stati nel workflow",
                           "Aggiunto un nuovo permesso utente ""'Modifica Operazione in Verifica"", nel categoria ""Agenda Movimenti Aziendali"". 
                           Questo permesso permette di evitare i controlli dovuti allo stato di un servizio sul workflow. 
                           Per esempio, se un azienda ha il servizio ""Quaderno di Campagna - SQNPI"" e si trova in uno stato uguale o successivo a ""Verifica In corso"", l'utente non potra' modificare o fare
                           nuove operazioni, avendo questo permesso si potra' evitare questo controllo.",
                           cliente:="Regione Umbria",
                           idTicketAssistenza:=0, idTicketSviluppo:=163377, idTicketTesting:=0,
                           autore:="Uhalid Abou El Kheir",
                           noteTecniche:="Agli utenti che hanno questo permesso verra' anche dato il permesso per sbloccare l'operazione.",
                           noteTest:="")

            Riga_Changelog(enum_Tipo_Changelog.Feature,
                           "Anagrafica Appezzamento NG",
                           "Aggiunti campi sabbia-limo-argilla per calcolare la tessitura dell'appezzamento",
                           "Regione Umbria",
                           0, 163347, 0,
                           DEV_Funcy,
                           noteTecniche:="Per poter testare è necessario puntare i WebService del PianoConcimazione al link di TestInterni, allinearsi con lo sviluppatore al momento del test.
                           Quando il test è approvato bisogna aggiornare i WebService di Produzione (Giulia)")

            Riga_Changelog(enum_Tipo_Changelog.Bug,
                           "Sincro dati APP",
                           "Fix per mantenere il riferimento all'operazione di agenda negli allegati delle visite app",
                           cliente:="CAI",
                           idTicketAssistenza:=164326, idTicketSviluppo:=0, idTicketTesting:=0,
                           autore:="Carlo Giovanardi",
                           noteTest:="Inviare una visita con allegato dall'app e successivamente caricare i dati dal brogliaccio per verificare che venga mantenuto il riferimento")

            '==================================================================

            Riga_Data("13 Febbraio 2025")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Audit Coprob", "23/04/2024")

            Riga_Requisiti("Migra",
                           "Aggiunte tabelle in lingua del modulo zootecnia",
                           Ver:="781")

            Riga_Requisiti("Aggancio",
                           "Aggiunte viste in lingua del modulo zootecnia", "130")

            Riga_Requisiti("Configurazione_Siti",
                           "Aggiunta chiave 'UserPwdConnectionString_toCrypt' (super server)", "150")

            Riga_Requisiti("AgronicaCoreAPI",
                           "Per modifiche relative all'autenticazione",
                           "16/12/2024")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")

            Riga_Bug("APP - ScriviAppezzamento",
                     "Irrobustito il processo di verifica dell'esistenza degli appezzamenti inviati da APP a WEB per impedire la creazione di doppioni",
                     noteTest:="Da APP replicare i casi di duplicazione segnalati dal Kenya e Costa d'Avorio, 
                     creare un appezzamento offline e inviarlo successivamente a web, 
                     provare la sincronizzazione del nuovo appezzamento in situazione di scarsa connessione e/o 
                     durante la sincronizzazione mettersi in modalità aereo in modo da non recepire il risultato inviato da web all'app, 
                     successivamente riattivare la connessione e ritentare la sincronizzazione dell'appezzamento, 
                     controllare che non siano stati creati dei doppioni lato web ")

            '==================================================================

            Riga_Data("07 Febbraio 2025")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Audit Coprob", "23/04/2024")

            Riga_Requisiti("Migra",
                           "Aggiunte tabelle in lingua del modulo zootecnia",
                           Ver:="781")

            Riga_Requisiti("Aggancio",
                           "Aggiunte viste in lingua del modulo zootecnia", "130")

            Riga_Requisiti("Configurazione_Siti",
                           "Aggiunta chiave 'UserPwdConnectionString_toCrypt' (super server)", "150")

            Riga_Requisiti("AgronicaCoreAPI",
                           "Per modifiche relative all'autenticazione",
                           "16/12/2024")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")

            Riga_Bug("WidgetIndiciProduttivitaWS",
                     "Fix query estrazione dashboard sintetica indici di rischio (avoid division by 0)",
                     "CreditAgricole",
                     noteTest:="no test")

            '==================================================================

            Riga_Data("04 Febbraio 2025")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Audit Coprob", "23/04/2024")

            Riga_Requisiti("Migra",
                           "Aggiunte tabelle in lingua del modulo zootecnia",
                           Ver:="781")

            Riga_Requisiti("Aggancio",
                           "Aggiunte viste in lingua del modulo zootecnia", "130")

            Riga_Requisiti("Configurazione_Siti",
                           "Aggiunta chiave 'UserPwdConnectionString_toCrypt' (super server)", "150")

            Riga_Requisiti("AgronicaCoreAPI",
                           "Per modifiche relative all'autenticazione",
                           "16/12/2024")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")

            Riga_Bug("Checklist Hevea Eni CIV",
                     "Corretto bug che non faceva comparire le colonne audit nella griglia di ricerca")

            '==================================================================

            Riga_Data("31 Gennaio 2025")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Audit Coprob", "23/04/2024")

            Riga_Requisiti("Migra",
                           "Aggiunte tabelle in lingua del modulo zootecnia",
                           Ver:="781")

            Riga_Requisiti("Aggancio",
                           "Aggiunte viste in lingua del modulo zootecnia", "130")

            Riga_Requisiti("Configurazione_Siti",
                           "Aggiunta chiave 'UserPwdConnectionString_toCrypt' (super server)", "150")

            Riga_Requisiti("AgronicaCoreAPI",
                           "Per modifiche relative all'autenticazione",
                           "16/12/2024")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")

            Riga_Bug("Security - AgronicaCoreParametri",
                     "Fix per recuperare nome db anche in caso di stringa connessione codificata")

            Riga_Bug("Distribuzione Ammendanti Organici o Liquami (new!)",
                     "Fix calcolo N,P,K medio ponderato",
                     idPerforma:=34645, cliente:="Genagricola")

            Riga_Bug("Lettura Agenda",
                     "I dati del Gis WKT collegato vengono ora letti solo per le righe di Mov_Destinazioni che rappresentano degli impianti (Appezza > 0) e non anche per le righe di magazzino",
                     noteTecniche:="Non dovrebbe cambiare nulla perché tanto facendo quella query non avrebbe trovato dati, ma così facendo ci risparmiamo una query pesante e che in alcuni ambienti (tipo Coldiretti) spesso va in errore per saturazione della memoria")

            Riga_Bug("Security - AgronicaCoreParametri",
                     "Fix per recuperare nome db anche in caso di stringa connessione codificata")

            '==================================================================

            Riga_Data("24 Gennaio 2025")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Audit Coprob", "23/04/2024")

            Riga_Requisiti("Migra",
                           "Aggiunte tabelle in lingua del modulo zootecnia",
                           Ver:="781")

            Riga_Requisiti("Aggancio",
                           "Aggiunte viste in lingua del modulo zootecnia", "130")

            Riga_Requisiti("Configurazione_Siti",
                           "Aggiunta chiave 'UserPwdConnectionString_toCrypt' (super server)", "150")

            Riga_Requisiti("AgronicaCoreAPI",
                           "Per modifiche relative all'autenticazione",
                           "16/12/2024")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")

            Riga_Text("Configurazione Operazioni Colturali - Anagrafiche Rilievi Indici Coltura",
                    "Inseriti DAL e webmethod per la lettura, l'inserimento, la cancellazione e la modifica delle anagrafiche per i rilievi degli indici di coltura",
                    noteTest:="Provare a inserire, modificare e cancellare un'anagrafica per un rilievo di indici di coltura")

            Riga_Bug("Profilazione NG",
                    "Aggiusta il cambio password eseguito sugli utenti dalla griglia della nuova profilazione",
                    noteTest:="Segnalato a voce da Giacomo, non penso ci sia un ticket aperto al riguardo")

            Riga_Bug("Modifica Azienda",
                     "Fix modifica azienda che andava in errore 500 (quando era presente un dato relativo alla data iscrizione libro soci)",
                     "Eni Costa Avorio ", 34509)

            Riga_Bug("Menù Agenda (new!)",
                     "Fix caricamento Menù Agenda quando ci sono delle Operazioni registrate su un Lotto Esercizio/Produzione con l'apostrofo (esempio: MELO MODI' BIO)",
                     "COZ", 34550)

            Riga_Bug("Caricamento specie Menu Agenda (ng)",
                     "Fix caricamento specie filtri menu agenda: ora mostra tutte quelle presenti in azienda a prescindere dalla validità dell'esercizio",
                     "CAI", 34543,
                     noteTest:="Creare un esercizio con le seguenti caratterisitche:
                     - validità 01/01/2024-31/12/2024
                     - assegnare una specie non presente nel 2025
                     Verificare che la specie sia filtrabile nel menu agenda")

            '==================================================================

            Riga_Data("20 Gennaio 2025")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Audit Coprob", "23/04/2024")

            Riga_Requisiti("Migra",
                           "Aggiunte tabelle in lingua del modulo zootecnia",
                           Ver:="781")

            Riga_Requisiti("Aggancio",
                           "Aggiunte viste in lingua del modulo zootecnia", "130")

            Riga_Requisiti("Configurazione_Siti",
                           "Aggiunta chiave 'UserPwdConnectionString_toCrypt' (super server)", "150")

            Riga_Requisiti("AgronicaCoreAPI",
                           "Per modifiche relative all'autenticazione",
                           "16/12/2024")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")

            Riga_Text("Default Filtro Ricerca",
                      "Il Filtro di Ricerca (new!) è ora il default in caso di mancanza di configurazione (arrivando da chiamanti come stampe e pagine varie)")

            Riga_Text("Autenticazione",
                      "Migliorata la logica che in condizioni di debug e CORS (può succedere in debug con i sorgenti avviati) evita il controllo di sicurezza",
                            noteTest:="Non testabile")

            Riga_Text("Abaco DPI",
                      "Migliorato il filtro di import dpi che ora permette un set di disciplinari da importare",
                            noteTest:="Non testabile")

            Riga_Text("Zootecnia",
                     "Gestita Localizzazione del modulo",
                     cliente:="Inalca", noteTecniche:="ID TFS 8600")

            Riga_Text("Configurazione Operazioni Colturali",
                        "Web method per letture necessarie alla pagina 'Configurazione Operazioni Colturali'")

            Riga_Text("Rilievo Fioritura/Fasi Fenologiche in QdCA",
                      "Aggiunto blocco al salvataggio se si scelgono Fasi Fenologiche di specie vegetali diverse",
                            noteTest:="Selezionare una Fase Fenologica di una specie, poi deselezionare le righe degli impianti. 
                            Scegliere una specie diversa, selezionare gli impianti e la nuova Fase Fenologica. 
                            Una volta che sono state aggiunte le Fasi con impianti di Specie diverse salvare l'Operazione e verificare se appare il messaggio bloccante.")

            Riga_Text("SQL DataProvider parametrizzatore order by",
                      "Ora la presenza di una clausola OFFSET all'interno di un Order By è contemplato",
                        noteTest:="Non testabile")

            Riga_Bug("Trattamenti su derrate (Post Raccolta/Concia del Seme) x Controlli Etichetta",
                     "gestiti controlli di etichetta per i seguenti dosaggi: ml/1 t di prodotto (ex. K-Obiol EC 25 - 6557) e ml/100 kg di bulbilli",
                     noteTest:="Ticket #161272")

            Riga_Bug("Anagrafiche (new!)",
                     "Corretto ordinamento dropdown disciplinari",
                     noteTest:="Ticket #159952")

            Riga_Bug("Griglia Ricette NG - Concia del Seme",
                     "ora nel dettaglio tecnico compaiono i prodotti in questo formato 'formulato (qta tot distribuita ) - principi attivi - avversità', come per i trattamenti antiparassitari così da rispecchiare quello che si vede in griglia agenda",
                     noteTest:="Ticket #161347 ")

            Riga_Text("QDCA",
                     "Diversificazione controlli accesso a QDCA per Coldiretti\RegioneUmbria",
                     cliente:="RegioneUmbria", noteTecniche:="Se abilitato il controllo tramite chiave confg_siti, vengono verificati i servizi bluarancio (se modalità coldiretti) mentre in modalità RegioneUmbria non viene controllato nulla ")

            Riga_Bug("Modifica Patentino contatto",
                     "Fix modifica patentini da modifica contatto",
                     "Genagricola ", 34396,
                     noteTest:="Creare/Modificare patentini da anagrafica contatti. 
                     Provare su ambiente configurato per salvataggio su db e salvataggio su filesystem (chiedere con Anna F. per indicazioni)
                     In caso di presenza allegato, testare corretta apertura file da gestione patentini e da ricerca documenti.")

            '==================================================================

            Riga_Data("17 Gennaio 2025")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Audit Coprob", "23/04/2024")

            Riga_Requisiti("Migra",
                           "Nuova Tabella Operazione_Causale,
                           Nuova colonna Flag_Encrypted in tabella Connessioni",
                           Ver:="778")

            Riga_Requisiti("Aggancio",
                           "Per viste compliance ISCC", "127")

            Riga_Requisiti("Configurazione_Siti",
                           "Aggiunta chiave 'UserPwdConnectionString_toCrypt' (super server)", "150")

            Riga_Requisiti("AgronicaCoreAPI",
                           "Per modifiche relative all'autenticazione",
                           "16/12/2024")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")

            Riga_Bug("Rilievo Indici Coltura (QDCA new)",
                     "Gestito il salvataggio degli indici coltura in cui bisogna scegliere una data (esempio: indice 'Date of sowing' per il Ricino in Eni Kenya)",
                     idPerforma:=34459,
                     cliente:="ENI KENYA",
                     noteTecniche:="La chiamata non è stata risolta del tutto ,
                     manca da gestire il salvataggio degli indici di tipo testuale (esempio: 'If farmer did Not planted castor, why')")

            '==================================================================

            Riga_Data("15 Gennaio 2025")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Audit Coprob", "23/04/2024")

            Riga_Requisiti("Migra",
                           "Nuova Tabella Operazione_Causale,
                           Nuova colonna Flag_Encrypted in tabella Connessioni",
                           Ver:="778")

            Riga_Requisiti("Aggancio",
                           "Per viste compliance ISCC", "127")

            Riga_Requisiti("Configurazione_Siti",
                           "Aggiunta chiave 'UserPwdConnectionString_toCrypt' (super server)", "150")

            Riga_Requisiti("AgronicaCoreAPI",
                           "Per modifiche relative all'autenticazione,
                           Primo rilascio Analisi Terreno (new!),
                           Aggiunta end-point per nuova pagina import utenti",
                           "16/12/2024")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")

            Riga_Bug("Sync Azienda creata da APP",
                     "risolto baco che impediva la sync delle aziende create da APP",
                     noteTest:="Mettere l'APP offline, creare una nuova azienda, creare un appezzamento collegato ad essa, mettere l'app online e fare sync da 'piano colturale'")

            '==================================================================

            Riga_Data("14 Gennaio 2025")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Audit Coprob", "23/04/2024")

            Riga_Requisiti("Migra",
                           "Nuova Tabella Operazione_Causale,
                           Nuova colonna Flag_Encrypted in tabella Connessioni",
                           Ver:="778")

            Riga_Requisiti("Aggancio",
                           "Per viste compliance ISCC", "127")

            Riga_Requisiti("Configurazione_Siti",
                           "Aggiunta chiave 'UserPwdConnectionString_toCrypt' (super server)", "150")

            Riga_Requisiti("AgronicaCoreAPI",
                           "Per modifiche relative all'autenticazione,
                           Primo rilascio Analisi Terreno (new!),
                           Aggiunta end-point per nuova pagina import utenti",
                           "16/12/2024")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")

            Riga_Bug("Controlli su Validita Esercizi Anagrafica NG ",
                     "Fix controllo ricette",
                     "Aboca", 34250,
                     noteTecniche:="Verificava la colonna ricette_destinazioni.validita_inizio anziché ricetta_operazione.validita_inizio")

            '==================================================================

            Riga_Data("10 Gennaio 2025")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Audit Coprob", "23/04/2024")

            Riga_Requisiti("Migra",
                           "Nuova Tabella Operazione_Causale,
                           Nuova colonna Flag_Encrypted in tabella Connessioni",
                           Ver:="778")

            Riga_Requisiti("Aggancio",
                           "Per viste compliance ISCC", "127")

            Riga_Requisiti("Configurazione_Siti",
                           "Aggiunta chiave 'UserPwdConnectionString_toCrypt' (super server)", "150")

            Riga_Requisiti("AgronicaCoreAPI",
                           "Per modifiche relative all'autenticazione,
                           Primo rilascio Analisi Terreno (new!),
                           Aggiunta end-point per nuova pagina import utenti",
                           "16/12/2024")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")

            Riga_Bug("Problemi alla login con numero elevato di utenti",
                     "Migliorata la logica che elimina i token per evitare problemi con numero elevato di utenti")

            '==================================================================

            Riga_Data("07 Gennaio 2025")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Audit Coprob", "23/04/2024")

            Riga_Requisiti("Migra",
                           "Nuova Tabella Operazione_Causale,
                           Nuova colonna Flag_Encrypted in tabella Connessioni",
                           Ver:="778")

            Riga_Requisiti("Aggancio",
                           "Per viste compliance ISCC", "127")

            Riga_Requisiti("Configurazione_Siti",
                           "Aggiunta chiave 'UserPwdConnectionString_toCrypt' (super server)", "150")

            Riga_Requisiti("AgronicaCoreAPI",
                           "Per modifiche relative all'autenticazione,
                           Primo rilascio Analisi Terreno (new!),
                           Aggiunta end-point per nuova pagina import utenti",
                           "16/12/2024")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")

            Riga_Bug(
                Titolo:="Profilazione NG",
                Testo:="Modifica la gestione delle eccezioni in disabilitazione dei permessi sul superuser per assicurare la
                    visualizzazione del messaggio di errore",
                noteTest:="In riferimento al documento di test della profilazione, nessun ticket aperto trovato",
                noteTecniche:="Tentando di disabilitare il permesso di Accesso al Menu Principale sul superuser verrà ora
                    lanciata una GiasException per assicurare che venga mostrato il messaggio riportante l'errore."
            )

            '==================================================================

            Riga_Data("03 Gennaio 2025")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Audit Coprob", "23/04/2024")

            Riga_Requisiti("Migra",
                           "Nuova Tabella Operazione_Causale,
                           Nuova colonna Flag_Encrypted in tabella Connessioni",
                           Ver:="778")

            Riga_Requisiti("Aggancio",
                           "Per viste compliance ISCC", "127")

            Riga_Requisiti("Configurazione_Siti",
                           "Aggiunta chiave 'UserPwdConnectionString_toCrypt' (super server)", "150")

            Riga_Requisiti("AgronicaCoreAPI",
                           "Per modifiche relative all'autenticazione,
                           Primo rilascio Analisi Terreno (new!),
                           Aggiunta end-point per nuova pagina import utenti",
                           "16/12/2024")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")

            Riga_Bug(
                "Menu Visite (new!) - Dettaglio Rilievo",
                "corretta traduzione dei valori 1/0 in 'Presente/Assente' nel caso di Rilievo Avversità; corretta la traduzione dei valori per i Rilievi Danni; aggiunta la colonna 'Causali Rilievo' (al momento significativa solo per Rilievo Produzione Prevista, se selezionate in creazione del rilievo)",
                noteTest:="ticket #159971"
            )

            '==================================================================

            Riga_Data("20 Dicembre 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Audit Coprob", "23/04/2024")

            Riga_Requisiti("Migra",
                           "Nuova Tabella Operazione_Causale,
                           Nuova colonna Flag_Encrypted in tabella Connessioni",
                           Ver:="778")

            Riga_Requisiti("Aggancio",
                           "Per viste compliance ISCC", "127")

            Riga_Requisiti("Configurazione_Siti",
                           "Aggiunta chiave 'UserPwdConnectionString_toCrypt' (super server)", "150")

            Riga_Requisiti("AgronicaCoreAPI",
                           "Per modifiche relative all'autenticazione,
                           Primo rilascio Analisi Terreno (new!),
                           Aggiunta end-point per nuova pagina import utenti",
                           "16/12/2024")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")

            Riga_Text("QdCA",
                     "Spostato Controllo se è attiva la modalità QdC IV o Azienda mancante all'apertura dell'operazione di agenda e al cambio di Data",
                     cliente:="Coldiretti")

            Riga_Bug(
                "Profilazione NG",
                "Aggiunge dei check per fare in modo da impedire che venga disattivato il permesso di Accesso al
                Menu Principale sull'intera installazione",
                noteTest:="Era capitato che, dalla profilazione nuova, nella pagina di installazione che contiene i
                permessi del cliente superuser, venisse disattivato il permesso di Accesso al Menù Principale (codice
                permesso 1). Questa operazione comporta la disattivazione del permesso a cascata anche per tutti gli
                altri utenti, impedendo di fatti il login in gias. Si è quindi decisio di aggiungere dei controlli, per
                fare in modo che non sia possibile disattivare tale permesso a livello di superuser."
            )

            Riga_Text("Movimenti X Impianto",
                      "Corretta la verifica dei movimenti su impianto per APP")


            Riga_Bug("Sincronizzazione Checklist Coprob",
                     " Corretto bug che non avviava il processo GSB tipo_sincro 115")


            Riga_Text("Esportazione QDCA verso Agea",
                     "Valorizzato l'applicationTypeCode nelle phytochemicalTreatments e nelle organicFertilizations",
                        noteTest:="Nessun Test Necessario")

            Riga_Bug("Metadati - Operazioni",
                     "Corretto baco che impediva di filtrare le lavorazioni applicabili dall'utente nell'attività",
                     noteTest:="Qualsiasi installazione gias dove siano state configurate le lavorazioni visibili per l'utente, entrare con lo stesso utente nell'app e verificare che solo le lavorazioni selezionate siano visibili")

            '==================================================================

            Riga_Data("16 Dicembre 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Audit Coprob", "23/04/2024")

            Riga_Requisiti("Migra",
                           "Nuova Tabella Operazione_Causale,
                           Nuova colonna Flag_Encrypted in tabella Connessioni",
                           Ver:="778")

            Riga_Requisiti("Aggancio",
                           "Per viste compliance ISCC", "127")

            Riga_Requisiti("Configurazione_Siti",
                           "Aggiunta chiave 'UserPwdConnectionString_toCrypt' (super server)", "150")

            Riga_Requisiti("AgronicaCoreAPI",
                           "Per modifiche relative all'autenticazione,
                           Primo rilascio Analisi Terreno (new!),
                           Aggiunta end-point per nuova pagina import utenti",
                           "16/12/2024")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")

            Riga_Bug("Cancellazione Fattura collegata a DDT che cancella anche il DDT",
                     " Corretto bug che cancellava, oltre alla fattura, anche il ddt collegato) ",
                     "",
                     32700,
                     "",
                     "Provare a cancellare un ddt collegato a fattura ed una fattura collegata a ddt. Nel primo caso ci deve un blocco, nel secondo caso deve cancellare solo la fattura e non il ddt.")

            Riga_Text("Ottimizzazione Query Ricerca Documentale, Aggiorna dati CDG e Checklist",
                     "Aggiunta alle query di ricerca OPTION (USE HINT ('FORCE_LEGACY_CARDINALITY_ESTIMATION')) per avere buone performance anche in caso di livello retrocompatibilità Sql 2019",
                     "",
                     noteTest:="Verificare che le ricerche vengano eseguite correttamente")

            Riga_Text("Profilazione NG",
                     "- Gestione nuove proprietà filtrabili nei filtri pre-caricamento utenti:
                     Ora possibile pre-filtrare gli utenti basandosi anche su CF, email, data ultima modifica, data ultimo accesso
                     - Aggiunta nuova pagina in angular per l'import di utenti da excel",
                     noteTest:="Per la pagina nuova pagina di import utenti è necessario aver abilitato in lettura il permesso 297
                     (ManutenzioneArchivi_Import_Utenti_Da_Excel, lo stesso che serviva per accede alla vecchia pagina di import).
                     Per accedere alla pagina dovrebbe essere presente un nuovo pulsante nel menù sulla sinistra sotto
                     'Ammin. del sistema' -> 'Importazione Utenti da Excel (new!)'.
                     ")

            Riga_Text("Import Documenti",
                     "Importazione Documenti per Zani",
                     noteTest:="Come da Documento \\rubino2\\documentazione\\GIAS --- Clienti --- GRANFRUTTA ZANI\\Import - export conferimenti\\Import documentale conferimenti su GIAS.docx")

            Riga_Text("Porting Operazione CONCIA DEL SEME sul QdCA Angular",
                      "Gestione Operazione CONCIA DEL SEME sul QdCA (new!)
                      Gestione nuove colonne griglia Menu Agenda per riportare i dati dei Prodotti Magazzino Trattati (Nome prodotti, Lotti Accettazione coinvolti, Qta in quintali trattata)",
                      noteTecniche:="La CONCIA del SEME ANGULAR è un'operazione registrata sui Prodotti di Magazzino.
                      Tutte le operazioni create con il vecchio NON possono essere aperte con il nuovo e verrà fatto un redirect puntuale",
                      noteTest:="1) Fare una carico di magazzino di SEMENTI. Nb. l'unità di misura deve essere espressa in KG (multipli/sottomultipli) [lo stesso vale per trasformati vegetali in Trattamenti Post Raccolta]
                      2) Provare ad aprire una concia del seme creata con l'interfaccia vecchia --> deve essere sempre aperta sull'interfaccia vecchia anche passando da menu angular 
                      3) Provare ad aprire una concia del seme creata con l'interfaccia nuova --> deve essere sempre aperta sull'interfaccia nuova anche passando da menu vecchio. NB. In caso di mancati permessi ANGULAR, il menù vecchio deve dare errore 'Mancanza di Permessi'
                      Non sono gestiti i controlli di conformità sul numero max di interventi consentiti 
                      Non sono gestiti i controlli di conformità a posteriori
                      Gestiti i controlli su dose max/min del prodotto
                      IL PREGRESSO NON COMPARIRA' PIU' NEL VERIFICA CONFORMITA' A POSTERIORI!")

            Riga_Text("Ricette di TRATTAMENTI POST RACCOLTA e CONCIA DEL SEME sul QdCA Angular",
                      "Gestione Ricette e Brogliacci + Ribaltamenti
                      Gestione nuove colonne griglia Menu Agenda per riportare i dati dei Prodotti Magazzino Trattati (Nome prodotti, Qta in quintali trattata)",
                      noteTest:="Testare ricette, brogliacci e ribaltamenti
                      NB. Sulle ricette/brogliacci di Trattamenti Post Raccolta NON è gestita la suddivisione dei prodotti per esercizio di provenienza raccolta, quindi verrà visualizzata una singola riga a parità di Magazzino - Prodotto - Lotto. In fase di ribaltamento, la quantità trattata verrà riproporzionata su tutte le righe con lo stesso Magazzino - Prodotto - Lotto.
                      1) Fare Raccolta su più impianti, selezionando l'opzione di creazione lotto 'Automatica da Esercizio Corrente', così da avere effettivamente una riga di carico proveniente da ogni impianto.
                      2) Creare una ricetta di Post Raccolta sul prodotto appena raccolto (visualizzato accorpato in una singola riga)
                      3) Ribaltare in agenda (qta totale trattata in ricetta esplosa su tutte le righe originali)
                      
                      Per le ricette di Concia del Seme, esistenti anche sul vecchio modulo ma su impianti, vale la stessa regola dell'agenda: le ricette create con il vecchio possono essere aperte solo con il vecchio
                      4) Provare ad aprire una ricetta di concia del seme creata con l'interfaccia vecchia --> deve essere sempre aperta sull'interfaccia vecchia anche passando da menu angular 
                      5) Provare ad aprire una ricetta di concia del seme creata con l'interfaccia nuova --> deve essere sempre aperta sull'interfaccia nuova anche passando da menu vecchio. NB. In caso di mancati permessi ANGULAR, il menù vecchio deve dare errore 'Mancanza di Permessi'
                      
                      NB2: sia in agenda che ricette/brogliacci, non verranno considerati i prodotti caricati a magazzino con unità di misura diversa da KG (+ multipli/sottomultipli)
                      6) Fare uno scarico di prodotto per andare sotto giacenza, verificare che il valore negativo non sia presente in griglia 'Prodotti Magazzino' ")

            Riga_Text("Caricamento specie QdCA (new!)",
                      "Sia in griglia Menu Agenda che all'interno dell'operazione la lista delle Specie considera anche tutte quelle di TRASFORMATI VEGETALI e SEMENTI presenti in magazzino (con giacenza > 0 alla data in caso di edit operazione)",
                      noteTest:="Effettuare un carico di TRASFORMATI VEGETALI/SEMENTI su una specie NON presente in anagrafica.
                      Verificare che la nuova specie sia correttamente caricata in edit operazione e nei filtri del Menu Agenda. Se si sposta la data dell'operazione prima del carico, la specie delle sparire dalla lista.
                      In edit operazione, verificare che non ci siano anomalie/errori se si seleziona una specie che non ha impianti/prodotti magazzino a parità di operazione (ex. trattamento antiparassitario sulla specie del trasformato/semente appena caricata da magazzino, che non abbia impianti attivi in anagrafica e viceversa)")

            Riga_Text("Anagrafe imprese",
                     "Disciplinare aziendale predefinito messo in pagina azienda invece che nella profilazione imprese",
                     noteTest:="inserire/modificare disciplinare aziendale predefinito")


            Riga_Bug("MVV Elettronico",
                      "Sistemato invio al sian con mezzo di trasporto enodotto")

            Riga_Text("Nuova Stampa Registri ACA",
                      "Per nuovo codice stampe 'SchedaCampagna_Multicentro_ACA' e codice anagrafe 'Nr domanda ACA' su esercizio",
                      "Regione Umbria",
                      0,
                      "",
                      "Popolare un 'Nr domanda ACA' su un esercizio, se fosse necessario si possono indicare più numeri domanda separati da virgola (max caratteri 250)")

            Riga_Text("ENI CIV Checklist Hevea",
                     "Inserite colonne di ricerca in lista checklist",
                     noteTest:="Verificare presenza colonne risposte alle domande 1.3, 1.8, 3.1, 3.2, 3.3")

            Riga_Text("QdCA - Rilievo Produzione Prevista",
                     "Realizzato il metodo di lettura delle causali da associare al Rilievo",
                     "Orogel")

            Riga_Text("QdC NG",
                        "- Aggiunto menù a tendina 'Modalità di Applicazione' per le fertilizzazioni",
                        noteTest:="PROVENIENTE DAL RAMO UMBRIA - GIA' TESTATO")

            Riga_Bug("Documentale",
                        "Sistemazione filtri operazioni colturali da categoria/tipologia documentale",
                        noteTest:="PROVENIENTE DAL RAMO UMBRIA - GIA' TESTATO")

            Riga_Text("Security",
                      "Attivata modalità nel web.config per criptare il viewstate delle pagine",
                      noteTest:="Non testabile")

            Riga_Text("QdC NG",
                        "- Aggiunta nuova proprietà consiglioIrrigazione in DettaglioIrrigazione",
                        noteTest:="PROVENIENTE DAL RAMO UMBRIA - (Nessun test necessario)")

            Riga_Text("Esportazione QDCA verso Agea",
                     "Aggiunte all'esportazione le operazioni di Concia del Seme (seedTreatment) e Trattamento Post Raccolta (productTreatment)",
                        noteTest:="Nessun Test Necessario", noteTecniche:="ID TFS 8606")

            Riga_Text("Security", "Interventi vari per evitare il passaggio della stringa di connessione al db (8507).
                      Modifiche web.config per consentire la criptazione dell'appsettings con aggiunta dei parametri super server dove mancava.
                      Gestione criptazione user e password nella tabella connessioni del super server e caricamento iniziale in memoria delle stringhe di connessione,
                      per consentire la successiva decodifica ed evitare il passaggio negli obj parametri della stringa completa ma solo dell'id db.",
                      noteTecniche:="attivare la modalità impostando a 'true' la chiave 'UserPwdConnectionString_toCrypt' in configurazione siti super server",
                      noteTest:="verificare login iniziale e passaggio tra i vari siti usando sia il menu nuovo angular sia quello classico")

            Riga_Text("Enumerativi",
                      "Aggiorna l'enumerativo enum_PagineGiasNG con alcuni valori presi dalla copia Angular dello stesso.",
                      noteTest:="Nessun Test Necessario")

            Riga_Text("Analisi Terreno (new!) - Aggancio Mappa GIS",
                       "Aggancio alla Mappa del GIS: a seconda dell'entità selezionata, il GIS carica layer diversi. 
                       Dalla mappa è possibile attivare la modalità 'Campione' e selezionare un punto da inserire nel campione, analogamente le coordinate inserite nella sezione del Campione andranno a creare/spostare il punto sulla Mappa. ")

            Riga_Text("Analisi Terreno (new!) - Aggancio Chiamanti",
                      "E' possibile richiamare le Analisi Terreno (new!) da PUA, UMA, Piano Concimazione e Piano Nutrizionale 
                      NB. Quando un'analisi è associata ad un piano di concimazione etc, non è possibile cancellarla oppure modificare i parametri: entrando in modifica la sezione 'Parametri Analisi' sarà in sola visualizzazione e comparirà un messaggio di alert per l'utente. Il resto dei dati sarà modificabile",
                      noteTecniche:="L'impostazione SUPERUSER_Mod_Analisi_Terreno (892) deve essere impostata con valore 2 
                      Da interfaccia: Utenti e Permessi (new!) -> Impostazioni Superuser -> Impostazioni Superuser -> 'ALTRE IMPOSTAZIONI' -> 'Modalità richiamo Analisi del Terreno' -> 'Avanzata (Angular)'",
                      noteTest:="Verificare note versione siti citati
                      Verificare che la modifica di un'analisi agganciata non permetta la cancellazione e la modifica dei parametri")

            Riga_Text("Anagrafica Appezzamenti x ParcoMacchine",
                      "Implementate le funzioni per lettura, scrittura, modifica e cancellazione della tabella AppezzamentiXParcoMacchine",
                      noteTest:="Le funzioni sono soggette alla presenza del permesso 'Gestione associazioni appezzamenti / parco macchine' e sono richiamabili da interfaccia angular, griglia esercizi delle anagrafiche (solo scrittura e cancellazione al momento)")

            Riga_Text("Esportazione QDCA verso Agea",
                     "Aggiunta all'esportazione insieme alla Data l'Ora salvata nelle Operazioni di Campagna",
                        noteTest:="Nessun Test Necessario", noteTecniche:="ID TFS 8594")

            Riga_Text("Menu Visite (new!) - Dettaglio Rilievo",
                     "Tramite switch sul pannello dei Filtri sul Menu Visite, è possibile visualizzare una griglia con specificati Appezzamento, Indice e Valore del Rilievo associato a una visita")

            Riga_Bug("Impostazione filtro di ricerca",
                      "Se non è presente l'impostazione utente specifica il filtro di ricerca da usare (SUPERUSER_Mod_Filtro_Ricerca), si usa il filtrone precedente al filtrone NG")

            Riga_Text("Esportazione QDCA verso Agea",
                     "Valorizzato l'applicationTypeCode nelle chemicalFertilizations",
                        noteTest:="Nessun Test Necessario")

            Riga_Bug("Profilazione NG",
                      "Aggiusta il salvataggio di utenti di tipo azienda",
                      noteTest:="Per qualche ragione i dati passati alla funzione di salvataggio degli utenti di tipo
                      Azienda erano stati mischiati: una modifica fatta sul campo telefono della griglia si rifletteva
                      con la riscrittura del codice fiscale, l'email sembrava scambiata con la ragione sociale, etc.",
                      idPerforma:=33976, cliente:="Coprob")

            '==================================================================

            Riga_Data("12 Dicembre 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Audit Coprob", "23/04/2024")

            Riga_Requisiti("Migra",
                           "- Nuove colonne tabella Utenti_Token_JWT
                                 - Nuova colonna smtp_password_isEncrypted in tabella Configurazione_Servizi
                                 - Spostamento tabella MisuraXIndiciMaturita_Anagrafiche in db server",
                           Ver:="777")

            Riga_Requisiti("Configurazione_Siti",
                           "Inserite le chiavi 'password_smtp_isEncrypted', 'PasswordArteaWS_isEncrypted' e 'cr'", "149")

            Riga_Requisiti("Aggancio",
                           "Per viste compliance ISCC", "127")

            Riga_Requisiti("Gias_Configurazione_Siti",
                           "Aggiornamento valori colonna Visibilità in tabella Configurazione_Siti", "146")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")

            Riga_Bug("Sequence",
                     "Alla creazione delle Sequence startValue = 1",
                     noteTest:="")

            '==================================================================

            Riga_Data("06 Dicembre 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Audit Coprob", "23/04/2024")

            Riga_Requisiti("Migra",
                           "- Nuove colonne tabella Utenti_Token_JWT
                                 - Nuova colonna smtp_password_isEncrypted in tabella Configurazione_Servizi
                                 - Spostamento tabella MisuraXIndiciMaturita_Anagrafiche in db server",
                           Ver:="777")

            Riga_Requisiti("Configurazione_Siti",
                           "Inserite le chiavi 'password_smtp_isEncrypted', 'PasswordArteaWS_isEncrypted' e 'cr'", "149")

            Riga_Requisiti("Aggancio",
                           "Per viste compliance ISCC", "127")

            Riga_Requisiti("Gias_Configurazione_Siti",
                           "Aggiornamento valori colonna Visibilità in tabella Configurazione_Siti", "146")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")

            Riga_Bug("Analisi Terreno NG - D2G Demetra",
                     "Corretta scrittura Analisi Terreno Integrazione D2G",
                     noteTest:="Trovato internamente. 
                     Testare D2G e G2D delle Analisi del Terreno su ambiente di test Coldiretti. Verificare corretto passaggio tramite modulo 'Consulta Sincro Dati App'")

            Riga_Bug("Invio mail e import artea",
                        "Modificato il meccanismo che viene usato per evitare di effettuare il check di autenticazione su alcune chiamate",
                        noteTest:="Id Redmine 156602 e 156600 Fare le stesse prove")

            Riga_Bug("Ricerca Doc Contabili",
                        "Correzione regressione per lingua Francese che impediva l'utilizzo della pagina, introdotta nella versione del 22 Novembre " &
                        "con il fix della segnalazione 33590 relativa alle categorie 'Altri Beni Strumentali', 'Riga Descrizione Libera' e 'Servizi'",
                        noteTest:="Provare con un utente in lingua francese la ricerca dei doc contabili in modalità riga e provare la modifica di un qualsiasi DDT esistente")

            Riga_Bug("Ricerca Doc Contabili Conferimento",
                        "Correzione arrotondamento in calcolo peso netto a pagamento, per allinearlo alla pagina di modifica",
                        noteTest:="Provare ad inserire un conferimento impostando il peso in kg con decimali con degrado e verificare che il netto a pagamento " &
                        "abbia lo stesso valore sia in modifica, che nella ricerca che nella griglia di riepilogo righe del singolo conferimento",
                        noteTecniche:="Emerso da segnalazione testinterni Redmine 158625")

            Riga_Bug("GIS - Accesso dati mappe satellitari",
                     "Corretto controllo che verificava obbligatoriamente la presenza dell'impostazione 1012 sull'utente. Ma tale impostazione è valida solo per il vecchio GIS", "ConsorziAgrari",
                     33856,
                     "Creare un nuovo utente che abbia i permessi di utilizzo del GIS e visibilita sulla azienda ROSSI ALPI MATTEO, provare a visualizzare ad accedere al calendario dei dati delle mappe")

            Riga_Bug("Fatturazione Automatica DDT",
                        "Correzione fatturazione automatica DDT. In caso di cancellazione della fattura cancellava anche id ddt",
                        idPerforma:=32700,
                        noteTest:="Provare a schedulare fatturazione automatica ddt e verificare comportamento in caso di cancellazione di ddt (- >blocco) e fattura (-> cancellazione solo fattura)")

            Riga_Text("Demetra Raccolte Interscambio con GIAS",
                     "La raccolta fra D2G viene sempre passata come se fosse automatica, quindi senza indicazione puntuale sugli impianti della qta raccolta - fix rispetto al rilascio precedente in quanto la raccolta G2D può essere automatica o manuale",
                     "Coldiretti",
                     noteTest:="Creare una raccolta su Demetra e vedere che arrivi correttamente su GIAS (su demetra nasce automatica), creare due raccolte su GIAS, una automatica e una manuale e verificare che su Demetra arrivi come era su Gias")

            '==================================================================

            Riga_Data("29 Novembre 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Audit Coprob", "23/04/2024")

            Riga_Requisiti("Migra",
                           "- Nuove colonne tabella Utenti_Token_JWT
                                 - Nuova colonna smtp_password_isEncrypted in tabella Configurazione_Servizi
                                 - Spostamento tabella MisuraXIndiciMaturita_Anagrafiche in db server",
                           Ver:="777")

            Riga_Requisiti("Configurazione_Siti",
                           "Inserite le chiavi 'password_smtp_isEncrypted', 'PasswordArteaWS_isEncrypted' e 'cr'", "149")

            Riga_Requisiti("Aggancio",
                           "Per viste compliance ISCC", "127")

            Riga_Requisiti("Gias_Configurazione_Siti",
                           "Aggiornamento valori colonna Visibilità in tabella Configurazione_Siti", "146")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")

            Riga_Bug("Profilazione NG",
                     "Corregge la condizioni di caricamento della sezione blocchi agenda delle impostazioni utente",
                     noteTest:="Precedentemente la sezione blocchi agenda veniva nascosta unicamente se l'utente non
                     era in possesso del permesso di modifica sulle impostazioni blocchi agenda. Ora la sezione viene
                     nascosta se l'utente non è provvisto né di permesso in modifica dei blocchi agenda né di quello
                     sulle impostazioni avanzate.
                     Relativo alle segnalazioni sul documento TEST_GESTIONE_UTENTI_new, ticket #156649")

            Riga_Bug("Profilazione NG - Impostazione annata agraria",
                     "Corregge la valorizzazione dell'impostazione annata agraria con il valore salvato in precedenza",
                     noteTest:="Relativo al ticket #156624")

            Riga_Bug("Analisi Terreno NG",
                     "Corretta modifica analisi senza certificato e senza schema impostati",
                     noteTest:="Relativo al ticket #157388 (punto 3)")

            Riga_Text("Demetra Raccolte Interscambio con GIAS",
                     "La raccolta fra GIAS e Demetra viene sempre passata come se fosse automatica, quindi senza indicazione puntuale sugli impianti della qta raccolta",
                     "Coldiretti",
                     noteTest:="Creare una raccolta su Demetra e vedere che arrivi correttamente su GIAS (su demetra nasce automatica), creare due raccolte su GIAS, una automatica e una manuale e verificare che su Demetra arrivi sempre come automatica")

            Riga_Text("Demetra Concia del seme Interscambio con GIAS",
                     "E' stata sospesa la concia del seme nell'interscambio D2G e G2D",
                     "Coldiretti",
                     noteTest:="Creare una concia del seme su GIAS e verificare che non arrivi su Demetra, e viceversa")

            '==================================================================

            Riga_Data("22 Novembre 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Audit Coprob", "23/04/2024")

            Riga_Requisiti("Migra",
                           "- Nuove colonne tabella Utenti_Token_JWT
                                 - Nuova colonna smtp_password_isEncrypted in tabella Configurazione_Servizi
                                 - Spostamento tabella MisuraXIndiciMaturita_Anagrafiche in db server",
                           Ver:="777")

            Riga_Requisiti("Configurazione_Siti",
                           "Inserite le chiavi 'password_smtp_isEncrypted', 'PasswordArteaWS_isEncrypted' e 'cr'", "149")

            Riga_Requisiti("Aggancio",
                           "Per viste compliance ISCC", "127")

            Riga_Requisiti("Gias_Configurazione_Siti",
                           "Aggiornamento valori colonna Visibilità in tabella Configurazione_Siti", "146")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")

            Riga_Bug("Profilazione NG",
                     "Nasconde il permesso per manualistica e videocorsi dalla griglia di attribuzione permessi",
                     noteTest:="Precedentemente il permesso per Manualistica e Videocorsi compariva nella tabella di attribuzione dei permessi ma in alcuni ambienti
                     risultava impossibile abilitarlo. A seguito di un controllo è risultato che il permesso deriva dalla presenza del presenza del relativo pulsante
                     di menù ma, di fatto, è come se fosse sempre abilitato, decidendo quindi di nasconderlo dalla griglia.
                     Relativo alle segnalazioni sul documento TEST_GESTIONE_UTENTI_new, ticket #156649")

            Riga_Bug("Attribuzione visibilità NG",
                     "Corregge il check sulla visibilità degli utenti selezionati",
                     noteTest:="Precedentemente capitava che in certi casi, selezionando un semplice utente con il superuser compariva il messaggio:
                     'Impossibile visualizzare le aziende in visibilità: gli utenti hanno visibilità diversa'. Ciò era dovuto al fatto che per l'utente selezionato
                     fossero presenti due record in tabella con Id_Servizio e visibilità diversa. Il bug è stato corretto aggiungendo un filtro in modo da
                     controllare solo i valori riferiti all'Id_Servisio per GiasOnline (valore 5).
                     Relativo alle segnalazioni sul documento TEST_GESTIONE_UTENTI_new, ticket #156649")

            Riga_Bug("Leggi Rapporto Documenti",
                       "Corretto filtro nella query che ne impediva la parametrizzazione.",
                      noteTecniche:="riferimento alla mail 'ENI CIV: errore query LeggiRapportoSpecificoxDocumenti' 18/11/2024 13:05 sulle 
                      dimensioni del file di log di Eni Costa D'Avorio",
                      noteTest:="non testabile")

            Riga_Bug("Modifica Documenti (scadenziario)",
                     "Fix extrazione nome DBUtenti che impediva la corretta apertura in modifica di un documento",
                     "Coldiretti",
                     noteTest:="Ce ne siamo accorti internamente. 
                     Provare su un ambiente di test Coldiretti: creare un documento dal modulo del documentale e aprirlo in modifica")

            Riga_Bug("Concia del Seme NG",
                     "Ripristino aggancio Concia del Seme agli Impianti per errata archiviazione",
                     noteTest:="Registrare Concia del Seme su Impianti")

            Riga_Bug("Ricerca Documenti Contabili",
                     "Miglioramento in griglia di ricerca righe, ora la colonna 'Categoria prodotto' mostra anche le seguenti descrizioni: " &
                     "'Altri Beni Strumentali', 'Riga Descrizione Libera' e 'Servizi', prima per i prodotti di queste tre categorie la cella appariva vuota",
                     "Az. Agr. Alba di Viola Amedeo", 33590)

            Riga_Bug("Esportazione QDCA verso Agea",
                     "Sistemata esportazione QDCA di un'azienda che ha dei Fabbricati senza la via indicata in anagrafica")

            '==================================================================

            Riga_Data("18 Novembre 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Audit Coprob", "23/04/2024")

            Riga_Requisiti("Migra",
                           "- Nuove colonne tabella Utenti_Token_JWT
                                 - Nuova colonna smtp_password_isEncrypted in tabella Configurazione_Servizi
                                 - Spostamento tabella MisuraXIndiciMaturita_Anagrafiche in db server",
                           Ver:="777")

            Riga_Requisiti("Configurazione_Siti",
                           "Inserite le chiavi 'password_smtp_isEncrypted', 'PasswordArteaWS_isEncrypted' e 'cr'", "149")

            Riga_Requisiti("Aggancio",
                           "Per viste compliance ISCC", "127")

            Riga_Requisiti("Gias_Configurazione_Siti",
                           "Aggiornamento valori colonna Visibilità in tabella Configurazione_Siti", "146")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")

            Riga_Bug("Elenco Impianti da Catasto",
                     " Corretto bug che mostrava tutti gli impianti sulla particella anche se non erano presenti nell'anagrafica dell'azienda corrente (caso di particella 'condivisa') ",
                     "ABOCA",
                     33556,
                     "",
                     "Creare la stessa particella su 2 aziende diverse, creare per ogni azienda 1 impianto su quella particella, 
                     poi andare dalla griglia di anagrafica catasto, vedere gli impianti sulla particella in questione, NON deve comparire l'impianto dell'altra azienda ")

            Riga_Bug("Appezzamento Edit",
                     "Corretto controllo metodo di produzione che non considerava il vincolo.",
                     noteTest:="Da interfaccia provare a modificare ad impostare ultizzo terreno da integrtato a biologico (appezzamento) ed il relativo vincolo su esercizio (Bio); salvare e non deve dare errore. Fare anche il test con altre combinazioni (integrato\nessun vincolo) etc.. ")

            '==================================================================

            Riga_Data("15 Novembre 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Audit Coprob", "23/04/2024")

            Riga_Requisiti("Migra",
                           "- Nuove colonne tabella Utenti_Token_JWT
                                 - Nuova colonna smtp_password_isEncrypted in tabella Configurazione_Servizi
                                 - Spostamento tabella MisuraXIndiciMaturita_Anagrafiche in db server",
                           Ver:="777")

            Riga_Requisiti("Configurazione_Siti",
                           "Inserite le chiavi 'password_smtp_isEncrypted', 'PasswordArteaWS_isEncrypted' e 'cr'", "149")

            Riga_Requisiti("Aggancio",
                           "Per viste compliance ISCC", "127")

            Riga_Requisiti("Gias_Configurazione_Siti",
                           "Aggiornamento valori colonna Visibilità in tabella Configurazione_Siti", "146")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")

            Riga_Bug("Profilazione NG",
                     "Corretto l'inizializzazione dei layers GIS dall'attribuzione dei permessi",
                     noteTest:="Dal documento di test, aggiusta i casi di caricamento infinito in attribuzione dei permessi.
                     Da testare su profili provvisti di permesso Cartografia Aziendale attivo (codice permesso 12)",
                     noteTecniche:="Aggiunta clausola NOLOCK in lettura degli utenti su cui eseguire l'operazione.
                     L'operazione era eseguita in una transizione sul db utenti quindi senza la clausola si verificavano casi di deadlock.")

            Riga_Text("Primo rilascio Analisi Terreno (new!)",
                      "Primo Rilascio Analisi Terreno (new!) griglia di ricerca + modifica e creazione
                      Differenze rispetto al vecchio modulo:
                      - Sul nuovo modulo NON è possibile associare un'analisi ad entità differenti (ex. impresa e centro).
                      - L'associazione ad entità dello stesso tipo è stata migliorata grazie all'utilizzo delle griglie. (ex. associare l'analisi a 100 particelle)
                      - Sul nuovo modulo è possibile inserire un solo campione
                        Tuttavia, il pregresso è comunque gestito e verranno mostrati tutti i campioni associati ad analisi create con il vecchio con la possibilità di cancellarli o editarli
	                  - La griglia delle analisi sarà filtrata in base all'azienda corrente (in alto a dx)
	                  - Dalla griglia sarà possibile inserire e visualizzare i documenti, così come in modifica dell'analisi
	                  - Implementata funzione di cancellazione multipla analisi
	                  - Dalla griglia sarà possibile editare in linea alcuni valori della testata (descrizioni, note, validità...)
	                  - All'interno della modifica, gli schemi dei laboratori vengono filtrati per tipo analisi (uno schema delle analisi fitofarmaci non sarà visibile)
	                  - Quando si seleziona uno schema, la griglia dei parametri sarà espansa con tutti i parametri gestiti già disposti in linea. L'utente può inserire il valore nel parametro che gli interessa, ma non può eliminare righe. Per cancellare un valore, basta svuotare la colonna relativa
	                    Se non si selezionano schemi, comparirà un pulsante per aggiungere manualmente le righe di parametri + possibilità di eliminarli.",
                     noteTest:="Creare analisi del terreno dal vecchio modulo e aprirle con il nuovo. 
                     Creare analisi del terreno con il nuovo modulo e verificare che tutti i valori siano gestiti correttamente. 
                     Aprire analisi del terreno create con il vecchio modulo, aventi diversi campioni associati e verificare che tutti i valori siano gestiti correttamente. 
                     Associare e visualizzare documenti dalla griglia e da interfaccia di modifica
                     Verificare che la modifica di un analisi associata ad un piano di concimazione venga bloccata",
                     noteTecniche:="Sono state apportate modifiche a modelli e funzioni che hanno impatto anche sull'interscambio con Demetra. TESTARE D2G e G2D Analisi.")

            Riga_Text("Sincro APP",
                      "Nuova lettura aziende in base a un dato cartografico inviato dall'app",
                      noteTest:="Su APP dalla sezione Sincronizza Dati premere sul nuovo pulsante Da Mappa, 
                      seguire le istruzioni presentate dall'app e premere sul pulsante di refresh per cercare le aziende, 
                      verificare che le aziende da restituire all'app vengano filtrate correttamente in base al dato cartografico ricevuto")

            Riga_Text("Ricerca documenti contabili",
                     "Aggiunta alle query di ricerca OPTION (USE HINT ('FORCE_LEGACY_CARDINALITY_ESTIMATION')) per avere buone performance anche in caso di livello retrocompatibilità Sql 2019",
                     "Eni Costa Avorio",
                     noteTest:="Verificare che le ricerche vengano eseguite correttamente")

            Riga_Bug("Coprob Checklist Trasporti",
                     "Corretto filtro ricerca documenti.",
                     noteTest:="Verificare che id documenti vengano filtrati correttamente in base alla variazione di ditta trasporto/conducente/targa")

            Riga_Bug("Export QdC to Agea",
                     "Magazzini in export QdCToAgea, corretta query.",
                     cliente:="Coldiretti",
                     noteTest:="Nessun test - ancora WIP")


            '==================================================================

            Riga_Data("12 Novembre 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Audit Coprob", "23/04/2024")

            Riga_Requisiti("Migra",
                           "- Nuove colonne tabella Utenti_Token_JWT
                                 - Nuova colonna smtp_password_isEncrypted in tabella Configurazione_Servizi
                                 - Spostamento tabella MisuraXIndiciMaturita_Anagrafiche in db server",
                           Ver:="777")

            Riga_Requisiti("Configurazione_Siti",
                           "Inserite le chiavi 'password_smtp_isEncrypted', 'PasswordArteaWS_isEncrypted' e 'cr'", "149")

            Riga_Requisiti("Aggancio",
                           "Per viste compliance ISCC", "127")

            Riga_Requisiti("Gias_Configurazione_Siti",
                           "Aggiornamento valori colonna Visibilità in tabella Configurazione_Siti", "146")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")

            Riga_Bug("Anagrafiche",
                     "Corretto salvataggio validità codici esercizio per anagrafiche angular")

            '==================================================================

            Riga_Data("11 Novembre 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Audit Coprob", "23/04/2024")

            Riga_Requisiti("Migra",
                           "- Nuove colonne tabella Utenti_Token_JWT
                                 - Nuova colonna smtp_password_isEncrypted in tabella Configurazione_Servizi
                                 - Spostamento tabella MisuraXIndiciMaturita_Anagrafiche in db server",
                           Ver:="777")

            Riga_Requisiti("Configurazione_Siti",
                           "Inserite le chiavi 'password_smtp_isEncrypted', 'PasswordArteaWS_isEncrypted' e 'cr'", "149")

            Riga_Requisiti("Aggancio",
                           "Per viste compliance ISCC", "127")

            Riga_Requisiti("Gias_Configurazione_Siti",
                           "Aggiornamento valori colonna Visibilità in tabella Configurazione_Siti", "146")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")

            Riga_Bug("Profilazione NG",
                      "- Modifica gestione errori in creazione e modifica utenti per mostrare il messaggio di errore fuori anche con la chiave RestituisciEccezioniGeneriche abilitata
                      - Aggiorna la funzione di caricamento degli utenti per carcare di migliorarne le tempistiche.",
                      noteTecniche:="Modifica la query di lettura degli utenti applicando una clausola GROUP BY invece che DISTINCT nella lettura di Utenti_Permessi.
                      Esegue la verifica di accettazione del GDPR direttamente con la lettura degli utenti da caricare.")

            '==================================================================

            Riga_Data("08 Novembre 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Audit Coprob", "23/04/2024")

            Riga_Requisiti("Migra",
                           "- Nuove colonne tabella Utenti_Token_JWT
                                 - Nuova colonna smtp_password_isEncrypted in tabella Configurazione_Servizi
                                 - Spostamento tabella MisuraXIndiciMaturita_Anagrafiche in db server",
                           Ver:="777")

            Riga_Requisiti("Configurazione_Siti",
                           "Inserite le chiavi 'password_smtp_isEncrypted', 'PasswordArteaWS_isEncrypted' e 'cr'", "149")

            Riga_Requisiti("Aggancio",
                           "Per viste compliance ISCC", "127")

            Riga_Requisiti("Gias_Configurazione_Siti",
                           "Aggiornamento valori colonna Visibilità in tabella Configurazione_Siti", "146")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")

            Riga_Text("Autenticazione",
                      "-Prima di ogni chiamata ai web method è stato introdotto un controllo di sicurezza (global asax)
                            -I web method che accedono alla tabella Utenti_Token_JWT sono stati modificati per gestire la nuova logica di autenticazione
                            -Le classi che leggono e scrivono nella tabella Utenti_Token_JWT sono state modificate per gestire le nuove colonne e la nuova logica di autenticazione
                            -Eliminazione token scaduti",
                            noteTest:="Effettuare login più altre operazioni (possibilmente con lo stesso utente da due browser o pc diversi contemporaneamente) e verificare che tutto funzioni correttamente",
                            noteTecniche:="Il controllo di sicurezza viene effettuato solo quando il valore della chiave ControlloAutenticazioneConAuthCookie nei appsettings è uguale a true")

            Riga_Text("Refresh token",
                      "Refresh degli auth cookie usando il refresh token")

            Riga_Bug("Raccolta New",
                      "Sistemato salvataggio raccolta",
                      idPerforma:=32706,
                      cliente:="Regione Umbria",
                      noteTest:="Registrare una nuova Raccolta selezionando tutti gli impianti (possibilmente più di 300) in una azienda dove sono stati registrati anche dei trattamenti su quegli impianti ed in cui scatta il controllo di Data Carenza." & vbCrLf &
                        "Verificare se va a buon fine il salvataggio.")

            Riga_Text("Codifica e decodifica password_smtp e PasswordArteaWS",
                      "-Introdotti metodi per la codifica e decodifica usando la classe AES
                            -Introdotta chiave cr2 in web.config o app.config")

            Riga_Text("Ricerca Prodotti da APP",
                      "Forzato caricamento categoria Formulati classificati come Trappole se cerco come categoria prodotto le Trappole Commerciali",
                        noteTest:="Nessun test necessario")

            Riga_Text("Export QdC to Agea",
                      "Aggiunti nel tracciato json sezione magazzini, macchine e operatori",
                      cliente:="Coldiretti",
                      noteTest:="Nessun test - ancora WIP")

            Riga_Text("Rilievi NG",
                      "Aggiunge funzioni di lettura della tabella MisuraxIndiciMaturita_Anagrafiche per
                      permettere l'utilizzo di valori selezionabili in un menù a discesa come per i rilievi avversità",
                      noteTest:="Al momento non esiste interfaccia per popolare la tabella MisuraxIndiciMaturita_Anagrafiche (prossimo sviluppo)")

            Riga_Text("Security - Report",
                      "Rese parametriche alcune query per impedire sql injection")

            Riga_Text("Security - Query",
                      "Parametrizzate massivamente molte query in filtri aggiuntivi, order by e clausole IN per impedire sql injection")

            Riga_Text("Export QdC to Agea",
                      "Cambiato formato date per ChemicalFertilization, FarmingEvent, GrowthStage, Irrigation, OrganicFertilization, PhytochemicalTreatment",
                      cliente:="Coldiretti",
                      noteTest:="Nessun test")

            Riga_Text("Lettura indici maturita APP",
                      "Sostituito metodo Leggi con GetValuesFor per MisuraxIndiciMaturita_Anagrafiche",
                      cliente:="Orogel")

            Riga_Text("Gestione sequence default",
              "Gestione sequence per progressivi chiavi tabelle di default attive per tutti. 
                Disattivabili esclusivamente impostando a False la chiave Allow_Sql_Sequence nell'appsettings")

            Riga_Text("Porting Operazione TRATTAMENTI POST RACCOLTA sul QdCA Angular",
                      "Gestione Operazione TRATTAMENTI POST RACCOLTA sul QdCA (new!) (per ora solo come operazione di agenda)",
                      noteTecniche:="Tutte le operazioni create con il vecchio possono essere aperte con il nuovo AD ECCEZIONE di quelle che utilizzano SEMILAVORATI, tipo di prodotto non gestito su tutto il nuovo QdCA",
                      noteTest:="1) Fare una raccolta con carico di magazzino sul QdcA (new!)
                        - Raccolte con varie casistiche di generazione lotto, con particolare attenzione alla voce 'Esercizio Corrente'
                      Trattare il prodotto raccolto con l'operazione TRATTAMENTI POST RACCOLTA NG
                      Es: di Specie con formulato per il post raccolta: ORZO - prodotto ACTELLIC 2P (2518)
                      2) Provare ad aprire un post raccolta creato con l'interfaccia vecchia che abbia più avversità --> su angular ne verrà gestita solo 1 (randomica),
                      3) Provare ad aprire un post raccolta creato con l'interfaccia vecchia che utilizzi i semilavorati --> su angular non verrà aperto, ma verrà fatto il redirect alla vecchia interfaccia
                      NB: Rispetto alla vecchia, i formulati vengono caricati tenendo conto della finalità del prodotto (inseribile da Anagrafica Prodotti). 
                      Non sono gestiti i controlli di conformità sul numero max di interventi consentiti 
                      Non sono gestiti i controlli di conformità a posteriori
                      Gestiti i controlli su dose max/min del prodotto (provare ORZO - prodotto ACTELLIC 2P (2518))")

            '==================================================================

            Riga_Data("30 Ottobre 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Audit Coprob", "23/04/2024")

            Riga_Requisiti("Migra",
                           "- Nuovo permesso per elaborazione massiva compliance iscc e nuova tabella per elaborazioni asincrone
                           - Nuova tabella per la gestione dei permessi d'installazione",
                           Ver:="774")

            Riga_Requisiti("Aggancio",
                           "Per viste compliance ISCC", "127")

            Riga_Requisiti("Gias_Configurazione_Siti",
                           "Aggiornamento valori colonna Visibilità in tabella Configurazione_Siti", "146")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")

            Riga_Bug("Profilazione NG",
                      "- Fix attribuzione permessi profilo")

            '==================================================================

            Riga_Data("29 Ottobre 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Audit Coprob", "23/04/2024")

            Riga_Requisiti("Migra",
                           "- Nuovo permesso per elaborazione massiva compliance iscc e nuova tabella per elaborazioni asincrone
                           - Nuova tabella per la gestione dei permessi d'installazione",
                           Ver:="774")

            Riga_Requisiti("Aggancio",
                           "Per viste compliance ISCC", "127")

            Riga_Requisiti("Gias_Configurazione_Siti",
                           "Aggiornamento valori colonna Visibilità in tabella Configurazione_Siti", "146")

            Riga_Requisiti("AgronicaCoreAPI",
                           "Compliance ISCC", "13/09/2024")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")

            Riga_Bug("Profilazione NG",
                      "- Fix assegnazione visibilità totale
                      - Fix attribuzione permessi con grado di attivazione maggiore rispetto a quello in installazione:
                          Se ho un permesso attivo in installazione come 'sola lettura' non potrò più attivarlo su un profilo come permesso 'lettura + scrittura'
                      - Rende impossibile creare nuovi utenti con la stessa email
                      - Aggiunge end-point per la funzione di conteggio utenti")

            Riga_Text("Ricerca Prodotti [i18n]",
                     "Aggiunte traduzioni per voce 'Cod. Articolo'",
                     "ENI CIV")

            Riga_Bug("GISWS.asmx",
                      "- Fix sottomissione massiva compliance ISCC con stato iniziale corretto.")

            Riga_Bug("Anagrafica Esercizi",
                      "Corretto controllo vincolo Bio per impianti con Terreno Nudo",
                      "Aboca")

            Riga_Text("Anagrafica Ng Esercizi",
                      "Riportato meccanismo di chiusura e replica esercizio",
                      cliente:="Aboca")

            '==================================================================

            Riga_Data("22 Ottobre 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Audit Coprob", "23/04/2024")

            Riga_Requisiti("Migra",
                           "- Nuovo permesso per elaborazione massiva compliance iscc e nuova tabella per elaborazioni asincrone
                           - Nuova tabella per la gestione dei permessi d'installazione",
                           Ver:="774")

            Riga_Requisiti("Aggancio",
                           "Per viste compliance ISCC", "127")

            Riga_Requisiti("Gias_Configurazione_Siti",
                           "Aggiornamento valori colonna Visibilità in tabella Configurazione_Siti", "146")

            Riga_Requisiti("AgronicaCoreAPI",
                           "Compliance ISCC", "13/09/2024")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")

            Riga_Bug("Semina con Aggiornamento Anagrafica NG",
                      "Fix visualizzazione errori per blocco modifica anagrafica",
                      "Eurovo", 33014)

            Riga_Bug("Anagrafica impianto",
                      "Controllo sul modifica specie/destinazione d'uso su impianti (con destinazione d'uso) 
                      dove sono state registrate operazioni singole (cioè operazioni che non coinvolto altri impianti se non quello in modifica).",
                      "ABOCA", noteTest:="è possibile la modifica di specie/destinazione d'uso su impianti su cui sono state registrate operazioni che non coinvolgono altri impianti. Altrimenti la modifica deve essere disabilitata ")

            Riga_Bug("Compliance ISCC",
                      "disattivato invio a ElastiSearch per i log di controllo causano sovraccarico datareader (ambiente ENI non ha ES configurato)",
                      "ENI", noteTest:="La sottomissione puntuale di controllo ISCC non deve andare in errore mai")

            Riga_Bug("LeggiInterviste",
                      "Sistemato bug id_area non passato come numerico")

            Riga_Bug("Rilievi NG",
                     "Modifica il caricamento delle erbe infestanti per permettere di selezionare anche infestanti singole inveche che unicamente i gruppi",
                     cliente:="Orogel")

            Riga_Text("Interscambio Demetra:",
                     "- Import Lavoratori QdC reso configurabile filtro sui rapporti contabili ammessi ",
                     "Coldiretti", noteTest:="nessun test")

            '==================================================================

            Riga_Data("21 Ottobre 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Audit Coprob", "23/04/2024")

            Riga_Requisiti("Migra",
                           "- Nuovo permesso per elaborazione massiva compliance iscc e nuova tabella per elaborazioni asincrone
                           - Nuova tabella per la gestione dei permessi d'installazione",
                           Ver:="774")

            Riga_Requisiti("Aggancio",
                           "Per viste compliance ISCC", "127")

            Riga_Requisiti("Gias_Configurazione_Siti",
                           "Aggiornamento valori colonna Visibilità in tabella Configurazione_Siti", "146")

            Riga_Requisiti("AgronicaCoreAPI",
                           "Compliance ISCC", "13/09/2024")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")

            Riga_Text("Profilazione NG",
                      "Aggiunge chiamata per controllare che delle imprese abbiano figli nella gerarchia della visibilità",
                      noteTest:="Usata per decidere se mostrare il nuovo alert in assegnazione della visibilità")

            Riga_Bug("Profilazione NG",
                      "- Aggiusta la creazione della query per l'assegnazione della visibilità a più utenti
                      - Aggiusta il caricamento delle impostazioni nella sezione Agenda Controlli/Blocchi
                      - Aggiusta creazione di utenti senza dati spid nei db in cui è abilitata la gestione dello spid")

            Riga_Bug("App Zootecnia",
                      "Corretta lettura PDC da restituire all'APP")

            Riga_Bug("Compliance ISCC",
                      "Eliminata apertura connessione\transazione per sovraccarico datareader")

            Riga_Bug("Raccolta NG",
                      "Integra la correzione al bug per cui era impossibile preporre la chiusura di
                      un esercizio già chiuso alla data corrente tramite un'operazione di raccolta con chiusura esercizi",
                      idPerforma:=32424, cliente:="Genagricola")

            Riga_Bug("GIS Ng",
                     "Corretti salvataggio e lettura poligoni impianti e appezzamenti",
                     "Aboca")

            Riga_Text("Aggiunta Impianti ad Appezzamento Esistente",
                     "Corretto controllo su Operazioni Agenda / CdG quando si aggiunge un impianto ad un appezzamento gà esistente",
                     "Genagricola", 32995)

            Riga_Text("Modifica Raccolta",
                      "Corretto controllo su scarichi collegati a Raccolta mostrato in modifica/cancellazione dell'operazione",
                      "Genagricola", 32998,
                      noteTecniche:="Aggiunto filtro per verificare solo scarichi con Data_Movimento >= data della Raccolta, così da escludere scarichi di anni prima non coerenti a prescindere.
                      Rimasto aperto il tema di filtro su piva")

            Riga_Text("Profilazione Imprese",
                      "Porting Profilazione Imprese su NG (con nuovo permesso dedicato)",
                      noteTest:="Porting completo e testabile, il 'default regolamento e disciplinare' è stato spostato sull'anagrafica impresa, per la funzionalità 'cambia impresa' utilizzare la selezione imprese dell'header")

            Riga_Bug("Bilancio di massa",
                     "Corretta visualizzazione per parametri qualitativi non settati",
                     "ENI Costa Avorio")

            Riga_Text("Widget Multi Azienda",
                      "Modificati titoli widget:
                      'Aziende con Movimenti recenti' (old: 'Aziende Movimentate')
                      'Aziende con Movimenti da Inizio Campagna' (old: 'Aziende Inizio Campagna')")

            Riga_Bug("Controlli Aggiunta Impianto",
                     "Quando si aggiungono nuovi impianti non ci devono essere blocchi di nessun tipo.
                     Quando si modificano impianti/esercizi, devono essere fatti tutti i controlli del caso",
                     "Noceto", 33067,
                     noteTest:="Requisiti per i test:
                     - Due impianti della stessa specie
                     - Una qualsiasi operazione di agenda registrata su entrambi
                     Testare tutte le seguenti casistiche BLOCCANTI, modificando i dati già esistenti:
                     - Cambio specie in uno dei due impianti coinvolti nell'operazione
                     - Chiusura impianto o esercizio antecedente all'operazione in uno dei due impianti coinvolti nell'operazione (provare sia impianto+esercizio che esercizio)
                     - Apertura impianto o esercizio successivo all'operazione in uno dei due impianti coinvolti nell'operazione (provare sia impianto+esercizio che esercizio)    
                     Testare tutte le seguenti casistiche NON BLOCCANTI, modificando i dati già esistenti:
                     - Chiusura esercizio antecedente all'operazione e creazione di un nuovo esercizio che vada a 'coprire' la data dell'operazione
                     NB: creando un esercizio che NON 'copre' la data dell'operazione, deve dare errore!
                        es: Operazione il 20/10/2024;
                            OK --> Esercizio uno validità fine 19/10/2024; Esercizio due (stesso impianto) validità inizio 20/10/2024
                            KO --> Esercizio uno validità fine 10/10/2024; Esercizio due (stesso impianto) validità inizio 01/11/2024
                     Altri test NON BLOCCANTI:
                     - Aggiunta di un nuovo impianto della stessa specie
                     - Aggiunta di un nuovo impianto con specie diversa
                     - Aggiunta nuovo esercizio ")

            Riga_Text("Lettura Contatti LeggiRapportoDocumenti",
                      "Aggiunta estrazione colonna 'Compliance_ISCC' da tabella Imprese", noteTest:="Nessun Test")

            Riga_Text("Anagrafica impianti",
                      "Controllo per impianti con operazioni senza altri impianti per cambio specie",
                      "Aboca")

            Riga_Text("Controllo sottoscrizione servizio bluarancio per qdca",
                      "Aggiunto controllo su inserimento nuova operazione agenda che l'azienda su cui si opera abbia sottoscritto il servizio qdc impresa verde o a cura azienda",
                      "Coldiretti")

            '==================================================================

            Riga_Data("11 Ottobre 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Audit Coprob", "23/04/2024")

            Riga_Requisiti("Migra",
                           "- Nuovo permesso per elaborazione massiva compliance iscc e nuova tabella per elaborazioni asincrone
                           - Nuova tabella per la gestione dei permessi d'installazione",
                           Ver:="774")

            Riga_Requisiti("Aggancio",
                           "Per viste compliance ISCC", "127")

            Riga_Requisiti("Gias_Configurazione_Siti",
                           "Aggiornamento valori colonna Visibilità in tabella Configurazione_Siti", "146")

            Riga_Requisiti("AgronicaCoreAPI",
                           "Compliance ISCC", "13/09/2024")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")

            Riga_Text("Sync APP InfestantiAttive",
                      "Aggiunta nuova tabella nei dati comuni sincronizzati nell'app")

            Riga_Text("Profilazione NG",
                      "Aggiunto permesso per la modifica dei dati dell'utente corrente" &
                      "- Aggiunge gestione accesso spid nella griglia utenti della nuova profilazione
                      - Aggiunge controlli/avvisi su email utente: email deve essere obbligatoria e univoca per i nuovi utenti, per quelli già esistenti o con accesso spid attivo vengono mostrati solo degli avvisi
                      - Modifica query per il caricamento dei permessi assegnabili ai profili
                      - Aggiunge funzione per ottenere gli utenti collegati a un profilo, usata all'assegnazione di un permesso a un profilo per visualizzare il numero di utenti su cui si ripercuote la modifica",
                      noteTecniche:="necessaria chiave paginaIndex_LoginSPID attiva")

            Riga_Text("Sync APP GruppoAvversitaAttive",
                      "Aggiunta nuova tabella nei dati comuni sincronizzati nell'app")

            Riga_Text("Security - XSS",
                      "Aggiunto attributo 'class' tra quelli consentiti nelle chiamate ai web method")

            Riga_Text("Profilazione NG",
                      "Controlla la presenza del permesso gestione impostazioni agenda blocchi al caricamento delle impostazioni",
                      noteTest:="Se un utente non ha abilitato il permesso 247 (Impostazione Utenti Blocchi Agenda) non sarà più in
                      grado di vedere e modificare la relativa sezione nelle impostazioni utente")

            Riga_Text("IsAlive",
                      "URL HubAgea per IsAlive letto da super_server invece che da server",
                      cliente:="Coldiretti",
                      noteTest:="Nessun test")

            Riga_Text("GIS - Compliance ISCC",
                      "Modificata chiamata Sottomissione richieste per gestire o la singola azienda o un elenco di aziende specifico oppure sottomettere una richiesta massiva per tutte le aziende in visibilità all'utente.",
                      cliente:="ENI",
                      noteTecniche:="Il flusso è accoppiato al Task GSB 164 per elaborare la richiesta massiva sottomessa, che a sua volta sottomette le richieste puntuali di elaborazione algoritmi gis per vengono evase dal task GSB 135",
                      noteTest:="L'utente deve avere il permesso 568 (CalcoloMassivo_Compliance_ISCC_AziendeInVisibilita), andare nel GIS ed aprire 'Configurazione Algoritmi' -> scheda 'CheckList' (non usare il super user o verrà fatta un richiesta per tutte le aziende di ENI)
                      Click sul pulsante 'Sottometti tutte le aziende in visibilità'. Attendere le elaborazioni dei due GSB e controllare l'esito sul GIS (o nel filtrone) per una o più aziende")

            Riga_Text("Bilancio di Massa",
                      "Aggiunte classi Bilancio per calcolo del bilancio di massa")

            Riga_Text("Ricerca Prodotti [i18n]",
                      "Gestita traduzione per dicitura 'Cod. Articolo' eventualmente mostrata nei risultati di ricerca", "ENI CIV")

            Riga_Text("Visite NG",
                      "Aggiunge i nuovi rilievi alla lista di lavCod gestibili da visita")

            Riga_Text("Profilazione NG",
                      "Aggiunge la pagina di gestione dei permesse attivi sull'installazione corrente",
                      noteTest:="I permessi d'installazione equivalgono ai permessi attivi sul cliente SuperUser. Se un permesso non è attivo a livello d'installazione non sarà possibile attribuirlo ad alcun profilo utente.")

            Riga_Text("Profilazione Imprese",
                      "Porting Profilazione Imprese su NG (con nuovo permesso dedicato)",
                      noteTest:="Porting completo e testabile, il 'default regolamento e disciplinare' è stato spostato sull'anagrafica impresa, per la funzionalità 'cambia impresa' utilizzare la selezione imprese dell'header")

            Riga_Text("QdC NG",
                      "Gestisce l'operazione di abbattimento e le sue opzioni di chiusura")

            '==================================================================

            Riga_Data("09 Ottobre 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Audit Coprob", "23/04/2024")

            Riga_Requisiti("Migra",
                           "1) Per tabelle compliance ISCC, nuovo permesso gestione impostazioni imprese
                                  2) Per l'inserimento di una nuova colonna nella tabella Configurazione_Siti", "772")

            Riga_Requisiti("Aggancio",
                           "Per viste compliance ISCC", "127")

            Riga_Requisiti("Gias_Configurazione_Siti",
                           "Aggiornamento valori colonna Visibilità in tabella Configurazione_Siti", "146")

            Riga_Requisiti("AgronicaCoreAPI",
                           "Compliance ISCC", "13/09/2024")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")

            Riga_Text("Anagrafica Impianti",
                      "Aggiunto generazione nuovo codice impianto in salvataggio impianti, se nuovo impianto e codice impianto non valorizzato.",
                      noteTest:="La generazione di un nuovo codice impianto dipende da impostazioni a db")

            Riga_Text("Anagrafica Esercizi",
                      "Aggiunto WS per generazione descrizioni esercizi")

            '================================

            Riga_Data("08 Ottobre 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Audit Coprob", "23/04/2024")

            Riga_Requisiti("Migra",
                           "1) Per tabelle compliance ISCC, nuovo permesso gestione impostazioni imprese
                                  2) Per l'inserimento di una nuova colonna nella tabella Configurazione_Siti", "772")

            Riga_Requisiti("Aggancio",
                           "Per viste compliance ISCC", "127")

            Riga_Requisiti("Gias_Configurazione_Siti",
                           "Aggiornamento valori colonna Visibilità in tabella Configurazione_Siti", "146")

            Riga_Requisiti("AgronicaCoreAPI",
                           "Compliance ISCC", "13/09/2024")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")

            Riga_Bug("Profilazione NG",
                     "Aggiorna le funzioni di attribuzione dei permessi a un profilo e associazione degli utenti a un profilo")

            Riga_Text("Anagrafica Impianti",
                      "Aggiunto web service per lettura nuovo codice impianto. Aggiornata lettura impianti per estrazione dato algoritmo codifica e superficie poligono associato.")

            '================================

            Riga_Data("04 Ottobre 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Audit Coprob", "23/04/2024")

            Riga_Requisiti("Migra",
                           "1) Per tabelle compliance ISCC, nuovo permesso gestione impostazioni imprese
                                  2) Per l'inserimento di una nuova colonna nella tabella Configurazione_Siti", "772")

            Riga_Requisiti("Aggancio",
                           "Per viste compliance ISCC", "127")

            Riga_Requisiti("Gias_Configurazione_Siti",
                           "Aggiornamento valori colonna Visibilità in tabella Configurazione_Siti", "146")

            Riga_Requisiti("AgronicaCoreAPI",
                           "Compliance ISCC", "13/09/2024")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")

            Riga_Text("Ricerca Documenti Contabili",
                     "Miglioramento query del metodo Cerca_Documenti (CoreContabDAL): aggiunte parti di tabelle solo quando necessario (quando presenti o meno destinatari, vettori, cessionari, agenti o capi area)")

            '================================

            Riga_Data("03 Ottobre 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Audit Coprob", "23/04/2024")

            Riga_Requisiti("Migra",
                           "1) Per tabelle compliance ISCC, nuovo permesso gestione impostazioni imprese
                                  2) Per l'inserimento di una nuova colonna nella tabella Configurazione_Siti", "772")

            Riga_Requisiti("Aggancio",
                           "Per viste compliance ISCC", "127")

            Riga_Requisiti("Gias_Configurazione_Siti",
                           "Aggiornamento valori colonna Visibilità in tabella Configurazione_Siti", "146")

            Riga_Requisiti("AgronicaCoreAPI",
                           "Compliance ISCC", "13/09/2024")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")

            Riga_Bug("Passaggio Sito Audit",
                     "Valorizzazione corretta del codice fiscale utente (il campo veniva valorizzato con username)",
                     idPerforma:=32658, cliente:="Coprob")

            '================================

            Riga_Data("30 Settembre 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Audit Coprob", "23/04/2024")

            Riga_Requisiti("Migra",
                           "1) Per tabelle compliance ISCC, nuovo permesso gestione impostazioni imprese
                                  2) Per l'inserimento di una nuova colonna nella tabella Configurazione_Siti", "772")

            Riga_Requisiti("Aggancio",
                           "Per viste compliance ISCC", "127")

            Riga_Requisiti("Gias_Configurazione_Siti",
                           "Aggiornamento valori colonna Visibilità in tabella Configurazione_Siti", "146")

            Riga_Requisiti("AgronicaCoreAPI",
                           "Compliance ISCC", "13/09/2024")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")

            Riga_Bug("Nuovo Quaderno di Campagna",
                     "Corretto salvataggio delle Note in una Operazione di Campagna effettuata su impianti con una destinazione d'uso (ES: USO AGRICOLO - DA DEFINIRE)")

            Riga_Bug("Profilazione NG",
                     "- Aggiunge la possibilità di copiare le impostazini del profilo associato direttamente in creazione di un nuovo utente
                     - Aggiunge la possibilità di copiare anche le impostazioni utente alla copia di una tipologia utente")

            '================================

            Riga_Data("25 Settembre 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Audit Coprob", "23/04/2024")

            Riga_Requisiti("Migra",
                           "1) Per tabelle compliance ISCC, nuovo permesso gestione impostazioni imprese
                                  2) Per l'inserimento di una nuova colonna nella tabella Configurazione_Siti", "772")

            Riga_Requisiti("Aggancio",
                           "Per viste compliance ISCC", "127")

            Riga_Requisiti("Gias_Configurazione_Siti",
                           "Aggiornamento valori colonna Visibilità in tabella Configurazione_Siti", "146")

            Riga_Requisiti("AgronicaCoreAPI",
                           "Compliance ISCC", "13/09/2024")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")

            Riga_Bug("Raccolta NG",
                     "Corretto il bug per cui in chiusura di un esercizio veniva chiuso sempre il primo esercizio
                     associato all'impianto, anche se già chiuso in altra precedente", idPerforma:=32424, cliente:="Genagricola")

            Riga_Bug("Profilazione NG - visibilità aziendale",
                     "Corretta l'assegnazione in visibilità di imprese",
                     noteTecniche:="Il bug si presentava cercando di assegnare in visibilità un'impresa la cui piva ha un formato diverso da quello classico a 11 caratteri",
                     idPerforma:=32469, cliente:="CoProB")

            Riga_Bug("Profilazione NG",
                     "Aggiusta il caricamento dei permessi nella griglia di riepilogo sui profili per mostrare
                     correttamente il livello di permesso attribuito (colonne 'Permesso Lettura' e 'Permesso Scrittura')")

            '================================

            Riga_Data("20 Settembre 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Audit Coprob", "23/04/2024")

            Riga_Requisiti("Migra",
                           "1) Per tabelle compliance ISCC, nuovo permesso gestione impostazioni imprese
                                  2) Per l'inserimento di una nuova colonna nella tabella Configurazione_Siti", "772")

            Riga_Requisiti("Aggancio",
                           "Per viste compliance ISCC", "127")

            Riga_Requisiti("Gias_Configurazione_Siti",
                           "Aggiornamento valori colonna Visibilità in tabella Configurazione_Siti", "146")

            Riga_Requisiti("AgronicaCoreAPI",
                           "Compliance ISCC", "13/09/2024")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")

            Riga_Bug("Appezzamento Scrivi",
                     "Corrette funzioni di controllo in scrivi appezzamento")

            Riga_Text("Risposta WebService",
                     " - Gestione della RispostaStandard.Errore con errore generico 'Error' se chiave appsettings: RestituisciEccezioniGeneriche = True ")

            Riga_Text("Lettura contatti per Doc Contabili",
                     " - Velocizzata la lettura includendo nella query le sole colonne utilizzate successivamente 
                       -  Inserito filtro per Partita Iva")

            '================================

            Riga_Data("19 Settembre 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Audit Coprob", "23/04/2024")

            Riga_Requisiti("Migra",
                           "1) Per tabelle compliance ISCC, nuovo permesso gestione impostazioni imprese
                                  2) Per l'inserimento di una nuova colonna nella tabella Configurazione_Siti", "772")

            Riga_Requisiti("Aggancio",
                           "Per viste compliance ISCC", "127")

            Riga_Requisiti("Gias_Configurazione_Siti",
                           "Aggiornamento valori colonna Visibilità in tabella Configurazione_Siti", "146")

            Riga_Requisiti("AgronicaCoreAPI",
                           "Compliance ISCC", "13/09/2024")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")

            Riga_Text("GIS",
                      "Introdotta gestione buchi in import poligoni gis da Catasto DXF e ShapeFiles generici",
                      noteTest:="Importare un dxf, importare uno shape file entrambi dal modulo GIS. disegnare e cancellare un nuovo impianto dal GIS nuovo. fare un giro di verificare sui sementieri")

            Riga_Bug("Anagrafica",
                     "fix controllo chiave campo in scrittura appezzamento",
                     noteTest:="nessun test perché riproducibile solo nel GSB invio pcg da Demetra a Gias")

            Riga_Bug("Categorie Magazzino",
                     "Ripristinati WebMethod spariti (Leggi_Categorie_Magazzino, CategorieMagazzino, Leggi_Generico_Categorie_Magazzino, Leggi_Categorie_Magazzino_Impostazioni)",
                     noteTest:="Entrare nella pagina delle giacenze di magazzino, ora non deve dare più errore")

            Riga_Bug("Profilazione NG",
                     "Aggiorna la funzione per la lettura delle aziende in visibilità e quella per il controllo della gerarchia",
                     noteTest:="Assegnando un'impresa con figli in visibilità o, all'apertura della finestra di assegnazione se l'utente aveva molte aziende in visibilità, c'era comparissero errori")

            Riga_Bug("Refactor funzioni di controllo scrittura",
                     "Refactorate le funzioni di controllo lato server di scrittura appezzamenti/impianti/esercizi perché siano Sub invece che Function quando appropriato.")

            '================================

            Riga_Data("17 Settembre 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Audit Coprob", "23/04/2024")

            Riga_Requisiti("Migra",
                           "1) Per tabelle compliance ISCC, nuovo permesso gestione impostazioni imprese
                                  2) Per l'inserimento di una nuova colonna nella tabella Configurazione_Siti", "772")

            Riga_Requisiti("Aggancio",
                           "Per viste compliance ISCC", "127")

            Riga_Requisiti("Gias_Configurazione_Siti",
                           "Aggiornamento valori colonna Visibilità in tabella Configurazione_Siti", "146")

            Riga_Requisiti("AgronicaCoreAPI",
                           "Compliance ISCC", "13/09/2024")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")

            Riga_Text("Codifica e decodifica stringhe",
                      "Introdotta la possibilità di effettuare una codifica semplice delle stringhe",
                      noteTecniche:="Per poter abilitare la codifica semplice la proprietà UsaCodificaSemplice in appsettings deve essere impostata a true")

            Riga_Bug("Anagrafica NG",
                      "fix controllo chiave campo in scrittura appezzamento",
                      noteTecniche:="aggiunto controllo che la chiave del campo può non essere valorizzata (null)",
                      noteTest:="Provare ad inserire un appezzamento\impianto con e senza campo non deve dare errore")

            Riga_Bug("SQL Dataprovider",
                      "fix query in cui era presente una clausola OR e una data",
                      noteTecniche:="fix eseguito all'interno del filtroAggiuntivo",
                      noteTest:="Modificare o creare un qualsiasi impianto da AnagraficaNG, non deve andare in errore")

            Riga_Bug("Profilazione NG",
                      "Aggiorna la funzione di lettura delle categorie di magazzino per le impostazioni aziendali")

            Riga_Text("SQL Dataprovider",
                      "Modificati criteri log su disco e verso Elastic Search",
                      noteTecniche:="Non testabile da assistenza")

            '================================

            Riga_Data("13 Settembre 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Audit Coprob", "23/04/2024")

            Riga_Requisiti("Migra",
                           "1) Per tabelle compliance ISCC, nuovo permesso gestione impostazioni imprese
                                  2) Per l'inserimento di una nuova colonna nella tabella Configurazione_Siti", "772")

            Riga_Requisiti("Aggancio",
                           "Per viste compliance ISCC", "127")

            Riga_Requisiti("Gias_Configurazione_Siti",
                           "Aggiornamento valori colonna Visibilità in tabella Configurazione_Siti", "146")

            Riga_Requisiti("AgronicaCoreAPI",
                           "Compliance ISCC", "13/09/2024")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")

            Riga_Bug("Esportazione Registro Trattamenti Agea",
                      "sistemato caricamento menù a tendina Imprese",
                      noteTecniche:="Risoluzione problema segnalato nell'excel '\\rubino2\documentazione\GIAS --- Clienti --- Coldiretti\Prj Demetra 2023\Logging\Analisi LOG ES.xlsx' dove il valore della colonna Funzione è 'AgronicaCoreUtility.Filtrone.CreaDTFiltrone()'",
                      noteTest:="Nessun Test Necessario")

            Riga_Text("DataProvider - Log Agro_Sequenze",
                      "Modifica Scrivi_Log per loggare anche NomeTabella in caso di exception",
                      noteTest:="Non testabile da assistenza")

            Riga_Text("Security - SQL Dataprovider",
                      " - Implementato utilizzo dei parametri SQL nella maggior parte dei punti in cui non erano utilizzati",
                      noteTecniche:="Usate sempre Agro_SQL_Save_Clausola_IN e Agro_SQL_Save_xFiltroAggiuntivo nella composizione di query in cui compaiono parametri inseriti da interfaccia",
                      noteTest:="Effettuare giri generici sulla pagina cercando di utilizzare eventuali filtri disponibili nel modo più specifico possibile")

            Riga_Text("Security - XSS Detection",
                      "Aggiunto controllo per evitare script injection nelle chiamate ai web method")

            Riga_Text("GIS",
                      "Rilascio sviluppi per compliance ISCC lato GIS",
                      noteTest:="Dal GIS, aprire la funzione CONFIGURAZIONE ALGORITMI, spostarsi nella tab CHECKLIST ed utilizzare le funzioni; il flusso completo richiede che nel GSB sia attivo il tipo_sincro=135 Algoritmi GIS")

            Riga_Text("Banca Cambiano",
                      "Rilascio Modello valutativo (situazione patrimoniale, conto economico e rendiconto finanziario)", "Banca Cambiano")

            Riga_Text("Permessi utenti",
                      "Aggiunge il codice enumerativo per il nuovo permesso di gestione delle impostazioni d'impresa (562)")

            Riga_Text("Lettura indici maturita APP",
                      "Aggiunta lettura MisuraXIndiciMaturita_Anagrafiche per sincro dati APP")

            Riga_Text("Esportazione Registro Trattamenti Agea",
                      "Esportazione nuove operazioni di fertilizzazione chimica/organica e irrigazione",
                          noteTest:="Nessun test necessario")

            Riga_Text("Esportazione Registro Trattamenti Agea",
                      "Invio LOG ad Elastic Search",
                       noteTest:="Nessun test necessario")

            Riga_Text("DataProvider",
                      "Aggiunto contatore indirizzi in gestione via sequence",
                      noteTest:="nessun test")

            Riga_Bug("DataProvider",
                      "fix indirizzi e agronica_log_invio_chiamate in gestione via sequence in minuscolo",
                      noteTest:="nessun test")

            Riga_Text("Security - Web.Config",
                      "- Aggiunta/sistemata parte del ""customErrors"" Per fare in modo che compilando in debug localmente sia visibile il messaggio di errore dettagliato, " &
                      "mentre compilando in Release per metterlo in produzione si verrà rimandati alla pagina di errore generico.",
                      "Coldiretti",
                      noteTest:="Non testabile",
                      noteTecniche:="In fase di compilazione/pubblicazione il valore del tag &lt;customErrors> del file Web.Config viene sovrascritto dal relativo blocco nel file Web.Debug.Config o Web.Release.config")

            Riga_Text("Modifiche relative al Filtro Ricerca (new!)",
                      "Varie modifiche relative al funzionamento del Filtro Ricerca (new!)")


            Riga_Text("Controlli lato server",
                      "Allineate le funzioni di controllo scrittura appezzamento, progetto e impianto lato server con quelle lato client.")

            Riga_Text("Anagrafiche NG",
                      "Aggiunto controllo lunghezza CAP sul salvataggio indirizzi Centri, appezzamenti e Imprese",
                      noteTest:="Il CAP solo nel caso di indirizzi italiani non può superare la lunghezza di 5 caratteri")

            Riga_Text("Operazioni di lettura tabella Configurazione_Siti",
                        "Modificate le operazioni di lettura nella tabella Configurazione_Siti in maniera tale da restituire al client solo le chiavi visibili lato client",
                       noteTest:="Non testabile")

            Riga_Text("GIS - GDAL",
                      "Aggiornamento versione librerie GDAL a versione 3.9.1",
                      noteTest:="nessun test")

            Riga_Text("GIS - Algoritmi",
                      "Aggiunto nuovo algoritmo gis per elaborazione Raster Overlay in locale al posto di GEE",
                      noteTest:="nessun test")

            '================================

            Riga_Data("11 Settembre 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Audit Coprob", "23/04/2024")

            Riga_Requisiti("Migra",
                           "Per tabelle compliance ISCC", "771")

            Riga_Requisiti("Aggancio",
                           "Per viste compliance ISCC", "127")

            Riga_Requisiti("Configurazione_Siti",
                           "Aggiunta chiave AgroProfilazione_MaxUtentiCaricatiDefault per indicare il limite massimo di utenti sotto cui vengono caricati di default tutti gli utenti", "143")

            Riga_Requisiti("AgronicaCoreAPI",
                           "IsAlive", "26/04/2024")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")

            Riga_Bug("Data Provider - Traduzione tabelle",
                     " - Fix gestione traduzione automatica di query in lingua per cui se non c'era lo spazio dopo il nome di tabella andava in errore la query",
                     "ENI Costa Avorio")

            Riga_Bug("GIS Esportazione File",
                     "Corretta esportazione entità layer impianti",
                     noteTest:="testare con meno di 500 impianti, con un numero compreso tra 500 e 1000 e con un numero maggiore di 1000")

            '================================

            Riga_Data("10 Settembre 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Audit Coprob", "23/04/2024")

            Riga_Requisiti("Migra",
                           "Per tabelle compliance ISCC", "771")

            Riga_Requisiti("Aggancio",
                           "Per viste compliance ISCC", "127")

            Riga_Requisiti("Configurazione_Siti",
                           "Aggiunta chiave AgroProfilazione_MaxUtentiCaricatiDefault per indicare il limite massimo di utenti sotto cui vengono caricati di default tutti gli utenti", "143")

            Riga_Requisiti("AgronicaCoreAPI",
                           "IsAlive", "26/04/2024")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")

            Riga_Text("Documentale, Ricerca/Nuovo Documento",
                     "- Aggiunta 'P. Iva' visibile nel menù a discesa delle aziende sia in Nuovo Documento che nei filtri di Ricerca
                     - Aggiunta colonna P. Iva nel griglia di Ricerca Documenti")

            '================================

            Riga_Data("06 Settembre 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Audit Coprob", "23/04/2024")

            Riga_Requisiti("Migra",
                           "Per tabelle compliance ISCC", "771")

            Riga_Requisiti("Aggancio",
                           "Per viste compliance ISCC", "127")

            Riga_Requisiti("Configurazione_Siti",
                           "Aggiunta chiave AgroProfilazione_MaxUtentiCaricatiDefault per indicare il limite massimo di utenti sotto cui vengono caricati di default tutti gli utenti", "143")

            Riga_Requisiti("AgronicaCoreAPI",
                           "IsAlive", "26/04/2024")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")

            Riga_Bug("Anagrafica Macchine Ng",
                     "Aggiornata funzione di scrittura delle macchine per gestire correttamente il cambio visibilità di una macchina movimentata")

            Riga_Text("GIS",
                     "Aggiunto parametro opzionale a metodo TestaPoligonoWKTValid per bypassare l'invio del log applicativo a Elasti Search", "coldiretti", 0, "", "nessun test")

            Riga_Text("Traduzioni",
                     "Aggiunte e corrette voci per Doc. Contabili e Giacenze di Magazzino", "ENI CIV")

            '================================

            Riga_Data("03 Settembre 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Audit Coprob", "23/04/2024")

            Riga_Requisiti("Migra",
                           "Per tabelle compliance ISCC", "771")

            Riga_Requisiti("Aggancio",
                           "Per viste compliance ISCC", "127")

            Riga_Requisiti("Configurazione_Siti",
                           "Aggiunta chiave AgroProfilazione_MaxUtentiCaricatiDefault per indicare il limite massimo di utenti sotto cui vengono caricati di default tutti gli utenti", "143")

            Riga_Requisiti("AgronicaCoreAPI",
                           "IsAlive", "26/04/2024")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")

            Riga_Bug("GIS - Importazione layer catasto",
                      "Corretto un bug che impediva il caricamento del layer del catasto a partire da shapefile quando si seleziona ""nessuna trasformazione"" ",
                      "Aboca, tutti",
                      noteTest:="Selezionare un file di particelle catastali valido proiettato su SRID 4326. Davide et al.: Per esempio il file che hai prodotto tu per il catasto personalizzato in Aboca")

            Riga_Bug("Profilazione NG",
                      "- Aggiorna il caricamento delle opzioni per la scelta magazzino predefinito nelle impostazioni utente
                      - Ottimizza la funzione di ricerca visibilità per gruppo utente")

            Riga_Bug("QdC NG",
                      "Fix salvataggio note per operazioni in cui non è specificato l'utilizzo di terreno (es. visite)")

            '================================

            Riga_Data("30 Agosto 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Audit Coprob", "23/04/2024")

            Riga_Requisiti("Migra",
                           "Per tabelle compliance ISCC", "771")

            Riga_Requisiti("Aggancio",
                           "Per viste compliance ISCC", "127")

            Riga_Requisiti("Configurazione_Siti",
                           "Aggiunta chiave AgroProfilazione_MaxUtentiCaricatiDefault per indicare il limite massimo di utenti sotto cui vengono caricati di default tutti gli utenti", "143")

            Riga_Requisiti("AgronicaCoreAPI",
                           "IsAlive", "26/04/2024")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")

            Riga_Text("GIS - Dati Satellitari",
                      "- creato nuovo endpoint che lettura elaborazioni dati satellitari da GEE in GiasAPP che prende in input l'ID della entità GIS al posto del poligono WKT",
                      "Consorzi Agrari Italia",
                      30988, "essendo che la query di estrazione dati va per intersezione, nei casi di poligoni sovrapposti vengono estratti i dati anche di altri poligoni",
                      "da app leggere visualizzare i dati SAT, tutte le immagini devono essere coerenti con il poligono disegnato e non risultare essere di altri poligoni")

            '================================

            Riga_Data("27 Agosto 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Audit Coprob", "23/04/2024")

            Riga_Requisiti("Migra",
                           "Per tabelle compliance ISCC", "771")

            Riga_Requisiti("Aggancio",
                           "Per viste compliance ISCC", "127")

            Riga_Requisiti("Configurazione_Siti",
                           "Aggiunta chiave AgroProfilazione_MaxUtentiCaricatiDefault per indicare il limite massimo di utenti sotto cui vengono caricati di default tutti gli utenti", "143")

            Riga_Requisiti("AgronicaCoreAPI",
                           "IsAlive", "26/04/2024")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")

            Riga_Bug("Profilazione NG",
                      "- Aggiusta la formulazione delle query massive per l'assegnazione della visibilità agli utenti")

            Riga_Bug("Note QdC - NG",
                      "- Fa in modo che, se il componente sull'interfaccia web non viene inizializzato, vengono utilizzate le note di default",
                      noteTest:="Modifiche fatte a seguito della segnalazione nel documento di test della RC_2024-07-15")

            '================================

            Riga_Data("23 Agosto 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Audit Coprob", "23/04/2024")

            Riga_Requisiti("Migra",
                           "Per tabelle compliance ISCC", "771")

            Riga_Requisiti("Aggancio",
                           "Per viste compliance ISCC", "127")

            Riga_Requisiti("Configurazione_Siti",
                           "Aggiunta chiave AgroProfilazione_MaxUtentiCaricatiDefault per indicare il limite massimo di utenti sotto cui vengono caricati di default tutti gli utenti", "143")

            Riga_Requisiti("AgronicaCoreAPI",
                           "IsAlive", "26/04/2024")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")

            Riga_Text("GiasAPP - popola rilievo app",
                      "Aggiunta lettura Danni Raccolta",
                      noteTest:="Nessun Test Necessario")

            Riga_Text("Security",
                      " - Aggiornamento libreria RestSharp da 106.11.7 a 106.15.0 [fix CVE-2021-27293]")

            Riga_Text("QdC NG",
                      " - Abilita ricette e brogliaccio per operazione di abbattimento impianti")

            Riga_Bug("Dichiarazione di non utilizzo Trattamenti/Fertilizzazioni",
                      "Sistemata visualizzazione messaggio di blocco della dichiarazione di non utilizzo",
                        idPerforma:=31688,
                        noteTest:="Salvare una Dichiarazione di non utilizzo Trattamenti/Fertilizzazioni senza selezionare il centro e la specie." & vbCrLf &
                        "Fare un nuovo Trattamento/Fertilizzazione verificando che il messaggio di blocco della dichiarazione di non utilizzo venga mostrato solo per gli esercizi attivi alla data di registrazione della dichiarazione selezionandoli dalla griglia.")

            Riga_Text("Copia/Sposta Appezzamenti - Preservazione Date e Utente creazione",
                      "Preservazione della data e dell'utente di creazione di appezzamento, impianti ed esercizi quando si copia/sposta da anagrafica.",
                      noteTest:="Possibile verificare tramite Filtro di Ricerca (new!) --> Mostra 'Piano Colturale' --> in fondo alla selezione delle colonne sono presenti Data e Utente Creazione App/Imp/Ese. ")

            Riga_Text("Widget 'Stime di produzione colture principali'",
                      "Fix calcolo Stime di Produzione")

            Riga_Text("Gestione eccezione generica",
                      " - Gestione eccezione generica e chiave in appsettings.config")

            Riga_Text("SQL Data Provider nuova impostazione e mail di log",
                      " - Reinserita la possibilità di ricevere una mail di log per le query in errore" &
                      " - Inserita la possibilità di impostare la non esecuzione della query originale in caso di errore di parametrizzazione")

            Riga_Text("Esportazione Agea",
                      "Cambiato calcolo waterQuantity e del productQuantity per ettaro nei phytochemicalTreatments + WIP nuove operazioni da esportare",
                       noteTest:="Nessun test necessario")

            Riga_Text("Zoo",
                      "Aggiunta icona per redirect elenco trattamenti capo nella griglia di Riepilogo Consistenze quando c'è la nuova grafica",
                       noteTest:="Nessun test necessario")

            Riga_Text("Nuovo Quaderno di Campagna",
                      "Sistemato caricamento menù a tendina delle Specie e dei Centri Aziendali per visualizzare solamente quelli con esercizi attivi alla data dell'operazione")

            Riga_Text("Esportazione Agea",
                      "Esclusa l'Operazione 'Concia del Seme' tra le Operazioni da inviare come 'PhytochemicalTreatment'",
                       noteTest:="Nessun test necessario")

            Riga_Text("WIP Impostazione 'Modalità filtro di ricerca' (SUPERUSER_Mod_Filtro_Ricerca)",
                      "WIP Aggiunta impostazione 'Modalità filtro di ricerca' (SUPERUSER_Mod_Filtro_Ricerca) per permettere di scegliere tra l'utilizzo del filtrone_nuovo.aspx (modalità base) e del Filtro Ricerca (new!) (modalità avanzata)",
                      noteTecniche:="NON TESTABILE")

            '================================

            Riga_Data("20 Agosto 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Audit Coprob", "23/04/2024")

            Riga_Requisiti("Migra",
                           "Nuove colonna Tabella Audit_Impostazioni", "766")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Aggiunta chiave AgroProfilazione_MaxUtentiCaricatiDefault per indicare il limite massimo di utenti sotto cui vengono caricati di default tutti gli utenti", "143")

            Riga_Requisiti("AgronicaCoreAPI",
                           "IsAlive", "26/04/2024")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")

            Riga_Bug("Anagrafica NG - Leggi Vincoli",
                     "Fix su lettura vincoli per clienti con disciplinare privato", "Al Molejn", 32018)

            Riga_Text("Widget Dashboard",
                     "Rinominato widget Colture in Colture Principali attive e widget Produzione Colture in Produzione Principali Colture nell’anno")

            Riga_Bug("Profilazione NG",
                     "- Fix salvataggio e controlli sulla validità utente: forza l'username a un formato tutto minuscolo, aggiusta il pattern per stabilire se il codice fiscale ha un formato corretto
                     - Aggiorna funzione di lettura utenti per la griglia: permette di visualizzare i nuovi utenti creati anche se non rispettano i filtri attualmente impostati
                     - Aggiorna le chiamate per l'attribuzione della visibilità: ottimizza le query per un utilizzo massivo
                     - Aggiunge un controllo sulla visibilità in login per prevenire l'accesso di utenti senza visibilità")

            Riga_Bug("Quaderno di Campagna",
                     "Cambiato messaggio di sotto giacenza al salvataggio dell'operazione", cliente:="Genagricola", idPerforma:=31989)

            '================================

            Riga_Data("19 Agosto 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Audit Coprob", "23/04/2024")

            Riga_Requisiti("Migra",
                           "Nuove colonna Tabella Audit_Impostazioni", "766")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Aggiunta chiave AgroProfilazione_MaxUtentiCaricatiDefault per indicare il limite massimo di utenti sotto cui vengono caricati di default tutti gli utenti", "143")

            Riga_Requisiti("AgronicaCoreAPI",
                           "IsAlive", "26/04/2024")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")

            Riga_Bug("AnagraficaNg ParcoMacchine",
                     "Corretta modifica contatto associato: permesso eliminare contatto associato")

            '================================

            Riga_Data("08 Agosto 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Audit Coprob", "23/04/2024")

            Riga_Requisiti("Migra",
                           "Nuove colonna Tabella Audit_Impostazioni", "766")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Aggiunta chiave AgroProfilazione_MaxUtentiCaricatiDefault per indicare il limite massimo di utenti sotto cui vengono caricati di default tutti gli utenti", "143")

            Riga_Requisiti("AgronicaCoreAPI",
                           "IsAlive", "26/04/2024")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")

            Riga_Bug("QdC NG - Note",
                      "Ripristina funzionalità di gestione delle note default dal tool nel QdC angular",
                      idPerforma:=31829)

            '================================

            Riga_Data("02 Agosto 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Audit Coprob", "23/04/2024")

            Riga_Requisiti("Migra",
                           "Nuove colonna Tabella Audit_Impostazioni", "766")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Aggiunta chiave AgroProfilazione_MaxUtentiCaricatiDefault per indicare il limite massimo di utenti sotto cui vengono caricati di default tutti gli utenti", "143")

            Riga_Requisiti("AgronicaCoreAPI",
                           "IsAlive", "26/04/2024")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")

            Riga_Text("Anagrafiche Ng",
                      "Aggiunta gestione nuovo campo numero certificato taratura per ParcoMacchine")

            Riga_Text("AgroChatGPT",
                      "Aggiornate domande per verificare la corretezza di un Trattamento",
                      noteTest:="Nessun Test Necessario")

            Riga_Bug("GIS Esportazione Entita",
                     "Corretta query pulizia tabella EntitaXEsportazioni")

            Riga_Bug("Anagrafiche Ng",
                     "Corretta lettura fabbricati", idPerforma:=31479)

            Riga_Text("Hardening Sicurezza Viste/Report",
                      "Aggiunto controllo per evitare HTML/Js injections. I caratteri speciali inseriti vengono salvati con le relative codifiche.",
                      noteTest:="Su una qualunque griglia Angular (QdC, Visite, Anagrafiche..) creare e salvare una vista che contiene il seguente testo '&lt;script>'.
                      Ricaricando la griglia/pagina i caratteri speciali < e > dovranno essere stati sostituiti con &amp;lt; e &amp;gt;")

            Riga_Text("Esportazione Agea",
                      "Aggiunta valorizzazione del calibrationId e aggiunto default a 'NO_ADVERSITY' se non viene scelta una avversità.",
                       noteTest:="Nessun test necessario")

            Riga_Text("Stampa Scheda Colturale BIO da Operazioni Colturali (new!)",
                      "Aggiunto messaggio 'Non sono presenti impianti colturali BIO sull’azienda selezionata. Selezionare un'azienda con impianti BIO per proseguire con la stampa.' se si cerca di lanciare la stampa in una azienda senza impianti BIO ")

            '================================

            Riga_Data("29 Luglio 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Audit Coprob", "23/04/2024")

            Riga_Requisiti("Migra",
                           "Nuove colonna Tabella Audit_Impostazioni", "766")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Aggiunta chiave AgroProfilazione_MaxUtentiCaricatiDefault per indicare il limite massimo di utenti sotto cui vengono caricati di default tutti gli utenti", "143")

            Riga_Requisiti("AgronicaCoreAPI",
                           "IsAlive", "26/04/2024")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")

            Riga_Bug("Menu Agenda NG",
                      "Fix redirect in info/modifica per le operazioni di trapianto dopo aver cambio azienda")

            Riga_Text("HubAgea",
                      "Aggiunto health check",
                      cliente:="Coldiretti",
                      noteTest:="Nessun Test Necessario")

            Riga_Text("Alert_Entita.asmx",
                      "Modificato WebMethod per fix a 'Gestione Patentini - Contatti'")

            Riga_Bug("Widgets meteo",
                      "Rimuove riferiemnto alla password dell'utente dalla chiamata per la lettura delle informazioni utente")

            '================================
            Riga_Data("25 Luglio 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Audit Coprob", "23/04/2024")

            Riga_Requisiti("Migra",
                           "Nuove colonna Tabella Audit_Impostazioni", "766")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Aggiunta chiave AgroProfilazione_MaxUtentiCaricatiDefault per indicare il limite massimo di utenti sotto cui vengono caricati di default tutti gli utenti", "143")

            Riga_Requisiti("AgronicaCoreAPI",
                           "IsAlive", "26/04/2024")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")

            Riga_Bug("GIS",
                      "Fix query gis per regressione sulla chiamata da APP.",
                      noteTecniche:="Nel caso di chiamate da app il vettore layerElementiGrafici_cod è null",
                      noteTest:="registrare nuovo appezzamento da gis su APP, non deve dare errore")

            '================================

            Riga_Data("23 Luglio 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Audit Coprob", "23/04/2024")

            Riga_Requisiti("Migra",
                           "Nuove colonna Tabella Audit_Impostazioni", "766")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Aggiunta chiave AgroProfilazione_MaxUtentiCaricatiDefault per indicare il limite massimo di utenti sotto cui vengono caricati di default tutti gli utenti", "143")

            Riga_Requisiti("AgronicaCoreAPI",
                           "IsAlive", "26/04/2024")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")

            Riga_Text("GIS",
                      "Ottimizzata query per escludere i layer non richiesti se si richiama il GIS dall'agenda - parte 2",
                      noteTecniche:="quanto si accede al GIS dal QDC non venivano filtrate le informazioni di dettaglio relativa ai layers",
                      noteTest:="nessuno, verranno fatti i test di carico")

            Riga_Bug("QdC NG - Operazione Rilievi",
                      "Aggiunge la lettura delle impostazioni per la personalizzazione delle banche dati rilievi",
                      noteTecniche:="Impostazioni in questione: 819,820,821,822,832,833. Tutte gestite a livello di superuser.")

            '================================

            Riga_Data("19 Luglio 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Audit Coprob", "23/04/2024")

            Riga_Requisiti("Migra",
                           "Nuove colonna Tabella Audit_Impostazioni", "766")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Aggiunta chiave AgroProfilazione_MaxUtentiCaricatiDefault per indicare il limite massimo di utenti sotto cui vengono caricati di default tutti gli utenti", "143")

            Riga_Requisiti("AgronicaCoreAPI",
                           "IsAlive", "26/04/2024")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")

            Riga_Bug("CDG",
                      "Tolto filtro contatto valorizzato. Ora vengono visualizzate anche le macchine con contatto-proprietario valorizzato.")

            Riga_Text("Lettura dati per widget ultimi acquisti",
                      "Migliorata la performance della lettura degli ultimi acquisti")

            Riga_Text("Lettura dettagli utente", "Migliorata la lettura dei dettagli dell'utente in alcuni punti")

            Riga_Text("Utilizzata funzione Leggi_Superuser_e_ProgressivoGIAS per recuperare Progressivo Gias e PWD Superuser", "Performance Migliorate")

            '================================

            Riga_Data("18 Luglio 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Audit Coprob", "23/04/2024")

            Riga_Requisiti("Migra",
                           "Nuove colonna Tabella Audit_Impostazioni", "766")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Aggiunta chiave AgroProfilazione_MaxUtentiCaricatiDefault per indicare il limite massimo di utenti sotto cui vengono caricati di default tutti gli utenti", "143")

            Riga_Requisiti("AgronicaCoreAPI",
                           "IsAlive", "26/04/2024")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")

            Riga_Text("GIS",
                      "Ottimizzata query per escludere i layer non richiesti se si richiama il GIS dall'agenda",
                      noteTecniche:="nel caso in sui si valorizzi puntualmente l'elenco dei layer richiesti, l'elenco dei layer visibili dall'utente viene rimpiazzato dal primo elenco",
                      noteTest:="nessuno, verranno fatti i test di carico")

            Riga_Text("Esportazione Agea",
                      "Gestione Log",
                      noteTest:="Nessun Test Necessario")

            Riga_Bug("SonarQube risolte le seguenti segnalazioni:",
                      "- S5254 '&lt;html>' element should have a language attribute" &
                      "- S5256 Tables should have headers" &
                      "- ImgWithoutAltCheck Image, area and button with image tags should have an 'alt' attribute" &
                      "- BoldAndItalicTagsCheck '&lt;strong>' and '&lt;em>' tags should be used" &
                      "- FieldsetWithoutLegendCheck '&lt;fieldset>' tags should contain a '&lt;legend>'" &
                      "- S3923 All branches in a conditional structure should not have exactly the same implementation" &
                      "- S1862 Related 'If/ElseIf' statements should not have the same condition" &
                      "- S1656 Variables should not be self-assigned" &
                      "- S1751 Loops with at most one iteration should be refactored")

            '================================

            Riga_Data("17 Luglio 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Audit Coprob", "23/04/2024")

            Riga_Requisiti("Migra",
                           "Nuove colonna Tabella Audit_Impostazioni", "766")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Aggiunta chiave AgroProfilazione_MaxUtentiCaricatiDefault per indicare il limite massimo di utenti sotto cui vengono caricati di default tutti gli utenti", "143")

            Riga_Requisiti("AgronicaCoreAPI",
                           "IsAlive", "26/04/2024")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")

            Riga_Text("SQL Sequence",
                      "Aggiunto parametro attivazione in appsettings")

            Riga_Text("Esportazione Agea",
                      "Miglioramento pagina di esportazione registro trattamenti Agea")

            '================================

            Riga_Data("15 Luglio 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Audit Coprob", "23/04/2024")

            Riga_Requisiti("Migra",
                           "Nuove colonna Tabella Audit_Impostazioni", "766")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Aggiunta chiave AgroProfilazione_MaxUtentiCaricatiDefault per indicare il limite massimo di utenti sotto cui vengono caricati di default tutti gli utenti", "143")

            Riga_Requisiti("AgronicaCoreAPI",
                           "IsAlive", "26/04/2024")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")

            Riga_Bug("Profilazione NG",
                     "- Abilita modifica password utenti
                     - Ripristina modifiche per la lettura della chiave di abilitazione dello spid dal super server")

            Riga_Text("Algoritmi GIS",
                      "Aggiornato algoritmo per esportazione shape file",
                      noteTest:="Esportare dal layer degli impianti un insieme di poligoni ottenuti dalla ricerca con il filtrone, impostando il check nei parametri di esportazione sulla dicitura 'Esporta poligoni visibili'. Una volta fatto click su 'avvia esportazione', a differenza della versione precedente, la procedura si comporterà come se fosse stato selezionato il flag 'Esporta tutto' ")

            Riga_Text("Statistometro",
                      " - Per redirect a statistometro")

            Riga_Text("Lettura utenti, Utenti_Read.Leggi",
                      "Introdotto parametro opzionale nella lettura degli utenti")

            Riga_Text("SQL Sequence",
                      "nuova gestione id tabella tramite sequence per alcune tabelle", "", 0, "
                      Queste le tabelle coinvolte:
                      	Agenda
	                    Movimenti
	                    Movimenti_dettagli
	                    Movimenti_dettagli_tecnici
	                    Movimenti_dettagli_tecnici_extra
	                    Idtestatatemp
	                    Ricette
	                    Ricette_operazioni
	                    Ricette_dettaglio_tecnico
	                    Ricette_dettagli
	                    Ricette_destinazioni
                        Raccoglitore,
                        gis_entita,
                        gis_elementigrafici,
                        impresa_progetto,
                        Agronica_Log_Invio_Chiamate
                      ", "")

            Riga_Text("SQL Sequence",
                      "esclusa gestione sequence dai database 2008", "", 0, "", "")

            Riga_Text("Contatori",
                      "Normalizzata funzione di richiamo stack counter", "", 0, "", "")

            Riga_Text("ISOLATION LEVEL READ UNCOMMITTED",
                      "ISOLATION LEVEL READ UNCOMMITTED in tutte le query che non sono dentro una transazione", "", 0,
                      "Disabilitabile tramite la chiave nell'appsettings: : &lt;add key=""ReadUncommittedDefault"" value=""False""/>.",
                      "")

            Riga_Text("Lettura operazioni APP",
                      "Passaggio alla funzione Leggi_GruppoOperazioni_NG per la sincronizzazione delle operazioni su APP")

            Riga_Text("Gestione redirect per scelta imprese",
                      "Inserite le classi e i metodi necessari per fare il redirect verso il Filtrino Imprese o verso il Filtro Ricerca (new!), in base al valore dell'impostazione")

            Riga_Text("Profilazione",
                      "- Alla modifica dei permessi utenti, se è presente il permesso cartografia (12), inizializza i layers del GIS")

            Riga_Bug("Esportazione Agea",
                      "Fix esportazione phytochemicalTreatments")

            '================================

            Riga_Data("03 Luglio 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Audit Coprob", "23/04/2024")

            Riga_Requisiti("Migra",
                           "Nuove colonna Tabella Audit_Impostazioni", "766")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Aggiunta chiave AgroProfilazione_MaxUtentiCaricatiDefault per indicare il limite massimo di utenti sotto cui vengono caricati di default tutti gli utenti", "143")

            Riga_Requisiti("AgronicaCoreAPI",
                           "IsAlive", "26/04/2024")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")

            Riga_Text("Algoritmi GIS",
                      "Aggiornato algoritmo per esportazione shape file",
                      noteTest:="Esportare dal layer degli impianti un insieme di poligoni ottenuti dalla ricerca con il filtrone, impostando il check nei parametri di esportazione sulla dicitura 'Esporta poligoni visibili'. Una volta fatto click su 'avvia esportazione', a differenza della versione precedente, la procedura si comporterà come se fosse stato selezionato il flag 'Esporta tutto' ")

            Riga_Text("Statistometro",
                      " - Per redirect a statistometro")

            Riga_Bug("GIS",
                      "Ripulita query lettura gis da tabella GerarchiaImprese.",
                      "ConsorzioAgrarioItalia",
                      30988,
                      "Utilizzando la tabella GerarchiaImprese nella query di lettura dl GIS provoca un sdoppiamento delle dei poligoni qualora l'azienda agricola sia figlia di più padri. Per ovviare al problema e mantenere la compatibilità con i sementieri è stata utilizzata per tabella gruppi_utente per identificare il layer della organizzazione aziendale",
                      "Sulla APP richiamare gli indici satellitari per gli impianti della azienda agricola 'SOCIETA' AGRICOLA I SABBIONI S.S' e verificare che le mappe vengano correttamente visualizzate. ")

            Riga_Bug("Profilazione NG",
                     "- Fix creazione utente in ambienti con gestione spid abilitata
                     - Aggiunge possibilità di specificare filtri su note utente, profilo e gruppo per caricamento utenti visualizzati in griglia
                     - Fix caricamento valori impostazioni nelle sezioni 'Agronomia' e 'Agenda'")

            '================================

            Riga_Data("21 Giugno 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Audit Coprob", "23/04/2024")

            Riga_Requisiti("Migra",
                           "Nuove colonna Tabella Audit_Impostazioni", "766")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Aggiunta chiave AgroProfilazione_MaxUtentiCaricatiDefault per indicare il limite massimo di utenti sotto cui vengono caricati di default tutti gli utenti", "143")

            Riga_Requisiti("AgronicaCoreAPI",
                           "IsAlive", "26/04/2024")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")

            Riga_Bug("Lettura impianti APP",
                     "Corretta lettura impianti per APP, gestiti impianti bloccati")

            Riga_Bug("Elastic Search",
                     "Inserita gestione escaping per log applicativi inviati ad elastic search", "Coldiretti",
                     noteTecniche:="Aggiunto remove escaping pre e post serializzazione",
                     noteTest:="no test")

            Riga_Bug("Profilazione NG",
                     "- Considera anche orario in data creazione utente
                     - Rende più lineare la gestione della transazione durante l'assegnazione della visibilità per evitare errori di transazione rimasta aperta
                     - Aggiusta attribuzione permessi a profili utenti")

            '================================
            Riga_Data("14 Giugno 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Audit Coprob", "23/04/2024")

            Riga_Requisiti("Migra",
                           "Nuove colonna Tabella Audit_Impostazioni", "766")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Aggiunta chiave AgroProfilazione_MaxUtentiCaricatiDefault per indicare il limite massimo di utenti sotto cui vengono caricati di default tutti gli utenti", "143")

            Riga_Requisiti("AgronicaCoreAPI",
                           "IsAlive", "26/04/2024")

            Riga_Requisiti("AuthDispatcher",
                           "Per utilizzo Abaco Geo Proxy", "Rilasciare")

            Riga_Text("QdC NG - magazzino esterno",
                     "- Controllo giacenza rispetto a magazzino esterno su validazione prodotto e salvataggio qdc, anche multicentro
                     - Controllo impostazioni di giacenza del magazzino esterno anche nel caricamento dei prodotti (formulati, fertilizzanti, sementi, insetti)",
                    noteTest:="testare quanto descritto su operazioni semplici (es: solo magazzino interno) e complesse (es: multicentro, su più impianti, con più prodotti, alcuni presi da magazzino esterno, altri da magazzino interno, verificando che i valori di giacenza e i blocchi siano corretti")

            Riga_Bug("QdC NG - semina",
                     "Fix - filtro tipologie sementi (sem_cod) applicato nel menu a tendina delle sementi anche se non si usa il magazzino")

            Riga_Text("QDC",
                      "  Rinominare 'Quaderno di Campagna' diventa 'Quaderno (QDCA)', 'Quaderno' diventa 'Quaderno (QDCA)', 'QdC' diventa 'QDCA'")

            Riga_Bug("Nuovo QdC",
                      "Sistemata apertura in info di un Brogliaccio proveniente da APP")

            Riga_Text("Profilazione NG",
                      "- Aggiunge gestione finestra temporale utenti
                      - Aggiunge filtri pre-caricamento utenti")

            Riga_Text("Profilazione NG",
                      "Implementata gestione delle transizioni di stato nei gruppi utente.")

            Riga_Text("Log Provider",
          " Modificato il valore di default da logga solo su file a logga solo su DB", noteTest:="Non testabile")


            Riga_Text("Coprob - Checklist Trasporti",
                     "- Adeguamento core audit e documentale per checklist filiera trasporti coprob. ", cliente:="COPROB",
                    noteTest:="Ripristinare backup produzione su testinterni.")

            Riga_Text("Nuovo Quaderno di Campagna",
                     "Forzata apertura in modifica al vecchio QdC (pagina Trattamenti_2.aspx) se si cerca di modificare una Semina con scarico da Magazzino dell'azienda padre/superuser (giro utilizzato da Conserve italia)")

            Riga_Text("Audit Impostazioni",
                     "Gestita nuova tabella 'Audit_Impostazioni' invece di 'Utenti_Impostazioni'")

            Riga_Text("GIS, SAT & Raster",
                     "Visualizzazione dei Raster e degli indici vegetativi attraverso l'impiego di ABACO GEOPROXY",
                     noteTest:="Contattare il Team GIS Agronica per le spiegazioni opportune sui test da fare")

            Riga_Text("Widget GIS",
                     "Aggiunto pulsante nella pagina di configurazione degli algoritmi per avviare la ri-elaborazione dei dati per i Widget GIS",
                     noteTest:="Contattare il Team GIS Agronica per le spiegazioni opportune sui test da fare")

            Riga_Text("GISNg Algoritmi",
                      "Nuovo Web service per attivazione di algoritmi GIS utilizzabile da interfaccia utente, nuovo Web service per configurazione Cfg di algoritmi GIS utilizzabile da interfaccia utente",
                    noteTest:="Nessun Test Richiesto")

            Riga_Text("Sincro",
                      "Nuovo servizio per sincronizzazione storico attivita e rilievi da APP",
                      noteTest:="Nessun Test Richiesto")

            Riga_Text("bottone Statistiche piano colturale",
                      "- passa dal filtrone al posto del filtrotto")

            '================================

            Riga_Data("11 Giugno 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Audit Coprob", "23/04/2024")

            Riga_Requisiti("Migra",
                           "Nuove colonne tabelle UMA", "761")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave Server 'Utenti_Visibilia_Appoggio_Da_Capostipiti' per visibilita aziendale nuova", "134")

            Riga_Requisiti("AgronicaCoreAPI",
                           "IsAlive", "26/04/2024")

            Riga_Bug("Anagrafica NG",
                     "Corretta lettura superficie disponibile catasto campi")

            '================================

            Riga_Data("07 Giugno 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Audit Coprob", "23/04/2024")

            Riga_Requisiti("Migra",
                           "Nuove colonne tabelle UMA", "761")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave Server 'Utenti_Visibilia_Appoggio_Da_Capostipiti' per visibilita aziendale nuova", "134")

            Riga_Requisiti("AgronicaCoreAPI",
                           "IsAlive", "26/04/2024")

            Riga_Text("Profilazione NG", "Ottimizza chiamate caricamento")

            '================================

            Riga_Data("03 Giugno 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Audit Coprob", "23/04/2024")

            Riga_Requisiti("Migra",
                           "Nuove colonne tabelle UMA", "761")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave Server 'Utenti_Visibilia_Appoggio_Da_Capostipiti' per visibilita aziendale nuova", "134")

            Riga_Requisiti("AgronicaCoreAPI",
                           "IsAlive", "26/04/2024")

            Riga_Bug("Imprese Angular",
                     "Corretta lettura contatti Organismo Di Controllo per imprese angrafica angular in creazione nuova impresa")

            Riga_Bug("Anagrafica Angular",
                     "- creato nuovo web service CaricaDatiCatastali_Ng per anagrafica budget angular
	                  - creato module UtilityParticelle in AgronicaCoreAnagrafeDAL
	                  - refactoring classe ParticelleCatastali_R
	                  - corretta e aggiornata lettura superfici catasto appezzamento angular
	                  - aggiornata lettura catasto campi per estrazione dati ripartizione superficie
	                  - corretta e aggiornata lettura catasto appezzamenti
	                  - corretta e aggiornata lettura catasto per campi")

            Riga_Bug("Profilazione NG",
                    "- Evita di applicare le impostazioni legate al profilo in creazione di un nuovo utente
                    - Velocizza scrittura nuova tipologia utente")

            Riga_Text("AWS_log",
                     "Aggiunto (NOLOCK) nella query di lettura",
                        noteTest:="Nessun test necessario")

            Riga_Text("Caricamento Menù Agenda",
                     "Aggiunto (NOLOCK) nella query di lettura",
                        noteTest:="Nessun test necessario")

            '---------------------------------------------------------------------------

            Riga_Data("29 Maggio 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Audit Coprob", "23/04/2024")

            Riga_Requisiti("Migra",
                           "Nuove colonne tabelle UMA", "761")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave Server 'Utenti_Visibilia_Appoggio_Da_Capostipiti' per visibilita aziendale nuova", "134")

            Riga_Requisiti("AgronicaCoreAPI",
                           "IsAlive", "26/04/2024")

            Riga_Bug("Nuovo Quaderno di Campagna",
                     "Fix apertura in modifica di Operazioni senza scarico di Prodotti da Magazzino",
                            cliente:="Orogel", idPerforma:=30568)

            '---------------------------------------------------------------------------

            Riga_Data("27 Maggio 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Audit Coprob", "23/04/2024")

            Riga_Requisiti("Migra",
                           "Nuove colonne tabelle UMA", "761")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave Server 'Utenti_Visibilia_Appoggio_Da_Capostipiti' per visibilita aziendale nuova", "134")

            Riga_Requisiti("AgronicaCoreAPI",
                           "IsAlive", "26/04/2024")

            Riga_Bug("Profilazione NG",
                     "- Fix applicazione valore default in lettura impostazioni per form angular
                     - Modifica lettura delle impostazioni salvate nella tabella FiltroMono (es. filtro
                     gruppi vegetali, specie vegetali) per seguire la scalarità utente > profilo > superuser
                     - Fix caricamento utenti con diverse validità permessi in modo da mostrare sempre una sola riga per utente
                     - Fix ribaltamento impostazioni da profilo per ignorare impostazione preferiti menu
                     - Modifica query per il caricamento dei permessi")

            '---------------------------------------------------------------------------

            Riga_Data("20 Maggio 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Audit Coprob", "23/04/2024")

            Riga_Requisiti("Migra",
                           "Nuove colonne tabelle UMA", "761")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave Server 'Utenti_Visibilia_Appoggio_Da_Capostipiti' per visibilita aziendale nuova", "134")

            Riga_Requisiti("AgronicaCoreAPI",
                           "IsAlive", "26/04/2024")

            Riga_Bug("Profilazione",
                     "Fix creazione profili utenti sia nella pagina gias che NG")

            '---------------------------------------------------------------------------

            Riga_Data("17 Maggio 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Audit Coprob", "23/04/2024")

            Riga_Requisiti("Migra",
                           "Nuove colonne tabelle UMA", "761")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave Server 'Utenti_Visibilia_Appoggio_Da_Capostipiti' per visibilita aziendale nuova", "134")

            Riga_Requisiti("AgronicaCoreAPI",
                           "IsAlive", "26/04/2024")

            Riga_Bug("SQL DataProvider ordinamento orderby",
                     "Fix ordinamento dei risultati di alcune query (come il widget delle colture)")

            Riga_Text("Miglioramento lettura imprese all'apertura di alcune pagine",
                      "Velocizzata la lettura delle imprese prendendo solo i campi necessari, introdotto metodo nuovo per la lettura")

            Riga_Text("Migliorata la parametrizzazione delle query",
                      "Velocizzata la procedura di sostituzione del testo necessaria per la parametrizzazione")

            Riga_Bug("Nuovo QdC",
                      "Sistemata apertura in info di un Brogliaccio proveniente da APP")

            '---------------------------------------------------------------------------

            Riga_Data("13 Maggio 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Audit Coprob", "23/04/2024")

            Riga_Requisiti("Migra",
                           "Nuove colonne tabelle UMA", "761")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave Server 'Utenti_Visibilia_Appoggio_Da_Capostipiti' per visibilita aziendale nuova", "134")

            Riga_Requisiti("AgronicaCoreAPI",
                           "IsAlive", "26/04/2024")

            Riga_Text("Profilazione NG",
                     "- Modifica query per il caricamento dei permessi assegnabili ai profili
                     - Aggiunge funzione per ottenere gli utenti collegati a un profilo, usata all'assegnazione di un
                     permesso a un profilo per visualizzare il numero di utenti su cui si ripercuote la modifica")

            '================================

            Riga_Data("10 Maggio 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Audit Coprob", "23/04/2024")

            Riga_Requisiti("Migra",
                           "Nuove colonne tabelle UMA", "761")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave Server 'Utenti_Visibilia_Appoggio_Da_Capostipiti' per visibilita aziendale nuova", "134")

            Riga_Requisiti("AgronicaCoreAPI",
                           "IsAlive", "26/04/2024")

            Riga_Bug("Entity Framework",
                     "Riallineamento modello EF per piano colturale")

            Riga_Bug("Checklist SQNPI/BIO",
                     "Blocco Upload documenti",
                     "COPROB",
                     7812,
                     "Reimpostato il filtro per determinare lo stato di un audit partendo dalla tipologia funzione VerificaPermessoUploadDocumento")

            Riga_Bug("Servizio IsAlive",
                     "Fix - eliminata connectionString da response db")

            '================================

            Riga_Data("09 Maggio 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Audit Coprob", "23/04/2024")

            Riga_Requisiti("Migra",
                           "Nuove colonne tabelle UMA", "761")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave Server 'Utenti_Visibilia_Appoggio_Da_Capostipiti' per visibilita aziendale nuova", "134")

            Riga_Requisiti("AgronicaCoreAPI",
                           "IsAlive", "26/04/2024")

            Riga_Bug("Modulo Quote Conferimento",
                     "Fix su errore caricamento tabella visualizza quote conferimento",
                     cliente:="ASIPO")

            Riga_Bug("Servizio IsAlive",
                     "Fix su valorizzazione link GiasBase da chiamare.")

            '================================

            Riga_Data("08 Maggio 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Audit Coprob", "23/04/2024")

            Riga_Requisiti("Migra",
                           "Nuove colonne tabelle UMA", "761")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave Server 'Utenti_Visibilia_Appoggio_Da_Capostipiti' per visibilita aziendale nuova", "134")

            Riga_Requisiti("AgronicaCoreAPI",
                           "IsAlive", "26/04/2024")

            Riga_Bug("Redirect Planning fascicolo",
                    "Corretto bug sul redirect del planning fascicolo, ora su NG funziona come sul .Net")

            Riga_Text("Profilazione NG",
                    "- Aggiunge gestione accesso Spid nella griglia utenti della nuova profilazione, necessaria chiave paginaIndex_LoginSPID attiva
                    - Aggiunge controlli/avvisi su email utente: email deve essere obbligatoria e univoca per i nuovi
                    utenti, per quelli già esistenti o con accesso Spid attivo vengono mostrati solo degli avvisi")

            Riga_Text("GISNg",
                      "Aggiunto endpoint GetEnvelopeWKT")

            Riga_Text("Servizio IsAlive",
                      "Completamento sviluppo; cambio verifica raggiungibilità DBs.")

            '================================

            Riga_Data("07 Maggio 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Audit Coprob", "23/04/2024")

            Riga_Requisiti("Migra",
                           "Nuove colonne tabelle UMA", "761")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave Server 'Utenti_Visibilia_Appoggio_Da_Capostipiti' per visibilita aziendale nuova", "134")

            Riga_Requisiti("AgronicaCoreAPI",
                           "IsAlive", "26/04/2024")

            Riga_Bug("Nuovo Quaderno di Campagna",
                     "Sistemato salvataggio colonna Sup. Riduzione [Ha] Buffer nelle Ricette",
                     cliente:="IDSC",
                     idPerforma:=30120)

            Riga_Bug("Data Provider",
                     "Fix su lettura config base, verifica del count e non solo del numero dei record",
                     idPerforma:=30087)

            '================================

            Riga_Data("03 Maggio 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Audit Coprob", "23/04/2024")

            Riga_Requisiti("Migra",
                           "Nuove colonne tabelle UMA", "761")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave Server 'Utenti_Visibilia_Appoggio_Da_Capostipiti' per visibilita aziendale nuova", "134")

            Riga_Requisiti("AgronicaCoreAPI",
                           "IsAlive", "26/04/2024")

            Riga_Text("Contatti.asmx",
                     " CodiceASL da Contatto, recupero codice ASL da Indirizzo Impresa se contatto di tipo impresa ",
                     noteTest:="Risolve la segnalazione sul trasportatore RealBeef",
                     cliente:="INALCA")

            Riga_Text("Catasto.asmx",
                     " Corretta gestione cancellazione e inserimento e cambio chiave (prov, com, sezione, foglio, numero, subalterno) 
                     della stessa su più centri della stessa azienda o su più aziende ")

            Riga_Text("AgronicaCoreAnagrafeDAL/Imprese.vb",
                     " Gestito il caricamento di codice iscrizione libro soci e data iscrizione libro soci nel caricamento imprese per anagrafica nuova ")

            Riga_Bug("Profilazione NG",
                     "- Modifica il passaggio date validita permessi utente
                     - Aggiorna il caricamento dei valori impostazioni per ignorare i record della tabella guida_impostazioni_valori non validi
                     - Aggiunge end-points per il caricamento dei dati selezionabili nelle impostazioni 9, 215, 722, 1010, 1062")

            '================================

            Riga_Data("30 Aprile 2024 (HOTFIX)")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Audit Coprob", "23/04/2024")

            Riga_Requisiti("Migra",
                           "Nuove colonne tabelle UMA", "761")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave Server 'Utenti_Visibilia_Appoggio_Da_Capostipiti' per visibilita aziendale nuova", "134")

            Riga_Requisiti("AgronicaCoreAPI",
                           "IsAlive", "26/04/2024")

            Riga_Bug("QdC New",
                            "Letto il valore massimo della Classe Tessitura nella griglia degli Impianti", noteTest:="Nessun test necessario")

            '================================

            Riga_Data("26 Aprile 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Audit Coprob", "23/04/2024")

            Riga_Requisiti("Migra",
                           "Nuove colonne tabelle UMA", "761")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave Server 'Utenti_Visibilia_Appoggio_Da_Capostipiti' per visibilita aziendale nuova", "134")

            Riga_Requisiti("AgronicaCoreAPI",
                           "IsAlive", "26/04/2024")

            Riga_Bug("Profilazione NG",
                     "- Permette la specifica delle date di validità dei permessi già dalla creazione dell'utente
                     - Aggiorna la funzione di creazione di un utente per applicare automaticamente le impostazioni dal profilo assegnato
                     - Aggiusta la gestione della pagina per le impostazioni di aziende/centri
                     - Aggiorna il caricamento dei permessi utente mostrando una colonna contente una descrizione aggiuntiva
                     - Filtra i permessi caricati rendendo visibili solo quelli posseduti dall'utente che ha eseguito il login
                     - Aggiunge la gestione delle note di profilo: per ogni tipologia utente è ora possibile indicare e modificare il campo note
                     - Aggiusta passaggio piva in creazione utenti di tipo azienda
                     - Aggiunge controllo in creazione di più utenti per evitare l'inserimento di dati ripetuti
                     - Modifica lettura utenti per caricare direttamente tutti gli elementi se l'azienda ha meno di 200 utenti totali")

            Riga_Text("Parametrizzatore DataProvider",
                      "- Aggiunto nuovo parametrizzatore come default per velocizzare l'esecuzione delle query" &
                      "- Corretto caso che mandava in errore la procedura quando veniva utilizzata una funzione aggregata come criterio di order by")

            Riga_Text("machinekey.config",
                      "aggiunto il richiamo sul web.config")

            Riga_Text("GIS",
                      "Nello shapefile prodotto in fase di esportazione dei poligoni sul layer degli impianti è stata aggiunta, in tabella degli attributi, la colonna con il dato della cultivar",
                     noteTest:="Dal gis esportare uno shapefile, aprirlo in QGIS ed osservare che nello shape ora c'è la colonna cultivar.")

            Riga_Bug("Cancellazione analisi da Analisi_Modello / Import Demetra",
                     "Fix cancellazione analisi senza parametri specificati",
                     noteTest:="Non testabile da assistenza")

            Riga_Text("Ricerca Prodotti:",
                      "- Ora viene letto il campo 'descrizione addizionale' (extra_str) dalla tabella materie prime, sia in scarico che in carico",
                      idPerforma:=28545, cliente:="Asipo",
                      noteTecniche:="Non influisce sul calcolo della giacenza perché il valore preso da anagrafica non è differente fra una movimentazione e un'altra")

            Riga_Text("LOG APPLICATIVI",
                      "Modificata la funzione Scrivi_LOG:
                       - ora viene salvato il CUAA dell'impresa selezionata 
                       - aggiunta gestione di invio dei log applicativi ad Elastic Search (se attivo e correttamente configurato)
                       - per log generati dal dataprovider viene salvata anche la query in chiaro senza parametri",
                      noteTest:="Non testabile da assistenza")

            Riga_Text("Documentale",
                      "Inserito controllo permesso upload documento", "coprob", "7812")

            Riga_Text("Servizio IsAlive - WIP",
                      "Aggiunti WS per:
                       - verificare raggiungibilità DB;
                       - eseguire warmup EF;
                       - ottenere lista siti di cui verificare raggiungibilità;")

            Riga_Text("Doc Contabili:",
                      "- Le impostazioni lette sul superuser sono ora gestite anche a livello di impresa, per allineare comportamento con il QdC NG, " &
                      "in particolare sulla impostazione di gestione lotto per categoria prodotto",
                      noteTecniche:="Task TFS 6856")

            '================================

            Riga_Data("24 Aprile 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Lettura Formulati QdC", "05/09/2023")

            Riga_Requisiti("Migra",
                           "Nuove colonne tabelle UMA", "761")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave Server 'Utenti_Visibilia_Appoggio_Da_Capostipiti' per visibilita aziendale nuova", "134")

            Riga_Requisiti("AgronicaCoreAPI",
                           "Modificato timeout da 4 a 15 minuti (per fix su Copia/Sposta Appezzamento)", "26/03/2024")

            Riga_Bug("AnagraficaMacchine",
                     "Corretta lettura marca per anagrafica macchine angular")

            Riga_Text("Anagrafica NG",
                      "sostituito campo Agea_idParcella con Agea_codiBarrScheVali per adeguamento tracciato agea", noteTest:="nessun test")

            '================================
            Riga_Data("18 Aprile 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Lettura Formulati QdC", "05/09/2023")

            Riga_Requisiti("Migra",
                           "Nuove colonne tabelle UMA", "761")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave Server 'Utenti_Visibilia_Appoggio_Da_Capostipiti' per visibilita aziendale nuova", "134")

            Riga_Requisiti("AgronicaCoreAPI",
                           "Modificato timeout da 4 a 15 minuti (per fix su Copia/Sposta Appezzamento)", "26/03/2024")

            Riga_Bug("Anagrafica Campo",
                     "Corretto caricamento griglia appezzamenti appartenenti al campo + appezzamenti 'liberi'")

            Riga_Bug("QdC",
                     "Corretta apertura in modifica di una Operazione con doppio scarico dello stesso prodotto proveniente da due Magazzini Esterni diversi e caricati su due diversi Magazzini Aziendali")

            '================================

            Riga_Data("17 Aprile 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Lettura Formulati QdC", "05/09/2023")

            Riga_Requisiti("Migra",
                           "Nuove colonne tabelle UMA", "761")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave Server 'Utenti_Visibilia_Appoggio_Da_Capostipiti' per visibilita aziendale nuova", "134")

            Riga_Requisiti("AgronicaCoreAPI",
                           "Modificato timeout da 4 a 15 minuti (per fix su Copia/Sposta Appezzamento)", "26/03/2024")

            Riga_Bug("DatiPrevisionaliColture",
                     "Aggiornata lettura DatiPrevisionaliColture")

            Riga_Text("Albero Impresa",
                     "Aggiunta informazione relativa alla specie vegetale dell'impianto ")

            '================================

            Riga_Data("12 Aprile 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Lettura Formulati QdC", "05/09/2023")

            Riga_Requisiti("Migra",
                           "Nuove colonne tabelle UMA", "761")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave Server 'Utenti_Visibilia_Appoggio_Da_Capostipiti' per visibilita aziendale nuova", "134")

            Riga_Requisiti("AgronicaCoreAPI",
                           "Modificato timeout da 4 a 15 minuti (per fix su Copia/Sposta Appezzamento)", "26/03/2024")

            Riga_Bug("DatiPrevisionaliColture",
                     "Aggiornata lettura DatiPrevisionaliColture per stato_cod = 102")

            Riga_Bug("ModificaMultiplaImpianti",
                     "Corretta lettura DatiPrevisionaliColture per modifica multipla")

            '================================

            Riga_Data("10 Aprile 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Lettura Formulati QdC", "05/09/2023")

            Riga_Requisiti("Migra",
                           "Nuove colonne tabelle UMA", "761")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave Server 'Utenti_Visibilia_Appoggio_Da_Capostipiti' per visibilita aziendale nuova", "134")

            Riga_Requisiti("AgronicaCoreAPI",
                           "Modificato timeout da 4 a 15 minuti (per fix su Copia/Sposta Appezzamento)", "26/03/2024")

            Riga_Text("Menù Agenda NG",
                     "Quando viene cancellata una Operazione di Campagna vengono cancellati anche i Carichi e gli Scarichi di Magazzino associati")

            Riga_Text("Ricerca Documenti",
                        "Gestito redirect da Dashboard (nuovo widget 'Documenti Recenti')")

            Riga_Text("Menu",
                        "Valorizza il campo 'password' nella chiamata per recuperare le informazioni sull'utente attuale")

            Riga_Text("SonarQube",
                        "SonarQube - vbnet:S5445 - Path.GetTempFileName() is insecure",
                     noteTest:="Nessun test richiesto")

            Riga_Text("SonarQube",
                        "SonarQube - xml:S2068 - Hard-Coded Credentials are secure-sensitive",
                     noteTest:="Nessun test richiesto")

            Riga_Text("SonarQube",
                        "SonarQube - vbnet:S2077 - Formatting SQL queries is security-sensitive",
                     noteTest:="Nessun test richiesto")

            Riga_Text("GIS",
                        "Migliorata gestione delle prestazioni in fase di lettura delle etichette",
                        noteTest:="Verificare che le etichette vengano mostrate sia per il layer impianti, sia su un layer personalizzato")

            Riga_Text("Profilazione NG",
                        "- Permette di specificare le date di validità dei permessi in creazione di un nuovo utente
                        - Fix lettura valori impostazioni imprese/centri
                        - Aggiunge funzione di caricamento per griglia ricerca utilizzo impostazine impresa
                        - Aggiorna cancellare un'impostazione impresa/centro
                        - Aggiorna salvataggio utenti per asseganre le impostazioni del profilo alla creazione di un nuovo utente")

            '================================

            Riga_Data("08 Aprile 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Lettura Formulati QdC", "05/09/2023")

            Riga_Requisiti("Migra",
                           "Nuove colonne tabelle UMA", "761")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave Server 'Utenti_Visibilia_Appoggio_Da_Capostipiti' per visibilita aziendale nuova", "134")

            Riga_Requisiti("AgronicaCoreAPI",
                           "Modificato timeout da 4 a 15 minuti (per fix su Copia/Sposta Appezzamento)", "26/03/2024")

            Riga_Text("Lettura Giacenze Zootecniche",
                      " Migliorata performance query ", cliente:="INALCA")

            Riga_Text("QdC New",
                      "Aggiunta l'indicazione dell'Azienda nella colonna 'Magazzino' nella ricerca dei prodotti quando si cerca un prodotto da Magazzino Esterno.")

            Riga_Text("Menù Agenda NG",
                      "Quando viene cancellata una Operazione di Campagna vengono cancellati anche i Carichi e gli Scarichi di Magazzino associati")

            Riga_Text("Utenti Visibilita",
                      "Aggiusta assegnazione impostazioni da tipologia utente")

            '================================

            Riga_Data("04 Aprile 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Lettura Formulati QdC", "05/09/2023")

            Riga_Requisiti("Migra",
                           "Nuove colonne tabelle UMA", "761")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave Server 'Utenti_Visibilia_Appoggio_Da_Capostipiti' per visibilita aziendale nuova", "134")

            Riga_Requisiti("AgronicaCoreAPI",
                           "Modificato timeout da 4 a 15 minuti (per fix su Copia/Sposta Appezzamento)", "26/03/2024")

            Riga_Text("Lettura Giacenze Zootecniche",
                      " Migliorata performance query ", cliente:="INALCA")

            Riga_Bug("QdC New",
                      "Sistemata lettura Classe Tessitura nella griglia degli Impianti",
                        idPerforma:=28602,
                        cliente:="Genagricola")

            Riga_Bug("ConfigurazioneAgroMeteo",
                      "Corretto bug lettura Lat/Lng da piva per centro aziendale")

            Riga_Bug("Anagrafica Macchine",
                     "aggiornata gestione macchina movimentata anagrafiche angular")

            '================================

            Riga_Data("28 Marzo 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Lettura Formulati QdC", "05/09/2023")

            Riga_Requisiti("Migra",
                           "Nuove colonne tabelle UMA", "761")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave Server 'Utenti_Visibilia_Appoggio_Da_Capostipiti' per visibilita aziendale nuova", "134")

            Riga_Requisiti("AgronicaCoreAPI",
                           "Modificato timeout da 4 a 15 minuti (per fix su Copia/Sposta Appezzamento)", "26/03/2024")

            Riga_Text("Interfaccia Log Interscambio (Consulta Sincro Dati App)",
                      "Modificata query estrazione log G2D - Attivita (vengono caricate anche i log delle ricette)")

            '================================

            Riga_Data("26 Marzo 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Lettura Formulati QdC", "05/09/2023")

            Riga_Requisiti("Migra",
                           "Nuove colonne tabelle UMA", "761")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave Server 'Utenti_Visibilia_Appoggio_Da_Capostipiti' per visibilita aziendale nuova", "134")

            Riga_Requisiti("AgronicaCoreAPI",
                           "Modificato timeout da 4 a 15 minuti (per fix su Copia/Sposta Appezzamento)", "26/03/2024")

            Riga_Bug("Copia/Sposta Appezzamento",
                     "Fix funzionalità: modificata transazione da globale a singolo elemento")

            Riga_Bug("RefreshToken",
                     "Gestito unico refresh Token per utente")

            Riga_Text("GIS",
                      "Nuovi campi 'livello clusterizzazione' e 'check modalità heatmap' nelle impostazioni GIS dell'utente",
                      noteTest:="Verificare che tutte le impostazioni del GIS, soprattutto 'Livello clusterizzazione' e 'modalità visualizzazione cluster', nel menù 'setup visualizzazione' lato NG siano caricati e salvati correttamente")

            Riga_Text("GIS",
                      "Nuovi parametri di ritorno quando si applica un filtro temporale nel filtrone per la sincronizzazione con il GIS",
                      noteTest:="Applicare un filtro temporale nel filtrone e verificare che le date e periodi impostati siano correttamente ritornati sotto il nome di 'parametri'")

            Riga_Text("GIS",
                      "Nuovo bottone per il reset della Partita IVA nel filtrone quando si cerca di applicare l'azienda come criterio di ricerca",
                      noteTest:="Aprire un'istanza del filtrone e assicurarsi di trovarsi nell'area 'Azienda (Aziende E Centri)'. Verificare che alla pressione del pulsante subito a destra del campo 'Partita IVA' il campo stesso venga svuotato del valore impostato'")

            Riga_Text("GIS",
                      "Nuovi campi 'GMapsZoomLevel', 'TotalOriginalArea', e 'TotalOriginalFeatureNumber' aggiunti alle proprietà delle features",
                      noteTest:="Verificare che alla richiesta del caricamento delle feature questi valori siano correttamente passati con i valori di default")

            Riga_Bug("Aggiunto Invio Mail di Log per Segnalazioni Speciali",
                     "Aggiunto invio mail nel caso di Piva = stringa vuota per la funzione AgronicaCoreUtility.CaricaListControl.TutteSpecieColtivate_3_Data_Da_A()",
                     "Regione Umbria",
                     noteTest:="Non testare")

            Riga_Text("Interfaccia Log Interscambio (Consulta Sincro Dati App)",
                     "Modificato BIZ lettura")

            Riga_Text("DatiPrevisionaliColture",
                      "gestito dato dettSpeciPersonalizzatoCod, aggiunta colonna dettSpeciePersonalizzatoCod in SpecieVegetali_Default, aggiunta lettura dettSpeciePersonalizzato in lettura esercizi Angular")

            Riga_Bug("Profilazione NG",
                     "- Gestisce gli errori in caricamento delle impostazioni utente
                     - Aggiusta l'assegnazione del profilo utente per gestire i casi in cui esso non abbia permessi o impostazioni collegate
                     - Aggiorna la data di fine validità permessi alla disattivazione di un utente")

            Riga_Text("Sincro Dati App (Interfaccia Log Interscambio)",
                     "Import/Export Mov Magazzino aggiornate colonne 'piva', 'chiave gias' e 'chiave esterna'")

            Riga_Text("Anagrafica NG",
                      "- Aggiunta gestione nuovi campi chiave tracciato agea in lettura\scrittura piano colturale (appezzamenti/Impianti) e corrispondente parte del budget", "Coldiretti", 0, "collegato all'import da data_publish", "")

            '================================

            Riga_Data("21 Marzo 2024 BIS")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Lettura Formulati QdC", "05/09/2023")

            Riga_Requisiti("Migra",
                           "Nuovo Campo DW_CDG_Costi_Ricavi.Proprietario_Macchina e Nuovo campo Agea_Cod in Parco_Macchine", "758")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave Server 'Utenti_Visibilia_Appoggio_Da_Capostipiti' per visibilita aziendale nuova", "134")

            Riga_Bug("GIS",
                     "Eni KEnya - anomalia visualizzazione poligoni su GIS, si verifica quando alcuni punti sono troppo vicini all'equatore. Questo fa sì che SQL Server converta la latitudine in formato Esponenziale e da lì non è leggibile.",
                     idPerforma:=29249,
                     noteTest:="Disegnare un poligono vicino all'equatore, impostando come coordinate nella toolbar -00.008355 e 037.9274320. Aiutarsi attivando la griglia di Google maps setup visualizzazione")

            '================================

            Riga_Data("21 Marzo 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Lettura Formulati QdC", "05/09/2023")

            Riga_Requisiti("Migra",
                           "Nuovo Campo DW_CDG_Costi_Ricavi.Proprietario_Macchina e Nuovo campo Agea_Cod in Parco_Macchine", "758")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave Server 'Utenti_Visibilia_Appoggio_Da_Capostipiti' per visibilita aziendale nuova", "134")

            Riga_Text("Sincro Dati App (Interfaccia Log Interscambio)",
                     "Aggiunti tipi interscambio 'D2G - Movimenti Magazzino' e 'D2G - Fornitori'.")

            Riga_Text("Requisiti stabilimento",
                     "Valorizzata colonna Resa Media per contratto")

            '================================

            Riga_Data("14 Marzo 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Lettura Formulati QdC", "05/09/2023")

            Riga_Requisiti("Migra",
                           "Nuovo Campo DW_CDG_Costi_Ricavi.Proprietario_Macchina e Nuovo campo Agea_Cod in Parco_Macchine", "758")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave Server 'Utenti_Visibilia_Appoggio_Da_Capostipiti' per visibilita aziendale nuova", "134")

            Riga_Bug("Codici anagrafe",
                     "Fix lettura codice anagrafe a seguito di cambio codice", cliente:="ASIPO")

            Riga_Bug("Griglia Impianti Nuovo QdC",
                     "Fix lettura Distanza tra Fila e su Fila", cliente:="ASIPO", idPerforma:=29167)

            '================================

            Riga_Data("12 Marzo 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Lettura Formulati QdC", "05/09/2023")

            Riga_Requisiti("Migra",
                           "Nuovo Campo DW_CDG_Costi_Ricavi.Proprietario_Macchina e Nuovo campo Agea_Cod in Parco_Macchine", "758")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave Server 'Utenti_Visibilia_Appoggio_Da_Capostipiti' per visibilita aziendale nuova", "134")

            Riga_Text("Profilazione NG",
                     "- Aggiunge funzione per sincronizzazione permessi degli utenti collegati a una tipologia
                     - Ottimizza assegnazione impostazioni utente da tipologia per far fronte ad operazioni eseguite molti utenti in contemporanea")

            Riga_Bug("Campo Anagrafica NG",
                     "Quando si crea un campo da anagrafica non viene più creato un poligono associato sul GIS.
                     Quando si cancella un campo, i poligoni associati vengono cancellati.")

            Riga_Bug("Profilazione NG",
                     "- Fix attribuzione tipologia per utenti creati tramite import
                     - Fix caricamento data ultimo accesso utenti
                     - Fix disattivazione utenti (pressione bidoncino in riga e successivo salvataggio)")

            Riga_Text("Demetra",
                     "Export attività: gestito il multioperazione + NOLOCK su query di export")

            Riga_Text("DatiPrevisionaliColture",
                      "aggiunta lettura resa totale prevista per esercizi")

            Riga_Text("Menu Agenda",
                      "Ottimizzazione Ricerca delle Operazioni Colturali", noteTest:="Lanciare la ricerca delle Operazioni Colturali (pagina NG e non)")

            Riga_Text("Codice Anagrafe",
                      "- Aggiunto ""Ultima verifica ispettiva"" e ""SAU tot. azienda ha"" come codici anagrafe impresa ", cliente:="ASIPO")

            Riga_Text("QdC NG",
                      "- gestione campi di blocco agenda nel modello attivita, WIP")

            Riga_Text("AnagraficaNg",
                      "aggiornata lettura e scrittura macchine per gestione Codice AGEA in anagrafica macchine angular")

            Riga_Text("Interfaccia Log Interscambio (Consulta Sincro Dati App)",
                     "Aggiunto tipo interscambio 'D2G - Piano Colturale'.")

            Riga_Text("Anagrafica Esercizio",
                     "Aggiunto scrittura e lettura delle ddl Lavorazione e Specifica.")

            '================================

            Riga_Data("07 Marzo 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Lettura Formulati QdC", "05/09/2023")

            Riga_Requisiti("Migra",
                           "Nuovo Campo DW_CDG_Costi_Ricavi.Proprietario_Macchina e Nuovo campo Agea_Cod in Parco_Macchine", "758")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave Server 'Utenti_Visibilia_Appoggio_Da_Capostipiti' per visibilita aziendale nuova", "134")

            Riga_Bug("Centri Aziendali",
                     "Corretta query di aggiornamento Visibilità utente in scrittura di nuovo centro aziendale ")

            '================================

            Riga_Data("04 Marzo 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Lettura Formulati QdC", "05/09/2023")

            Riga_Requisiti("Migra",
                           "Nuovo Campo DW_CDG_Costi_Ricavi.Proprietario_Macchina e Nuovo campo Agea_Cod in Parco_Macchine", "758")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave Server 'Utenti_Visibilia_Appoggio_Da_Capostipiti' per visibilita aziendale nuova", "134")

            Riga_Bug("Centri Aziendali",
                     "Corretta query caricamento griglia centri aziendali")

            Riga_Bug("Centri Anagrafica NG",
                     "Il valore della colonna AT_Prevalente (usata nel LAN), viene preservato.",
                     "Cantina Valmorri", 26960)

            '================================

            Riga_Data("29 Febbraio 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Lettura Formulati QdC", "05/09/2023")

            Riga_Requisiti("Migra",
                           "Nuovo Campo DW_CDG_Costi_Ricavi.Proprietario_Macchina e Nuovo campo Agea_Cod in Parco_Macchine", "758")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave Server 'Utenti_Visibilia_Appoggio_Da_Capostipiti' per visibilita aziendale nuova", "134")

            Riga_Text("CDG",
                      "Inserita colonna Proprietario_Macchina in Report CDG")

            Riga_Text("Profilazione NG",
                      "- Modifica funzione di scrittura ed aggiornamento permessi utente da tipologia per sopportare grandi moli di lavoro
                      - Aggiunge end-point per caricamento dati in dropdown impostazioni
                      - Nasconde il campo azienda selezionata nel filtrone per la selezione delle aziende durante la modifica della visibilità
                      - Aggiunge default per lingua utente: se la lingua utente non è stata impostata da interfaccia, utilizza italiano come lingua di default. Fatto per evitare utenti con Lingua_Cod = 0.")

            Riga_Text("Modifiche UI Regione Umbria",
                      "Modificato messaggio 'Sei sicuro di voler uscire da GIAS?' in 'Sei sicuro di voler uscire?'")

            Riga_Text("Import Attività Demetra",
                     "Udm NR per Confusione e disorientamento sessuale, import avversità solo per operazioni che le contemplano")

            Riga_Text("Import Contatti Demetra",
                     " Cambiata logica di reciclo Cod_Contatto / Impostazione Validita Fine invece di eliminazione di un Contatto con solo ruolo Terzista")

            Riga_Bug("Import Analisi Terreno Demetra",
                     "salvataggio lat/long su analisi ")

            Riga_Text("Import Analisi Terreno Demetra",
                     "Le coordinate passate da demetra vengono salvate sempre sul primo campione associato all'analisi")

            Riga_Bug("Parco_Macchine",
                     "Corretta valorizzazione tipo operazione in modifica entita su agroncia_log_anagrafe", "coldiretti", 0)

            Riga_Text("Import Attivita Demetra",
                     "Recepita chiave macchina completa (piva_sacod_maccod) invece del solo maccod")

            Riga_Text("Import Analisi Demetra",
                     "Conversione date da UTC a Local in ricezione analisi")

            Riga_Bug("Import Analisi Demetra",
                     "Fix cancellazione analisi da import")

            Riga_Bug("Import lavoratori Demetra",
                     "Fix eccezione modifica rapporti contabili su campi nullable")

            Riga_Text("Profilazione NG",
                     "Applica nuova funzione di aggiornamento massivo dei permessi utenti a partire dalla loro tipologia.
                     Chiamata da interfaccia su tutti gli utenti collegati a una data tipologia quando si aggiornano i permessi di quest'ultima,
                     o sugli utenti selezionati quando questi vengono associati a una tipologia. ")

            Riga_Text("Visibilità Azienda",
                     "Inverte la logica di default per l'utilizzo della chiave Utenti_Visibilia_Appoggio_Da_Capostipiti:
                     se la chiave non è presente o è presente con valore 1 viene applicata la nuova procedure,
                     altrimenti, se è presente con valore diverso da 1 viene applicata la vecchia procedura.")

            Riga_Text("Modello",
                     "Aggiunta nuova proprietà (ageaCod) in modello Parco_Macchine, proprietà di tipo oggetto MacchineCodificaAgea" & vbCrLf &
                     "Aggiunto oggetto di metaschema MacchineCodificaAgea dericato da BaseCodeDescrStr", "Coldiretti", 0, noteTest:="nessun test")

            Riga_Text("Macchine.asmx",
                     "Aggiunta gestione in scrittura\lettura tramite modello nuovo campo Agea_Cod dove viene valorizzato il codice macchina agea collegato al classcode della stessa", "Coldiretti", 0, noteTest:="nessun test")

            Riga_Bug("Macchine.asmx",
                     "Fix valorizzazione data modifica in modifica macchina", "Coldiretti", 0, noteTest:="nessun test")

            Riga_Text("Import Contatti Demetra",
                     "Aggiunto controllo per evitare importazione di un contatto già importato con un altro codice_esterno (Evita tighe duplicate in interscambio contatti ")

            Riga_Text("Analisi Modello, Import Analisi Demetra",
                     "Aggiunta salvataggio PIVA in Agronica_Log_Analisi e Agronica_Log_Invio_Analisi")

            Riga_Text("Interfaccia Log Interscambio (Consulta Sincro Dati App)",
                     "Completamento interfaccia")

            Riga_Bug("Import Contatti Demetra",
                     " Fix errata lettura codice_patentino_esterno in importazione patentiti ")

            Riga_Text("Import attivita Demetra",
                     "Se frequenza zero, non viene salvata l'intervallo che ci passano")

            Riga_Text("Import Contatti Demetra",
                     "Implementato fallback lettura inrescambio patentini per cercare chiave aggiornata se non viene trovato il patentino attraverso il codice Gias ")

            Riga_Bug("GisWS.asmx",
                     "Rilassato vincolo su wktparser per conversione di geojson. Tutte le tipologie 'multi' non hanno più il vincolo che devono essere composte da almeno due elementi")

            Riga_Bug("Import Analisi Terreno",
                     "fix creazione certificato collegato analisi")

            Riga_Bug("Interfaccia Log Interscambio ( Consulta sincro dati app)",
                     "fix caricamento dati per il tipo 'G2D - Movimenti Magazzino'")

            Riga_Bug("Profilazione NG",
                      "- Fix aggiornamento utenti per mantenere la validità dei permessi anche se l'utente non è associato a un profilo
                      - Fix caricamento e salvataggio impostazioni per formati di stampa documenti contabili
                      - Fix creazione utente per importazione: passa il campo Tipologia_Cod alla creazione del record sulla tabella utenti")

            Riga_Bug("GisWS.asmx",
                     "Rilassato vincolo su wktparser per conversione di geojson. Tutte le tipologie 'multi' non hanno più il vincolo che devono essere composte da almeno due elementi - parte 2")

            Riga_Text("Import Lavoratori QdC", "scrittura chiave_esterna in Agronica_Log_Invio_Contatti ")

            '================================

            Riga_Data("22 Febbraio 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Lettura Formulati QdC", "05/09/2023")

            Riga_Requisiti("Migra",
                           "Nuovo campo Mac_Cod in Agronica_Log_Invio_Anagrafe", "757")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave Server 'Utenti_Visibilia_Appoggio_Da_Capostipiti' per visibilita aziendale nuova", "134")

            Riga_Bug("Stampe di Campagna Nuovo QdC NG",
                     "Fix passaggio filtro coltura nelle Stampe di Campagna", idPerforma:=28813, cliente:="AGRITES")

            '================================

            Riga_Data("20 Febbraio 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Lettura Formulati QdC", "05/09/2023")

            Riga_Requisiti("Migra",
                           "Nuovo campo Mac_Cod in Agronica_Log_Invio_Anagrafe", "757")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave Server 'Utenti_Visibilia_Appoggio_Da_Capostipiti' per visibilita aziendale nuova", "134")

            Riga_Bug("Anagrafica",
                     "Corrette query Catasto e Impianti per filtri su centro e campi")

            Riga_Text("Gestione quote conferimento",
                     "Aggiunte colonne percentuale e Resa kg/ha ", cliente:="ASIPO")

            '================================

            Riga_Data("14 Febbraio 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Lettura Formulati QdC", "05/09/2023")

            Riga_Requisiti("Migra",
                           "Nuovo campo Mac_Cod in Agronica_Log_Invio_Anagrafe", "757")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave Server 'Utenti_Visibilia_Appoggio_Da_Capostipiti' per visibilita aziendale nuova", "134")

            Riga_Text("Anagrafica NG",
                      "Cambiate query per calcolo corretto di superficie totale campi, centri e imprese alla data di filtro",
                      cliente:="CAB Terra",
                      noteTest:="")

            Riga_Text("Anagrafica Imprese",
                      "lettura e modifica inline del dato sul gruppo di raccolta a livello di azienda",
                      cliente:="ASIPO",
                      noteTest:="")

            Riga_Text("Utente Token JWT",
                      "Gestione token JWT su db server, aggiunte chiamate per creazione/lettura/refresh")

            Riga_Bug("Anagrafica Centri",
                     "Aggiunta cancellazione celle alla cancellazione di un centro aziendale")

            Riga_Bug("QdC NG",
                     "Aggiunti NO LOCK nelle query di caricamento delle Macchine", noteTest:="Nessun test necessario")

            Riga_Bug("Anagrafica NG",
                     "Fix cancellazione appezzamento\impianto\esercizio con scrittura log_anagrafe disattivata", "coldiretti", 0, "", "nessun test")

            Riga_Bug("Import attività Demetra",
                     "Fix cancellazione attivita", "", 0, "", "nessun test")

            Riga_Bug("Import Analisi Terreno Demetra - Cancellazione",
                     "Per poter importare un'Analisi Terreno da cancellare, non è necessario specificare lat e long")

            Riga_Text("Import Analisi Terreno Demetra",
                     "In Log_Invio_Chiamate.Dati_Inviati viene inserito il json di ImportDemetra contenente il CUAA")

            Riga_Text("Autenticazione via token ",
                     "- Inserita funziona di invalidazione token (no JWT) in caso di accesso da demetra per garantire i token mono uso" & vbCrLf, "coldiretti", 0, "il token utilizzato viene invalidato tramite sotituzione con un GUID, aggiunto lock a livello di riga in modifica token per evitare deadlock", "no test")

            Riga_Text("Import Fabbricati Demetra",
                     "Implementata scrittura Log_Invio_Chiamate per Import Fabbricati Demetra ")

            Riga_Bug("Particelle x Fabbricati",
                     "Fix lettura proprietà particella.proprietario (gestione dbNull)")

            Riga_Text("WS DPI",
                     "Aggiunto campo IDEnte su DTO di ritorno per utilizzo su Import da DataPublish ", "Coldiretti", 0, "", "nessun test")

            Riga_Bug("Import Macchine Demetra",
                     "Fix scrittura campo Origine in Agronica_Log_Anagrafe quando importiamo da Demetra ")

            Riga_Text("Import Attivita Demetra",
                      "- Aggiunte le sei operazioni mancanti alla liste delle operazioni ammissibili")

            Riga_Text("Consulta Sincro Dati App - Modalità 'Demetra' per visualizzazione log interscambio",
                      "Implementata interfaccia per visualizzare i log di import/export con Demetra. Se sull'ambiente esiste almeno un record nella tabella Agronica_Log_Invio_Chiamate con Tipo_Esportazione *_Demetra, l'interfaccia verrà caricata in 'modalità Demetra'. Compaiono tra i tipi solo quelli gestiti e sparisce lo switch 'Visualizza descrizione'")

            Riga_Bug("Macchine.asmx",
                     "Corretto metodo IsMacchinaMovimentata che non bloccava se la macchina era movimentata")

            Riga_Text("Macchine.asmx",
                     "Modificato BIZ che controlla se la macchina è movimentata, aggiunto controllo anche su Ricette e Controllo di Gestione")

            Riga_Text("Import Contatti Demetra",
                     "In Log_Invio_Chiamate.Dati_Inviati viene inserito il json di ImportDemetra contenente il CUAA")

            Riga_Bug("Import Attività Demetra",
                     "fix operatore Demetra mancante")

            Riga_Text("Analisi Modello",
                     "gestita valorizzazione entità particelle catastali")

            Riga_Text("Menu Agenda NG",
                      "Aggiunge la lettura del campo 'Ricetta_Operazione_Des', usato nella griglia del brogliaccio come campo note e del campo 'Origine', usato nella griglia operazioni per individuare le operazioni create da demetra")

            Riga_Text("Import Attivita Demetra",
                      "- Aggiunti controlli bloccanti")

            '================================

            Riga_Data("09 Febbraio 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Lettura Formulati QdC", "05/09/2023")

            Riga_Requisiti("Migra",
                           "Indice su Agronica_Log_Anagrafe", "755")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave Server 'Utenti_Visibilia_Appoggio_Da_Capostipiti' per visibilita aziendale nuova", "134")

            Riga_Bug("Budget Impianto",
                     " - Fix per imputazione gruppo di raccolta su esercizio budget ",
                     cliente:="ASIPO")

            Riga_Bug("Anagrafica Catasto",
                     " - Corretti controlli per cancellazione particella 'condivisa' da più aziende ",
                     cliente:="ASIPO")

            Riga_Bug("BDG Griglia Impianti ",
                     " - Possibilità di aggiungere un nuovo ordine se all'impianto di BDG è già collegato 1 o più ordini ma questi sono in stato cancellato",
                     cliente:="ASIPO")

            '================================

            Riga_Data("06 Febbraio 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Lettura Formulati QdC", "05/09/2023")

            Riga_Requisiti("Migra",
                           "Indice su Agronica_Log_Anagrafe", "755")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave Server 'Utenti_Visibilia_Appoggio_Da_Capostipiti' per visibilita aziendale nuova", "134")

            Riga_Bug("Scarico Sementi da Doc Contabile",
                     "- Se categoria impostata con opzione 'Solo Movimentati' o 'Solo presenti' non si riusciva ad eseguire uno scarico " &
                     "tramite il doc contabile perché si aspettava delle proprietà in più non esposte dalla query di giacenza ",
                     cliente:="CAB Terra", idPerforma:=28189,
                     noteTecniche:="Risolve anche chiamata <b>[28361] di 'Ferrarini Monica Azienda Agricola'</b>",
                     noteTest:="Provare ad eseguire scarichi di magazzino (da nuovo doc contabile) nella categoria sementi utilizzando tutte e tre le impostazioni; " &
                               "avendo modificato la query di giacenza, verificare che non si sia rotta la pagina che mostra le movimentazioni di magazzino e le giacenze per le varie categorie")

            '================================

            Riga_Data("02 Febbraio 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Lettura Formulati QdC", "05/09/2023")

            Riga_Requisiti("Migra",
                           "Indice su Agronica_Log_Anagrafe", "755")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave Server 'Utenti_Visibilia_Appoggio_Da_Capostipiti' per visibilita aziendale nuova", "134")

            Riga_Text("Import Analisi Demetra",
                      "- Aggiunta gestione eventuali valori NULL per i dettagli delle analisi" &
                      "- Cambiata logica di import: ora la transazione è globale per tutte le analisi, ovvero se anche solo una va in errore non ne verrà importata nessuna")

            Riga_Text("Import Contatti Demetra",
                      "- Cambiata Logica di reimport di un contatto precedentemente cancellato. Ora permette di reimportarlo anche se ancora presente in interscambio ma eliminato da interfaccia GIAS" &
                      " Aggiunto controllo in fase di importazione per verificare se un contatto che sarebbe da importare nuovo (non presente in tabella interscambio) non sia già stato creato da atri flussi ")

            Riga_Text("Import attivita Demetra",
                      "- Valorizzato disciplinare in modo che sia trattamenti che fertilizzazioni abbiano il disciplinare impostato a  'Solo Etichetta'")

            Riga_Bug("Appezzamento.asmx",
                     "Rimossa scrittura reg_impianti_codici per id 1328 (data inizio portinnesto) perché già presente la scrittura su campo dedicato. Inoltra va in conflitto nella parte di migra che esegue il ribaltamento",
                     noteTest:="Nessun test necessario")

            Riga_Bug("Raccolta NG",
                     "Fix caricamento descrizione unità di misura in raccolta con ripartizione automatica")

            Riga_Text("Scadenziario",
                      "aggiornata pagina scadenziario per inserimento allegati da catasto angular")

            Riga_Text("Migra",
                      "aggiunta colonna 'proprietario' su tabella ParticelleCatastali")

            Riga_Text("Anagrafica Ng",
                      "aggiornato modello ParticelleCatastali per nuovo campo 'proprietario', aggiornata lettura e scrittura ParticelleCatastali")

            Riga_Text("Modifica Multipla Impianti",
                      "Aggiornati metodi di lettura rese previste")

            Riga_Text("Requisiti stabilimento",
                      "Sistemazione query",
                     noteTest:="Nessun test previsto")

            Riga_Bug("Operazioni Colturali (new!) - Menu Agenda NG",
                  "Fix lancio stampa 'Scheda Colturale BIO' da Operazioni Colturali (new!)",
                   cliente:="Ferrarini Monica Azienda Agricola", idPerforma:=27054)

            Riga_Bug("Documentale - Allegati su FS",
                  "Fix apertura/salvataggio allegati su filesystem: aggiunta funzione AggiungiSlashSeNonEsiste quando si cerca la cartella dove vengono salvati gli allegati")

            Riga_Text("Import attivita Demetra",
                   "Valorizzato disciplinare in modo che sia trattamenti che fertilizzazioni abbiano il disciplinare impostato a  'Solo Etichetta'")

            Riga_Text("Import Analisi Demetra",
                     "Cambiata logica di import: ora la transazione è globale per tutte le analisi, ovvero se anche solo una va in errore non ne verrà importata nessuna")

            Riga_Text("Demetra - Import Attività", "Completamento di tutti gli import attività", "", 0, "", "Nessun test per assistenza")

            Riga_Text("QdC - Griglia Impianti",
                      "Cambiato il tipo di colonna restituito per le Date.",
                      noteTest:="Nessun test necessario")

            Riga_Bug("QdC - Controlli",
                     "Evitati i controlli di etichetta se si seleziona come disciplinare 'Nessun Disciplinare - Nessuna Etichetta'",
                     idPerforma:=28400,
                     cliente:="CAIAGROMEC SERVIZI SRL")

            Riga_Text("Webc.Config  Documentale",
                      "Impostate chiavi maxAllowedContentLength e maxRequestLength a 75MB, per poter permettere agli utenti di allegare file FINO a 50MB",
                      noteTest:="Caricare sul Documentale un allegato di 50MB")

            '================================

            Riga_Data("01 Febbraio 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Lettura Formulati QdC", "05/09/2023")

            Riga_Requisiti("Migra",
                           "Indice su Agronica_Log_Anagrafe", "755")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave Server 'Utenti_Visibilia_Appoggio_Da_Capostipiti' per visibilita aziendale nuova", "134")

            Riga_Bug("QdC New - Distribuzione Insetti/Acari",
                     "Fix logica di identificazione insetti impollinatore (se non ci sono avversità collegate, è un impollinatore)",
                     "Agrites", 28528)

            '================================

            Riga_Data("30 Gennaio 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Lettura Formulati QdC", "05/09/2023")

            Riga_Requisiti("Migra",
                           "Indice su Agronica_Log_Anagrafe", "755")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave Server 'Utenti_Visibilia_Appoggio_Da_Capostipiti' per visibilita aziendale nuova", "134")

            Riga_Bug("Sincro impresa APP",
                      "Fix contatto azienda se presente tipologia gerarchia per aggregatore")

            Riga_Text("Smartseed",
                      "Resa compatibile la generazione della mappa a rateo variabile secondo specifiche smartseeds",
                     noteTest:="Nessun test previsto")

            '================================

            Riga_Data("29 Gennaio 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Lettura Formulati QdC", "05/09/2023")

            Riga_Requisiti("Migra",
                           "Indice su Agronica_Log_Anagrafe", "755")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave Server 'Utenti_Visibilia_Appoggio_Da_Capostipiti' per visibilita aziendale nuova", "134")

            Riga_Bug("QdC NG",
                  "Fix ricerca per numero di registrazione dei prodotti fitosanitari in giacenza ",
                    noteTest:="Cercare un prodotto fitosanitario prima per numero registrazione e poi per descrizione con e senza il flag 'Solo Prodotti in giacenza' nel QdC nuovo (angular)")

            '================================

            Riga_Data("26 Gennaio 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Lettura Formulati QdC", "05/09/2023")

            Riga_Requisiti("Migra",
                           "Indice su Agronica_Log_Anagrafe", "755")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave Server 'Utenti_Visibilia_Appoggio_Da_Capostipiti' per visibilita aziendale nuova", "134")

            Riga_Text("Visibilità Aziendale",
                  "Implementa nuova funzione per il caricamento della tabella Utenti_Visibilita_Appoggio,
                  richiamandola al login dell'utente e nel servizio in background.")

            '================================

            Riga_Data("23 Gennaio 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Lettura Formulati QdC", "05/09/2023")

            Riga_Requisiti("Migra",
                           "Indice su Agronica_Log_Anagrafe", "755")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave Server 'Azienda_Timesheet_Tecnici' per Timesheet CAI", "127")

            Riga_Bug("Documentale - tipologie utente collegate a categoria Catasto",
                     "Fix caricamento tipologie create da utente, sotto categoria 'Catasto'",
                     "Asipo", 28210)

            Riga_Bug("QdC - Controllo BufferZone",
                     "Fix controlli sulla bufferzone",
                     "Studio Tini", 28147)

            Riga_Text("Generazione token utente",
                      "Aggiunto controllo su idDB (server e utente) prima della generazione del token utente 
                      e recupero dei due tramite stringa connessione")

            Riga_Text("ProfilazioneNG",
                      "- Aggiunte colonne di Username_Creazione e Username_Modifica nella tabella Utenti_Dettagli
                      con relativa visualizzazione da interfaccia web")

            Riga_Bug("Anagrafica impianti griglia",
                     "Eliminati decimali in calcolo piante impianto")

            '================================

            Riga_Data("22 Gennaio 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Lettura Formulati QdC", "05/09/2023")

            Riga_Requisiti("Migra",
                           "Indice su Agronica_Log_Anagrafe", "754")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave Server 'Azienda_Timesheet_Tecnici' per Timesheet CAI", "127")

            Riga_Bug("Import Fabbricati Demetra",
                     "Fix su lettura chiave ricevuta da Demetra - split su underscore per ricavare piva, sa_cod e fabbricato_cod")

            Riga_Bug("Import Attivita Demetra",
                     "Fix su lettura chiave ricevuta da Demetra - split su underscore per ricavare piva, sa_cod e fabbricato_cod")

            Riga_Text("Movimenti Carico/Scarico",
                     "I doc di carico/scarico vengono aperti nella nuova gestione documenti, anche se il movimento è nato con la vecchia")

            Riga_Bug("Import Analisi Demetra",
                     "Fix serializzazione oggetto in entrata")

            '================================

            Riga_Data("17 Gennaio 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Lettura Formulati QdC", "05/09/2023")

            Riga_Requisiti("Migra",
                           "Indice su Agronica_Log_Anagrafe", "754")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave Server 'Azienda_Timesheet_Tecnici' per Timesheet CAI", "127")

            Riga_Bug("Anagrafica Impianti: ",
                     "Corretto caricamento combo organismo referente", "OROGEL", 0,
                     "non caricava correttamente i padri della gerarchia dell'azienda")

            '================================

            Riga_Data("16 Gennaio 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Lettura Formulati QdC", "05/09/2023")

            Riga_Requisiti("Migra",
                           "Indice su Agronica_Log_Anagrafe", "754")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave Server 'Azienda_Timesheet_Tecnici' per Timesheet CAI", "127")

            Riga_Text("Anagrafica Impianti: ",
                     "Gestito salvataggio codice 0 per la Certificazione del Prodotto", "OROGEL")

            '================================
            Riga_Data("11 Gennaio 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Lettura Formulati QdC", "05/09/2023")

            Riga_Requisiti("Migra",
                           "Indice su Agronica_Log_Anagrafe", "754")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave Server 'Azienda_Timesheet_Tecnici' per Timesheet CAI", "127")

            Riga_Bug("Budget: ",
                     "Fix parametrizzazione query leggiElencoBdgTestata per Agro_SQL_SaveNum con apici", "ASIPO")

            Riga_Text("ProfilazioneNG",
                      "- Nella creazione della tabella utenti, quando carica il numero di accessi utenti, esegue un
                      controllo sulla chiave 'LoginLogAccessoTutti' per verificare che il conteggio degli accessi sia
                      abilitato. In precedenza se il conteggio era disabilitato, il numero di accessi poteva assumere
                      solo valore 0 o 1, ora in caso sia disabilitato il campo numero accessi viene ignorato e
                      valorizzato sempre a 0. La data di ultimo accesso viene sempre valorizzata se è stato fatto
                      almeno un accesso.")

            Riga_Bug("Profilazione NG",
                      "- Carica le impostazioni utente considerando come default i valori indicati nelle impostazioni superuser.
                      - Fix caricamento impostazione utente filtro codici varietà: ora legge e salva correttamente i valori nella
                      tabella Utenti_Impostazioni_FiltroMono")

            Riga_Bug("Sementieri",
                     "Corretta lettura specie vegetali per crezione impianto da GIS, con sportello o mappatura libera")

            Riga_Text("ParametriAgenda_2010",
                      "Aggiunta la proprietà TipoRicetta",
                        noteTest:="Nessun test necessario")

            Riga_Text("Demetra - Import Attività", "WIP per primo test con Demetra (aggiunta gestione irrigazione)", "", 0, "", "Nessun test per assistenza")

            Riga_Text("Demetra - Import Fabbricati", "Aggiunti campi su EF", "", 0, "", "Nessun test per assistenza")

            Riga_Bug("QDC - Manodopera",
                     "Fix caricamento Patentino per i contatti che hanno codice fiscale salvato erroneamente con spazi/tab iniziali/finali", "Coldiretti", 28194)

            '================================

            Riga_Data("05 Gennaio 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Lettura Formulati QdC", "05/09/2023")

            Riga_Requisiti("Migra",
                           "GIS, Ottimizzazioni", "752")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave Server 'Azienda_Timesheet_Tecnici' per Timesheet CAI", "127")

            Riga_Text("Login",
                      "Aggiunta chiamata per calcolo lunghezza nuovo token durante refresh", "", 0, "", "Nessun test per assistenza")

            Riga_Text("Analisi Zoo",
                      "Gestione Core DataEntry Parametri Correzione", "Inalca", 7146)

            Riga_Text("Demetra - Import Attività", "WIP per primo test con Demetra (aggiunta gestione operatori)", "", 0, "", "Nessun test per assistenza")

            '================================

            Riga_Data("02 Gennaio 2024")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Lettura Formulati QdC", "05/09/2023")

            Riga_Requisiti("Migra",
                           "GIS, Ottimizzazioni", "752")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave Server 'Azienda_Timesheet_Tecnici' per Timesheet CAI", "127")

            Riga_Bug("GIS",
                     "Quando ci sono pochi poligoni, ma sparsi su molti layer, la funzione di lettura va in timeout. Ad esempio sul db del Sigaro Toscano, facendo login con il super user")

            '================================

            Riga_Data("20 Dicembre 2023")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Lettura Formulati QdC", "05/09/2023")

            Riga_Requisiti("Migra",
                           "GIS, Ottimizzazioni", "752")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave Server 'Azienda_Timesheet_Tecnici' per Timesheet CAI", "127")

            Riga_Text("Pratiche.asmx",
                      "Cambiato controllo per visualizzazione errore di mancata creazione pratica", "Coldiretti", 0, "", "Nessun test per assistenza")

            Riga_Bug("Kendo Grid NG",
                     "Fix memorizzazione Viste Grid dopo aggiornamento")

            Riga_Text("GIS - Ottimizzazioni",
                      "Aggiunta gestione colonna Poligono_GeoEntity_WKT per ridurre le elaborazioni lato server della decodifica del poligono", "ENI", 0, "La valorizzazione del campo è gestita tramite un trigger da applicare tramite migra", "utilizzo normale del GIS, non deve dare errore ne in singola azienda ne su visualizzazione totale")

            Riga_Text("Demetra - Import Macchine",
                      "Aggiornati metodi per scrittura modifica e cancellazione macchine per import da Demetra")

            Riga_Text("GIS - Ottimizzazioni",
                      "Ottimizzata query GIS per considerare solo i Layers visibili", "ENI", 0, "La valorizzazione del campo è gestita tramite un trigger da applicare tramite migra", "utilizzo normale del GIS, non deve dare errore ne in singola azienda ne su visualizzazione totale")

            Riga_Text("Demetra - Import Macchine",
                      "Aggiornati modelli EF per interscambio macchine D2G")

            Riga_Text("Profilazione NG",
                      "- Abilita rimozione e copia visibilità aziendale da altro utente
                      - Abilita modifica intervallo di validità permessi
                      - Aggiorna funzioni per copia impostazioni aziendali da altra azienda per gestire il tutto in un'unica transizione
                      - Aggiorna salvataggio impostazioni per gestire scrittura dei dati dell'impostazione 187 (Filtro SQL Materie Prime) sulla tabella Utenti_Impostazioni_FiltroMono
                      - Aggiorna funzione per il caricamento dei dati aggiuntivi server per le impostazioni utente")

            Riga_Bug("Dati Previsionali Colture",
                     "corretti metodi lettura rese previste")

            Riga_Bug("Anagrafica Imprese",
                     "aggiornato salvataggio indirizzo nuova impresa, per correzione scrittura contatto associato")

            Riga_Text("Budget",
                      "Caricamento numero ordini legati a elemento budget, per fare in modo, lato client, di non mostrare il pulsante genera ordine", "ASIPO", 0, "", "")

            Riga_Text("Timezone",
                      " FUNZIONALITA' NON ANCORA ATTIVATA. Archiviato supporto per trasformazione dati datetime con fuso orario corretto (da Client a Server e viceversa). ", "ASIPO", 0, "", "")

            Riga_Bug("GIS - Permessi Layer personalizzati",
                     "Corretto bug su gestione permessi layer personalizzati per bypassare utenti_visibilita_appoggio")

            Riga_Bug("GIS - Permessi Layer personalizzati",
                     "Corretto bug su errata gestione flag visibile-attivo su layer personalizzati che compometteva la visualizzazione")

            Riga_Text("Demetra - Import Analisi",
                      "Aggiunti metodi per scrittura/modifica/cancellazione analisi per import da Demetra")

            Riga_Text("Demetra - Import Attività", "WIP per primo test con Demetra", "", 0, "", "Nessun test per assistenza")


            Riga_Text("Demetra - Export Fabbricati",
                      "Aggiunti metodi per export verso Demetra")

            '================================

            Riga_Data("14 Dicembre 2023")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Lettura Formulati QdC", "05/09/2023")

            Riga_Requisiti("Migra",
                           "GIS, Banche", "750")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave Server 'Azienda_Timesheet_Tecnici' per Timesheet CAI", "127")

            Riga_Text("Demetra - Import Attività", "WIP per primo test con Demetra", "", 0, "", "Nessun test per assistenza")

            '================================

            Riga_Data("13 Dicembre 2023")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Lettura Formulati QdC", "05/09/2023")

            Riga_Requisiti("Migra",
                           "GIS, Banche", "750")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave Server 'Azienda_Timesheet_Tecnici' per Timesheet CAI", "127")

            Riga_Bug("Anagrafica NG", "Fix duplicazione codici nel salvataggio impresa")

            '================================

            Riga_Data("12 Dicembre 2023")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Lettura Formulati QdC", "05/09/2023")

            Riga_Requisiti("Migra",
                           "GIS, Banche", "750")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave Server 'Azienda_Timesheet_Tecnici' per Timesheet CAI", "127")

            Riga_Text("Anagrafica NG", "Fix codici nascosti nel salvataggio impresa")

            '================================

            Riga_Data("11 Dicembre 2023")

            Riga_Text("Anagrafica NG", "Fix codici nascosti nel salvataggio impresa")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Lettura Formulati QdC", "05/09/2023")

            Riga_Requisiti("Migra",
                           "GIS, Banche", "750")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave Server 'Azienda_Timesheet_Tecnici' per Timesheet CAI", "127")

            Riga_Bug("GIS",
                     "FIX sulle griglie degli indici delle Banche, NON è più supportata in kendo una colonna il cui titolo contiene le parentesi quadre")

            '================================

            Riga_Data("06 Dicembre 2023")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Lettura Formulati QdC", "05/09/2023")

            Riga_Requisiti("Migra",
                           "GIS, ZOO, Altro", "748")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave Server 'Azienda_Timesheet_Tecnici' per Timesheet CAI", "127")

            Riga_Text("Profilazione NG",
                     "- Aggiunge funzioni per caricamento transizioni di stato usate dai gruppi utenti
                     - Aggiunge gestione visibilità nulla
                     - Aggiunge funzione per copia impostazioni da altro utente o impresa")

            Riga_Bug("Profilazione NG",
                     "- Fix caricamento impostazioni imprese
                     - Blocca categoria di ricerca nella pagina del filtrone nuovo")

            Riga_Text("Quaderno di Campagna NG",
                     "Aggiunta la Strigliatura tra le operazioni ricettabili")

            Riga_Text("Agronica_Log_Anagrafe Fabbricati",
                      "Aggiunta gestione dei log per i fabbricati",
                      noteTest:="Non testabile da assistenza")

            Riga_Text("Sincro Demetra",
                      "Gestione sincro attività (WIP)",
                      noteTest:="Non testabile da assistenza")

            Riga_Text("Valutazioni",
                      "Core per valorizzazione conti in base all'attività", "Banca Cambiano")

            Riga_Bug("Visite NG",
                     "Fix: ora il centro del rilievo associato alla visita potrà differire da quello della visita")

            Riga_Bug("Visite NG",
                     "Fix: visibilità dei tecnici con superUser loggato")

            Riga_Text("GIS",
                      "Aggiunta gestione del MULTIPOLYGON in fase di registrazione del polugono GIS.",
                      "Coldiretti",
                      noteTecniche:="Gestione lato core per adeguamento interfaccia con Demetra, ma non è gestito lato UI",
                      noteTest:="nessun test")

            Riga_Text("PianoColturale",
                      "Aggiunta pèossibilità di bypassare la scrittura della tabella Agronica_Log_Anagrafe (solo servizi, no angular)",
                      "Coldiretti",
                      noteTest:="nessun test")

            Riga_Bug("EntityFramework",
                     "Aggiornato modello EF SpecieVegetali_Default")

            Riga_Text("Anagrafica Contatti",
                      "Aggiornato controllo inserimento Piva in creazione nuovo contatto",
                      "CAIAGROMEC")

            Riga_Text("Dati Previsionali Colture NG",
                      "Creati metodi per creazione cancellazione e modifica dati previsionali")

            Riga_Text("Import macchine Demetra",
                      "Predisposizione metodi per import macchine anagrafica D2G")

            Riga_Text("Migra",
                      "Aggiornata tabella SpecieVegetali_Default")

            Riga_Bug("Errore stampe QDC", "Inserito messaggio di avviso quando un utente tenta di entrare
                      nella sezione stampe dal QDC con un'azienda che non ha impianti (in precedenza restituiva un errore server non chiaro)")

            Riga_Bug("Notifiche - CUAA",
                     "Modficata gesitone transazione SQL registrazione notifica CUAA")

            '================================

            Riga_Data("01 Dicembre 2023")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Lettura Formulati QdC", "05/09/2023")

            Riga_Requisiti("Migra",
                           "GIS, ZOO, Altro", "746")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave Server 'Azienda_Timesheet_Tecnici' per Timesheet CAI", "127")

            Riga_Text("Generazione anteprima poligoni GIS - static maps",
                     "Gestita generazione immagine di anteprima poligono per Appezzamento/Impianto/Entrambi/Nessuno tramite chiave di configurazione LayerAbilitati su GIS_StaticMapCFG", "", 0, "la modifica viene recepita da APP/Web/Import PianocColturale/Sincro", "nessun test")

            '================================

            Riga_Data("29 Novembre 2023")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Lettura Formulati QdC", "05/09/2023")

            Riga_Requisiti("Migra",
                           "GIS, ZOO, Altro", "746")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave Server 'Azienda_Timesheet_Tecnici' per Timesheet CAI", "127")

            Riga_Bug("Lettura configurazione siti per NG",
                     "Lettura in cascata su conf siti server e conf siti superserver")

            Riga_Bug("Lettura giacenze Zoo",
                     "Fix lettura giacenze zoo per App")

            Riga_Bug("Semina con Aggiornamento Anagrafica",
                     "Bug fix semina con aggiornamento anagrafica, su appezzamenti con almeno due impianti associati",
                     "Copagri", 27652)

            '================================

            Riga_Data("27 Novembre 2023")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Lettura Formulati QdC", "05/09/2023")

            Riga_Requisiti("Migra",
                           "GIS, ZOO, Altro", "746")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave Server 'Azienda_Timesheet_Tecnici' per Timesheet CAI", "127")

            Riga_Bug("Ribaltamento in QdC NG",
                        "Sistemato messaggio di avversità mancante quando si ribalta in QdC un brogliaccio proveniente da APP o DEMETRA", noteTest:="Creare un brogliaccio su APP di un Trattamento Antiparassitario senza indicare l'avversità e ribaltarlo sul web in QdC verificando che venga mostrato il messaggio di avversità mancante.")

            '================================
            Riga_Data("22 Novembre 2023 bis")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Lettura Formulati QdC", "05/09/2023")

            Riga_Requisiti("Migra",
                           "GIS, ZOO, Altro", "746")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave Server 'Azienda_Timesheet_Tecnici' per Timesheet CAI", "127")

            Riga_Bug("Fix Visite NG",
                        "Sistemata visibilità utenti con visibilità totale", "OROGEL", 0, "", "Test utenti con vari tipi di visibilità")

            '================================

            Riga_Data("22 Novembre 2023")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Lettura Formulati QdC", "05/09/2023")

            Riga_Requisiti("Migra",
                           "GIS, ZOO, Altro", "746")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave Server 'Azienda_Timesheet_Tecnici' per Timesheet CAI", "127")

            Riga_Text("Anagrafica NG e Budget", "Inserito campo settimana trapianto/Semina e Settimana Raccolta in Anagrafica e Budget", "ASIPO", 0, "", "Campi solo in lettura (in griglia) dopo aver salvato il dato")

            Riga_Text("Dati Previsionali Colture",
                      "Aggiunti metodi per lettura indirizzo centro e dati previsionali")

            Riga_Bug("Codici Anagrafe",
                     "Corretto bug scrittura codici campo anagrafe budget e anagrafe effettiva")

            Riga_Text("Ereditatore",
                      "Implementata scrittura resa prevista su modifica multipla impianto")

            Riga_Text("RequisitiStabilimento",
                      "Aggiornati metodo e query di lettura")

            Riga_Text("Anagrafe NG",
                      "Aggiornato metodo scrittura modifica Fabbricati")

            Riga_Text("WidgetIndiciProduttivita",
                      "Aggiornato metodo lettura")

            Riga_Text("Localizzazione",
                      "Gestite traduzioni in inglese per le griglie delle ricette e del brogliaccio nel QdC Angular e Aspx")

            Riga_Text("GIS, Tutto",
                      "Messaggistica per notifica operazioni asincrone",
                      noteTest:="Nessun test richiesto")

            Riga_Text("GIS",
                      "Verifica esistenza e caricamento palette per Raster",
                      noteTest:="Nessun test richiesto")

            Riga_Text("GIS, Anagrafica",
                      "Eliminazione appezzamento, aggancio delle verifiche di sportello per i sementieri",
                      noteTest:="testare con attenzione inserimento, modifica, cancellazione di un appezzamento (impianto ed esercizio), sia da anagrafica, sia da GIS")

            Riga_Text("GIS",
                      "Esportazione Bulk file shape, modifica su entita con migliaia di valori in lista",
                      noteTest:="Testare di nuovo la procedura su Aboca in test interni")

            Riga_Text("GIS",
                      "",
                      noteTest:="Nessun test richiesto")

            Riga_Text("GIS",
                      "Piano concimazione asincrono",
                      noteTest:="Nessun test richiesto")

            Riga_Text("GIS",
                      "Lettura del livello di Zoom minimo in fase di visualizzazione totale da configurazione super-user",
                      noteTest:="Nessun test richiesto")

            Riga_Text("Profilazione NG",
                      "- Aggiorna chiamate gestione impostazioni
                       - Aggiorna salvataggio utenti: gestione associazione a gruppi e profili
                       - Aggiunge gestione e ricerca visibilità aziendale
                       - Aggiunge gestione visibilità nulla tramite piva fittizia ('###########')")

            Riga_Text("Gestione richieste/ordini di sementi",
                        "Aggiornate chiamate per gestione dati riguardanti richieste/ordini sementi")

            Riga_Bug("Lettura Anagrafiche zoo per app",
                        "Sistemate letture che davano errore", "INALCA", 0, "", "Nessun test da parte di assistenza")

            '================================

            Riga_Data("20 Novembre 2023")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Lettura Formulati QdC", "05/09/2023")

            Riga_Requisiti("Migra",
                           "Modificate Tabelle CAC_Codifica_Cultivar e CAC_Codifica_Veg_Cod", "745")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave Server 'Azienda_Timesheet_Tecnici' per Timesheet CAI", "127")

            Riga_Text("Visibilità Visite per utenti con nessuna azienda associata",
                      "Gli utenti con nessuna azienda associata hanno completa visibilità delle visite (come il superuser)")

            '================================

            Riga_Data("17 Novembre 2023")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Lettura Formulati QdC", "05/09/2023")

            Riga_Requisiti("Migra",
                           "Modificate Tabelle CAC_Codifica_Cultivar e CAC_Codifica_Veg_Cod", "745")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave Server 'Azienda_Timesheet_Tecnici' per Timesheet CAI", "127")

            Riga_Bug("Catasto.asmx",
                      "Mancata scrittura log_anagrafe impresexparticelle")

            Riga_Text("Passaggio Siti NG",
                      "Passaggio sito GiasOnline Vecchio")

            Riga_Text("Localizzazione",
                      "Aggiunte traduzioni in inglese e francese per la griglia del quaderno di campagna ed altre traduzioni comuni")

            '================================

            Riga_Data("15 Novembre 2023")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Lettura Formulati QdC", "05/09/2023")

            Riga_Requisiti("Migra",
                           "Modificate Tabelle CAC_Codifica_Cultivar e CAC_Codifica_Veg_Cod", "745")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave Server 'Azienda_Timesheet_Tecnici' per Timesheet CAI", "127")

            Riga_Text("Tipi Codifica Specie Vegetali e Varietà",
                      "Aggiunti tipi Codifica WMS-ONPLANT OROGEL FRESCO, WMS-ONPLANT OROGEL SURGELATO, DBWIN OROGEL FRESCO, DBWIN OROGEL SURGELATO e OROGEL-PASA")

            Riga_Bug("Nuovo Quaderno di Campagna NG",
                      "Corretta lettura default disciplinare predefinito da impostazione utente", "ASIPO", "27329")

            Riga_Bug("Traduzioni Operazioni Colturali (new!)",
                     "Fix traduzioni in inglese per la griglia del QdC")

            '================================

            Riga_Data("13 Novembre 2023")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Lettura Formulati QdC", "05/09/2023")

            Riga_Requisiti("Migra",
                           "Nuovi Permessi utente", "742")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave Server 'Azienda_Timesheet_Tecnici' per Timesheet CAI", "127")

            Riga_Bug("Creazione materie prime",
                     "Fix generazione descrizione di sementi e trasformati vegetali creati automaticamente",
                     noteTest:="Dalla creazione di prodotti nell'operazione di semina in alcuni casi non venivano valorizzate le descrizioni di
                     specie vegetale e cultivar, finendo per provocare la concatenazione di stringhe vuote separate da trattini.
                     Aggiunta chiamata per caricamento delle descrizioni")

            Riga_Text("Cac Codifica Prodotti",
                      "Aggiunto case per tipo codifica = non definito", "Coldiretti")

            '================================

            Riga_Data("10 Novembre 2023")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Lettura Formulati QdC", "05/09/2023")

            Riga_Requisiti("Migra",
                           "Nuovi Permessi utente", "742")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave Server 'Azienda_Timesheet_Tecnici' per Timesheet CAI", "127")

            Riga_Bug("GIS",
                     "In fase di copia layer personalizzati da SuperUser ad utente, implementato controllo dei permessi layer impostati su utente/gruppo")

            Riga_Text("GIS",
                     "Esportazione BULK con gestione via GSB in background ")

            Riga_Text("GIS",
                     "Conversione automatica tiff in formato COG per caricamento su layer ")

            Riga_Bug("Scarico Prodotti con Lotto",
                     "FIX ARCHIVIAZIONE PRECEDENTE - scarico prodotti, di qualsiasi categoria, con Lotto avente il carattere apostrofo (') nel nome", "Confagricoltura Umbria", 26951,
                     noteTest:="Caricare un prodotto inserendo nel Lotto il carattere apostrofo (') ex. --> ABC'DE, successivamente provare lo scarico in un operazione di campagna.
                     TESTARE SIA SUL VECCHIO QUADERNO CHE SUL QDC NG, per le seguenti categorie:
                     - Fertilizzanti
                     - Formulati, Coadiuvanti, Corroboranti/Fisiofarmaci
                     - Insetti utili (solo sulla nuova)
                     - Semente e Materiale Vivaistico
                     Per attivare l'utilizzo del lotto: 
                     - IMPOSTAZIONI UTENTE > IMPOSTAZIONI SUPERUSER > Filtro lotti Categorie Magazzino 
                     - impostazione 208 Utilizzo Magazzino attiva per categoria (non editabile da interfaccia)")

            Riga_Text("Utility Oggetti - ObjectExtension.vb",
                      "Aggiunte due tool utility per gli oggetti:
                      - PropertyCopier (=Reflection), copia le proprietà da un oggetto all'altro [parametro opzionale key_properties_to_exclude per indicare i nomi di proprietà che non devono essere sovrascritte/copiate (ex. PK, piva-sa_cod-appezza...)]
                      - RiempiCampi_StandardGias, passando un oggetto POCO riempe i campi standard GIAS (username e data creazione/modifica, inviato) [parametro opzionale edit_creazione per sovrascrivere o meno le proprietà di username/data creazione]")

            Riga_Text("Nuovo Quaderno di Campagna NG",
                      "Aggiunta lettura a scalare dei Comuni (prima li leggo dal catasto degli impianti selezionati, se non c'è catasto li prendo dall' indirizzo dell'appezzamento, se non ci sono indirizzi nell'appezzamento li leggo dall'indirizzo dei centri)",
                        noteTest:="Nessun test Necessario")

            Riga_Text("Modifiche per vivaio sementi",
                      "Modifiche a lettura/scrittura per vivaio sementi")

            Riga_Bug("Indirizzi Budget",
                     "Fix lettura indirizzi impianti budget",
                     noteTest:="Inserire degli indirizzi in un impianto budget, riaprendo in modifica devono riapparire correttamente",
                     noteTecniche:="Andava a leggere dalla tabella degli indirizzi senza filtrare per id_budget")

            Riga_Text("Cancellazione Impianto Budget",
                      "Aggiunto controllo sulla cancellazione di un impianto budget (sia dalla griglia che da edit completo): se sull'impianto sono state registrate delle Richieste di Materiale Vivaistico la cancellazione verrà bloccata con relativo messaggio.")

            Riga_Text("Ribaltamento Impianto Budget su Piano Colturale Effettivo",
                      "Aggiunta nuova funzionalità di ribaltamento impianti su Piano Colturale Effettivo: sotto nuovo permesso Budget_Ribaltamento_Su_Reale (534) apparirà sulla testa della griglia impianti il pulsante 'RIBALTA SU PIANO COLTURALE EFFETTIVO'.
                      Se si ha il permesso verranno mostrate due nuove colonne con i dati relativi all'operazione di ribaltamento: Ribaltato (SI/NO) e Data Ribaltamento.",
                      noteTest:="Breve riepilogo dei test necessari:
                      - Ribaltamento base (devono essere correttamente ribaltati tutti i dati di appezzamento-impianto-esercizio, particolare attenzione a codici ed indirizzi). Se un impianto è sotto un Campo verrà ribaltato anche questo. 
                      - Ribaltamento 'numero 2' di uno stesso impianto rimuovendo/scombinando/sostituendo completamente gli esercizi presenti 
                      - Gli impianti possono essere ribaltati infinite volte, ma la procedura verrà bloccata (per singolo impianto) se sul corrispettivo reale sono state registrate delle operazioni/ricette/costi. Deve comparire un messaggio di alert in questi casi.
                      - Cancellazione di un impianto già ribaltato (guardare colonna Ribaltato = SI), verrà chiesto all'utente se si desidera o meno cancellare anche il corrispettivo impianto reale: se esistono operazioni registrate, quest'ultimo non verrà cancellato (messaggio di avviso), mentre l'impianto budget verrà cancellato comunque.
                      - Idem sopra per la cancellazione di un Campo ribaltato (facendo sempre riferimento a registrazioni sugli impianti associati). 
                      PER CHIARIMENTI FARE RIFERIMENTO AD ANNA F.",
                      noteTecniche:="PER I TEST NECESSARIO PERMESSO Budget_Ribaltamento_Su_Reale (534), Impostazioni Utente -> Budget -> Ribaltamento Budget su Reale")

            Riga_Text("Nuovo Menu Agenda NG e Attuale Menu Agenda:",
                      "- Abilitato bottone Importa nel PUA per tutte e 6 le Fertilizzazioni")

            '================================


            Riga_Data("07 Novembre 2023")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Lettura Formulati QdC", "05/09/2023")

            Riga_Requisiti("Migra",
                           "Nuovi Permessi utente", "742")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave Server 'Azienda_Timesheet_Tecnici' per Timesheet CAI", "127")

            Riga_Bug("Audit Core BIZ",
                      "Fix per correggere la visualizzazione delle checklist SQNPI", "REUMBRIA", 27224)

            Riga_Text("Cac Codifica Prodotti",
                      "Aggiunto case per tipo codifica = non definito", "Coldiretti")

            '================================

            Riga_Data("06 Novembre 2023")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Lettura Formulati QdC", "05/09/2023")

            Riga_Requisiti("Migra",
                           "Nuovi Permessi utente", "742")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave Server 'Azienda_Timesheet_Tecnici' per Timesheet CAI", "127")

            Riga_Text("Adeguamenti Core",
                      "Versione rilasciata per adeguamento di modifiche operate dentro ai core che potrebbero essere richiamati anche tramite questo sito")

            '================================

            Riga_Data("03 Novembre 2023")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Lettura Formulati QdC", "05/09/2023")

            Riga_Requisiti("Migra",
                           "Nuovi Permessi utente", "742")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave Server 'Azienda_Timesheet_Tecnici' per Timesheet CAI", "127")

            Riga_Text("Aggiunto nuovo parametro opzionale in Movimenti.CaricaOperazioni", "", noteTest:="Nessun test Necessario")

            '================================

            Riga_Data("31 Ottobre 2023")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Lettura Formulati QdC", "05/09/2023")

            Riga_Requisiti("Migra",
                           "Nuovi Permessi utente", "742")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave Server 'Azienda_Timesheet_Tecnici' per Timesheet CAI", "127")

            Riga_Bug("Valutazioni",
                     "- Fix su visibilità azienda in griglia" &
                     "- Fix esportazione Excel" &
                     "- Fix su utilizzo di piano universale")

            Riga_Text("GIS",
                        "Informazioni su Raster")

            '================================

            Riga_Data("30 Ottobre 2023")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Lettura Formulati QdC", "05/09/2023")

            Riga_Requisiti("Migra",
                           "Nuovi Permessi utente", "742")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave Server 'Azienda_Timesheet_Tecnici' per Timesheet CAI", "127")

            Riga_Bug("Anagrafica NG ",
                        "Corretta chiamata leggi Vincoli", "", 0, "", "Nessun test da parte di assistenza")

            Riga_Text("Anagrafica Budget NG ",
                        "Allineate funzioni salvataggio impianti BDG con impianti reali", "", 0, "", "Nessun test da parte di assistenza")

            Riga_Text("GIS",
                        "Esportazione Bulk di un layer, prima versione", noteTest:="Testare con attenzione")

            Riga_Text("WebConfig",
                      "Aggiunto file separato sessionState", "coldiretti", 0, "", "Nessun Test")

            Riga_Text("Appezzamento",
                      "Aggiunta scrittura reg_impianti_codici personalizzati su scrittura piano colturale", "coldiretti", 0, "", "Nessun Test")

            '================================

            Riga_Data("26 ottobre 2023")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Lettura Formulati QdC", "05/09/2023")

            Riga_Requisiti("Migra",
                           "Nuovi campi MUZ", "741")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave Server 'Azienda_Timesheet_Tecnici' per Timesheet CAI", "127")

            Riga_Bug("Anagrafica NG ",
                        "Corretta chiamata leggi Vincoli", "", 0, "", "Nessun test da parte di assistenza")

            Riga_Text("Anagrafica Budget NG ",
                        "Allineate funzioni salvataggio impianti BDG con impianti reali", "", 0, "", "Nessun test da parte di assistenza")

            Riga_Text("GIS",
                        "Gestione intersezione con raster multipli per algoritmi deforestazione, gestione nuovo algoritmo deforestazione per anno")

            Riga_Text("Nuovi Menu NG Agenda,Ricette e Brogliaccio",
                        " - Aggiunta colonna 'Data Creazione'")

            '================================

            Riga_Data("25 Ottobre 2023")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Lettura Formulati QdC", "05/09/2023")

            Riga_Requisiti("Migra",
                           "Nuovi campi MUZ", "741")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave Server 'Azienda_Timesheet_Tecnici' per Timesheet CAI", "127")

            Riga_Text("Nuovo Menu Visite NG",
                        "Aggiunto il bottone 'Vai a Rilievo' nella griglia del menù Visite per modificare il rilievo agganciato alla visita.")

            Riga_Text("Nuovo Menu Agenda NG",
                        "Aggiunto il bottone 'Vai a Visita' nella griglia del menù Agenda per modificare la visita agganciata al rilievo.")

            Riga_Text("Nuovo Menu Agenda NG",
                      "- Fix dettaglio tecnico per operazioni colturali senza prodotti: non viene più mostrata la dicitura ALTRI FORMULATI
                       - Rimuove cambio cultura in menu agenda")

            Riga_Text("Anagrafica NG",
                        "Gestione stato impianto da nuova tabella FasiCicloColturale_Anagrafiche")

            Riga_Text("MUZ",
                        "Prima versione con la gestione delle MUZ")

            Riga_Bug("Visualizza Movimenti da Operazione Agenda ",
                        "Fix visualizzazione categorie non inerenti all'operazione corrente (segnalazione Test QdC Nuovo)")

            Riga_Text("Visite New",
                      "Versione aggiornata visite New", "", 0,
                      noteTecniche:="sono state modificate tante logiche rispetto alla prima versione presentata, si suggerisce nuovo incontro di allineamento e/o lettura manuali \\\\rubino2\\DOCUMENTAZIONE\\GIAS --- Clienti --- Consorzi Agrari d' Italia - CAI\\Visite\\Manuali x assistenza",
                      noteTest:="test totale: creazione, modifica, eliminazione visite con o senza rilievo associato, flussi da/verso app, visibilità operatori; riunioni ferie e permessi da CdG")

            Riga_Text("Importa Brogliacci da APP NG",
                      "- Fix passaggio parametri alle chiamate da angular")

            Riga_Text("Dashboard Header",
                      "- Aggiunto avviso per licenze e/o permessi scaduti, username e mail nel riquadro utente della header")

            Riga_Text("Budget",
                      "- Riallineamento caricamento dati impianti come da anagrafica reale", "", 0, "", "Nessun test")

            Riga_Text("WebConfig",
                      "Aggiunto file separato httpCookies", "coldiretti", 0, "", "Nessun Test")


            '================================

            Riga_Data("23 Ottobre 2023")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Lettura Formulati QdC", "05/09/2023")

            Riga_Requisiti("Migra",
                           "Nuovi campi visite QdC", "740")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave Server 'Azienda_Timesheet_Tecnici' per Timesheet CAI", "127")

            Riga_Text("File di progetto",
                        "A causa di un merge fatto male erano spariti dei file dal progetto")

            '================================

            Riga_Data("19 Ottobre 2023")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Lettura Formulati QdC", "05/09/2023")

            Riga_Requisiti("Migra",
                           "Nuovi campi visite QdC", "740")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave Server 'Azienda_Timesheet_Tecnici' per Timesheet CAI", "127")

            Riga_Text("Quaderno di Campagna NG",
                        "Cambiati parametri per le funzioni Core",
                        noteTest:="Nessun test Necessario")

            Riga_Bug("Valutazioni Rischio",
                     "Fix join tra testata e piano conti quando il piano conti è valido per tutte le imprese")

            Riga_Text("Visualizza Movimenti da Operazione Agenda ",
                        "Nascosto pulsante per stampa")

            Riga_Bug("Visualizza Movimenti da Operazione Agenda ",
                        "Con la nuova grafica: rimossa linguetta apertura sezione filtri 'volante' (da documento Test QDC Nuovo)")

            Riga_Bug("Modifica Operazione da Verifica Conformità",
                     "Fix Modifica Operazione da Verifica Conformità",
                     noteTest:="Provare tutti i giri:
                     - Menu Vecchio BS con i pulsanti --> pagina verifica conformità
                     - Menu Dashboard --> pagina verifica conformità
                     - Menu Agenda BS --> pulsante verifica conformità dentro la pagina
                     - Menu Agenda ANGULAR --> pulsante verifica conformità sulla testata griglia
                     Provare con e senza le chiavi/permessi angular (ex. senza abilitazione ad angular i multioperazione vengono bloccati)")

            Riga_Text("Carica Effluenti da Fertilizzazione",
                        "Se il salvataggio va a buon fine verrà dato un messaggio di conferma, dopodiché il popup verrà chiuso automaticamente (come prima)")

            Riga_Bug("Verifica Giacenze Magazzino -- PUA",
                     "Fix conversione da m3 a litri quando viene mostrato il messaggio di quantità insufficiente per lo scarico.",
                     noteTest:="Testare sia in inserimento prodotto che al salvataggio dell'operazione.")

            Riga_Bug("Codice zona in impianto",
                     "Corretto salvataggio e riapertura in modifica del campo codice zona che in alcuni casi non permetteva il salvataggio dell'impianto",
                     "Fruttagel")

            '================================

            Riga_Data("13 Ottobre 2023")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Lettura Formulati QdC", "05/09/2023")

            Riga_Requisiti("Migra",
                           "Nuovi campi visite QdC", "740")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave Server 'Azienda_Timesheet_Tecnici' per Timesheet CAI", "127")

            Riga_Text("Utenti_Visibilita.asmx",
                     "- gestita creazione password randomica" & vbCrLf &
                     "- gestito flag azienda_persona")

            Riga_Text("Visite NG",
                      "Fix visite con rilievo fasi fenologiche multiple", "", 0, "", "Vedere manuali Visite per i test, sono state riviste alcune logiche precedentemente testate e approvate")

            Riga_Bug("Modifica Validità Campo NG",
                     "Fix controlli sulle validità dei campi", "CAI", 25335)

            Riga_Bug("Quaderno di Campagna NG",
                     "Fix Controllo Blocco Fioritura")

            Riga_Text("Verifica Carenza Raccolta NG",
                      "La colonna Data Raccolta Utile viene sempre popolata con il trattamento più restringente effettuato, anche se la data operazione non rientra nella carenza indicata",
                      noteTest:="Un possibile esempio per chiarezza: 
                      Trattamento effettuato il 01/09/2023 con carenza 20gg.
                      Una raccolta effettuata in data 13/10/2023 mostrerà la colonna 'Data Utile Prima Raccolta' popolata con '21/09/2023 - Prodotto XYZ distribuito il 01/09/2023 (Carenza: 20 gg)'.
                      (Prima questo dato veniva mostrato solo se la raccolta era compresa fra il 01/09 ed il 20/09)")

            Riga_Text("Copia Operazioni - Multi Attività",
                      "Ora è possibile selezionare una data per l'intervento, quando si seleziona un multi attività",
                      noteTest:="Provare la copia di un multi attività (ex. trattamento+concimazione, con colonna Cod Multi Attività <> 0).
                      Selezionando le righe dalla pagina di copia deve apparire il 'Ripielogo Interventi' dopo è possibile scegliere la data in cui si vuole copiare l'operazione multi selezionata. ")

            Riga_Text("Carica Effluenti da Ferilizzazione",
                      "Ora il carico viene salvato con i titoli di N indicati nel PUA")

            Riga_Text("Localizzazione Kendo",
                      "Impostato caricamento in lingua dei messaggi di kendo", noteTecniche:="le impostazioni di cultura date e numeri rimangono in italiano")

            Riga_Text("Localizzazione",
                      "Importate traduzioni inglese per widget nuova dashboard e fix vari")

            '================================

            Riga_Data("10 Ottobre 2023")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Lettura Formulati QdC", "05/09/2023")

            Riga_Requisiti("Migra",
                           "Nuovi campi visite QdC", "740")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave Server 'Azienda_Timesheet_Tecnici' per Timesheet CAI", "127")

            Riga_Bug("Gestione imprese/centri/appezzamenti codici",
                     "Fix su salvataggio elementi anagrafici da App")

            '================================

            Riga_Data("10 Ottobre 2023")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Lettura Formulati QdC", "05/09/2023")

            Riga_Requisiti("Migra",
                           "Nuovi campi visite QdC", "740")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave Server 'Azienda_Timesheet_Tecnici' per Timesheet CAI", "127")

            Riga_Text("Lettura Effluenti",
                      "Aggiunto nuovo parametro objParametri_Super_Server alla funzione AgronicaCoreWebService.PianoConcimazione_WS.Effluenti per la lettura degli Effluenti")

            Riga_Text("Imprese.asmx",
                      "Fix chiamata leggi imprese", "", 0, "", "Nessun test")

            Riga_Text("Imprese Ng",
                      "Aggiunta lettura colonne Data_Inizio_Prevista, Data_Fine_Prevista, Data_Fioritura_Prevista")

            Riga_Text("Widget Ng",
                      "Aggiunto WS per lettura IndiciProduttivita e modelli WidgetIndiciProduttivita", "Intesa San Paolo")

            Riga_Text("Dati Previsionali Colture",
                      "Aggiunto WS per lettura dat previsionali colture e modelli", "Asipo")

            Riga_Text("Dati Previsionali Colture",
                      "Aggiunte colonne in tabella SpecieVegetali_Default e creata tabella SpecieVegetali_DefaultGlobali su Metaschema", "Asipo")
            Riga_Text("Modifica nome header",
                      "Cambiato il nome che si vede nella header nel menu a tendina del profilo, ora si vedrà Nome Cognome o Rag. Soc. + Username Commerciale")

            Riga_Bug("Verifica Carenza Raccolta NG",
                     "Ottimizzato controllo verifica raccolta",
                     "CAI", 26539)

            Riga_Text("AgronicaCoreUtentiBIZ/Utenti_R.asmx",
                      " Unificata procedura InizializzaTabellaUtentiVisibilitaAppoggio (non usa più la chiave Utenti_Visibilia_Appoggio_Con_EF) chiamta dal webmethod Autenticazione", "", 0, "", "")

            Riga_Text("Widget",
                      "Aggiunta lettura KPI per Indici Produttivita", "Intesa San Paolo")

            Riga_Text("Visite",
                      "Aggiunta specie animale, ristrutturazione per rendere non obbligatoria l'attività QdC collegata (questo però è WIP)", "", 0, "", "Vedere manuali Visite per i test, sono state riviste alcune logiche precedentemente testate e approvate")

            Riga_Text("Anagrafica Angular",
                      "Gestione Codici anagrafici, implementata gestione con date di validita",
                      "",
                      0,
                      "",
                      "In tutte gli elementi dell'anagrafica che hanno una tabella codici verificare che sia 
                      possibile inserire più di una volta lo stesso codice ma in periodi temporali diversi e
                      non sovrapposti")

            Riga_Text("Anagrafica Angular",
                      "Aggiunta dropdown per la selezione del Codice Zona negli impianti")

            Riga_Text("AgendaStatistiche",
                     "Aggiunto metodo LeggiAgendaStatistiche per il report campagna")

            '================================

            Riga_Data("06 Ottobre 2023")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Lettura Formulati QdC", "05/09/2023")

            Riga_Requisiti("Migra",
                           "Nuove tabella GIS_Bookmark", "739")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave 'pathFileRaster_GoogleEarthEngine' per GIS " &
                           "Nuove chiavi 'Prefisso_PIVA_Generica_Da_Assegnare' per personalizzazione prefisso PIVA generata automaticamente nella creazione imprese, e 'Blocco_Inserimento_PIVA_Impresa' per blocco inserimento manuale PIVA in creazione impresa ", "126")

            Riga_Text("Passaggio da siti Angular",
                      "Fix passaggio siti: dashboard angular verso altri, quando l'utente usernmame contiene una @",
                      "Coldiretti ", 26590)

            '================================
            Riga_Data("03 Ottobre 2023")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Lettura Formulati QdC", "05/09/2023")

            Riga_Requisiti("Migra",
                           "Nuove tabella GIS_Bookmark", "739")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave 'pathFileRaster_GoogleEarthEngine' per GIS " &
                           "Nuove chiavi 'Prefisso_PIVA_Generica_Da_Assegnare' per personalizzazione prefisso PIVA generata automaticamente nella creazione imprese, e 'Blocco_Inserimento_PIVA_Impresa' per blocco inserimento manuale PIVA in creazione impresa ", "126")

            Riga_Text("GIS.aspx",
                      "Gestita memorizzazione dei parametri GEE di lettura dei rasters")

            Riga_Text("Creazione Impresa NG",
                      "Aggiunta gestione personalizzazione prefisso PIVA generata automaticamente, con default ad 'F'",
                      noteTecniche:="La personalizzazione va inserita in configurazione siti, sotto la chiave 'Prefisso_PIVA_Generica_Da_Assegnare'")

            Riga_Text("MenuAgenda NG",
                      "Corregge il caricamento dei macchinari selezionabili nella modifica massiva delle attività agenda")

            Riga_Text("Raccolta NG",
                      "Corregge il salvataggio del campo oraIngresso dei carichi di raccolta")

            '================================

            Riga_Data("02 Ottobre 2023 BIS")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Lettura Formulati QdC", "05/09/2023")

            Riga_Requisiti("Migra",
                           "Nuove tabella GIS_Bookmark", "739")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave AgronicaSpecialNavigationCFG + modifiche a chiavi per GIS", "125")

            Riga_Text("GIS.aspx",
                      "Piccoli fix (2) su gestione caricamento file in GCP")

            '================================

            Riga_Data("02 Ottobre 2023")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Lettura Formulati QdC", "05/09/2023")

            Riga_Requisiti("Migra",
                           "Nuove tabella GIS_Bookmark", "739")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave AgronicaSpecialNavigationCFG + modifiche a chiavi per GIS", "125")

            Riga_Text("GIS.aspx",
                      "Piccoli fix su gestione caricamento file in GCP")

            Riga_Bug("MenuAgenda NG",
                      "Corretto dettaglio tecnico per ricevimenti DDT", , 26376)

            '================================

            Riga_Data("29 Settembre 2023")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Lettura Formulati QdC", "05/09/2023")

            Riga_Requisiti("Migra",
                           "Nuove tabella GIS_Bookmark", "739")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave AgronicaSpecialNavigationCFG + modifiche a chiavi per GIS", "125")

            Riga_Text("GIS.aspx",
                      "Lettura/Invio dei dati per generazione della mappa di prescrizione a rateo variabile",
                      noteTest:="Da testare dopo formazione di Vanni su nuova piattaforma SAT/GEE")

            Riga_Text("Dati Rete Acqua",
                      "Riorganizzate funzionalità per leggere\scrivere su tabelle IOT_* anzichè prendere i dati dal meteo per la parte relativa alla rete idrica", "MIDAR", 0, "", "no test"
                      )

            Riga_Text("Utenti visibilità appoggio",
                      "- Aggiunta API per creazione utente e assegnazione visibilità imprese ")

            Riga_Text("Profilazione NG",
                      "- Fix chiamate API 
                       - Crea API per modifica lingua utente")

            Riga_Text("MenuAgenda NG",
                      "- Fix dettaglio tecnico per operazioni di rilievo fasi fenologiche e avversità")

            Riga_Text("Integrazione Demetra",
                      "aggiunti Web Service per verifica esistenza utente gias, aggiunti ws per creazione pratiche e avanzamento di stato delle stesse, modifica lettura ricezionenotifiche da sistemi esterni",
                      "Coldiretti", 0, "", "no test")

            Riga_Text("Sementi Germinabilità",
                     "Aggiunta nuova colonna su Materie_Prime e codice per gestirla")

            Riga_Text("Sementi Materiale Vivavistico",
                     "Aggiunto codice per gestione nuova categoria di richiesta materiale vivavistico e passaggio dati tra NG e planning")

            Riga_Bug("Cancellazione Multi Attività",
                     "Il messaggio di conferma indica ora correttamente tutte le operazioni correlate a quella che si sta cercando di cancellare")

            Riga_Text("Affinamento ribaltamento ricette da APP a QdC NG",
                      "-fix disciplinari multi attivita
                      -avversita non obbligatoria")

            Riga_Text("Affinamento legame visita con attivita collegata in nuove visite NG",
                      "-inserimento cau_mov_rif 6852 in riferimento visita-attivita collegata")

            Riga_Text("Fix generali Visite e QdC",
                      "aggiunti fix per alcuni aspetti di Visite e QdC")


            Riga_Bug("Lettura Impianti Angrafica NG",
                      "Corretto calcolo Piante Impianto")

            Riga_Text(
                "RequisitiStabilimento NG",
                "Aggiunto WS RequisitiStabilimento per nuovo componente Angular, aggiunti inoltre metodi in ImpreseContratti e ImpreseContrattoFasi per lettura e salvataggio dati RequisitiStabilimento",
                "ASIPO")

            Riga_Text(
                "Budget",
                "Aggiunta chiamata per lettura elenco BudgetTestata inerente sviluppi RequisitiStabilimento",
                "ASIPO")

            Riga_Text("Localizzazione:",
                      "- Aggiunta gestione traduzioni per le unità di misura ed i rapporti contabili. Aggiunte relative traduzioni in inglese e francese",
                      "ENI")

            Riga_Text("Altre Lavorazioni QdC",
                      "proposte solo le attività effettivamente collegate al lav_cod 162")

            Riga_Text("Leggi Centri Zoo",
                      "Caricamento Combo per solo centri che hanno una stalla")

            Riga_Text("Utenti_Profili",
                      "Ottimizzato reset DataUltimoRiportoUtentiVisibilitaAppoggio in fase di creazione Imprese/Centri/gerarchiaImprese")

            Riga_Text("Insert Utenti Visibilita appoggio",
                      "Inserito try catch vuoto in inserimento di utenti visibilita appoggio quando si inserisce una nuova impresa o un nuovo centro")

            '================================

            Riga_Data("20 Settembre 2023")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Lettura Formulati QdC", "05/09/2023")

            Riga_Requisiti("Migra",
                           "Nuove tabella GIS_Bookmark", "738")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave LinkAgronicaDomandaIrrigua", "124")

            Riga_Bug("GIS.asmx",
                      "- Cambiata creazione input per calcolo rateo variabile con GEE")

            Riga_Bug("Budget",
                      "- Fix e allineamento salvataggio con anagrafica")

            Riga_Bug("Filtrone",
                      "- Fix query fabbricati")

            Riga_Text("Utenti visibilità appoggio",
                      "- migliorata velocità login ")

            '================================

            Riga_Data("19 Settembre 2023")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Lettura Formulati QdC", "05/09/2023")

            Riga_Requisiti("Migra",
                           "Nuove tabella GIS_Bookmark", "738")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave LinkAgronicaDomandaIrrigua", "124")

            Riga_Bug("GIS.asmx",
                      "- Fix su lettura della mappa di prescrizione su pive fittizie ...")

            '================================

            Riga_Data("14 Settembre 2023")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Lettura Formulati QdC", "05/09/2023")

            Riga_Requisiti("Migra",
               "Nuove tabella GIS_Bookmark", "738")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave LinkAgronicaDomandaIrrigua", "124")

            Riga_Bug("GIS.asmx",
                      "- Fix su procedura di riporto dei dati da Google Cloud Platform ")

            '================================

            Riga_Data("13 Settembre 2023")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Lettura Formulati QdC", "05/09/2023")

            Riga_Requisiti("Migra",
               "Nuove tabella GIS_Bookmark", "738")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave LinkAgronicaDomandaIrrigua", "124")

            Riga_Bug("Imprese.asmx",
                      "- Fix LeggiImpreseConFiltroUtente_Modello che si schiantava sulla colonna CUAA")

            '================================

            Riga_Data("08 Settembre 2023")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Lettura Formulati QdC", "05/09/2023")

            Riga_Requisiti("Migra",
               "Nuove tabella GIS_Bookmark", "738")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave LinkAgronicaDomandaIrrigua", "124")

            Riga_Bug("Caricamento imprese per Albero",
                      "Query per caricamento imprese in JOIN con la Utenti_Visibilita_Appoggio")

            Riga_Text("Raccolte NG",
                      "Fix valorizzazione data per carichi di magazzino legati alle raccolte")

            Riga_Bug("MenuAgenda NG",
                      "Fix caricamento lista persone per modifica massiva operazioni")

            Riga_Bug("GIS",
                      "CoreWS - GIS - Bug fix su rilievi vegeto produttivi: Impossibile associare l'identificatore in più parti 'mdt.FF_Classe'.",
            noteTest:="Verificare nuovamente la visualizzazione dei rilievi di avversità e fenologici, anche selezionando da casella a discesa la relativa tipologia di layer.")

            Riga_Text("Menu Visite NG",
                      "migliorie alla query delle visite e alla gestione degli utenti non correttamente figurati")

            Riga_Text("GIS",
                      "Gestione dei bookmark e dell'ultima posizione", "", 0, "", "nessun test")

            '================================

            Riga_Data("05 Settembre 2023")

            Riga_Requisiti("AgronicaWebService2010",
                           "per Lettura Formulati QdC", "05/09/2023")

            Riga_Requisiti("Migra",
                           "Nuove tabella tabella SistemiEsterni_RicezioneNotifiche", "737")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave LinkAgronicaDomandaIrrigua", "124")

            Riga_Text("Catasto.asmx",
                      "- aggiunto nuovo metodo massivo per modifica\creazione\cancellazione particelle catastali ", "DEMETRA", 0, "", "nessun test")

            Riga_Text("Notifiche.asmx",
                      "- Nuovo Webservice per gestire la ricezione da notifiche da sistemi esterni", "DEMETRA", 0, "Prima archiviazione di un pacchetto di sviluppi più ampio", "nessun test")

            Riga_Text("QdC NG",
                      "- cambiato metodo di lettura del raccoglitore per esigenze tecniche", "", 0, "", "Provare a creare/modificare/cancellare della attività multi operazione, tutto deve funzionare (popolare tutti i dati, compresi operatori/macchine/note/posizione")

            Riga_Text("Menu Visite",
                      "cambiata la query per il menu visite")

            Riga_Bug("QdC NG",
                     "- Fix semine per sementi con codice 1", "", 0, "", "Testare le semine senza prodotti e testare le semine con un prodotto con codice 1 (BFENI: ricino)")

            Riga_Text("Widget GHG e Stime di Produzione",
                      "- creazione CoreWs per i due Widget", "", 0, "", "")

            Riga_Text("Crea Prodotti",
                      "Fix valorizzazione valore restituito")

            Riga_Text("Profilazione NG",
                      "- Aggiornamento e fix chiamate dopo passaggio API")

            Riga_Bug("Prodotto in Esercizio",
                      "- fix salvataggio", "ASIPO", 0, "", "Nessun test")

            Riga_Text("Gestione Gruppi di raccolta",
                      "- gruppi di raccolta in imprese e in esercizio", "ASIPO", 0, "", "Nessun test")

            Riga_Text("Login Utente",
                      "- Migliorata query di Aggiornamento Visibilita Utente")

            Riga_Text("GruppiRaccolta",
                      "Aggiunta gestione gruppi raccolta per imprese, esercizi budget ed esercizi Ng")

            Riga_Text("GruppiRaccolta",
                      "Implementata gestione scrittura, modifica e cancellazione gruppi raccolta per pagina gruppi-raccolta Ng")

            Riga_Text("GIS",
                      "Gesione del carattere § su chiamate a Google Cloud platform")

            Riga_Text("Menu Visite",
                      "aggiunti nuovi campi nella select della query")

            Riga_Text("QdC NG",
                      "Aggiunta gestione nuova attività CONFUSIONE / DISORIENTAMENTO SESSUALE sul nuovo quaderno NG",
                      noteTest:="Da testare tutto: operazione agenda, ricette/brogliacci, ribaltamenti, multicentro, copia operazione, verifica conformità dentro l'operazione e a posteriori, stampa schede campagna/multicentro/bio, stampa ricette/odl")

            '================================

            Riga_Data("30 Agosto 2023")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Nuove tabelle AgronicaDomandaIrrigua 
                                  Nuova colonna per Materie_prime", "735")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave LinkAgronicaDomandaIrrigua", "124")

            Riga_Text("Utenti_Visibilita_Appoggio",
                      "- Popolamento per 'delta' della tabella, vengono cancellati solo i record effettivamente non più visibili e inseriti solo quelli non già presenti ")

            '================================

            Riga_Data("29 Agosto 2023")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Nuove tabelle AgronicaDomandaIrrigua 
                                  Nuova colonna per Materie_prime", "735")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave LinkAgronicaDomandaIrrigua", "124")

            Riga_Text("Utenti visibilità login",
                      "- tolto XLOCK in query di cancellazione utenti_visibilità_appoggio " &
                      "- BulkCopy per inserimento massivo in utenti_visibilità_appoggio ")

            Riga_Text("Cancellazione contatto NG",
                      "- Gestita cancellazione utente associato",
                      noteTecniche:="tabella ContattiXUtentiGias")

            Riga_Bug("Modifica Multipla Attività (NG)",
                     "Corretta chiamata API")

            Riga_Text("Profilazione NG",
                      "Aggiornamento e fix chiamate dopo passaggio API")

            '================================

            Riga_Data("24 Agosto 2023")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Nuove tabelle AgronicaDomandaIrrigua 
                                  Nuova colonna per Materie_prime", "735")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "116")

            Riga_Requisiti("Configurazione_Siti",
                           "Nuova chiave LinkAgronicaDomandaIrrigua", "124")

            Riga_Text("Tecnologia Sementi",
                     "Aggiunta nuova colonna su Materie_Prime e codice per gestirla")


            Riga_Bug("MenuAgenda New",
                      "- Fix costruzione dettaglio tecnico ricette: nascoste le quantità dose/Ha o dose/Hl per formulati e fertilizzanti.
                      Vengono ora mostrate le sole quantità totali per ogni prodotto.
                      - Fix costruzione dettaglio tecnico per ricevimenti ed emissioni DDT")

            Riga_Text("Rilievi NG", "Rilievo avversità in campo: è possibile ora selezionare la stessa avversità con due unità di misura diverse (richiesta di Donato Cillis)")

            Riga_Bug("Invio Ricette ad APP",
                     "Fix invio ricette ad APP (in modifica operazione dava errore vuoto)")

            Riga_Text("Documentale e checklist - creazione di un nuovo elemento",
                        "Aggiunto parametro per impostare il blocco della pratica alla sua creazione")

            Riga_Text("Unità di misura alternativa",
                      "Gestione Unità di misura alternativa in Impianti")

            Riga_Text("Utility.asmx",
                      "Aggiunti ws per richiamo pagine nuovo sito AgronicaDomandaIrrigua", "MIDAR", 0, "compatibilità per vecchia UI e nuova UI Angular", "nessun test")

            Riga_Text("Crea Prodotti",
                      "Riattiva API creazione prodotti e aggiorna il valore restituito in modo da distinguere le sementi dai trasformati creati")

            Riga_Text("Menu Visite - Operatori e Aziende",
                      "aggiunti i core e API per popolare le DDL operatore e aziende nel form di modifica e nuova visita")

            Riga_Text("Contatti.asmx",
                      "Nuovo metodo per lettura contatti anagrafica macchine",
                      noteTecniche:="WebMethod Leggi_Contatti_Macchine")

            Riga_Text("Quaderno di Campagna NG",
                      "- Aggiunta indicazione del Preventivato per le Ricette nelle colonne dell'N [Kg/Ha],P [Kg/Ha],K [Kg/Ha],Mg [Kg/Ha],Cu [Kg/Ha] nella griglia degli Impianti " &
                      "- Aggiunta indicazione della Dose/Ha di Prodotto Preventivato Residua per le Ricette di Fertilizzazione")

            Riga_Text("Rilievi fasi fenologiche APP+NG",
                      "- I rilievi con n fasi fenologiche creati su APP vengono creati su WEB come n rilievi accomunati dal codice multiattività (come avviene se venissero creati direttamente su WEB) ",
                       noteTest:="Creare un rilievo con 2 fasi fenologiche e vedere che viene importato come 2 rilievi accomunati dal codice multiattività; aggiungere/togliere una fase fenologica su APP e vedere che gli n rilievi precedente creati su web vengono modificati di conseguenza (vengono cancellati e reinseriti, pertanto cambiano gli id_agenda e il codice multiattività")

            Riga_Text("Agenzie Invio Email Ricette",
                        "Nuovi metodi per la lettura dati ricette e dati agenzie")

            Riga_Bug("Log Ricetta App",
                        "Aggiunto object_data al log delle ricette create da App")

            Riga_Bug("Anagrafica NG",
                      " Controllo su date di validita di campo e appezzamento in fase di creazione/modifica di appezzamento ",
                      "",
                      0,
                      "",
                      "Provare a creare/moificare appezzamenti cambiando la validita con campi associati, deve dare errore se le date di validità dell'appezzamento non sono incluse nelle date di validità del campo ")

            Riga_Bug("GIS",
                     "Importazione di shape su layer generici: se le colonne del DBF hanno uno nome ""catastale"", ad esempio ""Foglio"", allora l'importazione può andare in errore in fase di interpretazione dei dati catastali.",
                    noteTest:="Nessun test richiesto, il bug è stato riscontrato nell'importazione delle aree omogenee del ""Sigaro Toscano""   ")

            Riga_Text("GIS",
                        "Cancellazione di tutti i dati sui soli layer personalizzati",
                        noteTest:="Creare un layer, fare un paio di disegni, utilizzare il pulsante rosso 'Elimina Dati Layer' dalla scheda 'Dati'. Saranno eliminati tutti di dati del layer. Testare il fatto che i soli utenti autorizzati in cancellazione vedano il pulsante. ")

            Riga_Text("Anagrafica NG",
                        "Aggiunta colonna Organismo Referente in griglia impianti")

            Riga_Bug("Semina con Frazionamento / Aggiornamento Anagrafica NG",
                        "Aggiunto controllo sulla validità del prodotto quando si effettua una semina / trapianto con aggiornamento anagrafica: 
                        se il prodotto non ha la specie correttamente impostata in anagrafica, ne verrà impedito l'utilizzo.")

            '================================

            Riga_Data("01 Agosto 2023")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Nuovi records e tabelle per GIS, nuovi campi per Visite", "733")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "114")

            Riga_Requisiti("Configurazione_Siti",
                           "Configurazione salvataggio layer raster", "121")

            Riga_Text("Passaggio a Core API", "Modificati tutti i metodi per passaggio a CoreAPI")

            Riga_Bug("GISNg",
                     "Corretta lettura proprietà appIdRate per features")

            '================================

            Riga_Data("01 Agosto 2023")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Nuovi records e tabelle per GIS, nuovi campi per Visite", "733")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "114")

            Riga_Requisiti("Configurazione_Siti",
                           "Configurazione salvataggio layer raster", "121")

            Riga_Bug("Creazione Magazzino da Creazione Centro NG",
                     "Fix creazione Magazzino collegato ad un nuovo Centro Aziendale NG: il fabbricato ad esso collegato viene creato con la corretta denominazione",
                     "Consorzi Agrari d’Italia - CAI", 25141,
                     noteTest:="Creare un centro aziendale da angular (sia in riga, che creazione completa): il fabbricato creato ad esso associato deve avere denominazione default 'Magazzino n.01'")

            Riga_Bug("Caricamento Disciplinari multi attività (trattamento+fertilizzazione) NG",
                     "Vengono ora caricate correttamente le intersezioni fra i disciplinari delle due tipologie di operazione")

            Riga_Bug("Menu Agenda NG",
                     "Fix caricamento Principio Attivo nella colonna 'Dettaglio Tecnico' per i Trattamenti.")

            Riga_Bug("Invio Ricette ad APP",
                     "Non viene più mostrato l'errore delle MultiAttività a sproposito.")

            Riga_Bug("Quaderno di Campagna NG",
                     "Fix caricamento Data Scadenza Patentino per gli operatori in modifica di una Operazione.")

            Riga_Bug("Copia/Sposta Appezzamenti - Problema Poligoni Impianti (Anagrafica NG)",
                     "Quando si copia/sposta un appezzamento vengono ora copiati/spostati anche i poligoni associati agli impianti")

            Riga_Bug("Errore Cambio Specie (Anagrafica NG)",
                     "L'errore non viene più mostrato a sproposito. (Controllava erroneamente il campo della varietà).",
                     noteTest:="Deve essere permesso cambiare la varietà da interfaccia sempre, a prescindere dalla presenza o meno di operazioni agenda.")

            Riga_Bug("Griglia QdC NG - Principi Attivi in Dettaglio Tecnico",
                     "Ora vengono mostrati correttamente i PA dei trattamenti, sempre",
                     noteTest:="1) eseguire un trattamento ed un carico di fitofarmaci nello stesso giorno
                     2) filtrare la griglia delle operazioni una volta con solo 'Trattamenti', ed una volta senza filtri sul tipo operazione.
                     In entrambi i casi devono essere visibili il principi attivi del trattamento.")

            '================================

            Riga_Data("25 Luglio 2023")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Nuovi records e tabelle per GIS, nuovi campi per Visite", "733")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "114")

            Riga_Requisiti("Configurazione_Siti",
                           "Configurazione salvataggio layer raster", "121")

            Riga_Bug("Anagrafica impianti angular",
                     "corretto caricamento organismo referente")

            '================================

            Riga_Data("20 Luglio 2023")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Nuovi records e tabelle per GIS, nuovi campi per Visite", "733")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "114")

            Riga_Requisiti("Configurazione_Siti",
                           "Configurazione salvataggio layer raster", "121")


            Riga_Bug("QdC - Lavorazioni semplici",
                      "Fix caricamento specie, c'era un bug in caricamento dovuto ai nuovi lavori sulle visite")


            '================================

            Riga_Data("19 Luglio 2023")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Nuovi records e tabelle per GIS, nuovi campi per Visite", "733")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "114")

            Riga_Requisiti("Configurazione_Siti",
                           "Configurazione salvataggio layer raster", "121")

            Riga_Text("Anagrafica impianti angular",
                     "- introdotto parametro 'Vincoli' che sostituisce Regolamento-Disciplinare-Reg.Fertilizzazioni")

            Riga_Text("Impostazioni utente app",
                     "create nuove impostazioni utente app per lavorazioni in campo e gestione posizione nel disegno poligono")

            Riga_Bug("Anagrafica Imprese",
                     "corretto bug per visualizzazione albero in creazione impresa", "", 0,
                     "In creazione dell'impresa se il tipo impresa era cooperativa/op/etc metteva in gerarchiaImprese il flag foglia = 0 
                     e fino a che non si metteva un'impresa sotto a quella appena creata non veniva mostrata nell'albero ",
                     "Su un archivio con poche imprese aprire 2 finestre una con l'albero delle imprese (filtro di ricerca) e l'altra con 
                     l'anagrafica e provare a creare/modificare/eliminare aziende e relative gerarchie e vedere se di volta in volta 
                     l'albero è coerente con le operazioni fatte")

            Riga_Text("GISWS",
                     "Aggiunta nuova API per recupero static map su elementografico gis")

            Riga_Text("Invio Ricette ad App NG",
                      "Aggiunta gestione controlli sull'invio delle Ricette ad APP.
                      Sarà impossibile inviare la ricetta se:
                      - Si tratta di una multi attività (due operazioni nella stessa ricetta)
                      - Il magazzino utilizzato NON è visibile dall'APP (opzione attivabile dall'anagrafica del magazzino)
                      - Se lo stesso prodotto è stato scaricato da due magazzini diversi
                      - Se lo stesso prodotto è stato scaricato dallo stesso magazzino, ma da lotti diversi.
                      Il controllo scatta o da menu (selezionando n ricette e cliccando il pulsante 'Invia all'APP' sulla griglia), oppure da dentro la ricetta, al salvataggio, se si è sputata l'opzione di invio.
                      Implementata con la gestione traduzioni.")

            Riga_Text("Redirect alla Trattamenti_2, operazioni non gestite",
                      "Per le seguenti operazioni è stato implementato il redirect alla pagina vecchia (Trattamenti_2):
                      - Distribuzione ammendati con disciplinare PAN 2018-2021 ER (questo disciplinare mostra una combo animali che non gestiamo sulla nuova)
                      - Visite con più di un'operazione agganciata (questo redirect sarà testabile una volta pronta l'interfaccia)")

            Riga_Bug("Controllo Movimenti Magazzino Cancellazione Azienda NG",
                     "Quando si cancella l'azienda vengono controllati correttamente anche i movimenti di magazzino. Se esistono, l'operazione viene bloccata.",
                     "Orogel", 24751)

            Riga_Text("QdC New - Ribaltamento ricette/brogliacci da app a nuovo QdC",
                     "Ribaltando una ricetta/brogliaccio proveniente da app su nuovo QdC si apre la maschera di inserimento agenda, precompilata con i dati inseriti nella app.
                     Poiché nella app ci sono meno controlli rispetto al web, i dati inseriti potrebbero essere non applicabili, in tal caso il sistema li segnala. In particolare:
                     - formulato non corretto: il formulato non è compatibile con gli altri dati (specie, disciplinare, avversità, etc) --> va sostituito prima di salvare
                     - formulato ambiguo: il formulato non ha tutti i dati per essere identificato univocamente sul web, pertanto vengono proposte le varie alternative --> bisogna sceglierne una prima di salvare
                     - avversità non corretta: l'avversità non è compatibile con gli altri dati (specie, disciplinare, formulato, etc) --> va sostituita prima di salvare
                     - avversità ambigua: l'avversità non ha tutti i dati per essere identificata univocamente sul web, pertanto vengono proposte le varie alternative --> bisogna sceglierne una prima di salvare
                     - avversità non valorizzata: l'avversità è obbligatoria, pertanto se arriva non popolata va indicata prima di salvare
                     ", "", 0, "",
                    "testare il comportamento sopra citato in ribaltamento, testare che gli altri scenari funzionino correttamente (creazione/modifica agenda, creazione/modifica ricetta, creazione/modifica brogliaccio, per tutte le operazioni (non solo i trattamenti)
                    con particolare attenzione ai meccanismi di correlazione formulato/avversita.")

            Riga_Text("QdC New - Visite - WIP non ancora testabile",
                     "Aggiunte posizione, ora fine; possibilità di salvare una visita senza impianti")

            Riga_Text("QdC New - Rilievi",
                      "- Aggiunge funzione per caricamento preventivo delle unità di misura
                             - Fix salvataggio rilievi avversità: creando più righe rilievo con la stessa avversità ma unità di misura differenti, venivano salvate su database tante righe quante erano le avversità, appiattendo i dati sulle unità di misura.
                               In riapertura, le righe con avversità uguali venivano caricate con la stessa unità di misura, causando errori nel caricamento dei dati.")

            Riga_Text("Menu Visite - WIP non ancora testabile", "aggiunto caricamento menu visite")

            Riga_Text("Controlli Gerarchia Imprese NG",
                      "Aggiunti controlli per la Gerarchia Imprese (quando si elimina un'impresa, quando si cambia la cooperativa referente...)",
                      noteTest:="Fare test vari: cambiare la cooperative referente di imprese che siano o meno referenti a loro volto di altre imprese; 
                      Rimuovere tutti le aziende che fanno riferimento ad un impresa X.... 
                      Per verificare la correttezza delle modifiche utilizzare l'albero presente nella pagina 'Filtro di Ricerca'")

            Riga_Bug("QdC - Caricamento Operazioni",
                      "Sistemato caricamento operazioni nel caso in cui si faccia login con un utente con una lingua straniera")

            Riga_Text("QdC Angular",
                      "Aggiornate funzioni per corretta lettura insetti, avversità ed attività, per operazione Distribuzione Insetti su QdC Angular")

            Riga_Text("Macchine",
                      "- Introdotto campo ExternalAPIKey in 'Altri Dati Macchina' " &
                      "- Visualizzata Unità di Misura nella caratteristica di una macchina ")

            Riga_Text("Esercizi NG",
                      " - Gestione Articolo in esercizio, WIP ")

            Riga_Text("Documentale",
                      "- Fix problema della PIVA mancante per le pratiche create con una tipologia multipla ", idPerforma:=24872)

            Riga_Text("Gis NG",
                      "- Aggiunto nuovo end-point per filtro avanzato impianti")

            Riga_Text("QdC - Configurazione Operazioni Colturali",
                      "- Aggiunti nuovi end-point per per gestione tabella imprese parametri GHG")

            Riga_Text("DatiReteAcqua",
                      "- Aggiunto nuovo core ws per poter richiamare i dati iot relativi alla rete acqua, trasferita da meteosuite a db server cliente", "MIDAR", 0, "i dati devono essere precedentemente caricati tramite GSB", "nessun test")

            Riga_Text("Visite wip",
                     "Aggiunti nuovi coreWS per richiamo operazioni e attività")

            '================================

            Riga_Data("28 Giugno 2023")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Nuovi records e tabelle per GIS", "732")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "114")

            Riga_Requisiti("Configurazione_Siti",
                           "Configurazione salvataggio layer raster", "121")

            Riga_Bug("CDG - Motorino",
                     "- Cambiata la query per timeout su cancellazione CDG_Testata")

            '================================

            Riga_Data("23 Giugno 2023")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Nuovi records e tabelle per GIS", "732")

            Riga_Requisiti("Aggancio",
                           "Nuove tabelle per aggancio", "114")

            Riga_Requisiti("Configurazione_Siti",
                           "Configurazione salvataggio layer raster", "121")

            Riga_Text("GIS",
                     "Importazione dal layer ""acque"" dei DXF catastali sul nuovo layer ""Corpi idrici superficiali"" ")

            Riga_Text("GIS",
                     "Gestione della configurazione di algoritmi proiezione generica fra layers di tipo vettoriale. ")

            Riga_Text("MenuAgenda NG",
                     " - Arrotonda quantità in dettaglio tecnico a 2 decimali " &
                     " - Aggiunge dettaglio tecnico in griglie ricette e brogliaccio ")

            Riga_Text("Griglia Impianti Operazioni NG",
                      "Aggiunta gestione colonna CU Massimo - Distribuito - Distribuibile.
                      Anche per NPK, se il valore MASSIMO non è stato impostato da anagrafica, comparirà la scritta 'Non Definito'")

            Riga_Text("Preservazione data creazione Operazioni e Ricette",
                      "La data creazione viene preservata per agenda e ricette di tutti i tipi.",
                      "Regione Umbria / Umbria Digitale", 24374)

            Riga_Bug("Prodotti con Lotto - Operazioni NG",
                     "Fix confronti lotto in caricamento prodotti")

            Riga_Bug("NG Semina e Raccolta con Aggiornamento Anagrafica",
                     "Fix su semina e raccolta con Aggiornamento Anagrafica (righe doppie, superfici non riportate, gruppo varietale sbagliato)")

            Riga_Text("Modifica Multipla Operazioni NG",
                      "Modificata la lettura delle macchine, ora è allineata alla vecchia e mostra i costi + Aggiunta API
                      Aggiunta gestione check 'Solo Aziendali' per non visualizzare gli elementi di altre aziende, sia per griglia manodopera che macchine")

            Riga_Bug("Sincro dati APP",
                     "Fix per impedire che i dati dell'app con data futura vengano passati a web come pianificati (6964)")

            Riga_Bug("Salva e vai ai costi NG",
                     "Fix salvataggio costi su agenda in caso di modifica e in caso di eliminazione agenda per deselezione impianti da operazione multicentro")

            Riga_Bug("Campi NG",
                     "Superficie appezzamento completa in sezione Appezzamenti Campo dentro a pagina di Edit del Campo")

            Riga_Bug("Macchine NG",
                     "Fix salvataggio parametro visibileControlloGestione")

            Riga_Text("QdC NG",
                     "Redirect alla pagina Raccolta.aspx per le raccolte create con dei semilavorati vegetali")


            '================================

            Riga_Data("20 Giugno 2023")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Nuove tabelle per GIS", "731")

            Riga_Requisiti("Aggancio",
                           "nuova tabella e drop vista per gestione MisuraXAvversita_Anagrafiche et al.", "111")

            Riga_Requisiti("Configurazione_Siti",
                           "Configurazione salvataggio layer raster", "121")

            Riga_Bug("CDG",
                     "Fix errore bombardino su operazioni generate da angular con multicentro e multioperazione")

            '==================================

            Riga_Data("19 Giugno 2023")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Nuove tabelle per GIS", "731")

            Riga_Requisiti("Aggancio",
                           "nuova tabella e drop vista per gestione MisuraXAvversita_Anagrafiche et al.", "111")

            Riga_Requisiti("Configurazione_Siti",
                           "Configurazione salvataggio layer raster", "121")

            Riga_Bug("CDG",
                     "Fix errore bombardino su operazioni generate da angular con multicentro e multioperazione")

            '==================================

            Riga_Data("15 Giugno 2023")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Nuove tabelle per GIS", "731")

            Riga_Requisiti("Aggancio",
                           "nuova tabella e drop vista per gestione MisuraXAvversita_Anagrafiche et al.", "111")

            Riga_Requisiti("Configurazione_Siti",
                           "Configurazione salvataggio layer raster", "121")

            Riga_Text("GIS Ng",
                      "Gestione delle configurazioni di algoritmi GIS, relativa interfaccia ed applicazione di configurazioni GIS su tutto il layer o su singoli poligoni")

            Riga_Bug("Anagrafica NG",
                      "Fix popolamento elenco Gruppo Varietale")

            Riga_Bug("Anagrafica NG",
                      "Fix eliminazione appezzamento-impianto-esercizio", "", 0, "Nell'eliminazione andava a controllare il campo_cod che non è un dato obbligatorio", "Nessun test")

            '==================================

            Riga_Data("09 Giugno 2023")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Nuova colonna ProprietarioCapi su tabella Fabbricati", "729")

            Riga_Requisiti("Aggancio",
                           "nuova tabella e drop vista per gestione MisuraXAvversita_Anagrafiche et al.", "111")

            Riga_Requisiti("Configurazione_Siti",
                           "Per chiave LinkGiasBase", "119")

            Riga_Text("GIS Ng",
                      "Aggiornata funzione di lettura SpecieVegetali per salvataggio nuovo impianto da GIS in caso Sementieri")

            Riga_Text("GIS Ng",
                      "Aggiornata funzione lettura GruppoVarietale per salvataggio nuovo impianto da GIS in caso Sementieri")

            Riga_Text("Amministrazione Sistema - Consulta Sincro Dati App",
                      "Gestione chiamate lato server per integrazioni pagina di consultazione sincronizzazione dati app",
                      "CAI")

            Riga_Bug("QdC NG:",
                      "- Aggiunta indicazione aggiornata della Giacenza totale e alla data del Prodotto in modifica di una Operazione")

            Riga_Text("Controlli Rame Fertilizzazioni",
                      "I controlli sul rame vengono fatti a prescidere dal disciplinare selezionato o meno.")

            Riga_Bug("Controlli massimali rame in Inserimento Prodotto Fitosanitario",
                      "Fix sui controlli rame quando si inserisce un prodotto fitosanitario. (non venivano considerati gli altri prodotti della stessa operazione) ")

            Riga_Text("QdC NG Raccolte:",
                      " - Gestione multicentro " &
                      " - Gestione salvataggio Cod_Progetto" &
                      " - Rimuove gestione Cal_Cod")

            Riga_Text("Riallineamento funzioni Budget",
                      "Portato al passo il codice budget con il suo corrispettivo anagrafe")

            Riga_Bug("Breadcrumbs",
                      "Fix bug breadcrumbs sbagliati quando su Angular QdC si fa salva e vai ai costi e poi si torna indietro.")

            Riga_Text("Disciplinare Impianto - Operazione Colturali",
                      "Unione delle colonne 'Disciplinare' e 'Regolament'o sotto un'unica colonna 'Disciplinare'",
                      noteTecniche:="SE Regolamento_Cod = 4 --> Disciplinare = 'BIO'
                      Altrimenti SE Disciplinare_Cod > 0 --> Disciplinare = Disciplinare_Des
                      Altrimenti --> Disciplinare = 'Nessuno' ")

            Riga_Text("Parametri GHG",
                      "Gestione Trasferimento di Magazzino, Viene creato una nuova campionatura per il prodotto in carico e che considera le variazioni di parametri quali la distanza",
                      "IBF Kenia")

            Riga_Text("CDG",
                      "Gestione Bombardino per operazioni multicentro e/o multioperazione. Da testare.")

            '==================================

            Riga_Data("01 Giugno 2023")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Nuova colonna ProprietarioCapi su tabella Fabbricati", "729")

            Riga_Requisiti("Aggancio",
                           "nuova tabella e drop vista per gestione MisuraXAvversita_Anagrafiche et al.", "111")

            Riga_Requisiti("Configurazione_Siti",
                           "Per chiave LinkGiasBase", "119")

            Riga_Bug("Fabbricati",
                     "Corretta scrittura Indirizzo")

            Riga_Bug("Imprese - Contatti",
                     "Corretta scrittura Codice fiscale utente creazione/modifica in tabella contatti")

            Riga_Bug("Esercizi",
                     "Corretta data default per Data Fine Prevista")

            Riga_Bug("Magazzini",
                     "Fix scrittura carico da App")

            '==================================

            Riga_Data("31 Maggio 2023")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Nuova colonna ProprietarioCapi su tabella Fabbricati", "729")

            Riga_Requisiti("Aggancio",
                           "nuova tabella e drop vista per gestione MisuraXAvversita_Anagrafiche et al.", "111")

            Riga_Requisiti("Configurazione_Siti",
                           "Per chiave LinkGiasBase", "119")

            Riga_Bug("Proposta GHG",
                     "Fix proposta GHG per considerare la PIVA corrente",
                     "BF ENI")

            '==================================


            Riga_Data("29 Maggio 2023")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Nuova colonna ProprietarioCapi su tabella Fabbricati", "729")

            Riga_Requisiti("Aggancio",
                           "nuova tabella e drop vista per gestione MisuraXAvversita_Anagrafiche et al.", "111")

            Riga_Requisiti("Configurazione_Siti",
                           "Per chiave LinkGiasBase", "119")

            Riga_Bug("Anagrafica Impresa",
                     "Fix salvataggio impresa app e corretto rapporto contabile",
                     "BF ENI")

            '==================================

            Riga_Data("26 Maggio 2023")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Nuova colonna ProprietarioCapi su tabella Fabbricati", "729")

            Riga_Requisiti("Aggancio",
                           "nuova tabella e drop vista per gestione MisuraXAvversita_Anagrafiche et al.", "111")

            Riga_Requisiti("Configurazione_Siti",
                           "Per chiave LinkGiasBase", "119")

            Riga_Bug("Checklist",
                     "Fix gestione aggiornamento checklist con rintracciabilità",
                     "COPROB",
                     noteTecniche:="Aggiunta proprietà Rintracciabilita in classe AuditModel")

            Riga_Bug("GHG", "- Fix priorità lettura tabelle")

            Riga_Text("QdC NG, scelta prodotti",
                     "Aggiunta opzione di filtro giacenza > 0 anche se l'impostazione utente delle giacenze è di tipo 'Solo movimentati'")

            Riga_Bug("Verifica conformità Multi Attivita",
                     "Piccolo fix nel caso di Verifica conformità Multi Attivita Fertilizzazione con Disciplinare 'Solo Etichetta'")

            Riga_Bug("QdC NG, disciplinari",
                     "fix nel reperimento dei disciplinari privati", "", 0, "", "Provare su Collis che ha i disciplinari privati, devono venire proposti sia nelle operazioni singole che nel multi operazione")

            '==================================

            Riga_Data("24 Maggio 2023")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Nuova colonna ProprietarioCapi su tabella Fabbricati", "729")

            Riga_Requisiti("Aggancio",
                           "nuova tabella e drop vista per gestione MisuraXAvversita_Anagrafiche et al.", "111")

            Riga_Requisiti("Configurazione_Siti",
                           "Per chiave LinkGiasBase", "119")

            Riga_Text("QdC NG MultiCentro",
                     "Multicentro su rilievo", "", 0, "", "Testare il multicentro su tutti i tipi di rilievo NG, anche le fasi fenologiche, anche aggiungendo e togliendo righe di rilievo in modifica")

            Riga_Bug("QdC NG Raccolta",
                     "Fix Raccolta Fast", "", 0, "Creare una raccolta senza prodotti, deve salvare e riaprirsi correttamente")

            '==================================

            Riga_Data("23 Maggio 2023")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Nuova colonna ProprietarioCapi su tabella Fabbricati", "729")

            Riga_Requisiti("Aggancio",
                           "nuova tabella e drop vista per gestione MisuraXAvversita_Anagrafiche et al.", "111")

            Riga_Requisiti("Configurazione_Siti",
                           "Per chiave LinkGiasBase", "119")

            Riga_Text("QdC NG MultiCentro",
                     "- Adattate alcune chiamate per compatibilità con il nuova gestione MultiCentro di Angular")

            Riga_Text("QdC Gestione Anagrafica Misura x Avversità",
                     "- Endpoint registrazione retail")

            Riga_Text("Retail",
                     "- Endpoint a supporto della gestione di Anagrafica Misura x Avversità")

            Riga_Text("QdC NG MultiCentro",
                     "Completata gestione multicentro (sia agende che ricette)", "", 0, "",
                     "Creare agende e ricette con impianti appartenenti a centri diversi, con vari scenari (operazioni ultimo campo editato dose/ha oppure dose/hl oppure qta totale, alla riapertura in modifica i dosaggi devono essere invariati, la qta totale deve essere il totale dei vari centri, e nel caso fosse l'ultimo campo editato, deve essere identica a quella imputata, anche come decimali. Stesso test per l'acqua. Provare anche con il multioperazione. Provare copie di agenda, copie di ricette e ribaltamento agenda/ricetta e ricetta/agenda")

            Riga_Bug("QdC NG Brogliaccio",
                     "- Fix cancellazione Brogliaccio")

            Riga_Bug("QdC NG Raccolta",
                     "- Fix ripartizione quantità in raccolta multicentro e in ribaltamento a ricetta")

            Riga_Text("ProprietarioCapi", "Aggiunta nuova colonna ProprietarioCapi su tabella Fabbricati, modificati il DAL e BIZ per permettere il salvataggio e aggiornamento di questo campo")

            Riga_Text("Amministrazione Sistema - Consulta Sincro Dati App",
                      "Gestione chiamate lato server per nuova pagina di consultazione sincronizzazione dati app",
                      "CAI",
                      noteTecniche:="Il pulsante di menù di abilita tramite il seguente permesso: GiasAPP - Consulta Sincro Dati App da Web")

            Riga_Text("Verifica Conformità in Edit Operazione multi-centro",
                      "Aggiunta gestione del Verifica Conformità per le operazioni multi-centro")

            '==================================

            Riga_Data("19 Maggio 2023")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Aggiunta colonna Pratica_Cod in Audit", "727")

            Riga_Requisiti("Aggancio",
                           "Tabelle notifiche push", "110")

            Riga_Requisiti("Configurazione_Siti",
                           "Per chiave LinkGiasBase", "119")

            Riga_Bug("Brogliaccio - Carica Dati APP",
                     "- Ulteriore fix per compatibilità con la vecchia app dove non viene valorizzato il campo Invia_App",
                     "ABOCA", 24055)

            '==================================

            Riga_Data("18 Maggio 2023")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Aggiunta colonna Pratica_Cod in Audit", "727")

            Riga_Requisiti("Aggancio",
                           "Tabelle notifiche push", "110")

            Riga_Requisiti("Configurazione_Siti",
                           "Per chiave LinkGiasBase", "119")

            Riga_Bug("Brogliaccio - Carica Dati APP",
                     "- Fix per compatibilità con la vecchia app dove non viene valorizzato il campo Invia_App",
                     "ABOCA")

            '==================================

            Riga_Data("17 Maggio 2023 BIS")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Aggiunta colonna Pratica_Cod in Audit", "727")

            Riga_Requisiti("Aggancio",
                           "Tabelle notifiche push", "110")

            Riga_Requisiti("Configurazione_Siti",
                           "Per chiave LinkGiasBase", "119")

            Riga_Text("MenuAgenda NG",
                     " - Carica da database le descrizioni per il menu stampe preferite" &
                     " - Rende case insensitive il lotto per le attività di Raccolta e Semina")

            '==================================

            Riga_Data("17 Maggio 2023")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Aggiunta colonna Pratica_Cod in Audit", "727")

            Riga_Requisiti("Aggancio",
                           "Tabelle notifiche push", "110")

            Riga_Requisiti("Configurazione_Siti",
                           "Per chiave LinkGiasBase", "119")

            Riga_Bug("Documentale - Gestione Workflow",
                     "- Fix per aprire il codice pratica quando si inserisci un nuovo documento con multi-tipologia",
                     "COPROB")

            Riga_Bug("Scrittura Impresa",
                     "- Fix perché scriveva Cod_Rapporto = 0 nella Risorsa Umana del contatto impresa")

            '==================================

            Riga_Data("12 Maggio 2023")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Aggiunta colonna Pratica_Cod in Audit", "727")

            Riga_Requisiti("Aggancio",
                           "Tabelle notifiche push", "110")

            Riga_Requisiti("Configurazione_Siti",
                           "Per chiave LinkGiasBase", "119")

            Riga_Text("GIS - Visualizzazione totale",
                      "Gestione lettura/scrittura impostazione utente auto zoom visualizzazione totale")

            Riga_Text("GIS - Avversità",
                      "Generazione tiles descrizione per avversità in funzione di quanto indicato in anagrafica misure per avversità")

            Riga_Text("QdC NG - Rilievi",
                      "Gestione coordinate rilievi in campo")

            Riga_Text("QdC NG - Raccolta",
                      " - Fix salvataggio unità di misura per le quantità raccolte" &
                      " - Fix caricamento delle quantità di prodotti raccolti da più impianti")

            Riga_Bug("Ricerca documenti",
                     "- Non mostrava il vettore nella ricerca testata se questo è di proprietà di altra impresa",
                     "Collaudo Inalca")

            Riga_Bug("Esportazione su Excel",
                     "- Ora non cerca di esportare erroneamente le colonne 'Azione' (quelle che contengono pulsanti)",
                     "UMA", 23848)

            Riga_Text("QdC NG",
                      " - Aggiunta condizione per mostrare/nascondere il nuovo flag 'Magazzini Agenzie' per le Ricette." &
                      " - Aggiunta colonna per visualizzare i Magazzini delle Agenzie nel menu delle Ricette." &
                      " - Impostato default nel menù a tendina del magazzino all'ultimo magazzino in qui è stato movimentato il prodotto scelto dall'agenzia nelle Ricette" &
                      " - Nascosta la colonna 'Magazzini Agenzie' per le Ricette se nell'archivio non ci sono delle imprese di tipo Agenzie.")

            Riga_Text("MisuraxAvversita",
                      "Gestione colonna Tipo Controllo per le Unità Misura")

            Riga_Text("QdC NG - Ricette",
                      " - Gestito flag invia_app nella pagina ricette." &
                      " - Controllo sotto giacenza mai bloccante." &
                      " - Lotto mai obbligatorio")

            Riga_Text("Copia Sposta Appezzamenti",
                      "Implementato BE per funzionalità di Copia Sposta Appezzamenti")

            Riga_Text("LOG Particelle Catastali",
                      "Implementato LOG per scrittura / modifica / cancellazione delle Particelle Catastali su Agronica_Log_Anagrafe")

            Riga_Text("GHG",
                      "Gestiti Core lettura, salvataggio e calcolo fattore standard")

            Riga_Text("Sementieri",
                      "Impostata lettura Finalità e SpecieVegetale per pagina salvataggio nuovo impianto da GISNg quando si ha Sportello o MappaturaLibera attiva")

            Riga_Bug("Sementieri",
                     "Corretta lettura SpecieVegetale per pagina edit AppezzamentoImpianto Ng")

            Riga_Bug("Sementieri",
                     "Corretto passaggio dati a pagina GISNg da pagina Gestione Isolamenti")

            Riga_Bug("GISNg",
                     "Corretta lettura dati kendoGrid per layer tecnici in campo")

            Riga_Bug("Investimento Catasto",
                      "Corretta lettura Organismo Referente")

            Riga_Text("InvestimentoCatasto",
                      "Aggiunta lettura PadreInGerarchia")

            Riga_Bug("AnagraficaNg",
                     "Corretto controllo validita temporale impianti")

            Riga_Bug("Ricerca Documenti",
                       "Fix visualizzazione allegati in assenza dell'impostazione di gestione del workflow documentale")

            Riga_Text("GiasBase", "Eliminati tutti i riferimenti fissi al GiasBase (/GiasBase/... etc nel codice - lettura parametrizzata tramite chiave in configurazione siti")

            Riga_Text("GisNG",
                      "- Aggiunto nuovo web service per upload allegato su mappa di prescrizione " &
                      "- Adeguato ws lettura mappa di prescrizione xml per conversione in geojson tramite nuovo package NetTopologySuite")

            '==================================

            Riga_Data("28 Aprile 2023")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Aggiunta tabelle per notifiche push", "726")

            Riga_Requisiti("Aggancio",
                           "Tabelle notifiche push", "110")

            Riga_Requisiti("Configurazione_Siti",
                           "Per nuova chiave CompressioneRispostaAjax utilizzata per abilitare compressione risposta standard", "102")

            Riga_Bug("Menu Agenda NG",
                     "Fix cancellazione Brogliaccio multi (con stesso Raccoglitore_Cod).")

            Riga_Bug("Semina con Aggiornamento Anagrafica NG",
                     "Fix controlli che impedivano la semina con aggiornamento anagrafica, pur seminando la stessa specie")

            Riga_Text("Messaggi cancellazione Operazioni Multiple",
                      "Migliorato messaggio cancellazione operazioni multiple")

            Riga_Text("Notifiche PUSH APP",
                      "Gestione lista di Servizi di Notifiche Sottoscrivibili")

            '==================================

            Riga_Data("21 Aprile 2023")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Aggiunta colonna Pratica_Cod in Allegati_Documenti", "725")

            Riga_Requisiti("Aggancio",
                           "Tabelle Stadi Crescita", "104")

            Riga_Requisiti("Configurazione_Siti",
                           "Per nuova chiave CompressioneRispostaAjax utilizzata per abilitare compressione risposta standard", "102")

            Riga_Text("GHG", "Aggiunta api per il calcolo del GHG")

            Riga_Text("DAL Lettura Tabella Metaschema Codifica_SpecieVegetali_SistemiEsterni",
                      "Aggiunti DAL Lettura Tabella Metaschema Codifica_SpecieVegetali_SistemiEsterni")

            Riga_Text("DAL Lettura Tabella Agronica_Log_Invio_Ricetta",
                      "Aggiunti DAL Lettura Tabella Agronica_Log_Invio_Ricetta")

            Riga_Text("Workflow Documentale",
                      "- Creazione di una pratica associata ad un nuovo documento nel caso sia valorizzata l'impostazione utente SUPERUSER_Documentale_GestioneWorkFlow" &
                      "- Introduzione di colonne apposite e rimozione delle vecchie colonne legate alla validazione dei documenti durante la lettura dell'elenco documenti nel caso sia valorizzata l'impostazione utente SUPERUSER_Documentale_GestioneWorkFlow")

            Riga_Text("Anagrafica NG - Visualizzazione Campi",
                      "Migliorate prestazioni query caricamento griglia campi")

            Riga_Text("Anagrafica NG - Visualizzazione Impianti",
                      "Migliorate prestazioni query caricamento griglia impianti")

            Riga_Text("Anagrafica NG - Imprese",
                      "Implementata creazione contatto aziendale pubblico",
                      noteTest:="Quando si crea una nuova impresa compare il check contatto pubblico nella sezione contatti, 
                      verificare che venga creato correttamente pubblico il contatto dell'impresa sotto il superuser")

            Riga_Text("Sincro - Nogmo WIP",
                        "- gestione animale da inviare a Nogmo e invio WIP")

            Riga_Bug("Anagrafica NG - macchine",
                     "corretto bug, che sulle macchine pubbliche non funzionava la verifica che la macchina era movimentata e quindi si riusciva sempre a cancellare",
                     noteTecniche:="Creare una macchina pubblica su un'azienda e utilizzarla in una operazione di agenda di un'altra azienda e poi provare a cancellare la macchina, deve dare errore")

            Riga_Text("QdC NG - rilievi",
                      "Implementata i seguenti rilievi: avversità, fasi fenologiche, infestanti, danni, indici maturità, indici rese",
                      noteTest:="provare tutti questi rilievi in creazione/modifica, devono essere uguali come funzionamento al Gias")

            Riga_Text("QdC NG - polverulento ",
                      "Eliminata la gestione dei prodotti polverulenti da geodisinfestazione e concia del seme",
                      noteTest:="verificare che per queste due operazioni non chieda l'acqua obbligatoria in presenza di prodotto non polverulenti")

            Riga_Text("QdC NG - ricette ",
                      "Completamento testata ricetta, ribaltamento agenda-ricetta, ricetta-agenda, ricetta-brogliaccio, brogliaccio-agenda, copia ricette, ricette multioperazione",
                      noteTest:="provare tutti questi scenari, verificare soprattutto che venga correttamente gestito il codice multioperazione (se si ribalta/copia una agenda/ricetta multioperazione, il sistema deve creare una situazione multioperazione analoga, con codice multioperazione generato appositamente")

            Riga_Text("QdC NG - ricette ",
                      "Aggiunto il pulsante di Salvataggio Ricette dedicato (Salva e:) per aggiungere nuove ricette e modificare quelle con la stessa testata di Ricetta")

            Riga_Text("Anagrafiche NG - Macchine ",
                      "Aggiunto campo HubIoT_PlatformDestination")

            Riga_Text("Verifica Conformità al Salvataggio Operazione NG",
                      "Migliorata la grafica del messaggio del Verifica Conformità al salvataggio di un'operazione.
                      Ex. Per le giustificazioni di intervento veniva dato lo stesso messaggio tante volte quanti erano gli impianti selezionati, ora viene mostrato un singolo messaggio per tipo segnalazione.")

            Riga_Bug("Verifica Conformità Operazione - Controlli Macroelementi_Distribuiti (NPK Mg)",
                     "fix lettura e confronto raccoglitore_cod quando si vanno a leggere i Macroelementi_Distribuiti nelle operazioni",
                     "Genagricola", 23678)

            Riga_Bug("Denominazione Appezzamento",
                     "La descrizione di un Appezzamento può ora avere una lunghezza max di 500 caratteri",
                     "Aboca", 23661)

            Riga_Text("Localizzazione:",
                      "Aggiunte traduzioni in inglese per i seguenti moduli: Giacenze Magazzino, Documenti Contabili, Lavorazioni e Menu Operazioni di Agenda",
                      "Eni Kenia")

            Riga_Text("Agenda",
                      "nuovo endpoint VerificaEsistenzaEntitaAnagrafiche", noteTest:="Nessun test necessario")

            Riga_Text("GIS, sementi",
                      "Riporto della retinatura come da modalità sementi integrando in tematizzazione",
                      noteTest:="Nessun test necessario")

            Riga_Text("GIS",
                      "in base ad un impostazione nel ""Setup di visualizzazione"", decidere se recuperiamo o meno, il lat lng in surroga alla mancanza del rilievo puntuale",
                      noteTest:="Nessun test necessario")

            Riga_Text("GIS",
                      "Sulla tipologia layer ""avversità"" ora vengono visualizzati punti interni al poligono laddove non è stato rilevato il punto. Gestire se sono punti o tutto il poligono in maniera configurabile (da ""Setup di visualizzazione"")",
                      noteTest:="Nessun test necessario")

            Riga_Bug("GIS",
                     "Verifica mancata lettura delle avversità come memorizzate nella tabella DSS_Agronica_Stazioni_Meteo_ModelliPrevisionali_SRV",
                     noteTest:="Nessun test necessario")

            Riga_Bug("GIS,",
                     "Query di Lettura dei campioni: impostare il parametro per il filtro per data esattamente come su agenda",
                     noteTest:="Creare un analisi con due campioni, uno in data odierna ed uno al giorno precedente, registrare i punti nel gis, uscire, rientrare, osservare che ora vengono mostrati entrambi i punti")

            Riga_Text("GIS",
                      "[DotNet] - sui layer personalizzati, in Anagrafica_DataStruct definire una nuova colonna ""CampoChiave"" ", noteTest:="")

            Riga_Text("GIS",
                      "Interna, backend, modalità tecnici in campo, porting su CoreWS/API degli endpoint ""UltimaPosizione"", ""Percorsi"" ",
                      noteTest:="Nessun test necessario")

            Riga_Bug("Widget - Ultimi acquisti",
                     "Fix mancato caricamento in caso di dati nulli",
                     noteTest:="Nessun test necessario")

            Riga_Bug("Permesso cancellazione brogliaccio NG",
                     "Corretto permesso letto per cancellazione brogliacci da angular. (al momento non vengono ancora cancellate le operazioni collegate per il multi, quindi l'eliinazione ha effetto solo per il brogliaccio selezionato.)")

            Riga_Bug("Edit prodotto Diserbo NG",
                     "Fix edit prodotto diserbo con campo dose_consigliata_anno = NULL (== non indicata)",
                     noteTest:="Fare un diserbo con prodotto PIRAMAX EC (che non ha indicata la dose max anno), salvare e rientrare in modifica dell'operazione. Si deve essere in grado di editare il prodotto senza la comparsa di un errorone rosso.")

            '==================================

            Riga_Data("11 Aprile 2023")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "aggiunto campo Invia_HubIoT in Ricette_Operazioni", "724")

            Riga_Requisiti("Aggancio",
                           "Tabelle Stadi Crescita", "104")

            Riga_Requisiti("Configurazione_Siti",
                           "Per nuova chiave CompressioneRispostaAjax utilizzata per abilitare compressione risposta standard", "102")

            Riga_Bug("GIS NG",
                      "Corretto riferimento a dbutenti ABICOA fisso su lettura permessi layer GIS")

            Riga_Bug("Verifica Massimali Azoto in aggiunta prodotto (NG)",
                     "Ora il controllo sui massimali di Azoto utilizza correttamente il dato N Utile (Azoto x Efficienza) per verificare gli apporti")

            Riga_Text("Verifica Conformità Edit Operazione NG",
                      "Aggiunta gestione del multioperazione per i controlli sul rame: quando si clicca sul pulsante 'Verifica Conformità' vengono passate tutte le operazioni presenti a video con i relativi quantitativi di CU.
                      Stesso quando si inserisce un prodotto: vengono fatti dei controlli sugli apporti di rame presente in tutte le operazioni correnti -- se si sfora il massimale di 4KG/HA verrà impedito l'inserimento del prodotto",
                      noteTest:="Provare ad eseguire un trattamento assieme ad una fertilizzazione:
                      - Inserire un quantitativo di Rame nella fertilizzazione pari a 3-3,9 KG/HA (o comunque in modo che l'operazione presa da sola non sfori).
                      - Nel trattamento antiparassitario inserire un prodotto con Rame metallico (o simile) in modo da superare il limite di 4 kg/ha di CU se sommato all'operazione precedente. 
                      IL CONTROLLO IN INSERIMENTO PRODOTTO DEVE IMPEDIRE L'AGGIUNTA DELL'ANTIPARASSITARIO! (Messaggio del tipo: La dose di rame distribuita sull'appezzamento X (XXX Kg/ha) supera la massima consentita (4 Kg/Ha all'anno))")

            Riga_Bug("Cancellazione multipla NG",
                     "Fix cancellazione multipla di due operazioni con stesso Raccoglitore_Cod",
                     noteTest:="Creare un multi operazione. Una volta nel menu, selezionare entrambi gli elementi (con stesso COd. Multi Attività) e cancellarli con la cancellazione multipla (icona rossa con due bidoncini in alto a dx sulla griglia). 
                     La cancellazione deve andare a buon fine.")

            '==================================

            Riga_Data("05 Aprile 2023")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "aggiunto campo Invia_HubIoT in Ricette_Operazioni", "724")

            Riga_Requisiti("Aggancio",
                           "Tabelle Stadi Crescita", "104")

            Riga_Requisiti("Configurazione_Siti",
                           "Per nuova chiave CompressioneRispostaAjax utilizzata per abilitare compressione risposta standard", "102")

            Riga_Bug("Diserbo NG",
                      "Corretta lettura della percentuale di riduzione in modifica del prodotto")

            '==================================

            Riga_Data("30 Marzo 2023")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "aggiunto campo Invia_HubIoT in Ricette_Operazioni", "724")

            Riga_Requisiti("Aggancio",
                           "Tabelle Stadi Crescita", "104")

            Riga_Requisiti("Configurazione_Siti",
                           "Per nuova chiave CompressioneRispostaAjax utilizzata per abilitare compressione risposta standard", "102")

            Riga_Text("GIS",
                      "Gestione del caricamento delle mappe di prescrizione via CoreWS (Porting da versione precedente, dove il caricamento avveniva sul GIS in Agenda)")

            Riga_Text("Zoo",
                      " Funzione di Lettura Causali Morte", "Inalca", 0, "", "Vedi versione Agenda")

            Riga_Text("Contatti ",
                      " Funzione di lettura codici ausl da indirizzo", "Inalca", 0, "", "nessun test, WIP")

            Riga_Bug("GIS",
                      "Risolto un bug che impediva la corretta generazione delle mappe di prescrizione quando il servizio mappe non risolve la proiezione UTM da WGS84")

            Riga_Bug("Raccolte NG",
                      "Modificata l'assegnazione del lotto da esercizio corrente: l'assegnazione era stata erroneamente basata sul campo Progetto_Cod. " &
                      " Ora quando il lotto è generato automaticamente da esercizio viene valorizzato con Progetto_Nome. ")

            Riga_Text("CAC - Codifica Prodotti Aziendali (Prodotti.asmx + CaricaListControl_2010) ",
                      " Implementazione nuovo webmethod per permettere il caricamento dei Sementi (elem_cod = 10) dalla tabella materie_prime)")

            Riga_Text(" Dashboard ",
                      " Miglioramento performance gestione permessi sui widget della dashboard")

            Riga_Bug(" Dashboard ",
                      " Fix passaggio parametri errati in QS su redirect da widget ultimi prodotti utilizzati per apertura pagina agenda GestioneMagazzini/GestioneMagazziniBS.aspx")

            Riga_Text("Nuovo Link Profitosan",
                      "Sostituzione vecchio link con la nuova pagina del Profitosan (pagina specifiche del fitosanitario)")

            Riga_Bug("Creazione Campo inline Anagrafica NG",
                     "Fix creazione campo inline (dalla griglia)",
                     noteTecniche:="Andava in errore il controllo sui piani concimazione collegati.")

            Riga_Text("Gestione campi Invia_APP, Invia_HubIoT, Origine per rilettura in modifica Agenda/Ricette/Ricette_Operazioni",
                        "", "", 0, "Per il momento questa gestione non è visibile nè sul Gias nè su NG", "Creare/modificare una agenda e una ricetta (di qualsiasi tipo) da Gias e da NG e verificare che tutto funzioni correttamente ")

            '==================================

            Riga_Data("29 Marzo 2023")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Nuovo campo Origine su Agenda e Ricette", "723")

            Riga_Requisiti("Aggancio",
                           "Tabelle Stadi Crescita", "104")

            Riga_Requisiti("Configurazione_Siti",
                           "Per nuova chiave CompressioneRispostaAjax utilizzata per abilitare compressione risposta standard", "102")

            Riga_Text("Creazione Prodotti",
                      "Aggiunge l'impostazione SUPERUSER_Creazione_Prodotti (1081) per la determinazione dei prodotti da creare. " &
                      " L'impostazione può assumere i valori 0 = Nessuno, 1 = Sementi, 2 =  Trasformati Vegetali, 3 = Tutti e due. " &
                      " Il check sull'impostazione avviene nella funzione Crea_MateriaPrima_Specie_Varieta_Regolamento, richiamata nella " &
                      " creazione prodotti dall'operazione di semina del QdC, nella creazione di un impianto, in APP dal salvataggio di un impianto. ")

            Riga_Text("GIS Ng",
                      "implementate scrittura e modifica entità campi da GIS Ng")

            '==================================
            Riga_Data("24 Marzo 2023")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Nuovo campo Origine su Agenda e Ricette", "723")

            Riga_Requisiti("Aggancio",
                           "Tabelle Stadi Crescita", "104")

            Riga_Requisiti("Configurazione_Siti",
                           "Per nuova chiave CompressioneRispostaAjax utilizzata per abilitare compressione risposta standard", "102")

            Riga_Text("QdC NG",
                      "Modificata lettura avversità per caricare da metaschema le descrizioni delle avversità non trovate su web service")

            '==================================
            Riga_Data("22 Marzo 2023")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Nuovo campo Origine su Agenda e Ricette", "723")

            Riga_Requisiti("Aggancio",
                           "Tabelle Stadi Crescita", "104")

            Riga_Requisiti("Configurazione_Siti",
                           "Per nuova chiave CompressioneRispostaAjax utilizzata per abilitare compressione risposta standard", "102")

            Riga_Text("QdC NG",
                      "- Modificati default impostazioni usa magazzino (true) e giacenze (tutti i prodotti)" &
                      "- Iniziato sviluppo per Ricette")

            Riga_Bug("Raccolta NG",
                     "Fix ripartizione di un prodotto su più magazzini: è ora possibile raccogliere un prodotto e ripartirlo su magazzini diversi senza che ciò causi problemi in lettura dell'operazione")

            Riga_Text("MenuAgenda NG",
                      "Trasmette messaggio di errore ad Angular in caso la chiamata a infomodifica operazione abbia riscontrato problemi e non fosse possibile visualizzare attività a causa di blocchi esterni - Attiva l'eliminazione di ricette multi (con Raccoglitore_Cod diverso da 0). Cercando di eliminare una ricetta multipla verranno selezionate automaticamente le ricette collegate, verrà visualizzato un messaggio di conferma e solo alla conferma verranno eliminate le ricette in un'unica transizione")

            '==================================

            Riga_Data("17 Marzo 2023")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Nuovo campo Origine su Agenda e Ricette", "723")

            Riga_Requisiti("Aggancio",
                           "Tabelle Stadi Crescita", "104")

            Riga_Requisiti("Configurazione_Siti",
                           "Per nuova chiave CompressioneRispostaAjax utilizzata per abilitare compressione risposta standard", "102")

            Riga_Bug("Gestione Redirect Filtrone da GiasNG",
                     "Alcune pagine sono logicamente sulle stampe ma fisicamente sulla agenda 2010
                     per ovviare ai problemi con redirect da angular, viene fatto un redirect al gestioneRichieste del agenda
                     che si occuperà di fare il redirect corretto sfruttando l'IDSezione in querystring, meccanismo gia usato prima per i redirect tra progetti diversi.")

            Riga_Text("Localization.asmx",
                      "Aggiunto supporto per localizzazione centralizzata header + sidebar")

            Riga_Text("Albero Anagrafiche/Gis",
                      "Refactoring e centralizzazione dati nodo albero appezzamenti/impianti")

            Riga_Text("QdC NG",
                      "Aggiunto controllo che la lista di lav_cod proveniente da angular sia valorizzata")

            Riga_Text("Alert.asmx, Alert_Elenco.asmx",
                      "Modifiche alle funzioni del documentale per Audit Checklist")

            Riga_Text("Menu Agenda NG",
                      " - Alla creazione di una ricetta di raccolta, aggiunto il salvataggio delle destinazioni per i carichi di magazzino " &
                       " - Crea API per la creazione di un prodotto (semente, trasformato vegetale) ")

            Riga_Bug("Caricamento contatti in modifica multipla",
                     "Il caricamento dei contatti avveniva tramite lettura diretta dei contatti e successivo filtro, " &
                     " che però non produceva l'output atteso. Ora Il filtro viene eseguito direttamente nella query.")

            Riga_Text("GruppiUtente_PermessiStato.vb",
                     "(TEC) Aggiunta impostazione superuser per disattivare il controllo delle operazioni consentite in base allo stato pratica, dei doc contabili con gestione workflow",
                    "ENI Kenya", 0, "",
                    "Serve per avere il workflow abilitato sui ddt emessi senza dover configurare la tabella del db_utenti GruppiUtente_PermessiStato" &
                    "perché per quanto ne sappiamo ora nel loro ambiente tutti gli utenti dovranno poter fare tutto su quei ddt indipendentemente dallo stato del documento" &
                    "Di default è abilitata, così da Iniziative Biometano non hanno cambiamenti, ma la possiamo disabilitare da ENI per semplificare le configurazioni")

            Riga_Bug("Dashboard - Widget Operazione Colturali",
                     " Fix errata visualizzazione rilievi in assenza di operazioni colturali")

            Riga_Text("Alert_Elenco.asmx.vb",
                     "(TEC) Spostata logica WebMethod Leggi_File_Allegato in AgronicaCoreScadenziario_BIZ.Allegati")

            Riga_Bug("Macchine NG",
                     " corretto salvataggio macchine legate a contatto ",
                     "",
                     0,
                     "",
                     "  Bisogna creare una macchina legata ad un contatto da un'altra pagina di GIAS (ddt ingresso/uscita) e provare a modificarla dall'anagrafica angular verificando che non cancelli il dato relativo al contatto di appartenenza ")

            Riga_Text("QdC NG",
                     "Controllo acqua anche al conferma prodotto")

            Riga_Text("Agenda/Ricette",
                     "Aggiunti campi Invia_app, Origine, gestiti in lettura e salvataggio", "", 0, "", "Testare letture e salvataggi da GIAS, per verificare che non ci siano comportamenti anomali")

            Riga_Bug("Ricette",
                    "Correzione procedure di salvataggio per uniformare i campi che mancavano in alcuni scenari", "", 0, "", "Testare letture e salvataggi da GIAS, per verificare che non ci siano comportamenti anomali")

            Riga_Text("Ricette NG",
                     "Creazione testata ricetta WIP", "", 0, "", "ancora non testabile")


            Riga_Bug("lettura parametri qualitativi non filtrati per spe/var",
                     "Eliminata la ricerca del flag obbligatorio e della tabella di riferimento (non ancora usata) che fa andare in errore le lavorazioni", "Fruttagel anche se non usa le lavorazioni perchè si nota solo lì", 0, "", "Test lavorazioni")

            Riga_Bug("QdC NG",
                      "Sistemato caso di 'Dose/ha Consigliata' per le Fertilizzazioni.")

            Riga_Text("QdC NG",
                     "Modifica default impostazioni: 208-SUPERUSER_GESTIONE_MAGAZZINO_ABILITATA=1, 180-UTENTE_COD_FILTRO_CATEGORIAMAGAZZINO_GIACENZE: mostra tutti, magazzino e anagrafica", "", 0, "La ricerca prodotti mostrerà di base tutti i prodotti, sia quelli presenti in magazzino che quelli presenti solo in anagrafica. Se non diversamente specificato nelle impostazioni.", "")

            '==================================

            Riga_Data("13 Marzo 2023")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Terzo Rilascio Tabelle Gis 2023", "721")

            Riga_Requisiti("Aggancio",
                           "Tabelle Stadi Crescita", "104")

            Riga_Requisiti("Configurazione_Siti",
                           "Per nuova chiave CompressioneRispostaAjax utilizzata per abilitare compressione risposta standard", "102")

            Riga_Bug("Cancellazione Contatto",
                      "- Si basava sull'assunto errato che l'impresa Gias potesse avere piva solo numerica, " &
                      "per cui se la piva conteneva delle lettere veniva erroneamente permessa la cancellazione del contatto collegato " &
                      "non rilevandolo come il contatto dell'impresa")

            '==================================

            Riga_Data("10 Marzo 2023")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Terzo Rilascio Tabelle Gis 2023", "721")

            Riga_Requisiti("Aggancio",
                           "Tabelle Stadi Crescita", "104")

            Riga_Requisiti("Configurazione_Siti",
                           "Per nuova chiave CompressioneRispostaAjax utilizzata per abilitare compressione risposta standard", "102")

            Riga_Text("Anagrafica Impresa NG",
                      "Aggiunto codice impresa 'Certificate Number'")

            '==================================

            Riga_Data("06 Marzo 2023")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Terzo Rilascio Tabelle Gis 2023", "721")

            Riga_Requisiti("Aggancio",
                           "Tabelle Stadi Crescita", "104")

            Riga_Requisiti("Configurazione_Siti",
                           "Per nuova chiave CompressioneRispostaAjax utilizzata per abilitare compressione risposta standard", "102")

            Riga_Text("Anagrafiche NG - Modifica chiamata albero",
                      " - Modificata chiamata per non passare più il filtro sulla data, che viene gestito lato client")

            Riga_Text("Gis NG - Modifiche varie",
                      " - Filtro elementi albero in base a filtro temporale " &
                      " - Gestione tipo oggetto grafico in inserimento nuovo layer " &
                      " - Gestione rilievi fenologici con BBCH non impostato " &
                      " - Gestione codice analisi campione in salvataggio dati layer con attributi " &
                      " - Caricamento punti layer avversità/fasi fenologiche per rilievi da QdC " &
                      " - Caricamento layer impianti per avversità " &
                      " - Nuovi EndPoint ListaUtenti, ListaUtentiDatiBase e ListaGruppiUtente " &
                      " - Gestione dati utente in inserimento nuovo layer " &
                      " - Copia layer da superuser se al caricamento della tipologia standard non sono presenti " &
                      " - Albero: gestione 4 decimali descrizione nodo dopo salvataggio appezzamento " &
                      " - Albero: popolate date validità nodo dopo salvataggio appezzamento")

            Riga_Text("Anagrafiche/Gis - Albero",
                      " - Modificata generazione chiave albero per le analisi in modo da considerare " &
                      "i campi chiave particella alfanumerici (provincia, comune, sezione, subalterno) " &
                      "uguali a '0' nel non caso fossero impostati " &
                      " - Popolato tipo nodo in caso di anagrafica e planning totale")

            Riga_Text("Appezzamento.asmx - Scrittura appezzamento",
                      "In base a specifica configurazione, introdotta scrittura automatica riparto catasto " &
                      "e imprese particelle quando si inserisce da Gis un nuovo poligono che rappresenta un " &
                      "appezzamento",
                      noteTecniche:="Impostazione da abilitare: ScriviAppezza_RipartoCatastoDaEntita (1079). " &
                      "Le tabelle coinvolte sono AppezzamentiXParticelle e ImpreseXParticelle.")

            Riga_Text("Gias NG - Redirect",
                      "Aggiornato redirect da GiasNG a sito Analisi")

            Riga_Text("QdC NG",
                      " - Modificati oggetti utilizzati lato server per la nuova modalità di inserimento prodotti senza kendo grid " &
                      " - Aggiunge API per creazione semente e trasformati vegetali")

            Riga_Text("Anagrafiche NG",
                      " - Aggiunto endpoint per chiamata eliminazione contatto")

            Riga_Bug("MenuAgenda NG",
                     "Fix passaggio documentale in redirect verifica conformità")

            Riga_Text(" Widget Dashboard",
                      " - Modifiche endpoints x configurazione ed elenco widgets (permessi)")

            Riga_Text("Ricette NG/Gias",
                      "Gestione raccoglitore e polverulento, WIP su QdC NG, non visibile su Gias", "", 0, "", "Essendo cambiate tante classi base, testare su GIAS creazione/modifica/copia/ribaltamento verso agenda e brogliaccio delle ricette")

            Riga_Bug("Lettura Albero Anagrafica",
                      "Corretto numero decimali in per superfici appezzamenti e impianti")

            Riga_Text("Modifica Multipla Operazioni NG",
                      "Migliorati messaggio di salvataggio + gestione traduzioni")

            Riga_Bug("Fix Contatti - Tecnici di Riferimento",
                     "Cambiato il salvataggio lato server dei contatti, in precedenza venivano scritti incorrettamente, inoltre il bottone 'salva in padre' non aveva alcun effetto. 
                     Corretta la lettura del Tecnico, precedentemente caricava con piva sbagliata.")

            Riga_Bug("Anagrafica impianti NG",
                     " - Fix lettura dati catastali per particelle duplicate ", , , , "Non è testabile, ce ne siamo accorti noi in sviluppo")

            Riga_Text("MenuAgenda NG",
                      "Gestiti passaggi QdC/Ricetta/Brogliaccio")

            '==================================

            Riga_Data("23 Febbraio 2023")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Modifiche Tabelle Gis 2023", "720")

            Riga_Requisiti("Aggancio",
                           "Tabelle Stadi Crescita", "104")

            Riga_Requisiti("Configurazione_Siti",
                           "Per nuova chiave CompressioneRispostaAjax utilizzata per abilitare compressione risposta standard", "102")

            Riga_Bug("Anagrafica NG",
                     " - Scrittura impianti con colonna cartografia " & vbCrLf &
                     " - Scrittura anagrafica macchine inline ")

            '==================================

            Riga_Data("21 Febbraio 2023")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Modifiche Tabelle Gis 2023", "720")

            Riga_Requisiti("Aggancio",
                           "Tabelle Stadi Crescita", "104")

            Riga_Requisiti("Configurazione_Siti",
                           "Per nuova chiave CompressioneRispostaAjax utilizzata per abilitare compressione risposta standard", "102")

            Riga_Text("Albero anagrafica NG",
                      "Aggiornata lettura nodi albero per restituire anche data inizio e fine nell'oggetto AjaxTreeNodeJsonObject")

            Riga_Text("Gis 2023 - Server",
                      "- Correzioni varie lettura layer tramite AggiornaElencoTipologie4 " &
                      "- Correzioni varie caricamento geojson avversità e rilievi vegeto produttivi" &
                      "- Scrittura associazione layer a tipo oggetto in salvataggio nuovo layer personalizzato")

            Riga_Text("MenuAgenda NG",
                      "- Passaggio flag Invia_App in griglia ricette e abilitazione tramite API" &
                      "- Aggiunto setup per gestione visibilità dei documenti contabili nel menu agenda " &
                      "- Gestione redirect stampe da menu agenda" &
                      "- Fix salvataggio raccolta fast")

            Riga_Text("Redirect Gias NG",
                      "- Redirect sito Analisi")

            Riga_Text("[Angular] ",
                      "- Gerarchia Macchine WIP")

            Riga_Text("Web.Config",
                      "Aggiunto targetframework 4.8 per supporto TLS di default (https), Necessario per WS Meteo")

            '==================================

            Riga_Data("15 Febbraio 2023")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Modifiche Tabelle Gis 2023 - Seconda tranche", "719")

            Riga_Requisiti("Aggancio",
                           "Tabelle Stadi Crescita", "104")

            Riga_Requisiti("Configurazione_Siti",
                           "Per nuova chiave CompressioneRispostaAjax utilizzata per abilitare compressione risposta standard", "102")

            Riga_Bug("Carica raccolte da APP",
                      "Fix errore in caricamento raccolte verso agenda")

            Riga_Bug("Operazione_agenda_Utility.vb",
                      "Fix errore in composizione filtro aggiuntivo su campo lotto con apici - Aggiunto Agro_sql_SaveText")

            '==================================
            Riga_Data("10 Febbraio 2023")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Modifiche Tabelle Gis 2023 - Seconda tranche", "719")

            Riga_Requisiti("Aggancio",
                           "Tabelle Stadi Crescita", "104")

            Riga_Requisiti("Configurazione_Siti",
                           "Per nuova chiave CompressioneRispostaAjax utilizzata per abilitare compressione risposta standard", "102")

            Riga_Text("Gestione Richieste",
                      "- Implementato redirect sito piano concimazione (per piano concimazione e pua)")

            Riga_Text("Cache SpecieVegetali",
                      "- Integrato nel gestore cache anche la Cache relativa alle specie vegetali visibili dall'utente ")

            Riga_Text("Cancellazione impianti/appezzamenti collegati al GIS",
                      "Aggiunto controllo cancellazione impianto/appezzamento: verranno cancellati tutti i collegamenti con entità gis (poligoni)")

            Riga_Text("Polverulento su nuovo QdC",
                        "Controlli acqua obbligatoria/vietata in base al flag polverulento, inserita la miscibilità prodotti polverulenti e non in verifica conformità", "", 0,
                        "Le impostazioni sulla 'miscibilità di prodotti polverulenti e non' è presente nelle impostazioni utente, errori e warning in inserimento e salvataggio",
                        "Sul Gias appare la nuova voce sotto (ETI) delle verifiche di conformità, rilevata sulla base del flag polverulento letto tramite web service, quindi potrebbe essere gestito anche sul Gias attuale configurando opportunamente le impostazioni")

            Riga_Bug("Nuovo QdC Angular",
                        "Aggiunto controllo per escludere nell'elenco delle operazioni le operazioni preferite che sono state salvate più di una volta")

            Riga_Text("GIS NgG",
                      "Aggiornate funzioni di scrittura e modifica ElementiGrafici per gestire entità di tipo POINT e LINESTRING")

            Riga_Text("MenuAgenda",
                      "Aggiorna visualizzazione di quantità e unità di misura dei prodotti")

            Riga_Text("MenuAgenda NG",
                      "Gestisce redirect a sito stampe")

            Riga_Text("Impostazioni Utente",
                      "Introduce setup per visualizzazione DDT in quaderno di campagna")

            Riga_Text("Anagrafica",
                      "- Aggiunti strutture di riferimento per gli elementi base Anagrafici (Impresa, CentroAziendale, Appezzamento, Impianto, Esercizio)")

            Riga_Text("CentroAziendale.asmx.vb",
                      "- Lettura coordinate per proposta piano colturale grafico in QdC NG")

            Riga_Text("Gis 2023",
                      "- Nuova colonna chiave TipologiaLayer_cod in GIS_LayerTiles" &
                      "- Gestione tematizzazioni")

            '==================================

            Riga_Data("27 Gennaio 2023")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Tabelle Stadi Crescita (Metaschema)", "717")

            Riga_Requisiti("Aggancio",
                           "Tabelle Stadi Crescita", "104")

            Riga_Requisiti("Configurazione_Siti",
                           "Per nuova chiave CompressioneRispostaAjax utilizzata per abilitare compressione risposta standard", "102")

            Riga_Text("QdC NG",
                      "- Aggiunta colonna ZVN nella grid impianti" &
                      "- Sostituiti i Disciplinari 'Nessun Disciplinare','Nessun disciplinare | nessun vincolo normativo' e 'Reg. UE 848/2018 (Ex Reg. CE 834/07) - BIO' con le nuove descrizioni.")

            Riga_Text("Traduzioni Agenda NG",
                      "Aggiunte traduzioni per i messaggi mostrati nelle operazioni d'agenda")

            Riga_Text("Menu Agenda",
                      "Aggiunte quantità e unità di misura per i carichi di magazzino")

            Riga_Bug("Modifica Data Operazione Raccolta NG",
                     "è ora possibile modificare la data dell'operazione di raccolta")

            Riga_Bug("Organismo Referente Doppio Esercizio",
                     "Se la Piva di un'azienda è salvata sia come contatto (organismo referente) che come impresa padre nella gerarchia, verrà visualizzata una volta sola nella tendina degli organismi referenti (verrà mostrato il tipo contatto)")

            Riga_Bug("Creazione raccolta multicentro NG",
                     "La ripartizione manuale delle quantità raccolte ora viene correttamente per le operazioni multicentro")

            Riga_Bug("Dashboard",
                     "Bug fix su logout che non concatenava in query string la pivasuperuser)")

            Riga_Text("Dashboard",
                     "Widget attivita / rilievi / Visite mostrano mappa statica degli impianti)")

            Riga_Text("Dashboard",
                     "Widget dss indicatori funzionante")

            Riga_Text("Dashboard",
                     "Implementato nel Widget visite link modifica/visualizza su ogni operazione ")

            Riga_Text("Anagrafica Appezzamento (Angular)",
                      "Aggiunta gestione tecnico e contributi")

            Riga_Text("Gis 2023",
                      "Caricamento dati etichetta rilievi fasi fenologiche")

            Riga_Text("Cancellazione impianti/appezzamenti collegati al GIS NG",
                      "Aggiunto controllo cancellazione impianto/appezzamento: se esiste un collegamento con un entità gis (poligono) la cancellazione viene impedita")

            Riga_Text("RicercaDocumenti.asmx",
                        "Aggiunto webmethod per nuovo widget dashboard per ultimi acquisti")


            '==================================

            Riga_Data("25 Gennaio 2023")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Modifiche Tabelle Gis 2023", "716")

            Riga_Requisiti("Configurazione_Siti",
                           "Per nuova chiave CompressioneRispostaAjax utilizzata per abilitare compressione risposta standard", "102")

            Riga_Text("Gis 2023",
                      "- Gestione permesso visualizzazione totale" &
                      "- Modifica etichette impianti e rilievi" &
                      "- Lettura tipo oggetto per layer")

            '==================================

            Riga_Data("23 Gennaio 2023")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Aggiunte Tabelle Gis 2023", "715")

            Riga_Requisiti("Configurazione_Siti",
                           "Per nuova chiave CompressioneRispostaAjax utilizzata per abilitare compressione risposta standard", "102")

            Riga_Bug("Appezzamento.vb",
                     "- ROLLBACK Fix lettura codici anagrafe appezzamento per app")

            '==================================

            Riga_Data("20 Gennaio 2023")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Aggiunte Tabelle Gis 2023", "715")

            Riga_Requisiti("Configurazione_Siti",
                           "Per nuova chiave CompressioneRispostaAjax utilizzata per abilitare compressione risposta standard", "102")

            Riga_Bug("Appezzamento.asmx",
                     "- Fix bug per aggiornamento static maps in modifica di appezzamento/impianto")

            Riga_Bug("Appezzamento.vb",
                     "- Fix lettura codici anagrafe appezzamento per app")

            '==================================

            Riga_Data("17 Gennaio 2023")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Aggiunte Tabelle Gis 2023", "715")

            Riga_Requisiti("Configurazione_Siti",
                           "Per nuova chiave CompressioneRispostaAjax utilizzata per abilitare compressione risposta standard", "102")

            Riga_Bug("Widget_Dati.asmx",
                     "- Fix bug per grafico negativo sulla produzione restanti colture")

            Riga_Bug("Impresa (Angular)",
                     "- Fix Impresa Padre")

            '==================================


            Riga_Data("16 Gennaio 2023")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Aggiunte Tabelle Gis 2023", "715")

            Riga_Requisiti("Configurazione_Siti",
                           "Per nuova chiave CompressioneRispostaAjax utilizzata per abilitare compressione risposta standard", "102")

            Riga_Bug("QdC NG",
                     "- Fix caricamento Specie Vegetali e Destinazioni d'uso senza il filtro per il Centro Aziendale")

            Riga_Text("Gis 2023 (Angular)",
                      "- Implementazione funzioni server per lettura nuova tabelle + filtro temporale avanzato")

            Riga_Text("MenuAgenda",
                      "- Aggiunge quantità e unità di misura prodotto in Dettaglio Tecnico attività")

            Riga_Text("Anagrafica Appezzamento (Angular)",
                      "- Aggiunta gestione codici per Low ILUC, terreno inutilizzato e degradato",
                      "ENI")

            Riga_Text("Dashboard",
                      "- Aggiunto WebMethod Logout (per logout da Gias NG)")

            '==================================

            Riga_Data("10 Gennaio 2023")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Aggiunte Colonne ColoreAlternativo / ClasseCssIcona nella tabella MenuBS_2017_Sezioni - Aggiunte tabelle Widgets / Utenti_Widgets", "709")

            Riga_Requisiti("Configurazione_Siti",
                           "Per nuova chiave CompressioneRispostaAjax utilizzata per abilitare compressione risposta standard", "102")

            Riga_Bug("Sincro Dati APP",
                     "- Fix cancellazione agenda da app" &
                     "- Fix lettura dati cartografici impianti")

            '==================================

            Riga_Data("09 Gennaio 2023")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Aggiunte Colonne ColoreAlternativo / ClasseCssIcona nella tabella MenuBS_2017_Sezioni - Aggiunte tabelle Widgets / Utenti_Widgets", "709")

            Riga_Requisiti("Configurazione_Siti",
                           "Per nuova chiave CompressioneRispostaAjax utilizzata per abilitare compressione risposta standard", "102")

            Riga_Bug("ImpiantiEditNG",
                     "Bug Fix: dettaglio_varieta_personalizzato, corretta lettura dato per anagrafica angular")

            Riga_Bug("Anagrafica Impresa",
                     "- Bug Fix su cancellazione impresa e controllo PUA")

            '==================================

            Riga_Data("23 Dicembre 2022")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Aggiunte Colonne ColoreAlternativo / ClasseCssIcona nella tabella MenuBS_2017_Sezioni - Aggiunte tabelle Widgets / Utenti_Widgets", "709")

            Riga_Requisiti("Configurazione_Siti",
                           "Per nuova chiave CompressioneRispostaAjax utilizzata per abilitare compressione risposta standard", "102")

            Riga_Text("UMA",
                      "- Leggi Imprese")

            Riga_Text("Esercizio",
                      "- Cambiamenti Organismo Referente")

            '==================================

            Riga_Data("22 Dicembre 2022")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Aggiunte Colonne ColoreAlternativo / ClasseCssIcona nella tabella MenuBS_2017_Sezioni - Aggiunte tabelle Widgets / Utenti_Widgets", "709")

            Riga_Requisiti("Configurazione_Siti",
                           "Per nuova chiave CompressioneRispostaAjax utilizzata per abilitare compressione risposta standard", "102")

            Riga_Text("GIS 2022",
                      "- GisWS.asmx: gestione ritorno codice entità dopo incolla" &
                      "- Appezzamento.asmx: corretta gestione codice fiscale tecnico in lettura geoJson in scrittura appezzamento")

            Riga_Text("QdC Angular",
                      "- Aggiunge valori di default per note in operazioni colturali")

            Riga_Text("QdC",
                      "- Creazione automatica prodotto al salvataggio impianto")

            Riga_Text("Dashboard Widgtes",
                      "- Aggiunti metodi per caricamento Colture, Rilievi, Attività, Visite")

            Riga_Bug("Nuovo QdC Angular:",
                      "- Sistemata visualizzazione Data Scadenza Patentino Operatore in modifica di una operazione")

            Riga_Text("Angular",
                      "- Gestione passaggio vari siti: Audit, PianiCampionamento, PianiSemina, Uma")

            '==================================

            Riga_Data("20 Dicembre 2022")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Aggiunte Colonne ColoreAlternativo / ClasseCssIcona nella tabella MenuBS_2017_Sezioni - Aggiunte tabelle Widgets / Utenti_Widgets", "709")

            Riga_Requisiti("Configurazione_Siti",
                           "Per nuova chiave CompressioneRispostaAjax utilizzata per abilitare compressione risposta standard", "102")

            Riga_Text("ISTAT",
                      "- Valorizzazione di comune default in elenco province + gestione gerarchia in elenco stati ")

            Riga_Bug("ISTAT - Province",
                      "- Bugfix per lettura province estero da APP")

            '==================================

            Riga_Data("13 Dicembre 2022")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Aggiunte Colonne ColoreAlternativo / ClasseCssIcona nella tabella MenuBS_2017_Sezioni - Aggiunte tabelle Widgets / Utenti_Widgets", "709")

            Riga_Requisiti("Configurazione_Siti",
                           "Per nuova chiave CompressioneRispostaAjax utilizzata per abilitare compressione risposta standard", "102")

            Riga_Text("Dashboard",
                      "Aggiunti webmethod per gestione configurazione widgets")

            Riga_Text("Anagrafica Impianti NG",
                      "Gestiti correttamente cover crops, monitorato e flag Secondo Raccolto")

            Riga_Text("Gis - Gestione tipo entità in salvataggio impianto",
                      "Gestita la modifica del tipo entità, dovuta alla modifica della specie, in fase di salvataggio impianto")

            '==================================

            Riga_Data("06 Dicembre 2022")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Aggiunte Colonne ColoreAlternativo / ClasseCssIcona nella tabella MenuBS_2017_Sezioni - Aggiunte tabelle Widgets / Utenti_Widgets", "709")

            Riga_Requisiti("Configurazione_Siti",
                           "Per nuova chiave CompressioneRispostaAjax utilizzata per abilitare compressione risposta standard", "102")

            Riga_Text("QdC",
                      "- Modifica salvataggio operazioni colturali preferite" &
                      "- Sistemato caricamento UdM")

            Riga_Text("Anagrafica NG: Cancellazione associazione a Piano Concimazione quando vengono cancellati gli impianti",
                     "Se un impianto collegato ad un piano di concimazione viene cancellato, vengono cancellati anche i collegamenti ad esso")

            '==================================

            Riga_Data("02 Dicembre 2022")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Aggiunte Colonne ColoreAlternativo / ClasseCssIcona nella tabella MenuBS_2017_Sezioni - Aggiunte tabelle Widgets / Utenti_Widgets", "709")

            Riga_Requisiti("Configurazione_Siti",
                           "Per nuova chiave CompressioneRispostaAjax utilizzata per abilitare compressione risposta standard", "102")

            Riga_Text("GIS",
                      "- Aggiunto parametro cfgalbero alla chiamata WS LeggiElencoEntitaGeoJson 
                       - Aggiunto campo nodeinfo (di tipo GeoJSONAgroGisPropTreeNode) in risposta alla chiamata WS LeggiElencoEntitaGeoJson 
                      ")


            Riga_Text("Appezzamento",
                      "- Aggiunti campi obj_app e obj_imp su appezzamento\impianto -> modello 
                       - valorizzati campi obj_app e obj_imp su Scrivi_Appezzamento_Anagrafica con GeoJosn della Entità 
                      ")

            Riga_Text("Lettura impianti",
                      "- Aggiunto parametro Germinabilità e Validita Inizio Impianto")

            Riga_Text("Dashboard",
                      "- Lettura / Salvataggio widgets (solo spostamento)")

            Riga_Text("Griglia QdC",
                      "- Aggiunge colonna per Raccoglitore_Cod")

            Riga_Bug("Operazioni Colturali",
                     "Aggiunto controllo non nullità parametri_aggiuntivi_list durante inserimento dosi prodotto")

            Riga_Text("Anagrafica NG",
                      "- Esercizio: Aggiunta certificazione aziendale e certificazione prodotto")

            '==================================

            Riga_Data("28 Novembre 2022")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Aggiunte Colonne ColoreAlternativo / ClasseCssIcona nella tabella MenuBS_2017_Sezioni", "707")

            Riga_Requisiti("Configurazione_Siti",
                           "Per nuova chiave CompressioneRispostaAjax utilizzata per abilitare compressione risposta standard", "102")

            Riga_Text("Log Modifica Anagrafiche",
                      "- Implementata scrittura su Agronica_Log_Anagrafe quando si modificano/inseriscono/cancellano elementi di anagrafica (da azienda fino a esercizio) sia da Anagrafica che da Operazioni NG (ex. abbattimento, semina con frazionamento, raccolta..)
                      (inizio uso del campo 'Note' su tabella per aiutare il programmatore/l'assistenza a capire da dove sono state effettuate le modifiche ex. 'Operazione eseguita da Anagrafica (NG)', 'Abbattimento Impianti (NG)')")

            Riga_Bug("Ribaltamento brogliaccio in QdC da Angular",
                     "Ribaltando un brogliaccio da angular non veniva letto correttamente il Tipo_Destinazione in Ricette_Destinazioni, e quindi veniva perso il collegamento con il magazzino selezionato")

            Riga_Text("Dashboard",
                      "Gestione redirect fix")

            Riga_Text("Anagrafica Impianti", " Gestione Certificazione Commerciale in esercizio ")

            '==================================

            Riga_Data("21 Novembre 2022")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Nuova tabella Utenti_Navigazione_Aziende", "703")

            Riga_Requisiti("Configurazione_Siti",
                           "Per nuova chiave CompressioneRispostaAjax utilizzata per abilitare compressione risposta standard", "102")

            Riga_Text("QdC Angular",
                     "- Aggiunti nuovi campi restituiti nel caricamento delle Macchine")

            Riga_Text("Codifica Prodotti Aziendali",
                      "- Impostata voce 'Non Definito' come principale in elenco Tipo Codifica per Agrisol",
                      "Agrisol")

            '==================================

            Riga_Data("16 Novembre 2022")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Nuova tabella Utenti_Navigazione_Aziende", "703")

            Riga_Requisiti("Configurazione_Siti",
                           "Per nuova chiave CompressioneRispostaAjax utilizzata per abilitare compressione risposta standard", "102")

            Riga_Text("QdC Angular",
                     "- Aggiunti nuovi campi restituiti nel caricamento delle Macchine e Operatori" &
                     " - Aggiunto richiamo alla funzione Controlla_Massimali" &
                     " - Aggiunta funzione blocco attività")

            Riga_Text("Gis Angular",
                     " - Modifica chiamata WS per gestire oggetto 'UtilizzoTerreno' al posto di codice specie." &
                     " - Gestione permesso 365 per visualizzazione layer WMS (esclusione automatica dello stesso layer per il GIS attuale)" &
                     " - Valorizzazione TipologiaLayer e LayerCod in chiave entita elemento GeoJson" &
                     " - Aggiunti nuovi parametri lettura Elenco Entita GeoJson (specie\destinazione uso; campo; elenco layer)")

            Riga_Text("Anagrafica NG",
                      "Aggiunti controlli su cancellazione Azienda/Centro/Campo (se esistono Operazioni registrate sugli impianti / CdG collegati agli esercizi, la cancellazione viene impedita)")

            Riga_Text("Macchine NG",
                      "Gestione caratteristiche")

            '==================================

            Riga_Data("11 Novembre 2022")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Nuova tabella Utenti_Navigazione_Aziende", "703")

            Riga_Requisiti("Configurazione_Siti",
                           "Per nuova chiave CompressioneRispostaAjax utilizzata per abilitare compressione risposta standard", "102")

            Riga_Text("QdC NG ",
                     "- Lettura e salvataggio multi-operazione macchine e operatori" &
                     "- Commentato lettura magazzino gerarchico e terzista")

            Riga_Text("Dashboard ",
                     "- Aggiunti webmethod chiamati dalle API per SideBar e Header nuova dashboard")

            Riga_Text("Raccolta NG ",
                     "- Gestione Cal_Cod come progressivo negativo " &
                     "- Redirect vecchia pagina in caso raccolta con semilavorati")

            '==================================

            Riga_Data("04 Novembre 2022")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Modifiche gestione lotto prodotto", "700")

            Riga_Requisiti("Configurazione_Siti",
                           "Per nuova chiave CompressioneRispostaAjax utilizzata per abilitare compressione risposta standard", "102")

            Riga_Text("Raccolta NG ",
                     "- Nuova raccolta angular WIP")

            Riga_Text("Nuovo QdC NG",
                     "- Blocco Multi Operazioni WIP")

            '==================================

            Riga_Data("25 Ottobre 2022")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Modifiche gestione lotto prodotto", "700")

            Riga_Requisiti("Configurazione_Siti",
                           "Per nuova chiave CompressioneRispostaAjax utilizzata per abilitare compressione risposta standard", "102")

            Riga_Text("Anagrafica Budget Colturale ",
                     "- Aggiunti controlli CdG su modifica date/eliminazione elementi anagrafica budget")

            '==================================

            Riga_Data("24 Ottobre 2022")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Modifiche gestione lotto prodotto", "700")

            Riga_Requisiti("Configurazione_Siti",
                           "Per nuova chiave CompressioneRispostaAjax utilizzata per abilitare compressione risposta standard", "102")

            Riga_Text("Anagrafica NG",
                     "- Gestione certificazione aziendale " &
                     "- Gestione salvataggio immagini macchine ")

            '==================================

            Riga_Data("21 Ottobre 2022")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Modifiche gestione lotto prodotto", "700")

            Riga_Requisiti("Configurazione_Siti",
                           "Per nuova chiave CompressioneRispostaAjax utilizzata per abilitare compressione risposta standard", "102")

            Riga_Text("Gestione Lotto Prodotto",
                     "- Rimozione scrittura lotto 'indefinito', viene ora scritto lotto vuoto ('')" &
                     "- Differenziazione in ricerche fra lotto vuoto ('') e non filtrare per lotto")

            '==================================

            Riga_Data("17 Ottobre 2022")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Aggiunta Colonna Rintracciabilita in Tabella Audit", "696")

            Riga_Requisiti("Configurazione_Siti",
                           "Per nuova chiave CompressioneRispostaAjax utilizzata per abilitare compressione risposta standard", "102")

            Riga_Text("GIS 2022 - GisWS.asmx",
                      "- Nuovo EndPoint SalvaVisibilitaLayer" &
                      "- Memorizzazione opzionale SistemaDiRiferimentoPredefinito")

            '==================================

            Riga_Data("11 Ottobre 2022")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Aggiunta Colonna Rintracciabilita in Tabella Audit", "696")

            Riga_Requisiti("Configurazione_Siti",
                           "Per nuova chiave CompressioneRispostaAjax utilizzata per abilitare compressione risposta standard", "102")

            Riga_Text("Codifica Prodotti Aziendali",
                      "- Aggiunta voce 'Non Definito' in elenco Tipo Codifica per Agrisol")

            '==================================

            Riga_Data("28 Settembre 2022")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Aggiunta Colonna Rintracciabilita in Tabella Audit", "696")

            Riga_Requisiti("Configurazione_Siti",
                           "Per nuova chiave CompressioneRispostaAjax utilizzata per abilitare compressione risposta standard", "102")

            Riga_Text("TipiEnumerativi: ",
                      "aggiunto enum per pagina GISNg")

            Riga_Text("GisWS.asmx/AggiornaElencoTipologie3",
                      "Aggiunto parametro per poter leggere layer non visibili")

            Riga_Text("ControlloInserimentiDosiProdotto",
                      "wip funzione ControlloInserimentiDosiProdotto per nuovo QDC Angular ")

            Riga_Text("DPI.asmx e file collegati",
                      "WIP modifiche a caricamenti disciplinari per nuovo QDC Angular")

            Riga_Text("STD_Sementi, STD_UnitaDiMisura",
                      "Fix funzioni di lettura")

            Riga_Text("Nuovo QdC Angular",
                      "- Fix caricamento grid impianti" &
                      "- Impostato default DPI da impostazione utente/imprese codici")

            '==================================

            Riga_Data("23 Settembre 2022")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Aggiunta Colonna Rintracciabilita in Tabella Audit", "696")

            Riga_Requisiti("Configurazione_Siti",
                           "Per nuova chiave CompressioneRispostaAjax utilizzata per abilitare compressione risposta standard", "102")

            Riga_Text("Fabbricati.asmx/LeggiFabbricati: ",
                      "aggiunto parametro per forzare tipologia di destinazione magazzini")

            '==================================

            Riga_Data("21 Settembre 2022")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Aggiunta Colonna Rintracciabilita in Tabella Audit", "696")

            Riga_Requisiti("Configurazione_Siti",
                           "Per nuova chiave CompressioneRispostaAjax utilizzata per abilitare compressione risposta standard", "102")

            Riga_Text("Anagrafica NG",
                      "- Fix vari")

            '==================================

            Riga_Data("19 Settembre 2022")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Aggiunta Colonna Rintracciabilita in Tabella Audit", "696")

            Riga_Requisiti("Configurazione_Siti",
                           "Per nuova chiave CompressioneRispostaAjax utilizzata per abilitare compressione risposta standard", "102")

            Riga_Text("Anagrafica NG",
                      "- Fix su messaggi ed eliminazione" &
                      "- campo residuo in distinta")

            '==================================

            Riga_Data("12 Settembre 2022")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Aggiunta Colonna Rintracciabilita in Tabella Audit", "696")

            Riga_Requisiti("Configurazione_Siti",
                           "Per nuova chiave CompressioneRispostaAjax utilizzata per abilitare compressione risposta standard", "102")

            Riga_Text("GIS 2022",
                      "- Completamento funzioni CoreWS + CoreAPI" &
                      "- Modifica nomenclatura classi C#")

            '==================================

            Riga_Data("08 Settembre 2022")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Aggiunta Colonna Rintracciabilita in Tabella Audit", "696")

            Riga_Requisiti("Configurazione_Siti",
                           "Per nuova chiave CompressioneRispostaAjax utilizzata per abilitare compressione risposta standard", "102")

            Riga_Bug("Documentale - Contratti",
                      "- fix errore nuovo allegato di tipo contratto")

            '==================================

            Riga_Data("06 Settembre 2022")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Aggiunta Colonna Rintracciabilita in Tabella Audit", "696")

            Riga_Requisiti("Configurazione_Siti",
                           "Per nuova chiave CompressioneRispostaAjax utilizzata per abilitare compressione risposta standard", "102")

            Riga_Bug("Documentale - Nuovo Doc",
                      "fix creazione percorso salvataggio allegati: ora vengono create correttamente sottocartelle in C:\GIASLAN\AgronicaStampe_Allegati")

            '==================================

            Riga_Data("05 Settembre 2022")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Aggiunta Colonna Rintracciabilita in Tabella Audit", "696")

            Riga_Requisiti("Configurazione_Siti",
                           "Per nuova chiave CompressioneRispostaAjax utilizzata per abilitare compressione risposta standard", "102")

            Riga_Text("WebConfig",
                      "Aggiunto globalization ")

            Riga_Bug("Documentale - Nuovo Doc",
                      "fix creazione percorso salvataggio allegati. ora vengono create correttamente sottocartelle in C:\GIASLAN\AgronicaStampe_Allegati")

            '==================================

            Riga_Data("01 Settembre 2022")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Aggiunta Colonna Rintracciabilita in Tabella Audit", "696")

            Riga_Requisiti("Configurazione_Siti",
                           "Per nuova chiave CompressioneRispostaAjax utilizzata per abilitare compressione risposta standard", "102")

            Riga_Text("CoreDpiBIZ",
                      "aggiunta funzione Leggi_Disciplinari_Testata_conRegolamentoConcimazione alla classe DPI_Leggi")

            Riga_Text("Operazioni NG",
                      "Implementazioni filtro operazioni da visualizzare (default preferite) e pulsante salvataggio nuove")

            Riga_Text("Anagrafica NG",
                      "Gestione contatti superuser in creazione nuova impresa")

            Riga_Bug("Anagrafica NG",
                     "Aggiornate funzioni salvataggio Macchine e CostoUnitario, Campi e Appezzamenti")

            Riga_Text("Agenda NG",
                      "Aggiunto reindirizzamento alle pagine RilieviBS, GestioneMagazziniBS, Verifica_DoseConsigliataBS, Verifica_DisciplinareBS")

            Riga_Text("Operazioni NG",
                      "Implementazioni filtro operazioni da visualizzare (default preferite) e pulsante salvataggio nuove")

            Riga_Text("Utilizzo Nuovo Modello STD",
                      "Implementazione di costanti ClassType per sostituire le stringhe volanti nel codice")

            Riga_Text("Intera Soluzione - Nuova costante Terreno Nudo WIP",
                      "Aggiunta costante stringa 'Terreno Nudo', per sostituire le stringhe volanti nel codice ")

            Riga_Text("Operazioni NG",
                      "Implementata lettura ddl Trasformati Vegetali")

            Riga_Text("Angular Menu Agenda Zoo",
                      "Aggiunto il caricamento dello Zoo in menu agenda")

            Riga_Text("GIS",
                      "Primo rilascio back-end GIS 2022")

            Riga_Text("Ricerca Documenti Contabili",
                      "- In ricerca righe estratta nuova colonna 'Aliquota Iva'")

            Riga_Text("QdC NG",
                      "- Gestito redirect all'interno di una nuova Operazione su Angular verso l'AgronicaAgenda_2010" &
                      "- Lettura modello Localizzazione")

            Riga_Text("CoreDAL/Utenti_Profili: ",
                     "Aggiunta generazione log su file nelle operazioni di scrittura, modifica e cancellazione sulla tabella [rif chiamata 19463]")

            Riga_Text("Indirizzi Contatto in Doc Contabile: ",
                     "- Cambiata dicitura sede legale dell'impresa da 'Sede legale' a 'Sede legale Impresa'")

            Riga_Text("Anagrafica NG:",
                     "Check particelle con contratti")

            Riga_Bug("Anagrafica NG - Controlli su movimenti Ricette",
                     "Fix controlli su movimenti ricette")

            Riga_Text("Operazioni NG",
                      "Aggiunto al webmethod salvataggio operazione lista opzioni Operazione")

            '==================================

            Riga_Data("12 Agosto 2022")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Aggiunta Colonna Rintracciabilita in Tabella Audit", "696")

            Riga_Requisiti("Configurazione_Siti",
                           "Per nuova chiave CompressioneRispostaAjax utilizzata per abilitare compressione risposta standard", "102")

            Riga_Text("CoreDpiBIZ",
                      "aggiunta funzione Leggi_Disciplinari_Testata_conRegolamentoConcimazione alla classe DPI_Leggi")

            Riga_Text("Agenda NG",
                      "- Aggiornato modello Attivita" &
                      "- Aggiunta lettura Attivita Personalizzate" &
                      "- Modifiche caricamento grid Impianti")

            Riga_Bug("Agenda.asmx",
                     "Fix caricamento data scadenza in griglia per contratti di affitto")

            Riga_Bug("Catasto.asmx",
                     "Fix filtro su data per caricamento imprese per particelle")

            '==================================

            Riga_Data("09 Agosto 2022")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Aggiunta Colonna Rintracciabilita in Tabella Audit", "696")

            Riga_Requisiti("Configurazione_Siti",
                           "Per nuova chiave CompressioneRispostaAjax utilizzata per abilitare compressione risposta standard", "102")

            Riga_Text("Audit SQNPI COPROB:",
                      "Aggiunto campo 'Rintracciabilità' nella testata checklist ==> AgronicaCoreWS\Audit\Audit.asmx\ScriviAudit && AgronicaCoreAuditDAL\Audit.vb\scrivi && AgronicaCoreAuditBIZ\AuditCheckList.vb\LeggiAuditJson")

            Riga_Text("Menù Visite",
                      "Aggiunto collegamento a documentale in pagina Menù Visite ==>  AgronicaCoreVisite\Visite.asmx.vb\Leggi_ListaVisite_ToKendoGrids")
            '==================================

            Riga_Data("03 Agosto 2022")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Aggiunta Colonna id_ImpreseXParticelle in Tabella Alert_Entita", "695")

            Riga_Requisiti("Configurazione_Siti",
                           "Per nuova chiave CompressioneRispostaAjax utilizzata per abilitare compressione risposta standard", "102")

            Riga_Text("Anagrafica NG",
                      " - Modificata query impianti per lettura e modifica nuovi campi (Bufferzone e NPK)" &
                      " - Modficata query contatti per visibilità centro" &
                      " - Aggiunta query lettura fabbricati ")

            Riga_Text("GiasNG",
                      " - Passaggio Parametri objAgenda tra GiasOnline e GiasNG")

            Riga_Text("Anagrafica NG",
                      "Gestione breadcrumbs (filtri ricerca centro e campo)")

            Riga_Text("Catasto.asmx",
                      "Creato webmethod per lettura Catasto Documentale")

            Riga_Text("Alert_Entita.asmx",
                      "implementate funzioni per gestire nuova colonna Id_ImpresexParticelle per Catasto Documentale")

            Riga_Text("Alert.asmx",
                      "implementata funzione scrittura per gestire nuova colonna Id_ImpresexParticelle per Catasto Documentale")

            '==================================

            Riga_Data("01 Agosto 2022")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Aggiunta Colonna id_ImpreseXParticelle in Tabella Alert_Entita", "695")

            Riga_Requisiti("Configurazione_Siti",
                           "Per nuova chiave CompressioneRispostaAjax utilizzata per abilitare compressione risposta standard", "102")


            Riga_Bug("MenuBS_Agenda_Nuovo",
                      "- Spostato la function CaricaGrigliaOperazioni verso WS da WebForm.")

            '==================================

            Riga_Data("28 Luglio 2022")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "aggiunto raccoglitore_cod in ricette_operazioni",
                           "691")

            Riga_Requisiti("Configurazione_Siti",
                           "Per nuova chiave CompressioneRispostaAjax utilizzata per abilitare compressione risposta standard", "102")

            Riga_Bug("Scadenze Documentale",
                      "- Bugfix apertura scadenza senza allegato: la funzione tentava di ricercare il nome del file anche se non indicato")

            Riga_Text("[Angular] Qdc Menu Agenda - Cancellazione Ricette",
                      "La cancellazione delle ricette insieme ai vincoli (costi)")

            Riga_Text("[Angular] Qdc Menu Agenda - Cancellazione Operazioni",
                      "La cancellazione multipla e singola delle operazioni")

            Riga_Text("[Angular] Qdc Menu Agenda - Carica impostazioni applicazione",
                      "Gli altri vincoli della cancellazione di un'applicazione")

            Riga_Text("[Angular] Qdc Menu Agenda - Codifica prodotti applicazione",
                      "La codifica dei prodotti di un'applicazione")

            Riga_Text("[Angular] Qdc Menu Agenda - reindirizzamento in GIAS2010",
                      "Reindirizzamento da Angular a GIAS2010 di varie pagine")

            '==================================

            Riga_Data("22 Luglio 2022")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "aggiunto raccoglitore_cod in ricette_operazioni",
                           "691")

            Riga_Requisiti("Configurazione_Siti",
                           "Per nuova chiave CompressioneRispostaAjax utilizzata per abilitare compressione risposta standard", "102")

            Riga_Bug("Anagrafica NG",
                      "- Bugfix salvataggio particelle catastali,
                      - Bugfix salvataggio e lettura aziende
                      - Bugfix salvataggio e lettura del CodiceOperatore nei centri
                      - Bugfix corretta lettura e scrittura di CUAAProprietario, N_Autorizzazione_Trasporto e Data Carico e Scarico macchine")

            Riga_Text("Agenda NG",
                      "- Aggiunto reindirizzamento alle pagine VerificaConformita e VerificaSostenibilita" &
                      "- Aggiunta funzione Imposta_DoseConsentitaDiserbo in AgronicaCoreMapper\Utility.vb")

            Riga_Text("Budget Anagrafica",
                      "WIP")

            Riga_Bug("Agenda NG",
                      "- Bugfix corretta lettura disciplinare per letture Verbose")

            Riga_Text("Ricerca Documenti Contabili",
                      "- In ricerca dettagli ordini di acquisto e vendita estratta nuova colonna 'Stato Evasione'")

            Riga_Bug("Scadenze Documentale",
                      "- Corretta gestione localizzazione nella griglia riferimenti contabili in modifica di un allegato")

            '==================================

            Riga_Data("16 Luglio 2022")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "aggiunto raccoglitore_cod in ricette_operazioni",
                           "691")

            Riga_Requisiti("Configurazione_Siti",
                           "Per nuova chiave CompressioneRispostaAjax utilizzata per abilitare compressione risposta standard", "102")

            Riga_Bug("Conferimento",
                      "- Bugfix degrado (descritto in versione agenda) ")

            Riga_Text(" : ",
                      " ")

            '==================================

            Riga_Data("08 Luglio 2022")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "aggiunto raccoglitore_cod in ricette_operazioni",
                           "691")

            Riga_Requisiti("Configurazione_Siti",
                           "Per nuova chiave CompressioneRispostaAjax utilizzata per abilitare compressione risposta standard", "102")

            Riga_Text("Impostazioni Aziende/Centri",
                      "- Aggiunte funzioni lettura/scrittura impostazioni impresa")

            Riga_Text("Profilazione",
                      "- Aggiunte informazioni accesso e GDPR in caricamento " &
                      "- Modifica query lettura permessi")

            Riga_Text("Nuovo QdC Angular:",
                            "- Aggiunte funzioni di Controllo_Sportello e Inizializza_QdC_NG in Agenda.asmx" &
                            "- Aggiunta funzione Controlla_Soglia_Avversita_Soddisfatta in Avversita.asmx" &
                            "- Aggiunta funzione getLinkProfitosan in Utility.asmx" &
                            "- Aggiunta lettura e gestione delle Imprese_Impostazioni in warmUpGiasNG")

            Riga_Text("Anagrafica Prodotti: ",
                      "Aggiunta colonna gruppo merce in griglia elenco prodotti")

            '==================================

            Riga_Data("01 Luglio 2022")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "aggiunto raccoglitore_cod in ricette_operazioni",
                           "691")

            Riga_Requisiti("Configurazione_Siti",
                           "Per nuova chiave CompressioneRispostaAjax utilizzata per abilitare compressione risposta standard", "102")

            Riga_Text("QdC operazioni",
                       "Operazioni.asmx.vb : Modificato il WebMethod Leggi_Operazioni_Modello per leggere anche le combinazioni possibili di operazioni
                        Aggiunta lettura tabella Operazioni_Combinazioni
                       Modificata la classe LeggiOperazioni: aggiunti due nuove proprietà: lista_Lav_Cod (per le operazioni già selezionate) e FiltraImpostazioniUtente per filtrare o meno i lav_cod proposti")

            Riga_Bug("Anagrafica NG",
                       "Sistemazioni varie")

            Riga_Bug("Ricerca Documenti Contabili:",
                      "- Correzione visibilità gruppi merce in ricerca per fatturazione: non venivano correttamente nascosti i documenti")

            '==================================

            Riga_Data("23 Giugno 2022")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "aggiunto raccoglitore_cod in ricette_operazioni",
                           "691")

            Riga_Requisiti("Configurazione_Siti",
                           "Per nuova chiave CompressioneRispostaAjax utilizzata per abilitare compressione risposta standard", "102")

            Riga_Bug("Tipo_Entita_Chiavi.vb per documentale",
                       "Correzione Salvataggio documento (che ha tipo_entita_cod = 0)")

            '==================================

            Riga_Data("21 Giugno 2022")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "aggiunto raccoglitore_cod in ricette_operazioni",
                           "691")

            Riga_Requisiti("Configurazione_Siti",
                           "Per nuova chiave CompressioneRispostaAjax utilizzata per abilitare compressione risposta standard", "102")

            Riga_Bug("Tipo_Entita_Chiavi.vb per documentale",
                       "Correzione Salvataggio documento (Rif. chiamata 18613)")

            '==================================
            Riga_Data("17 Giugno 2022")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "aggiunto raccoglitore_cod in ricette_operazioni",
                           "691")

            Riga_Requisiti("Configurazione_Siti",
                           "Per nuova chiave CompressioneRispostaAjax utilizzata per abilitare compressione risposta standard", "102")

            Riga_Text("Anagrafica Gruppi Merce",
                      "L'interfaccia che permette di gestire i gruppi merce")

            Riga_Text("Prodotti.asmx:",
                      "Aggiunta gestione gruppi merce in ricerca prodotti in caso di chiamata non GiasAPP ed in caso non si debba restituire griglia prodotti")

            Riga_Text("Agenda.asmx:",
                      "Aggiunti webMethod per lettura Agenda e ricette/brogliaccio nel documentale")

            Riga_Text("Alert_Elenco.asmx:",
                      "Aggiunti controlli per visualizzare le colonne legate ad Agenda e Ricette nella ricerca Documenti")

            Riga_Text("Tipo_Entita_Chiavi.asmx:",
                      "Aggiunta gestione tipo entita secondario per il documentale")

            '==================================

            Riga_Data("13 Giugno 2022")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "aggiunto permesso app",
                           "689")

            Riga_Requisiti("Configurazione_Siti",
                           "Per nuova chiave CompressioneRispostaAjax utilizzata per abilitare compressione risposta standard", "102")

            Riga_Bug("AgronicaCoreMapper:",
                      "Fix calcolo qta.")

            '==================================

            Riga_Data("10 Giugno 2022")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "aggiunto permesso app",
                           "689")

            Riga_Requisiti("Configurazione_Siti",
                           "Per nuova chiave CompressioneRispostaAjax utilizzata per abilitare compressione risposta standard", "102")

            Riga_Text("Anagrafica NG",
                      "Fix vari")

            Riga_Text("Alert_Tipologia",
                      "Aggiunto filtro su tipologie documento da passare all'app")

            Riga_Text("Sincro Dati APP",
                      "Aggiunta posizione rilievo + nuove impostazioni/permessi")

            Riga_Text("Autorizzazioni Gruppi Merce",
                      "[GiasNG] Aggiunta la pagina che permette di configurare i Gruppi Utenti per Gruppi Merce")

            Riga_Text("[GiasNG] Menu Agenda QdC",
                      "Aggiunte alcune funzioni che gestiscono funzionalità di Menu Agenda della parte di Angular")

            Riga_Text("Documenti Contabili",
                      "Modificata firma LastNumDocumento")

            Riga_Text("[GiasNG] Nuovo QdC",
                      "Fix caricamento griglia impianti e aggiornamento Parametri_ObjParametriAgenda_NG con Stato e Ricetta_Operazione_Cod.")

            Riga_Text("Ricerca documenti contabili",
                     "- Aggiunte colonne quantità evase e residue in ordini di acquisto")

            Riga_Bug("Ricerca documenti contabili",
                     "- Correzione in calcolo quantità evase e residue in caso di ordini completamente evasi")

            Riga_Text("AnagraficaNG: Appezzamenti",
                     "- Aggiunta gestione degli indirizzi in pagina di Edit")

            Riga_Text("AnagraficaNG: Macchine",
                     "- Aggiornato salvataggio costi macchina, per adeguarlo alle modifiche fatte su tabella Prodotti_Costi per il Budjet")

            Riga_Bug("AnagraficaNG: Macchine",
                     "- Corretta gestione della visibilità della macchina")

            Riga_Bug("Anagrafica: Macchine",
                     "- Corretto salvataggio Mac_Cod_Origine")

            Riga_Text("Ricerca prodotti (Prodotti.asmx)",
                      "- Gestito caricamento prodotti a fronte della categoria Servizi")

            Riga_Text("Contatti.asmx:",
                      "Aggiunto parametro in lettura contatti per documenti contabili, per nuova gestione di collegamento fra conferimenti e raccolte pre-esistenti")

            '==================================

            Riga_Data("03 Giugno 2022")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "per Documentale",
                           "688")

            Riga_Requisiti("Configurazione_Siti",
                           "Per nuova chiave CompressioneRispostaAjax utilizzata per abilitare compressione risposta standard", "102")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Bug("RispostaStandard",
                     "- Fix errore di risposta compressa su catene di chiamate a web methods")

            Riga_Text("Alert, Alert_Elenco, Alert_Entità",
                      "Aggiunta gestione della nuova colonna CompressoDaGias (omonime tabelle) per lettura e scrittura documenti e scadenze (Documentale)")

            Riga_Text("Antivirus.asmx",
                      "Aggiunto webmethod per controllare se l'antivirus è attivo (aggiunto per nuovo componente kendoUpload nel documentale, per evitare il controllo file-to-file se l'antivirus è in realtà inattivo)")

            '==================================

            Riga_Data("27 Maggio 2022")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "per Agenda documentale",
                           "687")

            Riga_Requisiti("Configurazione_Siti",
                           "Per nuova chiave CompressioneRispostaAjax utilizzata per abilitare compressione risposta standard", "102")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Text("Angular appezzamenti", "fix e sviluppi vari per interfaccia")

            Riga_Text("[GiasNG] Quaderno di Campagna",
                      "Funzionalità di copiare un'operazione singola e il reindirizzamento a Gias2010.")

            Riga_Text("Nuovo QdC Angular:",
                      "- Aggiunto filtro per Campo nella lettura degli impianti." &
                      "- Aggiunti come valori restituiti anche Cod_Rapporto e Rapporto_Des nel caricamento della griglia degli Operatori." &
                      "- Aggiunto codice stato tra i valori restituiti nel caricamento della dropdown dei Centri Aziendali.")

            Riga_Text("Budget:", "Creazione asmx per anagrafica Bdg")

            Riga_Text("Anagrafica Angular:", "Ripristinato filtro visibilità temporale")

            Riga_Bug("AnagraficheNG - Macchine -> Costi",
                     "- fix salvataggio prodotti costi
                      - fix caricamento prodotti costi")

            Riga_Text("GiasOnline, InvestimentoCatasto:",
                      "- Implementata lettura dati sui campi
                       - Aggiornata lettura dati sugli Appezzamenti
                       - Aggiunto salvataggio viste griglia")

            Riga_Text("RispostaStandard",
                     "- gestione compressione rispostastandard  lato server / decompressione lato client JS")

            '==================================

            Riga_Data("13 Maggio 2022")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "per Utenti_Viste Angular",
                           "685")

            Riga_Requisiti("Configurazione_Siti",
                           "aggiunte chiavi per export tracciabilità POC", "94")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Text("Angular appezzamenti", "fix e sviluppi vari per interfaccia")

            Riga_Text("[GiasNG] Quaderno di Campagna",
                      "Funzionalità di copiare un'operazione singola e il reindirizzamento a Gias2010.")

            Riga_Text("CoreMetaschemaDAL",
                      "aggiunto core lettura x codifiche ENOGIS / ARTEA:
                        - Codifica_FasiFenologicheEpoche_SistemiEsterni")

            Riga_Text("Documenti contabili - Gestione Permessi Stato Workflow",
                      "- Implementazioni necessarie per controllo centralizzato permessi (cancellazione)" &
                      " in base a gruppo utente e stato workflow" &
                      "- Implementazioni nella ricerca per permessi di modifica e cancellazione in base stato documento")

            Riga_Text("Profilazione - Prima archiviazione",
                      "- Gestione caricamento e salvataggio utenti, tipologie " &
                      "- Prima versione caricamento gerarchia permessi")

            '==================================


            Riga_Data("09 Maggio 2022")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "per Utenti_Viste Angular",
                           "684")

            Riga_Requisiti("Configurazione_Siti",
                           "aggiunte chiavi per export tracciabilità POC", "94")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Text("Angular appezzamenti", "fix e sviluppi vari per interfaccia")

            Riga_Text("Angular Viste",
                      "Aggiunto l'id della griglia come chiave di una vista. Aggiunge flessibilità alla creazione di una vista.")

            '==================================

            Riga_Data("06 Maggio 2022")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Workflow Doc. Contabili",
                           "683")

            Riga_Requisiti("Configurazione_Siti",
                           "aggiunte chiavi per export tracciabilità POC", "94")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Bug("Documentale - Ricerca Documenti",
                     "fix filtri sulle colonne della griglia (ex. Validazione è ora un filtro con i check)")

            '==================================


            Riga_Data("04 Maggio 2022")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Workflow Doc. Contabili",
                           "683")

            Riga_Requisiti("Configurazione_Siti",
                           "aggiunte chiavi per export tracciabilità POC", "94")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Bug("Documentale - Ricerca Documenti",
                     "- fix modifica validazione documento da griglia
                      - fix caricamento documenti per l'utente DemoApp")

            '==================================

            Riga_Data("03 Maggio 2022")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Workflow Doc. Contabili",
                           "683")

            Riga_Requisiti("Configurazione_Siti",
                           "aggiunte chiavi per export tracciabilità POC", "94")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Text("Documenti contabili - Gestione Workflow",
                      "Modificata gestione abilitazione workflow da SuperUser a Impresa")

            Riga_Bug("Angular catasto", "fix modifica chiave particella")

            '==================================

            Riga_Data("02 Maggio 2022")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Workflow Doc. Contabili",
                           "683")

            Riga_Requisiti("Configurazione_Siti",
                           "aggiunte chiavi per export tracciabilità POC", "94")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Text("Redirect Angular Gias NG:",
                      "Aggiunto Id_Agenda in Parametri_ObjParametriAgenda_NG.")

            Riga_Text("GiasNG:",
                      "Funzioni di test per i vari elementi anagrafici.")

            Riga_Bug("CoreWS PianoConcimazione",
                     "Bug fix selezione piano di concimazione standard (nel calcolo degli apporti massimi di macroelementi non rientravano i casi in cui N-P-K non erano presenti nella lista dei fattori correttivi) [rif. chiamata: 17828]")

            Riga_Bug("Angular Macchine", "fix modifica macchina, campo interessato: costo acquisto")

            Riga_Bug("Angular campi", "fix modifica del campo")

            Riga_Text("Qdc Menu Agenda Angular", "Caricamento ricette")

            Riga_Text("Anagrafiche", "gestito codice particella su possessi - modello nuovo")

            '==================================

            Riga_Data("14 Aprile 2022")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Per Nuovo Controllo Diserbo",
                           "682")

            Riga_Requisiti("Configurazione_Siti",
                           "aggiunte chiavi per export tracciabilità POC", "94")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Bug("Appezzamento.asmx", "fix inizializzazione parametro idReg a 0")

            '==================================

            Riga_Data("13 Aprile 2022")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Per Nuovo Controllo Diserbo",
                           "682")

            Riga_Requisiti("Configurazione_Siti",
                           "aggiunte chiavi per export tracciabilità POC", "94")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Text("Utenti",
                      "Aggiunto sorgente CoreWs per scrittura utenti da Angular")

            Riga_Text("Ricerca prodotti da APP",
                      "Gestito il caso di utente che non ha autorizzazione ad andare sotto giacenza che prevale sulle impostazioni legate alla categoria prodotto")

            Riga_Text("CoreMetaschemaDAL",
                      "aggiunti core lettura x codifiche ENOGIS / ARTEA:
                        - Codifica_Macchine_SistemiEsterni
                        - Codifica_Avversita_SistemiEsterni
                        - Codifica_FormeAllevamento_SistemiEsterni
                        - Codifica_Operazioni_SistemiEsterni
                        - Codifica_UnitaMisura_SistemiEsterni
                        - Sistemi_Esterni")

            Riga_Text("Anagrafica NG", "Sviluppi vari")

            Riga_Text("Sito Angular", "caricamento moduli attivi per azienda")

            Riga_Text("Appezzamento", "cambiato dettaglio messaggio errore in creazione/modifica appezzamento")

            '==================================

            Riga_Data("04 Aprile 2022")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Per Documentale",
                           "678")

            Riga_Requisiti("Configurazione_Siti",
                           "aggiunte chiavi per export tracciabilità POC", "94")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Text("Capitolato Cliente",
                      "Creato nuovo web method per la lettura dei DPI Pubblici")

            Riga_Text("Ricerca elenco completo prodotti (singola categoria)",
                      "Allineamento chiamate per aggiunta parametri filtro codice prodotto e filtro codice trappola in pagina Prodotti.asmx")

            '==================================

            Riga_Data("28 Marzo 2022")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Per Documentale",
                           "678")

            Riga_Requisiti("Configurazione_Siti",
                           "aggiunte chiavi per export tracciabilità POC", "94")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Text("Ricerca elenco completo prodotti (singola e multicategoria)",
                      "Aggiunto parametro diversificazione descrizione fertilizzanti in pagina Prodotti.asmx")

            '==================================

            Riga_Data("24 Marzo 2022")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Per Documentale",
                           "678")

            Riga_Requisiti("Configurazione_Siti",
                           "aggiunte chiavi per export tracciabilità POC", "94")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Text("GiasNG:",
                      "Inclusa staticMap in griglia impianti")

            Riga_Bug("GiasNG:",
                      "scrittura e modifica indirizzi imprese/centri")

            Riga_Bug("GiasNG:",
                      "aggiunti campi mancanti a modello ParcoMacchine")

            Riga_Text("GiasNG:",
                      "aggiornata scrittura ParcoMacchine per rispettare nuovo modello")

            Riga_Text("GiasNG:",
                      "aggiornata lettura centri aziendali modello")

            Riga_Bug("Parametrizzatore:",
                      "aggiornata ricerca dei parametri nelle query")

            Riga_Text("Gias:",
                      "aggiornate tabelle impianti ed esercizi Filtrone")

            '==================================

            Riga_Data("17 Marzo 2022")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Per Documentale",
                           "678")

            Riga_Requisiti("Configurazione_Siti",
                           "aggiunte chiavi per export tracciabilità POC", "94")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Text("GiasNG:",
                      "Gestione menu preferiti e sezioni")

            Riga_Text("AnagraficaNG:",
                      "Fix salvataggio impianti")

            '==================================

            Riga_Data("11 Marzo 2022")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Per Documentale",
                           "678")

            Riga_Requisiti("Configurazione_Siti",
                           "aggiunte chiavi per export tracciabilità POC", "94")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Text("Documentale:",
                      "Commentato scrittura file di backup su FS.")

            '==================================

            Riga_Data("04 Marzo 2022 Bis")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Per Documentale",
                           "678")

            Riga_Requisiti("Configurazione_Siti",
                           "aggiunte chiavi per export tracciabilità POC", "94")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Text("Anagrafica NG", "Sviluppi vari su controlli e letture per appezzamenti, centri, fabbricati, imprese, impianti")



            '==================================
            Riga_Data("04 Marzo 2022")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Per Documentale",
                           "678")

            Riga_Requisiti("Configurazione_Siti",
                           "aggiunte chiavi per export tracciabilità POC", "94")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Bug("RicercaDocumenti.asmx:",
                      "Fix in query ricerca dcomuneti per conteggio Documenti Allegati (inner join su alert_elenco x scartare righe non collegate)")

            Riga_Text("Ricerca documenti:",
                      " - Condizionata la visibilità pulsante documenti in base alla presenza")

            '==================================

            Riga_Data("01 Marzo 2022")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Per Documentale",
                           "678")

            Riga_Requisiti("Configurazione_Siti",
                           "aggiunte chiavi per export tracciabilità POC", "94")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Text("Alert_Entita.asmx:",
                      "Aggiunto funzione Leggi_documenti_con_agenda")

            '==================================

            Riga_Data("25 Febbraio 2022")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Per colonna Tipo_Associazione in Mov_Dettagli_Riferimenti",
                           "677")

            Riga_Requisiti("Configurazione_Siti",
                           "aggiunte chiavi per export tracciabilità POC", "94")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            '==================================

            Riga_Data("16 Febbraio 2022")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Per colonna Tipo_Associazione in Mov_Dettagli_Riferimenti",
                           "677")

            Riga_Requisiti("Configurazione_Siti",
                           "aggiunte chiavi per export tracciabilità POC", "94")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Text("Anagrafica ANGULAR:",
                      "Fine implementazione controlli a cascata sulle validità e aggiunti controlli su Operazioni/Ricette/Pua per impianti")

            Riga_Text("Redirect GiasNG:",
                      " - Migliorate redirect da e per GiasNG")

            Riga_Text("Lavorazioni.asmx:",
                      "Nuove funzioni per lettura ordini lavorazione in fase di inserimento lavorazione")

            Riga_Text("Contatti.asmx:",
                      "Aggiunto parametro in lettura contatti per documenti contabili, per nuova gestione di collegamento fra conferimenti e raccolte pre-esistenti")

            '==================================

            Riga_Data("10 Febbraio 2022")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Per importazione lavorazioni macchine",
                           "676")

            Riga_Requisiti("Configurazione_Siti",
                           "aggiunte chiavi per export tracciabilità POC", "94")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Text("Modificate tabelle importazione lavorazioni macchine:",
                      "- In importazione calibrature, rimozione colonna riferimento calibratrice" &
                      "- In linee macchine lavorazione, aggiunta colonna tipo importatore" &
                      "- Rimossa tabella calibratrici")

            Riga_Text("Anagrafica ANGULAR:",
                      "Implementati controlli a cascata sulle validità (con cancellazione di Impianti/Esercizi se necessario) per Appezzamenti, Campi e Centri")

            Riga_Bug("Anagrafica ANGULAR:",
                     "Bug fix vari lettura codici di Appezzamenti, Campi, Centri e Imprese")

            Riga_Bug("Anagrafica ANGULAR - Parco Macchine:",
                     "- Bug fix lettura e scrittura costi Macchine" &
                     "- Bug fix modifica Macchina: se la visibilità veniva modificata generava un errore della Primary Key. Fixato sovrascivendo il record.")

            '==================================

            Riga_Data("04 Febbraio 2022")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Per nuova tabella FF_ImportDatiMacchineOperazioniAgenda",
                           "674")

            Riga_Requisiti("Configurazione_Siti",
                           "aggiunte chiavi per export tracciabilità POC", "94")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Text("Documentale:",
                      "- Migliorata Gestione dei LOG di Errore, Scrittura, Modifica e Cancellazione di un documento.")

            '==================================

            Riga_Data("01 Febbraio 2022")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Per nuova tabella FF_ImportDatiMacchineOperazioniAgenda",
                           "674")

            Riga_Requisiti("Configurazione_Siti",
                           "aggiunte chiavi per export tracciabilità POC", "94")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Text("Anagrafica ANGULAR:",
                      "Aggiunta controlli a cascata validità per Impresa, Centro, Campo e Appezzamento")

            Riga_Text("Anagrafica ANGULAR:",
                      "Aggiunta scrittura codici e indirizzi per Centro")

            Riga_Bug("Anagrafica ANGULAR:",
                     "Bug fix vari su scrittura centro, codici, campo_codici e indirizzi")

            Riga_Bug("Ricerca Doc Contabili:",
                     "Correzione nel recupero delle note dei documenti di registrazione")
            '==================================

            Riga_Data("26 Gennaio 2022 bis")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Per nuova tabella FF_ImportDatiMacchineOperazioniAgenda",
                           "674")

            Riga_Requisiti("Configurazione_Siti",
                           "aggiunte chiavi per export tracciabilità POC", "94")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Text("Anagrafica ANGULAR:",
                     "Aggiunti: scrittura Gerarchia Imprese, controlli su PIVA, scrittura impresa come contatto Cliente e Fornitore superuser")

            Riga_Bug("Anagrafica ANGULAR:",
                     "Bug fix modifica codici")

            Riga_Bug("Anagrafica ANGULAR:",
                     "Bug fix scrittura UtentixImprese")

            Riga_Bug("Documentale:",
                     "Bug fix: Reso univoco il nome del file su disco anche in modifica")

            Riga_Bug("Documentale:",
                     "Bug fix: Controllata esistenza del file su disco anche in modifica prima di salvare i records su tabella")

            Riga_Bug("Documentale:",
                     "Miglioramento: Confrontato array di byte usato per salvare il file su disco con quello dopo la rilettura in modo da dare errore se non sono identici")

            '==================================

            Riga_Data("26 Gennaio 2022")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")
            Riga_Requisiti("Migra",
                           "Per nuova tabella FF_ImportDatiMacchineOperazioniAgenda",
                           "674")

            Riga_Requisiti("Configurazione_Siti",
                           "aggiunte chiavi per export tracciabilità POC", "94")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Bug("Anagrafica ANGULAR:",
                     "Fix webservice Appezzamento su controllo gruppo finalità in creazione (applicato solo se la specie\varietà è valorizzata)")
            '==================================

            Riga_Data("24 Gennaio 2022")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Per nuova tabella FF_ImportDatiMacchineOperazioniAgenda",
                           "674")

            Riga_Requisiti("Configurazione_Siti",
                           "aggiunte chiavi per export tracciabilità POC", "94")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Bug("GIS su EF:",
                     "CoreWS - Gestione dei dati Spaziali in Entity Framework su sistemi win11 o Srv 2022 ... errore segnalato: Spatial types and functions are not available for this provider because the assembly 'Microsoft.SqlServer.Types' version 10 or higher could not be found.")

            '==================================

            Riga_Data("21 Gennaio B 2022")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Per nuova tabella FF_ImportDatiMacchineOperazioniAgenda",
                           "674")

            Riga_Requisiti("Configurazione_Siti",
                           "aggiunte chiavi per export tracciabilità POC", "94")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Bug("Anagrafica ANGULAR:",
                     "Fix generali per MacchineNG, ImpresaNG, CampiNG, AppezzamentiNG e scrittura Indirizzi (valore defualt com e prov ISTAT)")

            Riga_Text("Anagrafica ANGULAR:",
                     "Aggiunti controlli date CampiNG")

            Riga_Text("Documentale:",
                     "Migliorata la velocità della Ricerca dei documenti.")

            '==================================

            Riga_Data("21 Gennaio 2022")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Per nuova tabella FF_ImportDatiMacchineOperazioniAgenda",
                           "674")

            Riga_Requisiti("Configurazione_Siti",
                           "aggiunte chiavi per export tracciabilità POC", "94")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Bug("Anagrafica ANGULAR:",
                     "Fix generali per MacchineNG, ImpresaNG, CampiNG, AppezzamentiNG e scrittura Indirizzi (valore defualt com e prov ISTAT)")

            Riga_Text("Anagrafica ANGULAR:",
                     "Aggiunti controlli date CampiNG")

            Riga_Text("Web Service Appezzamento:",
                     "Fix gestione data inizio-fine validità di appezzamento e impianto")

            Riga_Text("Web Service Appezzamento:",
                     "Fix gestione dati indirizzo appezzamento")

            '==================================

            Riga_Data("17 Gennaio 2022")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Per nuova tabella FF_ImportDatiMacchineOperazioniAgenda",
                           "674")

            Riga_Requisiti("Configurazione_Siti",
                           "aggiunte chiavi per export tracciabilità POC", "94")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Text("Traccibialità:",
                      "BIZ e DAL per gestione tabella FF_ImportDatiMacchineOperazioniAgenda (WIP)")

            Riga_Text("Anagrafica ANGULAR:",
                     "Aggiunti controlli salvataggio Appezzamenti e Impianti")

            Riga_Bug("Anagrafica ANGULAR:",
                     "Fixato errore nella lettura delle validità dei Campi")

            Riga_Text("Cache:",
                      "Aggiunto asmx con metodo ClearCache chiamato da esterno")

            Riga_Text("Cache:",
                      "Aggiunto scache automatico in scrittura/Modifica configurazione siti")

            '==================================

            Riga_Data("03 Gennaio 2022")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Per colonne su APP_Documenti che collegano attività e destinazioni",
                           "673")

            Riga_Requisiti("Configurazione_Siti",
                           "aggiunte chiavi per export tracciabilità POC", "94")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Text("Core FreshAndFood:",
                     "Nuova funzione per controllo univocità identificativo lavorazione")

            Riga_Text("Core APP:",
                     "Caricamento di allegati in tabelle di frontiera (ad esempio dati BTM di SDF) collegati con attività (APP_Ricette_Operazioni) e destinazioni (APP_Ricette_Destinazioni_ID)")

            Riga_Text("Core APP:",
                     "Fix anomalia su estensione poligono con STBuffer")

            Riga_Text("Core APP:",
                     "Prima del salvataggio di un poligno (via EF) viene effettuato ora un test per invertire i punti se questo viene disegnato in senso orario")

            Riga_Text("Anagrafica:",
                     "Salvataggio catasto appezzamento ")

            '==================================

            Riga_Data("27 Dicembre 2021")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Per modifica tipo colonne Descrizione_* tabella Utenti_Profili",
                           "672")

            Riga_Requisiti("Configurazione_Siti",
                           "aggiunte chiavi per export tracciabilità POC", "94")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Bug("CoreDAL CentriAziendali e GerarchiaImprese EF:",
                     "Bug fix per mantenere allineata utenti_visibilita_appoggio all'aggiunta/cancellazione dei centri e gerarchia imprese EF")


            Riga_Bug("GIS:",
                     "La funzione che ""aumenta"" il poligono utilizzando le routine di SQL ora gestisce sia poligoni disegnati in senso orario che antiorario")

            '==================================

            Riga_Data("17 Dicembre 2021")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Indispensabile per integrazione macchine (Lavorazioni / Ordini Lavorazione)",
                           "669")

            Riga_Requisiti("Configurazione_Siti",
                           "aggiunte chiavi per export tracciabilità POC", "94")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Bug("Tracciabilità",
                     "x POC: Fix estrazione dati per elem_cod_padre,mat_cod_padre, pro_cod_padre")

            '==================================

            Riga_Data("15 Dicembre 2021")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Indispensabile per integrazione macchine (Lavorazioni / Ordini Lavorazione)",
                           "669")

            Riga_Requisiti("Configurazione_Siti",
                           "aggiunte chiavi per export tracciabilità POC", "94")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Text("Impianti.asmx:",
                      "Lettura Anteprima PNG dai dati salvati attraverso le google maps static API")

            Riga_Bug("FreshAndFood.asmx:",
                      "Bug Fix su LeggiProdotti_FF in caso di utilizzo parametro filters")

            '==================================

            Riga_Data("13 Dicembre 2021")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Indispensabile per integrazione macchine (Lavorazioni / Ordini Lavorazione)",
                           "669")

            Riga_Requisiti("Configurazione_Siti",
                           "aggiunte chiavi per export tracciabilità POC", "94")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Text("Core:",
                      "Integrata libreria SSH.Net per gestione SFTP Client")

            '==================================

            Riga_Data("09 Dicembre 2021")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Indispensabile per integrazione macchine (Lavorazioni / Ordini Lavorazione)",
                           "669")

            Riga_Requisiti("Configurazione_Siti",
                           "aggiunta chiave DataProviderLogConfig", "80")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Text("Core:",
                      "Aggiornamento dei core interni")

            '==================================

            Riga_Data("06 Dicembre 2021")

            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Indispensabile per integrazione macchine (Lavorazioni / Ordini Lavorazione)",
                           "669")

            Riga_Requisiti("Configurazione_Siti",
                           "aggiunta chiave DataProviderLogConfig", "80")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Text("Core:",
                      "Aggiornamento dei core interni")

            '==================================

            Riga_Data("03 Dicembre 2021")

            Riga_Requisiti("AgronicaWebService2010",
            "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
            "Indispensabile per integrazione macchine (Lavorazioni / Ordini Lavorazione)", "669")

            Riga_Requisiti("Configurazione_Siti",
            "aggiunta chiave DataProviderLogConfig", "80")

            Riga_Requisiti("WEB.CONFIG :",
            "Modifiche NO")

            Riga_Text("Core:",
                      "Aggiornamento dei core interni")

            '==================================

            Riga_Data("29 Novembre 2021 bis")

            Riga_Requisiti("AgronicaWebService2010",
            "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
            "Indispensabile per integrazione macchine (Lavorazioni / Ordini Lavorazione)", "669")

            Riga_Requisiti("Configurazione_Siti",
            "aggiunta chiave DataProviderLogConfig", "80")

            Riga_Requisiti("WEB.CONFIG :",
            "Modifiche NO")

            Riga_Bug("Ricerca giacenze:", "Correzione alle funzioni che costruiscono le select per ordine campi errato che faceva fallire la Union")

            '==================================

            Riga_Data("29 Novembre 2021")

            Riga_Requisiti("AgronicaWebService2010",
            "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
            "Indispensabile per integrazione macchine (Lavorazioni / Ordini Lavorazione)", "669")

            Riga_Requisiti("Configurazione_Siti",
            "aggiunta chiave DataProviderLogConfig", "80")

            Riga_Requisiti("WEB.CONFIG :",
            "Modifiche NO")

            Riga_Text("Core:", "Aggiunto progetto AgronicaCoreIntegrazioneMacchine")

            Riga_Text("Movimenti:", "Aggiunto campo Cod_Macchina_Lav con relativa gestione in BIZ / DAL nella tabella Movimenti")

            Riga_Text("Lavorazioni:", "Gestione Lotto Testata in Configurazione Lavorazioni")


            '==================================


            Riga_Data("19 Novembre 2021")

            Riga_Requisiti("AgronicaWebService2010",
                       "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Indispensabile per Documentale", "668")

            Riga_Requisiti("Configurazione_Siti",
                           "aggiunta chiave DataProviderLogConfig", "80")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Text("Core:",
                      "Aggiornamento dei core interni")

            '==================================

            Riga_Data("08 Novembre 2021")


            Riga_Requisiti("AgronicaWebService2010",
                           "per PUA", "10/03/2021")

            Riga_Requisiti("Migra",
                           "Indispensabile per Documentale", "668")

            Riga_Requisiti("Configurazione_Siti",
                           "aggiunta chiave DataProviderLogConfig", "80")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Text("DataProvider :",
                      "<ul><li>Aggiunta cache per risultati statici (<code>Configurazione_Siti_R</code>)</li>" &
                      "<li>Aggiunta gestione cache per query in errore causa lunghezza eccessiva</li>" &
                      "<li>Aggiunta gestione cache per query in errore causa tempo parsing troppo lungo</li></ul>")

            Riga_Bug("DataProvider :",
                     "Bug fix su finta injection ")

            Riga_Text("Scadenzario :",
                      "Invio email per rapporto contabile ")

            '==================================

            Riga_Data("28 Ottobre 2021")


            Riga_Requisiti("AgronicaWebService2010: 10/03/2021",
                           "per PUA")

            Riga_Requisiti("Migra '667 ",
                           "Indispensabile per GIS ")

            Riga_Requisiti("Configurazione_Siti '80' :",
                           "aggiunta chiave DataProviderLogConfig")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Text("Appezzamento.asmx:",
                      "Lettura dell'immagine statica associata al poligono")

            Riga_Bug("Ricerca Doc. Contabili:",
                      " Bug Fix Numero Accettazione che non compariva se era troppo lungo ")

            Riga_Text("Documentale:",
                      " Storicizzazione Documentale ")


            '==================================

            Riga_Data("13 Ottobre 2021")

            Riga_Requisiti("AgronicaWebService2010: 10/03/2021",
                           "per PUA")

            Riga_Requisiti("Migra '665 ",
                           "Indispensabile per GIS ")

            Riga_Requisiti("Configurazione_Siti '80' :",
                           "aggiunta chiave DataProviderLogConfig")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Bug("Alert_Elenco.asmx:",
                      "Funzione Cancella: correzione per la localizzazione")

            Riga_Text("Alert.VB:",
                      "Importazione Report QDC in Documentale per Agribologna")

            Riga_Text("GIS:",
                      " Al salvataggio del poligono a video, se configurato (chiave: <code>GIS_StaticMapCFG</code>) viene memorizzato un png nella nuova colonna static map in <code>gis_elementiGrafici</code> ")

            Riga_Text("Documentale:",
                      " Importazione Report QDC in Documentale ")



            '==================================

            Riga_Data("06 Settembre 2021")

            Riga_Requisiti("AgronicaWebService2010: 10/03/2021",
                           "per PUA")

            Riga_Requisiti("Migra '659': ",
                           "Indispensabile per Documentale")

            Riga_Requisiti("Migra '659': ",
                           "Indispensabile per Schedulazione Documenti")

            Riga_Requisiti("Configurazione_Siti '80' :",
                           "aggiunta chiave DataProviderLogConfig")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Text("Imprese.asmx: ",
                      "migliorata chiamata carica imprese ")

            '==================================
            '==================================

            Riga_Data("27 Agosto 2021")

            Riga_Requisiti("AgronicaWebService2010: 10/03/2021",
                           "per PUA")

            Riga_Requisiti("Migra '659': ",
                           "Indispensabile per Documentale")

            Riga_Requisiti("Migra '659': ",
                           "Indispensabile per Schedulazione Documenti")

            Riga_Requisiti("Configurazione_Siti '80' :",
                           "aggiunta chiave DataProviderLogConfig")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Text("GisWS.asmx: ",
                      "API di chiamata a funzione STBuffer di SQL Spatial")

            '==================================

            Riga_Data("18 Agosto 2021")

            Riga_Requisiti("AgronicaWebService2010: 10/03/2021",
                           "per PUA")

            Riga_Requisiti("Migra '657' + Aggancio '94': ",
                           "per permessi GiasAPP")

            Riga_Requisiti("Configurazione_Siti '80' :",
                           "aggiunta chiave DataProviderLogConfig")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Text("Utenti_Impostazioni_R.asmx: ",
                      "aggiunta gestione permessi utente per GiasAPP")
            Riga_Text("Documentale:",
                      "BugFix in Modifica Documento quando si cambia l'allegato.")

            '==================================

            Riga_Data("05 Agosto 2021")

            Riga_Requisiti("AgronicaWebService2010: 10/03/2021",
                           "per PUA")

            Riga_Requisiti("Migra '657' + Aggancio '94': ",
                           "per permessi GiasAPP")

            Riga_Requisiti("Configurazione_Siti '80' :",
                           "aggiunta chiave DataProviderLogConfig")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Text("Utenti_Impostazioni_R.asmx: ",
                      "aggiunta gestione pemessi utente per GiasAPP")

            Riga_Text("Alert.asmx: ",
                      "aggiunta funzione per passaggio docuemnti su database")

            '==================================

            Riga_Data("28 Luglio 2021")

            Riga_Requisiti("AgronicaWebService2010: 10/03/2021",
                           "per PUA")

            Riga_Requisiti("Migra '655' + Aggancio '94': ",
                           "per UMA")

            Riga_Requisiti("Configurazione_Siti '80' :",
                           "aggiunta chiave DataProviderLogConfig")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Text("Anagrafica: ",
                      "chiamate per Angular")

            Riga_Text("Ricerca Documenti: ",
                      " implementato Order By con sort del DT per ovviare a rallentamenti Order By Sql Server")

            '==================================

            Riga_Data("22 Luglio 2021")

            Riga_Requisiti("AgronicaWebService2010: 10/03/2021",
                           "per PUA")

            Riga_Requisiti("Migra '652' + Aggancio '94': ",
                           "per UMA")

            Riga_Requisiti("Configurazione_Siti '80' :",
                           "aggiunta chiave DataProviderLogConfig")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Text("import export agribologna: ",
                      "import ddt e export trapianti")

            '==================================

            '==================================

            Riga_Data("20 Luglio 2021")

            Riga_Requisiti("AgronicaWebService2010: 10/03/2021",
                           "per PUA")

            Riga_Requisiti("Migra '652' + Aggancio '94': ",
                           "per UMA")

            Riga_Requisiti("Configurazione_Siti '80' :",
                           "aggiunta chiave DataProviderLogConfig")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            '==================================

            '==================================

            Riga_Data("13 Luglio 2021")

            Riga_Requisiti("AgronicaWebService2010: 10/03/2021",
                           "per PUA")

            Riga_Requisiti("Migra '652' + Aggancio '94': ",
                           "per UMA")

            Riga_Requisiti("Configurazione_Siti '80' :",
                           "aggiunta chiave DataProviderLogConfig")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Text("CDG:",
                      "Correzione Bug Aboca Dati da App")

            '==================================

            Riga_Data("08 Luglio 2021")

            Riga_Requisiti("AgronicaWebService2010: 10/03/2021",
                           "per PUA")

            Riga_Requisiti("Migra '652' + Aggancio '94': ",
                           "per UMA")

            Riga_Requisiti("Configurazione_Siti '80' :",
                           "aggiunta chiave DataProviderLogConfig")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Text("UMA:",
                      "modifiche varie")

            '==================================

            Riga_Data("05 Luglio 2021")

            Riga_Requisiti("AgronicaWebService2010: 10/03/2021",
                           "per PUA")

            Riga_Requisiti("Migra '652' + Aggancio '94': ",
                           "per UMA")

            Riga_Requisiti("Configurazione_Siti '80' :",
                           "aggiunta chiave DataProviderLogConfig")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Text("Codifiche:",
                      "modificata funzione ElencoTipiCodifica x fruitmodena")


            '==================================

            Riga_Data("25 Giugno 2021")

            Riga_Requisiti("AgronicaWebService2010: 10/03/2021",
                           "per PUA")

            Riga_Requisiti("Migra '651' + Aggancio '83': ",
                           "")

            Riga_Requisiti("Configurazione_Siti '80' :",
                           "aggiunta chiave DataProviderLogConfig")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Text("UMA:",
                      "Modifiche documentale")

            '==================================
            '==================================

            Riga_Data("18 Giugno 2021")

            Riga_Requisiti("AgronicaWebService2010: 10/03/2021",
                           "per PUA")

            Riga_Requisiti("Migra '650' + Aggancio '83': ",
                           "Indispensabile per documentale")

            Riga_Requisiti("Configurazione_Siti '80' :",
                           "aggiunta chiave DataProviderLogConfig")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Text("UMA:",
                      "Modifiche documentale")

            '==================================
            '==================================

            Riga_Data("11 Giugno 2021")

            Riga_Requisiti("AgronicaWebService2010: 10/03/2021",
                           "per PUA")

            Riga_Requisiti("Migra '647' + Aggancio '83': ",
                           "Indispensabile per documentale")

            Riga_Requisiti("Configurazione_Siti '80' :",
                           "aggiunta chiave DataProviderLogConfig")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Text("Sequenza Tabelle con EntityFramework:",
                      "Bugfix per cui per il primo record di ogni tabella la funzione NuovoId_Tabella_EF non eseguiva il SaveChanges")

            '==================================

            Riga_Data("07 Giugno 2021")

            Riga_Requisiti("AgronicaWebService2010: 10/03/2021",
                           "per PUA")

            Riga_Requisiti("Migra '647' + Aggancio '83': ",
                           "Indispensabile per documentale")

            Riga_Requisiti("Configurazione_Siti '80' :",
                           "aggiunta chiave DataProviderLogConfig")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Text("CategTipologiaDocumentiXUtenti_DAL.vb:",
                      "Fix Controllo Permessi Nuovo Documento ")

            '==================================

            Riga_Data("31 Maggio 2021")

            Riga_Requisiti("AgronicaWebService2010: 10/03/2021",
                           "per PUA")

            Riga_Requisiti("Migra '647' + Aggancio '83': ",
                           "Indispensabile per documentale")

            Riga_Requisiti("Configurazione_Siti '80' :",
                           "aggiunta chiave DataProviderLogConfig")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Text("IVA_Aliquote.amsx:",
                      "Gestiti tutti i parametri di filtro + ordinamento per Sigla e non più Codice")

            Riga_Text("Utenti_Impostazioni_R.asmx:",
                      "Lettura impostazioni GIS e Modalità Bordo Macchine per GIAS APP ")

            Riga_Text("Alert_Area.asmx e Alert_Tipologia.asmx:",
                      "Aggiunto parametro tipoPermessoDaControllare per permessi Documentale. ")

            Riga_Text("Alert_Elenco.asmx:",
                      "Gestione permessi Documentale. ")

            Riga_Text("Alert_Elenco.asmx:",
                      "Gestione Schema Template UMA ")

            '==================================

            Riga_Data("18 Maggio 2021")

            Riga_Requisiti("AgronicaWebService2010: 10/03/2021",
                           "per PUA")

            Riga_Requisiti("Migra '644' + Aggancio '83': ",
                           "Indispensabile per tabelle APP + Tracciabilità")

            Riga_Requisiti("Configurazione_Siti '80' :",
                           "aggiunta chiave DataProviderLogConfig")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Text("Alert_Elenco.amsx:",
                      "Modificata Leggi_Elenco_ToKendoGrid_New per gestire  permessi sulla Categoria e Tipologia utenti")

            '==================================

            Riga_Data("14 Maggio 2021")

            Riga_Requisiti("AgronicaWebService2010: 10/03/2021",
                           "per PUA")

            Riga_Requisiti("Migra '644' + Aggancio '83': ",
                           "Indispensabile per tabelle APP + Tracciabilità")

            Riga_Requisiti("Configurazione_Siti '80' :",
                           "aggiunta chiave DataProviderLogConfig")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Text("Tracciabilità:",
                      "Bugfix per collegamenti lavorazioni")

            '==================================

            Riga_Data("10 Maggio 2021 ")

            Riga_Requisiti("AgronicaWebService2010: 10/03/2021",
                           "per PUA")

            Riga_Requisiti("Migra '642' + Aggancio '83': ",
                           "Indispensabile per tabelle APP ")

            Riga_Requisiti("Configurazione_Siti '80' :",
                           "aggiunta chiave DataProviderLogConfig")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Text("Caricamento Dati APP:",
                      " Corretta Anomalia: importazione Agenda da Tabelle frontiera APP: se ci sono più operazioni per ciascun upload non vegono creati i record nella tabella movimenti ")

            '==================================

            Riga_Data("29 Aprile 2021 ")

            Riga_Requisiti("AgronicaWebService2010: 10/03/2021",
                           "per PUA")

            Riga_Requisiti("Migra '642' + Aggancio '83': ",
                           "Indispensabile per tabelle APP ")

            Riga_Requisiti("Configurazione_Siti '80' :",
                           "aggiunta chiave DataProviderLogConfig")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Text("Documentale:",
                      " modificati filtri sugli indici")

            '==================================

            Riga_Data("27 Aprile 2021 ")

            Riga_Requisiti("AgronicaWebService2010: 10/03/2021",
                           "per PUA")

            Riga_Requisiti("Migra '642' + Aggancio '83': ",
                           "Indispensabile per tabelle APP ")

            Riga_Requisiti("Configurazione_Siti '80' :",
                           "aggiunta chiave DataProviderLogConfig")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Text("Report Acquisti:",
                      " cambiata  logica di filtro - Applicato filtro visibilità utente")

            '==================================

            Riga_Data("26 Aprile 2021 ")

            Riga_Requisiti("AgronicaWebService2010: 10/03/2021",
                           "per PUA")

            Riga_Requisiti("Migra '642' + Aggancio '83': ",
                           "Indispensabile per tabelle APP ")

            Riga_Requisiti("Configurazione_Siti '80' :",
                           "aggiunta chiave DataProviderLogConfig")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Text("UMA:",
                      "Lettura tabelle metaschema")

            Riga_Text("Alert_Elenco.asmx - Ricerca Documenti:",
                      "Modificata la Leggi_Elenco_ToKendoGrid_New per visualizzare nella griglia di Ricerca dei Documenti Sì/No al posto di True/False come valore degli Indici. ")

            '==================================

            Riga_Data("21 Aprile 2021 ")

            Riga_Requisiti("AgronicaWebService2010: 10/03/2021",
                           "per PUA")

            Riga_Requisiti("Migra '642' + Aggancio '83': ",
                           "Indispensabile per tabelle APP ")

            Riga_Requisiti("Configurazione_Siti '80' :",
                           "aggiunta chiave DataProviderLogConfig")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Text("Albero 2017 (fast via jSon):",
                      "- Albero Anagrafica - Rimossa lettura delle icone, in attesa di gestione diversa: Lo ""split"" della funzione ""QryDaElencoIcone"" manda in timeout la query")

            Riga_Text("Rilievi.asmx:",
                      "- GIAS APP - Lettura di tabella Matrice a favore della GIAS APP ")

            Riga_Text("Alert.asmx:",
                      "Aggiunto web method per importazione documenti da tabelle di frontiera app")

            Riga_Text("SqlDataProvider:",
                      "Bug fix (errore Tettetruria di venerdi su task lanciato da AgroGsb), migliorie")

            Riga_Text("Visite.asmx:",
                      "Aggiunta gestione per scrivere documenti allegati alle visite APP su tabelle di frontiera")

            Riga_Text("Ricette.asmx:",
                      "Tabelle frontiera app: Tabella APP_Ricette_Dettaglio_Tecnico, nuova colonna FF_Classe ")

            Riga_Text("Ricette.asmx:",
                      "Riporto da tabelle frontiera APP a tabelle Agenda per rilievi ")

            Riga_Text("Utenti_Impostazioni_R.asmx:",
                      "Lettura impostazioni Pro-APP ")

            Riga_Text("Alert_Tipologia.asmx:",
                      "Aggiunto web method per lettura Categoria e Tipologia da app")

            '==================================

            Riga_Data("06 Aprile 2021")

            Riga_Requisiti("AgronicaWebService2010: 10/03/2021",
                           "per PUA")

            Riga_Requisiti("Migra '639' + Aggancio '83': ",
                           "Indispensabile")

            Riga_Requisiti("Configurazione_Siti '80' :",
                           "aggiunta chiave DataProviderLogConfig")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Text("Albero 2017 (fast via jSon):",
                      "- Aggiustamenti vari")

            '==================================

            Riga_Data("01 Aprile 2021")

            Riga_Requisiti("AgronicaWebService2010: 10/03/2021",
                           "per PUA")

            Riga_Requisiti("Migra '635' + Aggancio '83': ",
                           "Indispensabile")

            Riga_Requisiti("Configurazione_Siti '80' :",
                           "aggiunta chiave DataProviderLogConfig")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Text("Imprese.asmx:",
                      " - Carica Imprese con CUAA ")

            Riga_Text("Prodotti.asmx:",
                      "- Aggiunto web method per lettura da app dei prodotti non magazzino (banche dati)." &
                      "- Aggiunto web method per lettura da app dei prodotti non in giacenza.")

            Riga_Text("SqlDataProvider:",
                      "- Migliorie performance SqlDataProvider. Parsing automatico della clausola Order BY nelle query")

            Riga_Text("SqlDataProvider:",
                      "- Gestito Order BY con query complesse contenenti distinct / union / union all")

            Riga_Text("SqlDataProvider:",
                      "- Gestione delle Query con clausola ""For Json Path"" in apposito metodo dedicato nei data provider")

            Riga_Text("Albero 2017 (fast via jSon):",
                      "- Riportato flag per decidere il filtro per codice fiscale tecnico, attivato filtro su utenti_Visibilita_appoggio")

            '==================================

            Riga_Data("22/03/2021")

            Riga_Requisiti("AgronicaWebService2010: 10/03/2021",
                           "per PUA")

            Riga_Requisiti("Migra '635' + Aggancio '83': ",
                           "Indispensabile")

            Riga_Requisiti("Configurazione_Siti '80' :",
                           "aggiunta chiave DataProviderLogConfig")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Text("Prodotti.asmx - Ricerca  Anagrafica Prodotti:",
                      "Aggiunto campo Alias tra i campi restituiti dalla ricerca dell'Anagrafica Prodotti.")

            Riga_Text("Fabbricati.asmx:",
                      "Aggiunto web method per lettura anagrafiche magazzini da APP")


            '==================================

            Riga_Data("15/03/2021")

            Riga_Requisiti("AgronicaWebService2010: 10/03/2021",
                           "per PUA")

            Riga_Requisiti("Migra '635' + Aggancio '83': ",
                           "Indispensabile")

            Riga_Requisiti("Configurazione_Siti '80' :",
                           "aggiunta chiave DataProviderLogConfig")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Text("Ricerca prodotti:",
                      "Modifiche per gestione Alias")

            Riga_Text("SqlClient:",
                      "Impostato uso SqlClient di default al posto dell'OleDb")

            '==================================

            Riga_Data("10/03/2021")

            Riga_Requisiti("AgronicaWebService2010: 10/03/2021",
                           "per PUA")

            Riga_Requisiti("Migra '635' + Aggancio '83': ",
                           "Indispensabile")

            Riga_Requisiti("Configurazione_Siti '80' :",
                           "aggiunta chiave DataProviderLogConfig")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Text("Documentale:",
                      "- Antivirus per Allegati")

            Riga_Text("SqlDataProvider:",
                      "Sql Injection")

            Riga_Text("AgronicaCoreDPI/PianoConcimazione.asmx:",
                      "creata funzione PC_Finalita_Rer_BPercNMasResa_WS")

            Riga_Text("Versione .Net 4.8",
                      "Passaggio alla versione .Net 4.8")

            '==================================
            Riga_Data("05/03/2021")

            Riga_Requisiti("AgronicaWebService2010: 29/10/2020",
                           "letture per rilievi")

            Riga_Requisiti("Migra '635' + Aggancio '83': ",
                           "Indispensabile")

            Riga_Requisiti("Configurazione_Siti '80' :",
                           "aggiunta chiave DataProviderLogConfig")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Text("Documentale:",
                      "- Antivirus per Allegati")

            Riga_Text("SqlDataProvider:",
                      "Sql Injection")

            Riga_Text("AgronicaCoreDPI/PianoConcimazione.asmx:",
                      "aggiunto parametro Pua_Tipo alla funzione PC_PrecessioneColturale_WS")

            Riga_Text("Prodotti.asmx:",
                      "BugFix ricerca Anagrafica Prodotti se campo Produzione_Propria nella tabella Prodotti_Extra_Privata è 'NULL'.")

            Riga_Text("Contatti.asmx:",
                      "Fix per restituire solo i contatti validi alla data odierna nella chiamata per l'APP")

            Riga_Text("Attivita.vb:",
                      "- Aggiunto import 'AgronicaCoreDataProvider.DataProviderExtensions' per richiamare in Debug la funzione .ToOrigin()." &
                      "- Aggiunto la Piva tra i parametri restituiti dalla funzione LeggiAttivitaXGrigliaCDG.")

            Riga_Text("Alert_Area.asmx:",
                      "Aggiunto nella funzione di cancellazione di un' Area anche la cancellazione sulla tabella CategTipologiaDocumentiXUtenti.")

            Riga_Text("Alert_Tipologia.asmx:",
                      "Aggiunto nella funzione di cancellazione di una Tipologia anche la cancellazione sulle tabelle CategTipologiaDocumentiXUtenti e Alert_IndicexTipologia.")

            Riga_Text("Versione .Net 4.8",
                      "Passaggio alla versione .Net 4.8")

            '==================================

            Riga_Data("05 Febbraio 2021")

            Riga_Requisiti("AgronicaWebService2010: 29/10/2020",
                           "letture per rilievi")

            Riga_Requisiti("Migra '624' + Aggancio '83': ",
                           "Indispensabile")

            Riga_Requisiti("Configurazione_Siti '74' :",
                           "per SSO SAML2; per nuova chiave AbilitaHashPassword")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Text("Documentale:",
                      "- Modifiche Luke per Romagnoli")

            '==================================

            Riga_Data("04 Febbraio 2021")

            Riga_Requisiti("AgronicaWebService2010: 29/10/2020",
                           "letture per rilievi")

            Riga_Requisiti("Migra '624' + Aggancio '83': ",
                           "Indispensabile")

            Riga_Requisiti("Configurazione_Siti '74' :",
                           "per SSO SAML2; per nuova chiave AbilitaHashPassword")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Text("Ricerca Documenti:",
                      "- Bugfix ricerca ordini mostrava imponibile di qta ancora da evadere anziché quelli della riga stessa" & vbCrLf &
                      "- Bugfix parametri qualitativi di tipo 3 potrebbero avere anche dei decimali")

            '==================================

            Riga_Data("29 Gennaio 2021")

            Riga_Requisiti("AgronicaWebService2010: 29/10/2020",
                           "letture per rilievi")

            Riga_Requisiti("Migra '624' + Aggancio '83': ",
                           "Indispensabile")

            Riga_Requisiti("Configurazione_Siti '74' :",
                           "per SSO SAML2; per nuova chiave AbilitaHashPassword")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Text("FF_CampionamentoConferimento:",
                      "-Aggiunta Join con la vista 'Cultivar'nella funzione 'Leggi_Listini_Prezzi_Prodotti' per ottenere la Varietà." &
                      "-Aggiunte Join con le viste 'SpecieVegetali' e 'Cultivar' nella funzione 'Leggi_Listini_CampionamentoConferito_Prodotti_Equivalenti' per ottenere la Specie e la Varietà.")

            Riga_Text("FF_LiquidazioneSoci:",
                      "Aggiunto Join con la tabella 'Materie_Prime' e le viste 'Cultivar'nella funzione 'Leggi_Risultato_Liquidazione';" &
                      "per ottenere il Codice Articolo,Regolamento,Codice Esterno,Specie e Varietà.")

            Riga_Text("Prodotti.asmx:",
                      "Nascoste di default le colonne 'EAN/GTIN','Barcode Interno','Produzione Propria','Peso Netto','Peso Sgocciolato','Tara','Peso Egalizzato' e 'Immagine'. ")

            Riga_Text("Ricerca Documenti:",
                      "- Bugfix ordini con evasione forzata devono essere esclusi solo se si usa nella ricerca 'Solo Ordini non evasi'" & vbCrLf &
                      "- Bugfix segni imponibile/iva/importi")

            '==================================

            Riga_Data("27 Gennaio 2021")

            Riga_Requisiti("AgronicaWebService2010:  29/10/2020",
                           "letture per rilievi")

            Riga_Requisiti("Migra '624' + Aggancio '83': ",
                           "Indispensabile")

            Riga_Requisiti("Configurazione_Siti '74' :",
                           "per SSO SAML2; per nuova chiave AbilitaHashPassword")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Text("Prodotti.asmx.vb:",
                      "-Aggiunto campo 'Aliquota Iva' e 'Immagine' tra quelli restituiti dalla ricerca dell'Anagrafica Prodotti." &
                      "-Eliminata la funzione 'LeggiElencoCompletoProdottiPerAnagrafica' perchè non era più utilizzata.")

            Riga_Text("Ricerca Documenti Contabili:",
                      "Bugfix non comparivano gli ordini con lotto indefinito poiché hanno Sa_Cod = -1 e non Sa_Cod = 0")

            Riga_Text("Ricerca prodotti:",
                      "Aggiunta ricerca codice esterno")

            Riga_Text("Indirizzi.asmx.vb:",
                      "Alla lettura di ricerca indirizzi-contatti aggiunto parametro per ricercare i conferenti")

            Riga_Text("Contatti.asmx.vb:",
                      "Alla lettura di ricerca 'LeggiRapportoDocumenti' aggiunto 'Rapporto Contabile' fra i dati restituiti")

            '==================================

            Riga_Data("21 Gennaio 2021")

            Riga_Requisiti("AgronicaWebService2010: 29/10/2020",
                           "letture per rilievi")

            Riga_Requisiti("Migra '624' + Aggancio '83': ",
                           "Indispensabile")

            Riga_Requisiti("Configurazione_Siti '74' :",
                           "per SSO SAML2; per nuova chiave AbilitaHashPassword")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Text("Metaschema:",
                      "corretto CaricaImpiantiIrrigazioni")

            '==================================

            Riga_Data("18 Gennaio 2021")

            Riga_Requisiti("AgronicaWebService2010: 29/10/2020",
                           "letture per rilievi")

            Riga_Requisiti("Migra '624' + Aggancio '83': ",
                           "Indispensabile")

            Riga_Requisiti("Configurazione_Siti '74' :",
                           "per SSO SAML2; per nuova chiave AbilitaHashPassword")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Text("calcolo lotti conferimento:",
                      "modifiche per proporre il lotto anche per i prodotti non legati a linea")

            '==================================

            Riga_Data("17 Gennaio 2021")

            Riga_Requisiti("AgronicaWebService2010: 29/10/2020",
                           "letture per rilievi")

            Riga_Requisiti("Migra '624' + Aggancio '83': ",
                           "Indispensabile")

            Riga_Requisiti("Configurazione_Siti '74' :",
                           "per SSO SAML2; per nuova chiave AbilitaHashPassword")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Text("x rilascio agenda x fruttagel...:",
                      "dettagliare modifiche please")

            '==================================

            Riga_Data("14 Gennaio 2021")

            Riga_Requisiti("AgronicaWebService2010: 29/10/2020",
                           "letture per rilievi")

            Riga_Requisiti("Migra '624' + Aggancio '83': ",
                           "Indispensabile")

            Riga_Requisiti("Configurazione_Siti '74' :",
                           "per SSO SAML2; per nuova chiave AbilitaHashPassword")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Text("Indirizzi:",
                      "Aggiunta lettura per ricercare contatti ed indirizzi")

            '==================================

            Riga_Data("9 Gennaio 2021")

            Riga_Requisiti("AgronicaWebService2010: 29/10/2020",
                           "letture per rilievi")

            Riga_Requisiti("Migra '624' + Aggancio '83': ",
                           "Indispensabile")

            Riga_Requisiti("Configurazione_Siti '74' :",
                           "per SSO SAML2; per nuova chiave AbilitaHashPassword")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Text("Listini_Prezzi.asmx:",
                      "Aggiunta proprietà Tipo_Classe fra i dati restituiti")

            Riga_Text("Prodotti.asmx:",
                      "Aggiunte colonne per campi 'Dati Vendita Al Dettaglio' tra quelli restituiti nella griglia di ricerca dell'Anagrafica Prodotti")

            Riga_Text("Prodotti.asmx:",
                      "Tolta gestione di CONFEZIONI_PRODOTTI e ALTRI_BENI_AMMORTIZZABILI che non sono utilizzate e davano errori nella ricerca giacenze")

            Riga_Text("Contabilita.asmx:",
                      "LeggiRapportiContabili: Aggiunti argomenti alla funzione per filtrare le varie tipologie di rapporto")

            '==================================

            Riga_Data("5 Gennaio 2021")

            Riga_Requisiti("AgronicaWebService2010: 29/10/2020",
                           "letture per rilievi")

            Riga_Requisiti("Migra '624' + Aggancio '83': ",
                           "Indispensabile")

            Riga_Requisiti("Configurazione_Siti '74' :",
                           "per SSO SAML2; per nuova chiave AbilitaHashPassword")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Text("Ricerca prodotti e Controllo se prodotto movimentato:",
                      "modifiche e bug fix")

            '==================================

            Riga_Data("31 Dicembre 2020")

            Riga_Requisiti("AgronicaWebService2010: 29/10/2020",
                           "letture per rilievi")

            Riga_Requisiti("Migra '624' + Aggancio '83': ",
                           "Indispensabile")

            Riga_Requisiti("Configurazione_Siti '74' :",
                           "per SSO SAML2; per nuova chiave AbilitaHashPassword")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Text("Campionamento e liquidazioni conferimento:",
                      "Modifiche varie per gestire prodotti non legati a linea")

            '==================================

            Riga_Data("30 Dicembre 2020")

            Riga_Requisiti("AgronicaWebService2010: 29/10/2020",
                           "letture per rilievi")

            Riga_Requisiti("Migra '624' + Aggancio '83': ",
                           "Indispensabile")

            Riga_Requisiti("Configurazione_Siti '74' :",
                           "per SSO SAML2; per nuova chiave AbilitaHashPassword")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Text("Ricerca prodotti:",
                      "Correzione sui filtri")


            '==================================

            Riga_Data("28 Dicembre 2020")

            Riga_Requisiti("AgronicaWebService2010: 29/10/2020",
                           "letture per rilievi")

            Riga_Requisiti("Migra '624' + Aggancio '83': ",
                           "Indispensabile")

            Riga_Requisiti("Configurazione_Siti '74' :",
                           "per SSO SAML2; per nuova chiave AbilitaHashPassword")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Text("Ricerca prodotti:",
                      "Modifiche per filtri aggiuntivi su materie prime e per necessità ricerca anagrafica prodotti")

            '==================================

            Riga_Data("15 Dicembre 2020")

            Riga_Requisiti("AgronicaWebService2010: 29/10/2020",
                           "letture per rilievi")

            Riga_Requisiti("Migra '624' + Aggancio '83': ",
                           "Indispensabile")

            Riga_Requisiti("Configurazione_Siti '74' :",
                           "per SSO SAML2; per nuova chiave AbilitaHashPassword")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Text("Importazione Ricette APP:",
                      "Aggiunto controllo per impedire l'importazione di irrigazioni su impianti con terreno nudo")

            '==================================

            Riga_Data("13 Dicembre 2020")

            Riga_Requisiti("AgronicaWebService2010: 29/10/2020",
                           "letture per rilievi")

            Riga_Requisiti("Migra '624' + Aggancio '83': ",
                           "Indispensabile")

            Riga_Requisiti("Configurazione_Siti '74' :",
                           "per SSO SAML2; per nuova chiave AbilitaHashPassword")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Text("Chiusura per dipendenza agenda: ",
                      "")

            '==================================

            Riga_Data("09 Dicembre 2020")

            Riga_Requisiti("AgronicaWebService2010: 29/10/2020",
                           "letture per rilievi")

            Riga_Requisiti("Migra '624' + Aggancio '83': ",
                           "Indispensabile")

            Riga_Requisiti("Configurazione_Siti '74' :",
                           "per SSO SAML2; per nuova chiave AbilitaHashPassword")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Text("Ricerca documenti: ",
                      "Correzione al calcolo degrado nella griglia con dettaglio prodotti")

            Riga_Text("IrrigazioneBS - Nuova Pagina Irrigazione (Operazioni.asmx.vb):",
                      "Nascosta la possibilità di creare una nuova Ricetta o Brogliaccio di Irrigazione se non è attiva la chiave 'IrrigazioneBS' nella tabella 'Configurazione_Siti'.")

            Riga_Text("Operazioni.asmx:",
                      "Metodo lettura GruppoOperazioni.")


            '==================================

            Riga_Data("26 novembre 2020")

            Riga_Requisiti("AgronicaWebService2010: 29/10/2020",
                           "letture per rilievi")

            Riga_Requisiti("Migra '624' + Aggancio '83': ",
                           "Indispensabile")

            Riga_Requisiti("Configurazione_Siti '74' :",
                           "per SSO SAML2; per nuova chiave AbilitaHashPassword")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Text("Utenti_R.asmx: ",
                      "Inserita nuova gestione login e hash password")

            '==================================

            Riga_Data("16 novembre 2020 bis")

            Riga_Requisiti("AgronicaWebService2010: 29/10/2020",
                           "letture per rilievi")

            Riga_Requisiti("Migra '622' + Aggancio '83': ",
                           "Indispensabile")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Text("Ricerca prodotti:",
                      "Correzione errore per campi Sem_Cod,GRVA_COD_VEG,Cat_Cod")

            '==================================

            Riga_Data("16 novembre 2020")

            Riga_Requisiti("AgronicaWebService2010: 29/10/2020",
                           "letture per rilievi")

            Riga_Requisiti("Migra '622' + Aggancio '83': ",
                           "Indispensabile")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Text("Ricerca documenti contabili :",
                      "Fix campo non univoco nella query per filtro Causale_Trasporto_Cod")

            Riga_Text("Prodotto_Edit_UC - Anagrafica Prodotti:",
                      "- Restituite le colonne 'Categoria Magazzino','Tipologia Semente' e 'Tipologia Varietale' nella funzione 'LeggiElencoCompletoProdottiPerAnagrafica'.")

            '==================================

            Riga_Data("12 Novembre 2020")

            Riga_Requisiti("AgronicaWebService2010: 29/10/2020",
                           "letture per rilievi")

            Riga_Requisiti("Migra '622' + Aggancio '83': ",
                           "Indispensabile")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Text("LeggiParametriQualitativi F&F :",
                      "modificate query parametri qualitativi per restituire valore minimo e massimo")

            Riga_Text("Ricerca Prodotti:",
                      "aggiunta ricerca di alcune categorie di magazzino che non erano gestite")

            Riga_Text("Ricerca Prodotti:",
                      "aggiunta lettura tipologia semente, tipologia varietale e categoria")

            '==================================

            Riga_Data("6 Novembre 2020")

            Riga_Requisiti("AgronicaWebService2010: 29/10/2020",
                           "letture per rilievi")

            Riga_Requisiti("Migra '618' + Aggancio '83' :",
                           "Per nuove colonne su Movimenti_Dettagli")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Text("Modifiche F&F :",
                           "Varie legate a cambiamenti agenda")

            '==================================

            Riga_Data("3 Novembre 2020 bis")

            Riga_Requisiti("AgronicaWebService2010: 29/10/2020",
                           "letture per rilievi")

            Riga_Requisiti("Migra '618' + Aggancio '83' :",
                           "Per nuove colonne su Movimenti_Dettagli")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Text("Ricerca prodotti :",
                           "Corretto errore lettura regolamento su guacenze di magazzino")

            '==================================

            Riga_Data("3 Novembre 2020")

            Riga_Requisiti("AgronicaWebService2010: 29/10/2020",
                           "letture per rilievi")

            Riga_Requisiti("Migra '618' + Aggancio '83' :",
                           "Per nuove colonne su Movimenti_Dettagli")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Text("Parametri qualitativi :",
                           "Gestione parametri qualitativi data e stringa")

            '==================================

            Riga_Data("2 Novembre 2020")

            Riga_Requisiti("AgronicaWebService2010: 29/10/2020",
                           "letture per rilievi")

            Riga_Requisiti("Migra '618' + Aggancio '83' :",
                           "Per nuove colonne su Movimenti_Dettagli")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Text("Rilievi: ",
                      "modificata funzione LetturaMisureXAvversita per restituire il parametro soglia")

            '==================================

            Riga_Data("29 Ottobre 2020")

            Riga_Requisiti("AgronicaWebService2010: 21/06/2019",
                           "letture per PUA")

            Riga_Requisiti("Migra '614' + Aggancio '83' :",
                           "Per campi aggiunti su liquidazioni")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Text("  Documentale:",
                           " - Correzione errore filtro stato validazione su lista documenti ")

            '==================================

            Riga_Data("26 Ottobre 2020")

            Riga_Requisiti("AgronicaWebService2010: 21/06/2019",
                           "letture per PUA")

            Riga_Requisiti("Migra '614' + Aggancio '83' :",
                           "Per campi aggiunti su liquidazioni")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Text("  AgronicaCoreDPI:",
                           " - Lettura disciplinare-ente ")

            '==================================

            Riga_Data("21 Ottobre 2020")

            Riga_Requisiti("AgronicaWebService2010: 21/06/2019",
                           "letture per PUA")

            Riga_Requisiti("Migra '614' + Aggancio '83' :",
                           "Per campi aggiunti su liquidazioni")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Text("Passaggio a SqlClient:",
                           "Ricompilazione per passaggio")

            '==================================

            Riga_Data("7 Ottobre 2020 ")

            Riga_Requisiti("AgronicaWebService2010: 21/06/2019",
                           "letture per PUA")

            Riga_Requisiti("Migra '614' + Aggancio '83' :",
                           "Per campi aggiunti su liquidazioni")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Text("Ricerca prodotti :",
                           "Aggiunta funzione di ricerca prodotti su più categorie con filtro per specie e varietà")

            Riga_Text("Documentale:",
                           "Correzione per segnalazioni Valerio")

            '==================================

            Riga_Data("11 Settembre 2020 ")

            Riga_Requisiti("AgronicaWebService2010: 21/06/2019",
                           "letture per PUA")

            Riga_Requisiti("Migra '612' + Aggancio '83' :",
                           "INDISPENSABILE")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Text("Ricette APP",
                      " - Aggiunti campi per interventi Demetra + modifica modello EF" &
                      " - Aggiunto web method per importare ricette di una singola azienda")

            Riga_Text("getListaImpianti_APP",
                      " - Modifica per comporre APP_NOME = App.01 kpin: 1953 blockname: A start: 15/08/2020" &
                      " se presenti i campi kpin, blockname e imprese_progetti.data_inizio_prevista")

            '==================================

            Riga_Data("25 Agosto 2020")

            Riga_Requisiti("AgronicaWebService2010: 21/06/2019",
                           "letture per PUA")

            Riga_Requisiti("Migra '610' + Aggancio '83' :",
                           "INDISPENSABILE")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Text("Documentale + APP",
                      "Fix per trasformazione di String in Date quando il server è in Inglese con formato data Americano")

            '==================================

            Riga_Data("24 Agosto 2020")

            Riga_Requisiti("AgronicaWebService2010: 21/06/2019",
                           "letture per PUA")

            Riga_Requisiti("Migra '610' + Aggancio '83' :",
                           "INDISPENSABILE")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Text("Gias To Canopy ",
                      "Prima Versione per test")

            '==================================

            Riga_Data("19 Agosto 2020")

            Riga_Requisiti("AgronicaWebService2010: 21/06/2019",
                           "letture per PUA")

            Riga_Requisiti("Migra '605' + Aggancio '83' :",
                           "INDISPENSABILE")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Text("OPERAZIONE CRYSTAL ",
                      "No modifiche ai CoreWS ma rilasciato per modifiche fatte sui core.")

            '==================================

            Riga_Data("05 Agosto 2020")

            Riga_Requisiti("AgronicaWebService2010: 21/06/2019",
                           "letture per PUA")

            Riga_Requisiti("Migra '605' + Aggancio '83' :",
                           "INDISPENSABILE")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Text("FF_Etichette: ",
                      "bug fix su lettura parametri qualitativi")

            '==================================

            Riga_Data("03 Agosto 2020")

            Riga_Requisiti("AgronicaWebService2010: 21/06/2019",
                           "letture per PUA")

            Riga_Requisiti("Migra '605' + Aggancio '83' :",
                           "INDISPENSABILE")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Text("Albero Anagrafica: ",
                      "bug fix su albero anagrafica, particelle catastali lette erroneamente da altro centro se con stesso cod appezza")


            '==================================

            Riga_Data("27 Luglio 2020")

            Riga_Requisiti("AgronicaWebService2010: 21/06/2019",
                           "letture per PUA")

            Riga_Requisiti("Migra '605' + Aggancio '83' :",
                           "INDISPENSABILE")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO")

            Riga_Text("Nuova gestione prodotti: ",
                      "Modifiche varie ai WS")

            '==================================

            Riga_Data("21 Luglio 2020 ")

            Riga_Requisiti("AgronicaWebService2010: 21/06/2019",
                           "letture per PUA")

            Riga_Requisiti("Migra '605' + Aggancio '83' :",
                           "INDISPENSABILE")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche SI: introdotto file appsettings.config con link su web.config ")

            Riga_Text("Ricerca prodotti e ricerca giacenze: ",
                      "Aggiunta di colonne per ricerca nuova anagrafica prodotti e per conferimenti")

            '==================================

            Riga_Data("16 Luglio 2020 ")

            Riga_Requisiti("AgronicaWebService2010: 21/06/2019",
                           "letture per PUA")

            Riga_Requisiti("Migra '605' + Aggancio '83' :",
                           "INDISPENSABILE")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche SI: introdotto file appsettings.config con link su web.config ")

            Riga_Text("Albero Anagrafica: ",
                      "Gestione del caricamento di riparto catasto su appezzamenti")

            '==================================

            Riga_Data("13 Luglio 2020 ")

            Riga_Requisiti("AgronicaWebService2010: 21/06/2019",
                           "letture per PUA")

            Riga_Requisiti("Migra '605' + Aggancio '83' :",
                           "INDISPENSABILE")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche SI: introdotto file appsettings.config con link su web.config ")

            Riga_Text("Conferimenti e liquidazioni F&F: ",
                      "Modifiche varie")

            '==================================

            Riga_Data("2 Luglio 2020 ")

            Riga_Requisiti("AgronicaWebService2010: 21/06/2019",
                           "letture per PUA")

            Riga_Requisiti("Migra '604' + Aggancio '83' :",
                           "INDISPENSABILE")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO. ")

            '==================================

            Riga_Data("25 Giugno 2020 ")

            Riga_Requisiti("AgronicaWebService2010 : 21/06/2019",
                           "letture per PUA")

            Riga_Requisiti("Migra '604' + Aggancio '83' :",
                           "INDISPENSABILE")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO. ")

            Riga_Text("Contatti: ",
                      "Lettura dati per Anagrafica combo 'Riferimento trasferimento dati'")

            '==================================

            Riga_Data("22 Giugno 2020")

            Riga_Requisiti("AgronicaWebService2010 : 21/06/2019",
                           "letture per PUA")

            Riga_Requisiti("Migra '600' + Aggancio '83' :",
                           "NECESSARIO per nuovi campi documentale, APP + lingue")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO. ")

            Riga_Text("Conferimenti: ",
                      "Modifiche per ricerca documenti di conferimento")

            '==================================
            Riga_Data("19 Giugno 2020")

            Riga_Requisiti("AgronicaWebService2010 : 21/06/2019",
                           "letture per PUA")

            Riga_Requisiti("Migra '600' + Aggancio '83' :",
                           "NECESSARIO per nuovi campi documentale, APP + lingue")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO. ")

            Riga_Text("Audit.asmx: ",
                      "Modifiche per integrazione checklist global gap nel modulo Audit")

            Riga_Text("web.config: ",
                      "Impostato maxRequestLength = 51200 per errore upload Coprob")

            '==================================


            Riga_Data("16 Giugno 2020")

            Riga_Requisiti("AgronicaWebService2010 : 21/06/2019",
                           "letture per PUA")

            Riga_Requisiti("Migra '600' + Aggancio '83' :",
                           "NECESSARIO per nuovi campi documentale, APP + lingue")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO. ")

            Riga_Text(" ISTAT.asmx: ",
                      " - Lettura Comuni nella Tabella ISTAT")

            '==================================

            Riga_Data("12 Giugno 2020")

            Riga_Requisiti("AgronicaWebService2010 : 21/06/2019",
                           "letture per PUA")

            Riga_Requisiti("Migra '600' + Aggancio '83' :",
                           "NECESSARIO per nuovi campi documentale, APP + lingue")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO. ")

            Riga_Text("Ricerca documenti conferimento: ",
                      "Scartati documenti con Split = 1 (generati dal LAN)")

            '==================================
            Riga_Data("10 Giugno 2020")

            Riga_Requisiti("AgronicaWebService2010 : 21/06/2019",
                           "letture per PUA")

            Riga_Requisiti("Migra '600' + Aggancio '83' :",
                           "NECESSARIO per nuovi campi documentale, APP + lingue")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO. ")

            Riga_Text("Imprese.asmx: ",
                      "Inserito codice socio in descrizione impresa")

            Riga_Text("Visite.asmx: ",
                      "Aggiunto WS per scarico e importazione visite da APP")

            '==================================

            Riga_Data("04 Giugno 2020")

            Riga_Requisiti("AgronicaWebService2010 : 21/06/2019",
                           "letture per PUA")

            Riga_Requisiti("Migra '588' + Aggancio '83' :",
                           "NECESSARIO per nuovi campi documentale, APP + lingue")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO. ")

            Riga_Text("Albero Anagrafica 2017: ",
                      "Albero Anagrafica - Ricette impostata dicitura brogliaccio/Ordini/Distribuzione/PUA")

            '==================================

            Riga_Data("3 Giugno 2020")

            Riga_Requisiti("AgronicaWebService2010 : 21/06/2019",
                           "letture per PUA")

            Riga_Requisiti("Migra '588' + Aggancio '83' :",
                           "NECESSARIO per nuovi campi documentale, APP + lingue")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO. ")

            Riga_Text("Prodotti.asmx: ",
                      "Correzione su ricerca prodotti in caso di scarico (chiamata Aboca 5538)")

            '==================================

            Riga_Data("29 Maggio 2020")

            Riga_Requisiti("AgronicaWebService2010 : 21/06/2019",
                           "letture per PUA")

            Riga_Requisiti("Migra '588' + Aggancio '83' :",
                           "NECESSARIO per nuovi campi documentale, APP + lingue")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO. ")


            Riga_Text("Contatti.asmx: ",
                      "Fix join tra contatti e risorsa umana, per cui sui nuovi documenti si vedevano anche contatti privati di altre pive")


            Riga_Text("CentriAziendali.asmx: ",
                      "Nuovo web service getCentriAziendali_APP necessario perchè i centri aziendali usati nelle visite potrebbero non avere impianti collegati")


            '==================================
            Riga_Data("22 Maggio 2020")

            Riga_Requisiti("AgronicaWebService2010 : 21/06/2019",
                           "letture per PUA")

            Riga_Requisiti("Migra '588' + Aggancio '83' :",
                           "NECESSARIO per nuovi campi documentale, APP + lingue")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO. ")


            Riga_Text("Prodotti.asmx: ",
                      "ottimizzazione ricerca giacenze: introdotto parametro per indicare se eseguire la lettura giacenze con AGRODATAFINE in aggiunta a quella con data di riferimento giacenze")

            Riga_Text("EF / POCO: ",
                      "aggiunta colonne degrado alle tabelle di liquidazione")

            '==================================
            Riga_Data("19 Maggio 2020")

            Riga_Requisiti("AgronicaWebService2010 : 21/06/2019",
                           "letture per PUA")

            Riga_Requisiti("Migra '588' + Aggancio '83' :",
                           "NECESSARIO per nuovi campi documentale, APP + lingue")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO. ")

            Riga_Text("Dpi.asmx: ",
                      "modificato ordinamento nelle funzioni CaricaComboDPI_ConTipoRegolamento e CaricaComboDisciplinare")

            Riga_Text("Metaschema/SpecieVegetali.asmx: ",
                      "Ordinamento alfabetico specie vegetali con filtro utente ")

            Riga_Text("AgronicaCoreDPI/PianoConcimazione.asmx:",
                      "creata funzione CalcoloNPK_GrfiCod_StatoCod")

            '==================================

            Riga_Data("13 maggio 2020")

            Riga_Requisiti("AgronicaWebService2010 : 21/06/2019",
                           "letture per PUA")

            Riga_Requisiti("Migra '588' + Aggancio '83' :",
                           "NECESSARIO per nuovi campi documentale, APP + lingue")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO. ")

            Riga_Text("Rilievi: ",
                      "gestito parametro lingua_cod da passare al web service per la lettura delle MisureXAvversita")

            '==================================

            Riga_Data("11 maggio 2020")

            Riga_Requisiti("AgronicaWebService2010 : 21/06/2019",
                           "letture per PUA")

            Riga_Requisiti("Migra '588' + Aggancio '83' :",
                           "NECESSARIO per nuovi campi documentale, APP + lingue")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO. ")

            Riga_Text("Prodotti.asmx:",
                      "Modifiche per nuovi documenti")

            '==================================

            Riga_Data("6 maggio 2020")

            Riga_Requisiti("AgronicaWebService2010 : 21/06/2019",
                           "letture per PUA")

            Riga_Requisiti("Migra '588' + Aggancio '83' :",
                           "NECESSARIO per nuovi campi documentale, APP + lingue")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO. ")

            Riga_Text("FreshAndFood.asmx:",
                      "Aggiunto parametro Mat_Cod al metodo LeggiProdotti")


            '==================================

            Riga_Data("4 maggio 2020")

            Riga_Requisiti("AgronicaWebService2010 : 21/06/2019",
                           "letture per PUA")

            Riga_Requisiti("Migra '588' + Aggancio '83' :",
                           "NECESSARIO per nuovi campi documentale, APP + lingue")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO. ")

            Riga_Text("Gis/EntrateUscite.asmx:",
                      "ricevimento su APP dei parametri di configurazione menu e schedulazione invio coordinate")


            '==================================

            Riga_Data("29 aprile 2020")

            Riga_Requisiti("AgronicaWebService2010 : 21/06/2019",
                           "letture per PUA")

            Riga_Requisiti("Migra '588' + Aggancio '83' :",
                           "NECESSARIO per nuovi campi documentale, APP + lingue")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO. ")

            Riga_Text("Gis/EntrateUscite.asmx:",
                      "ricevimento dati da APP per orari entrata / uscita e per coordinate")


            '==================================

            Riga_Data("24 Aprile 2020")

            Riga_Requisiti("AgronicaWebService2010 : 21/06/2019",
                           "letture per PUA")

            Riga_Requisiti("Migra '585' + Aggancio '83' :",
                           "NECESSARIO per nuovi campi documentale e liquidazioni + lingue")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO. ")

            Riga_Text("Metaschema/GruppoFinalita.asmx:",
                      "aggiunto parametro objP_super_server alla funzione CaricaComboFinalita2")

            Riga_Text("Metaschema/PUA_Regolamenti.asmx:",
                      "aggiunto parametro objP_super_server alla funzione Leggi_PUA_RegolamentixEditImpianto")

            Riga_Text("AgronicaCoreDPI/PianoConcimazione.asmx:",
                      "aggiunto parametro objP_super_server alle funzioni CalcoloNPK e CalcoloNPK_grfi_cod")

            '==================================

            Riga_Data("14 Aprile 2020")

            Riga_Requisiti("AgronicaWebService2010 : 21/06/2019",
                           "letture per PUA")

            Riga_Requisiti("Migra '585' + Aggancio '83' :",
                           "NECESSARIO per nuovi campi documentale e liquidazioni + lingue")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO. ")

            Riga_Text("Documentale: ",
                      "modifica ai filtri della griglia documentale / scadenzario")

            '==================================

            Riga_Data("8 Aprile 2020")

            Riga_Requisiti("AgronicaWebService2010 : 21/06/2019",
                           "letture per PUA")

            Riga_Requisiti("Migra '585' + Aggancio '83' :",
                           "NECESSARIO per nuovi campi documentale e liquidazioni + lingue")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO. ")

            Riga_Text("Rilievi: ",
                      "gestito parametro lingua_cod da passare al web service per la lettura delle fasi fenologiche")

            '==================================

            Riga_Data("6 Aprile 2020 bis")

            Riga_Requisiti("AgronicaWebService2010 : 21/06/2019",
                           "letture per PUA")

            Riga_Requisiti("Migra '585' + Aggancio '83' :",
                           "NECESSARIO per nuovi campi documentale e liquidazioni + lingue")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO. ")

            Riga_Text("Documentale: ",
                      "Scrittura documenti non collegati a tipologie standard")

            '==================================

            Riga_Data("6 Aprile 2020")

            Riga_Requisiti("AgronicaWebService2010 : 21/06/2019",
                           "letture per PUA")

            Riga_Requisiti("Migra '585' + Aggancio '83' :",
                           "NECESSARIO per nuovi campi documentale e liquidazioni + lingue")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO. ")

            Riga_Text("LeggiIndContattiImpreseCentri: ",
                      "Cambiata descrizione tipo indirizzo per imprese e centri")

            '==================================

            Riga_Data("31 Marzo 2020")

            Riga_Requisiti("AgronicaWebService2010 : 21/06/2019",
                           "letture per PUA")

            Riga_Requisiti("Migra '584' + Aggancio '83' :",
                           "NECESSARIO per nuovi campi documentale e liquidazioni")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO. ")

            Riga_Text("Causali_Fattura.asmx: ",
                      "Creazione Core lettura + Web Method")

            Riga_Text("Prodotti e Giacenza: ",
                      "modifiche ricerca prodotti e giacenze per nuova gestione DDT")

            '==================================

            Riga_Data("27 Marzo 2020")

            Riga_Requisiti("AgronicaWebService2010 : 21/06/2019",
                           "letture per PUA")

            Riga_Requisiti("Migra '584' + Aggancio '83' :",
                           "NECESSARIO per nuovi campi documentale e liquidazioni")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO. ")

            Riga_Text("Contatti.asmx: ",
                      "Cambiamenti a LeggiRapportoSpecificoxDocumenti")

            '==================================

            Riga_Data("19 Marzo 2020")

            Riga_Requisiti("AgronicaWebService2010 : 21/06/2019",
                           "letture per PUA")

            Riga_Requisiti("Migra '584' + Aggancio '83' :",
                           "NECESSARIO per nuovi campi documentale e liquidazioni")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO. ")

            Riga_Text("Documentale / scadenze e liquidazioni: ",
                      "Modifiche varie")

            '==================================

            Riga_Data("12 Marzo 2020")

            Riga_Requisiti("AgronicaWebService2010 : 21/06/2019",
                           "letture per PUA")

            Riga_Requisiti("Migra '584' + Aggancio '83' :",
                           "NECESSARIO per nuovi campi documentale e liquidazioni")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO. ")

            Riga_Text("Documentale / scadenze e liquidazioni: ",
                      "Gestiti nuovi campi")

            '==================================

            Riga_Data("11 Marzo 2020")

            Riga_Requisiti("AgronicaWebService2010 : 21/06/2019",
                           "letture per PUA")

            Riga_Requisiti("Migra '581' + Aggancio '83' :",
                           "NECESSARIO per gestione lingue")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO. ")

            Riga_Text("DataProvider: ",
                      "Resa Compatibile lettura dati tab ""XLingue"" con codici lingua separati con trattino "" - "" (es.: IT-ch)")

            Riga_Text("GruppoFinalita.asmx: ",
                      "modificata funzione CaricaComboFinalita2 (introdotta lettura via web service della finalita del piano concimazione)")

            Riga_Text("PUA_Regolamenti.asmx: ",
                      "modificata funzione Leggi_PUA_RegolamentixEditImpianto (introdotta lettura via web service)")


            '==================================

            Riga_Data("27 Febbraio 2020")

            Riga_Requisiti("AgronicaWebService2010 : 21/06/2019",
                           "letture per PUA")

            Riga_Requisiti("Migra '581' + Aggancio '83' :",
                           "NECESSARIO per gestione lingue")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO. ")
            Riga_Text("Macchine.asmx: ",
                      "Aggiunto passaggio di xOrderBy e Piva a Leggi_Macchine_Per_Contatto")

            Riga_Text("RicercaDocumenti.asmx: ",
                      "Aggiunta di Sa_Cod_Agenda in select ")

            Riga_Text("RisorseUmane: ",
                      "aggiunto filtro su visibilità centri in LeggiRapportoSpecifico e LeggiRapportoSpecificoxDocumenti ")

            Riga_Text("Lingue: ",
                      "Modifiche varie")

            '==================================

            Riga_Data("21 Febbraio 2020")

            Riga_Requisiti("AgronicaWebService2010 : 21/06/2019",
                           "letture per PUA")

            Riga_Requisiti("Migra '577' :",
                           "NECESSARIO per nuova colonna Doc_Numero_Visualizzato in Movimenti")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO. ")

            Riga_Text("Listini_Prezzi.asmx",
                      "Modifica firma metodo RicercaListini Listini_Prezzi.asmx -> Leggi")

            '==================================

            Riga_Data("18 Febbraio 2020")

            Riga_Requisiti("AgronicaWebService2010 : 21/06/2019",
                           "letture per PUA")

            Riga_Requisiti("Migra '577' :",
                           "NECESSARIO per nuova colonna Doc_Numero_Visualizzato in Movimenti")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO. ")
            Riga_Text("PUA",
                      "aggiunta funzione Pua_Regolamenti a PianoConcimazione.asmx")

            Riga_Text("Conferimento",
                      "WIP vari per rilascio nuovo Doc Contabile")

            '==================================

            Riga_Data("12 Febbraio 2020")

            Riga_Requisiti("AgronicaWebService2010 : 21/06/2019",
                           "letture per PUA")

            Riga_Requisiti("Migra '577' :",
                           "NECESSARIO per nuova colonna Doc_Numero_Visualizzato in Movimenti")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO. ")

            Riga_Text("PUA",
                      "modificata funzione VaiAlPianoDistribuzione (passati nuovi parametri)")


            '==================================

            Riga_Data("10 Febbraio 2020")

            Riga_Requisiti("AgronicaWebService2010 : 21/06/2019",
                           "letture per PUA")

            Riga_Requisiti("Migra '577' :",
                           "NECESSARIO per nuova colonna Doc_Numero_Visualizzato in Movimenti")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO. ")

            Riga_Text("GiasAPP: ",
                      "Modifica ricercca attivitaXoperazioni ")

            '==================================

            Riga_Data("07 Febbraio 2020")

            Riga_Requisiti("AgronicaWebService2010 : 21/06/2019",
                           "letture per PUA")

            Riga_Requisiti("Migra '577' :",
                           "NECESSARIO per nuova colonna Doc_Numero_Visualizzato in Movimenti")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO. ")

            Riga_Text("RicercaDocumenti.asmx",
                      "Modifica funzione ricerca per filtro Centri Aziendali")

            '==================================

            Riga_Data("5 Febbraio 2020")

            Riga_Requisiti("AgronicaWebService2010 : 21/06/2019",
                           "letture per PUA")

            Riga_Requisiti("Migra '557' :",
                           "NECESSARIO per nuova gestione patentini")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO. ")

            Riga_Text("PUA",
                      "modificata funzione VaiAlPianoDistribuzione (passati nuovi parametri)")

            '==================================

            Riga_Data("29 Gennaio 2020")

            Riga_Requisiti("AgronicaWebService2010 : 21/06/2019",
                           "letture per PUA")

            Riga_Requisiti("Migra '557' :",
                           "NECESSARIO per nuova gestione patentini")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO. ")
            Riga_Text("Doc",
                      "Nuova gestione Documenti")


            '==================================

            Riga_Data("24 Gennaio 2020")

            Riga_Requisiti("AgronicaWebService2010 : 21/06/2019",
                           "letture per PUA")

            Riga_Requisiti("Migra '557' :",
                           "NECESSARIO per nuova gestione patentini")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO. ")

            Riga_Text("Ricerca attività per operazioni:",
                      " Corretta query e aggiunto utilizzo PIVA")


            '==================================

            Riga_Data("22 Gennaio 2020")

            Riga_Requisiti("AgronicaWebService2010 : 21/06/2019",
                           "letture per PUA")

            Riga_Requisiti("Migra '557' :",
                           "NECESSARIO per nuova gestione patentini")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO. ")

            Riga_Text("Ricerca parametri qualitativi",
                      " Rricerca parametri qualitativi specifici per PIVA")


            '==================================

            Riga_Data("02 Gennaio 2020")

            Riga_Requisiti("AgronicaWebService2010 : 21/06/2019",
                           "letture per PUA")

            Riga_Requisiti("Migra '557' :",
                           "NECESSARIO per nuova gestione patentini")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO. ")

            Riga_Text("Reg_Impianti.asmx",
                      " Controllo su appezzamenti movimentati")

            Riga_Text("ISTAT.asmx",
                      " Get Stati ")

            '==================================

            Riga_Data("19 Dicembre 2019")

            Riga_Requisiti("AgronicaWebService2010 : 21/06/2019",
                           "letture per PUA")

            Riga_Requisiti("Migra '557' :",
                           "NECESSARIO per nuova gestione patentini")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO. ")

            Riga_Text("Ricerca prodotti in giacenza",
                      "Correzione lettura giacenze ")

            '==================================

            Riga_Data("18 Dicembre 2019")

            Riga_Requisiti("AgronicaWebService2010 : 21/06/2019",
                           "letture per PUA")

            Riga_Requisiti("Migra '557' :",
                           "NECESSARIO per nuova gestione patentini")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO. ")

            Riga_Text("Ricerca parametri qualitativi",
                      "Aggiunta filtro Piva ")

            '==================================

            Riga_Data("11 Dicembre 2019")

            Riga_Requisiti("AgronicaWebService2010 : 21/06/2019",
                           "letture per PUA")

            Riga_Requisiti("Migra '557' :",
                           "NECESSARIO per nuova gestione patentini")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO. ")

            Riga_Text("WS Giacenze",
                      "correzione query Sql ")

            '==================================

            Riga_Data("10 Dicembre 2019")

            Riga_Requisiti("AgronicaWebService2010 : 21/06/2019",
                           "letture per PUA")

            Riga_Requisiti("Migra '557' :",
                           "NECESSARIO per nuova gestione patentini")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO. ")

            Riga_Text("WS Giacenze e Prodotti",
                      "aggiunto flag che indica se il prodotto è legato a linea ")


            '==================================

            Riga_Data("9 Dicembre 2019")

            Riga_Requisiti("AgronicaWebService2010 : 21/06/2019",
                           "letture per PUA")

            Riga_Requisiti("Migra '557' :",
                           "NECESSARIO per nuova gestione patentini")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO. ")

            Riga_Text("WS Giacenze",
                      "aggiunto parametro per filtrare solo prodotti con giacenza ")


            '==================================

            Riga_Data("6 Dicembre 2019")

            Riga_Requisiti("AgronicaWebService2010 :  21/06/2019",
                           "letture per PUA")

            Riga_Requisiti("Migra '557' :",
                           "NECESSARIO per nuova gestione patentini")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO. ")

            Riga_Text("WS Prodotti",
                      "modifiche alla lettura per agganciare anche specie e varietà di materie prime")

            '==================================

            Riga_Data("22 Novembre 2019")

            Riga_Requisiti("AgronicaWebService2010 : 21/06/2019",
                           "letture per PUA")

            Riga_Requisiti("Migra '557' :",
                           "NECESSARIO per nuova gestione patentini")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO. ")

            Riga_Text("PUA",
                      "- modificata funzione PC_Finalita_Rer_BPerc_WS per escludere doppioni" &
                      "- modificata funzione Leggi_Analisi_Testata_Filtrata")

            '==================================

            Riga_Data("13 Novembre 2019")

            Riga_Requisiti("AgronicaWebService2010 : 21/06/2019",
                           "letture per PUA")

            Riga_Requisiti("Migra '557' :",
                           "NECESSARIO per nuova gestione patentini")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO. ")

            Riga_Text("Cancellazione documenti",
                      " Correzione controlli cancellazione documenti")

            '==================================

            Riga_Data("12 Novembre 2019")

            Riga_Requisiti("AgronicaWebService2010 : 21/06/2019",
                           "letture per PUA")

            Riga_Requisiti("Migra '557' :",
                           "NECESSARIO per nuova gestione patentini")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO. ")

            Riga_Text("Lettura prodotti globale",
                      " Aggiunta parametro tipo regolamento PUA")

            '==================================

            Riga_Data("09 Ottobre 2019")

            Riga_Requisiti("AgronicaWebService2010 : 21/06/2019",
                           "letture per PUA")

            Riga_Requisiti("Migra '557' :",
                           "NECESSARIO per nuova gestione patentini")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO. ")

            Riga_Text("GIS",
                      " Bug fix su mancata assegnazione filtro temporale nell'albero")

            '==================================

            Riga_Data("07 Ottobre 2019")

            Riga_Requisiti("AgronicaWebService2010 : 21/06/2019",
                           "letture per PUA")

            Riga_Requisiti("Migra '557' :",
                           "NECESSARIO per nuova gestione patentini")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO. ")

            Riga_Text("Albero Anagrafica 2017",
                      " bug fix: imposto anche il tipo nodo chiave altrimenti se si imposta il tipo dell'impianto precedente e se è diverso viene impostata una chiave albero erronea. ")

            Riga_Text("PUA",
                      "modificata funzione PC_Finalita_Rer_BPerc_WS")

            '==================================

            Riga_Data("04 Ottobre 2019")

            Riga_Requisiti("AgronicaWebService2010 : 21/06/2019",
                           "letture per PUA")

            Riga_Requisiti("Migra '557' :",
                           "NECESSARIO per nuova gestione patentini")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO. ")

            Riga_Text("Albero Anagrafica 2017",
                      " bug fix: imposto anche il tipo nodo chiave altrimenti se si imposta il tipo dell'impianto precedente e se è diverso viene impostata una chiave albero erronea. ")

            Riga_Text("PUA",
                      "- Aggiunta funzione PC_Finalita_Rer_WS" &
                      "- Aggiunta funzione PC_Finalita_Rer_BPerc_WS")

            Riga_Text("Metaschema/SpecieVegetali.asmx:",
                      "Eliminata cache in leggi specie con filtro utente")

            '==================================

            Riga_Data("25 Settembre 2019")


            Riga_Requisiti("AgronicaWebService2010 : 21/06/2019",
                           "letture per PUA")

            Riga_Requisiti("Migra '557' :",
                           "NECESSARIO per nuova gestione patentini")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO. ")

            Riga_Text("WS estrazione attività per GiasAPP",
                      "Tolta where su Attivita_Extra_Campagna = 1 nell'estrazione di attività legate a progetti (cartellette) ")

            Riga_Text("WS Contatti",
                      "aggiunte chiamate per nuova gestione patentini ")

            '==================================

            Riga_Data("01 Agosto 2019")


            Riga_Requisiti("AgronicaWebService2010 : 21/06/2019",
                           "letture per PUA")

            Riga_Requisiti("Migra '543' :",
                           "NECESSARIO per nuovi campi in Agenda e Mov_Det_Tecnico_Extra")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO. ")

            Riga_Text("Albero Anagrafica Catasto",
                      "Descrizione nodo con Provincia, Comune e Cod Belfiore da join tabella ISTAT (per ora solo contesto GIS) ")

            '==================================


            Riga_Data("17 Luglio 2019")


            Riga_Requisiti("AgronicaWebService2010 : 21/06/2019",
                           "letture per PUA")

            Riga_Requisiti("Migra '543' :",
                           "NECESSARIO per nuovi campi in Agenda e Mov_Det_Tecnico_Extra")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO. ")

            Riga_Text("WS F&F",
                      "Correzioni varie")


            '==================================

            Riga_Data("21 Giugno 2019")


            Riga_Requisiti("AgronicaWebService2010 : 21/06/2019",
                           "letture per PUA")

            Riga_Requisiti("Migra '543' :",
                           "NECESSARIO per nuovi campi in Agenda e Mov_Det_Tecnico_Extra")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO. ")

            Riga_Text("PUA",
                      "Aggiunta lettura PC_TipoAcqua")


            '==================================

            Riga_Data("19 Giugno 2019")


            Riga_Requisiti("AgronicaWebService2010 : 07/06/2019",
                           "letture per PUA")

            Riga_Requisiti("Migra '543' :",
                           "NECESSARIO per nuovi campi in Agenda e Mov_Det_Tecnico_Extra")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO. ")

            Riga_Text("WS APP",
                      "Aggiunta gestione AttivitaXCentriAziendali")

            Riga_Text("PUA",
                      "Aggiunta lettura di chiave WS anche da configurazione siti super server")


            '==================================

            Riga_Data("17 Giugno 2019")

            Riga_Requisiti("AgronicaWebService2010 : 07/06/2019",
                           "letture per PUA")

            Riga_Requisiti("Migra '523' :",
                           "NECESSARIO per nuovi campi in Agenda e Mov_Det_Tecnico_Extra")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO. ")

            Riga_Text("Pua",
                      "Modificata dropdown analisi")

            Riga_Text("Gestione_Menu.asmx",
                      "aggiunto web method per indicazione pratica e stato")

            '==================================

            Riga_Data("07 Giugno 2019")

            Riga_Requisiti("AgronicaWebService2010 : 07/06/2019",
                           "letture per PUA")

            Riga_Requisiti("Migra '523' :",
                           "NECESSARIO per nuovi campi in Agenda e Mov_Det_Tecnico_Extra")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO. ")

            Riga_Text("Pua",
                      "Modifiche varie per rilascio nuovo PUA")

            '==================================

            Riga_Data("06 Giugno 2019")

            Riga_Requisiti("AgronicaWebService2010 : 14/05/2018",
                           "Audit: gestiti campi null su domande interviste")

            Riga_Requisiti("Migra '523' :",
                           "NECESSARIO per nuovi campi in Agenda e Mov_Det_Tecnico_Extra")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO. ")

            Riga_Text("Registri Telematici",
                      "Modifica per mappatura prodotti")

            '==================================


            Riga_Data("30 Maggio 2019")

            Riga_Requisiti("AgronicaWebService2010 : 14/05/2018",
                           "Audit: gestiti campi null su domande interviste")

            Riga_Requisiti("Migra '523' :",
                           "NECESSARIO per nuovi campi in Agenda e Mov_Det_Tecnico_Extra")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO. ")

            Riga_Text("Ricerca prodotti",
                      "Cambiato filtro per i formulati nel caso di chiamata dai costi")

            '==================================

            Riga_Data("29 Maggio 2019")

            Riga_Requisiti("AgronicaWebService2010 : 14/05/2018",
                           "Audit: gestiti campi null su domande interviste")

            Riga_Requisiti("Migra '523' :",
                           "NECESSARIO per nuovi campi in Agenda e Mov_Det_Tecnico_Extra")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO. ")

            Riga_Text("Pua",
                      "Modifiche varie per rilascio nuovo PUA")

            '==================================

            Riga_Data("24 Maggio 2019")

            Riga_Requisiti("AgronicaWebService2010 : 14/05/2018",
                           "Audit: gestiti campi null su domande interviste")

            Riga_Requisiti("Migra '523' :",
                           "NECESSARIO per nuovi campi in Agenda e Mov_Det_Tecnico_Extra")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO. ")

            Riga_Text("Metaschema/SpecieVegetali.asmx",
                      " - Filtro utente su restituzione specie vegetali")

            '==================================

            Riga_Data("07 Maggio 2019")

            Riga_Requisiti("AgronicaWebService2010 : 14/05/2018",
                           "Audit: gestiti campi null su domande interviste")

            Riga_Requisiti("Migra '523' :",
                           "NECESSARIO per nuovi campi in Agenda e Mov_Det_Tecnico_Extra")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO. ")

            Riga_Text("Core Utility:",
                      "Chiamata RestSharp")


            '==================================


            Riga_Data("02 Maggio 2019")

            Riga_Requisiti("AgronicaWebService2010 : 14/05/2018",
                           "Audit: gestiti campi null su domande interviste")

            Riga_Requisiti("Migra '523' :",
                           "NECESSARIO per nuovi campi in Agenda e Mov_Det_Tecnico_Extra")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO. ")

            Riga_Text("Ricette - ImportaRicetteDaTabelleAPP:",
                      " fix baco per cui veniva aperta inutilmente una connessione a EF ad ogni iterazione e dopo 100 operazioni andava in eccezione")


            '==================================

            Riga_Data("30 aprile 2019")

            Riga_Requisiti("AgronicaWebService2010 : 14/05/2018",
                           "Audit: gestiti campi null su domande interviste")

            Riga_Requisiti("Migra '523' :",
                           "NECESSARIO per nuovi campi in Agenda e Mov_Det_Tecnico_Extra")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO. ")

            Riga_Text("WS Ricerca prodotti:",
                      " Modifica per testare impostazione utente 843 che indica che codifica prodotti è pubblica")


            '==================================

            Riga_Data("26 aprile 2019")

            Riga_Requisiti("AgronicaWebService2010 : 14/05/2018",
                           "Audit: gestiti campi null su domande interviste")

            Riga_Requisiti("Migra '523' :",
                           "NECESSARIO per nuovi campi in Agenda e Mov_Det_Tecnico_Extra")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO. ")

            Riga_Text("WS Ricerca formulati:",
                      " Eliminati doppioni presenti in caso di periodi multipli di sospensione")

            Riga_Text("WS Ricerca prodotti:",
                      " Corretto errore sulla ricerca magazzini gestiti da APP")



            '==================================

            Riga_Data("24 aprile 2019")

            Riga_Requisiti("AgronicaWebService2010 : 14/05/2018",
                           "Audit: gestiti campi null su domande interviste")

            Riga_Requisiti("Migra '523' :",
                           "NECESSARIO per nuovi campi in Agenda e Mov_Det_Tecnico_Extra")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO. ")

            Riga_Text("WS Ricerca dipendenti:",
                      " Tolti zeri non significativi dal nr badge")

            Riga_Text("WS Ricerca formulati:",
                      " Gestita ricerca prodotti solo a giacenza")

            Riga_Text("WS Ricerca giacenze:",
                      " Corretto errore sulla ricerca magazzini gestiti da APP")



            '==================================

            Riga_Data("17 aprile 2019 tris")


            Riga_Requisiti("AgronicaWebService2010 : 14/05/2018",
                           "Audit: gestiti campi null su domande interviste")

            Riga_Requisiti("Migra '523' :",
                           "NECESSARIO per nuovi campi in Agenda e Mov_Det_Tecnico_Extra")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO. ")

            Riga_Text("Metaschema/SpecieVegetali.asmx",
                      " - Filtro utente su restituzione specie vegetali")

            '==================================



            Riga_Data("17 aprile 2019 bis")

            Riga_Requisiti("AgronicaWebService2010 : 14/05/2018",
                           "Audit: gestiti campi null su domande interviste")

            Riga_Requisiti("Migra '523' :",
                           "NECESSARIO per nuovi campi in Agenda e Mov_Det_Tecnico_Extra")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO. ")

            Riga_Text("WS Ricerca prodotti:",
                      " - Nuova modifica ricerca cod articolo cliente per prodotti di banche dati perchè ci sono più articoli dello stesso tipo codificati con codici cliente diversi")

            '==================================

            Riga_Data("17 aprile 2019")

            Riga_Requisiti("AgronicaWebService2010 : 14/05/2018",
                           "Audit: gestiti campi null su domande interviste")

            Riga_Requisiti("Migra '523' :",
                           "NECESSARIO per nuovi campi in Agenda e Mov_Det_Tecnico_Extra")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO. ")

            Riga_Text("WS Ricerca prodotti:",
                      " - Corretta ricerca cod articolo cliente per prodotti di banche dati ")

            '==================================

            Riga_Data("16 aprile 2019")

            Riga_Requisiti("AgronicaWebService2010 : 14/05/2018",
                           "Audit: gestiti campi null su domande interviste")

            Riga_Requisiti("Migra '523' :",
                           "NECESSARIO per nuovi campi in Agenda e Mov_Det_Tecnico_Extra")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO. ")

            Riga_Text("Dose NPK:",
                      " - Corretto baco su lettura valore N P K")

            '==================================


            Riga_Data("6 aprile 2019")

            Riga_Requisiti("AgronicaWebService2010 : 14/05/2018",
                           "Audit: gestiti campi null su domande interviste")

            Riga_Requisiti("Migra '523' :",
                           "NECESSARIO per nuovi campi in Agenda e Mov_Det_Tecnico_Extra")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO. ")

            Riga_Text("WS ricerca prodotti:",
                      "modifiche utilizzo nuova impostazione giacenze per categoria prodotto")

            '==================================

            Riga_Data("05 Aprile 2019")

            Riga_Requisiti("AgronicaWebService2010 : 14/05/2018",
                           "Audit: gestiti campi null su domande interviste")

            Riga_Requisiti("Migra '523' :",
                           "NECESSARIO per nuovi campi in Agenda e Mov_Det_Tecnico_Extra")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO. ")

            Riga_Text("GerarchiaImprese:",
                      "bugfix per funzioni con stesso nome, ma firme diverse")

            '==================================

            Riga_Data("04 Aprile 2019")

            Riga_Requisiti("AgronicaWebService2010 : 14/05/2018",
                           "Audit: gestiti campi null su domande interviste")

            Riga_Requisiti("Migra '523' :",
                           "NECESSARIO per nuovi campi in Agenda e Mov_Det_Tecnico_Extra")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO. ")

            Riga_Text("AlberoAnagrafica2017:",
                      "aggiunti dettagli su terreno nudo in planning")

            '==================================

            Riga_Data("3 aprile 2019")

            Riga_Requisiti("AgronicaWebService2010 : 14/05/2018",
                           "Audit: gestiti campi null su domande interviste")

            Riga_Requisiti("Migra '523' :",
                           "NECESSARIO per nuovi campi in Agenda e Mov_Det_Tecnico_Extra")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO. ")

            Riga_Text("WS CdG ed APP:",
                      "modifiche varie")

            '==================================

            Riga_Data("28 Marzo 2019")

            Riga_Requisiti("AgronicaWebService2010 : 14/05/2018",
                           "Audit: gestiti campi null su domande interviste")

            Riga_Requisiti("Migra '523' :",
                           "NECESSARIO per nuovi campi in Agenda e Mov_Det_Tecnico_Extra")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO. ")

            Riga_Text("Ricette.asmx:",
                      "gestione split attività su cambio data + ribaltamento attività indirette" &
                      "aggiunto semine/trapianti e pirodiserbo tra le operazioni ricettabili")

            '==================================
            Riga_Data("21 Marzo 2019")

            Riga_Requisiti("AgronicaWebService2010 : 14/05/2018",
                           "Audit: gestiti campi null su domande interviste")

            Riga_Requisiti("Migra '523' :",
                           "NECESSARIO per nuovi campi in Agenda e Mov_Det_Tecnico_Extra")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO. ")

            Riga_Text("Reg_Impianti.asmx:",
                      "lettura anagrafica apezzamenti-impianti")

            '==================================
            Riga_Data("15 Marzo 2019")

            Riga_Requisiti("AgronicaWebService2010 : 14/05/2018",
                           "Audit: gestiti campi null su domande interviste")

            Riga_Requisiti("Migra '523' :",
                           "NECESSARIO per nuovi campi in Agenda e Mov_Det_Tecnico_Extra")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO. ")

            Riga_Text("Operazioni.asmx:",
                      "Corretto filtro su gruppo di operazioni")

            '==================================
            Riga_Data("14 Marzo 2019")

            Riga_Requisiti("AgronicaWebService2010 : 14/05/2018",
                           "Audit: gestiti campi null su domande interviste")

            Riga_Requisiti("Migra '523' :",
                           "NECESSARIO per nuovi campi in Agenda e Mov_Det_Tecnico_Extra")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO. ")

            Riga_Text("Uteni_Impostazioni_R + Operazioni:",
                      "Gestione lettura operazioni e preferiti filtrato per gruppo di operazioni")


            '==================================
            Riga_Data("12 Marzo 2019")

            Riga_Requisiti("AgronicaWebService2010 : 14/05/2018",
                           "Audit: gestiti campi null su domande interviste")

            Riga_Requisiti("Migra '523' :",
                           "NECESSARIO per nuovi campi in Agenda e Mov_Det_Tecnico_Extra")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO. ")

            Riga_Text("Contatti:",
                      "- Leggi contatto specifico con cod_contatto")


            '==================================
            '==================================
            Riga_Data("11 Marzo 2019")

            Riga_Requisiti("AgronicaWebService2010 : 14/05/2018",
                           "Audit: gestiti campi null su domande interviste")

            Riga_Requisiti("Migra '523' :",
                           "NECESSARIO per nuovi campi in Agenda e Mov_Det_Tecnico_Extra")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO. ")

            Riga_Text("Scadenzario:",
                      "- Tolta in descrizione delle analisi il dettaglio su cosa era agganciata in quanto la scadenza è dell'analisi" &
                      "- Corretto baco per cui venivano cancellate le scadenze al 31/12/2100 con relativi allegati. Ora in caso di allegati e AGRODATAFINE non cancello più nulla")


            '==================================
            Riga_Data("08 Marzo 2019")

            Riga_Requisiti("AgronicaWebService2010 : 14/05/2018",
                           "Audit: gestiti campi null su domande interviste")

            Riga_Requisiti("Migra '523' :",
                           "NECESSARIO per nuovi campi in Agenda e Mov_Det_Tecnico_Extra")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO. ")

            Riga_Text("GerarchiaPadre:", "")

            '==================================

            Riga_Data("26 Febbraio 2019")

            Riga_Requisiti("AgronicaWebService2010 : 14/05/2018",
                           "Audit: gestiti campi null su domande interviste")

            Riga_Requisiti("Migra '523' :",
                           "NECESSARIO per nuovi campi in Agenda e Mov_Det_Tecnico_Extra")

            Riga_Requisiti("WEB.CONFIG :",
                           "Modifiche NO. ")

            Riga_Text("UtentiImpostazioni:",
                      "Agenda e CoreWS: modifica per passare come parametro se impostazione SuperUser o utente")

            '==================================

            Riga_Data("14 Febbraio 2019")

            Riga_Requisiti("AgronicaWebService2010 : 14/05/2018",
                      "Audit: gestiti campi null su domande interviste")

            Riga_Requisiti("Migra '508' :",
                      "NECESSARIO per nuovi campi e tabelle Zoo")

            Riga_Requisiti("WEB.CONFIG :",
                      "Modifiche NO. ")

            Riga_Text("Campi.asmx:",
                      "Lettura campi per sincro")

            '==================================

            Riga_Data("12 Febbraio 2019")

            Riga_Requisiti("AgronicaWebService2010 : 14/05/2018",
                      "Audit: gestiti campi null su domande interviste")

            Riga_Requisiti("Migra '508' :",
                      "NECESSARIO per nuovi campi e tabelle Zoo")

            Riga_Requisiti("WEB.CONFIG :",
                      "Modifiche NO. ")

            Riga_Text("Visite.asmx:",
                      "fix orario su visite")


            '==================================

            Riga_Data("01 Febbraio 2019")

            Riga_Requisiti("AgronicaWebService2010 : 14/05/2018",
                      "Audit: gestiti campi null su domande interviste")

            Riga_Requisiti("Migra '508' :",
                      "NECESSARIO per nuovi campi e tabelle Zoo")

            Riga_Requisiti("WEB.CONFIG :",
                      "Modifiche NO. ")

            Riga_Text("Utenti_Impostazioni_R.asmx:",
                      "nuovo core per cancellare le personalizzazioni sulla griglia kendo")


            '==================================

            Riga_Data("30 novembre 2018")

            Riga_Requisiti("AgronicaWebService2010 : 14/05/2018",
                      "Audit: gestiti campi null su domande interviste")

            Riga_Requisiti("Migra '508' :",
                      "NECESSARIO per nuovi campi e tabelle Zoo")

            Riga_Requisiti("WEB.CONFIG :",
                      "Modifiche NO. ")

            Riga_Text("CostantiPersonalizzate ",
                   "tolta la semina su sodo dalle operazioni ricettabili ")

            Riga_Text("Alert_Tipologia.asmx ",
                   "Ora se la scadenza è creata da un'altra azienda, la modifica e la cancellazione che vanno in errore mostrano l'azienda da cui è creata ")

            Riga_Text("Gestione_menu.asmx",
                   "Aggiunte API per gestione nuovo menu GIAS")

            '==================================

            Riga_Data("6 Novembre 2018")

            Riga_Requisiti("AgronicaWebService2010 : 14/05/2018",
                      "Audit: gestiti campi null su domande interviste")

            Riga_Requisiti("Migra '508' :",
                      "NECESSARIO per nuovi campi e tabelle Zoo")

            Riga_Requisiti("WEB.CONFIG :",
                      "Modifiche NO. ")

            Riga_Text("AgronicaCoreDPI ",
                   "Corretto baco su CaricaTipiConduzione")

            '==================================
            '==================================

            Riga_Data("2 Novembre 2018")

            Riga_Requisiti("AgronicaWebService2010 : 14/05/2018",
                      "Audit: gestiti campi null su domande interviste")

            Riga_Requisiti("Migra '508' :",
                      "NECESSARIO per nuovi campi e tabelle Zoo")

            Riga_Requisiti("WEB.CONFIG :",
                      "Modifiche NO. ")

            Riga_Text("WS APP Giacenze ",
                   "Estrazione delle giacenze dei soli magazzini gestiti da APP")

            '==================================

            Riga_Data("24 Ottobre 2018")

            Riga_Requisiti("AgronicaWebService2010 : 14/05/2018",
                      "Audit: gestiti campi null su domande interviste")

            Riga_Requisiti("Migra '508' :",
                      "NECESSARIO per nuovi campi e tabelle Zoo")

            Riga_Requisiti("WEB.CONFIG :",
                      "Modifiche NO. ")

            Riga_Text("Categorie_Magazzino.asmx, Prodotti.asmx ",
                   "x pagina codifica prodotti aziendali (AgronicaSincronizzatore)")

            '==================================

            Riga_Data("23 Ottobre 2018")

            Riga_Requisiti("Componenti '' :",
                      "")

            Riga_Requisiti("AgronicaWebService2010 : 14/05/2018",
                      "Audit: gestiti campi null su domande interviste")

            Riga_Requisiti("Migra '508' :",
                      "NECESSARIO per nuovi campi e tabelle Zoo")

            Riga_Requisiti("WEB.CONFIG :",
                      "Modifiche SI/NO. ")

            Riga_Text("WS APP",
                   "Modifiche per nuove autorizzazioni")

            '==================================

            Riga_Data("12 Ottobre 2018")

            Riga_Requisiti("Componenti '' :",
                      "")

            Riga_Requisiti("AgronicaWebService2010 : 14/05/2018",
                      "Audit: gestiti campi null su domande interviste")

            Riga_Requisiti("Migra '508' :",
                      "NECESSARIO per nuovi campi e tabelle Zoo")

            Riga_Requisiti("WEB.CONFIG :",
                      "Modifiche SI/NO. ")

            Riga_Text("Operazioni.asmx, Utenti_Impostazioni_R.asmx",
                   "Portate su costante le operazioni ricettabili")


            '==================================
            Riga_Data("08 Ottobre 2018")

            Riga_Requisiti("Componenti '' :",
                      "")

            Riga_Requisiti("AgronicaWebService2010 : 14/05/2018",
                      "Audit: gestiti campi null su domande interviste")

            Riga_Requisiti("Migra '508' :",
                      "NECESSARIO per nuovi campi e tabelle Zoo")

            Riga_Requisiti("WEB.CONFIG :",
                      "Modifiche SI/NO. ")

            Riga_Text("Agronica_Log_Anagrafe:",
                      "Aggiunto log in tabella in scrittura/modifica/cancellazione di Reg_Impianti+Imprese_Progetti (BIZ+EF)")

            Riga_Text("Operazioni.asmx, Utenti_Impostazioni_R.asmx",
                   "aggiunti nuovi WS per lettura operazioni e preferiti del brogliaccio" &
                   "filtrate le operazioni ricettabili alle sole gestite dalla pagina trattamenti_2")

            Riga_Text("Lettura tabelle zoo",
                   "Stalla_Raggruppamenti" & vbCrLf &
                   "Zoo_Animali" & vbCrLf &
                   "Fabbricati")

            '==================================

            Riga_Data("11 Settembre 2018")

            Riga_Requisiti("AgronicaCore '' :",
                      "")

            Riga_Requisiti("Componenti '' :",
                      "")

            Riga_Requisiti("AgronicaWebService2010 : 14/05/2018",
                      "Audit: gestiti campi null su domande interviste")

            Riga_Requisiti("Migra '503' :",
                      "NECESSARIO per modifiche a CdG")

            Riga_Requisiti("WEB.CONFIG :",
                      "Modifiche SI/NO. ")

            Riga_Text("WS APP Macchine e Persone",
                   "aggiunta gestione records pubblici e privati")

            '==================================

            Riga_Data("06 Settembre 2018")

            Riga_Requisiti("AgronicaCore '' :",
                      "")

            Riga_Requisiti("Componenti '' :",
                      "")

            Riga_Requisiti("AgronicaWebService2010 : 14/05/2018",
                      "Audit: gestiti campi null su domande interviste")

            Riga_Requisiti("Migra '503' :",
                      "NECESSARIO per modifiche a CdG")

            Riga_Requisiti("WEB.CONFIG :",
                      "Modifiche SI/NO. ")

            Riga_Text("WS APP Progetti",
                   "aggiunto filtro per partita iva")

            '==================================

            Riga_Data("05 Settembre 2018")

            Riga_Requisiti("AgronicaCore '' :",
                      "")

            Riga_Requisiti("Componenti '' :",
                      "")

            Riga_Requisiti("AgronicaWebService2010 : 14/05/2018",
                      "Audit: gestiti campi null su domande interviste")

            Riga_Requisiti("Migra '503' :",
                      "NECESSARIO per modifiche a CdG")

            Riga_Requisiti("WEB.CONFIG :",
                      "Modifiche SI/NO. ")

            Riga_Text("Scadenzario",
                   "fix su core per scadenze")

            '==================================

            Riga_Data("04 Settembre 2018")

            Riga_Requisiti("AgronicaCore '' :",
                      "")

            Riga_Requisiti("Componenti '' :",
                      "")

            Riga_Requisiti("AgronicaWebService2010 : 14/05/2018",
                      "Audit: gestiti campi null su domande interviste")

            Riga_Requisiti("Migra '503' :",
                      "NECESSARIO per modifiche a CdG")

            Riga_Requisiti("WEB.CONFIG :",
                      "Modifiche SI/NO. ")

            Riga_Text("Contab/Attivita.asmx",
                   "gestite attività relative alle aziende")

            Riga_Text("Contab/Ricette.asmx",
                   "gestite attività tempi risorse inviate da app")

            '==================================

            Riga_Data("24 Agosto 2018 B")

            Riga_Requisiti("AgronicaCore '' :",
                      "")

            Riga_Requisiti("Componenti '' :",
                      "")

            Riga_Requisiti("AgronicaWebService2010 : 14/05/2018",
                      "Audit: gestiti campi null su domande interviste")

            Riga_Requisiti("Migra '503' :",
                      "NECESSARIO per modifiche a CdG")

            Riga_Requisiti("WEB.CONFIG :",
                      "Modifiche SI/NO. ")

            Riga_Text("magazzino.vb",
                   "fix calcolo giaceze")


            '==================================

            Riga_Data("24 Agosto 2018")

            Riga_Requisiti("AgronicaCore '' :",
                      "")

            Riga_Requisiti("Componenti '' :",
                      "")

            Riga_Requisiti("AgronicaWebService2010 : 14/05/2018",
                      "Audit: gestiti campi null su domande interviste")

            Riga_Requisiti("Migra '503' :",
                      "NECESSARIO per modifiche a CdG")

            Riga_Requisiti("WEB.CONFIG :",
                      "Modifiche SI/NO. ")

            Riga_Text("Ricreati solo per modifiche a Core EF",
                   "Core WS ricreati solo per modifiche a Core EF")


            '==================================

            Riga_Data("22 Agosto 2018 BIS")

            Riga_Requisiti("AgronicaCore '' :",
                      "")

            Riga_Requisiti("Componenti '' :",
                      "")

            Riga_Requisiti("AgronicaWebService2010 : 14/05/2018",
                      "Audit: gestiti campi null su domande interviste")

            Riga_Requisiti("Migra '499' :",
                      "NECESSARIO per modifiche strutturali su varie cose")

            Riga_Requisiti("WEB.CONFIG :",
                      "Modifiche SI/NO. ")

            Riga_Text("Metaschema/Operazioni.asmx",
                   "Gestito la vendita/corrispettivi")


            '==================================

            Riga_Data("22 Agosto 2018")

            Riga_Requisiti("AgronicaCore '' :",
                      "")

            Riga_Requisiti("Componenti '' :",
                      "")

            Riga_Requisiti("AgronicaWebService2010 : 14/05/2018",
                      "Audit: gestiti campi null su domande interviste")

            Riga_Requisiti("Migra '499' :",
                      "NECESSARIO per modifiche strutturali su varie cose")

            Riga_Requisiti("WEB.CONFIG :",
                      "Modifiche SI/NO. ")

            Riga_Text("Ricette",
                   " Aggiunti controlli in travaso dati da interscambio a Ricette per APP " &
                   " Corretto baco per cui una ricetta travasata male veniva riprocessata al giro successivo" &
                   " Spostato codice in core")

            Riga_Text("WS App",
                   " Modifica WS prodotti e giacenze per estrarre solo le categorie di prodotto interessate ")

            '==================================



            Riga_Data("06 Agosto 2018")

            Riga_Requisiti("AgronicaCore '' :",
                      "")

            Riga_Requisiti("Componenti '' :",
                      "")

            Riga_Requisiti("AgronicaWebService2010 : 14/05/2018",
                      "Audit: gestiti campi null su domande interviste")

            Riga_Requisiti("Migra '499' :",
                      "NECESSARIO per modifiche strutturali su varie cose")

            Riga_Requisiti("WEB.CONFIG :",
                      "Modifiche SI/NO. ")

            Riga_Text("Rilievi",
                   " Gestiti i rilievi indici rese/raccolta ")

            '==================================


            Riga_Data("03 Agosto 2018")

            Riga_Requisiti("AgronicaCore '' :",
                      "")

            Riga_Requisiti("Componenti '' :",
                      "")

            Riga_Requisiti("AgronicaWebService2010 : 14/05/2018",
                      "Audit: gestiti campi null su domande interviste")

            Riga_Requisiti("Migra '499' :",
                      "NECESSARIO per modifiche strutturali su varie cose")

            Riga_Requisiti("WEB.CONFIG :",
                      "Modifiche SI/NO. ")

            Riga_Text("GiasAPP",
                   " modificata gestione transazione in importazione da tabelle di frontiera ")
            '==================================


            Riga_Data("02 Agosto 2018")

            Riga_Requisiti("AgronicaCore '' :",
                      "")

            Riga_Requisiti("Componenti '' :",
                      "")

            Riga_Requisiti("AgronicaWebService2010 : 14/05/2018",
                      "Audit: gestiti campi null su domande interviste")

            Riga_Requisiti("Migra '499' :",
                      "NECESSARIO per modifiche strutturali su varie cose")

            Riga_Requisiti("WEB.CONFIG :",
                      "Modifiche SI/NO. ")

            Riga_Text("GiasAPP",
                   "Aggiunto Web Service per impostazioni utente")

            Riga_Text("GiasAPP",
                   "Aggiunta lettura NrBadge sul WS Leggi_Contatti_APP")

            Riga_Text("GiasAPP",
                   "Aggiunta lettura Attivita_Extra_Campagna e introdotto filtro su Utilizzo_GiasAPP sul WS Leggi_Attivita_APP")

            Riga_Text("GiasAPP",
                   "Introdotto filtro su Visibile_ctrl_gestione sul WS Leggi_Macchine_APP")
            '==================================

            Riga_Data("10 Luglio 2018")

            Riga_Requisiti("AgronicaCore '' :",
                      "")

            Riga_Requisiti("Componenti '' :",
                      "")

            Riga_Requisiti("AgronicaWebService2010 : 14/05/2018",
                      "Audit: gestiti campi null su domande interviste")

            Riga_Requisiti("Migra '494' :",
                      "Per web service APP")

            Riga_Requisiti("WEB.CONFIG :",
                      "Modifiche SI/NO. ")

            Riga_Text("Rilievi",
                   "WIP")

            '==================================


            Riga_Data("09 Luglio 2018 ")

            Riga_Requisiti("AgronicaCore '' :",
                      "")

            Riga_Requisiti("Componenti '' :",
                      "")

            Riga_Requisiti("AgronicaWebService2010 : 14/05/2018",
                      "Audit: gestiti campi null su domande interviste")

            Riga_Requisiti("Migra '494' :",
                      "Per web service APP")

            Riga_Requisiti("WEB.CONFIG :",
                      "Modifiche SI/NO. ")

            Riga_Text("Lista_Categorie_Animali.asmx",
                   "Core di lettura")

            Riga_Text("Lista_Generi_Animali.asmx",
                   "Core di lettura")

            Riga_Text("Lista_IndirizziProd_Animali.asmx",
                   "Core di lettura")

            Riga_Text("Lista_Razze_Animali.asmx",
                   "Core di lettura")

            Riga_Text("Lista_Specie_Animali.asmx",
                   "Core di lettura")

            '==================================
            '==================================


            Riga_Data("06 Luglio 2018 ")

            Riga_Requisiti("AgronicaCore '' :",
                      "")

            Riga_Requisiti("Componenti '' :",
                      "")

            Riga_Requisiti("AgronicaWebService2010 : 14/05/2018",
                      "Audit: gestiti campi null su domande interviste")

            Riga_Requisiti("Migra '494' :",
                      "Per web service APP")

            Riga_Requisiti("WEB.CONFIG :",
                      "Modifiche SI/NO. ")


            Riga_Text("Metaschema/Epoche.asmx",
                   "Aggiunto WS per epoche fertilizzanti da richiamare nell'APP")

            Riga_Text("Web Services per APP",
                   "Nuovo Rilascio dei Web Services per prodotti e giacenze ")

            Riga_Text("Utenti_R.asmx",
                   " se l'utente non ha mai fatto il login attraverso l'interfaccia standard allora non viene inizializzata la tabella ""utenti_Visibilita_Appoggio"", quindi non viene impostata la visibilità ed erroneamente vengono mostrate tutte le aziende! ")

            '==================================

            Riga_Data("19 Giugno 2018")

            Riga_Requisiti("AgronicaCore '' :",
                      "")

            Riga_Requisiti("Componenti '' :",
                      "")

            Riga_Requisiti("AgronicaWebService2010 : 14/05/2018",
                      "Audit: gestiti campi null su domande interviste")

            Riga_Requisiti("Migra '494' :",
                      "Per web service APP")

            Riga_Requisiti("WEB.CONFIG :",
                      "Modifiche SI/NO. ")

            Riga_Text("AgronicaCoreUtenti_Biz/Utenti_R.asmx",
                   "Web service di autenticazione, corretto baco se viene chiamata la funzione con dati sbagliati ")

            Riga_Text("WS per Gias APP",
                   "Primo rilascio in beta")

            Riga_Text("Web Services per APP",
                   "Rilascio dei Web Services per prodotti e giacenze ")


            '==================================

            Riga_Data("04 Giugno 2018")

            Riga_Requisiti("AgronicaCore '' :",
                      "")

            Riga_Requisiti("Componenti '' :",
                      "")

            Riga_Requisiti("AgronicaWebService2010 : 14/05/2018",
                      "Audit: gestiti campi null su domande interviste")

            Riga_Requisiti("Migra '492' :",
                      "Per web service di autenticazione + scadenzario")

            Riga_Requisiti("WEB.CONFIG :",
                      "Modifiche SI/NO. ")


            Riga_Text("AgronicaCoreUtenti_Biz/Utenti_R.asmx",
                   "Web service di autenticazione")

            Riga_Text("Varie",
                   "Web service pro Gias APP")

            Riga_Fine()



            '==================================

            Riga_Data(" 23 Maggio 2018 ")

            Riga_Requisiti("AgronicaCore '' :",
                      "")

            Riga_Requisiti("Componenti '' :",
                      "")

            Riga_Requisiti("Migra '475' :",
                      "Messaggio per contatti e vasi non ancora telematizzati")

            Riga_Requisiti("AgronicaWebService2010 : 14/05/2018",
                      "Audit: gestiti campi null su domande interviste")

            Riga_Requisiti("WEB.CONFIG :",
                      "Modifiche NO. ")

            Riga_Text("Metaschema/Rilievi.asmx",
                   "modificata risposta lettura fasi fenologiche")

            Riga_Text("AgronicaCoreAuditBIZ",
                   "Aggiornate funzioni per il calcolo automatico livelli")


            '==================================

            Riga_Data(" 21 Maggio 2018 ")

            Riga_Requisiti("AgronicaCore '' :",
                      "")

            Riga_Requisiti("Componenti '' :",
                      "")

            Riga_Requisiti("Migra '475' :",
                      "Messaggio per contatti e vasi non ancora telematizzati")

            Riga_Requisiti("AgronicaWebService2010 : 11/05/2018",
                      "Audit: gestiti campi null su domande interviste")

            Riga_Requisiti("WEB.CONFIG :",
                      "Modifiche NO. ")

            Riga_Text(" WS cartella Anagrafica: ",
                   "Aggiunto WS Ricerca Globale Prodotti")

            Riga_Text(" WS F&F: ",
                   "Modificata ricerca magazzini")

            Riga_Text(" WS Metaschema: ",
                   "Aggiunti WS CategorieXUnitaMisura e PUA_Regolamenti")

            Riga_Text("Metaschema/Rilievi.asmx",
                   "modificata lettura fasi fenologiche")

            Riga_Text("Anagrafica/Prodotti.asmx",
                   "aggiunto WS di ricerca prodotti senza filtro per categorie")


            '==================================


            Riga_Data(" 16 Maggio 2018 ")

            Riga_Requisiti("AgronicaCore '' :",
                      "")

            Riga_Requisiti("Componenti '' :",
                      "")

            Riga_Requisiti("Migra '475' :",
                      "Messaggio per contatti e vasi non ancora telematizzati")

            Riga_Requisiti("AgronicaWebService2010 : 11/05/2018",
                      "Audit: gestiti campi null su domande interviste")

            Riga_Requisiti("WEB.CONFIG :",
                      "Modifiche NO. ")

            Riga_Text("AgronicaCoreAuditBIZ",
                   "Calcolo automatico livelli + ottimizzazione disposizioni attive")

            Riga_Text("CentroAziendale.asmx",
                   "Funzione GetCentri per gestione catastale in ImportazionePC_Anteprima")

            '==================================

            Riga_Data(" 11 Maggio 2018 ")

            Riga_Requisiti("AgronicaCore '' :",
                      "")

            Riga_Requisiti("Componenti '' :",
                      "")

            Riga_Requisiti("Migra '475' :",
                      "Messaggio per contatti e vasi non ancora telematizzati")

            Riga_Requisiti("AgronicaWebService2010 : 11/05/2018",
                      "Audit: gestiti campi null su domande interviste")

            Riga_Requisiti("WEB.CONFIG :",
                      "Modifiche NO. ")

            Riga_Text("AgronicaCoreAuditDAL",
                   "Aggiunto filtro visibilità imprese")

            '==================================

            Riga_Data(" 10 Maggio 2018 ")

            Riga_Requisiti("AgronicaCore '' :",
                      "")

            Riga_Requisiti("Componenti '' :",
                      "")

            Riga_Requisiti("Migra '475' :",
                      "Messaggio per contatti e vasi non ancora telematizzati")

            Riga_Requisiti("AgronicaWebService2010 : 04/05/2018",
                      "Audit gestione campi default e deroga")

            Riga_Requisiti("WEB.CONFIG :",
                      "Modifiche NO. ")

            Riga_Text("AgronicaCoreAuditBIZ",
                   "Corretto problema campi null + aggiunto filtro utente")

            '==================================

            Riga_Data(" 8 Maggio 2018 ")

            Riga_Requisiti("AgronicaCore '' :",
                      "")

            Riga_Requisiti("Componenti '' :",
                      "")

            Riga_Requisiti("Migra '475' :",
                      "Messaggio per contatti e vasi non ancora telematizzati")

            Riga_Requisiti("AgronicaWebService2010 : 04/05/2018",
                      "Audit gestione campi default e deroga")

            Riga_Requisiti("WEB.CONFIG :",
                      "Modifiche NO. ")

            Riga_Text("Operazioni.asmx, Visite.aspx",
                   "Fix di sicurezza per la regione, gestito per le visite")

            '==================================

            Riga_Data(" 4 Maggio 2018 ")

            Riga_Requisiti("AgronicaCore '' :",
                      "")

            Riga_Requisiti("Componenti '' :",
                      "")

            Riga_Requisiti("Migra '475' :",
                      "Messaggio per contatti e vasi non ancora telematizzati")

            Riga_Requisiti("AgronicaWebService2010 : 04/05/2018",
                      "Audit: ottimizzata API per codici disposizioni")

            Riga_Requisiti("WEB.CONFIG :",
                      "Modifiche NO. ")

            Riga_Text("AgronicaControlli_2010/AlberoAnagrafica2017.aspx",
                   "ora il nuovo albero mostra anche le icone")

            Riga_Text("Core campi/Programmazione_entita",
                   "fix per cui vengono azzerati i campo_cod nei planning quando si elimina un campo")

            Riga_Text("Audit.asmx",
                   "Aggiunta API ottimizzata per codici disposizioni")

            '==================================

            Riga_Data(" 27 Aprile 2018 ")

            Riga_Requisiti("AgronicaCore '' :",
                      "")

            Riga_Requisiti("Componenti '' :",
                      "")

            Riga_Requisiti("Migra '475' :",
                      "Messaggio per contatti e vasi non ancora telematizzati")

            Riga_Requisiti("AgronicaWebService2010 : 27/04/2018",
                      "Audit gestione campi default e deroga")

            Riga_Requisiti("WEB.CONFIG :",
                      "Modifiche NO. ")

            Riga_Text("Audit.asmx",
                   "Gestione campi default e deroga")

            '==================================

            Riga_Data(" 26 Aprile 2018 ")

            Riga_Requisiti("AgronicaCore '' :",
                      "")

            Riga_Requisiti("Componenti '' :",
                      "")

            Riga_Requisiti("Migra '475' :",
                      "Messaggio per contatti e vasi non ancora telematizzati")

            Riga_Requisiti("AgronicaWebService2010 : 24/04/2018",
                      "Audit + indici maturità")

            Riga_Requisiti("WEB.CONFIG :",
                      "Modifiche NO. ")

            Riga_Text("Metaschema/Rilievi.asmx, AgronicaCoreVisite/Visite.asmx",
                   "gestiti gli indici di maturità")

            '==================================

            Riga_Data(" 20 Aprile 2018 ")

            Riga_Requisiti("AgronicaCore '' :",
                      "")

            Riga_Requisiti("Componenti '' :",
                      "")

            Riga_Requisiti("Migra '475' :",
                      "Messaggio per contatti e vasi non ancora telematizzati")

            Riga_Requisiti("AgronicaWebService2010 :",
                      "Audit")

            Riga_Requisiti("WEB.CONFIG :",
                      "Modifiche NO. ")

            Riga_Text("AgronicaCoreUtentibiz/Utenti_Impostazioni_R.asmx",
                   "Gestione Atraverso Query Parametrica in risposta al security assessment di Leonardo Sistemi su RegioneER")


            Riga_Text("Metaschema/Operazioni.asmx",
                   "Rimosso parametri filtro su stringa Query in risposta al security assessment di Leonardo Sistemi su RegioneER")

            Riga_Text("Metaschema/Categorie_Magazzino.asmx",
                   " aggiunto web service per ricerca categorie di magazzino")

            Riga_Text("Audit/Audit.asmx",
                   " prima release ")

            '==================================

            Riga_Data("13 Aprile 2018")

            Riga_Requisiti("AgronicaCore '' :",
                      "")

            Riga_Requisiti("Componenti '' :",
                      "")

            Riga_Requisiti("Migra '475' :",
                      "Messaggio per contatti e vasi non ancora telematizzati")

            Riga_Requisiti("WEB.CONFIG :",
                      "Modifiche NO. ")

            Riga_Text("Metaschema/operazioni.asmx",
                   "aggiunte info sui preferiti in lista lavorazioni")

            Riga_Text("AgronicaCoreUtentiBIZ/Utenti_Impostazioni_R",
                   "Eliminato WS ridondante")

            '==================================

            Riga_Data("5 Febbraio 2018")

            Riga_Requisiti("AgronicaCore '' :",
                      "")

            Riga_Requisiti("Componenti '' :",
                      "")

            Riga_Requisiti("Migra '460' :",
                      "Messaggio per contatti e vasi non ancora telematizzati")

            Riga_Requisiti("WEB.CONFIG :",
                      "Modifiche NO. ")

            Riga_Text("Visite",
                   "WIP Visite")

            Riga_Text("",
                   "")

            '==================================

            '==================================


            Riga_Data("22 Gennaio 2018")

            Riga_Requisiti("AgronicaCore '' :",
                      "")

            Riga_Requisiti("Componenti '' :",
                      "")

            Riga_Requisiti("Migra '460' :",
                      "Messaggio per contatti e vasi non ancora telematizzati")

            Riga_Requisiti("WEB.CONFIG :",
                      "Modifiche NO. ")

            Riga_Text("Visite.asmx",
                   "Ora le operazioni d'agenda visibili sono filtrate in base alle implementazioni")

            Riga_Text("FreshAndFood.asmx",
                   "Modifiche legate alle lavorazioni F&F")

            '==================================

            Riga_Data("16 Gennaio 2018")

            Riga_Requisiti("AgronicaCore '' :",
                      "")

            Riga_Requisiti("Componenti '' :",
                      "")

            Riga_Requisiti("Migra '460' :",
                      "Messaggio per contatti e vasi non ancora telematizzati")

            Riga_Requisiti("WEB.CONFIG :",
                      "Modifiche NO. ")

            Riga_Text("Configurazione_Siti.asmx",
                   "Leggi")


            '==================================

            '==================================



            Riga_Data("05 Gennaio 2018")

            Riga_Requisiti("AgronicaCore '' :",
                      "")

            Riga_Requisiti("Componenti '' :",
                      "")

            Riga_Requisiti("Migra '460' :",
                      "Messaggio per contatti e vasi non ancora telematizzati")

            Riga_Requisiti("WEB.CONFIG :",
                      "Modifiche NO. ")

            Riga_Text("GIS+visite:",
                   "Wip")


            '==================================

            '==================================


            Riga_Data("29 Dicembre 2017")

            Riga_Requisiti("AgronicaCore '' :",
                      "")

            Riga_Requisiti("Componenti '' :",
                      "")

            Riga_Requisiti("Migra '475' :",
                      "per ???")

            Riga_Requisiti("WEB.CONFIG :",
                      "Modifiche NO. ")


            '==================================


            Riga_Data("12 Dicembre 2017")

            Riga_Requisiti("AgronicaCore '' :",
                      "")

            Riga_Requisiti("Componenti '' :",
                      "")

            Riga_Requisiti("Migra '460' :",
                      "per gestione errori Teleregistri")

            Riga_Requisiti("WEB.CONFIG :",
                      "Modifiche NO. ")

            Riga_Text("GIS+visite:",
                   "Wip")


            '==================================
            Riga_Data("04 Dicembre 2017")

            Riga_Requisiti("AgronicaCore '' :",
                      "")

            Riga_Requisiti("Componenti '' :",
                      "")

            Riga_Requisiti("Migra '475' :",
                      "")

            Riga_Requisiti("WEB.CONFIG :",
                      "Modifiche NO. ")

            Riga_Text("Teleregistri",
                      " - corretto baco su invio contatto pubblico")

            Riga_Fine()

            '==================================

            '==================================
            Riga_Data("01 Dicembre 2017 (2)")

            Riga_Requisiti("AgronicaCore '' :",
                      "")

            Riga_Requisiti("Componenti '' :",
                      "")

            Riga_Requisiti("Migra '475' :",
                      "")

            Riga_Requisiti("WEB.CONFIG :",
                      "Modifiche NO. ")

            Riga_Text("Teleregistri",
                      " - corretto baco su messaggio di ritorno da invio operazione")

            Riga_Fine()

            '==================================

            '==================================
            Riga_Data("01 Dicembre 2017")

            Riga_Requisiti("AgronicaCore '' :",
                      "")

            Riga_Requisiti("Componenti '' :",
                      "")

            Riga_Requisiti("Migra '475' :",
                      "")

            Riga_Requisiti("WEB.CONFIG :",
                      "Modifiche NO. ")

            Riga_Text("Teleregistri",
                      " - Lettura anagrafiche contatti soggetti pubblici")

            Riga_Fine()

            '==================================

            '==================================
            Riga_Data("29 Novembre 2017 (3)")

            Riga_Requisiti("AgronicaCore '' :",
                      "")

            Riga_Requisiti("Componenti '' :",
                      "")

            Riga_Requisiti("Migra '460' :",
                      "per gestione errori Teleregistri")

            Riga_Requisiti("WEB.CONFIG :",
                      "Modifiche NO. ")

            Riga_Text("Teleregistri",
                      " - Eliminato controllo su soggetto telematizzato per DB Vinificazione" & vbCrLf &
                      " - Messaggio di ritorno da invio operazione parlante")

            Riga_Fine()

            '==================================

            '==================================
            Riga_Data("29 Novembre 2017 (2)")

            Riga_Requisiti("AgronicaCore '' :",
                      "")

            Riga_Requisiti("Componenti '' :",
                      "")

            Riga_Requisiti("Migra '460' :",
                      "per gestione errori Teleregistri")

            Riga_Requisiti("WEB.CONFIG :",
                      "Modifiche NO. ")

            Riga_Text("Inserimento Operazioni su DB Vinificazione",
                      "Eliminato controllo su soggetto telematizzato")

            Riga_Fine()

            '==================================

            '==================================
            Riga_Data("29 Novembre 2017")

            Riga_Requisiti("AgronicaCore '' :",
                      "")

            Riga_Requisiti("Componenti '' :",
                      "")

            Riga_Requisiti("Migra '460' :",
                      "per gestione errori Teleregistri")

            Riga_Requisiti("WEB.CONFIG :",
                      "Modifiche NO. ")

            Riga_Text("Reg Fertilizzazioni",
                      "gestione baco per stato in produzione doppio")

            Riga_Fine()

            '==================================

            '==================================
            Riga_Data("24 Novembre 2017")

            Riga_Requisiti("AgronicaCore '' :",
                      "")

            Riga_Requisiti("Componenti '' :",
                      "")

            Riga_Requisiti("Migra '460' :",
                      "per gestione errori Teleregistri")

            Riga_Requisiti("WEB.CONFIG :",
                      "Modifiche NO. ")

            Riga_Text("DPI e Utenti Impostazioni ",
                      "Gestito l'utente CAA per Coldiretti")

            Riga_Fine()

            '==================================
            Riga_Data("21 Novembre 2017")

            Riga_Requisiti("AgronicaCore '' :",
                      "")

            Riga_Requisiti("Componenti '' :",
                      "")

            Riga_Requisiti("Migra '460' :",
                      "per gestione errori Teleregistri")

            Riga_Requisiti("WEB.CONFIG :",
                      "Modifiche NO. ")

            Riga_Text("Visite ",
                      "Aggiunti core di lettura per categorizzazioni")

            Riga_Text("Operazioni ",
                      "Aggiunte info su Gruppo Operazione in WS")

            Riga_Text("Metaschema ",
                      "Carica Combo Copertura")

            Riga_Text("Utenti ",
                      "Gestione impostazione permessi utente")

            Riga_Fine()


            '==================================
            Riga_Data("08 Novembre 2017")

            Riga_Requisiti("AgronicaCore '' :",
                      "")

            Riga_Requisiti("Componenti '' :",
                      "")

            Riga_Requisiti("Migra '460' :",
                      "per gestione errori Teleregistri")

            Riga_Requisiti("WEB.CONFIG :",
                      "Modifiche NO. ")

            Riga_Text("Impostazioni ",
                      "Modificati salvataggi preferenze disciplinari e magazzino per portale soci")

            Riga_Fine()

            '==================================
            Riga_Data("02 Novembre 2017")

            Riga_Requisiti("AgronicaCore '' :",
                      "")

            Riga_Requisiti("Componenti '' :",
                      "")

            Riga_Requisiti("Migra '460' :",
                      "per gestione errori Teleregistri")

            Riga_Requisiti("WEB.CONFIG :",
                      "Modifiche NO. ")

            Riga_Text("NC ",
                      "aggiunti core per non conformità")

            Riga_Text("Rilievi: ",
                      "lettura dati misure x avversità, DPI con incrocio su soglie")
            Riga_Fine()

            '==================================
            Riga_Data("24 Ottobre 2017")

            Riga_Requisiti("AgronicaCore '' :",
                      "")

            Riga_Requisiti("Componenti '' :",
                      "")

            Riga_Requisiti("Migra '460' :",
                      "per gestione errori Teleregistri")

            Riga_Requisiti("WEB.CONFIG :",
                      "Modifiche NO. ")

            Riga_Text("DPI.asmx:",
                      "aggiunto parametro flag_disciplinareprivato alla funzione CaricaComboDPI")

            Riga_Fine()

            '==================================

            Riga_Data("17 Ottobre 2017")

            Riga_Requisiti("AgronicaCore '' :",
                      "")

            Riga_Requisiti("Componenti '' :",
                      "")

            Riga_Requisiti("Migra '460' :",
                      "per gestione errori Teleregistri")

            Riga_Requisiti("WEB.CONFIG :",
                      "Modifiche SI/NO. ")

            Riga_Text("DPI.asmx:",
                      "Creato WS per tipo conduzione")

            Riga_Fine()

            '==================================

            Riga_Data("10 Ottobre 2017 ")

            Riga_Requisiti("AgronicaCore '' :",
                      "")

            Riga_Requisiti("Componenti '' :",
                      "")

            Riga_Requisiti("Migra '460' :",
                      "per gestione errori Teleregistri")

            Riga_Text("core vari:",
                      "aggiunti core per filtro menu agenda")

            Riga_Fine()

            '==================================

            Riga_Data("3 Ottobre 2017 ")

            Riga_Requisiti("AgronicaCore '' :",
                      "")

            Riga_Requisiti("Componenti '' :",
                      "")

            Riga_Requisiti("Migra '460' :",
                      "per gestione errori Teleregistri")

            Riga_Text("AgronicaCoreDPI/DPI.asmx/CaricaComboDPI:",
                      "aggiunta")

            Riga_Fine()

            '==================================

            Riga_Data("21 Settembre 2017 ")

            Riga_Requisiti("AgronicaCore '' :",
                      "")

            Riga_Requisiti("Componenti '' :",
                      "")

            Riga_Requisiti("Migra '460' :",
                      "per gestione errori Teleregistri")

            Riga_Text("Metaschema:",
                      "Aggiunti WebService per SpecieVegetali, Cultivar e Finalita")

            Riga_Fine()

            '==================================

            Riga_Data("20 Settembre 2017 ")

            Riga_Requisiti("AgronicaCore '' :",
                      "")

            Riga_Requisiti("Componenti '' :",
                      "")

            Riga_Requisiti("Migra '460' :",
                      "per gestione errori Teleregistri")

            Riga_Text("Teleregistri:",
                      "Migliorata gestione errori")

            Riga_Fine()


            '==================================

            Riga_Data("19 Settembre 2017 ")

            Riga_Requisiti("AgronicaCore '' :",
                      "")

            Riga_Requisiti("Componenti '' :",
                      "")

            Riga_Requisiti("Migra '457' :",
                      "Per Visite Ispettive")

            Riga_Text("Varie:",
                      "Core per PC WIP")

            Riga_Fine()

            '==================================

            Riga_Data("14 Settembre 2017 ")

            Riga_Requisiti("AgronicaCore '' :",
                      "")

            Riga_Requisiti("Componenti '' :",
                      "")

            Riga_Requisiti("Migra '457' :",
                      "Per Visite Ispettive")

            Riga_Text("RegistriTelematici.asmx:",
                      "Modifica per impostare ID Operazione")

            Riga_Fine()

            '==================================

            '==================================

            Riga_Data("12 Settembre 2017 ")

            Riga_Requisiti("AgronicaCore '' :",
                      "")

            Riga_Requisiti("Componenti '' :",
                      "")

            Riga_Requisiti("Migra '457' :",
                      "Per Visite Ispettive")

            Riga_Requisiti("WEB.CONFIG :",
                      "Modifiche SI. Parametri per nuova funzionalità Warm Up Entity Framework lanciata su Application_Start ")

            Riga_Text("RegistriTelematici.asmx:",
                      "Risolto baco su lettura anagrafiche Vasi")

            Riga_Text("Warm Up EF su Application_Start:",
                      "per maggiori info vedere il file 'Aggiornamento modello e utilizzo classi POCO.docx' nel progetto AgronicaCoreEntityFramework" &
                      "<br> ")

            Riga_Fine()

            '==================================

            Riga_Data(" 08 Settembre 2017 ")

            Riga_Requisiti("AgronicaCore '' :",
                      "")

            Riga_Requisiti("Componenti '' :",
                      "")

            Riga_Requisiti("Migra '457' :",
                      "Per Visite Ispettive")

            Riga_Requisiti("WEB.CONFIG :",
                      "Modifiche NO. ")

            Riga_Text("AgronicaCoreDPI/DPI.asmx/CaricaComboDPI_Regioni:",
                      "Gestiti miglioramenti")

            Riga_Text(" :",
                      "" &
                      "<br> ")

            Riga_Fine()


            '==================================

            Riga_Data("22 Agosto 2017 ")

            Riga_Requisiti("AgronicaCore '' :",
                      "")

            Riga_Requisiti("Componenti '' :",
                      "")

            Riga_Requisiti("Migra '457' :",
                      "Per Visite Ispettive")

            Riga_Requisiti("WEB.CONFIG :",
                      "Modifiche NO. ")

            Riga_Text("CentroAziendale.asmx e Reg_Impianto.asmx:",
                      "Aggiunti WebService per Centri aziendali ed impianti")

            Riga_Text("CentroAziendale.asmx e Reg_Impianto.asmx:",
                      "Aggiunti WebService per Anagrafiche Visite Ispettive")

            Riga_Text(" :",
                      "" &
                      "<br> ")

            Riga_Fine()

            '==================================

            Riga_Data("16 Agosto 2017 ")

            Riga_Requisiti("AgronicaCore '' :",
                      "")

            Riga_Requisiti("Componenti '' :",
                      "")

            Riga_Requisiti("Migra '455' :",
                      "")

            Riga_Requisiti("WEB.CONFIG :",
                      "Modifiche NO. ")

            Riga_Text("Teleregistri:",
                      "Cambia Richiesta - Inserito check per ID=0")

            Riga_Text("Teleregistri:",
                      "Leggi Operazioni - non visualizzo più operazioni errate in GIAS nella maschera SIAN")

            Riga_Text(" :",
                      "" &
                      "<br> ")

            Riga_Fine()

            '==================================

            '==================================

            Riga_Data("14 Agosto 2017 ")

            Riga_Requisiti("AgronicaCore '' :",
                      "")

            Riga_Requisiti("Componenti '' :",
                      "")

            Riga_Requisiti("Migra '455' :",
                      "")

            Riga_Requisiti("WEB.CONFIG :",
                      "Modifiche NO. ")

            Riga_Text("Teleregistri:",
                      "Soggetti - Indirizzo corretto Impresa gias da telematizzare")

            Riga_Text(" :",
                      "" &
                      "<br> ")

            Riga_Fine()

            '==================================

            '==================================

            Riga_Data("08 Agosto 2017 ")

            Riga_Requisiti("AgronicaCore '' :",
                      "")

            Riga_Requisiti("Componenti '' :",
                      "")

            Riga_Requisiti("Migra '455' :",
                      "")

            Riga_Requisiti("WEB.CONFIG :",
                      "Modifiche NO. ")

            Riga_Text("Alert_Area:",
                      "Corretto baco in scadenzario")

            Riga_Text(" :",
                      "" &
                      "<br> ")

            Riga_Fine()

            '==================================

            Riga_Data("07 Agosto 2017 ")

            Riga_Requisiti("AgronicaCore '' :",
                      "")

            Riga_Requisiti("Componenti '' :",
                      "")

            Riga_Requisiti("Migra '455' :",
                      "")

            Riga_Requisiti("WEB.CONFIG :",
                      "Modifiche NO. ")

            Riga_Text("CoreWS:",
                      "Aggiunta gestione Scadenzario")

            Riga_Text(" :",
                      "" &
                      "<br> ")

            Riga_Fine()

            '==================================

            Riga_Data("24 Luglio 2017 (2)")

            Riga_Requisiti("AgronicaCore '' :",
                      "")

            Riga_Requisiti("Componenti '' :",
                      "")

            Riga_Requisiti("Migra '442' :",
                      "")

            Riga_Requisiti("WEB.CONFIG :",
                      "Modifiche NO. ")

            Riga_Text("CoreWS:",
                      "Teleregistri: Soggetti - Concatenazione Cognome e Nome per Associazioni")

            Riga_Text(" :",
                      "" &
                      "<br> ")

            Riga_Fine()

            '==================================

            '==================================

            Riga_Data("24 Luglio 2017 ")

            Riga_Requisiti("AgronicaCore '' :",
                      "")

            Riga_Requisiti("Componenti '' :",
                      "")

            Riga_Requisiti("Migra '442' :",
                      "")

            Riga_Requisiti("WEB.CONFIG :",
                      "Modifiche NO. ")

            Riga_Text("CoreWS:",
                      "Teleregistri: Dimensione Colonne Ragione Sociale (nei soggetti) e Descrizione (nelle operazioni)")

            Riga_Text("CoreWS:",
                      "Teleregistri: Soggetti - Utilizzo del Tipo_Indirizzo_Default come indirizzo da inviare")

            Riga_Text(" :",
                      "" &
                      "<br> ")

            Riga_Fine()

            '==================================

            '==================================

            Riga_Data("07 Luglio 2017 ")

            Riga_Requisiti("AgronicaCore '' :",
                      "")

            Riga_Requisiti("Componenti '' :",
                      "")

            Riga_Requisiti("Migra '442' :",
                      "")

            Riga_Requisiti("WEB.CONFIG :",
                      "Modifiche NO. ")

            Riga_Text("CoreWS:",
                      "Teleregistri: Controllo su Valori Soggetti")

            Riga_Text(" :",
                      "" &
                      "<br> ")

            Riga_Fine()

            '==================================

            Riga_Data("05 Giugno 2017 ")

            Riga_Requisiti("AgronicaCore '' :",
                      "")

            Riga_Requisiti("Componenti '' :",
                      "")

            Riga_Requisiti("Migra '442' :",
                      "")

            Riga_Requisiti("WEB.CONFIG :",
                      "Modifiche SI/NO. ")

            Riga_Text("CoreWS:",
                      "Teleregistri: Ordinamento operazioni")

            Riga_Text(" :",
                      "" &
                      "<br> ")

            Riga_Fine()

            '==================================

            Riga_Data("05 Giugno 2017 ")

            Riga_Requisiti("AgronicaCore '' :",
                      "")

            Riga_Requisiti("Componenti '' :",
                      "")

            Riga_Requisiti("Migra '442' :",
                      "")

            Riga_Requisiti("WEB.CONFIG :",
                      "Modifiche SI/NO. ")

            Riga_Text("CoreWS:",
                      "Teleregistri: Visibilità contatto quando si invia una operazione che lo coinvolge")

            Riga_Text(" :",
                      "" &
                      "<br> ")

            Riga_Fine()

            '==================================


            Riga_Data("08 Maggio 2017 ")

            Riga_Requisiti("AgronicaCore '' :",
                      "")

            Riga_Requisiti("Componenti '' :",
                      "")

            Riga_Requisiti("Migra '442' :",
                      "Personalizzazioni griglia Kendo")

            Riga_Requisiti("WEB.CONFIG :",
                      "Modifiche SI/NO. ")

            Riga_Text("CoreWS:",
                      "Aggiunti Core WS per Operazioni agenda + impostazioni utente")

            Riga_Text(" :",
                      "" &
                      "<br> ")

            Riga_Fine()

            '==================================


            Riga_Data("21 Marzo 2017 ")

            Riga_Requisiti("AgronicaCore '' :",
                      "")

            Riga_Requisiti("Componenti '' :",
                      "")

            Riga_Requisiti("Migra '435' :",
                      "Aggiunta Tabella Risorse_Umane_Classificazioni")

            Riga_Requisiti("WEB.CONFIG :",
                      "Modifiche SI/NO. ")

            Riga_Text("Contab/Risorse_Umane_Classificazioni:",
                      "Aggiunto Core WS per uso sul LAN")

            Riga_Text(" :",
                      "" &
                      "<br> ")

            Riga_Fine()

            '==================================

            Riga_Data("03 Giugno 2016 ")

            Riga_Requisiti("AgronicaCore '' :",
                      "")

            Riga_Requisiti("Componenti '' :",
                      "")

            Riga_Requisiti("Migra '394' :",
                      "")

            Riga_Requisiti("WEB.CONFIG :",
                      "Modifiche SI/NO. ")

            Riga_Text("Funzioni per riporto saldi contabili in storicizzazione archivio .",
                      "")

            Riga_Text(" :",
                      "" &
                      "<br> ")

            Riga_Fine()

            '==================================


            '==================================

            Riga_Data("14 Aprile 2016 ")

            Riga_Requisiti("AgronicaCore '' :",
                      "")

            Riga_Requisiti("Componenti '' :",
                      "")

            Riga_Requisiti("Migra '394' :",
                      "")

            Riga_Requisiti("WEB.CONFIG :",
                      "Modifiche SI/NO. ")

            Riga_Text("Ricette.asmx :",
                      "Aggiunta core ricette come Web Service")

            Riga_Text(" :",
                      "" &
                      "<br> ")

            Riga_Fine()

            '==================================

            Riga_Data("22 Marzo 2016")

            Riga_Requisiti("AgronicaCore '' :",
                      "")

            Riga_Requisiti("Componenti '' :",
                      "")

            Riga_Requisiti("Migra '387'",
                      "")

            Riga_Requisiti("WEB.CONFIG :",
                      "Modifiche NO. ")

            Riga_Text("Contab/Cespiti :",
                      "Primo rilascio a clienti gestione cespiti")

            Riga_Text(" :",
                      "" &
                      "<br> ")

            Riga_Fine()

            '==================================

            Riga_Data("19 Gennaio 2016")

            Riga_Requisiti("AgronicaCore '' :",
                      "")

            Riga_Requisiti("Componenti '' :",
                      "")

            Riga_Requisiti("Migra '386'",
                      "")

            Riga_Requisiti("WEB.CONFIG :",
                      "Modifiche NO. ")

            Riga_Text("Contab/Cespiti :",
                      "Prima versione gestione cespiti")

            Riga_Text(" :",
                      "" &
                      "<br> ")

            Riga_Fine()

            '==================================

            Riga_Data("01 Settembre 2015")

            Riga_Requisiti("AgronicaCore '' :",
                      "")

            Riga_Requisiti("Componenti '' :",
                      "")

            Riga_Requisiti("Migra '378'",
                      "")

            Riga_Requisiti("WEB.CONFIG :",
                      "Modifiche NO. ")

            Riga_Text("AgronicaCoreUtentiBIZ/Utenti_Permessi_R.asmx :",
                      "Baseline")

            Riga_Text(" :",
                      "" &
                      "<br> ")

            Riga_Fine()

            '==================================

        End Sub


        '#################################################################################################
        Private Sub Riga_Requisiti(ByVal Titolo As String, ByVal Testo As String, Optional ByVal Ver As String = "")

            If _objChangeSito Is Nothing Then
                Dim Riga As New HtmlTableRow

                Riga.Cells.Add(New HtmlTableCell)

                Riga.Cells(0).BgColor = "#FFFFC0"
                Riga.Cells(0).Height = "20px"
                Riga.Cells(0).Attributes.Add("class", "Testo_08_Rosso")

                Riga.Cells(0).InnerHtml = "<b>" & Titolo & If(Not String.IsNullOrEmpty(Ver), " [" & Ver & "]", "") & "</b><br>" & Testo

                Me.TabellaVersione.Rows.Add(Riga)
            Else
                _objChangeSito.versioni.Last().requisiti.Add(New Requisito_Obj(Titolo, Testo, Ver))
            End If
        End Sub


        '#################################################################################################
        Private Sub Riga_Changelog(ByVal tipoChangelog As enum_Tipo_Changelog,
                                   ByVal area As String,
                                   ByVal descrizione As String,
                                   ByVal cliente As String,
                                   ByVal idTicketAssistenza As Integer,
                                   ByVal idTicketSviluppo As Integer,
                                   ByVal idTicketTesting As Integer,
                                   ByVal autore As String,
                                   Optional ByVal noteTecniche As String = "",
                                   Optional ByVal noteTest As String = ""
                                   )

            If _objChangeSito IsNot Nothing Then
                _objChangeSito.Riga_Changelog(tipoChangelog,
                                              area, descrizione,
                                              cliente,
                                              idTicketAssistenza, idTicketSviluppo, idTicketTesting,
                                              autore,
                                              noteTecniche, noteTest)
            End If

        End Sub

        '#################################################################################################
        <Obsolete("Usare la Funzione Riga_Changelog() con primo parametro = enum_Tipo_Changelog.Feature")>
        Private Sub Riga_Text(ByVal Titolo As String,
                              ByVal Testo As String,
                              Optional ByVal cliente As String = "",
                              Optional ByVal idPerforma As Integer = 0,
                              Optional ByVal noteTecniche As String = "",
                              Optional ByVal noteTest As String = ""
                              )

            If _objChangeSito Is Nothing Then
                Dim Riga As New HtmlTableRow

                Riga.Cells.Add(New HtmlTableCell)

                Riga.Cells(0).BgColor = "#c0ffc0"
                Riga.Cells(0).Height = "20px"
                Riga.Cells(0).Attributes.Add("class", "Testo_08_Nero")

                Riga.Cells(0).InnerHtml = "<b>" & Titolo & "</b><br>" & Testo

                Me.TabellaVersione.Rows.Add(Riga)
            Else
                _objChangeSito.versioni.Last().changelog.feature.Add(New Feature_Obj(True, Titolo, Testo, cliente, idPerforma, noteTecniche, noteTest))
            End If
        End Sub


        '#################################################################################################
        <Obsolete("Usare la Funzione Riga_Changelog() con primo parametro = enum_Tipo_Changelog.Bug")>
        Private Sub Riga_Bug(ByVal Titolo As String, ByVal Testo As String,
                             Optional ByVal cliente As String = "",
                             Optional ByVal idPerforma As Integer = 0,
                             Optional ByVal noteTecniche As String = "",
                             Optional ByVal noteTest As String = ""
                             )

            If _objChangeSito Is Nothing Then
                Dim Riga As New HtmlTableRow

                Riga.Cells.Add(New HtmlTableCell)

                Riga.Cells(0).BgColor = "#c0ffc0"
                Riga.Cells(0).Height = "20px"
                Riga.Cells(0).Attributes.Add("class", "Testo_08_Nero")

                Riga.Cells(0).InnerHtml = "<b>" & Titolo & "</b><br>" & Testo

                Me.TabellaVersione.Rows.Add(Riga)
            Else
                _objChangeSito.versioni.Last().changelog.bugfix.Add(New Bugfix_Obj(True, Titolo, Testo, cliente, idPerforma, noteTecniche, noteTest))
            End If

        End Sub



        '#################################################################################################
        Private Sub Riga_Fine()

            If _objChangeSito Is Nothing Then
                Dim Riga As New HtmlTableRow

                Riga.Cells.Add(New HtmlTableCell)

                Riga.Cells(0).BgColor = "whitesmoke"
                Riga.Cells(0).Height = "20px"
                Riga.Cells(0).Attributes.Add("class", "Testo_08_Nero")

                Riga.Cells(0).InnerHtml = "&nbsp;"

                Me.TabellaVersione.Rows.Add(Riga)
            End If

        End Sub

        '#################################################################################################
        Private Sub Riga_Versione(ByVal data As String, Optional ByVal versione As String = "")

            If _objChangeSito IsNot Nothing Then
                _objChangeSito.versioni.Add(New Versione_Obj(data, versione))
            End If

        End Sub

        '#################################################################################################
        <Obsolete("Usare la funzione Riga_Versione()")>
        Private Sub Riga_Data(ByVal DataAggiornamento As String)

            If _objChangeSito Is Nothing Then
                Dim Riga As New HtmlTableRow

                Riga.Cells.Add(New HtmlTableCell)

                Riga.Cells(0).BgColor = "#00bfff"
                Riga.Cells(0).Height = "25px"
                Riga.Cells(0).Attributes.Add("class", "Testo_08_Nero_Bold")
                Riga.Cells(0).InnerText = DataAggiornamento

                Me.TabellaVersione.Rows.Add(Riga)
            Else
                'Inizializzo tutto qui, visto che sono certa che sia il primo elemento del blocco
                _objChangeSito.versioni.Add(New Versione_Obj(DataAggiornamento))
            End If

        End Sub

    End Class
End Namespace