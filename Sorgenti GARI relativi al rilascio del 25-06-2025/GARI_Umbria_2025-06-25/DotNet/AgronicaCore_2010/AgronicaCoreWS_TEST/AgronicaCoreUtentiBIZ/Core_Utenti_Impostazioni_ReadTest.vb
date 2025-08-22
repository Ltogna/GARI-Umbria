Imports System

Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports AgronicaCoreUtentiDAL
Imports AgronicaCoreUtility


'''<summary>
'''Classe di test per Utenti_Impostazioni_ReadTest.
'''Creata per contenere tutti gli unit test Utenti_Impostazioni_ReadTest
'''</summary>
<TestClass()> _
Public Class Core_Utenti_Impostazioni_ReadTest


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


    Private Shared Sub CalcolaAnnata(ByVal dataRiferimento As DateTime, ByRef datainizio As DateTime, ByRef datafine As DateTime, ByVal sXdate As String)
        Utenti_Impostazioni_Read.AnnataAgrariaCalcola(dataRiferimento, datainizio, datafine, sXdate)
    End Sub

    Private Shared Function AnnataAgrariaVerifica(ByVal sXdate As String, ByVal DataRiferimento As DateTime, ByVal datainizio As DateTime, ByVal datainizioExpected As DateTime, ByVal datafine As DateTime, ByVal datafineExpected As DateTime) As Integer

        UnitTestHelper.stampaStringhe(sXdate, sXdate, "sXDate", "sXDate")
        Debug.Print("")
        UnitTestHelper.stampaStringhe(DataRiferimento, DataRiferimento, "Data Riferimento", "Data Riferimento")
        Debug.Print("")
        UnitTestHelper.stampaStringhe(datainizioExpected, datainizio, "Data Inizio", "Data Inizio")
        Debug.Print("")
        UnitTestHelper.stampaStringhe(datafineExpected, datafine, "Data Fine", "Data Fine")
        Debug.Print("")


        Dim Errore As Integer = 0

        Try
            Assert.AreEqual(datainizioExpected, datainizio)
            Assert.AreEqual(datafineExpected, datafine)
        Catch ex As Exception
            Debug.Print("In Errore!")
            Errore = 1
        End Try


        Debug.Print("-------------------")
        Debug.Print("")

        Return Errore

    End Function
    '''<summary>
    '''Test per AnnataAgrariaCalcola
    '''</summary>
    <TestMethod()> _
    Public Sub AnnataAgrariaCalcolaTest()

        Dim dataRiferimento As DateTime
        Dim datainizio As DateTime
        Dim datainizioExpected As DateTime
        Dim datafine As DateTime
        Dim datafineExpected As DateTime
        Dim sXdate As String

        Dim NumeroErrori As Integer = 0

        Debug.Print("Test dei valori 'Border line'")
        Debug.Print("inizio anno, fine anno, riferimento 01/01")
        dataRiferimento = #1/1/2015#
        sXdate = "01013112"
        datainizioExpected = #1/1/2015#
        datafineExpected = #12/31/2015#
        CalcolaAnnata(dataRiferimento, datainizio, datafine, sXdate)
        NumeroErrori += AnnataAgrariaVerifica(sXdate, dataRiferimento, datainizio, datainizioExpected, datafine, datafineExpected)

        Debug.Print("AnnataAgraria std: 01/11 al 30/10 anno successivo, riferimento 1/1 Anno Corrente")
        dataRiferimento = #1/1/2015#
        sXdate = "01113110"
        datainizioExpected = #11/1/2014#
        datafineExpected = #10/31/2015#
        CalcolaAnnata(dataRiferimento, datainizio, datafine, sXdate)
        NumeroErrori += AnnataAgrariaVerifica(sXdate, dataRiferimento, datainizio, datainizioExpected, datafine, datafineExpected)

        Debug.Print("AnnataAgraria std: 01/11 al 30/10 anno successivo, riferimento 30/11 Anno Corrente")
        dataRiferimento = #1/1/2015#
        sXdate = "01113110"
        datainizioExpected = #11/1/2014#
        datafineExpected = #10/31/2015#
        CalcolaAnnata(dataRiferimento, datainizio, datafine, sXdate)
        NumeroErrori += AnnataAgrariaVerifica(sXdate, dataRiferimento, datainizio, datainizioExpected, datafine, datafineExpected)

        Debug.Print("AnnataAgraria umbria: 11/11 al 10/11 anno successivo, riferimento 01/01 Anno Corrente")
        dataRiferimento = #1/1/2015#
        sXdate = "11111011"
        datainizioExpected = #11/11/2014#
        datafineExpected = #11/10/2015#
        CalcolaAnnata(dataRiferimento, datainizio, datafine, sXdate)
        NumeroErrori += AnnataAgrariaVerifica(sXdate, dataRiferimento, datainizio, datainizioExpected, datafine, datafineExpected)

        Debug.Print("AnnataAgraria Sementieri: 01/09 al 30/10 anno successivo, riferimento 01/09 Anno Corrente")
        dataRiferimento = #9/1/2015#
        sXdate = "01093108"
        datainizioExpected = #9/1/2015#
        datafineExpected = #8/31/2016#
        CalcolaAnnata(dataRiferimento, datainizio, datafine, sXdate)
        NumeroErrori += AnnataAgrariaVerifica(sXdate, dataRiferimento, datainizio, datainizioExpected, datafine, datafineExpected)



        'Test dei valori non border line
        dataRiferimento = #10/1/2015#
        sXdate = "01093108"
        datainizioExpected = #9/1/2015#
        datafineExpected = #8/31/2016#
        CalcolaAnnata(dataRiferimento, datainizio, datafine, sXdate)
        NumeroErrori += AnnataAgrariaVerifica(sXdate, dataRiferimento, datainizio, datainizioExpected, datafine, datafineExpected)


        If NumeroErrori = 0 Then
            Assert.AreEqual("", "")
        Else
            Assert.Fail()
        End If


    End Sub
End Class
