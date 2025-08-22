Imports System.Data.OleDb
Imports AgronicaCoreDataProvider.UtilityProvider
Imports AgronicaCoreDataProvider.AgronicaCoreParametri

Public Class Utenti_Profili_Read
    Inherits AgronicaCoreDataProvider.DataProvider

    Private Function StringListAggregator(utenti As IEnumerable(Of String), mettiApici As Boolean) As List(Of String)
        Dim prepareFilter = Function(acc, u)
                                If u.index Mod 10 = 0 Then
                                    acc.name &= ", " & vbNewLine & u.name
                                Else
                                    acc.name &= ", " & u.name
                                End If
                                Return acc
                            End Function
        Dim uu = utenti.AsParallel.
            DefaultIfEmpty(String.Empty).
            Select(Function(str) If(mettiApici, "'" & str & "'", str)).
            Select(Function(str, i) New With {.name = str, .index = i}).
            GroupBy(Function(u) u.index \ 10_000).
            Select(Function(batch) batch.Aggregate(prepareFilter).name).
            ToList ' lista di stringhe aggregate
        Return uu
    End Function

    ''' <param name="usernames">Usernames degli utenti interessati. Se la collezione contiene più di 10.000 username, quelli in eccesso verranno ignorati.</param>
    Public Function LeggiMassivo(
        ByVal usernames As IEnumerable(Of String),
        ByVal xFiltroAggiuntivo As String,
        ByVal xOrderBy As String,
        ByRef objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri
    ) As DataTable
        Dim xIn = StringListAggregator(usernames, usernames.Count > 1).FirstOrDefault
        Return LeggiMassivo(xIn, xFiltroAggiuntivo, xOrderBy, objParametri)
    End Function

    ''' <param name="usernames">Usernames degli utenti interessati già formattati per essere inseriti nella clausola IN.
    ''' es. <tt>'user1', 'user2', 'user3', ...</tt></param>
    Public Function LeggiMassivo(
        ByVal usernames As String,
        ByVal xFiltroAggiuntivo As String,
        ByVal xOrderBy As String,
        ByRef objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri
    ) As DataTable
        Dim NomeRoutine As String = "AgronicaCoreUtentiDAL.Utenti_Profili_Read.LeggiMassivo()"
        Dim StrSQL As New System.Text.StringBuilder With {.Length = 0}
        Dim DT As DataTable
        Try
            StrSQL.AppendLine(" SELECT * ")
            StrSQL.AppendLine(" FROM    Utenti_Profili (NOLOCK) ")
            StrSQL.AppendLine(" WHERE 1=1 ")
            If usernames.Contains(",") Then
                Dim conApici = usernames.Contains("',")
                StrSQL.AppendLine(" AND (Utente in ( " & Agro_SQL_Save_Clausola_IN(usernames, valoriStringa:=Not conApici, creaParametriSql:=Not conApici) & " )) ")
            Else
                usernames = usernames.Replace("'", "")
                StrSQL.AppendLine(" AND (Utente = '" & Agro_SQL_SaveText(usernames) & "' ) ")
            End If

            If objParametri.SuperUserUsername <> "" Then
                StrSQL.AppendLine(" AND (Utente_Profilo = '" & Agro_SQL_SaveText(objParametri.SuperUserUsername) & "') ")
            End If
            If xFiltroAggiuntivo <> "" Then
                StrSQL.AppendLine(" AND (" & Agro_SQL_Save_xFiltroAggiuntivo(xFiltroAggiuntivo, , objParametri) & ") ")
            End If

            If xOrderBy <> "" Then
                StrSQL.AppendLine(" ORDER BY " & Agro_SQL_Save_xOrderBy(xOrderBy, objParametri))
            End If

            DT = EseguiQuery_Lettura(objParametri, StrSQL.ToString, NomeRoutine)
        Catch ex As Exception
            Scrivi_LOG(objParametri, NomeRoutine, ex.Message)
            Throw New Exception("[" & NomeRoutine & "] : " & ex.Message)
        End Try
        Return DT
    End Function

    '##############################################################################################
    Public Function Leggi(ByVal Username_Utente As String,
                          ByVal Id_Servizio As Int32,
                                ByVal xSelezioneVariabile As AgronicaCoreDataProvider.AgronicaCoreParametri.enumSelezioneVariabile,
                                ByVal xFiltroAggiuntivo As String,
                                ByVal xOrderBy As String,
                                ByRef objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri
                                ) As DataTable

        Dim NomeRoutine As String = "AgronicaCoreUtentiDAL.Utenti_Profili_Read.Leggi()"

        '====================================================================================
        'Parametri opzionali :
        '   Profilo_SuperUser
        '   Username_Utente
        '   Id_Servizio
        '   FiltroAggiuntivo
        '====================================================================================

        Dim MessaggioErrore As String = ""
        Dim StrSQL As New System.Text.StringBuilder
        Dim DT As DataTable

        Try
            '---------------------------------------------
            Select Case xSelezioneVariabile

                Case AgronicaCoreDataProvider.AgronicaCoreParametri.enumSelezioneVariabile.Selezione_TabellaDatiMinimi

                    StrSQL.Length = 0
                    StrSQL.Append(" SELECT * ")
                    StrSQL.Append(" FROM    Utenti_Profili (NOLOCK) ")
                    StrSQL.Append(" WHERE 1=1 ")

                    If objParametri.SuperUserUsername <> "" Then
                        StrSQL.Append(" AND (Utente_Profilo = '" & Agro_SQL_SaveText(objParametri.SuperUserUsername) & "') ")
                    End If

                    If Username_Utente <> "" Then
                        StrSQL.Append(" AND (Utente = '" & Agro_SQL_SaveText(Username_Utente) & "') ")
                    End If

                    If Id_Servizio <> 0 Then
                        StrSQL.Append(" AND (Id_Servizio = " & Agro_SQL_SaveNum(Id_Servizio) & ") ")
                    End If


                    If xFiltroAggiuntivo <> "" Then
                        StrSQL.Append(" AND " & Agro_SQL_Save_xFiltroAggiuntivo(xFiltroAggiuntivo, , objParametri))
                    End If
                    '--------------------------------------------------------------------------
                    Select Case objParametri.FlagVisibilita
                        Case AgronicaCoreDataProvider.AgronicaCoreParametri.enumVisibilita.Visibilita_SoloNonCancellati
                            StrSQL.Append(" AND   Inviato >=0 ")
                        Case AgronicaCoreDataProvider.AgronicaCoreParametri.enumVisibilita.Visibilita_SoloCancellati
                            StrSQL.Append(" AND   Inviato =-1 ")
                        Case AgronicaCoreDataProvider.AgronicaCoreParametri.enumVisibilita.Visibilita_Tutti
                            '...................................
                        Case Else
                            Throw New Exception("Parametro non corretto nella query (FlagVisibilita)")
                    End Select
                    '--------------------------------------------------------------------------
                    If xOrderBy <> "" Then
                        StrSQL.Append(" ORDER BY " & Agro_SQL_Save_xOrderBy(xOrderBy, objParametri))
                    End If


                Case AgronicaCoreDataProvider.AgronicaCoreParametri.enumSelezioneVariabile.Selezione_TabellaCompleta
                    StrSQL.Length = 0
                    StrSQL.Append(" SELECT * ")
                    StrSQL.Append(" FROM    Utenti_Profili (NOLOCK) ")
                    StrSQL.Append(" WHERE 1=1 ")

                    If objParametri.SuperUserUsername <> "" Then
                        StrSQL.Append(" AND (Utente_Profilo = '" & Agro_SQL_SaveText(objParametri.SuperUserUsername) & "') ")
                    End If

                    If Username_Utente <> "" Then
                        StrSQL.Append(" AND (Utente = '" & Agro_SQL_SaveText(Username_Utente) & "') ")
                    End If

                    If Id_Servizio <> 0 Then
                        StrSQL.Append(" AND (Id_Servizio = " & Agro_SQL_SaveNum(Id_Servizio) & ") ")
                    End If


                    If xFiltroAggiuntivo <> "" Then
                        StrSQL.Append(" AND " & Agro_SQL_Save_xFiltroAggiuntivo(xFiltroAggiuntivo, , objParametri))
                    End If
                    '--------------------------------------------------------------------------
                    Select Case objParametri.FlagVisibilita
                        Case AgronicaCoreDataProvider.AgronicaCoreParametri.enumVisibilita.Visibilita_SoloNonCancellati
                            StrSQL.Append(" AND   Inviato >=0 ")
                        Case AgronicaCoreDataProvider.AgronicaCoreParametri.enumVisibilita.Visibilita_SoloCancellati
                            StrSQL.Append(" AND   Inviato =-1 ")
                        Case AgronicaCoreDataProvider.AgronicaCoreParametri.enumVisibilita.Visibilita_Tutti
                            '...................................
                        Case Else
                            Throw New Exception("Parametro non corretto nella query (FlagVisibilita)")
                    End Select
                    '--------------------------------------------------------------------------
                    If xOrderBy <> "" Then
                        StrSQL.Append(" ORDER BY " & Agro_SQL_Save_xOrderBy(xOrderBy, objParametri))
                    End If

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


    '##############################################################################################
    Public Function Leggi_2(ByVal Username_Utente As String,
                                ByVal Id_Servizio As Int32,
                                ByVal xSelezioneVariabile As AgronicaCoreDataProvider.AgronicaCoreParametri.enumSelezioneVariabile,
                                ByVal xFiltroAggiuntivo As String,
                                ByVal xOrderBy As String,
                                ByRef objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri
                                ) As DataTable

        Dim NomeRoutine As String = "AgronicaCoreUtentiDAL.Utenti_Profili_Read.Leggi()"

        '====================================================================================
        'Parametri opzionali :
        '   Profilo_SuperUser
        '   Username_Utente
        '   Id_Servizio
        '   FiltroAggiuntivo
        '====================================================================================

        Dim MessaggioErrore As String = ""
        Dim StrSQL As New System.Text.StringBuilder
        Dim DT As DataTable

        Try
            '---------------------------------------------
            Select Case xSelezioneVariabile

                Case AgronicaCoreDataProvider.AgronicaCoreParametri.enumSelezioneVariabile.Selezione_TabellaDatiMinimi

                    StrSQL.Length = 0
                    StrSQL.Append(" SELECT * ")
                    StrSQL.Append(" FROM    Utenti_Profili ")
                    StrSQL.Append(" WHERE 1=1 ")

                    If Username_Utente <> "" Then
                        StrSQL.Append(" AND Utente = '" & Agro_SQL_SaveText(Username_Utente) & "' ")
                    End If

                    If Id_Servizio <> 0 Then
                        StrSQL.Append(" AND Id_Servizio = " & Agro_SQL_SaveNum(Id_Servizio) & " ")
                    End If


                    If xFiltroAggiuntivo <> "" Then
                        StrSQL.Append(" AND " & Agro_SQL_Save_xFiltroAggiuntivo(xFiltroAggiuntivo, , objParametri))
                    End If

                    If xOrderBy <> "" Then
                        StrSQL.Append(" ORDER BY " & Agro_SQL_Save_xOrderBy(xOrderBy, objParametri))
                    End If

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

    '#############################################################
    Public Function Leggi_FiltroUtenteSQL(ByVal Username_Utente As String,
                                            ByVal Id_Servizio As Int32,
                                                ByVal xFiltroAggiuntivo As String,
                                                ByVal xOrderBy As String,
                                                ByRef objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri
                                                ) As String

        Dim NomeRoutine As String = "AgronicaCoreUtentiDAL.Utenti_Profili_Read.Leggi_FiltroUtenteSQL()"

        '====================================================================================
        'Parametri opzionali :
        '   Profilo_SuperUser
        '   Username_Utente
        '   Id_Servizio
        '====================================================================================

        Dim MessaggioErrore As String = ""
        Dim StrSQL As New System.Text.StringBuilder
        Dim DT As DataTable

        Try
            '---------------------------------------------
            DT = Leggi(Username_Utente,
                       Id_Servizio,
                       enumSelezioneVariabile.Selezione_TabellaCompleta,
                       xFiltroAggiuntivo,
                       xOrderBy,
                       objParametri)

            If Not IsNothing(DT) Then

                If DT.Rows.Count <> 0 Then
                    Return DT.Rows(0).Item("Descrizione_2")
                Else
                    Return ""
                End If
            Else
                Return ""
            End If

        Catch ex As Exception
            MessaggioErrore = ex.Message
            Scrivi_LOG(objParametri, NomeRoutine, MessaggioErrore)
            DT = Nothing
            Throw New Exception("[" & NomeRoutine & "] : " & MessaggioErrore)
        End Try


    End Function

    '#############################################################
    Public Function Is_UtenteGiasLan(ByVal xFiltroAggiuntivo As String,
                                        ByRef objParametri_Utenti As AgronicaCoreDataProvider.AgronicaCoreParametri
                                        ) As Boolean

        Dim NomeRoutine As String = "AgronicaCoreUtentiDAL.Utenti_Profili_Read.Is_UtenteGiasLan()"

        '====================================================================================
        'Parametri opzionali :
        '====================================================================================

        Dim MessaggioErrore As String = ""
        Dim StrSQL As New System.Text.StringBuilder
        Dim DT As DataTable
        Dim Flag_GiasLan As Boolean = False

        Try

            '---------------------------------------------
            DT = Leggi(objParametri_Utenti.UtenteUsername,
                       8,
                       enumSelezioneVariabile.Selezione_TabellaDatiMinimi,
                       xFiltroAggiuntivo,
                       "",
                       objParametri_Utenti)

            If Not IsNothing(DT) AndAlso DT.Rows.Count > 0 Then
                Flag_GiasLan = True
            End If

        Catch ex As Exception
            MessaggioErrore = ex.Message
            Scrivi_LOG(objParametri_Utenti, NomeRoutine, MessaggioErrore)
            DT = Nothing
            Throw New Exception("[" & NomeRoutine & "] : " & MessaggioErrore)
        End Try

        Return Flag_GiasLan


    End Function

    '#############################################################
    Public Function LeggiImpreseFiltroUtenteSQL(ByVal Username_Utente As String,
                                            ByVal Id_Servizio As Int32,
                                                ByVal xFiltroAggiuntivo As String,
                                                ByVal xOrderBy As String,
                                                ByRef objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri
                                                ) As String

        Dim NomeRoutine As String = "AgronicaCoreUtentiDAL.Utenti_Profili_Read.Leggi_FiltroUtenteSQL()"

        '====================================================================================
        'Parametri opzionali :
        '   Profilo_SuperUser
        '   Username_Utente
        '   Id_Servizio
        '====================================================================================

        Dim MessaggioErrore As String = ""
        Dim StrSQL As New System.Text.StringBuilder
        Dim DT As DataTable

        Try
            '---------------------------------------------
            DT = Leggi(Username_Utente,
                       Id_Servizio,
                       enumSelezioneVariabile.Selezione_TabellaCompleta,
                       xFiltroAggiuntivo,
                       xOrderBy,
                       objParametri)

            If Not IsNothing(DT) Then

                If DT.Rows.Count <> 0 Then
                    Return DT.Rows(0).Item("Descrizione_2")
                Else
                    Return ""
                End If
            Else
                Return ""
            End If

        Catch ex As Exception
            MessaggioErrore = ex.Message
            Scrivi_LOG(objParametri, NomeRoutine, MessaggioErrore)
            DT = Nothing
            Throw New Exception("[" & NomeRoutine & "] : " & MessaggioErrore)
        End Try


    End Function

    ''' <summary>
    ''' Legge le differenti visibilità basandosi su Descrizione_1. Considera solo i record con Id_Servizio = 5 (GiasOnline)
    ''' </summary>
    Public Function GetDistinctVisibility(usernames As IEnumerable(Of String), objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri) As DataTable
        Dim NomeRoutine As String = "AnagrafeCoreUtentiDAL.Utenti_Profili_Write.GetDistinctVisibility()"
        Dim xRisp As DataTable
        Dim StrSQL As New Text.StringBuilder With {.Length = 0}
        Dim xIn = StringListAggregator(usernames, True).FirstOrDefault
        Try
            StrSQL.AppendLine(" SELECT COUNT(DISTINCT Descrizione_1), Descrizione_1 ")
            StrSQL.AppendLine(" FROM Utenti_Profili (NOLOCK) ")
            StrSQL.AppendLine(" WHERE Id_Servizio = 5 ")
            StrSQL.AppendLine("     AND Utente in ( " & xIn & " ) ")
            StrSQL.AppendLine(" GROUP BY Descrizione_1 ")
            '--------------------------------------------------------------------------
            xRisp = EseguiQuery_Lettura(objParametri, StrSQL.ToString, NomeRoutine)
            '--------------------------------------------------------------------------
        Catch ex As Exception
            Scrivi_LOG(objParametri, NomeRoutine, ex.Message)
            Throw New Exception("[" & NomeRoutine & "] : " & ex.Message)
        End Try
        Return xRisp
    End Function

