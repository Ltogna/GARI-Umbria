Imports System.ComponentModel
Imports System.Web.Services
Imports AgronicaCoreDataProvider
Imports AgronicaCoreDTOStd.InData.importazioni
Imports AgronicaCoreModelsSTD.Utility
Imports AgronicaCoreUtility
Imports Newtonsoft.Json

' Per consentire la chiamata di questo servizio Web dallo script utilizzando ASP.NET AJAX, rimuovere il commento dalla riga seguente.
<System.Web.Script.Services.ScriptService()>
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")>
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<ToolboxItem(False)>
Public Class ImportAttivita
    Inherits System.Web.Services.WebService

    <WebMethod(EnableSession:=True)>
    <Script.Services.ScriptMethod()>
    Public Function ImportAttivitaDemetra(InData As Object) As Api_Response
        Return ImportAttivitaFromTipo(InData, AgronicaCoreModello.AppHelper.enum_Dati_App.AttivitaDemetra)
    End Function

    <WebMethod(EnableSession:=True)>
    <Script.Services.ScriptMethod()>
    Public Function ImportAttivitaNewAgri(InData As Object) As Api_Response
        Return ImportAttivitaFromTipo(InData, AgronicaCoreModello.AppHelper.enum_Dati_App.AttivitaNewAgri)
    End Function


    Private Function ImportAttivitaFromTipo(InData As Object, tipo As AgronicaCoreModello.AppHelper.enum_Import_App) As Api_Response
        Dim r As New Api_Response

        Dim objRequest = JsonConvert.DeserializeObject(Of CoreWS_Generic(Of ImportDemetra))(JsonConvert.SerializeObject(InData))
        Dim objParametri_Super_Server As AgronicaCoreParametri = Utility.convertStringtoOBJparametri(objRequest.objP.objP_super_server)
        Dim objParametri_Server As AgronicaCoreParametri = Utility.convertStringtoOBJparametri(objRequest.objP.objP_server)
        Dim objParametri_Utenti As AgronicaCoreParametri = Utility.convertStringtoOBJparametri(objRequest.objP.objP_utenti)

        Try

            Dim errorMessage As String = ""
            Dim objDemetraBIZ_util As New AgronicaCoreDemetraBIZ.Util
            If Not objDemetraBIZ_util.CheckRequest(objRequest, errorMessage) Then
                Throw New Exception(errorMessage)
            End If

            Dim cuaaRequest As String = objRequest.InData.CUAA

            Dim datiStr As String = AgroZip.DeCompressioneBase64(1, objRequest.InData.dati)
            Dim datiRequest As List(Of AgronicaCoreDTOStd.InData.Demetra.Attivita) = JsonConvert.DeserializeObject(Of List(Of AgronicaCoreDTOStd.InData.Demetra.Attivita))(datiStr)

            errorMessage = ""
            Dim objDemetraBIZ As New AgronicaCoreDemetraBIZ.ImportAttivita
            objDemetraBIZ.ImportAttivitaMulti(tipo, cuaaRequest, datiRequest,
                                              errorMessage, objParametri_Super_Server, objParametri_Server, objParametri_Utenti, objRequest.objP.user_Agent)

            If errorMessage <> "" Then
                Throw New Exception(errorMessage)
            End If

            r.message = Api_Response_Message_Type.Ok

        Catch ex As Exception

            r.errore = ex.Message
            r.message = ""

        End Try

        Return r

    End Function
End Class