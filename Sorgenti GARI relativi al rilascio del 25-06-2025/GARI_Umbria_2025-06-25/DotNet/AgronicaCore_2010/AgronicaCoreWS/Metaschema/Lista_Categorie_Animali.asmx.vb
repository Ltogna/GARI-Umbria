Imports System.Web.Services
Imports System.ComponentModel
Imports AgronicaCoreVarieBIZ
Imports AgronicaCoreDataProvider
Imports AgronicaCoreDataProvider.AgronicaCoreParametri
Imports AgronicaCoreDTOStd.InData.Metaschema
Imports AgronicaCoreMetaSchemaDAL
Imports Newtonsoft.Json

<Script.Services.ScriptService()>
<WebService(Namespace:="http://tempuri.org/")>
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<ToolboxItem(False)>
Public Class Lista_Categorie_Animali
	Inherits System.Web.Services.WebService

	<WebMethod(EnableSession:=True)>
	<Script.Services.ScriptMethod()>
	Public Function get_Lista_Categorie_Animali_NG(InData As CoreWS_Generic(Of get_Lista_Categorie_Animali)) As RispostaStandard
		Dim r As New RispostaStandard

		Dim objP_server As AgronicaCoreParametri
		If InData.objP.objP_server = "" Then
			objP_server = HttpContext.Current.Session("ASG_objParametri_Server")
		Else
			objP_server = Utility.convertStringtoOBJparametri(InData.objP.objP_server)
		End If

		Dim genCod As Integer = InData.InData.gen_cod
		Dim speCod As Integer = InData.InData.spe_cod
		Dim iproCod As Integer = InData.InData.ipro_cod

		Dim categorieAnimali_R As New Lista_Categorie_Animali_R
		Try
			Dim strP As String = "get_Lista_Categorie_Animali" & InData.InData.gen_cod & "_" & InData.InData.spe_cod & "_" & InData.InData.ipro_cod

			If IsNothing(HttpContext.Current.Cache(strP)) Then
				Dim listaCategorie = categorieAnimali_R.Leggi(genCod, speCod, iproCod, -1, 0, "", "",
															  enumSelezioneVariabile.Selezione_TabellaDatiMinimi, "", "",
															  objP_server)

				Dim strRisp = JsonConvert.SerializeObject(listaCategorie)
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
	Public Function get_Lista_Categorie_Animali(gen_cod As Integer, spe_cod As Integer, ipro_cod As Integer, objP_server As String) As RispostaStandard
		Dim r As New RispostaStandard

		Dim objParams_Server As AgronicaCoreParametri
		If objP_server = "" Then
			objParams_Server = HttpContext.Current.Session("ASG_objParametri_Server")
		Else
			objParams_Server = Utility.convertStringtoOBJparametri(objP_server)
		End If

		Dim categorieAnimali_R As New Lista_Categorie_Animali_R
		Try
			Dim strP As String = "get_Lista_Categorie_Animali" & gen_cod & "_" & spe_cod & "_" & ipro_cod
			If IsNothing(HttpContext.Current.Cache(strP)) Then
				Dim listaCategorie = categorieAnimali_R.Leggi(gen_cod, spe_cod, ipro_cod, -1, 0, "", "",
															  enumSelezioneVariabile.Selezione_TabellaDatiMinimi,
															  "", "", objParams_Server)

				Dim strRisp = JsonConvert.SerializeObject(listaCategorie)
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