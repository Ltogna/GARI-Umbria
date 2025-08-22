Imports AgronicaCoreVarieBIZ

Imports Microsoft.VisualStudio.TestTools.UnitTesting.Web

Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports AgronicaCoreWS
Imports Newtonsoft.Json.Linq
Imports Newtonsoft.Json
Imports AgronicaCoreUtility

Imports AgronicaCoreDataProvider


'''<summary>
'''Classe di test per ImpreseTest.
'''Creata per contenere tutti gli unit test ImpreseTest
'''</summary>
<TestClass()> _
Public Class ImpreseTest

    Private objP As String = "{""PivaSuperUser"":""00069880391"",""UsernameOperazione"":""00069880391"",""UtenteUsername"":""terremerse"",""UtenteCodFiscale"":""00069880391"",""SuperUserUsername"":""terremerse"",""FinestraTemporaleInizio"":""1900-01-01T00:00:00"",""FinestraTemporaleFine"":""2100-12-31T00:00:00"",""FlagVisibilita"":1,""FlagCancellazioneLogica"":0,""objConnessione"":null,""objTransazione"":null,""StringaConnessione"":""Provider=SQLOLEDB;Server=agrodev1\\sql2012ita;Initial Catalog=UnitTest_Server;User Id=agronauta;Password=s3rv1c3;"",""LogDirectory"":""C:\\GIASLAN\\Log\\"",""LogFileName"":""GiasOnlineLog.txt"",""LogDescrizioneUtente"":""terremerse"",""Lingua_Cod"":1}"
    Private objPUtenti As String = "{""PivaSuperUser"":""00069880391"",""UsernameOperazione"":""00069880391"",""UtenteUsername"":""terremerse"",""UtenteCodFiscale"":""00069880391"",""SuperUserUsername"":""terremerse"",""FinestraTemporaleInizio"":""1900-01-01T00:00:00"",""FinestraTemporaleFine"":""2100-12-31T00:00:00"",""FlagVisibilita"":1,""FlagCancellazioneLogica"":0,""objConnessione"":null,""objTransazione"":null,""StringaConnessione"":""Provider=SQLOLEDB;Server=agrodev1\\sql2012ita;Initial Catalog=UnitTest_Utenti;User Id=agronauta;Password=s3rv1c3;"",""LogDirectory"":""C:\\GIASLAN\\Log\\"",""LogFileName"":""GiasOnlineLog.txt"",""LogDescrizioneUtente"":""terremerse"",""Lingua_Cod"":1}"

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


    'TODO: assicurarsi che l'attributo UrlToTest specifichi l'URL di una pagina ASP.NET (ad esempio,
    ' http://.../Default.aspx). La specifica è necessaria per l'esecuzione dello unit test sul server Web
    ' per il test di una pagina, un servizio Web o WCF.
    '''<summary>
    '''Test per ScriviImpresa
    '''</summary>
    <TestMethod()> _
    Public Sub ScriviImpresaTest()

        Dim target As New Imprese

        Dim s As String = String.Empty ' TODO: Eseguire l'inizializzazione a un valore appropriato

        Try
            s = My.Computer.FileSystem.ReadAllText("C:\TFS_AreaLavoro\Gias\RamoPrincipale\Src\GiasDotNet\AgronicaCore_2010\AgronicaCoreXML\Xmls\Small_XmlPrivato_00084390392_2.xml")
        Catch ex As Exception

        End Try

        Dim objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri
        Dim a As JObject = JsonConvert.DeserializeObject(objP)
        objParametri = a.ToObject(Of AgronicaCoreDataProvider.AgronicaCoreParametri)()

        Dim expected As RispostaStandard = Nothing ' TODO: Eseguire l'inizializzazione a un valore appropriato
        Dim actual As RispostaStandard

        Dim objP2 As String = Utility.convertOBJparametritoString(objParametri)

        'actual = target.ScriviImpresa(s, objP2)

        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verificare la correttezza del metodo di test.")

    End Sub
End Class
