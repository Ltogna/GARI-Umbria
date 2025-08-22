Imports AgronicaCoreDataProvider.CostantiPersonalizzate
Imports AgronicaCoreDataProvider.TipiEnumerativi
Imports AgronicaCoreDataProvider.Sicurezza
Imports AgronicaCoreDataProvider.UtilityProvider
Imports AgronicaCoreUtility
Imports System.Xml
Imports AgronicaCoreXML.XML_Stampe
Imports AgronicaCoreModello.ParametriAgenda_Temp



Public Class GestioneMonitoraggioCE
    Inherits System.Web.UI.Page


    '----- Gestione Querystring
    Dim Qs_Piva, Qs_Rag_Soc, Qs_Modalita As String
    Dim Qs_Cod_RisUm, Qs_Filtro, Qs_PagRitorno As String

    Dim objParametri_Utenti As AgronicaCoreDataProvider.AgronicaCoreParametri
    Dim objParametri_Server As AgronicaCoreDataProvider.AgronicaCoreParametri

    Dim objParametriAgenda As ParametriAgenda

    '##########################################################################################################
    Private Sub GestioneCE_Init(sender As Object, e As System.EventArgs) Handles Me.Init
        AddHandler CType(Page.Master.FindControl("ImgBtn_AnnullaTutto"), ImageButton).Click, AddressOf Me.AnnullaTutto
    End Sub

    '##########################################################################################################
    Private Sub AnnullaTutto(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim objParametriAgenda As New ParametriAgenda
        objParametriAgenda.Piva = Qs_Piva

        Dim paginaOnLineRitorno As Integer = enum_PagineGiasOnline.MenuCartellaAziendale

        Response.Redirect(CType(Master, Agenda).TrovaRedirectCorretto(True, paginaOnLineRitorno, objParametriAgenda))

    End Sub

    '################################################################################################################
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        CType(Page.Master.FindControl("Lbl_Titolo"), Label).Text = "Filtro Monitoraggio Corpi Estranei"

        Response.Expires = 0

        '§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§

        Try

            '##############################################################
            '#####  Verifico Credenziali di Accesso  ######################
            '##############################################################

            'If Session("ASG_Utente_Username") = "" Then
            '    Response.Redirect("../../Default.aspx")
            'End If

         
            '########################################################################################
            '##### inizializzazione oggetti objParametri_Utenti e objParametri_Server  ##############
            '########################################################################################
            '---
            objParametri_Utenti = New AgronicaCoreDataProvider.AgronicaCoreParametri(Session("ASG_objParametri_Utenti"))
            objParametri_Server = New AgronicaCoreDataProvider.AgronicaCoreParametri(Session("ASG_objParametri_Server"))
            '---


            '##############################################################
            '#####  Recupero le informazioni da querystring  ##############
            '##############################################################

            Qs_Piva = Stringa_Decodifica(Request.QueryString("p").ToString, _
                            AgroKey_EncoderDecoder, _
                            Server)

            If Not IsNothing(Request.QueryString("rs")) Then

                Qs_Rag_Soc = Stringa_Decodifica(Request.QueryString("rs").ToString, _
                               AgroKey_EncoderDecoder, _
                               Server)


            Else
                ' TODo
                '''''Qs_Rag_Soc = RagSoc_from_Piva(Server, Session, Page, Qs_Piva)
            End If


            '##############################################################
            '#####  Verifico se sono in Post-Back  ########################
            '##############################################################

            If Not Page.IsPostBack Then

                '==========================================
                '===== Pagina caricata per la prima volta
                '==========================================

                '----- Verifico se l'utente dispone dei permessi per la visualizzazione della pagina

                Dim strDummy As String      'controllo accesso negato.....
                Dim UtenteAbilitato As Boolean

                Dim objPermessi As New AgronicaCoreUtentiDAL.Utenti_Permessi_R

                UtenteAbilitato = objPermessi.Controlla_Permessi_Utente( _
                                           Session("ASG_Utente_Username"), _
                                           Session("ASG_IdServizio"), _
                                           enum_Security_Attivita.Gest_CartellaAziendale_MonitoraggioCE, _
                                           enum_Security_Operazione.Modifica, _
                                           Date.Now, _
                                           "", _
                                           objParametri_Utenti)


                '----- Se l'utente non ha il permesso per visualizzare la pagina ... lo invio al menu.
                If UtenteAbilitato = False Then
                    Response.Redirect("../../GestioneCartellaAziendale/MenuCartella.aspx")
                    Exit Sub
                End If

            Else

                '==========================================
                '===== Pagina in postback
                '==========================================

                Select Case Me.SI_NO.Value

                    Case "0" 'NO

                    Case "1" 'SI
                        Cancella_MonitoraggioCE()
                End Select

                Exit Sub

            End If

            '==========================================
            '===== Carica controlli
            '==========================================

            Carica_Tipologia()

            'Non carico tutte le orticole visibili dall'utente
            'Carica_Specie()
            'carico solo le specie vegetali oggetto del monitoraggio
            Carica_SpecieVegetali_MonitoraggioCE()

            'se l'utente ha il magazzino impostato nel filtro di visibilità
            '-> far vedere solo quello
            'altrimenti caricarli tutti
            Dim Impostazione_Valore_1 As String
            Dim Piva As String = ""
            Dim Sa_Cod As Integer = 0
            Dim Fabbricato_Cod As Integer = 0

            Dim objImpostazioni As New AgronicaCoreUtentiDAL.Utenti_Impostazioni_Read
            Impostazione_Valore_1 = objImpostazioni.ImpostazioneValore1_from_ImpostazioneCod(enum_Impostazioni_Utenti.UTENTE_COD_MAGAZZINO_RIFERIMENTO, _
                                                                                             objParametri_Utenti, 1)

            objImpostazioni = Nothing
            'Impostazione_Valore_1 = ImpostazioneValore1_from_ImpostazioneCod(Server, Session, Page, _
            '                                                                Session("ASG_SuperUser_CodFiscale"), _
            '                                                                Session("ASG_Utente_Username"), _
            '                                                                enum_Impostazioni_Utenti.UTENTE_COD_MAGAZZINO_RIFERIMENTO)

            Dim objADD As New AgronicaCoreContabHLP.AccettazioneDaDiversi
            objADD.Leggi_ChiaveMagazzino_Default(Piva, Sa_Cod, Fabbricato_Cod, Impostazione_Valore_1)
            Dim clc = New AgronicaCoreUtility.CaricaListControl

            clc.Fabbricati(Me.Cmb_Magazzino,
                                                             False, "", "",
                                                             Qs_Piva,
                                                             Sa_Cod,
                                                             Fabbricato_Cod,
                                                             MAGAZZINO,
                                                             True,
                                                             "",
                                                             " Fabbricati.Fabbricato_Des ",
                                                                AGRODATAFINE,
                                                             objParametri_Server)

            '----------------------------------

            'Default
            Me.Txt_DataInizio.Text = "01/" + Right("00" + Date.Today.Month.ToString, 2) + "/" + Date.Today.Year.ToString

            'eliminato caricamento automatico in data 09/11/2011
            ' Toolbar_Carica()

            Lbl_NumModuliCarico.Visible = False

            '########################################################################

        Catch exc As Exception

            Messaggi.AgroMsgBox("Problemi nel caricamento della pagina: " + vbCrLf + exc.Message, Page)

        End Try

    End Sub





    '##################################################################################
    Private Function Carica_Produttore(ByVal Rag_Soc As String) As Boolean

        Try

            Dim DT_Prod As DataTable
            Dim FiltroAggiuntivo As String = ""

            If Rag_Soc <> "" Then
                FiltroAggiuntivo += "  (Imprese.Rag_Soc LIKE '%" & Agro_SQL_SaveText(Rag_Soc) & "%')   "
            End If

            Dim objGerarchiaImpresa As New AgronicaCoreAnagrafeDAL.GerarchiaImprese_R
            DT_Prod = objGerarchiaImpresa.LeggiFigli("", "", _
                                                    1, _
                                                     0, _
                                                     AgronicaCoreDataProvider.AgronicaCoreParametri.enumSelezioneVariabile.Selezione_JoinDescrizioni, _
                                                    FiltroAggiuntivo, "RagSoc_Figlio", _
                                                    objParametri_Server)

            If DT_Prod.Rows.Count > 0 Then

                AgronicaCoreUtility.CaricaListControl.Produttori(CType(Cmb_Produttore, ListControl), _
                                                                 True, "", "", _
                                                                 DT_Prod, "", "", objParametri_Server)

                If Me.Cmb_Produttore.Items.Count > 1 Then
                    Cmb_Produttore.SelectedIndex = 1
                End If

                Return True
            End If
            Return False

        Catch ex As Exception
            Messaggi.AgroMsgBox("Caricamento dei produttori. Si è verificato il seguente errore: " + ex.Message, Page)
            Return False
        End Try


    End Function


    '##################################################################################
    Private Function Carica_Tipologia() As Boolean

        Cmb_TipologiaRitrovamento.Items.Clear()

        Cmb_TipologiaRitrovamento.Items.Add(New ListItem("Tutti", "0"))
        Cmb_TipologiaRitrovamento.Items.Add(New ListItem("Aereoseparatore", "1"))
        Cmb_TipologiaRitrovamento.Items.Add(New ListItem("Cernitrice Ottica", "2"))
        Cmb_TipologiaRitrovamento.Items.Add(New ListItem("Cernita Manuale", "3"))

    End Function


    '##################################################################################
    Private Function Carica_Prodotto() As String

        Try

            If Cmb_Specie.SelectedValue <> "" Then

                Dim Veg_Cod As Integer
                Dim Regolamento As Integer
                Dim DT_MP As DataTable
                Dim objMateriePrime As New AgronicaCoreAnagrafeDAL.Materie_Prime_R
                Dim FiltroAggiuntivo As String

                Veg_Cod = Cmb_Specie.SelectedValue

                Select Case Me.Rbl_Biologico.SelectedValue
                    Case -1
                        FiltroAggiuntivo = ""
                    Case enum_Cod_Regolamento.Regolamento_bio
                        FiltroAggiuntivo = " Materie_Prime.Regolamento = 4 "
                    Case 0
                        FiltroAggiuntivo = " Materie_Prime.Regolamento <> 4 "
                    Case Else
                        FiltroAggiuntivo = ""
                End Select

                DT_MP = objMateriePrime.Leggi("", 0, _
                                            TRASFORMATI_VEGETALI, _
                                            0, "", Veg_Cod, 0, 0, 0, 0, 0, 0, "", 0, "", True, False, "", _
                                            AgronicaCoreDataProvider.AgronicaCoreParametri.enumSelezioneVariabile.Selezione_JoinCompleta, _
                                            FiltroAggiuntivo, "", objParametri_Server)

                If DT_MP.Rows.Count > 0 Then
                    AgronicaCoreUtility.CaricaListControl.Prodotto(CType(Me.Cmb_Prodotto, ListControl), _
                                                                   True, "", "0", _
                                                                   DT_MP, "", "", objParametri_Server)

                    'Ifme.Cmb_Prodotto.Items.Count > 1 Then
                    '   me.Cmb_Prodotto.SelectedIndex = 1
                    'End If
                    Return True
                Else
                    Me.Cmb_Prodotto.Items.Clear()
                    Me.Cmb_Prodotto.Enabled = False
                End If

            End If


            Return False

        Catch ex As Exception
            Messaggi.AgroMsgBox("Caricamento Specie. Si è verificato il seguente errore: " + ex.Message, Page)
            Return False
        End Try


    End Function


    '##################################################################################
    Private Function Carica_Specie() As Boolean

        Try

            Dim DT_Prod As DataTable
            Dim FiltroAggiuntivo As String = ""

            'filtro le orticole, visto che il monitoraggio viene fatto sulle orticole a foglia
            AgronicaCoreUtility.CaricaListControl.SpecieVegetale_Optimize(CType(Cmb_Specie, ListControl), _
                                                                          True, "Nessuna Specie", "0", _
                                                                          3, "", True, 0, 0, 0, _
                                                                          "", "", objParametri_Server, _
                                                                          objParametri_Utenti)
            If Cmb_Specie.Items.Count = 0 Then
                Throw New Exception("Errore Caricamento Specie")
            End If
            Return True

        Catch ex As Exception
            Messaggi.AgroMsgBox("Errore Caricamento Specie: " + ex.Message, Page)
            Return False
        End Try

    End Function


    '##################################################################################
    Private Sub Carica_SpecieVegetali_MonitoraggioCE()

        Try

            Cmb_Specie.Items.Add(New ListItem("Nessuna Specie", "0"))
            Cmb_Specie.Items.Add(New ListItem("Bietola da coste", 69))
            Cmb_Specie.Items.Add(New ListItem("Bietola da foglia (da taglio)", 107))
            Cmb_Specie.Items.Add(New ListItem("Cicoria", 14))
            Cmb_Specie.Items.Add(New ListItem("Cime di Rapa", 5000322))
            Cmb_Specie.Items.Add(New ListItem("Spinacio", 60))

        Catch ex As Exception
            Messaggi.AgroMsgBox("Errore Caricamento Specie: " + ex.Message, Page)
        End Try

    End Sub

    '##################################################################################
    Private Function Carica_Appezzamento() As Boolean


        Try

            Dim DT_Appezza As DataTable
            Dim FiltroAggiuntivo As String = ""

            Dim Piva As String
            Dim Veg_cod As Int32
            Dim ok As Boolean = False

            If Cmb_Produttore.SelectedValue <> "" Then
                If Cmb_Produttore.SelectedValue = "0" Then
                    Piva = ""
                Else
                    ok = True
                    Piva = Cmb_Produttore.SelectedValue
                End If
            Else
                Piva = ""
            End If

            If Cmb_Specie.SelectedValue <> "0" Then
                Veg_cod = Cmb_Specie.SelectedValue
            Else
                Veg_cod = 0
            End If

            If ok = True Then

                Dim objAppezzamento As New AgronicaCoreAnagrafeDAL.Appezzamento_Read
                DT_Appezza = objAppezzamento.LeggiAppezzamentiDaVegCod(Piva, Veg_cod, AgronicaCoreDataProvider.AgronicaCoreParametri.enumSelezioneVariabile.Selezione_JoinDescrizioni, _
                                "", "", objParametri_Server)

                If DT_Appezza.Rows.Count > 0 Then
                    Cmb_Appezzamento.Enabled = True
                    AgronicaCoreUtility.CaricaListControl.Appezzamento(CType(Cmb_Appezzamento, ListControl), _
                                                                        True, "", "", _
                                                                        DT_Appezza, "", "", objParametri_Server)

                    'If Cmb_Appezzamento.Items.Count > 1 Then
                    '    Cmb_Appezzamento.SelectedIndex = 1
                    'End If

                    Return True
                Else
                    Cmb_Appezzamento.Items.Clear()
                    Cmb_Appezzamento.Enabled = False
                End If
            End If
            Cmb_Appezzamento.Items.Clear()
            Cmb_Appezzamento.Enabled = False
            Return False

        Catch ex As Exception
            Messaggi.AgroMsgBox("Caricamento Prodotto. Si è verificato il seguente errore: " + ex.Message, Page)
            Return False
        End Try

    End Function


    '##################################################################################
    Private Sub Btn_Carica_Produttore_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Btn_Carica_Produttore.Click

        Dim Risultato As Boolean

        Risultato = Carica_Produttore(Me.Txt_FiltroRagSoc_Produttore.Text)

        If Risultato = False Then
            Messaggi.AgroMsgBox("Non è stato trovato alcun produttore con il criterio di filtro impostato.", Page)
            Cmb_Produttore.Items.Clear()
        Else
            Carica_Appezzamento()
        End If

    End Sub

    '##################################################################################
    Private Sub Cmb_Produttore_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cmb_Produttore.SelectedIndexChanged

        Carica_Appezzamento()

    End Sub

    '##################################################################################
    Private Sub Cmb_Specie_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cmb_Specie.SelectedIndexChanged

        Dim Risultato As Boolean

        If Me.Cmb_Specie.SelectedValue <> "0" Then

            Me.Cmb_Prodotto.Enabled = True
            'carico il prodotto
            Risultato = Carica_Prodotto()

            If Risultato = False Then
                'AgroMsgBox("Non è stato trovato alcun prodotto con il criterio di filtro impostato.", Page)
                Me.Cmb_Prodotto.Items.Clear()
                Me.Cmb_Prodotto.Enabled = False
            End If

        Else
            Me.Cmb_Prodotto.Items.Clear()
            Me.Cmb_Prodotto.Enabled = False

            'Me.Cmb_Appezzamento.Items.Clear()
            'Me.Cmb_Appezzamento.Enabled = False
        End If

        Carica_Appezzamento()

    End Sub

    '##################################################################################
    Private Sub Rbl_Biologico_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Rbl_Biologico.SelectedIndexChanged


        Carica_Prodotto()

    End Sub

    '##############################################################
    Private Sub ID_Trova_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ID_Trova.Click
        Toolbar_Carica()
    End Sub

    '##############################################################
    Private Sub ID_Nuovo_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ID_Nuovo.Click
        Toolbar_Nuovo()
    End Sub

    '##############################################################
    Private Sub ID_Modifica_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ID_Modifica.Click
        Toolbar_Modifica()
    End Sub

    '##############################################################
    Private Sub ID_Informazioni_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ID_Informazioni.Click
        Toolbar_Info()
    End Sub

    '##############################################################
    Private Sub ID_Azzera_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ID_Azzera.Click
        Toolbar_Reset()
    End Sub

    '##############################################################
    Private Sub ID_Stampa_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ID_Stampa.Click
        Toolbar_Stampa(enum_TipoStampa.Stampa)
    End Sub

    '##############################################################
    Private Sub ID_StampaAggregata_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ID_StampaAggregata.Click
        Toolbar_Stampa(enum_TipoStampa.StampaAggregata)
    End Sub

    '##############################################################
    Private Sub ID_Insetto_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ID_Insetto.Click
        Toolbar_GestioneCE()
    End Sub


    '##############################################################
    Private Sub btn_delete_riga_Click(sender As Object, e As System.EventArgs) Handles btn_delete_riga.Click

        If Verifica_Cancellazione() Then
            Cancella_MonitoraggioCE()
        End If



    End Sub

    '####################################################################################
    Private Function Verifica_Cancellazione() As Boolean

        Dim bRet As Boolean = True
        Dim k As Integer
        Dim i As Integer
        Dim j As Integer = 0

        For i = 0 To DataGrid_CE.Rows.Count - 1
            If CType(DataGrid_CE.Rows(i).FindControl("ChkSelezionaOperazione"), CheckBox).Checked = True Then
                j = j + 1
                k = i
            End If

        Next

        If j > 1 Then
            Messaggi.AgroMsgBox("Selezionare un elemento per volta!", Page)
            bRet = False
        End If

        Return bRet

        'Dim Testo As String

        'Testo = "Attenzione! Cancellare la registrazione selezionata?" & vbCrLf
        'Testo = Server.UrlEncode(Testo)

        'Dim Stringa As String = "<script language='vbscript'> " & _
        '                        " a = window.showModalDialog(" & Chr(34) & "../../AA_Script/Controlli/AgroSiNo/AgroSiNo.aspx?des=" & Testo & Chr(34) & "," & Chr(34) & Chr(34) & "," & Chr(34) & "dialogWidth:260px;dialogHeight:350px;status:no; center:yes;edge:raised; help:no;" & Chr(34) & ") " & _
        '                        vbCrLf & " document.all(" & Chr(34) & "SI_NO" & Chr(34) & ").value = a" & _
        '                        vbCrLf & " document.getElementById(" & Chr(34) & "Form1" & Chr(34) & ").submit()" & _
        '                        "</script>"

        'Me.FindControl("Form1").Controls.Add(New LiteralControl(Stringa))

    End Function



    '####################################################################################
    Private Sub Cancella_MonitoraggioCE()

        'per ogni riga che è attiva effettuo la cancellazione 
        Dim i As Integer

        Dim ogjAgenda As New AgronicaCoreContabDAL.Agenda_W
        Dim objMovimenti As New AgronicaCoreContabDAL.Movimenti_W
        Dim objMovimentiDettagli As New AgronicaCoreContabDAL.Movimenti_Dettagli_W

        Dim Errore As Boolean = False
        Dim StrDummy As String

        '------------------------------------------------
        '----- apro connessione e transazione
        '------------------------------------------------
        AgronicaCoreDataProvider.ConnessioniTransazioni.ApriConnessione(True, objParametri_Server)

        Try
            For i = 0 To DataGrid_CE.Rows.Count - 1
                If CType(DataGrid_CE.Rows(i).FindControl("ChkSelezionaOperazione"), CheckBox).Checked = True Then

                    objMovimentiDettagli.Cancella(DataGrid_CE.DataKeys(i).Item(0), _
                                                  DataGrid_CE.DataKeys(i).Item(1), _
                                                  DataGrid_CE.DataKeys(i).Item(2), _
                                                  DataGrid_CE.DataKeys(i).Item(3), _
                                                  0, _
                                                  "", _
                                                  objParametri_Server)

                    objMovimenti.Cancella(DataGrid_CE.DataKeys(i).Item(0), _
                                          DataGrid_CE.DataKeys(i).Item(1), _
                                          DataGrid_CE.DataKeys(i).Item(2), _
                                          DataGrid_CE.DataKeys(i).Item(3),
                                          "", _
                                          objParametri_Server)

                    ogjAgenda.Cancella(DataGrid_CE.DataKeys(i).Item(0), _
                                       DataGrid_CE.DataKeys(i).Item(1), _
                                       DataGrid_CE.DataKeys(i).Item(2), _
                                       "", _
                                       objParametri_Server)
                End If

            Next

            'chiudo la transazione
            AgronicaCoreDataProvider.ConnessioniTransazioni.ChiudiTransazione(1, objParametri_Server)
            'chiudo la connessione
            AgronicaCoreDataProvider.ConnessioniTransazioni.ChiudiConnessione(objParametri_Server)


        Catch exc As Exception
            Errore = True
            '------------------------------------------------
            'Si e' verificata una eccezione !!!!!!
            '------------------------------------------------
            'chiudo la transazione con il rollback
            AgronicaCoreDataProvider.ConnessioniTransazioni.ChiudiTransazione(2, objParametri_Server)
            'chiudo la connessione
            AgronicaCoreDataProvider.ConnessioniTransazioni.ChiudiConnessione(objParametri_Server)
            'Messaggio di errore
            StrDummy = exc.Message.ToString()

            'FACCIO APPARIRE UN ALERT......
            Messaggi.AgroMsgBox("Si è verificato un errore durante la fase di cancellazione: " & vbCrLf & StrDummy, Page)
            '------------------------------------------------
        End Try

        Toolbar_Carica()

        Me.SI_NO.Value = "0"


    End Sub



    '####################################################################################
    Private Sub Toolbar_GestioneCE()

        Dim QueryString As String

        QueryString = "?p=" & Stringa_Codifica(Qs_Piva, AgroKey_EncoderDecoder, Server)

        Dim StrWindowOpen As String = AgronicaCoreDataProvider.UtilityProvider.Page_ModalDialog_Script( _
                        "GestioneCE.aspx", QueryString, "", _
                        700, 1000, 0, 0, _
                        , , , , , , NomeForm:="aspnetForm")

        ScriptManager.RegisterClientScriptBlock(ID_Insetto, ID_Insetto.GetType(),
                                                String.Format("jQuery_{0}", "openmodal"), StrWindowOpen, False)

    End Sub

    '####################################################################################
    Private Sub Toolbar_Nuovo()

        Dim QueryString As String

        QueryString = "?p=" & Stringa_Codifica(Qs_Piva, AgroKey_EncoderDecoder, Server)
        Response.Redirect("./MonitoraggioCE_Edit.aspx" & QueryString)

    End Sub



    '####################################################################################
    Private Sub Toolbar_Modifica()

        ' controllo se è stata selezionata solamente una casella
        Dim i As Integer
        Dim j As Integer = 0
        Dim k As Integer

        For i = 0 To DataGrid_CE.Rows.Count - 1
            If CType(DataGrid_CE.Rows(i).FindControl("ChkSelezionaOperazione"), CheckBox).Checked = True Then
                j = j + 1
                k = i
            End If
        Next
        If j <> 1 Then
            Messaggi.AgroMsgBox("E' necessario selezionare una sola registrazione!", Page)
            Exit Sub
        End If

        Dim p As String
        Dim saCod As Integer
        Dim idAgenda As Integer
        Dim IdMov As Integer


        p = DataGrid_CE.DataKeys(k).Item(0)         'DataGrid_CE.Rows(k).Cells(1).Text
        saCod = DataGrid_CE.DataKeys(k).Item(1)     'DataGrid_CE.Rows(k).Cells(2).Text
        idAgenda = DataGrid_CE.DataKeys(k).Item(2)  'DataGrid_CE.Rows(k).Cells(3).Text
        IdMov = DataGrid_CE.DataKeys(k).Item(3)     'DataGrid_CE.Rows(k).Cells(4).Text

        Dim QueryString As String

        QueryString = "?p=" & Stringa_Codifica(Qs_Piva, AgroKey_EncoderDecoder, Server) & _
                        "&saCod=" & Stringa_Codifica(saCod, AgroKey_EncoderDecoder, Server) & _
                        "&idAgenda=" & Stringa_Codifica(idAgenda, AgroKey_EncoderDecoder, Server) & _
                        "&idMov=" & Stringa_Codifica(IdMov, AgroKey_EncoderDecoder, Server) & _
                        "&act=" & Stringa_Codifica("mod", AgroKey_EncoderDecoder, Server)

        Response.Redirect("./MonitoraggioCE_Edit.aspx" & QueryString)

    End Sub


    '####################################################################################
    Private Sub Toolbar_Reset()

        'resetto i parametri di ricerca
        Me.Txt_DataFine.Text = ""
        Me.Txt_DataInizio.Text = ""
        Me.Txt_FiltroRagSoc_Produttore.Text = ""


        Me.Cmb_Appezzamento.SelectedIndex = 0

        Me.Cmb_Prodotto.SelectedIndex = 0

        Me.Cmb_Produttore.SelectedIndex = 0

        Me.Cmb_Specie.SelectedIndex = 0

        DataGrid_CE.DataSource = Nothing
        DataGrid_CE.DataBind()

        Lbl_NumModuliCarico.Visible = False
        Lbl_NumModuliCarico.Text = "Numero Moduli di Carico trovati: "

    End Sub


    '####################################################################################
    Private Sub Toolbar_Info()

        ' controllo se è stata selezionata solamente una casella
        Dim i As Integer
        Dim j As Integer = 0
        Dim k As Integer

        For i = 0 To DataGrid_CE.Rows.Count - 1
            If CType(DataGrid_CE.Rows(i).FindControl("ChkSelezionaOperazione"), CheckBox).Checked = True Then
                j = j + 1
                k = i
            End If
        Next
        If j <> 1 Then
            Messaggi.AgroMsgBox("E' necessario selezionare una sola registrazione!", Page)
            Exit Sub
        End If

        Dim p As String
        Dim saCod As Integer
        Dim idAgenda As Integer
        Dim IdMov As Integer

        p = DataGrid_CE.DataKeys(k).Item(0)         'DataGrid_CE.Rows(k).Cells(1).Text
        saCod = DataGrid_CE.DataKeys(k).Item(1)     'DataGrid_CE.Rows(k).Cells(2).Text
        idAgenda = DataGrid_CE.DataKeys(k).Item(2)  'DataGrid_CE.Rows(k).Cells(3).Text
        IdMov = DataGrid_CE.DataKeys(k).Item(3)     'DataGrid_CE.Rows(k).Cells(4).Text


        Dim QueryString As String

        QueryString = "?p=" & Stringa_Codifica(Qs_Piva, AgroKey_EncoderDecoder, Server) & _
                        "&saCod=" & Stringa_Codifica(saCod, AgroKey_EncoderDecoder, Server) & _
                        "&idAgenda=" & Stringa_Codifica(idAgenda, AgroKey_EncoderDecoder, Server) & _
                        "&idMov=" & Stringa_Codifica(IdMov, AgroKey_EncoderDecoder, Server) & _
                        "&act=" & Stringa_Codifica("info", AgroKey_EncoderDecoder, Server)

        Response.Redirect("./MonitoraggioCE_Edit.aspx" & QueryString)

    End Sub


    '####################################################################################
    Private Sub Toolbar_Carica()

        Dim objCE_R As New AgronicaCoreAnagrafeDAL.CorpiEstranei_R
        Dim DT As DataTable
        Dim dal As Date
        Dim al As Date
        Dim xFiltroAggiuntivo As String
        Dim Piva_Produttore As String
        Dim Specie As Integer
        Dim Prodotto As Integer
        Dim Piva_App As String
        Dim Sa_Cod_App As Integer
        Dim Appezza_App As Integer
        Dim Id_Reg As Integer
        Dim Flag_Appezza As Boolean = False
        Dim Tipologia As Integer
        Dim Pericolosita As Integer
        Dim Sa_Cod As Integer
        Dim Fabbricato_Cod As Integer

        If Txt_DataInizio.Text <> "" Then
            dal = Txt_DataInizio.Text
        Else
            dal = Estremo_Validita_Inizio
        End If

        If Txt_DataFine.Text <> "" Then
            al = Txt_DataFine.Text
        Else
            al = Estremo_Validita_Fine
        End If

        Piva_Produttore = Cmb_Produttore.SelectedValue
        If (Cmb_Specie.SelectedValue <> "0") Then
            Specie = Cmb_Specie.SelectedValue
        Else
            Specie = 0
        End If

        If (Cmb_Prodotto.SelectedValue <> "") Then
            Prodotto = Me.Cmb_Prodotto.SelectedValue
        Else
            Prodotto = 0
        End If

        If Cmb_Appezzamento.SelectedValue <> "" Then
            Piva_App = Cmb_Appezzamento.SelectedValue.Split("|")(0)
            Sa_Cod_App = Cmb_Appezzamento.SelectedValue.Split("|")(1)
            Appezza_App = Cmb_Appezzamento.SelectedValue.Split("|")(2)
            Id_Reg = Cmb_Appezzamento.SelectedValue.Split("|")(3)
            Flag_Appezza = True
        End If

        Tipologia = Cmb_TipologiaRitrovamento.SelectedValue
        Pericolosita = Rbl_Pericolosita.SelectedValue

        Sa_Cod = Me.Cmb_Magazzino.SelectedValue.Split("|")(1)
        Fabbricato_Cod = Me.Cmb_Magazzino.SelectedValue.Split("|")(0)

        Select Case Me.Rbl_Biologico.SelectedValue
            Case -1
                xFiltroAggiuntivo = ""
            Case enum_Cod_Regolamento.Regolamento_bio
                xFiltroAggiuntivo = " Materie_Prime.Regolamento = 4 "
            Case 0
                xFiltroAggiuntivo = " Materie_Prime.Regolamento <> 4 "
            Case Else
                xFiltroAggiuntivo = ""
        End Select

        DT = objCE_R.LeggiCorpiEstraneiBolle(True, _
                                            Piva_Produttore, _
                                            dal, al, _
                                            Specie, _
                                            Prodotto, _
                                            Flag_Appezza, _
                                            Sa_Cod_App, Appezza_App, Id_Reg, _
                                            Tipologia, Pericolosita, _
                                            Sa_Cod, _
                                            Fabbricato_Cod, _
                                                xFiltroAggiuntivo, _
                                                "", _
                                                objParametri_Server)


        'Vettore di DataColumn
        Dim DtKeysCE(3) As String
        DtKeysCE(0) = "Piva"
        DtKeysCE(1) = "Sa_Cod"
        DtKeysCE(2) = "Id_Agenda"
        DtKeysCE(3) = "Id_Mov"
 
        DataGrid_CE.DataSource = DT
        DataGrid_CE.DataKeyNames = DtKeysCE
        DataGrid_CE.DataBind()

        If Not IsNothing(DT) Then
            Lbl_NumModuliCarico.Visible = True
            Lbl_NumModuliCarico.Text = "Numero Moduli di Carico trovati: " + CStr(DT.Rows.Count)
        End If

    End Sub

    Private Enum enum_TipoStampa

        Stampa = 1
        StampaAggregata = 2

    End Enum



    '####################################################################################
    Private Sub Toolbar_Stampa(ByVal Tipo_Stampa As enum_TipoStampa)

        'creo gli oggetti in sessione
        If Txt_DataInizio.Text <> "" Then
            Session("dal") = Txt_DataInizio.Text
        Else
            Session("dal") = Estremo_Validita_Inizio
        End If

        If Txt_DataFine.Text <> "" Then
            Session("al") = Txt_DataFine.Text
        Else
            Session("al") = Estremo_Validita_Fine
        End If

        Session("piva_produttore") = Cmb_Produttore.SelectedValue

        If (Cmb_Specie.SelectedValue <> "0") Then
            Session("veg_cod") = Cmb_Specie.SelectedValue
        Else
            Session("veg_cod") = 0
        End If

        If (Cmb_Prodotto.SelectedValue <> "") Then
            Session("mat_cod") = Me.Cmb_Prodotto.SelectedValue
        Else
            Session("mat_cod") = 0
        End If

        If Cmb_Appezzamento.SelectedValue <> "" Then
            Session("sa_cod") = Cmb_Appezzamento.SelectedValue.Split("|")(1)
            Session("appezza") = Cmb_Appezzamento.SelectedValue.Split("|")(2)
            Session("id_reg") = Cmb_Appezzamento.SelectedValue.Split("|")(3)
            Session("flag_appezza") = True
        Else
            Session("sa_cod") = 0
            Session("appezza") = 0
            Session("id_reg") = 0
            Session("flag_appezza") = False
        End If

        Session("tipologia") = Cmb_TipologiaRitrovamento.SelectedValue
        Session("pericolosita") = Rbl_Pericolosita.SelectedValue
        Session("regolamento") = Me.Rbl_Biologico.SelectedValue

        Session("sa_cod_fabbr") = Me.Cmb_Magazzino.SelectedValue.Split("|")(1)
        Session("fabbr_cod") = Me.Cmb_Magazzino.SelectedValue.Split("|")(0)

        Dim XmlParametri As String = ""

        Dim report As Integer = enum_CodificaStampe.ExportExcel_MonitoraggioCE
        Select Case Tipo_Stampa

            Case enum_TipoStampa.Stampa

                report = enum_CodificaStampe.ExportExcel_MonitoraggioCE

            Case enum_TipoStampa.StampaAggregata

                report = enum_CodificaStampe.ExportExcel_MonitoraggioCE_Aggregata

        End Select

        '-----------------------------------------------------------------------------------------
        '-----------------------------------------------------------------------------------------
        '-----------------------------------------------------------------------------------------
        Dim XmlDoc As New System.Xml.XmlDocument
        Dim Xml_FiltroStampa As System.Xml.XmlElement
        Xml_FiltroStampa = XmlDoc.CreateElement("FiltroStampa")



        Dim StrVariabiliStampe As String = ""
        Dim StrNodiVariabili As String = ""
        Dim StrNodo As String = ""
        Dim vVarStampe(13) As ElementoStampe

        vVarStampe(0).Nome = "dal"
        vVarStampe(0).Valore = Session("dal")

        vVarStampe(1).Nome = "al"
        vVarStampe(1).Valore = Session("al")

        vVarStampe(2).Nome = "piva_produttore"
        vVarStampe(2).Valore = Session("piva_produttore")

        vVarStampe(3).Nome = "veg_cod"
        vVarStampe(3).Valore = Session("veg_cod")

        vVarStampe(4).Nome = "mat_cod"
        vVarStampe(4).Valore = Session("mat_cod")

        vVarStampe(5).Nome = "flag_appezza"
        vVarStampe(5).Valore = Session("flag_appezza")

        vVarStampe(6).Nome = "sa_cod"
        vVarStampe(6).Valore = Session("sa_cod")

        vVarStampe(7).Nome = "appezza"
        vVarStampe(7).Valore = Session("appezza")

        vVarStampe(8).Nome = "id_reg"
        vVarStampe(8).Valore = Session("id_reg")

        vVarStampe(9).Nome = "tipologia"
        vVarStampe(9).Valore = Session("tipologia")

        vVarStampe(10).Nome = "pericolosita"
        vVarStampe(10).Valore = Session("pericolosita")

        vVarStampe(11).Nome = "regolamento"
        vVarStampe(11).Valore = Session("regolamento")

        vVarStampe(12).Nome = "sa_cod_fabbr"
        vVarStampe(12).Valore = Session("sa_cod_fabbr")

        vVarStampe(13).Nome = "fabbr_cod"
        vVarStampe(13).Valore = Session("fabbr_cod")

        ' todo
        StrNodo = XML_VariabiliStampe(vVarStampe)


        Xml_FiltroStampa.SetAttribute("username", Session("ASG_Utente_Username").ToString)
        Xml_FiltroStampa.SetAttribute("username_codfisc", Session("ASG_Utente_CodFiscale").ToString)
        Xml_FiltroStampa.SetAttribute("report", report)
        Xml_FiltroStampa.SetAttribute("user_profilo", Session("ASG_SuperUser_Username").ToString)
        Xml_FiltroStampa.SetAttribute("superuser_codfiscale", Session("ASG_SuperUser_CodFiscale").ToString)

        'metto a nothing gli oggetti di sessione
        Session("dal") = Nothing
        Session("al") = Nothing
        Session("piva_produttore") = Nothing
        Session("veg_cod") = Nothing
        Session("mat_cod") = Nothing
        Session("flag_appezza") = Nothing
        Session("sa_cod") = Nothing
        Session("appezza") = Nothing
        Session("id_reg") = Nothing
        Session("tipologia") = Nothing
        Session("pericolosita") = Nothing
        Session("regolamento") = Nothing


        'Rendo l'albero figlio del documento
        XmlDoc.AppendChild(Xml_FiltroStampa)

        'Inserisco l'XML nella stringa complessiva
        StrNodiVariabili = StrNodiVariabili & StrNodo

        'Inserisco gli elementi "VariabiliStampe" come figli del nodo "FiltroStampa"
        Xml_FiltroStampa.InnerXml = StrNodiVariabili

        'Estraggo la stringa XML complessiva
        XmlParametri = XmlDoc.InnerXml

        XmlParametri = XmlParametri.Replace("'", Chr(34))

        Dim strOpen As String = AgronicaCoreGestioneRichieste.RedirectGestione.ApriPopUp_SitoStampe_PassandoDirettamenteIParametri( _
                                         Enum_SiteRedirector.Sito_GiasOnline, _
                                        report, _
                                          CStr(Session("ASG_Utente_Username")), _
                                         CStr(Session("ASG_SuperUser_CodFiscale")), _
                                        StrNodiVariabili, _
                                         "", _
                                        "", _
                                        "", _
                                        "", _
                                            "", _
                                            "", _
                                            "")

        Page.Master.FindControl("FORM1").Controls.Add(New LiteralControl(strOpen))

        '-----------------------------------------------------------------------------------------
        '-----------------------------------------------------------------------------------------
        '-----------------------------------------------------------------------------------------

    End Sub

End Class