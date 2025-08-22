Imports System.ComponentModel
Imports System.Net.Http
Imports System.Net.Http.Headers
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports Agronica.Helper.Retail
Imports AgronicaCoreDataProvider
Imports AgronicaCoreDataProvider.My.Resources
Imports AgronicaCoreDTOStd.InData.AuthDispatcher
Imports AgronicaCoreDTOStd.InData.Provisioning
Imports AgronicaCoreDTOStd.InData.Provisioning.Retail
Imports AgronicaCoreModelsSTD.AuthDispatcher
Imports AgronicaCoreModelsSTD.provisioning
Imports AgronicaCoreVarieBIZ
Imports AgronicaCoreVarieDAL
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

' Per consentire la chiamata di questo servizio Web dallo script utilizzando ASP.NET AJAX, rimuovere il commento dalla riga seguente.
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")>
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<System.Web.Script.Services.ScriptService()>
<ToolboxItem(False)>
Public Class AuthDispatcher
    Inherits System.Web.Services.WebService

    <WebMethod()>
    <Script.Services.ScriptMethod()>
    Public Function RichiediSignedURL(ByVal InData As Object) As rispostaStandard(Of ElencoUrlFirmati)

        Dim resp As New rispostaStandard(Of ElencoUrlFirmati)

        Dim objParametri = DeserializzaInData(Of RichiestaSignedUrl_In)(InData)

        Try

            Dim cfgRead As New AgronicaCoreVarieDAL.Configurazione_Siti_R

            Dim DT = cfgRead.Leggi(0, CostantiPersonalizzate.AuthDispatcher_Configurations, "", "", objParametri.Server)

            If DT Is Nothing OrElse DT.Rows.Count <> 1 Then
                resp.Errore = "Errore nel recupero delle configurazioni per l'AuthDispatcher."
                Return resp
            End If

            Dim objConf = JObject.Parse(DT.Rows(0)("Valore").ToString)

            Dim baseUrl As String = objConf("BaseUrl").ToString

            Dim fullUrl = baseUrl.TrimEnd("/") & "/" & CostantiPersonalizzate.AuthDispatcher_SignUrlAPI

            Dim cf_client As New HttpClient()

            Dim inputObj As New JArray

            For Each richiesta In objParametri.InData.elencoRichieste

                inputObj.Add(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(richiesta)))

            Next

            Dim content As New StringContent(System.Text.RegularExpressions.Regex.Unescape(inputObj.ToString), Text.Encoding.UTF8, "application/json")
            cf_client.DefaultRequestHeaders.Add("x-api-key", objConf("ApiKey").ToString)

            Dim hr As HttpResponseMessage = cf_client.PostAsync(fullUrl, content).Result

            Select Case hr.StatusCode
                Case Net.HttpStatusCode.OK
                    resp.RispostaOK = True
                    resp.RispostaStringa = LeggiRispostaDispatcher(hr.Content.ReadAsStringAsync().Result)
                Case Net.HttpStatusCode.BadRequest
                    resp.RispostaOK = False
                    resp.Errore = String.Format("400_{0}", hr.Content.ReadAsStringAsync().Result)
                Case Net.HttpStatusCode.Unauthorized
                    resp.RispostaOK = False
                    resp.Errore = String.Format("401_{0}", hr.Content.ReadAsStringAsync().Result)
                Case Net.HttpStatusCode.InternalServerError
                    resp.RispostaOK = False
                    resp.Errore = String.Format("500_{0}", hr.Content.ReadAsStringAsync().Result)
                Case Else
                    resp.RispostaOK = False
                    resp.Errore = String.Format("500_{0}", hr.Content.ReadAsStringAsync().Result)
            End Select

            cf_client.Dispose()

        Catch ex As Exception
            resp.RispostaOK = False
            resp.Errore = ex.Message
        End Try

        Return resp
    End Function



    <WebMethod()>
    <Script.Services.ScriptMethod()>
    Public Function RichiediAbacoURL(ByVal InData As Object) As rispostaStandard(Of ElencoUrlFirmati)

        Dim resp As New rispostaStandard(Of ElencoUrlFirmati)

        Dim objParametri = DeserializzaInData(Of RichiestaAbacoUrl_In)(InData)

        Try

            Dim cfgRead As New AgronicaCoreVarieDAL.Configurazione_Siti_R

            Dim DT = cfgRead.Leggi(0, CostantiPersonalizzate.AuthDispatcher_Configurations, "", "", objParametri.Server)

            If DT Is Nothing OrElse DT.Rows.Count <> 1 Then
                resp.Errore = "Errore nel recupero delle configurazioni per l'AuthDispatcher."
                Return resp
            End If

            Dim objConf = JObject.Parse(DT.Rows(0)("Valore").ToString)

            Dim baseUrl As String = objConf("BaseUrl").ToString

            Dim fullUrl = baseUrl & "/" & CostantiPersonalizzate.AuthDispatcher_AbacoUrlAPI

            Dim cf_client As New HttpClient()

            Dim inputObj As New JArray

            Dim abacoBaseUrl As String
            Select Case objParametri.InData.RichiestaAbacoType
                Case RichiestaAbacoType.Satellite
                    abacoBaseUrl = objConf("AbacoBaseUrlSatellite").ToString
                Case RichiestaAbacoType.Raster
                    abacoBaseUrl = objConf("AbacoBaseUrlRaster").ToString
                Case Else
                    resp.Errore = "Errore nella richiesta: Tipo richiesta abaco non valido."
                    Return resp
            End Select

            For Each richiesta In objParametri.InData.elencoRichieste
                Dim abacoRequest As New RichiestaAbacoUrl With {
                    .BaseUrl = abacoBaseUrl,
                    .Bucket = richiesta.Bucket,
                    .Object = richiesta.Object,
                    .Coords = richiesta.Coords
                }

                inputObj.Add(JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(abacoRequest)))
            Next

            Dim content As New StringContent(System.Text.RegularExpressions.Regex.Unescape(inputObj.ToString), Text.Encoding.UTF8, "application/json")
            cf_client.DefaultRequestHeaders.Add("x-api-key", objConf("ApiKey").ToString)

            Dim hr As HttpResponseMessage = cf_client.PostAsync(fullUrl, content).Result

            Select Case hr.StatusCode
                Case Net.HttpStatusCode.OK
                    resp.RispostaOK = True
                    resp.RispostaStringa = LeggiRispostaDispatcher(hr.Content.ReadAsStringAsync().Result)
                Case Net.HttpStatusCode.BadRequest
                    resp.RispostaOK = False
                    resp.Errore = String.Format("400_{0}", hr.Content.ReadAsStringAsync().Result)
                Case Net.HttpStatusCode.Unauthorized
                    resp.RispostaOK = False
                    resp.Errore = String.Format("401_{0}", hr.Content.ReadAsStringAsync().Result)
                Case Net.HttpStatusCode.InternalServerError
                    resp.RispostaOK = False
                    resp.Errore = String.Format("500_{0}", hr.Content.ReadAsStringAsync().Result)
                Case Else
                    resp.RispostaOK = False
                    resp.Errore = String.Format("500_{0}", hr.Content.ReadAsStringAsync().Result)
            End Select

            cf_client.Dispose()

        Catch ex As Exception
            resp.RispostaOK = False
            resp.Errore = ex.Message
        End Try

        Return resp
    End Function

    <WebMethod()>
    <Script.Services.ScriptMethod()>
    Public Function AutenticaGEE(ByVal InData As Object) As RispostaStandard

        Dim resp As New RispostaStandard

        Dim objParametri = DeserializzaInData(Of String)(InData)

        Try

            Dim cfgRead As New AgronicaCoreVarieDAL.Configurazione_Siti_R

            Dim DT = cfgRead.Leggi(0, CostantiPersonalizzate.AuthDispatcher_Configurations, "", "", objParametri.Server)

            If DT Is Nothing OrElse DT.Rows.Count <> 1 Then
                resp.Errore = "Errore nel recupero delle configurazioni per l'AuthDispatcher."
                Return resp
            End If

            Dim objConf = JObject.Parse(DT.Rows(0)("Valore").ToString)

            Dim baseUrl As String = objConf("BaseUrl").ToString

            Dim fullUrl = System.IO.Path.Combine(baseUrl, CostantiPersonalizzate.AuthDispatcher_AutenticaGEE)

            Dim cf_client As New HttpClient()

            cf_client.DefaultRequestHeaders.Add("x-api-key", objConf("ApiKey").ToString)

            Dim hr As HttpResponseMessage = cf_client.PostAsync(fullUrl, New StringContent(String.Empty)).Result

            Select Case hr.StatusCode
                Case Net.HttpStatusCode.OK
                    resp.RispostaOK = True
                    resp.RispostaStringa = hr.Content.ReadAsStringAsync().Result
                Case Net.HttpStatusCode.BadRequest
                    resp.RispostaOK = False
                    resp.Errore = String.Format("400_{0}", hr.Content.ReadAsStringAsync().Result)
                Case Net.HttpStatusCode.Unauthorized
                    resp.RispostaOK = False
                    resp.Errore = String.Format("401_{0}", hr.Content.ReadAsStringAsync().Result)
                Case Net.HttpStatusCode.InternalServerError
                    resp.RispostaOK = False
                    resp.Errore = String.Format("500_{0}", hr.Content.ReadAsStringAsync().Result)
                Case Else
                    resp.RispostaOK = False
                    resp.Errore = String.Format("500_{0}", hr.Content.ReadAsStringAsync().Result)
            End Select

            cf_client.Dispose()

        Catch ex As Exception
            resp.RispostaOK = False
            resp.Errore = ex.Message
        End Try

        Return resp
    End Function

    <WebMethod()>
    <Script.Services.ScriptMethod()>
    Public Function AutenticaGoogleCloud(ByVal InData As Object) As RispostaStandard

        Dim resp As New RispostaStandard

        Dim objParametri = DeserializzaInData(Of String)(InData)

        Try

            Dim cfgRead As New AgronicaCoreVarieDAL.Configurazione_Siti_R

            Dim DT = cfgRead.Leggi(0, CostantiPersonalizzate.AuthDispatcher_Configurations, "", "", objParametri.Server)

            If DT Is Nothing OrElse DT.Rows.Count <> 1 Then
                resp.Errore = "Errore nel recupero delle configurazioni per l'AuthDispatcher."
                Return resp
            End If

            Dim objConf = JObject.Parse(DT.Rows(0)("Valore").ToString)

            Dim baseUrl As String = objConf("BaseUrl").ToString

            Dim fullUrl = System.IO.Path.Combine(baseUrl, CostantiPersonalizzate.AuthDispatcher_AutenticaGoogleCloud)

            Dim cf_client As New HttpClient()

            cf_client.DefaultRequestHeaders.Add("x-api-key", objConf("ApiKey").ToString)

            Dim hr As HttpResponseMessage = cf_client.PostAsync(fullUrl, New StringContent(String.Empty)).Result

            Select Case hr.StatusCode
                Case Net.HttpStatusCode.OK
                    resp.RispostaOK = True
                    resp.RispostaStringa = hr.Content.ReadAsStringAsync().Result
                Case Net.HttpStatusCode.BadRequest
                    resp.RispostaOK = False
                    resp.Errore = String.Format("400_{0}", hr.Content.ReadAsStringAsync().Result)
                Case Net.HttpStatusCode.Unauthorized
                    resp.RispostaOK = False
                    resp.Errore = String.Format("401_{0}", hr.Content.ReadAsStringAsync().Result)
                Case Net.HttpStatusCode.InternalServerError
                    resp.RispostaOK = False
                    resp.Errore = String.Format("500_{0}", hr.Content.ReadAsStringAsync().Result)
                Case Else
                    resp.RispostaOK = False
                    resp.Errore = String.Format("500_{0}", hr.Content.ReadAsStringAsync().Result)
            End Select

            cf_client.Dispose()

        Catch ex As Exception
            resp.RispostaOK = False
            resp.Errore = ex.Message
        End Try

        Return resp
    End Function

    Private Function LeggiRispostaDispatcher(ByVal result As String) As ElencoUrlFirmati
        Dim elenco As New ElencoUrlFirmati With {
            .elencoUrlFirmati = New List(Of UrlFirmato)
        }

        Dim listaRisultati = JArray.Parse(result)

        For Each risultato In listaRisultati
            elenco.elencoUrlFirmati.Add(Newtonsoft.Json.JsonConvert.DeserializeObject(Of UrlFirmato)(risultato.ToString))
        Next

        Return elenco
    End Function

    Private Class ObjParametri(Of T)

        Public Super_Server As AgronicaCoreParametri
        Public Server As AgronicaCoreParametri
        Public Utenti As AgronicaCoreParametri
        Public InData As T

    End Class

    Private Function DeserializzaInData(Of T)(ByVal InData As Object) As ObjParametri(Of T)

        Dim JsonSettings As New JsonSerializerSettings With {.DateTimeZoneHandling = DateTimeZoneHandling.Local}

        Dim objInData As CoreWS_Generic(Of T) = JsonConvert.DeserializeObject(Of CoreWS_Generic(Of T))(JsonConvert.SerializeObject(InData), JsonSettings)

        Dim objParametri As New ObjParametri(Of T)

        If objInData.objP.objP_super_server IsNot Nothing AndAlso Not objInData.objP.objP_super_server.Equals("") Then
            objParametri.Super_Server = Utility.convertStringtoOBJparametri(objInData.objP.objP_super_server)
        End If
        If objInData.objP.objP_server IsNot Nothing AndAlso Not objInData.objP.objP_server.Equals("") Then
            objParametri.Server = Utility.convertStringtoOBJparametri(objInData.objP.objP_server)
        End If
        If objInData.objP.objP_utenti IsNot Nothing AndAlso Not objInData.objP.objP_utenti.Equals("") Then
            objParametri.Utenti = Utility.convertStringtoOBJparametri(objInData.objP.objP_utenti)
        End If

        objParametri.InData = objInData.InData

        Return objParametri

    End Function

End Class