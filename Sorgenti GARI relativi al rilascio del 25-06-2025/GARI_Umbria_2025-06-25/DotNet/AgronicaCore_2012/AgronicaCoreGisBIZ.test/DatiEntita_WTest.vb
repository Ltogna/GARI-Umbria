Imports AgronicaCoreDataProvider

Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports AgronicaCoreGisBIZ
Imports System.Collections.ObjectModel



'''<summary>
'''Classe di test per DatiEntita_WTest.
'''Creata per contenere tutti gli unit test DatiEntita_WTest
'''</summary>
<TestClass()> _
Public Class DatiEntita_WTest


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


    '''<summary>
    '''Test per Costruttore DatiEntita_W
    '''</summary>
    <TestMethod()> _
    Public Sub DatiEntita_WConstructorTest()
        Dim target As DatiEntita_W = New DatiEntita_W()
        Assert.Inconclusive("TODO: Implementare il codice per la verifica della destinazione")
    End Sub

    '''<summary>
    '''Test per scrivi
    '''</summary>
    <TestMethod()> _
    Public Sub scriviTest()
        Dim target As DatiEntita_W = New DatiEntita_W() ' TODO: Eseguire l'inizializzazione a un valore appropriato
        Dim xmlDatiEntita As String = String.Empty ' TODO: Eseguire l'inizializzazione a un valore appropriato
        Dim OUTPUT_EntitaCod As Integer = 0 ' TODO: Eseguire l'inizializzazione a un valore appropriato
        Dim OUTPUT_EntitaCodExpected As Integer = 0 ' TODO: Eseguire l'inizializzazione a un valore appropriato
        Dim objParametri As AgronicaCoreParametri = Nothing ' TODO: Eseguire l'inizializzazione a un valore appropriato
        Dim objParametriExpected As AgronicaCoreParametri = Nothing ' TODO: Eseguire l'inizializzazione a un valore appropriato
        Dim expected As Boolean = False ' TODO: Eseguire l'inizializzazione a un valore appropriato
        Dim actual As Boolean


        Dim files As ReadOnlyCollection(Of String)
        files = My.Computer.FileSystem.GetFiles("C:\AgroSorgenti\AgronicaCore_2012\AgronicaCoreGisBIZ.test\XMLs", FileIO.SearchOption.SearchAllSubDirectories, "*.xml")
        objParametri = testHelper.GetObjParametri()

        For Each sNomeFile In files

            xmlDatiEntita = My.Computer.FileSystem.ReadAllText(sNomeFile)
            actual = target.scrivi(xmlDatiEntita, OUTPUT_EntitaCod, objParametri)

        Next

        Assert.Inconclusive("Verificare la correttezza del metodo di test.")
    End Sub
End Class
