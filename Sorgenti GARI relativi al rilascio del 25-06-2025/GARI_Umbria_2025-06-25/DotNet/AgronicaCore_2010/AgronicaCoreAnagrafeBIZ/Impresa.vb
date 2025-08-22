Imports System.Data.Entity.Infrastructure
Imports System.Linq
Imports System.Transactions
Imports System.Xml
Imports System.Xml.Linq
Imports AgronicaCoreAnagrafeDAL
Imports AgronicaCoreDataProvider
Imports AgronicaCoreDataProvider.AgronicaCoreParametri
Imports AgronicaCoreDataProvider.CostantiPersonalizzate
Imports AgronicaCoreDataProvider.My.Resources
Imports AgronicaCoreDataProvider.TipiEnumerativi
Imports AgronicaCoreDataProvider.UtilityProvider
Imports AgronicaCoreDTOStd.InData.Metaschema
Imports AgronicaCoreEntityFramework
Imports AgronicaCoreEntityFramework_POCO
Imports AgronicaCoreModelsSTD.anagrafiche
Imports AgronicaCoreModelsSTD.baseClass
Imports AgronicaCoreUtility
Imports AgronicaCoreVarieBIZ
Imports ClosedXML.Excel
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports System.Net
Imports AgronicaCoreModelsSTD.Gis

