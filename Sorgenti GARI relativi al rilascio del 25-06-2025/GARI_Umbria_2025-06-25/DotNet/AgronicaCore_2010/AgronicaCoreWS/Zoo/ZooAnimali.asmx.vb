Imports System.ComponentModel
Imports System.Web.Services
Imports AgronicaCoreDataProvider
Imports AgronicaCoreEntityFramework
Imports AgronicaCoreModello.AppHelper
Imports AgronicaCoreModelsSTD
Imports AgronicaCoreModelsSTD.anagrafiche
Imports AgronicaCoreModelsSTD.pianiDiCampionamento
Imports AgronicaCorePianidiCampionamentoBiz
Imports AgronicaCoreUtility
Imports AgronicaCoreVarieBIZ
Imports Newtonsoft.Json

' Per consentire la chiamata di questo servizio Web dallo script utilizzando ASP.NET AJAX, rimuovere il commento dalla riga seguente.
<System.Web.Script.Services.ScriptService()>
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")>
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<ToolboxItem(False)>
Public Class ZooAnimali
    Inherits System.Web.Services.WebService

    <WebMethod()>
    <Script.Services.ScriptMethod()>
    Public Function CaricaElencoStalle(ByVal InData As CoreWS_Generic(Of AgronicaCoreDTOStd.InData.Zoo.LeggiStalle)) As rispostaStandard(Of List(Of anagrafiche.Stalla))
        Dim r As New rispostaStandard(Of List(Of anagrafiche.Stalla))
        Dim objParametri_Server As AgronicaCoreParametri = Utility.convertStringtoOBJparametri(InData.objP.objP_server)

        Dim gefutils As New Gias_EF_Utility
        Dim EFConnString As String = gefutils.GetEntityConnectionString(objParametri_Server.StringaConnessione)
        Dim GiasContext As New Gias_DeveloperServer_Entities(EFConnString)

        Dim paramsLeggiStalle = InData.InData

        Try
            Dim objFabbricati As New AgronicaCoreAnagrafeDAL.Fabbricati_R
            'Dim listaStalle As New List(Of AgronicaCoreEntityFramework_POCO.Stalla)
            Dim dtStalle As New DataTable

            Dim piva As String = paramsLeggiStalle.impresa.partitaIva
            Dim saCod As Integer = 0
            Dim staNum As Integer = 0
            If Not IsNothing(paramsLeggiStalle.centro) AndAlso Not IsNothing(paramsLeggiStalle.centro.primaryKey) AndAlso paramsLeggiStalle.centro.primaryKey.codice <> 0 Then
                saCod = paramsLeggiStalle.centro.primaryKey.codice

                If Not IsNothing(paramsLeggiStalle.stalla) AndAlso Not IsNothing(paramsLeggiStalle.stalla.primaryKey) AndAlso paramsLeggiStalle.stalla.primaryKey.codice <> 0 Then
                    staNum = paramsLeggiStalle.stalla.primaryKey.codice
                End If
            End If

            dtStalle = objFabbricati.LeggiStalle_x_anagrafica(piva, saCod, staNum, "", "", objParametri_Server)

            Dim listaStalleSTD As New List(Of anagrafiche.Stalla)
            If Not IsNothing(dtStalle) AndAlso dtStalle.Rows.Count > 0 Then
                For Each st In dtStalle.Rows
                    Dim chiaveStalla As String = st("chiave")
                    Dim pivaStalla As String = chiaveStalla.Split("_")(0)
                    Dim saCodStalla As Integer = chiaveStalla.Split("_")(1)
                    Dim staNumStalla As Integer = chiaveStalla.Split("_")(2)

                    Dim stallaSTD As New anagrafiche.Stalla(pivaStalla, saCodStalla, staNumStalla, st("Fabbricato"))
                    'Dim fabbStalla = (From f In GiasContext.Fabbricati Where f.PIVA = st.PIVA And f.SA_COD = st.sa_cod And f.Fabbricato_Cod = st.STA_NUM).FirstOrDefault

                    'stallaSTD.primaryKey = New FabbricatoLight.PK With {
                    '	.codice = staNumStalla,
                    '	.centroAziendalePK = New anagrafiche.CentroAziendale.PK(pivaStalla, st.PIVA)
                    '}
                    'stallaSTD.descrizione = st.STA_DES

                    'INDIRIZZO
                    'If paramsLeggiStalle.getIndirizzo Then
                    '	Dim indirizzo = (From i In GiasContext.Indirizzi Where i.cod_indirizzo = fabbStalla.Indirizzo_Cod).FirstOrDefault
                    '	stallaSTD.indirizzo = New Indirizzo With {
                    '		.codice = indirizzo.cod_indirizzo,
                    '		.istatComune = New metaschema.Istat(),
                    '		.via = indirizzo.ind_des,
                    '		.frazione = indirizzo.frz_des,
                    '		.cap = indirizzo.CAP,
                    '		.stato = New metaschema.CodiciNazioniISO3166(indirizzo.stato),
                    '		.note = indirizzo.note,
                    '		.flag_cancellazione = False
                    '	}
                    'End If

                    stallaSTD.tipo = st("Tipo_Fabbricato_Cod")
                    'stallaSTD.tipo = fabbStalla.Tipo_Fabbricato_Cod
                    'stallaSTD.volumeConvenzionale = 0
                    stallaSTD.flag_cancellazione = False
                    'stallaSTD.validita = New IntervalloTemporale(st.Validita_Inizio, st.Validita_Fine)
                    'stallaSTD.specie = New metaschema.utilizzi.Specie(st.SPE_COD)
                    'stallaSTD.indirizzoProd = New metaschema.IndirizzoProduttivo(st.IPRO_COD)
                    'stallaSTD.tipoRicovero = New metaschema.TipoRicovero()
                    'stallaSTD.sottotipoRicovero = New metaschema.SottotipoRicovero()

                    'RAGGRUPPAMENTI
                    'If paramsLeggiStalle.getRaggruppamenti Then
                    '	stallaSTD.gruppiAnimali = New List(Of SottogruppoStalla)
                    '	Dim raggruppamenti = (From sr In GiasContext.Stalla_Raggruppamenti Where sr.PIVA = st.PIVA And sr.sa_cod = st.sa_cod And sr.STA_NUM = st.STA_NUM).ToList
                    '	For Each raggr In raggruppamenti
                    '		Dim raggrSTD As New SottogruppoStallaLight
                    '		raggrSTD.nome = raggr.Raggruppamento_Des
                    '		raggrSTD.codice = raggr.Raggruppamento_Cod
                    '		raggrSTD.stallaPK = stallaSTD.primaryKey
                    '		'raggrSTD.tipo = New metaschema.TipoGruppo_Zoo(raggr.Raggruppamento_Tipo)
                    '		'raggrSTD.validita = New IntervalloTemporale(raggr.Validita_Inizio, raggr.Validita_Fine)
                    '		'raggrSTD.area = raggr.Mq
                    '		'raggrSTD.specie = stallaSTD.specie
                    '		'raggrSTD.razza = New metaschema.Razza(raggr.RAZ_COD)
                    '		'raggrSTD.stato = New metaschema.TipologiaCapoAnimale(raggr.STATO_COD)

                    '		stallaSTD.gruppiAnimali.Add(raggrSTD)
                    '	Next
                    'End If

                    listaStalleSTD.Add(stallaSTD)
                Next
            End If


            r.RispostaStringa = listaStalleSTD
            r.RispostaOK = True

        Catch ex As Exception
            r.Errore = "Errore durante l'operazione: " & vbCrLf & Gestione_Eccezioni_2015.MessaggioCompletoDataEccezione(ex, True, source:=True)
            r.RispostaOK = False
        End Try

        Return r

    End Function

    <WebMethod()>
    <Script.Services.ScriptMethod()>
    Public Function CaricaElencoRaggruppamenti(ByVal InData As CoreWS_Generic(Of AgronicaCoreDTOStd.InData.Zoo.LeggiSottogruppiStalla)) As rispostaStandard(Of List(Of anagrafiche.SottogruppoStallaLight))
        Dim r As New rispostaStandard(Of List(Of anagrafiche.SottogruppoStallaLight))
        Dim objParametri_Server As AgronicaCoreParametri = Utility.convertStringtoOBJparametri(InData.objP.objP_server)

        Dim gefutils As New Gias_EF_Utility
        Dim EFConnString As String = gefutils.GetEntityConnectionString(objParametri_Server.StringaConnessione)
        Dim GiasContext As New Gias_DeveloperServer_Entities(EFConnString)

        Dim paramsLeggiRaggruppamento = InData.InData

        Try
            Dim objRaggr_Stalla As New AgronicaCoreAnagrafeDAL.Stalla_Raggruppamenti_R
            'Dim listaRaggruppamenti As New List(Of AgronicaCoreEntityFramework_POCO.Stalla_Raggruppamenti)
            Dim dtRaggr As New DataTable

            Dim piva As String = paramsLeggiRaggruppamento.impresa.partitaIva
            Dim saCod As Integer = 0
            Dim staNum As Integer = 0
            Dim raggrCod As Integer = 0
            If Not IsNothing(paramsLeggiRaggruppamento.centro) AndAlso Not IsNothing(paramsLeggiRaggruppamento.centro.primaryKey) AndAlso paramsLeggiRaggruppamento.centro.primaryKey.codice <> 0 Then
                saCod = paramsLeggiRaggruppamento.centro.primaryKey.codice

                If Not IsNothing(paramsLeggiRaggruppamento.stalla) AndAlso Not IsNothing(paramsLeggiRaggruppamento.stalla.primaryKey) AndAlso paramsLeggiRaggruppamento.stalla.primaryKey.codice <> 0 Then
                    staNum = paramsLeggiRaggruppamento.stalla.primaryKey.codice
                    If Not IsNothing(paramsLeggiRaggruppamento.raggruppamento) AndAlso paramsLeggiRaggruppamento.raggruppamento.codice <> 0 Then
                        raggrCod = paramsLeggiRaggruppamento.raggruppamento.codice
                    End If
                End If
            End If

            dtRaggr = objRaggr_Stalla.Leggi_x_anagrafica(objParametri_Server.PivaSuperUser, piva, saCod, staNum, raggrCod,
                                                         "", "", objParametri_Server)

            Dim listaraggruppamentiSTD As New List(Of anagrafiche.SottogruppoStallaLight)
            If Not IsNothing(dtRaggr) AndAlso dtRaggr.Rows.Count > 0 Then
                For Each raggr In dtRaggr.Rows
                    Dim raggrSTD As New SottogruppoStallaLight(raggr("piva"), raggr("sa_cod"), raggr("STA_NUM"), raggr("raggruppamento_cod"), raggr("raggruppamento_des"))

                    'raggrSTD.tipo = New metaschema.TipoGruppo_Zoo(raggr.Raggruppamento_Tipo)
                    'raggrSTD.validita = New IntervalloTemporale(raggr.Validita_Inizio, raggr.Validita_Fine)
                    'raggrSTD.area = raggr.Mq
                    'raggrSTD.specie = New metaschema.utilizzi.Specie(raggr.SPE_COD)
                    'raggrSTD.razza = New metaschema.Razza(raggr.RAZ_COD)
                    'raggrSTD.stato = New metaschema.TipologiaCapoAnimale(raggr.STATO_COD)

                    listaraggruppamentiSTD.Add(raggrSTD)
                Next
            End If

            r.RispostaStringa = listaraggruppamentiSTD
            r.RispostaOK = True

        Catch ex As Exception
            r.Errore = "Errore durante l'operazione: " & vbCrLf & Gestione_Eccezioni_2015.MessaggioCompletoDataEccezione(ex, True, source:=True)
            r.RispostaOK = False
        End Try

        Return r

    End Function

    <WebMethod()>
    <Script.Services.ScriptMethod()>
    Public Function CaricaGiacenzeZoo(ByVal InData As CoreWS_Generic(Of AgronicaCoreDTOStd.InData.Zoo.LeggiGiacenzeZoo)) As rispostaStandard(Of List(Of Zoo.GiacenzaZoo))
        Dim r As New rispostaStandard(Of List(Of Zoo.GiacenzaZoo))
        Dim objParametri_Server As AgronicaCoreParametri = Utility.convertStringtoOBJparametri(InData.objP.objP_server)
        Dim objParametri_Utenti As AgronicaCoreParametri = Utility.convertStringtoOBJparametri(InData.objP.objP_utenti)
        Dim paramsLeggiGiacenze = InData.InData

        Dim gefutils As New Gias_EF_Utility
        Dim EFConnString As String = gefutils.GetEntityConnectionString(objParametri_Server.StringaConnessione)
        Dim GiasContext As New Gias_DeveloperServer_Entities(EFConnString)

        Try
            Dim piva As String = ""
            Dim saCod As Integer = 0
            Dim staNum As Integer = 0
            Dim raggrCod As Integer = 0
            Dim codAnimale As Integer = 0
            Dim matricola As String = ""
            Dim dataGiacenza As Date = Nothing
            Dim listaCodAnimali As List(Of Integer) = Nothing

            Dim filtroAll As Boolean = False
            Dim filtroVisibilitaUtente As Boolean = False
            Dim filtraGiacenze As Boolean = True
            Dim filtraFornitori As Boolean = False

            'controllo parametri
            If Not IsNothing(paramsLeggiGiacenze.impresa) AndAlso paramsLeggiGiacenze.impresa.partitaIva <> "" Then
                piva = paramsLeggiGiacenze.impresa.partitaIva
            End If
            If piva <> "" AndAlso Not IsNothing(paramsLeggiGiacenze.centro) AndAlso Not IsNothing(paramsLeggiGiacenze.centro.primaryKey) AndAlso paramsLeggiGiacenze.centro.primaryKey.codice <> 0 Then
                saCod = paramsLeggiGiacenze.centro.primaryKey.codice
            End If
            If saCod <> 0 AndAlso Not IsNothing(paramsLeggiGiacenze.stalla) AndAlso Not IsNothing(paramsLeggiGiacenze.stalla.primaryKey) AndAlso paramsLeggiGiacenze.stalla.primaryKey.codice <> 0 Then
                staNum = paramsLeggiGiacenze.stalla.primaryKey.codice
            End If
            If staNum <> 0 AndAlso Not IsNothing(paramsLeggiGiacenze.raggruppamento) AndAlso paramsLeggiGiacenze.raggruppamento.codice <> 0 Then
                raggrCod = paramsLeggiGiacenze.raggruppamento.codice
            End If
            If Not IsNothing(paramsLeggiGiacenze.codAnimale) AndAlso paramsLeggiGiacenze.codAnimale <> 0 Then
                codAnimale = paramsLeggiGiacenze.codAnimale
            End If
            If Not IsNothing(paramsLeggiGiacenze.matricola) AndAlso paramsLeggiGiacenze.matricola <> "" Then
                matricola = paramsLeggiGiacenze.matricola
            End If
            If Not IsNothing(paramsLeggiGiacenze.data) AndAlso IsDate(paramsLeggiGiacenze.data) Then
                dataGiacenza = paramsLeggiGiacenze.data
            End If
            If paramsLeggiGiacenze.codAnimale = 0 AndAlso Not IsNothing(paramsLeggiGiacenze.lista_CodAnimale) AndAlso paramsLeggiGiacenze.lista_CodAnimale.Count > 0 Then
                listaCodAnimali = paramsLeggiGiacenze.lista_CodAnimale
            End If

            'controllo filtri
            If Not IsNothing(paramsLeggiGiacenze.bAll) Then
                filtroAll = paramsLeggiGiacenze.bAll
            End If

            '''Commentato, per ora filtro sempre per visibilità utente
            'If Not IsNothing(paramsLeggiGiacenze.filtroVisibilitaUtente) Then
            '	filtroVisibilitaUtente = paramsLeggiGiacenze.filtroVisibilitaUtente
            'End If
            Dim objProfilo As New AgronicaCoreUtentiDAL.Utenti_Profili_Read
            Dim DTProfilo As DataTable
            Dim Sql_Permessi As String = ""


            DTProfilo = objProfilo.Leggi(
                objParametri_Utenti.UtenteUsername,
                5,
                AgronicaCoreDataProvider.AgronicaCoreParametri.enumSelezioneVariabile.Selezione_TabellaCompleta,
                "",
                "",
                objParametri_Utenti
            )



            If DTProfilo.Rows.Count > 0 Then
                Sql_Permessi = DTProfilo.Rows(0).Item("Descrizione_2") & ""
            End If
            '----------------------------------------------------------------

            Dim Filtro_Visibilita_Utente = True
            If Sql_Permessi = "" Then
                Filtro_Visibilita_Utente = False
            End If


            '''Commentato, per ora filtro sempre la giacenza
            'If Not IsNothing(paramsLeggiGiacenze.filtraGiacenze) Then
            '	filtraGiacenze = paramsLeggiGiacenze.filtraGiacenze
            'End If
            filtraGiacenze = True

            If Not IsNothing(paramsLeggiGiacenze.filtraFornitori) Then
                filtraFornitori = paramsLeggiGiacenze.filtraFornitori
            End If

            Dim objZoo_R As New AgronicaCoreAnagrafeDAL.Zoo_Animali
            Dim dtGiacenzeZoo As DataTable = objZoo_R.Leggi_Giacenze(piva, saCod, staNum, raggrCod,
                                                                     codAnimale, dataGiacenza, objParametri_Server,
                                                                     filtroAll, Filtro_Visibilita_Utente,
                                                                     listaCodAnimali, filtraGiacenze, filtraFornitori,
                                                                     mostraPesate:=False,
                                                                     mostraAnomalie:=True,
                                                                     xFiltroAggiuntivo:="",
                                                                     Matricola:="",
                                                                     MostraGGPrimoCaricamento:=paramsLeggiGiacenze.mostraGGPrimoCaricamento)

            Dim lista_Anomalie = (From lp In GiasContext.Zoo_Animali_Anomalie Select lp.Codice, lp.Descrizione).ToDictionary(Function(x) x.Codice, Function(y) y.Descrizione)

            r.RispostaStringa = New List(Of Zoo.GiacenzaZoo)
            If Not IsNothing(dtGiacenzeZoo) AndAlso dtGiacenzeZoo.Rows.Count > 0 Then
                For Each row In dtGiacenzeZoo.Rows
                    Dim giacenza As New Zoo.GiacenzaZoo

                    Dim capo As New CapoAnimaleLight(row("Piva"), row("Cod_Animale"), row("Matricola"))
                    capo.nome = row("Nome")
                    capo.sesso = row("Sesso")
                    capo.flagCancellazione = False
                    capo.dataNascita = CDate(row("Dat_Nascita"))

                    capo.genere = New metaschema.Genere(CInt(row("GEN_COD")))
                    capo.specie = New metaschema.utilizzi.Specie(CInt(row("SPE_COD")), row("SPE_DES"))
                    'capo.indirizzoProd = New metaschema.IndirizzoProduttivo(CInt(row("IPRO_COD")), row("IPRO_DES"))
                    capo.razza = New metaschema.Razza(CInt(row("RAZ_COD")), row("RAZ_DES"))
                    'capo.categoria = New metaschema.Categoria()
                    'capo.tipologia = New metaschema.TipologiaCapoAnimale(CInt(row("TIPO_COD")), row("Tipo_Des"))

                    'MADRE CAPO
                    'capo.madre = New CapoAnimale()
                    'capo.madre.matricola = IIf(IsDBNull(row("Mat_Madre")), "", row("Mat_Madre"))
                    'capo.madre.razza = New metaschema.Razza(CInt(row("RazCod_Madre")), row("RazDes_Madre"))

                    'PADRE CAPO
                    'capo.padre = New CapoAnimale()
                    'capo.padre.matricola = IIf(IsDBNull(row("Mat_Padre")), "", row("Mat_Padre"))
                    'capo.padre.razza = New metaschema.Razza(CInt(row("RazCod_Padre")), row("RazDes_Padre"))

                    'METODO PRODUZIONE
                    'capo.metodoProduzione = New metaschema.MetodoProduzione
                    'capo.metodoProduzione.descrizione = row("Metodo_produzione")
                    capo.validita = New IntervalloTemporale(CDate(row("Validita_Inizio")), CDate(row("Validita_Fine")))

                    'FORNITORE
                    'capo.fornitore = New Contatto()
                    'capo.fornitore.primaryKey = New Contatto.PK(capo.partitaIva, row("CF_Fornitore"))
                    'capo.lottoFornitore = row("Lotto_Fornitore")
                    'If filtraFornitori Then
                    '	capo.fornitore.ragione_Sociale = IIf(IsDBNull(row("RagSoc_FornFatt")), "", row("RagSoc_FornFatt"))
                    'End If

                    'capo.codiceFiscaleDetentore = row("CF_DETENTORE")
                    'capo.codiceFiscaleProprietario = row("CF_PROPRIETARIO")
                    'capo.codiceAziendaNascita = ""
                    'capo.codiceAziendaFornitore = IIf(IsDBNull(row("Codice_Azienda_Fornitore")), "", row("Codice_Azienda_Fornitore"))
                    'capo.idCapo_BDN = row("Id_Capo_BDN")
                    'capo.Codice_Azienda_Uscita = IIf(IsDBNull(row("Codice_Azienda_Uscita")), "", row("Codice_Azienda_Uscita"))
                    'capo.numCertificato = row("Certificato")
                    'giacenza.flagBDN = IIf(capo.idCapo_BDN <> 0, "Si", "No")
                    'capo.stallaSvezzamento = row("Stalla_Svezzamento")
                    'capo.note = row("Note")
                    'capo.anomalieNote = row("Anomalie_Note")

                    'MODELLO 4 INGRESSO
                    'capo.ingresso_mm_id = IIf(IsDBNull(row("Modello4_Ingresso")), "", row("Modello4_Ingresso"))
                    'capo.ingresso_modello4_numero = IIf(IsDBNull(row("Modello4_Ingresso_Numero")), "", row("Modello4_Ingresso_Numero"))
                    'capo.ingresso_modello4_prenotazione = IIf(IsDBNull(row("Modello4_Ingresso_Prenotazione")), "", row("Modello4_Ingresso_Prenotazione"))
                    'capo.ingresso_modello4_data_prenotazione = row("Data_Documento_Ingresso")

                    'MODELLO 4 USCITA
                    'capo.uscita_mm_id = IIf(IsDBNull(row("Modello4_Uscita")), "", row("Modello4_Uscita"))
                    'capo.uscita_modello4_numero = IIf(IsDBNull(row("Modello4_Uscita_Numero")), "", row("Modello4_Uscita_Numero"))
                    'capo.uscita_modello4_prenotazione = IIf(IsDBNull(row("Modello4_Uscita_Prenotazione")), "", row("Modello4_Uscita_Prenotazione"))
                    'capo.uscita_modello4_data_prenotazione = IIf(IsDBNull(row("Data_Documento_Uscita")), "", row("Data_Documento_Uscita"))

                    'STATO ACCRESCIMENTO
                    'capo.statiAccrescimento = New List(Of StatoAccrescimento)
                    'Dim capoStatoAccr As New StatoAccrescimento(CInt(row("Stato_Cod")), row("Stato_Des"))
                    'capo.statiAccrescimento.Add(capoStatoAccr)

                    'ESERCIZIO
                    capo.esercizi = New List(Of EsercizioCapoAnimale)
                    Dim capoEsercizio As New EsercizioCapoAnimale()
                    capoEsercizio.codice = row("Cod_Progetto")
                    capoEsercizio.descrizione = row("Codice_Distinta")
                    capoEsercizio.codice_capo_animale = capo.codice
                    capoEsercizio.progettoNome = row("Codice_Distinta")
                    capoEsercizio.validita = New IntervalloTemporale()
                    capo.esercizi.Add(capoEsercizio)

                    'DATI ANOMALIE
                    If Not IsDBNull(row("Anomalie")) AndAlso CStr(row("Anomalie")) <> "" Then
                        Dim str_Anomalie = CStr(row("Anomalie"))
                        Dim anomalieArr As String() = str_Anomalie.Split(",")
                        Dim lista_Anomalie_capo = New List(Of AnomalieCapoAnimale)
                        For Each anomalia In anomalieArr
                            Dim anomaliaDesc = lista_Anomalie.Item(CInt(anomalia))
                            lista_Anomalie_capo.Add(New AnomalieCapoAnimale(CInt(anomalia), anomaliaDesc, capo.codice))
                        Next
                        capo.anomalie = lista_Anomalie_capo
                    Else
                        capo.anomalie = New List(Of AnomalieCapoAnimale)
                    End If

                    'Anomalie_Note
                    If Not IsDBNull(row("Anomalie_Note")) AndAlso CStr(row("Anomalie_Note")) <> "" Then
                        capo.anomalieNote = CStr(row("Anomalie_Note"))
                    Else
                        capo.anomalieNote = ""
                    End If
                    giacenza.capo = capo
                    giacenza.primaryKey = New Zoo.GiacenzaZoo.PK(capo.partitaIva, capo.codice)

                    'CENTRO AZIENDALE
                    giacenza.centro = New CentroAziendaleLight(New anagrafiche.CentroAziendale.PK(CInt(row("Sa_Cod")), capo.partitaIva))
                    giacenza.centro.nome = row("sa_nome")

                    'STALLA
                    giacenza.stalla = New FabbricatoLight(capo.partitaIva, CInt(row("Sa_Cod")), row("STA_NUM"), row("STA_DES"))
                    'giacenza.stalla.primaryKey = New FabbricatoLight.PK With {
                    '	.codice = row("STA_NUM"),
                    '	.centroAziendalePK = giacenza.centro.primaryKey
                    '}
                    'giacenza.stalla.descrizione = row("STA_DES")

                    'RAGGRUPPAMENTO
                    giacenza.raggruppamento = New SottogruppoStallaLight(capo.partitaIva, CInt(row("Sa_Cod")), row("STA_NUM"), CInt(row("Raggruppamento_Cod")), row("Raggruppamento_Des"))
                    'giacenza.raggruppamento.codice = CInt(row("Raggruppamento_Cod"))
                    'giacenza.raggruppamento.nome = row("Raggruppamento_Des")
                    'giacenza.raggruppamento.stallaPK = giacenza.stalla.primaryKey

                    'UNITA DI MISURA
                    'giacenza.unitaMisura = New metaschema.UnitaDiMisura(CInt(row("Udm_Cod")))
                    'giacenza.unitaMisura.descrizione = row("Udm_Des")
                    'giacenza.unitaMisura.simbolo = row("Udm_Sim")

                    giacenza.lotto = row("Codice_Distinta")
                    giacenza.giorniInStalla = CInt(row("giorni_in_stalla"))
                    giacenza.auslAzNascita = row("AUSL_AZI_NASCITA")

                    If paramsLeggiGiacenze.mostraGGPrimoCaricamento Then
                        giacenza.giorniInStalla_primoCaricamento = If(Not IsDBNull(row("giorni_stalla_primo_caricamento")), row("giorni_stalla_primo_caricamento"), "0")
                        giacenza.dataPrimoCaricamento = If(Not IsDBNull(row("data_primo_caricamento")) AndAlso IsDate(row("data_primo_caricamento")), CDate(row("data_primo_caricamento")), CostantiPersonalizzate.AGRODATAINIZIO)
                    End If

                    'DATI FORNITORI
                    'If filtraFornitori Then
                    '	giacenza.fornFatt_CF = IIf(IsDBNull(row("CF_FornFatt")), "", row("CF_FornFatt"))
                    '	giacenza.fornFatt_RagSoc = IIf(IsDBNull(row("RagSoc_FornFatt")), "", row("RagSoc_FornFatt"))
                    '	giacenza.fornProv_CF = IIf(IsDBNull(row("CF_FornProv")), "", row("CF_FornProv"))
                    '	giacenza.fornProv_RagSoc = IIf(IsDBNull(row("RagSoc_FornProv")), "", row("RagSoc_FornProv"))
                    'End If
                    'giacenza.fornProv_Contatto = IIf(IsDBNull(row("Fornitore_Provenienza")), "", row("Fornitore_Provenienza"))

                    'giacenza.numBolla_Forn = IIf(IsDBNull(row("N_Bolla_Fornitore")), "", row("N_Bolla_Fornitore"))
                    'giacenza.numBolla_Uscita = IIf(IsDBNull(row("N_Bolla_Uscita")), "", row("N_Bolla_Uscita"))
                    'giacenza.dataIngressoDDT = IIf(IsDBNull(row("Data_DDT_Ingresso")), Nothing, row("Data_DDT_Ingresso"))
                    'giacenza.dataUscitaDDT = IIf(IsDBNull(row("Data_DDT_Uscita")), Nothing, row("Data_DDT_Uscita"))

                    giacenza.utenteCreazione = row("Utente_Creazione")
                    giacenza.dataCreazione = CDate(row("Data_Creazione"))
                    giacenza.utenteModifica = row("Utente_Modifica")
                    giacenza.dataModifica = CDate(row("Data_Modifica"))

                    r.RispostaStringa.Add(giacenza)
                Next

                r.RispostaOK = True
            End If

        Catch ex As Exception
            r.Errore = "Errore durante l'operazione: " & vbCrLf & Gestione_Eccezioni_2015.MessaggioCompletoDataEccezione(ex, True, source:=True)
            r.RispostaOK = False
        End Try

        Return r

    End Function


    <WebMethod()>
    <Script.Services.ScriptMethod()>
    Public Function ScriviOperazioneZoo(InData As Object) As RispostaStandard

        Dim r As New RispostaStandard

        Try

            Dim datiRequest As String = JsonConvert.SerializeObject(InData)
            Dim objRequest = JsonConvert.DeserializeObject(Of CoreWS_Generic(Of AgronicaCoreModelsSTD.attivita.Attivita))(datiRequest)
            Dim objParametri_Super_Server As AgronicaCoreParametri = Utility.convertStringtoOBJparametri(objRequest.objP.objP_super_server)
            Dim objParametri_Server As AgronicaCoreParametri = Utility.convertStringtoOBJparametri(objRequest.objP.objP_server)
            Dim objParametri_Utenti As AgronicaCoreParametri = Utility.convertStringtoOBJparametri(objRequest.objP.objP_utenti)
            Dim objSincroHelper As New AgronicaCoreMapper.SincroAppHelper(objParametri_Super_Server, objParametri_Server, objParametri_Utenti)

            ' imposta lingua utente
            Dim lingua = objSincroHelper.ImpostaLingua()
            Dim operazione As AgronicaCoreModelsSTD.attivita.Attivita = objRequest.InData
            Dim piva As String = operazione.centroAziendale.primaryKey.partitaIva
            Dim codice As String = operazione.codice
            Dim unid As String = operazione.guid
            Dim cancellato As Boolean = operazione.cancellato
            Dim aggiornamento As Boolean = False
            Dim riferimento As String = ""
            Dim errore As String = ""

            If String.IsNullOrEmpty(unid) Then
                unid = Guid.NewGuid().ToString()
            Else
                aggiornamento = True
            End If

            ' importazione dati app
            Dim dati As String = JsonConvert.SerializeObject(operazione)
            objSincroHelper.SincroDatiApp(unid, enum_Dati_App.AttivitaZoo, dati, riferimento, aggiornamento, cancellato, True, piva, codice, userAgent:=objRequest.objP.user_Agent, erroreImport:=errore)

            r.RispostaStringa = unid
            r.RispostaOK = True
            r.Errore = errore

        Catch ex As Exception

            r.RispostaOK = False
            r.Errore = "Errore durante l'operazione: " & Gestione_Eccezioni.MessaggioCompletoDataEccezione(ex, False)

        End Try

        Return r

    End Function

    <WebMethod()>
    <Script.Services.ScriptMethod()>
    Public Function LeggiPianiCampionamento(ByVal InData As CoreWS_Generic(Of InData.Zoo.LeggiPianiCampionamento)) As rispostaStandard(Of List(Of PianoDiCampionamento))
        Dim r As New rispostaStandard(Of List(Of PianoDiCampionamento))
        Dim objParametri_Server As AgronicaCoreParametri = Utility.convertStringtoOBJparametri(InData.objP.objP_server)

        'Dim gefutils As New Gias_EF_Utility
        'Dim EFConnString As String = gefutils.GetEntityConnectionString(objParametri_Server.StringaConnessione)
        'Dim GiasContext As New Gias_DeveloperServer_Entities(EFConnString)

        Dim piva As String = ""
        Dim saCod As Integer = 0
        Dim staNum As Integer = 0
        Dim paramsLeggiPianiCampionamento = InData.InData
        Dim listaPianiCampionamento As New List(Of PianoDiCampionamento)

        Try

            If paramsLeggiPianiCampionamento IsNot Nothing Then
                If paramsLeggiPianiCampionamento.impresa IsNot Nothing Then
                    piva = paramsLeggiPianiCampionamento.impresa.partitaIva
                End If
                If paramsLeggiPianiCampionamento.stalla IsNot Nothing Then
                    piva = paramsLeggiPianiCampionamento.stalla.partitaIva
                    saCod = paramsLeggiPianiCampionamento.stalla.saCod
                    staNum = paramsLeggiPianiCampionamento.stalla.fabbricato_cod
                End If
                If paramsLeggiPianiCampionamento.data > CostantiPersonalizzate.AGRODATAINIZIO Then
                    objParametri_Server.FinestraTemporaleInizio = paramsLeggiPianiCampionamento.data
                    objParametri_Server.FinestraTemporaleFine = paramsLeggiPianiCampionamento.data
                End If
            End If

            'legge i PDC filtrando per i parametri passati
            Dim dt = PDC_Testata_Helper.Leggi_PDC_APP(piva, saCod, staNum, objParametri_Server)

            For Each pdc In dt.Rows
                'Dim statoPDC = AgronicaCorePianidiCampionamentoDAL.PDC_R.GetStato(pdc.Item("PDC_Stato"))
                Dim pdcSTD As New PianoDiCampionamento With {
                    .codice = pdc.Item("Id_PDC_Testata"),
                    .descrizione = pdc.Item("PDC_Testata_Des"),
                    .data_istantanea = pdc.Item("PDC_Data_Istantanea"),
                    .impresa = New RifImpresa(pdc.Item("PivaOwner")),
                    .centro = New RifCentroAziendale(pdc.Item("PivaOwner"), pdc.Item("Sa_CodOwner")),
                    .stalla = New RifFabbricato(pdc.Item("PivaOwner"), pdc.Item("Sa_CodOwner"), pdc.Item("Fabbricato_CodOwner")),
                    .stato = New baseClass.BaseCodeDescr(pdc.Item("PDC_Stato"), ""),
                    .validita = New IntervalloTemporale(pdc.Item("Validita_Inizio"), pdc.Item("Validita_Fine")),
                    .daCampagna = False,
                    .daZoo = True
                }
                listaPianiCampionamento.Add(pdcSTD)
            Next

            r.RispostaStringa = listaPianiCampionamento
            r.RispostaOK = True

        Catch ex As Exception
            r.Errore = "Errore durante l'operazione: " & vbCrLf & Gestione_Eccezioni_2015.MessaggioCompletoDataEccezione(ex, True, source:=True)
            r.RispostaOK = False
        End Try

        Return r

    End Function

    <WebMethod()>
    <Script.Services.ScriptMethod()>
    Public Function ScriviPianoCampionamento(InData As Object) As RispostaStandard

        Dim r As New RispostaStandard

        Try

            Dim datiRequest As String = JsonConvert.SerializeObject(InData)
            Dim objRequest = JsonConvert.DeserializeObject(Of CoreWS_Generic(Of PianoDiCampionamento))(datiRequest)
            Dim objParametri_Super_Server As AgronicaCoreParametri = Utility.convertStringtoOBJparametri(objRequest.objP.objP_super_server)
            Dim objParametri_Server As AgronicaCoreParametri = Utility.convertStringtoOBJparametri(objRequest.objP.objP_server)
            Dim objParametri_Utenti As AgronicaCoreParametri = Utility.convertStringtoOBJparametri(objRequest.objP.objP_utenti)
            Dim objSincroHelper As New AgronicaCoreMapper.SincroAppHelper(objParametri_Super_Server, objParametri_Server, objParametri_Utenti)
            Dim pdc As PianoDiCampionamento = objRequest.InData

            Dim piva As String = pdc.impresa.partitaIva
            Dim codice As String = pdc.codice
            Dim unid As String = pdc.guid
            Dim cancellato As Boolean = pdc.cancellato
            Dim aggiornamento As Boolean = False
            Dim riferimento As String = ""

            If String.IsNullOrEmpty(unid) Then
                unid = Guid.NewGuid().ToString()
                If pdc.codice > 0 Then
                    riferimento = pdc.codice
                End If
            Else
                aggiornamento = True
            End If

            ' importazione dati app
            Dim dati As String = JsonConvert.SerializeObject(pdc)
            objSincroHelper.SincroDatiApp(unid, enum_Dati_App.PianoCampionamento, dati, riferimento, aggiornamento, cancellato, True, piva, codice, userAgent:=objRequest.objP.user_Agent)

            r.RispostaStringa = unid
            r.RispostaOK = True

        Catch ex As Exception

            r.RispostaOK = False
            r.Errore = "Errore durante l'operazione: " & Gestione_Eccezioni.MessaggioCompletoDataEccezione(ex, False)

        End Try

        Return r

    End Function

    <WebMethod()>
    <Script.Services.ScriptMethod()>
    Public Function ScriviCapoAnimale(InData As Object) As RispostaStandard

        Dim r As New RispostaStandard

        Try

            Dim datiRequest As String = JsonConvert.SerializeObject(InData)
            Dim objRequest = JsonConvert.DeserializeObject(Of CoreWS_Generic(Of CapoAnimale))(datiRequest)
            Dim objParametri_Super_Server As AgronicaCoreParametri = Utility.convertStringtoOBJparametri(objRequest.objP.objP_super_server)
            Dim objParametri_Server As AgronicaCoreParametri = Utility.convertStringtoOBJparametri(objRequest.objP.objP_server)
            Dim objParametri_Utenti As AgronicaCoreParametri = Utility.convertStringtoOBJparametri(objRequest.objP.objP_utenti)
            Dim objSincroHelper As New AgronicaCoreMapper.SincroAppHelper(objParametri_Super_Server, objParametri_Server, objParametri_Utenti)
            Dim capo As CapoAnimale = objRequest.InData

            Dim piva As String = capo.partitaIva
            Dim codice As String = capo.codice
            Dim unid As String = "" 'capo.guid
            Dim cancellato As Boolean = False 'capo.cancellato
            Dim aggiornamento As Boolean = False
            Dim riferimento As String = ""

            If String.IsNullOrEmpty(unid) Then
                unid = Guid.NewGuid().ToString()
                If capo.codice > 0 Then
                    riferimento = capo.codice
                End If
            Else
                aggiornamento = True
            End If

            Dim gefutils As New Gias_EF_Utility
            Dim EFConnString As String = gefutils.GetEntityConnectionString(objParametri_Server.StringaConnessione)
            Dim GiasContext As New Gias_DeveloperServer_Entities(EFConnString)
            Dim objScrivi_Zoo As New AgronicaCoreAnagrafeBIZ.Zoo
            If Not objScrivi_Zoo.Aggiorna_CapoAnimale(capo, GiasContext, objParametri_Server) Then
                r.Errore = "Errore aggiornamento capo con matricola " & capo.matricola
                r.RispostaOK = False
                Return r
            End If

            ' importazione dati app
            Dim dati As String = JsonConvert.SerializeObject(capo)
            objSincroHelper.SincroDatiApp(unid, enum_Dati_App.CapoAnimale, dati, riferimento, aggiornamento, cancellato, True, piva, codice, userAgent:=objRequest.objP.user_Agent)

            r.RispostaStringa = unid
            r.RispostaOK = True

        Catch ex As Exception

            r.RispostaOK = False
            r.Errore = "Errore durante l'operazione: " & Gestione_Eccezioni.MessaggioCompletoDataEccezione(ex, False)

        End Try

        Return r

    End Function

End Class