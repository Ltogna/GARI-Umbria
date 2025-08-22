Imports System.Xml
Imports System.Text
Imports AgronicaCoreDataProvider.TipiEnumerativi
Imports AgronicaCoreDataProvider.CostantiPersonalizzate
Imports AgronicaCoreDataProvider.AgronicaCoreParametri
Imports AgronicaCoreDataProvider
Imports AgronicaCoreModelsSTD
Imports AgronicaCoreMapper
Imports AgronicaCoreAnagrafeDAL
Imports AgronicaCoreModelsSTD.anagrafiche
Imports AgronicaCoreEntityFramework
Imports System.Web.UI.WebControls
Imports AgronicaCoreUtility

Public Class import_ParmaFrance
	Dim objParametriServer As AgronicaCoreParametri
	Dim objParametriUtenti As AgronicaCoreParametri
	Dim objStalla_R As New AgronicaCoreAnagrafeDAL.Stalla_R
	Dim objStalla_Raggruppamenti_R As New AgronicaCoreAnagrafeDAL.Stalla_Raggruppamenti_R
	Dim obj_AttivitaZooToAgenda_w As New AttivitaZootecnicaToAgenda
	Dim GiasContext As Gias_DeveloperServer_Entities

	Public Sub New(objParametriServer As AgronicaCoreParametri, objParametriUtenti As AgronicaCoreParametri)
		Me.objParametriServer = objParametriServer
		Me.objParametriUtenti = objParametriUtenti

		Dim gefutils As New Gias_EF_Utility
		Dim EFConnString As String = gefutils.GetEntityConnectionString(objParametriServer.StringaConnessione)
		'Dim scope As New TransactionScope()
		GiasContext = New Gias_DeveloperServer_Entities(EFConnString)

	End Sub
	Dim objLog As New AgronicaCoreDataProvider.LogProvider

	''' <summary>
	''' Importazione capi ParmaFrance
	''' </summary>
	''' <param name="PivaSelezionata"></param>
	''' <param name="StringaConnessione"></param>
	''' <param name="Utente_Username"></param>
	''' <param name="Utente_Password"></param>
	''' <param name="ProgressivoGIAS"></param>
	''' <param name="CodiceChiaveCliente"></param>
	''' <param name="LogDirectory"></param>
	''' <param name="LogFileName"></param>
	''' <param name="Messaggio"></param>
	''' <returns></returns>
	Public Function importaCapi(ByVal PivaSelezionata As String,
								ByVal StringaConnessione As String,
								ByVal Utente_Username As String,
								ByVal Utente_Password As String,
								ByVal ProgressivoGIAS As Integer,
								ByVal CodiceChiaveCliente As Integer,
								ByVal LogDirectory As String,
								ByVal LogFileName As String,
								ByRef Messaggio As String) As Boolean
		Dim res As Boolean = False

		Dim customLOGParams As New CustomLOGParams With {
			.LogDescrizioneUtente = objParametriServer.LogDescrizioneUtente,
			.LogDirectory = LogDirectory,
			.LogFileName = LogFileName
		}

		Try
			objLog.Scrivi_LOG(objParametriServer,
							  Reflection.MethodBase.GetCurrentMethod().Name,
							  "Inizio importazione",
							  CustomLOGParams:=customLOGParams)

			Dim dtCapiTotali As New DataTable
			Dim MessaggioErrore As String = ""
			CreaDT(StringaConnessione, MessaggioErrore, dtCapiTotali)

			Dim listaStalle As New List(Of String)
			Dim listaIngressi As New List(Of DateTime)
			If MessaggioErrore <> "" Then
				Throw New Exception(Messaggio)
			End If
			For Each row As DataRow In dtCapiTotali.Rows
				'controllo su date e sesso per verificare eventuali errori
				If Not IsDate(row("F4")) Then
					Throw New Exception("Valore errato per la data di nascita del capo n. " & row("F1") & ": " & row("F4"))
				End If
				If Not IsDate(row("F9")) Then
					Throw New Exception("Valore errato per la data di partenza del capo n. " & row("F1") & ": " & row("F9"))
				End If
				If Not IsDate(row("F10")) Then
					Throw New Exception("Valore errato per la data di arrivo del capo n. " & row("F1") & ": " & row("F10"))
				End If
				If row("F3") <> "M" And row("F3") <> "F" Then
					Throw New Exception("Valore errato per il sesso del capo n. " & row("F1") & ": " & row("F3"))
				End If

				If Not listaStalle.Contains(row("F19")) Then
					listaStalle.Add(row("F19"))
				End If
			Next

			For Each staDes In listaStalle
				Try
					Messaggio &= " <h5>Stalla " & staDes & " </h5>"
					Dim dtCapiStalla As DataTable = dtCapiTotali.Select(" F19 = '" & staDes & "' ").CopyToDataTable

					'lettura capi in DB
					Dim raggruppamentoCod As Integer = 0
					Dim saCod As Integer = 0
					Dim staNum As Integer = 0
					Dim piva As String = ""

					GetDatiPerQuery(PivaSelezionata, dtCapiStalla, piva,
									saCod, staNum, raggruppamentoCod)

					'Controllo visibilità utente su stalla/azienda
					Dim visibilita As Boolean = ControlloVisibilitaAziende(piva, saCod, staNum)

					If Not visibilita Then
						Messaggio &= "Non è possibile sincronizzare la stalla perché non si dispone dei permessi di visibilità necessari"

					Else
						Dim dtCapiNew As DataTable = dtCapiStalla.Clone
						Dim dtCapiUpdate As DataTable = dtCapiStalla.Clone
						'Messaggio &= "Stalla " & st & ": " & vbCrLf

						For Each row In dtCapiStalla.Rows
							If IsDBNull(row("F1")) Then
								Continue For
							End If

							If IsNumeric(CStr(row("F1")).Substring(0, 2)) Then
								row("Matricola") = "FR" & CStr(row("F1")).Trim()
							Else
								row("Matricola") = row("F1")
							End If
							row("Matricola") = CStr(row("Matricola")).Trim()
							row("Stalla_Nascita") = row("F2")

							'If row("F14").ToString.Split(".")(0) = "FR" Then
							'    Dim temp As String = row("F14")
							'    temp = temp.Replace("FR.", "INTRA.FR.")
							'    row("F14") = temp
							'End If

							Dim mat As String = row("Matricola")
							Dim capo = (From z In GiasContext.Zoo_Animali Where z.Matricola = mat And z.PIVA = PivaSelezionata Select z).FirstOrDefault

							If IsNothing(capo) Then
								Dim rowToAdd = dtCapiNew.Rows.Add() 'test, da spostare nell'if
								rowToAdd.ItemArray = row.ItemArray
							Else
								Dim rowToUpdate = dtCapiUpdate.Rows.Add()
								rowToUpdate.ItemArray = row.ItemArray
							End If
						Next

						If dtCapiNew.Rows.Count > 0 Then
							Messaggio &= " <p>Capi Inseriti:</p>"
							CaricaCapi(dtCapiNew, saCod, staNum, raggruppamentoCod,
								   piva, objLog, LogDirectory, LogFileName, Messaggio,
								   customLOGParams)
						End If

						If dtCapiUpdate.Rows.Count > 0 Then
							Messaggio &= " <p>Capi Aggiornati:</p>"
							AggiornaCapi(dtCapiUpdate, saCod, staNum, raggruppamentoCod,
									 piva, objLog, LogDirectory, LogFileName, Messaggio,
									 GiasContext, customLOGParams)
						End If

					End If

				Catch ex As Exception
					Messaggio &= "<p>" & ex.Message & "</p>"
				End Try
			Next

			res = True

		Catch ex As Exception
			objLog.Scrivi_LOG(objParametriServer,
							  Reflection.MethodBase.GetCurrentMethod().Name,
							  "Errore scrittura dati: " & ex.Message,
							  CustomLOGParams:=customLOGParams)
			Messaggio = ex.Message
			res = False

		Finally
			GiasContext.Dispose()

		End Try

		objLog.Scrivi_LOG(objParametriServer,
						  Reflection.MethodBase.GetCurrentMethod().Name,
						  "Fine importazione",
						  CustomLOGParams:=customLOGParams)

		Return res

	End Function

	''' <summary>
	''' Controllo della visibilità utente sulle stalle delle diverse aziende
	''' </summary>
	''' <param name="piva"></param>
	''' <param name="saCod"></param>
	''' <param name="staNum"></param>
	''' <returns></returns>
	Private Function ControlloVisibilitaAziende(ByVal piva As String, ByVal saCod As Integer, ByVal staNum As Integer) As Boolean
		Dim visibilita As Boolean = False

		Dim classFiltrone As New AgronicaCoreUtility.Filtrone
		Dim classJoin As New JoinFiltrone
		classJoin.bCentriAziendali = True
		classJoin.bGerarchiaImprese = True
		Dim dtImpresexCentri As DataTable = classFiltrone.CreaDTFiltrone(objParametriServer, "",
																		 enum_TipoSelect_FiltroneSuperNova.CentriAziendali,
																		 "", classJoin)

		If Not IsNothing(dtImpresexCentri) AndAlso dtImpresexCentri.Rows.Count > 0 Then
			Dim listaImprese = dtImpresexCentri.ToExpandoObject.ToList
			Dim listaAziende As List(Of String) = listaImprese.Select(Of String)(Function(row) row("PIVA")).ToList

			'visibilità su Azienda
			If listaAziende.Contains(piva) Then
				Dim listaCentri As List(Of Integer) =
					listaImprese.Where(Function(row) row("PIVA") = piva).Select(Of Integer)(Function(row) row("sa_cod")).ToList

				'visibilità su AziendaxCentro
				If listaCentri.Contains(saCod) Then
					visibilita = True
				End If
			End If
		End If

		Return visibilita

	End Function

	''' <summary>
	''' Ricava il Raggruppamento_Cod
	''' </summary>
	''' <param name="PivaSelezionata"></param>
	''' <param name="DTCapi"></param>
	''' <param name="piva"></param>
	''' <param name="saCod"></param>
	''' <param name="staNum"></param>
	''' <param name="raggruppamentoCod"></param>
	Private Sub GetDatiPerQuery(ByVal PivaSelezionata As String,
								ByVal dtCapi As DataTable,
								ByRef piva As String,
								ByRef saCod As Integer,
								ByRef staNum As Integer,
								ByRef raggruppamentoCod As Integer)
		Dim row As DataRow = dtCapi.Select("").FirstOrDefault()

		'Estrazione codice azienda BDN dalla datatable dei capi che stiamo importando
		Dim BDN_Codice_Azienda As String = row("F19")

		Dim dtStalla As DataTable = objStalla_R.Leggi("", 0, 0,
													  enumSelezioneVariabile.Selezione_TabellaCompleta,
													  "Stalla.BDN_Codice_Azienda = '" & BDN_Codice_Azienda & "' ",
													  "", objParametriServer)

		If IsNothing(dtStalla) OrElse dtStalla.Rows.Count = 0 Then
			Throw New GiasException("Stalla " & BDN_Codice_Azienda & " non presente in GIAS")
		ElseIf dtStalla.Rows.Count > 1 Then
			Dim drStallaF = dtStalla.Select(" Piva = '" & PivaSelezionata & "' ")
			If drStallaF.Length = 0 Then
				Throw New GiasException("Al codice Azienda " & BDN_Codice_Azienda & " corrispondono più stalle su GIAS")
			ElseIf drStallaF.Length > 1 Then
				Throw New GiasException("Al codice Azienda " & BDN_Codice_Azienda & " corrispondono più stalle su GIAS")
			ElseIf drStallaF.Length = 1 Then
				dtStalla = drStallaF.CopyToDataTable
			End If
		End If

		'Estrazione Dati da DTStalla
		Dim drStalla As DataRow = dtStalla.Select("").FirstOrDefault()
		piva = drStalla("PIVA")
		saCod = drStalla("sa_cod")
		staNum = drStalla("STA_NUM")

		Dim dtRaggruppamenti As DataTable
		dtRaggruppamenti = objStalla_Raggruppamenti_R.Leggi_x_anagrafica(objParametriServer.PivaSuperUser, piva, saCod,
																		 staNum, raggruppamentoCod,
																		 " Stalla_Raggruppamenti.flag_bdn = 1 ",
																		 "", objParametriServer)

		If IsNothing(dtRaggruppamenti) OrElse dtRaggruppamenti.Rows.Count = 0 Then
			Throw New GiasException("Stalla " & BDN_Codice_Azienda & " senza un raggruppamento correttamente configurato per l'importazione")
		End If

		raggruppamentoCod = dtRaggruppamenti.Select("").FirstOrDefault()("raggruppamento_cod")

	End Sub

	''' <summary>
	''' Crea DataTable importando i dati dal file excel
	''' </summary>
	''' <param name="StringaConnessione"></param>
	''' <param name="Messaggio"></param>
	''' <param name="DTCapi"></param>
	Private Sub CreaDT(ByVal StringaConnessione As String,
					   ByRef Messaggio As String,
					   ByRef DTCapi As DataTable)
		Try
			Dim ds As New DataSet
			Dim MyConnection As New System.Data.OleDb.OleDbConnection(StringaConnessione)

			Dim counter As Integer = 0

			MyConnection.Open()

			Dim dtSheet = MyConnection.GetSchema("Tables")
			Dim firstSheet = dtSheet.Rows(0)("TABLE_NAME").ToString()
			Dim da As New System.Data.OleDb.OleDbDataAdapter("SELECT * FROM [" + firstSheet + "]", MyConnection)

			da.Fill(ds, "fileXls")

			'ds.Tables(0).AcceptChanges()
			'For Each row In ds.Tables(0).Rows
			'    ds.Tables(0).Rows(counter).Delete()
			'    counter += 1
			'    If counter >= 1 Then
			'        Exit For
			'    End If
			'Next
			ds.Tables(0).AcceptChanges()
			MyConnection.Close()
			DTCapi = ds.Tables(0)

			'elimina le righe vuote dell'import da excel a datatable
			DTCapi = DTCapi.Rows.Cast(Of DataRow).Where(Function(row)
															Return Not row.ItemArray.All(Function(field)
																							 Return IsDBNull(field) Or
																							 String.Compare((field.ToString).Trim(), String.Empty) = 0
																						 End Function)
														End Function).CopyToDataTable()

			DTCapi.Columns(0).ColumnName = "F1"
			DTCapi.Columns(1).ColumnName = "F2"
			DTCapi.Columns(2).ColumnName = "F3"
			DTCapi.Columns(3).ColumnName = "F4"
			DTCapi.Columns(4).ColumnName = "F5"
			DTCapi.Columns(5).ColumnName = "F6"
			DTCapi.Columns(6).ColumnName = "F7"
			DTCapi.Columns(7).ColumnName = "F8"
			DTCapi.Columns(8).ColumnName = "F9"
			DTCapi.Columns(9).ColumnName = "F10"
			DTCapi.Columns(10).ColumnName = "F11"
			DTCapi.Columns(11).ColumnName = "F12"
			DTCapi.Columns(12).ColumnName = "F13"
			DTCapi.Columns(13).ColumnName = "F14"
			DTCapi.Columns(14).ColumnName = "F15"
			DTCapi.Columns(15).ColumnName = "F16"
			DTCapi.Columns(16).ColumnName = "F17"
			DTCapi.Columns(17).ColumnName = "F18"
			DTCapi.Columns(18).ColumnName = "F19"

			If DTCapi.Columns.Count > 19 Then
				DTCapi.Columns(19).ColumnName = "F20"
			Else
				Throw New Exception("Colonna matricola madre mancante")
			End If

			If DTCapi.Columns.Count > 20 Then
				DTCapi.Columns(20).ColumnName = "Stalla_Svezzamento"
			Else
				DTCapi.Columns.Add(New DataColumn("Stalla_Svezzamento", GetType(String)))
				'Throw New Exception("Colonna stalla svezzamento mancante")
			End If

			DTCapi.Columns.Add(New DataColumn("Matricola", GetType(String)))
			DTCapi.Columns.Add(New DataColumn("Stalla_Nascita", GetType(String)))

		Catch ex As Exception
			Messaggio = "Errore all'apertura del file excel: " & ex.Message
		End Try

	End Sub

	''' <summary>
	''' Caricamento dei capi su DB
	''' </summary>
	''' <param name="dtCapiNew"></param>
	''' <param name="saCod"></param>
	''' <param name="staNum"></param>
	''' <param name="raggruppamentoCod"></param>
	''' <param name="piva"></param>
	''' <param name="objLog"></param>
	''' <param name="LogDirectory"></param>
	''' <param name="LogFileName"></param>
	''' <param name="Messaggio"></param>
	Private Sub CaricaCapi(ByVal dtCapiNew As DataTable,
						   ByRef saCod As Integer,
						   ByRef staNum As String,
						   ByRef raggruppamentoCod As Integer,
						   ByVal piva As String,
						   ByVal objLog As AgronicaCoreDataProvider.LogProvider,
						   ByVal LogDirectory As String,
						   ByVal LogFileName As String,
						   ByRef Messaggio As String,
						   ByVal customLOGParams As CustomLOGParams)
		Dim LSTCapiSenzaDuplicati = dtCapiNew.ToExpandoObject.ToList
		Dim listaIngressi = From cp In LSTCapiSenzaDuplicati
							Select cp.Item("F10") Distinct.ToList()

		Dim objAttivita As New attivita.Attivita
		Dim centroAzienda_Cod As Integer = 0 ' Controllare se effettivamente 0 è corretto
		objAttivita.fine = AGRODATAFINE
		objAttivita.job = New attivita.Zootecnia(LAVCOD_ACQUISTO_ANIMALI, "")
		objAttivita.centroAziendale = New anagrafiche.CentroAziendale With {
			.primaryKey = New anagrafiche.CentroAziendale.PK(centroAzienda_Cod, piva)
		}

		Dim listaCapiAnimali As New List(Of attivita.centri_di_costo.CentroDiCosto)

		For Each ingresso In listaIngressi
			Try
				'filtra i capi per data ingresso
				Dim LSTCapiDaCaricareDCFiltrati = LSTCapiSenzaDuplicati.Where(Function(x)
																				  Return x.Item("F10") = ingresso
																			  End Function).ToList
				If LSTCapiDaCaricareDCFiltrati.Count = 0 Then
					Continue For
				End If

				objLog.Scrivi_LOG(objParametriServer,
								  System.Reflection.MethodBase.GetCurrentMethod().Name,
								  "Inizio carico " & LSTCapiDaCaricareDCFiltrati.Count & " capi in data " & CDate(ingresso).ToShortDateString(),
								  CustomLOGParams:=customLOGParams)

				For Each capo In LSTCapiDaCaricareDCFiltrati
					Dim codiceCapo As String = capo("Matricola")

					Try
						Dim codAziendaNascita As String = capo("Stalla_Nascita")
						Dim sesso As String = capo("F3")
						Dim dataNascita As String = capo("F4")
						Dim codRazza As Integer = RicavaRazzaCapoAnimale(capo("F5"))
						Dim codRazzaPadre As Integer = RicavaRazzaCapoAnimale(capo("F6"))
						Dim codRazzaMadre As Integer = RicavaRazzaCapoAnimale(capo("F7"))
						Dim numCertificato As String = capo("F14")
						Dim genere As Integer = 1 'Placeholder
						Dim specie As Integer = 1 'Placeholder

						Dim matricolaMadre As String = ""
						If Not IsDBNull(capo("F20")) Then
							Dim addFRPrefix As String = "FR"
							If IsNumeric(CStr(capo("F20")).Substring(0, 2)) Then
								matricolaMadre = addFRPrefix & capo("F20")
							Else
								matricolaMadre = capo("F20")
							End If
						End If

						'ricava Cod_Contatto della stalla svezzamento
						Dim stallaSvezzamento As String = ""
						Dim codContatto_StallaSvezz As String = ""
						If Not IsDBNull(capo("Stalla_Svezzamento")) Then
							stallaSvezzamento = capo("Stalla_Svezzamento")
							If stallaSvezzamento <> "" Then
								Dim contatto_StallaSvezz = GiasContext.Risorse_Umane.Where(Function(row) row.Settore_Des = stallaSvezzamento AndAlso
																						   (row.Cod_Rapporto = COD_ALLEVATORE Or row.Cod_Rapporto = COD_FORNITORE)).FirstOrDefault
								If Not IsNothing(contatto_StallaSvezz) Then
									codContatto_StallaSvezz = contatto_StallaSvezz.Cod_Contatto
								Else
									Throw New Exception("La stalla di svezzamento " & stallaSvezzamento & " inserita per il capo " & codiceCapo & " non è presente su GIAS.")
								End If
							End If
						End If

						Dim objAnimale = CreaCapoAnimale(piva, saCod, codiceCapo,
														 dataNascita, sesso, codRazza,
														 ingresso, numCertificato,
														 raggruppamentoCod, genere,
														 specie, codRazzaMadre,
														 codRazzaPadre, codAziendaNascita,
														 matricolaMadre, codContatto_StallaSvezz)
						listaCapiAnimali.Add(objAnimale)

					Catch ex As Exception
						Messaggio &= "<p> Errore matricola " & codiceCapo & ": " & ex.Message & "</p>"
					End Try
				Next

				objAttivita.inizio = CDate(ingresso)
				objAttivita.centriDiCosto = listaCapiAnimali

				If listaCapiAnimali.Count > 0 Then
					Dim Id_Agenda As Integer = obj_AttivitaZooToAgenda_w.ScriviAttivitaZootecnicaToAgenda(objAttivita,
																									  objParametriServer)

					BloccaOperazione(piva, Id_Agenda, True)

				End If


				Dim LSTMatricole = (From a As attivita.centri_di_costo.CapoAnimaleCDC In listaCapiAnimali
									Select a.capoAnimale.matricola).ToList

				Messaggio &= "<ul><li>" & String.Join("</li><li>", LSTMatricole) & "</li></ul>"

				objLog.Scrivi_LOG(objParametriServer,
								  System.Reflection.MethodBase.GetCurrentMethod().Name,
								  "Fine carico " & LSTCapiDaCaricareDCFiltrati.Count & " capi in data " &
								  CDate(ingresso).ToShortDateString() & ": " &
								  String.Join(",", LSTMatricole),
								  CustomLOGParams:=customLOGParams)

			Catch ex As Exception
				Dim msgEx = ex.Message
				If ex.InnerException IsNot Nothing Then
					msgEx &= " Inner Exception:" & ex.InnerException.Message
				End If
				Messaggio &= ex.Message & vbCrLf
				objLog.Scrivi_LOG(objParametriServer,
								  System.Reflection.MethodBase.GetCurrentMethod().Name,
								  "Errore carico in data " & CDate(ingresso).ToShortDateString() & vbCrLf &
								  " Errore:" & msgEx,
								  CustomLOGParams:=customLOGParams)
			End Try

		Next

	End Sub

	Private Function BloccaOperazione(Piva As String, Id_Agenda As Integer, SettaTipoAccettazione1 As Boolean)
		Dim agendaDal As New AgronicaCoreContabDAL.Agenda_W
		'agendaDal.Agenda_Blocca(Piva, 0, Id_Agenda, "", objParametriServer)

		If SettaTipoAccettazione1 Then
			agendaDal.ModificaPuntuale(Piva, 0, Id_Agenda, objParametriServer, Tipo_Accettazione:=1)
		End If

	End Function

	''' <summary>
	''' Aggiornamento capi su DB
	''' </summary>
	''' <param name="DTCapiDaAggiornare"></param>
	''' <param name="sa_cod"></param>
	''' <param name="sta_num"></param>
	''' <param name="Raggruppamento_Cod"></param>
	''' <param name="piva"></param>
	''' <param name="objLog"></param>
	''' <param name="LogDirectory"></param>
	''' <param name="LogFileName"></param>
	''' <param name="Messaggio"></param>
	Private Sub AggiornaCapi(ByVal DTCapiDaAggiornare As DataTable,
							 ByRef sa_cod As Integer,
							 ByRef sta_num As String,
							 ByRef Raggruppamento_Cod As Integer,
							 ByVal piva As String,
							 ByVal objLog As AgronicaCoreDataProvider.LogProvider,
							 ByVal LogDirectory As String,
							 ByVal LogFileName As String,
							 ByRef Messaggio As String,
							 ByRef GiasContext As Gias_DeveloperServer_Entities,
							 ByVal customLOGParams As CustomLOGParams)

		Try

			Dim listMatricole As New List(Of String)

			For Each capoUpd As DataRow In DTCapiDaAggiornare.Rows
				Dim matricolaCapo As String = capoUpd("Matricola")
				listMatricole.Add(matricolaCapo)
				'If Not IsDBNull(capoUpd("F20")) AndAlso CStr(capoUpd("F20")) <> "" Then
				'	Continue For
				'End If

				Dim objZoo As New AgronicaCoreAnagrafeDAL.Zoo_Animali
				Dim cod_progetto As Integer = objZoo.CodProgetto_Da_Matricola_E_Stalla(piva, sa_cod, sta_num, matricolaCapo, objParametriServer)

				Dim capo = (From z In GiasContext.Zoo_Animali
							Where z.Cod_Progetto = cod_progetto
							Select z).FirstOrDefault

				'controlla il prefisso della matricola del capo
				If Not IsNothing(capo) Then
					If IsNumeric(CStr(capoUpd("F20")).Substring(0, 2)) Then
						capo.MAT_MADRE = "FR" & capoUpd("F20")
					Else
						capo.MAT_MADRE = capoUpd("F20")
					End If


					'controlla il Cod_Contatto della stalla svezzamento
					Dim stallaSvezzamento As String = ""
					If Not IsDBNull(capoUpd("Stalla_Svezzamento")) AndAlso CStr(capoUpd("Stalla_Svezzamento")) <> "" Then
						stallaSvezzamento = CStr(capoUpd("Stalla_Svezzamento"))

						Dim codContatto_StallaSvezz As String = ""
						Dim contatto_StallaSvezz = GiasContext.Risorse_Umane.Where(Function(row) row.Settore_Des = stallaSvezzamento AndAlso
																							   (row.Cod_Rapporto = COD_ALLEVATORE Or row.Cod_Rapporto = COD_FORNITORE)).FirstOrDefault
						If Not IsNothing(contatto_StallaSvezz) Then
							codContatto_StallaSvezz = contatto_StallaSvezz.Cod_Contatto
						Else
							Throw New Exception("La stalla di svezzamento " & stallaSvezzamento & " inserita per il capo " & matricolaCapo & " non è presente su GIAS.")
						End If
						capo.Stalla_Svezzamento = codContatto_StallaSvezz
					End If

					'Dim sesso As String = capoUpd("F3")
					'Dim dataNascita As String = capoUpd("F4")
					'Dim codRazza As Integer = RicavaRazzaCapoAnimale(capoUpd("F5"))
					'Dim codRazzaPadre As Integer = RicavaRazzaCapoAnimale(capoUpd("F6"))
					'Dim codRazzaMadre As Integer = RicavaRazzaCapoAnimale(capoUpd("F7"))
					Dim numCertificato As String = capoUpd("F14")
					'Dim genere As Integer = 1 'Placeholder
					'Dim specie As Integer = 1 'Placeholder


					capo.Certificato = numCertificato


				End If

				objLog.Scrivi_LOG(objParametriServer,
								  System.Reflection.MethodBase.GetCurrentMethod().Name,
								  "Fine AggiornaCapi " & DTCapiDaAggiornare.Rows.Count,
								  CustomLOGParams:=customLOGParams)

			Next

			Messaggio &= "<ul><li>" & String.Join("</li><li>", listMatricole) & "</li></ul>"

			GiasContext.SaveChanges()
			GiasContext.Core.AcceptAllChanges()

		Catch ex As Exception
			Dim msgEx = ex.Message
			If Not IsNothing(ex.InnerException) Then
				msgEx &= " Inner Exception:" & ex.InnerException.Message
			End If
			Messaggio &= ex.Message & vbCrLf
			objLog.Scrivi_LOG(objParametriServer,
							  System.Reflection.MethodBase.GetCurrentMethod().Name,
							  "Errore carico in AggiornaCapi " & vbCrLf &
							  " Errore:" & msgEx,
							  CustomLOGParams:=customLOGParams)
		End Try

	End Sub

	'Creazione di capo animale da caricare sul DB
	Private Function CreaCapoAnimale(ByVal piva As String,
									ByVal sa_cod As Integer,
									ByVal matricolaCapo As String,
									ByVal dataNascita As String,
									ByVal sesso As String,
									ByVal codRazza As Integer,
									ByVal ingresso As String,
									ByVal numCertificato As String,
									ByVal Raggruppamento_Cod As Integer,
									ByVal genere As Integer,
									ByVal specie As Integer,
									ByVal codRazzaMadre As Integer,
									ByVal codRazzaPadre As Integer,
									ByVal codAziendaNascita As String,
									ByVal MatricolaMadre As String,
									ByVal stallaSvezzamento As String) As attivita.centri_di_costo.CapoAnimaleCDC
		' Togliere logdirectory e logfilename

		Dim CapoCDC As New attivita.centri_di_costo.CapoAnimaleCDC()

		Dim objImprese As New Imprese_Read
		Dim DTimprese As DataTable
		DTimprese = objImprese.Leggi_x_anagrafica(piva,
												"",
												"",
												Me.objParametriServer)

		Dim rowImprese As DataRow = DTimprese.Select("").FirstOrDefault

		Dim codDetentore As String = rowImprese.Item("Codice_Cuaa")

		Dim objCapo As New CapoAnimale With {
			.partitaIva = piva,
			.matricola = matricolaCapo,
			.sesso = sesso,
			.codice = 0,
			.nome = "",
			.collare = "",
			.lottoFornitore = "",
			.dataNascita = dataNascita,
			.numCertificato = numCertificato,
			.codiceFiscaleDetentore = codDetentore,
			.codiceFiscaleProprietario = "",
			.codiceAziendaNascita = codAziendaNascita,
			.stallaSvezzamento = stallaSvezzamento,
			.indirizzoProd = New metaschema.IndirizzoProduttivo(0),
			.categoria = New metaschema.Categoria(0),
			.tipologia = New metaschema.TipologiaCapoAnimale(1),
			.metodoProduzione = New metaschema.MetodoProduzione(1),
			.idCapo_BDN = 0,
			.genere = New metaschema.Genere(genere),
			.specie = New metaschema.utilizzi.Specie(specie),
			.razza = New metaschema.Razza(codRazza),
			.esercizi = New List(Of AgronicaCoreModelsSTD.anagrafiche.EsercizioCapoAnimale),
			.statiAccrescimento = New List(Of AgronicaCoreModelsSTD.anagrafiche.StatoAccrescimento),
			.fornitore = New AgronicaCoreModelsSTD.anagrafiche.Contatto With {
						.primaryKey = New AgronicaCoreModelsSTD.anagrafiche.Contatto.PK With {
							.partitaIva = ""
						}
					},
			.validita = New anagrafiche.IntervalloTemporale With {
				.inizio = ingresso,
				.fine = AGRODATAFINE
				},
			.validitaConversione = New AgronicaCoreModelsSTD.anagrafiche.IntervalloTemporale With {
					.inizio = AGRODATAINIZIO,
					.fine = AGRODATAFINE
				},
			.madre = New CapoAnimale With {
				.razza = New metaschema.Razza(codRazzaMadre),
				.codice = 0,
				.matricola = MatricolaMadre
				},
			.padre = New CapoAnimale With {
				.razza = New metaschema.Razza(codRazzaPadre),
				.codice = 0,
				.matricola = ""
				},
			.ingresso_modello4_data_prenotazione = ingresso
		}
		CreaDistinte(objCapo)

		CreaStatiAccrescimento(objCapo)

		CapoCDC.codice = New attivita.centri_di_costo.CentroDiCosto.CodeType(sa_cod)
		CapoCDC.capoAnimale = objCapo
		CapoCDC.sottogruppoStalla_ingresso = New SottogruppoStalla
		CapoCDC.sottogruppoStalla_ingresso.codice = Raggruppamento_Cod

		CapoCDC.sottogruppoStalla_uscita = New SottogruppoStalla
		CapoCDC.sottogruppoStalla_uscita.codice = Raggruppamento_Cod

		If CapoCDC.capoAnimale.validita.inizio <> ingresso Then
			Dim a = 0
		End If

		Return CapoCDC
	End Function

	Private Function RicavaRazzaCapoAnimale(ByVal codRazza As Integer)
		Dim DTCodificaRazzeAnimaliFRtoBDN As DataTable

		'---------------Nuova-Query-StringBuilder---------------
		Dim DataProvider As New DataProvider
		Dim Stb As New System.Text.StringBuilder
		Dim NomeRoutine As String = "estrazioneCodRazzaBDNDaCodRazzaFR" ' Da nominare

		Stb.Length = 0
		Stb.AppendLine("SELECT *")
		Stb.AppendLine("FROM Cac_Codifica_InfoAggiuntive ")
		Stb.AppendLine("WHERE InfoAgg_Cod = 9 AND Argomento_Cod = " & codRazza & "")

		DTCodificaRazzeAnimaliFRtoBDN = DataProvider.EseguiQuery_Lettura(objParametriServer, Stb.ToString, NomeRoutine)
		'---------------Fine-Query-StringBuilder---------------

		Dim rowCodificaRazzeAnimliFRtoBDN As DataRow = DTCodificaRazzeAnimaliFRtoBDN.Select("").FirstOrDefault

		If IsNothing(rowCodificaRazzeAnimliFRtoBDN) Then
			Throw New GiasException("Errore! Razza UE " & codRazza & " non mappata in GIAS.") 'INC non esiste nel db quindi ritorna nothinge e da errore
		End If

		Dim razzaBDN As String = rowCodificaRazzeAnimliFRtoBDN.Item("TestoAux_1")
		Dim codiceRazzaBDN As Integer = rowCodificaRazzeAnimliFRtoBDN.Item("CodiceAux_3")
		Dim obj_RazzeAnimali_R As New AgronicaCoreMetaSchemaDAL.Codifica_BDN_RazzeAnimali
		Dim DTCodificaRazzeAnimaliBDNtoAgronica As New DataTable

		DTCodificaRazzeAnimaliBDNtoAgronica = obj_RazzeAnimali_R.leggi(Me.objParametriServer,
															"",
															"",
															"",
															"",
															"",
															AgronicaCoreDataProvider.TipiEnumerativi.enum_Esportazioni_Sistema_Cod.BDN,
															razzaBDN) 'BDN enum da cambiare? Creare?

		If IsNothing(DTCodificaRazzeAnimaliBDNtoAgronica.Rows.Count = 0) Then
			Throw New GiasException("Errore! Razza BDN " & razzaBDN & " non mappata in GIAS.") 'INC non esiste nel db quindi ritorna nothinge e da errore
		End If
		Dim razzaAgronica = ""
		If DTCodificaRazzeAnimaliBDNtoAgronica.Rows.Count > 1 And codiceRazzaBDN > 0 Then
			Dim rowCodificaMultipla As DataRow = DTCodificaRazzeAnimaliBDNtoAgronica.Select(" RAZZA_ID = " & codiceRazzaBDN).FirstOrDefault
			If Not IsNothing(rowCodificaMultipla) Then
				razzaAgronica = CStr(rowCodificaMultipla.Item("RAZ_COD"))
			Else
				Throw New GiasException("Errore! Razza BDN " & razzaBDN & " non mappata in GIAS.") 'INC non esiste nel db quindi ritorna nothinge e da errore
			End If
		Else
			razzaAgronica = DTCodificaRazzeAnimaliBDNtoAgronica.Rows(0)("RAZ_COD")
		End If


		Return razzaAgronica
	End Function

	Private Sub CreaDistinte(ByRef objCapo As CapoAnimale)

		Dim objEsercizio As New AgronicaCoreModelsSTD.anagrafiche.EsercizioCapoAnimale
		objEsercizio.codice_capo_animale = objCapo.codice
		objEsercizio.validita = New IntervalloTemporale

		objEsercizio.validita.inizio = objCapo.validita.inizio
		objEsercizio.validita.fine = objCapo.validita.fine

		objEsercizio.progettoNome = ""

		objCapo.esercizi.Add(objEsercizio)

	End Sub

	Private Sub CreaStatiAccrescimento(ByRef objCapo As CapoAnimale)
		Dim Piva As String = objCapo.partitaIva
		Dim Gen_Cod As Integer = objCapo.genere.codice
		Dim Spe_Cod As Integer = objCapo.specie.codice
		Dim Tipo_Cod As Integer = objCapo.tipologia.codice
		Dim Zoo_Animali_Lista_Stati_Accrescimento As List(Of AgronicaCoreEntityFramework_POCO.Zoo_Animali_Lista_Stati_Accrescimento)
		Dim GiasContext As AgronicaCoreEntityFramework.Gias_DeveloperServer_Entities
		Dim gefutils As New Gias_EF_Utility
		Dim EFConnString As String = gefutils.GetEntityConnectionString(Me.objParametriServer.StringaConnessione)

		GiasContext = New Gias_DeveloperServer_Entities(EFConnString)

		Zoo_Animali_Lista_Stati_Accrescimento = (From a In GiasContext.Zoo_Animali_Lista_Stati_Accrescimento Select a).ToList()

		'Lettura degli stati di accrescimento da DB
		Dim StatiAccrescimento = From zan In Zoo_Animali_Lista_Stati_Accrescimento
								 Where (zan.PIVA = Piva Or zan.Sa_Cod = -1) And zan.GEN_COD = Gen_Cod And zan.SPE_COD = Spe_Cod And zan.TIPO_COD = Tipo_Cod
								 Select zan
								 Order By zan.Giorno_Da

		If IsNothing(StatiAccrescimento) OrElse StatiAccrescimento.Count = 0 Then
			Throw New GiasException("Errore! Non esistono stati di accrescimento per questo capo.")

		End If

		For Each sa In StatiAccrescimento
			Dim objStatoAccr As New StatoAccrescimento

			'-------------------------------------------------------------------------------------------------------
			'Aggiunge alla data di nascita i giorni di durata del periodo dello stato di accrescimento
			'-------------------------------------------------------------------------------------------------------

			objStatoAccr.validita = New IntervalloTemporale

			'VALIDITA_INIZIO
			If IsNothing(sa.Giorno_Da) Then
				objStatoAccr.validita.inizio = objCapo.dataNascita
			Else
				objStatoAccr.validita.inizio = objCapo.dataNascita.AddDays(sa.Giorno_Da)
			End If

			'VALIDITA_FINE
			If IsNothing(sa.Giorno_A) Then
				objStatoAccr.validita.fine = AGRODATAFINE
			Else
				objStatoAccr.validita.fine = objCapo.dataNascita.AddDays(sa.Giorno_A)
			End If

			'-------------------------------------------------------------------------------------------------------

			objStatoAccr.codice = sa.STATO_COD
			objStatoAccr.descrizione = sa.Stato_Des

			objCapo.statiAccrescimento.Add(objStatoAccr)
		Next

	End Sub

End Class