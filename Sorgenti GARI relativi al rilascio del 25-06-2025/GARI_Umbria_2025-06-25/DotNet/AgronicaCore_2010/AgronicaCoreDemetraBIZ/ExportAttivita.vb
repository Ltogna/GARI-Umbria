Imports System.Transactions
Imports AgronicaCoreAnagrafeDAL
Imports AgronicaCoreContabDAL
Imports AgronicaCoreDataProvider
Imports AgronicaCoreDataProvider.TipiEnumerativi
Imports AgronicaCoreDTOStd.InData.Demetra
Imports AgronicaCoreDTOStd.InData.importazioni
Imports AgronicaCoreEntityFramework
Imports AgronicaCoreModello
Imports AgronicaCoreModello.OperazioneAgenda_Temp
Imports AgronicaCoreModello.Utility_Agenda
Imports AgronicaCoreModelsSTD
Imports AgronicaCoreModelsSTD.attivita
Imports AgronicaCoreModelsSTD.attivita.centri_di_costo
Imports AgronicaCoreModelsSTD.attivita.dettagli
Imports AgronicaCoreModelsSTD.attivita.risorse
Imports AgronicaCoreModelsSTD.costanti
Imports AgronicaCoreModelsSTD.metaschema
Imports AgronicaCoreModelsSTD.metaschema.avversita
Imports AgronicaCoreUtility
Imports AgronicaCoreVarieDAL
Imports Newtonsoft.Json
Imports Attivita = AgronicaCoreModelsSTD.attivita.Attivita
Imports Impianto = AgronicaCoreDTOStd.InData.Demetra.Impianto
Imports Irrigazione = AgronicaCoreDTOStd.InData.Demetra.Irrigazione
Imports Prodotto = AgronicaCoreDTOStd.InData.Demetra.Prodotto

