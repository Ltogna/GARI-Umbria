Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports AgronicaCoreDataProvider
Imports AgronicaCoreVarieBIZ
Imports AgronicaCoreAuditBIZ
Imports Newtonsoft.Json.Linq
Imports Newtonsoft.Json
Imports AgronicaCoreDataProvider.TipiEnumerativi
Imports AgronicaCoreDataProvider.CostantiPersonalizzate

' Per consentire la chiamata di questo servizio Web dallo script utilizzando ASP.NET AJAX, rimuovere il commento dalla riga seguente.
<System.Web.Script.Services.ScriptService()>
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")>
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<ToolboxItem(False)>
Public Class Audit
    Inherits System.Web.Services.WebService

    <WebMethod()>
    <Script.Services.ScriptMethod()>
    Public Function LeggiRegolamenti(ByVal tipo As Integer, ByVal objP_server As String) As rispostaStandard(Of List(Of AuditRegolamentiModel))
        Dim r As New rispostaStandard(Of List(Of AuditRegolamentiModel))
        Try
            Dim objParametri_Server As AgronicaCoreParametri = Utility.convertStringtoOBJparametri(objP_server)
            Dim xLettura As New AuditAgronicaWS(objParametri_Server)
            Dim rval As List(Of AuditRegolamentiModel) = xLettura.LeggiRegolamenti(tipo)
            'Dim xLettura As New AuditRegolamenti_R
            'Dim rval As List(Of AuditRegolamentiModel) = xLettura.Leggi(tipo, objParametri_Server)
            r.RispostaOK = True
            r.RispostaStringa = rval
        Catch ex As Exception
            r.RispostaOK = False
            r.Errore = "Errore durante l'operazione: " & AgronicaCoreUtility.Gestione_Eccezioni.MessaggioCompletoDataEccezione(ex, False)
        End Try
        Return r
    End Function

    <WebMethod()>
    <Script.Services.ScriptMethod()>
    Public Function LeggiStati(ByVal tipo As Integer, ByVal objP_server As String) As rispostaStandard(Of List(Of AuditStatiModel))
        Dim r As New rispostaStandard(Of List(Of AuditStatiModel))
        Try
            Dim objParametri_Server As AgronicaCoreParametri = Utility.convertStringtoOBJparametri(objP_server)
            Dim xLettura As New AuditAgronicaWS(objParametri_Server)
            Dim rval As List(Of AuditStatiModel) = xLettura.LeggiStati(tipo)
            r.RispostaOK = True
            r.RispostaStringa = rval
        Catch ex As Exception
            r.RispostaOK = False
            r.Errore = "Errore durante l'operazione: " & AgronicaCoreUtility.Gestione_Eccezioni.MessaggioCompletoDataEccezione(ex, False)
        End Try
        Return r
    End Function

    <WebMethod()>
    <Script.Services.ScriptMethod()>
    Public Function LeggiCampi(ByVal tipo As Integer, ByVal cod_reg As Integer, ByVal objP_server As String) As rispostaStandard(Of List(Of AuditCampiModel))
        Dim r As New rispostaStandard(Of List(Of AuditCampiModel))
        Try
            Dim objParametri_Server As AgronicaCoreParametri = Utility.convertStringtoOBJparametri(objP_server)
            Dim xLettura As New AuditAgronicaWS(objParametri_Server)
            Dim rval As List(Of AuditCampiModel) = xLettura.LeggiCampi(tipo, cod_reg)
            r.RispostaOK = True
            r.RispostaStringa = rval
        Catch ex As Exception
            r.RispostaOK = False
            r.Errore = "Errore durante l'operazione: " & AgronicaCoreUtility.Gestione_Eccezioni.MessaggioCompletoDataEccezione(ex, False)
        End Try
        Return r
    End Function

    <WebMethod()>
    <Script.Services.ScriptMethod()>
    Public Function LeggiDisposizioni(ByVal tipo As Integer, ByVal cod_reg As Integer, ByVal campo As Integer, ByVal cod_disp As Integer, ByVal objP_server As String) As rispostaStandard(Of List(Of AuditDisposizioniModel))
        Dim r As New rispostaStandard(Of List(Of AuditDisposizioniModel))
        Try
            Dim objParametri_Server As AgronicaCoreParametri = Utility.convertStringtoOBJparametri(objP_server)
            Dim xLettura As New AuditAgronicaWS(objParametri_Server)
            Dim rval As List(Of AuditDisposizioniModel) = xLettura.LeggiDisposizioni(tipo, cod_reg, campo, cod_disp)
            r.RispostaOK = True
            r.RispostaStringa = rval
        Catch ex As Exception
            r.RispostaOK = False
            r.Errore = "Errore durante l'operazione: " & AgronicaCoreUtility.Gestione_Eccezioni.MessaggioCompletoDataEccezione(ex, False)
        End Try
        Return r
    End Function

    <WebMethod()>
    <Script.Services.ScriptMethod()>
    Public Function LeggiSezioni(ByVal tipo As Integer, ByVal cod_reg As Integer, ByVal cod_dis As Integer, ByVal parte As Integer, ByVal data As String, ByVal objP_server As String) As rispostaStandard(Of List(Of AuditSezioniModel))
        Dim r As New rispostaStandard(Of List(Of AuditSezioniModel))
        Try
            Dim objParametri_Server As AgronicaCoreParametri = Utility.convertStringtoOBJparametri(objP_server)
            Dim xLettura As New AuditAgronicaWS(objParametri_Server)
            Dim rval As List(Of AuditSezioniModel) = xLettura.LeggiSezioni(tipo, cod_reg, cod_dis, parte, data)
            'Dim xLettura As New AuditSezioni_R
            'Dim rval As List(Of AuditSezioniModel) = xLettura.Leggi(tipo, cod_reg, cod_dis, parte, data_validita, objParametri_Server)
            r.RispostaOK = True
            r.RispostaStringa = rval
        Catch ex As Exception
            r.RispostaOK = False
            r.Errore = "Errore durante l'operazione: " & AgronicaCoreUtility.Gestione_Eccezioni.MessaggioCompletoDataEccezione(ex, False)
        End Try
        Return r
    End Function

    <WebMethod()>
    <Script.Services.ScriptMethod()>
    Public Function LeggiDisposizioniAttive(ByVal tipo As Integer, ByVal cod_reg As Integer, ByVal piva As String, ByVal data As String, ByVal objP_server As String) As rispostaStandard(Of List(Of AuditDisposizioniModel))
        Dim r As New rispostaStandard(Of List(Of AuditDisposizioniModel))
        Dim objParametri_Server As AgronicaCoreParametri = Utility.convertStringtoOBJparametri(objP_server)
        Try
            Dim xLettura As New AuditCheckList
            Dim data_rif As Date = AGRODATAINIZIO
            If data <> "" Then
                data_rif = CDate(data)
            End If
            Dim rval As List(Of AuditDisposizioniModel) = xLettura.LeggiDisposizioniAttive(tipo, cod_reg, piva, data_rif, objParametri_Server)
            r.RispostaOK = True
            r.RispostaStringa = rval
        Catch ex As Exception
            r.RispostaOK = False
            r.Errore = "Errore durante l'operazione: " & AgronicaCoreUtility.Gestione_Eccezioni.MessaggioCompletoDataEccezione(ex, False)
        End Try
        Return r
    End Function


    <WebMethod()>
    <Script.Services.ScriptMethod()>
    Public Function LeggiDisposizioniAttiveTrasporti(ByVal tipo As Integer, ByVal cod_reg As Integer, ByVal disposizione_cod As Integer, ByVal piva As String, ByVal data As String, ByVal objP_server As String) As rispostaStandard(Of List(Of AuditDisposizioniModel))
        Dim r As New rispostaStandard(Of List(Of AuditDisposizioniModel))
        Dim objParametri_Server As AgronicaCoreParametri = Utility.convertStringtoOBJparametri(objP_server)
        Try
            Dim xLettura As New AuditCheckList
            Dim data_rif As Date = AGRODATAINIZIO
            If data <> "" Then
                data_rif = CDate(data)
            End If
            Dim rval As List(Of AuditDisposizioniModel) = xLettura.LeggiDisposizioniAttive(tipo, cod_reg, piva, data_rif, objParametri_Server, disposizione_cod)
            r.RispostaOK = True
            r.RispostaStringa = rval
        Catch ex As Exception
            r.RispostaOK = False
            r.Errore = "Errore durante l'operazione: " & AgronicaCoreUtility.Gestione_Eccezioni.MessaggioCompletoDataEccezione(ex, False)
        End Try
        Return r
    End Function


    <WebMethod()>
    <Script.Services.ScriptMethod()>
    Public Function LeggiCodici(ByVal tipo As Integer, ByVal cod_reg As Integer, ByVal cod_aud As Integer, ByVal cod_dis As Integer, ByVal cod_sez As Integer, ByVal data As String, ByVal objP_server As String) As rispostaStandard(Of List(Of AuditRisposteModel))
        Dim r As New rispostaStandard(Of List(Of AuditRisposteModel))
        Dim objParametri_Server As AgronicaCoreParametri = Utility.convertStringtoOBJparametri(objP_server)
        Try
            Dim xLettura As New AuditCheckList
            Dim rval As List(Of AuditRisposteModel) = xLettura.LeggiCodici(tipo, cod_reg, cod_aud, cod_dis, cod_sez, data, objParametri_Server)
            r.RispostaOK = True
            r.RispostaStringa = rval
        Catch ex As Exception
            r.RispostaOK = False
            r.Errore = "Errore durante l'operazione: " & AgronicaCoreUtility.Gestione_Eccezioni.MessaggioCompletoDataEccezione(ex, False)
        End Try
        Return r
    End Function

    <WebMethod()>
    <Script.Services.ScriptMethod()>
    Public Function LeggiCodiciDefault(ByVal tipo As Integer, ByVal cod_reg As Integer, ByVal cod_dis As Integer, ByVal cod_sez As Integer, ByVal data As String, ByVal risposte As String, ByVal objP_server As String) As rispostaStandard(Of List(Of AuditFormModel))
        Dim r As New rispostaStandard(Of List(Of AuditFormModel))
        Dim objParametri_Server As AgronicaCoreParametri = Utility.convertStringtoOBJparametri(objP_server)
        Try
            Dim xLettura As New AuditCheckList
            Dim rval As List(Of AuditFormModel) = xLettura.LeggiCodiciDefault(tipo, cod_reg, cod_dis, cod_sez, data, risposte, objParametri_Server)
            r.RispostaOK = True
            r.RispostaStringa = rval
        Catch ex As Exception
            r.RispostaOK = False
            r.Errore = "Errore durante l'operazione: " & AgronicaCoreUtility.Gestione_Eccezioni.MessaggioCompletoDataEccezione(ex, False)
        End Try
        Return r
    End Function

    <WebMethod()>
    <Script.Services.ScriptMethod()>
    Public Function LeggiCodiciDeroga(ByVal tipo As Integer, ByVal cod_reg As Integer, ByVal cod_dis As Integer, ByVal cod_sez As Integer, ByVal data As String, ByVal risposte As String, ByVal objP_server As String) As rispostaStandard(Of List(Of AuditFormModel))
        Dim r As New rispostaStandard(Of List(Of AuditFormModel))
        Dim objParametri_Server As AgronicaCoreParametri = Utility.convertStringtoOBJparametri(objP_server)
        Try
            Dim xLettura As New AuditCheckList
            Dim rval As List(Of AuditFormModel) = xLettura.LeggiCodiciDeroga(tipo, cod_reg, cod_dis, cod_sez, data, risposte, objParametri_Server)
            r.RispostaOK = True
            r.RispostaStringa = rval
        Catch ex As Exception
            r.RispostaOK = False
            r.Errore = "Errore durante l'operazione: " & AgronicaCoreUtility.Gestione_Eccezioni.MessaggioCompletoDataEccezione(ex, False)
        End Try
        Return r
    End Function

    <WebMethod()>
    <Script.Services.ScriptMethod()>
    Public Function LeggiInterviste(ByVal tipo As Integer, ByVal cod_reg As Integer, ByVal cod_int As Integer, ByVal piva As String, ByVal inizio As String, ByVal fine As String, ByVal objP_server As String) As rispostaStandard(Of List(Of AuditIntervisteModel))
        Dim r As New rispostaStandard(Of List(Of AuditIntervisteModel))
        Dim objParametri_Server As AgronicaCoreParametri = Utility.convertStringtoOBJparametri(objP_server)
        Try
            Dim xLettura As New AuditProfilazione
            Dim data_inizio As Date = AGRODATAINIZIO
            Dim data_fine As Date = AGRODATAFINE
            If inizio IsNot Nothing Then
                data_inizio = CDate(inizio)
            End If
            If fine IsNot Nothing Then
                data_fine = CDate(fine)
            End If
            Dim rval As List(Of AuditIntervisteModel) = xLettura.LeggiInterviste(tipo, cod_reg, cod_int, piva, data_inizio, data_fine, 0, objParametri_Server)
            r.RispostaOK = True
            r.RispostaStringa = rval
        Catch ex As Exception
            r.RispostaOK = False
            r.Errore = "Errore durante l'operazione: " & AgronicaCoreUtility.Gestione_Eccezioni.MessaggioCompletoDataEccezione(ex, False)
        End Try
        Return r
    End Function



    <WebMethod()>
    <Script.Services.ScriptMethod()>
    Public Function LeggiIntervisteKendo(ByVal tipo As Integer, ByVal cod_reg As Integer, ByVal cod_int As Integer, ByVal piva As String, ByVal inizio As String, ByVal fine As String, ByVal objP_server As String) As RispostaStandard
        Dim r As New RispostaStandard
        Dim objParametri_Server As AgronicaCoreParametri = Utility.convertStringtoOBJparametri(objP_server)
        Try
            Dim xLettura As New AuditProfilazione
            Dim data_inizio As Date = AGRODATAINIZIO
            Dim data_fine As Date = AGRODATAFINE
            If inizio IsNot Nothing Then
                data_inizio = CDate(inizio)
            End If
            If fine IsNot Nothing Then
                data_fine = CDate(fine)
            End If
            Dim rval As List(Of AuditIntervisteModel) = xLettura.LeggiInterviste(tipo, cod_reg, cod_int, piva, data_inizio, data_fine, 0, objParametri_Server)
            Dim serializerSettings As New JsonSerializerSettings With {.ReferenceLoopHandling = ReferenceLoopHandling.Ignore}
            r.RispostaOK = True
            r.RispostaStringa = JsonConvert.SerializeObject(rval.ToList(), Formatting.None, serializerSettings)
        Catch ex As Exception
            r.RispostaOK = False
            r.Errore = "Errore durante l'operazione: " & AgronicaCoreUtility.Gestione_Eccezioni.MessaggioCompletoDataEccezione(ex, False)
        End Try
        Return r
    End Function

    <WebMethod()>
    <Script.Services.ScriptMethod()>
    Public Function ScriviInterviste(ByVal tipo As Integer, ByVal cod_reg As Integer, ByVal cod_int As Integer, ByVal nome_int As String, ByVal piva As String, ByVal inizio As String, ByVal fine As String, ByVal risposte As String, ByVal objP_server As String) As rispostaStandard(Of Boolean)
        Dim r As New rispostaStandard(Of Boolean)
        Dim objParametri_Server As AgronicaCoreParametri = Utility.convertStringtoOBJparametri(objP_server)
        Try
            Dim xScrittura As New AuditProfilazione
            Dim rval As Boolean = False
            Dim data_inizio As Date = AGRODATAINIZIO
            Dim data_fine As Date = AGRODATAFINE
            If inizio <> "" Then
                data_inizio = CDate(inizio)
            End If
            If fine <> "" Then
                data_fine = CDate(fine)
            End If
            rval = xScrittura.ScriviInterviste(tipo, cod_reg, cod_int, nome_int, piva, data_inizio, data_fine, risposte, objParametri_Server)
            r.RispostaOK = True
            r.RispostaStringa = rval
        Catch ex As Exception
            r.RispostaOK = False
            r.Errore = "Errore durante l'operazione: " & AgronicaCoreUtility.Gestione_Eccezioni.MessaggioCompletoDataEccezione(ex, False)
        End Try
        Return r
    End Function

    <WebMethod()>
    <Script.Services.ScriptMethod()>
    Public Function CancellaInterviste(ByVal tipo As Integer, ByVal cod_reg As Integer, ByVal cod_int As Integer, ByVal piva As String, ByVal objP_server As String) As rispostaStandard(Of Boolean)
        Dim r As New rispostaStandard(Of Boolean)
        Dim objParametri_Server As AgronicaCoreParametri = Utility.convertStringtoOBJparametri(objP_server)
        Try
            Dim xScrittura As New AuditProfilazione
            Dim rval As Boolean = False
            If cod_int <> 0 Then
                rval = xScrittura.CancellaInterviste(tipo, cod_reg, cod_int, piva, objParametri_Server)
            End If
            r.RispostaOK = True
            r.RispostaStringa = rval
        Catch ex As Exception
            r.RispostaOK = False
            r.Errore = "Errore durante l'operazione: " & AgronicaCoreUtility.Gestione_Eccezioni.MessaggioCompletoDataEccezione(ex, False)
        End Try
        Return r
    End Function

    <WebMethod()>
    <Script.Services.ScriptMethod()>
    Public Function LeggiRisposteInterviste(ByVal tipo As Integer, ByVal cod_reg As Integer, ByVal cod_int As Integer, ByVal piva As String, ByVal objP_server As String) As rispostaStandard(Of List(Of AuditRisposteIntervisteModel))
        Dim r As New rispostaStandard(Of List(Of AuditRisposteIntervisteModel))
        Dim objParametri_Server As AgronicaCoreParametri = Utility.convertStringtoOBJparametri(objP_server)
        Try
            Dim serializerSettings As New JsonSerializerSettings With {.ReferenceLoopHandling = ReferenceLoopHandling.Ignore}
            Dim xLettura As New AuditProfilazione
            Dim rval As List(Of AuditRisposteIntervisteModel) = xLettura.LeggiRisposteInterviste(tipo, cod_reg, cod_int, piva, objParametri_Server)
            r.RispostaOK = True
            r.RispostaStringa = rval
        Catch ex As Exception
            r.RispostaOK = False
            r.Errore = "Errore durante l'operazione: " & AgronicaCoreUtility.Gestione_Eccezioni.MessaggioCompletoDataEccezione(ex, False)
        End Try
        Return r
    End Function

    <WebMethod()>
    <Script.Services.ScriptMethod()>
    Public Function LeggiRisposteIntervisteKendo(ByVal tipo As Integer, ByVal cod_reg As Integer, ByVal cod_int As Integer, ByVal piva As String, ByVal objP_server As String) As RispostaStandard
        Dim r As New RispostaStandard
        Dim objParametri_Server As AgronicaCoreParametri = Utility.convertStringtoOBJparametri(objP_server)
        Try
            Dim serializerSettings As New JsonSerializerSettings With {.ReferenceLoopHandling = ReferenceLoopHandling.Ignore}
            Dim xLettura As New AuditProfilazione
            Dim rval As List(Of AuditRisposteIntervisteModel) = xLettura.LeggiRisposteInterviste(tipo, cod_reg, cod_int, piva, objParametri_Server)
            r.RispostaOK = True
            r.RispostaStringa = JsonConvert.SerializeObject(rval.ToList(), Formatting.None, serializerSettings)
        Catch ex As Exception
            r.RispostaOK = False
            r.Errore = "Errore durante l'operazione: " & AgronicaCoreUtility.Gestione_Eccezioni.MessaggioCompletoDataEccezione(ex, False)
        End Try
        Return r
    End Function

    <WebMethod()>
    <Script.Services.ScriptMethod()>
    Public Function LeggiAudit(ByVal tipo As Integer, ByVal cod_reg As Integer, ByVal cod_aud As Integer, ByVal piva As String, ByVal objP_server As String, ByVal objP_utenti As String) As rispostaStandard(Of List(Of AuditModel))
        Dim r As New rispostaStandard(Of List(Of AuditModel))
        Dim objParametri_Server As AgronicaCoreParametri = Utility.convertStringtoOBJparametri(objP_server)
        Dim objParametri_Utenti As AgronicaCoreParametri = Utility.convertStringtoOBJparametri(objP_utenti)
        Try
            Dim xLettura As New AuditCheckList
            Dim rval As List(Of AuditModel) = xLettura.LeggiAudit(cod_aud, tipo, cod_reg, piva, AGRODATAINIZIO, AGRODATAFINE, objParametri_Server, objParametri_Utenti)
            r.RispostaOK = True
            r.RispostaStringa = rval
        Catch ex As Exception
            r.RispostaOK = False
            r.Errore = "Errore durante l'operazione: " & AgronicaCoreUtility.Gestione_Eccezioni.MessaggioCompletoDataEccezione(ex, False)
        End Try
        Return r
    End Function

    <WebMethod()>
    <Script.Services.ScriptMethod()>
    Public Function LeggiAuditKendo(ByVal tipo As Integer, ByVal cod_reg As Integer, ByVal piva As String, ByVal inizio As String, ByVal fine As String, ByVal objP_server As String, ByVal objP_utenti As String) As RispostaStandard
        Dim r As New RispostaStandard
        Dim objParametri_Server As AgronicaCoreParametri = Utility.convertStringtoOBJparametri(objP_server)
        Dim objParametri_Utenti As AgronicaCoreParametri = Utility.convertStringtoOBJparametri(objP_utenti)
        Try
            Dim xLettura As New AuditCheckList
            Dim data_inizio As Date = AGRODATAINIZIO
            Dim data_fine As Date = AGRODATAFINE
            If inizio IsNot Nothing Then
                data_inizio = CDate(inizio)
            End If
            If fine IsNot Nothing Then
                data_fine = CDate(fine)
            End If
            Dim rval As List(Of AuditModel) = xLettura.LeggiAudit(0, tipo, cod_reg, piva, data_inizio, data_fine, objParametri_Server, objParametri_Utenti)
            Dim serializerSettings As New JsonSerializerSettings With {.ReferenceLoopHandling = ReferenceLoopHandling.Ignore}
            r.RispostaOK = True
            r.RispostaStringa = JsonConvert.SerializeObject(rval.ToList(), Formatting.None, serializerSettings)
        Catch ex As Exception
            r.RispostaOK = False
            r.Errore = "Errore durante l'operazione: " & AgronicaCoreUtility.Gestione_Eccezioni.MessaggioCompletoDataEccezione(ex, False)
        End Try
        Return r
    End Function

    <WebMethod()>
    <Script.Services.ScriptMethod()>
    Public Function ScriviAudit(ByVal tipo As Integer, ByVal cod_reg As Integer, ByVal cod_aud As Integer, ByVal piva As String, ByVal data As String, ByVal stato As String, ByVal note As String, ByVal Campionato As Integer, ByVal Rintracciabilita As Integer, ByVal risposte As String, ByVal calcola As String, ByVal objP_server As String) As rispostaStandard(Of Boolean)
        Dim r As New rispostaStandard(Of Boolean)
        Dim objParametri_Server As AgronicaCoreParametri = Utility.convertStringtoOBJparametri(objP_server)
        Try
            Dim xScrittura As New AuditCheckList
            Dim rval As Boolean = False
            If data <> "" Then
                rval = xScrittura.ScriviAudit(tipo, cod_reg, cod_aud, piva, CDate(data), stato, note, Campionato, risposte, calcola, objParametri_Server, Rintracciabilita)
            End If
            r.RispostaOK = True
            r.RispostaStringa = rval
        Catch ex As Exception
            r.RispostaOK = False
            r.Errore = "Errore durante l'operazione: " & AgronicaCoreUtility.Gestione_Eccezioni.MessaggioCompletoDataEccezione(ex, False)
        End Try
        Return r
    End Function

    <WebMethod()>
    <Script.Services.ScriptMethod()>
    Public Function CancellaAudit(ByVal tipo As Integer, ByVal cod_reg As Integer, ByVal cod_aud As Integer, ByVal piva As String, ByVal objP_server As String) As rispostaStandard(Of Boolean)
        Dim r As New rispostaStandard(Of Boolean)
        Dim objParametri_Server As AgronicaCoreParametri = Utility.convertStringtoOBJparametri(objP_server)
        Try
            Dim xScrittura As New AuditCheckList
            Dim rval As Boolean = False
            If cod_aud <> 0 Then
                rval = xScrittura.CancellaAudit(tipo, cod_reg, cod_aud, piva, objParametri_Server)
            End If
            r.RispostaOK = True
            r.RispostaStringa = rval
        Catch ex As Exception
            r.RispostaOK = False
            r.Errore = "Errore durante l'operazione: " & AgronicaCoreUtility.Gestione_Eccezioni.MessaggioCompletoDataEccezione(ex, False)
        End Try
        Return r
    End Function

    <WebMethod()>
    <Script.Services.ScriptMethod()>
    Public Function LeggiAuditRisposte(ByVal tipo As Integer, ByVal cod_reg As Integer, ByVal cod_aud As Integer, ByVal cod_dis As Integer, ByVal parte As Integer, ByVal data As String, ByVal objP_server As String) As rispostaStandard(Of List(Of AuditRisposteModel))
        Dim r As New rispostaStandard(Of List(Of AuditRisposteModel))
        Dim objParametri_Server As AgronicaCoreParametri = Utility.convertStringtoOBJparametri(objP_server)
        Try
            Dim xLettura As New AuditCheckList
            Dim rval As List(Of AuditRisposteModel) = xLettura.LeggiAuditRisposte(tipo, cod_reg, cod_aud, cod_dis, parte, data, objParametri_Server)
            r.RispostaOK = True
            r.RispostaStringa = rval
        Catch ex As Exception
            r.RispostaOK = False
            r.Errore = "Errore durante l'operazione: " & AgronicaCoreUtility.Gestione_Eccezioni.MessaggioCompletoDataEccezione(ex, False)
        End Try
        Return r
    End Function

    <WebMethod()>
    <Script.Services.ScriptMethod()>
    Public Function LeggiAuditPunteggi(ByVal tipo As Integer, ByVal cod_reg As Integer, ByVal cod_aud As Integer, ByVal piva As String, ByVal data As String, ByVal objP_server As String) As rispostaStandard(Of List(Of AuditPunteggiModel))
        Dim r As New rispostaStandard(Of List(Of AuditPunteggiModel))
        Dim objParametri_Server As AgronicaCoreParametri = Utility.convertStringtoOBJparametri(objP_server)
        Try
            Dim data_val As Date = AGRODATAINIZIO
            If data <> "" Then
                data_val = CDate(data)
            End If
            Dim xLettura As New AuditCheckList
            Dim rval As List(Of AuditPunteggiModel) = xLettura.LeggiAuditPunteggi(tipo, cod_reg, cod_aud, piva, data_val, objParametri_Server)
            r.RispostaOK = True
            r.RispostaStringa = rval
        Catch ex As Exception
            r.RispostaOK = False
            r.Errore = "Errore durante l'operazione: " & AgronicaCoreUtility.Gestione_Eccezioni.MessaggioCompletoDataEccezione(ex, False)
        End Try
        Return r
    End Function

    <WebMethod()>
    <Script.Services.ScriptMethod()>
    Public Function CalcolaLivello(ByVal tipo As Integer, ByVal cod_reg As Integer, ByVal piva As String, ByVal cod_dis As Integer, ByVal cod_sez As Integer, ByVal livello As String, ByVal risposte As String, ByVal objP_server As String) As RispostaStandard
        Dim r As New RispostaStandard
        Dim objParametri_Server As AgronicaCoreParametri = Utility.convertStringtoOBJparametri(objP_server)
        Try
            Dim xLettura As New AuditCheckList
            Dim rval As String = xLettura.CalcolaLivello(tipo, cod_reg, piva, cod_dis, cod_sez, livello, risposte, objParametri_Server)
            r.RispostaOK = True
            r.RispostaStringa = rval
        Catch ex As Exception
            r.RispostaOK = False
            r.Errore = "Errore durante l'operazione: " & AgronicaCoreUtility.Gestione_Eccezioni.MessaggioCompletoDataEccezione(ex, False)
        End Try
        Return r
    End Function

End Class