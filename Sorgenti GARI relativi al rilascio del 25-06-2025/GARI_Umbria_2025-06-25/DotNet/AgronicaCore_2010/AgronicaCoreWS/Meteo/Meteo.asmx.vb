
Imports System.Web.Services
Imports System.Web.Script.Serialization
Imports AgronicaCoreDataProvider
Imports AgronicaCoreMeteoCommon
Imports AgronicaCoreVarieBIZ
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports AgronicaCoreUtility
Imports AgronicaCoreDTOStd.InData.Widgets
Imports AgronicaCoreUtentiDAL
Imports AgronicaCoreDataProvider.AgronicaCoreParametri
Imports AgronicaCoreMeteoBiz
Imports AgronicaCoreModelliPrevisionaliCommon


' Per consentire la chiamata di questo servizio Web dallo script utilizzando ASP.NET AJAX, rimuovere il commento dalla riga seguente.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")>
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<System.Web.Script.Services.ScriptService()>
Public Class Meteo
    Inherits System.Web.Services.WebService


    <WebMethod()>
    <Script.Services.ScriptMethod()>
    Public Function ModelliPrevisionali_ElaboraIndicatori(ByVal InData As Object) As rispostaStandard(Of OutputRisultatoIndicatori)

        Dim risp As New rispostaStandard(Of OutputRisultatoIndicatori)
        Dim a As New JsonSerializerSettings With {.DateTimeZoneHandling = DateTimeZoneHandling.Local}

        Dim params As CoreWS_Generic(Of Widget_Modelli_Previsionali_Indicatori_IN) =
            JsonConvert.DeserializeObject(Of CoreWS_Generic(Of Widget_Modelli_Previsionali_Indicatori_IN))(JsonConvert.SerializeObject(InData), a)

        If params.objP.objP_server = "" Then
            risp.Errore = "objP_server non valorizzato"
            Return risp
        End If

        If params.objP.objP_utenti = "" Then
            risp.Errore = "objP_utenti non valorizzato"
            Return risp
        End If

        Try

            Dim piva As String = params.InData.Piva

            'Disattivato causa errore lato Angular, da correggere/riattivare una volta risolto il passaggio date
            Dim parExtra As String = "" 'JsonConvert.SerializeObject(params.InData.ParamExtra)

            Dim objParametri_Server As AgronicaCoreParametri = AgronicaCoreDataProvider.Utility.convertStringtoOBJparametri(params.objP.objP_server)
            Dim objParametri_Utenti As AgronicaCoreParametri = AgronicaCoreDataProvider.Utility.convertStringtoOBJparametri(params.objP.objP_utenti)
            InizializzaLingua(objParametri_Server, objParametri_Utenti)

            Dim modelliHlp As New ModelliCommon(objParametri_Server)

            risp.RispostaStringa = modelliHlp.CalcolaIndicatori(piva, parExtra)
            risp.RispostaOK = True

        Catch ex As Exception

            risp.RispostaOK = False
            risp.Errore = "Errore durante l'operazione: " & Gestione_Eccezioni.MessaggioCompletoDataEccezione(ex, False)
        End Try

        Return risp
    End Function


    <WebMethod()>
    <Script.Services.ScriptMethod()>
    Public Function ElaboraMonitoraggioSuolo(ByVal InData As Object) As rispostaStandard(Of AgronicaCoreMeteoBiz.RisultatoMeteoMonitoraggioSuolo)

        Dim risp As New rispostaStandard(Of AgronicaCoreMeteoBiz.RisultatoMeteoMonitoraggioSuolo) With {
            .RispostaStringa = New AgronicaCoreMeteoBiz.RisultatoMeteoMonitoraggioSuolo,
            .RispostaOK = False
        }

        Dim a As New JsonSerializerSettings With {.DateTimeZoneHandling = DateTimeZoneHandling.Local}

        Dim params As CoreWS_Generic(Of String) =
            JsonConvert.DeserializeObject(Of CoreWS_Generic(Of String))(JsonConvert.SerializeObject(InData), a)

        If params.objP.objP_server = "" Then
            risp.Errore = "objP_server non valorizzato"
            Return risp
        End If

        If params.objP.objP_utenti = "" Then
            risp.Errore = "objP_utenti non valorizzato"
            Return risp
        End If


        Try

            Dim SxC As New AgronicaCoreMeteoDAL.DSS_Stazioni_X_Controllo_Reader
            Dim objParametri_Server As AgronicaCoreParametri = AgronicaCoreDataProvider.Utility.convertStringtoOBJparametri(params.objP.objP_server)
            Dim objParametri_Utenti As AgronicaCoreParametri = AgronicaCoreDataProvider.Utility.convertStringtoOBJparametri(params.objP.objP_utenti)
            InizializzaLingua(objParametri_Server, objParametri_Utenti)

            Dim piva = params.InData
            Dim dtSxC = SxC.Leggi(piva, Nothing, objParametri_Server)

            If dtSxC IsNot Nothing AndAlso dtSxC.Rows.Count > 0 Then

                Dim objMeteoNT As New AgronicaCoreWebService.MeteoNT
                Dim jss = New JavaScriptSerializer With {
                    .MaxJsonLength = Integer.MaxValue
                }

                For Each row In dtSxC.Rows

                    Dim par = JObject.Parse(row("Parametri").ToString)

                    Dim objParams As New JObject(
                        New JProperty("TipoSorgente", CInt(row("Tipo_Sorgente"))),
                        New JProperty("Sorgente", CInt(row("Stazione_Cod"))),
                        New JProperty("Parametri", par("sensore_15"))
                        )

                    Dim ss As String = objMeteoNT.DatiMeteoElaboraMonitoraggioSuolo(objParams, objParametri_Server)

                    Dim tmp_r = jss.Deserialize(Of rispostaStandard(Of RisultatoMeteoMonitoraggioSuolo.Stazione))(ss)

                    If tmp_r.RispostaOK Then

                        risp.RispostaStringa.Stazioni.Add(tmp_r.RispostaStringa)
                    End If
                Next
            End If

            risp.RispostaOK = True

        Catch ex As Exception
            risp.RispostaOK = False
            risp.Errore = "Errore durante l'operazione: " & Gestione_Eccezioni.MessaggioCompletoDataEccezione(ex, False)

        End Try

        Return risp

    End Function


    <WebMethod(EnableSession:=True)>
    <Script.Services.ScriptMethod()>
    Public Function DatiMeteo_ElaboraRiepilogo(ByVal InData As Object) As rispostaStandard(Of AgronicaCoreMeteoBiz.RisultatoMeteoRiepilogo)

        Dim risp As New rispostaStandard(Of AgronicaCoreMeteoBiz.RisultatoMeteoRiepilogo) With {
            .RispostaStringa = New AgronicaCoreMeteoBiz.RisultatoMeteoRiepilogo,
            .RispostaOK = False
        }

        Dim a As New JsonSerializerSettings With {.DateTimeZoneHandling = DateTimeZoneHandling.Local}

        Dim params As CoreWS_Generic(Of String) =
            JsonConvert.DeserializeObject(Of CoreWS_Generic(Of String))(JsonConvert.SerializeObject(InData), a)

        If params.objP.objP_server = "" Then
            risp.Errore = "objP_server non valorizzato"
            Return risp
        End If

        If params.objP.objP_utenti = "" Then
            risp.Errore = "objP_utenti non valorizzato"
            Return risp
        End If

        Dim objParametri_Server As AgronicaCoreParametri = AgronicaCoreDataProvider.Utility.convertStringtoOBJparametri(params.objP.objP_server)
        Dim objParametri_Utenti As AgronicaCoreParametri = AgronicaCoreDataProvider.Utility.convertStringtoOBJparametri(params.objP.objP_utenti)
        InizializzaLingua(objParametri_Server, objParametri_Utenti)


        Try

            Dim piva = params.InData
            Dim SxM As New AgronicaCoreMeteoDAL.DSS_Stazioni_X_Monitor_Reader

            Dim dtSxM = SxM.Leggi(piva, objParametri_Server)

            If dtSxM IsNot Nothing AndAlso dtSxM.Rows.Count > 0 Then

                Dim objMeteoNT As New AgronicaCoreWebService.MeteoNT
                Dim jss = New JavaScriptSerializer With {
                    .MaxJsonLength = Integer.MaxValue
                }

                For Each row In dtSxM.Rows

                    Dim objParams As New JObject(
                        New JProperty("TipoSorgente", CInt(row("Tipo_Sorgente"))),
                        New JProperty("Sorgente", CInt(row("Stazione_Cod"))),
                        New JProperty("NumOre", CInt(row("Monitor_Ore")))
                        )

                    Dim ss As String = objMeteoNT.DatiMeteoElaboraRiepilogo(objParams, objParametri_Server)

                    Dim tmp_r = jss.Deserialize(Of rispostaStandard(Of RisultatoMeteoRiepilogo.Stazione))(ss)

                    If tmp_r.RispostaOK Then

                        risp.RispostaStringa.Stazioni.Add(tmp_r.RispostaStringa)
                    End If
                Next
            End If

            risp.RispostaOK = True

        Catch ex As Exception

            risp.RispostaOK = False
            risp.Errore = "Errore durante l'operazione: " & Gestione_Eccezioni.MessaggioCompletoDataEccezione(ex, False)

        End Try

        Return risp

    End Function



    Private Sub InizializzaLingua(objParametri_Server As AgronicaCoreParametri, objParametri_Utenti As AgronicaCoreParametri)

        Dim linguaCodiceISO As String = "it"
        Try
            Dim leggiLingua As New Lingue_Read
            Dim dtLingua As DataTable = leggiLingua.Leggi_datoCODGIAS(objParametri_Server.Lingua_Cod, enumSelezioneVariabile.Selezione_TabellaCompleta, "", "", objParametri_Utenti)
            If Not IsNothing(dtLingua) AndAlso dtLingua.Rows.Count > 0 Then
                linguaCodiceISO = dtLingua.Rows(0)("CodiceISO")
            End If
        Catch

        Finally
            System.Threading.Thread.CurrentThread.CurrentUICulture = New System.Globalization.CultureInfo(linguaCodiceISO)
        End Try



    End Sub


End Class



