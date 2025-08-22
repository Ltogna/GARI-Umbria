Imports ImportazioneBDN

Namespace VetInfoResponseModel
	Public Class VetInfoResponse(Of T)
		Public errors As List(Of VetInfoError_Response)
		Public pagination As VetInfoPagination_Response
		Public data As List(Of T)
		Public success As Boolean

	End Class

	Public Class VetInfoPagination_Response
		Inherits VetInfoModel.VetInfoPagination

		Public records As Integer
		Public totalRecords As Integer

	End Class

	Public Class VetInfoError_Response
		Public field As String
		Public code As String
		Public message As String
		Public index As Integer?

	End Class

	Public Class VetInfoData_Response
		Public extra1 As String
		Public extra2 As String
		Public extra3 As String
		Public extra4 As String
		Public extraObject As String

	End Class

End Namespace

