Imports AgronicaCoreDataProvider
Imports AgronicaCoreDataProvider.TipiEnumerativi
Imports AgronicaCoreDataProvider.CostantiPersonalizzate
Imports AgronicaCoreEntityFramework
Imports System.Transactions

Public Class Cantina_Insiemi_R
    Inherits AgronicaCoreDataProvider.DataProvider

    '##############################################################################################
    Public Function Leggi(ByVal Piva As String,
                            ByVal Sa_Cod As Integer,
                            ByVal Insieme_Cod As Integer,
                            ByVal Piano_Cod As Integer,
                            ByVal xFiltroAggiuntivo As String,
                            ByVal xOrderBy As String,
                            ByRef objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri
                            ) As DataTable

        Dim NomeRoutine As String = "AgronicaCoreAnagrafeDAL.Cantina_Insiemi_R.Leggi()"

        Dim MessaggioErrore As String = ""
        Dim StrSQL As New System.Text.StringBuilder
        Dim DT As DataTable

        Try

            StrSQL.AppendLine(" SELECT * ")
            StrSQL.AppendLine(" FROM  Cantina_Insiemi ")
            StrSQL.AppendLine(" WHERE 1=1")

            If Piva <> "" Then
                StrSQL.AppendLine(" AND Piva = '" & Agro_SQL_SaveText(Piva) & "' ")
            End If

            If Sa_Cod <> 0 Then
                StrSQL.AppendLine(" AND Sa_Cod = " & Agro_SQL_SaveNum(Sa_Cod) & " ")
            Else
                'leggo se ci sono filtri sui centri
                Dim objUtentiVisibilita As New AgronicaCoreUtentiDAL.Utenti_Visibilita_Appoggio_R
                Dim FiltroCentri As String = ""
                Dim DtCentriVisibili As DataTable
                DtCentriVisibili = objUtentiVisibilita.Leggi(enum_TipoEntita.Centro, " Piva='" & Agro_SQL_SaveText(Piva) & "'", "", objParametri)
                If Not DtCentriVisibili Is Nothing AndAlso DtCentriVisibili.Rows.Count > 0 Then
                    For i = 0 To DtCentriVisibili.Rows.Count - 1
                        FiltroCentri &= DtCentriVisibili.Rows(i).Item("sa_cod") & ","
                    Next
                    If FiltroCentri <> "" Then
                        StrSQL.AppendLine(" AND (Sa_Cod IN (" & Agro_SQL_Save_Clausola_IN(Left(FiltroCentri, FiltroCentri.Length - 1), False) & ") ) ")
                    End If
                End If
            End If


            If Insieme_Cod <> 0 Then
                StrSQL.AppendLine(" AND Insieme_Cod = " & Agro_SQL_SaveNum(Insieme_Cod) & " ")
            End If

            If Piano_Cod <> 0 Then
                StrSQL.AppendLine(" AND Piano_Cod = " & Agro_SQL_SaveNum(Piano_Cod) & " ")
            End If

            '--------------------------------------------------------------------------
            If xFiltroAggiuntivo <> "" Then
                StrSQL.AppendLine(" AND " & Agro_SQL_Save_xFiltroAggiuntivo(xFiltroAggiuntivo, , objParametri))
            End If
            '--------------------------------------------------------------------------
            Select Case objParametri.FlagVisibilita
                Case AgronicaCoreDataProvider.AgronicaCoreParametri.enumVisibilita.Visibilita_SoloNonCancellati
                    StrSQL.AppendLine(" AND   Inviato >=0 ")
                Case AgronicaCoreDataProvider.AgronicaCoreParametri.enumVisibilita.Visibilita_SoloCancellati
                    StrSQL.AppendLine(" AND   Inviato =-1 ")
                Case AgronicaCoreDataProvider.AgronicaCoreParametri.enumVisibilita.Visibilita_Tutti
                    '...................................
                Case Else
                    Throw New Exception("Parametro non corretto nella query (FlagVisibilita)")
            End Select
            '--------------------------------------------------------------------------
            If xOrderBy <> "" Then
                StrSQL.AppendLine(" ORDER BY " & Agro_SQL_Save_xOrderBy(xOrderBy, objParametri))
            Else
                StrSQL.AppendLine(" ORDER BY Identificativo ")
            End If

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

End Class

