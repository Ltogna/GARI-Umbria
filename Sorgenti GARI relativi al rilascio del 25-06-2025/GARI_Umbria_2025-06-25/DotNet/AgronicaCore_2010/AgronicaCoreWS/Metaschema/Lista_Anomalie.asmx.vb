Imports System.Web.Services
Imports System.ComponentModel
Imports AgronicaCoreVarieBIZ
Imports AgronicaCoreDataProvider
Imports AgronicaCoreDataProvider.AgronicaCoreParametri
Imports AgronicaCoreMetaSchemaDAL

' Per consentire la chiamata di questo servizio Web dallo script utilizzando ASP.NET AJAX, rimuovere il commento dalla riga seguente.
<System.Web.Script.Services.ScriptService()>
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")>
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<ToolboxItem(False)>
Public Class Lista_Anomalie
	Inherits System.Web.Services.WebService

	<WebMethod(EnableSession:=True)>
	<Script.Services.ScriptMethod()>
	Public Function get_Lista_Anomalie(objP_server As String) As RispostaStandard
		Dim r As New RispostaStandard

		Dim objParams_Server As AgronicaCoreParametri
		If objP_server = "" Then
			objParams_Server = HttpContext.Current.Session("ASG_objParametri_Server")
		Else
			objParams_Server = Utility.convertStringtoOBJparametri(objP_server)
		End If

		Dim zooAnomalie_R As New Lista_Anomalie_R
		Try
			Dim strP As String = "get_Lista_Anomalie"

			If IsNothing(HttpContext.Current.Cache(strP)) Then
				Dim listaAnomalie = zooAnomalie_R.Leggi(0, enumSelezioneVariabile.Selezione_TabellaDatiMinimi, "", "", objParams_Server)
				Dim strRisp = Newtonsoft.Json.JsonConvert.SerializeObject(listaAnomalie)

				r.RispostaOK = True
				r.RispostaStringa = strRisp
			Else
				r.RispostaStringa = HttpContext.Current.Cache(strP)
				r.RispostaOK = True
			End If

		Catch ex As Exception
			r.RispostaOK = False
			r.Errore = Gestione_Eccezioni_2015.MessaggioCompletoDataEccezione(ex, True, source:=True)
		End Try

		Return r

	End Function

End Class