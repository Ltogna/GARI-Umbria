Imports System.ComponentModel
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports AgronicaCoreDataProvider
Imports AgronicaCoreDataProvider.TipiEnumerativi
Imports AgronicaCoreDataProvider.My.Resources
Imports AgronicaCoreDTOStd.InData.IsAlive
Imports AgronicaCoreModelsSTD.IsAlive
Imports AgronicaCoreEntityFramework
Imports AgronicaCoreVarieBIZ
Imports Newtonsoft.Json
Imports AgronicaCoreUtility

' Per consentire la chiamata di questo servizio Web dallo script utilizzando ASP.NET AJAX, rimuovere il commento dalla riga seguente.
<System.Web.Script.Services.ScriptService()>
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class IsAlive
	Inherits System.Web.Services.WebService

	Private Shared lanToWebSiteBasePath As String
	Private Shared objP_Server As AgronicaCoreParametri
	Private Shared objP_Super_Server As AgronicaCoreParametri
	Private Shared ReadOnly sitesMapper As New Dictionary(Of String, Tuple(Of String, String))

	Shared Sub New()
		sitesMapper.Add("Agenda", New Tuple(Of String, String)("LinkAgronicaAgenda2010", "/AgronicaAgenda_2010/IsAlive/IsAlive.asmx/IsAlive"))
		sitesMapper.Add("Analisi", New Tuple(Of String, String)("LinkAgronicaAnalisi_2010", "/AgronicaAnalisi_2010/IsAlive/IsAlive.asmx/IsAlive"))
		sitesMapper.Add("PianoConcimazione", New Tuple(Of String, String)("LinkPianoConcimazione_2017", "/PianoConcimazione_2017/IsAlive/IsAlive.asmx/IsAlive"))
		sitesMapper.Add("PUA", New Tuple(Of String, String)("LinkAgronicaPua", "/AgronicaPUA/IsAlive/IsAlive.asmx/IsAlive"))
		sitesMapper.Add("Stampe", New Tuple(Of String, String)("LinkAgronicaStampe_2010", "/AgronicaStampe_2010/IsAlive/IsAlive.asmx/IsAlive"))
		sitesMapper.Add("WebApiProfilatore", New Tuple(Of String, String)("", "/AgronicaWebApiProfilatore/IsAlive/IsAlive.asmx/IsAlive"))
		sitesMapper.Add("GiasBase", New Tuple(Of String, String)("LinkGiasBase", "/GiasBase/Default.aspx"))
		sitesMapper.Add("GiasNG", New Tuple(Of String, String)("LinkAgronicaGiasNG", "/GiasNG/"))
		sitesMapper.Add("Ws_Importa_GIAS", New Tuple(Of String, String)("", "/WS_Importa_GIAS/IsAlive/IsAlive.asmx/IsAlive"))
		sitesMapper.Add("Audit", New Tuple(Of String, String)("LinkAgronicaAudit", "/AgronicaAudit/IsAlive/IsAlive.asmx/IsAlive"))
		sitesMapper.Add("PianiCampionamento", New Tuple(Of String, String)("LinkAgronicaPianiCampionamento", "/AgronicaPianiCampionamento/IsAlive/IsAlive.asmx/IsAlive"))
		sitesMapper.Add("Planning", New Tuple(Of String, String)("LinkAgronicaPlanning", "/AgronicaPlanning/IsAlive/IsAlive.asmx/IsAlive"))
		sitesMapper.Add("Sincronizzatore", New Tuple(Of String, String)("LinkAgronicaSincronizzatore", "/AgronicaSincronizzatoreWeb/IsAlive/IsAlive.asmx/IsAlive"))
		sitesMapper.Add("NetCoreApi", New Tuple(Of String, String)("GiasOnline_NetCore_API", "/AgronicaNetCoreAPI/IsAlive"))
		sitesMapper.Add("HubAgea", New Tuple(Of String, String)("LinkAPIHubAgea", "/api/health/is-alive"))
	End Sub

	<WebMethod()>
	<Script.Services.ScriptMethod()>
	Public Function ReachableDB(InData As Object) As rispostaStandard(Of List(Of ReachableDBsOUT))
		Dim r As New rispostaStandard(Of List(Of ReachableDBsOUT))
		r.RispostaStringa = New List(Of ReachableDBsOUT)

		Dim objReq = JsonConvert.DeserializeObject(Of CoreWS_Generic(Of String))(JsonConvert.SerializeObject(InData))

		Try
			'lettura db su Super_Server
			Dim objParametri_Super_Server As AgronicaCoreParametri =
			Utility.convertStringtoOBJparametri(objReq.objP.objP_super_server)
			Dim connessioni_R As New AgronicaCoreDataProvider.Connessioni
			Dim respSuperServer = connessioni_R.IsAlive(objParametri_Super_Server)

			If respSuperServer Then
				r.RispostaStringa.Add(New ReachableDBsOUT("Super_Server", GetConnectionString(objParametri_Super_Server.StringaConnessione), "", Nothing, True))
			Else
				r.RispostaStringa.Add(New ReachableDBsOUT("Super_Server", GetConnectionString(objParametri_Super_Server.StringaConnessione), Gias.DB_SuperServer_Unreachable, Nothing, False))
			End If

			'se Super_Server andato a buon fine, legge db su Server
			Dim objParametri_Server As AgronicaCoreParametri =
				Utility.convertStringtoOBJparametri(objReq.objP.objP_server)
			Dim imprese_R As New AgronicaCoreAnagrafeBIZ.Impresa_R
			Dim respServer As RispostaStandard = imprese_R.IsAlive(objParametri_Server)

			'Warm up di EF
			Dim msgEF As String = ""
			If respServer.RispostaOK Then
				'se DB server up, warmup di EF
				msgEF = WarmUpEF(objParametri_Server)

				If msgEF <> "" Then
					r.RispostaStringa.Add(New ReachableDBsOUT("Server", GetConnectionString(objParametri_Server.StringaConnessione), "", msgEF, False))
				Else
					r.RispostaStringa.Add(New ReachableDBsOUT("Server", GetConnectionString(objParametri_Server.StringaConnessione), "", Nothing, True))
				End If
			Else
				r.RispostaStringa.Add(New ReachableDBsOUT("Server", GetConnectionString(objParametri_Server.StringaConnessione), Gias.DB_Server_Unreachable, Nothing, False))
			End If

			'se Server e EF andati a buon fine, legge db su Utenti
			Dim objParametri_Utenti As AgronicaCoreParametri =
				Utility.convertStringtoOBJparametri(objReq.objP.objP_utenti)
			Dim utenti_R As New AgronicaCoreUtentiBIZ.Utenti
			Dim respUtenti As RispostaStandard = utenti_R.IsAlive(objParametri_Utenti)

			If respUtenti.RispostaOK Then
				r.RispostaStringa.Add(New ReachableDBsOUT("Utenti", GetConnectionString(objParametri_Utenti.StringaConnessione), "", Nothing, True))
			Else
				r.RispostaStringa.Add(New ReachableDBsOUT("Utenti", GetConnectionString(objParametri_Utenti.StringaConnessione), Gias.DB_Utenti_Unreachable, Nothing, False))
			End If

			r.RispostaOK = True
		Catch ex As Exception
			r.RispostaOK = False
			r.Errore = Gestione_Eccezioni.MessaggioCompletoDataEccezione(ex, False)
		End Try

		Return r

	End Function

	''' <summary>
	''' WarmUp delle tabelle su EF
	''' </summary>
	''' <param name="objParametri_Server"></param>
	''' <returns></returns>
	Private Function WarmUpEF(ByRef objParametri_Server As AgronicaCoreParametri) As String
		Dim err As String = ""

		Try
			Dim gefutils As New Gias_EF_Utility
			Dim EFConnString As String = gefutils.GetEntityConnectionString(objParametri_Server.StringaConnessione)

			Using GiasContext As New Gias_DeveloperServer_Entities(EFConnString)
				Dim imprese = From im In GiasContext.Imprese Select im
			End Using
		Catch ex As Exception
			err = " Sistema di accesso ai dati basato su EntityFramework non attivo."
		End Try

		Return err

	End Function

	<WebMethod()>
	<Script.Services.ScriptMethod()>
	Public Function ReachableSites(InData As Object) As rispostaStandard(Of List(Of ReachableSiteIN))
		Dim r As New rispostaStandard(Of List(Of ReachableSiteIN))

		Dim objReq = JsonConvert.DeserializeObject(Of CoreWS_Generic(Of List(Of String)))(JsonConvert.SerializeObject(InData))
		objP_Server = Utility.convertStringtoOBJparametri(objReq.objP.objP_server)
		objP_Super_Server = Utility.convertStringtoOBJparametri(objReq.objP.objP_super_server)

		Dim sites As List(Of String) = objReq.InData

		Dim configSiti As New AgronicaCoreVarieDAL.Configurazione_Siti_R
		lanToWebSiteBasePath = configSiti.Leggi_Valore(0, "LanToWebSiteBasePath", "", "", objP_Server)

		Try
			r.RispostaStringa = New List(Of ReachableSiteIN)
			r.RispostaOK = True

			If sites.Count = 0 Then
				Return r
			End If

			sites.Where(Function(siteKey) Not IsNothing(siteKey) AndAlso Not siteKey.Equals("")).
				ToList.ForEach(Sub(siteKey)
								   If Not r.RispostaStringa.Any(Function(site As ReachableSiteIN) site.key.Equals(siteKey)) Then
									   r.RispostaStringa.Add(ReadSiteByKey(siteKey))
								   End If
							   End Sub)
		Catch ex As Exception
			r.RispostaOK = False
			r.Errore = Gestione_Eccezioni_2015.MessaggioCompletoDataEccezione(ex, True, source:=True)
		End Try

		Return r

	End Function

	Private Function ReadSiteByKey(siteKey As String) As ReachableSiteIN
		Dim confSiti_R As New AgronicaCoreVarieDAL.Configurazione_Siti_R
		Dim tupleKeyLink As Tuple(Of String, String) =
			If(sitesMapper.ContainsKey(siteKey), sitesMapper(siteKey), New Tuple(Of String, String)("", ""))
		Dim siteVal As String
		Dim dtConfSiti As DataTable

		If String.Compare(siteKey, "HubAgea") = 0 Then
			dtConfSiti = confSiti_R.Leggi(0, tupleKeyLink.Item1, "", "", objP_Super_Server)
			siteVal = If(dtConfSiti.Rows.Count = 1, dtConfSiti(0)("Valore"), "")
		ElseIf tupleKeyLink.Item1 <> "" Then
			dtConfSiti = confSiti_R.Leggi(0, tupleKeyLink.Item1, "", "", objP_Server)

			If Not IsNothing(dtConfSiti) AndAlso dtConfSiti.Rows.Count = 1 Then
				siteVal = dtConfSiti(0)("Valore")
			Else
				dtConfSiti = confSiti_R.Leggi(0, siteKey, "", "", objP_Server)
				siteVal = If(dtConfSiti.Rows.Count = 1, dtConfSiti(0)("Valore"), "")
			End If
		ElseIf tupleKeyLink.Item2 <> "" Then
			siteVal = tupleKeyLink.Item2
		Else
			dtConfSiti = confSiti_R.Leggi(0, siteKey, "", "", objP_Server)
			siteVal = If(dtConfSiti.Rows.Count = 1, dtConfSiti(0)("Valore"), "")
		End If

		Return New ReachableSiteIN(siteKey, PurificaUrl(siteKey, lanToWebSiteBasePath, siteVal))
	End Function

	Private Function PurificaUrl(siteKey As String, basePath As String, url As String) As String
		If Not siteKey.Equals("GiasBase") AndAlso Not siteKey.Equals("GiasNG") AndAlso url.ToLower().EndsWith("gestionerichieste.aspx") Then
			url = url.Replace("GestioneRichieste.aspx", "IsAlive/IsAlive.asmx/IsAlive")
		ElseIf siteKey.Equals("GiasNG") AndAlso url.ToLower().EndsWith("gestionerichieste") Then
			url = url.Replace("GestioneRichieste", "")
		ElseIf siteKey.equals("GiasBase") Then
			url = AggiungiSlashSeNonEsiste(url)
		ElseIf siteKey.Equals("NetCoreApi") AndAlso
			Not url.Contains("/IsAlive?InData=") Then
			url &= "/IsAlive?InData=" & siteKey
		ElseIf siteKey.Equals("HubAgea") Then
			url &= "/api/health/is-alive"
		End If

		If url.ToLower.StartsWith("http") Then
			Return url
		Else
			Return String.Format("{0}{1}", basePath, url)
		End If
	End Function

	Private Function GetConnectionString(ByVal input As String) As String
		Dim result As String = ""
		Dim pattern As String = "Server=([^;]+)|Initial Catalog=([^;]+)"
		Dim matches = Regex.Matches(input, pattern, RegexOptions.None, TimeSpan.FromSeconds(3))

		Dim serverValue As String = ""
		Dim catalogValue As String = ""

		For Each match In matches
			If match.Groups(1).Success Then
				serverValue = match.Groups(1).Value
			ElseIf match.Groups(2).Success Then
				catalogValue = match.Groups(2).Value
			End If
		Next

		If serverValue <> "" AndAlso catalogValue <> "" Then
			result = "Server=" & serverValue & "; Initial Catalog=" & catalogValue
		ElseIf serverValue <> "" Then
			result = "Server=" & serverValue
		ElseIf catalogValue <> "" Then
			result = "Initial Catalog=" & catalogValue
		End If

		Return result

	End Function

	Private Function AggiungiSlashSeNonEsiste(ByVal Percorso As String) As String
		If String.IsNullOrEmpty(Percorso) Then
			Return String.Empty
		End If

		If Not Percorso.ToLowerInvariant.Contains("http") AndAlso
			Not Percorso.StartsWith("/") Then
			Percorso = "/" & Percorso
		End If

		If Not Percorso.EndsWith("/") Then
			Return Percorso & "/"
		Else
			Return Percorso
		End If
	End Function

End Class