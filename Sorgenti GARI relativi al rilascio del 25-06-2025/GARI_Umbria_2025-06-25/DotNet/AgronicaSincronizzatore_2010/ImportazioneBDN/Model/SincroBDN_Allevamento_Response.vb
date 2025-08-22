
Public Class SincroBDN_Allevamento_Response
	Public cf_Detentore As String

	Public Detentore As String

	Public cf_Proprietario As String

	Public Proprietario As String
	Public NumeroCapiBDN As Integer


	Public listaCapi_Ingresso As List(Of Object)
	Public nCapi_Ingresso As Integer

	Public listaCapi_Uscita As List(Of Object)
	Public nCapi_Uscita As Integer

	Public capiDaControllare As SincroBDN_CapiDaControllare_Response

End Class


Public Class SincroBDN_CapiDaControllare_Response
	Public capiDB As List(Of String)
	Public capiBDN As List(Of String)
	Public MatricoleDuplicate As List(Of String)
End Class