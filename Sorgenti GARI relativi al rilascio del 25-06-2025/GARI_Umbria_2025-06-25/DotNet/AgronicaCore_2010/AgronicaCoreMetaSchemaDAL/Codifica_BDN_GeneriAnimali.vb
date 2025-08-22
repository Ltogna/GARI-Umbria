Imports System.Data.OleDb
Imports AgronicaCoreDataProvider.UtilityProvider
Imports AgronicaCoreDataProvider.CostantiPersonalizzate
Imports AgronicaCoreDataProvider.TipiEnumerativi

Public Class Codifica_BDN_GeneriAnimali
    Inherits AgronicaCoreDataProvider.DataProvider

    Public Function leggi(ByRef objParametri_Server As AgronicaCoreDataProvider.AgronicaCoreParametri,
                          ByRef GrSpe_Id As String,
                          ByRef Gen_Cod As String,
                          ByRef Sistema_Cod As String) As DataTable
        Dim NomeRoutine As String = "AgronicaCoreMetaSchemaDAL.Codifica_BDN_GeneriAnimali.leggi()"

        Dim MessaggioErrore As String = ""
        Dim Stb As New System.Text.StringBuilder
        Dim DT As DataTable

        Try

            '-----------------------------------------------------------------------------------------------------------------
            Stb.AppendLine("SELECT * ")
            Stb.AppendLine(" FROM [dbo].[Codifica_BDN_GeneriAnimali] ")
            Stb.AppendLine(" WHERE 1 = 1 ")

            If GrSpe_Id <> "" Then
                Stb.AppendLine(" AND GRSPE_ID = " + Agro_SQL_SaveText_NULL(GrSpe_Id) + " ")
            End If

            If Gen_Cod <> "" Then
                Stb.AppendLine(" AND GEN_COD = " + Agro_SQL_SaveText_NULL(Gen_Cod) + " ")
            End If

            If Sistema_Cod <> "" Then
                Stb.AppendLine(" AND Sistema_Cod = " + Agro_SQL_SaveText_NULL(Sistema_Cod) + " ")
            End If

            '-----------------------------------------------------------------------------------------------------------------
            DT = EseguiQuery_Lettura(objParametri_Server, Stb.ToString, NomeRoutine)
            '-----------------------------------------------------------------------------------------------------------------

        Catch ex As Exception

            MessaggioErrore = ex.Message
            Scrivi_LOG(objParametri_Server, NomeRoutine, MessaggioErrore)
            DT = Nothing
            Throw New Exception("[" & NomeRoutine & "] : " & MessaggioErrore)

        End Try

        Return DT

    End Function

    Public Function Gen_Cod_da_GrSpe_Id(ByRef objParametri_Server As AgronicaCoreDataProvider.AgronicaCoreParametri,
                                        ByRef GrSpe_Id As String) As String
        Dim NomeRoutine As String = "AgronicaCoreMetaSchemaDAL.Codifica_Macchine_Agea.Gen_Cod_da_GrSpe_Id()"

        Dim MessaggioErrore As String = ""
        Dim Stb As New System.Text.StringBuilder
        Dim Str_Res As String = ""

        Try

            Dim dt = leggi(objParametri_Server, GrSpe_Id, "", "")

            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                Str_Res = dt.Rows(0)("Gen_Cod")

            End If

        Catch ex As Exception

            MessaggioErrore = ex.Message
            Scrivi_LOG(objParametri_Server, NomeRoutine, MessaggioErrore)
            Throw New Exception("[" & NomeRoutine & "] : " & MessaggioErrore)

        End Try

        Return Str_Res

    End Function

    Public Function GrSpe_Id_da_Gen_Cod(ByRef objParametri_Server As AgronicaCoreDataProvider.AgronicaCoreParametri,
                                        ByRef Gen_Cod As String) As String
        Dim NomeRoutine As String = "AgronicaCoreMetaSchemaDAL.Codifica_Macchine_Agea.GrSpe_Id_da_Gen_Cod()"

        Dim MessaggioErrore As String = ""
        Dim Stb As New System.Text.StringBuilder
        Dim Str_Res As String = ""

        Try

            Dim dt = leggi(objParametri_Server, "", Gen_Cod, "")

            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                Str_Res = dt.Rows(0)("GrSpe_Id")

            End If

        Catch ex As Exception

            MessaggioErrore = ex.Message
            Scrivi_LOG(objParametri_Server, NomeRoutine, MessaggioErrore)
            Throw New Exception("[" & NomeRoutine & "] : " & MessaggioErrore)

        End Try

        Return Str_Res

    End Function


End Class
