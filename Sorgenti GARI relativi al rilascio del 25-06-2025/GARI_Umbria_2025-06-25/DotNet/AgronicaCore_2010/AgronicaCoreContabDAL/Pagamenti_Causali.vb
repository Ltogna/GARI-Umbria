Imports System.Data.OleDb
Imports AgronicaCoreDataProvider.UtilityProvider
Imports AgronicaCoreDataProvider.CostantiPersonalizzate
Imports AgronicaCoreDataProvider.TipiEnumerativi
Imports AgronicaCoreDataProvider.AgronicaCoreParametri

Public Class Pagamenti_Causali_R
    Inherits AgronicaCoreDataProvider.DataProvider

    '##############################################################################################
    Public Function Leggi(ByVal Piva As String, _
                            ByVal Cau_Pagamento As Int32, _
                                ByVal xFiltroAggiuntivo As String, _
                                ByVal xOrderBy As String, _
                                ByRef objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri _
                                ) As DataTable

        Dim NomeRoutine As String = "AgronicaCoreContabDAL.Pagamenti_Causali_R.Leggi()"

        Dim MessaggioErrore As String = ""
        Dim StrSQL As New System.Text.StringBuilder
        Dim DT As DataTable

        Try

            StrSQL.Length = 0
            StrSQL.Append(" SELECT  * ")
            StrSQL.Append(" FROM    Pagamenti_Causali ")

            StrSQL.Append(" WHERE ( Piva_SuperUser = 'AAAAAAAAAAA' OR Piva_SuperUser = '" & Agro_SQL_SaveText(objParametri.PivaSuperUser) & "' ) ")

            If Cau_Pagamento <> 0 Then
                StrSQL.Append(" AND Cau_Pagamento = " & Agro_SQL_SaveNum(Cau_Pagamento) & "   " & vbCrLf)
            End If

            '--------------------------------------------------------------------------
            If xFiltroAggiuntivo <> "" Then
                StrSQL.Append(" AND " & Agro_SQL_Save_xFiltroAggiuntivo(xFiltroAggiuntivo, , objParametri))
            End If
            '--------------------------------------------------------------------------

            If xOrderBy <> "" Then
                StrSQL.Append(" ORDER BY " & Agro_SQL_Save_xOrderBy(xOrderBy, objParametri))
            End If
            '------------------------------------------------------------------

            '--------------------------------------------------------------------------
            DT = EseguiQuery_Lettura(objParametri, StrSQL.ToString, NomeRoutine)
            '--------------------------------------------------------------------------

        Catch ex As Exception
            MessaggioErrore = ex.Message
            Scrivi_LOG(objParametri, NomeRoutine, MessaggioErrore)
            DT = Nothing
            Throw New Exception("[" & NomeRoutine & "] : " & MessaggioErrore)
        End Try

        Return DT

    End Function

    '###################################################################################
    Public Sub CauPagamentoSiglaDes_from_CauPagamento(ByVal Piva As String, _
                                                        ByVal Cau_Pagamento As Int32, _
                                                        ByVal xFiltroAggiuntivo As String, _
                                                        ByVal xOrderBy As String, _
                                                        ByRef objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri, _
                                                        ByRef Cau_Pagamento_Sigla As String, _
                                                        ByRef Cau_Pagamento_Des As String)

        Dim NomeRoutine As String = "AgronicaCoreContabDAL.Pagamenti_Causali_R.CauPagamentoSiglaDes_from_CauPagamento()"

        Dim MessaggioErrore As String = ""
        Dim StrSQL As New System.Text.StringBuilder
        Dim DT As DataTable

        Try

            DT = Leggi(Piva, _
                        Cau_Pagamento, _
                        xFiltroAggiuntivo, _
                        xOrderBy, _
                        objParametri)

            If Not DT Is Nothing AndAlso DT.Rows.Count > 0 Then
                Cau_Pagamento_Sigla = DT.Rows(0).Item("Cau_Pagamento_Sigla")
                Cau_Pagamento_Des = DT.Rows(0).Item("Cau_Pagamento_Des")
            End If

            DT = Nothing

        Catch ex As Exception
            MessaggioErrore = ex.Message
            Scrivi_LOG(objParametri, NomeRoutine, MessaggioErrore)
            DT = Nothing
            Throw New Exception("[" & NomeRoutine & "] : " & MessaggioErrore)
        End Try

    End Sub

