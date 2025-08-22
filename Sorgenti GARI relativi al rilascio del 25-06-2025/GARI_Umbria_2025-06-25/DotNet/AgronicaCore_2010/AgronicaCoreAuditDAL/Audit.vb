Imports AgronicaCoreDataProvider.UtilityProvider
Imports AgronicaCoreDataProvider.DataProviderExtensions
Imports AgronicaCoreDataProvider.TipiEnumerativi

Public Class Audit_R
    Inherits AgronicaCoreDataProvider.DataProvider

    '##############################################################################################
    Public Function Leggi(
                            ByVal Audit_Cod As Int32,
                            ByVal Audit_Tipo As Int32,
                            ByVal Regolamento_Cod As Int32,
                            ByVal Piva As String,
                            ByVal Validita_Inizio As Date,
                            ByVal Validita_Fine As Date,
                            ByVal xFiltroAggiuntivo As String,
                            ByVal xOrderBy As String,
                            ByRef objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri,
                            Optional ByVal workFlow As Boolean = False
                         ) As DataTable

        Dim NomeRoutine As String = "AgronicaCoreAuditDAL.Audit_R.Leggi()"

        Dim MessaggioErrore As String = ""
        Dim StrSQL As New System.Text.StringBuilder
        Dim DT As DataTable

        Try

            '------------------------------------------------------------------

            StrSQL.Length = 0

            StrSQL.Append(" SELECT * ")
            StrSQL.Append(" FROM  Audit ")
            If workFlow Then
                StrSQL.Append(" LEFT JOIN Pratiche p on p.pratica_Cod = Audit.Pratica_Cod ")
                StrSQL.Append(" LEFT JOIN Pratiche_Stati_Attuali psa on psa.pratica_Cod = Audit.Pratica_Cod ")
            End If
            StrSQL.Append(" WHERE Audit.Validita_inizio <= " & Agro_SQL_SaveDate(Validita_Fine) & " ")
            StrSQL.Append(" AND   Audit.Validita_Fine >= " & Agro_SQL_SaveDate(Validita_Inizio) & " ")

            If objParametri.PivaSuperUser <> "" Then
                StrSQL.Append(" AND Audit.Audit_SuperUser = '" & Agro_SQL_SaveText(objParametri.PivaSuperUser) & "'   ")
            End If

            If Audit_Cod <> 0 Then
                StrSQL.Append(" AND Audit.Audit_Cod = " & Agro_SQL_SaveNum(Audit_Cod) & "   ")
            End If

            If Audit_Tipo <> 0 Then
                StrSQL.Append(" AND Audit.Audit_Tipo = " & Agro_SQL_SaveNum(Audit_Tipo) & "   ")
            End If

            If Regolamento_Cod <> 0 Then
                StrSQL.Append(" AND Audit.Regolamento_Cod = " & Agro_SQL_SaveNum(Regolamento_Cod) & "   ")
            End If

            If Piva <> "" Then
                StrSQL.Append(" AND Audit.Piva = '" & Agro_SQL_SaveText(Piva) & "'   ")
            End If

            If xFiltroAggiuntivo <> "" Then
                StrSQL.Append(" AND " & Agro_SQL_Save_xFiltroAggiuntivo(xFiltroAggiuntivo, , objParametri))
            End If

            '--------------------------------------------------------------------------
            Select Case objParametri.FlagVisibilita
                Case AgronicaCoreDataProvider.AgronicaCoreParametri.enumVisibilita.Visibilita_SoloNonCancellati
                    StrSQL.Append(" AND   Audit.Inviato >=0 ")
                Case AgronicaCoreDataProvider.AgronicaCoreParametri.enumVisibilita.Visibilita_SoloCancellati
                    StrSQL.Append(" AND   Audit.Inviato =-1 ")
                Case AgronicaCoreDataProvider.AgronicaCoreParametri.enumVisibilita.Visibilita_Tutti
                    '...................................
                Case Else
                    Throw New Exception("Parametro non corretto nella query (FlagVisibilita)")
            End Select
            '--------------------------------------------------------------------------

            If xOrderBy <> "" Then
                StrSQL.Append(" ORDER BY " & Agro_SQL_Save_xOrderBy(xOrderBy, objParametri))
            Else
                StrSQL.Append(" ORDER BY Audit.Audit_SuperUser, Audit.Piva, Audit.Audit_Tipo, Audit.Audit_Cod Asc ")
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

    Public Function LeggiAudit(ByVal Audit_Cod As Integer,
                               ByVal Audit_Tipo As Integer,
                               ByVal Regolamento_Cod As Integer,
                               ByVal Piva As String,
                               ByVal Validita_Inizio As Date,
                               ByVal Validita_Fine As Date,
                               ByVal xFiltroAggiuntivo As String,
                               ByVal xOrderBy As String,
                               ByRef objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri,
                               Optional ByVal NC As Boolean = False,
                               Optional ByVal workFlow As Boolean = False
                               ) As DataTable

        Dim NomeRoutine As String = "AgronicaCoreAuditDAL.Audit_R.LeggiAudit()"

        Dim MessaggioErrore As String = ""
        Dim StrSQL As New System.Text.StringBuilder
        Dim DT As DataTable

        Try

            '------------------------------------------------------------------

            If Audit_Tipo > 10 Then

                StrSQL.AppendLine(" IF NOT OBJECT_ID('tempdb.dbo.#campi_codici_CC1279') IS NULL ")
                StrSQL.AppendLine("    BEGIN ")
                StrSQL.AppendLine("    DROP TABLE #campi_codici_CC1279 ")
                StrSQL.AppendLine(" END")
                StrSQL.AppendLine(" ")

                StrSQL.AppendLine(" SELECT ")
                StrSQL.AppendLine(" '3_'+Piva+'_'+CAST(SA_Cod AS VARCHAR)+'_'+CAST(Campo_Cod AS VARCHAR) riferimento_cod ")
                StrSQL.AppendLine(", id_cod ")
                StrSQL.AppendLine(", val_cod ")
                StrSQL.AppendLine(" INTO #campi_codici_CC1279")
                StrSQL.AppendLine(" FROM campi_codici ")
                StrSQL.AppendLine(" WHERE id_cod = 1279 ")
                StrSQL.AppendLine(" ")

                StrSQL.AppendLine(" IF NOT OBJECT_ID('tempdb.dbo.#campi_codici_CC1326') IS NULL ")
                StrSQL.AppendLine("    BEGIN ")
                StrSQL.AppendLine("    DROP TABLE #campi_codici_CC1326 ")
                StrSQL.AppendLine(" END")
                StrSQL.AppendLine(" ")

                StrSQL.AppendLine(" SELECT ")
                StrSQL.AppendLine(" '3_'+Piva+'_'+CAST(SA_Cod AS VARCHAR)+'_'+CAST(Campo_Cod AS VARCHAR) riferimento_cod ")
                StrSQL.AppendLine(", id_cod ")
                StrSQL.AppendLine(", val_cod ")
                StrSQL.AppendLine(" INTO #campi_codici_CC1326")
                StrSQL.AppendLine(" FROM campi_codici ")
                StrSQL.AppendLine(" WHERE id_cod = 1326 ")
                StrSQL.AppendLine(" ")

            End If

            StrSQL.AppendLine(" SELECT DISTINCT Audit.*,Imprese.Rag_soc " & If(NC, ",NC.ID_NC ", ""))

            If workFlow Then
                StrSQL.AppendLine(" ,p.Servizio_Cod ,psa.Stato_Cod ")
                StrSQL.AppendLine(" ,(select top 1 data_creazione from pratiche_stati where pratica_cod=Audit.pratica_cod and stato_cod=" & enum_AuditStatoWorkflow.PraticaProntaAutocontrollo & " order by data_creazione desc) as Data_Chiusura_Pratica ")
            Else
                StrSQL.AppendLine(" ,null AS Stato_Cod ")
            End If

            If Audit_Tipo > 10 Then
                StrSQL.AppendLine(" ,IC.val_cod AS Codice_Socio, IC1010.val_cod AS CUAA, IC1324.val_cod AS Contratto_Produzione ")
                StrSQL.AppendLine(" ,CC1279.val_cod AS Codice_Campo, CC1326.val_cod AS Filiera ")
                StrSQL.AppendLine(" ,i.piva AS Piva_OP, i.Rag_Soc AS Rag_Soc_OP ")
            End If

            StrSQL.AppendLine(" FROM  Audit INNER Join Imprese ON Audit.Piva = Imprese.PIVA ")

            If workFlow Then
                StrSQL.AppendLine(" LEFT JOIN pratiche p ON p.pratica_Cod = Audit.pratica_Cod ")
                StrSQL.AppendLine(" LEFT JOIN pratiche_Stati_Attuali psa ON psa.pratica_Cod = Audit.pratica_Cod ")
            End If

            ' Non Conformita
            If NC Then
                StrSQL.AppendLine(" LEFT JOIN NC_Testata NC ON NC.TipoNC_Chiave = Audit.Piva+'|'+CAST(Audit.Audit_Cod AS VARCHAR)+'|'+CAST(Audit.Audit_Tipo AS VARCHAR)+'|'+CAST(Audit.Regolamento_Cod AS VARCHAR) ")
            End If

            If Audit_Tipo > 10 Then
                StrSQL.AppendLine(" LEFT JOIN Imprese_Codici IC ON IC.Piva = Imprese.Piva AND IC.id_cod = 1033 ")
                StrSQL.AppendLine(" LEFT JOIN Imprese_Codici IC1010 ON IC1010.Piva = Imprese.Piva AND IC1010.id_cod = 1010 ")
                StrSQL.AppendLine(" LEFT JOIN Imprese_Codici IC1324 ON IC1324.Piva = Imprese.Piva AND IC1324.id_Cod = 1324 ")
                StrSQL.AppendLine(" LEFT JOIN #campi_codici_CC1279 CC1279 ON CC1279.riferimento_cod = Audit.Riferimento_Cod ")
                StrSQL.AppendLine(" LEFT JOIN #campi_codici_CC1326 CC1326 ON CC1326.riferimento_cod = Audit.Riferimento_Cod ")
                StrSQL.AppendLine(" LEFT JOIN GerarchiaImprese gi on gi.Figlio=Audit.Piva LEFT JOIN Imprese i on i.piva=gi.Padre LEFT JOIN Imprese_Codici ic1088 on ic1088.PIVA=Audit.Piva and ic1088.id_cod=1088 ")
            End If

            StrSQL.AppendLine(" WHERE Audit.Validita_inizio <= " & Agro_SQL_SaveDate(Validita_Fine) & " ")
            StrSQL.AppendLine(" AND   Audit.Validita_Fine >= " & Agro_SQL_SaveDate(Validita_Inizio) & " ")

            If objParametri.PivaSuperUser <> "" Then
                StrSQL.AppendLine(" AND Audit.Audit_SuperUser = '" & Agro_SQL_SaveText(objParametri.PivaSuperUser) & "'   ")
            End If

            If Audit_Cod <> 0 Then
                StrSQL.AppendLine(" AND Audit.Audit_Cod = " & Agro_SQL_SaveNum(Audit_Cod) & "   ")
            End If

            If Audit_Tipo <> 0 Then
                StrSQL.AppendLine(" AND Audit.Audit_Tipo = " & Agro_SQL_SaveNum(Audit_Tipo) & "   ")
            End If

            If Regolamento_Cod <> 0 Then
                StrSQL.AppendLine(" AND Audit.Regolamento_Cod = " & Agro_SQL_SaveNum(Regolamento_Cod) & "   ")
            End If

            ' filtro su visibilita imprese
            If Piva <> "" Then
                StrSQL.AppendLine(" AND Audit.Piva = '" & Agro_SQL_SaveText(Piva) & "'   ")
            Else
                Dim FiltroImprese As String = ""
                Dim UtentiVisibilitaLeggi As New AgronicaCoreUtentiDAL.Utenti_Visibilita_Appoggio_R
                Dim ImpreseVisibili As DataTable = UtentiVisibilitaLeggi.Leggi(1, "", "", objParametri)
                If Not ImpreseVisibili Is Nothing AndAlso ImpreseVisibili.Rows.Count > 0 Then
                    For i = 0 To ImpreseVisibili.Rows.Count - 1
                        FiltroImprese &= "'" & Agro_SQL_SaveText(ImpreseVisibili.Rows(i).Item("Piva")) & "',"
                    Next
                    If FiltroImprese <> "" Then
                        StrSQL.AppendLine(" AND Audit.Piva IN (" & Agro_SQL_Save_Clausola_IN(Left(FiltroImprese, FiltroImprese.Length - 1), True) & ") ")
                    End If
                End If
            End If

            If xFiltroAggiuntivo <> "" Then
                StrSQL.AppendLine(" AND " & Agro_SQL_Save_xFiltroAggiuntivo(xFiltroAggiuntivo, , objParametri))
            End If

            If xOrderBy <> "" Then
                StrSQL.AppendLine(" ORDER BY " & Agro_SQL_Save_xOrderBy(xOrderBy, objParametri))
            Else
                StrSQL.AppendLine(" ORDER BY Audit.Audit_SuperUser, Audit.Piva, Audit.Audit_Tipo, Audit.Audit_Cod Asc ")
            End If

            If LivelloCompatibilita(objParametri) >= 150 Then
                StrSQL.AppendLine(" OPTION (USE HINT ('FORCE_LEGACY_CARDINALITY_ESTIMATION')) ")
            End If



            If Audit_Tipo > 10 Then
                StrSQL.AppendLine(" ")
                StrSQL.AppendLine(" DROP TABLE #campi_codici_CC1279")
                StrSQL.AppendLine(" DROP TABLE #campi_codici_CC1326")
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

    Public Function LeggiAuditLabel(ByVal Audit_Tipo As Integer, ByRef objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri) As DataTable

        Dim NomeRoutine As String = "AgronicaCoreAuditDAL.Audit_R.LeggiAuditLabel()"

        Dim MessaggioErrore As String = ""
        Dim StrSQL As New System.Text.StringBuilder
        Dim DT As DataTable

        Try

            '------------------------------------------------------------------
            StrSQL.AppendLine(" SELECT * FROM Audit_Label ")
            StrSQL.AppendLine(" WHERE Audit_Tipo = " & Agro_SQL_SaveNum(Audit_Tipo) & "   ")

            '--------------------------------------------------------------------------
            DT = EseguiQuery_Lettura(objParametri, StrSQL.ToString, NomeRoutine)
            '--------------------------------------------------------------------------

        Catch ex As Exception
            MessaggioErrore = ex.Message
            Scrivi_LOG(objParametri, NomeRoutine, MessaggioErrore)
            DT = Nothing
            'Throw New Exception("[" & NomeRoutine & "] : " & MessaggioErrore)
        End Try

        Return DT

    End Function

