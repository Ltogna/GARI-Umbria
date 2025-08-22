Imports System.Text
Imports AgronicaCoreDataProvider
Imports AgronicaCoreDataProvider.CostantiPersonalizzate
Imports AgronicaCoreDataProvider.AgronicaCoreParametri
Imports AgronicaCoreDataProvider.TipiEnumerativi

Public Class APP_Dati_R
    Inherits AgronicaCoreDataProvider.DataProvider

    Public Function Leggi_DatiAPP(ByVal ID As String,
                                  ByVal Tipo As String,
                                  ByVal Solo_Non_Sincronizzati As Boolean,
                                  ByVal xFiltroAggiuntivo As String,
                                  ByVal xOrderBy As String,
                                  ByRef objParametri As AgronicaCoreParametri,
                                  Optional ByVal Piva As String = "",
                                  Optional ByVal Data As Date = AGRODATAINIZIO,
                                  Optional ByVal Username As String = "") As DataTable


        Dim NomeRoutine As String = "AgronicaCoreContabDAL.APP_Dati_R.Leggi_DatiAPP()"

        Dim MessaggioErrore As String = ""
        Dim StrSQL As New StringBuilder
        Dim DT As DataTable

        Try

            Dim App_Dati As Boolean = Left(Tipo, 4) <> "APP_"

            StrSQL.AppendLine(" SELECT * ")
            StrSQL.AppendLine(" FROM " & If(App_Dati, "App_Dati", Tipo) & " with(nolock) ")
            StrSQL.AppendLine(" WHERE  1 = 1 ")

            If Not String.IsNullOrEmpty(ID) Then
                StrSQL.AppendLine(" And ID Like '%" & Agro_SQL_SaveText(ID) & "%' ")
            End If

            If App_Dati AndAlso Not String.IsNullOrEmpty(Tipo) Then
                StrSQL.AppendLine(" AND Tipo IN (" & Agro_SQL_Save_Clausola_IN(Tipo, True) & ") ")
            End If

            If Not String.IsNullOrEmpty(Piva) Then
                StrSQL.AppendLine(" AND Piva = '" & Agro_SQL_SaveText(Piva) & "' ")
            End If

            If Data <> AGRODATAINIZIO Then
                StrSQL.AppendLine(" AND datainvio >= " & Agro_SQL_SaveDate(Data) & " ")
            End If

            If Not String.IsNullOrEmpty(Username) Then
                StrSQL.AppendLine(" AND Username_Creazione = '" & Agro_SQL_SaveText(Username) & "' ")
            End If

            If Solo_Non_Sincronizzati Then
                StrSQL.AppendLine(" AND Importato_Data IS NULL ")
            End If

            If xFiltroAggiuntivo <> "" Then
                StrSQL.AppendLine(" AND " & Agro_SQL_Save_xFiltroAggiuntivo(xFiltroAggiuntivo, , objParametri))
            End If

            If xOrderBy <> "" Then
                StrSQL.AppendLine(" ORDER BY " & Agro_SQL_Save_xOrderBy(xOrderBy, objParametri))
            Else
                StrSQL.AppendLine(" ORDER BY ID ")
            End If

            '--------------------------------------------------------------------------
            DT = EseguiQuery_Lettura(objParametri, StrSQL.ToString, NomeRoutine)
            '--------------------------------------------------------------------------

        Catch ex As Exception

            MessaggioErrore = ex.Message
            Scrivi_LOG(objParametri, NomeRoutine, MessaggioErrore)
            Throw New Exception("[" & NomeRoutine & "] : " & MessaggioErrore)

        End Try

        Return DT

    End Function

    Public Function Leggi_QDCA(ByVal ID As String,
                                  ByVal Tipo As String,
                                  ByVal Solo_Non_Sincronizzati As Boolean,
                                  ByVal xFiltroAggiuntivo As String,
                                  ByVal xOrderBy As String,
                                  ByRef objParametri As AgronicaCoreParametri,
                                  Optional ByVal Piva As String = "",
                                  Optional ByVal Data As Date = AGRODATAINIZIO,
                                  Optional ByVal Username As String = "") As DataTable

        Dim NomeRoutine As String = "AgronicaCoreContabDAL.APP_Dati_R.Leggi_DatiAPP()"

        Dim MessaggioErrore As String = ""
        Dim StrSQL As New StringBuilder
        Dim DT As DataTable

        Try

            Dim App_Dati As Boolean = Left(Tipo, 4) <> "APP_"

            StrSQL.AppendLine(" SELECT * ")
            StrSQL.AppendLine(" FROM " & If(App_Dati, "App_Dati", Tipo) & " with(nolock) ")
            StrSQL.AppendLine(" WHERE  1 = 1 ")

            If Not String.IsNullOrEmpty(ID) Then
                StrSQL.AppendLine(" And ID Like '%" & Agro_SQL_SaveText(ID) & "%' ")
            End If

            If App_Dati AndAlso Not String.IsNullOrEmpty(Tipo) Then
                StrSQL.AppendLine(" AND Tipo IN (" & Agro_SQL_Save_Clausola_IN(Tipo, True) & ") ")
            End If

            If Not String.IsNullOrEmpty(Piva) Then
                StrSQL.AppendLine(" AND Piva = '" & Agro_SQL_SaveText(Piva) & "' ")
            End If

            If Data <> AGRODATAINIZIO Then
                StrSQL.AppendLine(" AND datainvio >= " & Agro_SQL_SaveDate(Data) & " ")
            End If

            If Not String.IsNullOrEmpty(Username) Then
                StrSQL.AppendLine(" AND Username_Creazione = '" & Agro_SQL_SaveText(Username) & "' ")
            End If

            If Solo_Non_Sincronizzati Then
                StrSQL.AppendLine(" AND Importato_Data IS NULL ")
            End If

            If xFiltroAggiuntivo <> "" Then
                StrSQL.AppendLine(" AND " & Agro_SQL_Save_xFiltroAggiuntivo(xFiltroAggiuntivo, , objParametri))
            End If

            If xOrderBy <> "" Then
                StrSQL.AppendLine(" ORDER BY " & Agro_SQL_Save_xOrderBy(xOrderBy, objParametri))
            Else
                StrSQL.AppendLine(" ORDER BY ID ")
            End If

            '--------------------------------------------------------------------------
            DT = EseguiQuery_Lettura(objParametri, StrSQL.ToString, NomeRoutine)
            '--------------------------------------------------------------------------

        Catch ex As Exception

            MessaggioErrore = ex.Message
            Scrivi_LOG(objParametri, NomeRoutine, MessaggioErrore)
            Throw New Exception("[" & NomeRoutine & "] : " & MessaggioErrore)

        End Try

        Return DT


    End Function

    Public Function Consulta_Sincro_Dati_App(ByVal Tipi As List(Of String),
                                             ByVal FiltroImportati As Integer,
                                             ByVal xFiltroAggiuntivo As String,
                                             ByVal xOrderBy As String,
                                             ByRef objParametri_Server As AgronicaCoreParametri,
                                             ByRef objParametri_Utenti As AgronicaCoreParametri,
                                             Optional ByVal Data_Inizio As Date = AGRODATAINIZIO,
                                             Optional ByVal Data_Fine As Date = AGRODATAFINE,
                                             Optional ByVal DatiAggiuntivi As Boolean = False) As DataTable

        Dim NomeRoutine As String = "AgronicaCoreContabDAL.APP_Dati_R.Consulta_Sincro_Dati_App()"

        Dim MessaggioErrore As String = ""
        Dim StrSQL As New StringBuilder
        Dim DT As DataTable

        Try

            Dim Filtro_Visibilita_Utente = LeggiSeFiltroVisibilitaUtentePresente(objParametri_Utenti)

            StrSQL.AppendLine(" SELECT u.[user] utente, ")
            StrSQL.AppendLine(" a.Piva azienda_cod, ")
            StrSQL.AppendLine(" i.rag_soc azienda_des, ")
            StrSQL.AppendLine(" a.Importato_Data data_sincro, ")

            If Not IsNothing(DatiAggiuntivi) AndAlso DatiAggiuntivi Then
                StrSQL.AppendLine(" a.Dati, ")
            End If

            StrSQL.AppendLine(" a.Tipo, ")
            StrSQL.AppendLine(" a.Riferimento,")
            StrSQL.AppendLine(" a.importato_errore importato_errore,")
            StrSQL.AppendLine(" a.cancellato cancellato")
            StrSQL.AppendLine(" FROM app_dati a")
            StrSQL.AppendLine(" LEFT OUTER JOIN imprese i ON i.piva = a.piva ")
            StrSQL.AppendLine(" LEFT OUTER JOIN utenti u ON u.CODICE_FISCALE = a.Username_Creazione ")

            If Filtro_Visibilita_Utente Then
                StrSQL.AppendLine(" LEFT JOIN Utenti_Visibilita_Appoggio uva (NOLOCK) On a.Piva = uva.Piva AND uva.Entita_Cod=1 AND uva.Username = " & Agro_SQL_SaveText_NULL(objParametri_Server.UtenteUsername) & " ")
            End If

            StrSQL.AppendLine(" WHERE 1 = 1")

            If Filtro_Visibilita_Utente Then
                StrSQL.AppendLine(" AND uva.Username = " & Agro_SQL_SaveText_NULL(objParametri_Server.UtenteUsername) & " ")
            End If

            If Not IsNothing(Tipi) AndAlso Tipi.Count() > 0 Then
                StrSQL.AppendLine(" AND a.tipo IN ( " & Agro_SQL_Save_Clausola_IN(String.Join(",", Tipi), True) & " ) ")
            End If

            Select Case FiltroImportati

                'Importati
                Case 0

                    StrSQL.AppendLine(" AND a.Importato_Data IS NOT NULL")

                    If Data_Inizio <> AGRODATAINIZIO Then
                        StrSQL.AppendLine(" AND CONVERT(Datetime, a.Importato_Data) >= " & Agro_SQL_SaveDateTime(Data_Inizio) & " ")
                    End If

                    If Data_Fine <> AGRODATAFINE Then
                        StrSQL.AppendLine(" AND CONVERT(Datetime, a.Importato_Data) <= " & Agro_SQL_SaveDateTime(Data_Fine) & " ")
                    End If

                'Non importati
                Case 1

                    StrSQL.AppendLine(" AND a.Importato_Data IS NULL")

                'Tutti
                Case 2

                    If Data_Inizio <> AGRODATAINIZIO OrElse Data_Fine <> AGRODATAFINE Then

                        StrSQL.AppendLine(" AND ( a.Importato_Data IS NULL")

                        StrSQL.AppendLine(" OR ( ")

                        If Data_Inizio <> AGRODATAINIZIO Then
                            StrSQL.Append("CONVERT(Datetime, a.Importato_Data) >= " & Agro_SQL_SaveDateTime(Data_Inizio) & " ")
                        End If

                        If Data_Inizio <> AGRODATAINIZIO AndAlso Data_Fine <> AGRODATAFINE Then
                            StrSQL.AppendLine(" AND ")
                        End If

                        If Data_Fine <> AGRODATAFINE Or (IsNothing(Data_Fine)) Then
                            StrSQL.AppendLine("CONVERT(Datetime, a.Importato_Data) <= " & Agro_SQL_SaveDateTime(Data_Fine) & " ")
                        End If

                        StrSQL.Append(") ) ")

                    End If

            End Select

            If xFiltroAggiuntivo <> "" Then
                StrSQL.AppendLine(" AND " & Agro_SQL_Save_xFiltroAggiuntivo(xFiltroAggiuntivo, , objParametri_Server))
            End If

            If xOrderBy <> "" Then
                StrSQL.AppendLine(" ORDER BY " & Agro_SQL_Save_xOrderBy(xOrderBy, objParametri_Server))
            Else
                StrSQL.AppendLine(" ORDER BY a.Importato_Data desc ")
            End If

            '--------------------------------------------------------------------------
            DT = EseguiQuery_Lettura(objParametri_Server, StrSQL.ToString, NomeRoutine)
            '--------------------------------------------------------------------------

        Catch ex As Exception

            MessaggioErrore = ex.Message
            Scrivi_LOG(objParametri_Server, NomeRoutine, MessaggioErrore)
            Throw New Exception("[" & NomeRoutine & "] : " & MessaggioErrore)

        End Try

        Return DT

    End Function

    Private Function LeggiSeFiltroVisibilitaUtentePresente(objParametri_Utenti As AgronicaCoreParametri) As Boolean

        Dim objProfilo As New AgronicaCoreUtentiDAL.Utenti_Profili_Read
        Dim DTProfilo As DataTable
        Dim Sql_Permessi As String = ""

        DTProfilo = objProfilo.Leggi(objParametri_Utenti.UtenteUsername,
                                     5,
                                     AgronicaCoreParametri.enumSelezioneVariabile.Selezione_TabellaCompleta,
                                     "",
                                     "",
                                     objParametri_Utenti)

        If DTProfilo.Rows.Count > 0 Then
            Sql_Permessi = DTProfilo.Rows(0).Item("Descrizione_2") & ""
        End If

        Dim Filtro_Visibilita_Utente = True
        If Sql_Permessi = "" Then
            Filtro_Visibilita_Utente = False
        End If

        Return Filtro_Visibilita_Utente

    End Function

    Public Function Leggi_RicetteAPPDaImportareInAgenda(ByRef objParametri As AgronicaCoreParametri, Optional ByVal guid As String = "", Optional ByVal lav_cod As String = "") As DataTable

        Dim NomeRoutine As String = "AgronicaCoreContabDAL.APP_Dati_R.Leggi_RicetteAPPDaImportareInAgenda()"
        Dim MessaggioErrore As String = ""
        Dim DT As DataTable

        Try

            Dim StrSQL As New StringBuilder
            StrSQL.AppendLine(" SELECT Ricetta_SuperUser, Ricetta_Cod, Ricetta_Operazione_Cod, Lav_Cod, Raccoglitore_Cod ")
            StrSQL.AppendLine(" FROM  Ricette_Operazioni AS ro")
            StrSQL.AppendLine(" WHERE NOT EXISTS (SELECT * FROM RicettexAgenda WHERE Ricetta_Operazione_Cod = ro.Ricetta_Operazione_Cod AND Ricetta_SuperUser = '" + objParametri.PivaSuperUser + "' ) ")

            ' escludo eventuali ricette che arrivano dall'app (considero solo quelle eseguite)
            StrSQL.AppendLine(" AND W_Anagrafica_Stati_Cod = " & enum_WWorflow_WAnagraficaStati.Esecuzione_ed_avanzamento_delle_ricette_Eseguita & " ")

            If Not String.IsNullOrEmpty(guid) Then
                StrSQL.AppendLine(" AND ro.APP_Ricetta_Operazione_ID LIKE '%" & Agro_SQL_SaveText(guid) & "%' ")
            End If

            If Not String.IsNullOrEmpty(lav_cod) AndAlso lav_cod <> "0" Then
                'StrSQL.AppendLine(" AND ro.Lav_Cod IN (" & Agro_SQL_Save_Clausola_IN(lav_cod) & ") ")
            End If

            StrSQL.AppendLine(" ORDER BY Ricetta_SuperUser, Ricetta_Cod, Ricetta_Operazione_Cod Asc ")

            '--------------------------------------------------------------------------
            DT = EseguiQuery_Lettura(objParametri, StrSQL.ToString, NomeRoutine)
            '--------------------------------------------------------------------------

            ' filtro su lavorazioni da importare in agenda (fix per attivita miste)
            If Not String.IsNullOrEmpty(lav_cod) AndAlso lav_cod <> "0" Then
                Dim lavorazioni = lav_cod.Split(",")
                For Each row As DataRow In DT.Rows
                    If Not lavorazioni.Contains(row.Item("Lav_Cod")) Then
                        DT.Rows.Clear()
                        Exit For
                    End If
                Next
            End If

        Catch ex As Exception
            MessaggioErrore = ex.Message
            Scrivi_LOG(objParametri, NomeRoutine, MessaggioErrore)
            Throw New Exception("[" & NomeRoutine & "] : " & MessaggioErrore)
        End Try

        Return DT

    End Function


    Public Function Leggi_PianoCampionamentoAPP(ByRef objParametri As AgronicaCoreParametri, Optional ByVal guid As String = "") As DataTable

        Dim NomeRoutine As String = "AgronicaCoreContabDAL.APP_Dati_R.Leggi_PianoCampionamentoAPP()"
        Dim MessaggioErrore As String = ""
        Dim DT As DataTable

        Try

            Dim StrSQL As New StringBuilder
            StrSQL.AppendLine(" SELECT d.ID, d.Piva, a.Id_PDC_Testata, a.PivaOwner ")
            StrSQL.AppendLine(" FROM APP_dati AS d ")
            StrSQL.AppendLine(" INNER JOIN PDC_Testata a ON d.riferimento = a.Id_PDC_Testata AND a.PivaSuperUser = '" + objParametri.PivaSuperUser + "' ")
            StrSQL.AppendLine(" WHERE d.tipo='80' AND d.riferimento<>'' ")

            If Not String.IsNullOrEmpty(guid) Then
                StrSQL.AppendLine(" AND d.ID LIKE '%" & Agro_SQL_SaveText(guid) & "%' ")
            End If

            '--------------------------------------------------------------------------
            DT = EseguiQuery_Lettura(objParametri, StrSQL.ToString, NomeRoutine)
            '--------------------------------------------------------------------------

        Catch ex As Exception
            MessaggioErrore = ex.Message
            Scrivi_LOG(objParametri, NomeRoutine, MessaggioErrore)
            Throw New Exception("[" & NomeRoutine & "] : " & MessaggioErrore)
        End Try

        Return DT

    End Function

    Public Function Leggi_AgendaAPP(ByRef objParametri As AgronicaCoreParametri, Optional ByVal guid As String = "") As DataTable

        Dim NomeRoutine As String = "AgronicaCoreContabDAL.APP_Dati_R.Leggi_AgendaAPP()"
        Dim MessaggioErrore As String = ""
        Dim DT As DataTable

        Try

            Dim StrSQL As New StringBuilder
            StrSQL.AppendLine(" SELECT d.ID, a.Id_Agenda, a.Piva, a.Sa_Cod, a.Lav_Cod, a.Data_Creazione, a.Username_Creazione ")
            StrSQL.AppendLine(" FROM APP_dati AS d ")
            StrSQL.AppendLine(" LEFT JOIN Agenda a ON d.riferimento = a.Id_Agenda ")
            StrSQL.AppendLine(" WHERE d.riferimento <> '' ")

            If Not String.IsNullOrEmpty(guid) Then
                StrSQL.AppendLine(" AND d.ID LIKE '%" & Agro_SQL_SaveText(guid) & "%' ")
            End If

            '--------------------------------------------------------------------------
            DT = EseguiQuery_Lettura(objParametri, StrSQL.ToString, NomeRoutine)
            '--------------------------------------------------------------------------

        Catch ex As Exception
            MessaggioErrore = ex.Message
            Scrivi_LOG(objParametri, NomeRoutine, MessaggioErrore)
            Throw New Exception("[" & NomeRoutine & "] : " & MessaggioErrore)
        End Try

        Return DT

    End Function

    Public Function Leggi_AttivitaAPP(ByRef objParametri As AgronicaCoreParametri, Optional ByVal guid As String = "") As DataTable

        Dim NomeRoutine As String = "AgronicaCoreContabDAL.APP_Dati_R.Leggi_AttivitaAPP()"
        Dim MessaggioErrore As String = ""
        Dim DT As DataTable

        Try

            Dim StrSQL As New StringBuilder
            StrSQL.AppendLine(" SELECT ro.APP_Ricetta_Operazione_ID,ro.Ricetta_SuperUser, ro.Ricetta_Cod, ro.Ricetta_Operazione_Cod, rxa.Id_Agenda, a.Piva, a.Sa_Cod, a.Lav_Cod ")
            StrSQL.AppendLine(" FROM Ricette_Operazioni AS ro ")
            StrSQL.AppendLine(" LEFT JOIN RicettexAgenda AS rxa ON rxa.Ricetta_Operazione_Cod = ro.Ricetta_Operazione_Cod AND ro.Ricetta_SuperUser = '" + objParametri.PivaSuperUser + "' ")
            StrSQL.AppendLine(" LEFT JOIN Agenda a ON rxa.Id_Agenda = a.Id_Agenda ")
            StrSQL.AppendLine(" WHERE ro.APP_Ricetta_Operazione_ID <> '' ")

            If Not String.IsNullOrEmpty(guid) Then
                StrSQL.AppendLine(" AND ro.APP_Ricetta_Operazione_ID LIKE '%" & Agro_SQL_SaveText(guid) & "%' ")
            End If

            '--------------------------------------------------------------------------
            DT = EseguiQuery_Lettura(objParametri, StrSQL.ToString, NomeRoutine)
            '--------------------------------------------------------------------------

        Catch ex As Exception
            MessaggioErrore = ex.Message
            Scrivi_LOG(objParametri, NomeRoutine, MessaggioErrore)
            Throw New Exception("[" & NomeRoutine & "] : " & MessaggioErrore)
        End Try

        Return DT

    End Function

    Public Function Leggi_AttivitaCdGAPP(ByRef objParametri As AgronicaCoreParametri, Optional ByVal guid As String = "") As DataTable

        Dim NomeRoutine As String = "AgronicaCoreContabDAL.APP_Dati_R.Leggi_AttivitaCdGAPP()"
        Dim MessaggioErrore As String = ""
        Dim DT As DataTable

        Try

            Dim StrSQL As New StringBuilder
            StrSQL.AppendLine(" SELECT APP_CDG_Generale_ID, Id_CDG, Id_Agenda, Piva ")
            StrSQL.AppendLine(" FROM CDG_Testata ")
            StrSQL.AppendLine(" WHERE APP_CDG_Generale_ID <> '' And Budget = 0 ")

            If Not String.IsNullOrEmpty(guid) Then
                StrSQL.AppendLine(" AND APP_CDG_Generale_ID LIKE '%" & Agro_SQL_SaveText(guid) & "%' ")
            End If

            '--------------------------------------------------------------------------
            DT = EseguiQuery_Lettura(objParametri, StrSQL.ToString, NomeRoutine)
            '--------------------------------------------------------------------------

        Catch ex As Exception
            MessaggioErrore = ex.Message
            Scrivi_LOG(objParametri, NomeRoutine, MessaggioErrore)
            Throw New Exception("[" & NomeRoutine & "] : " & MessaggioErrore)
        End Try

        Return DT

    End Function

    Public Function Leggi_DocumentiAPP(ByRef objParametri As AgronicaCoreParametri, Optional ByVal guid As String = "") As DataTable

        Dim NomeRoutine As String = "AgronicaCoreContabDAL.APP_Dati_R.Leggi_DocumentiAPP()"
        Dim MessaggioErrore As String = ""
        Dim DT As DataTable

        Try

            Dim StrSQL As New StringBuilder
            StrSQL.AppendLine(" SELECT ID_Elenco ")
            StrSQL.AppendLine(" FROM Alert_Elenco ")
            StrSQL.AppendLine(" WHERE ID_App <> '' AND PivaSuperUser = '" + objParametri.PivaSuperUser + "' ")

            If Not String.IsNullOrEmpty(guid) Then
                StrSQL.AppendLine(" AND ID_App LIKE '%" & Agro_SQL_SaveText(guid) & "%' ")
            End If

            '--------------------------------------------------------------------------
            DT = EseguiQuery_Lettura(objParametri, StrSQL.ToString, NomeRoutine)
            '--------------------------------------------------------------------------

        Catch ex As Exception
            MessaggioErrore = ex.Message
            Scrivi_LOG(objParametri, NomeRoutine, MessaggioErrore)
            Throw New Exception("[" & NomeRoutine & "] : " & MessaggioErrore)
        End Try

        Return DT

    End Function

    Public Function Leggi_DocumentiAttivitaAPP(ByRef objParametri As AgronicaCoreParametri) As DataTable

        Dim NomeRoutine As String = "AgronicaCoreContabDAL.APP_Dati_R.Leggi_DocumentiAttivitaAPP()"
        Dim MessaggioErrore As String = ""
        Dim DT As DataTable

        Try

            Dim StrSQL As New StringBuilder
            StrSQL.AppendLine(" SELECT a.id_alert_entita, ISNULL(d.id_agenda,0) AS id_agenda, ISNULL(c.ricetta_operazione_cod,0) AS ricetta_operazione_cod ")
            StrSQL.AppendLine(" FROM alert_entita a INNER JOIN alert_elenco b ON a.id_alert_entita=b.id_alert_entita ")
            StrSQL.AppendLine(" INNER JOIN ricette_operazioni c ON a.ricetta_operazione_cod=c.ricetta_operazione_cod ")
            StrSQL.AppendLine(" LEFT JOIN ricettexagenda d ON c.ricetta_operazione_cod = d.ricetta_operazione_cod ")
            StrSQL.AppendLine(" WHERE b.ID_App <> '' AND (ISNULL(d.id_agenda,0) <> a.id_agenda OR ISNULL(c.ricetta_operazione_cod,0) <> a.ricetta_operazione_cod) ")

            '--------------------------------------------------------------------------
            DT = EseguiQuery_Lettura(objParametri, StrSQL.ToString, NomeRoutine)
            '--------------------------------------------------------------------------

        Catch ex As Exception
            MessaggioErrore = ex.Message
            Scrivi_LOG(objParametri, NomeRoutine, MessaggioErrore)
            Throw New Exception("[" & NomeRoutine & "] : " & MessaggioErrore)
        End Try

        Return DT

    End Function

    Public Function AppDati_NonInviati(ByVal tipoEsportazione As enum_Esportazioni_Sistema_Cod,
                                       ByVal tipo As String, 'AppHelper.enum_Dati_App
                                       ByVal PivaAmmesse As List(Of String),
                                       ByVal xFiltroAggiuntivo As String,
                                       ByVal xOrderBy As String,
                                       ByRef objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri
                                       ) As DataTable

        Dim nomeRoutine As String = "AgronicaCoreContabDAL.APP_Dati_R.AppDati_NonInviati()"

        Dim messaggioErrore As String = ""
        Dim stb As New StringBuilder
        Dim dt As DataTable

        Try

            stb.Length = 0

            stb.AppendLine(" WITH ")

            stb.AppendLine(" brogliacci_demetra as ( ")
            stb.AppendLine(" SELECT Piva, Codice, Dati, SUBSTRING(  ")
            stb.AppendLine(" riferimento, ")
            stb.AppendLine(" CHARINDEX('|', riferimento)+1,  ")
            stb.AppendLine(" LEN(riferimento) - CHARINDEX('|', riferimento)  ")
            stb.AppendLine(" ) ID_Ricetta  ")
            stb.AppendLine(" FROM APP_Dati (NOLOCK) ")
            stb.AppendLine(" WHERE tipo = '" & Agro_SQL_SaveText(tipo) & "'")
            stb.AppendLine(" AND cancellato = 0 ") 'non cancellate da Demetra (altrimenti sarebbe stato = 1)
            stb.AppendLine(" AND CHARINDEX('|', riferimento) > 0 ") 'associate a un brogliaccio 
            stb.AppendLine(" AND SUBSTRING(riferimento,0,CHARINDEX('|',riferimento,0)) = 0 ") 'ma non ancora ribaltate in agenda
            If PivaAmmesse IsNot Nothing AndAlso PivaAmmesse.Count > 0 Then
                stb.AppendLine(" AND Piva IN (" & Agro_SQL_Save_Clausola_IN(String.Join(",", PivaAmmesse), True) & ") ")
            End If

            stb.AppendLine(" ), ")

            stb.AppendLine(" log_ricette_cancellate AS ( ")
            stb.AppendLine(" SELECT Param3 AS Piva, Param1 as Ricetta_Cod, Chiave as Ricetta_Operazione_Cod, Data_Ora_RegistrazioneLog, Raccoglitore_Cod ")
            stb.AppendLine(" FROM Agronica_Log_Ricette_UltimaOperazione (NOLOCK) ")
            stb.AppendLine(" WHERE 1=1 ")
            stb.AppendLine(" And Tipo = 'Ricette_Operazioni' ")
            stb.AppendLine(" AND UltimaOperazione = 3 ")

            stb.AppendLine(" ), ")

            stb.AppendLine(" log_invio AS ( ")
            stb.AppendLine(" SELECT ID_Ricetta, Max(Id_Log_Invio) id_log_invio ")
            stb.AppendLine(" FROM Agronica_Log_Invio_Ricette (NOLOCK) ")
            stb.AppendLine(" WHERE Tipo_Esportazione = " & Agro_SQL_SaveNum(tipoEsportazione))
            stb.AppendLine(" GROUP BY ID_Ricetta  ")
            stb.AppendLine(" ), ")


            stb.AppendLine(" log_invio_chiamate as (  ")
            stb.AppendLine(" SELECT ID, Esito, Data_Invio ")
            stb.AppendLine(" FROM Agronica_Log_Invio_Chiamate (NOLOCK) ")
            stb.AppendLine(" WHERE Tipo_Esportazione = " & Agro_SQL_SaveNum(tipoEsportazione))
            stb.AppendLine(" ) ")

            stb.AppendLine(" SELECT bd.ID_Ricetta, bd.Codice as CodiceDemetra, bd.Dati, ISNULL(log_invio_chiamate.Esito, '') AS Esito, ISNULL(log_invio_chiamate.ID, '') AS ID_Chiamata, Ricetta_Operazione_Cod, Raccoglitore_Cod ")
            stb.AppendLine(" FROM brogliacci_demetra bd ")
            stb.AppendLine(" INNER JOIN log_ricette_cancellate ")
            stb.AppendLine(" ON (log_ricette_cancellate.Piva = bd.Piva AND log_ricette_cancellate.Ricetta_Cod = bd.ID_Ricetta) ")
            stb.AppendLine(" LEFT OUTER JOIN log_invio ")
            stb.AppendLine(" ON (bd.ID_Ricetta = log_invio.ID_Ricetta)  ")
            stb.AppendLine(" LEFT OUTER JOIN log_invio_chiamate ")
            stb.AppendLine(" ON (log_invio.id_log_invio = log_invio_chiamate.ID) ")
            stb.AppendLine(" WHERE 1=1 ")
            stb.AppendLine(" AND (log_invio_chiamate.Data_Invio IS NULL OR (log_ricette_cancellate.Data_Ora_RegistrazioneLog >= log_invio_chiamate.Data_Invio AND log_invio_chiamate.Esito IN ('OK','BLK')) OR (log_invio_chiamate.Esito = 'KO'))  ")

            If xFiltroAggiuntivo <> "" Then
                stb.AppendLine(" AND " & Agro_SQL_Save_xFiltroAggiuntivo(xFiltroAggiuntivo, , objParametri))
            End If

            If xOrderBy <> "" Then
                stb.AppendLine(" ORDER BY " & Agro_SQL_Save_xOrderBy(xOrderBy, objParametri))
            End If

            '--------------------------------------------------------------------------
            dt = EseguiQuery_Lettura(objParametri, stb.ToString, nomeRoutine)
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

