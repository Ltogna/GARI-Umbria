
Imports AgronicaCoreDataProvider
Imports AgronicaCoreDataProvider.Sicurezza
Imports AgronicaCoreDataProvider.CostantiPersonalizzate
Imports AgronicaCoreDataProvider.TipiEnumerativi
Imports AgronicaCoreDataProvider.UtilityProvider
Imports AgronicaCoreDataProvider.AgronicaCoreParametri
Imports AgronicaCoreVarieBIZ
Imports System.Web.Services
Imports Newtonsoft.Json.Linq
Imports Newtonsoft.Json
Imports System.Security.Cryptography
Imports System.IO
Imports AgronicaCoreUtentiDAL.ws_ppt
Imports AgronicaCoreWebService
Public Class Request
    Public Property DataInizio As DateTime

End Class
Public Class UtentiProvisioningDetResponse
    Public Property RagioneSociale As String
    Public Property Cognome As String
    Public Property Nome As String
    Public Property Via As String
    Public Property NumeroCivico As String
    Public Property Citta As String
    Public Property Provincia As String
    Public Property Cap As String
    Public Property Tel As String
    Public Property Fax As String
    Public Property Email As String
    Public Property Piva As String
    Public Property CodiceTransazioneComm As String
    Public Property CodiceSdi As String
    Public Property NLicenze As Integer
    Public Property CodiceCoupon As String
    Public Property ModalitaPagamento As String
    Public Property StatoPagamento As String
    Public Property PEC As String
    Public Property DataCreazione As Date?
    Public Property DataModifica As Date?
End Class

Public Class UtentiProvisioningResponse
    Public Property RispostaOk As Boolean
    Public Property Errore As String
    Public Property Payload As List(Of UtentiProvisioningDetResponse)
End Class


