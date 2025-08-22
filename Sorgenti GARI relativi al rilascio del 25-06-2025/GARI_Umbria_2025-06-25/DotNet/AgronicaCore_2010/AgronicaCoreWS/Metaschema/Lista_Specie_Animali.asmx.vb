Imports System.Web.Services
Imports System.ComponentModel
Imports AgronicaCoreVarieBIZ
Imports AgronicaCoreDataProvider
Imports AgronicaCoreEntityFramework
Imports Newtonsoft.Json
Imports AgronicaCoreMetaSchemaDAL
Imports AgronicaCoreDataProvider.AgronicaCoreParametri

' Per consentire la chiamata di questo servizio Web dallo script utilizzando ASP.NET AJAX, rimuovere il commento dalla riga seguente.
<Script.Services.ScriptService()>
<WebService(Namespace:="http://tempuri.org/")>
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<ToolboxItem(False)>
Public Class Lista_Specie_Animali
	Inherits System.Web.Services.WebService

	Protected Overrides Sub Finalize()
		MyBase.Finalize()
	End Sub

	<WebMethod(EnableSession:=True)>
	<Script.Services.ScriptMethod()>
	Public Function get_Lista_Specie_Animali_NG(InData As Object) As RispostaStandard
		Dim r As New RispostaStandard

		InData = JsonConvert.DeserializeObject(Of CoreWS_Generic(Of String))(JsonConvert.SerializeObject(InData))
		Dim genCod As Integer = InData.InData

		Dim objP_Server As AgronicaCoreParametri
		If InData.objP.objP_server = "" Then
			objP_Server = HttpContext.Current.Session("ASG_objParametri_Server")
		Else
			objP_Server = Utility.convertStringtoOBJparametri(InData.objP.objP_server)
		End If

		Dim specieAnimali_R As New Lista_Specie_Animali_R
		Try
			Dim strP As String = "get_Lista_Specie_Animali" & genCod
			If IsNothing(HttpContext.Current.Cache(strP)) Then
				Dim listaSpecie = specieAnimali_R.Leggi(genCod, -1, enumSelezioneVariabile.Selezione_TabellaDatiMinimi, "", "", objP_Server)

				Dim strRisp = JsonConvert.SerializeObject(listaSpecie)
				HttpContext.Current.Cache(strP) = strRisp

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

	<WebMethod(EnableSession:=True)>
	<Script.Services.ScriptMethod()>
	Public Function get_Lista_Specie_Animali(gen_cod As Integer, objP_server As String) As RispostaStandard
		Dim r As New RispostaStandard

		Dim objParams_Server As AgronicaCoreParametri
		If objP_server = "" Then
			objParams_Server = HttpContext.Current.Session("ASG_objParametri_Server")
		Else
			objParams_Server = Utility.convertStringtoOBJparametri(objP_server)
		End If

		Dim specieAnimali_R As New Lista_Specie_Animali_R
		Try
			Dim strP As String = "get_Lista_Specie_Animali" & gen_cod

			If IsNothing(HttpContext.Current.Cache(strP)) Then
				Dim listaSpecie = specieAnimali_R.Leggi(gen_cod, -1, enumSelezioneVariabile.Selezione_TabellaDatiMinimi, "", "", objParams_Server)

				Dim strRisp = JsonConvert.SerializeObject(listaSpecie)
				HttpContext.Current.Cache(strP) = strRisp

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