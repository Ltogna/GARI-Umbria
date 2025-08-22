Imports AgronicaCoreDataProvider
Imports AgronicaCoreDataProvider.Sicurezza
Imports AgronicaCoreDataProvider.CostantiPersonalizzate
Imports AgronicaCoreDataProvider.TipiEnumerativi
Imports AgronicaCoreDataProvider.UtilityProvider
Imports AgronicaCoreDataProvider.AgronicaCoreParametri
Imports AgronicaCoreUtility
Imports AgronicaCoreGestioneRichieste
Imports AgronicaControlli_2010
Imports AgronicaCoreModello.OperazioneAgenda_Temp
Imports AgronicaCoreModello.ParametriAgenda_Temp




Public Class OperazioneDiCura
    Inherits System.Web.UI.Page

    Public Master_Agenda As Agenda

    Dim objParametri_Server, objParametri_Utenti As AgronicaCoreParametri
    Dim objParametriAgenda As ParametriAgenda
    Dim Movimento_Dettaglio_Contabilizzato As Integer = 1 '1 se data<corrente -1 se data futura
    Dim NomeProdottoTabaccoCurato As String = "Prodotto Curato" 'standardizzo come "Prodotto Curato (Specie)"
    Dim NomeProdottoTabaccoInCura As String = "Prodotto In Cura"

    Private Sub OperazioneDiCura_Init(sender As Object, e As System.EventArgs) Handles Me.Init
        Master_Agenda = CType(Page.Master, Agenda)
        AddHandler Master_Agenda.Property_ImgBtn_AnnullaTutto.Click, AddressOf Me.AnnullaTutto
    End Sub

  

#Region "Metodi Eseguiti nel Load"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load



        verificoCredenzialiDiAccesso()
        inizializzoObjParametri()
        inizializzoParametriPagina()


        'If Not IsPostBack Then

        'End If

        LeggiImpostazioni()

        If Not IsPostBack Then

            Session("DtGridViewColli") = Nothing
            Session("DtGridViewInfornature") = Nothing

            Dim DtInfornature As DataTable = InizializzaDtInfornature()
            Finalizza_Infornatured(DtInfornature)

            Dim DtSfornature As DataTable = InizializzaDtSfornature()
            finalizzaSfornatura(DtSfornature, False)

            VerificaPermessi()
            caricaControlli()
            'disabilitaControlli()
            'TextBoxDataInizioCura.Text = Data
            'TextBoxDataFineCura.Text = CDate(Data).AddDays(7).ToShortDateString
            'caricaComboEssiccatoio()
            'caricaComboMagazzinoPostLavorazione()
            'caricaComboProdottoLavorato(NomeProdottoTabaccoCurato)
        Else


        End If


    End Sub

    Private Sub verificoCredenzialiDiAccesso()
        '----- Verifico che l'utente sia autenticato
        If Session("ASG_Utente_Username") = "" Then
            Session("ObjparametriAgendaProvenienzaPiva") = Nothing
            Session("ObjparametriAgendaProvenienzaSa_Cod") = Nothing
            Session("ObjparametriAgendaProvenienzaVeg_Cod") = Nothing
            Session("ObjparametriAgendaProvenienzaData") = Nothing
            Session("ObjparametriAgendaProvenienzaSa_Cod_magazzino") = Nothing
            Session("ObjparametriAgendaProvenienzafabbricato_Cod") = Nothing
            Response.Redirect("~/Custom500.aspx")
        End If
    End Sub

    Private Sub inizializzoObjParametri()
        objParametri_Server = New AgronicaCoreDataProvider.AgronicaCoreParametri(Session("ASG_objParametri_Server"))
        objParametri_Utenti = New AgronicaCoreDataProvider.AgronicaCoreParametri(Session("ASG_objParametri_Utenti"))
    End Sub

    Private Sub inizializzoParametriAgenda()
        objParametriAgenda = New ParametriAgenda
        objParametriAgenda.OperazioneMulticentro = True
    End Sub

    Private Sub inizializzoParametriPagina()

        'script Centri
        ScriptManager.RegisterStartupScript(UpdatePanelPerScript, UpdatePanelOperazione.GetType(),
                                         String.Format("jQuery_{0}", ComboCentroAziendale.ClientID), ComboCentroAziendale.GetJS(), True)
        'script Magazzini
        ScriptManager.RegisterStartupScript(UpdatePanelPerScript, UpdatePanelOperazione.GetType(),
                                         String.Format("jQuery_{0}", ComboMagazzini.ClientID), ComboMagazzini.GetJS(), True)

        ScriptManager.RegisterStartupScript(UpdatePanelPerScript, UpdatePanelOperazione.GetType(),
                                 String.Format("jQuery_{0}", ComboSpecie.ClientID), ComboSpecie.GetJS(), True)

        'due assieme non vanno
        ' ''script Centri
        ''ScriptManager.RegisterStartupScript(UpdatePanelPerScript, UpdatePanelOperazione.GetType(),
        ''                                 String.Format("jQuery_{0}", ComboCentroDestinazioneCure.ClientID), ComboCentroDestinazioneCure.GetJS(), True)
        ' ''script Magazzini
        ''ScriptManager.RegisterStartupScript(UpdatePanelPerScript, UpdatePanelOperazione.GetType(),
        ''                                 String.Format("jQuery_{0}", ComboMagazziniDestinazioneCure.ClientID), ComboMagazziniDestinazioneCure.GetJS(), True)


        Dim strJS As New StringBuilder
        strJS.AppendLine("$(document).ready(function () {                       ")
        strJS.AppendLine("   $('#" & cmb_Categoria.ClientID & "').combobox();")
        strJS.AppendLine("   $('#" & ComboCentroDestinazioneCure.ClientID & "').combobox();")
        strJS.AppendLine("   $('#" & ComboMagazziniDestinazioneCure.ClientID & "').combobox();")
        strJS.AppendLine("       $('#" + txt_DataOperazioneAlGiorno.ClientID + "').datepicker({                    ")
        strJS.AppendLine("               dateFormat: 'dd/mm/yy',          ")
        strJS.AppendLine("               disabled: false,          ")
        strJS.AppendLine("               changeMonth: true,         ")
        strJS.AppendLine("               changeYear: true          ")
        strJS.AppendLine("          });    ")
        strJS.AppendLine("       $('#" + TextBoxDataInizioCura.ClientID + "').datepicker({                    ")
        strJS.AppendLine("               dateFormat: 'dd/mm/yy',          ")
        strJS.AppendLine("               disabled: false,          ")
        strJS.AppendLine("               changeMonth: true,         ")
        strJS.AppendLine("               changeYear: true          ")
        strJS.AppendLine("          });    ")
        strJS.AppendLine("       $('#" + TextBoxDataFineCura.ClientID + "').datepicker({                    ")
        strJS.AppendLine("               dateFormat: 'dd/mm/yy',          ")
        strJS.AppendLine("               disabled: false,          ")
        strJS.AppendLine("               changeMonth: true,         ")
        strJS.AppendLine("               changeYear: true          ")
        strJS.AppendLine("          });    ")
        strJS.AppendLine("       $('#" + OraInizioCura.ClientID + "').timepicker()")
        strJS.AppendLine("       ;    ")
        strJS.AppendLine("       $('#" + OraFineCura.ClientID + "').timepicker()   ")
        strJS.AppendLine("       ;    ")
        strJS.AppendLine("                ")
        strJS.AppendLine("                     ")
        strJS.AppendLine("                     ")
        strJS.AppendLine(" });")
        ScriptManager.RegisterStartupScript(UpdatePanelOperazione, UpdatePanelOperazione.GetType(),
                                      String.Format("jQuery_{0}", UpdatePanelOperazione.ClientID), strJS.ToString, True)



        inizializzoParametriAgenda()

        Dim objOperazioniLeggi As New AgronicaCoreMetaSchemaDAL.Operazioni_R
        Dim Lav_Des As String = objOperazioniLeggi.LavorazioneDes_from_LavorazioneCod(objParametriAgenda.Lav_Cod, objParametri_Server)
        objParametriAgenda.Lav_Des = Lav_Des

        CType(Master.FindControl("Lbl_Titolo"), Label).Text = Lav_Des

        Select Case CInt(objParametriAgenda.Lav_Cod)

            Case LAVCOD_CURA
                objParametriAgenda.Cau_Mov = CStr(enum_Agenda_Causali.LINEA_PRODUZIONE)


        End Select
    End Sub

    Private Sub VerificaPermessi()
        Dim UtenteAbilitato_Lettura As Boolean = False
        Dim UtenteAbilitato_Modifica As Boolean = False
        Dim UtenteAbilitato_Lettura_Temp As Boolean = False

        Dim objPermessi As New AgronicaCoreUtentiDAL.Utenti_Permessi_R
        UtenteAbilitato_Lettura_Temp = objPermessi.Controlla_Permessi_Utente( _
                                    Session("ASG_Utente_Username"), _
                                    Session("ASG_IdServizio"), _
                                    enum_Security_Attivita.Agenda_Operazione_Di_Cura, _
                                    enum_Security_Operazione.Lettura, _
                                    Date.Now, _
                                    "", _
                                    objParametri_Utenti)

        Session("UtenteAbilitato_Lettura") = UtenteAbilitato_Lettura_Temp
        UtenteAbilitato_Lettura = UtenteAbilitato_Lettura_Temp

        If UtenteAbilitato_Lettura = False Then
            Response.Redirect("~/classi/Agro_Pages/AccessoNonConsentito.aspx")
            Exit Sub
        End If

        Dim UtenteAbilitato_Modifica_Temp As Boolean = False
        UtenteAbilitato_Modifica_Temp = objPermessi.Controlla_Permessi_Utente( _
                                   Session("ASG_Utente_Username"), _
                                   Session("ASG_IdServizio"), _
                                   enum_Security_Attivita.Agenda_Operazione_Di_Cura, _
                                   enum_Security_Operazione.Modifica, _
                                   Date.Now, _
                                   "", _
                                   objParametri_Utenti)

        Session("UtenteAbilitato_Modifica") = UtenteAbilitato_Modifica_Temp
        UtenteAbilitato_Modifica = UtenteAbilitato_Modifica_Temp

        If (Not (objParametriAgenda.Tipo_Operazione = enum_TipoOperazioneDB.Lettura)) AndAlso UtenteAbilitato_Modifica = False Then
            Response.Redirect("~/classi/Agro_Pages/AccessoNonConsentito.aspx")
            Exit Sub
        End If


    End Sub

    Private Sub LeggiImpostazioni()

        Select Case objParametriAgenda.Tipo_Operazione
            Case TipiEnumerativi.enum_TipoOperazioneDB.Scrittura
                'verificare se gestire diversamente per non perdere informazioni in modifica
        End Select

        '--------------------------------------
        'leggo le eventuali IMPOSTAZIONI UTENTE
        Dim ObjUtenti As New AgronicaCoreUtentiDAL.Utenti_Impostazioni_Read
        'Dim Tipo_Raccolta_Val As String
        'Tipo_Raccolta_Val = ObjUtenti.Impostazione_Valore_From_Impostazione_Cod_Utente_Poi_SuperUser( _
        '                                                enum_Impostazioni_Utenti.UTENTE_COD_RACCOLTA_TIPO, _
        '                                                 objParametri_Utenti)
        'If IsNumeric(Tipo_Raccolta_Val) Then
        '    Tipo_Raccolta = CInt(Tipo_Raccolta_Val)
        'Else
        '    Tipo_Raccolta = enum_RACCOLTA_TIPO.Fast
        'End If

        'Dim Tipologia_Prodotto_Val As String
        'Tipologia_Prodotto_Val = ObjUtenti.Impostazione_Valore_From_Impostazione_Cod_Utente_Poi_SuperUser( _
        '                                                enum_Impostazioni_Utenti.UTENTE_COD_RACCOLTA_TIPOLOGIA_PRODOTTO, _
        '                                                 objParametri_Utenti)

        'Dim Tipologia_Prodotto As RACCOLTA_TIPOLOGIA_PRODOTTO
        'If IsNumeric(Tipologia_Prodotto_Val) Then
        '    Tipologia_Prodotto = CInt(Tipologia_Prodotto_Val)
        'Else
        '    Tipologia_Prodotto = RACCOLTA_TIPOLOGIA_PRODOTTO.Non_Specificata
        'End If

        'ImpostaVisibilita(Tipologia_Prodotto)




        'If Not IsPostBack Then
        'Else
        'End If


        'Impostazione fissa, tipologia pagina etc




    End Sub

    Private Sub ImpostaVisibilita(ByVal Tipologia_Prodotto As RACCOLTA_TIPOLOGIA_PRODOTTO)
        ''Select Case Tipo_Raccolta
        ''    Case enum_RACCOLTA_TIPO.Fast
        ''        'senza opzioni
        ''        DivOpzioni.Visible = False

        ''        'senza magazzino e data magazzino
        ''        Master_Operazione.Property_Div_ProvenienzaRisorse.Visible = False
        ''        DivMagazzino.Visible = False

        ''        'senza quantita e senza indicare prodotto
        ''        DivProdotto.Visible = False

        ''    Case enum_RACCOLTA_TIPO.Leggera, enum_RACCOLTA_TIPO.Leggera_Con_Dettagli_Magazzino
        ''        'senza opzioni
        ''        DivOpzioni.Visible = True

        ''        'Con magazzino ma senza data magazzino e lotto
        ''        Master_Operazione.Property_Div_ProvenienzaRisorse.Visible = True
        ''        If Tipo_Raccolta = enum_RACCOLTA_TIPO.Leggera Then
        ''            DivMagazzino.Visible = False
        ''        Else
        ''            DivMagazzino.Visible = True
        ''        End If


        ''        'con quantita e prodotto
        ''        DivProdotto.Visible = True

        ''        Select Case Tipologia_Prodotto
        ''            Case RACCOLTA_TIPOLOGIA_PRODOTTO.Non_Specificata
        ''                'CheckBoxSemilavorati lascio libero
        ''                CheckBoxSemilavorati.Visible = True
        ''            Case RACCOLTA_TIPOLOGIA_PRODOTTO.Default_Su_Semilavorati
        ''                'default, quindi solo alla richiesta
        ''                If Not IsPostBack Then
        ''                    CheckBoxSemilavorati.Checked = True
        ''                    CheckBoxSemilavorati.Visible = True
        ''                End If
        ''            Case RACCOLTA_TIPOLOGIA_PRODOTTO.Default_Su_Trasormati
        ''                'default, quindi solo alla richiesta
        ''                If Not IsPostBack Then
        ''                    CheckBoxSemilavorati.Checked = False
        ''                    CheckBoxSemilavorati.Visible = True
        ''                End If
        ''            Case RACCOLTA_TIPOLOGIA_PRODOTTO.Fissa_Su_Semilavorati
        ''                CheckBoxSemilavorati.Checked = True
        ''                CheckBoxSemilavorati.Visible = False
        ''            Case RACCOLTA_TIPOLOGIA_PRODOTTO.Fissa_Su_Trasormati
        ''                CheckBoxSemilavorati.Checked = False
        ''                CheckBoxSemilavorati.Visible = False
        ''        End Select


        ''    Case enum_RACCOLTA_TIPO.Standard

        ''    Case enum_RACCOLTA_TIPO.Raccolta_e_Cura_Tabacco
        ''        ComboLavorazione.SelectedValue = 1
        ''        CambiataLavorazione()

        ''    Case Else
        ''        Throw New Exception("Tipo raccolta non codificata in enum_RACCOLTA_TIPO")
        ''End Select
    End Sub

    Private Sub caricaControlli()

        CaricaCentriAziendali()
        CaricaMagazzini()

        AgronicaCoreUtility.CaricaListControl.CategorieMagazzino( _
                                            Me.cmb_Categoria, _
                                            True, "", "", _
                                            0, _
                                              "", _
                                             0, _
                                             False, _
                                             True, _
                                             "Elem_Cod <> " & COADIUVANTI.ToString, _
                                             "", objParametri_Server)

        ComboSpecie.PrimaRiga_Flag = True
        ComboSpecie.PrimaRiga_Value = "-1"
        ComboSpecie.PrimaRiga_Text = "Selezionare una specie"
        ComboSpecie.Carica_Tutte_Specie_Esistenti = True
        ComboSpecie.Veg_Cod_ModificaLettura = 0
        ComboSpecie.Usa_Filtro_Utente = True
        ComboSpecie.CaricaComboSpecie()
        ComboSpecie.ddl_Specie.SelectedValue = objParametriAgenda.Veg_Cod
        ComboSpecie.ddl_Specie.SelectedIndex = ComboSpecie.ddl_Specie.Items.IndexOf(ComboSpecie.ddl_Specie.Items.FindByValue(objParametriAgenda.Veg_Cod))
        objParametriAgenda.Veg_Cod = ComboSpecie.Valore_Combo


        Dim data As String = objParametriAgenda.Data.ToShortDateString
        txt_DataOperazioneAlGiorno.Text = data
        TextBoxDataInizioCura.Text = data
        TextBoxDataFineCura.Text = objParametriAgenda.Data.AddDays(7).ToShortDateString


        caricaComboCentroCura()
        ComboCentroCura.SelectedValue = objParametriAgenda.Sa_Cod

        caricaComboEssiccatoio()

        caricaComboProdottoInEssiccatoio(NomeProdottoTabaccoInCura)

        caricaComboProdottoLavorato(NomeProdottoTabaccoCurato)

        CaricaCentriAziendali2()
        CaricaMagazzini2()

        Select Case objParametriAgenda.Tipo_Operazione

            Case TipiEnumerativi.enum_TipoOperazioneDB.Scrittura
                If objParametriAgenda.PaginaSitoOrigine = enum_PagineAgenda_2010.Pagina_Raccolta Then
                    cmb_Categoria.SelectedValue = objParametriAgenda.Elem_Cod
                    TxtLotto.Text = objParametriAgenda.Lotto
                    cercaProdotto()
                    If GridView_Giacenze.Rows.Count > 0 Then
                        CType(GridView_Giacenze.Rows(0).Cells(0).Controls(1), CheckBox).Checked() = True
                    End If
                    'preimposto anche il magazzino di ritorno
                    ComboCentroDestinazioneCure.SelectedValue = objParametriAgenda.Piva & "/" & objParametriAgenda.Sa_Cod
                    cambiatoCentroDestinazioneCure()
                    DivRitorno.Visible = True
                    DivRicerca.Visible = False
                    If objParametriAgenda.Veg_Cod = "" Or objParametriAgenda.Veg_Cod = "0" Or objParametriAgenda.Veg_Cod = "-1" Then
                        Throw New Exception("Non è arrivata la specie")
                    End If
                    ComboSpecie.ddl_Specie.SelectedValue = objParametriAgenda.Veg_Cod
                    If Session("ObjparametriAgendaProvenienzaPiva") = "" Then
                        Throw New Exception("Non è arrivata la piva")
                    End If
                    TextBoxPivaProv.Text = "LU/" & CStr(CDate(objParametriAgenda.Data).Year).Substring(2, 2) & "/01/" & Session("ObjparametriAgendaProvenienzaPiva")
                    TextBoxPivaProv.Enabled = False
                End If

            Case TipiEnumerativi.enum_TipoOperazioneDB.Modifica
                RipristinaControlliDaAgenda()

            Case TipiEnumerativi.enum_TipoOperazioneDB.Lettura
                RipristinaControlliDaAgenda()
                Div1.Visible = False
                ' OpzioniInfornature.Visible = False
                ComboProdottoInLavorazione.Enabled = False
                GridViewInfornature.Enabled = False
                ' OpzioniSfornature.Visible = False
                ComboProdottoLavorato.Enabled = False
                GridViewColli.Enabled = False
                Div4.Visible = False



        End Select



    End Sub

#End Region


