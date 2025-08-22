

Imports System.Data.OleDb
Imports AgronicaCoreDataProvider.UtilityProvider
Imports AgronicaCoreDataProvider.TipiEnumerativi
Imports AgronicaCoreDataProvider.CostantiPersonalizzate
Imports AgronicaCoreDataProvider.AgronicaCoreParametri

Public Class Audit_Codici_R
    Inherits AgronicaCoreDataProvider.DataProvider

    '##############################################################################################
    Public Function Leggi(ByVal Audit_Tipo As enum_AuditTipi, _
                                       ByVal Regolamento_Cod As Integer, _
                                       ByVal Disposizione_Cod As Integer, _
                                       ByVal Sezione_Cod As Long, _
                                      ByVal Parte_Cod As Long, _
                                      ByRef MessaggioErrore As String, _
                                       ByVal xFiltroAggiuntivo As String, _
                                        ByVal xOrderBy As String, _
                                        ByRef objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri, _
                                      Optional ByVal Audit_Cod As Integer = 0, _
                                      Optional ByVal Tipo_Audit As String = "", _
                                      Optional ByVal Punto_Numero As String = "", _
                                      Optional ByVal Descrizione As String = "", _
                                      Optional ByVal Punteggio As Integer = 0, _
                                      Optional ByVal Nota As Integer = -1, _
                                      Optional ByVal Validita_Inizio As Date = #1/1/1900#, _
                                      Optional ByVal Validita_Fine As Date = #12/31/2100#, _
                                      Optional ByVal OrdinaxDate As Boolean = False, _
                                      Optional ByVal Profilo_cod As Integer = 0 _
                                        ) As DataTable

        'TipiEnumerativi.enum_AuditTipi.AuditTipi_SicurezzaLavoro, CInt(RegolamentoCod), CInt(disp), DT_Sezioni.Rows(i).Item("Sezione_Cod"), FiltroParte, strErr, CInt(Audit_Cod), , , , , , _
        '                                         Audit_Data_Inizio, Audit_Data_Fine
        '----- Descrizione
        Dim NomeRoutine As String = "AgronicaCoreAuditDAL.Audit_Codici_R.Leggi()"

        '----- Variabili
        Dim StrSQL As New System.Text.StringBuilder
        Dim DT As DataTable

        Try

            StrSQL.Length = 0

            'StrSQL = " SELECT Audit_Codici.*, Audit_Disposizioni.Disp_Nome, Audit_Disposizioni.Descrizione AS [Direttiva], " & _
            '         " " & _
            '         " ISNULL ((SELECT Audit_Risposte.Valore  FROM Audit_Risposte " & _
            '         " WHERE Audit_Risposte.Audit_Cod = " & SQL_SaveNum(Audit_Cod) & " " & _
            '         " AND Audit_Risposte.Disp_Cod = Audit_Codici.Disp_Cod " & _
            '         " AND Audit_Risposte.Punto_Numero = Audit_Codici.Punto_Numero COLLATE SQL_Latin1_General_CP850_CI_AS), NULL) AS Valore " & _
            '         " " & _
            '         " FROM   Audit_Codici " & _
            '         " INNER JOIN Audit_Disposizioni ON Audit_Codici.Disp_Cod = Audit_Disposizioni.Disp_Cod "

            StrSQL.Append(" SELECT Audit_Codici.*, Audit_Disposizioni.Disp_Nome, Audit_Disposizioni.Descrizione AS [Direttiva], ")
            StrSQL.Append(" ISNULL ((SELECT Audit_Risposte.Valore  FROM Audit_Risposte ")
            StrSQL.Append(" WHERE Audit_Risposte.Audit_Cod = " & Agro_SQL_SaveNum(Audit_Cod) & " ")
            StrSQL.Append(" AND Audit_Risposte.Audit_Tipo = " & Agro_SQL_SaveNum(Audit_Tipo) & " ")
            StrSQL.Append(" AND Audit_Risposte.Regolamento_Cod = " & Agro_SQL_SaveNum(Regolamento_Cod) & " ")
            StrSQL.Append(" AND Audit_Risposte.Audit_Tipo = Audit_Codici.Audit_Tipo ")
            StrSQL.Append(" AND Audit_Risposte.Regolamento_Cod = Audit_Codici.Regolamento_Cod ")
            StrSQL.Append(" AND Audit_Risposte.Disp_Cod = Audit_Codici.Disp_Cod ")
            StrSQL.Append(" AND Audit_Risposte.Punto_Numero = Audit_Codici.Punto_Numero COLLATE SQL_Latin1_General_CP850_CI_AS), NULL) AS Valore, ")
            StrSQL.Append(" ISNULL ((SELECT Audit_Risposte.Valore_2  FROM Audit_Risposte ")
            StrSQL.Append(" WHERE Audit_Risposte.Audit_Cod = " & Agro_SQL_SaveNum(Audit_Cod) & " ")
            StrSQL.Append(" AND Audit_Risposte.Audit_Tipo = " & Agro_SQL_SaveNum(Audit_Tipo) & " ")
            StrSQL.Append(" AND Audit_Risposte.Regolamento_Cod = " & Agro_SQL_SaveNum(Regolamento_Cod) & " ")
            StrSQL.Append(" AND Audit_Risposte.Audit_Tipo = Audit_Codici.Audit_Tipo ")
            StrSQL.Append(" AND Audit_Risposte.Regolamento_Cod = Audit_Codici.Regolamento_Cod ")
            StrSQL.Append(" AND Audit_Risposte.Disp_Cod = Audit_Codici.Disp_Cod ")
            StrSQL.Append(" AND Audit_Risposte.Punto_Numero = Audit_Codici.Punto_Numero COLLATE SQL_Latin1_General_CP850_CI_AS), NULL) AS Valore_2, ")
            StrSQL.Append(" ISNULL ((SELECT Audit_Risposte.PropostaCorrettiva  FROM Audit_Risposte ")
            StrSQL.Append(" WHERE Audit_Risposte.Audit_Cod = " & Agro_SQL_SaveNum(Audit_Cod) & " ")
            StrSQL.Append(" AND Audit_Risposte.Audit_Tipo = " & Agro_SQL_SaveNum(Audit_Tipo) & " ")
            StrSQL.Append(" AND Audit_Risposte.Regolamento_Cod = " & Agro_SQL_SaveNum(Regolamento_Cod) & " ")
            StrSQL.Append(" AND Audit_Risposte.Audit_Tipo = Audit_Codici.Audit_Tipo ")
            StrSQL.Append(" AND Audit_Risposte.Regolamento_Cod = Audit_Codici.Regolamento_Cod ")
            StrSQL.Append(" AND Audit_Risposte.Disp_Cod = Audit_Codici.Disp_Cod ")
            StrSQL.Append(" AND Audit_Risposte.Punto_Numero = Audit_Codici.Punto_Numero COLLATE SQL_Latin1_General_CP850_CI_AS), '') AS ValPropostaCorrettiva ")
            StrSQL.Append(" ")
            StrSQL.Append(" FROM   Audit_Codici ")
            StrSQL.Append(" INNER JOIN Audit_Disposizioni ON Audit_Codici.Audit_Tipo = Audit_Disposizioni.Audit_Tipo AND Audit_Codici.Regolamento_Cod = Audit_Disposizioni.Regolamento_Cod AND Audit_Codici.Disp_Cod = Audit_Disposizioni.Disp_Cod ")



            ' NOTA BENE : NON eliminare 
            ' COLLATE SQL_Latin1_General_CP850_CI_AS -> specifica la regola di confronto, 
            ' se fossero diverse darebbe errore (voce Confronto nella struttura del campo Audit_Risposte.Punto_Numero) 

            'If Parte_Cod <> 0 Then
            '    StrSQL &= " INNER JOIN Audit_Sezioni ON Audit_Codici.Sezione_Cod = Audit_Sezioni.Sezione_Cod "
            'End If

            If Parte_Cod <> 0 Then
                StrSQL.Append(" INNER JOIN Audit_Sezioni ON Audit_Codici.Audit_Tipo = Audit_Sezioni.Audit_Tipo AND Audit_Codici.Regolamento_Cod = Audit_Sezioni.Regolamento_Cod AND Audit_Codici.Sezione_Cod = Audit_Sezioni.Sezione_Cod ")
            End If

            'StrSQL &= " WHERE 1=1 "
            StrSQL.Append(" WHERE Audit_Codici.Audit_Tipo = " & Agro_SQL_SaveNum(Audit_Tipo))

            If Regolamento_Cod <> 0 Then
                StrSQL.Append(" AND Audit_Codici.Regolamento_Cod = " & Agro_SQL_SaveNum(Regolamento_Cod))
            End If

            If Validita_Fine <> #12/31/2100# Then
                StrSQL.Append(" AND Audit_Codici.Validita_inizio <= " & Agro_SQL_SaveDate(Validita_Fine))
            End If

            If Validita_Inizio <> #1/1/1900# Then
                StrSQL.Append(" AND Audit_Codici.Validita_Fine >= " & Agro_SQL_SaveDate(Validita_Inizio))
            End If

            If Nota <> -1 Then
                StrSQL.Append(" AND Audit_Codici.Nota = " & Agro_SQL_SaveNum(Nota))
            End If

            If Disposizione_Cod <> 0 Then
                StrSQL.Append(" AND Audit_Codici.Disp_Cod = " & Agro_SQL_SaveNum(Disposizione_Cod))
            End If

            If Sezione_Cod <> 0 Then
                StrSQL.Append(" AND Audit_Codici.Sezione_Cod = " & Agro_SQL_SaveNum(Sezione_Cod))
            End If

            If Parte_Cod <> 0 Then
                StrSQL.Append(" AND Audit_Sezioni.Parte = " & Agro_SQL_SaveNum(Parte_Cod))
            End If

            If Punto_Numero <> "" Then
                StrSQL.Append(" AND Audit_Codici.Punto_Numero = '" & Agro_SQL_SaveText(Punto_Numero) & "'")
            End If

            If Descrizione <> "" Then
                StrSQL.Append(" AND Audit_Codici.Descrizione LIKE '%" & Agro_SQL_SaveText(Descrizione) & "%'")
            End If

            If Punteggio <> 0 Then
                StrSQL.Append(" AND Audit_Codici.Punteggio = " & Agro_SQL_SaveNum(Punteggio))
            End If


            If Tipo_Audit <> "" Then    'A=atti, N=norme
                StrSQL.Append(" AND Audit_Disposizioni.Disp_Nome LIKE '" & Agro_SQL_SaveText(Tipo_Audit) & "%'")
            End If


            If xFiltroAggiuntivo <> "" Then
                StrSQL.Append(" AND " & Agro_SQL_Save_xFiltroAggiuntivo(xFiltroAggiuntivo, , objParametri))
            End If
            '--------------------------------------------------------------------------
            Select Case objParametri.FlagVisibilita
                Case AgronicaCoreDataProvider.AgronicaCoreParametri.enumVisibilita.Visibilita_SoloNonCancellati
                    StrSQL.Append(" AND   Audit_Codici.Inviato >=0 ")
                Case AgronicaCoreDataProvider.AgronicaCoreParametri.enumVisibilita.Visibilita_SoloCancellati
                    StrSQL.Append(" AND   Audit_Codici.Inviato =-1 ")
                Case AgronicaCoreDataProvider.AgronicaCoreParametri.enumVisibilita.Visibilita_Tutti
                    '...................................
                Case Else
                    Throw New Exception("Parametro non corretto nella query (FlagVisibilita)")
            End Select
            '--------------------------------------------------------------------------

            If xOrderBy <> "" Then
                StrSQL.Append(" ORDER BY " & Agro_SQL_Save_xOrderBy(xOrderBy, objParametri))
            Else

                'If OrdinaxDate Then
                '    StrSQL.Append(" ORDER BY Audit.Validita_Inizio DESC, Audit.Audit_Cod DESC, Audit.Audit_SuperUser, Audit.Piva ASC ")
                'Else
                '    StrSQL.Append(" ORDER BY Audit.Audit_SuperUser, Audit.Piva, Audit.Validita_Inizio ASC ")
                'End If
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


