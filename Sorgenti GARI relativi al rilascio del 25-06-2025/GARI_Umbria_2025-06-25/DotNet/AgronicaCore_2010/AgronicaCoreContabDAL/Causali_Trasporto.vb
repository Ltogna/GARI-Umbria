Imports System.Text
Imports AgronicaCoreDataProvider.AgronicaCoreParametri
Imports AgronicaCoreDataProvider.TipiEnumerativi
Imports AgronicaCoreDataProvider.UtilityProvider

Public Class Causali_Trasporto_R
    Inherits AgronicaCoreDataProvider.DataProvider

    '##############################################################################################
    Public Function Leggi(ByVal Causale_Trasporto_Cod As Integer,
                          ByVal Causale_Trasporto_Sigla As String,
                          ByVal Causale_Trasporto_Des As String,
                          ByVal ChkDefault As Integer,
                          ByVal Tipo As enum_Tipo_CausaliTrasporto,
                          ByVal ChkPrefissoSuffisso As Integer,
                          ByVal xFiltroAggiuntivo As String,
                          ByVal xOrderBy As String,
                          ByRef objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri
                          ) As DataTable

        Dim nomeRoutine As String = "AgronicaCoreContabDAL.Causali_Trasporto_R.Leggi()"

        Dim messaggioErrore As String = ""
        Dim stb As New StringBuilder
        Dim dt As DataTable

        Try

            stb.Length = 0

            stb.AppendLine(" SELECT * ")
            stb.AppendLine(" FROM Causali_Trasporto ")
            stb.AppendLine(" WHERE (Piva_SuperUser = '" & Agro_SQL_SaveText(objParametri.PivaSuperUser) & "' OR Piva_SuperUser = 'AAAAAAAAAAA') ")
            
            If Causale_Trasporto_Cod <> 0 Then
                stb.AppendLine(" AND Causale_Trasporto_Cod = " & Agro_SQL_SaveNum(Causale_Trasporto_Cod) & " ")
            End If

            If Causale_Trasporto_Sigla <> "" Then
                stb.AppendLine(" AND Causale_Trasporto_Sigla = '" & Agro_SQL_SaveText(Causale_Trasporto_sigla) & "' ")
            End If

            If Causale_Trasporto_Des <> "" Then
                stb.AppendLine(" AND Causale_Trasporto_Des = '" & Agro_SQL_SaveText(Causale_Trasporto_Des) & "' ")
            End If

            If ChkDefault <> -1 Then
                stb.AppendLine(" AND ChkDefault = " & Agro_SQL_SaveNum(ChkDefault) & " ")
            End If

            If Tipo <> enum_Tipo_CausaliTrasporto.Non_Impostato Then
                stb.AppendLine(" AND Tipo = " & Agro_SQL_SaveNum(CInt(Tipo)) & " ")
            End If

            If ChkPrefissoSuffisso <> -1 Then
                stb.AppendLine(" AND ChkPrefissoSuffisso = " & Agro_SQL_SaveNum(ChkPrefissoSuffisso) & " ")
            End If


            If xFiltroAggiuntivo <> "" Then
                stb.AppendLine(" AND " & Agro_SQL_Save_xFiltroAggiuntivo(xFiltroAggiuntivo, , objParametri))
            End If
            '--------------------------------------------------------------------------
            Select Case objParametri.FlagVisibilita
                Case enumVisibilita.Visibilita_SoloNonCancellati
                    stb.AppendLine(" AND   Inviato >=0 ")
                Case enumVisibilita.Visibilita_SoloCancellati
                    stb.AppendLine(" AND   Inviato =-1 ")
                Case enumVisibilita.Visibilita_Tutti
                    '...................................
                Case Else
                    Throw New Exception("Parametro non corretto nella query (FlagVisibilita)")
            End Select
            '--------------------------------------------------------------------------

            If xOrderBy <> "" Then
                stb.AppendLine(" ORDER BY " & Agro_SQL_Save_xOrderBy(xOrderBy, objParametri))            
            End If

            '--------------------------------------------------------------------------
            dt = EseguiQuery_Lettura(objParametri, stb.ToString, nomeRoutine)
            '--------------------------------------------------------------------------

        Catch ex As Exception
            messaggioErrore = ex.Message
            Scrivi_LOG(objParametri, nomeRoutine, messaggioErrore)
            dt = Nothing
            Throw New Exception("[" & nomeRoutine & "] : " & messaggioErrore)
        End Try

        Return dt

    End Function

    Public Function CausaleTrasportoCodFromDes(ByVal causaleTrasportoDes As String,
                                               ByRef objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri
                                               ) As Integer

        Dim nomeRoutine As String = "AgronicaCoreContabDAL.Causali_Trasporto_R.CausaleTrasportoCodFromDes()"

        Dim messaggioErrore As String = ""
        Dim dt As DataTable
        Dim causaleTrasportoCod As Integer = 0

        Try

            dt = Leggi(0, "", causaleTrasportoDes, -1, enum_Tipo_CausaliTrasporto.Non_Impostato, -1, "", "", objParametri)

            If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                causaleTrasportoCod = dt.Rows(0).Item("Causale_Trasporto_Cod")
            End If

        Catch ex As Exception
            messaggioErrore = ex.Message
            Scrivi_LOG(objParametri, nomeRoutine, messaggioErrore)
            dt = Nothing
            Throw New Exception("[" & nomeRoutine & "] : " & messaggioErrore)
        End Try

        Return causaleTrasportoCod

    End Function

End Class


'#################################################################
'#################################################################
'#################################################################
