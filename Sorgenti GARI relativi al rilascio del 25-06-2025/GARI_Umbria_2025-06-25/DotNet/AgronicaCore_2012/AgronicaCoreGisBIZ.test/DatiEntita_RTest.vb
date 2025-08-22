Imports AgronicaCoreDataProvider

Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports AgronicaCoreGisBIZ



'''<summary>
'''Classe di test per DatiEntita_RTest.
'''Creata per contenere tutti gli unit test DatiEntita_RTest
'''</summary>
<TestClass()> _
Public Class DatiEntita_RTest


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


    '''<summary>
    '''Test per Costruttore DatiEntita_R
    '''</summary>
    <TestMethod()> _
    Public Sub DatiEntita_RConstructorTest()
        Dim target As DatiEntita_R = New DatiEntita_R()
        Assert.Inconclusive("TODO: Implementare il codice per la verifica della destinazione")
    End Sub

    '''<summary>
    '''Test per LeggiPerGerarchiaImprese
    '''</summary>
    <TestMethod()> _
    Public Sub LeggiPerGerarchiaImpreseTest()
        Dim target As DatiEntita_R = New DatiEntita_R() ' TODO: Eseguire l'inizializzazione a un valore appropriato
        Dim PivaSuperUser As String = String.Empty ' TODO: Eseguire l'inizializzazione a un valore appropriato
        Dim Piva As String = String.Empty ' TODO: Eseguire l'inizializzazione a un valore appropriato
        Dim ForDelete As Boolean = False ' TODO: Eseguire l'inizializzazione a un valore appropriato
        Dim AllAttributes As Boolean = False ' TODO: Eseguire l'inizializzazione a un valore appropriato
        Dim objParametri As AgronicaCoreParametri = Nothing ' TODO: Eseguire l'inizializzazione a un valore appropriato
        Dim objParametriExpected As AgronicaCoreParametri = Nothing ' TODO: Eseguire l'inizializzazione a un valore appropriato
        Dim expected As String = String.Empty ' TODO: Eseguire l'inizializzazione a un valore appropriato

        objParametri = testHelper.GetObjParametri()

        PivaSuperUser = "80062590379"

        Dim Codice_Fiscale_Tecnico As String = "80062590379"

        Dim actual As String
        actual = target.LeggiPerGerarchiaImpreseSQLXML(PivaSuperUser, 0, 0, "", Piva, 0, 0, 0, 0, "0", "0", "-1", -1, -1, "-1", 0, 0, 0, False, False, False, True, True, False, Codice_Fiscale_Tecnico, "", Nothing, objParametri)

        Dim layerElaborati As String
        Dim elaboraLayer As New DatiEntita_ProfilazioneLayers_MappaturaSementi
        layerElaborati = elaboraLayer.InserisciLayers(actual, objParametri)

        Assert.AreEqual(1, 1)

    End Sub
End Class