End Class


Public Class Audit_W
    Inherits AgronicaCoreDataProvider.DataProvider


    '################################################################################
    Public Function Audit_Copia(ByVal Audit_Cod_from As Integer,
                                ByVal Audit_Cod_to As Integer,
                                ByVal Audit_SuperUser_From As String,
                                ByVal Piva_Origine As String,
                                ByVal Piva_Destinazione As String,
                                ByRef objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri,
                                Optional ByVal Audit_Tipo As Integer = 0,
                                Optional ByVal Regolamento_Cod As Integer = 0
                                ) As Boolean

        Dim messaggioErrore As String

        '----- Descrizione
        Dim DescrizioneFunzione As String = "Audit : Copia"
        Dim NomeRoutine As String = "AgronicaCoreAuditDAL.Audit_W.Audit_Copia()"
        'Se non specificata la piva destinazione allora la copia si esegue sulla stessa azienda
        If Piva_Destinazione = "" Then
            Piva_Destinazione = Piva_Origine
        End If

        Dim xRisp As Boolean

        Dim StrSQL As New System.Text.StringBuilder
        Try
            If Audit_Cod_from = 0 Or Audit_SuperUser_From = "" Or objParametri.UsernameOperazione = "" Then
                Throw New Exception("Audit_Cod = 0 Or Audit_SuperUser = "" Or UserName_Creazione = "" Or UserName_Modifica = "" Or MessaggioErrore <> """)
            End If



            '-------Aggiungo il nuovo record ad Audit-----------------------
            StrSQL.Length = 0
            StrSQL.Append(" INSERT INTO [Audit]  ")
            'StrSQL.Append(" select (select max(audit.Audit_Cod)+1 as MaxAuditCod from audit) as [Audit_Cod]  ")
            StrSQL.Append(" select    " & Agro_SQL_SaveNum(Audit_Cod_to) & "  as [Audit_Cod]  ")
            StrSQL.Append("           , '" & Agro_SQL_SaveText(objParametri.PivaSuperUser) & "' as [Audit_SuperUser]  ")
            StrSQL.Append("           ,[Audit_Tipo]  ")
            StrSQL.Append("           , '" & Agro_SQL_SaveText(Piva_Destinazione) & "'  as [Piva]  ")
            StrSQL.Append("           ,null as[Audit_Responsabile]  ")
            StrSQL.Append("           ,[Note]  ")
            StrSQL.Append("           ,null as [inviato]  ")
            StrSQL.Append("           ,null as[datainvio]  ")
            StrSQL.Append("           ,[Validita_Inizio]  ")
            StrSQL.Append("           ,[Validita_Fine]  ")
            StrSQL.Append("           ,[Data_Creazione]  ")
            StrSQL.Append("           ,[Data_Modifica]  ")
            StrSQL.Append("           , '" & Agro_SQL_SaveText(objParametri.UsernameOperazione) & "' as [Username_Creazione]  ")
            StrSQL.Append("           , '" & Agro_SQL_SaveText(objParametri.UsernameOperazione) & "' as [Username_Modifica]  ")
            StrSQL.Append("           ,[DataLock]  ")
            StrSQL.Append("           ,[Audit_Stato]  ")
            StrSQL.Append("           ,[Regolamento_Cod]  ")
            StrSQL.Append(" from audit  ")
            StrSQL.Append(" where  ")
            StrSQL.Append("           Audit_SuperUser = '" & Agro_SQL_SaveText(Audit_SuperUser_From) & "' ")
            StrSQL.Append("           AND Audit_Cod = " & Agro_SQL_SaveNum(Audit_Cod_from) & " ")
            If Piva_Origine <> "" Then
                StrSQL.Append("       AND Piva = '" & Agro_SQL_SaveText(Piva_Origine) & "'  ")
            End If

            'Effettuo l'inserimento

            '--------------------------------------------------------------------------
            xRisp = EseguiQuery_Scrittura(objParametri, StrSQL.ToString, NomeRoutine)
            '--------------------------------------------------------------------------


            'Verifico la presenza di errori
            If Not xRisp Then
                Throw New Exception("Modulo Condizionalita : " & DescrizioneFunzione & " : " & messaggioErrore)
                Return False
            End If

            '--------Copio i nuovi record nell'Audit_Risposte ------------------------------------------
            StrSQL.Length = 0
            StrSQL.Append(" INSERT INTO [Audit_Risposte]  ")
            StrSQL.Append(" SELECT " & Agro_SQL_SaveNum(Audit_Cod_to) & "   as [Audit_Cod] ")
            StrSQL.Append("       ,[Audit_SuperUser] ")
            StrSQL.Append("       ,[Disp_Cod] ")
            StrSQL.Append("       ,[Punto_Numero] ")
            StrSQL.Append("       ,[Valore] ")
            StrSQL.Append("       ,null as [inviato] ")
            StrSQL.Append("       ,null as [datainvio] ")
            StrSQL.Append("       ,[Validita_Inizio] ")
            StrSQL.Append("       ,[Validita_Fine] ")
            StrSQL.Append("       ,[Data_Creazione] ")
            StrSQL.Append("       ,[Data_Modifica] ")
            StrSQL.Append("       , '" & Agro_SQL_SaveText(objParametri.UsernameOperazione) & "' as [Username_Creazione]  ")
            StrSQL.Append("       , '" & Agro_SQL_SaveText(objParametri.UsernameOperazione) & "' as [Username_Modifica]  ")
            StrSQL.Append("       ,[DataLock] ")
            StrSQL.Append("       ,[Audit_Tipo] ")
            StrSQL.Append("       ,[Regolamento_Cod] ")
            StrSQL.Append("       ,[PropostaCorrettiva] ")
            StrSQL.Append("   FROM [Audit_Risposte] ")
            StrSQL.Append("   WHERE ")
            StrSQL.Append("           [Audit_SuperUser] = '" & Agro_SQL_SaveText(Audit_SuperUser_From) & "' ")
            StrSQL.Append("           AND [Audit_Cod] = " & Agro_SQL_SaveNum(Audit_Cod_from) & " ")

            'Effettuo l'inserimento
            '--------------------------------------------------------------------------
            xRisp = EseguiQuery_Scrittura(objParametri, StrSQL.ToString, NomeRoutine)
            '--------------------------------------------------------------------------


            'Verifico la presenza di errori
            If Not xRisp Then
                Throw New Exception("Modulo Condizionalita : " & DescrizioneFunzione & " : " & messaggioErrore)
                Return False
            End If


        Catch ex As Exception

            messaggioErrore = ex.Message
            Scrivi_LOG(objParametri, NomeRoutine, messaggioErrore)
            xRisp = False
            Throw New Exception("[" & NomeRoutine & "] : " & messaggioErrore)
        Finally

            StrSQL = Nothing

        End Try

        Return True

    End Function


    Public Function scrivi(
                           ByVal Audit_Cod As Integer _
                        , ByVal Audit_Tipo As Integer _
                        , ByRef Audit_Responsabile As Integer _
                        , ByRef Audit_Stato As Integer _
                        , ByVal Regolamento_Cod As Integer _
                        , ByVal Piva As String _
                        , ByVal Note As String _
                        , ByVal Campionato As Integer _
                        , ByVal Validita_Inizio As Date _
                        , ByVal Validita_Fine As Date _
                        , ByVal Data_creazione As Date _
                        , ByVal Data_modifica As Date _
                        , ByVal username_creazione As String _
                        , ByVal username_modifica As String _
                        , ByRef objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri _
                        , Optional ByVal Riferimento_Cod As String = Nothing _
                        , Optional ByVal Profilo_Cod As Integer = 0 _
                        , Optional ByVal Rintracciabilita As Integer = 0 _
                        , Optional ByVal ExtraInfo As String = "" _
                        , Optional ByVal Pratica_Cod As Integer = 0
                    ) As Boolean


        Dim NomeRoutine As String = "AgronicaCoreAuditDAL.Audit_W.Scrivi()"

        '====================================================================================
        'Parametri opzionali :
        '   objConnessione = nothing    =>  viene creata (e distrutta) con StringaConnessione
        '   DirectoryLOG = ""           =>  viene usato il valore di default
        '   FileLOG = ""                =>  viene usato il valore di default
        '====================================================================================

        Dim MessaggioErrore As String = ""
        Dim StrSQL As New System.Text.StringBuilder
        Dim xRisp As Boolean = False

        Try
            '---------------------------------------------
            StrSQL.Length = 0
            StrSQL.Append("INSERT INTO Audit( " & vbCrLf)
            StrSQL.Append("   [Audit_Cod] " & vbCrLf)
            StrSQL.Append("  ,[Audit_Tipo] " & vbCrLf)
            StrSQL.Append("  ,[Audit_Responsabile] " & vbCrLf)
            StrSQL.Append("  ,[Audit_Stato] " & vbCrLf)
            StrSQL.Append("  ,[Regolamento_Cod] " & vbCrLf)
            StrSQL.Append("  ,[Piva] " & vbCrLf)
            StrSQL.Append("  ,[Note] " & vbCrLf)
            StrSQL.Append("  ,[Audit_SuperUser] " & vbCrLf)
            StrSQL.Append("  ,[Inviato] " & vbCrLf)
            StrSQL.Append("  ,[DataInvio] " & vbCrLf)
            StrSQL.Append("  ,[Data_Creazione] " & vbCrLf)
            StrSQL.Append("  ,[Data_Modifica] " & vbCrLf)
            StrSQL.Append("  ,[UserName_Creazione] " & vbCrLf)
            StrSQL.Append("  ,[UserName_Modifica] " & vbCrLf)
            StrSQL.Append("  ,[Validita_Inizio] " & vbCrLf)
            StrSQL.Append("  ,[Validita_Fine] " & vbCrLf)
            StrSQL.Append("  ,[Campionato] " & vbCrLf)
            StrSQL.Append("  ,[Rintracciabilita] " & vbCrLf)
            StrSQL.Append("  ,[ExtraInfo] " & vbCrLf)
            StrSQL.Append("  ,[Pratica_Cod] " & vbCrLf)

            ' riferimento elemento anagrafico
            If Not IsNothing(Riferimento_Cod) Then
                StrSQL.Append("  ,[Riferimento_Cod] " & vbCrLf)
            End If

            ' riferimento profilo azienda
            If Profilo_Cod <> 0 Then
                StrSQL.Append("  ,[Profilo_Cod] " & vbCrLf)
            End If


            StrSQL.Append("       ) ")
            StrSQL.Append("VALUES (")
            StrSQL.Append(" ")
            StrSQL.Append(" " & Agro_SQL_SaveNum(Audit_Cod) & " " & vbCrLf)
            StrSQL.Append(", " & Agro_SQL_SaveNum(Audit_Tipo) & " " & vbCrLf)
            If IsNothing(Audit_Responsabile) Then
                StrSQL.Append(", NULL " & vbCrLf)
            Else
                StrSQL.Append(", " & Agro_SQL_SaveNum(Audit_Responsabile) & " " & vbCrLf)
            End If
            If IsNothing(Audit_Stato) Then
                StrSQL.Append(", NULL ")
            Else
                StrSQL.Append(", " & Agro_SQL_SaveNum(Audit_Stato) & " " & vbCrLf)
            End If
            StrSQL.Append(", " & Agro_SQL_SaveNum(Regolamento_Cod) & " " & vbCrLf)
            StrSQL.Append(",'" & Agro_SQL_SaveText(Piva) & "'" & vbCrLf)
            If Note Is Nothing Then
                StrSQL.Append(", NULL ")
            Else
                StrSQL.Append(",'" & Agro_SQL_SaveText(Note) & "'" & vbCrLf)
            End If

            StrSQL.Append(",'" & Agro_SQL_SaveText(objParametri.PivaSuperUser) & "'" & vbCrLf)

            StrSQL.Append(", 0  ")
            StrSQL.Append(", Null  ")
            StrSQL.Append(", " & Agro_SQL_SaveDateTime(Data_creazione) & "  ")
            StrSQL.Append(", " & Agro_SQL_SaveDateTime(Data_modifica) & "  ")
            StrSQL.Append(",'" & Agro_SQL_SaveText(username_creazione) & "' ")
            StrSQL.Append(",'" & Agro_SQL_SaveText(username_modifica) & "' ")
            StrSQL.Append(", " & Agro_SQL_SaveDate(Validita_Inizio) & "  ")
            StrSQL.Append(", " & Agro_SQL_SaveDate(Validita_Fine) & "  ")

            If IsNothing(Campionato) Then
                Campionato = 0
            End If
            StrSQL.Append(", " & Agro_SQL_SaveNum(Campionato) & "  ")

            StrSQL.Append(", " & Agro_SQL_SaveNum(Rintracciabilita) & "  ")

            StrSQL.Append(",'" & Agro_SQL_SaveText(ExtraInfo) & "' ")

            StrSQL.Append("," & Agro_SQL_SaveNum(Pratica_Cod) & " ")

            ' riferimento elemento anagrafico
            If Not IsNothing(Riferimento_Cod) Then
                StrSQL.Append(",'" & Agro_SQL_SaveText(Riferimento_Cod) & "' ")
            End If

            ' riferimento profilo azienda
            If Profilo_Cod <> 0 Then
                StrSQL.Append("," & Agro_SQL_SaveNum(Profilo_Cod) & " ")
            End If

            StrSQL.Append(")")
            '--------------------------------------------


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

    '########################################################################################
    Public Function Cancellazione(ByVal Audit_Cod As Long, _
                                        ByVal Audit_SuperUser As String, _
                                        ByVal Piva As String, _
                                        ByRef objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri) As Integer

        Dim NomeRoutine As String = "AgronicaCoreAuditDAL.Audit_W.Cancellazione()"

        '====================================================================================
        'Parametri opzionali :
        '   objConnessione = nothing    =>  viene creata (e distrutta) con StringaConnessione
        '   DirectoryLOG = ""           =>  viene usato il valore di default
        '   FileLOG = ""                =>  viene usato il valore di default
        '====================================================================================

        Dim MessaggioErrore As String = ""
        Dim StrSQL As New System.Text.StringBuilder
        Dim xRisp As Boolean = False

        Try
            StrSQL.Length = 0
            StrSQL.AppendLine(" DELETE " & _
                 " FROM  Audit " & _
                 " WHERE Audit_SuperUser = '" & Agro_SQL_SaveText(Audit_SuperUser) & "'")

            'If Audit_Cod <> 0 Then
            StrSQL.AppendLine(" AND Audit_Cod = " & Agro_SQL_SaveNum(Audit_Cod))
            'End If

            'If Regolamento_Cod <> 0 Then
            '    StrSQL &= " AND Regolamento_Cod = " & SQL_SaveNum(Regolamento_Cod)
            'End If

            If Piva <> "" Then
                StrSQL.AppendLine(" AND Piva = '" & Agro_SQL_SaveText(Piva) & "'")
            End If

            'Effettuo la modifica
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