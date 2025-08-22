Imports System.Text


Public Class ClassiDiSpecie_R
    Inherits AgronicaCoreDataProvider.DataProvider

    Public Function SpecieAgronicaFromClasseDiSpecie( _
                    ByVal ID_Specie As Integer, _
                    ByVal ID_SottoSpecie As Integer, _
                    ByVal ID_Gruppo As Integer, _
                    ByVal ID_Genotipo As Integer, _
                    ByRef objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri _
            ) As DataTable



        Dim DT As DataTable
        Dim NomeRoutine As String = "ClassiDiSpecie_R.SpecieAgronicaFromClasseDiSpecie"
        Dim MessaggioErrore As String = ""

        Try
            Dim Stb As New StringBuilder

            Stb.Append("select distinct veg.veg_cod, veg.veg_des " & vbCrLf)
            Stb.Append(" from Sementieri_ClassiDiSpecieVegetali ss " & vbCrLf)
            Stb.Append("    inner join Mappatura_Specie mp  " & vbCrLf)
            Stb.Append("        on  mp.ID_Specie = ss.id_specie " & vbCrLf)
            Stb.Append("        and mp.ID_Sottospecie = ss.ID_Sottospecie " & vbCrLf)
            Stb.Append("        and mp.ID_Gruppo = ss.ID_Gruppo  " & vbCrLf)
            Stb.Append("        and mp.ID_Genotipo = ss.ID_Genotipo  " & vbCrLf)
            Stb.Append("    inner join SpecieVegetali  veg " & vbCrLf)
            Stb.Append("        on veg.veg_cod = mp.veg_cod " & vbCrLf)
            Stb.Append(" where 1=1 " & vbCrLf)

            If ID_Specie <> 0 Then
                Stb.Append(" and mp.ID_Specie = " & ID_Specie & vbCrLf)
            End If

            If ID_SottoSpecie <> 0 Then
                Stb.Append(" and mp.ID_SottoSpecie = " & ID_SottoSpecie & vbCrLf)
            End If

            If ID_Gruppo <> 0 Then
                Stb.Append(" and mp.ID_Gruppo = " & ID_Gruppo & vbCrLf)
            End If

            If ID_Genotipo <> 0 Then
                Stb.Append(" and mp.ID_Genotipo = @ID_Genotipo " & vbCrLf)
            End If

            '--------------------------------------------------------------------------
            DT = EseguiQuery_Lettura(objParametri, Stb.ToString, NomeRoutine)
            '--------------------------------------------------------------------------

        Catch ex As Exception

            MessaggioErrore = ex.Message
            Scrivi_LOG(objParametri, NomeRoutine, MessaggioErrore)
            DT = Nothing
            Throw New Exception("[" & NomeRoutine & "] : " & MessaggioErrore)

        End Try

        Return DT

    End Function


    Public Function distinct_sementieri_classidispecievegetali_des(ByRef objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri) As DataTable

        Dim DT As DataTable
        Dim NomeRoutine As String = "ClassiDiSpecie_R.SpecieAgronicaFromClasseDiSpecie"
        Dim MessaggioErrore As String = ""

        Try
            Dim Stb As New StringBuilder

            Stb.Append("select distinct id_specie, sementieri_classidispecievegetali_des " & vbCrLf)
            Stb.Append(" from Sementieri_ClassiDiSpecieVegetali " & vbCrLf)
            Stb.Append(" order by sementieri_classidispecievegetali_des " & vbCrLf)

            '--------------------------------------------------------------------------
            DT = EseguiQuery_Lettura(objParametri, Stb.ToString, NomeRoutine)
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



Public Class ClassiDiSpecie_W

End Class