Public Class Cantina_Insiemi_W
    Inherits AgronicaCoreDataProvider.DataProvider

    '##############################################################################################
    Public Function Scrivi(
                    ByVal Piva As String,
                    ByVal Sa_Cod As Integer,
                    ByVal Piano_Cod As Integer,
                    ByVal Insieme_Cod As Integer,
                    ByVal Insieme_Des As String,
                    ByRef objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri,
                    Optional ByVal Modulo_Generazione As Integer = enum_Omni_Modulo_Generazione.FreshFood,
                    Optional ByVal Validita_Inizio As Date = AGRODATAINIZIO,
                    Optional ByVal Validita_Fine As Date = AGRODATAFINE,
                    Optional ByVal DimX As Integer = 0,
                    Optional ByVal DimY As Integer = 0,
                    Optional ByVal DimH As Integer = 0,
                    Optional ByVal NumX As Integer = 0,
                    Optional ByVal NumY As Integer = 0,
                    Optional ByVal NumH As Integer = 0,
                    Optional ByVal PosX As Integer = 0,
                    Optional ByVal PosY As Integer = 0,
                    Optional ByVal Rotazione As Decimal = 0,
                    Optional ByVal Tipo_Posizionamento As Integer = 0,
                    Optional ByVal Spessore As Integer = 0,
                    Optional ByVal Colore_Esterno As Integer = 0,
                    Optional ByVal Data_Creazione As Date = #2/1/1900#,
                    Optional ByVal Data_Modifica As Date = #2/1/1900#,
                    Optional ByVal Username_Creazione As String = "",
                    Optional ByVal Username_Modifica As String = ""
                ) As Boolean


        Dim NomeRoutine As String = "AgronicaCoreAnagrafeDAL.Cantina_Insiemi_W.Scrivi()"

        '====================================================================================
        'Parametri opzionali :

        '====================================================================================

        Dim MessaggioErrore As String = ""
        Dim StrSQL As New System.Text.StringBuilder
        Dim xRisp As Boolean = False

        Try

            If Data_Creazione = #2/1/1900# Then
                Data_Creazione = Date.Now
            End If

            If Data_Modifica = #2/1/1900# Then
                Data_Modifica = Date.Now
            End If

            If Username_Creazione = "" Then
                Username_Creazione = objParametri.UsernameOperazione
            End If

            If Username_Modifica = "" Then
                Username_Modifica = objParametri.UsernameOperazione
            End If

            '---------------------------------------------
            StrSQL.AppendLine(" INSERT Cantina_Insiemi ( ")
            StrSQL.AppendLine("   [Piva] ")
            StrSQL.AppendLine("  ,[Sa_Cod] ")
            StrSQL.AppendLine("  ,[Piano_Cod] ")
            StrSQL.AppendLine("  ,[Insieme_Cod] ")
            StrSQL.AppendLine("  ,[Insieme_Des] ")
            StrSQL.AppendLine("  ,[Modulo_Generazione] ")

            StrSQL.AppendLine("  ,[DimX] ")
            StrSQL.AppendLine("  ,[DimY] ")
            StrSQL.AppendLine("  ,[DimH] ")
            StrSQL.AppendLine("  ,[NumX] ")
            StrSQL.AppendLine("  ,[NumY] ")
            StrSQL.AppendLine("  ,[NumH] ")
            StrSQL.AppendLine("  ,[PosX] ")
            StrSQL.AppendLine("  ,[PosY] ")
            StrSQL.AppendLine("  ,[Rotazione] ")
            StrSQL.AppendLine("  ,[Tipo_Posizionamento] ")
            StrSQL.AppendLine("  ,[Spessore] ")
            StrSQL.AppendLine("  ,[Colore_Esterno] ")
            StrSQL.AppendLine("  ,[Data_Creazione] ")
            StrSQL.AppendLine("  ,[Data_Modifica] ")
            StrSQL.AppendLine("  ,[Username_Creazione] ")
            StrSQL.AppendLine("  ,[Username_Modifica] ")
            StrSQL.AppendLine("  ,[Validita_Inizio] ")
            StrSQL.AppendLine("  ,[Validita_Fine] ")

            StrSQL.AppendLine(" ) ")

            StrSQL.AppendLine(" VALUES ( ")
            StrSQL.AppendLine(" '" & Agro_SQL_SaveText(Piva) & "'")
            StrSQL.AppendLine(" , " & Agro_SQL_SaveNum(Sa_Cod) & " ")
            StrSQL.AppendLine(" ," & Agro_SQL_SaveNum(Piano_Cod) & " ")
            StrSQL.AppendLine(" , " & Agro_SQL_SaveNum(Insieme_Cod) & " ")
            StrSQL.AppendLine(" ,'" & Agro_SQL_SaveText(Insieme_Des) & "'")
            StrSQL.AppendLine(" , " & Agro_SQL_SaveNum(Modulo_Generazione) & " ")

            StrSQL.AppendLine(" , " & Agro_SQL_SaveNum(DimX) & " ")
            StrSQL.AppendLine(" , " & Agro_SQL_SaveNum(DimY) & " ")
            StrSQL.AppendLine(" , " & Agro_SQL_SaveNum(DimH) & " ")
            StrSQL.AppendLine(" , " & Agro_SQL_SaveNum(NumX) & " ")
            StrSQL.AppendLine(" , " & Agro_SQL_SaveNum(NumY) & " ")
            StrSQL.AppendLine(" , " & Agro_SQL_SaveNum(NumH) & " ")
            StrSQL.AppendLine(" , " & Agro_SQL_SaveNum(PosX) & " ")
            StrSQL.AppendLine(" , " & Agro_SQL_SaveNum(PosY) & " ")
            StrSQL.AppendLine(" ,'" & Agro_SQL_SaveText(Rotazione) & "'")
            StrSQL.AppendLine(" , " & Agro_SQL_SaveNum(Tipo_Posizionamento) & " ")
            StrSQL.AppendLine(" , " & Agro_SQL_SaveNum(Spessore) & " ")
            StrSQL.AppendLine(" , " & Agro_SQL_SaveNum(Colore_Esterno) & " ")

            StrSQL.AppendLine("	, " & Agro_SQL_SaveDateTime(Data_Creazione) & " ")
            StrSQL.AppendLine("	, " & Agro_SQL_SaveDateTime(Data_Modifica) & " ")
            StrSQL.AppendLine("	,'" & Agro_SQL_SaveText(Username_Creazione) & "' ")
            StrSQL.AppendLine("	,'" & Agro_SQL_SaveText(Username_Modifica) & "' ")
            StrSQL.AppendLine(" , " & Agro_SQL_SaveDate(Validita_Inizio) & " ")
            StrSQL.AppendLine(" , " & Agro_SQL_SaveDate(Validita_Fine) & " ")
            StrSQL.AppendLine(" ) ")

            '--------------------------------------------------------------------------
            xRisp = EseguiQuery_Scrittura(objParametri, StrSQL.ToString, NomeRoutine)
            '--------------------------------------------------------------------------

        Catch ex As Exception

            MessaggioErrore = ex.Message
            Scrivi_LOG(objParametri, NomeRoutine, MessaggioErrore)
            Throw New Exception("[" & NomeRoutine & "] : " & MessaggioErrore)

        End Try

        Return xRisp

    End Function


    '#################################################################
    Public Sub Cancella(ByVal Piva As String,
                        ByVal Sa_Cod As Integer,
                        ByVal Piano_Cod As Integer,
                        ByVal Insieme_Cod As Integer,
                        ByRef objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri)

        Dim NomeRoutine As String = "AgronicaCoreAnagrafeDAL.Cantina_Insiemi_W.Cancella()"

        Dim MessaggioErrore As String = ""
        Dim StrSQL As New System.Text.StringBuilder
        Dim xRisp As Boolean = False

        Try

            StrSQL.AppendLine(" DELETE ")
            StrSQL.AppendLine(" FROM Cantina_Caratteristiche ")
            StrSQL.AppendLine(" WHERE PIVA      = '" & Agro_SQL_SaveText(Trim(Piva)) & "' ")
            StrSQL.AppendLine(" AND Sa_Cod    =  " & Agro_SQL_SaveNum(Sa_Cod) & " ")
            StrSQL.AppendLine(" AND Piano_Cod = " & Agro_SQL_SaveNum(Piano_Cod) & " ")

            If Piano_Cod <> 0 Then
                StrSQL.Append(" AND Insieme_Cod = " & Agro_SQL_SaveNum(Insieme_Cod) & "  ")
            End If

            xRisp = EseguiQuery_Scrittura(objParametri, StrSQL.ToString, NomeRoutine)

        Catch ex As Exception
            MessaggioErrore = ex.Message
            Scrivi_LOG(objParametri, NomeRoutine, MessaggioErrore)
            Throw New Exception("[" & NomeRoutine & "] : " & MessaggioErrore)
        End Try

    End Sub

