Imports System.Net
Imports AgronicaCoreDataSTD
Imports AgronicaCoreDataProvider.My.Resources

Public Class Util

    Public Function CheckRequest(objRequest As AgronicaCoreDTOStd.InData.NewAgri.RequestNDistribuito, ByRef errorMessage As String) As Boolean

        errorMessage = ""

        If objRequest Is Nothing Then

            errorMessage = Gias.JsonNonValorizzato
            Return False

        Else

            If objRequest.CUAA = "" Then
                errorMessage = Gias.CuaaNonValorizzato
                Return False
            End If

            If IsNothing(objRequest.ElencoAppezzamenti) OrElse (objRequest.ElencoAppezzamenti IsNot Nothing AndAlso objRequest.ElencoAppezzamenti.Count = 0) Then

                errorMessage = Gias.DatiAppezzamentiNonValorizzati
                Return False
            Else
                For Each appezzamento In objRequest.ElencoAppezzamenti
                    If appezzamento.Chiave_Appezzamento = "" Then
                        errorMessage = String.Format(Gias.AppezzamentoIndexErrore_, objRequest.ElencoAppezzamenti.IndexOf(appezzamento))
                        Return False
                    End If
                Next
            End If
        End If

        Return True
    End Function

End Class