Public Class Impresa_R
    Inherits AgronicaCoreDataProvider.LogProvider

    Public Function VerificaEsistenzaImpresaByPiva(ByVal PivaCF As String,
                                                     ByRef objParametri_server As AgronicaCoreParametri) As Boolean

        Dim xRead As New AgronicaCoreAnagrafeDAL.Imprese_Read
        Dim DT As DataTable

        DT = xRead.Leggi(PivaCF,
                         enumSelezioneVariabile.Selezione_TabellaCompleta,
                         "",
                         "",
                         objParametri_server)

        If DT Is Nothing OrElse DT.Rows.Count = 0 Then
            Return False
        End If

        Return True

    End Function

    Public Function VerificaEsistenzaImpresaByCUAA(ByVal CUAA As String,
                                                   ByRef objParametri_server As AgronicaCoreParametri) As Boolean

        Dim xRead As New AgronicaCoreAnagrafeDAL.Imprese_Codici_Read
        Dim Piva As String

        Piva = xRead.Piva_from_CUAA(CUAA, objParametri_server)

        Return Not Piva.Equals("")

    End Function

    ''' <summary>
    ''' Ottiene una lista di p.iva presenti nel filtro utente ma assenti in tabella imprese
    ''' </summary>
    Public Function VerificaSituazioneImpreseDataVisibilitàUtente(ByVal user As String,
                                                                  ByVal listaStatiDaEsculdere As String,
                                                                  ByVal objParametri_server As AgronicaCoreParametri,
                                                                  ByVal objParametri_utenti As AgronicaCoreParametri
                                                                  ) As List(Of String)

        Try

            Dim xLeggiImprese As New AgronicaCoreAnagrafeDAL.Imprese_Read
            Dim xLeggiVisibilita As New AgronicaCoreUtentiDAL.Utenti_Profili_Read

            Dim dtProfili As DataTable =
                xLeggiVisibilita.Leggi(user, 5, enumSelezioneVariabile.Selezione_TabellaDatiMinimi, "", "", objParametri_utenti)

            Dim listaImpreseTrovate As List(Of String)
            Dim listaImpreseDaFiltro As List(Of String)
            Dim filtroImpostato As String = ""
            Dim filtroXml As String = ""

            If dtProfili.Rows.Count > 0 Then

                filtroImpostato = dtProfili.Rows(0)("Descrizione_2")

                If filtroImpostato = "" Then
                    Return New List(Of String)
                End If

                filtroXml = dtProfili.Rows(0)("Descrizione_1")


                listaImpreseTrovate = leggiImpreseViaFiltrone(objParametri_server, filtroImpostato)

                listaImpreseDaFiltro = impostaVisibilitaUtente_LeggiPiveDaXml(filtroXml)

                If listaImpreseTrovate.Count = 0 Then
                    Return listaImpreseDaFiltro
                End If


                If listaImpreseDaFiltro.Count = 1 AndAlso String.IsNullOrEmpty(listaImpreseDaFiltro.FirstOrDefault) Then
                    Return New List(Of String)
                End If

                listaImpreseDaFiltro.Remove(String.Empty)

                '' VAnni: 18/8/2017: gestione attraverso stati, se non vengono passati come parametro, basta un dt vuoto.
                Dim praticheR As New AgronicaCoreProfilazioneDAL.Pratiche_R
                Dim dtPratica As DataTable = Nothing
                If listaStatiDaEsculdere <> "" Then
                    dtPratica = praticheR.Leggi_conStatoAttuale(0, "", "", "", 0, 0, 0, 1050, 0, AGRODATAINIZIO, AGRODATAFINE, " Pratiche_Stati.Stato_Cod in (" & listaStatiDaEsculdere & ") and Pratiche.Piva in ( " & listaImpreseTrovateFiltro(listaImpreseTrovate) & ")", "", objParametri_server, 0, 0)
                End If

                If dtPratica IsNot Nothing Then

                    For Each drPratica In dtPratica.Rows
                        listaImpreseDaFiltro.Remove(drPratica("piva"))
                    Next

                End If


                Return listaImpreseDaFiltro


            Else
                Return New List(Of String)
            End If

        Catch ex As Exception

            Dim MessaggioErrore As String = Gestione_Eccezioni_2015.MessaggioCompletoDataEccezione(ex, True, source:=True)

            Scrivi_LOG(objParametri_server, "verificaSituazioneImpresaDataVisibilitàutente", MessaggioErrore)

            'passo l'eccezione corrente al throw come inner exception ..:
            Throw New Exception("[verificaSituazioneImpresaDataVisibilitàutente] : " & MessaggioErrore, ex)

        End Try

    End Function

    Private Function listaImpreseTrovateFiltro(ByVal listaImpreseTrovate As List(Of String)) As String

        Dim newList As New List(Of String)

        For Each cImp In listaImpreseTrovate
            newList.Add("'" & cImp & "'")
        Next

        Return String.Join(",", newList)

    End Function

    Private Shared Function leggiImpreseViaFiltrone(objParametri_server As AgronicaCoreParametri, filtroImpostato As String) As List(Of String)

        Dim listaImprese As New List(Of String)

        Dim classJoin As New JoinFiltrone
        classJoin.bGerarchiaImprese = True
        classJoin.bImpreseXIndirizzi = True
        classJoin.bIstat = True
        classJoin.bListaProvince = True
        classJoin.bIndirizzi = True
        classJoin.bCentriAziendali = True
        classJoin.bCentrixIndirizzi = True
        classJoin.bIndirizziCentro = True
        classJoin.bIstatCentro = True
        classJoin.bListaProvinceCentro = True
        classJoin.bAppezzamento = True
        classJoin.bRegImpianti = True
        classJoin.bImpreseProgetti = True
        classJoin.bImpreseCodici = True
        classJoin.bCentriAziendaliCodici = True
        classJoin.bAppezzamentoCodici = True
        classJoin.bRegImpiantiCodici = True
        classJoin.bCampi = True
        classJoin.bAppezzamentixParticelle = True
        classJoin.bGruppoVarietale = True
        classJoin.bCultivar = True
        classJoin.bSpecieVegetali = True
        classJoin.bGruppoVegetale = True

        classJoin.bMovDestinazioni = True
        classJoin.bAgenda = True
        classJoin.bMovimenti = True
        classJoin.bMovDettaglioTecnico = True
        classJoin.bMovimentiDettagli = True
        classJoin.bOperazioni = True
        classJoin.bGruppoOperazioni = True
        classJoin.bRisorseUmane = True
        classJoin.bContatti = True
        classJoin.bRapportiContabili = True

        Dim RsImprese As DataTable
        Dim i As Integer
        Dim Filtrone As New Filtrone

        '---------------------------------
        'INSERIMENTO IMPRESE VISIBILI
        '---------------------------------
        RsImprese = Filtrone.CreaDTFiltrone(objParametri_server, filtroImpostato, CInt(2), "", classJoin)
        If RsImprese IsNot Nothing AndAlso RsImprese.Rows.Count > 0 Then
            Dim drImprese As DataRow() = RsImprese.Select(" piva is not null ")
            If drImprese IsNot Nothing AndAlso drImprese.Length > 0 Then
                For i = 0 To drImprese.Length - 1
                    If Not listaImprese.Contains(drImprese(i).Item("piva")) Then
                        listaImprese.Add(drImprese(i).Item("piva"))

                    End If
                    'RsImprese.MoveNext()
                Next
            End If
        End If

        Return listaImprese
    End Function

    Private Function impostaVisibilitaUtente_LeggiPiveDaXml(ByVal sXml As String) As List(Of String)

        Dim xDocPivas As XDocument = XDocument.Parse(sXml)

        Dim rval As List(Of String) = (From ll In xDocPivas.Elements("DatiFiltri").Elements("Filtro").Elements("Impresa") Where CStr(ll.Attribute("piva")) <> "abc" Select CStr(ll.Attribute("piva"))).ToList

        Return rval

    End Function

    Private Function VerificaFiltroImprese(ByVal filtro As String) As Boolean
        Return True
    End Function

    Public Function Impresa_Leggi(ByVal Piva As String,
                                  ByVal ForDelete As Boolean,
                                  ByVal AllAttributes As Boolean,
                                  ByRef objParametri As AgronicaCoreParametri,
                                  Optional ByVal TipoG2G As Integer = 0,
                                  Optional ByVal xFiltroAggiuntivo As String = "",
                                  Optional ByVal xOrderBy As String = ""
                                  ) As String

        Dim NomeRoutine As String = "AnagrafeBIZ.Impresa_R.Impresa_Leggi()"

        '====================================================================================
        'Parametri opzionali :
        '   Piva = ""                   =>  si leggono tutte le imprese dell'utente
        '====================================================================================

        Dim MessaggioErrore As String = ""

        Dim i As Integer

        Dim XmlDoc As XmlDocument

        Dim XmlDatiImprese As XmlElement
        Dim XmlImpresa As XmlElement
        Dim XmlIndirizzo As XmlElement
        Dim XmlCodice As XmlElement

        Dim objUtentixImprese As New AgronicaCoreAnagrafeDAL.UtentixImprese_Read
        Dim objImpresexIndirizzi As New AgronicaCoreAnagrafeDAL.ImpresexIndirizzi_R
        Dim objImpresexCodici As New AgronicaCoreAnagrafeDAL.Imprese_Codici_Read

        Dim DtImprese As DataTable
        Dim DtIndirizzi As DataTable
        Dim DtCodici As DataTable

        Dim RowCountImprese As Integer
        Dim RowCountImpresexIndirizzi As Integer
        Dim RowCountImpreseCodici As Integer

        Dim RisultatoFunzione As String

        '------------------------------
        Dim FlagConnessioneLocale As Boolean = False



        Try

            '------------------------------
            'Verifico se e' stata impostata una connessione
            If IsNothing(objParametri.objConnessione) Then
                FlagConnessioneLocale = True
                objParametri.objConnessione = DataProviderFactory.Instance.CreaNuovaConnessione(objParametri.StringaConnessione)
            End If
            '------------------------------


            '#################################
            '##########  IMPRESA  ############
            '#################################

            DtImprese = objUtentixImprese.Leggi(Piva,
                                                enumSelezioneVariabile.Selezione_TabellaCompleta,
                                                xFiltroAggiuntivo,
                                                xOrderBy,
                                                objParametri,
                                                TipoG2G)

            'Se ottengo almeno un risultato, creo la struttura XML
            If Not IsNothing(DtImprese) Then

                '----- < Documento XML > -----
                XmlDoc = New XmlDocument

                XmlDatiImprese = XmlDoc.CreateElement("DatiImprese")

                'Effettuo un ciclo sulle imprese
                RowCountImprese = 0
                Do While RowCountImprese <= DtImprese.Rows.Count - 1

                    '----- < IMPRESA > -----
                    XmlImpresa = XmlDoc.CreateElement("Impresa")

                    i = RowCountImprese   'Alias

                    With XmlImpresa
                        .SetAttribute("TipoOperazioneDB", IIf(ForDelete, "3", "0"))
                        .SetAttribute("piva", Agro_SQL_Load(DtImprese.Rows(i).Item("PIVA")))
                        .SetAttribute("rag_soc", Agro_SQL_Load(DtImprese.Rows(i).Item("rag_soc")))

                        'Se voglio tutti gli attributi della tabella
                        If AllAttributes Then
                            .SetAttribute("delega", Agro_SQL_Load(DtImprese.Rows(i).Item("Delega")))
                            .SetAttribute("at_prevalente", Agro_SQL_Load(DtImprese.Rows(i).Item("AT_Prevalente")))
                            .SetAttribute("forma_giuridica", Agro_SQL_Load(DtImprese.Rows(i).Item("Forma_Giuridica")))
                            .SetAttribute("forma_conduzione", Agro_SQL_Load(DtImprese.Rows(i).Item("Forma_Conduzione")))
                            .SetAttribute("sup_totale", Agro_SQL_Load(DtImprese.Rows(i).Item("Sup_Totale")))
                            .SetAttribute("note", Agro_SQL_Load(DtImprese.Rows(i).Item("Blk_Inizio_Note")))

                            .SetAttribute("validazione", Agro_SQL_Load(DtImprese.Rows(i).Item("Validazione")))
                            .SetAttribute("data_validazione", Agro_SQL_Load(DtImprese.Rows(i).Item("Data_Validazione")))
                            .SetAttribute("username_validazione", Agro_SQL_Load(DtImprese.Rows(i).Item("UserName_Validazione")))
                            .SetAttribute("blk_flag", Agro_SQL_Load(DtImprese.Rows(i).Item("Blk_Flag")))
                            .SetAttribute("blk_inizio_data", Agro_SQL_Load(DtImprese.Rows(i).Item("Blk_Inizio_Data")))
                            .SetAttribute("blk_inizio_username", Agro_SQL_Load(DtImprese.Rows(i).Item("Blk_Inizio_Username")))
                            .SetAttribute("blk_fine_data", Agro_SQL_Load(DtImprese.Rows(i).Item("Blk_Fine_Data")))
                            .SetAttribute("blk_fine_username", Agro_SQL_Load(DtImprese.Rows(i).Item("Blk_Fine_Username")))
                            .SetAttribute("blk_fine_note", Agro_SQL_Load(DtImprese.Rows(i).Item("Blk_Fine_Note")))
                        End If

                        .SetAttribute("tipoimpresagerarchia", Agro_SQL_Load(DtImprese.Rows(i).Item("TipoImpresaGerarchia")))
                        .SetAttribute("validita_inizio", Agro_SQL_Load(DtImprese.Rows(i).Item("Validita_Inizio")))
                        .SetAttribute("validita_fine", Agro_SQL_Load(DtImprese.Rows(i).Item("Validita_Fine")))

                        .SetAttribute("data_creazione", Agro_SQL_Load(DtImprese.Rows(i).Item("Data_Creazione")))
                        .SetAttribute("data_modifica", Agro_SQL_Load(DtImprese.Rows(i).Item("Data_Modifica")))
                        .SetAttribute("username_creazione", Agro_SQL_Load(DtImprese.Rows(i).Item("Username_Creazione")))
                        .SetAttribute("username_modifica", Agro_SQL_Load(DtImprese.Rows(i).Item("Username_Modifica")))
                    End With


                    '#################################
                    '##########  INDIRIZZI  ##########
                    '#################################

                    DtIndirizzi = objImpresexIndirizzi.Leggi(Agro_SQL_Load(DtImprese.Rows(RowCountImprese).Item("PIVA")),
                                                             0,
                                                             0,
                                                             enumSelezioneVariabile.Selezione_JoinCompleta,
                                                             "",
                                                             "",
                                                             objParametri)

                    'Se ottengo almeno un risultato, creo la struttura XML
                    If Not IsNothing(DtIndirizzi) Then

                        RowCountImpresexIndirizzi = 0
                        For i = 0 To DtIndirizzi.Rows.Count - 1

                            '----- < INDIRIZZO > -----
                            XmlIndirizzo = XmlDoc.CreateElement("Indirizzo")

                            With XmlIndirizzo
                                .SetAttribute("TipoOperazioneDB", IIf(ForDelete, "3", "0"))
                                .SetAttribute("tipo_indirizzo", Agro_SQL_Load(DtIndirizzi.Rows(i).Item("Tipo_Indirizzo")))
                                .SetAttribute("cod_indirizzo", Agro_SQL_Load(DtIndirizzi.Rows(i).Item("cod_indirizzo")))
                                .SetAttribute("ind_des", Agro_SQL_Load(DtIndirizzi.Rows(i).Item("ind_des")))
                                .SetAttribute("frz_des", Agro_SQL_Load(DtIndirizzi.Rows(i).Item("frz_des")))
                                .SetAttribute("cap", Agro_SQL_Load(DtIndirizzi.Rows(i).Item("CAP")))
                                .SetAttribute("com_des", Agro_SQL_Load(DtIndirizzi.Rows(i).Item("com_des")))
                                .SetAttribute("pro_cod", Agro_SQL_Load(DtIndirizzi.Rows(i).Item("pro_cod")))
                                .SetAttribute("pro_des", Agro_SQL_Load(DtIndirizzi.Rows(i).Item("pro_des")))
                                .SetAttribute("stato", Agro_SQL_Load(DtIndirizzi.Rows(i).Item("stato")))
                                .SetAttribute("note", Agro_SQL_Load(DtIndirizzi.Rows(i).Item("note")))
                                .SetAttribute("pro_cod_istat", Agro_SQL_Load(DtIndirizzi.Rows(i).Item("pro_cod_istat")))
                                .SetAttribute("com_cod_istat", Agro_SQL_Load(DtIndirizzi.Rows(i).Item("com_cod_istat")))
                                .SetAttribute("validita_inizio", Agro_SQL_Load(DtIndirizzi.Rows(i).Item("validita_inizio")))
                                .SetAttribute("validita_fine", Agro_SQL_Load(DtIndirizzi.Rows(i).Item("validita_fine")))

                                .SetAttribute("validazione", Agro_SQL_Load(DtIndirizzi.Rows(i).Item("Validazione")))
                                .SetAttribute("data_validazione", Agro_SQL_Load(DtIndirizzi.Rows(i).Item("Data_Validazione")))
                                .SetAttribute("username_validazione", Agro_SQL_Load(DtIndirizzi.Rows(i).Item("UserName_Validazione")))
                                .SetAttribute("codice_lingua", Agro_SQL_Load(DtIndirizzi.Rows(i).Item("Codice_Lingua")))
                                .SetAttribute("codice_alternativo", Agro_SQL_Load(DtIndirizzi.Rows(i).Item("Codice_Alternativo")))

                                .SetAttribute("data_creazione", Agro_SQL_Load(DtIndirizzi.Rows(i).Item("Data_Creazione")))
                                .SetAttribute("data_modifica", Agro_SQL_Load(DtIndirizzi.Rows(i).Item("Data_Modifica")))
                                .SetAttribute("username_creazione", Agro_SQL_Load(DtIndirizzi.Rows(i).Item("Username_Creazione")))
                                .SetAttribute("username_modifica", Agro_SQL_Load(DtIndirizzi.Rows(i).Item("Username_Modifica")))
                            End With

                            XmlImpresa.AppendChild(XmlIndirizzo)
                            '----- < / INDIRIZZO > -----

                        Next

                        DtIndirizzi.Dispose()
                        XmlIndirizzo = Nothing

                    End If



                    '#################################
                    '##########  CODICI  #############
                    '#################################

                    DtCodici = objImpresexCodici.Leggi(Agro_SQL_Load(DtImprese.Rows(RowCountImprese).Item("PIVA")),
                                                       0,
                                                       enumSelezioneVariabile.Selezione_TabellaCompleta,
                                                       "",
                                                       "",
                                                       objParametri)

                    'Se ottengo almeno un risultato, creo la struttura XML
                    If Not IsNothing(DtCodici) Then

                        RowCountImpreseCodici = 0
                        Do While RowCountImpreseCodici <= DtCodici.Rows.Count - 1

                            '----- < CODICE > -----
                            XmlCodice = XmlDoc.CreateElement("Codice")

                            i = RowCountImpreseCodici   'Alias

                            With XmlCodice
                                .SetAttribute("TipoOperazioneDB", IIf(ForDelete, "3", "0"))
                                .SetAttribute("id_cod", Agro_SQL_Load(DtCodici.Rows(i).Item("id_cod")))
                                .SetAttribute("val_cod", Agro_SQL_Load(DtCodici.Rows(i).Item("val_cod")))
                                .SetAttribute("descrizione", Agro_SQL_Load(DtCodici.Rows(i).Item("descrizione")))
                                .SetAttribute("validita_inizio", Agro_SQL_Load(DtCodici.Rows(i).Item("validita_inizio")))
                                .SetAttribute("validita_fine", Agro_SQL_Load(DtCodici.Rows(i).Item("validita_fine")))

                                .SetAttribute("validazione", Agro_SQL_Load(DtCodici.Rows(i).Item("Imprese_Codici_Validazione")))
                                .SetAttribute("data_validazione", Agro_SQL_Load(DtCodici.Rows(i).Item("Imprese_Codici_Data_Validazione")))
                                .SetAttribute("username_validazione", Agro_SQL_Load(DtCodici.Rows(i).Item("Imprese_Codici_UserName_Validazione")))

                                .SetAttribute("data_creazione", Agro_SQL_Load(DtCodici.Rows(i).Item("Imprese_Codici_Data_creazione")))
                                .SetAttribute("data_modifica", Agro_SQL_Load(DtCodici.Rows(i).Item("Imprese_Codici_data_modifica")))
                                .SetAttribute("username_creazione", Agro_SQL_Load(DtCodici.Rows(i).Item("Imprese_Codici_username_creazione")))
                                .SetAttribute("username_modifica", Agro_SQL_Load(DtCodici.Rows(i).Item("Imprese_Codici_username_modifica")))
                            End With

                            XmlImpresa.AppendChild(XmlCodice)
                            '----- < / CODICE > -----

                            RowCountImpreseCodici += 1

                        Loop

                        DtCodici.Dispose()
                        XmlCodice = Nothing

                    End If


                    '#################################
                    '#################################
                    '#################################



                    XmlDatiImprese.AppendChild(XmlImpresa)
                    '----- < / IMPRESA > -----

                    RowCountImprese += 1

                Loop

                DtImprese.Dispose()
                XmlImpresa = Nothing

                XmlDoc.AppendChild(XmlDatiImprese)
                '----- < / Documento XML > -----

                RisultatoFunzione = XmlDoc.OuterXml
                XmlDatiImprese = Nothing
                XmlDoc = Nothing

            Else

                'Altrimenti, se non risulta selezionata nessuna impresa ...
                RisultatoFunzione = ""

            End If

            '------------------------------



        Catch ex As Exception
            RisultatoFunzione = ""
            MessaggioErrore = ex.Message
            Scrivi_LOG(objParametri, NomeRoutine, MessaggioErrore)
            Throw New Exception("[" & NomeRoutine & "] : " & MessaggioErrore)
        Finally

            'Pulizia
            objUtentixImprese = Nothing
            objImpresexCodici = Nothing
            objImpresexIndirizzi = Nothing

            If FlagConnessioneLocale Then
                objParametri.objConnessione.Close()
                objParametri.objConnessione.Dispose()
            End If
        End Try

        'Restituisco il risultato
        Return RisultatoFunzione

    End Function

    Public Function Impresa_Leggi_Anagrafica(ByVal Piva As String,
                                             ByVal Leggi_Indirizzo As Boolean,
                                             ByVal Leggi_Padri As Boolean,
                                             ByVal Leggi_Contatti As Boolean,
                                             ByVal Leggi_Contatto_Superuser As Boolean,
                                             ByVal Leggi_Codici As Boolean,
                                             ByRef objParametri_Server As AgronicaCoreParametri,
                                             ByRef Optional objParametri_Utenti As AgronicaCoreParametri = Nothing,
                                             ByRef Optional objParametri_Super_Server As AgronicaCoreParametri = Nothing) As AgronicaCoreModelsSTD.anagrafiche.Impresa

        Dim impresa As New AgronicaCoreModelsSTD.anagrafiche.Impresa
        Dim DT_Impresa As DataTable

        If Piva = "" Then
            Throw New Exception("Piva Obbligatoria")
        End If

        Dim objImpreseR As New AgronicaCoreAnagrafeDAL.Imprese_Read
        DT_Impresa = objImpreseR.DatiIntestazioneImpresa(Piva, "", "", objParametri_Server)

        If DT_Impresa.Rows.Count > 0 Then

            impresa.partitaIva = DT_Impresa.Rows(0)("Piva")
            impresa.ragioneSociale = DT_Impresa.Rows(0)("Rag_Soc")

            If Not IsDBNull(DT_Impresa.Rows(0)("Forma_Giuridica_Cod")) AndAlso IsNumeric(DT_Impresa.Rows(0)("Forma_Giuridica_Cod")) Then
                impresa.forma_Giuridica = New AgronicaCoreModelsSTD.metaschema.FormeGiuridiche(DT_Impresa.Rows(0)("Forma_Giuridica_Cod")) With {
                    .descrizione = DT_Impresa.Rows(0)("Forma_Giuridica_Des")
                }
            End If

            impresa.tipo_Impresa = CInt(DT_Impresa.Rows(0)("TipoImpresaGerarchia"))

            impresa.validita = New AgronicaCoreModelsSTD.anagrafiche.IntervalloTemporale(CDate(DT_Impresa.Rows(0)("Validita_Inizio")),
                                                                                         CDate(DT_Impresa.Rows(0)("Validita_Fine")))

            Dim objImpreseCodici As New AgronicaCoreAnagrafeDAL.Imprese_Codici_Read
            impresa.CUAA = objImpreseCodici.Leggi_CUAA(Piva, objParametri_Server)

            Dim objContatti As New AgronicaCoreAnagrafeBIZ.RisorseUmane_R
            Dim tecnico = objImpreseCodici.Leggi_Codice_from_Imprese_Codici(Piva, enum_CodiciAnagrafe.Tecnico, objParametri_Server)
            If tecnico <> "" Then
                ' l'errore nella lettura del tecnico da Angular sta qui, passo una Piva sbagliata
                Dim listTecnico = objContatti.Leggi_Tecnici("", tecnico, objParametri_Server)
                If listTecnico.Count > 0 Then
                    impresa.tecnicoReferente = listTecnico(0)
                End If

            End If

            Dim odc As String = objImpreseCodici.Leggi_Codice_from_Imprese_Codici(Piva, enum_CodiciAnagrafe.Organismo_di_Controllo, objParametri_Server)
            If odc <> "" AndAlso IsNumeric(odc) Then
                Dim risorse_umane_R As New AgronicaCoreAnagrafeBIZ.RisorseUmane_R
                impresa.organismo_di_Controllo = risorse_umane_R.Leggi(CInt(odc), False, objParametri_Server)
            End If

            If Leggi_Padri Then
                impresa.impresaPadre = Leggi_Padri_Impresa(Piva, objParametri_Server)
            End If

            If Leggi_Indirizzo Then
                Dim IndirizzoBIZ_R As New AgronicaCoreAnagrafeBIZ.Indirizzi_R
                impresa.indirizzi = IndirizzoBIZ_R.Leggi_Indirizzi_Associati_Impresa(Piva, objParametri_Server)
            End If

            If Leggi_Contatti Then
                impresa.contatti = Leggi_Contatti_Impresa(impresa.partitaIva, objParametri_Server)
            End If

            If Leggi_Contatto_Superuser Then
                impresa.contatto_superuser = Leggi_Contatto_Impresa_Superuser(impresa.partitaIva, objParametri_Server)
            End If

            If Leggi_Codici Then
                Dim objCodici As New AgronicaCoreAnagrafeBIZ.Codici_R
                impresa.codici = objCodici.Leggi_Codici_Impresa(impresa.partitaIva,
                                                                objParametri_Server,
                                                                New List(Of enum_CodiciAnagrafe)({
                                                                                                 enum_CodiciAnagrafe.Organismo_di_Controllo,
                                                                                                 enum_CodiciAnagrafe.Tecnico,
                                                                                                 enum_CodiciAnagrafe.Codice_Libro_Soci,
                                                                                                 enum_CodiciAnagrafe.Data_Iscrizione_Libro_Soci,
                                                                                                 enum_CodiciAnagrafe.CodiceCUAA,
                                                                                                 enum_CodiciAnagrafe.Codice_Ausl,
                                                                                                 enum_CodiciAnagrafe.TitoloPossesso,
                                                                                                 enum_CodiciAnagrafe.Disciplinare_Aziendale_Default,
                                                                                                 enum_CodiciAnagrafe.PivaSuperUser_Origine_Dato,
                                                                                                 enum_CodiciAnagrafe.Codice_Certificazione}),
                                                                True)
            End If

            impresa.certificazione = Leggi_Certificazione(impresa.partitaIva, objParametri_Server)

            impresa.gruppoRaccolta = Leggi_GruppoRaccolta(impresa.partitaIva, objParametri_Server)

            impresa.disciplinareAziendalePredefinito = Leggi_DisciplinareAziendalePredefinito(impresa.partitaIva,
                                                                                              objParametri_Super_Server,
                                                                                              objParametri_Server,
                                                                                              objParametri_Utenti)
        End If

        Return impresa
    End Function

    Private Function Leggi_DisciplinareAziendalePredefinito(partitaIva As String,
                                                            objParametri_Super_Server As AgronicaCoreParametri,
                                                            objParametri_Server As AgronicaCoreParametri,
                                                            objParametri_Utenti As AgronicaCoreParametri) As BaseCodiceDescr
        Dim result As BaseCodiceDescr = Nothing
        Dim objDisciplinare As New AgronicaCoreAnagrafeDAL.Imprese_Codici_Read
        Dim codiceDisciplinare = objDisciplinare.Leggi_Codice_from_Imprese_Codici(CStr(partitaIva), enum_CodiciAnagrafe.Disciplinare_Aziendale_Default, objParametri_Server)
        If Not String.IsNullOrEmpty(codiceDisciplinare) Then
            Dim filtro As New LeggiVincoli
            filtro.validita = New IntervalloTemporale()
            filtro.validita.inizio = AGRODATAINIZIO
            filtro.validita.fine = AGRODATAFINE

            Dim listItems = AgronicaCoreWebService.Vincoli_WS.LeggiVincoli(filtro, objParametri_Super_Server, objParametri_Server, objParametri_Utenti)

            If (Not IsNothing(listItems) AndAlso listItems.Any()) Then
                result = listItems _
                    .Select(Function(x) New BaseCodiceDescr(code:=ConvertiCodice(x.codice),
                        descr:=x.descrizione)) _
                    .Where(Function(e) e.codice = codiceDisciplinare) _
                .FirstOrDefault()
            End If
        End If

        Return result
    End Function

    Private Function ConvertiCodice(codice As String) As String
        Dim parts = codice.Split("_")
        If (parts.Length > 1) Then
            Return $"{parts(1)}/{parts(0)}"
        Else
            Return codice
        End If
    End Function

    Public Function Imprese_Leggi_VisibilitaUtente_CUAA(objParametri_Server As AgronicaCoreParametri, objParametri_Utenti As AgronicaCoreParametri,
                                                        Optional ByVal moduli_Generazione As List(Of Integer) = Nothing) As DataTable
        Dim DT As DataTable

        Dim objProfilo As New AgronicaCoreUtentiDAL.Utenti_Profili_Read
        Dim DTProfilo As DataTable
        Dim Sql_Permessi As String = ""


        DTProfilo = objProfilo.Leggi(
                objParametri_Utenti.UtenteUsername,
                CInt(5),
                enumSelezioneVariabile.Selezione_TabellaCompleta,
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

        Dim objImprese_Dal As New AgronicaCoreAnagrafeDAL.Imprese_Read

        If Not IsNothing(moduli_Generazione) Then
            DT = objImprese_Dal.Leggi_ImpreseCUAA_Visibilita_Utente(objParametri_Server, objParametri_Utenti, Filtro_Visibilita_Utente, QueryBuilderUtility.GeneraClausolaINDaList(moduli_Generazione))
        Else
            DT = objImprese_Dal.Leggi_ImpreseCUAA_Visibilita_Utente(objParametri_Server, objParametri_Utenti, Filtro_Visibilita_Utente)
        End If

        Return DT
    End Function

    Public Function Leggi_Contatti_Impresa(Piva As String, ByRef objParametri_Server As AgronicaCoreParametri) As List(Of AgronicaCoreModelsSTD.anagrafiche.Contatto)
        Dim impresa_contatti As New List(Of AgronicaCoreModelsSTD.anagrafiche.Contatto)
        Dim objContattiR As New AgronicaCoreAnagrafeBIZ.Contatti_MultiHost_R
        Dim c = objContattiR.Leggi_Contatti_Impresa(Piva, objParametri_Server)
        Return impresa_contatti
    End Function

    Public Function Leggi_Certificazione(Piva As String, ByRef objParametri_Server As AgronicaCoreParametri) As List(Of BaseCodeDescr)
        Dim objImprese As New AgronicaCoreAnagrafeDAL.Imprese_Read
        Dim objCAC As New AgronicaCoreAnagrafeDAL.CertificazioniAziendali_R

        Dim DT As DataTable = objImprese.Leggi_Certificato(Piva, objParametri_Server)
        Dim list As List(Of BaseCodeDescr) = New List(Of BaseCodeDescr)
        If DT.Rows.Count <> 0 Then
            Dim listaCodici As List(Of Integer) = New List(Of Integer)
            For Each codice In DT.Rows(0).Item("val_cod").ToString.Split(",".ToCharArray)
                listaCodici.Add(CInt(codice))
            Next

            Dim DB As DataTable = objCAC.Leggi(listaCodici, "", AGRODATAINIZIO, AGRODATAFINE, "", "", objParametri_Server)

            For Each codice In DB.Rows
                list.Add(New BaseCodeDescr(code:=CInt(codice.Item("CA_Cod")), descr:=codice.Item("CA_Des")))
            Next
        End If
        Return list
    End Function

    Public Function Leggi_GruppoRaccolta(Piva As String, ByRef objParametri_Server As AgronicaCoreParametri) As BaseCodeDescr
        Dim objImprese As New AgronicaCoreAnagrafeDAL.Gruppi_Raccolta_Read

        Dim gr As BaseCodeDescr = Nothing
        Dim DT As DataTable = objImprese.LeggiGruppoRaccoltaImpresa(Piva, objParametri_Server, "")
        If DT.Rows.Count <> 0 Then
            gr = New BaseCodeDescr(code:=DT.Rows(0).Item("GruppoRaccolta_Cod"), descr:=DT.Rows(0).Item("GruppoRaccolta_Des"))
        End If
        Return gr
    End Function

    Public Function Leggi_Certificazioni_Disponibili(ByRef objParametri_Server As AgronicaCoreParametri) As List(Of BaseCodeDescr)
        Dim objCAC As New AgronicaCoreAnagrafeDAL.CertificazioniAziendali_R

        Dim DT As DataTable = objCAC.Leggi(New List(Of Integer), "", AGRODATAINIZIO, AGRODATAFINE, "", "", objParametri_Server)
        Dim list As List(Of BaseCodeDescr) = New List(Of BaseCodeDescr)

        If DT.Rows.Count <> 0 Then
            For Each codice As DataRow In DT.Rows
                list.Add(New BaseCodeDescr(code:=CInt(codice.Item("CA_Cod")), descr:=codice.Item("CA_Des")))
            Next
        End If
        Return list
    End Function

    Public Function Leggi_Contatto_Impresa_Superuser(Piva As String, ByRef objParametri_Server As AgronicaCoreParametri) As AgronicaCoreModelsSTD.anagrafiche.Contatto
        Dim objContattiR As New AgronicaCoreAnagrafeBIZ.Contatti_MultiHost_R

        Dim c = objContattiR.Leggi_Contatto(objParametri_Server.PivaSuperUser, Piva, False, False, objParametri_Server)

        Return c
    End Function

    Public Function Leggi_Padri_Impresa(Piva As String, ByRef objParametri_Server As AgronicaCoreParametri) As List(Of AgronicaCoreModelsSTD.anagrafiche.ImpresaPadre)
        Dim objGerarchia As New AgronicaCoreAnagrafeDAL.GerarchiaImprese_R
        Dim TempDt As DataTable

        'Leggo le imprese padri associate al profilo selezionato			
        TempDt = objGerarchia.LeggiPadriGerarchia_AncheVuoti("", Piva, 0, 0, AGRODATAINIZIO, AGRODATAFINE, "", "", objParametri_Server)

        Dim padri = New List(Of AgronicaCoreModelsSTD.anagrafiche.ImpresaPadre)
        If TempDt IsNot Nothing AndAlso TempDt.Rows.Count > 0 Then
            For Each rowPadre In TempDt.Rows
                Dim padre = New AgronicaCoreModelsSTD.anagrafiche.ImpresaPadre
                padre.partitaIva = rowPadre("Padre")
                padre.ragioneSociale = rowPadre("RagSoc_Padre")
                padre.codice_iscrizione_libro_soci = rowPadre("LibroSoci_Codice")
                padre.data_iscrizione_libro_soci = IIf(rowPadre("LibroSoci_DataIscrizione").Equals(""), New Date(1900, 1, 1), rowPadre("LibroSoci_DataIscrizione"))
                padri.Add(padre)
            Next
        End If

        Return padri
    End Function

    'Dim objContattiR As New AgronicaCoreAnagrafeBIZ.Contatti_MultiHost_R

    'Dim c = objContattiR.Leggi_Contatto(objParametri_Server.PivaSuperUser, Piva, False, False, objParametri_Server)

    '    Return c
    'End Function

    Public Function Leggi_Max_Data_Modifica(objParametri_Server As AgronicaCoreParametri,
                                            objParametri_Utenti As AgronicaCoreParametri) As DateTime
        Using GiasContext As Gias_DeveloperServer_Entities = Gias_EF_Utility.CreateGiasContextConnection(objParametri_Server.StringaConnessione)

            Dim objImprese As New AgronicaCoreAnagrafeDAL.Imprese_Read

            Dim objProfilo As New AgronicaCoreUtentiDAL.Utenti_Profili_Read
            Dim DTProfilo As DataTable
            Dim Sql_Permessi As String = ""


            DTProfilo = objProfilo.Leggi(objParametri_Utenti.UtenteUsername,
                                         5,
                                         enumSelezioneVariabile.Selezione_TabellaCompleta,
                                         "", "",
                                         objParametri_Utenti)

            If DTProfilo.Rows.Count > 0 Then
                Sql_Permessi = DTProfilo.Rows(0).Item("Descrizione_2") & ""
            End If
            '----------------------------------------------------------------

            Dim Filtro_Visibilita_Utente = True
            If Sql_Permessi = "" Then
                Filtro_Visibilita_Utente = False
            End If
            Dim maxData = objImprese.Leggi_Max_DataModifica("",
                                                        objParametri_Server,
                                                        objParametri_Utenti,
                                                        Filtro_Visibilita_Utente)

            Dim maxLogEls = GiasContext.Agronica_Log_Anagrafe.Where(Function(row) row.Tipo = "Imprese").Select(Function(row) row.Data_Ora_RegistrazioneLog).Max()
            Dim maxLog = If(maxLogEls, AGRODATAINIZIO)

            Dim DateList As New List(Of DateTime)({maxData, maxLog})



            Return DateList.Max()

        End Using
    End Function

    Public Function Leggi_Imprese_APP(tipo As String, ByRef objParametri_Server As AgronicaCoreParametri) As List(Of AgronicaCoreModelsSTD.anagrafiche.Impresa)

        Dim imprese As New List(Of Impresa)
        Dim objCodici As New AgronicaCoreAnagrafeBIZ.Codici_R
        Dim objIndirizzi As New AgronicaCoreAnagrafeBIZ.Indirizzi_R
        Dim objCentri As New AgronicaCoreAnagrafeBIZ.CentroAziendale_R
        Dim codiciEsclusi = New List(Of enum_CodiciAnagrafe) ' codici anagrafe da escludere

        Dim classFiltrone As New Filtrone
        Dim filtro As String = If(String.IsNullOrEmpty(tipo), "", "TipoImpresaGerarchia In (" & tipo & ")")
        Dim ordinamento As String = "ORDER BY Rag_Soc ASC"
        Dim classJoin As New JoinFiltrone With {.bGerarchiaImprese = True}
        Dim dtImprese = classFiltrone.CreaDTFiltrone(objParametri_Server, filtro, enum_TipoSelect_FiltroneSuperNova.Imprese, ordinamento, classJoin)

        For Each row In dtImprese.Rows

            Dim impresa =
                New Impresa() With {
                    .partitaIva = row("Piva"),
                    .ragioneSociale = row("Rag_Soc"),
                    .CUAA = If(IsDBNull(row("CodiceCuaa")), "", row("CodiceCuaa")),
                    .tipo_Impresa = row("TipoImpresaGerarchia")
                }

            impresa.impresaPadre = Leggi_Padri_Impresa(row("Piva"), objParametri_Server)
            impresa.codici = objCodici.Leggi_Codici_Impresa(row("Piva"), objParametri_Server, codiciEsclusi, True)
            impresa.indirizzi = objIndirizzi.Leggi_Indirizzi_Associati_Impresa(row("Piva"), objParametri_Server)

            If tipo <> "" Then
                impresa.centriAziendali = objCentri.Leggi_Centri_APP(row("Piva"), Today, objParametri_Server)
            End If

            imprese.Add(impresa)

        Next

        Return imprese

    End Function

    Public Function Leggi_Padre_Gerarchia(ByRef objParametri_Server As AgronicaCoreParametri, ByRef objParametri_Utenti As AgronicaCoreParametri) As String
        Dim leggiGerarchiaImprese As New GerarchiaImprese_R
        Dim gruppo_Utente_R As New AgronicaCoreUtentiDAL.Utenti_xGruppi_Utente_R
        Dim dtGruppiUtente = gruppo_Utente_R.Leggi(objParametri_Server.UtenteUsername, 0, "", "", objParametri_Utenti)
        Dim gruppo As Integer = If(dtGruppiUtente.Rows.Count > 0, dtGruppiUtente.Rows(0)("Gruppi_Utente_Cod"), 0)
        Dim dtPadriGerarchia = leggiGerarchiaImprese.LeggiPadriGerarchiaImprese(gruppo, 0, enum_Id_Servizio.GiasAPP, objParametri_Server)
        Dim padreGerarchia As String = If(dtPadriGerarchia.Rows.Count > 0, dtPadriGerarchia.Rows(0).Item("Piva"), "")
        Return padreGerarchia
    End Function

    Public Function IsAlive(ByRef objParametri_Server As AgronicaCoreParametri) As RispostaStandard
        Dim r As New RispostaStandard

        Try
            Dim imprese_R As New AgronicaCoreAnagrafeDAL.Imprese_Read
            r.RispostaOK = imprese_R.IsAlive(objParametri_Server)
            r.Errore = If(r.RispostaOK, "", Gias.DB_Server_Unreachable)
        Catch ex As Exception
            r.RispostaOK = False
            r.Errore = Gias.DB_Server_Unreachable
        End Try

        Return r

    End Function

End Class




'#############################################################################################################
'#############################################################################################################
'#############################################################################################################
'#############################################################################################################



Public Class Impresa_W
    Inherits AgronicaCoreDataProvider.DataProvider

    'Public Function Impresa_EF_Scrivi( _
    '                            ByVal DatiImpresa As AgronicaCoreEntityFramework.Imprese, _
    '                                ByRef OUTPUT_Piva As String, _
    '                                    ByRef objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri, _
    '                                    ByRef objParametri_Utenti As AgronicaCoreDataProvider.AgronicaCoreParametri _
    '                                    ) As Boolean



    '    Dim gefutils As New AgronicaCoreEntityFramework.Gias_EF_Utility

    '    Dim EFConnString As String = gefutils.GetEntityConnectionString(objParametri.StringaConnessione)

    '    Dim campTestata As New AgronicaCoreEntityFramework.Gias_DeveloperServer_Entities(EFConnString)


    '    campTestata.AddToImprese(DatiImpresa)

    '    campTestata.SaveChanges()

    '    OUTPUT_Piva = DatiImpresa.PIVA

    'End Function

    'Public Function Impresa_EF_Scrivi( _
    '                            ByVal DatiImpresa As String, _
    '                                ByRef OUTPUT_Piva As String, _
    '                                    ByRef objParametri As AgronicaCoreDataProvider.AgronicaCoreParametri, _
    '                                    ByRef objParametri_Utenti As AgronicaCoreDataProvider.AgronicaCoreParametri _
    '                                    ) As Boolean


    '    '--------- deserializza

    '    Dim gefutils As New AgronicaCoreEntityFramework.Gias_EF_Utility

    '    Dim EFConnString As String = gefutils.GetEntityConnectionString(objParametri.StringaConnessione)

    '    Dim campTestata As New AgronicaCoreEntityFramework.Gias_DeveloperServer_Entities(EFConnString)


    '    Dim ef2 As New AgronicaCoreEntityFramework.Imprese

    '    ef2 = AgronicaCoreUtility.AgroSerializer.Deserialize_Use_DataContractSerializer_XmlString(Of AgronicaCoreEntityFramework.Imprese)(DatiImpresa)


    '    campTestata.AddToImprese(ef2)


    '    campTestata.SaveChanges()        



    'End Function

    '============================================================================

    Public Function Impresa_Scrivi(ByVal DatiImpresa As String,
                                   ByRef OUTPUT_Piva As String,
                                   ByRef objParametri As AgronicaCoreParametri,
                                   ByRef objParametri_Utenti As AgronicaCoreParametri,
                                   Optional ByVal TipoG2G As Integer = 0,
                                   Optional NoteLog As String = ""
                                   ) As Boolean

        Const nomeRoutine = "AgronicaCoreAnagrafeBIZ.Impresa_W.Impresa_Scrivi()"

        Dim XmlDoc As XmlDocument

        Dim objImpresexCodici As AgronicaCoreAnagrafeDAL.Imprese_Codici_Write

        Dim objIndirizzi As AgronicaCoreAnagrafeDAL.Indirizzi_Write
        Dim objImpresexIndirizzi As AgronicaCoreAnagrafeDAL.ImpresexIndirizzi_W
        'Dim objPersone As Object
        Dim objImprese As AgronicaCoreAnagrafeDAL.Imprese_Write
        'Dim objImpresexPersone As Object
        'Dim objImpresexParticelle2 As AgronicaCoreAnagrafeDAL.ImpresexParticelle2_W
        Dim objUtentixImprese As AgronicaCoreAnagrafeDAL.UtentixImprese_Write
        'Dim objCentri As AgronicaCoreAnagrafeDAL.CentriAziendali_Write
        'Dim objCentrixIndirizzi As AgronicaCoreAnagrafeDAL.CentrixIndirizzi_Write
        'Dim objCentrixRubrica As AgronicaCoreAnagrafeDAL.CentrixRubrica_Write
        'Dim objCentri_Codici As AgronicaCoreAnagrafeDAL.Centri_Codici_Write
        'Dim objUtentixStrutture As AgronicaCoreAnagrafeDAL.UtentixStrutture_Write
        'Dim ObjCampi As AgronicaCoreAnagrafeDAL.Campi_W
        'Dim objCampixParticelle As AgronicaCoreAnagrafeDAL.CampixParticelle_W
        'Dim objUtentixCampi As AgronicaCoreAnagrafeDAL.UtentixCampi_Write
        'Dim objAppezzamenti As AgronicaCoreAnagrafeDAL.Appezzamento_Write
        'Dim objUtentixAppezzamenti As AgronicaCoreAnagrafeDAL.UtentixAppezzamenti_W
        'Dim objAppezzaxParticelle As AgronicaCoreAnagrafeDAL.AppezzaxParticelle_W
        'Dim objReg_Impianti As AgronicaCoreAnagrafeDAL.Reg_Impianti_Write
        'Dim objFabbricati As AgronicaCoreAnagrafeDAL.Fabbricati_W
        Dim objSequenze As New Agro_Sequenze
        Dim objGerarchiaImprese As AgronicaCoreAnagrafeDAL.GerarchiaImprese_W

        'Figlio Maggiore = Centro Aziendale
        Dim objCentriScrivi As AgronicaCoreAnagrafeBIZ.CentroAziendale_W
        Dim objCentriLeggi As AgronicaCoreAnagrafeBIZ.CentroAziendale_R

        Dim XmlCentri As String

        Dim ObjContattiLeggi As AgronicaCoreAnagrafeBIZ.Contatti_MultiHost_R
        Dim ObjContattiScrivi As AgronicaCoreAnagrafeBIZ.Contatti_W
        Dim ObjParco_Macchine As AgronicaCoreContabDAL.Parco_Macchine_W

        Dim Dummy As Object
        Dim Cod_Indirizzo As Long

        Dim xDatiImprese As XmlNodeList
        Dim xDatiImpresa As XmlElement
        Dim xImprese As XmlNodeList
        Dim xImpresa As XmlElement
        Dim xGerarchie As XmlNodeList
        Dim xGerarchia As XmlElement
        Dim xIndirizzi As XmlNodeList
        Dim xIndirizzo As XmlElement
        Dim xCodici As XmlNodeList
        Dim xCodice As XmlElement
        'Dim xPersone As XmlNodeList
        'Dim xPersona As XmlElement
        Dim XmlDatiContatti As XmlNodeList
        Dim XmlDatiContatto As XmlElement

        Dim i_DatiImpresa As Integer
        Dim i_Impresa As Integer
        Dim i_Gerarchia As Integer
        Dim i_Indirizzo As Integer
        Dim i_Codice As Integer
        'Dim i_Persona As Integer

        Dim OpeDB_Impresa As String
        Dim OpeDB_Gerarchia As String
        Dim OpeDB_Indirizzo As String
        Dim OpeDB_Codice As String
        ' Dim OpeDB_Persona As String

        Dim XmlContatti As String

        Dim ImpresaPadre As String
        Dim TipoImpresaGerarchia As Long

        Dim FlagTransazioneLocale As Boolean = False
        Dim FlagConnessioneLocale As Boolean = False

        Dim MessaggioErrore As String = ""
        Dim xRisp As Boolean = True
        Dim xPiva As String = ""

        Dim objAgronicaLogAnagrafeW As New AgronicaCoreAnagrafeDAL.AgronicaLogAnagrafe_W

        Try

            '------------------------------
            'Se la connessione è chiusa la apro e se la transazione è chiusa la inizio
            ConnessioniTransazioni.ApriConnessioneXCoreBiz(FlagConnessioneLocale,
                                                           FlagTransazioneLocale,
                                                           objParametri)
            '------------------------------

            XmlDoc = New XmlDocument
            XmlDoc.LoadXml(DatiImpresa)

            '------------------------------

            xDatiImprese = XmlDoc.GetElementsByTagName("DatiImprese")

            i_DatiImpresa = 0

            Do While i_DatiImpresa < xDatiImprese.Count

                'Prelevo l'i-esimo blocco di DatiImprese (in realtà ne esiste uno solo)
                xDatiImpresa = xDatiImprese.Item(i_DatiImpresa)

                '------------------------------

                xImprese = xDatiImpresa.GetElementsByTagName("Impresa")

                i_Impresa = 0

                Do While i_Impresa < xImprese.Count

                    'Prelevo la i-esima impresa
                    xImpresa = xImprese.Item(i_Impresa)

                    'Prelevo gli attributi dell'impresa selezionata
                    OpeDB_Impresa = xImpresa.GetAttribute("TipoOperazioneDB")

                    If Val(xImpresa.GetAttribute("tipoimpresagerarchia") & "") = 0 Then

                        TipoImpresaGerarchia = 1

                    Else

                        TipoImpresaGerarchia = Val(xImpresa.GetAttribute("tipoimpresagerarchia") & "")

                    End If


                    'Creo l'oggetto COM
                    objImprese = New AgronicaCoreAnagrafeDAL.Imprese_Write
                    objUtentixImprese = New AgronicaCoreAnagrafeDAL.UtentixImprese_Write
                    objGerarchiaImprese = New AgronicaCoreAnagrafeDAL.GerarchiaImprese_W

                    'Verifico l'operazione richiesta
                    Select Case OpeDB_Impresa

                        Case "0"    'LEGGI -------------------------------------------------------

                            OUTPUT_Piva = CStr(xImpresa.GetAttribute("piva"))

                        Case "1"    'SALVA -------------------------------------------------------

                            OUTPUT_Piva = CStr(xImpresa.GetAttribute("piva"))

                            Dummy = objImprese.Scrivi(xImpresa.GetAttribute("piva"),
                                                      xImpresa.GetAttribute("rag_soc"),
                                                      Agro_XML_GetString(xImpresa, "delega", ""),
                                                      Agro_XML_GetString(xImpresa, "at_prevalente", ""),
                                                      Agro_XML_GetString(xImpresa, "forma_giuridica", ""),
                                                      Agro_XML_GetString(xImpresa, "forma_conduzione", ""),
                                                      Agro_XML_GetDecimal(xImpresa, "sup_totale", 0),
                                                      TipoImpresaGerarchia,
                                                      Agro_XML_GetString(xImpresa, "note", ""),
                                                      CDate(xImpresa.GetAttribute("validita_inizio")),
                                                      CDate(xImpresa.GetAttribute("validita_fine")),
                                                      objParametri,
                                                      Data_creazione:=Agro_XML_GetDate(xImpresa, "data_creazione", #2/1/1900#),
                                                      Data_modifica:=Agro_XML_GetDate(xImpresa, "data_modifica", #2/1/1900#),
                                                      username_creazione:=Agro_XML_GetString(xImpresa, "username_creazione", ""),
                                                      username_modifica:=Agro_XML_GetString(xImpresa, "username_modifica", ""),
                                                      Validazione:=Agro_XML_GetInteger(xImpresa, "validazione", 0),
                                                      Data_Validazione:=Agro_XML_GetDate(xImpresa, "data_validazione", Now),
                                                      UserName_Validazione:=Agro_XML_GetString(xImpresa, "username_validazione", ""),
                                                      Blk_Flag:=Agro_XML_GetInteger(xImpresa, "blk_flag", 0),
                                                      Blk_Inizio_Data:=Agro_XML_GetDate(xImpresa, "blk_inizio_data", Now),
                                                      Blk_Inizio_Username:=Agro_XML_GetString(xImpresa, "blk_inizio_username", ""),
                                                      Blk_Fine_Data:=Agro_XML_GetDate(xImpresa, "blk_fine_data", Now),
                                                      Blk_Fine_Username:=Agro_XML_GetString(xImpresa, "blk_fine_username", ""),
                                                      Blk_Fine_Note:=Agro_XML_GetString(xImpresa, "blk_fine_note", "")
                                                      )

                            Dummy = objUtentixImprese.Scrivi(xImpresa.GetAttribute("piva"),
                                                             CDate(xImpresa.GetAttribute("validita_inizio")),
                                                             CDate(xImpresa.GetAttribute("validita_fine")),
                                                             objParametri,
                                                             Validazione:=Agro_XML_GetInteger(xImpresa, "validazione", 0),
                                                             Data_Validazione:=Agro_XML_GetDate(xImpresa, "data_validazione", Now),
                                                             UserName_Validazione:=Agro_XML_GetString(xImpresa, "username_validazione", "")
                                                             )

                            'Leggo il padre dell'impresa
                            ImpresaPadre = CStr(xImpresa.GetAttribute("padre") & "")

                            'Può essere utile gestire la gerarchia con i soli nodi "GerarchiaImprese"
                            If UCase(ImpresaPadre) <> "NON_IMPOSTATO" Then

                                Dummy = objGerarchiaImprese.Scrivi(ImpresaPadre,
                                                                   xImpresa.GetAttribute("piva"),
                                                                   CDate(xImpresa.GetAttribute("validita_inizio")),
                                                                   CDate(xImpresa.GetAttribute("validita_fine")),
                                                                   objParametri,
                                                                   objParametri_Utenti)

                            End If


                            '************************************************
                            '*********** INIZIO LOGGING *******************
                            '************************************************
                            objAgronicaLogAnagrafeW.Scrivi(CInt(OpeDB_Impresa),
                                                           enum_TipoEntita_Des.Imprese,
                                                           CStr(xImpresa.GetAttribute("piva")),
                                                           Nothing, Nothing, Nothing, Nothing, Nothing,
                                                           NoteLog, enum_Id_Servizio.GiasOnline, objParametri)
                            '************************************************
                            '*********** FINE LOGGING *********************
                            '************************************************


                        Case "2"    'MODIFICA -------------------------------------------------------

                            OUTPUT_Piva = CStr(xImpresa.GetAttribute("piva"))


                            Dim validazione As Integer?
                            If Not xImpresa.HasAttribute("validazione") Then
                                validazione = Nothing
                            Else
                                validazione = CInt(xImpresa.GetAttribute("validazione"))
                            End If

                            Dim datavalidazione As Date
                            If Not xImpresa.HasAttribute("data_validazione") Then
                                datavalidazione = AGRODATAINIZIO
                            Else
                                datavalidazione = CDate(xImpresa.GetAttribute("data_validazione"))
                            End If

                            Dim UserName_Validazione As String
                            If Not xImpresa.HasAttribute("username_validazione") Then
                                UserName_Validazione = Nothing
                            Else
                                UserName_Validazione = CStr(xImpresa.GetAttribute("username_validazione"))
                            End If

                            Dim Blk_Flag As Integer?
                            If Not xImpresa.HasAttribute("blk_flag") Then
                                Blk_Flag = Nothing
                            Else
                                Blk_Flag = CInt(xImpresa.GetAttribute("blk_flag"))
                            End If

                            Dim Blk_Inizio_Data As Date
                            If Not xImpresa.HasAttribute("blk_inizio_data") Then
                                Blk_Inizio_Data = AGRODATAINIZIO
                            Else
                                Blk_Inizio_Data = CDate(xImpresa.GetAttribute("blk_inizio_data"))
                            End If

                            Dim Blk_Fine_Data As Date
                            If Not xImpresa.HasAttribute("blk_fine_data") Then
                                Blk_Fine_Data = AGRODATAFINE
                            Else
                                Blk_Fine_Data = CDate(xImpresa.GetAttribute("blk_fine_data"))
                            End If

                            Dim Blk_Inizio_Username As String
                            If Not xImpresa.HasAttribute("blk_inizio_username") Then
                                Blk_Inizio_Username = Nothing
                            Else
                                Blk_Inizio_Username = CStr(xImpresa.GetAttribute("blk_inizio_username"))
                            End If

                            Dim Blk_Fine_Username As String
                            If Not xImpresa.HasAttribute("blk_fine_username") Then
                                Blk_Fine_Username = Nothing
                            Else
                                Blk_Fine_Username = CStr(xImpresa.GetAttribute("blk_fine_username"))
                            End If

                            Dim Blk_Fine_Note As String
                            If Not xImpresa.HasAttribute("blk_fine_note") Then
                                Blk_Fine_Note = Nothing
                            Else
                                Blk_Fine_Note = CStr(xImpresa.GetAttribute("blk_fine_note"))
                            End If

                            objImprese.Modifica(CStr(xImpresa.GetAttribute("piva")),
                                                CStr(xImpresa.GetAttribute("rag_soc")),
                                                Agro_XML_GetString(xImpresa, "delega", ""),
                                                Agro_XML_GetString(xImpresa, "at_prevalente", ""),
                                                Agro_XML_GetString(xImpresa, "forma_giuridica", ""),
                                                Agro_XML_GetString(xImpresa, "forma_conduzione", ""),
                                                Agro_XML_GetDecimal(xImpresa, "sup_totale", 0),
                                                TipoImpresaGerarchia,
                                                Agro_XML_GetString(xImpresa, "note", ""),
                                                CDate(xImpresa.GetAttribute("validita_inizio")),
                                                CDate(xImpresa.GetAttribute("validita_fine")),
                                                "",
                                                objParametri,
                                                Data_modifica:=Agro_XML_GetDate(xImpresa, "data_modifica", #2/1/1900#),
                                                username_modifica:=Agro_XML_GetString(xImpresa, "username_modifica", ""),
                                                Validazione:=validazione,
                                                Data_Validazione:=datavalidazione,
                                                UserName_Validazione:=UserName_Validazione,
                                                Blk_Flag:=Blk_Flag,
                                                Blk_Inizio_Data:=Blk_Inizio_Data,
                                                Blk_Inizio_Username:=Blk_Inizio_Username,
                                                Blk_Fine_Data:=Blk_Fine_Data,
                                                Blk_Fine_Username:=Blk_Fine_Username,
                                                Blk_Fine_Note:=Blk_Fine_Note
                                                )


                            '************************************************
                            '*********** INIZIO LOGGING *******************
                            '************************************************
                            objAgronicaLogAnagrafeW.Scrivi(CInt(OpeDB_Impresa),
                                                           enum_TipoEntita_Des.Imprese,
                                                           CStr(xImpresa.GetAttribute("piva")),
                                                           Nothing, Nothing, Nothing, Nothing, Nothing,
                                                           NoteLog, enum_Id_Servizio.GiasOnline, objParametri)
                            '************************************************
                            '*********** FINE LOGGING *********************
                            '************************************************

                        Case "3"    'CANCELLA -------------------------------------------------------

                            '====================================================================================================
                            '------------------------------------------------------------------------
                            'Pulitore Kendo Cache
                            '------------------------------------------------------------------------
                            Dim ObjKendo As New AgronicaCoreVarieDAL.KendoCache_W

                            Dummy = ObjKendo.CancellaxPIVA(CStr(xPiva),
                                                            objParametri)

                            ObjKendo = Nothing
                            '====================================================================================================


                            '************************************************
                            '*********** INIZIO LOGGING *******************
                            '************************************************
                            objAgronicaLogAnagrafeW.Scrivi(CInt(OpeDB_Impresa),
                                                           enum_TipoEntita_Des.Imprese,
                                                           CStr(xImpresa.GetAttribute("piva")),
                                                           Nothing, Nothing, Nothing, Nothing, Nothing,
                                                           NoteLog, enum_Id_Servizio.GiasOnline, objParametri)
                            '************************************************
                            '*********** FINE LOGGING *********************
                            '************************************************


                    End Select


                    '-------------------------------------------------------------
                    ' GERARCHIA IMPRESA
                    '-------------------------------------------------------------

                    'Prelevo l'elenco della Gerarchia
                    xGerarchie = xImpresa.GetElementsByTagName("GerarchiaImprese")

                    i_Gerarchia = 0

                    Do While i_Gerarchia < xGerarchie.Count

                        'Prelevo l'i-esima Gerarchia
                        xGerarchia = xGerarchie.Item(i_Gerarchia)

                        'Prelevo gli attributi della gerarchia selezionata
                        OpeDB_Gerarchia = xGerarchia.GetAttribute("TipoOperazioneDB")

                        'Leggo il padre dell'impresa
                        ImpresaPadre = CStr(xGerarchia.GetAttribute("padre") & "")

                        Dim objGerarchiaImprese_R As New AgronicaCoreAnagrafeDAL.GerarchiaImprese_R
                        'Verifico l'operazione richiesta
                        Select Case OpeDB_Gerarchia

                            Case "0"    'LEGGI -------------------------------------------------------

                            Case "1"    'SALVA -------------------------------------------------------

                                Dummy = objGerarchiaImprese.Scrivi(ImpresaPadre,
                                                                   xImpresa.GetAttribute("piva"),
                                                                   CDate(xImpresa.GetAttribute("validita_inizio")),
                                                                   CDate(xImpresa.GetAttribute("validita_fine")),
                                                                   objParametri,
                                                                   objParametri_Utenti)

                            Case "3"    'ELIMINA -------------------------------------------------------

                                objGerarchiaImprese.Cancella(ImpresaPadre,
                                                             xImpresa.GetAttribute("piva"),
                                                             "",
                                                             objParametri,
                                                             objParametri_Utenti)

                            Case "2"

                                Dim DTPadre = objGerarchiaImprese_R.LeggiPadriGerarchia(ImpresaPadre,
                                                                          xImpresa.GetAttribute("piva"),
                                                                          0,
                                                                          0,
                                                                          CDate(xImpresa.GetAttribute("validita_inizio")),
                                                                          CDate(xImpresa.GetAttribute("validita_fine")),
                                                                          "",
                                                                          "",
                                                                          objParametri)

                                If DTPadre.Rows.Count = 0 Then
                                    Dummy = objGerarchiaImprese.Scrivi(ImpresaPadre,
                                                                   xImpresa.GetAttribute("piva"),
                                                                   CDate(xImpresa.GetAttribute("validita_inizio")),
                                                                   CDate(xImpresa.GetAttribute("validita_fine")),
                                                                   objParametri,
                                                                   objParametri_Utenti)
                                End If


                        End Select

                        'Incremento l'indice
                        i_Gerarchia = i_Gerarchia + 1

                    Loop


                    '-------------------------------------------------------------
                    ' INDIRIZZI
                    '-------------------------------------------------------------

                    'Prelevo l'elenco degli indirizzi
                    'xIndirizzi = xImpresa.GetElementsByTagName("Indirizzo")
                    xIndirizzi = xImpresa.SelectNodes("//DatiImprese/Impresa/Indirizzo")

                    i_Indirizzo = 0

                    Do While i_Indirizzo < xIndirizzi.Count

                        'Prelevo l'i-esimo indirizzo
                        xIndirizzo = xIndirizzi.Item(i_Indirizzo)

                        'Prelevo gli attributi dell'indirizzo selezionato
                        OpeDB_Indirizzo = xIndirizzo.GetAttribute("TipoOperazioneDB")

                        'Creo l'oggetto COM
                        objImpresexIndirizzi = New AgronicaCoreAnagrafeDAL.ImpresexIndirizzi_W
                        objIndirizzi = New AgronicaCoreAnagrafeDAL.Indirizzi_Write

                        Cod_Indirizzo = CInt(xIndirizzo.GetAttribute("cod_indirizzo"))

                        'Verifico l'operazione richiesta
                        Select Case OpeDB_Indirizzo

                            Case "0"    'LEGGI -------------------------------------------------------

                            Case "1"    'SALVA -------------------------------------------------------

                                If Cod_Indirizzo <= 0 Then

                                    'Richiedo un nuovo codice indirizzo
                                    Cod_Indirizzo = objSequenze.NuovoId_Tabella("Indirizzi",
                                                                                CInt(xIndirizzo.GetAttribute("basecode")),
                                                                                CInt(xIndirizzo.GetAttribute("topcode")),
                                                                                objParametri)

                                Else

                                    'Esportazione in Locale dell'Indirizzo dell'Impresa


                                End If

                                'Salvo l'indirizzo
                                Dummy = objIndirizzi.Scrivi(Cod_Indirizzo,
                                                            CStr(xIndirizzo.GetAttribute("ind_des")),
                                                            CStr(xIndirizzo.GetAttribute("frz_des")),
                                                            CStr(xIndirizzo.GetAttribute("cap")),
                                                            CStr(xIndirizzo.GetAttribute("com_des")),
                                                            CStr(xIndirizzo.GetAttribute("pro_cod")),
                                                            CStr(xIndirizzo.GetAttribute("stato")),
                                                            CStr(xIndirizzo.GetAttribute("note")),
                                                            CStr(xIndirizzo.GetAttribute("pro_cod_istat")),
                                                            CStr(xIndirizzo.GetAttribute("com_cod_istat")),
                                                            CDate(xImpresa.GetAttribute("validita_inizio")),
                                                            CDate(xImpresa.GetAttribute("validita_fine")),
                                                            objParametri,
                                                            Data_creazione:=Agro_XML_GetDate(xIndirizzo, "data_creazione", #2/1/1900#),
                                                            Data_modifica:=Agro_XML_GetDate(xIndirizzo, "data_modifica", #2/1/1900#),
                                                            username_creazione:=Agro_XML_GetString(xIndirizzo, "username_creazione", ""),
                                                            username_modifica:=Agro_XML_GetString(xIndirizzo, "username_modifica", ""),
                                                            Validazione:=Agro_XML_GetInteger(xIndirizzo, "validazione", 0),
                                                            Data_Validazione:=Agro_XML_GetDate(xIndirizzo, "data_validazione", Now),
                                                            UserName_Validazione:=Agro_XML_GetString(xIndirizzo, "username_validazione", ""),
                                                            Codice_Lingua:=Agro_XML_GetString(xIndirizzo, "codice_lingua", ""),
                                                            Codice_Alternativo:=Agro_XML_GetString(xIndirizzo, "codice_alternativo", "")
                                                            )

                                'Salvo la relazione Impresa x Indirizzo
                                Dummy = objImpresexIndirizzi.Scrivi(xImpresa.GetAttribute("piva"),
                                                                    Cod_Indirizzo,
                                                                    CInt(xIndirizzo.GetAttribute("tipo_indirizzo")),
                                                                    CDate(xImpresa.GetAttribute("validita_inizio")),
                                                                    CDate(xImpresa.GetAttribute("validita_fine")),
                                                                    objParametri,
                                                                    Data_creazione:=Agro_XML_GetDate(xIndirizzo, "data_creazione", #2/1/1900#),
                                                                    Data_modifica:=Agro_XML_GetDate(xIndirizzo, "data_modifica", #2/1/1900#),
                                                                    username_creazione:=Agro_XML_GetString(xIndirizzo, "username_creazione", ""),
                                                                    username_modifica:=Agro_XML_GetString(xIndirizzo, "username_modifica", ""),
                                                                    Validazione:=Agro_XML_GetInteger(xIndirizzo, "validazione", 0),
                                                                    Data_Validazione:=Agro_XML_GetDate(xIndirizzo, "data_validazione", Now),
                                                                    UserName_Validazione:=Agro_XML_GetString(xIndirizzo, "username_validazione", "")
                                                                    )

                            Case "2"    'MODIFICA -------------------------------------------------------

                                objIndirizzi.Modifica(CInt(xIndirizzo.GetAttribute("cod_indirizzo")),
                                                      xIndirizzo.GetAttribute("ind_des"),
                                                      xIndirizzo.GetAttribute("frz_des"),
                                                      xIndirizzo.GetAttribute("cap"),
                                                      xIndirizzo.GetAttribute("com_des"),
                                                      xIndirizzo.GetAttribute("pro_cod"),
                                                      xIndirizzo.GetAttribute("stato"),
                                                      xIndirizzo.GetAttribute("note"),
                                                      xIndirizzo.GetAttribute("pro_cod_istat"),
                                                      xIndirizzo.GetAttribute("com_cod_istat"),
                                                      CDate(xIndirizzo.GetAttribute("validita_inizio")),
                                                      CDate(xIndirizzo.GetAttribute("validita_fine")),
                                                      "",
                                                      objParametri,
                                                      Data_modifica:=Agro_XML_GetDate(xIndirizzo, "data_modifica", #2/1/1900#),
                                                      username_modifica:=Agro_XML_GetString(xIndirizzo, "username_modifica", ""),
                                                      Validazione:=If(Not xIndirizzo.HasAttribute("validazione"), Nothing, CInt(xIndirizzo.GetAttribute("validazione"))),
                                                      Data_Validazione:=If(Not xIndirizzo.HasAttribute("data_validazione"), AGRODATAINIZIO, CDate(xIndirizzo.GetAttribute("data_validazione"))),
                                                      UserName_Validazione:=If(Not xIndirizzo.HasAttribute("username_validazione"), Nothing, CStr(xIndirizzo.GetAttribute("username_validazione"))),
                                                      Codice_Lingua:=If(Not xIndirizzo.HasAttribute("codice_lingua"), Nothing, CStr(xIndirizzo.GetAttribute("codice_lingua"))),
                                                      Codice_Alternativo:=If(Not xIndirizzo.HasAttribute("codice_alternativo"), Nothing, CStr(xIndirizzo.GetAttribute("codice_alternativo")))
                                                      )

                            Case "3"    'ELIMINA -------------------------------------------------------

                                objImpresexIndirizzi.Cancella(CStr(xImpresa.GetAttribute("piva")),
                                                              CInt(xIndirizzo.GetAttribute("cod_indirizzo")),
                                                              CInt(xIndirizzo.GetAttribute("tipo_indirizzo")),
                                                              "",
                                                              objParametri)

                                objIndirizzi.Cancella(CInt(xIndirizzo.GetAttribute("cod_indirizzo")),
                                                      "",
                                                      objParametri)

                        End Select

                        'Elimino l'oggetto
                        objImpresexIndirizzi = Nothing
                        objIndirizzi = Nothing

                        'Incremento l'indice
                        i_Indirizzo += 1

                    Loop

                    '-------------------------------------------------------------
                    ' CONTATTO
                    '-------------------------------------------------------------

                    'Prelevo l'eventuale contatto
                    XmlDatiContatti = xImpresa.GetElementsByTagName("DatiContatti")

                    If XmlDatiContatti.Count > 0 Then

                        XmlDatiContatto = XmlDatiContatti.Item(0)

                        XmlContatti = XmlDatiContatto.OuterXml

                        'Istanzio l'oggetto
                        ObjContattiScrivi = New AgronicaCoreAnagrafeBIZ.Contatti_W

                        Dummy = ObjContattiScrivi.Contatto_Scrivi(CStr(XmlContatti),
                                                                  Nothing,
                                                                  Nothing,
                                                                  objParametri)

                        ObjContattiScrivi = Nothing

                    End If

                    '-------------------------------------------------------------
                    ' CODICI
                    '-------------------------------------------------------------


                    'Istanzio l'oggetto
                    objImpresexCodici = New AgronicaCoreAnagrafeDAL.Imprese_Codici_Write

                    ' se G2G cancello eventuali codici presenti
                    If TipoG2G <> 0 AndAlso OpeDB_Impresa = 2 Then
                        objImpresexCodici.Cancella(CStr(xImpresa.GetAttribute("piva")), 0, "", objParametri)
                    End If

                    'Prelevo l'elenco dei codici
                    xCodici = xImpresa.GetElementsByTagName("Codice")

                    i_Codice = 0

                    Do While i_Codice < xCodici.Count

                        'Prelevo l'i-esimo indirizzo
                        xCodice = xCodici.Item(i_Codice)

                        'Prelevo gli attributi del codice selezionato (se G2G forzo inserimento)
                        OpeDB_Codice = If(TipoG2G = 0, xCodice.GetAttribute("TipoOperazioneDB"), "1")

                        Dim Validazione As Integer = 0
                        If CStr(xCodice.GetAttribute("validazione")) <> "" AndAlso IsNumeric(xCodice.GetAttribute("validazione")) Then
                            Validazione = CInt(xCodice.GetAttribute("validazione"))
                        End If

                        Dim data_validazione As Date = AGRODATAINIZIO
                        If CStr(xCodice.GetAttribute("data_validazione")) <> "" AndAlso IsDate(xCodice.GetAttribute("data_validazione")) Then
                            data_validazione = CDate(xCodice.GetAttribute("data_validazione"))
                        End If

                        Dim objImpresexCodici_R As New AgronicaCoreAnagrafeDAL.Imprese_Codici_Read

                        'Verifico l'operazione richiesta
                        Select Case OpeDB_Codice

                            Case "0"    'LEGGI -------------------------------------------------------

                            Case "1"    'SALVA -------------------------------------------------------

                                If CInt(xCodice.GetAttribute("id_cod")) = enum_CodiciAnagrafe.CodiceCUAA Then

                                    Dim objCodici As New AgronicaCoreAnagrafeDAL.Imprese_Codici_Read
                                    If CStr(xCodice.GetAttribute("val_cod")) <> "" Then
                                        Dim Piva_FromCUAA = objCodici.Piva_from_CUAA(CStr(xCodice.GetAttribute("val_cod")), objParametri)
                                        If Piva_FromCUAA <> "" Then
                                            If Piva_FromCUAA <> CStr(xImpresa.GetAttribute("piva")) Then
                                                Throw New Exception("CUAA già presente nella PIVA:" & Piva_FromCUAA)
                                            Else
                                                Dummy = objImpresexCodici.Cancella(CStr(xImpresa.GetAttribute("piva")), CInt(xCodice.GetAttribute("id_cod")), "", objParametri)
                                            End If
                                        End If

                                    End If

                                End If

                                If objImpresexCodici_R.Leggi(CStr(xImpresa.GetAttribute("piva")), CInt(xCodice.GetAttribute("id_cod")), enumSelezioneVariabile.Selezione_TabellaCompleta, "", "", objParametri).Rows.Count = 0 Then


                                    Dummy = objImpresexCodici.Scrivi(CStr(xImpresa.GetAttribute("piva")),
                                                                 CInt(xCodice.GetAttribute("id_cod")),
                                                                 CStr(xCodice.GetAttribute("val_cod")),
                                                                 CDate(xCodice.GetAttribute("validita_inizio")),
                                                                 CDate(xCodice.GetAttribute("validita_fine")),
                                                                 objParametri,
                                                                 Data_creazione:=Agro_XML_GetDate(xCodice, "data_creazione", #2/1/1900#),
                                                                 Data_modifica:=Agro_XML_GetDate(xCodice, "data_modifica", #2/1/1900#),
                                                                 username_creazione:=Agro_XML_GetString(xCodice, "username_creazione", ""),
                                                                 username_modifica:=Agro_XML_GetString(xCodice, "username_modifica", ""),
                                                                 Validazione:=Validazione,
                                                                 Data_Validazione:=data_validazione,
                                                                 UserName_Validazione:=Agro_XML_GetString(xCodice, "username_validazione", "")
                                                                 )

                                End If

                            Case "2"    'MODIFICA -------------------------------------------------------

                                objImpresexCodici.Modifica(CStr(xImpresa.GetAttribute("piva")),
                                                           CInt(xCodice.GetAttribute("id_cod")),
                                                           CStr(xCodice.GetAttribute("val_cod")),
                                                           CDate(xCodice.GetAttribute("validita_inizio")),
                                                           CDate(xCodice.GetAttribute("validita_fine")),
                                                           "",
                                                           objParametri,
                                                           Data_modifica:=Agro_XML_GetDate(xCodice, "data_modifica", #2/1/1900#),
                                                           username_modifica:=Agro_XML_GetString(xCodice, "username_modifica", ""),
                                                           Validazione:=Validazione,
                                                           Data_Validazione:=data_validazione,
                                                           UserName_Validazione:=If(Not xCodice.HasAttribute("username_validazione"), Nothing, CStr(xCodice.GetAttribute("username_validazione")))
                                                           )

                            Case "3"    'ELIMINA -------------------------------------------------------

                                objImpresexCodici.Cancella(CStr(xImpresa.GetAttribute("piva")),
                                                           CInt(xCodice.GetAttribute("id_cod")),
                                                           "",
                                                           objParametri)

                        End Select

                        'Incremento l'indice
                        i_Codice += 1

                    Loop

                    'Elimino l'oggetto
                    objImpresexCodici = Nothing

#Region "Commentato"
                    ''''''''''    '-------------------------------------------------------------
                    ''''''''''    ' PERSONE
                    ''''''''''    '-------------------------------------------------------------

                    ''''''''''    'Prelevo l'elenco delle persone
                    ''''''''''    xPersone = xImpresa.GetElementsByTagName("Persone")

                    ''''''''''    i_Persona = 0

                    ''''''''''    Do While i_Persona < xPersone.Count

                    ''''''''''        'Prelevo l'i-esimo indirizzo
                    ''''''''''        xPersona = xPersone.Item(i_Persona)

                    ''''''''''        'Prelevo gli attributi dell'indirizzo selezionato
                    ''''''''''        OpeDB_Persona = xPersona.GetAttribute("TipoOperazioneDB")

                    ''''''''''        'Creo l'oggetto COM
                    ''''''''''        objPersone = CreateObject("Agro_Anagrafe_AD.Persone_W")
                    ''''''''''        objImpresexPersone = CreateObject("Agro_Anagrafe_AD.ImpresexPersone_W")

                    ''''''''''        'Verifico l'operazione richiesta
                    ''''''''''        Select Case OpeDB_Persona
                    ''''''''''            '
                    ''''''''''        Case "0"    'LEGGI -------------------------------------------------------
                    ''''''''''                '
                    ''''''''''            Case "1"    'SALVA -------------------------------------------------------
                    ''''''''''                '
                    ''''''''''                'Salvo i dati della persona
                    ''''''''''                Dummy = objPersone.Scrivi( _
                    ''''''''''                         CStr(xPersona.GetAttribute("cod_fis")), _
                    ''''''''''                         CStr(xPersona.GetAttribute("cognome")), _
                    ''''''''''                         CStr(xPersona.GetAttribute("nome")), _
                    ''''''''''                         CStr(xPersona.GetAttribute("ragione_sociale")), _
                    ''''''''''                         CDate(xPersona.GetAttribute("data_nascita")), _
                    ''''''''''                         CStr(xPersona.GetAttribute("sesso")), _
                    ''''''''''                         CStr(xPersona.GetAttribute("cittadinanza")), _
                    ''''''''''                         CStr(Username_Operazione), _
                    ''''''''''                         CDate(xPersona.GetAttribute("validita_inizio")), _
                    ''''''''''                         CDate(xPersona.GetAttribute("validita_fine")), _
                    ''''''''''                         objCnManager, _
                    ''''''''''                         ConnessioneAlternativa)
                    ''''''''''                '
                    ''''''''''                'Salvo la relazione Impresa x Persona
                    ''''''''''                Dummy = objImpresexPersone.Scrivi( _
                    ''''''''''                         CStr(xPersona.GetAttribute("cod_fis")), _
                    ''''''''''                         CStr(xImpresa.GetAttribute("piva")), _
                    ''''''''''                         CStr(Username_Operazione), _
                    ''''''''''                         CDate(xPersona.GetAttribute("validita_inizio")), _
                    ''''''''''                         CDate(xPersona.GetAttribute("validita_fine")), _
                    ''''''''''                         objCnManager, _
                    ''''''''''                         ConnessioneAlternativa)
                    ''''''''''                '
                    ''''''''''                '
                    ''''''''''            Case "2"    'MODIFICA -------------------------------------------------------
                    ''''''''''                '
                    ''''''''''                objPersone.Modifica( _
                    ''''''''''                         CStr(xPersona.GetAttribute("cod_fis")), _
                    ''''''''''                         CStr(xPersona.GetAttribute("cognome")), _
                    ''''''''''                         CStr(xPersona.GetAttribute("nome")), _
                    ''''''''''                         CStr(xPersona.GetAttribute("ragione_sociale")), _
                    ''''''''''                         CDate(xPersona.GetAttribute("data_nascita")), _
                    ''''''''''                         CStr(xPersona.GetAttribute("sesso")), _
                    ''''''''''                         CStr(xPersona.GetAttribute("cittadinanza")), _
                    ''''''''''                         CDate(xPersona.GetAttribute("validita_inizio")), _
                    ''''''''''                         CDate(xPersona.GetAttribute("validita_fine")), _
                    ''''''''''                         CStr(Username_Operazione), _
                    ''''''''''                         objCnManager, _
                    ''''''''''                         ConnessioneAlternativa)

                    ''''''''''                'Aggiorno le finestre temporali

                    ''''''''''                objImpresexPersone.AggiornaValiditaInizio( _
                    ''''''''''                         CStr(xPersona.GetAttribute("cod_fis")), _
                    ''''''''''                         CStr(xImpresa.GetAttribute("piva")), _
                    ''''''''''                         CStr(Username_Operazione), _
                    ''''''''''                         CDate(xPersona.GetAttribute("validita_inizio")), _
                    ''''''''''                         objCnManager, _
                    ''''''''''                         ConnessioneAlternativa)

                    ''''''''''                objImpresexPersone.AggiornaValiditaFine( _
                    ''''''''''                        CStr(xPersona.GetAttribute("cod_fis")), _
                    ''''''''''                        CStr(xImpresa.GetAttribute("piva")), _
                    ''''''''''                        CStr(Username_Operazione), _
                    ''''''''''                        CDate(xPersona.GetAttribute("validita_fine")), _
                    ''''''''''                        objCnManager, _
                    ''''''''''                        ConnessioneAlternativa)


                    ''''''''''                '
                    ''''''''''            Case "3"    'ELIMINA -------------------------------------------------------
                    ''''''''''                '
                    ''''''''''                objImpresexPersone.Cancella( _
                    ''''''''''                         CStr(xPersona.GetAttribute("cod_fis")), _
                    ''''''''''                         CStr(xImpresa.GetAttribute("piva")), _
                    ''''''''''                         CStr(Username_Operazione), _
                    ''''''''''                         objCnManager, _
                    ''''''''''                         ConnessioneAlternativa)

                    ''''''''''                objPersone.Cancella( _
                    ''''''''''                         CStr(xPersona.GetAttribute("cod_fis")), _
                    ''''''''''                         CStr(Username_Operazione), _
                    ''''''''''                         CDate(xPersona.GetAttribute("validita_fine")), _
                    ''''''''''                         objCnManager, _
                    ''''''''''                         ConnessioneAlternativa)
                    ''''''''''                '
                    ''''''''''        End Select

                    ''''''''''        'Elimino l'oggetto
                    ''''''''''        objImpresexPersone = Nothing
                    ''''''''''        objPersone = Nothing

                    ''''''''''        'Incremento l'indice
                    ''''''''''        i_Persona = i_Persona + 1

                    ''''''''''    Loop

#End Region

                    Select Case OpeDB_Impresa

                        Case "2" 'MODIFICA

                            'AGGIORNO LE FINESTRE TEMPORALI DEI FIGLI

                            objUtentixImprese.AggiornaValiditaInizio(objParametri.PivaSuperUser,
                                                                     CStr(xImpresa.GetAttribute("piva")),
                                                                     CDate(xImpresa.GetAttribute("validita_inizio")),
                                                                     "",
                                                                     objParametri)

                            objUtentixImprese.AggiornaValiditaFine(objParametri.PivaSuperUser,
                                                                   CStr(xImpresa.GetAttribute("piva")),
                                                                   CDate(xImpresa.GetAttribute("validita_fine")),
                                                                   "",
                                                                   objParametri)

#Region "Commentato"

                            ' Giulia: 11/7/2019: Commentato perché potrebbe generarmi cmq dei casini 
                            ' perché ci potrebbero essere delle operazioni oltre quella data, e cmq non sono considerate le distinte

                            'objCentri = New AgronicaCoreAnagrafeDAL.CentriAziendali_Write
                            'objCentri.AggiornaValiditaInizio(CStr(xImpresa.GetAttribute("piva")),
                            '                                0,
                            '                                CDate(xImpresa.GetAttribute("validita_inizio")),
                            '                                "",
                            '                                objParametri)

                            'objCentri.AggiornaValiditaFine(CStr(xImpresa.GetAttribute("piva")),
                            '                               0,
                            '                               CDate(xImpresa.GetAttribute("validita_fine")),
                            '                               "",
                            '                               objParametri)
                            'objCentri = Nothing

                            ''Modifico le validità dei fabbricati
                            'objFabbricati = New AgronicaCoreAnagrafeDAL.Fabbricati_W
                            'objFabbricati.AggiornaValiditaInizio(CStr(xImpresa.GetAttribute("piva")),
                            '                                     0,
                            '                                     0,
                            '                                     CDate(xImpresa.GetAttribute("validita_inizio")),
                            '                                     "",
                            '                                     objParametri)

                            'objFabbricati.AggiornaValiditaFine(CStr(xImpresa.GetAttribute("piva")),
                            '                                   0,
                            '                                   0,
                            '                                   CDate(xImpresa.GetAttribute("validita_fine")),
                            '                                   "",
                            '                                   objParametri)
                            'objFabbricati = Nothing

                            ''Modifico le validità delle particelle
                            'objImpresexParticelle2 = New AgronicaCoreAnagrafeDAL.ImpresexParticelle2_W
                            'objImpresexParticelle2.AggiornaValiditaInizio(CStr(xImpresa.GetAttribute("piva")),
                            '                                              0, "", "", "", 0, 0, "",
                            '                                              CDate(xImpresa.GetAttribute("validita_inizio")),
                            '                                              "",
                            '                                              objParametri)

                            'objImpresexParticelle2.AggiornaValiditaFine(CStr(xImpresa.GetAttribute("piva")),
                            '                                            0, "", "", "", 0, 0, "",
                            '                                            CDate(xImpresa.GetAttribute("validita_fine")),
                            '                                            "",
                            '                                            objParametri)
                            'objImpresexParticelle2 = Nothing

                            'objCentrixIndirizzi = New AgronicaCoreAnagrafeDAL.CentrixIndirizzi_Write
                            'objCentrixIndirizzi.AggiornaValiditaInizio(CStr(xImpresa.GetAttribute("piva")),
                            '                                           0, 0, 0,
                            '                                           CDate(xImpresa.GetAttribute("validita_inizio")),
                            '                                           "",
                            '                                           objParametri)

                            'objCentrixIndirizzi.AggiornaValiditaFine(CStr(xImpresa.GetAttribute("piva")),
                            '                                         0, 0, 0,
                            '                                         CDate(xImpresa.GetAttribute("validita_fine")),
                            '                                         "",
                            '                                         objParametri)
                            'objCentrixIndirizzi = Nothing

                            'objCentrixRubrica = New AgronicaCoreAnagrafeDAL.CentrixRubrica_Write
                            'objCentrixRubrica.AggiornaValiditaInizio(CStr(xImpresa.GetAttribute("piva")),
                            '                                         0, 0,
                            '                                         CDate(xImpresa.GetAttribute("validita_inizio")),
                            '                                         "",
                            '                                         objParametri)

                            'objCentrixRubrica.AggiornaValiditaFine(CStr(xImpresa.GetAttribute("piva")),
                            '                                       0, 0,
                            '                                       CDate(xImpresa.GetAttribute("validita_fine")),
                            '                                       "",
                            '                                       objParametri)
                            'objCentrixRubrica = Nothing

                            'objCentri_Codici = New AgronicaCoreAnagrafeDAL.Centri_Codici_Write
                            'objCentri_Codici.AggiornaValiditaInizio(CStr(xImpresa.GetAttribute("piva")),
                            '                                        0, 0,
                            '                                        CDate(xImpresa.GetAttribute("validita_inizio")),
                            '                                        "",
                            '                                        objParametri)

                            'objCentri_Codici.AggiornaValiditaFine(CStr(xImpresa.GetAttribute("piva")),
                            '                                      0, 0,
                            '                                      CDate(xImpresa.GetAttribute("validita_fine")),
                            '                                      "",
                            '                                      objParametri)
                            'objCentri_Codici = Nothing

                            'objUtentixStrutture = New AgronicaCoreAnagrafeDAL.UtentixStrutture_Write
                            'objUtentixStrutture.AggiornaValiditaInizio(objParametri.PivaSuperUser,
                            '                                           CStr(xImpresa.GetAttribute("piva")),
                            '                                           0,
                            '                                           CDate(xImpresa.GetAttribute("validita_inizio")),
                            '                                           "",
                            '                                           objParametri)
                            'objUtentixStrutture.AggiornaValiditaFine(objParametri.PivaSuperUser,
                            '                                         CStr(xImpresa.GetAttribute("piva")),
                            '                                         0,
                            '                                         CDate(xImpresa.GetAttribute("validita_fine")),
                            '                                         "",
                            '                                         objParametri)
                            'objUtentixStrutture = Nothing

                            'ObjCampi = New AgronicaCoreAnagrafeDAL.Campi_W
                            'ObjCampi.AggiornaValiditaInizio(CStr(xImpresa.GetAttribute("piva")),
                            '                                0, 0,
                            '                                CDate(xImpresa.GetAttribute("validita_inizio")),
                            '                                "",
                            '                                objParametri)
                            'ObjCampi.AggiornaValiditaFine(CStr(xImpresa.GetAttribute("piva")),
                            '                                0, 0,
                            '                                CDate(xImpresa.GetAttribute("validita_fine")),
                            '                                "",
                            '                                objParametri)
                            'ObjCampi = Nothing

                            'objUtentixCampi = New AgronicaCoreAnagrafeDAL.UtentixCampi_Write
                            'objUtentixCampi.AggiornaValiditaInizio(CStr(xImpresa.GetAttribute("piva")),
                            '                                       0, 0,
                            '                                       CDate(xImpresa.GetAttribute("validita_inizio")),
                            '                                       "",
                            '                                        objParametri)
                            'objUtentixCampi.AggiornaValiditaFine(CStr(xImpresa.GetAttribute("piva")),
                            '                                     0, 0,
                            '                                     CDate(xImpresa.GetAttribute("validita_fine")),
                            '                                     "",
                            '                                     objParametri)
                            'objUtentixCampi = Nothing

                            'objCampixParticelle = New AgronicaCoreAnagrafeDAL.CampixParticelle_W
                            'objCampixParticelle.AggiornaValiditaInizio(CStr(xImpresa.GetAttribute("piva")),
                            '                                           0, 0, "", "", "", 0, 0, "",
                            '                                           CDate(xImpresa.GetAttribute("validita_inizio")),
                            '                                           "",
                            '                                           objParametri)
                            'objCampixParticelle.AggiornaValiditaFine(CStr(xImpresa.GetAttribute("piva")),
                            '                                         0, 0, "", "", "", 0, 0, "",
                            '                                         CDate(xImpresa.GetAttribute("validita_fine")),
                            '                                         "",
                            '                                         objParametri)
                            'objCampixParticelle = Nothing


                            'objAppezzamenti = New AgronicaCoreAnagrafeDAL.Appezzamento_Write
                            'objAppezzamenti.AggiornaValiditaInizio(CStr(xImpresa.GetAttribute("piva")),
                            '                                       0, 0, 0,
                            '                                       CDate(xImpresa.GetAttribute("validita_inizio")),
                            '                                       "",
                            '                                       objParametri)
                            'objAppezzamenti.AggiornaValiditaFine(CStr(xImpresa.GetAttribute("piva")),
                            '                                     0, 0, 0,
                            '                                     CDate(xImpresa.GetAttribute("validita_fine")),
                            '                                     "",
                            '                                     objParametri)
                            'objAppezzamenti = Nothing

                            'objUtentixAppezzamenti = New AgronicaCoreAnagrafeDAL.UtentixAppezzamenti_W
                            'objUtentixAppezzamenti.AggiornaValiditaInizio(CStr(xImpresa.GetAttribute("piva")),
                            '                                              0, 0, 0,
                            '                                              CDate(xImpresa.GetAttribute("validita_inizio")),
                            '                                              "",
                            '                                              objParametri)
                            'objUtentixAppezzamenti.AggiornaValiditaFine(CStr(xImpresa.GetAttribute("piva")),
                            '                                            0, 0, 0,
                            '                                            CDate(xImpresa.GetAttribute("validita_fine")),
                            '                                            "",
                            '                                            objParametri)
                            'objUtentixAppezzamenti = Nothing

                            'objAppezzaxParticelle = New AgronicaCoreAnagrafeDAL.AppezzaxParticelle_W
                            'objAppezzaxParticelle.AggiornaValiditaInizio(CStr(xImpresa.GetAttribute("piva")),
                            '                                             0, 0, 0, "", "", "", 0, 0, "",
                            '                                             CDate(xImpresa.GetAttribute("validita_inizio")),
                            '                                             "",
                            '                                             objParametri)
                            'objAppezzaxParticelle.AggiornaValiditaFine(CStr(xImpresa.GetAttribute("piva")),
                            '                                           0, 0, 0, "", "", "", 0, 0, "",
                            '                                           CDate(xImpresa.GetAttribute("validita_fine")),
                            '                                           "",
                            '                                           objParametri)
                            'objAppezzaxParticelle = Nothing

                            'objReg_Impianti = New AgronicaCoreAnagrafeDAL.Reg_Impianti_Write
                            'objReg_Impianti.AggiornaValiditaInizio(CStr(xImpresa.GetAttribute("piva")),
                            '                                       0, 0, 0, 0,
                            '                                       CDate(xImpresa.GetAttribute("validita_inizio")),
                            '                                       objParametri)
                            'objReg_Impianti.AggiornaValiditaFine(CStr(xImpresa.GetAttribute("piva")),
                            '                                     0, 0, 0, 0,
                            '                                     CDate(xImpresa.GetAttribute("validita_fine")),
                            '                                     objParametri)
                            'objReg_Impianti = Nothing
#End Region

                        Case "3" 'ELIMINA

                            ''xPiva = CStr(xImpresa.GetAttribute("piva"))

                            'Cancellazione del PARCO MACCHINE

                            ObjParco_Macchine = New AgronicaCoreContabDAL.Parco_Macchine_W
                            ObjParco_Macchine.Cancella(CStr(xImpresa.GetAttribute("piva")),
                                                       0, "",
                                                       "",
                                                       objParametri)
                            ObjParco_Macchine = Nothing


                            'Cancellazione dei CONTATTI

                            ObjContattiLeggi = New AgronicaCoreAnagrafeBIZ.Contatti_MultiHost_R
                            XmlContatti = ObjContattiLeggi.Contatto_Leggi(CStr(xImpresa.GetAttribute("piva")),
                                                                          "", "",
                                                                          True,
                                                                          objParametri)
                            ObjContattiLeggi = Nothing

                            If XmlContatti <> "" Then
                                ObjContattiScrivi = New AgronicaCoreAnagrafeBIZ.Contatti_W
                                ObjContattiScrivi.Contatto_Scrivi(XmlContatti,
                                                                  Nothing,
                                                                  Nothing,
                                                                  objParametri)
                                ObjContattiScrivi = Nothing
                            End If


                            'Elimino il figlio Maggiore

                            'Cancello TUTTI i Centri Aziendali
                            objCentriLeggi = New AgronicaCoreAnagrafeBIZ.CentroAziendale_R
                            XmlCentri = objCentriLeggi.CentroAziendale_Leggi(CStr(xImpresa.GetAttribute("piva")),
                                                                             0,
                                                                             True, True,
                                                                             AGRODATAINIZIO,
                                                                             AGRODATAFINE,
                                                                             objParametri)
                            objCentriLeggi = Nothing

                            If XmlCentri <> "" Then
                                objCentriScrivi = New AgronicaCoreAnagrafeBIZ.CentroAziendale_W
                                objCentriScrivi.CentroAziendale_Scrivi(XmlCentri,
                                                                       Nothing,
                                                                       Nothing,
                                                                       objParametri,
                                                                       objParametri_Utenti)
                            End If

                            '--------------------------------------------------------------------------------------------------
                            '------------------------------   CANCELLAZIONE IMPRESA  ------------------------------------------
                            '--------------------------------------------------------------------------------------------------

                            objGerarchiaImprese = New AgronicaCoreAnagrafeDAL.GerarchiaImprese_W

                            objGerarchiaImprese.Cancella("",
                                                         CStr(xImpresa.GetAttribute("piva")),
                                                         "",
                                                         objParametri,
                                                         objParametri_Utenti)
                            objGerarchiaImprese = Nothing
                            '--------------------------------------------------------------------------------------------------

                            objSequenze.CancellaSeqCentri_Aziendali(CStr(xImpresa.GetAttribute("piva")),
                                                                    objParametri)

                            '--------------------------------------------------------------------------------------------------

                            objUtentixImprese.Cancella(objParametri.PivaSuperUser,
                                                       CStr(xImpresa.GetAttribute("piva")),
                                                       "",
                                                       objParametri)

                            objImprese.Cancella(CStr(xImpresa.GetAttribute("piva")),
                                                "",
                                                objParametri)

                    End Select

                    'Elimino l'oggetto

                    'objCentriLeggi = Nothing
                    'objCentriScrivi = Nothing
                    objImprese = Nothing
                    objUtentixImprese = Nothing

                    ''''''''''    '-------------------------------------------------------------

                    'Incremento l'indice
                    i_Impresa += 1

                Loop

                '------------------------------

                'Incremento l'indice
                i_DatiImpresa += 1

            Loop

            '------------------------------

            'Elimino tutti gli oggetti utilizzati

            'xPersona = Nothing
            'xPersone = Nothing
            xCodice = Nothing
            xCodici = Nothing
            xIndirizzo = Nothing
            xIndirizzi = Nothing
            xImpresa = Nothing
            xImprese = Nothing
            xDatiImpresa = Nothing
            xDatiImprese = Nothing
            XmlDoc = Nothing

            '------------------------------
            objSequenze = Nothing
            '------------------------------

            ConnessioniTransazioni.ChiudiTransazioneXCoreBiz(FlagTransazioneLocale, objParametri)
            xRisp = True

        Catch ex As Exception
            xRisp = False

            'Faccio il rollback della transazione
            If objParametri.objTransazione IsNot Nothing Then
                ConnessioniTransazioni.ChiudiTransazione(2, objParametri)
            End If

            '//////////////////////////////////////////////////////////////////////
            MessaggioErrore = "(Piva=" & OUTPUT_Piva & ")" &
                              " : " & ex.Message
            '//////////////////////////////////////////////////////////////////////

            Scrivi_LOG(objParametri, nomeRoutine, MessaggioErrore)

            Throw New Exception("[" & nomeRoutine & "] : " & MessaggioErrore)

        Finally
            ConnessioniTransazioni.ChiudiConnessioneXCoreBiz(FlagConnessioneLocale, objParametri)
        End Try

        Return xRisp

    End Function

    Public Function Scrivi_Impresa_Anagrafica(ByRef objImpresa As AgronicaCoreModelsSTD.anagrafiche.Impresa,
                                              ByRef objParametri_Server As AgronicaCoreParametri,
                                              ByRef objParametri_Utenti As AgronicaCoreParametri,
                                              Optional ByVal creaCentro As Boolean = True,
                                              Optional ByVal nomeCentro As String = Nothing,
                                              Optional ByVal creaMagazzino As Boolean = True,
                                              Optional ByVal nomeMagazzino As String = Nothing,
                                              Optional ByVal NoteLog As String = NOTELOG_ANAGRAFE_NG,
                                              Optional ByVal OpenNewTransaction As Boolean = True
                                              ) As AgronicaCoreModelsSTD.anagrafiche.Impresa

        Const nomeRoutine = "AgronicaCoreAnagrafeBIZ.Impresa_W.Scrivi_Impresa_Anagrafica()"
        Dim messaggioErrore As String = ""

        Dim gefutils As New Gias_EF_Utility
        Dim EFConnString As String = gefutils.GetEntityConnectionString(objParametri_Server.StringaConnessione)

        Dim Piva As String = ""

        Dim scope As TransactionScope = Nothing

        If OpenNewTransaction Then
            Dim scopeOption As New TransactionScopeOption
            Dim transactionOptions As New TransactionOptions
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted

            scope = New TransactionScope(scopeOption, transactionOptions)
        End If

        Try

            If objImpresa Is Nothing Then
                Throw New GiasException("Impresa non valorizzata")
            End If


            'Dim scopeOption As New TransactionScopeOption
            'Dim transactionOptions As New TransactionOptions
            'TransactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted
            'Using scope As New TransactionScope(scopeOption, transactionOptions)
            Using GiasContext As New Gias_DeveloperServer_Entities(EFConnString)

                If Not objImpresa.flag_cancellazione Then

                    If objImpresa.partitaIva IsNot Nothing Then
                        Piva = objImpresa.partitaIva
                    End If

                    If String.IsNullOrEmpty(Piva) Then
                        Piva = GeneraPivaFittizia(objParametri_Server)
                        objImpresa.partitaIva = Piva
                    End If

                    Dim pivaEF As AgronicaCoreEntityFramework_POCO.Imprese

                    Dim Tipo_Operazione = "0" ' 0: Non fare niente - 1: Creazione nuova Impresa - 2: Modifica Impresa

                    pivaEF = (From p In GiasContext.Imprese Where Piva = p.PIVA).FirstOrDefault()

                    If pivaEF Is Nothing Then
                        Tipo_Operazione = "1"
                    Else
                        Tipo_Operazione = "2"
                        Piva = objImpresa.partitaIva
                    End If

                    Dim Impresa As AgronicaCoreEntityFramework_POCO.Imprese
                    Dim UtentixImprese As AgronicaCoreEntityFramework_POCO.UtentiXImprese

                    If Tipo_Operazione = "1" Then
                        Verifica_PivaImpresa(objImpresa, objParametri_Server)
                        Impresa = AgronicaCoreAnagrafeDAL.EFImprese.CreateImpreseEF(GiasContext, objParametri_Server, Piva, objImpresa.ragioneSociale, objParametri_Utenti.UsernameOperazione)
                        Piva = Impresa.PIVA

                        Dim DatiImpresaStr = ""
                        If objImpresa IsNot Nothing Then
                            Dim a As New JsonSerializerSettings With {.DateTimeZoneHandling = DateTimeZoneHandling.Local}
                            DatiImpresaStr = JsonConvert.SerializeObject(objImpresa, a)
                        End If

                        'Scrittura tabella Agronica_Log_Anagrafe
                        Dim objLogAnagrafeW As New AgronicaLogAnagrafe_W
                        Dim log As Agronica_Log_Anagrafe = objLogAnagrafeW.CreaLogAnagrafeEF(
                            enum_TipoEntita_Des.Imprese,
                            CStr(Piva),
                            Nothing,
                            Nothing,
                            Nothing,
                            Nothing,
                            Nothing,
                            enum_TipoOperazioneDB.Scrittura,
                            objParametri_Server,
                            enum_Id_Servizio.GiasOnline,
                            NoteLog,
                            DatiImpresaStr
                            )

                        GiasContext.Agronica_Log_Anagrafe.Add(log)
                        GiasContext.SaveChanges()

                        ' ---------- UTENTIxIMPRESE --------------- '
                        UtentixImprese = AgronicaCoreAnagrafeDAL.EFImprese.CreateUtentixImprese(
                            GiasContext,
                            objParametri_Server,
                            objParametri_Utenti.PivaSuperUser,
                            Piva,
                            objParametri_Utenti.UsernameOperazione
                            )

                    Else
                        Verifica_ValiditaInizioFine(objImpresa, GiasContext, objParametri_Server, objParametri_Utenti)

                        Impresa = (From imp In GiasContext.Imprese Where imp.PIVA = Piva).FirstOrDefault()

                        Dim DatiImpresaStr = ""
                        If objImpresa IsNot Nothing Then
                            Dim a As New JsonSerializerSettings With {.DateTimeZoneHandling = DateTimeZoneHandling.Local}
                            DatiImpresaStr = JsonConvert.SerializeObject(objImpresa, a)
                        End If

                        'Scrittura tabella Agronica_Log_Anagrafe
                        Dim objLogAnagrafeW As New AgronicaLogAnagrafe_W
                        Dim log As Agronica_Log_Anagrafe = objLogAnagrafeW.CreaLogAnagrafeEF(
                            enum_TipoEntita_Des.Imprese,
                            CStr(Piva),
                            Nothing,
                            Nothing,
                            Nothing,
                            Nothing,
                            Nothing,
                            enum_TipoOperazioneDB.Modifica,
                            objParametri_Server,
                            enum_Id_Servizio.GiasOnline,
                            NoteLog,
                            DatiImpresaStr
                            )

                        GiasContext.Agronica_Log_Anagrafe.Add(log)
                        GiasContext.SaveChanges()

                    End If

                    If Impresa Is Nothing Then
                        Throw New GiasException("Impresa non trovata")
                    End If

                    If objImpresa Is Nothing Then
                        Throw New GiasException("Impresa non valorizzata")
                    End If

                    Impresa.TipoImpresaGerarchia = objImpresa.tipo_Impresa

                    Impresa.rag_soc = objImpresa.ragioneSociale
                    Impresa.Forma_Giuridica = ""

                    If objImpresa.forma_Giuridica IsNot Nothing Then
                        Impresa.Forma_Giuridica = objImpresa.forma_Giuridica.codice
                    End If

                    If objImpresa.validita.inizio < AGRODATAINIZIO OrElse objImpresa.validita.inizio > AGRODATAFINE Then
                        objImpresa.validita.inizio = AGRODATAINIZIO
                    End If
                    If objImpresa.validita.fine < AGRODATAINIZIO OrElse objImpresa.validita.fine > AGRODATAFINE Then
                        objImpresa.validita.fine = AGRODATAFINE
                    End If

                    Impresa.Validita_Inizio = objImpresa.validita.inizio
                    Impresa.Validita_Fine = objImpresa.validita.fine

                    If objImpresa.gruppoRaccolta IsNot Nothing Then
                        Impresa.GruppoRaccolta_Cod = objImpresa.gruppoRaccolta.codice
                    End If

                    'Colonne non usate e settate a ZERO/STRINGA VUOTA
                    Impresa.AT_Prevalente = ""
                    Impresa.Delega = ""
                    Impresa.Forma_Conduzione = ""
                    Impresa.Sup_Totale = 0

                    Impresa.Data_Modifica = DateTime.Now
                    Impresa.Username_Modifica = objParametri_Utenti.UsernameOperazione

                    ' ---------- INDIRIZZO -------------- ' !! DA SISTEMARE!!
                    Dim indirizzoBIZ As New AgronicaCoreAnagrafeBIZ.Indirizzi_W
                    'Se l'azienda non ha indirizzi, ne creo uno fittizio con stato ITALIA, ciò serve per il salvataggio del contatto associato che risulterebbe altrimenti estero
                    If (objImpresa.indirizzi Is Nothing) Then
                        Dim address As New IndirizzoAssociato
                        address.indirizzo = New Indirizzo(0)
                        address.indirizzo.cap = "00000"
                        address.indirizzo.frazione = ""
                        address.indirizzo.note = ""
                        address.indirizzo.via = ""
                        address.indirizzo.istatComune = New AgronicaCoreModelsSTD.metaschema.Istat
                        address.indirizzo.istatComune.com = "000"
                        address.indirizzo.istatComune.prov = "000"
                        address.indirizzo.istatComune.reg = "000"
                        address.indirizzo.stato = New AgronicaCoreModelsSTD.metaschema.CodiciNazioniISO3166
                        address.indirizzo.stato.codice = "IT"
                        address.indirizzo.stato.descrizione = ""

                        objImpresa.indirizzi.Add(address)
                    End If

                    For Each add As AgronicaCoreModelsSTD.anagrafiche.IndirizzoAssociato In objImpresa.indirizzi
                        If (add.indirizzo.istatComune.com IsNot Nothing) AndAlso (add.indirizzo.istatComune.prov IsNot Nothing) Then

                            indirizzoBIZ.Scrivi_Indirizzi_Associati_Impresa(
                                add,
                                GiasContext,
                                Impresa,
                                objParametri_Server,
                                objParametri_Utenti
                                )
                        End If
                    Next


                    ' ---------- CODICI --------------- '
                    Dim codiciBIZ As New Codici_W
                    If (objImpresa.codici IsNot Nothing) Then
                        For Each codice In objImpresa.codici
                            If codice.validita Is Nothing Then
                                codice.validita = New IntervalloTemporale()
                            Else
                                If codice.validita.inizio < AGRODATAINIZIO Then
                                    codice.validita.inizio = AGRODATAINIZIO
                                End If

                                If codice.validita.fine < AGRODATAINIZIO Then
                                    codice.validita.fine = AGRODATAFINE
                                End If

                            End If
                        Next
                        If Tipo_Operazione = "2" Then
                            If objImpresa.codici IsNot Nothing Then
                                Dim listIdCod As New List(Of Integer)
                                'listIdCod = (From c In objImpresa.codici Select c.codiceAnagrafe.codice).ToList
                                listIdCod.Add(enum_CodiciAnagrafe.CodiceCUAA)
                                listIdCod.Add(enum_CodiciAnagrafe.Organismo_di_Controllo)
                                listIdCod.Add(enum_CodiciAnagrafe.Tecnico)

                                Dim imprese_codici_query As DbQuery(Of Imprese_Codici) = (From ic In GiasContext.Imprese_Codici Where ic.PIVA = Piva AndAlso Not listIdCod.Contains(ic.id_cod))

                                Dim excludeList = New List(Of enum_CodiciAnagrafe)({
                                                                                                 enum_CodiciAnagrafe.Organismo_di_Controllo,
                                                                                                 enum_CodiciAnagrafe.Tecnico,
                                                                                                 enum_CodiciAnagrafe.Codice_Libro_Soci,
                                                                                                 enum_CodiciAnagrafe.Data_Iscrizione_Libro_Soci,
                                                                                                 enum_CodiciAnagrafe.CodiceCUAA,
                                                                                                 enum_CodiciAnagrafe.Codice_Ausl,
                                                                                                 enum_CodiciAnagrafe.TitoloPossesso,
                                                                                                 enum_CodiciAnagrafe.Disciplinare_Aziendale_Default,
                                                                                                 enum_CodiciAnagrafe.PivaSuperUser_Origine_Dato})

                                imprese_codici_query = imprese_codici_query.Where(Function(x) Not excludeList.Contains(x.id_cod))

                                imprese_codici_query = imprese_codici_query.Where(Function(x) x.id_cod < 2000 OrElse x.id_cod > 3000)



                                Dim imprese_codici_del = imprese_codici_query.ToList
                                If imprese_codici_del.Count > 0 Then
                                    GiasContext.Imprese_Codici.RemoveRange(imprese_codici_del)
                                End If

                                GiasContext.SaveChanges()

                            End If
                        End If

                        For Each codici As AgronicaCoreModelsSTD.anagrafiche.CodiciAnagrafeValori In objImpresa.codici
                            If (codici.valore IsNot Nothing) Then
                                codiciBIZ.Scrivi_Codici_Impresa_NoEF(codici,
                                                                    GiasContext,
                                                                    Impresa,
                                                                    objParametri_Server,
                                                                    objParametri_Utenti
                                                                    )
                            End If
                        Next

                    End If

                    If (objImpresa.CUAA IsNot Nothing) Then
                        Scrivi_Codice_Impresa(enum_CodiciAnagrafe.CodiceCUAA, objImpresa.CUAA, GiasContext, Impresa, objParametri_Server, objParametri_Utenti)
                    End If
                    If (objImpresa.organismo_di_Controllo IsNot Nothing) Then
                        Scrivi_Codice_Impresa(enum_CodiciAnagrafe.Organismo_di_Controllo, objImpresa.organismo_di_Controllo.codice, GiasContext, Impresa, objParametri_Server, objParametri_Utenti)
                    End If
                    If objImpresa.tecnicoReferente IsNot Nothing AndAlso
                       objImpresa.tecnicoReferente.primaryKey IsNot Nothing Then
                        Scrivi_Codice_Impresa(enum_CodiciAnagrafe.Tecnico, objImpresa.tecnicoReferente.primaryKey.codice,
                                              GiasContext, Impresa, objParametri_Server, objParametri_Utenti)
                    End If
                    If (objImpresa.certificazione IsNot Nothing AndAlso objImpresa.certificazione.Count <> 0) Then
                        Dim certificazione_string = ""
                        Dim sb As New System.Text.StringBuilder()
                        Dim index = objImpresa.certificazione.Count - 1
                        If objImpresa.certificazione.Count > 1 Then
                            For Each cert_codice In objImpresa.certificazione
                                certificazione_string = sb.Append(cert_codice.codice.ToString & If(index = 0, "", ",")).ToString()
                                index = index - 1
                            Next
                        Else
                            certificazione_string = objImpresa.certificazione(0).codice
                        End If

                        Scrivi_Codice_Impresa(enum_CodiciAnagrafe.Codice_Certificazione, certificazione_string, GiasContext, Impresa, objParametri_Server, objParametri_Utenti)
                    Else
                        'Scrivi_Codice_Impresa(enum_CodiciAnagrafe.Codice_Certificazione, "", GiasContext, Impresa, objParametri_Server, objParametri_Utenti)
                    End If


                    ' ---------- CONTATTI --------------- '
                    'Dim Contatto As AgronicaCoreEntityFramework_POCO.Contatti
                    If Tipo_Operazione = "1" Then
                        'If objImpresa.contatti IsNot Nothing AndAlso objImpresa.contatti.Count = 0 Then
                        '    Scrivi_Contatti_ClienteFonitore(objImpresa, GiasContext, Impresa, Contatto, objParametri_Server, objParametri_Utenti, Tipo_Operazione)
                        'End If

                        'If objImpresa.contatti IsNot Nothing AndAlso objImpresa.contatti.Count > 0 Then
                        '    For Each contattoSingolo In objImpresa.contatti
                        '        If contattoSingolo.primaryKey.codice = "" Then
                        '            contattoSingolo.primaryKey.codice = objImpresa.partitaIva
                        '        End If
                        '        Scrivi_Contatti_Impresa(contattoSingolo, objImpresa, GiasContext, Impresa, Contatto, objParametri_Server, objParametri_Utenti, Tipo_Operazione)
                        '    Next
                        If IsNothing(objImpresa.contatto_superuser) Then
                            Dim contatto_superuser As New Contatto
                            Dim risorsaFornitore As New RisorseUmane With {.rapportoContabile = New RapportoContabile(enum_Rapporti_Contabili_Standard.Fornitore)}
                            Dim risorsaCliente As New RisorseUmane With {.rapportoContabile = New RapportoContabile(enum_Rapporti_Contabili_Standard.Cliente)}

                            contatto_superuser.risorseUmane = New List(Of RisorseUmane)
                            contatto_superuser.risorseUmane.Add(risorsaFornitore)
                            contatto_superuser.risorseUmane.Add(risorsaCliente)
                            contatto_superuser.visibilitaPubblica = False

                            contatto_superuser.primaryKey = New Contatto.PK(objParametri_Server.PivaSuperUser, objImpresa.partitaIva)

                            objImpresa.contatto_superuser = contatto_superuser
                        End If
                        ScriviContatto_Superuser(objImpresa, GiasContext, objParametri_Server, objParametri_Utenti)
                    End If

                    ' ---------- GERARCHIA IMPRESA --------------- '
                    If objImpresa.impresaPadre IsNot Nothing Then

                        Dim listaPadri As List(Of ImpresaPadre) = (From p In objImpresa.impresaPadre Where p.partitaIva <> "" Select p).ToList

                        If Piva <> objParametri_Server.PivaSuperUser AndAlso listaPadri.Count = 0 Then
                            listaPadri.Add(New ImpresaPadre With {
                                    .partitaIva = objParametri_Server.PivaSuperUser
                                })
                        End If
                        For Each padre In listaPadri
                            If padre.partitaIva = Piva Then
                                Throw New GiasException("Impresa referente = impresa")
                            End If
                        Next

                        If Piva = objParametri_Server.PivaSuperUser Then
                            listaPadri = New List(Of ImpresaPadre)
                        End If
                        Scrivi_Gerarchia_Impresa(Piva, objImpresa.tipo_Impresa, listaPadri, CInt(Tipo_Operazione), objParametri_Server, objParametri_Utenti, GiasContext)
                    End If

                    Dim CentriDBEF = (From centro In GiasContext.Centri_Aziendali Where
                                                         centro.PIVA = Piva)
                    If CentriDBEF.Count = 0 Then
                        Try
                            Dim objUtentiVisibilitaR As New AgronicaCoreUtentiDAL.Utenti_Visibilita_Appoggio_R
                            Dim DtImpreseVisibili As DataTable
                            DtImpreseVisibili = objUtentiVisibilitaR.Leggi(enum_TipoEntita.Impresa, "", "", objParametri_Server)
                            If DtImpreseVisibili IsNot Nothing AndAlso DtImpreseVisibili.Rows.Count > 0 Then
                                Dim objUtentiVisibilita As New AgronicaCoreUtentiDAL.Utenti_Visibilita_Appoggio_W
                                objUtentiVisibilita.Scrivi(enum_TipoEntita.Impresa, Piva, 0, 0, 0, objParametri_Server)
                            End If
                        Catch ex As Exception

                        End Try

                        If creaCentro Then
                            Crea_Centro(objImpresa, objParametri_Server, objParametri_Utenti, GiasContext, nomeCentro, creaMagazzino, nomeMagazzino)
                        End If
                    End If

                    Dim w As New AgronicaCoreAnagrafeDAL.Imprese_Codici_Write
                    If objImpresa.disciplinareAziendalePredefinito IsNot Nothing AndAlso objImpresa.disciplinareAziendalePredefinito.codice <> "" Then

                        w.Cancella(Piva, enum_CodiciAnagrafe.Disciplinare_Aziendale_Default, "", objParametri_Server)

                        If objImpresa.disciplinareAziendalePredefinito.codice <> "0" Then
                            w.Scrivi(Piva, enum_CodiciAnagrafe.Disciplinare_Aziendale_Default,
                                                       objImpresa.disciplinareAziendalePredefinito.codice,
                                                       AGRODATAINIZIO, AGRODATAFINE, objParametri_Server)
                        End If

                    End If

                Else

                    EliminaImpresa(objImpresa, GiasContext, objParametri_Server, objParametri_Utenti, NoteLog:=NoteLog)

                End If

                GiasContext.SaveChanges()

                If scope IsNot Nothing Then
                    scope.Complete()
                    scope.Dispose()
                End If

            End Using
            'End Using

        Catch ex As GiasException
            If scope IsNot Nothing Then
                scope.Dispose()
            End If

            messaggioErrore = ex.Message
            Throw ex
        Catch ex As Exception
            If scope IsNot Nothing Then
                scope.Dispose()
            End If

            messaggioErrore = ex.Message
            Throw New Exception("[" & nomeRoutine & "] : " & messaggioErrore)
            Return Nothing
        Finally
            If scope IsNot Nothing Then
                scope.Dispose()
            End If
        End Try

        Return objImpresa
    End Function

    Private Sub ScriviContatto_Superuser(objImpresa As AgronicaCoreModelsSTD.anagrafiche.Impresa, GiasContext As Gias_DeveloperServer_Entities, objParametri_Server As AgronicaCoreParametri, objParametri_Utenti As AgronicaCoreParametri)
        Dim nomeRoutine As String = "AgronicaCoreAnagrafeBIZ.Impresa.ScriviContatto_Superuser()"
        Dim messaggioErrore As String = ""

        Dim Cod_Rapporto As Integer
        Dim Cod_Contatto As String = objImpresa.partitaIva
        Dim id_CF As Integer = Ottieni_ID_CF_Contatto((objImpresa.indirizzi(0).indirizzo.stato.codice).ToString)


        Dim RisorseUmaneEF As AgronicaCoreEntityFramework_POCO.Risorse_Umane
        Dim ContattiEF As AgronicaCoreEntityFramework_POCO.Contatti

        Try
            Dim ImpresaSuperuserEF As AgronicaCoreEntityFramework_POCO.Imprese
            Dim pivaSuperUser As String = ""
            If objImpresa.contatto_superuser.primaryKey IsNot Nothing Then
                If objImpresa.contatto_superuser.primaryKey.partitaIva IsNot Nothing AndAlso objImpresa.contatto_superuser.primaryKey.partitaIva IsNot "" Then
                    pivaSuperUser = objImpresa.contatto_superuser.primaryKey.partitaIva
                Else
                    pivaSuperUser = objParametri_Server.PivaSuperUser
                End If
            Else
                pivaSuperUser = objParametri_Server.PivaSuperUser
            End If

            ImpresaSuperuserEF = (From superuser In GiasContext.Imprese Where
                                                         superuser.PIVA = pivaSuperUser).FirstOrDefault()

            Dim sa_cod = 0
            If objImpresa.contatto_superuser.visibilitaPubblica Then
                sa_cod = -1
            End If

            Dim ContattiDBEF = (From contatto In GiasContext.Contatti
                                Where contatto.Piva = ImpresaSuperuserEF.PIVA AndAlso
                                      contatto.Cod_Contatto = Cod_Contatto AndAlso
                                      contatto.Sa_Cod = sa_cod)

            If ContattiDBEF.Count = 0 Then
                ContattiEF = AgronicaCoreAnagrafeDAL.EFImprese.CreaContattoImpresa(GiasContext,
                                                                      objParametri_Server,
                                                                      ImpresaSuperuserEF,
                                                                      Cod_Contatto,
                                                                      sa_cod,
                                                                      id_CF,
                                                                      objParametri_Utenti.UsernameOperazione
                                                                      )
            Else
                ContattiEF = ContattiDBEF.First()
            End If



            For Each risorsaUmana In objImpresa.contatto_superuser.risorseUmane

                Cod_Rapporto = risorsaUmana.rapportoContabile.codice

                Dim RisorseUmaneDBEF = (From risorsa In GiasContext.Risorse_Umane
                                        Where risorsa.Piva = ImpresaSuperuserEF.PIVA AndAlso
                                              risorsa.Cod_Contatto = Cod_Contatto AndAlso
                                              risorsa.Sa_Cod = sa_cod AndAlso
                                              risorsa.Cod_Rapporto = risorsaUmana.rapportoContabile.codice)

                If RisorseUmaneDBEF.Count = 0 Then
                    RisorseUmaneEF = AgronicaCoreAnagrafeDAL.EFImprese.CreaRisorseUmaneImpresa(GiasContext,
                                                                       objParametri_Server,
                                                                       ImpresaSuperuserEF,
                                                                       ContattiEF,
                                                                       objParametri_Utenti.UsernameOperazione
                                                                       )
                Else
                    RisorseUmaneEF = RisorseUmaneDBEF.First()
                End If

                RisorseUmaneEF.Cod_Rapporto = Cod_Rapporto
                If risorsaUmana.attivita IsNot Nothing Then
                    RisorseUmaneEF.Attivita_Des = risorsaUmana.attivita
                End If

                If risorsaUmana.settore IsNot Nothing Then
                    RisorseUmaneEF.Settore_Des = risorsaUmana.settore
                End If

                GiasContext.SaveChanges()

            Next

            ContattiEF.Codice_Fiscale = objImpresa.CUAA
            ContattiEF.Rag_Soc = objImpresa.ragioneSociale
            ContattiEF.Codice_Fiscale = objImpresa.CUAA

            GiasContext.SaveChanges()
        Catch ex As Exception
            messaggioErrore = Gestione_Eccezioni_2015.MessaggioCompletoDataEccezione(ex, False, source:=True)
            Throw New Exception("[" & nomeRoutine & "] : " & messaggioErrore)
        End Try

    End Sub

    Private Sub EliminaImpresa(objImpresa As AgronicaCoreModelsSTD.anagrafiche.Impresa,
                               ByRef GiasContext As Gias_DeveloperServer_Entities,
                               ByRef objParametri_Server As AgronicaCoreParametri,
                               ByRef objParametri_Utenti As AgronicaCoreParametri,
                               Optional ByVal NoteLog As String = NOTELOG_ANAGRAFE_NG)

        Dim Piva = objImpresa.partitaIva
        Dim PivaSuperUser = objParametri_Server.PivaSuperUser

        If (Piva = PivaSuperUser) Then
            Throw New GiasException(Gias.ImpossibileEliminareAziendaSuperuser)
        End If

        If (Piva Is Nothing) Then
            'A volte capita che non venga tirato su l'oggetto Impresa...non si capisce perché. Accade randomicamente
            Throw New Exception("Azienda non letta correttamente!")
        End If

        Dim GerarchiaPadre = (From gi In GiasContext.GerarchiaImprese Where gi.Padre = Piva).ToList

        If GerarchiaPadre.Count > 0 Then
            Throw New GiasException(String.Format(Gias.ImpossibileEliminareAziendaReferente, objImpresa.ragioneSociale))
        End If


        'Prima di poter eliminare controllo CdG e Movimenti
        Dim objControlloAgenda As New AgronicaCoreAnagrafeBIZ.Reg_Impianto_W
        Dim objControlloCdG As New AgronicaCoreAnagrafeBIZ.Progetto_W
        Dim controlloCdG = objControlloCdG.controllo_CdGxEliminazione(Nothing, Piva, 0, 0, 0, 0, objParametri_Server)
        If objControlloAgenda.controllo_MovimentiRicettexEliminazione(Nothing, Piva, 0, 0, 0, objParametri_Server) Then

            Dim MessaggioErroreAgenda As String = (String.Format(Gias.ImpossibileEliminareAziendaOperazioniRegistrateImpianti, objImpresa.ragioneSociale))
            Throw New GiasException(MessaggioErroreAgenda)

        ElseIf controlloCdG.errore Then

            Dim messaggioErroreCdG As String = (String.Format(Gias.ImpossibileEliminareAziendaCostiRegistratiEsercizio, objImpresa.ragioneSociale))
            Throw New GiasException(messaggioErroreCdG)
        End If
        Dim msgErroreVincoliPUA = objControlloAgenda.controllo_CancellazioneVincoliLetamazioniPUAxEliminazione(Nothing, Piva, 0, 0, 0, objParametri_Server)
        If msgErroreVincoliPUA <> "" Then
            Throw New GiasException(msgErroreVincoliPUA)
        End If


        Dim centri_R As New AgronicaCoreAnagrafeDAL.CentriAziendali_Read
        Dim centri_aziendaliDB = centri_R.Leggi(Piva, 0, enumSelezioneVariabile.Selezione_TabellaDatiMinimi, "", "", objParametri_Server)
        Dim objCentri_W As New AgronicaCoreAnagrafeBIZ.CentroAziendale_W
        For Each centro In centri_aziendaliDB.Rows()
            Dim objCentro As New AgronicaCoreModelsSTD.anagrafiche.CentroAziendale(New AgronicaCoreModelsSTD.anagrafiche.CentroAziendale.PK(centro("sa_cod"), centro("Piva")))
            objCentro.flag_cancellazione = True
            objCentri_W.Scrivi_Centro_Anagrafica(objCentro, objParametri_Server, objParametri_Utenti, NoteLog:=NoteLog)
        Next

        Dim RisorseUmaneDB = (From c In GiasContext.Risorse_Umane Where c.Piva = PivaSuperUser AndAlso c.Cod_Contatto = Piva)
        GiasContext.Risorse_Umane.RemoveRange(RisorseUmaneDB)
        GiasContext.SaveChanges()

        Dim ContattiDB = (From c In GiasContext.Contatti Where c.Piva = PivaSuperUser AndAlso c.Cod_Contatto = Piva)
        GiasContext.Contatti.RemoveRange(ContattiDB)
        GiasContext.SaveChanges()

        Dim UtentiXImpreseDB = (From i In GiasContext.UtentiXImprese Where i.PIVA = Piva).ToList
        GiasContext.UtentiXImprese.RemoveRange(UtentiXImpreseDB)
        GiasContext.SaveChanges()

        Dim ImpresexIndirizziDB = (From i In GiasContext.ImpresexIndirizzi Where i.PIVA = Piva).ToList
        GiasContext.ImpresexIndirizzi.RemoveRange(ImpresexIndirizziDB)
        GiasContext.SaveChanges()

        Dim indirizzi_Cod As List(Of Integer) = ImpresexIndirizziDB.Select(Function(x) x.cod_indirizzo).ToList
        Dim IndirizziDB = (From i In GiasContext.Indirizzi Where indirizzi_Cod.Contains(i.cod_indirizzo)).ToList
        GiasContext.Indirizzi.RemoveRange(IndirizziDB)
        GiasContext.SaveChanges()

        Dim Imprese_CodiciDB = (From i In GiasContext.Imprese_Codici Where i.PIVA = Piva).ToList
        GiasContext.Imprese_Codici.RemoveRange(Imprese_CodiciDB)
        GiasContext.SaveChanges()

        Dim GerarchiaImpreseDB = (From gi In GiasContext.GerarchiaImprese Where gi.Figlio = Piva).ToList
        GiasContext.GerarchiaImprese.RemoveRange(GerarchiaImpreseDB)
        GiasContext.SaveChanges()
        For Each impresaPadre In GerarchiaImpreseDB
            controllaReimpostaFogliaGerarchiaImprese(impresaPadre.Padre, impresaPadre, Modifica_0_Cancellazione_1:=1, GiasContext)
        Next

        Dim ImpreseDB = (From i In GiasContext.Imprese Where i.PIVA = Piva).ToList
        GiasContext.Imprese.RemoveRange(ImpreseDB)
        GiasContext.SaveChanges()



        Dim DatiImpresaStr = ""
        If objImpresa IsNot Nothing Then
            Dim a As New JsonSerializerSettings With {.DateTimeZoneHandling = DateTimeZoneHandling.Local}
            DatiImpresaStr = JsonConvert.SerializeObject(objImpresa, a)
        End If

        'Scrittura tabella Agronica_Log_Anagrafe
        Dim objLogAnagrafeW As New AgronicaLogAnagrafe_W
        Dim log As Agronica_Log_Anagrafe = objLogAnagrafeW.CreaLogAnagrafeEF(enum_TipoEntita_Des.Imprese,
                                                                             CStr(Piva), "",
                                                                             "", "",
                                                                             Nothing, Nothing,
                                                                             enum_TipoOperazioneDB.Cancellazione,
                                                                             objParametri_Server, enum_Id_Servizio.GiasOnline,
                                                                             NoteLog, DatiImpresaStr)
        GiasContext.Agronica_Log_Anagrafe.Add(log)
        GiasContext.SaveChanges()

    End Sub