#Region "EventiControlli"

    Protected Sub BTN_ComboSpecie_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BTN_ComboSpecie.Click
        CambioSpecie()
    End Sub

    Private Sub CambioSpecie()
        objParametriAgenda.Veg_Cod = ComboSpecie.Valore_Combo
        caricaComboProdottoInEssiccatoio(NomeProdottoTabaccoInCura)
        caricaComboProdottoLavorato(NomeProdottoTabaccoCurato)
    End Sub

    Private Sub CaricaCentriAziendali()
        ComboCentroAziendale.Piva = objParametriAgenda.Piva
        ComboCentroAziendale.CaricaComboCentroAziendale(True)
        ComboCentroAziendale.ddl_CentroAziendale.SelectedValue = objParametriAgenda.Sa_Cod
        ComboCentroAziendale.ddl_CentroAziendale.SelectedIndex = ComboCentroAziendale.ddl_CentroAziendale.Items.IndexOf(ComboCentroAziendale.ddl_CentroAziendale.Items.FindByValue(objParametriAgenda.Sa_Cod))
    End Sub

    Protected Sub BTN_ComboCentroAziendale_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BTN_ComboCentroAziendale.Click
        objParametriAgenda.Sa_Cod = ComboCentroAziendale.Valore_Combo
        objParametriAgenda.Fabbricato = "0"
        CaricaMagazzini()
    End Sub

    Private Sub CaricaMagazzini()
        'ComboMagazzini.Sa_Cod = objParametriAgenda.Sa_Cod
        ComboMagazzini.Piva = objParametriAgenda.Piva
        ComboMagazzini.Sa_Cod = objParametriAgenda.Sa_Cod
        ComboMagazzini.Flag_CodCentroFabbricato = True
        ComboMagazzini.TipoMagazzino = MAGAZZINO
        ComboMagazzini.Flag_GestioneMagazziniImpresaPadre = False
        ComboMagazzini.TestoRiga0 = "Tutti i Magazzini"
        ComboMagazzini.CaricaComboMagazzini()
        'imposto il valore
        If objParametriAgenda.Fabbricato <> "0" AndAlso Split(objParametriAgenda.Fabbricato, "|").Count = 3 Then
            'ok
        Else
            If Split(objParametriAgenda.Fabbricato, "|").Count = 2 Then
                'manca la piva
                objParametriAgenda.Fabbricato = objParametriAgenda.Fabbricato & "|" & objParametriAgenda.Piva
            Else
                objParametriAgenda.Fabbricato = "0"
            End If
        End If
        ComboMagazzini.ddl_Magazzini.SelectedValue = objParametriAgenda.Fabbricato
        ComboMagazzini.ddl_Magazzini.SelectedIndex = ComboMagazzini.ddl_Magazzini.Items.IndexOf(ComboMagazzini.ddl_Magazzini.Items.FindByValue(objParametriAgenda.Fabbricato))

    End Sub

    Protected Sub BTN_Magazzini_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BTN_ComboMagazzini.Click
        objParametriAgenda.Fabbricato = ComboMagazzini.Valore_Combo
        'tolgo tutti i sentri se ho selezionato un fabbricato
        If objParametriAgenda.Fabbricato <> "0" AndAlso objParametriAgenda.Fabbricato.Split("|").Count > 1 Then
            objParametriAgenda.Sa_Cod = objParametriAgenda.Fabbricato.Split("|")(1)
            ComboCentroAziendale.ddl_CentroAziendale.SelectedValue = objParametriAgenda.Sa_Cod
            ComboCentroAziendale.ddl_CentroAziendale.SelectedIndex = ComboCentroAziendale.ddl_CentroAziendale.Items.IndexOf(ComboCentroAziendale.ddl_CentroAziendale.Items.FindByValue(objParametriAgenda.Sa_Cod))
        End If

    End Sub

    Protected Sub BTN_ComboCategoria_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BTN_cmb_Categoria.Click
        TxtCodProdotto.Text = ""
        TxtLotto.Text = ""
        TxtProdotto.Text = ""
    End Sub

    Protected Sub ImgBtn_Cerca_Giacenza_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ImgBtn_Cerca_Giacenza.Click
        cercaProdotto()
    End Sub

    Protected Sub cercaProdotto()
        Dim Mat_Cod As Integer
        Dim Cal_Cod As Integer
        Dim Cod_Progetto As Integer
        Dim Fase_Cod As Integer
        Dim Udm_Cod As Integer
        Dim Elem_Cod As Integer
        Dim Pro_Cod As Integer
        Dim Lotto As String
        Dim piva As String
        Dim sa_cod As Integer
        Dim Fabbricato_Cod As Integer
        LeggiValori(Mat_Cod, Cal_Cod, Cod_Progetto, Fase_Cod, Udm_Cod, Elem_Cod, Pro_Cod, Lotto, piva, sa_cod, Fabbricato_Cod)


        Dim StringaFilter As String

        If Lotto.Trim <> "" And Lotto.ToLower.Trim <> "indefinito" Then
            'StringaFilter2 = "(Movimenti_dettagli.Lotto like '%" + Lotto + "%')"
            'ok
        Else
            ' StringaFilter2 = ""
            Lotto = CStr(CInt(LOTTO_NONDEFINITO))
        End If

        If StringaFilter <> "" Then
            StringaFilter = StringaFilter + " AND Movimenti_Dettagli.Elem_Cod <> -1"
        Else
            StringaFilter = " Movimenti_Dettagli.Elem_Cod <> -1"
        End If

        StringaFilter = String.Format("AND {0}", StringaFilter)

        Dim dataRicerca As Date

        If txt_DataOperazioneAlGiorno.Text = "" Then
            dataRicerca = AGRODATAFINE
        Else
            dataRicerca = CDate(txt_DataOperazioneAlGiorno.Text)
        End If

        Dim DtGiacenze As DataTable
        Dim objGiacenze As New AgronicaCoreContabDAL.Giacenze_R


        DtGiacenze = objGiacenze.SchedaGiacenzeMagazzino(dataRicerca, _
                                                 piva, _
                                                 sa_cod, _
                                                 Fabbricato_Cod, _
                                                 Elem_Cod, Pro_Cod, Mat_Cod, Cal_Cod, Cod_Progetto, Fase_Cod, Udm_Cod, Lotto, _
                                                Not Me.CheckBoxGiacenze0.Checked, _
                                                 StringaFilter, "", "", "", "", "", "", "", "", _
                                                 "", "", _
                                                "", _
                                                objParametri_Server, objParametri_Utenti)





        Dim Num_Prodotti As Integer = 0
        Dim Num_Record As Integer = 0

        ' Me.LBL_NumProd.Text = CStr(0)

        Dim objCatMag As New AgronicaCoreMetaSchemaDAL.Categorie_Magazzino_R
        Dim objMat As New AgronicaCoreAnagrafeDAL.Materie_Prime_R
        Dim objUdm As New AgronicaCoreMetaSchemaDAL.UnitaMisura_R





        Dim Dt As New DataTable

        '----- Definisco la struttura del DataTable

        Dt.Columns.Add(New DataColumn("piva", GetType(String)))
        Dt.Columns.Add(New DataColumn("Sa_Cod", GetType(String)))
        Dt.Columns.Add(New DataColumn("Sa_Nome", GetType(String)))
        Dt.Columns.Add(New DataColumn("Fabbricato_Cod", GetType(String)))
        Dt.Columns.Add(New DataColumn("Fabbricato_Des", GetType(String)))


        Dt.Columns.Add(New DataColumn("Cat_Cod", GetType(String)))
        Dt.Columns.Add(New DataColumn("Cat_Des", GetType(String)))
        Dt.Columns.Add(New DataColumn("Pro_Cod", GetType(String)))
        Dt.Columns.Add(New DataColumn("Pro_Des", GetType(String)))
        Dt.Columns.Add(New DataColumn("Mat_Cod", GetType(String)))
        Dt.Columns.Add(New DataColumn("Lotto_Int", GetType(String)))
        Dt.Columns.Add(New DataColumn("Lotto_Acc", GetType(String)))
        Dt.Columns.Add(New DataColumn("Cod_Progetto", GetType(String)))
        Dt.Columns.Add(New DataColumn("Param_Des", GetType(String)))
        Dt.Columns.Add(New DataColumn("Cal_Cod", GetType(String)))
        Dt.Columns.Add(New DataColumn("Cal_Des", GetType(String)))
        Dt.Columns.Add(New DataColumn("Udm_Cod", GetType(String)))
        Dt.Columns.Add(New DataColumn("Udm_Des", GetType(String)))
        Dt.Columns.Add(New DataColumn("Qta_Mag", GetType(String)))

        Dt.Columns.Add(New DataColumn("Qta", GetType(String)))

        Dt.Columns.Add(New DataColumn("Prezzo_Unitario", GetType(String)))
        Dt.Columns.Add(New DataColumn("Totale", GetType(String)))

        Dt.Columns.Add(New DataColumn("Fase_Cod", GetType(String)))

        Dim DtKeys(18) As String
        DtKeys(0) = "Piva"
        DtKeys(1) = "Sa_Cod"
        DtKeys(2) = "Sa_Nome"
        DtKeys(3) = "Fabbricato_Cod"
        DtKeys(4) = "Fabbricato_Des"
        DtKeys(5) = "Cat_Cod"
        DtKeys(6) = "Cat_Des"
        DtKeys(7) = "Pro_Cod"
        DtKeys(8) = "Pro_Des"
        DtKeys(9) = "Mat_Cod"
        DtKeys(10) = "Lotto_Int"
        DtKeys(11) = "Lotto_Acc"
        DtKeys(12) = "Cod_Progetto"
        DtKeys(13) = "Param_Des"
        DtKeys(14) = "Cal_Cod"
        DtKeys(15) = "Cal_Des"
        DtKeys(16) = "Udm_Cod"
        DtKeys(17) = "Udm_Des"
        DtKeys(18) = "Qta_Mag"

        GridView_Giacenze.DataKeyNames = DtKeys

        If Not DtGiacenze Is Nothing AndAlso DtGiacenze.Rows.Count > 0 Then


            For i = 0 To DtGiacenze.Rows.Count - 1

                Dim Giacenza As Decimal = 0
                If Not IsDBNull(DtGiacenze.Rows(i).Item("Giacenza")) AndAlso IsNumeric(Not IsDBNull(DtGiacenze.Rows(i).Item("Giacenza"))) Then
                    Giacenza = CDbl(DtGiacenze.Rows(i).Item("Giacenza"))
                    'If Arrotondamento >= 0 Then
                    '    Giacenza = Agro_Math.RoundNumber(Giacenza, Arrotondamento)
                    'End If
                End If

                'oltre al filtro nella query, devo fare anche il filtro su codice
                'perchè molti Decimal vengono salvati come valori infinatamente piccoli
                'ad esempio 0.00003680000000017003
                If Me.CheckBoxGiacenze0.Checked = True Or (Giacenza <> 0 And Not (Giacenza < QTA_GiancenzeVisualizzate And Giacenza > -QTA_GiancenzeVisualizzate)) Then

                    Num_Record += 1

                    Dim dr As DataRow

                    If Me.TxtProdotto.Text <> "" Then

                        Dim Descrizione As String
                        Dim Cod_Articolo_ As String

                        Select Case DtGiacenze.Rows(i).Item("Mat_Cod")

                            Case 0
                                Descrizione = objCatMag.ProDes_from_ProCod(DtGiacenze.Rows(i).Item("Elem_Cod"), DtGiacenze.Rows(i).Item("Pro_Cod"), objParametri_Server)

                            Case Else
                                Descrizione = objMat.MatDes_from_MatCod(CStr(piva),
                                                                        DtGiacenze.Rows(i).Item("Elem_Cod"),
                                                                        DtGiacenze.Rows(i).Item("Mat_Cod"),
                                                                        "", Cod_Articolo_, "", objParametri_Server)

                        End Select

                        If InStr(Descrizione, Me.TxtProdotto.Text, CompareMethod.Text) <> 0 Then

                            Num_Prodotti += 1

                            'Creo una nuova riga
                            dr = Dt.NewRow


                            'magazzino
                            dr.Item("Piva") = DtGiacenze.Rows(i).Item("Piva")
                            dr.Item("Sa_Cod") = DtGiacenze.Rows(i).Item("Sa_Cod")
                            dr.Item("Sa_Nome") = New AgronicaCoreAnagrafeDAL.CentriAziendali_Read().SaNome_from_SaCod(DtGiacenze.Rows(i).Item("Piva"), DtGiacenze.Rows(i).Item("Sa_Cod"), objParametri_Server)
                            dr.Item("Fabbricato_Cod") = DtGiacenze.Rows(i).Item("Id_Destinazione")
                            dr.Item("Fabbricato_Des") = New AgronicaCoreAnagrafeDAL.Fabbricati_R().FabbricatoDes_from_FabbricatoCod(DtGiacenze.Rows(i).Item("Piva"), DtGiacenze.Rows(i).Item("Sa_Cod"), DtGiacenze.Rows(i).Item("Id_Destinazione"), objParametri_Server)


                            'Definisco i valori
                            dr.Item("Cat_Cod") = IIf(Not IsDBNull(DtGiacenze.Rows(i).Item("Elem_Cod")), DtGiacenze.Rows(i).Item("Elem_Cod"), 0)
                            dr.Item("Cat_Des") = objCatMag.NomeComune_from_ElemCod(DtGiacenze.Rows(i).Item("Elem_Cod"), objParametri_Server)
                            dr.Item("Pro_Cod") = IIf(Not IsDBNull(DtGiacenze.Rows(i).Item("Pro_Cod")), DtGiacenze.Rows(i).Item("Pro_Cod"), 0)

                            dr.Item("Mat_Cod") = IIf(Not IsDBNull(DtGiacenze.Rows(i).Item("Mat_Cod")), DtGiacenze.Rows(i).Item("Mat_Cod"), 0)

                            'Select Case dr.Item("Mat_Cod")
                            '    Case 0
                            '        dr.Item("Pro_Des") = Descrizione ' + " (Cod:" + CStr(DtGiacenze.Rows(i).Item("Pro_Cod")) + ")"
                            '    Case Else
                            dr.Item("Pro_Des") = Descrizione '+ " (Cod:" + CStr(Cod_Articolo_) + ")"
                            'End Select

                            dr.Item("Cod_Progetto") = IIf(Not IsDBNull(DtGiacenze.Rows(i).Item("Cod_Progetto")), DtGiacenze.Rows(i).Item("Cod_Progetto"), 0)

                            If DtGiacenze.Rows(i).Item("Elem_Cod") = SEMILAVORATI_VEGETALI Then
                                If DtGiacenze.Rows(i).Item("Cod_Progetto") = 0 Then
                                    dr.Item("Lotto_Int") = "Da Terzi"
                                Else
                                    Dim objProgetto As New AgronicaCoreAnagrafeDAL.Impresa_Progetti_R

                                    dr.Item("Lotto_Int") = objProgetto.ProgettoNome_from_ProgettoCod(DtGiacenze.Rows(i).Item("Cod_Progetto"), Nothing, objParametri_Server)
                                End If
                            Else
                                dr.Item("Lotto_Int") = ""
                            End If

                            dr.Item("Lotto_Acc") = IIf(Not IsDBNull(DtGiacenze.Rows(i).Item("Lotto")), DtGiacenze.Rows(i).Item("Lotto"), "")
                            dr.Item("Cal_Cod") = IIf(Not IsDBNull(DtGiacenze.Rows(i).Item("Cal_Cod")), DtGiacenze.Rows(i).Item("Cal_Cod"), 0)

                            'faccio il controllo altrimenti mi carica sempre calibro A per tutte le categorie di prodotto
                            Select Case dr.Item("Cal_Cod")

                                Case Is > 0

                                    dr.Item("Param_Des") = "Calibro"
                                    Dim objCalibri As New AgronicaCoreMetaSchemaDAL.CalibriFrutti_R
                                    dr.Item("Cal_Des") = objCalibri.CalDes_from_CalCod(dr.Item("Cal_Cod"), objParametri_Server)
                                    objCalibri = Nothing

                                Case Is < 0


                                    '#######################################
                                    Dim ObjCampionature As New AgronicaCoreContabDAL.Materie_Prime_Campionature_R
                                    Dim DtCampionature As DataTable

                                    DtCampionature = ObjCampionature.Leggi(dr.Item("Cal_Cod"), "", 0, 0, 0,
                                                                           "", True,
                                                                           AgronicaCoreDataProvider.AgronicaCoreParametri.enumSelezioneVariabile.Selezione_TabellaCompleta,
                                                                           "", "", objParametri_Server)

                                    If Not DtCampionature Is Nothing AndAlso DtCampionature.Rows.Count > 0 Then

                                        dr.Item("Param_Des") = ""
                                        dr.Item("Cal_Des") = DtCampionature.Rows(0).Item("Descrizione")

                                    End If

                                    DtCampionature = Nothing
                                    ObjCampionature = Nothing
                                    '#######################################

                                Case 0

                                    dr.Item("Param_Des") = ""
                                    dr.Item("Cal_Des") = ""

                            End Select


                            dr.Item("Udm_Cod") = IIf(Not IsDBNull(DtGiacenze.Rows(i).Item("Udm_Cod")), DtGiacenze.Rows(i).Item("Udm_Cod"), 0)
                            dr.Item("Udm_Des") = objUdm.UdmDes_from_UdmCod(DtGiacenze.Rows(i).Item("Udm_Cod"), Nothing, objParametri_Server)

                            ' If Arrotondamento >= 0 Then
                            Giacenza = Math.Round(Giacenza, 2, MidpointRounding.AwayFromZero)
                            'End If

                            dr.Item("Qta_Mag") = Giacenza

                            dr.Item("Fase_Cod") = IIf(Not IsDBNull(DtGiacenze.Rows(i).Item("Fase_Cod")), DtGiacenze.Rows(i).Item("Fase_Cod"), 0)

                            'Associo alla tabella la nuova riga creata
                            Dt.Rows.Add(dr)


                        End If 'INSTR



                    Else 'caso di non ricerca prodotto


                        'Creo una nuova riga
                        dr = Dt.NewRow

                        'magazzino
                        dr.Item("Piva") = DtGiacenze.Rows(i).Item("Piva")
                        dr.Item("Sa_Cod") = DtGiacenze.Rows(i).Item("Sa_Cod")
                        dr.Item("Sa_Nome") = New AgronicaCoreAnagrafeDAL.CentriAziendali_Read().SaNome_from_SaCod(DtGiacenze.Rows(i).Item("Piva"), DtGiacenze.Rows(i).Item("Sa_Cod"), objParametri_Server)
                        dr.Item("Fabbricato_Cod") = DtGiacenze.Rows(i).Item("Id_Destinazione")
                        dr.Item("Fabbricato_Des") = New AgronicaCoreAnagrafeDAL.Fabbricati_R().FabbricatoDes_from_FabbricatoCod(DtGiacenze.Rows(i).Item("Piva"), DtGiacenze.Rows(i).Item("Sa_Cod"), DtGiacenze.Rows(i).Item("Id_Destinazione"), objParametri_Server)



                        'Definisco i valori
                        dr.Item("Cat_Cod") = IIf(Not IsDBNull(DtGiacenze.Rows(i).Item("Elem_Cod")), DtGiacenze.Rows(i).Item("Elem_Cod"), 0)
                        dr.Item("Cat_Des") = objCatMag.NomeComune_from_ElemCod(DtGiacenze.Rows(i).Item("Elem_Cod"), objParametri_Server)
                        dr.Item("Pro_Cod") = IIf(Not IsDBNull(DtGiacenze.Rows(i).Item("Pro_Cod")), DtGiacenze.Rows(i).Item("Pro_Cod"), 0)

                        dr.Item("Mat_Cod") = IIf(Not IsDBNull(DtGiacenze.Rows(i).Item("Mat_Cod")), DtGiacenze.Rows(i).Item("Mat_Cod"), 0)


                        Select Case dr.Item("Mat_Cod")

                            Case 0
                                dr.Item("Pro_Des") = objCatMag.ProDes_from_ProCod(DtGiacenze.Rows(i).Item("Elem_Cod"), DtGiacenze.Rows(i).Item("Pro_Cod"), objParametri_Server) '+ " (Cod:" + CStr(DtGiacenze.Rows(i).Item("Pro_Cod")) + ")"

                            Case Else
                                Dim Cod_Articolo As String
                                dr.Item("Pro_Des") = objMat.MatDes_from_MatCod(CStr(piva),
                                                                               DtGiacenze.Rows(i).Item("Elem_Cod"),
                                                                               DtGiacenze.Rows(i).Item("Mat_Cod"),
                                                                               "", Cod_Articolo, "", objParametri_Server) '+ " (Cod:" + CStr(Cod_Articolo) + ")"

                        End Select

                        dr.Item("Cod_Progetto") = IIf(Not IsDBNull(DtGiacenze.Rows(i).Item("Cod_Progetto")), DtGiacenze.Rows(i).Item("Cod_Progetto"), 0)

                        If DtGiacenze.Rows(i).Item("Elem_Cod") = SEMILAVORATI_VEGETALI Then
                            If DtGiacenze.Rows(i).Item("Cod_Progetto") = 0 Then
                                dr.Item("Lotto_Int") = "Da Terzi"
                            Else
                                Dim objProgetto As New AgronicaCoreAnagrafeDAL.Impresa_Progetti_R
                                dr.Item("Lotto_Int") = objProgetto.ProgettoNome_from_ProgettoCod(DtGiacenze.Rows(i).Item("Cod_Progetto"), Nothing, objParametri_Server)
                            End If
                        Else
                            dr.Item("Lotto_Int") = ""
                        End If

                        dr.Item("Lotto_Acc") = IIf(Not IsDBNull(DtGiacenze.Rows(i).Item("Lotto")), DtGiacenze.Rows(i).Item("Lotto"), "")
                        dr.Item("Cal_Cod") = IIf(Not IsDBNull(DtGiacenze.Rows(i).Item("Cal_Cod")), DtGiacenze.Rows(i).Item("Cal_Cod"), 0)

                        'faccio il controllo altrimenti mi carica sempre calibro A per tutte le categorie di prodotto
                        Select Case dr.Item("Cal_Cod")

                            Case Is > 0
                                dr.Item("Param_Des") = "Calibro"
                                Dim objCalibri As New AgronicaCoreMetaSchemaDAL.CalibriFrutti_R
                                dr.Item("Cal_Des") = objCalibri.CalDes_from_CalCod(dr.Item("Cal_Cod"), objParametri_Server)
                                objCalibri = Nothing
                            Case Is < 0

                                Dim ObjCampionature As New AgronicaCoreContabDAL.Materie_Prime_Campionature_R
                                Dim DtCampionature As DataTable

                                DtCampionature = ObjCampionature.Leggi(dr.Item("Cal_Cod"), "", 0, 0, 0, "",
                                                                       True,
                                                                       AgronicaCoreDataProvider.AgronicaCoreParametri.enumSelezioneVariabile.Selezione_TabellaCompleta,
                                                                       "", "", objParametri_Server)

                                If Not DtCampionature Is Nothing AndAlso DtCampionature.Rows.Count > 0 Then

                                    dr.Item("Param_Des") = ""
                                    dr.Item("Cal_Des") = DtCampionature.Rows(0).Item("Descrizione")

                                End If

                                DtCampionature = Nothing
                                ObjCampionature = Nothing
                                '#######################################

                            Case 0

                                dr.Item("Param_Des") = ""
                                dr.Item("Cal_Des") = ""

                        End Select

                        dr.Item("Udm_Cod") = IIf(Not IsDBNull(DtGiacenze.Rows(i).Item("Udm_Cod")), DtGiacenze.Rows(i).Item("Udm_Cod"), 0)
                        Dim simbolo As String = ""
                        dr.Item("Udm_Des") = objUdm.UdmDes_from_UdmCod(DtGiacenze.Rows(i).Item("Udm_Cod"), simbolo, objParametri_Server)

                        ' If Arrotondamento >= 0 Then
                        Giacenza = Math.Round(Giacenza, 2, MidpointRounding.AwayFromZero)
                        'End If

                        dr.Item("Qta_Mag") = Giacenza

                        dr.Item("Fase_Cod") = IIf(Not IsDBNull(DtGiacenze.Rows(i).Item("Fase_Cod")), DtGiacenze.Rows(i).Item("Fase_Cod"), 0)

                        'Associo alla tabella la nuova riga creata
                        Dt.Rows.Add(dr)


                    End If 'ricerca prodotto o no

                End If 'Filtro arrotondamento giacenza

            Next 'ciclo giacenze

        End If


        'If Me.TxtProdotto.Text <> "" Then
        '    Me.LBL_NumProd.Text = CStr(Num_Prodotti)
        'Else
        '    Me.LBL_NumProd.Text = CStr(Num_Record)
        'End If


        'Elimino il recordset
        DtGiacenze = Nothing


        '----- Associo il DataTable con la DataGrid

        GridView_Giacenze.DataSource = Dt
        GridView_Giacenze.DataBind()

        For i = 0 To GridView_Giacenze.Rows.Count - 1
            If (i Mod 2 = 0) Then
                GridView_Giacenze.Rows(i).BackColor = Drawing.Color.PaleGoldenrod
            End If
        Next

        Dim somma As Decimal
        somma = 0

        'For i = 0 To Me.GridView_Giacenze.Rows.Count - 1



        '    Select Case CDbl(Me.GridView_Giacenze.Rows(i).Cells(19).Text)

        '        Case Is > 0

        '            Me.GridView_Giacenze.Rows(i).Cells(19).BackColor = Drawing.Color.MediumSpringGreen

        '        Case Is = 0

        '            Me.GridView_Giacenze.Rows(i).Cells(19).BackColor = Drawing.Color.Gold

        '        Case Is < 0

        '            Me.GridView_Giacenze.Rows(i).Cells(19).BackColor = Drawing.Color.Tomato

        '    End Select


        'Next

    End Sub

    Private Sub LeggiValori(ByRef Mat_Cod As Integer, ByRef Cal_Cod As Integer, ByRef Cod_Progetto As Integer, ByRef Fase_Cod As Integer, ByRef Udm_Cod As Integer, ByRef Elem_Cod As Integer, ByRef Pro_Cod As Integer, ByRef Lotto As String, ByRef piva As String, ByRef sa_cod As Integer, ByRef Fabbricato_Cod As Integer)
        Mat_Cod = 0
        Cal_Cod = 0
        Cod_Progetto = 0
        Fase_Cod = 0
        Udm_Cod = 0
        'Dim Lotto As String = LOTTO_NONDEFINITO 'usato come filtro con like

        Elem_Cod = 0
        Pro_Cod = 0
        Lotto = ""

        piva = objParametriAgenda.Piva
        ' Dim sa_cod As Integer = objParametriAgenda.Sa_Cod 'no, potrebbe essere 0=tutti
        sa_cod = 0
        If objParametriAgenda.Fabbricato.Split("|").Count > 1 Then
            sa_cod = objParametriAgenda.Fabbricato.Split("|")(1)
        End If
        Fabbricato_Cod = objParametriAgenda.Fabbricato.Split("|")(0)

        'controllo nel caso di attivazione magazzino esterno non gestito
        If objParametriAgenda.Fabbricato.Split("|").Count > 2 Then
            If objParametriAgenda.Fabbricato.Split("|")(2) <> piva Then
                Throw New Exception("objParametriAgenda.Fabbricato.Split('|')(3) <> piva")
            End If
        End If

        If Me.cmb_Categoria.SelectedIndex = 0 Then
            Elem_Cod = 0
        Else
            Elem_Cod = Me.cmb_Categoria.SelectedValue
        End If

        If TxtCodProdotto.Text <> "" AndAlso IsNumeric(TxtCodProdotto.Text) Then
            Pro_Cod = TxtCodProdotto.Text
        End If

        If Me.TxtLotto.Text <> "" Then
            Lotto = Me.TxtLotto.Text
        End If

    End Sub

