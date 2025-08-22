
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Public Class ObjParamsSecret

    Private Shared Function CreateParams(utenti As String, server As String, superServer As String) As AgronicaCoreDataProvider.ObjParams
        Return New AgronicaCoreDataProvider.ObjParams With {
                    .ObjParametri_Utenti = AgronicaCoreDataProvider.Utility.convertStringtoOBJparametri(utenti),
                    .ObjParametri_Server = AgronicaCoreDataProvider.Utility.convertStringtoOBJparametri(server),
                    .ObjParametri_SuperServer = AgronicaCoreDataProvider.Utility.convertStringtoOBJparametri(superServer)
                }
    End Function

    Private Shared Function CreateParamsUncrypted(utenti As String, server As String, superServer As String) As AgronicaCoreDataProvider.ObjParams
        Dim getObjP = Function(str As String)
                          Dim obj As JObject = JsonConvert.DeserializeObject(str)
                          Return obj.ToObject(Of AgronicaCoreDataProvider.AgronicaCoreParametri)()
                      End Function
        Return New AgronicaCoreDataProvider.ObjParams With {
                    .ObjParametri_Utenti = getObjP(utenti),
                    .ObjParametri_Server = getObjP(server),
                    .ObjParametri_SuperServer = getObjP(superServer)
                }
    End Function

    Public Shared Function GetObjParams(dbName As String) As AgronicaCoreDataProvider.ObjParams
        Select Case dbName

            Case Else
                Throw New ArgumentException("No data for current case")
        End Select
    End Function

End Class
