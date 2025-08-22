Imports AgronicaCoreVarieBIZ

Imports Microsoft.VisualStudio.TestTools.UnitTesting.Web

Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports AgronicaCoreWS



'''<summary>
'''Classe di test per PlanningTest.
'''Creata per contenere tutti gli unit test PlanningTest
'''</summary>
<TestClass()> _
Public Class PlanningTest


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
            testContextInstance = Value
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
    '''Test per ScriviPianificazione
    '''</summary>
    <TestMethod()> _
     Public Sub ScriviPianificazioneTest()
        Dim target As Planning = New Planning() ' TODO: Eseguire l'inizializzazione a un valore appropriato
        Dim s As String = String.Empty ' TODO: Eseguire l'inizializzazione a un valore appropriato
        Dim objP_server As String = String.Empty ' TODO: Eseguire l'inizializzazione a un valore appropriato
        Dim expected As RispostaStandard = Nothing ' TODO: Eseguire l'inizializzazione a un valore appropriato
        Dim actual As RispostaStandard



        'actual = target.ScriviPianificazione(s, objP_server)

        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verificare la correttezza del metodo di test.")
    End Sub
End Class