'#################################################################
'#################################################################
'#################################################################

Public Class Audit_Codici_W
    Inherits AgronicaCoreDataProvider.DataProvider



    '##############################################################################################
    Public Function Scrivi(ByRef objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri _
                , Optional ByVal Data_creazione As Date = #2/1/1900# _
                , Optional ByVal Data_modifica As Date = #2/1/1900# _
                , Optional ByVal username_creazione As String = "" _
                , Optional ByVal username_modifica As String = "" _
                ) As Boolean


        Dim NomeRoutine As String = "AgronicaCoreAuditDAL.Audit_Codici_W.Scrivi()"

        '====================================================================================
        'Parametri opzionali :

        '====================================================================================

        Dim MessaggioErrore As String = ""
        Dim Stb As New System.Text.StringBuilder
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
            Stb.Length = 0
            Stb.Append(" INSERT ... " + vbCrLf)

            Stb.Append("              (")
            Stb.Append("              Inviato,            datainvio, ")
            Stb.Append("              Data_Creazione,     Data_Modifica, ")
            Stb.Append("              UserName_Creazione, UserName_Modifica, ")
            Stb.Append("              Validita_Inizio,    Validita_Fine, ")
            Stb.Append("              ) ")

            Stb.Append(" VALUES ( ")



            Stb.Append("         , 0  " + vbCrLf)
            Stb.Append("         , Null  " + vbCrLf)

            Stb.Append("			, " & Agro_SQL_SaveDate(Data_creazione) & "  ")
            Stb.Append("			, " & Agro_SQL_SaveDate(Data_modifica) & "  ")
            Stb.Append("			,'" & Agro_SQL_SaveText(username_creazione) & "' ")
            Stb.Append("			,'" & Agro_SQL_SaveText(username_modifica) & "' ")



            Stb.Append(") ")

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







    '#################################################################
    Public Function Cancella(ByVal xFiltroAggiuntivo As String, _
                              ByRef objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri _
                              ) As Boolean

        '----- Descrizione
        Dim NomeRoutine As String = "AgronicaCoreAuditDAL.Audit_Codici_W.Cancella()"

        Dim MessaggioErrore As String = ""
        Dim Stb As New System.Text.StringBuilder
        Dim xRisp As Boolean = False

        Try
            '---------------------------------------------
            Stb.Length = 0

            '---------------------------------------------
            If objParametri.FlagCancellazioneLogica = enumCancellazioneLogica.CancellazioneLogica Then
                Stb.Append(" UPDATE ... ")
                Stb.Append(" SET ")
                Stb.Append("         Username_Modifica = '" & Agro_SQL_SaveText(objParametri.UsernameOperazione) & "' ")
                Stb.Append("         ,Data_Modifica= " & Agro_SQL_SaveDate(Date.Now) & " ")
                Stb.Append("         ,Inviato = -1 ")
                Stb.Append(" WHERE   1=1 ")
                Stb.Append(" AND     Inviato >= 0 ")
            Else
                Stb.Append(" DELETE FROM ... ")
                Stb.Append(" WHERE 1=1 ")
            End If
            '---------------------------------------------

            If xFiltroAggiuntivo <> "" Then
                Stb.Append(" AND " & Agro_SQL_Save_xFiltroAggiuntivo(xFiltroAggiuntivo, , objParametri))
            End If

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

