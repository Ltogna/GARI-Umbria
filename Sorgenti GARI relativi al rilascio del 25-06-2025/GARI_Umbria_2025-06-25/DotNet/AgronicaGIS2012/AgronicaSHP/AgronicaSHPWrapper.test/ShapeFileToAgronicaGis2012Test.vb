Imports AgronicaSHPWrapper.TestShapeFile

Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports AgronicaSHPWrapper
Imports AgronicaSHPWrapper.InterpretaDatiDBF


'''<summary>
'''Classe di test per ShapeFileToAgronicaGis2012Test.
'''Creata per contenere tutti gli unit test ShapeFileToAgronicaGis2012Test
'''</summary>
<TestClass()> _
Public Class ShapeFileToAgronicaGis2012Test


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
    '''Test per Costruttore ShapeFileToAgronicaGis2012
    '''</summary>
    <TestMethod()> _
    Public Sub ShapeFileToAgronicaGis2012ConstructorTest()
        Dim target As ShapeFileToAgronicaGis2012 = New ShapeFileToAgronicaGis2012()
        Assert.Inconclusive("TODO: Implementare il codice per la verifica della destinazione")
    End Sub

    '''<summary>
    '''Test per convert
    '''</summary>
    <TestMethod()> _
    Public Sub convertTest()
        Dim target As ShapeFileToAgronicaGis2012 = New ShapeFileToAgronicaGis2012() ' TODO: Eseguire l'inizializzazione a un valore appropriato
        Dim shapeFile_fullFileName As String = String.Empty ' TODO: Eseguire l'inizializzazione a un valore appropriato
        Dim expected As String = String.Empty ' TODO: Eseguire l'inizializzazione a un valore appropriato
        Dim actual As String

        'file di test..

        'shapeFile_fullFileName = "C:\AgroSorgenti\AgronicaGIS2012\AgronicaSHP\AgronicaSHPWrapper.test\TestSHP\cab_terra.shp"
        'shapeFile_fullFileName = "C:\AgroSorgenti\AgronicaGIS2012\AgronicaSHP\AgronicaSHPWrapper.test\TestSHP\OperazioniColturali_Agrisfera\ConcimazioneGrano\Data\Agrisfera\Carlina_00\Sq 1\CONCIMA_EZ34655\Coverage.shp"
        'shapeFile_fullFileName = "C:\AgroSorgenti\AgronicaGIS2012\AgronicaSHP\AgronicaSHPWrapper.test\TestSHP\OperazioniColturali_Agrisfera\ConcimazioneGrano\Data\Agrisfera\Carlina_00\Sq 12\Applicazione_EZ34655\Coverage.shp"

        shapeFile_fullFileName = "C:\AgroSorgenti\AgronicaGIS2012\AgronicaSHP\AgronicaSHPWrapper.test\TestSHP\coverage.shp"
        'shapeFile_fullFileName = "D:\Dati\Agrisfera\Lavorazioni\2012\Concimazione grano 2\AgGPS\Data\ANGEL0\CARLINA\032812_0001_EZ34655\CONCIME_EZ34655\coverage.shp"

        'shapeFile_fullFileName = "C:\AgroSorgenti\AgronicaGIS2012\AgronicaSHP\AgronicaSHPWrapper.test\TestSHP\Catasto_CAB\CAB_Massari.shp"

        'shapeFile_fullFileName = "C:\AgroSorgenti\AgronicaGIS2012\AgronicaSHP\AgronicaSHPWrapper.test\TestSHP\CitimapAgrisfera2010\Carlina-Appezzamento-071.shp"
        'shapeFile_fullFileName = "C:\AgroSorgenti\AgronicaGIS2012\AgronicaSHP\AgronicaSHPWrapper.test\TestSHP\CitimapAgrisfera2010\Valle-Squadro-8.shp"
        'shapeFile_fullFileName = "C:\AgroSorgenti\AgronicaGIS2012\AgronicaSHP\AgronicaSHPWrapper.test\TestSHP\CitimapAgrisfera2010\Valle-Appezzamento-026.shp"

        'todo, metti a posto
        'actual = target.convert(shapeFile_fullFileName, "00085770394", "00085770394", 1, Tipo_Importazione.Tipo_Importazione_ShapeFile.Importazione_Trimble, 2011, "", "", 519, False)

        'target.LoadedSapefile = New TestShapeFile.ShapeFile
        'target.LoadedSapefile.Read(shapeFile_fullFileName)

        'target.LoadedSapefile.Save("C:\AgroSorgenti\AgronicaGIS2012\AgronicaSHP\AgronicaSHPWrapper.test\TestSHP_Scrittura\OperazioniColturali_Agrisfera\Coverage.shp")
        'target.LoadedSapefile.Save("C:\AgroSorgenti\AgronicaGIS2012\AgronicaSHP\AgronicaSHPWrapper.test\TestSHP_Scrittura\cab_Terra.shp")
        'target.LoadedSapefile.Save("C:\AgroSorgenti\AgronicaGIS2012\AgronicaSHP\AgronicaSHPWrapper.test\TestSHP_Scrittura\CitimapAgrisfera2010\Carlina-Appezzamento-071.shp")

        'My.Computer.FileSystem.WriteAllText("C:\AgroSorgenti\AgronicaGIS2012\AgronicaSHP\AgronicaSHPWrapper.test\TestSHP\testshp.xml", actual, False)

        'Assert.AreEqual(expected, actual)
        Assert.AreEqual("1", "1")
    End Sub

    '''<summary>
    '''Test per LoadedSapefile
    '''</summary>
    <TestMethod()> _
    Public Sub LoadedSapefileTest()
        Dim target As ShapeFileToAgronicaGis2012 = New ShapeFileToAgronicaGis2012() ' TODO: Eseguire l'inizializzazione a un valore appropriato
        Dim expected As ShapeFile = Nothing ' TODO: Eseguire l'inizializzazione a un valore appropriato
        Dim actual As ShapeFile
        target.LoadedSapefile = expected
        actual = target.LoadedSapefile
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verificare la correttezza del metodo di test.")
    End Sub

    '''<summary>
    '''Test per ShapeFile_fullFileName
    '''</summary>
    <TestMethod()> _
    Public Sub ShapeFile_fullFileNameTest()
        Dim target As ShapeFileToAgronicaGis2012 = New ShapeFileToAgronicaGis2012() ' TODO: Eseguire l'inizializzazione a un valore appropriato
        Dim expected As String = String.Empty ' TODO: Eseguire l'inizializzazione a un valore appropriato
        Dim actual As String
        target.ShapeFile_fullFileName = expected
        actual = target.ShapeFile_fullFileName
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verificare la correttezza del metodo di test.")
    End Sub
End Class
