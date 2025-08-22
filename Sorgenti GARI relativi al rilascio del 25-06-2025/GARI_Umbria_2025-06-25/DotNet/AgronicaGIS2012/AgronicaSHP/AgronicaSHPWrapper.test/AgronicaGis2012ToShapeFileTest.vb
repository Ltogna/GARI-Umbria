Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports AgronicaSHPWrapper
Imports AgronicaCoreDataProvider



'''<summary>
'''Classe di test per AgronicaGis2012ToShapeFileTest.
'''Creata per contenere tutti gli unit test AgronicaGis2012ToShapeFileTest
'''</summary>
<TestClass()> _
Public Class AgronicaGis2012ToShapeFileTest


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
    '''Test per Convert
    '''</summary>
    <TestMethod()> _
    Public Sub ConvertTest()
        Dim target As AgronicaGis2012ToShapeFile = New AgronicaGis2012ToShapeFile() ' TODO: Eseguire l'inizializzazione a un valore appropriato
        Dim xmlToConvert As String = String.Empty ' TODO: Eseguire l'inizializzazione a un valore appropriato
        Dim filename As String = String.Empty ' TODO: Eseguire l'inizializzazione a un valore appropriato
        Dim expected As Boolean = False ' TODO: Eseguire l'inizializzazione a un valore appropriato
        Dim actual As Boolean

        Dim leggi As New AgronicaCoreGisDAL.GIS_Entita_R
        Dim objParametri As AgronicaCoreParametri = testHelper.GetObjParametri("00085770394")

        'xmlToConvert = leggi.LeggiXML("00085770394", 0, 0, "00085770394", 0, 0, 0, 0, "", "", "-1", "-1", -1, "-1", 21723, 0, AgronicaCoreParametri.enumSelezioneVariabile.Selezione_TabellaCompleta, "", "", objParametri)
        'xmlToConvert = leggi.LeggiPerGerarchiaImpreseSQLXML(objParametri.PivaSuperUser, "00085770394", False, True, objParametri)
        xmlToConvert = xmlToConvert.Replace("<DatiEntita>", "<DatiEntita xmlns=""http://www.agronica.it/grafica/"" xmlns:gml=""http://www.opengis.net/gml"">")

        filename = "C:\AgroSorgenti\AgronicaGIS2012\AgronicaSHP\AgronicaSHPWrapper.test\TestSHP_Scrittura\OperazioniColturali_Agrisfera\FromDB.shp"

        'da db
        '  Vanni, 28/05/2016 11:43:39: todo: decommentare
        'actual = target.Convert(xmlToConvert, filename)

        'da shape pre-aperto
        'target.LoadedSapefile = New TestShapeFile.ShapeFile
        'Dim shapeFile_fullFileName As String = "C:\AgroSorgenti\AgronicaGIS2012\AgronicaSHP\AgronicaSHPWrapper.test\TestSHP\coverage.shp"

        'target.LoadedSapefile.Read(shapeFile_fullFileName)
        'target.LoadedSapefile.Save(filename)

        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verificare la correttezza del metodo di test.")


    End Sub

    '''<summary>
    '''Test per Costruttore AgronicaGis2012ToShapeFile
    '''</summary>
    <TestMethod()> _
    Public Sub AgronicaGis2012ToShapeFileConstructorTest()
        Dim target As AgronicaGis2012ToShapeFile = New AgronicaGis2012ToShapeFile()
        Assert.Inconclusive("TODO: Implementare il codice per la verifica della destinazione")
    End Sub
End Class
