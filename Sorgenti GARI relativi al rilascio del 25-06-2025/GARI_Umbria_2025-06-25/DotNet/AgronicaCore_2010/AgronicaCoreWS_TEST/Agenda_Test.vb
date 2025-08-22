Imports Microsoft.VisualStudio.TestTools.UnitTesting.Web
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports Newtonsoft.Json
Imports AgronicaCoreContabObject

Imports AgronicaCoreWS
Imports Newtonsoft.Json.Linq
Imports System.Reflection
Imports AgronicaCoreUtility
Imports AgronicaCoreDataProvider.CostantiPersonalizzate




'''<summary>
'''Classe di test per TestTest.
'''Creata per contenere tutti gli unit test TestTest
'''</summary>
<TestClass()> _
Public Class Agenda_Test
    Private objP As String = "{""PivaSuperUser"":""00069880391"",""UsernameOperazione"":""00069880391"",""UtenteUsername"":""terremerse"",""UtenteCodFiscale"":""00069880391"",""SuperUserUsername"":""terremerse"",""FinestraTemporaleInizio"":""1900-01-01T00:00:00"",""FinestraTemporaleFine"":""2100-12-31T00:00:00"",""FlagVisibilita"":1,""FlagCancellazioneLogica"":0,""objConnessione"":null,""objTransazione"":null,""StringaConnessione"":""Provider=SQLOLEDB;Server=agrodev1\\sql2012ita;Initial Catalog=UnitTest_Server;User Id=agronauta;Password=s3rv1c3;"",""LogDirectory"":""C:\\GIASLAN\\Log\\"",""LogFileName"":""GiasOnlineLog.txt"",""LogDescrizioneUtente"":""terremerse"",""Lingua_Cod"":1}"
    Private objPUtenti As String = "{""PivaSuperUser"":""00069880391"",""UsernameOperazione"":""00069880391"",""UtenteUsername"":""terremerse"",""UtenteCodFiscale"":""00069880391"",""SuperUserUsername"":""terremerse"",""FinestraTemporaleInizio"":""1900-01-01T00:00:00"",""FinestraTemporaleFine"":""2100-12-31T00:00:00"",""FlagVisibilita"":1,""FlagCancellazioneLogica"":0,""objConnessione"":null,""objTransazione"":null,""StringaConnessione"":""Provider=SQLOLEDB;Server=agrodev1\\sql2012ita;Initial Catalog=UnitTest_Utenti;User Id=agronauta;Password=s3rv1c3;"",""LogDirectory"":""C:\\GIASLAN\\Log\\"",""LogFileName"":""GiasOnlineLog.txt"",""LogDescrizioneUtente"":""terremerse"",""Lingua_Cod"":1}"

    Private testContextInstance As TestContext

    Private Piva As String = "02287360396"
    Private LAV_COD As String = "74"


    '=======================================================
    'Service provided by Telerik (www.telerik.com)
    'Conversion powered by NRefactory.
    'Twitter: @telerik
    'Facebook: facebook.com/telerik
    '=======================================================

    <TestMethod()> _
    Public Sub GetJSONagenda()
        Dim objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri
        Dim a As JObject = JsonConvert.DeserializeObject(objP)
        objParametri = a.ToObject(Of AgronicaCoreDataProvider.AgronicaCoreParametri)()



        'Dim actual = AgronicaCoreWS.Agenda.getNewAgenda(Piva, 0, 33, 0, LAV_COD, Now.Today, Utility.convertOBJparametritoString(objParametri))
        'Dim b As JObject = JsonConvert.DeserializeObject(AgronicaCoreWS.Agenda.getJSON_Agenda_js())

        'Dim expected = b.ToObject(Of AgronicaCoreContabObject.Parco_Macchine)()
        'UnitTestHelper.LookLikeEachOther(expected, actual)
    End Sub

End Class
