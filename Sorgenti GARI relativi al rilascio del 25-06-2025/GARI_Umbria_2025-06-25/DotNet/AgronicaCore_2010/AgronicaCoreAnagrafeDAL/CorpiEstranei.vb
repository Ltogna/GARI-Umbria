Imports AgronicaCoreDataProvider
Imports AgronicaCoreDataProvider.AgronicaCoreParametri
Imports AgronicaCoreDataProvider.CostantiPersonalizzate
Imports AgronicaCoreDataProvider.TipiEnumerativi

Public Class CorpiEstranei_W
    Inherits AgronicaCoreDataProvider.DataProvider


    'Public Function Scrivi_NONVABENE( _
    '                        ByVal Piva As String, _
    '                            ByVal Descrizione As String, _
    '                            ByVal LimiteMax_Aeroseparatori As Integer, _
    '                            ByVal LimiteMax_CernitriciOttiche As Integer, _
    '                            ByVal LimiteMax_CernitaManuale As Integer, _
    '                                ByRef objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri _
    '                                ) As Boolean

    '    Dim NomeRoutine As String = "AgronicaCoreAnagrafeDAL.CorpiEstranei_W.Scrivi()"

    '    '====================================================================================
    '    'Parametri opzionali :
    '    '   objConnessione = nothing    =>  viene creata (e distrutta) con StringaConnessione
    '    '   DirectoryLOG = ""           =>  viene usato il valore di default
    '    '   FileLOG = ""                =>  viene usato il valore di default
    '    '====================================================================================

    '    Dim MessaggioErrore As String = ""
    '    Dim StrSQL As New System.Text.StringBuilder
    '    Dim xRisp As Boolean = False
    '    Dim dt As DataTable
    '    Dim id As Integer

    '    Dim Cod_CorpoEstraneo As Integer

    '    Try

    '        '---------------------------------------------
    '        StrSQL.Length = 0

    '        StrSQL.Append(" SELECT max(Cod_CorpoEstraneo)as ID FROM CorpiEstranei ")

    '        dt = EseguiQuery_Lettura(objParametri, StrSQL.ToString, NomeRoutine)

    '        id = dt.Rows(0).Item("ID") + 1

    '        StrSQL.Length = 0
    '        StrSQL.Append("INSERT INTO CorpiEstranei  ")
    '        StrSQL.Append(" (Piva_SuperUser, Piva, Cod_CorpoEstraneo, Desc_CorpoEstraneo) ")
    '        StrSQL.Append(" VALUES ('" & Agro_SQL_SaveText(objParametri.PivaSuperUser) & "',")
    '        StrSQL.Append(" '" & Agro_SQL_SaveText(Piva) & "',")
    '        StrSQL.Append(" " & id & " , '" & Agro_SQL_SaveText(Descrizione) & "') ")

    '        '--------------------------------------------------------------------------
    '        xRisp = EseguiQuery_Scrittura(objParametri, StrSQL.ToString, NomeRoutine)
    '        '--------------------------------------------------------------------------

    '        StrSQL.Length = 0
    '        StrSQL.Append("INSERT INTO CorpiEstranei_LimitiPericolosita  ")
    '        StrSQL.Append(" (ID, Piva_SuperUser, Piva, Cod_CorpoEstraneo, Cod_Pericolosita, Desc_Pericolosita, LimiteMax_Aeroseparatori , LimiteMax_CernitriciOttiche , LimiteMax_CernitaManuale) ")
    '        StrSQL.Append(" VALUES ((SELECT max(ID) +1 FROM CorpiEstranei_LimitiPericolosita ) ,'" & Agro_SQL_SaveText(objParametri.PivaSuperUser) & "',")
    '        StrSQL.Append(" '" & Agro_SQL_SaveText(Piva) & "',")
    '        StrSQL.Append(" " & id & " , 1, 'Alta' , " & Agro_SQL_SaveNum(LimiteMax_Aeroseparatori) & " , ")
    '        StrSQL.Append(" " & Agro_SQL_SaveNum(LimiteMax_CernitriciOttiche) & " , " & Agro_SQL_SaveNum(LimiteMax_CernitaManuale) & ") ")

    '        '--------------------------------------------------------------------------
    '        xRisp = EseguiQuery_Scrittura(objParametri, StrSQL.ToString, NomeRoutine)
    '        '--------------------------------------------------------------------------

    '    Catch ex As Exception

    '        MessaggioErrore = ex.Message
    '        Scrivi_LOG(objParametri, NomeRoutine, MessaggioErrore)
    '        xRisp = False
    '        Throw New Exception("[" & NomeRoutine & "] : " & MessaggioErrore)

    '    End Try

    '    Return xRisp

    'End Function


    Public Function Scrivi(ByVal Piva As String,
                           ByVal Cod_CorpoEstraneo As Integer,
                           ByVal Desc_CorpoEstraneo As String,
                           ByVal Validita_Inizio As Date,
                           ByVal Validita_Fine As Date,
                           ByRef objParametri As AgronicaCoreParametri
                           ) As Boolean

        Const nomeRoutine = "AgronicaCoreAnagrafeDAL.CorpiEstranei_W.Scrivi()"

        Dim messaggioErrore As String = ""
        Dim StrSQL As New Text.StringBuilder
        Dim xRisp As Boolean = False

        Try

            StrSQL.Length = 0
            StrSQL.Append(" INSERT INTO CorpiEstranei ")
            StrSQL.Append("             (Piva_SuperUser,    Piva,   ")
            StrSQL.Append("               Cod_CorpoEstraneo, Desc_CorpoEstraneo, ")

            StrSQL.Append("              Inviato,            datainvio, ")
            StrSQL.Append("              Data_Creazione,     Data_Modifica, ")
            StrSQL.Append("              UserName_Creazione, UserName_Modifica, ")
            StrSQL.Append("              Validita_Inizio,    Validita_Fine ")

            StrSQL.Append("              ) ")

            StrSQL.Append(" VALUES (")
            StrSQL.Append("          '" & Agro_SQL_SaveText(objParametri.PivaSuperUser) & "'  ")
            StrSQL.Append("         , '" & Agro_SQL_SaveText(Piva) & "'  ")
            StrSQL.Append("         , " & Agro_SQL_SaveNum(Cod_CorpoEstraneo) & "  ")
            StrSQL.Append("         , '" & Agro_SQL_SaveText(Desc_CorpoEstraneo) & "'  ")

            StrSQL.Append("         , 0  ")
            StrSQL.Append("         , Null  ")
            StrSQL.Append("         , " & Agro_SQL_SaveDate(Date.Now) & "  ")
            StrSQL.Append("         , " & Agro_SQL_SaveDate(Date.Now) & "  ")
            StrSQL.Append("         ,'" & Agro_SQL_SaveText(objParametri.UsernameOperazione) & "' ")
            StrSQL.Append("         ,'" & Agro_SQL_SaveText(objParametri.UsernameOperazione) & "' ")
            StrSQL.Append("         , " & Agro_SQL_SaveDate(Validita_Inizio) & "  ")
            StrSQL.Append("         , " & Agro_SQL_SaveDate(Validita_Fine) & "  ")

            StrSQL.Append(") ")

            '--------------------------------------------------------------------------
            xRisp = EseguiQuery_Scrittura(objParametri, StrSQL.ToString, nomeRoutine)
            '--------------------------------------------------------------------------

        Catch ex As Exception
            messaggioErrore = ex.Message
            Scrivi_LOG(objParametri, nomeRoutine, messaggioErrore)
            xRisp = False
            Throw New Exception("[" & nomeRoutine & "] : " & messaggioErrore)
        End Try

        Return xRisp

    End Function


    Public Function Cancella(ByVal Piva As String,
                             ByVal Cod_CorpoEstraneo As Integer,
                             ByVal xFiltroAggiuntivo As String,
                             ByRef objParametri As AgronicaCoreParametri
                             ) As Boolean

        Const nomeRoutine = "AgronicaCoreAnagrafeDAL.CorpiEstranei_W.Cancella()"

        Dim messaggioErrore As String = ""
        Dim StrSQL As New Text.StringBuilder
        Dim xRisp As Boolean = False

        Try

            If Piva = "" Then
                Throw New Exception("Parametro non corretto nella query (Piva obbligatorio)")
            End If

            If Cod_CorpoEstraneo = 0 Then
                Throw New Exception("Parametro non corretto nella query (Cod_CorpoEstraneo obbligatorio)")
            End If

            '---------------------------------------------
            StrSQL.Length = 0

            If objParametri.FlagCancellazioneLogica = enumCancellazioneLogica.CancellazioneLogica Then

                StrSQL.Length = 0
                StrSQL.Append(" UPDATE CorpiEstranei ")
                StrSQL.Append(" SET ")
                StrSQL.Append("         Username_Modifica = '" & Agro_SQL_SaveText(objParametri.UsernameOperazione) & "' ")
                StrSQL.Append("         ,Data_Modifica= " & Agro_SQL_SaveDate(Date.Now) & " ")
                StrSQL.Append("         ,Inviato = -1 ")
                StrSQL.Append(" WHERE   Piva_SuperUser = '" & Agro_SQL_SaveText(objParametri.PivaSuperUser) & "' ")
                StrSQL.Append(" AND     Piva = '" & Agro_SQL_SaveText(Piva) & "' ")
                StrSQL.Append(" AND     Cod_CorpoEstraneo = " & Agro_SQL_SaveNum(Cod_CorpoEstraneo) & " ")
                StrSQL.Append(" AND     Inviato >= 0 ")

            Else

                StrSQL.Length = 0
                StrSQL.Append(" DELETE ")
                StrSQL.Append(" FROM    CorpiEstranei ")
                StrSQL.Append(" WHERE   Piva_SuperUser = '" & Agro_SQL_SaveText(objParametri.PivaSuperUser) & "' ")
                StrSQL.Append(" AND     Piva = '" & Agro_SQL_SaveText(Piva) & "' ")
                StrSQL.Append(" AND     Cod_CorpoEstraneo = " & Agro_SQL_SaveNum(Cod_CorpoEstraneo) & " ")

            End If
            '---------------------------------------------

            If xFiltroAggiuntivo <> "" Then
                StrSQL.Append(" AND " & Agro_SQL_Save_xFiltroAggiuntivo(xFiltroAggiuntivo, , objParametri))
            End If

            '--------------------------------------------------------------------------
            xRisp = EseguiQuery_Scrittura(objParametri, StrSQL.ToString, nomeRoutine)
            '--------------------------------------------------------------------------

        Catch ex As Exception
            messaggioErrore = ex.Message
            Scrivi_LOG(objParametri, nomeRoutine, messaggioErrore)
            xRisp = False
            Throw New Exception("[" & nomeRoutine & "] : " & messaggioErrore)
        End Try

        Return xRisp

    End Function

End Class

'##############################################################################################
'##############################################################################################
'##############################################################################################
'##############################################################################################
'##############################################################################################