#Region "SOSTITUITO CON Scrivi_Codici() in AgronicaCore_2010\AgronicaCoreAnagrafeBIZ\Codici.vb"
    'SOSTITUITO CON Scrivi_Indirizzi_Associati() in AgronicaCore_2010\AgronicaCoreAnagrafeBIZ\Indirizzi.vb
    'Private Function Scrivi_Indirizzo_Impresa(indirizzoAssociato As AgronicaCoreModelsSTD.anagrafiche.IndirizzoAssociato,
    '                                        ByRef GiasContext As Gias_DeveloperServer_Entities,
    '                                        ByRef Impresa As AgronicaCoreEntityFramework_POCO.Imprese,
    '                                        ByRef objParametri_Server As AgronicaCoreDataProvider.AgronicaCoreParametri,
    '                                        ByRef objParametri_Utenti As AgronicaCoreDataProvider.AgronicaCoreParametri)

    '    'Dim Cod_Indirizzo As Integer
    '    'Dim indirizzoEF As AgronicaCoreEntityFramework_POCO.Indirizzi

    '    'If indirizzoAssociato.Cod_Indirizzo = 0 Then

    '    '    indirizzoEF = AgronicaCoreAnagrafeDAL.EFImprese.CreateIndirizzoImpresa(GiasContext, objParametri_Server, Impresa, indirizzoAssociato.tipo_Indirizzo, objParametri_Utenti.UsernameOperazione)
    '    '    Cod_Indirizzo = indirizzoEF.cod_indirizzo

    '    'Else

    '    '    Cod_Indirizzo = indirizzoAssociato.Cod_Indirizzo
    '    '    indirizzoEF = (From ind In GiasContext.Indirizzi Where
    '    '                                                     ind.cod_indirizzo = Cod_Indirizzo).FirstOrDefault()
    '    'End If

    '    'indirizzoEF.ind_des = indirizzoAssociato.Via
    '    'indirizzoEF.frz_des = indirizzoAssociato.Frazione
    '    'indirizzoEF.pro_cod_istat = indirizzoAssociato.Prov
    '    'indirizzoEF.com_cod_istat = indirizzoAssociato.Com
    '    'indirizzoEF.CAP = indirizzoAssociato.CAP
    '    'indirizzoEF.stato = indirizzoAssociato.Stato
    '    'indirizzoEF.note = indirizzoAssociato.Note

    '    'Return Cod_Indirizzo

    'End Function
