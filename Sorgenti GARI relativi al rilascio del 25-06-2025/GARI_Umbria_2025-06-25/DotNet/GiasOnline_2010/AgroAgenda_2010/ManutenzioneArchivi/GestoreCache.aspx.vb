Imports System.Net.Http
Imports System.Web.Services
Imports AgronicaCoreDataProvider
Imports AgronicaCoreModello.ParametriAgenda_Temp
Imports AgronicaCoreVarieBIZ
Imports Newtonsoft.Json

Public Class GestoreCache
    Inherits System.Web.UI.Page

    Dim objParametriAgenda As ParametriAgenda
    Dim objParametri_Server, objParametri_Utenti, objParametri_Super_Server As AgronicaCoreParametri

    Public objparametri_server_string, objparametri_utenti_string As String
    Public isSuperUser As Boolean = False

    Private Sub InizializzoObjParametri()
        objParametriAgenda = New ParametriAgenda
        objParametri_Server = New AgronicaCoreParametri(Session("ASG_objParametri_Server"))
        objParametri_Utenti = New AgronicaCoreParametri(Session("ASG_objParametri_Utenti"))
        objParametri_Super_Server = New AgronicaCoreParametri(Session("ASG_objParametri_Super_Server"))
        objparametri_server_string = Utility.convertOBJparametritoString(objParametri_Server)
        isSuperUser = (objParametri_Server.SuperUserUsername.ToLower() = objParametri_Server.UtenteUsername.ToLower())
    End Sub

    Private Sub InizializzoParametriPagina()

        Master.SetTitoloPaginaCustom("Gestione Cache (DataProvider)")

    End Sub

    Public Shadows ReadOnly Property Master() As AgroAgenda_2010.AgendaBootstrap
        Get
            Return CType(MyBase.Master, AgroAgenda_2010.AgendaBootstrap)
        End Get
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        InizializzoObjParametri()

        If Not IsPostBack Then
            InizializzoParametriPagina()
        End If

        'Controllo se l'utente ha i permessi per accedere
        'Dim objPermessi As New AgronicaCoreUtentiDAL.Utenti_Permessi_R
        'Dim UtenteAbilitatoLettura As Boolean = objPermessi.Controlla_Permessi_Utente(
        '                                    Session("ASG_Utente_Username"),
        '                                    Session("ASG_IdServizio"),
        '                                    enum_Security_Attivita.Contabilita_MVVElettronico,
        '                                    enum_Security_Operazione.Lettura,
        '                                    Date.Now,
        '                                    "",
        '                                    objParametri_Utenti)

        'Dim UtenteAbilitatoScrittura As Boolean = objPermessi.Controlla_Permessi_Utente(
        '                                   Session("ASG_Utente_Username"),
        '                                   Session("ASG_IdServizio"),
        '                                   enum_Security_Attivita.Contabilita_MVVElettronico,
        '                                   enum_Security_Operazione.Modifica,
        '                                   Date.Now,
        '                                   "",
        '                                   objParametri_Utenti)

        If isSuperUser = False Then
            Response.Redirect("~/Classi/Agro_Pages/AccessoNonConsentito.aspx")
        End If

        If Not Page.IsPostBack Then
            hf_cacheAbilitata.Value = MemoryCacheFactory.Instance.EnebaleCacheDataProvider
        End If

    End Sub