#End Region


#Region "Cura Tabacco"

    'Private Sub caricaComboUDS()
    '    Dim objFabbricati As New AgronicaCoreAnagrafeDAL.Fabbricati_R
    '    Dim ListaPivaUds As String = objFabbricati.Lista_piva_Con_Forni(objParametri_Server)
    '    AgronicaCoreUtility.CaricaListControl.Imprese(ComboUDS, True, "", "-1", _
    '                                                          "", " imprese.piva in (" & ListaPivaUds & ") ", "", objParametri_Server)
    '    'preseleziono l'uds aziendale se presente
    '    ComboUDS.SelectedValue = objParametriAgenda.Piva
    '    If ComboUDS.Items.Count = 2 Then
    '        ComboUDS.SelectedIndex = 1
    '    End If
    'End Sub

    'Protected Sub ComboUDS_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboUDS.SelectedIndexChanged
    '    caricaComboCentroCura()
    '    caricaComboEssiccatoio()
    'End Sub

    Private Sub caricaComboCentroCura()
        Dim objFabbricati As New AgronicaCoreAnagrafeDAL.Fabbricati_R
        Dim ListaSaCodUds As String = objFabbricati.Lista_Sacod_Con_Forni(objParametriAgenda.Piva, objParametri_Server)
        AgronicaCoreUtility.CaricaListControl.Centri_Aziendali(ComboCentroCura, True, "", "-1",
                                                objParametriAgenda.Piva,
                                                True, 2, " Centri_Aziendali.Sa_Cod in (" & ListaSaCodUds & ") ", "", objParametri_Server)
        If ComboCentroCura.Items.Count = 2 Then
            ComboCentroCura.SelectedIndex = 1
            objParametriAgenda.Sa_Cod = ComboCentroCura.SelectedValue
        End If
    End Sub

    Protected Sub ComboCentroCuraS_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboCentroCura.SelectedIndexChanged
        objParametriAgenda.Sa_Cod = ComboCentroCura.SelectedValue
        caricaComboEssiccatoio()
    End Sub

    Private Sub caricaComboEssiccatoio()
        Dim clc = New AgronicaCoreUtility.CaricaListControl
        clc.Fabbricati(ComboEssiccatoio, True, "", "-1",
                                                 objParametriAgenda.Piva,
                                                 ComboCentroCura.SelectedValue, 0,
                                         enum_FabbricatiTipi.essiccatoio, False,
                                         "",
                                         " Fabbricato_Des ", CDate(TextBoxDataInizioCura.Text), objParametri_Server)
        If ComboEssiccatoio.Items.Count = 2 Then
            ComboEssiccatoio.SelectedIndex = 1
        End If
    End Sub

    Private Sub caricaComboProdottoInEssiccatoio(ByVal RicercaNomeProdotto As String)

        If objParametriAgenda.Veg_Cod = "-1" Or objParametriAgenda.Veg_Cod = "0" Or objParametriAgenda.Veg_Cod = "" Then
            ComboProdottoInLavorazione.Items.Add(New ListItem("Seleziona una specie", "-2"))
            Exit Sub
        End If

        Dim Elem_Cod As Integer = TRASFORMATI_VEGETALI
        Dim clc = New AgronicaCoreUtility.CaricaListControl
        clc.Materie_Prime(ComboProdottoInLavorazione, False, "", "",
                                                            CAU_CARICO, "",
                                                            -1, 0, Elem_Cod, False,
                                                            RicercaNomeProdotto, "", "", "", 0, 0, 0, 0, 0, "", objParametriAgenda.Veg_Cod, 0, 0, 0, 0, 0, 0, objParametriAgenda.Data, "", "", objParametri_Server, objParametri_Utenti)
        If ComboProdottoInLavorazione.Items.Count = 0 Then
            ComboProdottoInLavorazione.Items.Add(New ListItem("Genera '" & RicercaNomeProdotto & " (specie)'", "-1"))
        Else
            'seleziono 
        End If
    End Sub

    Private Sub CaricaCentriAziendali2()
        'ComboCentroDestinazioneCure.Piva = objParametriAgenda.Piva
        'ComboCentroDestinazioneCure.CaricaComboCentroAziendale(True)
        'ComboCentroDestinazioneCure..ddl_Specie.SelectedValue = objParametriAgenda.Sa_Cod
        'ComboCentroDestinazioneCure.ddl_CentroAziendale.SelectedIndex = ComboCentroDestinazioneCure.ddl_CentroAziendale.Items.IndexOf(ComboCentroDestinazioneCure.ddl_CentroAziendale.Items.FindByValue(objParametriAgenda.Sa_Cod))
        Dim objFabbricati As New AgronicaCoreAnagrafeDAL.Fabbricati_R
        Dim ListaSaCodUds As String = objFabbricati.Lista_Sacod_Con_Forni(objParametriAgenda.Piva, objParametri_Server)
        AgronicaCoreUtility.CaricaListControl.Centri_Aziendali(ComboCentroDestinazioneCure, True, "Seleziona un Centro", "-1",
                                                objParametriAgenda.Piva,
                                                True, 1, "", "", objParametri_Server)
        cambiatoCentroDestinazioneCure()
    End Sub

    Protected Sub BTN_ComboCentroDestinazioneCure_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BTN_ComboCentroDestinazioneCure.Click
        cambiatoCentroDestinazioneCure()
    End Sub

    Private Sub cambiatoCentroDestinazioneCure()
        CaricaMagazzini2()
    End Sub

    Private Sub CaricaMagazzini2()
        ' ''ComboMagazziniDestinazioneCure.Sa_Cod = objParametriAgenda.Sa_Cod
        ''ComboMagazziniDestinazioneCure.Piva = objParametriAgenda.Piva
        ''ComboMagazziniDestinazioneCure.Sa_Cod = objParametriAgenda.Sa_Cod
        ''ComboMagazziniDestinazioneCure.Flag_CodCentroFabbricato = True
        ''ComboMagazziniDestinazioneCure.TipoMagazzino = MAGAZZINO
        ''ComboMagazziniDestinazioneCure.Flag_GestioneMagazziniImpresaPadre = False
        ''ComboMagazziniDestinazioneCure.TestoRiga0 = "Seleziona il Magazzino"
        ''ComboMagazziniDestinazioneCure.CaricaComboMagazzini()
        ''If ComboMagazziniDestinazioneCure.ddl_Magazzini.Items.Count = 2 Then
        ''    ComboMagazziniDestinazioneCure.ddl_Magazzini.SelectedIndex = 1
        ''End If
        ComboMagazziniDestinazioneCure.Items.Clear()
        If ComboCentroDestinazioneCure.SelectedValue <> "0" AndAlso
            ComboCentroDestinazioneCure.SelectedValue <> "-1" AndAlso
            ComboCentroDestinazioneCure.SelectedValue <> "" AndAlso
            ComboCentroDestinazioneCure.SelectedValue.Split("/").Count = 2 Then

            If objParametriAgenda.Piva <> ComboCentroDestinazioneCure.SelectedValue.Split("/")(0) Then
                Throw New Exception("qualcosa non va, il magazzino destinazione ancora non puo supportare azienda diversa")
            End If

            Dim clc = New AgronicaCoreUtility.CaricaListControl
            clc.Fabbricati(ComboMagazziniDestinazioneCure, True, "Seleziona il Magazzino", "-1",
                                         ComboCentroDestinazioneCure.SelectedValue.Split("/")(0),
                                         ComboCentroDestinazioneCure.SelectedValue.Split("/")(1), 0,
                                 enum_FabbricatiTipi.MagazzinoAziendale, False,
                                 "",
                                 " Fabbricato_Des ", AGRODATAFINE, objParametri_Server)
        End If

        If ComboMagazziniDestinazioneCure.Items.Count = 2 Then
            ComboMagazziniDestinazioneCure.SelectedIndex = 1
        End If
        cambiatoMagazzino()
    End Sub

    Protected Sub BTN_ComboMagazziniDestinazioneCure_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BTN_ComboMagazziniDestinazioneCure.Click

        cambiatoMagazzino()

    End Sub

    Private Sub cambiatoMagazzino()

    End Sub

    Private Sub caricaComboProdottoLavorato(ByVal RicercaNomeProdotto As String)

        If objParametriAgenda.Veg_Cod = "-1" Or objParametriAgenda.Veg_Cod = "0" Or objParametriAgenda.Veg_Cod = "" Then
            ComboProdottoLavorato.Items.Add(New ListItem("Seleziona una specie", "-2"))
            Exit Sub
        End If

        Dim Elem_Cod As Integer = TRASFORMATI_VEGETALI
        Dim clc = New AgronicaCoreUtility.CaricaListControl
        clc.Materie_Prime(ComboProdottoLavorato, False, "", "",
                                                            CAU_CARICO, "",
                                                            -1, 0, Elem_Cod, False,
                                                            RicercaNomeProdotto, "", "", "", 0, 0, 0, 0, 0, "", objParametriAgenda.Veg_Cod, 0, 0, 0, 0, 0, 0, objParametriAgenda.Data, "", "", objParametri_Server, objParametri_Utenti)
        If ComboProdottoLavorato.Items.Count = 0 Then
            ComboProdottoLavorato.Items.Add(New ListItem("Genera '" & RicercaNomeProdotto & " (specie)'", "-1"))
        Else
            'seleziono 
        End If
    End Sub

    Protected Sub ImgBtn_Inserisci_Infornatura_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ImgBtn_Inserisci_Infornatura.Click

        Inserisci_Infornatura()

    End Sub

    Private Sub Inserisci_Infornatura()
        Dim UDS As String = objParametriAgenda.Piva 'ComboUDS.SelectedValue
        Dim Centro As Integer = ComboCentroCura.SelectedValue
        Dim Essiccatoio As Integer = ComboEssiccatoio.SelectedValue
        Dim Mat_Cod_Prodotto_In_Essiccatoio As Integer = ComboProdottoInLavorazione.SelectedValue
        Dim NCassoni As Integer = TextBoxNCassoni.Text
        Dim dataInizioCura As String = TextBoxDataInizioCura.Text
        Dim oraInizio As String = OraInizioCura.Text

        Dim Msg As String = ""
        If UDS = "" Or UDS = "-1" Then
            Msg = "Selezionare una USD. "
        End If
        If Centro = 0 Or Centro = -1 Then
            Msg = "Selezionare un centro di cura. "
        End If
        If Essiccatoio = 0 Or Essiccatoio = -1 Then
            Msg = "Selezionare un Essiccatoio. "
        End If
        If NCassoni < 0 Then
            Msg = "Selezionare il numero di cassoni caricati. "
        End If
        If dataInizioCura = "" Or Not IsDate(dataInizioCura) Then
            Msg = "Selezionare la data di fine cura. "
        End If
        If oraInizio = "" Or Not IsDate(oraInizio) Then
            Msg = "Selezionare l'ora di fine cura. "
        End If

        Dim Magazzino_Sa_Cod As Integer = 0
        Dim Magazzino_Sa_Nome As String = ""
        Dim Magazzino_Fabbricato_Cod As Integer = 0
        Dim Magazzino_Fabbricato_Des As String = ""
        Dim Magazzino_Cat_Cod As Integer = 0
        Dim Magazzino_Cat_Des As String = ""
        Dim Magazzino_Pro_Cod As String = "0"
        Dim Magazzino_Pro_Des As String = ""
        Dim Magazzino_Mat_Cod As Integer = 0
        Dim Magazzino_Lotto_Int As String = ""
        Dim Magazzino_Lotto_Acc As String = ""
        Dim Magazzino_Cod_Progetto As Integer = 0
        Dim Magazzino_Param_Des As String = ""
        Dim Magazzino_Cal_Cod As Integer = 0
        Dim Magazzino_Cal_Des As String = ""
        Dim Magazzino_Udm_Cod As Integer = 0
        Dim Magazzino_Udm_Des As String = ""
        Dim Magazzino_Qta As Decimal = 0



        Dim selected As Integer = 0
        For i = 0 To GridView_Giacenze.Rows.Count - 1
            If CType(GridView_Giacenze.Rows(i).Cells(0).Controls(1), CheckBox).Checked() Then
                If selected Then
                    selected += 1
                    Msg = "Selezionare solo un prodotto nel magazzino. "
                    Exit For
                Else
                    selected = 1
                    Magazzino_Sa_Cod = GridView_Giacenze.DataKeys(i).Item("Sa_Cod")
                    Magazzino_Sa_Nome = GridView_Giacenze.DataKeys(i).Item("Sa_Nome")
                    Magazzino_Fabbricato_Cod = GridView_Giacenze.DataKeys(i).Item("Fabbricato_Cod")
                    Magazzino_Fabbricato_Des = GridView_Giacenze.DataKeys(i).Item("Fabbricato_Des")
                    Magazzino_Cat_Cod = GridView_Giacenze.DataKeys(i).Item("Cat_Cod")
                    Magazzino_Cat_Des = GridView_Giacenze.DataKeys(i).Item("Cat_Des")
                    Magazzino_Pro_Cod = GridView_Giacenze.DataKeys(i).Item("Pro_Cod")
                    Magazzino_Pro_Des = GridView_Giacenze.DataKeys(i).Item("Pro_Des")
                    Magazzino_Mat_Cod = GridView_Giacenze.DataKeys(i).Item("Mat_Cod")
                    Magazzino_Lotto_Int = GridView_Giacenze.DataKeys(i).Item("Lotto_Int")
                    Magazzino_Lotto_Acc = GridView_Giacenze.DataKeys(i).Item("Lotto_Acc")
                    Magazzino_Cod_Progetto = GridView_Giacenze.DataKeys(i).Item("Cod_Progetto")
                    Magazzino_Param_Des = GridView_Giacenze.DataKeys(i).Item("Param_Des")
                    Magazzino_Cal_Cod = GridView_Giacenze.DataKeys(i).Item("Cal_Cod")
                    Magazzino_Cal_Des = GridView_Giacenze.DataKeys(i).Item("Cal_Des")
                    Magazzino_Udm_Cod = GridView_Giacenze.DataKeys(i).Item("Udm_Cod")
                    Magazzino_Udm_Des = GridView_Giacenze.DataKeys(i).Item("Udm_Des")
                    Magazzino_Qta = GridView_Giacenze.DataKeys(i).Item("Qta_Mag")
                End If
            End If


        Next

        If selected = 0 Then
            Msg = "Selezionare un prodotto nel magazzino. "
        End If

        If Msg <> "" Then
            Messaggi.AgroMsgBox(Msg,
                                Page, , UpdatePanelPerScript)
            Exit Sub
        End If

        Dim Dt As DataTable = InizializzaDtInfornature()

        Dim dr As DataRow

        'For i = 0 To nColli - 1
        dr = Dt.NewRow

        'DATI Essiccatoio
        'Id univoco della infornatura di un prodotto del magazzino,
        'lo uso per indicare il lotto del carico e scarico dal Essiccatoio del prodotto tabacco in cura
        dr.Item("Id_Infornatura") = generaIdInfronatura(Magazzino_Lotto_Acc)
        dr.Item("Piva") = UDS
        dr.Item("Sa_Cod") = Centro
        dr.Item("Sa_nome") = ComboCentroCura.SelectedItem.Text
        dr.Item("Fabbricato_Cod") = Essiccatoio
        dr.Item("Fabbricato_Des") = ComboEssiccatoio.SelectedItem.Text
        dr.Item("Cassoni") = NCassoni
        dr.Item("Data_Inizio_Cura") = dataInizioCura
        dr.Item("Ora_Inizio_Cura") = oraInizio

        'DATI MAGAZZINO
        dr.Item("Magazzino_Sa_Cod") = Magazzino_Sa_Cod
        dr.Item("Magazzino_Sa_Nome") = Magazzino_Sa_Nome
        dr.Item("Magazzino_Fabbricato_Cod") = Magazzino_Fabbricato_Cod
        dr.Item("Magazzino_Fabbricato_Des") = Magazzino_Fabbricato_Des
        dr.Item("Magazzino_Cat_Cod") = Magazzino_Cat_Cod
        dr.Item("Magazzino_Cat_Des") = Magazzino_Cat_Des
        dr.Item("Magazzino_Pro_Cod") = Magazzino_Pro_Cod
        dr.Item("Magazzino_Pro_Des") = Magazzino_Pro_Des
        dr.Item("Magazzino_Mat_Cod") = Magazzino_Mat_Cod
        dr.Item("Magazzino_Lotto_Int") = Magazzino_Lotto_Int
        dr.Item("Magazzino_Lotto_Acc") = Magazzino_Lotto_Acc
        dr.Item("Magazzino_Cod_Progetto") = Magazzino_Cod_Progetto
        dr.Item("Magazzino_Param_Des") = Magazzino_Param_Des
        dr.Item("Magazzino_Cal_Cod") = Magazzino_Cal_Cod
        dr.Item("Magazzino_Cal_Des") = Magazzino_Cal_Des
        dr.Item("Magazzino_Udm_Cod") = Magazzino_Udm_Cod
        dr.Item("Magazzino_Udm_Des") = Magazzino_Udm_Des
        If IsNumeric(TextBoxQta.Text) AndAlso CInt(TextBoxQta.Text) >= 0 Then
            Magazzino_Qta = TextBoxQta.Text
        End If
        dr.Item("Magazzino_Qta") = Magazzino_Qta

        Dt.Rows.Add(dr)
        'Next



        If GridViewInfornature.Rows.Count <> 0 Then
            If Not IsNothing(Session("DtGridViewInfornature")) Then
                Dim dtOld As DataTable = Session("DtGridViewInfornature")
                For i = 0 To dtOld.Rows.Count - 1
                    If i = 0 Then
                        Dim UdsOld As String = dtOld.Rows(0).Item("Piva")
                        Dim CentroOld As String = dtOld.Rows(0).Item("Sa_Cod")
                        Dim EssiccatoioOld As String = dtOld.Rows(0).Item("Fabbricato_Cod")
                        If UDS <> UdsOld Then
                            Msg = "Attenzione, non è possibile inserire forni di aziende o centri differenti"
                        End If
                        If Centro <> CentroOld Then
                            Msg = "Attenzione, non è possibile inserire forni di aziende o centri differenti"
                        End If
                        If Essiccatoio = EssiccatoioOld Then
                            'permetto il Essiccatoio differente
                            'Msg = "Attenzione, non è possibile inserire due volte lo stesso Essiccatoio"
                        End If
                        If Msg <> "" Then
                            Messaggi.AgroMsgBox(Msg,
                                                Page, , UpdatePanelPerScript)
                            Exit Sub
                        End If
                    End If


                    Dt.ImportRow(dtOld.Rows(i))
                Next
            End If
        End If

        Finalizza_Infornatured(Dt)

    End Sub

    Private Shared Function InizializzaDtInfornature() As DataTable
        Dim Dt As New DataTable("Infornature")

        'dato infornatura (prodottto in combo)
        Dt.Columns.Add("Id_Infornatura", GetType(String))
        Dt.Columns.Add("Piva", GetType(String))
        Dt.Columns.Add("Rag_Soc", GetType(String))
        Dt.Columns.Add("Sa_Cod", GetType(Integer))
        Dt.Columns.Add("Sa_nome", GetType(String))
        Dt.Columns.Add("Fabbricato_Cod", GetType(Integer))
        Dt.Columns.Add("Fabbricato_Des", GetType(String))
        Dt.Columns.Add("Cassoni", GetType(Integer))
        Dt.Columns.Add("Data_Inizio_Cura", GetType(String))
        Dt.Columns.Add("Ora_Inizio_Cura", GetType(String))

        'Dati Magazzino
        Dt.Columns.Add(New DataColumn("Magazzino_Sa_Cod", GetType(String)))
        Dt.Columns.Add(New DataColumn("Magazzino_Sa_Nome", GetType(String)))
        Dt.Columns.Add(New DataColumn("Magazzino_Fabbricato_Cod", GetType(String)))
        Dt.Columns.Add(New DataColumn("Magazzino_Fabbricato_Des", GetType(String)))
        Dt.Columns.Add(New DataColumn("Magazzino_Cat_Cod", GetType(String)))
        Dt.Columns.Add(New DataColumn("Magazzino_Cat_Des", GetType(String)))
        Dt.Columns.Add(New DataColumn("Magazzino_Pro_Cod", GetType(String)))
        Dt.Columns.Add(New DataColumn("Magazzino_Pro_Des", GetType(String)))
        Dt.Columns.Add(New DataColumn("Magazzino_Mat_Cod", GetType(String)))
        Dt.Columns.Add(New DataColumn("Magazzino_Lotto_Int", GetType(String)))
        Dt.Columns.Add(New DataColumn("Magazzino_Lotto_Acc", GetType(String)))
        Dt.Columns.Add(New DataColumn("Magazzino_Cod_Progetto", GetType(String)))
        Dt.Columns.Add(New DataColumn("Magazzino_Param_Des", GetType(String)))
        Dt.Columns.Add(New DataColumn("Magazzino_Cal_Cod", GetType(String)))
        Dt.Columns.Add(New DataColumn("Magazzino_Cal_Des", GetType(String)))
        Dt.Columns.Add(New DataColumn("Magazzino_Udm_Cod", GetType(String)))
        Dt.Columns.Add(New DataColumn("Magazzino_Udm_Des", GetType(String)))
        Dt.Columns.Add(New DataColumn("Magazzino_Qta", GetType(String)))
        Return Dt
    End Function

    Private Sub Finalizza_Infornatured(ByVal Dt As DataTable)


        Dim DtKeys(25) As String
        DtKeys(0) = "Id_Infornatura"
        DtKeys(1) = "Piva"
        DtKeys(2) = "Sa_Cod"
        DtKeys(3) = "Fabbricato_Cod"
        DtKeys(4) = "Fabbricato_Des"
        DtKeys(5) = "Cassoni"
        DtKeys(6) = "Data_Inizio_Cura"
        DtKeys(7) = "Ora_Inizio_Cura"

        'magazzino
        DtKeys(8) = "Magazzino_Sa_Cod"
        DtKeys(9) = "Magazzino_Sa_Nome"
        DtKeys(10) = "Magazzino_Fabbricato_Cod"
        DtKeys(11) = "Magazzino_Fabbricato_Des"
        DtKeys(12) = "Magazzino_Cat_Cod"
        DtKeys(13) = "Magazzino_Cat_Des"
        DtKeys(14) = "Magazzino_Pro_Cod"
        DtKeys(15) = "Magazzino_Pro_Des"
        DtKeys(16) = "Magazzino_Mat_Cod"
        DtKeys(17) = "Magazzino_Lotto_Int"
        DtKeys(18) = "Magazzino_Lotto_Acc"
        DtKeys(19) = "Magazzino_Cod_Progetto"
        DtKeys(20) = "Magazzino_Param_Des"
        DtKeys(21) = "Magazzino_Cal_Cod"
        DtKeys(22) = "Magazzino_Cal_Des"
        DtKeys(23) = "Magazzino_Udm_Cod"
        DtKeys(24) = "Magazzino_Udm_Des"
        DtKeys(25) = "Magazzino_Qta"

        'ordino
        Dim dw As DataView = Dt.DefaultView
        dw.Sort = "Id_Infornatura, Sa_nome, Fabbricato_Des, Data_Inizio_Cura, Ora_Inizio_Cura "
        Dt = dw.ToTable
        Session("DtGridViewInfornature") = Dt

        GridViewInfornature.DataKeyNames = DtKeys
        GridViewInfornature.DataSource = Dt
        GridViewInfornature.DataBind()

        For i = 0 To GridViewInfornature.Rows.Count - 1
            If (i Mod 2 = 0) Then
                GridViewInfornature.Rows(i).BackColor = Drawing.Color.PaleGoldenrod
            End If
        Next

        VerificaBloccoPerGrigliaInfornature()
    End Sub

    Private Function generaIdInfronatura(ByVal lottomagazzino As String) As String

        'Id univoco della infornatura di un prodotto del magazzino,
        'lo uso per indicare il lotto del carico e scarico dal Essiccatoio del prodotto tabacco in cura

        Dim seq As New AgronicaCoreDataProvider.Agro_Sequenze
        Dim id As Integer = seq.NuovoId_Tabella("generaIdInfronatura", 0, 2000000000, objParametri_Server)

        'Il codice anche se ha dei valori parlanti non deve essere parlante, basta ce ne sia uno univoco
        'i dati importati da arpt avranno in testata 14-02- e il codice loro di 10 cifre ricavato dall'azienda e progressivo loro aziendale
        Dim risp As String = CStr(id).PadLeft(10, "0") '10 caratteri per id
        'risp = "02" & risp '2 caratteri per l'op non arpt, metto 02 NO, faccio id univoco, quel codice lo uso solo per sfornature
        risp = "LC/" & risp '2 caratteri per anno

        'If lottomagazzino <> "" And lottomagazzino <> "Indefinito" And lottomagazzino <> CStr(CInt(LOTTO_NONDEFINITO)) Then
        '    risp = risp & "/" & lottomagazzino
        'End If

        Return risp

    End Function

    Private Sub VerificaBloccoPerGrigliaInfornature()
        If GridViewInfornature.Rows.Count <> 0 Then
            TextBoxDataInizioCura.Enabled = False
            OraInizioCura.Enabled = False
            'ComboUDS.Enabled = False
            ComboCentroCura.Enabled = False
        Else
            TextBoxDataInizioCura.Enabled = True
            OraInizioCura.Enabled = True
            'ComboUDS.Enabled = True
            ComboCentroCura.Enabled = True
        End If

    End Sub

    Private Sub GridViewInfornature_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridViewInfornature.RowCommand
        Select Case e.CommandName
            Case "Cancella"
                Dim i As Integer = Convert.ToInt32(e.CommandArgument)
                CancellaInfornatura(i)
        End Select
    End Sub

    Private Sub CancellaInfornatura(ByVal i As Integer)
        'verifico di non avere una sfornatura
        Dim Id_Infornatura As String = GridViewInfornature.DataKeys(i).Item("Id_Infornatura")
        For j = 0 To GridViewColli.Rows.Count - 1
            If Id_Infornatura = GridViewColli.DataKeys(i).Item("Id_Infornatura") Then
                Messaggi.AgroMsgBox("Non è possibile eliminare l'infornatura perchè ci sono delle sfornature collegate.",
                                 Page, , UpdatePanelPerScript)
                Exit Sub
            End If
        Next

        If Not IsNothing(Session("DtGridViewInfornature")) Then
            Dim dtOld As DataTable = Session("DtGridViewInfornature")
            'For i = 0 To dtOld.Rows.Count - 1
            '    If i = 0 Then
            dtOld.Rows.RemoveAt(i)
            '    Exit For
            'End If

            '    Next

            Finalizza_Infornatured(dtOld)
        End If
    End Sub

    Protected Sub ImgBtn_Inserisci_Sfornatura_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ImgBtn_Inserisci_Sfornatura.Click
        Inserisci_Sfornatura()

    End Sub

    Private Sub Inserisci_Sfornatura()
        Dim UDS As String = objParametriAgenda.Piva 'ComboUDS.SelectedValue
        If IsNothing(ComboMagazziniDestinazioneCure.SelectedItem) Then
            Messaggi.AgroMsgBox("Selezionare il centro di destinazione e il magazzino",
        Page, , UpdatePanelPerScript)
            Exit Sub
        End If
        If ComboMagazziniDestinazioneCure.SelectedValue = "" Or ComboMagazziniDestinazioneCure.SelectedValue = "0" Or ComboMagazziniDestinazioneCure.SelectedValue = "-1" Then
            Messaggi.AgroMsgBox("Selezionare il magazzino",
                    Page, , UpdatePanelPerScript)
            Exit Sub
        End If

        If ComboCentroDestinazioneCure.SelectedValue.Split("/").Count <> 2 Then
            Throw New Exception("Selezionare il centro di destinazione")
        End If

        If objParametriAgenda.Piva <> ComboCentroDestinazioneCure.SelectedValue.Split("/")(0) Then
            Throw New Exception("qualcosa non va, il magazzino destinazione ancora non puo supportare azienda diversa")
        End If

        Dim Magazzino_Sa_Cod As Integer = ComboCentroDestinazioneCure.SelectedValue.Split("/")(1)
        Dim Magazzino_Fabbricato_Cod As Integer = ComboMagazziniDestinazioneCure.SelectedValue '.Split("|")(0) se si usa il controlo magazzini, che non uso perche va in conflitto con altro
        Dim Magazzino_Sa_Nome As String = ComboCentroDestinazioneCure.SelectedItem.Text
        Dim Magazzino_Fabbricato_Des As String = ComboMagazziniDestinazioneCure.SelectedItem.Text
        Dim CassoniSfornati As Integer = TextBoxCassoniSfornati.Text
        Dim nColli As Integer = TextBoxNColli.Text
        Dim inizioNum As Integer = TextBoxInizioNum.Text
        Dim dataFineCura As String = TextBoxDataFineCura.Text
        Dim oraFine As String = OraFineCura.Text

        Dim Msg As String = ""
        If UDS = "" Or UDS = "-1" Then
            Msg = "Selezionare una USD. "
        End If
        If Magazzino_Sa_Cod = 0 Or Magazzino_Sa_Cod = -1 Then
            Msg = "Selezionare un centro di destinazione. "
        End If
        If Magazzino_Fabbricato_Cod = 0 Or Magazzino_Fabbricato_Cod = -1 Then
            Msg = "Selezionare un Magazzino Di Destinazione. "
        End If
        If CassoniSfornati < 0 Then
            Msg = "Selezionare il numero di Cassoni Sfornati. "
        End If
        If nColli <= 0 Then
            Msg = "Selezionare il numero di colli curati. "
        End If
        If inizioNum < 0 Then
            Msg = "Selezionare l'inizio della numerazione. "
        End If
        If dataFineCura = "" Or Not IsDate(dataFineCura) Then
            Msg = "Selezionare la data di fine cura. "
        End If
        If oraFine = "" Or Not IsDate(oraFine) Then
            Msg = "Selezionare l'ora di fine cura. "
        End If

        Dim Id_Infornatura As String = ""

        Dim selected As Integer = 0
        For i = 0 To GridViewInfornature.Rows.Count - 1
            If CType(GridViewInfornature.Rows(i).Cells(0).Controls(1), CheckBox).Checked() Then
                If selected Then
                    selected += 1
                    Msg = "Selezionare una sola infornatura alla volta. "
                    Exit For
                Else
                    selected = 1
                    Id_Infornatura = GridViewInfornature.DataKeys(i).Item("Id_Infornatura")
                End If
            End If

        Next

        If selected = 0 Then
            Msg = "Selezionare una infornatura. "
        End If

        If Msg <> "" Then
            Messaggi.AgroMsgBox(Msg, _
                                Page, , UpdatePanelPerScript)
            Exit Sub
        End If

        Dim Dt As DataTable = InizializzaDtSfornature()

        Dim dr As DataRow

        For I = 0 To nColli - 1
            dr = Dt.NewRow
            dr.Item("Id_Infornatura") = Id_Infornatura
            dr.Item("Piva") = UDS

            dr.Item("Cassoni") = CassoniSfornati

            dr.Item("Data_Fine_Cura") = dataFineCura
            dr.Item("Ora_Fine_Cura") = oraFine
            dr.Item("Id_Lotto") = generaIdCollo(dataFineCura)

            If TextBoxPivaProv.Text.Trim <> "" Then
                dr.Item("ColloTemp") = TextBoxPivaProv.Text.Trim & "/" & CStr(inizioNum + I).PadLeft(5, "0") & ""
            Else
                dr.Item("ColloTemp") = inizioNum + I
            End If


            dr.Item("Magazzino_Sa_Cod") = Magazzino_Sa_Cod
            dr.Item("Magazzino_Sa_Nome") = Magazzino_Sa_Nome
            dr.Item("Magazzino_Fabbricato_Cod") = Magazzino_Fabbricato_Cod
            dr.Item("Magazzino_Fabbricato_Des") = Magazzino_Fabbricato_Des

            Dt.Rows.Add(dr)
        Next


        If GridViewColli.Rows.Count <> 0 Then
            If Not IsNothing(Session("DtGridViewColli")) Then
                Dim dtOld As DataTable = Session("DtGridViewColli")
                For I = 0 To dtOld.Rows.Count - 1
                    If I = 0 Then
                        'Dim UdsOld As String = dtOld.Rows(0).Item("Piva")
                        'Dim CentroOld As String = dtOld.Rows(0).Item("Sa_Cod")
                        'Dim EssiccatoioOld As String = dtOld.Rows(0).Item("Fabbricato_Cod")
                        'If UDS <> UdsOld Then
                        '    Msg = "Attenzione, non è possibile inserire forni di aziende o centri differenti"
                        'End If
                        'If Centro <> CentroOld Then
                        '    Msg = "Attenzione, non è possibile inserire forni di aziende o centri differenti"
                        'End If
                        'If Essiccatoio = EssiccatoioOld Then
                        '    'Msg = "Attenzione, non è possibile inserire due volte lo stesso Essiccatoio"
                        'End If
                        'If Msg <> "" Then
                        '    Messaggi.AgroMsgBox(Msg, _
                        '                        Page, , UpdatePanelPerScript)
                        '    Exit Sub
                        'End If
                    End If

                    Dt.ImportRow(dtOld.Rows(I))
                Next
            End If
        End If

        finalizzaSfornatura(Dt, True)
    End Sub

    Private Shared Function InizializzaDtSfornature() As DataTable
        Dim Dt As New DataTable("Cure")

        Dt.Columns.Add("Id_Infornatura", GetType(String))
        Dt.Columns.Add("Piva", GetType(String))

        Dt.Columns.Add("Cassoni", GetType(Integer))

        Dt.Columns.Add("Data_Fine_Cura", GetType(String))
        Dt.Columns.Add("Ora_Fine_Cura", GetType(String))
        Dt.Columns.Add("Id_Lotto", GetType(String))
        Dt.Columns.Add("ColloTemp", GetType(String))

        Dt.Columns.Add("Magazzino_Sa_Cod", GetType(Integer))
        Dt.Columns.Add("Magazzino_Sa_Nome", GetType(String))
        Dt.Columns.Add("Magazzino_Fabbricato_Cod", GetType(Integer))
        Dt.Columns.Add("Magazzino_Fabbricato_Des", GetType(String))

        'usti solo per il ripristino valori da agenda e per la creazione dell'oggetto da salvare
        Dt.Columns.Add("Collo", GetType(String))
        Dt.Columns.Add("Peso", GetType(String))

        Return Dt
    End Function

    Private Sub finalizzaSfornatura(ByVal Dt As DataTable, ByVal RecuperValori As Boolean)

        'recupero i valori
        Dim HtColli As New Hashtable
        Dim HtPesi As New Hashtable
        If RecuperValori Then
            For i = 0 To GridViewColli.Rows.Count - 1
                HtColli.Add(GridViewColli.DataKeys(i).Item("Id_Lotto"), CType(GridViewColli.Rows(i).Cells(8).Controls(1), TextBox).Text)
                HtPesi.Add(GridViewColli.DataKeys(i).Item("Id_Lotto"), CType(GridViewColli.Rows(i).Cells(9).Controls(1), TextBox).Text)
            Next
        Else

        End If


        Dim DtKeys(9) As String
        DtKeys(0) = "Id_Infornatura"
        DtKeys(1) = "Piva"
        DtKeys(2) = "Cassoni"
        DtKeys(3) = "Data_Fine_Cura"
        DtKeys(4) = "Ora_Fine_Cura"
        DtKeys(5) = "Id_Lotto"

        DtKeys(6) = "Magazzino_Sa_Cod"
        DtKeys(7) = "Magazzino_Sa_Nome"
        DtKeys(8) = "Magazzino_Fabbricato_Cod"
        DtKeys(9) = "Magazzino_Fabbricato_Des"

        'ordino
        Dim dw As DataView = Dt.DefaultView
        dw.Sort = "Id_Infornatura, Data_Fine_Cura, Ora_Fine_Cura, Magazzino_Sa_Cod, Magazzino_Fabbricato_Cod, Id_Lotto"
        Dt = dw.ToTable
        Session("DtGridViewColli") = Dt

        GridViewColli.DataKeyNames = DtKeys
        GridViewColli.DataSource = Dt
        GridViewColli.DataBind()

        Dim Id_SfornaturaOld As String = ""
        Dim Id_SfornaturaCorr As String = ""
        Dim Cambiato As Boolean = False
        For i = 0 To GridViewColli.Rows.Count - 1
            Id_SfornaturaCorr = GridViewColli.DataKeys(i).Item("Id_Infornatura") & "-" & GridViewColli.DataKeys(i).Item("Data_Fine_Cura") & "-" & GridViewColli.DataKeys(i).Item("Ora_Fine_Cura") & "-" & GridViewColli.DataKeys(i).Item("Magazzino_Sa_Cod") & "-" & GridViewColli.DataKeys(i).Item("Magazzino_Fabbricato_Cod") & ""
            If i = 0 Then
                Id_SfornaturaOld = Id_SfornaturaCorr
            Else
                If Id_SfornaturaOld = Id_SfornaturaCorr Then
                    'nascondo le prime 4 colonne
                    GridViewColli.Rows(i).Cells(0).Text = ""
                    GridViewColli.Rows(i).Cells(1).Text = ""
                    GridViewColli.Rows(i).Cells(2).Text = ""
                    GridViewColli.Rows(i).Cells(3).Text = ""
                    GridViewColli.Rows(i).Cells(4).Text = ""
                    GridViewColli.Rows(i).Cells(5).Text = ""
                    GridViewColli.Rows(i).Cells(6).Text = ""
                    GridViewColli.Rows(i).Cells(7).Text = ""
                Else
                    Id_SfornaturaOld = Id_SfornaturaCorr
                    Cambiato = Not Cambiato
                End If

            End If
            If Cambiato = False Then
                GridViewColli.Rows(i).BackColor = Drawing.Color.PaleGoldenrod
            End If

            CType(GridViewColli.Rows(i).Cells(8).Controls(1), TextBox).Text = CStr(Dt.Rows(i).Item("ColloTemp"))

            If RecuperValori Then
                If HtColli.Contains(GridViewColli.DataKeys(i).Item("Id_Lotto")) Then
                    CType(GridViewColli.Rows(i).Cells(8).Controls(1), TextBox).Text = HtColli(GridViewColli.DataKeys(i).Item("Id_Lotto"))
                End If
                If HtPesi.Contains(GridViewColli.DataKeys(i).Item("Id_Lotto")) Then
                    CType(GridViewColli.Rows(i).Cells(9).Controls(1), TextBox).Text = HtPesi(GridViewColli.DataKeys(i).Item("Id_Lotto"))
                End If
            Else
                CType(GridViewColli.Rows(i).Cells(8).Controls(1), TextBox).Text = Dt.Rows(i).Item("Collo")
                CType(GridViewColli.Rows(i).Cells(9).Controls(1), TextBox).Text = Dt.Rows(i).Item("Peso")
            End If

        Next
        VerificaBloccoPerGrigliaForni()
    End Sub

    Private Function generaIdCollo(data As Date) As String

        Dim seq As New AgronicaCoreDataProvider.Agro_Sequenze
        Dim id As Integer = seq.NuovoId_Tabella("generaIdCollo", 0, 2000000000, objParametri_Server)

        'Il codice anche se ha dei valori parlanti non deve essere parlante, basta ce ne sia uno univoco
        'i dati importati da arpt avranno in testata 14-02- e il codice loro di 10 cifre ricavato dall'azienda e progressivo loro aziendale
        Dim risp As String = CStr(id).PadLeft(10, "0") '10 caratteri per id
        risp = "01" & risp '2 caratteri per l'op non arpt, metto 02
        risp = "LN/" & data.Year.ToString.Substring(2, 2) & risp '2 caratteri per anno


        Return risp

    End Function

    Private Sub GridViewColli_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridViewColli.RowCommand

        Select Case e.CommandName
            Case "Cancella"
                Dim i As Integer = Convert.ToInt32(e.CommandArgument)
                CancellaSfornatura(i)
        End Select

    End Sub

    Private Sub CancellaSfornatura(ByVal i As Integer)

        If Not IsNothing(Session("DtGridViewColli")) Then
            Dim dtOld As DataTable = Session("DtGridViewColli")
            'For i = 0 To dtOld.Rows.Count - 1
            '    If i = 0 Then
            dtOld.Rows.RemoveAt(i)
            '    Exit For
            'End If

            '    Next

            finalizzaSfornatura(dtOld, True)
        End If
    End Sub

    Private Sub VerificaBloccoPerGrigliaForni()
        If GridViewColli.Rows.Count <> 0 Then
            TextBoxDataInizioCura.Enabled = False
            OraInizioCura.Enabled = False
            'ComboUDS.Enabled = False
            ComboCentroCura.Enabled = False
        Else
            TextBoxDataInizioCura.Enabled = True
            OraInizioCura.Enabled = True
            'ComboUDS.Enabled = True
            ComboCentroCura.Enabled = True
        End If

    End Sub

