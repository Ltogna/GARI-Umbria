Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports AgronicaCoreDataProvider
Imports AgronicaCoreDataProvider.CostantiPersonalizzate
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports AgronicaCoreVarieBIZ
Imports AgronicaCoreUtility
Imports AgronicaCoreDataProvider.AgronicaCoreParametri
Imports AgronicaCoreDTOStd
Imports AgronicaControlliGIS
Imports AgronicaCoreModelsSTD
Imports AgronicaCoreDTOStd.InData.Metaschema
Imports AgronicaCoreModelsSTD.metaschema.utilizzi

' Per consentire la chiamata di questo servizio Web dallo script utilizzando ASP.NET AJAX, rimuovere il commento dalla riga seguente.
<System.Web.Script.Services.ScriptService()>
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")>
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<ToolboxItem(False)>
Public Class SpecieVegetali
    Inherits System.Web.Services.WebService

    <WebMethod()>
    <Script.Services.ScriptMethod()>
    Public Function CaricaComboSpecieVegetali_conFiltroUtente_NG(InData As CoreWS_Generic(Of CaricaComboSpecieVegetali_conFiltroUtente)) As RispostaStandard

        Dim r As New RispostaStandard

        If InData.objP.objP_utenti = "" Then
            r.Errore = "objP_utenti non valorizzato"
            Return r
        End If


        Try

            Dim objParametri_Utenti As AgronicaCoreParametri = Utility.convertStringtoOBJparametri(InData.objP.objP_utenti)

            Dim coreWSCache = MemoryCacheFactory.Instance.COREWS_CACHE

            Dim key = "SpecieVegetali-" & objParametri_Utenti.UtenteUsername & "-" & InData.InData.Gru_Cod & "-" & InData.InData.LetteraIniziale & "-" & InData.InData.StringaCerca & "-" & InData.InData.FiltroAggiuntivo & "-" & InData.InData.Ordinamento

            Dim JArrayLista As New JArray()
            'If cache IsNot Nothing Then

            'If cache.Item(key) Is Nothing Then
            Dim objSpecVeg As New AgronicaCoreMetaSchemaDAL.SpecieVegetali_R
            Dim Dt As DataTable = objSpecVeg.SpecieVegetali_GestioneFiltroUtente_Leggi(0,
                                                                    InData.InData.Gru_Cod,
                                                                    InData.InData.LetteraIniziale,
                                                                    InData.InData.StringaCerca,
                                                                    InData.InData.FiltroAggiuntivo,
                                                                    InData.InData.Ordinamento,
                                                                    objParametri_Utenti)

            For Each dr In Dt.Rows
                JArrayLista.Add(New JObject(New JProperty("veg_cod", dr.Item("VEG_COD")), New JProperty("veg_des", dr.Item("VEG_DES")), New JProperty("Gru_Cod", dr.item("Gru_Cod"))))
            Next

            'cache.Item(key) = JArrayLista

            'Else
            '    JArrayLista = cache.Item(key)
            'End If

            'Else



            'End If

            r.RispostaStringa = JsonConvert.SerializeObject(JArrayLista, Formatting.None)
            r.RispostaOK = True

        Catch ex As Exception
            'uso questa funzione per ottenere il Messaggio..:
            r.Errore = "Errore durante l'operazione: " & vbCrLf & Gestione_Eccezioni_2015.MessaggioCompletoDataEccezione(ex, True, source:=True)
        End Try

        Return r

    End Function
    <WebMethod()>
    <Script.Services.ScriptMethod()>
    Public Function CaricaComboSpecieVegetali_conFiltroUtente(objP_utenti As String,
                                                               Gru_Cod As Integer,
                                                               LetteraIniziale As String, StringaCerca As String,
                                                               FiltroAggiuntivo As String, Ordinamento As String) As RispostaStandard

        Dim r As New RispostaStandard

        If objP_utenti = "" Then
            r.Errore = "objP_utenti non valorizzato"
            Return r
        End If


        Try

            Dim objParametri_Utenti As AgronicaCoreParametri = Utility.convertStringtoOBJparametri(objP_utenti)

            Dim coreWSCache = MemoryCacheFactory.Instance.COREWS_CACHE

            Dim key = "SpecieVegetali-" & objParametri_Utenti.UtenteUsername & "-" & Gru_Cod & "-" & LetteraIniziale & "-" & StringaCerca & "-" & FiltroAggiuntivo & "-" & Ordinamento

            Dim JArrayLista As New JArray()
            'If cache IsNot Nothing Then

            'If cache.Item(key) Is Nothing Then
            Dim objSpecVeg As New AgronicaCoreMetaSchemaDAL.SpecieVegetali_R
            Dim Dt As DataTable = objSpecVeg.SpecieVegetali_GestioneFiltroUtente_Leggi(0,
                                                                    Gru_Cod,
                                                                    LetteraIniziale,
                                                                    StringaCerca,
                                                                    FiltroAggiuntivo,
                                                                    Ordinamento,
                                                                    objParametri_Utenti)

            For Each dr In Dt.Rows
                JArrayLista.Add(New JObject(New JProperty("veg_cod", dr.Item("VEG_COD")), New JProperty("veg_des", dr.Item("VEG_DES")), New JProperty("Gru_Cod", dr.item("Gru_Cod"))))
            Next

            'cache.Item(key) = JArrayLista

            'Else
            '    JArrayLista = cache.Item(key)
            'End If

            'Else



            'End If

            r.RispostaStringa = JsonConvert.SerializeObject(JArrayLista, Formatting.None)
            r.RispostaOK = True

        Catch ex As Exception
            'uso questa funzione per ottenere il Messaggio..:
            r.Errore = "Errore durante l'operazione: " & vbCrLf & Gestione_Eccezioni_2015.MessaggioCompletoDataEccezione(ex, True, source:=True)
        End Try

        Return r

    End Function
    <WebMethod()>
    <Script.Services.ScriptMethod()>
    Public Function CaricaComboSpecieVegetaliDestinazioneUso_conFiltroUtente_NG(InData As CoreWS_Generic(Of CaricaComboSpecieVegetali_conFiltroUtente)) As RispostaStandard

        Dim r As New RispostaStandard

        If InData.objP.objP_server = "" Then
            r.Errore = "objP_server non valorizzato"
            Return r
        End If

        If InData.objP.objP_utenti = "" Then
            r.Errore = "objP_utenti non valorizzato"
            Return r
        End If


        Try

            Dim objParametri_Server As AgronicaCoreParametri = Utility.convertStringtoOBJparametri(InData.objP.objP_server)
            Dim objParametri_Utenti As AgronicaCoreParametri = Utility.convertStringtoOBJparametri(InData.objP.objP_utenti)

            Dim coreWSCache = MemoryCacheFactory.Instance.COREWS_CACHE

            Dim key = "CaricaComboSpecieVegetaliDestinazioneUso_conFiltroUtente" & objParametri_Server.PivaSuperUser & "-" & objParametri_Server.UtenteUsername & "-" & InData.InData.Gru_Cod & "-" & InData.InData.LetteraIniziale & "-" & InData.InData.StringaCerca & "-" & InData.InData.FiltroAggiuntivo & "-" & InData.InData.Ordinamento
            If coreWSCache IsNot Nothing Then
                If coreWSCache.Item(key) Is Nothing Then
                    Dim objS As New AgronicaCoreMetaSchemaDAL.SpecieVegetali_R
                    Dim DtS As DataTable = objS.SpecieVegetali_GestioneFiltroUtente_Leggi(0, InData.InData.Gru_Cod,
                                                                                InData.InData.LetteraIniziale, InData.InData.StringaCerca,
                                                                                 InData.InData.FiltroAggiuntivo,
                                                                                 InData.InData.Ordinamento,
                                                                                objParametri_Utenti)

                    Dim objD As New AgronicaCoreMetaSchemaDAL.Codici_Anagrafe_R
                    Dim DtD As DataTable = objD.DestinazioniUso_Leggi(InData.InData.FiltroAggiuntivo,
                                                                       InData.InData.Ordinamento,
                                                                      objParametri_Server)

                    Dim JArrayLista As New JArray()
                    For Each drS In DtS.Rows
                        JArrayLista.Add(New JObject(New JProperty("veg_cod", drS.Item("VEG_COD") & "|0"), New JProperty("veg_des", drS.Item("VEG_DES")), New JProperty("Gru_Cod", drS.Item("Gru_Cod"))))
                    Next
                    For Each drD In DtD.Rows
                        JArrayLista.Add(New JObject(New JProperty("veg_cod", "0|" & drD.Item("codice")), New JProperty("veg_des", drD.Item("descrizione")), New JProperty("Gru_Cod", "0")))
                    Next

                    Dim JArrayListaO As New JArray(JArrayLista.OrderBy(Function(obj) obj("veg_des").ToString.ToUpper))
                    Dim res = JsonConvert.SerializeObject(JArrayListaO, Formatting.None)
                    coreWSCache.Item(key) = res
                    r.RispostaStringa = res
                    r.RispostaOK = True
                Else
                    r.RispostaStringa = coreWSCache.Item(key)
                    r.RispostaOK = True
                End If
            End If

        Catch ex As Exception
            'uso questa funzione per ottenere il Messaggio..:
            r.Errore = "Errore durante l'operazione: " & vbCrLf & Gestione_Eccezioni_2015.MessaggioCompletoDataEccezione(ex, True, source:=True)
        End Try

        Return r

    End Function
    <WebMethod()>
    <Script.Services.ScriptMethod()>
    Public Function CaricaComboSpecieVegetaliDestinazioneUso_conFiltroUtente(objP_server As String,
                                                                             objP_utenti As String,
                                                                               Gru_Cod As Integer,
                                                                               LetteraIniziale As String, StringaCerca As String,
                                                                               FiltroAggiuntivo As String, Ordinamento As String) As RispostaStandard

        Dim r As New RispostaStandard

        If objP_server = "" Then
            r.Errore = "objP_server non valorizzato"
            Return r
        End If

        If objP_utenti = "" Then
            r.Errore = "objP_utenti non valorizzato"
            Return r
        End If


        Try

            Dim objParametri_Server As AgronicaCoreParametri = Utility.convertStringtoOBJparametri(objP_server)
            Dim objParametri_Utenti As AgronicaCoreParametri = Utility.convertStringtoOBJparametri(objP_utenti)

            Dim coreWSCache = MemoryCacheFactory.Instance.COREWS_CACHE

            Dim key = "CaricaComboSpecieVegetaliDestinazioneUso_conFiltroUtente" & objParametri_Server.PivaSuperUser & "-" & objParametri_Server.UtenteUsername & "-" & Gru_Cod & "-" & LetteraIniziale & "-" & StringaCerca & "-" & FiltroAggiuntivo & "-" & Ordinamento
            If coreWSCache IsNot Nothing Then
                If coreWSCache.Item(key) Is Nothing Then
                    Dim objS As New AgronicaCoreMetaSchemaDAL.SpecieVegetali_R
                    Dim DtS As DataTable = objS.SpecieVegetali_GestioneFiltroUtente_Leggi(0, Gru_Cod,
                                                                                LetteraIniziale, StringaCerca,
                                                                                FiltroAggiuntivo,
                                                                                Ordinamento,
                                                                                objParametri_Utenti)

                    Dim objD As New AgronicaCoreMetaSchemaDAL.Codici_Anagrafe_R
                    Dim DtD As DataTable = objD.DestinazioniUso_Leggi(FiltroAggiuntivo,
                                                                      Ordinamento,
                                                                      objParametri_Server)

                    Dim JArrayLista As New JArray()
                    For Each drS In DtS.Rows
                        JArrayLista.Add(New JObject(New JProperty("veg_cod", drS.Item("VEG_COD") & "|0"), New JProperty("veg_des", drS.Item("VEG_DES")), New JProperty("Gru_Cod", drS.Item("Gru_Cod"))))
                    Next
                    For Each drD In DtD.Rows
                        JArrayLista.Add(New JObject(New JProperty("veg_cod", "0|" & drD.Item("codice")), New JProperty("veg_des", drD.Item("descrizione")), New JProperty("Gru_Cod", "0")))
                    Next

                    Dim JArrayListaO As New JArray(JArrayLista.OrderBy(Function(obj) obj("veg_des").ToString.ToUpper))
                    Dim res = JsonConvert.SerializeObject(JArrayListaO, Formatting.None)
                    coreWSCache.Item(key) = res
                    r.RispostaStringa = res
                    r.RispostaOK = True
                Else
                    r.RispostaStringa = coreWSCache.Item(key)
                    r.RispostaOK = True
                End If
            End If

        Catch ex As Exception
            'uso questa funzione per ottenere il Messaggio..:
            r.Errore = "Errore durante l'operazione: " & vbCrLf & Gestione_Eccezioni_2015.MessaggioCompletoDataEccezione(ex, True, source:=True)
        End Try

        Return r

    End Function
    <WebMethod(EnableSession:=True)>
    Public Function GetUtilizzo_NG(InData As Object) As RispostaStandard

        Dim r As New RispostaStandard

        InData = JsonConvert.DeserializeObject(Of CoreWS_Generic(Of String))(JsonConvert.SerializeObject(InData))

        Dim Macrouso_Cod As String = InData.InData

        Try


            Dim objParametri_server As AgronicaCoreParametri = HttpContext.Current.Session("ASG_objParametri_Server")

            Dim objR As New AgronicaCoreMetaSchemaDAL.Codifica_SpecieVegetali_Agea_R

            Dim dt As DataTable = objR.Leggi("", "", 0, 0, 0, AGRODATAINIZIO, enumSelezioneVariabile.Selezione_TabellaDatiMinimi, "", "", objParametri_server)


            Dim lista As New List(Of String)

            For Each dr As DataRow In dt.Rows
                lista.Add("{""utilizzo"":""" & jSon.Escape(dr.Item("veg_des_agea")) & """, ""veg_cod_agea"":""" & dr.Item("veg_cod_agea") & """}")
            Next


            Dim strRisp As String = "[" & String.Join(",", lista.Distinct.ToList) & "]"


            r.RispostaOK = True
            r.RispostaStringa = strRisp

        Catch ex As Exception

            r.RispostaOK = False

            'uso questa funzione per ottenere il Messaggio..:
            Dim MessaggioErrore As String = AgronicaCoreDataProvider.Gestione_Eccezioni_2015.MessaggioCompletoDataEccezione(ex, True, source:=True)


            r.Errore = MessaggioErrore


        End Try

        Return r

    End Function
    <WebMethod(EnableSession:=True)>
    Public Function GetUtilizzo(ByVal Macrouso_Cod As String) As RispostaStandard

        Dim r As New RispostaStandard


        Try


            Dim objParametri_server As AgronicaCoreParametri = HttpContext.Current.Session("ASG_objParametri_Server")

            Dim objR As New AgronicaCoreMetaSchemaDAL.Codifica_SpecieVegetali_Agea_R

            Dim dt As DataTable = objR.Leggi("", "", 0, 0, 0, AGRODATAINIZIO, enumSelezioneVariabile.Selezione_TabellaDatiMinimi, "", "", objParametri_server)


            Dim lista As New List(Of String)

            For Each dr As DataRow In dt.Rows
                lista.Add("{""utilizzo"":""" & jSon.Escape(dr.Item("veg_des_agea")) & """, ""veg_cod_agea"":""" & dr.Item("veg_cod_agea") & """}")
            Next


            Dim strRisp As String = "[" & String.Join(",", lista.Distinct.ToList) & "]"


            r.RispostaOK = True
            r.RispostaStringa = strRisp

        Catch ex As Exception

            r.RispostaOK = False

            'uso questa funzione per ottenere il Messaggio..:
            Dim MessaggioErrore As String = AgronicaCoreDataProvider.Gestione_Eccezioni_2015.MessaggioCompletoDataEccezione(ex, True, source:=True)


            r.Errore = MessaggioErrore


        End Try

        Return r

    End Function
    <WebMethod()>
    <Script.Services.ScriptMethod()>
    Public Function CaricaComboCodici_Terreno_NG(InData As CoreWS_Generic(Of CaricaComboCodici_Terreno)) As RispostaStandard

        Dim r As New RispostaStandard

        If InData.objP.objP_server = "" Then
            r.Errore = "objP_server non valorizzato"
            Return r
        End If

        If InData.objP.objP_utenti = "" Then
            r.Errore = "objP_utenti non valorizzato"
            Return r
        End If


        Try

            Dim objParametri_Server As AgronicaCoreParametri = Utility.convertStringtoOBJparametri(InData.objP.objP_server)
            Dim objParametri_Utenti As AgronicaCoreParametri = Utility.convertStringtoOBJparametri(InData.objP.objP_utenti)

            Dim coreWSCache = MemoryCacheFactory.Instance.COREWS_CACHE

            Dim key = "CaricaComboCodici_Terreno-" & InData.InData.LetteraIniziale & "-" & InData.InData.StringaCerca & "-" & InData.InData.FiltroAggiuntivo & "-" & InData.InData.Ordinamento
            If coreWSCache IsNot Nothing Then
                If coreWSCache.Item(key) Is Nothing Then
                    Dim objD As New AgronicaCoreMetaSchemaDAL.Codici_Anagrafe_R
                    Dim DtD As DataTable = objD.DestinazioniUso_Leggi(InData.InData.FiltroAggiuntivo,
                                                                      InData.InData.Ordinamento,
                                                                      objParametri_Server)

                    Dim JArrayLista As New JArray()
                    For Each drD In DtD.Rows
                        JArrayLista.Add(New JObject(New JProperty("codice", drD.Item("codice")), New JProperty("descrizione", drD.Item("descrizione")), New JProperty("Gru_Cod", "0")))
                    Next

                    Dim JArrayListaO As New JArray(JArrayLista.OrderBy(Function(obj) obj("codice")))
                    Dim res = JsonConvert.SerializeObject(JArrayListaO, Formatting.None)
                    coreWSCache.Item(key) = res
                    r.RispostaStringa = res
                    r.RispostaOK = True
                Else
                    r.RispostaStringa = coreWSCache.Item(key)
                    r.RispostaOK = True
                End If
            End If

        Catch ex As Exception
            'uso questa funzione per ottenere il Messaggio..:
            r.Errore = "Errore durante l'operazione: " & vbCrLf & Gestione_Eccezioni_2015.MessaggioCompletoDataEccezione(ex, True, source:=True)
        End Try

        Return r

    End Function
    <WebMethod()>
    <Script.Services.ScriptMethod()>
    Public Function CaricaComboCodici_Terreno(objP_server As String,
                                             objP_utenti As String,
                                             LetteraIniziale As String, StringaCerca As String,
                                             FiltroAggiuntivo As String, Ordinamento As String) As RispostaStandard

        Dim r As New RispostaStandard

        If objP_server = "" Then
            r.Errore = "objP_server non valorizzato"
            Return r
        End If

        If objP_utenti = "" Then
            r.Errore = "objP_utenti non valorizzato"
            Return r
        End If


        Try

            Dim objParametri_Server As AgronicaCoreParametri = Utility.convertStringtoOBJparametri(objP_server)
            Dim objParametri_Utenti As AgronicaCoreParametri = Utility.convertStringtoOBJparametri(objP_utenti)

            Dim coreWSCache = MemoryCacheFactory.Instance.COREWS_CACHE

            Dim key = "CaricaComboCodici_Terreno-" & LetteraIniziale & "-" & StringaCerca & "-" & FiltroAggiuntivo & "-" & Ordinamento
            If coreWSCache IsNot Nothing Then
                If coreWSCache.Item(key) Is Nothing Then
                    Dim objD As New AgronicaCoreMetaSchemaDAL.Codici_Anagrafe_R
                    Dim DtD As DataTable = objD.DestinazioniUso_Leggi(FiltroAggiuntivo,
                                                                      Ordinamento,
                                                                      objParametri_Server)

                    Dim JArrayLista As New JArray()
                    For Each drD In DtD.Rows
                        JArrayLista.Add(New JObject(New JProperty("codice", drD.Item("codice")), New JProperty("descrizione", drD.Item("descrizione")), New JProperty("Gru_Cod", "0")))
                    Next

                    Dim JArrayListaO As New JArray(JArrayLista.OrderBy(Function(obj) obj("codice")))
                    Dim res = JsonConvert.SerializeObject(JArrayListaO, Formatting.None)
                    coreWSCache.Item(key) = res
                    r.RispostaStringa = res
                    r.RispostaOK = True
                Else
                    r.RispostaStringa = coreWSCache.Item(key)
                    r.RispostaOK = True
                End If
            End If

        Catch ex As Exception
            'uso questa funzione per ottenere il Messaggio..:
            r.Errore = "Errore durante l'operazione: " & vbCrLf & Gestione_Eccezioni_2015.MessaggioCompletoDataEccezione(ex, True, source:=True)
        End Try

        Return r

    End Function
    <WebMethod()>
    <Script.Services.ScriptMethod()>
    Public Function LeggiDestinazioniUso(InData As CoreWS_Generic(Of AgronicaCoreDTOStd.InData.Metaschema.LeggiDestinazioniUso)) As rispostaStandard(Of List(Of AgronicaCoreModelsSTD.metaschema.utilizzi.DestinazioneUso))

        Dim r As New rispostaStandard(Of List(Of AgronicaCoreModelsSTD.metaschema.utilizzi.DestinazioneUso))

        If InData.objP.objP_server = "" Then
            r.Errore = "objP_server non valorizzato"
            Return r
        End If

        Try

            Dim objParametri_Server As AgronicaCoreParametri = Utility.convertStringtoOBJparametri(InData.objP.objP_server)

            Dim coreWSCache = MemoryCacheFactory.Instance.COREWS_CACHE


            Dim key = "CaricaComboCodici_TerrenoModello"
            If coreWSCache IsNot Nothing Then
                If coreWSCache.Item(key) Is Nothing Then
                    Dim objD As New AgronicaCoreMetaSchemaDAL.Codici_Anagrafe_R
                    Dim DtD As DataTable = objD.DestinazioniUso_Leggi("",
                                                                      "",
                                                                      objParametri_Server)

                    Dim listaCodici = (From drD In DtD.Rows
                                       Select New AgronicaCoreModelsSTD.metaschema.utilizzi.DestinazioneUso(drD.Item("codice")) With {
                                            .descrizione = drD.Item("descrizione")
                                        }).ToList

                    coreWSCache.Item(key) = listaCodici
                    r.RispostaStringa = listaCodici
                    r.RispostaOK = True
                Else
                    r.RispostaStringa = coreWSCache.Item(key)
                    r.RispostaOK = True
                End If
            End If

        Catch ex As Exception
            'uso questa funzione per ottenere il Messaggio..:
            r.Errore = "Errore durante l'operazione: " & vbCrLf & Gestione_Eccezioni_2015.MessaggioCompletoDataEccezione(ex, True, source:=True)
        End Try

        Return r

    End Function


    <WebMethod()>
    <Script.Services.ScriptMethod()>
    Public Function CaricaComboSpecieVegetali_Join_GruppoVegetale(InData As CoreWS_Generic(Of Object)) As RispostaStandard

        Dim r As New RispostaStandard

        If InData.objP.objP_utenti = "" Then
            r.Errore = "objP_utenti non valorizzato"
            Return r
        End If

        If InData.objP.objP_server = "" Then
            r.Errore = "objP_server non valorizzato"
            Return r
        End If


        Try

            Dim objParametri_Server As AgronicaCoreParametri = Utility.convertStringtoOBJparametri(InData.objP.objP_server)

            Dim List As New List(Of Object)

            Dim Dt As DataTable

            Dim objSpecVeg As New AgronicaCoreMetaSchemaDAL.SpecieVegetali_R()
            Dt = objSpecVeg.Leggi_x_PDC_Modificata(objParametri_Server)

            For Each dRow As DataRow In Dt.Rows
                Dim Codice As String = dRow("veg_cod") & "|" & dRow("Gru_Cod")
                Dim Descrizione As String = dRow("veg_des").ToUpper() + " --- (" + dRow("Gruppo_Veg_Desc") + ")"
                List.Add(New With
                                {
                                     .Codice = Codice,
                                     .Descrizione = Descrizione
                                })
            Next

            r.RispostaOK = True
            r.RispostaStringa = JsonConvert.SerializeObject(List, Formatting.None)

        Catch ex As Exception
            'uso questa funzione per ottenere il Messaggio..:
            r.Errore = "Errore durante l'operazione: " & vbCrLf & Gestione_Eccezioni_2015.MessaggioCompletoDataEccezione(ex, True, source:=True)
        End Try

        Return r

    End Function

    <WebMethod()>
    <Script.Services.ScriptMethod()>
    Public Function LeggiFiltroUtente(InData As CoreWS_Generic(Of AgronicaCoreDTOStd.InData.Metaschema.LeggiSpecie)) As rispostaStandard(Of List(Of AgronicaCoreModelsSTD.metaschema.utilizzi.Specie))

        Dim r As New rispostaStandard(Of List(Of AgronicaCoreModelsSTD.metaschema.utilizzi.Specie))

        If InData.objP.objP_utenti = "" Then
            r.Errore = "objP_utenti non valorizzato"
            Return r
        End If

        Try

            Dim objParametri_Utenti As AgronicaCoreParametri = Utility.convertStringtoOBJparametri(InData.objP.objP_utenti)
            Dim objParametri_Server As AgronicaCoreParametri = Utility.convertStringtoOBJparametri(InData.objP.objP_server)

            Dim coreWSCache = MemoryCacheFactory.Instance.COREWS_CACHE

            Dim key = "SpecieVegetaliModello_" &
                objParametri_Utenti.UtenteUsername &
                If(InData.InData.ParametriSementieri IsNot Nothing, InData.InData.ParametriSementieri.Sementi, "") &
                If(InData.InData.ParametriSementieri IsNot Nothing, InData.InData.ParametriSementieri.SementiMappaturaLibera, "")

            Dim SpecieVegetaliUtente As List(Of AgronicaCoreModelsSTD.metaschema.utilizzi.Specie)
            If InData.InData.cache AndAlso coreWSCache IsNot Nothing And coreWSCache.Item(key) IsNot Nothing Then
                r.RispostaStringa = coreWSCache.Item(key)
                r.RispostaOK = True
                Return r
            End If

            Dim veg_cod_DT As New DataTable
            Dim xFiltroAggiuntivo As String = ""

            Dim mappaturaSpecieDaSportello As New AgronicaCoreSementieriDAL.Mappatura_Specie_R
            Dim codiceSportelloInt As Int32 = 0

            Dim isSementieri As Boolean
            Dim id_specie As Integer
            Dim id_sottospecie As Integer
            Dim id_gruppo As Integer
            Dim id_genotipo As Integer
            Dim isMappaturaLibera As Boolean = False

            If InData.InData.ParametriSementieri IsNot Nothing AndAlso InData.InData.ParametriSementieri.Sementi <> "" Then

                isSementieri = True

                Dim gp As New GisPurpose(InData.InData.ParametriSementieri.Sementi, If(InData.InData.ParametriSementieri.SementiMappaturaLibera = "True", "1", ""))
                id_specie = gp.SementiSportelloSpecie()
                id_sottospecie = gp.SementiSportelloSottospecie()
                id_gruppo = gp.SementiSportelloGruppo()
                id_genotipo = gp.SementiSportelloGenotipo()
                isMappaturaLibera = gp.SementiMappaturaLibera = "1"


                Dim codiceSportello = InData.InData.ParametriSementieri.Sementi.Split("|")(4)

                If Not Int32.TryParse(codiceSportello, codiceSportelloInt) Then
                    Throw New Exception("Il codice sportello deve essere un intero")
                End If



                'veg_cod_DT = mappaturaSpecieDaSportello.SpecieVegetaleDaCodiceSportello(codiceSportelloInt, "", "", objParametri_Server)

                'xFiltroAggiuntivo = " Veg_Cod in ( "
            End If

            'If veg_cod_DT IsNot Nothing AndAlso veg_cod_DT.Rows.Count > 0 Then

            '    For Each row In veg_cod_DT.Rows
            '        xFiltroAggiuntivo = String.Format("{0}{1}, ", xFiltroAggiuntivo, row("Veg_Cod"))
            '    Next

            '    xFiltroAggiuntivo = String.Format("{0} )", xFiltroAggiuntivo.Substring(0, xFiltroAggiuntivo.Length - 2))
            'Else
            '    xFiltroAggiuntivo = ""
            'End If

            Dim objSpecVeg As New AgronicaCoreMetaSchemaDAL.SpecieVegetali_R
            Dim Dt As DataTable = objSpecVeg.SpecieVegetali_GestioneFiltroUtente_Leggi(0,
                                                                    0,
                                                                    "",
                                                                    "",
                                                                    "",
                                                                    "",
                                                                    objParametri_Utenti,
                                                                    objParametri_Server:=objParametri_Server,
                                                                    isSementieri:=isSementieri,
                                                                    id_Specie:=id_specie,
                                                                    id_SottoSpecie:=id_sottospecie,
                                                                    id_Gruppo:=id_gruppo,
                                                                    id_Genotipo:=id_genotipo,
                                                                    isMappaturaLibera:=isMappaturaLibera)

            SpecieVegetaliUtente = (From dr In Dt.Rows Select New AgronicaCoreModelsSTD.metaschema.utilizzi.Specie(dr("veg_cod")) With {
                                        .descrizione = dr("Veg_Des")
                                    }).ToList

            coreWSCache.Set(key, SpecieVegetaliUtente, MemoryCacheFactory.Instance.CachePolicy)

            r.RispostaStringa = SpecieVegetaliUtente
            r.RispostaOK = True

        Catch ex As Exception
            'uso questa funzione per ottenere il Messaggio..:
            r.Errore = "Errore durante l'operazione: " & vbCrLf & Gestione_Eccezioni_2015.MessaggioCompletoDataEccezione(ex, True, source:=True)
        End Try

        Return r

    End Function

    <WebMethod()>
    <Script.Services.ScriptMethod()>
    Public Function Leggi(InData As CoreWS_Generic(Of AgronicaCoreDTOStd.InData.Metaschema.LeggiSpecie)) As rispostaStandard(Of List(Of AgronicaCoreModelsSTD.metaschema.utilizzi.Specie))
        Dim r As New rispostaStandard(Of List(Of AgronicaCoreModelsSTD.metaschema.utilizzi.Specie))

        If InData.objP.objP_utenti = "" Then
            r.Errore = "objP_utenti non valorizzato"
            Return r
        End If
        If InData.objP.objP_server = "" Then
            r.Errore = "objP_server non valorizzato"
            Return r
        End If

        Try
            Dim objParametri_Server As AgronicaCoreParametri = Utility.convertStringtoOBJparametri(InData.objP.objP_server)
            Dim objSpecVeg As New AgronicaCoreMetaSchemaDAL.SpecieVegetali_R()
            Dim SpecieVegetaliUtente As List(Of Specie)

            If InData.InData.gruppiVegetali IsNot Nothing Then
                Dim xFiltro = ""
                If InData.InData.gruppiVegetali.Any Then
                    xFiltro = " Gru_Cod in ( " &
                        InData.InData.gruppiVegetali.Select(Function(x) x.ToString).Aggregate(Function(acc, x) acc & ", " & x) &
                        " ) "
                End If
                SpecieVegetaliUtente = objSpecVeg.Leggi(
                    0, 0, LetteraIniziale:="", StringaCerca:="",
                    enumSelezioneVariabile.Selezione_TabellaCompleta,
                    xFiltro, xOrderBy:="", objParametri_Server
                ).AsEnumerable.
                Select(Function(dr) New Specie(dr("veg_cod"), dr("Veg_Des"))).
                ToList
            Else
                Dim coreWSCache = MemoryCacheFactory.Instance.COREWS_CACHE
                Dim key = "SpecieVegetaliModello"

                If coreWSCache IsNot Nothing And coreWSCache.Item(key) IsNot Nothing Then
                    r.RispostaStringa = coreWSCache.Item(key)
                    r.RispostaOK = True
                    Return r
                End If

                Dim Dt As DataTable = objSpecVeg.Leggi(
                    0, 0, LetteraIniziale:="", StringaCerca:="",
                    enumSelezioneVariabile.Selezione_TabellaDatiMinimi,
                    xFiltroAggiuntivo:="", xOrderBy:="", objParametri_Server
                )

                SpecieVegetaliUtente = (From dr In Dt.Rows Select New AgronicaCoreModelsSTD.metaschema.utilizzi.Specie(dr("veg_cod")) With {
                                        .descrizione = dr("Veg_Des")
                                    }).ToList

                coreWSCache.Set(key, SpecieVegetaliUtente, MemoryCacheFactory.Instance.CachePolicy)
            End If

            r.RispostaOK = True
            r.RispostaStringa = SpecieVegetaliUtente
        Catch ex As Exception
            'uso questa funzione per ottenere il Messaggio..:
            r.Errore = "Errore durante l'operazione: " & vbCrLf & Gestione_Eccezioni_2015.MessaggioCompletoDataEccezione(ex, True, source:=True)
            r.RispostaOK = False
        End Try
        Return r
    End Function

    <WebMethod()>
    <Script.Services.ScriptMethod()>
    Public Function Leggi_Da_Cultivar(InData As CoreWS_Generic(Of metaschema.utilizzi.Varieta)) As rispostaStandard(Of AgronicaCoreModelsSTD.metaschema.utilizzi.Specie)
        Dim r As New rispostaStandard(Of AgronicaCoreModelsSTD.metaschema.utilizzi.Specie)
        If InData.objP.objP_server = "" Then
            r.Errore = "objP_server non valorizzato"
            Return r
        End If

        Try
            Dim objParametri_Server As AgronicaCoreParametri = Utility.convertStringtoOBJparametri(InData.objP.objP_server)
            Dim objSpecVeg As New AgronicaCoreMetaSchemaBIZ.SpecieVegetali_R()
            Dim cultivar = InData.InData
            Dim SpecieVegetaliUtente = objSpecVeg.Leggi_Da_Cultivar(
                cultivar.codice, cultivar.descrizione,
                "", "",
                objParametri_Server
            )

            r.RispostaOK = True
            r.RispostaStringa = SpecieVegetaliUtente
        Catch ex As Exception
            'uso questa funzione per ottenere il Messaggio..:
            r.Errore = "Errore durante l'operazione: " & vbCrLf & Gestione_Eccezioni_2015.MessaggioCompletoDataEccezione(ex, True, source:=True)
        End Try

        Return r
    End Function

    <WebMethod()>
    <Script.Services.ScriptMethod()>
    Public Function LeggiGruppoVegetale(InData As CoreWS_Generic(Of String)) As rispostaStandard(Of List(Of baseClass.BaseCodeDescr))
        Dim r As New rispostaStandard(Of List(Of baseClass.BaseCodeDescr))
        If InData.objP.objP_server = "" Then
            r.Errore = "objP_server non valorizzato"
            Return r
        End If

        Try
            Dim objParametri_Server As AgronicaCoreParametri = Utility.convertStringtoOBJparametri(InData.objP.objP_server)
            Dim objSpecVeg As New AgronicaCoreMetaSchemaBIZ.SpecieVegetali_R()

            Dim grVeg = objSpecVeg.LeggiGruppiVegetali(objParametri_Server).ToList()

            r.RispostaStringa = grVeg
            r.RispostaOK = True
        Catch ex As Exception
            'uso questa funzione per ottenere il Messaggio..:
            r.Errore = "Errore durante l'operazione: " & vbCrLf & Gestione_Eccezioni_2015.MessaggioCompletoDataEccezione(ex, True, source:=True)
        End Try

        Return r
    End Function

End Class