Imports System.Text
Imports AgronicaCoreDataProvider
Imports AgronicaCoreDataProvider.TipiEnumerativi
Imports AgronicaCoreDataProvider.CostantiPersonalizzate
Imports AgronicaCoreEntityFramework
Imports Newtonsoft.Json

Public Class AgronicaLogAgenda_R
    Inherits AgronicaCoreDataProvider.DataProvider

    Public Function Leggi(ByVal ID As Integer,
                          ByVal SuperUser As String,
                          ByVal Utente As String,
                          ByVal Tipo_Operazione As enum_TipoOperazioneDB,
                          ByVal Id_Agenda As Integer,
                          ByVal Piva As String,
                          ByVal Sa_Cod As Integer,
                          ByVal Lav_Cod As Integer,
                          ByVal Id_Servizio As Integer,
                          ByVal xFiltroAggiuntivo As String,
                          ByVal xOrderBy As String,
                          ByRef objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri
                          ) As DataTable

        '====================================================================================
        'Parametri opzionali :
        '   ID = 0
        '   SuperUser = ""
        '   Utente = ""
        '   Tipo_Operazione = 0
        '   Id_Agenda = 0
        '   Piva = ""
        '   Sa_Cod = 0
        '   Lav_Cod = 0
        '   Id_Servizio = 0
        '====================================================================================

        Dim nomeRoutine As String = "AgronicaCoreContabDAL.AgronicaLogAgenda_R.Leggi()"

        Dim messaggioErrore As String = ""
        Dim stb As New StringBuilder
        Dim dt As DataTable

        Try

            stb.Length = 0

            stb.AppendLine(" SELECT * ")
            stb.AppendLine(" FROM Agronica_Log_Agenda ")
            stb.AppendLine(" WHERE 1=1 ")

            If ID <> 0 Then
                stb.AppendLine(" AND ID = " & Agro_SQL_SaveNum(ID) & " ")
            End If

            If SuperUser <> "" Then
                stb.AppendLine(" AND SuperUser = '" & Agro_SQL_SaveText(SuperUser) & "' ")
            End If

            If Utente <> "" Then
                stb.AppendLine(" AND Utente = '" & Agro_SQL_SaveText(Utente) & "' ")
            End If

            If Tipo_Operazione <> enum_TipoOperazioneDB.Lettura Then
                stb.AppendLine(" AND Tipo_Operazione = " & Agro_SQL_SaveNum(Tipo_Operazione) & " ")
            End If

            If Id_Agenda <> 0 Then
                stb.AppendLine(" AND Id_Agenda = " & Agro_SQL_SaveNum(Id_Agenda) & " ")
            End If

            If Piva <> "" Then
                stb.AppendLine(" AND Piva = '" & Agro_SQL_SaveText(Piva) & "' ")
            End If

            If Sa_Cod <> 0 Then
                stb.AppendLine(" AND Sa_Cod = " & Agro_SQL_SaveNum(Sa_Cod) & " ")
            End If

            If Lav_Cod <> 0 Then
                stb.AppendLine(" AND Lav_Cod = " & Agro_SQL_SaveNum(Lav_Cod) & " ")
            End If

            If Id_Servizio <> 0 Then
                stb.AppendLine(" AND Id_Servizio = " & Agro_SQL_SaveNum(Id_Servizio) & " ")
            End If

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

    Public Function Leggi_UltimaOperazione(
                      ByVal SuperUser As String,
                      ByVal UltimaOperazione As enum_TipoOperazioneDB,
                      ByVal Id_Agenda As Integer,
                      ByVal Piva As String,
                      ByVal Sa_Cod As Integer,
                      ByVal Lav_Cod As Integer,
                      ByVal Id_Servizio As Integer,
                      ByVal xFiltroAggiuntivo As String,
                      ByVal xOrderBy As String,
                      ByRef objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri
                      ) As DataTable

        '====================================================================================
        'Parametri opzionali :
        '   ID = 0
        '   SuperUser = ""
        '   Utente = ""
        '   Tipo_Operazione = 0
        '   Id_Agenda = 0
        '   Piva = ""
        '   Sa_Cod = 0
        '   Lav_Cod = 0
        '   Id_Servizio = 0
        '====================================================================================

        Dim nomeRoutine As String = "AgronicaCoreContabDAL.AgronicaLogAgenda_R.Leggi_UltimaOperazione()"

        Dim messaggioErrore As String = ""
        Dim stb As New StringBuilder
        Dim dt As DataTable

        Try

            stb.Length = 0

            stb.AppendLine(" SELECT * ")
            stb.AppendLine(" FROM Agronica_Log_Agenda_UltimaOperazione ")
            stb.AppendLine(" WHERE 1=1 ")

            If SuperUser <> "" Then
                stb.AppendLine(" AND SuperUser = '" & Agro_SQL_SaveText(SuperUser) & "' ")
            End If

            If UltimaOperazione <> enum_TipoOperazioneDB.Lettura Then
                stb.AppendLine(" AND UltimaOperazione = " & Agro_SQL_SaveNum(UltimaOperazione) & " ")
            End If

            If Id_Agenda <> 0 Then
                stb.AppendLine(" AND Id_Agenda = " & Agro_SQL_SaveNum(Id_Agenda) & " ")
            End If

            If Piva <> "" Then
                stb.AppendLine(" AND Piva = '" & Agro_SQL_SaveText(Piva) & "' ")
            End If

            If Sa_Cod <> 0 Then
                stb.AppendLine(" AND Sa_Cod = " & Agro_SQL_SaveNum(Sa_Cod) & " ")
            End If

            If Lav_Cod <> 0 Then
                stb.AppendLine(" AND Lav_Cod = " & Agro_SQL_SaveNum(Lav_Cod) & " ")
            End If

            If Id_Servizio <> 0 Then
                stb.AppendLine(" AND Id_Servizio = " & Agro_SQL_SaveNum(Id_Servizio) & " ")
            End If

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

    Public Function AgendeDaInviare_Demetra(ByVal tipoEsportazione As enum_Esportazioni_Sistema_Cod,
                                          ByVal tipo As String, 'AppHelper.enum_Dati_App
                                          ByVal PivaAmmesse As List(Of String),
                                          ByVal OperazioniAmmesse As List(Of Integer),
                                          ByVal xFiltroAggiuntivo As String,
                                          ByVal xOrderBy As String,
                                          ByRef objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri
                                          ) As DataTable

        Dim nomeRoutine As String = "AgronicaCoreContabDAL.AgronicaLogAgenda_R.AgendeDaInviare()"

        Dim messaggioErrore As String = ""
        Dim stb As New StringBuilder
        Dim dt As DataTable

        Try

            stb.Length = 0

            stb.AppendLine(" WITH ")

            stb.AppendLine(" log_agenda AS ( ")
            stb.AppendLine(" SELECT Piva, Id_Agenda, Data_Ora_RegistrazioneLog, UltimaOperazione, Lav_Cod, Raccoglitore_Cod  ")
            stb.AppendLine(" FROM Agronica_Log_Agenda_UltimaOperazione (NOLOCK)")
            stb.AppendLine(" WHERE 1 = 1 ")

            If OperazioniAmmesse IsNot Nothing AndAlso OperazioniAmmesse.Count > 0 Then
                stb.AppendLine(" AND Lav_Cod IN (" & Agro_SQL_Save_Clausola_IN(String.Join(",", OperazioniAmmesse), False) & ") ")
            End If

            If PivaAmmesse IsNot Nothing AndAlso PivaAmmesse.Count > 0 Then
                stb.AppendLine(" AND Piva IN (" & Agro_SQL_Save_Clausola_IN(String.Join(",", PivaAmmesse), True) & ") ")
            End If

            stb.AppendLine(" ), ")

            stb.AppendLine(" agendeDemetra as ( ")
            stb.AppendLine(" SELECT Piva, Codice, SUBSTRING(riferimento,0,CHARINDEX('|',riferimento,0)) ID_Agenda ")
            stb.AppendLine(" FROM APP_Dati (NOLOCK)")
            stb.AppendLine(" WHERE tipo = '" & Agro_SQL_SaveText(tipo) & "'")
            stb.AppendLine(" AND SUBSTRING(riferimento, 0, CHARINDEX('|',riferimento,0)) > 0 ")
            stb.AppendLine(" ), ")

            stb.AppendLine(" log_invio_agenda AS ( ")
            stb.AppendLine(" SELECT ID_Agenda, Max(Id_Log_Invio) id_log_invio ")
            stb.AppendLine(" FROM Agronica_Log_Invio_Agenda (NOLOCK)")
            stb.AppendLine(" WHERE Tipo_Esportazione = " & Agro_SQL_SaveNum(tipoEsportazione))
            stb.AppendLine(" GROUP BY ID_Agenda ")
            stb.AppendLine(" ), ")

            stb.AppendLine(" log_invio_chiamate  as ( ")
            stb.AppendLine(" SELECT ID, Esito, Data_Invio ")
            stb.AppendLine(" FROM Agronica_Log_Invio_Chiamate (NOLOCK)")
            stb.AppendLine(" WHERE Tipo_Esportazione = " & Agro_SQL_SaveNum(tipoEsportazione))
            stb.AppendLine(" ) ")

            stb.AppendLine(" Select log_agenda.Piva, log_agenda.Id_Agenda, log_agenda.Lav_Cod, UltimaOperazione, ISNULL(log_invio_chiamate.Esito, '') AS Esito, ISNULL(log_invio_chiamate.ID, 0) AS ID_Chiamata, ISNULL(agendeDemetra.Codice,'') As CodiceDemetra, log_agenda.Raccoglitore_Cod  ")
            stb.AppendLine(" FROM log_agenda ")
            stb.AppendLine(" LEFT OUTER JOIN log_invio_agenda ")
            stb.AppendLine(" ON (log_agenda.Id_Agenda = log_invio_agenda.ID_Agenda) ")
            stb.AppendLine(" LEFT OUTER JOIN log_invio_chiamate ")
            stb.AppendLine(" ON (log_invio_agenda.id_log_invio = log_invio_chiamate.ID) ")
            stb.AppendLine(" LEFT OUTER JOIN agendeDemetra ")
            stb.AppendLine(" ON (log_agenda.Id_Agenda = agendeDemetra.ID_Agenda) ")

            stb.AppendLine(" WHERE 1 = 1 ")

            stb.AppendLine(" AND (log_invio_chiamate.Data_Invio Is NULL OR (log_agenda.Data_Ora_RegistrazioneLog >= log_invio_chiamate.Data_Invio And log_invio_chiamate.Esito IN ('OK','BLK')) OR (log_invio_chiamate.Esito = 'KO')) ")

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

    Public Function VisiteDaInviare_CAI(ByVal lavCod As Integer,
                                        ByVal tipoEsportazione As enum_Esportazioni_Sistema_Cod,
                                        ByVal PivaAmmesse As List(Of String),
                                        ByVal PivaEscluse As List(Of String),
                                        ByVal Retry_Status As List(Of String),
                                        ByVal xFiltroAggiuntivo As String,
                                        ByVal xOrderBy As String,
                                        ByRef objParametri As AgronicaCoreParametri) As DataTable
        Dim nomeRoutine As String = "AgronicaCoreContabDAL.AgronicaLogAgenda_R.VisiteDaInviare_CAI()"

        Dim messaggioErrore As String = ""
        Dim stb As New StringBuilder
        Dim dt As DataTable

        Try

            stb.Length = 0
            stb.AppendLine("WITH ")

            'Tabella Agronica_Log_Agenda_UltimaOperazione
            stb.AppendLine("ultimoLogAgenda AS ( ")
            stb.AppendLine("	  SELECT Piva, Id_Agenda, Data_Ora_RegistrazioneLog, UltimaOperazione, Lav_Cod, Raccoglitore_Cod ")
            stb.AppendLine("	  FROM Agronica_Log_Agenda_UltimaOperazione (NOLOCK) ")
            stb.AppendLine("	  WHERE 1 = 1 ")

            If lavCod <> 0 Then
                stb.AppendLine(" AND Lav_Cod = " & Agro_SQL_SaveNum(lavCod) & " ")
            End If

            If PivaAmmesse IsNot Nothing AndAlso PivaAmmesse.Count > 0 Then stb.
                AppendLine(" AND Piva IN (" & Agro_SQL_Save_Clausola_IN(String.Join(",", PivaAmmesse), True) & ") ")

            If PivaEscluse IsNot Nothing AndAlso PivaEscluse.Count > 0 Then stb.
                AppendLine(" AND Piva NOT IN (" & Agro_SQL_Save_Clausola_IN(String.Join(",", PivaEscluse), True) & ") ")

            stb.AppendLine("), ")

            'Tabella Agronica_Log_Invio_Agenda
            stb.AppendLine("log_invio_agenda AS ( ")
            stb.AppendLine("	  SELECT ID_Agenda, MAX(Id_Log_Invio) id_log_invio ")
            stb.AppendLine("	  FROM Agronica_Log_Invio_Agenda (NOLOCK) ")
            stb.AppendLine("    WHERE Tipo_Esportazione = " & Agro_SQL_SaveNum(tipoEsportazione) & " ")
            stb.AppendLine("    GROUP BY ID_Agenda ")
            stb.AppendLine("), ")

            'Tabella Agronica_Log_Invio_Chiamate
            stb.AppendLine("log_invio_chiamate  AS ( ")
            stb.AppendLine("    SELECT ID, Esito, Data_Invio ")
            stb.AppendLine("    FROM Agronica_Log_Invio_Chiamate (NOLOCK) ")
            stb.AppendLine("    WHERE Tipo_Esportazione = " & Agro_SQL_SaveNum(tipoEsportazione) & " ")
            stb.AppendLine(") ")

            stb.AppendLine("SELECT ultimoLogAgenda.Piva,  ")
            stb.AppendLine("    ultimoLogAgenda.Id_Agenda, ")
            stb.AppendLine("    ultimoLogAgenda.Lav_Cod, ")
            stb.AppendLine("    ultimoLogAgenda.UltimaOperazione, ")
            stb.AppendLine("    ISNULL(log_invio_chiamate.Esito, '') AS Esito, ")
            stb.AppendLine("    ISNULL(log_invio_chiamate.ID, 0) AS ID_Chiamata, ")
            stb.AppendLine("    ultimoLogAgenda.Raccoglitore_Cod ")
            stb.AppendLine("FROM ultimoLogAgenda ")
            stb.AppendLine("LEFT OUTER JOIN log_invio_agenda ")
            stb.AppendLine("    ON ultimoLogAgenda.Id_Agenda = log_invio_agenda.ID_Agenda ")
            stb.AppendLine("LEFT OUTER JOIN log_invio_chiamate ")
            stb.AppendLine("    ON log_invio_agenda.id_log_invio = log_invio_chiamate.ID ")
            stb.AppendLine("WHERE 1 = 1 ")
            stb.AppendLine("    AND (log_invio_chiamate.Data_Invio IS NULL ")
            stb.AppendLine("    OR (ultimoLogAgenda.Data_Ora_RegistrazioneLog >= log_invio_chiamate.Data_Invio ")
            stb.AppendLine("        AND log_invio_chiamate.Esito IN ('OK','BLK')) ")

            ' Filtro sugli stati da eseguire nel retry dell'invio (se impostati)
            If Retry_Status.Count > 0 Then
                stb.AppendLine("    OR (log_invio_chiamate.Esito IN (" & Agro_SQL_Save_Clausola_IN(String.Join(", ", Retry_Status), True) & "))) ")
            End If

            If xFiltroAggiuntivo <> "" Then
                stb.AppendLine(" AND " & Agro_SQL_Save_xFiltroAggiuntivo(xFiltroAggiuntivo, , objParametri))
            End If

            If xOrderBy <> "" Then
                stb.AppendLine(" ORDER BY " & Agro_SQL_Save_xOrderBy(xOrderBy, objParametri))
            Else
                stb.AppendLine(" ORDER BY ultimoLogAgenda.Data_Ora_RegistrazioneLog DESC ")
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


