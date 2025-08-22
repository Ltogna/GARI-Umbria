Imports System.Collections.Concurrent
Imports System.Data.SqlClient
Imports System.Text
Imports System.Threading
Imports System.Threading.Tasks
Imports AgronicaCoreDataProvider
Imports AgronicaCoreDataProvider.TipiEnumerativi
Imports AgronicaCoreUtility
Imports AgronicaCoreVarieDAL
Imports Newtonsoft.Json

Public Class SincronizzatoreUtentiVisibilitaService

    Private ReadOnly _objParametriServer As AgronicaCoreParametri
    Private ReadOnly _objParametriUtente As AgronicaCoreParametri
    Private ReadOnly _objParametriSuperServer As AgronicaCoreParametri
    Private ReadOnly _configurazioneServizio As Configurazione_Servizio
    Private ReadOnly _parametriExtra As SincroUtentiVisibilitaAppoggio_ParametriExtra
    Private ReadOnly _logger As New LogProvider
    Private _logFileName As String = "LogSincronizzazionePermessi.txt"
    Private objectLocker As New Object()
    Dim customLOGParams As CustomLOGParams

    Public Sub New(
            ByVal objParametriServer As AgronicaCoreParametri,
            ByVal objParametriUtente As AgronicaCoreParametri,
            ByVal objParametriSuperServer As AgronicaCoreParametri,
            ByVal configurazioneServizio As Configurazione_Servizio
        )

        _objParametriServer = objParametriServer
        _objParametriUtente = objParametriUtente
        _objParametriSuperServer = objParametriSuperServer
        _configurazioneServizio = configurazioneServizio

        If Not String.IsNullOrEmpty(_configurazioneServizio.Parametri_Extra) Then
            _parametriExtra = JsonConvert.DeserializeObject(Of SincroUtentiVisibilitaAppoggio_ParametriExtra)(_configurazioneServizio.Parametri_Extra)
        Else
            _parametriExtra = New SincroUtentiVisibilitaAppoggio_ParametriExtra With
            {
                .Fattore = 2,
                .Operazione = "*"
            }
        End If

        customLOGParams = New CustomLOGParams With {
            .LogDescrizioneUtente = _objParametriServer.LogDescrizioneUtente,
            .LogDirectory = _configurazioneServizio.DirectoryLOG,
            .LogFileName = _logFileName
        }


    End Sub

    Public Sub New(
            ByVal objParametriServer As AgronicaCoreParametri,
            ByVal objParametriUtente As AgronicaCoreParametri,
            ByVal objParametriSuperServer As AgronicaCoreParametri
        )

        _objParametriServer = objParametriServer
        _objParametriUtente = objParametriUtente
        _objParametriSuperServer = objParametriSuperServer

        _parametriExtra = New SincroUtentiVisibilitaAppoggio_ParametriExtra With
                                {
                                    .Fattore = 2,
                                    .Operazione = "*"
                                }
        _configurazioneServizio = New Configurazione_Servizio With
        {
            .DirectoryLOG = "C:\GiasLAN\LOG\SincronizzazioneUtentiVisibilita"
        }

    End Sub

    Public Function SincronizzaTuttiGliUtenti_Parallel() As Boolean

        Dim gestoreUtente As New AgronicaCoreUtentiBIZ.Utenti
        Dim utenti_R As New AgronicaCoreUtentiDAL.Utenti_Read
        Dim objProfilo_R As New AgronicaCoreUtentiDAL.Utenti_Profili_Read
        Dim objProfilo_W As New AgronicaCoreUtentiDAL.Utenti_Profili_Write

        Dim classJoin As New JoinFiltrone
        classJoin.bGerarchiaImprese = True
        classJoin.bCentriAziendali = True

        Dim dtUtentiProfili As DataTable = Nothing
        Dim utentiProfili As List(Of IDictionary(Of String, Object)) = Nothing

        Dim debugMessage As String = String.Empty
        Dim strSql As String = String.Empty
        Dim PivaSuperUser = _objParametriServer.PivaSuperUser
        Dim Entita_Cod_Impresa = 1
        Dim Entita_Cod_Centro = 2
        Dim retVal As Boolean = True
        Dim usaCapostipiti As Boolean = False
        Dim nomeRoutine As String = "AgronicaCoreUtentiBIZ.SincronizzatoreUtentiVisibilitaService.SincronizzaTuttiGliUtenti_Parallel"

        Try

            Dim cnnServer As SqlConnection = DataProviderFactory.Instance.CreaNuovaConnessione(_objParametriServer.StringaConnessione)
            Dim cnnUtenti As SqlConnection = DataProviderFactory.Instance.CreaNuovaConnessione(_objParametriUtente.StringaConnessione)

            cnnServer.Open()
            cnnUtenti.Open()
            Dim sw As New Stopwatch
            sw.Start()

            Dim config As New AgronicaCoreVarieDAL.Configurazione_Siti_R
            Dim confSitiUsaCapostipiti = config.Leggi_Valore(Sito_Cod:=0, "Utenti_Visibilia_Appoggio_Da_Capostipiti", xFiltroAggiuntivo:="", xOrderBy:="", _objParametriServer).Trim()

            If String.IsNullOrWhiteSpace(confSitiUsaCapostipiti) OrElse confSitiUsaCapostipiti = "1" Then
                usaCapostipiti = True
            End If

            Dim xFiltroAgg = "" '" Username in ('1000034','100016','100017','1000202','100027','100037','1000176','1000065','1000256','1000315','295128','7018126','8315290','admin.bluarancio.new','carlini@coldiretti.it','claudio.carlini@bluarancio.com','coldirettinazionale','demetraprod')"


            dtUtentiProfili = utenti_R.Leggi_X_Sincronizzazione_Uuenti_Visibilita_Appoggio(_objParametriUtente)
            If Not IsNothing(dtUtentiProfili) Then
                utentiProfili = dtUtentiProfili.ToExpandoObject
            End If

            cnnServer.Close()
            cnnUtenti.Close()

            sw.Stop()

            debugMessage = String.Format("Tempo totale impiegato pre caricamento info utenti e profili {0} secondi.", sw.Elapsed.Seconds.ToString)
            _logger.Scrivi_LOG(_objParametriServer,
                       nomeRoutine,
                       debugMessage,
                       CustomLOGParams:=customLOGParams)


            sw.Start()

            Dim procCount As Integer = Environment.ProcessorCount
            Select Case _parametriExtra.Operazione
                Case "/"
                    procCount = Environment.ProcessorCount / _parametriExtra.Fattore
                Case "*"
                    procCount = Environment.ProcessorCount * _parametriExtra.Fattore
            End Select


            If Not IsNothing(utentiProfili) AndAlso utentiProfili.Any Then

                debugMessage = String.Format("Caricati {0} utenti. Calcolo dei chunks in corso...", utentiProfili.Count)
                _logger.Scrivi_LOG(_objParametriServer,
                       nomeRoutine,
                       debugMessage,
                       CustomLOGParams:=customLOGParams)


                Dim chunksUtenti = ChunkBigSizeListBy(Of IDictionary(Of String, Object))(utentiProfili, procCount)

                debugMessage = String.Format("Creati {0} chunks da {1} utenti l'uno.", chunksUtenti.Count, procCount)
                _logger.Scrivi_LOG(_objParametriServer,
                       nomeRoutine,
                       debugMessage,
                       CustomLOGParams:=customLOGParams)

                'Dim chunksUtenti = ChunkBy(Of IDictionary(Of String, Object))(utenti, procCount).ToList()
                Dim infoUtenti As New ConcurrentQueue(Of UtentiVisibilitaTempObject)
                Dim chunkCounuter As Integer = 0

                For Each chunck In chunksUtenti

                    Dim localCnn As SqlConnection = DataProviderFactory.Instance.CreaNuovaConnessione(_objParametriServer.StringaConnessione)
                    localCnn.Open()

                    Try

                        Dim swPerChunk As New Stopwatch
                        swPerChunk.Start()

                        chunkCounuter += 1
                        Dim chunckLista = chunck.ToList

                        'Lavez - 20/03/2025 - Le elaborazioni concorrenti devono essere limitate altrimetni si corre il rischio di saturare il server
                        Dim options As New ParallelOptions

                        If (Environment.ProcessorCount / 2) < 1 Then
                            options.MaxDegreeOfParallelism = 1
                        Else
                            options.MaxDegreeOfParallelism = Environment.ProcessorCount / 2
                        End If

                        Parallel.ForEach(chunckLista, options,
                          Sub(u As IDictionary(Of String, Object))

                              SyncLock (objectLocker)

                                  Dim Sql_Permessi As String = ""
                                  Dim Xml_Permessi As String = ""
                                  Dim userName As String = If(u("UserName") Is DBNull.Value, "", u("UserName"))
                                  Dim sb As New StringBuilder

                                  Dim dataModificaUtentiProfili As New DateTime
                                  Dim dataUltimoAggiornamentoPermessi As New DateTime?

                                  Sql_Permessi = u("Descrizione_2").ToString & ""
                                  Xml_Permessi = u("Descrizione_1").ToString & ""
                                  dataModificaUtentiProfili = u("Data_Modifica")
                                  dataUltimoAggiornamentoPermessi = If(u("DataUltimoRiportoUtentiVisibilitaAppoggio") Is DBNull.Value, CType(Nothing, DateTime?), u("DataUltimoRiportoUtentiVisibilitaAppoggio"))

                                  Dim visibilitaTotale As Boolean = False
                                  If Not usaCapostipiti Then
                                      visibilitaTotale = If(String.IsNullOrEmpty(Sql_Permessi), True, False)
                                  Else
                                      visibilitaTotale = If(String.IsNullOrEmpty(Xml_Permessi), True, False)
                                  End If

                                  Dim infoUtente As New UtentiVisibilitaTempObject With
                                {
                                    .permessiSql = Sql_Permessi,
                                    .utente_UserName = userName,
                                    .visibilitaTotale = visibilitaTotale,
                                    .nomeTabellaTemp = String.Empty,
                                    .daProcessare = False,
                                    .cnnServer = Nothing
                                }

                                  If Not infoUtente.visibilitaTotale Then
                                      If Not dataUltimoAggiornamentoPermessi.HasValue OrElse dataUltimoAggiornamentoPermessi.Value < dataModificaUtentiProfili Then

                                          infoUtente.daProcessare = True
                                          infoUtente.nomeTabellaTemp = Guid.NewGuid().ToString.Replace("-", "_")

                                          ' creazione tabella temporanea per utente
                                          Dim objUV_Creazione_Tmp_W As New AgronicaCoreUtentiDAL.Utenti_Visibilita_Appoggio_W
                                          strSql = objUV_Creazione_Tmp_W.Ottieni_Sql_Creazione_Tabella_Temp_Utenti_Visibilita_Appoggio(infoUtente.nomeTabellaTemp)
                                          sb.Append(strSql & "; ")
                                          sb.Append(Environment.NewLine)

                                          Dim objPServer = _objParametriServer.CreateDeepCopy(_objParametriServer)
                                          Dim objPUtenti = _objParametriUtente.CreateDeepCopy(_objParametriUtente)

                                          If Not usaCapostipiti Then
                                              Dim FiltroneImprese As New AgronicaCoreUtility.Filtrone
                                              FiltroneImprese.MantieniParametri = True
                                              Dim sqlFiltroneImprese = FiltroneImprese.CreaStringaQueryPerDTFiltrone(objPServer, Sql_Permessi, enum_TipoSelect_FiltroneSuperNova.Imprese_Visibilita_Appoggio, " ", classJoin, True, False).ToOrigin(objPServer)

                                              Dim objUT_Popola_Temp_Imprese_R As New AgronicaCoreUtentiDAL.Utenti_Visibilita_Appoggio_R
                                              strSql = objUT_Popola_Temp_Imprese_R.Ottieni_Sql_Popola_Tabella_Temporanea_DT_FiltroneImprese(infoUtente.nomeTabellaTemp, PivaSuperUser, userName, Entita_Cod_Impresa, sqlFiltroneImprese)
                                              sb.Append(strSql & "; ")
                                              sb.Append(Environment.NewLine)

                                              Dim filtroneCentri As New AgronicaCoreUtility.Filtrone
                                              filtroneCentri.MantieniParametri = True
                                              Dim sqlFiltroneCentri = filtroneCentri.CreaStringaQueryPerDTFiltrone(objPServer, Sql_Permessi, enum_TipoSelect_FiltroneSuperNova.CentriAziendali_Visibilita_Appoggio, " ", classJoin, True, False)

                                              Dim objUT_Popola_Temp_Centri_R As New AgronicaCoreUtentiDAL.Utenti_Visibilita_Appoggio_R
                                              strSql = objUT_Popola_Temp_Centri_R.Ottieni_Sql_Popola_Tabella_Temporanea_DT_FiltroneCentri(infoUtente.nomeTabellaTemp, PivaSuperUser, userName, Entita_Cod_Centro, sqlFiltroneCentri)
                                              sb.Append(strSql & "; ")
                                              sb.Append(Environment.NewLine)

                                              infoUtente.sql = sb.ToString

                                          Else

                                              Dim bizVisibilita As New AgronicaCoreUtentiBIZ.Utenti_Visibilita
                                              Dim pive = bizVisibilita.LeggiPiveCapostipiti(Xml_Permessi)

                                              Dim strSqlPopolaConGerarchia As String = String.Empty

                                              objUV_Creazione_Tmp_W.PopolaConGerarchia(userName, pive, objPServer, strSqlPopolaConGerarchia, String.Format("#Utenti_Visibilita_Appoggio_{0}", infoUtente.nomeTabellaTemp))
                                              sb.Append(strSqlPopolaConGerarchia & "; ")
                                              sb.Append(Environment.NewLine)

                                              infoUtente.sql = sb.ToString

                                          End If

                                          objPServer = Nothing
                                          objPUtenti = Nothing

                                      End If

                                  End If
                                  infoUtenti.Enqueue(infoUtente)


                              End SyncLock

                          End Sub)

                        Dim numProfiliProcessati = infoUtenti.Where(Function(q) q.daProcessare).Count()
                        Dim numProfiliChunk = chunck.Count()

                        If infoUtenti.Count > 0 Then

                            Dim infoUtente As UtentiVisibilitaTempObject = Nothing
                            Dim utentiDaProcessare As New List(Of UtentiVisibilitaTempObject)
                            Dim utentiConVisibilitaTotale As New List(Of UtentiVisibilitaTempObject)
                            While infoUtenti.TryDequeue(infoUtente)
                                If Not IsNothing(infoUtente) Then
                                    If infoUtente.visibilitaTotale Then
                                        utentiConVisibilitaTotale.Add(infoUtente)
                                    Else
                                        If infoUtente.daProcessare AndAlso Not String.IsNullOrEmpty(infoUtente.sql) Then
                                            utentiDaProcessare.Add(infoUtente)
                                        End If
                                    End If
                                End If
                            End While

                            Dim sbCreazioneTemps As New StringBuilder

                            ' .1 per utenti senza visibilità totale sincronizza tabella Utenti_Visibilità_Appoggio
                            If utentiDaProcessare.Count > 0 Then

                                For Each u In utentiDaProcessare
                                    sbCreazioneTemps.Append(String.Format("-- Inizio Utente {0} ", u.utente_UserName))
                                    sbCreazioneTemps.Append(Environment.NewLine)
                                    sbCreazioneTemps.Append(u.sql)
                                    sbCreazioneTemps.Append(Environment.NewLine)
                                    sbCreazioneTemps.Append(String.Format(" -- Select * from #Utenti_Visibilita_Appoggio_{0} ;", u.nomeTabellaTemp))
                                    sbCreazioneTemps.Append(Environment.NewLine)
                                    sbCreazioneTemps.Append(String.Format("-- Fine Utente {0} ", u.utente_UserName))
                                    sbCreazioneTemps.Append(Environment.NewLine)

                                    _logger.Scrivi_LOG(_objParametriServer,
                                                       nomeRoutine,
                                                       u.utente_UserName,
                                                       CustomLOGParams:=customLOGParams)
                                Next

                                Dim cmd As SqlCommand = CreaCommand(localCnn, _objParametriServer.TimeoutQuery, sbCreazioneTemps.ToString)
                                Try
                                    cmd.ExecuteNonQuery()
                                    cmd.CommandText = String.Empty

                                Catch ex As Exception
                                    retVal = False
                                    _logger.Scrivi_LOG(_objParametriServer,
                                                       nomeRoutine,
                                                       cmd.CommandText,
                                                       CustomLOGParams:=customLOGParams)
                                End Try

                                For Each u In utentiDaProcessare

                                    Try

                                        Dim objUV_PopolaDaTemp_Tmp_W As New AgronicaCoreUtentiDAL.Utenti_Visibilita_Appoggio_W
                                        strSql = objUV_PopolaDaTemp_Tmp_W.Ottieni_Sql_Popola_Utenti_Visibilita_Appoggio_Da_Tabella_Temp(
                                        u.nomeTabellaTemp, PivaSuperUser, u.utente_UserName)

                                        cmd.CommandText = strSql
                                        cmd.ExecuteNonQuery()
                                        cmd.CommandText = String.Empty

                                        Dim objUV_CancellaDaTemp_Tmp_W As New AgronicaCoreUtentiDAL.Utenti_Visibilita_Appoggio_W
                                        strSql = objUV_CancellaDaTemp_Tmp_W.Ottieni_Sql_Cancella_Utenti_Visibilita_Appoggio_Da_Tabella_Temp(
                                        u.nomeTabellaTemp, PivaSuperUser, u.utente_UserName)
                                        cmd.CommandText = strSql
                                        cmd.ExecuteNonQuery()
                                        cmd.CommandText = String.Empty

                                        Dim sb As New StringBuilder
                                        sb.AppendLine("declare @tmpTableName nvarchar(100)                     ")
                                        sb.AppendLine("set     @tmpTableName =   @tmpTable                     ")
                                        sb.AppendLine("declare @sql nvarchar(100)                              ")
                                        sb.AppendLine("SET     @sql = 'DROP TABLE ' + QUOTENAME(@tmpTableName)	 ")
                                        sb.AppendLine("EXEC sp_executesql @sql	                                 ")


                                        Dim Param1 As String = "#Utenti_Visibilita_Appoggio_" & u.nomeTabellaTemp
                                        cmd.Parameters.Add(New SqlParameter("tmpTable", Param1))

                                        strSql = sb.ToString()

                                        cmd.CommandText = strSql
                                        cmd.ExecuteNonQuery()
                                        cmd.Parameters.Clear()
                                        cmd.CommandText = String.Empty

                                        'Se l'aggiornamento sulla utenti_profili (che è atomica) non va a buon fine devo invalidare anche la transazione sul db_server.
                                        Dim risAggProfili = objProfilo_W.ModificaDataUtentiVisibilitaAppoggio(u.utente_UserName, 5, DateTime.Now, _objParametriUtente)
                                        If risAggProfili = False Then
                                            Throw New Exception("Errore nell'aggiornamento data ultimo riporto utenti visibilita appoggio")
                                        End If

                                    Catch ex As Exception

                                        retVal = False
                                        debugMessage = String.Format("Errore nella sincronizzazione dell'utente {0}:{1}{2}", u.utente_UserName, Environment.NewLine, ex.StackTrace)
                                        _logger.Scrivi_LOG(_objParametriServer,
                                                           nomeRoutine,
                                                           debugMessage,
                                                           CustomLOGParams:=customLOGParams)

                                    Finally
                                        If Not IsNothing(cmd) Then
                                            cmd.Dispose()
                                        End If
                                    End Try

                                Next

                            End If

                            ' .2 per utenti con visibilità totale toglie tutto da tabella Utenti_Visibilità_Appoggio
                            If utentiConVisibilitaTotale.Count > 0 Then

                                Dim objUC_W As New AgronicaCoreUtentiDAL.Utenti_Visibilita_Appoggio_W
                                Dim listaUtenti As String = String.Join(",", utentiConVisibilitaTotale.Select(Function(u) u.utente_UserName).ToList())
                                strSql = objUC_W.Ottieni_Sql_Cancella_Per_Piu_Utenti(0, "", listaUtenti, _objParametriServer)

                                Dim cmd As SqlCommand = CreaCommand(localCnn, _objParametriServer.TimeoutQuery, strSql)
                                cmd.ExecuteNonQuery()
                                cmd.CommandText = String.Empty

                                cmd.Dispose()

                            End If
                        End If

                        swPerChunk.Stop()
                        debugMessage = String.Format("Tempo impiegato per sincronizzazione chunk {0}/{1} con {2} profili di cui {3} processati: {4} millisecondi.", chunkCounuter, chunksUtenti.Count, numProfiliChunk, numProfiliProcessati, swPerChunk.ElapsedMilliseconds.ToString)
                        _logger.Scrivi_LOG(_objParametriServer,
                                           nomeRoutine,
                                           debugMessage,
                                           CustomLOGParams:=customLOGParams)

                    Catch ex As Exception

                        retVal = False
                        debugMessage = String.Format("Errore durante sincronizzazione del chunk {0}/{1}: {3}{4}", chunkCounuter, chunksUtenti.Count, Environment.NewLine, ex.StackTrace)
                        _logger.Scrivi_LOG(_objParametriServer,
                                           nomeRoutine,
                                           debugMessage,
                                           CustomLOGParams:=customLOGParams)
                    Finally
                        If Not IsNothing(localCnn) Then
                            If localCnn.State = ConnectionState.Open Then
                                localCnn.Close()
                            End If
                            localCnn.Dispose()
                        End If
                    End Try

                Next

            End If

            sw.Stop()
            debugMessage = String.Format("Tempo totale impiegato {0} secondi.", sw.Elapsed.TotalSeconds.ToString)
            _logger.Scrivi_LOG(_objParametriServer,
                               nomeRoutine,
                               debugMessage,
                               CustomLOGParams:=customLOGParams)

        Catch ex As Exception
            retVal = False
            debugMessage = ex.Message
            _logger.Scrivi_LOG(_objParametriServer,
                               nomeRoutine,
                               debugMessage,
                               CustomLOGParams:=customLOGParams)
        End Try

        Return retVal

    End Function

    Public Sub SincronizzaTuttiGliUtenti()

        Dim gestoreUtente As New AgronicaCoreUtentiBIZ.Utenti
        Dim utenti_R As New AgronicaCoreUtentiDAL.Utenti_Read
        Dim dtUtenti As DataTable = Nothing
        Dim utenti As List(Of IDictionary(Of String, Object)) = Nothing
        Dim nomeRoutine As String = "AgronicaCoreUtentiBIZ.SincronizzatoreUtentiVisibilitaService.SincronizzaTuttiGliUtenti"

        Try
            dtUtenti = utenti_R.Leggi(AgronicaCoreParametri.enumSelezioneVariabile.Selezione_TabellaCompleta, "", "", _objParametriUtente)
            If Not IsNothing(dtUtenti) Then
                utenti = dtUtenti.ToExpandoObject
            End If

            If Not IsNothing(utenti) AndAlso utenti.Any Then

                Dim sw As New Stopwatch
                sw.Start()

                For Each ut In utenti

                    Dim userNAme As String = If(ut("UserName") Is DBNull.Value, "", ut("UserName"))

                    If Not String.IsNullOrEmpty(userNAme) Then

                        Dim swPerUtente As New Stopwatch

                        Try
                            swPerUtente.Start()
                            gestoreUtente.InizializzaVisibilitaAppoggio(userNAme, _objParametriServer, _objParametriUtente)
                        Catch ex As Exception
                            Dim errorMessage As String = String.Format("Errore durante la sincronizzazione utente {0}:{1}{2}", userNAme, Environment.NewLine, ex.Message)
                            _logger.Scrivi_LOG(_objParametriServer,
                                               nomeRoutine,
                                               errorMessage,
                                               CustomLOGParams:=customLOGParams)
                        Finally
                            swPerUtente.Stop()
                            Dim debugMessage As String = String.Format("Tempo impiegato per sincronizzazione utente {0}: {1} millisecondi.", userNAme, swPerUtente.ElapsedMilliseconds.ToString)
                            _logger.Scrivi_LOG(_objParametriServer,
                                               nomeRoutine,
                                               debugMessage,
                                               CustomLOGParams:=customLOGParams)
                        End Try

                    End If

                Next

                sw.Stop()
                Dim messaggioFinale As String = String.Format("Tempo totale impiegato {0}", sw.ElapsedMilliseconds.ToString())
                _logger.Scrivi_LOG(_objParametriServer,
                                   nomeRoutine,
                                   messaggioFinale,
                                   CustomLOGParams:=customLOGParams)
            End If

        Catch ex As Exception
            ' TODO
        End Try

    End Sub

    Private Iterator Function ChunkBy(Of TSource)(ByVal source As IEnumerable(Of TSource), ByVal chunkSize As Integer) As IEnumerable(Of IEnumerable(Of TSource))
        While source.Any()
            Yield source.Take(chunkSize)
            source = source.Skip(chunkSize)
        End While
    End Function

    Private Function ChunkBigSizeListBy(Of T)(ByVal source As List(Of T), ByVal chunkSize As Integer) As List(Of List(Of T))

        Dim result As New List(Of List(Of T))

        While source.Count > 0
            Dim newChunk As List(Of T) = source.Take(chunkSize).ToList()
            result.Add(newChunk)
            source.RemoveRange(0, newChunk.Count)
        End While

        Return result

    End Function



    Private Function CreaCommand(ByVal connection As SqlConnection, ByVal timeout As Integer, ByVal strsql As String) As SqlCommand

        Dim cmd As SqlCommand = connection.CreateCommand()
        cmd.CommandType = CommandType.Text
        cmd.CommandTimeout = timeout
        cmd.CommandText = strsql

        Return cmd

    End Function

    Private Class UtentiVisibilitaTempObject

        Public objPServer As AgronicaCoreParametri = Nothing
        Public objPUtenti As AgronicaCoreParametri = Nothing
        Public cnnServer As SqlConnection = Nothing
        Public cnnUtenti As SqlConnection = Nothing
        Public utente_UserName As String
        Public nomeTabellaTemp As String = String.Empty
        Public sql As String
        Public permessiSql As String = String.Empty
        Public visibilitaTotale As Boolean = False
        Public daProcessare As Boolean = False

    End Class

End Class

Public Class SincroUtentiVisibilitaAppoggio_ParametriExtra

    Public Operazione As String
    Public Fattore As Integer

End Class