Public Class GestioneUtentiProfiling
    Inherits System.Web.UI.Page

    'Protected Overrides Sub InitializeCulture()
    '    MyBase.InitializeCulture()
    '    Lingua.Gias_InizializzaCultura_DaSession()
    'End Sub

    Dim objParametri_Server, objParametri_Utenti As AgronicaCoreParametri
    Public objparametri_server_string, objparametri_utenti_string As String

    'Private Sub inizializzoObjParametri()
    '    '---
    '    objParametri_Server = New AgronicaCoreDataProvider.AgronicaCoreParametri(Session("ASG_objParametri_Server"))
    '    objParametri_Utenti = New AgronicaCoreDataProvider.AgronicaCoreParametri(Session("ASG_objParametri_Utenti"))
    '    objparametri_server_string = AgronicaCoreDataProvider.Utility.convertOBJparametritoString(objParametri_Server)
    '    '---
    'End Sub

    <WebMethod(EnableSession:=True)>
    Public Shared Function GeneraGiasAppKey(ByVal pivasuperuser As String, ByVal versione As String) As RispostaStandard

        Dim r As New RispostaStandard

        Dim objParametri_Utenti As AgronicaCoreParametri = HttpContext.Current.Session("ASG_objParametri_Utenti")
        If HttpContext.Current.Session.IsNewSession OrElse IsNothing(objParametri_Utenti) Then
            r.Sessione = False
            Return r
        End If

        Try
            Dim UrlWS As String = ConfigurationManager.AppSettings("UrlWS_GestioneUtenti")

            UrlWS = UrlWS.Replace("Gias_service.asmx", "WS_Autenticazione.asmx")

            Dim xUtil As New AgronicaCoreUtility.Http
            Dim payload = "{ ""PivaSuperUser"" : """ & pivasuperuser & """, ""VersioneApp"" : """ & versione & """ }"

            Dim xRisp As String = xUtil.RestPostBasicAuth(UrlWS + "/GeneraGiasAppKey", payload, "", "", "d", True)  'xUtil.chiamaWS("", "", url_ws + "/Leggi_Capitolati", "application/json", "GET", "application/json", "")
            Dim rRisp As RispostaStandard = JsonConvert.DeserializeObject(Of RispostaStandard)(xRisp)

            r.RispostaOK = rRisp.RispostaOK
            If rRisp.RispostaOK = True Then
                r.RispostaStringa = rRisp.RispostaStringa
            Else
                r.Errore = rRisp.Errore
            End If

        Catch ex As Exception
            r.RispostaOK = False
            r.Errore = Gestione_Eccezioni_2015.MessaggioCompletoDataEccezione(ex, True, source:=True)
        End Try

        Return r

    End Function

    <WebMethod(EnableSession:=True)>
    Public Shared Function Ricerca_Utenti_Profiling(ByVal filtro_data_inizio As String) As RispostaStandard

        Dim r As New RispostaStandard
        Dim filtro_aggiuntivo As String = ""
        Dim DT As New DataTable


        Dim dt_finale As New DataTable
        Dim dr_finale As DataRow

        Dim objParametri_Utenti As AgronicaCoreParametri = HttpContext.Current.Session("ASG_objParametri_Utenti")
        If HttpContext.Current.Session.IsNewSession OrElse IsNothing(objParametri_Utenti) Then
            r.Sessione = False
            Return r
        End If

        Try
            Dim UrlWS As String = ""
            Dim Elenco As String = ""
            Dim righeElenco As JArray

            UrlWS = ConfigurationManager.AppSettings("UrlWS_GestioneUtenti")


            'UrlWS = "http://www.agronica.it/AgronicaWebService/Gias_service.asmx"
            'UrlWS = "http://localhost/AgronicaWebService2010/Gias_service.asmx"

            UrlWS = "https://localhost:44301/provisioning/RicercaListaUtentiProvisioning"

            Dim token As String = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjdlOTk4NmEzLWI1YjctNGNjZC04YWJjLWFiMGU3ZWE3YTg4NSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJUb20xIiwiTGlzdGFBbWJpdG9Fc3Rlcm8iOiJbXSIsIkxpc3RhUGVybWVzc2kiOiJbe1wiSWRTZXJ2aXppb1wiOjIwLFwiVmFsaWRpdGFGaW5lXCI6XCIyMTAwLTEyLTMxVDAwOjAwOjAwXCJ9XSIsIm5iZiI6MTY1ODc1NTQ4NiwiZXhwIjoxNjU4NzYyNjg2LCJpc3MiOiJQcm9maXRvc2FuIiwiYXVkIjoiVXRlbnRpUHJvZml0b3NhbiJ9.BnN_uNE53ZTh5o28Pk3EatoMXUlqX8S8cHMdMkcdCeo"

            Dim req As New Request
            req.DataInizio = CDate(filtro_data_inizio)
            Dim jsonTxt As String = JsonConvert.SerializeObject(req, Formatting.Indented)

            'da trovare il path e rinominare le variabili
            Dim xChiamataRest As New AgronicaCoreUtility.Http
            Dim DatiSrv As AgronicaCoreUtility.Http.JsonRestResponse = xChiamataRest.PostWS_RestSharp_JSON(UrlWS, token, jsonTxt)


            Dim risultato As UtentiProvisioningResponse = JsonConvert.DeserializeObject(Of UtentiProvisioningResponse)(DatiSrv.Content)

            'Costruzione DT Finale
            dt_finale = CostruisciDataTable(dt_finale)
            If DatiSrv.Content <> String.Empty Then
                For Each Dettaglio As UtentiProvisioningDetResponse In risultato.Payload

                    dr_finale = dt_finale.NewRow
                    dr_finale.Item(0) = Dettaglio.RagioneSociale
                    dr_finale.Item(1) = Dettaglio.Cognome
                    dr_finale.Item(2) = Dettaglio.Nome
                    dr_finale.Item(3) = Dettaglio.Via
                    dr_finale.Item(4) = Dettaglio.NumeroCivico
                    dr_finale.Item(5) = Dettaglio.Citta
                    dr_finale.Item(6) = Dettaglio.Provincia
                    dr_finale.Item(7) = Dettaglio.Cap
                    dr_finale.Item(8) = Dettaglio.Tel
                    dr_finale.Item(9) = Dettaglio.Fax
                    dr_finale.Item(10) = Dettaglio.Email
                    dr_finale.Item(11) = Dettaglio.Piva
                    dr_finale.Item(12) = Dettaglio.CodiceTransazioneComm
                    dr_finale.Item(13) = Dettaglio.CodiceSdi
                    dr_finale.Item(14) = Dettaglio.NLicenze
                    dr_finale.Item(15) = Dettaglio.CodiceCoupon
                    dr_finale.Item(16) = Dettaglio.ModalitaPagamento
                    dr_finale.Item(17) = Dettaglio.StatoPagamento
                    dr_finale.Item(18) = Dettaglio.PEC
                    dt_finale.Rows.Add(dr_finale)


                Next

            End If

            Dim serializerSettings As New JsonSerializerSettings()
            serializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            r.RispostaStringa = JsonConvert.SerializeObject(dt_finale, Formatting.None, serializerSettings)

            r.RispostaOK = True

        Catch ex As Exception

            r.RispostaOK = False

            'uso questa funzione per ottenere il Messaggio..:
            r.Errore = "Impossibile Caricare le Licenze" & vbCrLf &
            AgronicaCoreDataProvider.Gestione_Eccezioni_2015.MessaggioCompletoDataEccezione(ex, True, source:=True)
        End Try

        Return r

    End Function
    Private Shared Function CostruisciDataTable(ByRef dt_finale) As DataTable
        dt_finale.Columns.Add(New DataColumn("RagioneSociale", GetType(String)))
        dt_finale.Columns.Add(New DataColumn("Cognome", GetType(String)))
        dt_finale.Columns.Add(New DataColumn("Nome", GetType(String)))
        dt_finale.Columns.Add(New DataColumn("Via", GetType(String)))
        dt_finale.Columns.Add(New DataColumn("NumeroCivico", GetType(Integer)))
        dt_finale.Columns.Add(New DataColumn("Citta", GetType(String)))
        dt_finale.Columns.Add(New DataColumn("Provincia", GetType(String)))
        dt_finale.Columns.Add(New DataColumn("Cap", GetType(String)))
        dt_finale.Columns.Add(New DataColumn("Tel", GetType(String)))
        dt_finale.Columns.Add(New DataColumn("Fax", GetType(String)))
        dt_finale.Columns.Add(New DataColumn("Email", GetType(String)))
        dt_finale.Columns.Add(New DataColumn("Piva", GetType(String)))
        dt_finale.Columns.Add(New DataColumn("CodiceTransazioneComm", GetType(String)))
        dt_finale.Columns.Add(New DataColumn("CodiceSdi", GetType(String)))
        dt_finale.Columns.Add(New DataColumn("NLicenze", GetType(Integer)))
        dt_finale.Columns.Add(New DataColumn("CodiceCoupon", GetType(String)))
        dt_finale.Columns.Add(New DataColumn("ModalitaPagamento", GetType(String)))
        dt_finale.Columns.Add(New DataColumn("StatoPagamento", GetType(String)))
        dt_finale.Columns.Add(New DataColumn("PEC", GetType(String)))
        Return dt_finale
    End Function

    <WebMethod(EnableSession:=True)>
    Public Shared Function Aggiorna_Licenze(ByVal righeInseriteGrid_Licenze As String,
                                            ByVal righeModificateGrid_Licenze As String,
                                            ByVal righeCancellateGrid_Licenze As String
                                            ) As RispostaStandard


        Dim r As New RispostaStandard
        Dim objParametri_Utenti As AgronicaCoreParametri = HttpContext.Current.Session("ASG_objParametri_Utenti")
        If HttpContext.Current.Session.IsNewSession OrElse IsNothing(objParametri_Utenti) Then
            r.Sessione = False
            Return r
        End If

        Try
            Dim UrlWS As String = ""
            Dim Dummy As Integer

            UrlWS = ConfigurationManager.AppSettings("UrlWS_GestioneUtenti")


            'UrlWS = "http://www.agronica.it/AgronicaWebService/Gias_service.asmx"
            'UrlWS = "http://localhost/AgronicaWebService2010/Gias_service.asmx"

            UrlWS = UrlWS.Replace("Gias_service.asmx", "Ws_Ppt.asmx")

            Dim scrivi As New AgronicaCoreUtentiDAL.Utenti_CodiciGiasPro_W
            Dummy = scrivi.Aggiorna_LicenzeWS(UrlWS,
                                              righeInseriteGrid_Licenze,
                                              righeModificateGrid_Licenze,
                                              righeCancellateGrid_Licenze,
                                              objParametri_Utenti)


            r.RispostaStringa = CStr(Dummy)
            r.RispostaOK = True

        Catch ex As Exception

            r.RispostaOK = False

            'uso questa funzione per ottenere il Messaggio..:
            r.Errore = "Errore durante l'operazione: " & vbCrLf &
                AgronicaCoreDataProvider.Gestione_Eccezioni_2015.MessaggioCompletoDataEccezione(ex, False, source:=False)
        End Try

        Return r

    End Function



    '################################################################################################
    '################################################################################################
    '################################################################################################
    '################################################################################################
    '################################################################################################




    '#####################################################################

    <WebMethod(EnableSession:=True)>
    Public Shared Function ChiaveGiasOnline_Codifica(ByVal Username As String,
                                                     ByVal GiasOnline_Key As String,
                                                     ByVal CodiceProgressivoGIAS As Integer,
                                                     ByVal DataAttivazione As Date,
                                                     ByVal DataScadenza As Date,
                                                     ByVal ChkEstensione As String,
                                                     ByVal Mesi_Estensione As String,
                                                     ByVal DataScadenza_Estensione As String,
                                                     ByVal NumeroAziende As String,
                                                     ByVal NumeroUtenti As Integer,
                                                     ByVal SuperficieTotale As String,
                                                     ByVal NumeroAccessi As Integer,
                                                     ByVal NumeroUtilizzi As Integer) As RispostaStandard




        '---------------------------------------------------------------------
        '----- Definisco le variabili
        '---------------------------------------------------------------------
        Dim r As New RispostaStandard

        Dim Chiave As String = ""
        Dim Chiave3 As String
        Dim ChiaveLunghezza As Integer
        Dim NumeroModuli As Integer

        Dim i As Integer
        Dim Testo As String

        Dim Pos_Username As Integer
        Dim Pos_CodiceProgressivoGIAS As Integer
        Dim Pos_DataAttivazione As Integer
        Dim Pos_DataScadenza As Integer
        Dim Pos_NumeroAziende As Integer
        Dim Pos_NumeroUtenti As Integer
        Dim Pos_SuperficieTotale As Integer
        Dim Pos_NumeroAccessi As Integer
        Dim Pos_NumeroUtilizzi As Integer
        Dim Pos_DataGenerazioneChiave As Integer
        Dim Pos_VersioneCodifica As Integer
        Dim Pos_VersioneChiave As Integer
        Dim Pos_Modulo() As String
        Dim Pos_Checksum As Integer

        Dim Num_Username As Integer
        Dim Num_CodiceProgressivoGIAS As Integer
        Dim Num_DataAttivazione As Integer
        Dim Num_DataScadenza As Integer
        Dim Num_NumeroAziende As Integer
        Dim Num_NumeroUtenti As Integer
        Dim Num_SuperficieTotale As Integer
        Dim Num_NumeroAccessi As Integer
        Dim Num_NumeroUtilizzi As Integer
        Dim Num_DataGenerazioneChiave As Integer
        Dim Num_VersioneCodifica As Integer
        Dim Num_VersioneChiave As Integer
        Dim Num_Modulo As Integer
        Dim Num_Checksum As Integer


        Dim mX As Integer
        Dim mI As Date
        Dim mF As Date

        Dim Carattere As String
        Dim Codice As Integer
        Dim GruppoCodici As Integer

        'Dati della chiavi inutilizzati
        Dim DataGenerazioneChiave As Date = Date.Now
        Dim VersioneCodifica As Integer = 1
        Dim VersioneChiave As Integer = 1
        Dim Modulo_Flag() As Integer
        Dim Modulo_Inizio() As Date
        Dim Modulo_Fine() As Date
        Dim Checksum As String


        Dim DataAttivazione_Gias_Online As Date
        Dim NumeroAziende_Gias_Online As Integer
        Dim NumeroUtenti_Gias_Online As Integer
        Dim SuperficieTotale_Gias_Online As Integer
        Dim NumeroAccessi_Gias_Online As Integer
        Dim NumeroUtilizzi_Gias_Online As Integer
        Dim Scadenza_Gias_Online As Date

        Dim Count As Integer = 0
        Dim CodiceProgressivoGIASOld As Integer = 0

        Dim Risultato As String
        Dim MessaggioErrore As String = ""
        Dim bOk As Boolean


        Try

            'Inziializzazione
            Chiave = GiasOnline_Key

            Count = Len(GiasOnline_Key)

            'Controllo che chiave sia presente
            If Trim(GiasOnline_Key) <> "" Then


                Chiave = ""

                ReDim Modulo_Flag(0 To 15)
                ReDim Modulo_Inizio(0 To 15)
                ReDim Modulo_Fine(0 To 15)




                NumeroUtenti = 1

                NumeroAccessi = 0
                NumeroUtilizzi = 0


                'Determinazione Scadenza Licenze
                Dim AgroDecodifica As New AgronicaCoreDataProvider.Sicurezza


                Risultato = AgroDecodifica.ChiaveGiasOnline_Decodifica(GiasOnline_Key,
                                                                       Username,
                                                                       Username,
                                                                       CodiceProgressivoGIASOld,
                                                                       DataAttivazione_Gias_Online,
                                                                       Scadenza_Gias_Online,
                                                                       NumeroAziende_Gias_Online,
                                                                       NumeroUtenti_Gias_Online,
                                                                       SuperficieTotale_Gias_Online,
                                                                       NumeroAccessi_Gias_Online,
                                                                       NumeroUtilizzi_Gias_Online,
                                                                       DataGenerazioneChiave,
                                                                       VersioneCodifica,
                                                                       VersioneChiave,
                                                                       Modulo_Flag,
                                                                       Modulo_Inizio,
                                                                       Modulo_Fine,
                                                                       Checksum,
                                                                       MessaggioErrore)


                For i = 0 To 15
                    Modulo_Flag(i) = 0
                    Modulo_Inizio(i) = #1/1/1900#
                    Modulo_Fine(i) = #12/31/2100#
                Next

                SuperficieTotale_Gias_Online = 9999

                If Not IsNumeric(NumeroAziende_Gias_Online) Then
                    NumeroAziende_Gias_Online = 1000
                End If
                If NumeroAziende_Gias_Online = 0 Then
                    NumeroAziende_Gias_Online = 1000
                End If


                'Correzioen Bug
                If CodiceProgressivoGIAS = 0 And CodiceProgressivoGIASOld <> 0 Then
                    CodiceProgressivoGIAS = CodiceProgressivoGIASOld
                End If


                'Determinazione Nuova Scadenza
                Select Case CBool(ChkEstensione)

                    Case True

                        'Scadenza Puntuale
                        If IsDate(DataScadenza_Estensione) Then
                            Scadenza_Gias_Online = DataScadenza_Estensione
                            bOk = True
                        End If

                    Case False

                        'Mesi
                        If IsNumeric(Mesi_Estensione) Then
                            Scadenza_Gias_Online = DateAdd("M", CInt(Mesi_Estensione), Scadenza_Gias_Online)
                            bOk = True
                        End If

                End Select

                If bOk Then

                    '---------------------------------------------------------------------
                    '----- Verifico la versione della STRUTTURA della CHIAVE ...
                    '---------------------------------------------------------------------

                    Select Case VersioneChiave

                        Case 1  '----------------------------------------

                            'Definisco la lunghezza della stringa
                            ChiaveLunghezza = 1000

                            'Definisco il numero di moduli
                            NumeroModuli = 16

                            'Definisco la posizione degli elementi nella stringa
                            Pos_Username = 181
                            Pos_CodiceProgressivoGIAS = 15
                            Pos_DataAttivazione = 49
                            Pos_DataScadenza = 101
                            Pos_NumeroAziende = 141
                            Pos_NumeroUtenti = 89
                            Pos_SuperficieTotale = 134
                            Pos_NumeroAccessi = 148
                            Pos_NumeroUtilizzi = 41
                            Pos_DataGenerazioneChiave = 4
                            Pos_VersioneCodifica = 1
                            Pos_VersioneChiave = 26
                            Pos_Checksum = 98

                            Num_Username = 50
                            Num_CodiceProgressivoGIAS = 6
                            Num_DataAttivazione = 10
                            Num_DataScadenza = 10
                            Num_NumeroAziende = 6
                            Num_NumeroUtenti = 6
                            Num_SuperficieTotale = 6
                            Num_NumeroAccessi = 6
                            Num_NumeroUtilizzi = 6
                            Num_DataGenerazioneChiave = 10
                            Num_VersioneCodifica = 2
                            Num_VersioneChiave = 3
                            Num_Checksum = 3
                            Num_Modulo = 21

                            'Per i moduli uso un trucchetto .... mi devo ricordare di usare la funzione CINT()
                            Pos_Modulo = Split("261,284,310,343," &
                                           "374,396,423,449," &
                                           "491,515,559,584," &
                                           "627,693,716,758", ",")

                            'Creo una stringa vuota (riempio di caratteri ".")
                            Chiave = Space(ChiaveLunghezza).Replace(" ", ".")


                            '--- Inserisco gli elementi nella stringa 

                            Mid(Chiave, Pos_Username, Num_Username) = Username.PadRight(Num_Username)

                            Mid(Chiave, Pos_CodiceProgressivoGIAS, Num_CodiceProgressivoGIAS) = CStr(CodiceProgressivoGIAS).PadRight(Num_CodiceProgressivoGIAS)

                            Mid(Chiave, Pos_DataAttivazione, Num_DataAttivazione) = DataAttivazione_Gias_Online.ToShortDateString.PadRight(Num_DataAttivazione)

                            Mid(Chiave, Pos_DataScadenza, Num_DataScadenza) = Scadenza_Gias_Online.ToShortDateString.PadRight(Num_DataScadenza)

                            Mid(Chiave, Pos_NumeroAziende, Num_NumeroAziende) = CStr(NumeroAziende_Gias_Online).PadRight(Num_NumeroAziende)

                            Mid(Chiave, Pos_NumeroUtenti, Num_NumeroUtenti) = CStr(NumeroUtenti_Gias_Online).PadRight(Num_NumeroUtenti)

                            Mid(Chiave, Pos_SuperficieTotale, Num_SuperficieTotale) = CStr(SuperficieTotale_Gias_Online).PadRight(Num_SuperficieTotale)

                            Mid(Chiave, Pos_NumeroAccessi, Num_NumeroAccessi) = CStr(NumeroAccessi_Gias_Online).PadRight(Num_NumeroAccessi)

                            Mid(Chiave, Pos_NumeroUtilizzi, Num_NumeroUtilizzi) = CStr(NumeroUtilizzi_Gias_Online).PadRight(Num_NumeroUtilizzi)

                            Mid(Chiave, Pos_DataGenerazioneChiave, Num_DataGenerazioneChiave) = DataGenerazioneChiave.ToShortDateString.PadRight(Num_DataGenerazioneChiave)

                            Mid(Chiave, Pos_VersioneCodifica, Num_VersioneCodifica) = Microsoft.VisualBasic.Strings.Right("00" & CStr(VersioneCodifica), 2)

                            Mid(Chiave, Pos_VersioneChiave, Num_VersioneChiave) = CStr(VersioneChiave).PadRight(Num_VersioneChiave)

                            Mid(Chiave, Pos_Checksum, Num_Checksum) = "000"

                            '--- Considero i moduli


                            'Dim mX As Integer
                            'Dim mI As Date
                            'Dim mF As Date



                            For i = 0 To NumeroModuli - 1


                                If i <= UBound(Modulo_Flag) Then

                                    mX = Modulo_Flag(i)
                                    mI = Modulo_Inizio(i)
                                    'mF = Modulo_Fine(i)
                                    mF = Scadenza_Gias_Online 'Pareggio la scadenza del modulo

                                Else

                                    'Se il vettore in ingresso non contiene celle sufficienti...
                                    'integro con valori dummy

                                    mX = 0
                                    mI = "15/02/1999"
                                    mF = "21/06/2001"

                                End If


                                'Compongo la stringa per il modulo
                                Testo = mX &
                                    mI.ToShortDateString &
                                    mF.ToShortDateString

                                'Sostituisco nella stringa della chiave
                                Mid(Chiave, CInt(Pos_Modulo(i)), Num_Modulo) = Testo.PadRight(Num_Modulo)

                            Next



                        Case Else  '--------------------------------------


                    End Select


                    'Debug
                    Testo = Chiave


                    '---------------------------------------------------------------------
                    '----- Sostituisco i filler "." con caratteri casuali ...
                    '---------------------------------------------------------------------


                    For i = 1 To ChiaveLunghezza

                        'Recupero il carattere
                        Carattere = Mid(Chiave, i, 1)

                        'Sostituisco il filler con dei caratteri pseudo-casuali
                        If Carattere = "." Then


                            'Codice = CInt(Int((6 * Rnd()) + 1)) ' Generate random value between 1 and 6
                            GruppoCodici = CInt(Int((3 * Rnd()) + 1))


                            Select Case GruppoCodici

                                Case 1  ' 0..9
                                    Codice = CInt(Int((10 * Rnd()) + 48)) ' Generate random value between 97 and 122

                                Case 2  ' A..Z
                                    Codice = CInt(Int((26 * Rnd()) + 97)) ' Generate random value between 97 and 122

                                Case 3  ' a..z
                                    Codice = CInt(Int((26 * Rnd()) + 65)) ' Generate random value between 65 and 90

                            End Select


                            Mid(Chiave, i, 1) = Chr(Codice)

                        End If

                    Next


                    'Debug
                    Testo = Chiave



                    '---------------------------------------------------------------------
                    '----- Calcolo il Checksum e lo inserisco nella stringa ...
                    '---------------------------------------------------------------------

                    'Dim Carattere As String
                    'Dim Codice As Integer
                    'Dim GruppoCodici As Integer

                    Dim Somma As Integer


                    'Inizializzo
                    Somma = 0


                    For i = 1 To ChiaveLunghezza

                        'Recupero il carattere
                        Carattere = Mid(Chiave, i, 1)

                        'Conversione ASCII
                        Codice = Asc(Carattere)

                        'Sommo
                        Somma = Somma + Codice

                    Next


                    Testo = "0000000" & CStr(Somma)

                    'Considero come Checksum i tre caratteri meno significativi
                    Checksum = Microsoft.VisualBasic.Strings.Right(Testo, 3)

                    'Sostituisco il valore ottenuto nella chiave
                    Mid(Chiave, Pos_Checksum, Num_Checksum) = Checksum


                    'Debug
                    Testo = Chiave


                    '---------------------------------------------------------------------
                    '----- Verifico la versione dell'ALGORITMO di CODIFICA ... e codifico
                    '---------------------------------------------------------------------


                    'NOTA
                    'Codifico a partire dal 3' carattere

                    Chiave3 = Mid(Chiave, 3)

                    Select Case VersioneCodifica

                        Case 1

                            'Chiave = Mid(Chiave, 1, 2) & Sicurezza.Stringa_Codifica_Chiave(Chiave3, True)



                            '#####################################################################
                            Dim Key() As Byte = {12, 52, 74, 32, 33, 36, 23, 48, 14, 50,
                                 52, 112, 60, 14, 135, 116, 84, 149,
                                 81, 200, 211, 29, 65, 35}

                            Dim Iv() As Byte = {12, 36, 37, 97, 106, 56, 76, 18, 99, 107,
                                21, 123, 65, 114, 159, 196, 179,
                                198, 192, 241, 212, 123, 0, 54}
                            Dim StringaCriptata As String

                            '----- Encrypt di una stringa

                            Dim cryptoProvider As TripleDESCryptoServiceProvider = New TripleDESCryptoServiceProvider
                            Dim ms As MemoryStream = New MemoryStream
                            Dim cs As CryptoStream = New CryptoStream(ms,
                                                                          cryptoProvider.CreateEncryptor(Key, Iv),
                                                                          CryptoStreamMode.Write)

                            Dim sw As StreamWriter = New StreamWriter(cs)

                            sw.Write(Chiave3)
                            sw.Flush()
                            cs.FlushFinalBlock()
                            ms.Flush()

                            'Converto la stringa in formato compatibile per XML
                            StringaCriptata = Convert.ToBase64String(ms.GetBuffer(), 0, ms.Length)


                            StringaCriptata = Replace(StringaCriptata, "+", "^")

                            Chiave = Mid(Chiave, 1, 2) & StringaCriptata





                        Case Else

                            'Creo una stringa vuota (riempio di caratteri ".")
                            Chiave = Space(ChiaveLunghezza).Replace(" ", ".")

                    End Select

                    '---------------------------------------------------------------------
                    '----- Restituisco il risultato in uscita
                    '---------------------------------------------------------------------

                Else


                End If

            End If


            'Chiave = Stringa_Codifica(Chiave, True)

            If Not bOk Then
                Chiave = GiasOnline_Key
            End If

            Count = Len(Chiave)




            r.RispostaStringa = CStr(Chiave) & "|" & Format(Scadenza_Gias_Online, "dd/MM/yyyy")
            r.RispostaOK = True

        Catch ex As Exception

            r.RispostaOK = False

            'uso questa funzione per ottenere il Messaggio..:
            r.Errore = "Errore durante l'operazione: " & vbCrLf &
            AgronicaCoreDataProvider.Gestione_Eccezioni_2015.MessaggioCompletoDataEccezione(ex, False, source:=False)
        End Try

        Return r

    End Function





    '============================================================================================================
    '============================================================================================================
    '  PARAMETRO         DESCRIZIONE                                               LUNGHEZZA  MAX VAL
    'UserName
    'DataAttivazione     Data di attivazione                                              8   31129999 -> data di partenza del contratto
    'Durata              Durata in mesi del contratto                                     2         99 -> mesi di contratto dalla data di partenza
    'Ordine              Numero di ordine dell'utente per gestire le sequenze             4       2000 -> 2000 utenti
    'Versione            BitMask dei moduli attivabili                                    7    8388608 -> 23 moduli
    'Aziende             N° massimo di aziende                                            4       9999 -> 9999 aziende
    'Sup                 Sup Max per azienda in ha                                        1          9 -> 9 classi di sup.
    '                                                                             ____________
    '                                                                                    26
    '                                                                                   + 6           -> 2 seq di 3 cifre di check digit
    '                                                                             ____________
    '                                                                                    32
    '
    '============================================================================================================
    '
    '   ------------------- CLASSE
    '   0 =   10 Ha
    '   1 =   50 Ha
    '   2 =  100 Ha
    '   3 =  500 Ha
    '   4 = 1000 Ha
    '   5 = 2000 Ha
    '   6 = 3000 Ha
    '   7 = 4000 Ha
    '   8 = 5000 Ha
    '   9 = Nessun Limite
    '
    '   -------------------- MODULI
    '    1 = Gestione Magazzini
    '    2 = Gestione Catasto
    '    4 = Gestione Stampe
    '    8 = Quaderno di Campagna
    '   16 = Contabilita' Pro
    '
    '============================================================================================================



    '############################################################################################################
    <WebMethod(EnableSession:=True)>
    Public Shared Function ChiaveGiasLan_Codifica(ByVal UserName As String,
                                                  ByVal Ordine As Integer,
                                                  ByVal GiasLan_Key As String,
                                                  ByVal Aziende As String,
                                                  ByVal Superficie As String,
                                                  ByVal DataAttivazione As Date,
                                                  ByVal DataScadenza As Date,
                                                  ByVal ChkEstensione As String,
                                                  ByVal Mesi_Estensione As String,
                                                  ByVal DataScadenza_Estensione As String) As RispostaStandard
        '-----------------------------------------------------------
        ' Procedura di Codifica
        '(c) Agronica SRL - Stefano Flamigni - 30/03/2001 - v1.0
        '-----------------------------------------------------------

        '----- Variabili

        Dim r As New RispostaStandard
        Dim Mask As Long
        Dim sMask As String
        Dim sStart As String
        Dim sEnd As String
        Dim Pari As Long
        Dim Dispari As Long
        Dim i As Long
        Dim v1 As Long
        Dim v2 As Long


        Dim Durata As Integer = 0 'Mesi Durata        
        Dim Versione As Integer = 1

        Dim bOk As Boolean = False

        Try

            If Not IsNumeric(Aziende) Then
                Aziende = 9999
            Else
                If Aziende = 0 Then
                    Aziende = 9999
                End If
            End If

            Superficie = 9

            'Determinazione Nuova Scadenza
            Select Case CBool(ChkEstensione)

                Case True

                    'Scadenza Puntuale
                    If IsDate(DataScadenza_Estensione) Then
                        'Durata = DateDiff("M", CDate(DataScadenza), CDate(DataScadenza_Estensione))

                        'If Durata < 0 Then
                        '    DataAttivazione = DateAdd("M", Durata, CDate(DataScadenza))
                        'Else
                        '    DataAttivazione = DataScadenza
                        'End If

                        DataAttivazione = CDate(Now)
                        If CDate(DataAttivazione) < DataScadenza_Estensione Then
                            Durata = DateDiff("M", CDate(DataAttivazione), CDate(DataScadenza_Estensione))
                        Else
                            DataAttivazione = DataScadenza_Estensione
                            Durata = 0

                        End If

                        DataScadenza = DataScadenza_Estensione

                        bOk = True

                    End If

                Case False

                    'Mesi
                    Durata = CInt(Mesi_Estensione)
                    DataAttivazione = DataScadenza
                    bOk = True

                    DataScadenza = DateAdd("M", CDbl(Durata), DataScadenza)

            End Select




            If bOk Then



                UserName = UCase(UserName)
                Mask = 0
                For i = 1 To Len(UserName)
                    Mask = Mask + Asc(Mid(UserName, i, 1))
                Next
                Mask = Mask Mod 999999
                sMask = Mask
                sStart = Format(DataAttivazione, "ddMMyyyy") & Format(Durata, "00") & Format(Ordine, "0000") & Format(Versione, "0000000") & Format(CInt(Aziende), "0000") & Format(CInt(Superficie), "0")
                Pari = 0
                Dispari = 0

                For i = 1 To Len(sStart)
                    If (i Mod 2) = 0 Then
                        Pari = Pari + Val(Mid(sStart, i, 1))
                    Else
                        Dispari = Dispari + Val(Mid(sStart, i, 1))
                    End If
                Next
                sStart = sStart & Format(Dispari, "000") & Format(Pari, "000")

                sEnd = ""

                For i = 1 To Len(sStart)

                    v1 = Val(Mid(sStart, i, 1))
                    v2 = Val(Mid(sMask, 1 + ((i - 1) Mod Len(sMask)), 1))

                    sEnd = sEnd + (CStr((v1 + v2) Mod 10))
                Next

                GiasLan_Key = Left(sEnd, 8) & "-" & Mid(sEnd, 9, 8) & "-" & Mid(sEnd, 17, 8) & "-" & Right(sEnd, 8)

            End If

            '---------------------------------------------------------------------
            '----- Restituisco il risultato in uscita
            '---------------------------------------------------------------------

            r.RispostaStringa = CStr(GiasLan_Key) & "|" & Format(DataScadenza, "dd/MM/yyyy")
            r.RispostaOK = True

        Catch ex As Exception
            r.RispostaOK = False
            r.Errore = "Errore durante l'operazione: " & vbCrLf & Gestione_Eccezioni_2015.MessaggioCompletoDataEccezione(ex, False, source:=False)
        End Try

        Return r

    End Function





    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Master().Lbl_Titolo.Text = "Controllo di Gestione"

        Dim auth As String() = HttpContext.Current.Session("memberOf")
        Try
            If auth Is Nothing OrElse auth.Contains(ConfigurationManager.AppSettings("memberOfLicense").ToString()) = False Then
#If Not DEBUG Then
                Response.Redirect("~/Menu/Error.aspx")
#End If
            End If
        Catch ex As Exception
            Response.Redirect("~/Menu/Error.aspx")
        End Try

        Master.flag_MostraBtnIndietro = True

    End Sub


    Private Function CheckRules(ByVal Rules As String()) As Boolean
        Dim ret = False
        Try
            If Rules IsNot Nothing And Rules.Count > 0 And Rules.Contains("License") Then
                ret = True
            Else
                ret = False
            End If
        Catch ex As Exception

        End Try
        Return ret
    End Function

    Public Shadows ReadOnly Property Master As Profilatore_Bootstrap
        Get
            Return CType(MyBase.Master, Profilatore_Bootstrap)
        End Get
    End Property

End Class

Public Class RicercaUtentiProvisioningRequest
    Public Property DataInizio As DateTime
End Class