#End Region


#Region "Salvataggio - Uscita"

    Private Sub AnnullaTutto(sender As Object, e As ImageClickEventArgs)

        Dim link As String = "../Menu/Menu.aspx"

        Dim sitoorigine As Enum_SiteRedirector = objParametriAgenda.SitoOrigine

        If sitoorigine = Enum_SiteRedirector.GiasNG Then
            AgronicaCoreGestioneRichieste.MenuBS_2017_RedirectGestione.RedirectGenerico(objParametriAgenda.Piva,
                                                                                            Enum_SiteRedirector.GiasNG,
                                                                                            objParametriAgenda.PaginaSitoOrigine,
                                                                                            link,
                                                                                            objParametri_Server)

        Else
            Dim objConfigSiti As New AgronicaCoreVarieDAL.Configurazione_Siti_R
            Dim objParametri_Server As AgronicaCoreParametri = New AgronicaCoreDataProvider.AgronicaCoreParametri(Session("ASG_objParametri_Server"))
            Dim DTConfigSiti As DataTable = objConfigSiti.Leggi(0, "MenuAgendaBS", "", "", objParametri_Server)

            If Not IsNothing(DTConfigSiti) AndAlso DTConfigSiti.Rows.Count > 0 AndAlso LCase(DTConfigSiti.Rows(0).Item("Valore")) = "true" Then
                link = "../Menu/MenuBS_Agenda_Nuovo.aspx"
            End If

            Dim objParametriAgenda As New ParametriAgenda
            If objParametriAgenda.PaginaSitoOrigine = enum_PagineAgenda_2010.Pagina_Raccolta And
                objParametriAgenda.Tipo_Operazione = enum_TipoOperazioneDB.Scrittura Then

                objParametriAgenda.Svuota_DatiOperazione()
                objParametriAgenda.PaginaSitoOrigine = enum_PagineAgenda_2010.Pagina_Operazione_Di_Cura
                objParametriAgenda.Piva = Session("ObjparametriAgendaProvenienzaPiva")
                objParametriAgenda.Sa_Cod = Session("ObjparametriAgendaProvenienzaSa_Cod")
                objParametriAgenda.Veg_Cod = Session("ObjparametriAgendaProvenienzaVeg_Cod")
                objParametriAgenda.Data = Session("ObjparametriAgendaProvenienzaData")
                objParametriAgenda.RagSoc = ""
                objParametriAgenda.PaginaSitoOrigine = 0
            Else
                objParametriAgenda.Svuota_DatiOperazione()
            End If


            Session("ObjparametriAgendaProvenienzaPiva") = Nothing
            Session("ObjparametriAgendaProvenienzaSa_Cod") = Nothing
            Session("ObjparametriAgendaProvenienzaVeg_Cod") = Nothing
            Session("ObjparametriAgendaProvenienzaData") = Nothing
            Session("ObjparametriAgendaProvenienzaSa_Cod_magazzino") = Nothing
            Session("ObjparametriAgendaProvenienzafabbricato_Cod") = Nothing
        End If

        '  link = CType(Master.Master, Agenda).TrovaRedirectCorretto(False, enum_PagineGiasOnline.MenuAgenda, objParametriAgenda)
        Response.Redirect(link)


    End Sub

    Protected Sub ImgBtnSalvaTutto_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ImgBtnSalvaTutto.Click
        SalvaTutto()
    End Sub

    Protected Sub SalvaTutto()


        Dim messaggio_errore As String = ""
        If SalvaOperazioneAgenda(messaggio_errore) Then
            'premutosalva = True
            gestisciTipoSalvataggio()
        Else

            Messaggi.AgroMsgBox(Resources.AgronicaAgenda_2010.AttenzioneLOperazioneNonÈStataRegistrataBr & messaggio_errore, _
                      Page, , UpdatePanelPerScript)
        End If

    End Sub

    Private Sub gestisciTipoSalvataggio()

        Dim link As String = "../Menu/Menu.aspx"

        Dim sitoorigine As Enum_SiteRedirector = objParametriAgenda.SitoOrigine

        If sitoorigine = Enum_SiteRedirector.GiasNG Then
            AgronicaCoreGestioneRichieste.MenuBS_2017_RedirectGestione.RedirectGenerico(objParametriAgenda.Piva,
                                                                                            Enum_SiteRedirector.GiasNG,
                                                                                            objParametriAgenda.PaginaSitoOrigine,
                                                                                            link,
                                                                                            objParametri_Server)
        Else

            If objParametriAgenda.PaginaSitoOrigine = enum_PagineAgenda_2010.Pagina_Raccolta And
                objParametriAgenda.Tipo_Operazione = enum_TipoOperazioneDB.Scrittura Then

                objParametriAgenda.Piva = Session("ObjparametriAgendaProvenienzaPiva")
                objParametriAgenda.Sa_Cod = Session("ObjparametriAgendaProvenienzaSa_Cod")
                objParametriAgenda.Veg_Cod = Session("ObjparametriAgendaProvenienzaVeg_Cod")
                objParametriAgenda.Data = Session("ObjparametriAgendaProvenienzaData")
                objParametriAgenda.RagSoc = ""
                objParametriAgenda.PaginaSitoOrigine = 0
            Else

            End If

            objParametriAgenda.Svuota_DatiOperazione()

            Session("ObjparametriAgendaProvenienzaPiva") = Nothing
            Session("ObjparametriAgendaProvenienzaSa_Cod") = Nothing
            Session("ObjparametriAgendaProvenienzaVeg_Cod") = Nothing
            Session("ObjparametriAgendaProvenienzaData") = Nothing
            Session("ObjparametriAgendaProvenienzaSa_Cod_magazzino") = Nothing
            Session("ObjparametriAgendaProvenienzafabbricato_Cod") = Nothing



            Dim objConfigSiti As New AgronicaCoreVarieDAL.Configurazione_Siti_R
            Dim objParametri_Server As AgronicaCoreParametri = New AgronicaCoreDataProvider.AgronicaCoreParametri(Session("ASG_objParametri_Server"))
            Dim DTConfigSiti As DataTable = objConfigSiti.Leggi(0, "MenuAgendaBS", "", "", objParametri_Server)

            If Not IsNothing(DTConfigSiti) AndAlso DTConfigSiti.Rows.Count > 0 AndAlso LCase(DTConfigSiti.Rows(0).Item("Valore")) = "true" Then
                link = "../Menu/MenuBS_Agenda_Nuovo.aspx"
            End If

        End If

        Response.Redirect(link)

    End Sub

    Private Function SalvaOperazioneAgenda(ByRef messaggio_errore As String) As Boolean
        Dim res As Boolean = False


        If objParametriAgenda.Veg_Cod = "-1" Or objParametriAgenda.Veg_Cod = "0" Or objParametriAgenda.Veg_Cod = "" Then
            messaggio_errore = "Selezionare una specie."
            Return False
        End If

        If ComboProdottoInLavorazione.SelectedValue = "-2" Then
            messaggio_errore = "Selezionare una specie per poter generare o selezionare un prodotto in lavorazione."
            Return False
        End If

        If ComboProdottoLavorato.SelectedValue = "-2" Then
            messaggio_errore = "Selezionare una specie per poter generare o selezionare un prodotto lavorato."
            Return False
        End If



        '---------------------------------------
        ' recupero il CENTRO
        If (objParametriAgenda.Sa_Cod = "" Or objParametriAgenda.Sa_Cod = "0") And objParametriAgenda.OperazioneMulticentro = False Then
            messaggio_errore = messaggio_errore & Resources.AgronicaAgenda_2010.SelezionareUnCentroAziendale
            Return False
        End If

        'recupero impianti dalla gridview
        If GridViewInfornature.Rows.Count = 0 Then
            messaggio_errore = "Occorre che ci sia almeno una infornatura per salvare l'operazione."
            Return False
        End If

        Try


            '-----------------------------------------------------
            '----------- CONNESSIONE E TRANSAZIONE ---------------
            Dim objDP As New AgronicaCoreDataProvider.ConnessioniTransazioni
            ConnessioniTransazioni.ApriConnessione(True, objParametri_Server)
            '-----------------------------------------------------


            Select Case objParametriAgenda.Sa_Cod

                '--------------------------------------------------
                '--------------------------------------------------
                '--------- OPERAZIONE MULTI-CENTRO    -------------
                '--------------------------------------------------
                '--------------------------------------------------
                Case "0"
                    messaggio_errore = "L'operazione è singolo centro e salvata sul centro di cura"
                    Throw New Exception(messaggio_errore)
                Case Else

                    '--------------------------------------------------
                    '--------------------------------------------------
                    '--------- OPERAZIONE SINGOLO CENTRO    -----------
                    '--------------------------------------------------
                    '--------------------------------------------------

                    'controllo che non ci siano centri di cura differenti anche
                    'la pagina non permette di inserirli
                    Dim ListaSacod As New HashSet(Of Integer)
                    Dim Trovato As Boolean
                    For i = 0 To GridViewInfornature.Rows.Count - 1
                        Dim sacod As Integer = GridViewInfornature.DataKeys(i).Item("Sa_Cod")
                        If Not ListaSacod.Contains(sacod) Then
                            ListaSacod.Add(sacod)
                        End If
                    Next

                    If ListaSacod.Count > 1 Then
                        messaggio_errore = "L'operazione è singolo centro ma le infornature contengono " & ListaSacod.Count & " centri differenti"
                        Throw New Exception(messaggio_errore)
                    End If

                    Dim Agenda As Operazione_Agenda = creaOggettiAgendaLavorazione(messaggio_errore)
                    If IsNothing(Agenda) Then
                        Throw New Exception(Resources.AgronicaAgenda_2010.NonÈStatoPossibileCreareLOperazione)
                    End If

                    Dim objAgendaScrivi As New Agenda_Operazione_Helper

                    Dim Id_Agenda_Carico As Integer = 0
                    Dim Id_Agenda_Scarico As Integer = 0




                    '--------------------------------------------------------------------------------------------------------------------------------
                    '--------------SALVO-MODIFICO CURA--------------------------------------------------------------------------
                    '--------------------------------------------------------------------------------------------------------------------------------
                    If objParametriAgenda.Tipo_Operazione = enum_TipoOperazioneDB.Modifica Then

                        'prima di cancellare agenda leggo i riferimenti o li cancella

                        Dim x As New AgronicaCoreContabDAL.Mov_Dettagli_Riferimenti_R
                        Dim dtrifScarico As DataTable = x.Leggi_Specifica("", 0, 0, 0, 0, LAVCOD_SCARICO, "", _
                                                                   objParametriAgenda.Piva, _
                                                                            objParametriAgenda.Sa_Cod, _
                                                                            objParametriAgenda.Id_Agenda, _
                                                                            -1, -1, LAVCOD_CURA, "", _
                                                                             "", "", objParametri_Server)
                        Dim dtrifCarico As DataTable = x.Leggi_Specifica("", 0, 0, 0, 0, LAVCOD_CARICO, "", _
                                                    objParametriAgenda.Piva, _
                                                    objParametriAgenda.Sa_Cod, _
                                                    objParametriAgenda.Id_Agenda, _
                                                    -1, -1, LAVCOD_CURA, "", _
                                                     "", "", objParametri_Server)

                        If dtrifCarico.Rows.Count <> dtrifScarico.Rows.Count Then
                            Throw New Exception("Dovrebbe essere uguali")
                        End If
                        If dtrifCarico.Rows.Count <> 0 Then
                            If dtrifCarico.Rows.Count = 1 Then
                                Id_Agenda_Carico = dtrifCarico.Rows(0).Item("Id_Agenda")
                                Id_Agenda_Scarico = dtrifScarico.Rows(0).Item("Id_Agenda")
                            Else
                                Throw New Exception("Dovrebbe essercene due o nessuno")
                            End If
                        End If

                        Dim CancellataOperazione As Boolean = False
                        CancellataOperazione = objAgendaScrivi.Cancella(objParametriAgenda.Piva,
                                                                         objParametriAgenda.Sa_Cod,
                                                                         objParametriAgenda.Id_Agenda, False,
                                                                         objParametri_Server, logCancellazione:=False)

                    End If

                    Dim Id_Agenda As Integer
                    Id_Agenda = objAgendaScrivi.Scrivi(Agenda, objParametri_Server)




                    If objParametriAgenda.PaginaSitoOrigine = enum_PagineAgenda_2010.Pagina_Raccolta Then
                        'devo gestire il ritoprno del prodotto se sono tra aziende diverse
                        If objParametriAgenda.Piva <> Session("ObjparametriAgendaProvenienzaPiva") Then


                            'devo recuperare i movimenti di carico del prodotto curato nel pagazzino, scaricarli e caricarli nell'azienda di origine
                            Dim MovimentiCure As New List(Of Movimento)
                            For Each Movimento As Movimento In Agenda.Movimenti
                                If Movimento.Cau_Mov = "7300" Then
                                    If Movimento.Movimenti_Dettagli.Count > 0 Then
                                        If Movimento.Movimenti_Dettagli(0).Movimenti_Destinazioni.Count > 0 Then
                                            If Movimento.Movimenti_Dettagli(0).Movimenti_Destinazioni(0).Tipo = MAGAZZINO Then
                                                MovimentiCure.Add(Movimento)
                                            End If
                                        End If
                                    End If
                                End If
                            Next

                            '-----------------------------------------------------------------------------------
                            'creo l'agenda con mov scarico dei prodotti caricati
                            If MovimentiCure.Count > 0 Then



                                '--------------------------------------------------------------------------------------------------------------------------------
                                '--------------SALVO-MODIFICO SCARICO PER AZ--------------------------------------------------------------------------
                                '--------------------------------------------------------------------------------------------------------------------------------

                                Dim Tipo_Operazione As Integer = enum_TipoOperazioneDB.Scrittura
                                Dim Provenienza As String = Session("ObjparametriAgendaProvenienzaPiva")
                                Dim ProvenienzaDesc = Provenienza & " " & New AgronicaCoreAnagrafeDAL.Imprese_Read().RagSoc_from_Piva(Provenienza, objParametri_Server)
                                Dim agendaScarico As Operazione_Agenda = AgronicaCoreModello.OperazioneDiCuraClasse.CreaOggettoAgendaScaricoProdottoPerInvioAziendaOrigine(MovimentiCure, _
                                                                                       Id_Agenda_Scarico, _
                                                                                       Tipo_Operazione, ProvenienzaDesc)
                                If Id_Agenda_Scarico <> 0 And objParametriAgenda.Tipo_Operazione = enum_TipoOperazioneDB.Modifica Then
                                    Dim CancellataOperazione As Boolean = False
                                    CancellataOperazione = objAgendaScrivi.Cancella(agendaScarico.Piva,
                                                                                     agendaScarico.Sa_Cod,
                                                                                     Id_Agenda_Scarico, False,
                                                                                     objParametri_Server, logCancellazione:=False)
                                End If
                                Id_Agenda_Scarico = objAgendaScrivi.Scrivi(agendaScarico, objParametri_Server)



                                '--------------------------------------------------------------------------------------------------------------------------------
                                '--------------SALVO-MODIFICO SCARICO AZ--------------------------------------------------------------------------
                                '--------------------------------------------------------------------------------------------------------------------------------
                                '-----------------------------------------------------------------------------------
                                'creo il carico ricopiando lo scarico, prima faccio però la scrittura 
                                'altrimenti cambio i movimenti prima di scriverli
                                'dati del magazzino di destinazione
                                ' Dim ProvenienzaPiva As String = Session("ObjparametriAgendaProvenienzaPiva") 'sopra
                                'Session("ObjparametriAgendaProvenienzaSa_Cod")
                                'Session("ObjparametriAgendaProvenienzaVeg_Cod")
                                'ession("ObjparametriAgendaProvenienzaData")
                                Dim ProvenienzaSa_Cod_magazzino As Integer = Session("ObjparametriAgendaProvenienzaSa_Cod_magazzino")
                                Dim ProvenienzaFabbricato_Cod As Integer = Session("ObjparametriAgendaProvenienzafabbricato_Cod")

                                Tipo_Operazione = enum_TipoOperazioneDB.Scrittura
                                Dim udsDescr As String = New AgronicaCoreAnagrafeDAL.Imprese_Read().RagSoc_from_Piva(MovimentiCure(0).Piva, objParametri_Server)
                                udsDescr = udsDescr & " - " & New AgronicaCoreAnagrafeDAL.CentriAziendali_Read().SaNome_from_SaCod(MovimentiCure(0).Piva, MovimentiCure(0).Sa_Cod, objParametri_Server)
                                Dim agendaCarico As Operazione_Agenda = AgronicaCoreModello.OperazioneDiCuraClasse.CreaOggettoAgendaCaricoPressoAziendaOrigine(MovimentiCure, _
                                                                            Provenienza, _
                                                                            ProvenienzaSa_Cod_magazzino, _
                                                                            ProvenienzaFabbricato_Cod, _
                                                                            Id_Agenda_Carico, Tipo_Operazione, udsDescr)

                                If Id_Agenda_Carico <> 0 And objParametriAgenda.Tipo_Operazione = enum_TipoOperazioneDB.Modifica Then
                                    Dim CancellataOperazione As Boolean = False
                                    CancellataOperazione = objAgendaScrivi.Cancella(agendaCarico.Piva,
                                                                                     agendaCarico.Sa_Cod,
                                                                                     Id_Agenda_Carico, False,
                                                                                     objParametri_Server, logCancellazione:=False)
                                End If
                                Id_Agenda_Carico = objAgendaScrivi.Scrivi(agendaCarico, objParametri_Server)




                                '--------------------------------------------------------------------------------------------------------------------------------
                                '--------------SALVO-MODIFICO RIFERIMENTI--------------------------------------------------------------------------
                                '--------------------------------------------------------------------------------------------------------------------------------
                                Dim RifSc As New Movimento_Dettaglio_Riferimento
                                Dim RifCa As New Movimento_Dettaglio_Riferimento
                                RifSc.Piva_Rif = Agenda.Piva
                                RifCa.Piva_Rif = Agenda.Piva
                                RifSc.Sa_Cod_Rif = Agenda.Sa_Cod
                                RifCa.Sa_Cod_Rif = Agenda.Sa_Cod
                                RifSc.Id_Agenda_Rif = Id_Agenda
                                RifCa.Id_Agenda_Rif = Id_Agenda
                                RifSc.Id_Mov_Rif = -1
                                RifCa.Id_Mov_Rif = -1
                                RifSc.Id_Mov_Det_Rif = -1
                                RifCa.Id_Mov_Det_Rif = -1
                                RifSc.Lav_Cod_Rif = Agenda.Lav_Cod
                                RifCa.Lav_Cod_Rif = Agenda.Lav_Cod
                                RifSc.Piva = agendaScarico.Piva
                                RifCa.Piva = agendaCarico.Piva
                                RifSc.Sa_Cod = agendaScarico.Sa_Cod
                                RifCa.Sa_Cod = agendaCarico.Sa_Cod
                                RifSc.Id_Agenda = Id_Agenda_Scarico
                                RifCa.Id_Agenda = Id_Agenda_Carico
                                RifSc.Id_Mov = -1
                                RifCa.Id_Mov = -1
                                RifSc.Id_Mov_Det = -1
                                RifCa.Id_Mov_Det = -1
                                RifSc.Lav_Cod = LAVCOD_SCARICO
                                RifCa.Lav_Cod = LAVCOD_CARICO
                                Dim objRifScrivi As New Agenda_Movimenti_Dettagli_Riferimenti_Helper
                                If Id_Agenda_Carico <> 0 And RifSc.Id_Agenda_Rif <> 0 And
                                    RifCa.Id_Agenda_Rif <> 0 And RifSc.Id_Agenda <> 0 And RifCa.Id_Agenda <> 0 And
                                    objParametriAgenda.Tipo_Operazione = enum_TipoOperazioneDB.Modifica Then


                                    'Cancello intrambi i riferimenti
                                    'Dim CancellataOperazione As Boolean = False
                                    'CancellataOperazione = objRifScrivi.Cancellaxx(RifSc.Piva, _
                                    '                                                 RifSc.Sa_Cod, _
                                    '                                                 RifSc.Id_Agenda, _
                                    '                                                 RifSc.Id_Mov, _
                                    '                                                 RifSc.Id_Mov_Det, _
                                    '                                                 objParametri_Server)
                                    'In teoria non servirebbe perche quyandop cancello
                                    'l'op agenda cancell9o anche i riferimeni,
                                    'ma dato che è la cura che è un riferimento devo farlo forse bo
                                    Dim objMovimentiDettagliRif As New AgronicaCoreContabDAL.Mov_Det_Riferimenti_W
                                    Dim CancellataOperazione As Boolean = objMovimentiDettagliRif.Cancella_byChiaveRif(RifSc.Piva, _
                                                                                     RifSc.Sa_Cod, _
                                                                                     RifSc.Id_Agenda, _
                                                                                     RifSc.Id_Mov, _
                                                                                     RifSc.Id_Mov_Det, _
                                                                                      "", objParametri_Server)

                                End If

                                'scriv i riferimenti
                                objRifScrivi.Scrivi(RifSc, objParametri_Server)
                                objRifScrivi.Scrivi(RifCa, objParametri_Server)


                            End If


                        End If

                    Else

                        'devo cancellare eventuali op collegate,
                        'nel caso uno cambi da cura da raccolta a non,
                        'per ora tralascio
                        If Id_Agenda_Scarico <> 0 Or Id_Agenda_Carico <> 0 Then
                            Throw New Exception("Ancora da gestire cancellazione op collegate se cambio la modalità")
                        End If
                    End If


                    objAgendaScrivi = Nothing

            End Select

            res = True

            'commit transazione
            ConnessioniTransazioni.ChiudiTransazione(1, objParametri_Server)

        Catch ex As Exception

            'commit transazione rollback
            ConnessioniTransazioni.ChiudiTransazione(2, objParametri_Server)

            messaggio_errore = ex.Message
            res = False

        Finally

            'chiudi connessione
            ConnessioniTransazioni.ChiudiConnessione(objParametri_Server)

        End Try

        Return res

    End Function