#Region "Cantina Insiemi EF"
    Public Function DeleteCantinaInsiemiEF(
                                          ByVal Piva As String,
                                          ByVal Sa_Cod As Integer,
                                          ByVal insiemeCod As Integer,
                                          ByVal objParametri_Server As AgronicaCoreDataProvider.AgronicaCoreParametri,
                                          ByVal objParametri_Utenti As AgronicaCoreDataProvider.AgronicaCoreParametri,
                                          Optional NoteLog As String = NOTELOG_ANAGRAFE_NG,
                                          Optional SistemaOrigine As Integer = -1
                                          ) As Boolean

        Dim nomeRoutine As String = "AgronicaCoreAnagrafeDAL.Cantina_Insiemi_W.DeleteCantinaInsiemiEF()"
        Dim messaggioErrore As String = ""

        Dim gefutils As New Gias_EF_Utility
        Dim EFConnString As String = gefutils.GetEntityConnectionString(objParametri_Server.StringaConnessione)

        Try
            Dim scopeOption As New TransactionScopeOption
            Dim transactionOptions As New TransactionOptions
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted
            Using scope As New TransactionScope(scopeOption, transactionOptions)
                Using GiasContext As New Gias_DeveloperServer_Entities(EFConnString)

                    Dim insieme = (From v In GiasContext.Cantina_Insiemi Where v.Piva = Piva And
                                                               v.Sa_Cod = Sa_Cod And
                                                               v.Insieme_Cod = insiemeCod).FirstOrDefault

                    GiasContext.Cantina_Insiemi.Remove(insieme)
                    GiasContext.SaveChanges()
                    scope.Complete()
                    scope.Dispose()
                End Using
            End Using
        Catch ex As GiasException
            messaggioErrore = ex.Message
            Throw ex
        Catch ex As Exception
            messaggioErrore = ex.Message
            Throw New Exception("[" & nomeRoutine & "] : " & messaggioErrore)
            Return False
        End Try

        Return True

    End Function
#End Region

End Class