'#################################################################
'#################################################################
'#################################################################


Public Class AgronicaLogAgenda_W
    Inherits AgronicaCoreDataProvider.DataProvider

    '============================================================================
    Public Function Scrivi(ByVal Data_Ora_Lavorazione As Date,
                           ByVal Tipo_Operazione As Integer,
                           ByVal Des_Lib As String,
                           ByVal Id_Agenda As Long,
                           ByVal Piva As String,
                           ByVal Sa_Cod As Long,
                           ByVal Lav_Cod As Integer,
                           ByVal Id_Servizio As Integer,
                           ByRef objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri,
                           Optional ByVal agenda As Object = Nothing,
                           Optional ByVal Origine As Integer = -1,
                           Optional ByVal Raccoglitore_Cod As Integer = 0
                           ) As Boolean

        Const nomeRoutine = "AgronicaCoreContabDAL.AgronicaLogAgenda_W.Scrivi()"

        Dim messaggioErrore As String = ""
        Dim strSql As New StringBuilder
        Dim xRisp As Boolean = False

        Dim jsonAgenda = ""
        Try
            If agenda IsNot Nothing Then
                agenda.Id_Agenda = Id_Agenda
                Dim a As New JsonSerializerSettings With {.DateTimeZoneHandling = DateTimeZoneHandling.Local}
                jsonAgenda = JsonConvert.SerializeObject(agenda, a)
            End If
        Catch ex As Exception
            'DT: errori nella produzione del json da loggare non devono compromettere la scrittura del record di log
        End Try

        Try

            '---------------------------------------------
            strSql.Length = 0

            strSql.AppendLine(" INSERT INTO Agronica_Log_Agenda ")
            strSql.AppendLine("             ( SuperUser, Utente, Data_Ora_Lavorazione, Tipo_Operazione, Des_Lib, ")
            strSql.AppendLine("               Id_Agenda, Piva, Sa_Cod, Lav_Cod, Data_Ora_RegistrazioneLog, Id_Servizio, ")
            strSql.AppendLine("               object_data, Origine, Raccoglitore_Cod ) ")

            strSql.AppendLine(" VALUES ( ")
            strSql.AppendLine("          '" & Agro_SQL_SaveText(objParametri.PivaSuperUser) & "'  ")
            strSql.AppendLine("         ,'" & Agro_SQL_SaveText(objParametri.UsernameOperazione) & "' ")
            strSql.AppendLine("         , " & Agro_SQL_SaveDateTime(Data_Ora_Lavorazione) & "  ")
            strSql.AppendLine("         , " & Agro_SQL_SaveNum(Tipo_Operazione) & "  ")
            strSql.AppendLine("         ,'" & Agro_SQL_SaveText(Des_Lib) & "'  ")

            strSql.AppendLine("         , " & Agro_SQL_SaveNum(Id_Agenda) & "  ")
            strSql.AppendLine("         ,'" & Agro_SQL_SaveText(Piva) & "' ")
            strSql.AppendLine("         , " & Agro_SQL_SaveNum(Sa_Cod) & "  ")
            strSql.AppendLine("         , " & Agro_SQL_SaveNum(Lav_Cod) & "  ")
            strSql.AppendLine("         , GETDATE() ")
            strSql.AppendLine("         , " & Agro_SQL_SaveNum(Id_Servizio) & "  ")
            strSql.AppendLine("         ,'" & Agro_SQL_SaveText(jsonAgenda) & "'  ")
            strSql.AppendLine("         , " & Agro_SQL_SaveNum(Origine) & "  ")
            strSql.AppendLine("         , " & Agro_SQL_SaveNum(Raccoglitore_Cod) & "  ")
            strSql.AppendLine("         )")

            '--------------------------------------------------------------------------
            xRisp = EseguiQuery_Scrittura(objParametri, strSql.ToString, nomeRoutine)
            '--------------------------------------------------------------------------

        Catch ex As Exception
            messaggioErrore = ex.Message
            Scrivi_LOG(objParametri, nomeRoutine, messaggioErrore)
            xRisp = False
            Throw New Exception("[" & nomeRoutine & "] : " & messaggioErrore)
        End Try

        Return xRisp

    End Function

    Public Function Scrivi_EF(ByVal Data_Ora_Lavorazione As Date,
                              ByVal Tipo_Operazione As Integer,
                              ByVal Des_Lib As String,
                              ByVal Id_Agenda As Long,
                              ByVal Piva As String,
                              ByVal Sa_Cod As Long,
                              ByVal Lav_Cod As Integer,
                              ByVal Id_Servizio As Integer,
                              ByRef GiasContext As Gias_DeveloperServer_Entities,
                              ByRef objParametriServer As AgronicaCoreDataProvider.AgronicaCoreParametri,
                              Optional ByVal agenda As Object = Nothing,
                              Optional ByVal Origine As Integer = -1,
                              Optional ByVal Raccoglitore_Cod As Integer = 0
                              ) As Boolean

        Const nomeRoutine = "AgronicaCoreContabDAL.AgronicaLogAgenda_W.Scrivi_EF()"

        Dim messaggioErrore As String = ""
        Dim xRisp As Boolean = False

        Dim jsonAgenda = ""
        Try
            If agenda IsNot Nothing Then
                agenda.Id_Agenda = Id_Agenda
                Dim a As New JsonSerializerSettings With {.DateTimeZoneHandling = DateTimeZoneHandling.Local}
                jsonAgenda = JsonConvert.SerializeObject(agenda, a)
            End If
        Catch ex As Exception
            'DT: errori nella produzione del json da loggare non devono compromettere la scrittura del record di log
        End Try

        Try
            Dim log As New AgronicaCoreEntityFramework_POCO.Agronica_Log_Agenda
            log.SuperUser = objParametriServer.PivaSuperUser
            log.Utente = objParametriServer.UsernameOperazione
            log.Data_Ora_Lavorazione = Data_Ora_Lavorazione
            log.Tipo_Operazione = Tipo_Operazione
            log.Des_lib = Des_Lib
            log.Id_Agenda = Id_Agenda
            log.Piva = Piva
            log.Sa_Cod = Sa_Cod
            log.Lav_Cod = Lav_Cod
            log.Id_Servizio = Id_Servizio
            log.Data_Ora_RegistrazioneLog = DateTime.Now
            log.object_data = jsonAgenda
            log.Origine = Origine
            log.Raccoglitore_Cod = Raccoglitore_Cod

            GiasContext.Agronica_Log_Agenda.Add(log)

        Catch ex As Exception
            messaggioErrore = ex.Message
            Scrivi_LOG(objParametriServer, nomeRoutine, messaggioErrore)
            xRisp = False
            Throw New Exception("[" & nomeRoutine & "] : " & messaggioErrore)
        End Try

        Return xRisp

    End Function

End Class