#End Region


#Region "Creazione oggetto Agenda CURATABACCO"

    Private Function creaOggettiAgendaLavorazione(ByRef messaggioerrore As String) As Operazione_Agenda

        'Creo l'agenda con lav_cod 5004 e un movimento

        'INFORNATURE
        'Creo un movimento per ciascuna riga delle infornatura in gridviewinfornature
        'una riga genera due movimenti
        'uno di scarico da magazzino
        'uno di carico nel Essiccatoio dio tabacco in cura
        'i due movimenti sono legati fra loro tramite il campo...ExtraInt??


        'SFORNATURE
        'Creo un movimento di scarico essiccatoio per ciascuna sfornatura identificata da Id_Infornatura, Data_Fine_Cura, Ora_Fine_Cura,
        'dato che l'ora è nel movimento
        'le prime colonne a sinistra nella gridview colli, attenzione al numero cassoni che cagherà solo il primo e se inserisco
        'altre infornature con stessa idinfornatura, data e ora non considera i cassoni ma vanno in coda alle altre
        '
        'per ciascun movimento di scarico sfornatura c'è un movimento di carico nel/nei magazzini con gli n colli indicati nei dettagli


        Dim DtInfornature As DataTable = Session("DtGridViewInfornature")
        Dim DtSfornature As DataTable = Session("DtGridViewColli")
        'recupero i valori nelle textbox
        Dim HtColli As New Hashtable
        Dim HtPesi As New Hashtable
        For i = 0 To GridViewColli.Rows.Count - 1
            HtColli.Add(GridViewColli.DataKeys(i).Item("Id_Lotto"), CType(GridViewColli.Rows(i).Cells(8).Controls(1), TextBox).Text)
            HtPesi.Add(GridViewColli.DataKeys(i).Item("Id_Lotto"), CType(GridViewColli.Rows(i).Cells(9).Controls(1), TextBox).Text)
        Next
        If Not DtSfornature.Columns.Contains("Collo") Then
            DtSfornature.Columns.Add("Collo", GetType(Integer))
        End If
        If Not DtSfornature.Columns.Contains("Peso") Then
            DtSfornature.Columns.Add("Peso", GetType(String))
        End If

        For i = 0 To DtSfornature.Rows.Count - 1
            If HtColli.Contains(GridViewColli.DataKeys(i).Item("Id_Lotto")) Then
                DtSfornature.Rows(i).Item("Collo") = HtColli(GridViewColli.DataKeys(i).Item("Id_Lotto"))
            End If
            If HtPesi.Contains(GridViewColli.DataKeys(i).Item("Id_Lotto")) Then
                DtSfornature.Rows(i).Item("Peso") = HtPesi(GridViewColli.DataKeys(i).Item("Id_Lotto"))
            End If
        Next


        Dim veg_cod As Integer = objParametriAgenda.Veg_Cod '335 tabacco

        Dim AgendaLottoRacc As String = ""
        If objParametriAgenda.PaginaSitoOrigine = enum_PagineAgenda_2010.Pagina_Raccolta Then
            If objParametriAgenda.Lotto <> "" And objParametriAgenda.Lotto <> "Indefinito" Then
                AgendaLottoRacc = objParametriAgenda.Lotto
            End If
            If objParametriAgenda.Tipo_Operazione = enum_TipoOperazioneDB.Modifica Then
                AgendaLottoRacc = ""
            End If
        End If


        If DtInfornature.Rows.Count = 0 Then
            Throw New Exception("Ci deve essere almeno una infornatura")
        End If


        If DtSfornature.Rows.Count = 0 Then
            If objParametriAgenda.PaginaSitoOrigine = enum_PagineAgenda_2010.Pagina_Raccolta Then
                If objParametriAgenda.Piva <> Session("ObjparametriAgendaProvenienzaPiva") Then
                    If objParametriAgenda.Piva <> Session("ObjparametriAgendaProvenienzaPiva") Then
                        Throw New Exception("Ci deve essere almeno una sfornatura se si proviene da una raccolta di un'altra azienda, altrimenti il prodotto non riesce a ritornare all'azienda di orginine.")
                    End If
                End If
            End If
        End If


        Dim note As String = "Operazione di cura in essiccatoio"
        Dim Agenda As Operazione_Agenda
        Agenda = AgronicaCoreModello.OperazioneDiCuraClasse.CreaOggettoAgenda_CURATABACCO(objParametriAgenda.Id_Agenda, objParametri_Server, _
                                        enum_TipoOperazioneDB.Scrittura, _
                                        Session("ASD_ProgressivoGIAS"), _
                                        DtInfornature, DtSfornature, _
                                        ComboProdottoInLavorazione.SelectedValue, _
                                        ComboProdottoLavorato.SelectedValue, _
                                         note, "", veg_cod, NomeProdottoTabaccoCurato, NomeProdottoTabaccoInCura, _
                                        enum_UnitaMisura.KG, AgendaLottoRacc)



        Return Agenda


    End Function

#End Region


