Imports AgronicaCoreDataProvider.AgronicaCoreParametri
Imports AgronicaCoreDataProvider.UtilityProvider



Public Class AvversitaxGruppoAvversita_R

    Inherits AgronicaCoreDataProvider.DataProvider

    Public Function Leggi(ByVal COD As Int32,
                          ByVal AV_COD As Int32,
                          ByVal AV_GRU As Int32,
                          ByVal Validita_Inizio As Date,
                          ByVal Validita_Fine As Date,
                                ByVal xSelezioneVariabile As enumSelezioneVariabile,
                                ByVal xFiltroAggiuntivo As String,
                                ByVal xOrderBy As String,
                                ByRef objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri
                          ) As DataTable

        Const nomeRoutine = "AgronicaCoreMetaSchemaDAL.AvversitaxGruppoAvversita_R.Leggi()"

        Dim messaggioErrore As String = ""
        Dim StrSQL As New System.Text.StringBuilder
        Dim dt As DataTable

        Try

            Select Case xSelezioneVariabile

                Case enumSelezioneVariabile.Selezione_TabellaDatiMinimi

                Case enumSelezioneVariabile.Selezione_TabellaCompleta

                    StrSQL.Length = 0
                    StrSQL.Append(" SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;")
                    StrSQL.Append(" SELECT * " &
                                  " FROM  AvversitaxGruppoAvversita , Avversita , GruppoAvversita " &
                                  " WHERE AvversitaxGruppoAvversita.Validita_inizio < " & Agro_SQL_SaveDate(Validita_Fine) & " " &
                                  " AND   AvversitaxGruppoAvversita.Validita_Fine > " & Agro_SQL_SaveDate(Validita_Inizio) & " " &
                                  " AND   Avversita.Validita_inizio < " & Agro_SQL_SaveDate(Validita_Fine) & " " &
                                  " AND   Avversita.Validita_Fine > " & Agro_SQL_SaveDate(Validita_Inizio) & " " &
                                  " AND   GruppoAvversita.Validita_inizio < " & Agro_SQL_SaveDate(Validita_Fine) & " " &
                                  " AND   GruppoAvversita.Validita_Fine > " & Agro_SQL_SaveDate(Validita_Inizio) & " " &
                                  " AND   AvversitaxGruppoAvversita.AV_COD = Avversita.AV_COD " &
                                  " AND   AvversitaxGruppoAvversita.AV_GRU = GruppoAvversita.AV_GRU ")


                    If COD <> 0 Then
                        StrSQL.Append(" AND AvversitaxGruppoAvversita.COD = " & Agro_SQL_SaveNum(COD) & "  ")
                    End If

                    If AV_COD <> 0 Then
                        StrSQL.Append(" AND AvversitaxGruppoAvversita.AV_COD = " & Agro_SQL_SaveNum(AV_COD) & "  ")
                    End If

                    If AV_GRU <> 0 Then
                        StrSQL.Append(" AND AvversitaxGruppoAvversita.AV_GRU = " & Agro_SQL_SaveNum(AV_GRU) & "  ")
                    End If


                    '--------------------------------------------------------------------------
                    If xFiltroAggiuntivo <> "" Then
                        StrSQL.Append(" AND " & Agro_SQL_Save_xFiltroAggiuntivo(xFiltroAggiuntivo, , objParametri))
                    End If

                    '--------------------------------------------------------------------------

                    If xOrderBy <> "" Then
                        strSql.Append(" ORDER BY " & Agro_SQL_Save_xOrderBy(xOrderBy, objParametri))
                    Else
                        StrSQL.Append(" ORDER BY AvversitaxGruppoAvversita.AV_GRU ASC, AvversitaxGruppoAvversita.AV_COD ASC")
                    End If


                Case enumSelezioneVariabile.Selezione_JoinDescrizioni


                Case enumSelezioneVariabile.Selezione_JoinCompleta



            End Select



            '--------------------------------------------------------------------------
            dt = EseguiQuery_Lettura(objParametri, StrSQL.ToString, nomeRoutine)
            '--------------------------------------------------------------------------

        Catch ex As Exception
            messaggioErrore = ex.Message
            Scrivi_LOG(objParametri, nomeRoutine, messaggioErrore)
            dt = Nothing
            Throw New Exception("[" & nomeRoutine & "] : " & messaggioErrore)
        End Try

        Return dt


    End Function


    '#############################################################################################################
    '#############################################################################################################
    '#############################################################################################################



    Public Function Leggi2(ByVal COD As Int32,
                              ByVal AV_COD As Int32,
                              ByVal AV_GRU As Int32,
                              ByVal Livello As Int32,
                              ByVal Validita_Inizio As Date,
                              ByVal Validita_Fine As Date,
                                    ByVal xSelezioneVariabile As enumSelezioneVariabile,
                                    ByVal xFiltroAggiuntivo As String,
                                    ByVal xOrderBy As String,
                                    ByRef objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri
                              ) As DataTable

        Const nomeRoutine = "AgronicaCoreMetaSchemaDAL.AvversitaxGruppoAvversita_R.Leggi2()"

        Dim messaggioErrore As String = ""
        Dim StrSQL As New System.Text.StringBuilder
        Dim dt As DataTable

        Try

            Select Case xSelezioneVariabile

                Case enumSelezioneVariabile.Selezione_TabellaDatiMinimi

                Case enumSelezioneVariabile.Selezione_TabellaCompleta

                    StrSQL.Length = 0

                    StrSQL.Append(" SELECT * " &
                                    " FROM  AvversitaxGruppoAvversita , Avversita , GruppoAvversita " &
                                    " WHERE AvversitaxGruppoAvversita.Validita_inizio < " & Agro_SQL_SaveDate(Validita_Fine) & " " &
                                    " AND   AvversitaxGruppoAvversita.Validita_Fine > " & Agro_SQL_SaveDate(Validita_Inizio) & " " &
                                    " AND   Avversita.Validita_inizio < " & Agro_SQL_SaveDate(Validita_Fine) & " " &
                                    " AND   Avversita.Validita_Fine > " & Agro_SQL_SaveDate(Validita_Inizio) & " " &
                                    " AND   GruppoAvversita.Validita_inizio < " & Agro_SQL_SaveDate(Validita_Fine) & " " &
                                    " AND   GruppoAvversita.Validita_Fine > " & Agro_SQL_SaveDate(Validita_Inizio) & " " &
                                    " AND   AvversitaxGruppoAvversita.AV_COD = Avversita.AV_COD " &
                                    " AND   AvversitaxGruppoAvversita.AV_GRU = GruppoAvversita.AV_GRU ")


                    If COD <> 0 Then
                        StrSQL.Append(" AND AvversitaxGruppoAvversita.COD = " & Agro_SQL_SaveNum(COD) & "  ")
                    End If

                    If AV_COD <> 0 Then
                        StrSQL.Append(" AND AvversitaxGruppoAvversita.AV_COD = " & Agro_SQL_SaveNum(AV_COD) & "  ")
                    End If

                    If AV_GRU <> 0 Then
                        StrSQL.Append(" AND AvversitaxGruppoAvversita.AV_GRU = " & Agro_SQL_SaveNum(AV_GRU) & "  ")
                    End If

                    If Livello <> 0 Then
                        StrSQL.Append(" AND GruppoAvversita.Livello = " & Agro_SQL_SaveNum(Livello) & "  ")
                    End If


                    '--------------------------------------------------------------------------
                    If xFiltroAggiuntivo <> "" Then
                        StrSQL.Append(" AND " & Agro_SQL_Save_xFiltroAggiuntivo(xFiltroAggiuntivo, , objParametri))
                    End If

                    '--------------------------------------------------------------------------

                    If xOrderBy <> "" Then
                        strSql.Append(" ORDER BY " & Agro_SQL_Save_xOrderBy(xOrderBy, objParametri))
                    Else
                        StrSQL.Append(" ORDER BY AvversitaxGruppoAvversita.AV_GRU ASC, AvversitaxGruppoAvversita.AV_COD ASC")
                    End If


                Case enumSelezioneVariabile.Selezione_JoinDescrizioni


                Case enumSelezioneVariabile.Selezione_JoinCompleta



            End Select



            '--------------------------------------------------------------------------
            dt = EseguiQuery_Lettura(objParametri, StrSQL.ToString, nomeRoutine)
            '--------------------------------------------------------------------------

        Catch ex As Exception
            messaggioErrore = ex.Message
            Scrivi_LOG(objParametri, nomeRoutine, messaggioErrore)
            dt = Nothing
            Throw New Exception("[" & nomeRoutine & "] : " & messaggioErrore)
        End Try

        Return dt


    End Function


    '#############################################################################################################
    '#############################################################################################################
    '#############################################################################################################

    'A differenza della precedente mette in join anche GruppoAvversitaXGruppoAvversita
    'Livello è considerato quello di quest'ultima tabella
    Public Function Leggi3(ByVal COD As Int32,
                              ByVal AV_COD As Int32,
                              ByVal AV_GRU As Int32,
                              ByVal Livello As Int32,
                              ByVal Validita_Inizio As Date,
                              ByVal Validita_Fine As Date,
                                    ByVal xSelezioneVariabile As enumSelezioneVariabile,
                                    ByVal xFiltroAggiuntivo As String,
                                    ByVal xOrderBy As String,
                                    ByRef objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri
                              ) As DataTable

        Const nomeRoutine = "AgronicaCoreMetaSchemaDAL.AvversitaxGruppoAvversita_R.Leggi3()"

        Dim messaggioErrore As String = ""
        Dim StrSQL As New System.Text.StringBuilder
        Dim dt As DataTable

        Try

            Select Case xSelezioneVariabile

                Case enumSelezioneVariabile.Selezione_TabellaDatiMinimi

                Case enumSelezioneVariabile.Selezione_TabellaCompleta

                    StrSQL.Length = 0
                    StrSQL.Append(" SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;")
                    StrSQL.Append(" SELECT * " &
                                    " FROM  AvversitaxGruppoAvversita , Avversita , GruppoAvversita, GruppoAvversitaXGruppoAvversita " &
                                    " WHERE AvversitaxGruppoAvversita.Validita_inizio < " & Agro_SQL_SaveDate(Validita_Fine) & " " &
                                    " AND   AvversitaxGruppoAvversita.Validita_Fine > " & Agro_SQL_SaveDate(Validita_Inizio) & " " &
                                    " AND   Avversita.Validita_inizio < " & Agro_SQL_SaveDate(Validita_Fine) & " " &
                                    " AND   Avversita.Validita_Fine > " & Agro_SQL_SaveDate(Validita_Inizio) & " " &
                                    " AND   GruppoAvversita.Validita_inizio < " & Agro_SQL_SaveDate(Validita_Fine) & " " &
                                    " AND   GruppoAvversita.Validita_Fine > " & Agro_SQL_SaveDate(Validita_Inizio) & " " &
                                    " AND   GruppoAvversitaXGruppoAvversita.av_gru_a = AvversitaxGruppoAvversita.AV_GRU " &
                                    " AND   AvversitaxGruppoAvversita.AV_COD = Avversita.AV_COD " &
                                    " AND   AvversitaxGruppoAvversita.AV_GRU = GruppoAvversita.AV_GRU ")


                    If COD <> 0 Then
                        StrSQL.Append(" AND AvversitaxGruppoAvversita.COD = " & Agro_SQL_SaveNum(COD) & "  ")
                    End If

                    If AV_COD <> 0 Then
                        StrSQL.Append(" AND AvversitaxGruppoAvversita.AV_COD = " & Agro_SQL_SaveNum(AV_COD) & "  ")

                        'prendo il gruppo padre della singola col livello più alto
                        StrSQL.Append(" AND   GruppoAvversitaXGruppoAvversita.Livello =  ")
                        StrSQL.Append(" ( SELECT MAX(livello) from GruppoAvversitaXGruppoAvversita , AvversitaxGruppoAvversita ")
                        StrSQL.Append("   WHERE GruppoAvversitaXGruppoAvversita.AV_Gru_A = AvversitaxGruppoAvversita.AV_GRU ")
                        StrSQL.Append("   AND AvversitaxGruppoAvversita.AV_COD =  " & Agro_SQL_SaveNum(AV_COD))
                        StrSQL.Append(" )")
                    End If

                    If AV_GRU <> 0 Then
                        StrSQL.Append(" AND AvversitaxGruppoAvversita.AV_GRU = " & Agro_SQL_SaveNum(AV_GRU) & "  ")
                    End If

                    If Livello <> 0 Then
                        StrSQL.Append(" AND GruppoAvversitaXGruppoAvversita.Livello = " & Agro_SQL_SaveNum(Livello) & "  ")
                    End If




                    '--------------------------------------------------------------------------
                    If xFiltroAggiuntivo <> "" Then
                        StrSQL.Append(" AND " & Agro_SQL_Save_xFiltroAggiuntivo(xFiltroAggiuntivo, , objParametri))
                    End If

                    '--------------------------------------------------------------------------

                    If xOrderBy <> "" Then
                        strSql.Append(" ORDER BY " & Agro_SQL_Save_xOrderBy(xOrderBy, objParametri))
                    Else
                        StrSQL.Append(" ORDER BY AvversitaxGruppoAvversita.AV_GRU ASC, AvversitaxGruppoAvversita.AV_COD ASC")
                    End If


                Case enumSelezioneVariabile.Selezione_JoinDescrizioni


                Case enumSelezioneVariabile.Selezione_JoinCompleta



            End Select



            '--------------------------------------------------------------------------
            dt = EseguiQuery_Lettura(objParametri, StrSQL.ToString, nomeRoutine)
            '--------------------------------------------------------------------------

        Catch ex As Exception
            messaggioErrore = ex.Message
            Scrivi_LOG(objParametri, nomeRoutine, messaggioErrore)
            dt = Nothing
            Throw New Exception("[" & nomeRoutine & "] : " & messaggioErrore)
        End Try

        Return dt


    End Function



    Public Function LeggiDettaglioMinimo(AV_GRU_A As Integer,
                                         ByRef objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri
                                         ) As DataTable

        Const nomeRoutine = "AgronicaCoreMetaSchemaDAL.AvversitaxGruppoAvversita_R.LeggiDettaglioMinimo()"

        Dim messaggioErrore As String = ""
        Dim StrSQL As New System.Text.StringBuilder
        Dim dt As DataTable

        Try

            StrSQL.Length = 0
            StrSQL.AppendLine("SELECT AV_Gru_Da AS Vertice1_Padre, AV_Gru_A AS Vertice2_Figlio, GGA.Livello AS Peso")
            StrSQL.AppendLine("FROM GruppoAvversitaXGruppoAvversita GGA")
            StrSQL.AppendLine("     INNER JOIN GruppoAvversita GG1")
            StrSQL.AppendLine("         ON GGA.AV_Gru_Da = GG1.Av_Gru")
            StrSQL.AppendLine("     INNER JOIN GruppoAvversita GG2")
            StrSQL.AppendLine("         ON GGA.AV_Gru_A = GG2.Av_Gru")


            StrSQL.AppendLine("WHERE 1 = 1")
            If AV_GRU_A <> 0 Then
                StrSQL.Append(" AND AV_Gru_A = " & Agro_SQL_SaveNum(AV_GRU_A) & "  ")
            End If

            '--------------------------------------------------------------------------
            dt = EseguiQuery_Lettura(objParametri, StrSQL.ToString, nomeRoutine)
            '--------------------------------------------------------------------------

        Catch ex As Exception
            messaggioErrore = ex.Message
            Scrivi_LOG(objParametri, nomeRoutine, messaggioErrore)
            dt = Nothing
            Throw New Exception("[" & nomeRoutine & "] : " & messaggioErrore)
        End Try

        Return dt

    End Function

End Class