End Class


'#################################################################
'#################################################################
'#################################################################

Public Class Pagamenti_Causali_W
    Inherits AgronicaCoreDataProvider.DataProvider



    '##############################################################################################
    Public Function Scrivi( _
                          ByVal Piva_SuperUser As String _
                        , ByVal Cau_Pagamento As Integer _
                        , ByVal Cau_Pagamento_Sigla As String _
                        , ByVal Cau_Pagamento_Des As String _
                        , ByVal Giorni_Scadenza As Integer _
                        , ByVal Opzione As Integer _
                        , ByVal Cau_Risorsa As String _
                        , ByVal Tipo As Integer _
                        , ByVal validita_inizio As Date _
                        , ByVal validita_fine As Date _
                        , ByRef objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri _
                , Optional ByVal Data_creazione As Date = #2/1/1900# _
                , Optional ByVal Data_modifica As Date = #2/1/1900# _
                , Optional ByVal username_creazione As String = "" _
                , Optional ByVal username_modifica As String = "" _
            ) As Boolean


        Dim NomeRoutine As String = "Scrivi()"

        '====================================================================================
        'Parametri opzionali :

        '====================================================================================

        Dim MessaggioErrore As String = ""
        Dim StrSQL As New System.Text.StringBuilder
        Dim xRisp As Boolean = False

        Try

            If Data_creazione = #2/1/1900# Then
                Data_creazione = Date.Now
            End If

            If Data_modifica = #2/1/1900# Then
                Data_modifica = Date.Now
            End If

            If username_creazione = "" Then
                username_creazione = objParametri.UsernameOperazione
            End If

            If username_modifica = "" Then
                username_modifica = objParametri.UsernameOperazione
            End If



            '---------------------------------------------
            StrSQL.Length = 0
            StrSQL.Append(" INSERT Pagamenti_Causali  " + vbCrLf)

            StrSQL.Append("              (")

            StrSQL.Append("   [Piva_SuperUser] " & vbCrLf)
            StrSQL.Append("  ,[Cau_Pagamento] " & vbCrLf)
            StrSQL.Append("  ,[Cau_Pagamento_Sigla] " & vbCrLf)
            StrSQL.Append("  ,[Cau_Pagamento_Des] " & vbCrLf)
            StrSQL.Append("  ,[Giorni_Scadenza] " & vbCrLf)
            StrSQL.Append("  ,[Opzione] " & vbCrLf)
            StrSQL.Append("  ,[Cau_Risorsa] " & vbCrLf)
            StrSQL.Append("  ,[Tipo], " & vbCrLf)

            StrSQL.Append("              Inviato,            datainvio, ")
            StrSQL.Append("              Data_Creazione,     Data_Modifica, ")
            StrSQL.Append("              UserName_Creazione, UserName_Modifica, ")
            StrSQL.Append("              Validita_Inizio,    Validita_Fine ")
            StrSQL.Append("              ) ")

            StrSQL.Append(" VALUES ( ")

            StrSQL.Append(" '" & Agro_SQL_SaveText(Piva_SuperUser) & "'" & vbCrLf)
            StrSQL.Append(", " & Agro_SQL_SaveNum(Cau_Pagamento) & " " & vbCrLf)
            StrSQL.Append(",'" & Agro_SQL_SaveText(Cau_Pagamento_Sigla) & "'" & vbCrLf)
            StrSQL.Append(",'" & Agro_SQL_SaveText(Cau_Pagamento_Des) & "'" & vbCrLf)
            StrSQL.Append(", " & Agro_SQL_SaveNum(Giorni_Scadenza) & " " & vbCrLf)
            StrSQL.Append(", " & Agro_SQL_SaveNum(Opzione) & " " & vbCrLf)
            StrSQL.Append(",'" & Agro_SQL_SaveText(Cau_Risorsa) & "'" & vbCrLf)
            StrSQL.Append(", " & Agro_SQL_SaveNum(Tipo) & " " & vbCrLf)


            StrSQL.Append("         , 0  " + vbCrLf)
            StrSQL.Append("         , Null  " + vbCrLf)

            StrSQL.Append("			, " & Agro_SQL_SaveDateTime(Data_creazione) & "  ")
            StrSQL.Append("			, " & Agro_SQL_SaveDateTime(Data_modifica) & "  ")
            StrSQL.Append("			,'" & Agro_SQL_SaveText(username_creazione) & "' ")
            StrSQL.Append("			,'" & Agro_SQL_SaveText(username_modifica) & "' ")

            StrSQL.Append("			, " & Agro_SQL_SaveDate(validita_inizio) & "  ")
            StrSQL.Append("			, " & Agro_SQL_SaveDate(validita_fine) & "  ")


            StrSQL.Append(") ")

            '--------------------------------------------------------------------------
            xRisp = EseguiQuery_Scrittura(objParametri, StrSQL.ToString, NomeRoutine)
            '--------------------------------------------------------------------------

        Catch ex As Exception

            MessaggioErrore = ex.Message
            Scrivi_LOG(objParametri, NomeRoutine, MessaggioErrore)
            xRisp = False
            Throw New Exception("[" & NomeRoutine & "] : " & MessaggioErrore)

        End Try

        Return xRisp

    End Function







    '#################################################################
    Public Function Cancella(ByVal xFiltroAggiuntivo As String, _
                              ByRef objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri _
                              ) As Boolean

        '----- Descrizione
        Dim NomeRoutine As String = "Cancella()"

        Dim MessaggioErrore As String = ""
        Dim StrSQL As New System.Text.StringBuilder
        Dim xRisp As Boolean = False

        Try
            '---------------------------------------------
            StrSQL.Length = 0

            '---------------------------------------------
            If objParametri.FlagCancellazioneLogica = enumCancellazioneLogica.CancellazioneLogica Then
                StrSQL.Append(" UPDATE ... ")
                StrSQL.Append(" SET ")
                StrSQL.Append("         Username_Modifica = '" & Agro_SQL_SaveText(objParametri.UsernameOperazione) & "' ")
                StrSQL.Append("         ,Data_Modifica= " & Agro_SQL_SaveDate(Date.Now) & " ")
                StrSQL.Append("         ,Inviato = -1 ")
                StrSQL.Append(" WHERE   1=1 ")
                StrSQL.Append(" AND     Inviato >= 0 ")
            Else
                StrSQL.Append(" DELETE FROM ... ")
                StrSQL.Append(" WHERE 1=1 ")
            End If
            '---------------------------------------------

            If xFiltroAggiuntivo <> "" Then
                StrSQL.Append(" AND " & Agro_SQL_Save_xFiltroAggiuntivo(xFiltroAggiuntivo, , objParametri))
            End If

            '--------------------------------------------------------------------------
            xRisp = EseguiQuery_Scrittura(objParametri, StrSQL.ToString, NomeRoutine)
            '--------------------------------------------------------------------------

        Catch ex As Exception
            MessaggioErrore = ex.Message
            Scrivi_LOG(objParametri, NomeRoutine, MessaggioErrore)
            xRisp = False
            Throw New Exception("[" & NomeRoutine & "] : " & MessaggioErrore)
        End Try

        Return xRisp

    End Function




End Class