Public Class CorpiEstranei_R
    Inherits AgronicaCoreDataProvider.DataProvider



    '##############################################################################################
    Public Function Leggi(ByVal Cod_CorpoEstraneo As Integer,
                          ByVal xSelezioneVariabile As enumSelezioneVariabile,
                          ByVal xFiltroAggiuntivo As String,
                          ByVal xOrderBy As String,
                          ByRef objParametri As AgronicaCoreParametri
                          ) As DataTable

        Const nomeRoutine = "AgronicaCoreAnagrafeDAL.CorpiEstranei_R.Leggi()"

        Dim messaggioErrore As String = ""
        Dim StrSQL As New Text.StringBuilder
        Dim dt As DataTable

        Try
            Select Case xSelezioneVariabile

                Case enumSelezioneVariabile.Selezione_TabellaDatiMinimi
                    StrSQL.Length = 0
                    StrSQL.Append(" SELECT Cod_CorpoEstraneo , Desc_CorpoEstraneo")
                    StrSQL.Append(" FROM  CorpiEstranei ")
                    StrSQL.Append(" WHERE Validita_inizio < " & Agro_SQL_SaveDate(objParametri.FinestraTemporaleFine) & " ")
                    StrSQL.Append(" AND   Validita_Fine > " & Agro_SQL_SaveDate(objParametri.FinestraTemporaleInizio) & " ")

                    If Cod_CorpoEstraneo <> 0 Then
                        StrSQL.Append(" AND Cod_CorpoEstraneo = " & Agro_SQL_SaveNum(Cod_CorpoEstraneo) & " ")
                    End If

                    '--------------------------------------------------------------------------
                    If xFiltroAggiuntivo <> "" Then
                        StrSQL.Append(" AND " & Agro_SQL_Save_xFiltroAggiuntivo(xFiltroAggiuntivo, , objParametri))
                    End If
                    '--------------------------------------------------------------------------
                    Select Case objParametri.FlagVisibilita
                        Case enumVisibilita.Visibilita_SoloNonCancellati
                            StrSQL.Append(" AND   Inviato >=0 ")
                        Case enumVisibilita.Visibilita_SoloCancellati
                            StrSQL.Append(" AND   Inviato =-1 ")
                        Case enumVisibilita.Visibilita_Tutti
                            '...................................
                        Case Else
                            Throw New Exception("Parametro non corretto nella query (FlagVisibilita)")
                    End Select
                    '--------------------------------------------------------------------------
                    If xOrderBy <> "" Then
                        StrSQL.Append(" ORDER BY " & Agro_SQL_Save_xOrderBy(xOrderBy, objParametri))
                    Else
                        StrSQL.Append(" ORDER BY Desc_CorpoEstraneo")
                    End If
                Case enumSelezioneVariabile.Selezione_TabellaCompleta

                    StrSQL.Length = 0
                    StrSQL.Append(" SELECT * ")
                    StrSQL.Append(" FROM  CorpiEstranei ")
                    StrSQL.Append(" WHERE Validita_inizio < " & Agro_SQL_SaveDate(objParametri.FinestraTemporaleFine) & " ")
                    StrSQL.Append(" AND   Validita_Fine > " & Agro_SQL_SaveDate(objParametri.FinestraTemporaleInizio) & " ")

                    If Cod_CorpoEstraneo <> 0 Then
                        StrSQL.Append(" AND Cod_CorpoEstraneo = " & Agro_SQL_SaveNum(Cod_CorpoEstraneo) & " ")
                    End If

                    '--------------------------------------------------------------------------
                    If xFiltroAggiuntivo <> "" Then
                        StrSQL.Append(" AND " & Agro_SQL_Save_xFiltroAggiuntivo(xFiltroAggiuntivo, , objParametri))
                    End If
                    '--------------------------------------------------------------------------
                    Select Case objParametri.FlagVisibilita
                        Case enumVisibilita.Visibilita_SoloNonCancellati
                            StrSQL.Append(" AND   Inviato >=0 ")
                        Case enumVisibilita.Visibilita_SoloCancellati
                            StrSQL.Append(" AND   Inviato =-1 ")
                        Case enumVisibilita.Visibilita_Tutti
                            '...................................
                        Case Else
                            Throw New Exception("Parametro non corretto nella query (FlagVisibilita)")
                    End Select
                    '--------------------------------------------------------------------------
                    If xOrderBy <> "" Then
                        StrSQL.Append(" ORDER BY " & Agro_SQL_Save_xOrderBy(xOrderBy, objParametri))
                    Else
                        StrSQL.Append(" ORDER BY Desc_CorpoEstraneo")
                    End If

                Case enumSelezioneVariabile.Selezione_JoinDescrizioni
                    
                    StrSQL.Length = 0
                    StrSQL.Append(" SELECT CorpiEstranei.Cod_CorpoEstraneo, CorpiEstranei.Desc_CorpoEstraneo, CorpiEstranei_LimitiPericolosita.LimiteMax_Aeroseparatori,  ")
                    StrSQL.Append(" CorpiEstranei_LimitiPericolosita.LimiteMax_CernitriciOttiche, CorpiEstranei_LimitiPericolosita.LimiteMax_CernitaManuale  ")
                    StrSQL.Append(" FROM         CorpiEstranei INNER JOIN ")
                    StrSQL.Append(" CorpiEstranei_LimitiPericolosita ON CorpiEstranei.Cod_CorpoEstraneo = CorpiEstranei_LimitiPericolosita.Cod_CorpoEstraneo ")
                    StrSQL.Append(" WHERE CorpiEstranei.Validita_inizio < " & Agro_SQL_SaveDate(objParametri.FinestraTemporaleFine) & " ")
                    StrSQL.Append(" AND   CorpiEstranei.Validita_Fine > " & Agro_SQL_SaveDate(objParametri.FinestraTemporaleInizio) & " ")

                    If Cod_CorpoEstraneo <> 0 Then
                        StrSQL.Append(" AND CorpiEstranei.Cod_CorpoEstraneo = " & Agro_SQL_SaveNum(Cod_CorpoEstraneo) & " ")
                    End If

                    '--------------------------------------------------------------------------
                    If xFiltroAggiuntivo <> "" Then
                        StrSQL.Append(" AND " & Agro_SQL_Save_xFiltroAggiuntivo(xFiltroAggiuntivo, , objParametri))
                    End If
                    '--------------------------------------------------------------------------
                    Select Case objParametri.FlagVisibilita
                        Case enumVisibilita.Visibilita_SoloNonCancellati
                            StrSQL.Append(" AND   CorpiEstranei.Inviato >=0 ")
                        Case enumVisibilita.Visibilita_SoloCancellati
                            StrSQL.Append(" AND   CorpiEstranei.Inviato =-1 ")
                        Case enumVisibilita.Visibilita_Tutti
                            '...................................
                        Case Else
                            Throw New Exception("Parametro non corretto nella query (FlagVisibilita)")
                    End Select
                    '--------------------------------------------------------------------------
                    If xOrderBy <> "" Then
                        StrSQL.Append(" ORDER BY " & Agro_SQL_Save_xOrderBy(xOrderBy, objParametri))
                    Else
                        StrSQL.Append(" ORDER BY CorpiEstranei.Desc_CorpoEstraneo")
                    End If

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

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' controlla se esiste già un corpo estraneo passatagli la descrizione 
    ''' </summary>
    ''' -----------------------------------------------------------------------------
    Public Function EsisteCE(ByVal Descrizione As String,
                             ByVal xSelezioneVariabile As enumSelezioneVariabile,
                             ByRef objParametri As AgronicaCoreParametri
                             ) As DataTable

        Const nomeRoutine = "AgronicaCoreAnagrafeDAL.CorpiEstranei_R.EsisteCE()"

        Dim messaggioErrore As String = ""
        Dim StrSQL As New Text.StringBuilder
        Dim dt As DataTable

        Try
            Select Case xSelezioneVariabile

                Case enumSelezioneVariabile.Selezione_TabellaDatiMinimi
                    StrSQL.Length = 0
                    StrSQL.Append(" SELECT Cod_CorpoEstraneo , Desc_CorpoEstraneo")
                    StrSQL.Append(" FROM  CorpiEstranei ")
                    StrSQL.Append(" WHERE Validita_inizio < " & Agro_SQL_SaveDate(objParametri.FinestraTemporaleFine) & " ")
                    StrSQL.Append(" AND   Validita_Fine > " & Agro_SQL_SaveDate(objParametri.FinestraTemporaleInizio) & " ")

                    If Descrizione <> "" Then
                        StrSQL.Append(" AND LOWER(Desc_CorpoEstraneo) = LOWER('" & Agro_SQL_SaveText(Descrizione) & "') ")
                    End If

                    '--------------------------------------------------------------------------
                    Select Case objParametri.FlagVisibilita
                        Case enumVisibilita.Visibilita_SoloNonCancellati
                            StrSQL.Append(" AND   Inviato >=0 ")
                        Case enumVisibilita.Visibilita_SoloCancellati
                            StrSQL.Append(" AND   Inviato =-1 ")
                        Case enumVisibilita.Visibilita_Tutti
                            '...................................
                        Case Else
                            Throw New Exception("Parametro non corretto nella query (FlagVisibilita)")
                    End Select

                    StrSQL.Append(" ORDER BY Desc_CorpoEstraneo")

                Case enumSelezioneVariabile.Selezione_TabellaCompleta

                    StrSQL.Length = 0
                    StrSQL.Append(" SELECT * ")
                    StrSQL.Append(" FROM  CorpiEstranei ")
                    StrSQL.Append(" WHERE Validita_inizio < " & Agro_SQL_SaveDate(objParametri.FinestraTemporaleFine) & " ")
                    StrSQL.Append(" AND   Validita_Fine > " & Agro_SQL_SaveDate(objParametri.FinestraTemporaleInizio) & " ")

                    If Descrizione <> "" Then
                        StrSQL.Append(" AND LOWER(Desc_CorpoEstraneo) = LOWER('" & Agro_SQL_SaveText(Descrizione) & "') ")
                    End If

                    '--------------------------------------------------------------------------
                    Select Case objParametri.FlagVisibilita
                        Case enumVisibilita.Visibilita_SoloNonCancellati
                            StrSQL.Append(" AND   Inviato >=0 ")
                        Case enumVisibilita.Visibilita_SoloCancellati
                            StrSQL.Append(" AND   Inviato =-1 ")
                        Case enumVisibilita.Visibilita_Tutti
                            '...................................
                        Case Else
                            Throw New Exception("Parametro non corretto nella query (FlagVisibilita)")
                    End Select

                    StrSQL.Append(" ORDER BY Desc_CorpoEstraneo")

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

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' controlla se esiste già un corpo estraneo passatagli la descrizione 
    ''' </summary>
    ''' -----------------------------------------------------------------------------
    Public Function LeggiDescrizioneCE(ByVal Cod_CorpoEstraneo As Integer,
                                       ByRef objParametri As AgronicaCoreParametri
                                       ) As String

        Dim nomeRoutine As String = "AgronicaCoreAnagrafeDAL.CorpiEstranei_R.LeggiDescrizioneCE()"

        Dim dt As DataTable
        dt = Leggi(Cod_CorpoEstraneo,
                   enumSelezioneVariabile.Selezione_TabellaCompleta,
                   "", "", objParametri)

        If dt.Rows.Count > 0 Then
            Return dt.Rows(0).Item("Desc_CorpoEstraneo")
        End If

        Return ""

    End Function

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' controlla se esiste già un corpo estraneo passatagli la descrizione 
    ''' </summary>
    ''' -----------------------------------------------------------------------------
    Public Function LeggiRegistrazioneBolla(ByVal Piva As String,
                                            ByVal Numero_Bolla_Sin As String,
                                            ByVal Numero_Bolla As Integer,
                                            ByVal Numero_Bolla_Des As String,
                                            ByVal Anno_Bolla As Integer,
                                            ByVal xSelezioneVariabile As enumSelezioneVariabile,
                                            ByVal xFiltroAggiuntivo As String,
                                            ByVal xOrderBy As String,
                                            ByRef objParametri As AgronicaCoreParametri
                                            ) As DataTable

        Const nomeRoutine = "AgronicaCoreAnagrafeDAL.CorpiEstranei_R.EsisteRegistrazioneBolla()"

        Dim messaggioErrore As String = ""
        Dim StrSQL As New Text.StringBuilder
        Dim dt As DataTable

        Try
            Select Case xSelezioneVariabile

                Case enumSelezioneVariabile.Selezione_TabellaDatiMinimi

                Case enumSelezioneVariabile.Selezione_TabellaCompleta


                Case enumSelezioneVariabile.Selezione_JoinDescrizioni
                    StrSQL.Length = 0
                    StrSQL.Append("SELECT     Agenda.PIVA, Agenda.Sa_Cod, Agenda.Id_Agenda, Agenda.Lav_Cod, Movimenti.Data_Movimento, Movimenti.Mov_Desc, Movimenti.Doc_Numero, ")
                    StrSQL.Append(" Movimenti.Doc_Numero_Des, Movimenti.Doc_Numero_Sin, Movimenti.Extra_Int, Movimenti.Colli, Movimenti.Extra_Date, Movimenti.Ora,  Movimenti.Extra_Str, ")
                    StrSQL.Append(" Movimenti.Natura_Beni, Movimenti_dettagli.Pro_Cod, Movimenti_dettagli.Qta, Movimenti_dettagli.Mat_Cod")
                    StrSQL.Append(" FROM Agenda INNER JOIN  Movimenti ON Agenda.PIVA = Movimenti.PIVA AND Agenda.Sa_Cod = Movimenti.Sa_Cod AND Agenda.Id_Agenda = Movimenti.Id_Agenda INNER JOIN  Movimenti_dettagli ON Agenda.PIVA = Movimenti_dettagli.PIVA ")
                    StrSQL.Append(" AND Agenda.Sa_Cod = Movimenti_dettagli.Sa_Cod AND  Agenda.Id_Agenda = Movimenti_dettagli.Id_Agenda And Movimenti.Id_Mov = Movimenti_dettagli.Id_Mov  ")

                    StrSQL.Append(" WHERE     (Agenda.Lav_Cod = 5003) ")

                    If Anno_Bolla <> 0 Then
                        StrSQL.Append(" AND Movimenti.Extra_Int = " & Agro_SQL_SaveNum(Anno_Bolla) & " ")
                    End If

                    If Piva <> "" Then
                        StrSQL.Append(" AND  Agenda.Piva = '" & Agro_SQL_SaveText(Piva) & "' ")
                    End If

                    If Numero_Bolla_Sin <> "" Then
                        StrSQL.Append(" AND  Doc_Numero_Sin = '" & Agro_SQL_SaveText(Numero_Bolla_Sin) & "' ")
                    End If

                    If Numero_Bolla <> 0 Then
                        StrSQL.Append(" AND  Doc_Numero = " & Agro_SQL_SaveNum(Numero_Bolla) & " ")
                    End If

                    If Numero_Bolla_Des <> "" Then
                        StrSQL.Append(" AND  Doc_Numero_Des = '" & Agro_SQL_SaveText(Numero_Bolla_Des) & "' ")
                    End If

                    '--------------------------------------------------------------------------
                    If xFiltroAggiuntivo <> "" Then
                        StrSQL.Append(" AND " & Agro_SQL_Save_xFiltroAggiuntivo(xFiltroAggiuntivo, , objParametri))
                    End If
                    '--------------------------------------------------------------------------
                    If xOrderBy <> "" Then
                        StrSQL.Append(" ORDER BY " & Agro_SQL_Save_xOrderBy(xOrderBy, objParametri))
                    End If

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

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' controlla se esiste già un corpo estraneo passatagli la descrizione 
    ''' </summary>
    ''' -----------------------------------------------------------------------------
    Public Function EsisteRegistrazioneBolla(ByVal Piva As String,
                                             ByVal Numero_Bolla_Sin As String,
                                             ByVal Numero_Bolla As Integer,
                                             ByVal Numero_Bolla_Des As String,
                                             ByVal Anno_Bolla As Integer,
                                             ByRef objParametri As AgronicaCoreParametri
                                             ) As Boolean

        Dim nomeRoutine As String = "AgronicaCoreAnagrafeDAL.CorpiEstranei_R.EsisteRegistrazioneBolla()"

        Dim dt As DataTable
        dt = LeggiRegistrazioneBolla(Piva, Numero_Bolla_Sin, Numero_Bolla, Numero_Bolla_Des, Anno_Bolla,
                                     enumSelezioneVariabile.Selezione_JoinDescrizioni,
                                     "", "", objParametri)
        If dt.Rows.Count > 0 Then
            Return True
        End If

        Return False

    End Function


    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Legge la tabella dei limiti
    ''' </summary>
    ''' -----------------------------------------------------------------------------
    Public Function LeggiLimitiPericolosita(ByVal ID As Integer,
                                            ByVal Piva_SuperUser As String,
                                            ByVal xSelezioneVariabile As enumSelezioneVariabile,
                                            ByVal xFiltroAggiuntivo As String,
                                            ByVal xOrderBy As String,
                                            ByRef objParametri As AgronicaCoreParametri
                                            ) As DataTable

        Const nomeRoutine = "AgronicaCoreAnagrafeDAL.CorpiEstranei_R.LeggiLimitiPericolosita()"

        Dim messaggioErrore As String = ""
        Dim StrSQL As New Text.StringBuilder
        Dim dt As DataTable

        Try
            Select Case xSelezioneVariabile

                Case enumSelezioneVariabile.Selezione_TabellaDatiMinimi

                Case enumSelezioneVariabile.Selezione_TabellaCompleta

                    StrSQL.Length = 0
                    StrSQL.Append(" SELECT * ")
                    StrSQL.Append(" FROM CorpiEstranei_LimitiPericolosita ")
                    StrSQL.Append(" WHERE Validita_inizio < " & Agro_SQL_SaveDate(objParametri.FinestraTemporaleFine) & " ")
                    StrSQL.Append(" AND   Validita_Fine > " & Agro_SQL_SaveDate(objParametri.FinestraTemporaleInizio) & " ")

                    If ID <> 0 Then
                        StrSQL.Append(" AND ID = " & Agro_SQL_SaveNum(ID) & " ")
                    End If

                    If Piva_SuperUser <> "" Then
                        StrSQL.Append(" AND Piva_SuperUser = " & Agro_SQL_SaveText(Piva_SuperUser) & " ")
                    End If

                    '--------------------------------------------------------------------------
                    If xFiltroAggiuntivo <> "" Then
                        StrSQL.Append(" AND " & Agro_SQL_Save_xFiltroAggiuntivo(xFiltroAggiuntivo, , objParametri))
                    End If
                    '--------------------------------------------------------------------------
                    If xOrderBy <> "" Then
                        StrSQL.Append(" ORDER BY " & Agro_SQL_Save_xOrderBy(xOrderBy, objParametri))
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

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' controlla il superamento del limite
    ''' ritorna TRUE se il limite è superato
    ''' </summary>
    ''' -----------------------------------------------------------------------------
    Public Function SuperatoLimitePericolosita(ByVal Cod_corpoEstraneo As Integer,
                                               ByVal Aereoseparatore As Integer,
                                               ByVal CernitriceOttica As Integer,
                                               ByVal CernitriceManuale As Integer,
                                               ByRef objParametri As AgronicaCoreParametri
                                               ) As Boolean

        Dim nomeRoutine As String = "AgronicaCoreAnagrafeDAL.CorpiEstranei_R.SuperatoLimitePericolosita()"

        Dim dt As DataTable
        dt = LeggiLimitiPericolosita(0, "",
                                     enumSelezioneVariabile.Selezione_TabellaCompleta,
                                     " Cod_CorpoEstraneo = " & Cod_corpoEstraneo,
                                     "", objParametri)

        If dt.Rows.Count > 0 Then
            ''controllo i vari limiti 
            If dt.Rows(0).Item("LimiteMax_Aeroseparatori") < Aereoseparatore Then
                Return True
            End If

            If dt.Rows(0).Item("LimiteMax_CernitriciOttiche") < CernitriceOttica Then
                Return True
            End If

            If dt.Rows(0).Item("LimiteMax_CernitaManuale") < CernitriceManuale Then
                Return True
            End If
        End If

        Return False

    End Function

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Legge la registrazione dei corpi estranei e le bolle relative
    ''' </summary>
    ''' -----------------------------------------------------------------------------
    Public Function LeggiCorpiEstraneiBolle(ByVal Flag_Distinct As Boolean,
                                            ByVal Piva_Produttore As String,
                                            ByVal dal_Data_Arrivo As Date,
                                            ByVal al_Data_Arrivo As Date,
                                            ByVal Codice_Specie As Integer,
                                            ByVal Codice_Prodotto As Integer,
                                            ByVal Flag_Join_Appezzamento As Boolean,
                                            ByVal Sa_Cod_Appezzamento As Integer,
                                            ByVal Appezza_Appezzamento As Integer,
                                            ByVal Id_Reg_Appezzamento As Integer,
                                            ByVal Tipologia_Ritrovamento As Integer,
                                            ByVal Indice_Pericolosita As Integer,
                                            ByVal Sa_Cod As Integer,
                                            ByVal Fabbricato_Cod As Integer,
                                            ByVal xFiltroAggiuntivo As String,
                                            ByVal xOrderBy As String,
                                            ByRef objParametri As AgronicaCoreParametri
                                            ) As DataTable

        Const nomeRoutine = "AgronicaCoreAnagrafeDAL.CorpiEstranei_R.LeggiCorpiEstraneiBolle()"

        Dim messaggioErrore As String = ""
        Dim StrSQL As New Text.StringBuilder
        Dim Stb_Select As New Text.StringBuilder
        Dim Stb_SelectRif As New Text.StringBuilder
        Dim Stb_Join As New Text.StringBuilder
        Dim Stb_JoinRif As New Text.StringBuilder
        Dim Stb_Where As New Text.StringBuilder
        Dim Stb_WhereRif As New Text.StringBuilder

        Dim dt As DataTable

        Try
            StrSQL.Length = 0

            'la union deve avere le stesse colonne della prima select e nello stesso ordine
            'quindi devo mettere la condizione sul not distinct
            If Not Flag_Distinct AndAlso Flag_Join_Appezzamento Then
                StrSQL.Append(" ( ")
            End If

            ' CONVERT(varchar(50), CONVERT(int, Movimenti_CE.Doc_Numero)) 

            If Flag_Distinct Then
                Stb_Select.Append("SELECT  DISTINCT Agenda_CE.PIVA as Piva, Agenda_CE.Sa_Cod , Agenda_CE.Id_Agenda, Movimenti_CE.Id_Mov, Movimenti_CE.Extra_Int, CONVERT(VARCHAR(11),Movimenti_CE.Data_Movimento,106) as Data_Movimento ,  " & vbCrLf)
                Stb_Select.Append(" CONVERT(VARCHAR(11), Movimenti_Bolla_Accettazione.Data_Movimento, 106) AS Data_Bolla, " & vbCrLf)
                Stb_Select.Append(" (Movimenti_CE.Doc_Numero_Sin +     RIGHT(   ('00000'+  CONVERT(varchar(50), CONVERT(int, Movimenti_CE.Doc_Numero))     ), 5   ) +  Movimenti_CE.Doc_Numero_Des    ) As Numero_Bolla, Movimenti_Bolla_Accettazione.Cod_RisUm," & vbCrLf)
                Stb_Select.Append(" Movimenti_Bolla_Accettazione.Cod_RisUm," & vbCrLf)
                Stb_Select.Append(" Movimenti_CE.Natura_Beni, Movimenti_CE.Extra_Str, CONVERT(VARCHAR(11),Movimenti_CE.Extra_Date,106) as Extra_Date, CONVERT(VARCHAR(5),Movimenti_CE.Ora,108) as Ora , Movimenti_CE.Colli, Movimenti_CE.Mov_Desc,  " & vbCrLf)
                Stb_Select.Append(" Materie_Prime.Veg_Cod, Materie_Prime.Mat_Des, Contatti_Conferenti.Rag_Soc AS Rag_Soc_Conferente, ISNULL(Contatti_Produttori.Rag_Soc, '') AS Rag_Soc_Produttore, Movimenti_Dettagli_Raccolta.QTA , CONVERT(VARCHAR(11),Movimenti_CE.Data_Movimento,106) as 'Data di Arrivo', " & vbCrLf)
                Stb_Select.Append(" datepart(year, Movimenti_CE.Data_Movimento) AS Anno_Carico, datepart(month,Movimenti_CE.Data_Movimento) AS Mese_Carico, datepart(DAY,Movimenti_CE.Data_Movimento)  AS Giorno_Carico, Fabbr_Raccolta.Fabbricato_Des AS Stabilimento " & vbCrLf)
            Else
                Stb_Select.Append(" SELECT  Movimenti_CE.Extra_Int as Anno, " & vbCrLf)
                Stb_Select.Append(" (Movimenti_CE.Doc_Numero_Sin +     RIGHT(   ('00000'+  CONVERT(varchar(50), CONVERT(int, Movimenti_CE.Doc_Numero))     ), 5   ) +  Movimenti_CE.Doc_Numero_Des    ) As 'Numero Bolla',      " & vbCrLf)
                Stb_Select.Append(" CONVERT(VARCHAR(11),Movimenti_CE.Data_Movimento,106) as 'Data di Arrivo', Movimenti_CE.Colli as 'Carico Numero',     " & vbCrLf)
                Stb_Select.Append(" Movimenti_Dettagli_Raccolta.QTA as 'Peso Netto', Movimenti_Dettagli_Raccolta.Variazione as 'Degrado', Movimenti_Dettagli_Raccolta.udm_cod, 0 AS 'Netto Pagamento',   " & vbCrLf)
                Stb_Select.Append(" Materie_Prime.Mat_Des as 'Specie - Varieta',    " & vbCrLf)
                Stb_Select.Append(" ISNULL( (SELECT TOP 1 VAL_COD      " & vbCrLf)
                Stb_Select.Append(" FROM Materie_Prime_Campionature MP_Camp_Indice    " & vbCrLf)
                Stb_Select.Append(" WHERE MP_Camp_Indice.progressivo = Movimenti_Dettagli_Raccolta.cal_cod   " & vbCrLf)
                Stb_Select.Append(" AND MP_Camp_Indice.tipo = 'indice' AND MP_Camp_Indice.tipo_cod IN ('999', '12','1')  ), 0 ) AS Punteggio,    " & vbCrLf)
                Stb_Select.Append(" ISNULL( (SELECT TOP 1 ISNULL( materie_prime_calibri.cal_des , ' ') AS Calibro    " & vbCrLf)
                Stb_Select.Append(" FROM materie_prime_calibri   " & vbCrLf)
                Stb_Select.Append(" INNER JOIN Materie_Prime_Campionature MP_Camp_Calibro ON MP_Camp_Calibro.tipo_cod =  materie_prime_calibri.cal_cod    " & vbCrLf)
                Stb_Select.Append(" WHERE MP_Camp_Calibro.progressivo = Movimenti_Dettagli_Raccolta.cal_cod    " & vbCrLf)
                Stb_Select.Append(" AND MP_Camp_Calibro.tipo = 'calibro'), 0 ) AS Calibro ,   " & vbCrLf)
                Stb_Select.Append(" Contatti_Conferenti.Rag_Soc AS 'Ragione Sociale Conferente', ")
                Stb_Select.Append(" ISNULL(Contatti_Produttori.Rag_Soc, '') AS 'Ragione Sociale Produttore', " & vbCrLf)
                Stb_Select.Append(" -- Appezzamento.App_Nome as Appezzamento , Reg_Impianti.Sup_Imp as 'Superficie Impianto',   " & vbCrLf)
                Stb_Select.Append(" -- CONVERT(VARCHAR(11),Reg_Impianti.Validita_Inizio ,106)as 'Data Inizio Impianto',  " & vbCrLf)
                Stb_Select.Append(" CONVERT(VARCHAR(11),Movimenti_CE.Extra_Date,106) + ' - '+ CONVERT(VARCHAR(5),Movimenti_CE.Ora,108) as 'Data-Ora Inizio Cottura',  " & vbCrLf)
                Stb_Select.Append(" Movimenti_CE.Natura_Beni as 'Confezione / Marchio 1', " & vbCrLf)
                Stb_Select.Append(" Movimenti_CE.Aspetto as 'Confezione / Marchio 2',  Movimenti_CE.Extra_Str  as 'Livello Qualitativo', CONVERT(VARCHAR(50), Movimenti_CE.Tipo_Sconto) as 'Pesticidi',  " & vbCrLf)
                Stb_Select.Append(" Movimenti_Dettagli_CE.Fase_Cod as Fase_Cod, " & vbCrLf)
                Stb_Select.Append(" --'case per il separatore   " & vbCrLf)
                Stb_Select.Append(" CASE Movimenti_Dettagli_CE.Mat_Cod  " & vbCrLf)
                Stb_Select.Append(" WHEN '1' THEN 'Aereoseparatore'  " & vbCrLf)
                Stb_Select.Append(" WHEN '2' THEN 'Cernitrice Ottica'  " & vbCrLf)
                Stb_Select.Append(" WHEN '3' THEN 'Cernita Manuale'  " & vbCrLf)
                Stb_Select.Append(" ELSE 'Errore Conversione'  " & vbCrLf)
                Stb_Select.Append(" END as 'Tipologia Ritrovamento',  " & vbCrLf)
                Stb_Select.Append(" (Select CorpiEstranei.Desc_CorpoEstraneo from CorpiEstranei where  CorpiEstranei.Cod_CorpoEstraneo=Movimenti_Dettagli_CE.Pro_Cod) as 'Corpo Estraneo',  " & vbCrLf)
                Stb_Select.Append(" Movimenti_Dettagli_CE.QTA as 'Quantita Rilevata' ,  " & vbCrLf)
                Stb_Select.Append(" --'case per il Pericolosita   " & vbCrLf)
                Stb_Select.Append(" Case Movimenti_Dettagli_CE.Cod_Progetto  " & vbCrLf)
                Stb_Select.Append(" WHEN '0' THEN 'Medio/Bassa'  " & vbCrLf)
                Stb_Select.Append(" WHEN '1' THEN 'Alta'  " & vbCrLf)
                Stb_Select.Append(" ELSE 'Errore Conversione'  " & vbCrLf)
                Stb_Select.Append(" END as  'Pericolosita',  " & vbCrLf)
                Stb_Select.Append(" '' as  'Linea 1 - Aereoseparatore' ,'' as  'Linea 1 - Cernitrice Ottica' ,'' as  'Linea 1 - Cernita Manuale' , ")
                Stb_Select.Append(" '' as  'Linea 2 - Aereoseparatore' ,'' as  'Linea 2 - Cernitrice Ottica' ,'' as  'Linea 2 - Cernita Manuale' , ")
                Stb_Select.Append(" Movimenti_CE.Mov_Desc as 'Note',  " & vbCrLf)
                Stb_Select.Append(" Fabbr_Raccolta.Fabbricato_Des AS Stabilimento " & vbCrLf)

            End If

            If Flag_Join_Appezzamento Then
                Stb_SelectRif.Append(" , APPEZZAMENTO.APP_NOME AS 'Nome Appezzamento', Reg_Impianti.Validita_Inizio AS 'Data Inizio Impianto', Reg_Impianti.SUP_IMP AS 'Superficie Impianto' " & vbCrLf)
                'StrSQL.Append("  -- Imprese_Progetti.Progetto_Nome, " & vbCrLf)
            End If

            StrSQL.Append(Stb_Select)
            StrSQL.Append(Stb_SelectRif)

            Stb_Join.Append("FROM     Agenda AS Agenda_CE  " & vbCrLf)

            Stb_Join.Append("INNER JOIN Movimenti AS Movimenti_CE ON Movimenti_CE.Id_Agenda = Agenda_CE.Id_Agenda AND Movimenti_CE.PIVA = Agenda_CE.PIVA AND " & vbCrLf)
            Stb_Join.Append("Movimenti_CE.Sa_Cod = Agenda_CE.Sa_Cod  " & vbCrLf)

            Stb_Join.Append("INNER JOIN Movimenti_dettagli AS Movimenti_Dettagli_CE ON Movimenti_CE.Id_Mov = Movimenti_Dettagli_CE.Id_Mov AND  " & vbCrLf)
            Stb_Join.Append("Movimenti_CE.PIVA = Movimenti_Dettagli_CE.PIVA AND Movimenti_CE.Sa_Cod = Movimenti_Dettagli_CE.Sa_Cod AND  " & vbCrLf)
            Stb_Join.Append("Movimenti_CE.Id_Agenda = Movimenti_Dettagli_CE.Id_Agenda  " & vbCrLf)

            Stb_Join.Append("INNER JOIN Movimenti AS Movimenti_Bolla_Accettazione ON Movimenti_CE.Doc_Numero = Movimenti_Bolla_Accettazione.Doc_Numero AND  " & vbCrLf)
            Stb_Join.Append("Movimenti_CE.Doc_Numero_Des = Movimenti_Bolla_Accettazione.Doc_Numero_Des AND  " & vbCrLf)
            Stb_Join.Append("Movimenti_CE.Doc_Numero_Sin = Movimenti_Bolla_Accettazione.Doc_Numero_Sin AND  " & vbCrLf)
            Stb_Join.Append("Movimenti_CE.Extra_Int = YEAR(Movimenti_Bolla_Accettazione.Data_Movimento)  " & vbCrLf)

            Stb_Join.Append(" INNER JOIN Risorse_Umane Risorse_Umane_Conferenti ON Movimenti_Bolla_Accettazione.Cod_RisUm = Risorse_Umane_Conferenti.Cod_RisUm  " & vbCrLf)
            Stb_Join.Append(" INNER JOIN Contatti Contatti_Conferenti ON Risorse_Umane_Conferenti.Piva = Contatti_Conferenti.Piva AND Risorse_Umane_Conferenti.Cod_Contatto = Contatti_Conferenti.Cod_Contatto  " & vbCrLf)
            Stb_Join.Append(" LEFT OUTER JOIN Risorse_Umane Risorse_Umane_Produttori ON Movimenti_Bolla_Accettazione.Cod_Destinazione = Risorse_Umane_Produttori.Cod_RisUm  " & vbCrLf)
            Stb_Join.Append(" LEFT OUTER JOIN Contatti Contatti_Produttori ON Risorse_Umane_Produttori.Piva = Contatti_Produttori.Piva AND Risorse_Umane_Produttori.Cod_Contatto = Contatti_Produttori.Cod_Contatto  " & vbCrLf)

            Stb_Join.Append(" INNER JOIN Agenda AS Agenda_Bolla ON Movimenti_Bolla_Accettazione.PIVA = Agenda_Bolla.PIVA AND  " & vbCrLf)
            Stb_Join.Append(" Movimenti_Bolla_Accettazione.Sa_Cod = Agenda_Bolla.Sa_Cod AND Movimenti_Bolla_Accettazione.Id_Agenda = Agenda_Bolla.Id_Agenda  " & vbCrLf)

            Stb_Join.Append(" INNER JOIN Movimenti AS Movimenti_Bolla_Raccolta ON Agenda_Bolla.PIVA = Movimenti_Bolla_Raccolta.PIVA AND " & vbCrLf)
            Stb_Join.Append(" Agenda_Bolla.Sa_Cod = Movimenti_Bolla_Raccolta.Sa_Cod AND Agenda_Bolla.Id_Agenda = Movimenti_Bolla_Raccolta.Id_Agenda " & vbCrLf)

            Stb_Join.Append(" INNER JOIN Movimenti_dettagli AS Movimenti_Dettagli_Raccolta ON Movimenti_Bolla_Raccolta.PIVA = Movimenti_Dettagli_Raccolta.PIVA AND " & vbCrLf)
            Stb_Join.Append(" Movimenti_Bolla_Raccolta.Id_Mov = Movimenti_Dettagli_Raccolta.Id_Mov AND " & vbCrLf)
            Stb_Join.Append(" Movimenti_Bolla_Raccolta.Id_Agenda = Movimenti_Dettagli_Raccolta.Id_Agenda " & vbCrLf)

            Stb_Join.Append(" INNER JOIN Materie_Prime ON Movimenti_Dettagli_Raccolta.Elem_Cod = Materie_Prime.Elem_Cod AND " & vbCrLf)
            Stb_Join.Append(" Movimenti_Dettagli_Raccolta.Mat_Cod = Materie_Prime.Mat_Cod" & vbCrLf)

            Stb_Join.Append(" INNER JOIN Mov_Destinazioni Mov_Dest_Raccolta ON Movimenti_Dettagli_Raccolta.PIVA = Mov_Dest_Raccolta.Piva " & vbCrLf)
            Stb_Join.Append(" AND Movimenti_Dettagli_Raccolta.Sa_Cod = Mov_Dest_Raccolta.Sa_Cod AND  Movimenti_Dettagli_Raccolta.Id_Agenda = Mov_Dest_Raccolta.Id_Agenda " & vbCrLf)
            Stb_Join.Append(" AND Movimenti_Dettagli_Raccolta.Id_Mov = Mov_Dest_Raccolta.Id_Mov AND Movimenti_Dettagli_Raccolta.Id_Mov_Det = Mov_Dest_Raccolta.Id_Mov_Det   " & vbCrLf)

            Stb_Join.Append(" INNER JOIN Fabbricati Fabbr_Raccolta ON Mov_Dest_Raccolta.Piva = Fabbr_Raccolta.PIVA AND Mov_Dest_Raccolta.Sa_Cod = Fabbr_Raccolta.SA_COD AND Mov_Dest_Raccolta.Id_Destinazione = Fabbr_Raccolta.Fabbricato_Cod " & vbCrLf)

            Stb_Join.Append(" INNER JOIN specieVegetali ON specieVegetali.Veg_Cod = Materie_Prime.Veg_Cod " & vbCrLf)

            If Flag_Join_Appezzamento Then
                Stb_JoinRif.Append(" INNER JOIN  Mov_Dettagli_Riferimenti " & vbCrLf)
                Stb_JoinRif.Append(" ON Mov_Dettagli_Riferimenti.Piva = Agenda_Bolla.PIVA   " & vbCrLf)
                Stb_JoinRif.Append(" AND Mov_Dettagli_Riferimenti.Sa_Cod = Agenda_Bolla.Sa_Cod  " & vbCrLf)
                Stb_JoinRif.Append(" AND Mov_Dettagli_Riferimenti.Id_Agenda = Agenda_Bolla.Id_Agenda   " & vbCrLf)
                'la raccolta sta in piva_rif sa_cod_rif lav_cod_rif
                Stb_JoinRif.Append(" INNER JOIN  Agenda Agenda_RaccoltaImpianti ON Mov_Dettagli_Riferimenti.Piva_Rif = Agenda_RaccoltaImpianti.PIVA  " & vbCrLf)
                Stb_JoinRif.Append("             AND Mov_Dettagli_Riferimenti.Sa_Cod_Rif = Agenda_RaccoltaImpianti.Sa_Cod  " & vbCrLf)
                Stb_JoinRif.Append("             AND Mov_Dettagli_Riferimenti.Id_Agenda_Rif = Agenda_RaccoltaImpianti.Id_Agenda  " & vbCrLf)
                Stb_JoinRif.Append(" INNER JOIN  Movimenti Movimenti_RaccoltaImpianti ON Agenda_RaccoltaImpianti.PIVA = Movimenti_RaccoltaImpianti.PIVA  " & vbCrLf)
                Stb_JoinRif.Append("             AND Agenda_RaccoltaImpianti.Id_Agenda = Movimenti_RaccoltaImpianti.Id_Agenda " & vbCrLf)
                Stb_JoinRif.Append(" INNER JOIN  Movimenti_dettagli Movimenti_dettagli_RaccoltaImpianti ON Movimenti_RaccoltaImpianti.PIVA = Movimenti_dettagli_RaccoltaImpianti.PIVA  " & vbCrLf)
                Stb_JoinRif.Append("             AND Movimenti_RaccoltaImpianti.Id_Agenda = Movimenti_dettagli_RaccoltaImpianti.Id_Agenda  " & vbCrLf)
                Stb_JoinRif.Append("             AND Movimenti_RaccoltaImpianti.Id_Mov = Movimenti_dettagli_RaccoltaImpianti.Id_Mov  " & vbCrLf)
                Stb_JoinRif.Append(" INNER JOIN  Mov_Destinazioni Mov_Destinazioni_RaccoltaImpianti ON Movimenti_dettagli_RaccoltaImpianti.PIVA = Mov_Destinazioni_RaccoltaImpianti.Piva  " & vbCrLf)
                Stb_JoinRif.Append("             AND Movimenti_dettagli_RaccoltaImpianti.Sa_Cod = Mov_Destinazioni_RaccoltaImpianti.Sa_Cod  " & vbCrLf)
                Stb_JoinRif.Append("             AND Movimenti_dettagli_RaccoltaImpianti.Id_Agenda = Mov_Destinazioni_RaccoltaImpianti.Id_Agenda  " & vbCrLf)
                Stb_JoinRif.Append("             AND Movimenti_dettagli_RaccoltaImpianti.Id_Mov = Mov_Destinazioni_RaccoltaImpianti.Id_Mov  " & vbCrLf)
                Stb_JoinRif.Append("             AND Movimenti_dettagli_RaccoltaImpianti.Id_Mov_Det = Mov_Destinazioni_RaccoltaImpianti.Id_Mov_Det  " & vbCrLf)
                Stb_JoinRif.Append(" INNER JOIN Reg_Impianti ON Reg_Impianti.PIVA = Mov_Destinazioni_RaccoltaImpianti.Piva  " & vbCrLf)
                Stb_JoinRif.Append("             AND Reg_Impianti.Sa_Cod = Mov_Destinazioni_RaccoltaImpianti.Sa_Cod  " & vbCrLf)
                Stb_JoinRif.Append("             AND Reg_Impianti.Appezza = Mov_Destinazioni_RaccoltaImpianti.Appezza  " & vbCrLf)
                Stb_JoinRif.Append("             AND Reg_Impianti.Id_Reg = Mov_Destinazioni_RaccoltaImpianti.Id_Destinazione " & vbCrLf)
                Stb_JoinRif.Append(" INNER JOIN APPEZZAMENTO ON Reg_Impianti.PIVA = APPEZZAMENTO.Piva  " & vbCrLf)
                Stb_JoinRif.Append("             AND Reg_Impianti.Sa_Cod = APPEZZAMENTO.Sa_Cod  " & vbCrLf)
                Stb_JoinRif.Append("             AND Reg_Impianti.Appezza = APPEZZAMENTO.Appezza  " & vbCrLf)
            End If

            StrSQL.Append(Stb_Join)
            StrSQL.Append(Stb_JoinRif)

            'sbagliato!
            'If Flag_Join_Appezzamento = True Then
            '    StrSQL.Append(" INNER JOIN Appezzamento ON Contatti_Produttori.Cod_Contatto = Appezzamento.PIVA " & vbCrLf)
            '    StrSQL.Append(" INNER JOIN Reg_Impianti ON Appezzamento.PIVA = Reg_Impianti.PIVA " & vbCrLf)
            '    StrSQL.Append("AND Appezzamento.SA_COD = Reg_Impianti.SA_COD AND " & vbCrLf)
            '    StrSQL.Append("Appezzamento.APPEZZA = Reg_Impianti.APPEZZA " & vbCrLf)
            '    StrSQL.Append("AND Materie_Prime.Cul_Cod = Reg_Impianti.CUL_COD " & vbCrLf)
            'End If

            Stb_Where.Append("WHERE    (Agenda_CE.Lav_Cod = " & CStr(LAVCOD_MONITORAGGIO_CE) & ")  " & vbCrLf)
            Stb_Where.Append("AND      (Movimenti_Bolla_Accettazione.Cau_Mov = '" & CAU_REGISTRAZIONI & "') " & vbCrLf)
            Stb_Where.Append("AND      (Agenda_Bolla.Lav_Cod = " & CStr(LAVCOD_ACCETTAZIONE_DIVERSI) & ") " & vbCrLf)
            Stb_Where.Append("AND      (Movimenti_Bolla_Raccolta.Cau_Mov = '" & CAU_ACCETTAZIONE_BENI_DA_DIVERSI & "') " & vbCrLf)
            Stb_Where.Append("AND      (Movimenti_Dettagli_Raccolta.Elem_Cod = " & CStr(TRASFORMATI_VEGETALI) & ") " & vbCrLf)
            ''controllo sulla data
            Stb_Where.Append(" AND     Movimenti_CE.Data_Movimento <= " & Agro_SQL_SaveDate(al_Data_Arrivo) & " ")
            Stb_Where.Append(" AND     Movimenti_CE.Data_Movimento >= " & Agro_SQL_SaveDate(dal_Data_Arrivo) & " ")

            If Piva_Produttore <> "" Then
                Stb_Where.Append(" AND Contatti_Produttori.Cod_Contatto='" & Agro_SQL_SaveText(Piva_Produttore) & "' ")
            End If

            If Codice_Specie <> 0 Then
                Stb_Where.Append(" AND Materie_Prime.Veg_Cod=" & Agro_SQL_SaveNum(Codice_Specie) & " ")
            End If
            If Codice_Prodotto <> 0 Then
                Stb_Where.Append(" AND Movimenti_Dettagli_Raccolta.Mat_Cod=" & Agro_SQL_SaveNum(Codice_Prodotto) & " ")
            End If

            If Flag_Join_Appezzamento Then
                Stb_WhereRif.Append(" AND (Mov_Dettagli_Riferimenti.Lav_Cod = " & Agro_SQL_SaveNum(LAVCOD_ACCETTAZIONE_DIVERSI) & ")  " & vbCrLf)
                Stb_WhereRif.Append(" AND (Mov_Dettagli_Riferimenti.Lav_Cod_Rif = " & Agro_SQL_SaveNum(LAVCOD_RACCOLTA) & ") " & vbCrLf)
                'Stb_WhereRif.Append(" -- AND	        Movimenti_RaccoltaImpianti.Data_Movimento >= Imprese_Progetti.Validita_Inizio " & vbCrLf)
                'Stb_WhereRif.Append(" -- AND	        Movimenti_RaccoltaImpianti.Data_Movimento <= Imprese_Progetti.Validita_Fine " & vbCrLf)

                If Piva_Produttore <> "" Then
                    Stb_WhereRif.Append("AND Reg_Impianti.Piva= '" & Agro_SQL_SaveText(Piva_Produttore) & "' ")
                End If
                If Sa_Cod_Appezzamento <> 0 Then
                    Stb_WhereRif.Append("AND Reg_Impianti.Sa_Cod= " & Agro_SQL_SaveNum(Sa_Cod_Appezzamento) & " ")
                End If
                If Appezza_Appezzamento <> 0 Then
                    Stb_WhereRif.Append("AND Reg_Impianti.Appezza= " & Agro_SQL_SaveNum(Appezza_Appezzamento) & " ")
                End If
                If Id_Reg_Appezzamento <> 0 Then
                    Stb_WhereRif.Append("AND Reg_Impianti.Id_Reg= " & Agro_SQL_SaveNum(Id_Reg_Appezzamento) & " ")
                End If
            End If

            If Tipologia_Ritrovamento <> 0 Then
                Stb_Where.Append(" AND Movimenti_Dettagli_CE.Mat_Cod =" & Agro_SQL_SaveNum(Tipologia_Ritrovamento) & " ")
            End If

            If Indice_Pericolosita = 1 Then
                Stb_Where.Append(" AND Movimenti_Dettagli_CE.Cod_Progetto = 1")
            End If
            If Indice_Pericolosita = 2 Then
                Stb_Where.Append(" AND Movimenti_Dettagli_CE.Cod_Progetto = 0")
            End If

            If Sa_Cod <> 0 Then
                Stb_Where.Append(" AND     Mov_Dest_Raccolta.Sa_Cod = " & Agro_SQL_SaveNum(Sa_Cod) & "  " & vbCrLf)
            End If
            If Fabbricato_Cod <> 0 Then
                Stb_Where.Append(" AND     Mov_Dest_Raccolta.Id_Destinazione = " & Agro_SQL_SaveNum(Fabbricato_Cod) & "  " & vbCrLf)
            End If

            '--------------------------------------------------------------------------
            If xFiltroAggiuntivo <> "" Then
                Stb_Where.Append(" AND " & Agro_SQL_Save_xFiltroAggiuntivo(xFiltroAggiuntivo, , objParametri))
            End If
            '--------------------------------------------------------------------------

            StrSQL.Append(Stb_Where)
            StrSQL.Append(Stb_WhereRif)

            'la union deve avere le stesse colonne della prima select e nello stesso ordine
            'quindi devo mettere la condizione sul not distinct
            If Not Flag_Distinct AndAlso Flag_Join_Appezzamento Then

                StrSQL.Append(" ) ")
                StrSQL.Append(" UNION ALL " & vbCrLf)
                StrSQL.Append(" ( ")

                Stb_Select.Append(" , '' AS 'Nome Appezzamento', '01/01/1900' AS 'Data Inizio Impianto', 0 AS 'Superficie Impianto' " & vbCrLf)
                StrSQL.Append(Stb_Select)

                'Stb_Join.Append(" " & vbCrLf)
                StrSQL.Append(Stb_Join)

                Stb_Where.Append(" AND              " & vbCrLf)
                'MODIFICA DEL 04/04/2012: aggiunto il not exists
                Stb_Where.Append("       NOT EXISTS              " & vbCrLf)
                Stb_Where.Append("      (              " & vbCrLf)

                'Stb_Where.Append("                      (Agenda_Bolla.PIVA  " & vbCrLf)
                'Stb_Where.Append("                     + '_' + CONVERT(varchar(10), Agenda_Bolla.Sa_Cod)  " & vbCrLf)
                'Stb_Where.Append("                     + '_' + CONVERT(varchar(10), Agenda_Bolla.Id_Agenda)  " & vbCrLf)
                'Stb_Where.Append("                   NOT  IN (  " & vbCrLf)
                'Stb_Where.Append("                              SELECT Mov_Dettagli_Riferimenti.Piva  " & vbCrLf)
                'Stb_Where.Append("                              + '_' + CONVERT(varchar(10), Mov_Dettagli_Riferimenti.Sa_Cod)  " & vbCrLf)
                'Stb_Where.Append("                              + '_' + CONVERT(varchar(10), Mov_Dettagli_Riferimenti.Id_Agenda)  " & vbCrLf)
                Stb_Where.Append("                              SELECT  1            " & vbCrLf)
                Stb_Where.Append("                              FROM Mov_Dettagli_Riferimenti ")
                'l'accettazione sta in piva sa_cod lav_cod
                Stb_Where.Append("                              INNER JOIN  Agenda Agenda_Accettazione ON Mov_Dettagli_Riferimenti.Piva = Agenda_Accettazione.PIVA  " & vbCrLf)
                Stb_Where.Append("                              AND Mov_Dettagli_Riferimenti.Sa_Cod = Agenda_Accettazione.Sa_Cod  " & vbCrLf)
                Stb_Where.Append("                              AND Mov_Dettagli_Riferimenti.Id_Agenda = Agenda_Accettazione.Id_Agenda  " & vbCrLf)
                'la raccolta sta in piva_rif sa_cod_rif lav_cod_rif
                Stb_Where.Append("                              INNER JOIN  Agenda Agenda_RaccoltaImpianti ON Mov_Dettagli_Riferimenti.Piva_Rif = Agenda_RaccoltaImpianti.PIVA  " & vbCrLf)
                Stb_Where.Append("                                          AND Mov_Dettagli_Riferimenti.Sa_Cod_Rif = Agenda_RaccoltaImpianti.Sa_Cod  " & vbCrLf)
                Stb_Where.Append("                                          AND Mov_Dettagli_Riferimenti.Id_Agenda_Rif = Agenda_RaccoltaImpianti.Id_Agenda  " & vbCrLf)
                Stb_Where.Append("                              INNER JOIN  Movimenti Movimenti_RaccoltaImpianti ON Agenda_RaccoltaImpianti.PIVA = Movimenti_RaccoltaImpianti.PIVA  " & vbCrLf)
                Stb_Where.Append("                                          AND Agenda_RaccoltaImpianti.Id_Agenda = Movimenti_RaccoltaImpianti.Id_Agenda " & vbCrLf)
                Stb_Where.Append("                              INNER JOIN  Movimenti_dettagli Movimenti_dettagli_RaccoltaImpianti ON Movimenti_RaccoltaImpianti.PIVA = Movimenti_dettagli_RaccoltaImpianti.PIVA  " & vbCrLf)
                Stb_Where.Append("                                          AND Movimenti_RaccoltaImpianti.Id_Agenda = Movimenti_dettagli_RaccoltaImpianti.Id_Agenda  " & vbCrLf)
                Stb_Where.Append("                                          AND Movimenti_RaccoltaImpianti.Id_Mov = Movimenti_dettagli_RaccoltaImpianti.Id_Mov  " & vbCrLf)
                Stb_Where.Append("                              INNER JOIN  Mov_Destinazioni Mov_Destinazioni_RaccoltaImpianti ON Movimenti_dettagli_RaccoltaImpianti.PIVA = Mov_Destinazioni_RaccoltaImpianti.Piva  " & vbCrLf)
                Stb_Where.Append("                                          AND Movimenti_dettagli_RaccoltaImpianti.Sa_Cod = Mov_Destinazioni_RaccoltaImpianti.Sa_Cod  " & vbCrLf)
                Stb_Where.Append("                                          AND Movimenti_dettagli_RaccoltaImpianti.Id_Agenda = Mov_Destinazioni_RaccoltaImpianti.Id_Agenda  " & vbCrLf)
                Stb_Where.Append("                                          AND Movimenti_dettagli_RaccoltaImpianti.Id_Mov = Mov_Destinazioni_RaccoltaImpianti.Id_Mov  " & vbCrLf)
                Stb_Where.Append("                                          AND Movimenti_dettagli_RaccoltaImpianti.Id_Mov_Det = Mov_Destinazioni_RaccoltaImpianti.Id_Mov_Det  " & vbCrLf)
                Stb_Where.Append("                              INNER JOIN Reg_Impianti ON Reg_Impianti.PIVA = Mov_Destinazioni_RaccoltaImpianti.Piva  " & vbCrLf)
                Stb_Where.Append("                                          AND Reg_Impianti.Sa_Cod = Mov_Destinazioni_RaccoltaImpianti.Sa_Cod  " & vbCrLf)
                Stb_Where.Append("                                          AND Reg_Impianti.Appezza = Mov_Destinazioni_RaccoltaImpianti.Appezza  " & vbCrLf)
                Stb_Where.Append("                                          AND Reg_Impianti.Id_Reg = Mov_Destinazioni_RaccoltaImpianti.Id_Destinazione " & vbCrLf)
                Stb_Where.Append("                              WHERE       (Mov_Dettagli_Riferimenti.Lav_Cod = " & Agro_SQL_SaveNum(LAVCOD_ACCETTAZIONE_DIVERSI) & ")  " & vbCrLf)
                Stb_Where.Append("                              AND (Mov_Dettagli_Riferimenti.Lav_Cod_Rif = " & Agro_SQL_SaveNum(LAVCOD_RACCOLTA) & ") " & vbCrLf)
                'MODIFICA DEL 04/04/2012: aggiunta di queste tre clausole di join
                Stb_Where.Append("                              AND Agenda_Bolla.PIVA = Mov_Dettagli_Riferimenti.Piva " & vbCrLf)
                Stb_Where.Append("                              AND Agenda_Bolla.Sa_Cod = Mov_Dettagli_Riferimenti.Sa_Cod " & vbCrLf)
                Stb_Where.Append("                              AND Agenda_Bolla.Id_Agenda = Mov_Dettagli_Riferimenti.Id_Agenda " & vbCrLf)
                'Stb_Where.Append("                          )  " & vbCrLf)
                'Stb_Where.Append("                      )  " & vbCrLf)
                Stb_Where.Append("      )  " & vbCrLf)
                StrSQL.Append(Stb_Where)
                StrSQL.Append(" ) ")

            End If

            If xOrderBy <> "" Then
                StrSQL.Append(" ORDER BY  " & Agro_SQL_Save_xOrderBy(xOrderBy, objParametri))
            Else
                If Flag_Distinct Then
                    StrSQL.Append(" ORDER BY Anno_Carico, Mese_carico, Giorno_Carico, Numero_Bolla")
                Else
                    StrSQL.Append(" ORDER BY 'Numero Bolla', 'Corpo Estraneo'")
                End If
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


    '''' -----------------------------------------------------------------------------
    '''' <summary>
    '''' Legge la registrazione dei corpi estranei e le bolle relative
    '''' </summary>
    '''' -----------------------------------------------------------------------------
    'Public Function LeggiCorpiEstraneiBolle_ISNULL(ByVal Flag_Distinct As Boolean, _
    '                                        ByVal Piva_Produttore As String, _
    '                                        ByVal dal_Data_Arrivo As Date, _
    '                                        ByVal al_Data_Arrivo As Date, _
    '                                        ByVal Codice_Specie As Integer, _
    '                                        ByVal Codice_Prodotto As Integer, _
    '                                        ByVal Flag_Join_Appezzamento As Boolean, _
    '                                        ByVal Sa_Cod_Appezzamento As Integer, _
    '                                        ByVal Appezza_Appezzamento As Integer, _
    '                                        ByVal Id_Reg_Appezzamento As Integer, _
    '                                        ByVal Tipologia_Ritrovamento As Integer, _
    '                                        ByVal Indice_Pericolosita As Integer, _
    '                                            ByVal xSelezioneVariabile As AgronicaCoreDataProvider.AgronicaCoreParametri.enumSelezioneVariabile, _
    '                                            ByVal xFiltroAggiuntivo As String, _
    '                                            ByVal xOrderBy As String, _
    '                                            ByRef objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri _
    '                                            ) As DataTable


    '    Dim NomeRoutine As String = "AgronicaCoreAnagrafeDAL.CorpiEstranei_R.LeggiCorpiEstraneiBolle()"

    '    Dim MessaggioErrore As String = ""
    '    Dim StrSQL As New System.Text.StringBuilder
    '    Dim DT As DataTable

    '    Try
    '        Select Case xSelezioneVariabile

    '            Case AgronicaCoreDataProvider.AgronicaCoreParametri.enumSelezioneVariabile.Selezione_TabellaDatiMinimi

    '            Case AgronicaCoreDataProvider.AgronicaCoreParametri.enumSelezioneVariabile.Selezione_TabellaCompleta

    '            Case AgronicaCoreDataProvider.AgronicaCoreParametri.enumSelezioneVariabile.Selezione_JoinDescrizioni
    '                StrSQL.Length = 0
    '                If Flag_Distinct = True Then
    '                    StrSQL.Append("SELECT  DISTINCT Agenda_CE.PIVA as Piva, Agenda_CE.Sa_Cod , Agenda_CE.Id_Agenda, Movimenti_CE.Id_Mov, Movimenti_CE.Extra_Int, CONVERT(VARCHAR(11),Movimenti_CE.Data_Movimento,106) as Data_Movimento ,  " & vbCrLf)
    '                    StrSQL.Append(" CONVERT(VARCHAR(11), Movimenti_Bolla_Accettazione.Data_Movimento, 106) AS Data_Bolla, Movimenti_CE.Doc_Numero_Sin + RIGHT(('00000'+Convert(Varchar(5),Movimenti_CE.Doc_Numero)), 5) As Numero_Bolla, Movimenti_Bolla_Accettazione.Cod_RisUm," & vbCrLf)
    '                    StrSQL.Append(" Movimenti_CE.Natura_Beni, Movimenti_CE.Extra_Str, CONVERT(VARCHAR(11),Movimenti_CE.Extra_Date,106) as Extra_Date, CONVERT(VARCHAR(5),Movimenti_CE.Ora,108) as Ora , Movimenti_CE.Colli, Movimenti_CE.Mov_Desc,  " & vbCrLf)
    '                    StrSQL.Append(" Materie_Prime.Veg_Cod, Materie_Prime.Mat_Des, Contatti_Conferenti.Rag_Soc AS Rag_Soc_Conferente, ISNULL(Contatti_Produttori.Rag_Soc, '') AS Rag_Soc_Produttore, Movimenti_Dettagli_Raccolta.QTA " & vbCrLf)
    '                Else
    '                    StrSQL.Append("SELECT  Movimenti_CE.Extra_Int as Anno, (Movimenti_CE.Doc_Numero_Sin + RIGHT(('00000'+Convert(Varchar(5),Movimenti_CE.Doc_Numero)), 5)) As 'Numero Bolla',      " & vbCrLf)
    '                    StrSQL.Append("CONVERT(VARCHAR(11),Movimenti_CE.Data_Movimento,106) as 'Data di Arrivo', Movimenti_CE.Colli as 'Carico Numero', Movimenti_Dettagli_Raccolta.QTA as 'Kg.',    " & vbCrLf)
    '                    StrSQL.Append("Veg_DEs as Specie,    " & vbCrLf)
    '                    StrSQL.Append("ISNULL( (SELECT TOP 1 VAL_COD      " & vbCrLf)
    '                    StrSQL.Append("FROM Materie_Prime_Campionature MP_Camp_Indice    " & vbCrLf)
    '                    StrSQL.Append("WHERE MP_Camp_Indice.progressivo = Movimenti_Dettagli_Raccolta.cal_cod   " & vbCrLf)
    '                    StrSQL.Append("AND MP_Camp_Indice.tipo = 'indice' AND MP_Camp_Indice.tipo_cod IN ('999', '12','1')  ), 0 ) AS Punteggio,    " & vbCrLf)
    '                    StrSQL.Append("ISNULL( (SELECT TOP 1 ISNULL( materie_prime_calibri.cal_des , ' ') AS Calibro    " & vbCrLf)
    '                    StrSQL.Append("FROM materie_prime_calibri   " & vbCrLf)
    '                    StrSQL.Append("INNER JOIN Materie_Prime_Campionature MP_Camp_Calibro ON MP_Camp_Calibro.tipo_cod =  materie_prime_calibri.cal_cod    " & vbCrLf)
    '                    StrSQL.Append("WHERE MP_Camp_Calibro.progressivo = Movimenti_Dettagli_Raccolta.cal_cod    " & vbCrLf)
    '                    StrSQL.Append("AND MP_Camp_Calibro.tipo = 'calibro'), 0 ) AS Calibro ,   " & vbCrLf)
    '                    StrSQL.Append("ISNULL(Contatti_Produttori.Rag_Soc, '') AS 'Ragione Sociale Produttore', " & vbCrLf)
    '                    StrSQL.Append("-- Appezzamento.App_Nome as Appezzamento , Reg_Impianti.Sup_Imp as 'Superficie Impianto',   " & vbCrLf)
    '                    StrSQL.Append("-- CONVERT(VARCHAR(11),Reg_Impianti.Validita_Inizio ,106)as 'Data Inizio Impianto',  " & vbCrLf)
    '                    StrSQL.Append("CONVERT(VARCHAR(11),Movimenti_CE.Extra_Date,106) + ' - '+ CONVERT(VARCHAR(5),Movimenti_CE.Ora,108) as 'Data-Ora Inizio Cottura',  " & vbCrLf)
    '                    StrSQL.Append("Movimenti_CE.Natura_Beni as 'Confezione / Marchio', Movimenti_CE.Extra_Str  as 'Livello Qualitativo',    " & vbCrLf)
    '                    StrSQL.Append("--'case per il separatore   " & vbCrLf)
    '                    StrSQL.Append("CASE Movimenti_Dettagli_CE.Mat_Cod  " & vbCrLf)
    '                    StrSQL.Append("WHEN '1' THEN 'Aereoseparatore'  " & vbCrLf)
    '                    StrSQL.Append("WHEN '2' THEN 'Cernitrice Ottica'  " & vbCrLf)
    '                    StrSQL.Append("WHEN '3' THEN 'Cernita Manuale'  " & vbCrLf)
    '                    StrSQL.Append("ELSE 'Errore Conversione'  " & vbCrLf)
    '                    StrSQL.Append("END as 'Tipologia Ritrovamento',  " & vbCrLf)
    '                    StrSQL.Append("(Select CorpiEstranei.Desc_CorpoEstraneo from CorpiEstranei where  CorpiEstranei.Cod_CorpoEstraneo=Movimenti_Dettagli_CE.Pro_Cod) as 'Corpo Estraneo',  " & vbCrLf)
    '                    StrSQL.Append("Movimenti_Dettagli_CE.QTA as 'Quantita Rilevata' ,  " & vbCrLf)
    '                    StrSQL.Append("--'case per il Pericolosita   " & vbCrLf)
    '                    StrSQL.Append("Case Movimenti_Dettagli_CE.Cod_Progetto  " & vbCrLf)
    '                    StrSQL.Append("WHEN '0' THEN 'Medio/Bassa'  " & vbCrLf)
    '                    StrSQL.Append("WHEN '1' THEN 'Alta'  " & vbCrLf)
    '                    StrSQL.Append("ELSE 'Errore Conversione'  " & vbCrLf)
    '                    StrSQL.Append("END as  'Pericolosita',  " & vbCrLf)
    '                    StrSQL.Append("Movimenti_CE.Mov_Desc as 'Note',  " & vbCrLf)


    '                    StrSQL.Append(" '' AS 'Nome Appezzamento', '' AS 'Data Inizio Impianto', 0 AS 'Superficie Impianto' ,  " & vbCrLf)
    '                    StrSQL.Append(" ISNULL(  ( SELECT APPEZZAMENTO.APP_NOME + '|' +  CONVERT( varchar(10),Reg_Impianti.Validita_Inizio,103) + '|' +  convert( varchar(500),Reg_Impianti.SUP_IMP) " & vbCrLf)
    '                    StrSQL.Append("  -- Imprese_Progetti.Progetto_Nome, " & vbCrLf)

    '                    StrSQL.Append(" FROM        Mov_Dettagli_Riferimenti " & vbCrLf)
    '                    'la raccolta sta in piva_rif sa_cod_rif lav_cod_rif
    '                    StrSQL.Append(" INNER JOIN  Agenda Agenda_RaccoltaImpianti ON Mov_Dettagli_Riferimenti.Piva_Rif = Agenda_RaccoltaImpianti.PIVA  " & vbCrLf)
    '                    StrSQL.Append("             AND Mov_Dettagli_Riferimenti.Sa_Cod_Rif = Agenda_RaccoltaImpianti.Sa_Cod  " & vbCrLf)
    '                    StrSQL.Append("             AND Mov_Dettagli_Riferimenti.Id_Agenda_Rif = Agenda_RaccoltaImpianti.Id_Agenda  " & vbCrLf)
    '                    StrSQL.Append(" INNER JOIN  Movimenti Movimenti_RaccoltaImpianti ON Agenda_RaccoltaImpianti.PIVA = Movimenti_RaccoltaImpianti.PIVA  " & vbCrLf)
    '                    StrSQL.Append("             AND Agenda_RaccoltaImpianti.Id_Agenda = Movimenti_RaccoltaImpianti.Id_Agenda " & vbCrLf)
    '                    StrSQL.Append(" INNER JOIN  Movimenti_dettagli Movimenti_dettagli_RaccoltaImpianti ON Movimenti_RaccoltaImpianti.PIVA = Movimenti_dettagli_RaccoltaImpianti.PIVA  " & vbCrLf)
    '                    StrSQL.Append("             AND Movimenti_RaccoltaImpianti.Id_Agenda = Movimenti_dettagli_RaccoltaImpianti.Id_Agenda  " & vbCrLf)
    '                    StrSQL.Append("             AND Movimenti_RaccoltaImpianti.Id_Mov = Movimenti_dettagli_RaccoltaImpianti.Id_Mov  " & vbCrLf)
    '                    StrSQL.Append(" INNER JOIN  Mov_Destinazioni Mov_Destinazioni_RaccoltaImpianti ON Movimenti_dettagli_RaccoltaImpianti.PIVA = Mov_Destinazioni_RaccoltaImpianti.Piva  " & vbCrLf)
    '                    StrSQL.Append("             AND Movimenti_dettagli_RaccoltaImpianti.Sa_Cod = Mov_Destinazioni_RaccoltaImpianti.Sa_Cod  " & vbCrLf)
    '                    StrSQL.Append("             AND Movimenti_dettagli_RaccoltaImpianti.Id_Agenda = Mov_Destinazioni_RaccoltaImpianti.Id_Agenda  " & vbCrLf)
    '                    StrSQL.Append("             AND Movimenti_dettagli_RaccoltaImpianti.Id_Mov = Mov_Destinazioni_RaccoltaImpianti.Id_Mov  " & vbCrLf)
    '                    StrSQL.Append("             AND Movimenti_dettagli_RaccoltaImpianti.Id_Mov_Det = Mov_Destinazioni_RaccoltaImpianti.Id_Mov_Det  " & vbCrLf)
    '                    StrSQL.Append(" INNER JOIN Reg_Impianti ON Reg_Impianti.PIVA = Mov_Destinazioni_RaccoltaImpianti.Piva  " & vbCrLf)
    '                    StrSQL.Append("             AND Reg_Impianti.Sa_Cod = Mov_Destinazioni_RaccoltaImpianti.Sa_Cod  " & vbCrLf)
    '                    StrSQL.Append("             AND Reg_Impianti.Appezza = Mov_Destinazioni_RaccoltaImpianti.Appezza  " & vbCrLf)
    '                    StrSQL.Append("             AND Reg_Impianti.Id_Reg = Mov_Destinazioni_RaccoltaImpianti.Id_Destinazione " & vbCrLf)
    '                    StrSQL.Append(" INNER JOIN APPEZZAMENTO ON Reg_Impianti.PIVA = APPEZZAMENTO.Piva  " & vbCrLf)
    '                    StrSQL.Append("             AND Reg_Impianti.Sa_Cod = APPEZZAMENTO.Sa_Cod  " & vbCrLf)
    '                    StrSQL.Append("             AND Reg_Impianti.Appezza = APPEZZAMENTO.Appezza  " & vbCrLf)
    '                    StrSQL.Append(" WHERE       (Mov_Dettagli_Riferimenti.Lav_Cod = " & Agro_SQL_SaveNum(LAVCOD_ACCETTAZIONE_DIVERSI) & ")  " & vbCrLf)
    '                    StrSQL.Append(" AND         (Mov_Dettagli_Riferimenti.Lav_Cod_Rif = " & Agro_SQL_SaveNum(LAVCOD_RACCOLTA) & ") " & vbCrLf)
    '                    StrSQL.Append(" -- AND	        Movimenti_RaccoltaImpianti.Data_Movimento >= Imprese_Progetti.Validita_Inizio " & vbCrLf)
    '                    StrSQL.Append(" -- AND	        Movimenti_RaccoltaImpianti.Data_Movimento <= Imprese_Progetti.Validita_Fine " & vbCrLf)

    '                    StrSQL.Append(" and Mov_Dettagli_Riferimenti.Piva = Agenda_Bolla.PIVA   " & vbCrLf)
    '                    StrSQL.Append(" AND Mov_Dettagli_Riferimenti.Sa_Cod = Agenda_Bolla.Sa_Cod  " & vbCrLf)
    '                    StrSQL.Append(" AND Mov_Dettagli_Riferimenti.Id_Agenda = Agenda_Bolla.Id_Agenda   " & vbCrLf)
    '                    If Piva_Produttore <> "" Then
    '                        StrSQL.Append("AND Reg_Impianti.Piva= '" & Agro_SQL_SaveText(Piva_Produttore) & "' ")
    '                    End If
    '                    If Sa_Cod_Appezzamento <> 0 Then
    '                        StrSQL.Append("AND Reg_Impianti.Sa_Cod= " & Agro_SQL_SaveNum(Sa_Cod_Appezzamento) & " ")
    '                    End If
    '                    If Appezza_Appezzamento <> 0 Then
    '                        StrSQL.Append("AND Reg_Impianti.Appezza= " & Agro_SQL_SaveNum(Appezza_Appezzamento) & " ")
    '                    End If
    '                    If Id_Reg_Appezzamento <> 0 Then
    '                        StrSQL.Append("AND Reg_Impianti.Id_Reg= " & Agro_SQL_SaveNum(Id_Reg_Appezzamento) & " ")
    '                    End If
    '                    StrSQL.Append(" ) " & vbCrLf)
    '                    StrSQL.Append(" , '') AS appezzamento " & vbCrLf)
    '                End If

    '                StrSQL.Append("FROM     Agenda AS Agenda_CE  " & vbCrLf)

    '                StrSQL.Append("INNER JOIN Movimenti AS Movimenti_CE ON Movimenti_CE.Id_Agenda = Agenda_CE.Id_Agenda AND Movimenti_CE.PIVA = Agenda_CE.PIVA AND " & vbCrLf)
    '                StrSQL.Append("Movimenti_CE.Sa_Cod = Agenda_CE.Sa_Cod  " & vbCrLf)

    '                StrSQL.Append("INNER JOIN Movimenti_dettagli AS Movimenti_Dettagli_CE ON Movimenti_CE.Id_Mov = Movimenti_Dettagli_CE.Id_Mov AND  " & vbCrLf)
    '                StrSQL.Append("Movimenti_CE.PIVA = Movimenti_Dettagli_CE.PIVA AND Movimenti_CE.Sa_Cod = Movimenti_Dettagli_CE.Sa_Cod AND  " & vbCrLf)
    '                StrSQL.Append("Movimenti_CE.Id_Agenda = Movimenti_Dettagli_CE.Id_Agenda  " & vbCrLf)

    '                StrSQL.Append("INNER JOIN Movimenti AS Movimenti_Bolla_Accettazione ON Movimenti_CE.Doc_Numero = Movimenti_Bolla_Accettazione.Doc_Numero AND  " & vbCrLf)
    '                StrSQL.Append("Movimenti_CE.Doc_Numero_Des = Movimenti_Bolla_Accettazione.Doc_Numero_Des AND  " & vbCrLf)
    '                StrSQL.Append("Movimenti_CE.Doc_Numero_Sin = Movimenti_Bolla_Accettazione.Doc_Numero_Sin AND  " & vbCrLf)
    '                StrSQL.Append("Movimenti_CE.Extra_Int = YEAR(Movimenti_Bolla_Accettazione.Data_Movimento)  " & vbCrLf)

    '                StrSQL.Append(" INNER JOIN Risorse_Umane Risorse_Umane_Conferenti ON Movimenti_Bolla_Accettazione.Cod_RisUm = Risorse_Umane_Conferenti.Cod_RisUm  " & vbCrLf)
    '                StrSQL.Append(" INNER JOIN Contatti Contatti_Conferenti ON Risorse_Umane_Conferenti.Piva = Contatti_Conferenti.Piva AND Risorse_Umane_Conferenti.Cod_Contatto = Contatti_Conferenti.Cod_Contatto  " & vbCrLf)
    '                StrSQL.Append(" LEFT OUTER JOIN Risorse_Umane Risorse_Umane_Produttori ON Movimenti_Bolla_Accettazione.Cod_Destinazione = Risorse_Umane_Produttori.Cod_RisUm  " & vbCrLf)
    '                StrSQL.Append(" LEFT OUTER JOIN Contatti Contatti_Produttori ON Risorse_Umane_Produttori.Piva = Contatti_Produttori.Piva AND Risorse_Umane_Produttori.Cod_Contatto = Contatti_Produttori.Cod_Contatto  " & vbCrLf)

    '                StrSQL.Append("INNER JOIN Agenda AS Agenda_Bolla ON Movimenti_Bolla_Accettazione.PIVA = Agenda_Bolla.PIVA AND  " & vbCrLf)
    '                StrSQL.Append("Movimenti_Bolla_Accettazione.Sa_Cod = Agenda_Bolla.Sa_Cod AND Movimenti_Bolla_Accettazione.Id_Agenda = Agenda_Bolla.Id_Agenda  " & vbCrLf)

    '                StrSQL.Append("INNER JOIN Movimenti AS Movimenti_Bolla_Raccolta ON Agenda_Bolla.PIVA = Movimenti_Bolla_Raccolta.PIVA AND " & vbCrLf)
    '                StrSQL.Append("Agenda_Bolla.Sa_Cod = Movimenti_Bolla_Raccolta.Sa_Cod AND Agenda_Bolla.Id_Agenda = Movimenti_Bolla_Raccolta.Id_Agenda " & vbCrLf)

    '                StrSQL.Append("INNER JOIN Movimenti_dettagli AS Movimenti_Dettagli_Raccolta ON Movimenti_Bolla_Raccolta.PIVA = Movimenti_Dettagli_Raccolta.PIVA AND " & vbCrLf)
    '                StrSQL.Append("Movimenti_Bolla_Raccolta.Id_Mov = Movimenti_Dettagli_Raccolta.Id_Mov AND " & vbCrLf)
    '                StrSQL.Append("Movimenti_Bolla_Raccolta.Id_Agenda = Movimenti_Dettagli_Raccolta.Id_Agenda " & vbCrLf)

    '                StrSQL.Append("INNER JOIN Materie_Prime ON Movimenti_Dettagli_Raccolta.Elem_Cod = Materie_Prime.Elem_Cod AND " & vbCrLf)
    '                StrSQL.Append("Movimenti_Dettagli_Raccolta.Mat_Cod = Materie_Prime.Mat_Cod" & vbCrLf)

    '                StrSQL.Append("INNER JOIN specieVegetali ON specieVegetali.Veg_Cod = Materie_Prime.Veg_Cod " & vbCrLf)

    '                'If Flag_Join_Appezzamento = True Then
    '                '    StrSQL.Append(" INNER JOIN Appezzamento ON Contatti_Produttori.Cod_Contatto = Appezzamento.PIVA " & vbCrLf)
    '                '    StrSQL.Append(" INNER JOIN Reg_Impianti ON Appezzamento.PIVA = Reg_Impianti.PIVA " & vbCrLf)
    '                '    StrSQL.Append("AND Appezzamento.SA_COD = Reg_Impianti.SA_COD AND " & vbCrLf)
    '                '    StrSQL.Append("Appezzamento.APPEZZA = Reg_Impianti.APPEZZA " & vbCrLf)
    '                '    StrSQL.Append("AND Materie_Prime.Cul_Cod = Reg_Impianti.CUL_COD " & vbCrLf)
    '                'End If

    '                StrSQL.Append("WHERE    (Agenda_CE.Lav_Cod = 5003)  " & vbCrLf)
    '                StrSQL.Append("AND      (Movimenti_Bolla_Accettazione.Cau_Mov = '4000') " & vbCrLf)
    '                StrSQL.Append("AND      (Agenda_Bolla.Lav_Cod = 1054) " & vbCrLf)
    '                StrSQL.Append("AND      (Movimenti_Bolla_Raccolta.Cau_Mov = '7920') " & vbCrLf)
    '                StrSQL.Append("AND      (Movimenti_Dettagli_Raccolta.Elem_Cod = 210) " & vbCrLf)
    '                ''controllo sulla data
    '                StrSQL.Append(" AND     Movimenti_CE.Data_Movimento <= " & Agro_SQL_SaveDate(al_Data_Arrivo) & " ")
    '                StrSQL.Append(" AND     Movimenti_CE.Data_Movimento >= " & Agro_SQL_SaveDate(dal_Data_Arrivo) & " ")

    '                If Piva_Produttore <> "" Then
    '                    StrSQL.Append(" AND Contatti_Produttori.Cod_Contatto='" & Agro_SQL_SaveText(Piva_Produttore) & "' ")
    '                End If

    '                If Codice_Specie <> 0 Then
    '                    StrSQL.Append(" AND Materie_Prime.Veg_Cod=" & Agro_SQL_SaveNum(Codice_Specie) & " ")
    '                End If
    '                If Codice_Prodotto <> 0 Then
    '                    StrSQL.Append(" AND Movimenti_Dettagli_Raccolta.Mat_Cod=" & Agro_SQL_SaveNum(Codice_Prodotto) & " ")
    '                End If

    '                'If Flag_Join_Appezzamento = True Then
    '                '    If Piva_Produttore <> "" Then
    '                '        StrSQL.Append("AND Reg_Impianti.Piva= '" & Agro_SQL_SaveText(Piva_Produttore) & "' ")
    '                '    End If
    '                '    If Sa_Cod_Appezzamento <> 0 Then
    '                '        StrSQL.Append("AND Reg_Impianti.Sa_Cod= " & Agro_SQL_SaveNum(Sa_Cod_Appezzamento) & " ")
    '                '    End If
    '                '    If Appezza_Appezzamento <> 0 Then
    '                '        StrSQL.Append("AND Reg_Impianti.Appezza= " & Agro_SQL_SaveNum(Appezza_Appezzamento) & " ")
    '                '    End If
    '                '    If Id_Reg_Appezzamento <> 0 Then
    '                '        StrSQL.Append("AND Reg_Impianti.Id_Reg= " & Agro_SQL_SaveNum(Id_Reg_Appezzamento) & " ")
    '                '    End If

    '                'End If

    '                If Tipologia_Ritrovamento <> 0 Then
    '                    StrSQL.Append(" AND Movimenti_Dettagli_CE.Mat_Cod =" & Agro_SQL_SaveNum(Tipologia_Ritrovamento) & " ")
    '                End If

    '                If Indice_Pericolosita = 1 Then
    '                    StrSQL.Append(" AND Movimenti_Dettagli_CE.Cod_Progetto = 1")
    '                End If
    '                If Indice_Pericolosita = 2 Then
    '                    StrSQL.Append(" AND Movimenti_Dettagli_CE.Cod_Progetto = 0")
    '                End If
    '                '--------------------------------------------------------------------------
    '                If xFiltroAggiuntivo <> "" Then
    '                    StrSQL.Append(" AND " & Agro_SQL_Save_xFiltroAggiuntivo(xFiltroAggiuntivo, , objParametri))
    '                End If
    '                '--------------------------------------------------------------------------
    '                If xOrderBy <> "" Then
    '                    StrSQL.Append(" ORDER BY  " & xOrderBy)
    '                Else
    '                    If Flag_Distinct = True Then
    '                        StrSQL.Append(" ORDER BY Movimenti_CE.Extra_Int desc, Numero_Bolla desc" & xOrderBy)
    '                    Else
    '                        StrSQL.Append(" ORDER BY Movimenti_CE.Data_Movimento")
    '                    End If
    '                End If

    '            Case AgronicaCoreDataProvider.AgronicaCoreParametri.enumSelezioneVariabile.Selezione_JoinCompleta
    '                '
    '                '
    '                '
    '                '

    '        End Select

    '        '--------------------------------------------------------------------------
    '        DT = EseguiQuery_Lettura(objParametri, StrSQL.ToString, NomeRoutine)
    '        '--------------------------------------------------------------------------

    '    Catch ex As Exception

    '        MessaggioErrore = ex.Message
    '        Scrivi_LOG(objParametri, NomeRoutine, MessaggioErrore)
    '        DT = Nothing
    '        Throw New Exception("[" & NomeRoutine & "] : " & MessaggioErrore)

    '    End Try

    '    Return DT

    'End Function


    '' -----------------------------------------------------------------------------
    ''' <summary>
    ''' recupera il codice più grande
    ''' </summary>
    ''' -----------------------------------------------------------------------------
    Public Function Ricava_Nuovo_Cod_CorpoEstraneo(ByVal Piva As String,
                                                   ByVal xSelezioneVariabile As enumSelezioneVariabile,
                                                   ByRef objParametri As AgronicaCoreParametri
                                                   ) As Integer

        Const nomeRoutine = "AgronicaCoreAnagrafeDAL.CorpiEstranei_R.Ricava_Nuovo_Cod_CorpoEstraneo()"

        Dim messaggioErrore As String = ""
        Dim StrSQL As New Text.StringBuilder
        Dim dt As DataTable
        Dim codCorpoEstraneo As Integer

        Try
            Select Case xSelezioneVariabile

                Case enumSelezioneVariabile.Selezione_TabellaDatiMinimi

                    StrSQL.Length = 0
                    StrSQL.Append(" SELECT ISNULL(MAX(Cod_CorpoEstraneo), 0) AS Cod_CorpoEstraneo ")
                    StrSQL.Append(" FROM  CorpiEstranei ")
                    StrSQL.Append(" WHERE 1 = 1 ")
                    'StrSQL.Append(" WHERE Piva_SuperUser = '" & Agro_SQL_SaveText(objParametri.PivaSuperUser) & "' ")
                    'If Piva <> "" Then
                    '    StrSQL.Append(" AND Piva = '" & Agro_SQL_SaveText(Piva) & "' ")
                    'End If

                    '--------------------------------------------------------------------------
                    Select Case objParametri.FlagVisibilita
                        Case enumVisibilita.Visibilita_SoloNonCancellati
                            StrSQL.Append(" AND   Inviato >=0 ")
                        Case enumVisibilita.Visibilita_SoloCancellati
                            StrSQL.Append(" AND   Inviato =-1 ")
                        Case enumVisibilita.Visibilita_Tutti
                            '...................................
                        Case Else
                            Throw New Exception("Parametro non corretto nella query (FlagVisibilita)")
                    End Select

                Case enumSelezioneVariabile.Selezione_TabellaCompleta


                Case enumSelezioneVariabile.Selezione_JoinDescrizioni


                Case enumSelezioneVariabile.Selezione_JoinCompleta

            End Select

            '--------------------------------------------------------------------------
            dt = EseguiQuery_Lettura(objParametri, StrSQL.ToString, nomeRoutine)
            '--------------------------------------------------------------------------

            If Not IsNothing(dt) AndAlso dt.Rows.Count > 0 Then
                codCorpoEstraneo = CInt(dt.Rows(0).Item("Cod_CorpoEstraneo")) + 1
            Else
                Throw New Exception("Non sono stati trovati dati.")
            End If

        Catch ex As Exception
            codCorpoEstraneo = -1
            messaggioErrore = ex.Message
            Scrivi_LOG(objParametri, nomeRoutine, messaggioErrore)
            Throw New Exception("[" & nomeRoutine & "] : " & messaggioErrore)
        End Try

        Return codCorpoEstraneo

    End Function

End Class
