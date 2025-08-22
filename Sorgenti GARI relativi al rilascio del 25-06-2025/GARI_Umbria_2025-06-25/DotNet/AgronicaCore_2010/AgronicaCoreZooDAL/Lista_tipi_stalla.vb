Imports System.Data.OleDb
Imports AgronicaCoreDataProvider
Imports AgronicaCoreDataProvider.UtilityProvider

Public Class Lista_tipi_stalla_R
    Inherits AgronicaCoreDataProvider.DataProvider


    '##############################################################################################
    <Obsolete("DEPRECATA, usare AgronicaCoreMetaschemaDAL.Lista_Tipi_Stalla_R.Leggi()")>
    Public Function Leggi(
                        ByVal Gen_Cod As Long,
                        ByVal Spe_Cod As Long,
                        ByVal Ipro_Cod As Long,
                        ByVal Cod_Fabb As String,
                            ByVal xSelezioneVariabile As AgronicaCoreDataProvider.AgronicaCoreParametri.enumSelezioneVariabile,
                            ByVal xFiltroAggiuntivo As String,
                            ByVal xOrderBy As String,
                            ByRef objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri
                            ) As DataTable

        Dim NomeRoutine As String = "AgronicaCoreZooDAL.Lista_Specie_Animali_R.Lista_tipi_stalla_R()"


        Dim MessaggioErrore As String = ""
        Dim StrSQL As New System.Text.StringBuilder
        Dim DT As DataTable

        Try

            Select Case xSelezioneVariabile

                Case AgronicaCoreDataProvider.AgronicaCoreParametri.enumSelezioneVariabile.Selezione_TabellaDatiMinimi



                Case AgronicaCoreDataProvider.AgronicaCoreParametri.enumSelezioneVariabile.Selezione_TabellaCompleta

                    StrSQL.Length = 0


                    StrSQL.Append(" SELECT * ")
                    StrSQL.Append(" FROM  Lista_Tipi_Stalla ")
                    StrSQL.Append(" WHERE Validita_inizio < " & Agro_SQL_SaveDate(objParametri.FinestraTemporaleFine) & " ")
                    StrSQL.Append(" AND   Validita_Fine > " & Agro_SQL_SaveDate(objParametri.FinestraTemporaleInizio) & " ")


                    If Gen_Cod <> 0 Then
                        StrSQL.Append(" AND GEN_COD = " & Agro_SQL_SaveNum(Gen_Cod) & "  ")
                    End If

                    If Spe_Cod <> 0 Then
                        StrSQL.Append(" AND SPE_COD = " & Agro_SQL_SaveNum(Spe_Cod) & "  ")
                    End If

                    If Ipro_Cod <> 0 Then
                        StrSQL.Append(" AND IPRO_COD = " & Agro_SQL_SaveNum(Ipro_Cod) & "  ")
                    End If

                    If Cod_Fabb <> "" Then
                        StrSQL.Append(" AND COD_FABB = '" & Agro_SQL_SaveText(Cod_Fabb) & "'  ")
                    End If



                    If xFiltroAggiuntivo <> "" Then
                        StrSQL.Append(" AND " & Agro_SQL_Save_xFiltroAggiuntivo(xFiltroAggiuntivo, , objParametri))
                    End If
                    '--------------------------------------------------------------------------
                    Select Case objParametri.FlagVisibilita
                        Case AgronicaCoreDataProvider.AgronicaCoreParametri.enumVisibilita.Visibilita_SoloNonCancellati
                            StrSQL.Append(" AND   Lista_Tipi_Stalla.Inviato >=0 ")
                        Case AgronicaCoreDataProvider.AgronicaCoreParametri.enumVisibilita.Visibilita_SoloCancellati
                            StrSQL.Append(" AND   Lista_Tipi_Stalla.Inviato =-1 ")
                        Case AgronicaCoreDataProvider.AgronicaCoreParametri.enumVisibilita.Visibilita_Tutti
                            '...................................
                        Case Else
                            Throw New Exception("Parametro non corretto nella query (FlagVisibilita)")
                    End Select
                    '--------------------------------------------------------------------------
                    If xOrderBy <> "" Then
                        StrSQL.Append(" ORDER BY " & Agro_SQL_Save_xOrderBy(xOrderBy, objParametri))
                    Else
                        StrSQL.Append(" ORDER BY COD_FABB ASC ")
                    End If
                    '------------------------------------------------------------------

                Case AgronicaCoreDataProvider.AgronicaCoreParametri.enumSelezioneVariabile.Selezione_JoinDescrizioni
                    '
                    '
                    '
                    '


                Case AgronicaCoreDataProvider.AgronicaCoreParametri.enumSelezioneVariabile.Selezione_JoinCompleta
                    '
                    '
                    '
                    '


            End Select

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


    ''' <param name="IPro_Cod">To avoid using it as filter, pass -1 as value</param>
    <Obsolete("DEPRECATA, usare AgronicaCoreMetaschemaDAL.LeggiDaRegolamentoCategoria.Leggi()")>
    Public Function LeggiDaRegolamentoCategoria(
        Regolamento_Cod As Integer,
        Spe_Cod As Integer,
        Gen_Cod As Integer,
        Cat_Cod As Integer,
        IPro_Cod As Integer,
        objParametri As AgronicaCoreParametri,
        Optional xFiltroAggiuntivo As String = "",
        Optional xOrderBy As String = ""
    ) As DataTable
        Dim NomeRoutine As String = "AgronicaCoreMetaSchemaDAL.Lista_Tipi_Stalla_R.LeggiDaRegolamentoCategoria"
        Dim StrSQL As New Text.StringBuilder With {.Length = 0}
        Try
            StrSQL.AppendLine(" SELECT Lista_Tipi_Fabbricati.DESCR as Descr_Fabb, Lista_TipiStallaxCategorie.* ")
            StrSQL.AppendLine(" FROM  Lista_TipiStallaxCategorie ")
            StrSQL.AppendLine(" INNER JOIN Lista_Tipi_Fabbricati ")
            StrSQL.AppendLine("   ON Lista_TipiStallaxCategorie.COD_FABB COLLATE Latin1_General_CI_AS ")
            StrSQL.AppendLine("   = Lista_Tipi_Fabbricati.COD_FABB COLLATE Latin1_General_CI_AS ")

            StrSQL.AppendLine(" WHERE Lista_TipiStallaxCategorie.Validita_inizio <= " & Agro_SQL_SaveDate(objParametri.FinestraTemporaleFine) & " ")
            StrSQL.AppendLine(" AND   Lista_TipiStallaxCategorie.Validita_Fine >= " & Agro_SQL_SaveDate(objParametri.FinestraTemporaleInizio) & " ")

            If Spe_Cod <> 0 Then
                StrSQL.AppendLine(" AND Lista_TipiStallaxCategorie.Spe_Cod = " & Agro_SQL_SaveNum(Spe_Cod) & " ")
            End If
            If Gen_Cod <> 0 Then
                StrSQL.AppendLine(" AND Lista_TipiStallaxCategorie.Gen_Cod = " & Agro_SQL_SaveNum(Gen_Cod) & " ")
            End If
            If Cat_Cod > -1 Then
                StrSQL.AppendLine(" AND Lista_TipiStallaxCategorie.Cat_Cod = " & Agro_SQL_SaveNum(Cat_Cod) & " ")
            End If
            If IPro_Cod > -1 Then
                StrSQL.AppendLine(" AND Lista_TipiStallaxCategorie.IPro_Cod = " & Agro_SQL_SaveNum(IPro_Cod) & " ")
            End If
            If Regolamento_Cod <> 0 Then
                StrSQL.AppendLine(" AND Lista_TipiStallaxCategorie.Regolamento_Cod = " & Agro_SQL_SaveNum(Regolamento_Cod) & " ")
            End If
            '--------------------------------------------------------------------------
            If xFiltroAggiuntivo <> "" Then
                StrSQL.AppendLine(" AND " & Agro_SQL_Save_xFiltroAggiuntivo(xFiltroAggiuntivo, , objParametri))
            End If
            '--------------------------------------------------------------------------
            Select Case objParametri.FlagVisibilita
                Case AgronicaCoreDataProvider.AgronicaCoreParametri.enumVisibilita.Visibilita_SoloNonCancellati
                    StrSQL.AppendLine(" AND   Lista_TipiStallaxCategorie.Inviato >=0 ")
                Case AgronicaCoreDataProvider.AgronicaCoreParametri.enumVisibilita.Visibilita_SoloCancellati
                    StrSQL.AppendLine(" AND   Lista_TipiStallaxCategorie.Inviato =-1 ")
                Case AgronicaCoreDataProvider.AgronicaCoreParametri.enumVisibilita.Visibilita_Tutti
                    '...................................
                Case Else
                    Throw New Exception("Parametro non corretto nella query (FlagVisibilita)")
            End Select
            '--------------------------------------------------------------------------
            If xOrderBy <> "" Then
                StrSQL.AppendLine(" ORDER BY " & Agro_SQL_Save_xOrderBy(xOrderBy, objParametri))
            End If
            '--------------------------------------------------------------------------
            Return EseguiQuery_Lettura(objParametri, StrSQL.ToString, NomeRoutine)
        Catch ex As Exception
            Scrivi_LOG(objParametri, NomeRoutine, ex.Message)
            Throw New Exception("[" & NomeRoutine & "] : " & ex.Message)
        End Try
    End Function


End Class





'##############################################################################################
'##############################################################################################
'##############################################################################################
'##############################################################################################
'##############################################################################################