#Region "RipristinoValori CURATABACCO"

    Private Sub RipristinaControlliDaAgenda()


        Dim objAgenda As New Agenda_Operazione_Helper
        Dim Agenda As New Operazione_Agenda
        Agenda = objAgenda.Leggi(objParametriAgenda.Piva, _
                                     CInt(objParametriAgenda.Sa_Cod), _
                                     CInt(objParametriAgenda.Id_Agenda), _
                                     0, _
                                     objParametri_Server)


        If Not IsNothing(Agenda) Then

            RipristinaOperazioneDiCura(Agenda)

            RipristinaDaRiferimenti(Agenda)
        Else
            Throw New Exception("agenda nulla")
        End If


    End Sub

    Private Sub RipristinaDaRiferimenti(ByVal Agenda As Operazione_Agenda)
        Dim x As New AgronicaCoreContabDAL.Mov_Dettagli_Riferimenti_R
        Dim dtrifCarico As DataTable = x.Leggi_Specifica("", 0, 0, 0, 0, LAVCOD_SCARICO, "", _
                                                         objParametriAgenda.Piva, _
                                                         objParametriAgenda.Sa_Cod, _
                                                         objParametriAgenda.Id_Agenda, _
                                                         -1, -1, LAVCOD_CURA, "", _
                                                         "", "", objParametri_Server)
        Dim dtrifScarico As DataTable = x.Leggi_Specifica("", 0, 0, 0, 0, LAVCOD_CARICO, "", _
                                                        objParametriAgenda.Piva, _
                                                        objParametriAgenda.Sa_Cod, _
                                                        objParametriAgenda.Id_Agenda, _
                                                        -1, -1, LAVCOD_CURA, "", _
                                                        "", "", objParametri_Server)

        If dtrifCarico.Rows.Count <> dtrifScarico.Rows.Count Then
            Throw New Exception("Dovrebbe essere uguali")
        End If
        If dtrifCarico.Rows.Count <> 0 Then
            If dtrifCarico.Rows.Count = 1 Then

                'Vengo dalla raccolta quindi ho i carichi e scarichi collegati
                'Dim Id_Agenda_Carico As Integer = dtrifCarico.Rows(0).Item("Id_Agenda")
                'Dim Id_Agenda_Scarico As Integer = dtrifScarico.Rows(0).Item("Id_Agenda")

                'devo ripristinare i valori in sessione
                Dim objAgenda As New Agenda_Operazione_Helper
                Dim AgendaScarico As New Operazione_Agenda
                Dim AgendaCarico As New Operazione_Agenda
                AgendaCarico = objAgenda.Leggi(dtrifCarico.Rows(0).Item("Piva"), _
                                             CInt(dtrifCarico.Rows(0).Item("Sa_Cod")), _
                                             CInt(dtrifCarico.Rows(0).Item("Id_Agenda")), _
                                             0, _
                                             objParametri_Server)
                AgendaScarico = objAgenda.Leggi(dtrifScarico.Rows(0).Item("Piva"), _
                                             CInt(dtrifScarico.Rows(0).Item("Sa_Cod")), _
                                             CInt(dtrifScarico.Rows(0).Item("Id_Agenda")), _
                                             0, _
                                             objParametri_Server)

                If IsNothing(AgendaScarico) Or IsNothing(AgendaCarico) Then
                    Throw New Exception("l'operazione collegata non esiste")
                End If

                DivRitorno.Visible = True

                Session("ObjparametriAgendaProvenienzaPiva") = AgendaScarico.Piva
                Session("ObjparametriAgendaProvenienzaSa_Cod") = AgendaScarico.Sa_Cod
                Session("ObjparametriAgendaProvenienzaVeg_Cod") = objParametriAgenda.Veg_Cod
                Session("ObjparametriAgendaProvenienzaData") = Agenda.Data
                Session("ObjparametriAgendaProvenienzaSa_Cod_magazzino") = AgendaScarico.Movimenti(0).Movimenti_Dettagli(0).Movimenti_Destinazioni(0).Sa_Cod
                Session("ObjparametriAgendaProvenienzafabbricato_Cod") = AgendaScarico.Movimenti(0).Movimenti_Dettagli(0).Movimenti_Destinazioni(0).Id_Destinazione

                objParametriAgenda.PaginaSitoOrigine = enum_PagineAgenda_2010.Pagina_Raccolta

                ComboCentroDestinazioneCure.SelectedValue = AgendaCarico.Piva & "/" & AgendaCarico.Movimenti(0).Movimenti_Dettagli(0).Movimenti_Destinazioni(0).Sa_Cod
                cambiatoCentroDestinazioneCure()
                ComboMagazziniDestinazioneCure.SelectedValue = AgendaCarico.Movimenti(0).Movimenti_Dettagli(0).Movimenti_Destinazioni(0).Id_Destinazione
                TextBoxPivaProv.Text = AgendaCarico.Movimenti(0).Movimenti_Dettagli(0).Lotto.Remove(AgendaScarico.Movimenti(0).Movimenti_Dettagli(0).Lotto.Length - 6, 6)


            Else
                Throw New Exception("Dovrebbe essercene due o nessuno")
            End If
        End If
    End Sub

    Private Sub RipristinaOperazioneDiCura(ByVal Agenda As Operazione_Agenda)
        Dim MovimentiScaricoMagazzino As New List(Of Movimento)
        Dim MovimentiCaricoEssiccatoio As New List(Of Movimento)
        Dim MovimentiScaricoEssiccatoio As New List(Of Movimento)
        Dim MovimentiCaricoMagazzino As New List(Of Movimento)

        objParametriAgenda.Piva = Agenda.Piva
        objParametriAgenda.Sa_Cod = Agenda.Sa_Cod
        objParametriAgenda.Lav_Cod = Agenda.Lav_Cod

        'NOTE
        If Not IsNothing(Agenda.Note) Then
            For i = 0 To Agenda.Note.Count - 1
                objParametriAgenda.Note.Add(Agenda.Note(i))
            Next
        End If
        objParametriAgenda.salva()

        If Agenda.Lav_Cod <> objParametriAgenda.Lav_Cod Then
            Throw New NotImplementedException
        End If
        Select Case CInt(objParametriAgenda.Lav_Cod)
            Case LAVCOD_CURA
            Case Else
                Throw New NotImplementedException
        End Select

        'MOVIMENTI
        If Not IsNothing(Agenda.Movimenti) Then

            For Each movimentoCorrente As Movimento In Agenda.Movimenti

                Select Case movimentoCorrente.Cau_Mov

                    Case enum_Agenda_Causali.LINEA_PRODUZIONE
                        'non c'è nulla da leggere al momento

                    Case enum_Agenda_Causali.SCARICO

                        Select Case movimentoCorrente.Movimenti_Dettagli(0).Movimenti_Destinazioni(0).Tipo
                            Case MAGAZZINO
                                MovimentiScaricoMagazzino.Add(movimentoCorrente)
                            Case ESSICCATOIO
                                MovimentiScaricoEssiccatoio.Add(movimentoCorrente)
                        End Select

                    Case enum_Agenda_Causali.CARICO

                        Select Case movimentoCorrente.Movimenti_Dettagli(0).Movimenti_Destinazioni(0).Tipo
                            Case ESSICCATOIO
                                MovimentiCaricoEssiccatoio.Add(movimentoCorrente)
                            Case MAGAZZINO
                                MovimentiCaricoMagazzino.Add(movimentoCorrente)
                        End Select

                    Case Else
                        Throw New NotImplementedException

                End Select

            Next


        End If


        '................................
        'ho raggruppato i movimenti, ora devo associarli uno ad uno
        'in base all'extraint e ricreate le tabella
        If MovimentiScaricoMagazzino.Count <> MovimentiCaricoEssiccatoio.Count Then
            Throw New Exception("il nomero di movimenti di scarico magazzino e di carico essiccatoio devono essere uguali")
        End If
        Dim DtInfornature As DataTable = InizializzaDtInfornature()
        For Each MovScaricoMagazzino As Movimento In MovimentiScaricoMagazzino
            Dim dr As DataRow = DtInfornature.NewRow
            Dim IdCoppia As Integer = MovScaricoMagazzino.Extra_Int

            'Compilo i valor idel prodottoscaricato (ho un movimento con un dettaglio e una destinazione)
            If MovScaricoMagazzino.Movimenti_Dettagli.Count <> 1 Then
                Throw New Exception("Ci deve essere un solo mov dettaglio MovScaricoMagazzino")
            End If
            If MovScaricoMagazzino.Movimenti_Dettagli(0).Movimenti_Destinazioni.Count <> 1 Then
                Throw New Exception("Ci deve essere un solo mov destinazione MovScaricoMagazzino")
            End If

            For Each MovCaricoEssiccatoio As Movimento In MovimentiCaricoEssiccatoio
                If MovCaricoEssiccatoio.Extra_Int = IdCoppia Then

                    'Compilo i valori del prodotto inserito nell'essiccatoio  (ho un movimento con un dettaglio e una destinazione)
                    If MovCaricoEssiccatoio.Movimenti_Dettagli.Count <> 1 Then
                        Throw New Exception("Ci deve essere un solo mov dettaglio MovCaricoEssiccatoio")
                    End If
                    If MovCaricoEssiccatoio.Movimenti_Dettagli(0).Movimenti_Destinazioni.Count <> 1 Then
                        Throw New Exception("Ci deve essere un solo mov destinazione MovCaricoEssiccatoio")
                    End If

                    dr.Item("Id_Infornatura") = MovCaricoEssiccatoio.Movimenti_Dettagli(0).Lotto
                    dr.Item("Piva") = MovCaricoEssiccatoio.Piva
                    dr.Item("Sa_Cod") = MovCaricoEssiccatoio.Movimenti_Dettagli(0).Movimenti_Destinazioni(0).Sa_Cod
                    dr.Item("Sa_nome") = New AgronicaCoreAnagrafeDAL.CentriAziendali_Read().SaNome_from_SaCod(dr.Item("Piva"), dr.Item("Sa_Cod"), objParametri_Server)
                    dr.Item("Fabbricato_Cod") = MovCaricoEssiccatoio.Movimenti_Dettagli(0).Movimenti_Destinazioni(0).Id_Destinazione
                    dr.Item("Fabbricato_Des") = New AgronicaCoreAnagrafeDAL.Fabbricati_R().FabbricatoDes_from_FabbricatoCod(dr.Item("Piva"), dr.Item("Sa_Cod"), dr.Item("Fabbricato_Cod"), objParametri_Server)
                    dr.Item("Cassoni") = MovCaricoEssiccatoio.Movimenti_Dettagli(0).Qta
                    dr.Item("Data_Inizio_Cura") = MovCaricoEssiccatoio.Data.ToShortDateString
                    dr.Item("Ora_Inizio_Cura") = MovCaricoEssiccatoio.Ora.ToShortTimeString

                    'DATI MAGAZZINO
                    dr.Item("Magazzino_Sa_Cod") = MovScaricoMagazzino.Movimenti_Dettagli(0).Movimenti_Destinazioni(0).Sa_Cod
                    dr.Item("Magazzino_Sa_Nome") = New AgronicaCoreAnagrafeDAL.CentriAziendali_Read().SaNome_from_SaCod(dr.Item("Piva"), dr.Item("Magazzino_Sa_Cod"), objParametri_Server)
                    dr.Item("Magazzino_Fabbricato_Cod") = MovScaricoMagazzino.Movimenti_Dettagli(0).Movimenti_Destinazioni(0).Id_Destinazione
                    dr.Item("Magazzino_Fabbricato_Des") = New AgronicaCoreAnagrafeDAL.Fabbricati_R().FabbricatoDes_from_FabbricatoCod(dr.Item("Piva"), dr.Item("Magazzino_Sa_Cod"), dr.Item("Magazzino_Fabbricato_Cod"), objParametri_Server)
                    dr.Item("Magazzino_Cat_Cod") = MovScaricoMagazzino.Movimenti_Dettagli(0).Elem_Cod
                    dr.Item("Magazzino_Cat_Des") = New AgronicaCoreMetaSchemaDAL.Categorie_Magazzino_R().NomeComune_from_ElemCod(dr.Item("Magazzino_Cat_Cod"), objParametri_Server)
                    dr.Item("Magazzino_Pro_Cod") = MovScaricoMagazzino.Movimenti_Dettagli(0).Pro_Cod
                    dr.Item("Magazzino_Pro_Des") = ""
                    If MovScaricoMagazzino.Movimenti_Dettagli(0).Pro_Cod = 0 Then
                        dr.Item("Magazzino_Pro_Des") = New AgronicaCoreContabDAL.Contabilita_R().LeggiProdotto(objParametri_Server, "", "", MovScaricoMagazzino.Movimenti_Dettagli(0).Elem_Cod, dr.Item("Magazzino_Pro_Cod"), MovScaricoMagazzino.Movimenti_Dettagli(0).Mat_Cod, , , , , , , , False)
                    End If
                    dr.Item("Magazzino_Mat_Cod") = MovScaricoMagazzino.Movimenti_Dettagli(0).Mat_Cod
                    dr.Item("Magazzino_Lotto_Int") = ""
                    If MovScaricoMagazzino.Movimenti_Dettagli(0).Cod_Progetto <> 0 Then
                        dr.Item("Magazzino_Lotto_Int") = New AgronicaCoreAnagrafeDAL.Impresa_Progetti_R().Progetto_Nome_From_Progetto_Cod("", MovScaricoMagazzino.Movimenti_Dettagli(0).Cod_Progetto, objParametri_Server)
                    End If
                    dr.Item("Magazzino_Lotto_Acc") = MovScaricoMagazzino.Movimenti_Dettagli(0).Lotto
                    dr.Item("Magazzino_Cod_Progetto") = MovScaricoMagazzino.Movimenti_Dettagli(0).Cod_Progetto
                    dr.Item("Magazzino_Param_Des") = MovScaricoMagazzino.Movimenti_Dettagli(0)
                    dr.Item("Magazzino_Cal_Cod") = MovScaricoMagazzino.Movimenti_Dettagli(0).Cal_Cod
                    If CInt(dr.Item("Magazzino_Cal_Cod")) < 0 Then
                        dr.Item("Magazzino_Cal_Des") = New AgronicaCoreContabDAL.Materie_Prime_Campionature_R().Leggi_MateriaPrima_Campionature(0, "", 0, "", 0, "", "", 0, -dr.Item("Magazzino_Cal_Cod"), "", 0, 0, 0, "", False, "", "", objParametri_Server)
                    Else
                        dr.Item("Magazzino_Cal_Des") = New AgronicaCoreAnagrafeDAL.Materie_Prime_Calibri_R().Cal_Des_From_Cal_Cod(-dr.Item("Magazzino_Cal_Cod"), objParametri_Server)
                    End If
                    dr.Item("Magazzino_Udm_Cod") = MovScaricoMagazzino.Movimenti_Dettagli(0).Udm_Cod
                    dr.Item("Magazzino_Udm_Des") = New AgronicaCoreMetaSchemaDAL.UnitaMisura_R().UdmDes_from_UdmCod(dr.Item("Magazzino_Udm_Cod"), "", objParametri_Server)
                    dr.Item("Magazzino_Qta") = MovScaricoMagazzino.Movimenti_Dettagli(0).Qta

                End If

            Next

            DtInfornature.Rows.Add(dr)

        Next



        Dim DtSfornature As DataTable = InizializzaDtSfornature()
        For Each MovScaricoEssiccatoio As Movimento In MovimentiScaricoEssiccatoio
            Dim IdCoppia As Integer = MovScaricoEssiccatoio.Extra_Int

            'Compilo i valori del prodottoscaricato dall'essiccatorio (ho un movimento con un dettaglio e una destinazione)
            If MovScaricoEssiccatoio.Movimenti_Dettagli.Count <> 1 Then
                Throw New Exception("Ci deve essere un solo mov dettaglio MovScaricoEssiccatoio")
            End If
            If MovScaricoEssiccatoio.Movimenti_Dettagli(0).Movimenti_Destinazioni.Count <> 1 Then
                Throw New Exception("Ci deve essere un solo mov destinazione MovScaricoEssiccatoio")
            End If

            For Each MovCaricoMagazzino As Movimento In MovimentiCaricoMagazzino

                If MovCaricoMagazzino.Extra_Int = IdCoppia Then
                    'Compilo i valori dei lotti caricati nel magazzino
                    ' Ho sempre un movimento di scarico essiccatoio (sfornatura) associato ad un movimento di carico magazzino
                    ' ma stavolta ho piu dettagli nel  MovCaricoMagazzino, uno per lotto
                    ' quindi la riga la creo per ciascun dettaglio del mov di carico , i valori della sfoprnatura sono uguali per più righe, cambia il lotto

                    For Each MovDettaglioCaricoMagazzino As Movimento_Dettaglio In MovCaricoMagazzino.Movimenti_Dettagli
                        If MovDettaglioCaricoMagazzino.Movimenti_Destinazioni.Count <> 1 Then
                            Throw New Exception("Ci deve essere un solo mov destinazione MovCaricoMagazzino")
                        End If
                        Dim dr As DataRow = DtSfornature.NewRow

                        dr.Item("Id_Infornatura") = MovScaricoEssiccatoio.Movimenti_Dettagli(0).Lotto
                        dr.Item("Piva") = MovScaricoEssiccatoio.Piva

                        dr.Item("Cassoni") = MovScaricoEssiccatoio.Movimenti_Dettagli(0).Qta

                        dr.Item("Data_Fine_Cura") = MovScaricoEssiccatoio.Data.ToShortDateString
                        dr.Item("Ora_Fine_Cura") = MovScaricoEssiccatoio.Ora.ToShortTimeString

                        dr.Item("Id_Lotto") = MovDettaglioCaricoMagazzino.Extra_Str
                        dr.Item("ColloTemp") = MovDettaglioCaricoMagazzino.Lotto

                        dr.Item("Magazzino_Sa_Cod") = MovDettaglioCaricoMagazzino.Movimenti_Destinazioni(0).Sa_Cod
                        dr.Item("Magazzino_Sa_Nome") = New AgronicaCoreAnagrafeDAL.CentriAziendali_Read().SaNome_from_SaCod(dr.Item("Piva"), dr.Item("Magazzino_Sa_Cod"), objParametri_Server)
                        dr.Item("Magazzino_Fabbricato_Cod") = MovDettaglioCaricoMagazzino.Movimenti_Destinazioni(0).Id_Destinazione
                        dr.Item("Magazzino_Fabbricato_Des") = New AgronicaCoreAnagrafeDAL.Fabbricati_R().FabbricatoDes_from_FabbricatoCod(dr.Item("Piva"), dr.Item("Magazzino_Sa_Cod"), dr.Item("Magazzino_Fabbricato_Cod"), objParametri_Server)


                        dr.Item("Collo") = MovDettaglioCaricoMagazzino.Lotto
                        dr.Item("Peso") = MovDettaglioCaricoMagazzino.Qta

                        DtSfornature.Rows.Add(dr)

                    Next

                End If
            Next

        Next



        Finalizza_Infornatured(DtInfornature)
        finalizzaSfornatura(DtSfornature, False)

        Dim CentroCura As Integer = MovimentiCaricoEssiccatoio(0).Movimenti_Dettagli(0).Movimenti_Destinazioni(0).Sa_Cod
        Dim EssiccatoioCura As Integer = MovimentiCaricoEssiccatoio(0).Movimenti_Dettagli(0).Movimenti_Destinazioni(0).Id_Destinazione
        Dim Datainiz As Date = MovimentiCaricoEssiccatoio(0).Data
        Dim Orainiz As DateTime = MovimentiCaricoEssiccatoio(0).Ora.ToShortTimeString
        Dim ProdottoIngresso As Integer = MovimentiCaricoEssiccatoio(0).Movimenti_Dettagli(0).Mat_Cod

        Dim CentroDest As Integer = 0
        Dim MagazzDest As Integer = 0
        Dim ProdottoUscita As Integer = 0

        If MovimentiCaricoMagazzino.Count > 0 Then
            CentroDest = MovimentiCaricoMagazzino(0).Movimenti_Dettagli(0).Movimenti_Destinazioni(0).Sa_Cod
            MagazzDest = MovimentiCaricoMagazzino(0).Movimenti_Dettagli(0).Movimenti_Destinazioni(0).Id_Destinazione
            ProdottoUscita = MovimentiCaricoMagazzino(0).Movimenti_Dettagli(0).Mat_Cod
        End If

        Dim DataFin As Date = Datainiz
        Dim OraFin As DateTime = Orainiz
        If MovimentiScaricoEssiccatoio.Count > 0 Then
            DataFin = MovimentiScaricoEssiccatoio(0).Data
            OraFin = MovimentiScaricoEssiccatoio(0).Ora.ToShortTimeString
        End If

        Dim veg As Integer
        Dim cul As Integer
        Dim o As New AgronicaCoreAnagrafeDAL.Materie_Prime_R()
        o.VegCod_CulCod_from_MatCod("", TRASFORMATI_VEGETALI, ProdottoUscita, veg, cul, objParametri_Server)
        ComboSpecie.ddl_Specie.SelectedValue = veg
        ComboSpecie.ddl_Specie.SelectedIndex = ComboSpecie.ddl_Specie.Items.IndexOf(ComboSpecie.ddl_Specie.Items.FindByValue(veg))
        CambioSpecie()

        ComboCentroCura.SelectedValue = CentroCura
        caricaComboEssiccatoio()
        ComboEssiccatoio.SelectedValue = EssiccatoioCura
        OraFineCura.Text = OraFin.ToShortTimeString
        OraInizioCura.Text = Orainiz.ToShortTimeString
        TextBoxDataFineCura.Text = DataFin.ToShortDateString
        TextBoxDataInizioCura.Text = Datainiz.ToShortDateString
        ComboProdottoInLavorazione.SelectedValue = ProdottoIngresso
        ComboProdottoLavorato.SelectedValue = ProdottoUscita
        ComboCentroDestinazioneCure.SelectedValue = CentroDest
        cambiatoCentroDestinazioneCure()
        ComboMagazziniDestinazioneCure.SelectedValue = MagazzDest
    End Sub

#End Region




    Protected Sub ImageButtonRintraccia_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ImageButtonRintraccia.Click
        Dim scr As String = AgronicaCoreGestioneRichieste.RedirectGestione.PreparaScripPerPopup("RintracciaLotto.aspx", "Rinraccia")
        ScriptManager.RegisterStartupScript(UpdatePanelPerScript, UpdatePanelOperazione.GetType(),
                                  String.Format("jQuery_{0}", ImageButtonRintraccia.ClientID), scr, False)
    End Sub

End Class







'postata in agronicacoremodello perche usata anche da sincronizzatore, importatore cure
'Public Class OperazioneDiCuraClasse

