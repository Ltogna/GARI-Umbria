Namespace VetInfoModel
    Public Class VetInfoFilter
        Public field As String
        Public op As String
        Public value1 As String
        Public value2 As String
    End Class

    Public Class VetInfoPagination

        Public Sub New()
            recordsPerPage = 50
            page = 1
        End Sub

        Public recordsPerPage As Integer
        Public page As Integer
        Public sortBy As String
        Public sortType As String
    End Class

    Public Class Costanti
        Public Const EQUALS As String = "EQUALS"
        Public Const LESSTHAN As String = "LESSTHAN"
        Public Const MORETHAN As String = "MORETHAN"
        Public Const BETWEEN As String = "BETWEEN"
    End Class

End Namespace
