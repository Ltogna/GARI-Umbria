Imports AgronicaCoreDataProvider
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports AgronicaCoreGisBIZ

Imports System.Configuration.ConfigurationManager


'''<summary>
'''Classe di test per GIS_Entita_RTest.
'''Creata per contenere tutti gli unit test GIS_Entita_RTest
'''</summary>
<TestClass()> _
Public Class GIS_Entita_RTest


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
    '''Test per Leggi
    '''</summary>
    <TestMethod()> _
    Public Sub LeggiTest()
        Dim target As GIS_Entita_R = New GIS_Entita_R() ' TODO: Eseguire l'inizializzazione a un valore appropriato
        Dim PivaSuperUser As String = String.Empty ' TODO: Eseguire l'inizializzazione a un valore appropriato
        Dim Piva As String = String.Empty ' TODO: Eseguire l'inizializzazione a un valore appropriato
        Dim ForDelete As Boolean = False ' TODO: Eseguire l'inizializzazione a un valore appropriato
        Dim AllAttributes As Boolean = False ' TODO: Eseguire l'inizializzazione a un valore appropriato
        Dim objParametri As AgronicaCoreParametri = Nothing ' TODO: Eseguire l'inizializzazione a un valore appropriato
        Dim objParametriExpected As AgronicaCoreParametri = Nothing ' TODO: Eseguire l'inizializzazione a un valore appropriato
        Dim expected As String = String.Empty ' TODO: Eseguire l'inizializzazione a un valore appropriato
        Dim actual As String
        actual = target.Leggi(PivaSuperUser, Piva, ForDelete, AllAttributes, objParametri)
        Assert.AreEqual(objParametriExpected, objParametri)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verificare la correttezza del metodo di test.")
    End Sub

    '''<summary>
    '''Test per LeggiPerGerarchiaImprese
    '''</summary>
    <TestMethod()> _
    Public Sub LeggiPerGerarchiaImpreseTest()


        Dim target As GIS_Entita_R = New GIS_Entita_R() ' TODO: Eseguire l'inizializzazione a un valore appropriato
        Dim PivaSuperUser As String = String.Empty ' TODO: Eseguire l'inizializzazione a un valore appropriato
        Dim Piva As String = String.Empty ' TODO: Eseguire l'inizializzazione a un valore appropriato
        Dim ForDelete As Boolean = False ' TODO: Eseguire l'inizializzazione a un valore appropriato
        Dim AllAttributes As Boolean = False ' TODO: Eseguire l'inizializzazione a un valore appropriato
        Dim objParametri As AgronicaCoreParametri = Nothing ' TODO: Eseguire l'inizializzazione a un valore appropriato
        Dim objParametriExpected As AgronicaCoreParametri = Nothing ' TODO: Eseguire l'inizializzazione a un valore appropriato
        Dim expected As String = String.Empty ' TODO: Eseguire l'inizializzazione a un valore appropriato

        objParametri = testHelper.GetObjParametri()

        Dim actual As String
        actual = target.LeggiPerImpresa(PivaSuperUser, Piva, ForDelete, AllAttributes, objParametri)


        Assert.Inconclusive("Verificare la correttezza del metodo di test.")
    End Sub


    '''<summary>
    '''Test per LeggiPerImpresa
    '''</summary>
    <TestMethod()> _
    Public Sub LeggiPerImpresaTest()
        Dim target As GIS_Entita_R = New GIS_Entita_R() ' TODO: Eseguire l'inizializzazione a un valore appropriato
        Dim PivaSuperUser As String = String.Empty ' TODO: Eseguire l'inizializzazione a un valore appropriato
        Dim Piva As String = String.Empty ' TODO: Eseguire l'inizializzazione a un valore appropriato
        Dim ForDelete As Boolean = False ' TODO: Eseguire l'inizializzazione a un valore appropriato
        Dim AllAttributes As Boolean = False ' TODO: Eseguire l'inizializzazione a un valore appropriato
        Dim objParametri As AgronicaCoreParametri = Nothing ' TODO: Eseguire l'inizializzazione a un valore appropriato
        Dim objParametriExpected As AgronicaCoreParametri = Nothing ' TODO: Eseguire l'inizializzazione a un valore appropriato
        Dim expected As String = String.Empty ' TODO: Eseguire l'inizializzazione a un valore appropriato
        Dim actual As String
        actual = target.LeggiPerImpresa(PivaSuperUser, Piva, ForDelete, AllAttributes, objParametri)
        Assert.AreEqual(objParametriExpected, objParametri)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verificare la correttezza del metodo di test.")
    End Sub
End Class
