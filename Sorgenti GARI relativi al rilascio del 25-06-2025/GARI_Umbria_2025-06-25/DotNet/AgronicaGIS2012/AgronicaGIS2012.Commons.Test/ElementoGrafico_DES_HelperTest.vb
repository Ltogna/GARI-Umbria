Imports System.Text

Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports AgronicaGIS2012.Commons
Imports AgronicaCoreUtility


Imports Newtonsoft.Json.Linq
Imports Newtonsoft.Json





'''<summary>
'''Classe di test per ElementoGrafico_DES_HelperTest.
'''Creata per contenere tutti gli unit test ElementoGrafico_DES_HelperTest
'''</summary>
<TestClass()> _
Public Class ElementoGrafico_DES_HelperTest


    Private testContextInstance As TestContext

    '''<summary>
    '''Ottiene o imposta il contesto dei test, che fornisce
    '''funzionalità e informazioni sull'esecuzione dei test corrente.
    '''</summary>
    Public Property TestContext() As TestContext
        Get
            Return testContextInstance
        End Get
        Set(value As TestContext)
            testContextInstance = value
        End Set
    End Property

#Region "Attributi di test aggiuntivi"
    '
    'Durante la scrittura dei test è possibile utilizzare i seguenti attributi aggiuntivi:
    '
    'Utilizzare ClassInitialize per eseguire il codice prima di eseguire il primo test della classe
    '<ClassInitialize()>  _
    'Public Shared Sub MyClassInitialize(ByVal testContext As TestContext)
    'End Sub
    '
    'Utilizzare ClassCleanup per eseguire il codice dopo l'esecuzione di tutti i test di una classe
    '<ClassCleanup()>  _
    'Public Shared Sub MyClassCleanup()
    'End Sub
    '
    'Utilizzare TestInitialize per eseguire il codice prima di eseguire ciascun test
    '
    '<TestInitialize()>  _
    'Public Sub MyTestInitialize()
    'End Sub
    'Utilizzare TestCleanup per eseguire il codice dopo l'esecuzione di ciascun test
    '<TestCleanup()>  _
    'Public Sub MyTestCleanup()
    'End Sub
    '
#End Region


    Private objP As String = "{""PivaSuperUser"":""00069880391"",""UsernameOperazione"":""00069880391"",""UtenteUsername"":""terremerse"",""UtenteCodFiscale"":""00069880391"",""SuperUserUsername"":""terremerse"",""FinestraTemporaleInizio"":""1900-01-01T00:00:00"",""FinestraTemporaleFine"":""2100-12-31T00:00:00"",""FlagVisibilita"":1,""FlagCancellazioneLogica"":0,""objConnessione"":null,""objTransazione"":null,""StringaConnessione"":""Provider=SQLOLEDB;Server=agrodev1\\sql2012ita;Initial Catalog=UnitTest_Server;User Id=agronauta;Password=s3rv1c3;"",""LogDirectory"":""C:\\GIASLAN\\Log\\"",""LogFileName"":""GiasOnlineLog.txt"",""LogDescrizioneUtente"":""terremerse"",""Lingua_Cod"":1}"



    '''<summary>
    '''Test per QRY_GetFrom_ElementoGraficoDES_Piped
    '''</summary>
    <TestMethod()> _
    Public Sub QRY_GetFrom_ElementoGraficoDES_PipedTest()





        Dim Chiave_DaEstrarre As String = String.Empty ' TODO: Eseguire l'inizializzazione a un valore appropriato
        Dim ElementoGraficoSuccessivoAQuelloDaEstrarre As String = ""
        Dim stbExpected As StringBuilder = Nothing ' TODO: Eseguire l'inizializzazione a un valore appropriato
        Dim NomeColonna As String = String.Empty ' TODO: Eseguire l'inizializzazione a un valore appropriato
        Dim NomeColonnaAlias As String = "TEST"

        Dim ValoreEstratto As String
        Dim ValoreAtteso As String

        NomeColonna = "'|Plate§ 1339|from_time§ 14/08/2016 16:40:55|to_time§ 14/08/2016 16:40:55|isStop§ 0'"


        '1 test
        Chiave_DaEstrarre = "isStop"
        ValoreAtteso = "0"

        ValoreEstratto = EstraiValore(NomeColonna, NomeColonnaAlias, Chiave_DaEstrarre, ElementoGraficoSuccessivoAQuelloDaEstrarre)

        Dim ConteggioErrori As Integer = 0
        Try
            UnitTestHelper.stampaStringhe(ValoreAtteso, ValoreEstratto, "Valore Atteso ", "Valore Estratto")
            Assert.AreEqual(ValoreAtteso, ValoreEstratto)
        Catch ex As Exception
            Debug.Print("Errore!")
            ConteggioErrori += 1
        End Try



        '2 test
        Chiave_DaEstrarre = "to_time"
        ElementoGraficoSuccessivoAQuelloDaEstrarre = "isStop"
        ValoreAtteso = "14/08/2016 16:40:55"

        ValoreEstratto = EstraiValore(NomeColonna, NomeColonnaAlias, Chiave_DaEstrarre, ElementoGraficoSuccessivoAQuelloDaEstrarre)

        Try
            UnitTestHelper.stampaStringhe(ValoreAtteso, ValoreEstratto, "Valore Atteso ", "Valore Estratto")
            Assert.AreEqual(ValoreAtteso, ValoreEstratto)
        Catch ex As Exception
            Debug.Print("Errore!")
            ConteggioErrori += 1
        End Try


        Assert.AreEqual(0, ConteggioErrori)

    End Sub


    Public Function EstraiValore(ByVal NomeColonna As String, NomeColonnaAlias As String, ByVal Chiave_DaEstrarre As String, ByVal ElementoGraficoSuccessivoAQuelloDaEstrarre As String) As String

        Dim objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri
        Dim a As JObject = JsonConvert.DeserializeObject(objP)
        objParametri = a.ToObject(Of AgronicaCoreDataProvider.AgronicaCoreParametri)()

        Dim dP As New AgronicaCoreDataProvider.DataProvider

        Dim stb As New StringBuilder
        stb.Append("SELECT ")

        ElementoGrafico_DES_Helper.QRY_GetFrom_ElementoGraficoDES_Piped(Chiave_DaEstrarre, ElementoGraficoSuccessivoAQuelloDaEstrarre, stb, NomeColonna, NomeColonnaAlias)
        Dim ValoreEstratto As String = ""
        Dim dtP As DataTable = Nothing
        Dim qrY As String = stb.ToString
        Try
            dtP = dP.EseguiQuery_Lettura(objParametri, qrY, "")
        Catch ex As Exception

        End Try


        If Not dtP Is Nothing AndAlso dtP.Rows.Count > 0 Then
            ValoreEstratto = dtP.Rows(0)(0)
        Else
            ValoreEstratto = "aaaaaaaaaaaaaaaaaaaaaaa"
        End If

        Return ValoreEstratto

    End Function

End Class
