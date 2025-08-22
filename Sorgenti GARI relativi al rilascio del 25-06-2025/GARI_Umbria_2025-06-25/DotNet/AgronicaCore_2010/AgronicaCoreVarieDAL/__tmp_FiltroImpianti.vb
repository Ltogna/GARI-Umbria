



Public Class __tmp_FiltroImpianti_R
    Inherits AgronicaCoreDataProvider.DataProvider


    Public Function VerificaEsistenzaCampoDataCreazione(ByRef objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri) As Boolean


        Dim NomeRoutine As String = "VerificaEsistenzaCampoDataCreazione()"

        '====================================================================================
        'Parametri opzionali :

        '====================================================================================

        Dim MessaggioErrore As String = ""
        Dim Stb As New System.Text.StringBuilder
        Dim xRisp As Boolean = False
        Dim dt As DataTable

        Try



            '---------------------------------------------
            Stb.Length = 0
            Stb.AppendLine(" Select 1 from sys.tables tt  ")
            Stb.AppendLine(" inner Join sys.columns cc on tt.object_id = cc.object_id  ")
            Stb.AppendLine(" where tt.name = '__tmp_FiltroImpianti'  ")
            Stb.AppendLine(" And cc.name = 'Data_Creazione' ")

            '--------------------------------------------------------------------------
            dt = EseguiQuery_Lettura(objParametri, Stb.ToString, NomeRoutine)
            '--------------------------------------------------------------------------

            If dt.Rows.Count > 0 Then
                xRisp = True
            End If

        Catch ex As Exception

            MessaggioErrore = ex.Message
            Scrivi_LOG(objParametri, NomeRoutine, MessaggioErrore)
            xRisp = False
            Throw New Exception("[" & NomeRoutine & "] : " & MessaggioErrore)

        End Try

        Return xRisp

    End Function

    Public Function ConteggioRecordDistinctPivaDaIDTestataTemp(ByVal IDTestataTemp As Integer, ByVal objParametri_Server As AgronicaCoreDataProvider.AgronicaCoreParametri) As DataTable

        Dim NomeRoutine As String = "ConteggioRecordDistinctPivaDaIDTestataTemp"
        Dim MessaggioErrore As String = ""
        Dim stb As New System.Text.StringBuilder
        Dim DT As DataTable

        Try

            stb.Length = 0

            stb.AppendLine(" Select distinct piva  ")
            stb.AppendLine(" From __tmp_FiltroImpianti ")
            stb.AppendLine(" Where IDTestataTemp = " & IDTestataTemp)



            '-----------------------------------------------------------------------------------------------------------
            DT = EseguiQuery_Lettura(objParametri_Server, stb.ToString, NomeRoutine)
            '-----------------------------------------------------------------------------------------------------------


        Catch ex As Exception

            DT = Nothing
            MessaggioErrore = "[" & NomeRoutine & "]:" & ex.Message & " "
            Throw New Exception(MessaggioErrore)

        End Try

        Return DT
    End Function

    Public Function LeggiElencoDaIDTestatTemp(ByVal IDTestataTemp As Integer, ByVal objParametri_Server As AgronicaCoreDataProvider.AgronicaCoreParametri) As DataTable

        Dim NomeRoutine As String = "LeggiElencoDaIDTestatTemp"
        Dim MessaggioErrore As String = ""
        Dim stb As New System.Text.StringBuilder
        Dim DT As DataTable

        Try

            stb.Length = 0

            stb.AppendLine(" Select *  ")
            stb.AppendLine(" From __tmp_FiltroImpianti ")
            stb.AppendLine(" Where IDTestataTemp = " & IDTestataTemp)

            '-----------------------------------------------------------------------------------------------------------
            DT = EseguiQuery_Lettura(objParametri_Server, stb.ToString, NomeRoutine)
            '-----------------------------------------------------------------------------------------------------------

        Catch ex As Exception

            DT = Nothing
            MessaggioErrore = "[" & NomeRoutine & "]:" & ex.Message & " "
            Throw New Exception(MessaggioErrore)

        End Try

        Return DT

    End Function

    Public Function LeggiElencoDaUsername(ByVal username As String, ByVal objParametri_Server As AgronicaCoreDataProvider.AgronicaCoreParametri) As DataTable

        Dim NomeRoutine As String = "LeggiElencoDaUsername"
        Dim MessaggioErrore As String = ""
        Dim stb As New System.Text.StringBuilder
        Dim DT As DataTable

        Try

            stb.Length = 0

            stb.AppendLine(" Select *  ")
            stb.AppendLine(" From __tmp_FiltroImpianti ")
            stb.AppendLine(" Where IDTestataTemp = ")
            stb.AppendLine("(Select MAX(idTestataTemp) from __tmp_FiltroImpianti where username_creazione = '" & username & "')")

            '-----------------------------------------------------------------------------------------------------------
            DT = EseguiQuery_Lettura(objParametri_Server, stb.ToString, NomeRoutine)
            '-----------------------------------------------------------------------------------------------------------

        Catch ex As Exception

            DT = Nothing
            MessaggioErrore = "[" & NomeRoutine & "]:" & ex.Message & " "
            Throw New Exception(MessaggioErrore)

        End Try

        Return DT

    End Function

