Imports Microsoft.VisualStudio.TestTools.UnitTesting.Web
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports Newtonsoft.Json
Imports AgronicaCoreContabObject

Imports AgronicaCoreWS
Imports Newtonsoft.Json.Linq
Imports System.Reflection
Imports AgronicaCoreUtility




'''<summary>
'''Classe di test per TestTest.
'''Creata per contenere tutti gli unit test TestTest
'''</summary>
<TestClass()> _
Public Class Parco_Macchine_Test
    Private objP As String = "{""PivaSuperUser"":""00069880391"",""UsernameOperazione"":""00069880391"",""UtenteUsername"":""terremerse"",""UtenteCodFiscale"":""00069880391"",""SuperUserUsername"":""terremerse"",""FinestraTemporaleInizio"":""1900-01-01T00:00:00"",""FinestraTemporaleFine"":""2100-12-31T00:00:00"",""FlagVisibilita"":1,""FlagCancellazioneLogica"":0,""objConnessione"":null,""objTransazione"":null,""StringaConnessione"":""Provider=SQLOLEDB;Server=agrodev1\\sql2012ita;Initial Catalog=UnitTest_Server;User Id=agronauta;Password=s3rv1c3;"",""LogDirectory"":""C:\\GIASLAN\\Log\\"",""LogFileName"":""GiasOnlineLog.txt"",""LogDescrizioneUtente"":""terremerse"",""Lingua_Cod"":1}"
    Private objPUtenti As String = "{""PivaSuperUser"":""00069880391"",""UsernameOperazione"":""00069880391"",""UtenteUsername"":""terremerse"",""UtenteCodFiscale"":""00069880391"",""SuperUserUsername"":""terremerse"",""FinestraTemporaleInizio"":""1900-01-01T00:00:00"",""FinestraTemporaleFine"":""2100-12-31T00:00:00"",""FlagVisibilita"":1,""FlagCancellazioneLogica"":0,""objConnessione"":null,""objTransazione"":null,""StringaConnessione"":""Provider=SQLOLEDB;Server=agrodev1\\sql2012ita;Initial Catalog=UnitTest_Utenti;User Id=agronauta;Password=s3rv1c3;"",""LogDirectory"":""C:\\GIASLAN\\Log\\"",""LogFileName"":""GiasOnlineLog.txt"",""LogDescrizioneUtente"":""terremerse"",""Lingua_Cod"":1}"

    Private testContextInstance As TestContext

    Private Piva As String = "00069880391"
    Private Mac_Cod As String = "734"
    Private Flag As Boolean = True

    '=======================================================
    'Service provided by Telerik (www.telerik.com)
    'Conversion powered by NRefactory.
    'Twitter: @telerik
    'Facebook: facebook.com/telerik
    '=======================================================

    <TestMethod()> _
    Public Sub Parco_Macchine_734__Test()


        Dim objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri
        Dim a As JObject = JsonConvert.DeserializeObject(objP)
        objParametri = a.ToObject(Of AgronicaCoreDataProvider.AgronicaCoreParametri)()

        Dim obMacchine As New AgronicaCoreWS.Macchine

        Dim actual = obMacchine.getMacchina(Piva, Mac_Cod, AgronicaCoreDataProvider.Utility.convertOBJparametritoString(objParametri))

        Dim b As JObject = JsonConvert.DeserializeObject(obMacchine.getJSON_ParcoMacchine_js())

        Dim expected = b.ToObject(Of AgronicaCoreContabObject.Parco_Macchine)()
        UnitTestHelper.LookLikeEachOther(expected, actual)
    End Sub

    '<TestMethod()> _
    'Public Sub Parco_Macchine_734_modifica__Test()



    '    Dim obMacchine As New AgronicaCoreWS.Macchine
    '    Dim actual As Parco_Macchine = obMacchine.getMacchina(Piva, Mac_Cod, objP)
    '    actual.Descrizione = "nuova descrizione"

    '    Dim obUtenti As New AgronicaCoreWS.Utuility
    '    Dim ASG_ProgressivoGIAS As Integer = obUtenti.getProgressivoGias(objPUtenti)

    '    'MODIFICO 
    '    actual.Salva(ASG_ProgressivoGIAS, 5, objP)
    '    actual.Descrizione_per_Agenda = ""

    '    Dim expected As Parco_Macchine = obMacchine.getMacchina(Piva, Mac_Cod, objP)
    '    expected.Descrizione = "nuova descrizione"

    '    For Each p In expected.Costi
    '        p.ID = 0
    '    Next
    '    For Each p In actual.Costi
    '        p.ID = 0
    '    Next


    '    UnitTestHelper.LookLikeEachOther(expected, actual)
    'End Sub

    '<TestMethod()> _
    'Public Sub Parco_Macchine_generaStringone__Test()
    '    Dim exp = "<DatiAgenda><Agenda TipoOperazioneDB=""1"" piva=""00069880391"" sa_cod=""0"" id_agenda=""0"" lav_cod=""1008"" des_lib=""Giacenza Macchine forestali - verricelli"" tipo_accettazione=""0"" linea_cod=""0"" preparazione_cod=""0"" id_trasformazione=""0"" validita_inizio=""01/01/1900"" validita_fine=""31/12/2100"" basecode=""77070336"" topcode=""77201407"" blocco_flag=""0"" blocco_data=""01/01/1900 00.00.00"" blocco_username=""""><DatiMovimenti><Movimento TipoOperazioneDB=""1"" piva=""00069880391"" sa_cod=""0"" id_agenda=""0"" id_mov=""0"" cau_mov=""7300"" mov_desc=""Giacenza Macchine forestali - verricelli"" data_movimento=""07/09/2015"" ora=""07/09/2015 00.00.00"" scadenza=""31/12/2100"" scadenza_extra=""01/01/1900"" doc_numero_sin="""" doc_numero=""0"" doc_numero_des="""" num_protocollo=""0"" colli=""0"" peso=""0"" aspetto="""" causale_trasporto="""" tipo_sconto=""0"" cod_risum=""0"" cod_indirizzorisum=""0"" cod_destinazione=""0"" cod_indirizzodestinazione=""0"" mezzo=""0"" cod_vettore=""0"" cod_indirizzovettore=""0"" natura_beni="""" modalita=""0"" tara_veicolo=""0"" tara_imballi=""0"" tipo_peso=""0"" username_note="""" extra_str="""" extra_int=""0"" extra_date=""01/01/1900 00.00.00"" progr_protocollo=""0"" progr_registrazione=""0"" data_registrazione=""01/01/1900"" chklayout_bypass_fatturato=""0"" chklayout_join_prodotti=""0"" validita_inizio=""01/01/1900"" validita_fine=""31/12/2100"" basecode=""77070336"" topcode=""77201407""><DatiMovimenti_Dettagli><Movimento_Dettaglio TipoOperazioneDB=""1"" piva=""00069880391"" sa_cod=""0"" id_agenda=""0"" id_mov=""0"" id_mov_det=""0"" mov_det_des="""" elem_cod=""1"" pro_cod=""0"" mat_cod=""0"" cod_progetto=""0"" fase_cod=""0"" lotto="""" cal_cod=""0"" udm_cod=""38"" udm_cod_extra=""0"" qta=""1"" qta_extra=""0"" prezzo_unitario=""50000"" prezzo_unitario_netto=""0"" imponibile=""0"" imponibile_netto=""0"" cod_iva=""0"" iva=""0"" sconto=""0"" prezzo_effettivo=""0"" anno=""2015"" ric_cod=""0"" cod_conto=""0"" jolly_int=""0"" contabilizzato=""1"" pendente=""6"" extra_str="""" extra_int=""0"" extra_date=""01/01/1900 00.00.00"" validita_inizio=""01/01/1900"" validita_fine=""31/12/2100"" chkiva_manuale=""0"" cod_ivaindetraibile=""0"" qta_extra_totale=""0"" tara=""0"" chklayout_hide=""0"" variazione=""0"" listino_cod=""0"" basecode=""77070336"" topcode=""77201407"" id_destinazione=""0"" lav_cod=""0"" cau_mov=""7300""><DatiParcoMacchine><ParcoMacchina TipoOperazioneDB=""2"" piva=""00069880391"" sa_cod=""0"" mac_cod=""734"" class_code=""12.02"" mac_des=""descrizione macchina"" costo_acquisto=""50000"" targa=""targa1"" telaio=""telaio"" ditta_cod=""10002"" modello=""modello"" potenza=""10"" ammortamento=""10,5"" ammortizzato=""0"" data_immatricolazione=""03/08/2015"" ultima_manutenzione=""31/08/2015"" ultima_revisione=""06/08/2015"" stato_utilizzo=""in corso"" validita_inizio=""31/08/2015"" validita_fine=""31/12/2100"" basecode=""77070336"" topcode=""77201407"" note=""note"" tipo=""0"" n_immatricolazione=""1234"" n_immatricolazione_rimorchio=""12345"" n_autorizzazione_trasporto=""123456"" data_rilascio_autorizzazione=""04/08/2015"" peso=""50"" mac_cod_origine=""0"" piva_superuser_origine="""" chkdefault=""0"" portata_max=""0"" cod_contatto="""" alimentazione_cod=""1"" potenza_udm_cod=""5001028"" cuaa_proprietario=""CUAA"" denominazione_proprietario=""pippo inzaghi"" tipo_targa_cod=""1"" tipo_trazione_cod=""0"" n_omologazione="""" ditta_cod_motore=""0"" tipo_motore="""" matricola_motore="""" data_reimmatricolazione=""01/01/1900 00.00.00"" data_carico=""01/08/2015 00.00.00"" data_scarico=""02/08/2015 00.00.00"" titolopossesso=""4"" flag_attrezzatura_macchina=""M"" taratura_ugello=""10""><DatiProdotti_Costi><Prodotto_Costo TipoOperazioneDB=""1"" piva=""00069880391"" riferimento="""" elem_cod=""1"" pro_cod=""0"" mat_cod=""734"" udm_cod=""0"" mezzo=""2"" prezzo_unitario=""10"" veg_cod=""0"" validita_inizio=""01/08/2015"" validita_fine=""30/08/2015"" cul_cod=""0"" /><Prodotto_Costo TipoOperazioneDB=""1"" piva=""00069880391"" riferimento="""" elem_cod=""1"" pro_cod=""0"" mat_cod=""734"" udm_cod=""0"" mezzo=""1"" prezzo_unitario=""20"" veg_cod=""0"" validita_inizio=""01/08/2015"" validita_fine=""30/08/2015"" cul_cod=""0"" /></DatiProdotti_Costi></ParcoMacchina></DatiParcoMacchine></Movimento_Dettaglio></DatiMovimenti_Dettagli></Movimento></DatiMovimenti></Agenda></DatiAgenda>"
    '    Dim obMacchine As New AgronicaCoreWS.Macchine
    '    Dim ob = obMacchine.getMacchina(Piva, Mac_Cod, objP)
    '    Dim obUtenti As New AgronicaCoreWS.Utuility
    '    Dim ASG_ProgressivoGIAS As Integer = obUtenti.getProgressivoGias(objPUtenti)

    '    Dim actual As String = obMacchine.getStringoneXml(ob, ASG_ProgressivoGIAS, objP)

    '    'UnitTestHelper.LookLikeEachOther(XDocument.Parse(exp), XDocument.Parse(actual))
    '    Dim str = UnitTestHelper.confrontaStringhe(exp, actual)
    '    Debug.Write(str)

    '    Assert.AreEqual(exp, actual)
    'End Sub





    <TestMethod()> _
    Public Sub getElencoTipo_test()
        Dim obMacchine As New AgronicaCoreWS.Macchine
        Dim ob = obMacchine.getElencoTipo(objP)
        Assert.AreEqual(19, ob.Count)
    End Sub
    <TestMethod()> _
    Public Sub getElencoDettaglio1_MacchineForestali_test()
        Dim objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri
        
        Dim obMacchine As New AgronicaCoreWS.Macchine
        Dim ob = obMacchine.getElencoDettaglio1("12", objP)
        Assert.AreEqual(12, ob.Count)
    End Sub
    <TestMethod()> _
    Public Sub getElencoDettaglio2_Motori_trattori_ruote_test()
        Dim objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri
        Dim a As JObject = JsonConvert.DeserializeObject(objP)
        objParametri = a.ToObject(Of AgronicaCoreDataProvider.AgronicaCoreParametri)()

        Dim obMacchine As New AgronicaCoreWS.Macchine
        Dim ob = obMacchine.getElencoDettaglio2("01", "02", objP)
        Assert.AreEqual(6, ob.Count)
    End Sub


    <TestMethod()> _
    Public Sub getElencoMarca_test()
      
        Dim obMacchine As New AgronicaCoreWS.Macchine
        Dim ob = obMacchine.getElencoMarche(objP)
        Assert.AreEqual(1131, ob.Count)
    End Sub

    <TestMethod()> _
    Public Sub getTipoTarga_test()
        Dim objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri
        Dim a As JObject = JsonConvert.DeserializeObject(objP)
        objParametri = a.ToObject(Of AgronicaCoreDataProvider.AgronicaCoreParametri)()

        Dim obMacchine As New AgronicaCoreWS.Macchine
        Dim ob = obMacchine.getTipoTarga()
        Assert.AreEqual(4, ob.Count)
    End Sub
End Class