'    'Cura del tabacco(lav cod 5004) , simile alla 2 trasformazione(lav cod 5000) del lan
'    '
'    'Movimento 10001
'    '
'    'Scarico del prodotto x (tabacco raccolto) dal magazzino  
'    '               (un movimento per un prodotto, ma non escludo la possibilità di gestirne di più)
'    '                                                                                1 mov scarico        CAU_SCARICO As String = "7350" alla data inizio cura
'    '                                                                                1 dettaglio        di QtaTot
'    '
'    'Carico  del prodotto y (tabacco  in cura) nel fabbricato essiccatoio 222 
'    '               (un movimento per ciascun cassone, o uso il collo?? devo usare la stessa cosa che poi scarico 
'    '               altrimenti le giacenze non tornano e usare lo stesso lotto, con il lotto indico il 
'    '               cassone occupato, così posso controlare con giacenze i cassoni presenti, quindi occupati in una data
'    '                                                                               1 mov carico         CAU_CARICO As String = "7350" alla data inizio cura
'    '                                                                                n dettagli+dest (una per ciascun Essiccatoio) di QtaTot/n
'    '
'    'POSSO raggruppare scarico fEssiccatoio e carico magazzino dove indico lotto nello stesso movimento
'    'oppure posso infilare il codice del collo in un altro campo nello scarico Essiccatoio
'    'Scarico del prodotto y (tabacco  in cura) dal fabbricato essiccatoro 222        
'    '              scarico gli stessi lotti caricati perche toprnino giacenze
'    '
'    'Carico  del prodotto z (tabacco   curato) nel magazzino   
'    '              carico n colli(lotti), suddivisi in m forni 
'    '                                                                                 nXm mov carico (perche possono variare le date fine e l'ora e fanno fede quelle del movimento)          CAU_CARICO As String = "7350" alla data inizio cura
'    '                                                                                nXm dettagli+dest (n forni m colliEssiccatoio) 1 lotto per collo
'    '
'    Public Shared Function CreaOggettoAgenda_CURATABACCO( _
'                        ByVal ID_Agenda As Integer, _
'                        ByVal objParametri_Server As AgronicaCoreParametri, _
'                        ByVal TipoOperazione As Integer, _
'                        ByVal ASG_ProgressivoGIAS As Integer, _
'                        ByVal DtInfornature As DataTable, _
'                        ByVal DtSfornature As DataTable, _
'                        ByVal Mat_Cod_Tabacco_InCura As Integer, ByVal Mat_Cod_Tabacco_Curato As Integer, _
'                        ByVal Nota As String, ByRef messaggio_errore As String, _
'                        ByVal Veg_Cod As Integer, _
'                        ByVal NomeProdottoCurato As String, _
'                        ByVal NomeProdottoInEssiccatoio As String, _
'                        ByVal UdmCaricoMagazzino As enum_UnitaMisura, _
'                        ByVal AgendaLottoRacc As String) As Operazione_Agenda


'        'Attenzione, oora funziona con prodotto in Essiccatoio e curato come TRASFORMATI_VEGETALI
'        'veg_cod=335

'        'Anche se arrivano gia ordinati li riordino per sicurezza, altrimento non funzia nulla
'        Dim dw As DataView = DtInfornature.DefaultView
'        dw.Sort = "Id_Infornatura, Sa_nome, Fabbricato_Des, Data_Inizio_Cura, Ora_Inizio_Cura "
'        DtInfornature = dw.ToTable
'        Dim dw2 As DataView = DtSfornature.DefaultView
'        dw2.Sort = "Id_Infornatura, Data_Fine_Cura, Ora_Fine_Cura, Magazzino_Sa_Cod, Magazzino_Fabbricato_Cod, Id_Lotto"
'        DtSfornature = dw2.ToTable


'        '------------------------------------------------------------------
'        'Controlli vari
'        If DtInfornature.Rows.Count = 0 Then
'            Throw New Exception("Deve esserci almeno un prodotto di DtInfornature")
'        End If

'        Dim DataOperazione As Date = DtInfornature.Rows(0).Item("Data_Inizio_Cura")
'        Dim PivaOperazione As String = DtInfornature.Rows(0).Item("Piva")
'        Dim Sa_CodOperazione As Integer = DtInfornature.Rows(0).Item("Sa_Cod")
'        Dim OraOperazione As String = DtInfornature.Rows(0).Item("Ora_Inizio_Cura")
'        For i = 0 To DtInfornature.Rows.Count - 1
'            If DtInfornature.Rows(i).Item("Data_Inizio_Cura") <> DataOperazione Then
'                Throw New Exception("Deve esserci la stessa data nelle indornature")
'            End If
'            If DtInfornature.Rows(i).Item("Piva") <> PivaOperazione Then
'                Throw New Exception("Deve esserci la stessa piva nelle infornature")
'            End If
'            If DtInfornature.Rows(i).Item("Sa_Cod") <> Sa_CodOperazione Then
'                Throw New Exception("Deve esserci lo stesso centro nelle infornature")
'            End If
'            If DtInfornature.Rows(i).Item("Ora_Inizio_Cura") <> OraOperazione Then
'                Throw New Exception("Deve esserci la stessa ora nelle infornature")
'            End If
'        Next
'        For i = 0 To DtSfornature.Rows.Count - 1

'        Next



'        'GENERO PRODOTTO SE NON INDICATO (per ora solo sulla specie, varietà altre)
'        Dim Mat_Des_Ritorno As String = ""
'        Dim Mat_Des_Ritorno2 As String = ""
'        If Mat_Cod_Tabacco_Curato = -1 Then
'            Mat_Cod_Tabacco_Curato = GeneraProdottoCuraTabacco(objParametri_Server, ASG_ProgressivoGIAS, NomeProdottoCurato, "CURATO/", Veg_Cod)
'        Else
'            Mat_Des_Ritorno = New AgronicaCoreAnagrafeDAL.Materie_Prime_R().MatDes_from_MatCod("", TRASFORMATI_VEGETALI, Mat_Cod_Tabacco_Curato, "", "", "", objParametri_Server)
'        End If

'        If Mat_Cod_Tabacco_InCura = -1 Then
'            Mat_Cod_Tabacco_InCura = GeneraProdottoCuraTabacco(objParametri_Server, ASG_ProgressivoGIAS, NomeProdottoInEssiccatoio, "INCURA/", Veg_Cod)
'        Else
'            Mat_Des_Ritorno2 = New AgronicaCoreAnagrafeDAL.Materie_Prime_R().MatDes_from_MatCod("", TRASFORMATI_VEGETALI, Mat_Cod_Tabacco_InCura, "", "", "", objParametri_Server)
'        End If

'        Dim BaseCode As Integer
'        Dim TopCode As Integer
'        Call Calcola_BaseCode_TopCode(BaseCode, _
'                              TopCode, _
'                             ASG_ProgressivoGIAS)
'        Dim Lav_Cod As Integer = LAVCOD_CURA
'        Dim UdmEssiccatoi As Integer = enum_UnitaMisura.Numero
'        '------------------------------------------------------------------
'        'Creo l'agenda con lav_cod 5004 e un movimento
'        '------------------------------------------------------------------

'        Dim Agenda As Operazione_Agenda
'        Agenda = New Operazione_Agenda

'        Agenda.Tipo_Operazione = TipoOperazione
'        Agenda.Id_Agenda = ID_Agenda
'        Agenda.Data = DataOperazione
'        Agenda.Piva = PivaOperazione
'        Agenda.Sa_Cod = Sa_CodOperazione
'        Agenda.Lav_Cod = Lav_Cod
'        Agenda.Des_Lib = "Operazione di Cura"
'        If AgendaLottoRacc <> "" Then
'            Agenda.Des_Lib &= " (Lotto Raccolto: " & AgendaLottoRacc & ")"
'        End If
'        Agenda.BaseCode = BaseCode
'        Agenda.TopCode = TopCode
'        ''---------------MOVIMENTO CAU_LINEA_PRODUZIONE 10001 come trasformazione 
'        Dim Movimento_OperazioneColturale As New Movimento
'        Movimento_OperazioneColturale.Id_Agenda = Agenda.Id_Agenda
'        Movimento_OperazioneColturale.Piva = Agenda.Piva
'        Movimento_OperazioneColturale.Sa_Cod = Agenda.Sa_Cod
'        Movimento_OperazioneColturale.Lav_Cod = Lav_Cod
'        Movimento_OperazioneColturale.Cau_Mov = CAU_LINEA_PRODUZIONE
'        Movimento_OperazioneColturale.Mov_Desc = Nota
'        Movimento_OperazioneColturale.Data = Agenda.Data
'        Movimento_OperazioneColturale.BaseCode = Agenda.BaseCode
'        Movimento_OperazioneColturale.TopCode = Agenda.TopCode
'        Agenda.Movimenti.Add(Movimento_OperazioneColturale)


'        Dim seq As New AgronicaCoreDataProvider.Agro_Sequenze

'        '------------------------------------------------------------------
'        'INFORNATURE
'        'Creo 2 moviment1 per ciascuna riga delle infornatura in gridviewinfornature
'        'ne basterebbero due, uno per scarichi e uno per carichi ma meglio uniforrmare per gestire meglio collegamenti e rintracciata
'        'una riga genera due movimenti
'        'uno di scarico da magazzino
'        'uno di carico nel Essiccatoio di tabacco in cura
'        'i due movimenti sono legati fra loro tramite il campo...ExtraInt??
'        '------------------------------------------------------------------

'        'Attenzione, il movimento di carico/scarico ha il sa_cod della destinazione del carico/scarico
'        'quindi del Essiccatoio e del magazzino

'        For Each infornatura As DataRow In DtInfornature.Rows
'            Dim Id_Infornatura As String = infornatura.Item("Id_Infornatura")
'            Dim Piva As String = infornatura.Item("Piva")
'            Dim Sa_Cod As String = infornatura.Item("Sa_Cod")
'            Dim Fabbricato_Cod As String = infornatura.Item("Fabbricato_Cod")
'            Dim Fabbricato_Des As String = infornatura.Item("Fabbricato_Des")
'            Dim Cassoni As String = infornatura.Item("Cassoni")
'            Dim Data_Inizio_Cura As String = infornatura.Item("Data_Inizio_Cura")
'            Dim Ora_Inizio_Cura As DateTime = infornatura.Item("Ora_Inizio_Cura")
'            'magazzino
'            Dim Magazzino_Sa_Cod As String = infornatura.Item("Magazzino_Sa_Cod")
'            Dim Magazzino_Sa_Nome As String = infornatura.Item("Magazzino_Sa_Nome")
'            Dim Magazzino_Fabbricato_Cod As String = infornatura.Item("Magazzino_Fabbricato_Cod")
'            Dim Magazzino_Fabbricato_Des As String = infornatura.Item("Magazzino_Fabbricato_Des")
'            Dim Magazzino_Cat_Cod As String = infornatura.Item("Magazzino_Cat_Cod")
'            Dim Magazzino_Cat_Des As String = infornatura.Item("Magazzino_Cat_Des")
'            Dim Magazzino_Pro_Cod As String = infornatura.Item("Magazzino_Pro_Cod")
'            Dim Magazzino_Pro_Des As String = infornatura.Item("Magazzino_Pro_Des")
'            Dim Magazzino_Mat_Cod As String = infornatura.Item("Magazzino_Mat_Cod")
'            Dim Magazzino_Lotto_Int As String = infornatura.Item("Magazzino_Lotto_Int")
'            Dim Magazzino_Lotto_Acc As String = infornatura.Item("Magazzino_Lotto_Acc")
'            Dim Magazzino_Cod_Progetto As String = infornatura.Item("Magazzino_Cod_Progetto")
'            Dim Magazzino_Param_Des As String = infornatura.Item("Magazzino_Param_Des")
'            Dim Magazzino_Cal_Cod As String = infornatura.Item("Magazzino_Cal_Cod")
'            Dim Magazzino_Cal_Des As String = infornatura.Item("Magazzino_Cal_Des")
'            Dim Magazzino_Udm_Cod As String = infornatura.Item("Magazzino_Udm_Cod")
'            Dim Magazzino_Udm_Des As String = infornatura.Item("Magazzino_Udm_Des")
'            Dim Magazzino_Qta As String = infornatura.Item("Magazzino_Qta")


'            'genero l'id per associare i movimenti di scarico e carico per infornatura 
'            'e scarico da forno e carico magaz in forno per sfornatura da mettere nell'extraint
'            Dim idSC As Integer = seq.NuovoId_Tabella("GeneraIdOperazioneDiCuraMovInfornatura", 0, 2000000000, objParametri_Server)


'            '------------------------------------------------------------------
'            'Movimento scarico magazzino
'            Dim Movimento_Scarico_Magazzino As New Movimento
'            Movimento_Scarico_Magazzino.Id_Agenda = Agenda.Id_Agenda
'            Movimento_Scarico_Magazzino.Piva = Agenda.Piva
'            Movimento_Scarico_Magazzino.Sa_Cod = Magazzino_Sa_Cod
'            Movimento_Scarico_Magazzino.Data = Data_Inizio_Cura
'            Movimento_Scarico_Magazzino.Ora = Ora_Inizio_Cura
'            Movimento_Scarico_Magazzino.Lav_Cod = Lav_Cod
'            Movimento_Scarico_Magazzino.Cau_Mov = CAU_SCARICO
'            Movimento_Scarico_Magazzino.Mov_Desc = "Scarico di Magazzino Per Cura"
'            Movimento_Scarico_Magazzino.BaseCode = Agenda.BaseCode
'            Movimento_Scarico_Magazzino.TopCode = Agenda.TopCode

'            Movimento_Scarico_Magazzino.Extra_Int = idSC

'            Dim Movimento_Dettaglio_Scarico_Magazzino As New Movimento_Dettaglio
'            Movimento_Dettaglio_Scarico_Magazzino.Id_Agenda = Agenda.Id_Agenda
'            Movimento_Dettaglio_Scarico_Magazzino.Piva = Agenda.Piva
'            Movimento_Dettaglio_Scarico_Magazzino.Sa_Cod = Movimento_Scarico_Magazzino.Sa_Cod
'            Movimento_Dettaglio_Scarico_Magazzino.Data = Movimento_Scarico_Magazzino.Data
'            Movimento_Dettaglio_Scarico_Magazzino.Elem_Cod = Magazzino_Cat_Cod
'            Movimento_Dettaglio_Scarico_Magazzino.Pro_Cod = Magazzino_Pro_Cod
'            Movimento_Dettaglio_Scarico_Magazzino.Mat_Cod = Magazzino_Mat_Cod
'            Movimento_Dettaglio_Scarico_Magazzino.Mov_det_des = "Scarico di Magazzino Per Cura"
'            Movimento_Dettaglio_Scarico_Magazzino.Udm_Cod = Magazzino_Udm_Cod
'            Movimento_Dettaglio_Scarico_Magazzino.Qta = Magazzino_Qta
'            Movimento_Dettaglio_Scarico_Magazzino.Cal_Cod = Magazzino_Cal_Cod
'            Movimento_Dettaglio_Scarico_Magazzino.Lotto = Magazzino_Lotto_Acc
'            Movimento_Dettaglio_Scarico_Magazzino.Cod_Progetto = Magazzino_Cod_Progetto
'            Movimento_Dettaglio_Scarico_Magazzino.Contabilizzato = NONCONTABILE
'            Movimento_Dettaglio_Scarico_Magazzino.Pendente = enum_Pendenza.MovGiustificato
'            Movimento_Dettaglio_Scarico_Magazzino.Lav_Cod = Lav_Cod
'            Movimento_Dettaglio_Scarico_Magazzino.Cau_Mov = Movimento_Scarico_Magazzino.Cau_Mov
'            Movimento_Dettaglio_Scarico_Magazzino.Anno = 1900

'            Dim Movimento_Destinazione_Scarico_Magazzino As New Movimento_Destinazione
'            Movimento_Destinazione_Scarico_Magazzino.Id_Agenda = Agenda.Id_Agenda
'            Movimento_Destinazione_Scarico_Magazzino.Piva = Agenda.Piva
'            Movimento_Destinazione_Scarico_Magazzino.Sa_Cod = Movimento_Scarico_Magazzino.Sa_Cod
'            Movimento_Destinazione_Scarico_Magazzino.Id_Destinazione = Magazzino_Fabbricato_Cod
'            Movimento_Destinazione_Scarico_Magazzino.Data = Movimento_Scarico_Magazzino.Data
'            Movimento_Destinazione_Scarico_Magazzino.Tipo = MAGAZZINO
'            Movimento_Destinazione_Scarico_Magazzino.Qta = Movimento_Dettaglio_Scarico_Magazzino.Qta
'            Movimento_Destinazione_Scarico_Magazzino.BaseCode = Agenda.BaseCode
'            Movimento_Destinazione_Scarico_Magazzino.TopCode = Agenda.TopCode

'            Movimento_Dettaglio_Scarico_Magazzino.Movimenti_Destinazioni.Add(Movimento_Destinazione_Scarico_Magazzino)
'            Movimento_Scarico_Magazzino.Movimenti_Dettagli.Add(Movimento_Dettaglio_Scarico_Magazzino)
'            Agenda.Movimenti.Add(Movimento_Scarico_Magazzino)

'            '------------------------------------------------------------------
'            'Movimento Carico Essiccatoio
'            Dim Movimento_Carico_Essiccatoio As New Movimento
'            Movimento_Carico_Essiccatoio.Id_Agenda = Agenda.Id_Agenda
'            Movimento_Carico_Essiccatoio.Piva = Agenda.Piva
'            Movimento_Carico_Essiccatoio.Sa_Cod = Agenda.Sa_Cod 'attenzione, sa cod del centro non del magazzino
'            Movimento_Carico_Essiccatoio.Data = Data_Inizio_Cura
'            Movimento_Carico_Essiccatoio.Ora = Ora_Inizio_Cura
'            Movimento_Carico_Essiccatoio.Lav_Cod = Lav_Cod
'            Movimento_Carico_Essiccatoio.Cau_Mov = CAU_CARICO
'            Movimento_Carico_Essiccatoio.Mov_Desc = "Carico Essiccatoio Per Cura"
'            Movimento_Carico_Essiccatoio.BaseCode = Agenda.BaseCode
'            Movimento_Carico_Essiccatoio.TopCode = Agenda.TopCode

'            Movimento_Carico_Essiccatoio.Extra_Int = idSC

'            Dim Movimento_Dettaglio_Carico_Essiccatoio As New Movimento_Dettaglio
'            Movimento_Dettaglio_Carico_Essiccatoio.Id_Agenda = Agenda.Id_Agenda
'            Movimento_Dettaglio_Carico_Essiccatoio.Piva = Agenda.Piva
'            Movimento_Dettaglio_Carico_Essiccatoio.Sa_Cod = Agenda.Sa_Cod
'            Movimento_Dettaglio_Carico_Essiccatoio.Data = Movimento_Carico_Essiccatoio.Data
'            Movimento_Dettaglio_Carico_Essiccatoio.Elem_Cod = TRASFORMATI_VEGETALI
'            Movimento_Dettaglio_Carico_Essiccatoio.Pro_Cod = 0
'            Movimento_Dettaglio_Carico_Essiccatoio.Mat_Cod = Mat_Cod_Tabacco_InCura
'            Movimento_Dettaglio_Carico_Essiccatoio.Mov_det_des = "Carico Essiccatoio Per Cura"
'            Movimento_Dettaglio_Carico_Essiccatoio.Udm_Cod = UdmEssiccatoi
'            Movimento_Dettaglio_Carico_Essiccatoio.Qta = Cassoni
'            Movimento_Dettaglio_Carico_Essiccatoio.Cal_Cod = 0
'            Movimento_Dettaglio_Carico_Essiccatoio.Lotto = Id_Infornatura
'            Movimento_Dettaglio_Carico_Essiccatoio.Cod_Progetto = 0
'            Movimento_Dettaglio_Carico_Essiccatoio.Contabilizzato = NONCONTABILE
'            Movimento_Dettaglio_Carico_Essiccatoio.Pendente = enum_Pendenza.MovGiustificato
'            Movimento_Dettaglio_Carico_Essiccatoio.Lav_Cod = Lav_Cod
'            Movimento_Dettaglio_Carico_Essiccatoio.Cau_Mov = Movimento_Carico_Essiccatoio.Cau_Mov
'            Movimento_Dettaglio_Carico_Essiccatoio.Anno = 1900

'            Dim Movimento_Destinazione_Carico_Essiccatoio As New Movimento_Destinazione
'            Movimento_Destinazione_Carico_Essiccatoio.Id_Agenda = Agenda.Id_Agenda
'            Movimento_Destinazione_Carico_Essiccatoio.Piva = Agenda.Piva
'            Movimento_Destinazione_Carico_Essiccatoio.Sa_Cod = Agenda.Sa_Cod
'            Movimento_Destinazione_Carico_Essiccatoio.Id_Destinazione = Fabbricato_Cod
'            Movimento_Destinazione_Carico_Essiccatoio.Data = Movimento_Carico_Essiccatoio.Data
'            Movimento_Destinazione_Carico_Essiccatoio.Tipo = ESSICCATOIO
'            Movimento_Destinazione_Carico_Essiccatoio.Qta = Movimento_Dettaglio_Carico_Essiccatoio.Qta
'            Movimento_Destinazione_Carico_Essiccatoio.BaseCode = Agenda.BaseCode
'            Movimento_Destinazione_Carico_Essiccatoio.TopCode = Agenda.TopCode

'            Movimento_Dettaglio_Carico_Essiccatoio.Movimenti_Destinazioni.Add(Movimento_Destinazione_Carico_Essiccatoio)
'            Movimento_Carico_Essiccatoio.Movimenti_Dettagli.Add(Movimento_Dettaglio_Carico_Essiccatoio)
'            Agenda.Movimenti.Add(Movimento_Carico_Essiccatoio)

'        Next


'        '------------------------------------------------------------------
'        'SFORNATURE
'        'Creo un movimento di scarico essiccatoio per ciascuna sfornatura identificata da Id_Infornatura, Data_Fine_Cura, Ora_Fine_Cura,
'        'dato che l'ora è nel movimento
'        'le prime colonne a sinistra nella gridview colli, attenzione al numero cassoni che cagherà solo il primo e se inserisco
'        'altre infornature con stessa idinfornatura, data e ora non considera i cassoni ma vanno in coda alle altre
'        '
'        'per ciascun movimento di scarico sfornatura c'è un movimento di carico nel/nei magazzini con gli n colli indicati nei dettagli
'        ''------------------------------------------------------------------
'        Dim NewSfornatura As Boolean
'        Dim count As Integer = 0
'        Dim Id_SfornaturaCorr As String = ""
'        Dim Id_SfornaturaOld As String = ""
'        Dim Movimento_Carico_Magazzino As Movimento
'        For Each sfornatura As DataRow In DtSfornature.Rows
'            Dim Id_Infornatura As String = sfornatura.Item("Id_Infornatura")
'            Dim Piva As String = sfornatura.Item("Piva")
'            Dim Cassoni As Integer = sfornatura.Item("Cassoni")
'            Dim Data_Fine_Cura As Date = sfornatura.Item("Data_Fine_Cura")
'            Dim Ora_Fine_Cura As DateTime = sfornatura.Item("Ora_Fine_Cura")
'            Dim Id_Lotto As String = sfornatura.Item("Id_Lotto")
'            Dim Magazzino_Sa_Cod As String = sfornatura.Item("Magazzino_Sa_Cod")
'            Dim Magazzino_Sa_Nome As String = sfornatura.Item("Magazzino_Sa_Nome")
'            Dim Magazzino_Fabbricato_Cod As String = sfornatura.Item("Magazzino_Fabbricato_Cod")
'            Dim Magazzino_Fabbricato_Des As String = sfornatura.Item("Magazzino_Fabbricato_Des")
'            Dim Peso As Decimal = sfornatura.Item("Peso")
'            Dim Collo As String = sfornatura.Item("Collo")

'            Dim Sa_Cod_Rif As Integer
'            Dim Fabbricato_Cod_Rif As Integer


'            'cerco l'infornatura di riferimento, potrei anche inserire il centro e il Essiccatoio nella tabella delle sfornature
'            Dim trovata = False
'            For Each infornatura As DataRow In DtInfornature.Rows
'                Dim Id_Infornatura_Rif As String = infornatura.Item("Id_Infornatura")
'                Dim Piva_Rif As String = infornatura.Item("Piva")
'                Sa_Cod_Rif = infornatura.Item("Sa_Cod")
'                Fabbricato_Cod_Rif = infornatura.Item("Fabbricato_Cod")
'                Dim Fabbricato_Des_Rif As String = infornatura.Item("Fabbricato_Des")
'                Dim Cassoni_Rif As Integer = infornatura.Item("Cassoni")
'                Dim Data_Inizio_Cura_Rif As Date = infornatura.Item("Data_Inizio_Cura")
'                Dim Ora_Inizio_Cura_Rif As DateTime = infornatura.Item("Ora_Inizio_Cura")
'                If Id_Infornatura_Rif = Id_Infornatura Then
'                    'ok trovata
'                    If Piva <> Piva_Rif Then
'                        Throw New Exception("le piva non sono uguali")
'                    End If
'                    If Agenda.Sa_Cod <> Sa_Cod_Rif Then
'                        Throw New Exception("Sa_Cod non sono uguali")
'                    End If
'                    If DateDiff(DateInterval.Day, Data_Inizio_Cura_Rif, Data_Fine_Cura) < 0 Then
'                        Throw New Exception("La data di sfornatura è precedente all'infornatura")
'                    End If
'                    If Cassoni > Cassoni_Rif Then
'                        Throw New Exception("Sono stati sfornati piu cassoni di quanti sono infornati")
'                    End If
'                    trovata = True
'                    Exit For
'                End If
'            Next

'            If Not trovata Then
'                Throw New Exception("Non ho trovato l'infornatura")
'            End If

'            'chiave dela sfornatura
'            Id_SfornaturaCorr = Id_Infornatura & "-" & Data_Fine_Cura & "-" & Ora_Fine_Cura & "-" & Magazzino_Sa_Cod & "-" & Magazzino_Fabbricato_Cod & ""
'            If Id_SfornaturaOld = "" Then
'                Id_SfornaturaOld = Id_SfornaturaCorr
'                NewSfornatura = True
'            Else
'                If Id_SfornaturaOld = Id_SfornaturaCorr Then
'                    NewSfornatura = False
'                Else
'                    Id_SfornaturaOld = Id_SfornaturaCorr
'                    NewSfornatura = True
'                End If
'            End If

'            'Un movimento di scarico per ciascun codice infornatura
'            If NewSfornatura Then


'                'genero l'id per associare i movimenti di scarico e carico per infornatura 
'                'e scarico da forno e carico magaz in forno per sfornatura da mettere nell'extraint
'                Dim idSC As Integer = seq.NuovoId_Tabella("GeneraIdOperazioneDiCuraMovSfornatura",

'                Dim Movimento_Scarico_Essiccatoio As New Movimento
'                Movimento_Scarico_Essiccatoio.Id_Agenda = Agenda.Id_Agenda
'                Movimento_Scarico_Essiccatoio.Piva = Agenda.Piva
'                Movimento_Scarico_Essiccatoio.Sa_Cod = Sa_Cod_Rif
'                Movimento_Scarico_Essiccatoio.Data = Data_Fine_Cura.ToShortDateString
'                Movimento_Scarico_Essiccatoio.Ora = Ora_Fine_Cura
'                Movimento_Scarico_Essiccatoio.Lav_Cod = Lav_Cod
'                Movimento_Scarico_Essiccatoio.Cau_Mov = CAU_SCARICO
'                Movimento_Scarico_Essiccatoio.Mov_Desc = "Scarico da Essiccatoio Post Cura"
'                Movimento_Scarico_Essiccatoio.BaseCode = Agenda.BaseCode
'                Movimento_Scarico_Essiccatoio.TopCode = Agenda.TopCode

'                Movimento_Scarico_Essiccatoio.Extra_Int = idSC 'necessario per rintracciata

'                Dim Movimento_Dettaglio_Scarico_Essiccatoio As New Movimento_Dettaglio
'                Movimento_Dettaglio_Scarico_Essiccatoio.Id_Agenda = Agenda.Id_Agenda
'                Movimento_Dettaglio_Scarico_Essiccatoio.Piva = Agenda.Piva
'                Movimento_Dettaglio_Scarico_Essiccatoio.Sa_Cod = Sa_Cod_Rif
'                Movimento_Dettaglio_Scarico_Essiccatoio.Data = Data_Fine_Cura
'                Movimento_Dettaglio_Scarico_Essiccatoio.Elem_Cod = TRASFORMATI_VEGETALI
'                Movimento_Dettaglio_Scarico_Essiccatoio.Pro_Cod = 0
'                Movimento_Dettaglio_Scarico_Essiccatoio.Mat_Cod = Mat_Cod_Tabacco_InCura
'                Movimento_Dettaglio_Scarico_Essiccatoio.Mov_det_des = "Scarico da Essiccatoio Post Cura"
'                Movimento_Dettaglio_Scarico_Essiccatoio.Udm_Cod = enum_UnitaMisura.KG
'                Movimento_Dettaglio_Scarico_Essiccatoio.Qta = Cassoni
'                Movimento_Dettaglio_Scarico_Essiccatoio.Cal_Cod = 0
'                Movimento_Dettaglio_Scarico_Essiccatoio.Lotto = Id_Infornatura
'                Movimento_Dettaglio_Scarico_Essiccatoio.Extra_Int = 0
'                Movimento_Dettaglio_Scarico_Essiccatoio.Contabilizzato = NONCONTABILE
'                Movimento_Dettaglio_Scarico_Essiccatoio.Pendente = enum_Pendenza.MovGiustificato
'                Movimento_Dettaglio_Scarico_Essiccatoio.Lav_Cod = Lav_Cod
'                Movimento_Dettaglio_Scarico_Essiccatoio.Cau_Mov = Movimento_Scarico_Essiccatoio.Cau_Mov
'                Movimento_Dettaglio_Scarico_Essiccatoio.Anno = 1900

'                Dim Movimento_Destinazione_Scarico_Essiccatoio As New Movimento_Destinazione
'                Movimento_Destinazione_Scarico_Essiccatoio.Id_Agenda = Agenda.Id_Agenda
'                Movimento_Destinazione_Scarico_Essiccatoio.Piva = Agenda.Piva
'                Movimento_Destinazione_Scarico_Essiccatoio.Sa_Cod = Sa_Cod_Rif
'                Movimento_Destinazione_Scarico_Essiccatoio.Id_Destinazione = Fabbricato_Cod_Rif
'                Movimento_Destinazione_Scarico_Essiccatoio.Data = Data_Fine_Cura
'                Movimento_Destinazione_Scarico_Essiccatoio.Tipo = ESSICCATOIO
'                Movimento_Destinazione_Scarico_Essiccatoio.Qta = Movimento_Dettaglio_Scarico_Essiccatoio.Qta
'                Movimento_Destinazione_Scarico_Essiccatoio.BaseCode = Agenda.BaseCode
'                Movimento_Destinazione_Scarico_Essiccatoio.TopCode = Agenda.TopCode

'                Movimento_Dettaglio_Scarico_Essiccatoio.Movimenti_Destinazioni.Add(Movimento_Destinazione_Scarico_Essiccatoio)
'                Movimento_Scarico_Essiccatoio.Movimenti_Dettagli.Add(Movimento_Dettaglio_Scarico_Essiccatoio)

'                Agenda.Movimenti.Add(Movimento_Scarico_Essiccatoio)




'                'faccio un mov di carico per ciascuna infornatura
'                If count > 0 Then
'                    'se non sono al primo giro aggiungo il movimento precedente
'                    Agenda.Movimenti.Add(Movimento_Carico_Magazzino)
'                End If

'                Movimento_Carico_Magazzino = New Movimento
'                Movimento_Carico_Magazzino.Id_Agenda = Agenda.Id_Agenda
'                Movimento_Carico_Magazzino.Piva = Agenda.Piva
'                Movimento_Carico_Magazzino.Sa_Cod = Agenda.Sa_Cod
'                Movimento_Carico_Magazzino.Data = Data_Fine_Cura.ToShortDateString
'                Movimento_Carico_Magazzino.Ora = Ora_Fine_Cura
'                Movimento_Carico_Magazzino.Lav_Cod = Lav_Cod
'                Movimento_Carico_Magazzino.Cau_Mov = CAU_CARICO
'                Movimento_Carico_Magazzino.Mov_Desc = "Carichi Lotti nel Magazzino Post Cura"
'                Movimento_Carico_Magazzino.BaseCode = Agenda.BaseCode
'                Movimento_Carico_Magazzino.TopCode = Agenda.TopCode

'                Movimento_Carico_Magazzino.Extra_Int = idSC 'necessario per rintracciata

'            End If


'            'faccio n movimenti dettagli e destinazioni di carico per ciscuna riga
'            Dim Movimento_Dettaglio_Carico_Magazzino As New Movimento_Dettaglio
'            Movimento_Dettaglio_Carico_Magazzino.Id_Agenda = Agenda.Id_Agenda
'            Movimento_Dettaglio_Carico_Magazzino.Piva = Agenda.Piva
'            Movimento_Dettaglio_Carico_Magazzino.Sa_Cod = Magazzino_Sa_Cod
'            Movimento_Dettaglio_Carico_Magazzino.Data = Data_Fine_Cura
'            Movimento_Dettaglio_Carico_Magazzino.Elem_Cod = TRASFORMATI_VEGETALI
'            Movimento_Dettaglio_Carico_Magazzino.Pro_Cod = 0
'            Movimento_Dettaglio_Carico_Magazzino.Mat_Cod = Mat_Cod_Tabacco_Curato
'            Movimento_Dettaglio_Carico_Magazzino.Mov_det_des = "Carico Lotto nel Magazzino Post Cura (collo " & Collo & ")"
'            Movimento_Dettaglio_Carico_Magazzino.Udm_Cod = UdmCaricoMagazzino
'            Movimento_Dettaglio_Carico_Magazzino.Qta = Peso
'            Movimento_Dettaglio_Carico_Magazzino.Cal_Cod = 0
'            Movimento_Dettaglio_Carico_Magazzino.Lotto = Collo 'metto il collo nell'extrastring, dovrò trovare il modo di farlo vedere ma almeno la rintracciata va meglio
'            Movimento_Dettaglio_Carico_Magazzino.Extra_Str = Id_Lotto 'id univoco per rintracciata, dato che il lotto potrebbe non essere univoco per aziena 
'            Movimento_Dettaglio_Carico_Magazzino.Contabilizzato = NONCONTABILE
'            Movimento_Dettaglio_Carico_Magazzino.Pendente = enum_Pendenza.MovGiustificato
'            Movimento_Dettaglio_Carico_Magazzino.Lav_Cod = Lav_Cod
'            Movimento_Dettaglio_Carico_Magazzino.Cau_Mov = Movimento_Carico_Magazzino.Cau_Mov
'            Movimento_Dettaglio_Carico_Magazzino.Anno = 1900

'            Dim Movimento_Destinazione_Carico_Magazzino As New Movimento_Destinazione
'            Movimento_Destinazione_Carico_Magazzino.Id_Agenda = Agenda.Id_Agenda
'            Movimento_Destinazione_Carico_Magazzino.Piva = Agenda.Piva
'            Movimento_Destinazione_Carico_Magazzino.Sa_Cod = Magazzino_Sa_Cod
'            Movimento_Destinazione_Carico_Magazzino.Id_Destinazione = Magazzino_Fabbricato_Cod
'            Movimento_Destinazione_Carico_Magazzino.Data = Data_Fine_Cura
'            Movimento_Destinazione_Carico_Magazzino.Tipo = MAGAZZINO
'            Movimento_Destinazione_Carico_Magazzino.Qta = Movimento_Dettaglio_Carico_Magazzino.Qta
'            Movimento_Destinazione_Carico_Magazzino.BaseCode = Agenda.BaseCode
'            Movimento_Destinazione_Carico_Magazzino.TopCode = Agenda.TopCode

'            Movimento_Dettaglio_Carico_Magazzino.Movimenti_Destinazioni.Add(Movimento_Destinazione_Carico_Magazzino)
'            Movimento_Carico_Magazzino.Movimenti_Dettagli.Add(Movimento_Dettaglio_Carico_Magazzino)

'            count += 1
'            If count = DtSfornature.Rows.Count Then
'                'aggiungo l'ultimo movimento
'                Agenda.Movimenti.Add(Movimento_Carico_Magazzino)
'            End If

'        Next

'        Return Agenda

'    End Function

'    Public Shared Function GeneraProdottoCuraTabacco(ByVal objParametri_Server As AgronicaCoreParametri, _
'                                                     ByVal ASG_ProgressivoGIAS As Integer, _
'                                                     ByVal NomeProdottoTabaccoCurato As String, _
'                                                     ByVal PrefissoCodArticolo As String, _
'                                                     ByVal veg_cod As Integer) As Integer


'        Dim OUTPUT_Mat_Cod As Integer
'        'Dim veg_cod As Integer = 335
'        ' Dim Veg_Des As String = "Tabacco"
'        Dim Piva_Creazione As String = objParametri_Server.PivaSuperUser 'azienda che lo crea
'        Dim Sa_Cod_Creazione As Integer = PUBBLICO 'pubblico
'        'altre=5010411
'        'Virginia Bright=5010624
'        Dim Cul_Cod As Integer = New AgronicaCoreMetaSchemaDAL.Cultivar_R().CulCod_Altre_from_VegCod(veg_cod, objParametri_Server)
'        If Cul_Cod < 1 Then
'            Throw New Exception("non c'è la varietà altre")
'        End If
'        Dim objImportaGias As New AgronicaCoreAnagrafeBIZ.Importa_GIAS
'        Dim Regolamento As enum_Cod_Regolamento = enum_Cod_Regolamento.Regolamento_Nessuno
'        Dim Flag_Biologico As Boolean = False

'        Dim Cod_Articolo As String = Right("000" & veg_cod, 3) & _
'                       "/" & Right("00000000" & CStr(Cul_Cod), 8)

'        Cod_Articolo = PrefissoCodArticolo & Cod_Articolo

'        Dim objCore_MP_R As New AgronicaCoreAnagrafeDAL.Materie_Prime_R
'        Dim flag_esiste As Boolean = True

'        Dim m As New AgronicaCoreMetaSchemaDAL.SpecieVegetali_R()
'        Dim vegdes As String = m.VegDes_from_VegCod(veg_cod, objParametri_Server)

'        flag_esiste = objCore_MP_R.Esiste_TrasformatoVegetale(Piva_Creazione, _
'                                              veg_cod, _
'                                              Cul_Cod, _
'                                               " Mat_Des = '" & NomeProdottoTabaccoCurato & " (" & vegdes & ")' and  Cod_Articolo = '" & Cod_Articolo & "' and   Sa_Cod = -1 ", _
'                                                objParametri_Server)


'        If flag_esiste = False Then

'            Dim objCore_XML_Anagrafe As New AgronicaCoreXML.XML_Anagrafe
'            Dim objCore_MP_W As New AgronicaCoreAnagrafeBIZ.Materie_Prime_W
'            Dim Flag_Insert As Boolean
'            Dim Basecode As Integer = 0
'            Dim Topcode As Integer = 0
'            AgronicaCoreDataProvider.UtilityProvider.Calcola_BaseCode_TopCode(Basecode, _
'                                                                   Topcode, _
'                                                                    ASG_ProgressivoGIAS)



'            objImportaGias.Creazione_Automatica_TrasformatoVegetale(objParametri_Server, _
'                                                    objCore_XML_Anagrafe, _
'                                                     objCore_MP_W, _
'                                                     Flag_Insert, _
'                                                     OUTPUT_Mat_Cod, _
'                                                     Basecode, _
'                                                     Topcode, _
'                                                     Piva_Creazione, _
'                                                     Sa_Cod_Creazione, _
'                                                     Cod_Articolo, _
'                                                     NomeProdottoTabaccoCurato & " (" & vegdes & ")", _
'                                                     veg_cod, _
'                                                     Cul_Cod, _
'                                                     Regolamento, _
'                                                     Flag_Biologico)

'        Else
'            Throw New Exception("Il prodotto esiste")
'        End If

'        Return OUTPUT_Mat_Cod

'    End Function

'    Public Shared Function CreaOggettoAgendaScaricoProdottoPerInvioAziendaOrigine(ByVal MovimentiCure As List(Of Movimento), _
'                                                                    ByVal Id_Agenda As Integer, ByVal Tipo_Operazione As Integer, ByVal ProvenienzaDesc As String) As Operazione_Agenda


'        Dim Agenda As New Operazione_Agenda
'        Agenda.Tipo_Operazione = Tipo_Operazione
'        Agenda.Id_Agenda = Id_Agenda
'        'Agenda.Id_Agenda =
'        Agenda.Data = MovimentiCure(0).Data
'        Agenda.Piva = MovimentiCure(0).Piva
'        Agenda.Sa_Cod = MovimentiCure(0).Sa_Cod
'        Agenda.Lav_Cod = LAVCOD_SCARICO
'        Agenda.Des_Lib = "Scarico Prodotto Curato per Invio ad Azienda Origine (Az: " & ProvenienzaDesc & ")"
'        Agenda.BaseCode = MovimentiCure(0).BaseCode
'        Agenda.TopCode = MovimentiCure(0).TopCode

'        For Each Movimento As Movimento In MovimentiCure

'            Dim Movimento_Scarico As New Movimento
'            Movimento_Scarico.Id_Agenda = Id_Agenda
'            Movimento_Scarico.Piva = Movimento.Piva
'            Movimento_Scarico.Sa_Cod = Movimento.Sa_Cod
'            Movimento_Scarico.Data = Movimento.Data
'            Movimento_Scarico.Ora = Movimento.Ora
'            Movimento_Scarico.Lav_Cod = Agenda.Lav_Cod
'            Movimento_Scarico.Cau_Mov = CAU_SCARICO
'            Movimento_Scarico.Mov_Desc = "Scarico Prodotto Curato per Invio ad Azienda Origine (" & ProvenienzaDesc & ")"
'            Movimento_Scarico.BaseCode = Agenda.BaseCode
'            Movimento_Scarico.TopCode = Agenda.TopCode

'            For Each MovimentoDettaglioCarico As Movimento_Dettaglio In Movimento.Movimenti_Dettagli

'                Dim MovimentoDettaglioScarico As New Movimento_Dettaglio
'                MovimentoDettaglioScarico.Id_Agenda = Agenda.Id_Agenda
'                MovimentoDettaglioScarico.Piva = MovimentoDettaglioCarico.Piva
'                MovimentoDettaglioScarico.Sa_Cod = MovimentoDettaglioCarico.Sa_Cod
'                MovimentoDettaglioScarico.Data = MovimentoDettaglioCarico.Data
'                MovimentoDettaglioScarico.Elem_Cod = MovimentoDettaglioCarico.Elem_Cod
'                MovimentoDettaglioScarico.Pro_Cod = MovimentoDettaglioCarico.Pro_Cod
'                MovimentoDettaglioScarico.Cod_Progetto = MovimentoDettaglioCarico.Cod_Progetto
'                MovimentoDettaglioScarico.Mat_Cod = MovimentoDettaglioCarico.Mat_Cod
'                MovimentoDettaglioScarico.Mov_det_des = "Scarico Prodotto Curato per Invio ad Azienda Origine (" & ProvenienzaDesc & ")"
'                MovimentoDettaglioScarico.Udm_Cod = MovimentoDettaglioCarico.Udm_Cod
'                MovimentoDettaglioScarico.Qta = MovimentoDettaglioCarico.Qta
'                MovimentoDettaglioScarico.Cal_Cod = MovimentoDettaglioCarico.Cal_Cod
'                MovimentoDettaglioScarico.Lotto = MovimentoDettaglioCarico.Lotto
'                MovimentoDettaglioScarico.Contabilizzato = MovimentoDettaglioCarico.Contabilizzato
'                MovimentoDettaglioScarico.Pendente = MovimentoDettaglioCarico.Pendente
'                MovimentoDettaglioScarico.Lav_Cod = Agenda.Lav_Cod
'                MovimentoDettaglioScarico.Cau_Mov = CAU_SCARICO
'                MovimentoDettaglioScarico.Extra_Str = MovimentoDettaglioCarico.Extra_Str
'                MovimentoDettaglioScarico.Extra_Int = MovimentoDettaglioCarico.Extra_Int
'                MovimentoDettaglioScarico.Anno = MovimentoDettaglioCarico.Anno

'                For Each MovimentoDestinazioneCarico As Movimento_Destinazione In MovimentoDettaglioCarico.Movimenti_Destinazioni

'                    Dim MovimentoDestinazioneScarico As New Movimento_Destinazione
'                    MovimentoDestinazioneScarico.Id_Agenda = Agenda.Id_Agenda
'                    MovimentoDestinazioneScarico.Data = MovimentoDestinazioneCarico.Data
'                    MovimentoDestinazioneScarico.Piva = MovimentoDestinazioneCarico.Piva
'                    MovimentoDestinazioneScarico.Sa_Cod = MovimentoDestinazioneCarico.Sa_Cod
'                    MovimentoDestinazioneScarico.Id_Destinazione = MovimentoDestinazioneCarico.Id_Destinazione
'                    MovimentoDestinazioneScarico.Tipo = MAGAZZINO
'                    MovimentoDestinazioneScarico.Qta = MovimentoDestinazioneCarico.Qta
'                    MovimentoDestinazioneScarico.BaseCode = Agenda.BaseCode
'                    MovimentoDestinazioneScarico.TopCode = Agenda.TopCode

'                    MovimentoDettaglioScarico.Movimenti_Destinazioni.Add(MovimentoDestinazioneScarico)
'                Next
'                Movimento_Scarico.Movimenti_Dettagli.Add(MovimentoDettaglioScarico)

'            Next

'            Agenda.Movimenti.Add(Movimento_Scarico)

'        Next

'        Return Agenda

'    End Function

'    Public Shared Function CreaOggettoAgendaCaricoPressoAziendaOrigine(ByVal MovimentiCure As List(Of Movimento), _
'                                                        PivaOrigine As String, _
'                                                        da_cod_origine As Integer, _
'                                                        fabbricato_cod_origine As Integer, _
'                                                        ByVal Id_Agenda As Integer, ByVal Tipo_Operazione As Integer, ByVal udsDescr As String) As Operazione_Agenda

'        Dim Agenda As New Operazione_Agenda
'        Agenda.Tipo_Operazione = Tipo_Operazione
'        Agenda.Id_Agenda = Id_Agenda
'        'Agenda.Id_Agenda =
'        Agenda.Data = MovimentiCure(0).Data
'        Agenda.Piva = PivaOrigine
'        Agenda.Sa_Cod = da_cod_origine
'        Agenda.Lav_Cod = LAVCOD_CARICO
'        Agenda.Des_Lib = "Carico Prodotto Curato dall'uds " & udsDescr & " nell'Azienda Origine "
'        Agenda.BaseCode = MovimentiCure(0).BaseCode
'        Agenda.TopCode = MovimentiCure(0).TopCode

'        For Each Movimento As Movimento In MovimentiCure

'            Dim Movimento_Carico As New Movimento
'            Movimento_Carico.Id_Agenda = Agenda.Id_Agenda
'            Movimento_Carico.Piva = PivaOrigine
'            Movimento_Carico.Sa_Cod = da_cod_origine
'            Movimento_Carico.Data = Movimento.Data
'            Movimento_Carico.Ora = Movimento.Ora
'            Movimento_Carico.Lav_Cod = Agenda.Lav_Cod
'            Movimento_Carico.Cau_Mov = CAU_CARICO
'            Movimento_Carico.Mov_Desc = "Carico Prodotto Curato dall'uds " & udsDescr & " nell'Azienda Origine "
'            Movimento_Carico.BaseCode = Agenda.BaseCode
'            Movimento_Carico.TopCode = Agenda.TopCode

'            For Each MovimentoDettaglio As Movimento_Dettaglio In Movimento.Movimenti_Dettagli

'                Dim MovimentoDettaglioCarico As New Movimento_Dettaglio
'                MovimentoDettaglioCarico.Id_Agenda = Agenda.Id_Agenda
'                MovimentoDettaglioCarico.Piva = Agenda.Piva
'                MovimentoDettaglioCarico.Sa_Cod = Agenda.Sa_Cod
'                MovimentoDettaglioCarico.Data = MovimentoDettaglio.Data
'                MovimentoDettaglioCarico.Elem_Cod = MovimentoDettaglio.Elem_Cod
'                MovimentoDettaglioCarico.Pro_Cod = MovimentoDettaglio.Pro_Cod
'                MovimentoDettaglioCarico.Cod_Progetto = MovimentoDettaglio.Cod_Progetto
'                MovimentoDettaglioCarico.Mat_Cod = MovimentoDettaglio.Mat_Cod
'                MovimentoDettaglioCarico.Mov_det_des = "Carico Prodotto Curato dall'uds " & udsDescr & " nell'Azienda Origine "
'                MovimentoDettaglioCarico.Udm_Cod = MovimentoDettaglio.Udm_Cod
'                MovimentoDettaglioCarico.Qta = MovimentoDettaglio.Qta
'                MovimentoDettaglioCarico.Cal_Cod = MovimentoDettaglio.Cal_Cod
'                MovimentoDettaglioCarico.Lotto = MovimentoDettaglio.Lotto
'                MovimentoDettaglioCarico.Contabilizzato = MovimentoDettaglio.Contabilizzato
'                MovimentoDettaglioCarico.Pendente = MovimentoDettaglio.Pendente
'                MovimentoDettaglioCarico.Lav_Cod = Agenda.Lav_Cod
'                MovimentoDettaglioCarico.Cau_Mov = CAU_CARICO
'                MovimentoDettaglioCarico.Extra_Str = MovimentoDettaglio.Extra_Str
'                MovimentoDettaglioCarico.Extra_Int = MovimentoDettaglio.Extra_Int
'                MovimentoDettaglioCarico.Anno = MovimentoDettaglio.Anno

'                For Each MovimentoDestinazione As Movimento_Destinazione In MovimentoDettaglio.Movimenti_Destinazioni

'                    Dim MovimentoDestinazioneCarico As New Movimento_Destinazione
'                    MovimentoDestinazioneCarico.Id_Agenda = Agenda.Id_Agenda
'                    MovimentoDestinazioneCarico.Data = MovimentoDestinazione.Data
'                    MovimentoDestinazioneCarico.Piva = Agenda.Piva
'                    MovimentoDestinazioneCarico.Sa_Cod = Agenda.Sa_Cod
'                    MovimentoDestinazioneCarico.Id_Destinazione = fabbricato_cod_origine
'                    MovimentoDestinazioneCarico.Tipo = MAGAZZINO
'                    MovimentoDestinazioneCarico.Qta = MovimentoDestinazione.Qta
'                    MovimentoDestinazioneCarico.BaseCode = Agenda.BaseCode
'                    MovimentoDestinazioneCarico.TopCode = Agenda.TopCode

'                    MovimentoDettaglioCarico.Movimenti_Destinazioni.Add(MovimentoDestinazioneCarico)
'                Next
'                Movimento_Carico.Movimenti_Dettagli.Add(MovimentoDettaglioCarico)
'            Next

'            Agenda.Movimenti.Add(Movimento_Carico)

'        Next

'        Return Agenda

'    End Function

'End Class