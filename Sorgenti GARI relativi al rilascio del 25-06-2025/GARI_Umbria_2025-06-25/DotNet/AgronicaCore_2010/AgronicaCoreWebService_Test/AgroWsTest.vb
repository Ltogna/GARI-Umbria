Imports AgronicaCoreDataProvider
Imports AgronicaCoreWebService

Imports System
Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq


'''<summary>
'''Classe di test per AgroWsTest.
'''Creata per contenere tutti gli unit test AgroWsTest
'''</summary>
<TestClass()> _
Public Class AgroWsTest

    Private objP_Web_Utenti As String = "{""PivaSuperUser"":""00069880391"",""UsernameOperazione"":""00069880391"",""UtenteUsername"":""terremerse"",""UtenteCodFiscale"":""00069880391"",""SuperUserUsername"":""terremerse"",""FinestraTemporaleInizio"":""1900-01-01T00:00:00"",""FinestraTemporaleFine"":""2100-12-31T00:00:00"",""FlagVisibilita"":1,""FlagCancellazioneLogica"":0,""objConnessione"":null,""objTransazione"":null,""StringaConnessione"":""Provider=SQLOLEDB;Server=agrodev1\\sql2012ita;Initial Catalog=UnitTest_Utenti;User Id=agronauta;Password=s3rv1c3;"",""LogDirectory"":""C:\\GIASLAN\\Log\\"",""LogFileName"":""GiasOnlineLog.txt"",""LogDescrizioneUtente"":""terremerse"",""Lingua_Cod"":1}"


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


    Private Shared Function AccessoValido(ByVal n_Accessi_super_User As Integer, ByVal n_Accessi_utente As Integer, ByVal n_max_Accessi_utente As Integer, ByVal ultimo_Creato As DateTime, ByVal n_max_Accessi_super_User As Integer, ByVal AWS_log_DistanzaChiamate As Integer, ByVal AWS_log_DistanzaChiamate_UDM As Integer, ByRef messaggioErrore As String, ByRef NumeroErrori As Integer, ByVal expected As Boolean)

        'Dim actual As Boolean
        'actual = AgroWs_Accessor.GetAccessoValido(n_Accessi_super_User, n_Accessi_utente, n_max_Accessi_utente, ultimo_Creato, n_max_Accessi_super_User, AWS_log_DistanzaChiamate, AWS_log_DistanzaChiamate_UDM, messaggioErrore)

        'If actual <> expected Then
        '    NumeroErrori += 1
        'End If

        'Return actual
    End Function
    '''<summary>
    '''Test per GetAccessoValido
    '''</summary>
    <TestMethod(), _
     DeploymentItem("AgronicaCoreWebService.dll")> _
    Public Sub GetAccessoValidoTest()
        Dim n_Accessi_super_User As Integer = 0 ' TODO: Eseguire l'inizializzazione a un valore appropriato
        Dim n_Accessi_utente As Integer = 0 ' TODO: Eseguire l'inizializzazione a un valore appropriato
        Dim n_max_Accessi_utente As Integer = 0 ' TODO: Eseguire l'inizializzazione a un valore appropriato
        Dim ultimo_Creato As DateTime = New DateTime() ' TODO: Eseguire l'inizializzazione a un valore appropriato
        Dim n_max_Accessi_super_User As Integer = 0 ' TODO: Eseguire l'inizializzazione a un valore appropriato
        Dim AWS_log_DistanzaChiamate As Integer = 0 ' TODO: Eseguire l'inizializzazione a un valore appropriato
        Dim AWS_log_DistanzaChiamate_UDM As Integer = 0 ' TODO: Eseguire l'inizializzazione a un valore appropriato
        Dim messaggioErrore As String = ""

        Dim Expected As Boolean
        Dim actual As Boolean
        Dim NumeroErrori As Integer = 0

        Debug.Print("Test dei valori 'Border line'")

        Debug.Print("Tutto passa:")
        messaggioErrore = ""
        Expected = True
        n_Accessi_super_User = 50
        n_max_Accessi_super_User = 150
        n_Accessi_utente = 10
        n_max_Accessi_utente = 11
        AWS_log_DistanzaChiamate = 3
        AWS_log_DistanzaChiamate_UDM = Convert.ToInt32([Enum].Parse(GetType(DateInterval), DateInterval.Second))
        ultimo_Creato = DateAdd(DateInterval.Second, -10, Now)

        actual = AccessoValido(n_Accessi_super_User, n_Accessi_utente, n_max_Accessi_utente, ultimo_Creato, n_max_Accessi_super_User, AWS_log_DistanzaChiamate, AWS_log_DistanzaChiamate_UDM, messaggioErrore, NumeroErrori, Expected)
        Debug.Print(messaggioErrore & "(" & actual & ")" & vbCrLf)


        Debug.Print("Non passa il super user :")
        messaggioErrore = ""
        Expected = False
        n_Accessi_super_User = 155
        n_max_Accessi_super_User = 150
        n_Accessi_utente = 10
        n_max_Accessi_utente = 11
        AWS_log_DistanzaChiamate = 3
        AWS_log_DistanzaChiamate_UDM = Convert.ToInt32([Enum].Parse(GetType(DateInterval), DateInterval.Second))
        ultimo_Creato = DateAdd(DateInterval.Second, -10, Now)

        actual = AccessoValido(n_Accessi_super_User, n_Accessi_utente, n_max_Accessi_utente, ultimo_Creato, n_max_Accessi_super_User, AWS_log_DistanzaChiamate, AWS_log_DistanzaChiamate_UDM, messaggioErrore, NumeroErrori, Expected)
        Debug.Print(messaggioErrore & "(" & actual & ")" & vbCrLf)


        Debug.Print("Non passa user :")
        messaggioErrore = ""
        Expected = False
        n_Accessi_super_User = 5
        n_max_Accessi_super_User = 150
        n_Accessi_utente = 100
        n_max_Accessi_utente = 11
        AWS_log_DistanzaChiamate = 3
        AWS_log_DistanzaChiamate_UDM = Convert.ToInt32([Enum].Parse(GetType(DateInterval), DateInterval.Second))
        ultimo_Creato = DateAdd(DateInterval.Second, -10, Now)

        actual = AccessoValido(n_Accessi_super_User, n_Accessi_utente, n_max_Accessi_utente, ultimo_Creato, n_max_Accessi_super_User, AWS_log_DistanzaChiamate, AWS_log_DistanzaChiamate_UDM, messaggioErrore, NumeroErrori, Expected)
        Debug.Print(messaggioErrore & "(" & actual & ")" & vbCrLf)


        Debug.Print("Non passa per intervallo tempo :")
        messaggioErrore = ""
        Expected = False
        n_Accessi_super_User = 5
        n_max_Accessi_super_User = 150
        n_Accessi_utente = 1
        n_max_Accessi_utente = 11
        AWS_log_DistanzaChiamate = 3
        AWS_log_DistanzaChiamate_UDM = Convert.ToInt32([Enum].Parse(GetType(DateInterval), DateInterval.Second))
        ultimo_Creato = DateAdd(DateInterval.Second, -2, Now)

        actual = AccessoValido(n_Accessi_super_User, n_Accessi_utente, n_max_Accessi_utente, ultimo_Creato, n_max_Accessi_super_User, AWS_log_DistanzaChiamate, AWS_log_DistanzaChiamate_UDM, messaggioErrore, NumeroErrori, Expected)
        Debug.Print(messaggioErrore & "(" & actual & ")" & vbCrLf)

        If NumeroErrori > 0 Then
            Assert.Fail()
        Else
            Assert.AreEqual(True, True)
        End If





    End Sub

    '''<summary>
    '''Test per AgroWSEsterni_Verifica_Abusi
    '''</summary>
    <TestMethod()> _
    Public Sub AgroWSEsterni_Verifica_AbusiTest()


        Dim target As AgroWs = New AgroWs() ' TODO: Eseguire l'inizializzazione a un valore appropriato
        Dim Super_user_Username As String = String.Empty ' TODO: Eseguire l'inizializzazione a un valore appropriato
        Dim Username As String = String.Empty ' TODO: Eseguire l'inizializzazione a un valore appropriato
        Dim Id_Servizio As Integer = 0 ' TODO: Eseguire l'inizializzazione a un valore appropriato
        Dim Id_Attivita As Integer = 0 ' TODO: Eseguire l'inizializzazione a un valore appropriato
        Dim MessaggioErrore As String = String.Empty ' TODO: Eseguire l'inizializzazione a un valore appropriato
        Dim MessaggioErroreExpected As String = String.Empty ' TODO: Eseguire l'inizializzazione a un valore appropriato
        Dim expected As Boolean = False ' TODO: Eseguire l'inizializzazione a un valore appropriato

        Dim objParametri_WebUtenti As AgronicaCoreDataProvider.AgronicaCoreParametri
        Dim a As JObject = JsonConvert.DeserializeObject(objP_Web_Utenti)
        objParametri_WebUtenti = a.ToObject(Of AgronicaCoreDataProvider.AgronicaCoreParametri)()

        Debug.Print("Chiamata ok, super user = BluarancioQdC, utente = Coldiretti")
        Super_user_Username = "BluarancioQdC"
        Username = "Coldiretti"
        expected = True

        Dim actual As Boolean
        actual = target.AgroWSEsterni_Verifica_Abusi(Super_user_Username, Username, Id_Servizio, Id_Attivita, MessaggioErrore, objParametri_WebUtenti)
        Debug.Print("MessaggioErrore: " & MessaggioErrore)

        Assert.AreEqual(expected, actual)
    End Sub


    


End Class
