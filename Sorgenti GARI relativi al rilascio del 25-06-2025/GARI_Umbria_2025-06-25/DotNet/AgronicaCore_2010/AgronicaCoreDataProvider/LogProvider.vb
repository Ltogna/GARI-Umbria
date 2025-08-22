Imports System.Configuration
Imports System.IO
Imports System.Threading
Imports AgronicaCoreDataProvider.TipiEnumerativi
Imports Newtonsoft.Json

<Serializable()>
Public Class DbLogClass
    Public Username As String
    Public Errore As String
    Public Data As DateTime
    Public db As String
    Public istanza As String
    Public routine As String
    Public occorrenze As Integer
    Public hash As String

End Class

Public Class LogProvider

    '##############################################################################################
    <Obsolete("1.Da utilizzare quella con objParametri", True)>
    Public Sub Scrivi_LOG(
                        ByVal DirectoryLOG As String,
                        ByVal FileLOG As String,
                        ByVal IdentificatoreUtente As String,
                        ByVal NomeRoutine As String,
                        ByVal MessaggioErrore As String)

        'Dim DefaultDirectoryLOG As String = "C:\GIASLAN\Log"
        Dim DefaultDirectoryLOG As String = Path.GetTempPath()
        Dim DefaultFileLOG As String = "AgronicaCoreLOG.txt"

        Dim NomeCompletoFileLOG As String = ""
        Dim xStreamWriter As System.IO.StreamWriter = Nothing
        Dim Testo As String = ""

        Try

            'Verifico se e' stata indicata una directory di LOG
            If DirectoryLOG = "" Then
                DirectoryLOG = DefaultDirectoryLOG
            End If

            'Verifico se e' stato indicato un file di LOG
            If FileLOG = "" Then
                FileLOG = DefaultFileLOG
            End If

            'Verifico se esiste la DIRECTORY di LOG indicata ... altrimenti la creo
            If System.IO.Directory.Exists(DirectoryLOG) = False Then
                System.IO.Directory.CreateDirectory(DirectoryLOG)
            End If

            If Not DirectoryLOG.EndsWith("\") Then
                DirectoryLOG &= "\"
            End If

            'Costruisco il nome completo del file di LOG
            ' NomeCompletoFileLOG = (DirectoryLOG & "\" & FileLOG).Replace("\\", "\")
            NomeCompletoFileLOG = (DirectoryLOG & FileLOG).Replace("\\", "\")

            ReplaceInvalidChars(FileLOG, NomeCompletoFileLOG)

            'Verifico se esiste il FILE di LOG indicato ... altrimenti lo creo
            If System.IO.File.Exists(NomeCompletoFileLOG) = False Then

                'Creo il file di LOG nuovo
                xStreamWriter = System.IO.File.CreateText(NomeCompletoFileLOG)
                xStreamWriter.WriteLine("Inizializzazione file di LOG ... " &
                                    Date.Now.ToShortDateString & " " &
                                    Date.Now.ToLongTimeString)
                xStreamWriter.Flush()
                xStreamWriter.Close()

            End If

            'Costruisco la stringa di testo da scrivere
            Testo = Date.Now.ToShortDateString &
                    " " &
                    Date.Now.ToLongTimeString &
                    " {" &
                    IdentificatoreUtente &
                    "} : [" &
                    NomeRoutine &
                    "] : " &
                    MessaggioErrore


            'Apro il file di LOG
            xStreamWriter = System.IO.File.AppendText(NomeCompletoFileLOG)

            'Scrivo la stringa
            xStreamWriter.WriteLine(Testo)
            xStreamWriter.Flush()
            'xStreamWriter.Close()

        Catch ex As Exception

            Throw New Exception("LOG : " & ex.Message)

        Finally

            If Not IsNothing(xStreamWriter) Then
                xStreamWriter.Close()
            End If

        End Try

    End Sub


    '##############################################################################################
    ''' <param name="objParametri"></param>
    ''' <param name="NomeRoutine"></param>
    ''' <param name="MessaggioErrore"></param>
    ''' <param name="verificaInviaElasticSearch">
    '''     Default a true, ma potrebbe essere chiamata dalle esegui_query, dove nei casi di errore del parametrizzatore non vogliamo mandare ad ES
    ''' </param>
    ''' <param name="CustomLOGParams">
    '''     Aggiunta per l'allineamento delle chiamate alla Scrivi_LOG, passando sempre per questa versione con l'objParametri.
    '''     Così siamo in grado di mantenere invariato il funzionamento ed accettare valori imposti dal chiamante come in precedenza, possibilmente diversi da quelli contenuti nella objParametri. 
    ''' </param> 
    Public Sub Scrivi_LOG(ByRef objParametri As AgronicaCoreParametri,
                          ByVal NomeRoutine As String,
                          ByVal MessaggioErrore As String,
                            Optional verificaInviaElasticSearch As Boolean = True,
                            Optional CustomLOGParams As CustomLOGParams = Nothing)

        Dim DefaultDirectoryLOG As String = Path.GetTempPath()
        Dim DefaultFileLOG As String = "AgronicaCoreLOG.txt"

        Dim NomeCompletoFileLOG As String = ""
        Dim xStreamWriter As System.IO.StreamWriter = Nothing
        Dim Testo As String = ""

        Dim CUAA As String = ""
        Dim timeStamp As Date = Date.Now()

        Dim LogDirectory As String = DefaultDirectoryLOG
        Dim LogFileName As String = DefaultFileLOG
        Dim LogDescrizioneUtente As String = ""

        Try
            'Vado a ricavare il CUAA dell'ultima azienda selezionata
            If objParametri IsNot Nothing Then
                CUAA = LeggiCUAA(objParametri)
            End If

            If CustomLOGParams IsNot Nothing Then

                'Allineamento chiamate alla scrivi_log con objParametri
                'Per mantenere invariato il funzionamento aggiunto CustomLOGParams
                If Not String.IsNullOrEmpty(CustomLOGParams.LogDirectory) Then
                    LogDirectory = CustomLOGParams.LogDirectory
                End If

                If Not String.IsNullOrEmpty(CustomLOGParams.LogFileName) Then
                    LogFileName = CustomLOGParams.LogFileName
                End If

                If Not String.IsNullOrEmpty(CustomLOGParams.LogDescrizioneUtente) Then
                    LogDescrizioneUtente = CustomLOGParams.LogDescrizioneUtente
                End If

            ElseIf objParametri IsNot Nothing Then

                'Verifico se e' stata indicata una directory di LOG
                If objParametri.LogDirectory = "" Then
                    objParametri.LogDirectory = DefaultDirectoryLOG
                End If
                LogDirectory = objParametri.LogDirectory

                'Verifico se e' stato indicato un file di LOG
                If objParametri.LogFileName = "" Then
                    objParametri.LogFileName = DefaultFileLOG
                End If
                LogFileName = objParametri.LogFileName
                LogDescrizioneUtente = objParametri.LogDescrizioneUtente
            End If

            Dim LogxThread = False
            If Not IsNothing(ConfigurationManager.AppSettings("LogxThread")) AndAlso Not String.IsNullOrEmpty(ConfigurationManager.AppSettings("LogxThread").ToString()) Then
                If ConfigurationManager.AppSettings("LogxThread").ToString = "True" Then
                    LogxThread = True
                End If
            End If

            If LogxThread Then
                Dim idThread = System.Threading.Thread.CurrentThread.ManagedThreadId
                LogFileName = idThread.ToString() & "_" & LogFileName
            End If

            'Verifico se esiste la DIRECTORY di LOG indicata ... altrimenti la creo
            If System.IO.Directory.Exists(LogDirectory) = False Then
                System.IO.Directory.CreateDirectory(LogDirectory)
            End If

            'Costruisco il nome completo del file di LOG
            NomeCompletoFileLOG = (LogDirectory & "\" & LogFileName).Replace("\\", "\")

            'Verifico se esiste il FILE di LOG indicato ... altrimenti lo creo
            'Monitor.Enter(timeStamp)
            If System.IO.File.Exists(NomeCompletoFileLOG) = False Then

                'Creo il file di LOG nuovo
                xStreamWriter = System.IO.File.CreateText(NomeCompletoFileLOG)
                xStreamWriter.WriteLine("Inizializzazione file di LOG ... " & Date.Now.ToShortDateString & " " & Date.Now.ToLongTimeString)
                xStreamWriter.Flush()
                xStreamWriter.Close()

            End If

            'Costruisco la stringa di testo da scrivere
            'Lavez 28/05/2025 -  ho bisogno di segnare anche i millisecondi.....
            'Testo = Date.Now.ToShortDateString & " " & Date.Now.ToLongTimeString &
            Testo = Date.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") &
                    " {" & LogDescrizioneUtente & "} " &
                    If(CUAA <> "", "CUAA " & CUAA, "") & " : " &
                    " [" & NomeRoutine & "] : " &
                    MessaggioErrore

            'Apro il file di LOG
            xStreamWriter = System.IO.File.AppendText(NomeCompletoFileLOG)

            'Scrivo la stringa
            xStreamWriter.WriteLine(Testo)
            xStreamWriter.WriteLine(vbCrLf & "=================================================================================================" & vbCrLf)
            xStreamWriter.Flush()
            'xStreamWriter.Close()

            If verificaInviaElasticSearch AndAlso
                objParametri IsNot Nothing Then
                VerificaInviaElasticSearchLogger(CUAA, NomeRoutine, Testo, timeStamp, objParametri)
            End If

        Catch ex As Exception

            Throw New Exception("LOG : " & ex.Message)

        Finally

            If Not IsNothing(xStreamWriter) Then
                xStreamWriter.Close()
            End If
            'Monitor.Exit(timeStamp)
        End Try

    End Sub


    '################################################################################

    Public Sub Gestione_LogErrori(ByRef objParametri_Server As AgronicaCoreDataProvider.AgronicaCoreParametri,
                                         ByVal Sottocartella As String,
                                         ByVal NomeFile_ConEstensione As String,
                                         ByVal IdentUtente As String,
                                         ByVal NomeRoutine As String,
                                         ByVal Log_Errori As String)

        Dim Path_Errore As String

        Dim objLog As New AgronicaCoreDataProvider.LogProvider

        Sottocartella = Replace(Sottocartella, "\", "-")
        Sottocartella = Replace(Sottocartella, "/", "-")

        NomeFile_ConEstensione = Replace(NomeFile_ConEstensione, "\", "-")
        NomeFile_ConEstensione = Replace(NomeFile_ConEstensione, "/", "-")
        NomeFile_ConEstensione = Replace(NomeFile_ConEstensione, """", "")

        If objParametri_Server.LogDirectory <> "" Then
            If Not objParametri_Server.LogDirectory.EndsWith("\") Then
                objParametri_Server.LogDirectory &= "\"
            End If
            Path_Errore = objParametri_Server.LogDirectory & Sottocartella
        Else
            If System.IO.Directory.Exists("C:\GIASLAN\Log") = True Then
                Path_Errore = "C:\GIASLAN\Log\" & Sottocartella
            ElseIf System.IO.Directory.Exists("C:\Agronica_LOG") = True Then
                Path_Errore = "C:\Agronica_LOG\" & Sottocartella
            Else
                Path_Errore = Path.GetTempPath()
                If Not Path_Errore.EndsWith("\") Then
                    Path_Errore &= "\"
                End If
                Path_Errore &= Sottocartella
            End If
        End If

        If Not Path_Errore.EndsWith("\") Then
            Path_Errore &= "\"
        End If

        If Path_Errore.Length > 259 Then
            Throw New Exception("Superata la lunghezza tra path e nome del file: " & CStr(Path_Errore.Length) & "caratteri (max 259 caratteri).")
        End If

        Dim customLOGParams As New CustomLOGParams With {
                .LogDescrizioneUtente = IdentUtente,
                .LogDirectory = Path_Errore,
                .LogFileName = NomeFile_ConEstensione
            }

        objLog.Scrivi_LOG(objParametri_Server, NomeRoutine, Log_Errori, CustomLOGParams:=customLOGParams)

    End Sub

    Protected Function PopolaDizionarioTabelleInLingua(ByVal CodiceISO As String) As Dictionary(Of String, String)
        Dim tab_da_tradurre As New Dictionary(Of String, String) From {
            {"Analisi_Tipi", "[Analisi_Tipi_XLingue_" & CodiceISO & "]"},
            {"Avversita", "[Avversita_XLingue_" & CodiceISO & "]"},
            {"CategorieMagazzino", "[CategorieMagazzino_XLingue_" & CodiceISO & "]"},
            {"ClassificazioniFormulati", "[ClassificazioniFormulati_XLingue_" & CodiceISO & "]"},
            {"Cultivar", "[Cultivar_XLingue_" & CodiceISO & "]"},
            {"Epoche", "[Epoche_XLingue_" & CodiceISO & "]"},
            {"Fabbricati_Tipi", "[Fabbricati_Tipi_XLingue_" & CodiceISO & "]"},
            {"FormeAllevamento", "[FormeAllevamento_XLingue_" & CodiceISO & "]"},
            {"GruppoAvversita", "[GruppoAvversita_XLingue_" & CodiceISO & "]"},
            {"GruppoFinalita", "[GruppoFinalita_XLingue_" & CodiceISO & "]"},
            {"GruppoOperazioni", "[GruppoOperazioni_XLingua_" & CodiceISO & "]"},
            {"GruppoVarietale", "[GruppoVarietale_XLingue_" & CodiceISO & "]"},
            {"GruppoVegetale", "[GruppoVegetale_XLingue_" & CodiceISO & "]"},
            {"ImpiantiIrrigazioni", "[ImpiantiIrrigazioni_XLingue_" & CodiceISO & "]"},
            {"Macchine", "[Macchine_XLingue_" & CodiceISO & "]"},
            {"Materie_Prime", "[Materie_Prime_XLingue_" & CodiceISO & "]"},
            {"MenuBS_2017_Sezioni", "[MenuBS_2017_Sezioni_XLingua_" & CodiceISO & "]"},
            {"Note_Intervento", "[Note_Intervento_XLingue_" & CodiceISO & "]"},
            {"Note_Intervento_Gruppi", "[Note_Intervento_Gruppi_XLingue_" & CodiceISO & "]"},
            {"Note_Intervento_Utilizzo", "[Note_Intervento_Utilizzo_XLingue_" & CodiceISO & "]"},
            {"Operazioni", "[Operazioni_XLingue_" & CodiceISO & "]"},
            {"Portinnesti", "[Portinnesti_XLingue_" & CodiceISO & "]"},
            {"Prenotazione_Piante_Categoria", "[Prenotazione_Piante_Categoria_XLingue_" & CodiceISO & "]"},
            {"Prenotazione_Piante_Certificazione", "[Prenotazione_Piante_Certificazione_XLingue_" & CodiceISO & "]"},
            {"Rapporti_Contabili", "[Rapporti_Contabili_XLingue_" & CodiceISO & "]"},
            {"SpecieVegetali", "[SpecieVegetali_XLingue_" & CodiceISO & "]"},
            {"SpecieVegetaliXStadiCrescita", "[StadiXSpecieVegetali_XLingua_" & CodiceISO & "]"},
            {"StampeReport", "[StampeReport_XLingue_" & CodiceISO & "]"},
            {"Tipologie", "[Tipologie_XLingue_" & CodiceISO & "]"},
            {"TipologieSementi", "[TipologieSementi_XLingue_" & CodiceISO & "]"},
            {"UnitaMisura", "[UnitaMisura_XLingue_" & CodiceISO & "]"},
            {"Modalita_Applicazione_Globali", "[Modalita_Applicazione_Globali_XLingue_" & CodiceISO & "]"},
            {"Modalita_Applicazione", "[Modalita_Applicazione_XLingue_" & CodiceISO & "]"},
            {"Lista_Categorie_Animali", "[Lista_Categorie_Animali_XLingue_" & CodiceISO & "]"},
            {"Lista_Causali_Morte", "[Lista_Causali_Morte_XLingue_" & CodiceISO & "]"},
            {"Lista_Generi_Animali", "[Lista_Generi_Animali_XLingue_" & CodiceISO & "]"},
            {"Lista_IndirizziProd_Animali", "[Lista_IndirizziProd_Animali_XLingue_" & CodiceISO & "]"},
            {"Lista_Patologie", "[Lista_Patologie_XLingue_" & CodiceISO & "]"},
            {"Lista_Razze_Animali", "[Lista_Razze_Animali_XLingue_" & CodiceISO & "]"},
            {"Lista_Specie_Animali", "[Lista_Specie_Animali_XLingue_" & CodiceISO & "]"},
            {"Lista_Tipi_Raggruppamento_Stalla", "[Lista_Tipi_Raggruppamento_Stalla_XLingue_" & CodiceISO & "]"}
        }

        Return tab_da_tradurre

    End Function

    Private Shared Sub ReplaceInvalidChars(FileLOG As String, ByRef NomeCompletoFileLOG As String)
        '  Giulia, 04/11/2016 12.00.43: Migliore gestione errore di creazione file di log,
        '                   perché il nome file (spesso contenente la ragione sociale) contiene caratteri invalidi
        Dim InvalidName As Char() = System.IO.Path.GetInvalidFileNameChars
        'Dim InvalidPath As Char() = System.IO.Path.GetInvalidPathChars
        Dim InvalidCharFound As New Hashtable
        For Each CharInvalid In InvalidName
            If FileLOG.Contains(CharInvalid) Then
                InvalidCharFound.Add(FileLOG.IndexOf(CharInvalid), CharInvalid)
            End If
        Next
        If InvalidCharFound.Count <> 0 Then
            Dim StringInvaliChar As String = ""
            For Each pair In InvalidCharFound.Keys
                StringInvaliChar = StringInvaliChar & "Carattere: " & InvalidCharFound.Item(pair).ToString &
                                   " [ASCII code " & Asc(CChar(InvalidCharFound.Item(pair))) & "] - in posizione [" & CInt(pair) + 1 & "]" & vbCrLf
                'Tenta la sostituzione
                NomeCompletoFileLOG.Replace(CStr(InvalidCharFound.Item(pair)), "-")
            Next
        End If
    End Sub

    Private Function LeggiCUAA(ByRef objParametri As AgronicaCoreParametri) As String
        Const nomeRoutine = "LeggiCUAA"
        Dim CUAA As String = ""
        Dim messaggioErrore As String = ""
        Dim strSql As New Text.StringBuilder
        Dim dt As DataTable
        Dim objProvider As New AgronicaCoreDataProvider.DataProvider
        Try

            strSql.Length = 0
            strSql.AppendLine(" IF EXISTS (SELECT * FROM sys.views where [name] = 'Last_Impresa_Selezionata_Dashboard' )  ")
            strSql.AppendLine(" BEGIN ")
            strSql.AppendLine(" SELECT TOP 1 ISNULL(i.val_cod, '') AS CUAA, l.Piva  ")
            strSql.AppendLine(" FROM Last_Impresa_Selezionata_Dashboard l WITH (NOLOCK)")
            strSql.AppendLine(" LEFT JOIN Imprese_Codici i WITH (NOLOCK) ON i.piva = l.piva AND id_cod = " & UtilityProvider.Agro_SQL_SaveNum(enum_CodiciAnagrafe.CodiceCUAA) & " ")
            strSql.AppendLine(" WHERE 1 = 1 ")
            strSql.AppendLine(" AND Username = '" & UtilityProvider.Agro_SQL_SaveText(objParametri.UtenteUsername) & "' ")
            strSql.AppendLine(" ORDER BY l.datainvio DESC ")
            strSql.AppendLine(" END ")

            '--------------------------------------------------------------------------
            dt = objProvider.EseguiQuery_Lettura(objParametri, strSql.ToString, nomeRoutine, chiamaScriviLog:=False)
            '--------------------------------------------------------------------------
            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                CUAA = dt.Rows(0).Item("CUAA")
                'Se non risco a ricavare il CUAA, prendo la PIVA
                If CUAA = "" Then
                    CUAA = dt.Rows(0).Item("Piva") & " [PIVA]"
                End If
            End If
        Catch ex As Exception
            CUAA = ""
        End Try
        Return CUAA
    End Function

#Region "ELASTIC SEARCH"
    Private Sub VerificaInviaElasticSearchLogger(ByVal CUAA As String, ByVal NomeRoutine As String, ByVal Testo As String, ByVal timeStamp As Date, ByVal objParametri As AgronicaCoreParametri)
        If objParametri.StringaConnessioneEncrypted Then
            objParametri.StringaConnessione = objParametri.StringaConnessione
        End If
        ThreadPool.QueueUserWorkItem(Sub() ElasticSearchLogger(CUAA, NomeRoutine, Testo, timeStamp, objParametri))
    End Sub

    Private Sub ElasticSearchLogger(ByVal CUAA As String, ByVal NomeRoutine As String, ByVal Testo As String, ByVal timeStamp As Date, ByVal objParametri As AgronicaCoreParametri)

        Dim cfg As ConfigurazioneLogProviderEsteso = ConfigurazioneLogProviderFactory.Instance(objParametri)
        Dim urlColdirettiLogger As String = String.Empty
        Dim objParametriES As ParametriElasticSearch = Nothing

        If Not IsNothing(cfg) AndAlso Not IsNothing(cfg.ConfigurazioneElasticSearch) Then
            urlColdirettiLogger = cfg.ConfigurazioneElasticSearch.ElasticSearchUrl
            objParametriES = cfg.ConfigurazioneElasticSearch.Parametri
        End If

        If urlColdirettiLogger <> "" Then
            Try
                If objParametriES IsNot Nothing AndAlso objParametriES.Ambiente <> "" Then
                    Dim objLog As New ElasticSearchLogger(objParametriES.Ambiente, urlColdirettiLogger)
                    objLog.WriteLog(CUAA, NomeRoutine, Testo, timeStamp)
                Else
                    Throw New Exception("ParametriElasticSearch.Ambiente non configurato correttamente.")
                End If
            Catch ex As Exception
                Scrivi_LOG(objParametri,
                           NomeRoutine,
                           "Si è verificato un errore durante l'esportazione del log ad ElasticSearch: " & ex.Message, verificaInviaElasticSearch:=False)
            End Try
        End If
    End Sub
#End Region
End Class

Public Class CustomLOGParams
    Public LogDirectory As String = ""
    Public LogFileName As String = ""
    Public LogDescrizioneUtente As String = ""
End Class