Public Class ExportAttivita

    Public Class ParametriExtra
        Public DemetraBaseUrl As String
        Public apikey As String
        Public CUAA As String()

        Public FiltroEsportazione As Integer = -1
        Public Ambiente As String

        Public TimeoutCallEndPoint As Integer = -1

        Public OperazioniIncluse As Integer()
        Public OperazioniEscluse As Integer()

    End Class

    Public Function EsportaBrogliacciCancellati(parametriExtra As String,
                                                objParametri_Super_Server As AgronicaCoreParametri,
                                                objParametri_Server As AgronicaCoreParametri,
                                                objParametri_Utenti As AgronicaCoreParametri,
                                                configServizio As AgronicaCoreVarieDAL.Configurazione_Servizio,
                                                ByRef Messaggio_di_Ritorno_Opzionale As String) As Boolean

        Dim result As Boolean = True

        Dim gefutils As New AgronicaCoreEntityFramework.Gias_EF_Utility
        Dim EFConnString As String = gefutils.GetEntityConnectionString(objParametri_Server.StringaConnessione)
        Dim objDemetraBIZ_util As New AgronicaCoreDemetraBIZ.Util

        Try

            Dim objAppDati As New APP_Dati_R
            Dim objLogRicette As New AgronicaCoreContabDAL.AgronicaLogRicette_R

            Dim Parametri_Extra = JsonConvert.DeserializeObject(Of ParametriExtra)(parametriExtra)

            Dim piva_ammesse As New List(Of String)

            If Parametri_Extra.CUAA IsNot Nothing AndAlso Parametri_Extra.CUAA.Length > 0 Then
                Dim xImpCodR As New AgronicaCoreAnagrafeDAL.Imprese_Codici_Read
                For Each cuaa As String In Parametri_Extra.CUAA
                    piva_ammesse.Add(xImpCodR.Piva_from_CUAA(cuaa, objParametri_Server))
                Next
            End If

            'DT: non serve scremare per lav_Cod ammissibili Demetra/Gias perchè vengono filtrati in fase di import, se passano su app_dati e vengono poi cancellate, le inviamo in ogni caso
            Dim timeStamp As DateTime = DateTime.Now
            Dim dt_brogliacciNonInviati As DataTable = objAppDati.AppDati_NonInviati(enum_Esportazioni_Sistema_Cod.Demetra_Export_Attivita,
                                                                             AgronicaCoreModello.AppHelper.enum_Dati_App.AttivitaDemetra,
                                                                             piva_ammesse,
                                                                             xFiltroAggiuntivo:="", xOrderBy:="",
                                                                             objParametri_Server)

            Dim retrial As Boolean = False
            Dim IDChiamata As Integer = -1

            Dim listaAttivita As New List(Of AgronicaCoreModelsSTD.attivita.Attivita)

            '(raccoglitore_cod (codiceDemetra,già processato))
            Dim dictRaccoglitore As New Dictionary(Of Integer, Tuple(Of String, Boolean))
            For Each row As DataRow In dt_brogliacciNonInviati.Rows
                Dim racc = CInt(row("Raccoglitore_Cod"))
                If racc > 0 Then
                    If Not dictRaccoglitore.ContainsKey(racc) Then
                        dictRaccoglitore.Add(racc, Tuple.Create("", False))
                    End If

                    Dim codiceDemetra As String = row("CodiceDemetra")
                    If Not String.IsNullOrEmpty(codiceDemetra) Then
                        Dim suffissoDemetra = codiceDemetra.Substring(codiceDemetra.IndexOf("-", 0))
                        dictRaccoglitore(racc) = Tuple.Create(suffissoDemetra, False)
                    End If

                End If
            Next

            For Each row As DataRow In dt_brogliacciNonInviati.Rows

                Dim lstRicette As New List(Of Tuple(Of Integer, Integer))

                Dim esitoCorrente = Util_Costanti.ESITO_OK
                Dim errorMessageCorrente = ""

                Dim transOptions As New TransactionOptions With {.IsolationLevel = IsolationLevel.ReadCommitted}
                Using ts As New TransactionScope(TransactionScopeOption.Required, transOptions)
                    Using GiasContext As New AgronicaCoreEntityFramework.Gias_DeveloperServer_Entities(EFConnString)

                        Dim pacchettoDaInviare = Nothing

                        Try

                            'Open the contextObject connection state explicitly
                            GiasContext.Database.Connection.Open()

                            If row("Esito") = Util_Costanti.ESITO_KO OrElse row("Esito") = Util_Costanti.ESITO_BLK Then
                                retrial = True
                                IDChiamata = row("ID_Chiamata")
                            Else
                                retrial = False
                                IDChiamata = -1
                            End If

                            Dim strAttivita = row("Dati")
                            listaAttivita = JsonConvert.DeserializeObject(Of List(Of AgronicaCoreModelsSTD.attivita.Attivita))(strAttivita)
                            Dim piva = listaAttivita(0).centroAziendale.primaryKey.partitaIva

                            Dim readCUAADAL As New AgronicaCoreAnagrafeDAL.Imprese_Codici_Read
                            Dim cuaa As String = readCUAADAL.Leggi_CUAA(piva, objParametri_Server)

                            Dim datiDaEsportare = New List(Of AgronicaCoreDTOStd.InData.Demetra.Attivita)

                            If Not String.IsNullOrEmpty(cuaa) Then

                                Dim raccoglitore As Integer = row("Raccoglitore_Cod")

                                If raccoglitore = 0 Then

                                    lstRicette.Add(Tuple.Create(CInt(row("ID_Ricetta")), CInt(row("Ricetta_Operazione_Cod"))))

                                    Dim attivita_demetra As New AgronicaCoreDTOStd.InData.Demetra.Attivita

                                    attivita_demetra.codice_esterno = row("CodiceDemetra")
                                    attivita_demetra.flag_cancellazione = True

                                    datiDaEsportare.Add(attivita_demetra)
                                Else
                                    If dictRaccoglitore(raccoglitore).Item2 = False Then
                                        dictRaccoglitore(raccoglitore) = Tuple.Create(dictRaccoglitore(raccoglitore).Item1, True)

                                        Dim listaBrogliacci = objLogRicette.Leggi_UltimaOperazione("", enum_TipoOperazioneDB.Lettura, "Ricette_Operazioni", "", "", "", "", "", "", "", raccoglitore, 0, "", "", objParametri_Server)

                                        If listaBrogliacci IsNot Nothing AndAlso listaBrogliacci.Rows.Count > 0 Then
                                            For Each drBrogliaccioInLista In listaBrogliacci.Rows

                                                lstRicette.Add(Tuple.Create(CInt(drBrogliaccioInLista("Param1")), CInt(drBrogliaccioInLista("Chiave"))))

                                                Dim attivita_demetra As New AgronicaCoreDTOStd.InData.Demetra.Attivita

                                                attivita_demetra.codice_esterno = If(dictRaccoglitore(raccoglitore).Item1 <> "", drBrogliaccioInLista("Param5") & dictRaccoglitore(raccoglitore).Item1, "")
                                                attivita_demetra.flag_cancellazione = True

                                                datiDaEsportare.Add(attivita_demetra)

                                            Next
                                        End If
                                    End If

                                End If

                                If datiDaEsportare.Count > 0 Then

                                    Dim tzh As New JsonSerializerSettings With {.DateFormatString = "yyyy-MM-ddT00:00:00Z"}
                                    Dim datoCompresso As String = AgroZip.CompressioneBase64(1, JsonConvert.SerializeObject(datiDaEsportare, tzh))

                                    pacchettoDaInviare = New ImportDemetra
                                    pacchettoDaInviare.dati = datoCompresso
                                    pacchettoDaInviare.CUAA = cuaa

                                    Dim errorMessage As String = ""

                                    If Not objDemetraBIZ_util.CallEndpoint(Parametri_Extra.apikey, Parametri_Extra.DemetraBaseUrl, pacchettoDaInviare, errorMessage, timeoutCallEndPoint:=Parametri_Extra.TimeoutCallEndPoint) Then
                                        Throw New Exception(errorMessage)
                                    End If

                                Else
                                    Continue For
                                End If

                            Else
                                esitoCorrente = Util_Costanti.ESITO_BLK
                                errorMessageCorrente = Util_Costanti.BLK_MESSAGE_PREFIX & " CUAA non esistente"

                                Dim attivita_demetra As New AgronicaCoreDTOStd.InData.Demetra.Attivita

                                attivita_demetra.codice = row("Piva") & "_" & row("Id_Agenda")
                                attivita_demetra.codice_esterno = row("CodiceDemetra")

                                datiDaEsportare.Add(attivita_demetra)

                                Dim tzh As New JsonSerializerSettings With {.DateFormatString = "yyyy-MM-ddT00:00:00Z"}
                                Dim datoCompresso As String = AgroZip.CompressioneBase64(1, JsonConvert.SerializeObject(datiDaEsportare, tzh))

                                pacchettoDaInviare = New ImportDemetra
                                pacchettoDaInviare.dati = datoCompresso

                            End If


                            ''-----------------------------------
                            ''   SCRITTURA LOG INVIO RICETTE 'OK'
                            ''-----------------------------------

                            If retrial Then
                                Update_Log_Invio_Chiamate(IDChiamata, pacchettoDaInviare, enum_TipoOperazioneDB.Cancellazione, esitoCorrente, errorMessageCorrente, timeStamp, objParametri_Server)
                            Else
                                'TODO_DT: nel log invio ricetta la chiave esterna è integer, non riesco a scriverci la chiave Demetra che è stringa
                                Chiama_Scrivi_Log_Invio_Ricette(pacchettoDaInviare, lstRicette, chiave_esterna:=0, enum_TipoOperazioneDB.Cancellazione, esitoCorrente, errorMessageCorrente, timeStamp, objParametri_Server, GiasContext)
                            End If

                        Catch ex As Exception

                            Dim errorMessage = AgronicaCoreDataProvider.Gestione_Eccezioni_2015.MessaggioCompletoDataEccezione(ex, True, source:=True)

                            ''-----------------------------------
                            ''   SCRITTURA LOG INVIO RICETTE 'KO'
                            ''-----------------------------------
                            If retrial Then
                                Update_Log_Invio_Chiamate(IDChiamata, pacchettoDaInviare, enum_TipoOperazioneDB.Cancellazione, Util_Costanti.ESITO_KO, errorMessage, timeStamp, objParametri_Server)
                            Else
                                'TODO_DT: nel log invio ricetta la chiave esterna è integer, non riesco a scriverci la chiave Demetra che è stringa
                                Chiama_Scrivi_Log_Invio_Ricette(pacchettoDaInviare, lstRicette, chiave_esterna:=0, enum_TipoOperazioneDB.Cancellazione, Util_Costanti.ESITO_KO, errorMessage, timeStamp, objParametri_Server, GiasContext)
                            End If

                        Finally

                            If GiasContext.Database.Connection.State = ConnectionState.Open Then
                                GiasContext.Database.Connection.Close()
                            End If

                            ts.Complete()

                        End Try

                    End Using
                End Using
            Next

            result = True

            If Parametri_Extra.FiltroEsportazione <> Enum_FiltroEsportazione_to_ElasticSearch.Nessuno Then
                Dim objLogger As New ExtractLog
                Dim Tipo_Esportazione As New List(Of String) From {
                    enum_Esportazioni_Sistema_Cod.Demetra_Export_Attivita
                }
                objLogger.ExtractLog(configServizio, Tipo_Esportazione, Parametri_Extra.FiltroEsportazione, Parametri_Extra.Ambiente, objParametri_Server, Messaggio_di_Ritorno_Opzionale)
            End If

        Catch ex As Exception
            result = False

            Messaggio_di_Ritorno_Opzionale &= " Errore durante l'esportazione dei brogliacci : " & AgronicaCoreUtility.Gestione_Eccezioni.MessaggioCompletoDataEccezione(ex, True, source:=True)

            objDemetraBIZ_util.Chiama_ScriviLOG(configServizio.Tipo_Sincro.ToString, Messaggio_di_Ritorno_Opzionale, configServizio.DirectoryLOG, configServizio.Tipo_Sincro.ToString & "_log.txt", objParametri_Server)


        End Try

        Return result

    End Function

    Public Function EsportaAgende(parametriExtra As String,
                                  objParametri_Super_Server As AgronicaCoreParametri,
                                  objParametri_Server As AgronicaCoreParametri,
                                  objParametri_Utenti As AgronicaCoreParametri,
                                  configServizio As AgronicaCoreVarieDAL.Configurazione_Servizio,
                                  ByRef Messaggio_di_Ritorno_Opzionale As String) As Boolean

        Dim result As Boolean = True

        Dim gefutils As New AgronicaCoreEntityFramework.Gias_EF_Utility
        Dim EFConnString As String = gefutils.GetEntityConnectionString(objParametri_Server.StringaConnessione)
        Dim objDemetraBIZ_util As New AgronicaCoreDemetraBIZ.Util

        Try

            Dim objLogAgenda As New AgronicaCoreContabDAL.AgronicaLogAgenda_R

            Dim Parametri_Extra = JsonConvert.DeserializeObject(Of ParametriExtra)(parametriExtra)

            Dim piva_ammesse As New List(Of String)

            If Parametri_Extra.CUAA IsNot Nothing AndAlso Parametri_Extra.CUAA.Length > 0 Then
                Dim xImpCodR As New AgronicaCoreAnagrafeDAL.Imprese_Codici_Read
                For Each cuaa As String In Parametri_Extra.CUAA
                    piva_ammesse.Add(xImpCodR.Piva_from_CUAA(cuaa, objParametri_Server))
                Next
            End If

            Dim lstLavCod = getOperazioniDaProcessare(Parametri_Extra, AppHelper.enum_Dati_App.AttivitaDemetra)

            Dim timeStamp As DateTime = DateTime.Now
            Dim dt_agendeDaInviare As DataTable = objLogAgenda.AgendeDaInviare_Demetra(enum_Esportazioni_Sistema_Cod.Demetra_Export_Attivita,
                                                                             AgronicaCoreModello.AppHelper.enum_Dati_App.AttivitaDemetra,
                                                                             piva_ammesse,
                                                                             lstLavCod,
                                                                             xFiltroAggiuntivo:="", xOrderBy:="",
                                                                             objParametri_Server)

            Dim retrial As Boolean = False
            Dim IDChiamata As Integer = -1

            Dim listaAttivita As New List(Of AgronicaCoreModelsSTD.attivita.Attivita)
            Dim TipoOperazione As enum_TipoOperazioneDB

            '(raccoglitore_cod (codiceDemetra,già processato))
            Dim dictRaccoglitore As New Dictionary(Of Integer, Tuple(Of String, Boolean))
            For Each row As DataRow In dt_agendeDaInviare.Rows
                Dim racc = CInt(row("Raccoglitore_Cod"))
                If racc > 0 Then
                    If Not dictRaccoglitore.ContainsKey(racc) Then
                        dictRaccoglitore.Add(racc, Tuple.Create("", False))
                    End If

                    Dim codiceDemetra As String = row("CodiceDemetra")
                    If Not String.IsNullOrEmpty(codiceDemetra) Then
                        Dim suffissoDemetra = codiceDemetra.Substring(codiceDemetra.IndexOf("-", 0))
                        dictRaccoglitore(racc) = Tuple.Create(suffissoDemetra, False)
                    End If

                End If
            Next

            For Each row As DataRow In dt_agendeDaInviare.Rows

                Dim lstAgende As New List(Of Tuple(Of String, Integer, String))

                Dim InfoOperazione As InfoOperazione = GetInfoOperazione(row("Lav_Cod"), Attivita.Tipo_Attivita.QuadernoDiCampagna)

                'Vengono esportate solo le operazioni contemplate nello scambio Demetra/Gias (controllate dalla query di estrazione, ma ricontrolliamo)
                If Not lstLavCod.Contains(row("Lav_Cod")) Then
                    Continue For
                End If

                Dim esitoCorrente = Util_Costanti.ESITO_OK
                Dim errorMessageCorrente = ""

                Dim transOptions As New TransactionOptions With {.IsolationLevel = IsolationLevel.ReadCommitted}
                Using ts As New TransactionScope(TransactionScopeOption.Required, transOptions)
                    Using GiasContext As New AgronicaCoreEntityFramework.Gias_DeveloperServer_Entities(EFConnString)

                        Dim pacchettoDaInviare = Nothing

                        Try

                            'Open the contextObject connection state explicitly
                            GiasContext.Database.Connection.Open()

                            If row("Esito") = Util_Costanti.ESITO_KO OrElse row("Esito") = Util_Costanti.ESITO_BLK Then
                                retrial = True
                                IDChiamata = row("ID_Chiamata")
                            Else
                                retrial = False
                                IDChiamata = -1
                            End If

                            TipoOperazione = row("UltimaOperazione")

                            Dim readCUAADAL As New AgronicaCoreAnagrafeDAL.Imprese_Codici_Read
                            Dim cuaa As String = readCUAADAL.Leggi_CUAA(row("Piva"), objParametri_Server)

                            Dim datiDaEsportare = New List(Of AgronicaCoreDTOStd.InData.Demetra.Attivita)


                            If Not String.IsNullOrEmpty(cuaa) Then

                                Dim raccoglitore As Integer = row("Raccoglitore_Cod")

                                If raccoglitore = 0 Then

                                    lstAgende.Add(Tuple.Create(Of String, Integer, String)(row("Piva"), row("Id_Agenda"), row("CodiceDemetra")))

                                    Dim attivita_demetra As New AgronicaCoreDTOStd.InData.Demetra.Attivita

                                    If TipoOperazione <> enum_TipoOperazioneDB.Cancellazione Then
                                        attivita_demetra = MappaAttivitaToAttivitaDemetra(row("Piva"), CInt(row("Id_Agenda")), objParametri_Super_Server, objParametri_Server, objParametri_Utenti)
                                    End If

                                    attivita_demetra.codice = row("Piva") & "_" & row("Id_Agenda")
                                    attivita_demetra.codice_esterno = row("CodiceDemetra")
                                    attivita_demetra.flag_cancellazione = (TipoOperazione = enum_TipoOperazioneDB.Cancellazione)

                                    datiDaEsportare.Add(attivita_demetra)
                                Else
                                    If dictRaccoglitore(raccoglitore).Item2 = False Then
                                        dictRaccoglitore(raccoglitore) = Tuple.Create(dictRaccoglitore(raccoglitore).Item1, True)

                                        Dim listaAgende = objLogAgenda.Leggi_UltimaOperazione("", enum_TipoOperazioneDB.Lettura, 0, row("Piva"), 0, 0, 0, " Raccoglitore_Cod = " & raccoglitore, "", objParametri_Server)

                                        Dim codiceDemetra As String = ""

                                        If listaAgende IsNot Nothing AndAlso listaAgende.Rows.Count > 0 Then

                                            'tutte le agende del raccoglitore vanno inserite nel log_invio_agenda (quindi in lstAgende) anche nel caso in cui una sola di esse vada in errore
                                            For Each drAgendaInLista As DataRow In listaAgende.Rows

                                                Dim codiceDemetraRaccoglitore = If(dictRaccoglitore(raccoglitore).Item1 <> "", drAgendaInLista("Lav_Cod") & dictRaccoglitore(raccoglitore).Item1, "")
                                                lstAgende.Add(Tuple.Create(Of String, Integer, String)(drAgendaInLista("Piva"), drAgendaInLista("Id_Agenda"), codiceDemetraRaccoglitore))

                                            Next

                                            For Each drAgendaInLista As DataRow In listaAgende.Rows

                                                'DT: Non mandare se anche solo una operazione del multioperazione è fra le non contemplate demetra
                                                If Not lstLavCod.Contains(drAgendaInLista("Lav_Cod")) Then
                                                    datiDaEsportare.Clear()
                                                    Continue For
                                                End If

                                                Dim codiceDemetraRaccoglitore = If(dictRaccoglitore(raccoglitore).Item1 <> "", drAgendaInLista("Lav_Cod") & dictRaccoglitore(raccoglitore).Item1, "")

                                                Dim attivita_demetra As New AgronicaCoreDTOStd.InData.Demetra.Attivita

                                                If TipoOperazione <> enum_TipoOperazioneDB.Cancellazione Then
                                                    attivita_demetra = MappaAttivitaToAttivitaDemetra(drAgendaInLista("Piva"), drAgendaInLista("Id_Agenda"), objParametri_Super_Server, objParametri_Server, objParametri_Utenti)
                                                End If

                                                attivita_demetra.codice = drAgendaInLista("Piva") & "_" & drAgendaInLista("Id_Agenda")
                                                attivita_demetra.codice_esterno = codiceDemetraRaccoglitore
                                                attivita_demetra.flag_cancellazione = (TipoOperazione = enum_TipoOperazioneDB.Cancellazione)

                                                datiDaEsportare.Add(attivita_demetra)

                                            Next
                                        End If
                                    End If

                                End If

                                If datiDaEsportare.Count > 0 Then

                                    Dim tzh As New JsonSerializerSettings With {.DateFormatString = "yyyy-MM-ddT00:00:00Z"}
                                    Dim datoCompresso As String = AgroZip.CompressioneBase64(1, JsonConvert.SerializeObject(datiDaEsportare, tzh))

                                    pacchettoDaInviare = New ImportDemetra
                                    pacchettoDaInviare.dati = datoCompresso
                                    pacchettoDaInviare.CUAA = cuaa

                                    Dim errorMessage As String = ""

                                    If Not objDemetraBIZ_util.CallEndpoint(Parametri_Extra.apikey, Parametri_Extra.DemetraBaseUrl, pacchettoDaInviare, errorMessage, timeoutCallEndPoint:=Parametri_Extra.TimeoutCallEndPoint) Then
                                        Throw New Exception(errorMessage)
                                    End If

                                Else
                                    Continue For
                                End If

                            Else
                                esitoCorrente = Util_Costanti.ESITO_BLK
                                errorMessageCorrente = Util_Costanti.BLK_MESSAGE_PREFIX & " CUAA non esistente"

                                Dim attivita_demetra As New AgronicaCoreDTOStd.InData.Demetra.Attivita

                                attivita_demetra.codice = row("Piva") & "_" & row("Id_Agenda")
                                attivita_demetra.codice_esterno = row("CodiceDemetra")

                                datiDaEsportare.Add(attivita_demetra)

                                Dim tzh As New JsonSerializerSettings With {.DateFormatString = "yyyy-MM-ddT00:00:00Z"}
                                Dim datoCompresso As String = AgroZip.CompressioneBase64(1, JsonConvert.SerializeObject(datiDaEsportare, tzh))

                                pacchettoDaInviare = New ImportDemetra
                                pacchettoDaInviare.dati = datoCompresso

                            End If

                            ''-----------------------------------
                            ''   SCRITTURA LOG INVIO AGENDA 'OK'
                            ''-----------------------------------

                            If retrial Then
                                Update_Log_Invio_Chiamate(IDChiamata, pacchettoDaInviare, TipoOperazione, esitoCorrente, errorMessageCorrente, timeStamp, objParametri_Server)
                            Else
                                Chiama_Scrivi_Log_Invio_Agenda(pacchettoDaInviare, lstAgende, TipoOperazione, esitoCorrente, errorMessageCorrente, timeStamp, objParametri_Server, GiasContext)
                            End If


                        Catch ex As Exception

                            Dim errorMessage = AgronicaCoreDataProvider.Gestione_Eccezioni_2015.MessaggioCompletoDataEccezione(ex, True, source:=True)

                            ''-----------------------------------
                            ''   SCRITTURA LOG INVIO AGENDA 'KO'
                            ''-----------------------------------
                            If retrial Then
                                Update_Log_Invio_Chiamate(IDChiamata, pacchettoDaInviare, TipoOperazione, Util_Costanti.ESITO_KO, errorMessage, timeStamp, objParametri_Server)
                            Else
                                Chiama_Scrivi_Log_Invio_Agenda(pacchettoDaInviare, lstAgende, TipoOperazione, Util_Costanti.ESITO_KO, errorMessage, timeStamp, objParametri_Server, GiasContext)
                            End If


                        Finally

                            If GiasContext.Database.Connection.State = ConnectionState.Open Then
                                GiasContext.Database.Connection.Close()
                            End If

                            ts.Complete()

                        End Try

                    End Using
                End Using
            Next

            result = True

            If Parametri_Extra.FiltroEsportazione <> Enum_FiltroEsportazione_to_ElasticSearch.Nessuno Then
                Dim objLogger As New ExtractLog
                Dim Tipo_Esportazione As New List(Of String) From {
                    enum_Esportazioni_Sistema_Cod.Demetra_Export_Attivita
                }
                objLogger.ExtractLog(configServizio, Tipo_Esportazione, Parametri_Extra.FiltroEsportazione, Parametri_Extra.Ambiente, objParametri_Server, Messaggio_di_Ritorno_Opzionale)
            End If

        Catch ex As Exception
            result = False

            Messaggio_di_Ritorno_Opzionale &= " Errore durante l'esportazione delle agende : " & AgronicaCoreUtility.Gestione_Eccezioni.MessaggioCompletoDataEccezione(ex, True, source:=True)

            objDemetraBIZ_util.Chiama_ScriviLOG(configServizio.Tipo_Sincro.ToString, Messaggio_di_Ritorno_Opzionale, configServizio.DirectoryLOG, configServizio.Tipo_Sincro.ToString & "_log.txt", objParametri_Server)

        End Try

        Return result

    End Function

    Private Function getOperazioniDaProcessare(Parametri_Extra As ParametriExtra, tipo As AppHelper.enum_Dati_App) As List(Of Integer)

        'Se OperazioniIncluse e OperazioniEscluse sono vuoti o assenti, vengono elaborate tutte le operazioni ammissibili
        Dim lstLavCod = ImportAttivita.GetListaAttivitaAmmissibili(tipo)

        'Includi solo le operazioni specificate, se presenti
        If Parametri_Extra.OperazioniIncluse IsNot Nothing AndAlso Parametri_Extra.OperazioniIncluse.Any() Then
            lstLavCod = Parametri_Extra.OperazioniIncluse.ToList
        End If

        'Escludi le operazioni specificate, se presenti
        If Parametri_Extra.OperazioniEscluse IsNot Nothing AndAlso Parametri_Extra.OperazioniEscluse.Any() Then
            lstLavCod = lstLavCod.Except(Parametri_Extra.OperazioniEscluse.ToList).ToList
        End If

        Return lstLavCod

    End Function

    Private Function MappaAttivitaToAttivitaDemetra(piva As String, idAgenda As Integer, objParametri_Super_Server As AgronicaCoreParametri, objParametri_Server As AgronicaCoreDataProvider.AgronicaCoreParametri, objParametri_Utenti As AgronicaCoreParametri) As AgronicaCoreDTOStd.InData.Demetra.Attivita

        Dim attivitaDemetra As New AgronicaCoreDTOStd.InData.Demetra.Attivita

        Try

            Dim agendaHelper = New Agenda_Operazione_Helper()
            Dim agendaToActivity = New AgronicaCoreMapper.AgendaToAttivita()
            Dim agenda = agendaHelper.Leggi(piva, 0, idAgenda, 0, objParametri_Server)
            Dim attivita = agendaToActivity.AgendaSuAttivita(agenda, False, objParametri_Super_Server, objParametri_Server, objParametri_Utenti)

            attivitaDemetra.data = attivita.inizio
            attivitaDemetra.note = attivita.note

            attivitaDemetra.tipo_operazione = attivita.job.getCodice

            attivitaDemetra.utente_ultima_modifica = agenda.Username_Modifica

            Dim SuperficieTrattataTotale As Decimal = 0

            '''''''''''' IMPIANTI
            Dim objImpiantiCodici = New Reg_Impianti_Codici_R()
            attivitaDemetra.impianti = New List(Of Impianto)
            For Each centroDiCosto In attivita.centriDiCosto
                If (centroDiCosto.classType.Equals(ClassType.EsercizioCDC)) Then
                    Dim esercizioCDC = CType(centroDiCosto, centri_di_costo.EsercizioCDC)
                    Dim impianto As New Impianto
                    impianto.plot_id = objImpiantiCodici.Leggi_Codice_from_Reg_Impianti_Codici(esercizioCDC.esercizio.impiantoPK.appezzamentoPK.centroAziendalePK.partitaIva, esercizioCDC.esercizio.impiantoPK.appezzamentoPK.centroAziendalePK.codice, esercizioCDC.esercizio.impiantoPK.appezzamentoPK.codice, esercizioCDC.esercizio.impiantoPK.codice, enum_CodiceAnagrafe_Clienti.Demetra, objParametri_Server)

                    If String.IsNullOrEmpty(impianto.plot_id) Then
                        Dim message = String.Format(My.Resources.AgronicaCoreDemetraBIZ.ImpiantoSenzaPlotId, esercizioCDC.esercizio.impiantoPK.appezzamentoPK.centroAziendalePK.partitaIva, esercizioCDC.esercizio.impiantoPK.appezzamentoPK.centroAziendalePK.codice, esercizioCDC.esercizio.impiantoPK.appezzamentoPK.codice, esercizioCDC.esercizio.impiantoPK.codice)
                        Throw New Exception(message)
                    End If

                    impianto.superficie_trattata = esercizioCDC.superficieTrattata
                    attivitaDemetra.impianti.Add(impianto)

                    SuperficieTrattataTotale += esercizioCDC.superficieTrattata

                End If
            Next

            Dim InfoOperazione As InfoOperazione = GetInfoOperazione(attivitaDemetra.tipo_operazione, Attivita.Tipo_Attivita.QuadernoDiCampagna)

            '''''''''''' ACQUA
            If InfoOperazione.IsTrattamento OrElse InfoOperazione.IsFertilizzazione Then

                If Not IsNothing(attivita.risorse) Then

                    'DT: si assume che ci sia al massimo una risorsa acqua
                    Dim risorsaAcquaApplicata As risorse.RisorsaAcqua = (From a In attivita.risorse Where a.classType = costanti.ClassType.RisorsaAcqua).FirstOrDefault

                    If risorsaAcquaApplicata IsNot Nothing Then

                        attivitaDemetra.acqua = New Acqua

                        Select Case risorsaAcquaApplicata.doseAcqua
                            Case AgronicaCoreModelsSTD.attivita.risorse.RisorsaAcqua.TipoDoseAcqua.TOTALE
                                attivitaDemetra.acqua.quantita = risorsaAcquaApplicata.acqua
                                attivitaDemetra.acqua.tipo = TipoAcqua.hl_totale

                            Case AgronicaCoreModelsSTD.attivita.risorse.RisorsaAcqua.TipoDoseAcqua.HA
                                attivitaDemetra.acqua.quantita = risorsaAcquaApplicata.acqua
                                attivitaDemetra.acqua.tipo = TipoAcqua.hl_per_ha
                        End Select

                    End If
                End If
            End If

            '''''''''''' PRODOTTI
            attivitaDemetra.prodotti = New List(Of Prodotto)
            For Each risorsa In attivita.risorse
                Select Case risorsa.classType

                    Case costanti.ClassType.DettaglioTrattamento

                        Dim listaProdotti = GetDatiProdotto(risorsa, SuperficieTrattataTotale, InfoOperazione, objParametri_Server)

                        Dim dettaglioTrattamento = DirectCast(risorsa, dettagli.DettaglioTrattamento)
                        Dim avversita = GetAvversita(dettaglioTrattamento.avversitaGruppo)

                        If listaProdotti IsNot Nothing AndAlso listaProdotti.Count > 0 Then
                            For Each prod In listaProdotti
                                prod.avversita = avversita
                            Next
                        End If

                        attivitaDemetra.prodotti.AddRange(listaProdotti)

                    Case costanti.ClassType.DettaglioFertilizzazione

                        Dim listaProdotti = GetDatiProdotto(risorsa, SuperficieTrattataTotale, InfoOperazione, objParametri_Server)

                        Dim dettaglioFertilizzazione = DirectCast(risorsa, dettagli.DettaglioFertilizzazione)
                        If listaProdotti IsNot Nothing AndAlso listaProdotti.Count > 0 Then
                            For Each prod In listaProdotti
                                prod.Cu = dettaglioFertilizzazione.Cu
                                prod.N = dettaglioFertilizzazione.N
                                prod.P = dettaglioFertilizzazione.P
                                prod.K = dettaglioFertilizzazione.K
                            Next
                        End If

                        attivitaDemetra.prodotti.AddRange(listaProdotti)

                    Case costanti.ClassType.DettaglioSemina

                        Dim listaProdotti = GetDatiProdotto(risorsa, SuperficieTrattataTotale, InfoOperazione, objParametri_Server)
                        attivitaDemetra.prodotti.AddRange(listaProdotti)

                End Select

            Next

            '''''''''''' RACCOLTI
            attivitaDemetra.raccolti = New List(Of Raccolto)
            For Each risorsa In attivita.risorse
                If risorsa.classType = costanti.ClassType.DettaglioRaccolta Then

                    Dim listaRaccolti = GetDatiRaccolto(risorsa, SuperficieTrattataTotale, InfoOperazione, objParametri_Server)

                    attivitaDemetra.raccolti.AddRange(listaRaccolti)

                    attivitaDemetra.flag_manuale = (CType(risorsa, DettaglioRaccolta).Opzioni_Raccolta.Ripartizione = Opzioni_Raccolta.enum_Ripartizione_Raccolta.MANUALE)

                    attivitaDemetra.RaggruppaRaccolti(creaPlot:=False, lottoDistinti:=False)

                End If
            Next

            '''''''''''' IRRIGAZIONI
            attivitaDemetra.irrigazioni = New List(Of Irrigazione)
            For Each risorsa In attivita.risorse
                If risorsa.classType = costanti.ClassType.DettaglioIrrigazione Then

                    Dim dettaglioIrrigazione = DirectCast(risorsa, dettagli.DettaglioIrrigazione)

                    Dim irrigazioneDemetra As New Irrigazione

                    irrigazioneDemetra.impianto = New Impianto
                    irrigazioneDemetra.impianto.plot_id = objImpiantiCodici.Leggi_Codice_from_Reg_Impianti_Codici(dettaglioIrrigazione.esercizioCDC.esercizio.impiantoPK.appezzamentoPK.centroAziendalePK.partitaIva, dettaglioIrrigazione.esercizioCDC.esercizio.impiantoPK.appezzamentoPK.centroAziendalePK.codice, dettaglioIrrigazione.esercizioCDC.esercizio.impiantoPK.appezzamentoPK.codice, dettaglioIrrigazione.esercizioCDC.esercizio.impiantoPK.codice, enum_CodiceAnagrafe_Clienti.Demetra, objParametri_Server)
                    irrigazioneDemetra.impianto.superficie_trattata = dettaglioIrrigazione.esercizioCDC.superficieTrattata

                    irrigazioneDemetra.quantita = Math.Round(dettaglioIrrigazione.QtaRilevata, 4)
                    irrigazioneDemetra.udm = GetUdm(dettaglioIrrigazione.unitaDiMisura, InfoOperazione)

                    'DT: si è deciso di fare passare sempre m3/ha
                    If irrigazioneDemetra.udm = UdmProdotto.millimetri Then
                        irrigazioneDemetra.udm = UdmProdotto.metriCubiEttaro
                        irrigazioneDemetra.quantita = irrigazioneDemetra.quantita * 10
                    End If

                    irrigazioneDemetra.tipo = dettaglioIrrigazione.tipoIrrigazione.codice 'DT: Demetra usa un sottoinsieme dei nostri codici, usiamo direttamente il valore che abbiamo (potrebbe essere anche 0), se non lo trovano mettono 0

                    irrigazioneDemetra.inizio = dettaglioIrrigazione.DataInizio
                    irrigazioneDemetra.fine = dettaglioIrrigazione.DataFine

                    irrigazioneDemetra.frequenza = dettaglioIrrigazione.Frequenza


                    attivitaDemetra.irrigazioni.Add(irrigazioneDemetra)
                End If
            Next

            '''''''''''' MACCHINE e OPERATORI

            Dim objMacchina As New AgronicaCoreInterscambioBIZ.Interscambio_Parco_Macchine_R
            Dim objOperatore As New AgronicaCoreInterscambioBIZ.Interscambio_Contatti_R

            Dim GiasContext = Gias_EF_Utility.CreateGiasContextConnection(objParametri_Server.StringaConnessione)

            attivitaDemetra.macchine = New List(Of AgronicaCoreDTOStd.InData.Demetra.Macchina)
            attivitaDemetra.operatori = New List(Of AgronicaCoreDTOStd.InData.Demetra.Operatore)
            For Each risorsa In attivita.risorse
                Select Case risorsa.classType

                    Case costanti.ClassType.RisorsaMacchina
                        Dim macchina As New AgronicaCoreDTOStd.InData.Demetra.Macchina

                        Dim macchinaKey As String = GetMacchinaKey(attivitaDemetra.data, CType(risorsa, risorse.RisorsaMacchina).macchina.codice, objParametri_Server)
                        If macchinaKey = "" Then
                            Dim message = String.Format(My.Resources.AgronicaCoreDemetraBIZ.MacchinaNonEsistentePerChiaveGIAS, CType(risorsa, risorse.RisorsaMacchina).macchina.codice)
                            Throw New Exception(message)
                        End If

                        macchina.codice = macchinaKey

                        Dim interscambio = objMacchina.GetInterscambioParcoMacchine("", enum_SistemiEsterni.demetra, GiasContext, CType(risorsa, risorse.RisorsaMacchina).macchina.codice)
                        If interscambio IsNot Nothing Then
                            macchina.codice_esterno = interscambio.Codice_Esterno
                        End If

                        attivitaDemetra.macchine.Add(macchina)

                    Case costanti.ClassType.RisorsaPersona
                        Dim operatore As New AgronicaCoreDTOStd.InData.Demetra.Operatore

                        Dim risorsaUmana = CType(risorsa, risorse.RisorsaPersona).risorsaUmana
                        operatore.codice = risorsaUmana.contatto.primaryKey.partitaIva & "_" & risorsaUmana.contatto.primaryKey.codice

                        Dim interscambio = objOperatore.Leggi_Tabella_Interscambio_ChiaveGIAS(enum_SistemiEsterni.demetra, risorsaUmana.contatto.primaryKey.partitaIva, risorsaUmana.contatto.primaryKey.codice, 0, objParametri_Server)
                        If interscambio IsNot Nothing AndAlso interscambio.Rows.Count > 0 Then
                            operatore.codice_esterno = interscambio.Rows(0).Item("Codice_Esterno")
                        End If

                        attivitaDemetra.operatori.Add(operatore)

                End Select
            Next
        Catch ex As Exception
            Dim message = String.Format(My.Resources.AgronicaCoreDemetraBIZ.ErroreLetturaAttivita, piva, idAgenda)
            Throw New Exception(message & ": " + ex.Message)

        End Try

        Return attivitaDemetra

    End Function

    Private Function GetUdm(udm As UnitaDiMisura, InfoOperazione As InfoOperazione) As String

        Dim udmDemetra As String = ""

        '''''''''''' UDM
        Dim udmFound = False
        If InfoOperazione.IsTrattamento OrElse InfoOperazione.IsFertilizzazione Then
            Select Case udm.codice

                Case enum_UnitaMisura.Litri
                    udmDemetra = UdmProdotto.litri
                    udmFound = True

                Case enum_UnitaMisura.KG
                    udmDemetra = UdmProdotto.chilogrammi
                    udmFound = True

                Case enum_UnitaMisura.Numero_Diffusori
                    udmDemetra = UdmProdotto.numero
                    udmFound = True

                Case enum_UnitaMisura.Numero_Trappole
                    udmDemetra = UdmProdotto.numero
                    udmFound = True

            End Select

        ElseIf InfoOperazione.IsSemina Then

            Select Case udm.codice

                Case enum_UnitaMisura.KG
                    udmDemetra = UdmProdotto.chilogrammi
                    udmFound = True

                Case enum_UnitaMisura.Num_Piante, enum_UnitaMisura.Unita_Seme, enum_UnitaMisura.Confezioni
                    udmDemetra = UdmProdotto.numero
                    udmFound = True

            End Select

        ElseIf InfoOperazione.IsRaccolta Then

            Select Case udm.codice

                Case enum_UnitaMisura.KG
                    udmDemetra = UdmProdotto.chilogrammi
                    udmFound = True

                Case enum_UnitaMisura.Numero
                    udmDemetra = UdmProdotto.numero
                    udmFound = True

            End Select

        ElseIf InfoOperazione.IsIrrigazione Then

            Select Case udm.codice

                Case enum_UnitaMisura.Millimetri
                    udmDemetra = UdmProdotto.millimetri
                    udmFound = True

                Case enum_UnitaMisura.METRI3__HA
                    udmDemetra = UdmProdotto.metriCubiEttaro
                    udmFound = True

            End Select

        End If

        If Not udmFound Then
            Throw New Exception(String.Format(My.Resources.AgronicaCoreDemetraBIZ.UdmNonValida, udm.codice))
        End If

        Return udmDemetra

    End Function

    Private Function GetAvversita(avversitaGruppo As AvversitaGruppo) As AgronicaCoreDTOStd.InData.Demetra.Avversita

        Dim avversita As AgronicaCoreDTOStd.InData.Demetra.Avversita = Nothing

        If avversitaGruppo IsNot Nothing AndAlso avversitaGruppo.codice > 0 Then

            Select Case avversitaGruppo.classType

                Case ClassType.Avversita
                    avversita = New AgronicaCoreDTOStd.InData.Demetra.Avversita
                    avversita.tipo = TipoAvversita.avversita
                    avversita.codice = avversitaGruppo.codice

                Case ClassType.GruppoAvversita
                    avversita = New AgronicaCoreDTOStd.InData.Demetra.Avversita
                    avversita.tipo = TipoAvversita.gruppoAvversita
                    avversita.codice = avversitaGruppo.codice

            End Select

        End If

        Return avversita

    End Function

    Private Function GetDatiRaccolto(dettaglioRaccolta As DettaglioRaccolta, superficieTrattataTotale As Decimal, InfoOperazione As InfoOperazione, objParametri_Server As AgronicaCoreParametri) As List(Of Raccolto)

        Dim listaRaccolti As New List(Of Raccolto)

        Dim objImpiantiCodici = New Reg_Impianti_Codici_R()

        Try

            If dettaglioRaccolta.QuantitaSuImpianti IsNot Nothing AndAlso dettaglioRaccolta.QuantitaSuImpianti.Count > 0 AndAlso dettaglioRaccolta.prodotto IsNot Nothing AndAlso dettaglioRaccolta.prodotto.codice Then
                For Each qtaSuImpianti In dettaglioRaccolta.QuantitaSuImpianti

                    Dim raccoltoDemetra As New Raccolto
                    raccoltoDemetra.codice = dettaglioRaccolta.prodotto.codice
                    raccoltoDemetra.codice_esterno = "" 'DT: i trasformati vegetali nascono solo su GIAS, quindi sempre e solo codice valorizzato
                    raccoltoDemetra.quantita = Math.Round(qtaSuImpianti.Qta, 4)
                    raccoltoDemetra.udm = GetUdm(dettaglioRaccolta.unitaDiMisura, InfoOperazione)
                    raccoltoDemetra.plot_id = objImpiantiCodici.Leggi_Codice_from_Reg_Impianti_Codici(qtaSuImpianti.esercizioCDC.esercizio.impiantoPK.appezzamentoPK.centroAziendalePK.partitaIva, qtaSuImpianti.esercizioCDC.esercizio.impiantoPK.appezzamentoPK.centroAziendalePK.codice, qtaSuImpianti.esercizioCDC.esercizio.impiantoPK.appezzamentoPK.codice, qtaSuImpianti.esercizioCDC.esercizio.impiantoPK.codice, enum_CodiceAnagrafe_Clienti.Demetra, objParametri_Server)

                    '''''''''''' MAGAZZINO
                    If qtaSuImpianti.Magazzino IsNot Nothing Then
                        Dim piva = qtaSuImpianti.Magazzino.primaryKey.centroAziendalePK.partitaIva
                        Dim sa_cod = qtaSuImpianti.Magazzino.primaryKey.centroAziendalePK.codice
                        Dim fabbricato_cod = qtaSuImpianti.Magazzino.primaryKey.codice

                        raccoltoDemetra.magazzino = New Magazzino
                        raccoltoDemetra.magazzino.codice = piva & "_" & sa_cod & "_" & fabbricato_cod

                        Dim objFabbricato As New AgronicaCoreInterscambioBIZ.Interscambio_Fabbricati_R
                        Dim dtFabbricati = objFabbricato.Leggi_Tabella_Interscambio_ChiaveGIAS(enum_SistemiEsterni.demetra, piva, sa_cod, fabbricato_cod, objParametri_Server)

                        If dtFabbricati IsNot Nothing AndAlso dtFabbricati.Rows.Count > 0 Then
                            raccoltoDemetra.magazzino.codice_esterno = dtFabbricati.Rows(0).Item("Codice_Esterno")
                        End If

                        raccoltoDemetra.magazzino.lotto = qtaSuImpianti.Lotto
                    End If


                    listaRaccolti.Add(raccoltoDemetra)

                Next
            End If

        Catch ex As Exception
            Dim message = String.Format(My.Resources.AgronicaCoreDemetraBIZ.ErroreLetturaProdotto, dettaglioRaccolta.prodotto.codice)
            Throw New Exception(message & ": " + ex.Message)
        End Try

        Return listaRaccolti

    End Function

    Private Function GetDatiProdotto(dettaglio As RisorsaProdotto, superficieTrattataTotale As Decimal, InfoOperazione As InfoOperazione, objParametri_Server As AgronicaCoreParametri) As List(Of Prodotto)

        Dim listaProdotti As New List(Of Prodotto)

        Try

            If dettaglio.MagazziniMovimentazioni IsNot Nothing Then
                For Each movimentoMagazzino In dettaglio.MagazziniMovimentazioni

                    Dim prodottoDemetra As New Prodotto
                    prodottoDemetra.codice = movimentoMagazzino.Prodotto.codice
                    prodottoDemetra.quantita = Math.Round(movimentoMagazzino.Qta, 4)
                    prodottoDemetra.udm = GetUdm(movimentoMagazzino.udm, InfoOperazione)

                    '''''''''''' MAGAZZINO
                    Dim piva = movimentoMagazzino.Magazzino.primaryKey.centroAziendalePK.partitaIva
                    Dim sa_cod = movimentoMagazzino.Magazzino.primaryKey.centroAziendalePK.codice
                    Dim fabbricato_cod = movimentoMagazzino.Magazzino.primaryKey.codice

                    prodottoDemetra.magazzino = New Magazzino
                    prodottoDemetra.magazzino.codice = piva & "_" & sa_cod & "_" & fabbricato_cod

                    Dim objFabbricato As New AgronicaCoreInterscambioBIZ.Interscambio_Fabbricati_R
                    Dim dtFabbricati = objFabbricato.Leggi_Tabella_Interscambio_ChiaveGIAS(enum_SistemiEsterni.demetra, piva, sa_cod, fabbricato_cod, objParametri_Server)

                    If dtFabbricati IsNot Nothing AndAlso dtFabbricati.Rows.Count > 0 Then
                        prodottoDemetra.magazzino.codice_esterno = dtFabbricati.Rows(0).Item("Codice_Esterno")
                    End If

                    prodottoDemetra.magazzino.lotto = movimentoMagazzino.Lotto

                    listaProdotti.Add(prodottoDemetra)
                Next
            Else
                Dim prodottoDemetra As New Prodotto
                prodottoDemetra.codice = dettaglio.prodotto.codice
                prodottoDemetra.quantita = Math.Round(AgronicaCoreMapper.Utility.GetDoseTrasformata(dettaglio.doseHaReale, dettaglio.unitaDiMisuraIndicata.codice) * superficieTrattataTotale, 4)
                prodottoDemetra.udm = GetUdm(dettaglio.unitaDiMisura, InfoOperazione)
                listaProdotti.Add(prodottoDemetra)
            End If

        Catch ex As Exception
            Dim message = String.Format(My.Resources.AgronicaCoreDemetraBIZ.ErroreLetturaProdotto, dettaglio.prodotto.codice)
            Throw New Exception(message & ": " + ex.Message)
        End Try

        Return listaProdotti

    End Function

    Private Function GetMacchinaKey(dataOperazione As DateTime, mac_cod As Integer, objParametri_Server As AgronicaCoreParametri) As String

        Dim macchinaKey As String = ""

        Dim objMacchine As New AgronicaCoreContabDAL.Parco_Macchine_R

        Dim dtMacchine = objMacchine.Leggi2("",
                        mac_cod,
                        CostantiPersonalizzate.SACOD_NOFILTRO,
                         0, "", True,
                        "", "",
                        objParametri_Server)

        If dtMacchine IsNot Nothing AndAlso dtMacchine.Rows.Count > 0 Then
            macchinaKey = dtMacchine.Rows(0).Item("Piva") & "_" & dtMacchine.Rows(0).Item("Sa_Cod") & "_" & dtMacchine.Rows(0).Item("Mac_Cod")
        End If

        Return macchinaKey

    End Function

    Private Sub Chiama_Scrivi_Log_Invio_Ricette(pacchettoDaInviare As ImportDemetra, lstRicette As List(Of Tuple(Of Integer, Integer)), chiave_esterna As String, TipoOperazione As enum_TipoOperazioneDB, Esito As String, Dati_Ricevuti As String, Data_Invio As DateTime, objParametri_Server As AgronicaCoreParametri, giasContext As Gias_DeveloperServer_Entities)

        Dim objlog_invioChiamate As New AgronicaCoreVarieBIZ.Agronica_Log_Invio_Chiamate_W

        Dim DatiAttivita As String = ""
        If pacchettoDaInviare IsNot Nothing Then
            pacchettoDaInviare.dati = AgroZip.DeCompressioneBase64(1, pacchettoDaInviare.dati)

            Dim tzh As New JsonSerializerSettings With {.DateFormatString = "yyyy-MM-ddT00:00:00Z"}
            DatiAttivita = JsonConvert.SerializeObject(pacchettoDaInviare, tzh)
        End If

        objlog_invioChiamate.Scrivi_Log_Invio_Ricette(enum_Esportazioni_Sistema_Cod.Demetra_Export_Attivita, DatiAttivita, lstRicette, chiave_esterna, TipoOperazione, Esito, Dati_Ricevuti, objParametri_Server, giasContext, UtilizzaTransazione:=False, Data_Invio:=Data_Invio)

    End Sub

    Private Sub Chiama_Scrivi_Log_Invio_Agenda(pacchettoDaInviare As ImportDemetra, lstAgende As List(Of Tuple(Of String, Integer, String)), TipoOperazione As enum_TipoOperazioneDB, Esito As String, Dati_Ricevuti As String, Data_Invio As DateTime, objParametri_Server As AgronicaCoreParametri, giasContext As Gias_DeveloperServer_Entities)

        Dim objLogInvioChiamate As New AgronicaCoreVarieBIZ.Agronica_Log_Invio_Chiamate_W

        Dim DatiAttivita As String = ""
        If pacchettoDaInviare IsNot Nothing Then
            pacchettoDaInviare.dati = AgroZip.DeCompressioneBase64(1, pacchettoDaInviare.dati)

            Dim tzh As New JsonSerializerSettings With {.DateFormatString = "yyyy-MM-ddT00:00:00Z"}
            DatiAttivita = JsonConvert.SerializeObject(pacchettoDaInviare, tzh)
        End If

        objLogInvioChiamate.Scrivi_Log_Invio_Agenda(enum_Esportazioni_Sistema_Cod.Demetra_Export_Attivita, DatiAttivita, lstAgende, 0, TipoOperazione, Esito, Dati_Ricevuti, objParametri_Server, giasContext, UtilizzaTransazione:=False, Data_Invio:=Data_Invio)

    End Sub

    Private Sub Update_Log_Invio_Chiamate(ID As Integer, pacchettoDaInviare As ImportDemetra, TipoOperazione As enum_TipoOperazioneDB, Esito As String, Dati_Ricevuti As String, Data_Invio As DateTime, objParametri_Server As AgronicaCoreParametri)

        Dim objLogInvioChiamate As New AgronicaCoreVarieBIZ.Agronica_Log_Invio_Chiamate_W

        Dim DatiAttivita As String = ""
        If pacchettoDaInviare IsNot Nothing Then
            pacchettoDaInviare.dati = AgroZip.DeCompressioneBase64(1, pacchettoDaInviare.dati)

            Dim tzh As New JsonSerializerSettings With {.DateFormatString = "yyyy-MM-ddT00:00:00Z"}
            DatiAttivita = JsonConvert.SerializeObject(pacchettoDaInviare, tzh)

        End If

        objLogInvioChiamate.Update_Log_Invio_Chiamate(ID, enum_Esportazioni_Sistema_Cod.Demetra_Export_Attivita, DatiAttivita, TipoOperazione, Esito, Dati_Ricevuti, objParametri_Server, Data_Invio:=Data_Invio)

    End Sub

End Class