#Region "web services Cache"

    <WebMethod(EnableSession:=True)>
    Public Shared Function RicercaElementiCache() As RispostaStandard

        Dim r As New RispostaStandard
        Dim objParametri_Server As AgronicaCoreParametri = HttpContext.Current.Session("ASG_objParametri_Server")
        Dim objParametri_Super_Server As AgronicaCoreParametri = HttpContext.Current.Session("ASG_objParametri_Super_Server")

        If HttpContext.Current.Session.IsNewSession OrElse IsNothing(objParametri_Server) Then
            r.Sessione = False
            Return r
        End If

        Try

            Dim oggetti = MemoryCacheFactory.Instance.Elenca_Tuttto()

            Dim serializerSettings As New JsonSerializerSettings With {.ReferenceLoopHandling = ReferenceLoopHandling.Ignore}
            r.RispostaStringa = JsonConvert.SerializeObject(oggetti, Formatting.None, serializerSettings)
            r.RispostaOK = True

        Catch ex As Exception

            r.RispostaOK = False
            r.Errore = "Errore durante l'operazione: " & AgronicaCoreUtility.Gestione_Eccezioni.MessaggioCompletoDataEccezione(ex, False)

        End Try

        Return r
    End Function

    <WebMethod(EnableSession:=True)>
    Public Shared Function DammiUrlPerScaching() As RispostaStandard

        Dim r As New RispostaStandard
        Dim objParametri_Server As AgronicaCoreParametri = HttpContext.Current.Session("ASG_objParametri_Server")
        Dim objParametri_Super_Server As AgronicaCoreParametri = HttpContext.Current.Session("ASG_objParametri_Super_Server")

        If HttpContext.Current.Session.IsNewSession OrElse IsNothing(objParametri_Server) Then
            r.Sessione = False
            Return r
        End If

        Try
            Dim urls = getCaheServersUrls()
            r.RispostaStringa = JsonConvert.SerializeObject(urls.Where(Function(u) u <> String.Empty))
            r.RispostaOK = True

        Catch ex As Exception

            r.RispostaOK = False
            r.Errore = "Errore durante l'operazione: " & AgronicaCoreUtility.Gestione_Eccezioni.MessaggioCompletoDataEccezione(ex, False)

        End Try

        Return r

    End Function

    Private Shared Function getCaheServersUrls() As List(Of String)
        Dim objParametri_Server As AgronicaCoreParametri = HttpContext.Current.Session("ASG_objParametri_Server")

        ConfigurazioneAjaxFactory.Reset(objParametri_Server)

        Dim urls = New List(Of String)

        Dim configSiti As New AgronicaCoreVarieDAL.Configurazione_Siti_R
        Dim LanToWebSiteBasePath = configSiti.Leggi_Valore(0, "LanToWebSiteBasePath", "", "", objParametri_Server)
        Dim jsonServers = configSiti.Leggi_Valore(0, "ServerAddresses", "", "", objParametri_Server)
        Dim servers As String() = {}
        If (Not String.IsNullOrWhiteSpace(jsonServers)) Then
            servers = JsonConvert.DeserializeObject(Of String())(jsonServers)
        End If


        Dim chiavi = New List(Of String) From
                    {
                        "'GiasOnline_WS_Core_AgroWS_Core'",
                        "'LinkAgronicaStampe_2010'",
                        "'LinkAgronicaProfilazione'",
                        "'LinkAgronicaPlanning'",
                        "'LinkAgronicaAnalisi_2010'",
                        "'LinkAgronicaPianiCampionamento'",
                        "'LinkAgronicaPianiSemina'",
                        "'LinkAgronicaPua'",
                        "'LinkAgronicaAudit'",
                        "'LinkAgronicaSincronizzatore'",
                        "'LinkPianoConcimazione_2017'",
                        "'LinkAgronicaUMA'",
                        "'LinkAgronicaDomandaIrrigua'",
                        "'LinkAgronicaAgenda2010'"
                    }

        Dim xfiltroAggiuntivo As String = " chiave in (" + String.Join(",", chiavi) + ") "
        Dim dt As DataTable = configSiti.Leggi(0, "", xfiltroAggiuntivo, "", objParametri_Server)
        If Not IsNothing(dt) AndAlso dt.Rows.Count > 0 Then
            For Each row As DataRow In dt.Rows
                Dim url As String = ""

                Dim endpointPulisciCache As String = "Cache/CacheManager.asmx/PulisciCache"
                url = PurificaUrl(LanToWebSiteBasePath, row("valore").ToString.Trim, endpointPulisciCache)
                If Not url.ToLower.Contains("/Cache/CacheManager.asmx/PulisciCache".ToLower) Then
                    If url.EndsWith("/") Then
                        url = url & endpointPulisciCache
                    Else
                        url = url & "/" & endpointPulisciCache
                    End If
                End If
                urls.Add(url)

                Dim endpointPulisciCacheEFactory As String = "Cache/CacheManager.asmx/PulisciCacheEFactory"
                url = PurificaUrl(LanToWebSiteBasePath, row("valore").ToString.Trim, endpointPulisciCacheEFactory)
                If Not url.ToLower.Contains("/Cache/CacheManager.asmx/PulisciCacheEFactory".ToLower) Then
                    If url.EndsWith("/") Then
                        url = url & endpointPulisciCacheEFactory
                    Else
                        url = url & "/" & endpointPulisciCacheEFactory
                    End If
                End If
                urls.Add(url)
            Next
        End If

        Dim remoteServerUrls = New List(Of String)
        For Each server As String In servers
            For Each url As String In urls
                Dim uri As Uri = Nothing
                If Uri.TryCreate(url, UriKind.Absolute, uri) Then
                    remoteServerUrls.Add($"{server.TrimEnd("/")}/{uri.PathAndQuery.TrimStart("/")}")
                End If
            Next
        Next
        urls.AddRange(remoteServerUrls)

        Return urls

    End Function



    Private Shared Function PurificaUrl(basePath As String, url As String, endpoint As String) As String

        If String.IsNullOrEmpty(url) Then
            Return ""
        End If

        url = url.Replace("GestioneRichieste.aspx", endpoint)
        If url.ToLower.StartsWith("http") Then
            Return url
        Else
            Return String.Format("{0}{1}", basePath, url)
        End If

    End Function

    <WebMethod(EnableSession:=True)>
    Public Shared Function PulisciCache() As RispostaStandard
        Dim r As New RispostaStandard
        r.RispostaOK = True
        Dim objParametri_Server As AgronicaCoreParametri = HttpContext.Current.Session("ASG_objParametri_Server")

        If HttpContext.Current.Session.IsNewSession OrElse IsNothing(objParametri_Server) Then
            r.Sessione = False
            Return r
        End If

        Try
            ''Public Shared Function DammiUrlPerScaching() As RispostaStandard
            '' inizializza stringhe connessione in application
            'GlobalAsax_Helper.Inizializza_Stringhe_Connessione()

            '' Cache giasonline
            'MemoryCacheFactory.Instance.Pulisci_Tuttto()
            Dim clearCahecheEndPoints As List(Of String) = getCaheServersUrls()
            Dim erroriGias As New List(Of ErroreGias)

            For Each endpoint As String In clearCahecheEndPoints
                Try
                    Dim response As New HttpResponseMessage
                    Dim content As String = "{" & vbCrLf & "}"
                    If endpoint.Contains("PulisciCacheEFactory") Then
                        content = "{" + $"objP_server: '{JsonConvert.SerializeObject(HttpContext.Current.Session("ASG_objParametri_Server"))}'" + "}"
                    End If

                    response = Global_asax.HttpClient.PostAsync(endpoint, New StringContent(content, Nothing, "application/json")).Result

                    If (response.IsSuccessStatusCode) Then
                        r.RispostaStringaCustom &= "Chiamata POST: " & endpoint & " eseguita correttamente<br>"
                    Else
                        response = Global_asax.HttpClient.GetAsync(endpoint).Result
                        If (response.IsSuccessStatusCode) Then
                            r.RispostaStringaCustom &= "Chiamata GET: " & endpoint & " eseguita correttamente<br>"
                        End If
                    End If

                    response.EnsureSuccessStatusCode()
                Catch ex As Exception

                    r.RispostaOK = False
                    r.ErroriGias.Add(New ErroreGias With {
                                        .messaggio = "Errore durante l'operazione su " & endpoint & "<br>" & ex.Message & "<br>"
                                     }
                    )

                End Try
            Next
        Catch ex As Exception

            r.RispostaOK = False
            r.Errore = "Errore durante l'operazione: " & AgronicaCoreUtility.Gestione_Eccezioni.MessaggioCompletoDataEccezione(ex, False)

        End Try

        Return r
    End Function

    <WebMethod(EnableSession:=True)>
    Public Shared Function PulisciElemento(ByVal chiave As String, ByVal tipoCache As String) As RispostaStandard

        Dim r As New RispostaStandard
        Dim objParametri_Server As AgronicaCoreParametri = HttpContext.Current.Session("ASG_objParametri_Server")
        Dim objParametri_Super_Server As AgronicaCoreParametri = HttpContext.Current.Session("ASG_objParametri_Super_Server")

        If HttpContext.Current.Session.IsNewSession OrElse IsNothing(objParametri_Server) Then
            r.Sessione = False
            Return r
        End If

        Try

            MemoryCacheFactory.Instance.PulisciElemento(chiave, tipoCache)

            r.RispostaStringa = "Cache pulita correttamente"
            r.RispostaOK = True

        Catch ex As Exception

            r.RispostaOK = False
            r.Errore = "Errore durante l'operazione: " & AgronicaCoreUtility.Gestione_Eccezioni.MessaggioCompletoDataEccezione(ex, False)

        End Try

        Return r
    End Function

    <WebMethod(EnableSession:=True)>
    Public Shared Function AbilitaCache(ByVal abilita As Boolean) As RispostaStandard

        Dim r As New RispostaStandard
        Dim objParametri_Server As AgronicaCoreParametri = HttpContext.Current.Session("ASG_objParametri_Server")
        Dim objParametri_Super_Server As AgronicaCoreParametri = HttpContext.Current.Session("ASG_objParametri_Super_Server")

        If HttpContext.Current.Session.IsNewSession OrElse IsNothing(objParametri_Server) Then
            r.Sessione = False
            Return r
        End If

        Try

            MemoryCacheFactory.Instance.EnebaleCacheDataProvider = abilita

            r.RispostaOK = True

        Catch ex As Exception

            r.RispostaOK = False
            r.Errore = "Errore durante l'operazione: " & AgronicaCoreUtility.Gestione_Eccezioni.MessaggioCompletoDataEccezione(ex, False)

        End Try

        Return r
    End Function
#End Region


End Class