#End Region
#Region "SOSTITUITO CON Scrivi_Codici() in AgronicaCore_2010\AgronicaCoreAnagrafeBIZ\Codici.vb"
    'SOSTITUITO CON Scrivi_Codici() in AgronicaCore_2010\AgronicaCoreAnagrafeBIZ\Codici.vb
    'Private Function Scrivi_Codice_Impresa(codici As AgronicaCoreModelsSTD.anagrafiche.CodiciAnagrafeValori,
    '                                       ByRef GiasContext As Gias_DeveloperServer_Entities,
    '                                       ByRef Impresa As AgronicaCoreEntityFramework_POCO.Imprese,
    '                                       ByRef objParametri_Server As AgronicaCoreDataProvider.AgronicaCoreParametri,
    '                                       ByRef objParametri_Utenti As AgronicaCoreDataProvider.AgronicaCoreParametri)
    '    Scrivi_Codice_Impresa(codici.codiceAnagrafe.codice, codici.valore, GiasContext, Impresa, objParametri_Server, objParametri_Utenti)

    '    Return codici.codiceAnagrafe.codice
    'End Function
#End Region

    Private Sub Scrivi_Gerarchia_Impresa(Piva As String,
                                         Tipo_Impresa As Integer,
                                         listaPadri As List(Of ImpresaPadre),
                                         Tipo_Operazione As Integer,
                                         ByRef objParametri_Server As AgronicaCoreDataProvider.AgronicaCoreParametri,
                                         ByRef objParametri_Utenti As AgronicaCoreDataProvider.AgronicaCoreParametri,
                                         GiasContext As Gias_DeveloperServer_Entities)
        Dim GerarchiaImprese_Livello As New AgronicaCoreAnagrafeDAL.GerarchiaImprese_R
        If Tipo_Operazione = "1" Then

            If listaPadri.Count = 0 Then
                Dim listImpresePadri = Leggi_Imprese_Padri(objParametri_Server, objParametri_Utenti)
                Dim listStrPivaPadri = (From p In listImpresePadri Select p.partitaIva).ToList

                If listStrPivaPadri.Contains(objParametri_Server.PivaSuperUser) Then
                    listaPadri.Add(New ImpresaPadre With {.partitaIva = objParametri_Server.PivaSuperUser})
                Else
                    listaPadri.Add(New ImpresaPadre With {.partitaIva = listStrPivaPadri(0)})
                End If

            End If
            For Each padre In listaPadri
                Scrivi_Gerarchia_Impresa(Piva, padre.partitaIva, If(IsNothing(padre.codice_iscrizione_libro_soci), "", padre.codice_iscrizione_libro_soci),
                                         If(padre.data_iscrizione_libro_soci = Date.MinValue, AGRODATAINIZIO, padre.data_iscrizione_libro_soci),
                                         Tipo_Impresa,
                                         objParametri_Server,
                                         objParametri_Utenti,
                                         GiasContext)
            Next

        Else
            If listaPadri.Count = 0 AndAlso Piva <> objParametri_Server.PivaSuperUser Then
                Throw New Exception("Non è possibile modificare un impresa senza selezionare un'impresa Padre")
            End If

            If Piva = objParametri_Server.PivaSuperUser Then
                Return
            End If

            Dim listaPadriDB As List(Of String)
            Dim dtGerarchia = GerarchiaImprese_Livello.LeggixFiglio(Piva, enumSelezioneVariabile.Selezione_TabellaDatiMinimi, "", "", objParametri_Server)
            listaPadriDB = (From r In dtGerarchia.Rows Select CStr(r("Padre"))).ToList()

            Dim listaAdd As New List(Of ImpresaPadre)
            Dim listaDel As New List(Of String)

            For Each padre In listaPadri
                If Not listaPadriDB.Contains(padre.partitaIva) Then
                    listaAdd.Add(padre)
                Else
                    'If Not (From r In dtGerarchia.Select("Padre = '" + padre.partitaIva + "'")
                    '        Select If(IsDBNull(r("LibroSoci_Codice")), "", CStr(r("LibroSoci_Codice")))).ToList()(0).Equals(padre.codice_iscrizione_libro_soci) Or
                    '        Not Date.ParseExact((From r In dtGerarchia.Select("Padre = '" + padre.partitaIva + "'")
                    '                             Select If(IsDBNull(r("LibroSoci_DataIscrizione")), AGRODATAINIZIO, CStr(r("LibroSoci_DataIscrizione")))).ToList()(0), "dd/MM/yyyy", System.Globalization.DateTimeFormatInfo.InvariantInfo).Equals(padre.data_iscrizione_libro_soci) Then
                    listaDel.Add(padre.partitaIva)
                    listaAdd.Add(padre)
                    'End If
                End If
            Next

            For Each padre In listaPadriDB
                Dim listaPadriString As New List(Of String)
                For Each impresa In listaPadri
                    listaPadriString.Add(impresa.partitaIva)
                Next
                If Not listaPadriString.Contains(padre) Then
                    listaDel.Add(padre)
                End If
            Next

            For Each padre In listaDel
                Elimina_Gerarchia_Impresa(Piva, padre, objParametri_Server, objParametri_Utenti, GiasContext)
            Next

            For Each padre In listaAdd
                Scrivi_Gerarchia_Impresa(Piva, padre.partitaIva, If(IsNothing(padre.codice_iscrizione_libro_soci), "", padre.codice_iscrizione_libro_soci),
                                         If(padre.data_iscrizione_libro_soci = Date.MinValue, AGRODATAINIZIO, padre.data_iscrizione_libro_soci),
                                         Tipo_Impresa,
                                         objParametri_Server,
                                         objParametri_Utenti,
                                         GiasContext)
            Next

        End If
    End Sub

    Private Sub Scrivi_Gerarchia_Impresa(Figlio As String,
                                         Padre As String,
                                         Codice_Iscrizione As String,
                                         Data_Iscrizione As Date,
                                         Tipo_Impresa_Figlio As Integer,
                                         ByRef objParametri_Server As AgronicaCoreDataProvider.AgronicaCoreParametri,
                                         ByRef objParametri_Utenti As AgronicaCoreDataProvider.AgronicaCoreParametri,
                                         GiasContext As Gias_DeveloperServer_Entities)
        Dim GerarchiaImprese_Livello As New AgronicaCoreAnagrafeDAL.GerarchiaImprese_R
        Dim Livello As Integer

        Dim dtFigli = GerarchiaImprese_Livello.LeggixFiglio(Padre, enumSelezioneVariabile.Selezione_TabellaCompleta, "", "", objParametri_Server)

        If dtFigli.Rows.Count > 0 Then
            Livello = dtFigli(0).Item("livello") + 1
        End If

        Dim Foglia As Integer
        'If Tipo_Impresa_Figlio = TIPO_IMPRESA Then
        Foglia = 1
        'Else
        '    Foglia = 0
        'End If


        Dim GerarchiaImprese As AgronicaCoreEntityFramework_POCO.GerarchiaImprese
        controllaReimpostaFogliaGerarchiaImprese(Figlio, Nothing, Modifica_0_Cancellazione_1:=0, GiasContext, Foglia)
        GerarchiaImprese = AgronicaCoreAnagrafeDAL.EFImprese.CreateGerarchiaImprese(GiasContext, objParametri_Server, Padre, Figlio, Foglia, Livello, objParametri_Utenti.UsernameOperazione, objParametri_Utenti, Codice_Iscrizione, Data_Iscrizione)

        Dim GerarchiePadre = (From g In GiasContext.GerarchiaImprese Where g.Figlio = Padre AndAlso g.Foglia = 1).ToList
        If GerarchiePadre IsNot Nothing AndAlso GerarchiePadre.Count > 0 Then
            For Each gerarchiaPadre In GerarchiePadre
                gerarchiaPadre.Foglia = 0
            Next
            GiasContext.SaveChanges()
        End If

    End Sub

    Private Sub Elimina_Gerarchia_Impresa(Figlio As String,
                                         Padre As String,
                                         ByRef objParametri_Server As AgronicaCoreDataProvider.AgronicaCoreParametri,
                                         ByRef objParametri_Utenti As AgronicaCoreDataProvider.AgronicaCoreParametri,
                                         GiasContext As Gias_DeveloperServer_Entities)


        Dim gerarchiaImpresa = (From g In GiasContext.GerarchiaImprese Where g.Padre = Padre AndAlso g.Figlio = Figlio).FirstOrDefault

        If gerarchiaImpresa IsNot Nothing Then
            GiasContext.GerarchiaImprese.Remove(gerarchiaImpresa)
            GiasContext.SaveChanges()

            controllaReimpostaFogliaGerarchiaImprese(Padre, gerarchiaImpresa, Modifica_0_Cancellazione_1:=1, GiasContext)
        End If

    End Sub

    Private Shared Sub controllaReimpostaFogliaGerarchiaImprese(Padre As String,
                                                                gerarchiaImpresa As GerarchiaImprese,
                                                                Modifica_0_Cancellazione_1 As Integer,
                                                                GiasContext As Gias_DeveloperServer_Entities,
                                                                Optional ByRef newFoglia As Integer = 0)

        If Modifica_0_Cancellazione_1 = 1 Then
            'Alla cancellazione, se esistono altri figli oltre all'azienda che sto rimuovendo.
            'Se non esistono, imposto la Foglia = 1
            Dim listaFigli = (From gi In GiasContext.GerarchiaImprese Where gi.Padre = Padre).ToList()
            If listaFigli.Count = 0 Then
                Dim impresaPadre = (From gi In GiasContext.GerarchiaImprese Where gi.Figlio = Padre).FirstOrDefault()
                If impresaPadre IsNot Nothing Then
                    impresaPadre.Foglia = 1
                End If

                GiasContext.SaveChanges()
            End If
        Else
            'Se l'impresa figlio ha figli a sua volta, imposto Foglia = 0
            Dim listaFigli = (From gi In GiasContext.GerarchiaImprese Where gi.Padre = Padre).ToList()
            If listaFigli.Count > 0 Then
                newFoglia = 0
            End If
        End If
    End Sub

    Private Function Scrivi_Codice_Impresa(id_cod As Integer,
                                           val_cod As String,
                                           ByRef GiasContext As Gias_DeveloperServer_Entities,
                                           ByRef Impresa As AgronicaCoreEntityFramework_POCO.Imprese,
                                           ByRef objParametri_Server As AgronicaCoreDataProvider.AgronicaCoreParametri,
                                           ByRef objParametri_Utenti As AgronicaCoreDataProvider.AgronicaCoreParametri)

        Dim nomeRoutine As String = "AgronicaCoreAnagrafeBIZ.Impresa.Scrivi_Codice_Impresa()"
        Dim messaggioErrore As String = ""

        Dim ImpreseCodiciEF As AgronicaCoreEntityFramework_POCO.Imprese_Codici

        Dim piva = Impresa.PIVA
        ImpreseCodiciEF = (From cod In GiasContext.Imprese_Codici Where
                                                         cod.id_cod = id_cod AndAlso
                                                         piva = cod.PIVA).FirstOrDefault()
        Dim imp_codl = From ic In GiasContext.Imprese_Codici
                       Where ic.PIVA = piva AndAlso
                             ic.id_cod = id_cod
                       Select ic

        Try
            If val_cod <> "" AndAlso val_cod <> "0" Then
                If ImpreseCodiciEF Is Nothing Then
                    ImpreseCodiciEF = AgronicaCoreAnagrafeDAL.EFImprese.CreateImprese_CodiciEF(GiasContext, objParametri_Server, Impresa, id_cod,
                                                                                           val_cod, objParametri_Utenti.UsernameOperazione)
                End If


                ImpreseCodiciEF.id_cod = id_cod
                ImpreseCodiciEF.val_cod = val_cod
                ImpreseCodiciEF.Username_Modifica = objParametri_Server.UsernameOperazione
                ImpreseCodiciEF.Data_Modifica = DateTime.Now

            Else
                If ImpreseCodiciEF IsNot Nothing Then
                    ' funzione per cancellare il record
                    Dim app_cod = imp_codl.FirstOrDefault
                    GiasContext.Imprese_Codici.Attach(app_cod)
                    GiasContext.Imprese_Codici.Remove(app_cod)
                End If
            End If
            GiasContext.SaveChanges()

        Catch ex As Exception
            messaggioErrore = ex.Message
            Throw New Exception("[" & nomeRoutine & "] : " & messaggioErrore)
        End Try

        Return id_cod
    End Function

    'Private Sub Scrivi_Contatti_Impresa(contatti As AgronicaCoreModelsSTD.anagrafiche.Contatto,
    '                                    objImpresa As AgronicaCoreModelsSTD.anagrafiche.Impresa,
    '                                    ByRef GiasContext As Gias_DeveloperServer_Entities,
    '                                    ByRef Impresa As AgronicaCoreEntityFramework_POCO.Imprese,
    '                                    ByRef Contatto As AgronicaCoreEntityFramework_POCO.Contatti,
    '                                    ByRef objParametri_Server As AgronicaCoreParametri,
    '                                    ByRef objParametri_Utenti As AgronicaCoreParametri,
    '                                    tipo_operazione As Integer)

    '    Const nomeRoutine = "AgronicaCoreAnagrafeBIZ.Impresa.Scrivi_Contatti_Impresa()"
    '    Dim messaggioErrore As String = ""

    '    Dim trovato As Boolean = False

    '    Dim Cod_Contatto As String = contatti.primaryKey.codice
    '    Dim id_CF As Integer = Ottieni_ID_CF_Contatto((objImpresa.indirizzi(0).indirizzo.stato).ToString)
    '    Dim piva = objImpresa.partitaIva

    '    Dim RisorseUmaneEF As AgronicaCoreEntityFramework_POCO.Risorse_Umane
    '    Dim ContattiEF As AgronicaCoreEntityFramework_POCO.Contatti

    '    Dim pivaSuperUser = objParametri_Server.PivaSuperUser
    '    ContattiEF = (From cont In GiasContext.Contatti Where
    '                                                    cont.Piva = pivaSuperUser AndAlso
    '                                                    cont.Cod_Contatto = Cod_Contatto).FirstOrDefault()

    '    Try
    '        If ContattiEF Is Nothing Then
    '            If tipo_operazione = "1" Then 'SCRITTURA IMPRESA
    '                'il contatto non esiste
    '                Dim sa_cod = 0
    '                If contatti.visibilitaPubblica Then
    '                    sa_cod = -1
    '                End If
    '                ContattiEF = AgronicaCoreAnagrafeDAL.EFImprese.CreaContattoImpresa(GiasContext,
    '                                                                                  objParametri_Server,
    '                                                                                  Impresa,
    '                                                                                  Cod_Contatto,
    '                                                                                  sa_cod,
    '                                                                                  id_CF,
    '                                                                                  objParametri_Server.UsernameOperazione
    '                                                                                  )
    '            End If

    '            For Each risorsa In contatti.risorseUmane
    '                Dim Cod_Risum As Integer = risorsa.codice
    '                Dim Cod_Rapporto As Integer = risorsa.rapportoContabile.codice
    '                RisorseUmaneEF = (From risum In GiasContext.Risorse_Umane Where
    '                                                              risum.Piva = pivaSuperUser AndAlso
    '                                                              risum.Cod_Rapporto = Cod_Rapporto AndAlso
    '                                                              risum.Cod_RisUm = Cod_Risum).FirstOrDefault()
    '                If RisorseUmaneEF Is Nothing Then
    '                    'risorse umane non esiste
    '                    RisorseUmaneEF = AgronicaCoreAnagrafeDAL.EFImprese.CreaRisorseUmaneImpresa(GiasContext,
    '                                                                                       objParametri_Server,
    '                                                                                       Impresa,
    '                                                                                       ContattiEF,
    '                                                                                       objParametri_Utenti.UsernameOperazione
    '                                                                                       )
    '                    trovato = True
    '                End If

    '                If piva = objParametri_Server.PivaSuperUser Then
    '                    RisorseUmaneEF.Sa_Cod = PUBBLICO
    '                    Cod_Rapporto = COD_CLIENTE
    '                Else
    '                    Cod_Rapporto = COD_FORNITORE
    '                End If

    '                RisorseUmaneEF.Cod_Rapporto = risorsa.rapportoContabile.codice   'contatti.Cod_Rapporto     
    '                RisorseUmaneEF.Settore_Des = risorsa.settore
    '                RisorseUmaneEF.Attivita_Des = risorsa.attivita
    '            Next

    '        End If

    '        'se MODIFICA IMPRESA
    '        'Modifico solo il contatto
    '        If piva = objParametri_Server.PivaSuperUser Then
    '            'Cod_Rapporto = COD_CLIENTE
    '            ContattiEF.Sa_Cod = PUBBLICO
    '        End If
    '        ContattiEF.Rag_Soc = objImpresa.ragioneSociale
    '        ContattiEF.Codice_Fiscale = objImpresa.CUAA


    '        GiasContext.SaveChanges()
    '    Catch ex As Exception
    '        messaggioErrore = ex.Message
    '        Throw New Exception("[" & nomeRoutine & "] : " & messaggioErrore)
    '    End Try

    'End Sub

    Public Function Leggi_Imprese_Padri(objParametri_Server As AgronicaCoreParametri,
                                        objParametri_Utenti As AgronicaCoreParametri) As List(Of AgronicaCoreModelsSTD.anagrafiche.Impresa)
        Dim list As New List(Of AgronicaCoreModelsSTD.anagrafiche.Impresa)

        Dim ClassJoin As New JoinFiltrone
        ClassJoin.bGerarchiaImprese = True
        Dim classFiltrone As New AgronicaCoreUtility.Filtrone
        Dim xFiltroAggiuntivo = " TipoImpresaGerarchia<>1 "
        Dim xOrderBy = ""
        classFiltrone.ImpostaVariabiliJOIN_xFiltroUtente(xFiltroAggiuntivo, ClassJoin)
        Dim Dt_Imprese = classFiltrone.CreaDTFiltrone(objParametri_Server,
                                                    xFiltroAggiuntivo,
                                                    enum_TipoSelect_FiltroneSuperNova.Imprese,
                                                    xOrderBy,
                                                    ClassJoin)

        For Each row In Dt_Imprese.Rows
            Dim impresa = New AgronicaCoreModelsSTD.anagrafiche.Impresa()
            impresa.partitaIva = row("Piva")
            impresa.ragioneSociale = row("Rag_Soc")
            If IsDBNull(row("CodiceCuaa")) Then
                impresa.CUAA = ""
            Else
                impresa.CUAA = CStr(row("CodiceCuaa"))
            End If
            list.Add(impresa)
        Next

        Dim esisteSu = list.Where(Function(x) x.partitaIva = objParametri_Server.PivaSuperUser).FirstOrDefault()

        If esisteSu Is Nothing Then
            Dim objImprese_R As New AgronicaCoreAnagrafeDAL.Imprese_Read
            Dim dt_Su = objImprese_R.Leggi(objParametri_Server.PivaSuperUser, enumSelezioneVariabile.Selezione_TabellaCompleta, "", "", objParametri_Server)
            If dt_Su.Rows.Count Then
                list.Add(
                        New AgronicaCoreModelsSTD.anagrafiche.Impresa() With {
                            .partitaIva = dt_Su.Rows(0)("Piva"),
                            .ragioneSociale = dt_Su.Rows(0)("Rag_Soc")
                        }
                )
            End If

        End If

        Return list
    End Function

    Public Function GeneraPivaFittizia(ByRef objParametri_Server As AgronicaCoreDataProvider.AgronicaCoreParametri) As String

        Dim NomeRoutine As String = "AgronicaCoreDataProvider.Agro_Sequenze.GeneraPivaFittizia()"
        Dim MessaggioErrore As String = ""
        Dim StrSQL As New System.Text.StringBuilder
        Dim DT As DataTable

        Dim piva As String = ""

        Try

            objParametri_Server.ImpostaFinestre_con_SalvataggioTemporale(AGRODATAINIZIO, AGRODATAFINE)

            Dim ok = False


            Dim objAgroSe As New AgronicaCoreDataProvider.Agro_Sequenze
            Dim objCont As New AgronicaCoreAnagrafeDAL.Contatti_R

            Dim str_r As String = ""
            While Not ok

                Dim objConfig As New AgronicaCoreVarieDAL.Configurazione_Siti_R
                Dim PrefissoPIVAGenerica As String = objConfig.Leggi_Valore(0, "Prefisso_PIVA_Generica_Da_Assegnare", "", "", objParametri_Server)
                str_r = objAgroSe.NuovoId_Tabella("impresa", 0, 0, objParametri_Server).ToString.Replace("-", If(PrefissoPIVAGenerica = "", "F", PrefissoPIVAGenerica))
                'controllo se è già usato 
                DT = objCont.LeggiContattoSpecifico("", str_r, 0, enumSelezioneVariabile.Selezione_TabellaDatiMinimi, "", "", objParametri_Server)
                If DT.Rows.Count = 0 Then
                    ok = True
                End If
            End While

            objParametri_Server.ResettaFinestra()

            piva = str_r

        Catch ex As Exception

            MessaggioErrore = ex.Message
            Scrivi_LOG(objParametri_Server, NomeRoutine, MessaggioErrore)
            DT = Nothing
            Throw New Exception("[" & NomeRoutine & "] : " & MessaggioErrore)

        End Try

        Return piva

    End Function

    Private Function Verifica_PivaImpresa(ByRef impresa As AgronicaCoreModelsSTD.anagrafiche.Impresa,
                                               ByRef objParametri_Server As AgronicaCoreDataProvider.AgronicaCoreParametri)

        Dim MessaggioErrore As String = ""

        Dim stato = impresa.indirizzi(0).indirizzo.stato.descrizione
        Dim piva = impresa.partitaIva

        Try
            If stato = "IT" AndAlso (piva.Length <> 11 AndAlso piva.Length <> 16) Then
                MessaggioErrore += ("Inserire una Partita IVA valida.")
                Throw New GiasException(MessaggioErrore)
            End If

            Dim objImpreser As New AgronicaCoreAnagrafeDAL.Imprese_Read
            If objImpreser.VerificaEsistenza_PivaGIAS(piva, objParametri_Server) Then
                MessaggioErrore += ("Partita IVA già inserita.")
                Throw New GiasException(MessaggioErrore)
            End If

            If Not AgronicaCoreDataProvider.UtilityProvider.PivaValida(piva) Then
                MessaggioErrore += ("Rilevato carattere non valido nella PIVA. Usare solo valori ALFANUMERICI (A-Z, 0-9).")
                Throw New GiasException(MessaggioErrore)
            End If

        Catch ex As GiasException
            MessaggioErrore = ex.Message
            Throw ex

        Catch ex As Exception
            MessaggioErrore = ex.Message
            Throw New Exception(MessaggioErrore)
        End Try

        Return MessaggioErrore
    End Function

    Private Function Ottieni_ID_CF_Contatto(ByVal Stato As String) As enum_Contatti_IdCf

        Dim id_cf As enum_Contatti_IdCf = enum_Contatti_IdCf.PersonaGiuridica

        If Stato.ToLower().Trim() <> "it" Then
            id_cf = enum_Contatti_IdCf.ContattoEstero
        End If

        Return id_cf

    End Function

    Private Function Scrivi_Contatti_ClienteFonitore(objImpresa As AgronicaCoreModelsSTD.anagrafiche.Impresa,
                                          ByRef GiasContext As Gias_DeveloperServer_Entities,
                                          ByRef Impresa As AgronicaCoreEntityFramework_POCO.Imprese,
                                          ByRef Contatto As AgronicaCoreEntityFramework_POCO.Contatti,
                                          ByRef objParametri_Server As AgronicaCoreDataProvider.AgronicaCoreParametri,
                                          ByRef objParametri_Utenti As AgronicaCoreDataProvider.AgronicaCoreParametri,
                                             tipo_operazione As Integer
                                             )

        Dim nomeRoutine As String = "AgronicaCoreAnagrafeBIZ.Impresa.Scrivi_Contatti_ClienteFonitore()"
        Dim messaggioErrore As String = ""

        Dim Cod_Risum As Integer
        Dim Cod_Rapporto As Integer
        Dim Cod_Contatto As String = objImpresa.partitaIva
        Dim id_CF As Integer = Ottieni_ID_CF_Contatto((objImpresa.indirizzi(0).indirizzo.stato).ToString)


        Dim RisorseUmaneEF As AgronicaCoreEntityFramework_POCO.Risorse_Umane
        Dim ContattiEF As AgronicaCoreEntityFramework_POCO.Contatti

        Try
            Dim ImpresaSuperuserEF As AgronicaCoreEntityFramework_POCO.Imprese
            Dim pivaSuperUser = objParametri_Server.PivaSuperUser
            ImpresaSuperuserEF = (From superuser In GiasContext.Imprese Where
                                                         superuser.PIVA = pivaSuperUser).FirstOrDefault()

            Dim sa_cod = 0
            If Contatto IsNot Nothing AndAlso Contatto.Sa_Cod <> 0 Then
                sa_cod = Contatto.Sa_Cod
            End If

            ContattiEF = AgronicaCoreAnagrafeDAL.EFImprese.CreaContattoImpresa(GiasContext,
                                                                                  objParametri_Server,
                                                                                  ImpresaSuperuserEF,
                                                                                  Cod_Contatto,
                                                                                  sa_cod,
                                                                                  id_CF,
                                                                                  objParametri_Server.UsernameOperazione
                                                                                  )
            For i = 0 To 1
                If i = 0 Then
                    Cod_Rapporto = COD_CLIENTE
                Else
                    Cod_Rapporto = COD_FORNITORE
                End If

                RisorseUmaneEF = AgronicaCoreAnagrafeDAL.EFImprese.CreaRisorseUmaneImpresa(GiasContext,
                                                                                       objParametri_Server,
                                                                                       ImpresaSuperuserEF,
                                                                                       ContattiEF,
                                                                                       objParametri_Utenti.UsernameOperazione
                                                                                       )

                RisorseUmaneEF.Cod_Rapporto = Cod_Rapporto
                GiasContext.SaveChanges()
            Next

            ContattiEF.Codice_Fiscale = objImpresa.CUAA
            ContattiEF.Rag_Soc = objImpresa.ragioneSociale
            ContattiEF.Codice_Fiscale = objImpresa.CUAA

            GiasContext.SaveChanges()
        Catch ex As Exception
            messaggioErrore = ex.Message
            Throw New Exception("[" & nomeRoutine & "] : " & messaggioErrore)
        End Try


        Return Cod_Risum
    End Function

    Private Function Verifica_ValiditaInizioFine(ByRef impresa As AgronicaCoreModelsSTD.anagrafiche.Impresa,
                                                 ByRef GiasContext As AgronicaCoreEntityFramework.Gias_DeveloperServer_Entities,
                                                 ByRef objParametri_Server As AgronicaCoreDataProvider.AgronicaCoreParametri,
                                                 ByRef objParametri_Utenti As AgronicaCoreDataProvider.AgronicaCoreParametri)

        Dim MessaggioErrore As String = ""

        Dim Validita_Fine = impresa.validita.fine
        Dim Validita_Inizio = impresa.validita.inizio
        Dim piva = impresa.partitaIva

        Dim objCentroR As New AgronicaCoreAnagrafeDAL.CentriAziendali_Read
        Dim centro = objCentroR.Leggi(piva, 0, enumSelezioneVariabile.Selezione_TabellaCompleta, "", "", objParametri_Server)

        Dim objImpresaR As New AgronicaCoreAnagrafeDAL.Imprese_Read
        Dim imp = objImpresaR.Leggi(piva, enumSelezioneVariabile.Selezione_TabellaCompleta, "", "", objParametri_Server)

        Try

            If imp.Rows.Count > 0 Then

                'Se l'intervallo delle validità viene modificato, controllo e aggiorno, se necessario, il Centro
                If Validita_Inizio <> imp.Rows(0)("Validita_Inizio") OrElse Validita_Fine <> imp.Rows(0)("Validita_Fine") Then

                    If centro.Rows.Count > 0 Then
                        For Each c In centro.Rows
                            Dim sa_cod = c("sa_cod")

                            'Ripristino le date dopo ogni iterazione
                            Dim Validita_InizioNew = Validita_Inizio
                            Dim Validita_FineNew = Validita_Fine

                            'Controllo se le date del Centro rientrano nell'intervallo temporale dell'Impresa, in questo caso rimangono invariate
                            If c("Validita_Inizio") > Validita_InizioNew Then
                                Validita_InizioNew = c("Validita_Inizio")
                            End If
                            If Validita_FineNew > c("Validita_Fine") Then
                                Validita_FineNew = c("Validita_Fine")
                            End If

                            If c("Validita_Inizio") <> Validita_InizioNew Or Validita_FineNew <> c("Validita_Fine") Then
                                Dim objCentriR As New AgronicaCoreAnagrafeBIZ.CentroAziendale_R
                                Dim cenR = objCentriR.Centro_Leggi_Anagrafica(Piva:=piva,
                                                                    Sa_Cod:=sa_cod,
                                                                    Leggi_Impresa:=True,
                                                                    Leggi_Indirizzo:=True,
                                                                    Leggi_Codici:=True,
                                                                    Leggi_Rubrica:=True,
                                                                    Leggi_Catasto:=False,
                                                                    objParametri_Server
                                                                    )

                                cenR.validita = New AgronicaCoreModelsSTD.anagrafiche.IntervalloTemporale(Validita_InizioNew, Validita_FineNew)

                                Dim objCentriW As New AgronicaCoreAnagrafeBIZ.CentroAziendale_W
                                Dim cenW = objCentriW.Scrivi_Centro_Anagrafica(cenR, objParametri_Server, objParametri_Utenti)
                            End If
                        Next
                    End If
                End If
            End If

            If Validita_Fine < Validita_Inizio Then
                MessaggioErrore += ("La fine dell'Impresa non può precedere la sua data di inizio.")
                Throw New GiasException(MessaggioErrore)
            End If
        Catch ex As GiasException
            MessaggioErrore = ex.Message
            Throw ex
        Catch ex As Exception
            MessaggioErrore = ex.Message
            Throw New Exception(MessaggioErrore)
        End Try

        Return MessaggioErrore
    End Function

    Private Sub Crea_Centro(ByRef objImpresa As AgronicaCoreModelsSTD.anagrafiche.Impresa,
                                             ByRef objParametri_Server As AgronicaCoreDataProvider.AgronicaCoreParametri,
                                             ByRef objParametri_Utenti As AgronicaCoreDataProvider.AgronicaCoreParametri,
                                             ByRef GiasContext As Gias_DeveloperServer_Entities,
                                             Optional ByVal nomeCentro As String = Nothing,
                                             Optional ByVal creaMagazzino As Boolean = True,
                                             Optional ByVal nomeMagazzino As String = Nothing
                                             )

        Dim objCentro As New AgronicaCoreModelsSTD.anagrafiche.CentroAziendale(New AgronicaCoreModelsSTD.anagrafiche.CentroAziendale.PK(0, objImpresa.partitaIva))
        objCentro.nome = If(nomeCentro = Nothing, $"Centro {objImpresa.ragioneSociale}", nomeCentro)
        objCentro.tipologia = New AgronicaCoreModelsSTD.metaschema.TipologiaSede(AgronicaCoreModelsSTD.metaschema.TipologiaSede.TIPO.SedeLegale)

        If objImpresa.indirizzi IsNot Nothing AndAlso objImpresa.indirizzi.Count > 0 Then
            Dim indirizziAssociati As New List(Of AgronicaCoreModelsSTD.anagrafiche.IndirizzoAssociato)
            Dim indirizzoAssociato As New AgronicaCoreModelsSTD.anagrafiche.IndirizzoAssociato()
            indirizzoAssociato.tipo_Indirizzo = 1
            Dim indirizzo As New AgronicaCoreModelsSTD.anagrafiche.Indirizzo
            indirizzo.codice = 0
            indirizzo.cap = objImpresa.indirizzi(0).indirizzo.cap
            indirizzo.frazione = objImpresa.indirizzi(0).indirizzo.frazione
            indirizzo.note = objImpresa.indirizzi(0).indirizzo.note
            indirizzo.stato = objImpresa.indirizzi(0).indirizzo.stato
            indirizzo.via = objImpresa.indirizzi(0).indirizzo.via
            indirizzo.istatComune = objImpresa.indirizzi(0).indirizzo.istatComune
            indirizzoAssociato.indirizzo = indirizzo
            indirizziAssociati.Add(indirizzoAssociato)
            objCentro.indirizzi = indirizziAssociati
        End If

        objCentro.validita = New AgronicaCoreModelsSTD.anagrafiche.IntervalloTemporale(objImpresa.validita.inizio, objImpresa.validita.fine)

        Dim objCentro_W As New AgronicaCoreAnagrafeBIZ.CentroAziendale_W
        objCentro_W.Scrivi_Centro_Anagrafica(objCentro, objParametri_Server, objParametri_Utenti, creaMagazzino:=creaMagazzino, nomeMagazzino:=nomeMagazzino)

        ' restituisce centro aziendale creato
        If objImpresa.centriAziendali Is Nothing Then
            objImpresa.centriAziendali = New List(Of CentroAziendale) From {objCentro}
        End If

    End Sub

    Public Sub ImpostaVisibilitaUtente(ByRef objImpresa As AgronicaCoreModelsSTD.anagrafiche.Impresa,
                                       ByRef objParametri_Server As AgronicaCoreDataProvider.AgronicaCoreParametri,
                                       ByRef objParametri_Utenti As AgronicaCoreDataProvider.AgronicaCoreParametri)


        Dim username As String = objParametri_Server.UtenteUsername
        Dim cancellato As Boolean = objImpresa.flag_cancellazione
        Dim piva As String = objImpresa.partitaIva

        Dim utenteProfiliLeggi As New AgronicaCoreUtentiDAL.Utenti_Profili_Read
        Dim dtProfili As DataTable = utenteProfiliLeggi.Leggi(username, 5, AgronicaCoreParametri.enumSelezioneVariabile.Selezione_TabellaDatiMinimi, "", "", objParametri_Utenti)

        Dim listaPivaLette As New List(Of String)

        If dtProfili.Rows.Count > 0 AndAlso Not String.IsNullOrEmpty(dtProfili.Rows(0)("Descrizione_1")) Then

            Dim xDocPiva As XDocument = XDocument.Parse(dtProfili.Rows(0)("Descrizione_1").ToString())
            listaPivaLette = (From ll In xDocPiva.Elements("DatiFiltri").Elements("Filtro").Elements("DatiPive").Elements("Piva") Where CStr(ll.Attribute("piva")) <> piva Select CStr(ll.Attribute("piva"))).ToList

            If Not cancellato AndAlso Not listaPivaLette.Contains(piva) Then
                listaPivaLette.Add(piva)
            End If

            Dim d1 As String = ""
            Dim d2 As String = ""

            If listaPivaLette.Count > 0 Then

                Dim xDocRval As XDocument = XDocument.Parse("<DatiFiltri><Filtro><DatiPive></DatiPive><Impresa><Struttura><Appezzamento><Impianto><Agenda><Contatto /></Agenda></Impianto></Appezzamento></Struttura></Impresa><DatiGerarchiaImprese/></Filtro></DatiFiltri>")
                Dim lRval As New List(Of String)

                For Each curPiva In listaPivaLette
                    Dim xN As XElement = <Piva piva=<%= curPiva %>/>
                    xDocRval.Element("DatiFiltri").Element("Filtro").Element("DatiPive").Add(xN)
                    lRval.Add("Imprese.piva = '" & curPiva & "'")
                Next

                d1 = xDocRval.ToString()
                d2 = " ( " & String.Join(" OR ", lRval.ToArray) & " )"

            End If

            Dim utente_profili As New AgronicaCoreUtentiDAL.Utenti_Profili_Write
            If dtProfili.Rows.Count > 0 Then
                utente_profili.Modifica(username, 5, d1, d2, 0, 0, "", objParametri_Utenti)
            Else
                utente_profili.Scrivi(username, 5, d1, d2, 0, 0, CostantiPersonalizzate.AGRODATAINIZIO, CostantiPersonalizzate.AGRODATAFINE, objParametri_Utenti)
            End If

            Try
                ' aggiorna visibilità appoggio
                Dim objUtentiVisibilita As New AgronicaCoreUtentiDAL.Utenti_Visibilita_Appoggio_W
                objUtentiVisibilita.Cancella(0, "Piva='" & piva & "'", objParametri_Server)
                If Not cancellato Then
                    objUtentiVisibilita.Scrivi(enum_TipoEntita.Impresa, piva, 0, 0, 0, objParametri_Server)
                End If

            Catch ex As Exception

            End Try


        End If


    End Sub

    Public Sub Scrivi_Impresa_APP(ByRef impresa As AgronicaCoreModelsSTD.anagrafiche.Impresa,
                                  ByVal tipoOperazione As AgronicaCoreDataProvider.TipiEnumerativi.enum_TipoOperazioneDB,
                                  ByRef objParametri_Server As AgronicaCoreDataProvider.AgronicaCoreParametri,
                                  ByRef objParametri_Utenti As AgronicaCoreDataProvider.AgronicaCoreParametri,
                                  Optional ByVal impresaPadre As String = "",
                                  Optional ByVal impostaVisibilita As Boolean = False,
                                  Optional NoteLog As String = NOTELOG_ANAGRAFE_APP,
                                  Optional ByVal OpenNewTransaction As Boolean = True)

        If String.IsNullOrEmpty(impresa.partitaIva) Then
            impresa.partitaIva = GeneraPivaFittizia(objParametri_Server)
        End If

        If String.IsNullOrEmpty(impresa.CUAA) Then
            impresa.CUAA = impresa.partitaIva
        End If

        If impresa.tipo_Impresa = 0 Then
            impresa.tipo_Impresa = enum_TipoImpresaGerarchia.Impresa
        End If

        If impresa.impresaPadre Is Nothing OrElse impresa.impresaPadre.Count = 0 Then
            impostaVisibilita = True
            impresa.impresaPadre = New List(Of ImpresaPadre) From {
                New ImpresaPadre With {.partitaIva = If(impresaPadre = "", objParametri_Server.PivaSuperUser, impresaPadre)}
            }
        End If

        ' fix creare contatto azienda sotto padre gerarchia
        Dim objImprese As New Impresa_R
        Dim padreGerarchia As String = objImprese.Leggi_Padre_Gerarchia(objParametri_Server, objParametri_Utenti)
        Dim pivaContatto As String = If(padreGerarchia <> "", padreGerarchia, impresa.impresaPadre.First.partitaIva)

        impresa.contatto_superuser = New Contatto() With {
            .primaryKey = New Contatto.PK(pivaContatto, impresa.partitaIva),
            .risorseUmane = New List(Of RisorseUmane) From {
                New RisorseUmane() With {.rapportoContabile = New RapportoContabile(enum_Rapporti_Contabili_Standard.Conferente)}
            }
        }

        ' ricavo telefono azienda
        Dim telefono As String = ""
        If impresa.codici IsNot Nothing AndAlso impresa.codici.Count > 0 Then
            Dim dati = (From c In impresa.codici Where c.codiceAnagrafe.codice = enum_CodiciAnagrafe.GiasAPP_Dati_Impresa Select c.valore).FirstOrDefault
            If Not String.IsNullOrEmpty(dati) Then
                Dim datiImpresa As JObject = JsonConvert.DeserializeObject(dati)
                If datiImpresa IsNot Nothing Then
                    If datiImpresa.ContainsKey("telephone") Then
                        telefono = datiImpresa.GetValue("telephone")
                    End If
                End If
            End If
        End If

        If impresa.indirizzi Is Nothing AndAlso tipoOperazione = enum_TipoOperazioneDB.Scrittura Then
            Dim indirizzo As New Indirizzo() With {
                .via = "", .frazione = "", .cap = "00000", .note = "",
                .istatComune = New AgronicaCoreModelsSTD.metaschema.Istat With {.com = "000", .prov = "000"},
                .stato = New AgronicaCoreModelsSTD.metaschema.CodiciNazioniISO3166("IT")
            }
            impresa.indirizzi = New List(Of IndirizzoAssociato) From {
                New IndirizzoAssociato() With {.indirizzo = indirizzo, .tipo_Indirizzo = enum_IndirizzoTipo.SedeOperativa}
            }
        End If

        ' modifico indirizzo azienda
        If impresa.indirizzi IsNot Nothing AndAlso tipoOperazione = enum_TipoOperazioneDB.Modifica Then
            Dim indirizzixImprese As New ImpresexIndirizzi_R
            Dim indirizzi = indirizzixImprese.Leggi(impresa.partitaIva, 0, 0, enumSelezioneVariabile.Selezione_TabellaDatiMinimi, "", "", objParametri_Server)
            If impresa.indirizzi.Count > 0 AndAlso indirizzi.Rows.Count > 0 Then
                impresa.indirizzi(0).indirizzo.codice = indirizzi.Rows(0)("cod_indirizzo")
            End If
        End If

        If impresa.validita Is Nothing Then
            impresa.validita = New IntervalloTemporale(CostantiPersonalizzate.AGRODATAINIZIO, CostantiPersonalizzate.AGRODATAFINE)
        End If

        Scrivi_Impresa_Anagrafica(impresa, objParametri_Server, objParametri_Utenti, False, NoteLog:=NoteLog, OpenNewTransaction:=OpenNewTransaction)

        If tipoOperazione = enum_TipoOperazioneDB.Scrittura Then

            ' crea centro aziendale app
            Dim objCentro As New CentroAziendale_W
            Dim centro = Crea_Centro_APP(impresa, telefono)
            objCentro.Scrivi_Centro_APP(centro, tipoOperazione, objParametri_Server, objParametri_Utenti, NoteLog:=NoteLog)
            impresa.centriAziendali = New List(Of CentroAziendale) From {centro}

            ' imposta visibilità aziende per utente
            If impostaVisibilita AndAlso objParametri_Server.UtenteUsername <> objParametri_Server.SuperUserUsername Then
                ImpostaVisibilitaUtente(impresa, objParametri_Server, objParametri_Utenti)
            End If

        End If

    End Sub

    Public Function Crea_Centro_APP(ByRef impresa As Impresa, Optional ByVal telefono As String = "") As CentroAziendale

        Dim centro As New CentroAziendale(New CentroAziendale.PK(0, impresa.partitaIva)) With {
            .nome = "Centro " & impresa.ragioneSociale,
            .validita = New IntervalloTemporale(impresa.validita.inizio, impresa.validita.fine),
            .indirizzi = New List(Of IndirizzoAssociato) From {
                New IndirizzoAssociato With {
                    .tipo_Indirizzo = 1,
                    .indirizzo = New Indirizzo With {
                        .codice = 0,
                        .cap = impresa.indirizzi(0).indirizzo.cap,
                        .frazione = impresa.indirizzi(0).indirizzo.frazione,
                        .note = impresa.indirizzi(0).indirizzo.note,
                        .stato = impresa.indirizzi(0).indirizzo.stato,
                        .via = impresa.indirizzi(0).indirizzo.via,
                        .istatComune = impresa.indirizzi(0).indirizzo.istatComune
                    }
                }
            }
        }

        If Not String.IsNullOrEmpty(telefono) Then
            centro.rubricaVoci = New List(Of AgronicaCoreModelsSTD.anagrafiche.RubricaVoci) From {
                New AgronicaCoreModelsSTD.anagrafiche.RubricaVoci() With {
                    .rubrica = New AgronicaCoreModelsSTD.anagrafiche.Rubrica(0) With {.tipologia = "Telefono"},
                    .valore = telefono
                }
            }
        End If

        Return centro

    End Function

    Public Function creaExcel(ByRef imprese As List(Of Impresa), objParametri_Server As AgronicaCoreParametri) As String
        Dim config As New AgronicaCoreVarieDAL.Configurazione_Siti_R
        Dim path As String = config.Leggi_Valore(0, "PathFileTemporanei", "", "", objParametri_Server)
        If path = "" Then
            Throw New Exception("Configurazione file temporanei non trovata. impossibile proseguire")
        End If

        'Chiusura Path
        If Right(path, 1) <> "\" Then
            path &= "\"
        End If

        Dim sheetName = "Imprese"
        Dim workbook = New XLWorkbook()
        workbook.Worksheets.Add(sheetName)
        Dim worksheet = workbook.Worksheet(sheetName)

        worksheet.Cell(1, 1).SetValue("Partita Iva")
        worksheet.Cell(1, 1).Style.Font.Bold = True
        worksheet.Column(1).Width = 20

        worksheet.Cell(1, 2).SetValue("CUAA")
        worksheet.Cell(1, 2).Style.Font.Bold = True
        worksheet.Column(2).Width = 25

        worksheet.Cell(1, 3).SetValue("Ragione sociale")
        worksheet.Cell(1, 3).Style.Font.Bold = True
        worksheet.Column(3).Width = 100

        Dim row = 2
        For Each impresa In imprese
            worksheet.Cell(row, 1).SetValue(impresa.partitaIva)
            worksheet.Cell(row, 2).SetValue(impresa.CUAA)
            worksheet.Cell(row, 3).SetValue(impresa.ragioneSociale)
            row += 1
        Next

        Dim pathFileXlsx As String
        Dim nomeReport = "imprese__" & Format(DateTime.Now, "yyyy-MM-dd").Replace(" ", "") & "_" & Format(DateTime.Now, "HHmm ssffff").Replace(" ", "")
        pathFileXlsx = path & nomeReport & ".xlsx"
        workbook.SaveAs(pathFileXlsx)

        Return pathFileXlsx
    End Function

    Public Function Geolocate(objParams As AgronicaCoreParametri, piva As String) As LatLng

        Dim objImpresexIndirizzi As New AgronicaCoreAnagrafeDAL.ImpresexIndirizzi_R
        Dim addresses As DataTable = objImpresexIndirizzi.Leggi(piva, 0, 0, enumSelezioneVariabile.Selezione_JoinCompleta, "", "", objParams)

        Dim confSitiDAL As New AgronicaCoreVarieDAL.Configurazione_Siti_R
        Dim url = confSitiDAL.Leggi_Valore(0, "Google_GeocodingBaseUrl", "", "", objParams)

        Dim http_Request = New AgronicaCoreUtility.Http
        Dim hdr = New WebHeaderCollection()

        url &= $"&address={Me.ExtractAddresses(addresses).First}"

        Dim resp = http_Request.chiamaWS_RestShapr(
            "",
            "",
            url,
            "application/json",
            RestSharp.Method.GET,
            "application/json",
            ""
            )

        Dim geolocation = JsonConvert.DeserializeObject(Of GoogleGeolocationResponse)(resp.Content)
        Dim objImpreseCodiciWrite As New AgronicaCoreAnagrafeDAL.Imprese_Codici_Write

        If geolocation.status.ToUpper() = "OK" Then

            If String.IsNullOrEmpty(geolocation.results.Single.city()) Then
                Err.Raise(
                    vbObjectError + GiasError.googleGeolocationNoDataFound,
                    "AgronicaCoreAnagrafeBIZ.Impresa_W.Geolocate()",
                    My.Resources.AgronicaCoreAnagrafeBIZ.googleGeolocationWrongLocationError
                    )
            End If

            Dim latOK = objImpreseCodiciWrite.aggiorna(
                PIVA:=piva,
                id_cod:=enum_CodiciAnagrafe.Latitudine,
                val_cod:=geolocation.results.Single.location.lat.ToString,
                datainizio:=AGRODATAINIZIO,
                datafine:=AGRODATAFINE,
                objParametriServer:=objParams
                )

            Dim lngOK = objImpreseCodiciWrite.aggiorna(
                PIVA:=piva,
                id_cod:=enum_CodiciAnagrafe.Longitudine,
                val_cod:=geolocation.results.Single.location.lng.ToString,
                datainizio:=AGRODATAINIZIO,
                datafine:=AGRODATAFINE,
                objParametriServer:=objParams
                )

            If latOK AndAlso lngOK Then
                Return geolocation.results.Single.location()
            Else
                Throw New GiasException("Errore durante l'aggiornamento delle coordinate dell'azienda")
            End If
        Else
            Err.Raise(
                vbObjectError + GiasError.googleGeolocationNoDataFound,
                "AgronicaCoreAnagrafeBIZ.Impresa_W.Geolocate()",
                My.Resources.AgronicaCoreAnagrafeBIZ.googleGeolocationNoDataFoundError
                )
        End If
    End Function

    Private Function ExtractAddresses(addresses As DataTable) As List(Of String)
        Dim result As New List(Of String)
        For Each a In addresses.Rows
            Dim address = a.Item("ind_des")
            Dim fraction = a.Item("frz_des")
            Dim postalCode = a.Item("CAP")
            Dim country = a.Item("stato")
            Dim city = a.Item("com_des")
            Dim province = a.Item("pro_des")

            result.Add($"{address},{fraction},{postalCode},{country}{If(country = "IT", $",{city},{province}", "")}")
        Next
        Return result
    End Function
End Class