Public Class APP_Dati_W
    Inherits AgronicaCoreDataProvider.DataProvider

    Public Function Aggiorna_DocumentiAttivitaAPP(ByVal ID_Alert_Entita As Integer, ByVal ID_Agenda As Integer, ByVal Ricetta_Operazione_Cod As Integer, ByRef objParametri As AgronicaCoreParametri) As Boolean

        '----- Descrizione
        Dim NomeRoutine As String = "Aggiorna_DocumentiAttivitaAPP()"

        Dim MessaggioErrore As String = ""
        Dim StrSQL As New StringBuilder
        Dim xRisp As Boolean = False
        Dim tipoEntitaCod As Integer

        If ID_Agenda <> 0 Then
            tipoEntitaCod = enum_TipoEntita.OperazioneDiAgenda
        ElseIf Ricetta_Operazione_Cod <> 0 Then
            tipoEntitaCod = enum_TipoEntita.Ricetta
        Else
            Return False
        End If

        Try

            StrSQL.AppendLine(" UPDATE Alert_Entita ")
            StrSQL.AppendLine(" SET ")
            StrSQL.AppendLine("   Username_Modifica = '" & Agro_SQL_SaveText(objParametri.UsernameOperazione) & "' ")
            StrSQL.AppendLine("   ,Data_Modifica= " & Agro_SQL_SaveDateTime(Date.Now) & " ")
            StrSQL.AppendLine("   ,TipoEntita_Cod = " & Agro_SQL_SaveNum(tipoEntitaCod) & " ")
            StrSQL.AppendLine("   ,ID_Agenda = " & Agro_SQL_SaveNum(ID_Agenda) & " ")
            StrSQL.AppendLine("   ,Ricetta_Operazione_Cod = " & Agro_SQL_SaveNum(Ricetta_Operazione_Cod) & " ")
            StrSQL.AppendLine(" WHERE   ID_Alert_Entita = " & Agro_SQL_SaveNum(ID_Alert_Entita) & " ")

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

    Public Function Aggiorna_DatiAPP(
        ByVal ID As String,
        ByVal Tipo As String,
        ByVal Errori As String,
        ByVal Riferimento As String,
        ByVal Cancellato As Boolean,
        ByVal Importazione As Boolean,
        ByRef objParametri As AgronicaCoreParametri,
        Optional ByVal User_Agent As String = ""
    ) As Boolean

        '----- Descrizione
        Dim NomeRoutine As String = "Aggiorna_DatiAPP()"

        Dim MessaggioErrore As String = ""
        Dim StrSQL As New StringBuilder
        Dim xRisp As Boolean = False

        Try
            '---------------------------------------------
            Dim xErr As String = If(Errori.Length > 2000, Errori.Substring(0, 2000), Errori)

            StrSQL.AppendLine(" UPDATE APP_Dati ")
            StrSQL.AppendLine(" SET ")
            StrSQL.AppendLine("   Username_Modifica = '" & Agro_SQL_SaveText(objParametri.UsernameOperazione) & "' ")
            StrSQL.AppendLine("   ,Data_Modifica= " & Agro_SQL_SaveDateTime(Date.Now) & " ")
            If Importazione Then
                StrSQL.AppendLine("   ,Importato_Data = " & Agro_SQL_SaveDateTime(Date.Now) & " ")
            Else
                StrSQL.AppendLine("   ,Importato_Data = NULL ")
            End If
            StrSQL.AppendLine("   ,Importato_Errore = '" & Agro_SQL_SaveText(xErr) & "' ")
            If Not String.IsNullOrEmpty(User_Agent) Then
                StrSQL.AppendLine("   ,User_Agent = '" & Agro_SQL_SaveText(User_Agent) & "' ")
            End If
            StrSQL.AppendLine("   ,Riferimento='" & Agro_SQL_SaveText(Riferimento) & "' ")
            StrSQL.AppendLine("   ,cancellato=" & If(Cancellato, "1", "0") & " ")

            StrSQL.AppendLine("   ,inviato = 0 ")
            StrSQL.AppendLine("   ,Data_Invio_Sistema_Logging = NULL ")

            StrSQL.AppendLine(" WHERE   ID = '" & Agro_SQL_SaveText(ID) & "' ")
            If Not String.IsNullOrEmpty(Tipo) Then
                StrSQL.AppendLine(" AND Tipo = '" & Agro_SQL_SaveText(Tipo) & "' ")
            End If

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

    Public Function Aggiorna_Riferimento_DatiAPP(
        ByVal ID As String,
        ByVal Riferimento As String,
        ByVal objParametri As AgronicaCoreParametri
    ) As Boolean

        '----- Descrizione
        Dim NomeRoutine As String = "Aggiorna_Riferimento_DatiAPP()"

        Dim MessaggioErrore As String = ""
        Dim StrSQL As New StringBuilder
        Dim xRisp As Boolean = False

        Try
            '---------------------------------------------

            StrSQL.AppendLine(" UPDATE APP_Dati ")
            StrSQL.AppendLine(" SET ")
            StrSQL.AppendLine("   Username_Modifica = '" & Agro_SQL_SaveText(objParametri.UsernameOperazione) & "' ")
            StrSQL.AppendLine("   ,Data_Modifica= " & Agro_SQL_SaveDateTime(Date.Now) & " ")
            StrSQL.AppendLine("   ,Riferimento='" & Agro_SQL_SaveText(Riferimento) & "' ")
            StrSQL.AppendLine(" WHERE ID = '" & Agro_SQL_SaveText(ID) & "' ")

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

    Public Function Aggiorna_Cancellato_DatiAPP(
        ByVal ID As String,
        ByVal Cancellato As Boolean,
        ByVal objParametri As AgronicaCoreParametri
    ) As Boolean

        '----- Descrizione
        Dim NomeRoutine As String = "Aggiorna_Cancellato_DatiAPP()"

        Dim MessaggioErrore As String = ""
        Dim StrSQL As New StringBuilder
        Dim xRisp As Boolean = False

        Try
            '---------------------------------------------

            StrSQL.AppendLine(" UPDATE APP_Dati ")
            StrSQL.AppendLine(" SET ")
            StrSQL.AppendLine("   Username_Modifica = '" & Agro_SQL_SaveText(objParametri.UsernameOperazione) & "' ")
            StrSQL.AppendLine("   ,Data_Modifica= " & Agro_SQL_SaveDateTime(Date.Now) & " ")
            StrSQL.AppendLine("   ,Cancellato=" & If(Cancellato, Agro_SQL_SaveNum(1), Agro_SQL_SaveNum(0)))
            StrSQL.AppendLine(" WHERE ID = '" & Agro_SQL_SaveText(ID) & "' ")

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

    Public Function Cancella_DatiAPP(ByVal ID As String, ByVal Tipo As String, ByRef objParametri As AgronicaCoreParametri) As Boolean

        Dim NomeRoutine As String = "AgronicaCoreContabDAL.APP_Dati_W.Cancella_DatiAPP()"
        Dim MessaggioErrore As String = ""
        Dim StrSQL As New StringBuilder
        Dim xRisp As Boolean = False

        Try
            If Not String.IsNullOrEmpty(ID) Then
                StrSQL.AppendLine(" DELETE FROM APP_Dati ")
                StrSQL.AppendLine(" WHERE ID LIKE '%" & Agro_SQL_SaveText(ID) & "%' ")
                If Not String.IsNullOrEmpty(Tipo) Then
                    StrSQL.AppendLine(" AND Tipo = '" & Agro_SQL_SaveText(Tipo) & "' ")
                End If
                xRisp = EseguiQuery_Scrittura(objParametri, StrSQL.ToString, NomeRoutine)
            End If
        Catch ex As Exception
            MessaggioErrore = ex.Message
            Scrivi_LOG(objParametri, NomeRoutine, MessaggioErrore)
            Throw New Exception("[" & NomeRoutine & "] : " & MessaggioErrore)
        End Try

        Return xRisp

    End Function

    Public Function Scrivi_DatiAPP(
        ByVal ID As String,
        ByVal Tipo As String,
        ByVal Dati As String,
        ByVal Riferimento As String,
        ByVal Cancellato As Boolean,
        ByRef objParametri As AgronicaCoreParametri,
        Optional ByVal Piva As String = "",
        Optional ByVal Codice As String = "",
        Optional ByVal User_Agent As String = "",
        Optional ByVal Data_creazione As DateTime = #2/1/1900#,
        Optional ByVal Data_modifica As DateTime = #2/1/1900#,
        Optional ByVal username_creazione As String = "",
        Optional ByVal username_modifica As String = "",
        Optional ByVal Validita_Inizio As Date = AGRODATAINIZIO,
        Optional ByVal Validita_Fine As Date = AGRODATAFINE
        ) As Boolean

        Const nomeRoutine = "AgronicaCoreContabDAL.APP_Dati_W.Scrivi_DatiAPP()"

        Dim messaggioErrore As String = ""
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

            StrSQL.AppendLine("INSERT INTO APP_Dati( ")
            StrSQL.AppendLine("  ID, Tipo, Piva, Codice, Dati, Riferimento, Cancellato, ")
            If Not String.IsNullOrEmpty(User_Agent) Then
                StrSQL.AppendLine(" User_Agent, ")
            End If
            StrSQL.AppendLine("  Inviato, DataInvio, Data_Creazione, Data_Modifica, UserName_Creazione, UserName_Modifica, Validita_Inizio, Validita_Fine, Data_Invio_Sistema_Logging")
            StrSQL.AppendLine(") ")

            StrSQL.AppendLine("VALUES (")
            StrSQL.AppendLine("  '" & Agro_SQL_SaveText(ID) & "' ")
            StrSQL.AppendLine("  ,'" & Agro_SQL_SaveText(Tipo) & "' ")
            StrSQL.AppendLine("  ,'" & Agro_SQL_SaveText(Piva) & "' ")
            StrSQL.AppendLine("  ,'" & Agro_SQL_SaveText(Codice) & "' ")
            StrSQL.AppendLine("  ,'" & Agro_SQL_SaveText(Dati) & "' ")
            StrSQL.AppendLine("  ,'" & Agro_SQL_SaveText(Riferimento) & "' ")
            StrSQL.AppendLine("  , " & If(Cancellato, "1", "0") & " ")
            If Not String.IsNullOrEmpty(User_Agent) Then
                StrSQL.AppendLine("  ,'" & Agro_SQL_SaveText(User_Agent) & "' ")
            End If
            StrSQL.AppendLine("  , 0 ")
            StrSQL.AppendLine("  , " & Agro_SQL_SaveDateTime(Date.Now))
            StrSQL.AppendLine("	 , " & Agro_SQL_SaveDateTime(Data_creazione) & "  ")
            StrSQL.AppendLine("	 , " & Agro_SQL_SaveDateTime(Data_modifica) & "  ")
            StrSQL.AppendLine("	 ,'" & Agro_SQL_SaveText(username_creazione) & "' ")
            StrSQL.AppendLine("	 ,'" & Agro_SQL_SaveText(username_modifica) & "' ")
            StrSQL.AppendLine("  , " & Agro_SQL_SaveDate(Validita_Inizio) & "  ")
            StrSQL.AppendLine("  , " & Agro_SQL_SaveDate(Validita_Fine) & "  ")
            StrSQL.AppendLine("  ,  NULL ")

            StrSQL.AppendLine(")")

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

    Public Function UpdateMassivo_Inviato_ElasticSearch(ID As List(Of String),
                                                        timestamp As DateTime,
                                                        objParametri As AgronicaCoreParametri
                                                        ) As Boolean

        Dim NomeRoutine As String = "AgronicaCoreContabDAL.APP_Dati.UpdateMassivo_Inviato_ElasticSearch()"
        Dim StrSQL As New System.Text.StringBuilder

        If ID.Count = 0 Then
            Throw New Exception("PARAMETRO ID OBBLIGATORIO")
        End If

        Dim xRisp As Boolean

        Try

            StrSQL.Length = 0

            StrSQL.AppendLine(" UPDATE APP_Dati SET ")
            StrSQL.AppendLine("          Data_Modifica = " & Agro_SQL_SaveDateTime(Date.Now) & " ")
            StrSQL.AppendLine("        , Username_Modifica = '" & Agro_SQL_SaveText(objParametri.UsernameOperazione) & "' ")
            StrSQL.AppendLine("        , Data_Invio_Sistema_Logging = " & Agro_SQL_SaveDateTime(timestamp) & " ")
            StrSQL.AppendLine("        , Inviato = 1")

            StrSQL.AppendLine(" WHERE ID IN (" & Agro_SQL_Save_Clausola_IN(String.Join(",", ID), True) & ") ")

            '--------------------------------------------------------------------------
            xRisp = EseguiQuery_Scrittura(objParametri, StrSQL.ToString, NomeRoutine)
            '--------------------------------------------------------------------------

        Catch ex As Exception
            Scrivi_LOG(objParametri, NomeRoutine, ex.Message)
            Throw New Exception("[" & NomeRoutine & "] : " & ex.Message)
        End Try

        Return xRisp

    End Function
End Class