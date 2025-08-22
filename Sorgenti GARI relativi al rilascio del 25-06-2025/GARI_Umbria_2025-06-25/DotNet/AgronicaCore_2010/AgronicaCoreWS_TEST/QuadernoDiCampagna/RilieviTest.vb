Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting

<TestClass()> Public Class RilieviTest

    Private ReadOnly dbName As String = "aboca"
    Private params As AgronicaCoreDataProvider.ObjParams

    Private Sub SetParams()
        If params Is Nothing Then
            params = ObjParamsSecret.GetObjParams(dbName)
        End If
    End Sub

    <TestMethod()> Public Sub TestMethod1()
        SetParams()


    End Sub

End Class