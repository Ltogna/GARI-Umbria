Imports System.Text

<TestClass()>
Public Class UnitTest1

    Private testContextInstance As TestContext

    '''<summary>
    '''Ottiene o imposta il contesto del test che fornisce
    '''le informazioni e le funzionalità per l'esecuzione del test corrente.
    '''</summary>
    Public Property TestContext() As TestContext
        Get
            Return testContextInstance
        End Get
        Set(ByVal value As TestContext)
            testContextInstance = Value
        End Set
    End Property

#Region "Attributi di test aggiuntivi"
    '
    ' È possibile utilizzare i seguenti attributi aggiuntivi per la scrittura dei test:
    '
    ' Utilizzare ClassInitialize per eseguire il codice prima di eseguire il primo test della classe
    ' <ClassInitialize()> Public Shared Sub MyClassInitialize(ByVal testContext As TestContext)
    ' End Sub
    '
    ' Utilizzare ClassCleanup per eseguire il codice dopo l'esecuzione di tutti i test della classe
    ' <ClassCleanup()> Public Shared Sub MyClassCleanup()
    ' End Sub
    '
    ' Utilizzare TestInitialize per eseguire il codice prima di eseguire ciascun test
    ' <TestInitialize()> Public Sub MyTestInitialize()
    ' End Sub
    '
    ' Utilizzare TestCleanup per eseguire il codice dopo l'esecuzione di ciascun test
    ' <TestCleanup()> Public Sub MyTestCleanup()
    ' End Sub
    '
#End Region

    <TestMethod()>
    Public Sub TestMethod1()
        ' TODO: aggiungere qui la logica del test
    End Sub

End Class
