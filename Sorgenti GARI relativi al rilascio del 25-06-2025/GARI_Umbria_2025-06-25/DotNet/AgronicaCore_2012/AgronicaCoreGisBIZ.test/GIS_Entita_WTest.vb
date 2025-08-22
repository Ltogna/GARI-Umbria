Imports AgronicaCoreDataProvider

Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports AgronicaCoreGisBIZ
Imports System.Collections.ObjectModel



'''<summary>
'''Classe di test per GIS_Entita_WTest.
'''Creata per contenere tutti gli unit test GIS_Entita_WTest
'''</summary>
<TestClass()> _
Public Class GIS_Entita_WTest


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
    '''Test per Costruttore GIS_Entita_W
    '''</summary>
    <TestMethod()> _
    Public Sub GIS_Entita_WConstructorTest()
        Dim target As GIS_Entita_W = New GIS_Entita_W()
        Assert.Inconclusive("TODO: Implementare il codice per la verifica della destinazione")
    End Sub

    '''<summary>
    '''Test per scrivi
    '''</summary>
    <TestMethod()> _
    Public Sub scriviTest()

        Dim target As GIS_Entita_W = New GIS_Entita_W() ' TODO: Eseguire l'inizializzazione a un valore appropriato
        Dim DatiEntita As String = String.Empty ' TODO: Eseguire l'inizializzazione a un valore appropriato
        Dim OUTPUT_EntitaCod As Integer = 0 ' TODO: Eseguire l'inizializzazione a un valore appropriato
        Dim OUTPUT_EntitaCodExpected As Integer = 0 ' TODO: Eseguire l'inizializzazione a un valore appropriato
        Dim objParametri As AgronicaCoreParametri = Nothing ' TODO: Eseguire l'inizializzazione a un valore appropriato
        Dim objParametriExpected As AgronicaCoreParametri = Nothing ' TODO: Eseguire l'inizializzazione a un valore appropriato
        Dim expected As Boolean = False ' TODO: Eseguire l'inizializzazione a un valore appropriato

        Dim files As ReadOnlyCollection(Of String)
        files = My.Computer.FileSystem.GetFiles("C:\AgroSorgenti\AgronicaCore_2012\AgronicaCoreGisBIZ.test\XMLs", FileIO.SearchOption.SearchAllSubDirectories, "*.xml")

        For Each sNomeFile In files

            DatiEntita = My.Computer.FileSystem.ReadAllText(sNomeFile)
            objParametri = testHelper.GetObjParametri()


            Dim actual As Boolean
            actual = target.scrivi(DatiEntita, OUTPUT_EntitaCod, objParametri)
        Next

        Assert.Inconclusive("Verificare la correttezza del metodo di test.")
    End Sub
End Class