End Class


Public Class Utenti_Profili_Write
    Inherits AgronicaCoreDataProvider.DataProvider

    Private Function StringListAggregator(utenti As IEnumerable(Of String), mettiApici As Boolean) As List(Of String)
        Dim prepareFilter = Function(acc, u)
                                If u.index Mod 10 = 0 Then
                                    acc.name &= ", " & vbNewLine & u.name
                                Else
                                    acc.name &= ", " & u.name
                                End If
                                Return acc
                            End Function
        Dim uu = utenti.AsParallel.
            DefaultIfEmpty(String.Empty).
            Select(Function(str) If(mettiApici, "'" & str & "'", str)).
            Select(Function(str, i) New With {.name = str, .index = i}).
            GroupBy(Function(u) u.index \ 10_000).
            Select(Function(batch) batch.Aggregate(prepareFilter).name).
            ToList ' lista di stringhe aggregate
        Return uu
    End Function

    '##############################################################################################
    Public Function Scrivi(
                            ByVal Utente As String,
                            ByVal Id_Servizio As Int32,
                            ByVal Descrizione_1 As String,
                            ByVal Descrizione_2 As String,
                            ByVal Codice_1 As Int32,
                            ByVal Codice_2 As Int32,
                                ByVal Validita_Inizio As Date,
                                ByVal Validita_Fine As Date,
                                    ByRef objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri
                                    ) As Boolean

        Dim NomeRoutine As String = "AnagrafeCoreUtentiDAL.Utenti_Profili_Write.Scrivi()"

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
            StrSQL.Append("INSERT INTO Utenti_Profili(       ")
            StrSQL.Append("                    Utente, Utente_Profilo, Id_Servizio,  ")
            StrSQL.Append("                    Descrizione_1, Descrizione_2, Codice_1, Codice_2,  ")
            StrSQL.Append("                    Inviato,             DataInvio, ")
            StrSQL.Append("                    Data_Creazione,      Data_Modifica, ")
            StrSQL.Append("                    UserName_Creazione,  UserName_Modifica, ")
            StrSQL.Append("                    Validita_Inizio,     Validita_Fine ")
            StrSQL.Append("                    ) ")
            StrSQL.Append("VALUES (")
            StrSQL.Append("          '" & Agro_SQL_SaveText(Utente) & "' ")
            StrSQL.Append("         ,'" & Agro_SQL_SaveText(objParametri.SuperUserUsername) & "' ")
            StrSQL.Append("         , " & Agro_SQL_SaveNum(Id_Servizio) & " ")
            StrSQL.Append("         ,'" & Agro_SQL_SaveText(Descrizione_1) & "' ")
            StrSQL.Append("         ,'" & Agro_SQL_SaveText(Descrizione_2) & "' ")
            StrSQL.Append("         , " & Agro_SQL_SaveNum(Codice_1) & " ")
            StrSQL.Append("         , " & Agro_SQL_SaveNum(Codice_2) & " ")
            StrSQL.Append("         , 0  ")
            StrSQL.Append("         , Null  ")
            StrSQL.Append("         , " & Agro_SQL_SaveDateTime(Date.Now) & "  ")
            StrSQL.Append("         , " & Agro_SQL_SaveDateTime(Date.Now) & "  ")
            StrSQL.Append("         ,'" & Agro_SQL_SaveText(objParametri.UsernameOperazione) & "' ")
            StrSQL.Append("         ,'" & Agro_SQL_SaveText(objParametri.UsernameOperazione) & "' ")
            StrSQL.Append("         , " & Agro_SQL_SaveDate(Validita_Inizio) & "  ")
            StrSQL.Append("         , " & Agro_SQL_SaveDate(Validita_Fine) & "  ")
            StrSQL.Append(")")
            '---------------------------------------------


            '--------------------------------------------------------------------------
            xRisp = EseguiQuery_Scrittura(objParametri, StrSQL.ToString, NomeRoutine)
            '--------------------------------------------------------------------------

            'Dim log As New Text.StringBuilder()
            'log.AppendLine("Colonna -> Parametro ")
            'log.AppendLine("Utente -> " & Utente)
            'log.AppendLine("Id_Servizio -> " & Id_Servizio)
            'log.AppendLine("Descrizione_2 -> " & Descrizione_2.Substring(0, If(Descrizione_2.Length > 172, 172, Descrizione_2.Length))) 'Quantitativo di char sufficiente a mostrare fino a 5 imprese e capire se la stringa continua o meno
            'log.AppendLine("Data_Creazione -> " & Date.Now.ToString("G"))
            'log.AppendLine("Data_Modifica -> " & Date.Now.ToString("G"))
            'log.AppendLine("UserName_Creazione -> " & objParametri.UsernameOperazione)
            'log.AppendLine("UserName_Modifica -> " & objParametri.UsernameOperazione)
            'log.AppendLine("Validita_Inizio -> " & Validita_Inizio.ToString("G"))
            'log.AppendLine("Validita_Fine -> " & Validita_Fine.ToString("G"))
            'log.AppendLine()
            'Scrivi_LOG(objParametri, NomeRoutine, log.ToString)

        Catch ex As Exception
            MessaggioErrore = ex.Message
            Scrivi_LOG(objParametri, NomeRoutine, MessaggioErrore)
            xRisp = False
            Throw New Exception("[" & NomeRoutine & "] : " & MessaggioErrore)
        End Try

        Return xRisp

    End Function

    ''' <param name="utenti">Collezione di username. Se la collezione contiene più di 10.000 username, quelli in eccesso verranno ignorati.</param>
    ''' <param name="objParametri">objParametri_Utenti</param>
    Public Function ScriviMassivo(
        utenti As IEnumerable(Of String),
        Descrizione_1 As String, Descrizione_2 As String,
        ByRef objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri,
        Optional Codice_1 As Int32 = 0, Optional Codice_2 As Int32 = 0,
        Optional Id_Servizio As Int32 = AgronicaCoreDataProvider.TipiEnumerativi.enum_Id_Servizio.GiasOnline,
        Optional Validita_Inizio As Date = AgronicaCoreDataProvider.CostantiPersonalizzate.AGRODATAINIZIO,
        Optional Validita_Fine As Date = AgronicaCoreDataProvider.CostantiPersonalizzate.AGRODATAFINE
    ) As Boolean
        Dim NomeRoutine As String = "AnagrafeCoreUtentiDAL.Utenti_Profili_Write.ScriviMassivo()"
        Dim StrSQL As New Text.StringBuilder With {.Length = 0}
        Dim xRisp As Boolean
        Dim conApici = utenti.Count > 1
        Dim xIn = StringListAggregator(utenti, conApici).FirstOrDefault
        Try
            StrSQL.AppendLine(" ; WITH u_cte AS ( ")
            StrSQL.AppendLine("   SELECT UserName, getdate() as today FROM Utenti ")
            StrSQL.AppendLine("   WHERE UserName in ( " & Agro_SQL_Save_Clausola_IN(xIn, valoriStringa:=Not conApici, creaParametriSql:=Not conApici) & " ) ")
            StrSQL.AppendLine(" ) ")
            StrSQL.AppendLine(" INSERT INTO Utenti_profili ")
            StrSQL.AppendLine(" (  Utente, Utente_Profilo, Id_Servizio, ")
            StrSQL.AppendLine("    Descrizione_1, Descrizione_2, Codice_1, Codice_2, ")
            StrSQL.AppendLine("    Inviato,             DataInvio, ")
            StrSQL.AppendLine("    Data_Creazione,      Data_Modifica, ")
            StrSQL.AppendLine("    UserName_Creazione,  UserName_Modifica, ")
            StrSQL.AppendLine("    Validita_Inizio,     Validita_Fine ")
            StrSQL.AppendLine("  ) ")
            StrSQL.AppendLine(" SELECT ")
            StrSQL.AppendLine("   u_cte.UserName as Utente, ")
            StrSQL.AppendLine("   '" & Agro_SQL_SaveText(objParametri.SuperUserUsername) & "' as Utente_Profilo, ")
            StrSQL.AppendLine("   " & Agro_SQL_SaveNum(Id_Servizio) & " as Id_Servizio, ")
            StrSQL.AppendLine("   '" & Agro_SQL_SaveText(Descrizione_1) & "' as Descrizione_1, ")
            StrSQL.AppendLine("   '" & Agro_SQL_SaveText(Descrizione_2) & "' as Descrizione_2, ")
            StrSQL.AppendLine("   " & Agro_SQL_SaveNum(Codice_1) & " as Codice_1, ")
            StrSQL.AppendLine("   " & Agro_SQL_SaveNum(Codice_2) & " as Codice_2, ")
            StrSQL.AppendLine("   0 as inviato, Null as DataInvio, ")
            StrSQL.AppendLine("   u_cte.today as Data_Creazione, ")
            StrSQL.AppendLine("   u_cte.today as Data_Modifica, ")
            StrSQL.AppendLine("   '" & Agro_SQL_SaveText(objParametri.UsernameOperazione) & "' as Username_Creazione, ")
            StrSQL.AppendLine("   '" & Agro_SQL_SaveText(objParametri.UsernameOperazione) & "' as Username_Modifica, ")
            StrSQL.AppendLine("   " & Agro_SQL_SaveDate(Validita_Inizio) & " as Validita_Inizio, ")
            StrSQL.AppendLine("   " & Agro_SQL_SaveDate(Validita_Fine) & " as Validita_Fine ")
            StrSQL.AppendLine(" FROM u_cte ")

            xRisp = EseguiQuery_Scrittura(objParametri, StrSQL.ToString, NomeRoutine)
        Catch ex As Exception
            Scrivi_LOG(objParametri, NomeRoutine, ex.Message)
            Throw New Exception("[" & NomeRoutine & "] : " & ex.Message)
        End Try
        Return xRisp
    End Function

    '##############################################################################################
    Public Function Modifica(
                            ByVal Utente As String,
                            ByVal Id_Servizio As Int32,
                            ByVal Descrizione_1 As String,
                            ByVal Descrizione_2 As String,
                            ByVal Codice_1 As Int32,
                            ByVal Codice_2 As Int32,
                                ByVal xFiltroAggiuntivo As String,
                                ByRef objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri
                                ) As Boolean

        Dim NomeRoutine As String = "AnagrafeCoreUtentiDAL.Utenti_Profili_Write.Modifica()"

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
            StrSQL.Append("UPDATE Utenti_Profili SET ")
            StrSQL.Append("       Descrizione_1 = '" & Agro_SQL_SaveText(Descrizione_1) & "'")
            StrSQL.Append("       ,Descrizione_2 = '" & Agro_SQL_SaveText(Descrizione_2) & "'")
            StrSQL.Append("       ,Codice_1 = " & Agro_SQL_SaveNum(Codice_1) & " ")
            StrSQL.Append("       ,Codice_2 = " & Agro_SQL_SaveNum(Codice_2) & " ")
            StrSQL.Append("       ,Inviato           =  0 ")
            StrSQL.Append("       ,DataInvio         =  Null ")
            StrSQL.Append("       ,Data_Modifica     =  " & Agro_SQL_SaveDateTime(Date.Now))
            StrSQL.Append("       ,UserName_Modifica = '" & Agro_SQL_SaveText(objParametri.UsernameOperazione) & "'")
            StrSQL.Append("       ,Validita_Inizio   =  " & Agro_SQL_SaveDate(objParametri.FinestraTemporaleInizio))
            StrSQL.Append("       ,Validita_Fine     =  " & Agro_SQL_SaveDate(objParametri.FinestraTemporaleFine))
            StrSQL.Append(" WHERE Utente='" & Agro_SQL_SaveText(Utente) & "'")
            StrSQL.Append(" AND   Utente_Profilo = '" & Agro_SQL_SaveText(objParametri.SuperUserUsername) & "' ")
            StrSQL.Append(" AND   Id_Servizio = " & Agro_SQL_SaveNum(Id_Servizio) & " ")
            '---------------------------------------------

            If xFiltroAggiuntivo <> "" Then
                StrSQL.Append(" AND " & Agro_SQL_Save_xFiltroAggiuntivo(xFiltroAggiuntivo, , objParametri))
            End If

            '--------------------------------------------------------------------------
            xRisp = EseguiQuery_Scrittura(objParametri, StrSQL.ToString, NomeRoutine)
            '--------------------------------------------------------------------------

            'Dim log As New Text.StringBuilder()
            'log.AppendLine("Colonna -> Parametro ")
            'log.AppendLine("Utente -> " & Utente)
            'log.AppendLine("Id_Servizio -> " & Id_Servizio)
            'log.AppendLine("Descrizione_2 -> " & Descrizione_2.Substring(0, If(Descrizione_2.Length > 172, 172, Descrizione_2.Length))) 'Quantitativo di char sufficiente a mostrare fino a 5 imprese e capire se la stringa continua o meno
            'log.AppendLine("Data_Modifica -> " & Date.Now.ToString("G"))
            'log.AppendLine("UserName_Modifica -> " & objParametri.UsernameOperazione)
            'log.AppendLine("Validita_Inizio -> " & objParametri.FinestraTemporaleInizio.ToString("G"))
            'log.AppendLine("Validita_Fine -> " & objParametri.FinestraTemporaleFine.ToString("G"))
            'log.AppendLine()
            'Scrivi_LOG(objParametri, NomeRoutine, log.ToString)

        Catch ex As Exception
            MessaggioErrore = ex.Message
            Scrivi_LOG(objParametri, NomeRoutine, MessaggioErrore)
            xRisp = False
            Throw New Exception("[" & NomeRoutine & "] : " & MessaggioErrore)
        End Try

        Return xRisp

    End Function


    '##############################################################################################
    Public Function Cancella(
                            ByVal Utente As String,
                            ByVal Id_Servizio As Int32,
                                ByVal xFiltroAggiuntivo As String,
                                ByRef objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri
                                ) As Boolean

        Dim NomeRoutine As String = "AnagrafeCoreUtentiDAL.Utenti_Profili_Write.Cancella()"

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
            If objParametri.FlagCancellazioneLogica = AgronicaCoreDataProvider.AgronicaCoreParametri.enumCancellazioneLogica.CancellazioneLogica Then

                StrSQL.Length = 0
                StrSQL.Append(" UPDATE Utenti_Profili ")
                StrSQL.Append(" SET ")
                StrSQL.Append("      Username_Modifica = '" & Agro_SQL_SaveText(objParametri.UsernameOperazione) & "' ")
                StrSQL.Append("      ,Inviato = -1 ")
                StrSQL.Append(" WHERE    Utente = '" & Agro_SQL_SaveText(Utente) & "' ")
                StrSQL.Append(" AND Inviato >= 0")

                If Id_Servizio <> 0 Then
                    StrSQL.Append(" AND  Id_Servizio =  " & Agro_SQL_SaveNum(Id_Servizio) & " ")
                End If

                If objParametri.SuperUserUsername <> "" Then
                    StrSQL.Append(" AND  Utente_Profilo = '" & Agro_SQL_SaveText(objParametri.SuperUserUsername) & "' ")
                End If

            Else

                StrSQL.Length = 0
                StrSQL.Append(" DELETE ")
                StrSQL.Append(" FROM     Utenti_Profili ")
                StrSQL.Append(" WHERE    Utente = '" & Agro_SQL_SaveText(Utente) & "' ")

                If Id_Servizio <> 0 Then
                    StrSQL.Append(" AND  Id_Servizio =  " & Agro_SQL_SaveNum(Id_Servizio) & " ")
                End If

                If objParametri.SuperUserUsername <> "" Then
                    StrSQL.Append(" AND  Utente_Profilo = '" & Agro_SQL_SaveText(objParametri.SuperUserUsername) & "' ")
                End If


            End If
            '---------------------------------------------
            If xFiltroAggiuntivo <> "" Then
                StrSQL.Append(" AND " & Agro_SQL_Save_xFiltroAggiuntivo(xFiltroAggiuntivo, , objParametri))
            End If

            '--------------------------------------------------------------------------
            xRisp = EseguiQuery_Scrittura(objParametri, StrSQL.ToString, NomeRoutine)
            '--------------------------------------------------------------------------

            'Dim log As New Text.StringBuilder()
            'log.AppendLine("Cancellazione " & If(objParametri.FlagCancellazioneLogica = AgronicaCoreDataProvider.AgronicaCoreParametri.enumCancellazioneLogica.CancellazioneLogica, "Logica", "Fisica"))
            'log.AppendLine("Utente -> " & Utente)
            'log.AppendLine("Id_Servizio -> " & Id_Servizio)
            'log.AppendLine()
            'Scrivi_LOG(objParametri, NomeRoutine, log.ToString)

        Catch ex As Exception
            MessaggioErrore = ex.Message
            Scrivi_LOG(objParametri, NomeRoutine, MessaggioErrore)
            xRisp = False
            Throw New Exception("[" & NomeRoutine & "] : " & MessaggioErrore)
        End Try
        Return xRisp

    End Function

    ''' <param name="utenti">Collezione di username. Se la collezione contiene più di 10.000 username, quelli in eccesso verranno ignorati.</param>
    ''' <param name="objParametri">objParametri_Utenti</param>
    Public Function CancellaMassivo(
        ByVal utenti As IEnumerable(Of String), ByVal xFiltroAggiuntivo As String,
        ByRef objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri
    )
        Dim NomeRoutine As String = "AnagrafeCoreUtentiDAL.Utenti_Profili_Write.CancellaMassivo()"
        Dim StrSQL As New Text.StringBuilder With {.Length = 0}
        Dim xRisp As Boolean
        Dim conApici = utenti.Count > 1
        Dim xIn = StringListAggregator(utenti, conApici).FirstOrDefault

        Try
            If objParametri.FlagCancellazioneLogica = AgronicaCoreDataProvider.AgronicaCoreParametri.enumCancellazioneLogica.CancellazioneLogica Then
                StrSQL.AppendLine(" UPDATE Utenti_Profili ")
                StrSQL.AppendLine(" SET ")
                StrSQL.AppendLine("      Username_Modifica = '" & Agro_SQL_SaveText(objParametri.UsernameOperazione) & "' ")
                StrSQL.AppendLine("    , Inviato = -1 ")
            Else
                StrSQL.AppendLine(" DELETE ")
                StrSQL.AppendLine(" FROM     Utenti_Profili ")
            End If

            StrSQL.AppendLine(" WHERE    Utente IN (" & Agro_SQL_Save_Clausola_IN(xIn, valoriStringa:=Not conApici, creaParametriSql:=Not conApici) & ") ")

            '--------------------------------------------------------------------------
            If objParametri.SuperUserUsername <> "" Then
                StrSQL.AppendLine(" AND  Utente_Profilo = '" & Agro_SQL_SaveText(objParametri.SuperUserUsername) & "' ")
            End If
            If xFiltroAggiuntivo <> "" Then
                StrSQL.AppendLine(" AND " & Agro_SQL_Save_xFiltroAggiuntivo(xFiltroAggiuntivo, , objParametri))
            End If
            '--------------------------------------------------------------------------
            xRisp = EseguiQuery_Scrittura(objParametri, StrSQL.ToString, NomeRoutine)
        Catch ex As Exception
            Scrivi_LOG(objParametri, NomeRoutine, ex.Message)
            Throw New Exception("[" & NomeRoutine & "] : " & ex.Message)
        End Try
        Return xRisp
    End Function

    '##############################################################################################
    Public Function CancellaVisibilita(
                            ByVal Utente As String,
                            ByVal Id_Servizio As Int32,
                                ByVal xFiltroAggiuntivo As String,
                                ByRef objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri
                                ) As Boolean

        Dim NomeRoutine As String = "AnagrafeCoreUtentiDAL.Utenti_Visibilita_Appoggio_W.Cancella()"
        Dim MessaggioErrore As String = ""
        Dim StrSQL As New System.Text.StringBuilder
        Dim xRisp As Boolean = False

        Try

            StrSQL.Length = 0
            StrSQL.AppendLine(" DELETE ")
            StrSQL.AppendLine(" FROM     Utenti_Visibilita ")
            StrSQL.AppendLine(" WITH(ROWLOCK) ")
            StrSQL.AppendLine(" WHERE PivaSuperUser='" & Agro_SQL_SaveText(objParametri.PivaSuperUser) & "'")
            StrSQL.AppendLine(" AND Username = '" & Agro_SQL_SaveText(Utente) & "' ")

            '---------------------------------------------
            If xFiltroAggiuntivo <> "" Then
                strSql.AppendLine(" AND " & Agro_SQL_Save_xFiltroAggiuntivo(xFiltroAggiuntivo, , objParametri))
            End If

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

    Public Function ModificaDataUtentiVisibilitaAppoggio(ByVal Utente As String, ByVal Id_Servizio As Integer, ByVal nuovaData As DateTime, ByRef objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri) As Boolean

        Dim NomeRoutine As String = "AnagrafeCoreUtentiDAL.Utenti_Profili_Write.ModificaDataUtentiVisibilitaAppoggio()"

        Dim MessaggioErrore As String = ""
        Dim StrSQL As New System.Text.StringBuilder
        Dim xRisp As Boolean = False

        Try
            StrSQL.Length = 0
            StrSQL.AppendLine("UPDATE Utenti_Profili WITH (ROWLOCK) SET ")
            StrSQL.AppendLine("       DataUltimoRiportoUtentiVisibilitaAppoggio = " & Agro_SQL_SaveDateTime(nuovaData))
            StrSQL.AppendLine("WHERE  Utente='" & Agro_SQL_SaveText(Utente) & "'")
            StrSQL.AppendLine("AND    Utente_Profilo = '" & Agro_SQL_SaveText(objParametri.SuperUserUsername) & "' ")
            StrSQL.AppendLine("AND    Id_Servizio = " & Agro_SQL_SaveNum(Id_Servizio) & " ")

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

End Class