End Class


'#################################################################
'#################################################################
'#################################################################

Public Class __tmp_FiltroImpianti_W
    Inherits AgronicaCoreDataProvider.DataProvider



    '##############################################################################################
    Public Function CancellaVecchiRecordPerDataCreazione(ByRef objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri) As Boolean


        Dim NomeRoutine As String = "CancellaVecchiRecordPerDataCreazione()"

        '====================================================================================
        'Parametri opzionali :

        '====================================================================================

        Dim MessaggioErrore As String = ""
        Dim Stb As New System.Text.StringBuilder
        Dim xRisp As Boolean = False

        Try



            '---------------------------------------------
            Stb.Length = 0
            Stb.AppendLine(" Delete  ")
            Stb.AppendLine(" From __tmp_FiltroImpianti ")
            Stb.AppendLine(" Where DateDiff(d, Data_Creazione, GETDATE()) > 7")

            '--------------------------------------------------------------------------
            xRisp = EseguiQuery_Scrittura(objParametri, Stb.ToString, NomeRoutine)
            '--------------------------------------------------------------------------

        Catch ex As Exception

            MessaggioErrore = ex.Message
            Scrivi_LOG(objParametri, NomeRoutine, MessaggioErrore)
            xRisp = False
            Throw New Exception("[" & NomeRoutine & "] : " & MessaggioErrore)

        End Try

        Return xRisp

    End Function


    Public Function CancellaRecordDaUsername(ByRef objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri) As Boolean

        Dim NomeRoutine As String = "CancellaRecordDaUsername()"

        Dim MessaggioErrore As String = ""
        Dim Stb As New System.Text.StringBuilder
        Dim xRisp As Boolean = False

        Try

            '---------------------------------------------
            Stb.Length = 0
            Stb.AppendLine(" Delete  ")
            Stb.AppendLine(" From __tmp_FiltroImpianti ")
            Stb.AppendLine(" Where Username_Creazione = '" & objParametri.UtenteUsername & "'")

            '--------------------------------------------------------------------------
            xRisp = EseguiQuery_Scrittura(objParametri, Stb.ToString, NomeRoutine)
            '--------------------------------------------------------------------------

        Catch ex As Exception

            MessaggioErrore = ex.Message
            Scrivi_LOG(objParametri, NomeRoutine, MessaggioErrore)
            xRisp = False
            Throw New Exception("[" & NomeRoutine & "] : " & MessaggioErrore)

        End Try

        Return xRisp


    End Function


    Public Function CancellaRecordDaIDTestataTemp(ByVal IDTestataTemp As Integer, ByRef objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri) As Boolean

        Dim NomeRoutine As String = "CancellaRecordDaUsername()"

        Dim MessaggioErrore As String = ""
        Dim Stb As New System.Text.StringBuilder
        Dim xRisp As Boolean = False

        Try

            '---------------------------------------------
            Stb.Length = 0
            Stb.AppendLine(" Delete  ")
            Stb.AppendLine(" From __tmp_FiltroImpianti ")
            Stb.AppendLine(" Where IDTestataTemp = " & IDTestataTemp)

            '--------------------------------------------------------------------------
            xRisp = EseguiQuery_Scrittura(objParametri, Stb.ToString, NomeRoutine)
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
