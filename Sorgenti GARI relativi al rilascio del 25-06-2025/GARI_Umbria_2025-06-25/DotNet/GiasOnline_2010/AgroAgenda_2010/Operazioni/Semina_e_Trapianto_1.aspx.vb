Imports System.Web
Imports System.Web.Services
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
Imports System.Xml

Public Class Semina_e_Trapianto_1
    Inherits System.Web.UI.Page
    Implements iOperazioneGUI, iOperazioneGUI_Semina

    Dim PERMETTI_OPERAZIONI_SU_IMPIANTI_BLOCCATI As Boolean

    'Dim Tipo_Semina As enum_SEMINA_TIPO


    Protected Overrides Sub InitializeCulture()
        MyBase.InitializeCulture()
        Lingua.Gias_InizializzaCultura_DaSession()

    End Sub



    Dim objParametri_Server, objParametri_Utenti As AgronicaCoreParametri
    Dim objParametriAgenda As ParametriAgenda
    Dim Id_Agenda_Old As Integer
    Public Master_Operazione As Operazione
    Dim Movimento_Dettaglio_Contabilizzato As Integer = 1 '1 se data<corrente -1 se data futura
    Dim Movimento_Dettaglio_Pendente As Integer = 2
    Dim Movimento_Dettaglio_Extra_Date As Date
    Dim Movimento_Dettaglio_Anno As Integer = 1900
    Dim ListaValoriSeminaImpianti As New List(Of ValoriImpiantoESemineImpostati)
    Dim messaggioSiNo As Boolean = False

    Dim NumColonna_Check As Integer = 0
    Dim NumColonna_Rag_Soc As Integer = 1
    Dim NumColonna_Sa_Nome As Integer = 2
    Dim NumColonna_Campo_Des As Integer = 3

    Dim NumColonna_App_Nome As Integer = 4

    Dim NumColonna_MetodoProduttivo As Integer = 5

    Dim NumColonna_Distinta As Integer = 6
    Dim NumColonna_Specie_Attuale As Integer = 7
    Dim NumColonna_Specie As Integer = 8
    Dim NumColonna_Varietà As Integer = 9
    Dim NumColonna_Finalità As Integer = 10

    Dim NumColonna_Disciplinare As Integer = 11
    Dim NumColonna__Sup_App As Integer = 12
    Dim NumColonna__Sup_Semina As Integer = 13
    Dim NumColonna_DistTraFila As Integer = 14
    Dim NumColonna_DistSuFila As Integer = 15
    Dim NumColonna_Interbina As Integer = 16
    Dim NumColonna_Fila_Binata As Integer = 17
    Dim NumColonna_Germinabilita As Integer = 18
    Dim NumColonna_p_ha As Integer = 19
    Dim NumColonna_Descrizione_Lotto As Integer = 20
    Dim NumColonna_UnitaMisura As Integer = 21
    Dim NumColonna_UdmSimLotto As Integer = 22
    Dim NumColonna_PianteImpianto As Integer = 23
    Dim NumColonna_Catasto As Integer = 24

    'Se si usa il planning
    Dim NumColonna_Planning_Sup_App As Integer = 16
    Dim NumColonna_Planning_DistTraFila As Integer = 17
    Dim NumColonna_Planning_DistSuFila As Integer = 18
    Dim NumColonna_Planning_Interbina As Integer = 19
    Dim NumColonna_Planning_Germinabilita As Integer = 20

    Dim NumColonna_Planning_Veg_Cod As Integer = 23
    Dim NumColonna_Planning_Cul_Cod As Integer = 24
    Dim NumColonna_Planning_Prodotto As Integer = 25
    Dim NumColonna_Planning_UnitaMisura As Integer = 28
    Dim NumColonna_Planning_PianteImpianto As Integer = 30
    '------------

    'udm e quantità di default, da inserire nel movimento dettaglio 
    'quando non ho il magazzino e non sono visibili le colonne
    Dim UdmDefault As String = "0"
    Dim QtaDefault As String = "0"

    Dim ListaProdotto As AgronicaControlli_2010.ComboMateriePrime
    'Dim ListaSpecie As DropDownList
    Dim ListaMetodoProduzione As DropDownList
    Dim ListaVarieta As DropDownList
    Dim ListaFinalita As DropDownList
    Dim ListaDisciplinari As DropDownList
    Dim Dt_Impianti As DataTable
    Dim Dt_Magazzino As DataTable

    Dim HashSpecie As New Hashtable
    Dim DtCultivar As New DataTable
    Dim DtFinalita As New DataTable
    Dim DtDpi As New DataTable


    Dim Qs_Unid_Ricetta As String
    Dim Qs_Unid_Ricetta_Operazione As String
    Dim Qs_Unid_Operazione As String
    Dim Qs_Operazione_Ricetta As String
    Dim Qs_Ricetta_Cod As String

    Public objParametriAgenda_TargetOperazione As String

    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Master_Operazione = CType(Page.Master, Operazione)

        AddHandler Master_Operazione.Property_BTN_ChangeData.Click, AddressOf Me.aggiornaPerDataScadenza
        AddHandler Master_Operazione.Property_BTN_ComboSpecie.Click, AddressOf Me.CambioSpecie
        AddHandler Master_Operazione.Property_ImgBtn_AnnullaTutto.Click, AddressOf Me.AnnullaTutto
        AddHandler Master_Operazione.Property_BTN_CentroAziendale.Click, AddressOf Me.CambioCentro
        AddHandler Master_Operazione.Property_BTN_Magazzini.Click, AddressOf Me.CambioMagazzino
        AddHandler Master_Operazione.Property_ImgBtn_Salva.Click, AddressOf Me.SalvaTutto
        AddHandler CType(Ricerca.FindControlIterative(Page.Master, "ImgBtn_Carico"), ImageButton).Click, AddressOf Me.BTN_CaricoMagazzino
        AddHandler Master_Operazione.Property_ImgBtn_DDT.Click, AddressOf Me.ImgBtn_DDT_Click
        AddHandler Master_Operazione.Property_ImgBtn_DDT_Cancella.Click, AddressOf Me.ImgBtn_DDT_Cancella_Click
        AddHandler CType(Page.Master, MasterPage).PreRender, AddressOf Me.MasterUnload
        AddHandler Master_Operazione.Property_Btn_Conferma_Ricetta.Click, AddressOf Me.Btn_Conferma_Ricetta
        Dim script As New StringBuilder
        script.AppendLine("$(document).ready(function () { ")
        script.AppendLine("     colora_varieta(); ")

        script.AppendLine("     $('#dialogImpostazioniColonneSemine').dialog({ ")
        script.AppendLine("             autoOpen: false,")
        script.AppendLine("             modal: true,")
        script.AppendLine("             buttons: {")
        script.AppendLine("                 'Aggiungi': function () {")
        script.AppendLine("                 $(this).dialog('close');")
        script.AppendLine("                 SalvaImpostazioniColonneSemine();")
        script.AppendLine("             },")
        script.AppendLine("             'Annulla': function () {")
        script.AppendLine("                 $(this).dialog('close');")
        script.AppendLine("                 return false;")
        script.AppendLine("             }")
        script.AppendLine("         }")
        script.AppendLine("     });")


        script.AppendLine("     $('#btn_Impostazioni_ColonneSemine').click(function () {")
        script.AppendLine("             $('#dialogImpostazioniColonneSemine').dialog('open');")
        script.AppendLine("             $('#dialogImpostazioniColonneSemine').parent().appendTo($('form:first')); ")
        script.AppendLine("     });")


        script.AppendLine("     $('#chkSelezionaTuttiImp').click(function (){ ")
        script.AppendLine("         SelezionaDeselezionaTuttiImp();")
        script.AppendLine("     });")

        script.AppendLine("     $('#chkSelezionaTuttiImpPlanning').click(function (){ ")
        script.AppendLine("         SelezionaDeselezionaTuttiPlanning();")
        script.AppendLine("     });")

        'script.AppendLine("     $('#chkSelezionaTuttiGiacenze').click(function (){ ")
        'script.AppendLine("         alert('a');")
        'script.AppendLine("         SelezionaDeselezionaTuttiGiacenze();")
        'script.AppendLine("     });")

        script.AppendLine("     $('.ChkSelezionaImp').click(function (){ ")
        script.AppendLine("         ChkSelezionaImp_Click();")
        script.AppendLine("     });")

        script.AppendLine("      $('.CmbUdm').change(function(){CopiaValoriColonnaCmbUdm(this);}) ")

        script.AppendLine("      $('.TxtQtaGiacenza').bind('keyup',function(){DistribuisciQtaSeme(this);}) ")

        script.AppendLine("});")

        ScriptManager.RegisterStartupScript(Master_Operazione.Property_UpdatePanelPerScript,
                                Master_Operazione.Property_UpdatePanelPerScript.GetType(),
                                "jQuery_{0}", script.ToString, True)

    End Sub

    Private Sub MasterUnload(sender As Object, e As EventArgs)

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        verificoCredenzialiDiAccesso()
        inizializzoObjParametri()
        inizializzoParametriPagina()


        If Not IsNothing(Request.QueryString("unid_ricetta")) Then
            Qs_Unid_Ricetta = Stringa_Decodifica(Request.QueryString("unid_ricetta").ToString,
                                                AgroKey_EncoderDecoder,
                                                Server)
        Else
            Qs_Unid_Ricetta = ""
        End If
        If Not IsNothing(Request.QueryString("unid_ricetta_operazione")) Then
            Qs_Unid_Ricetta_Operazione = Stringa_Decodifica(Request.QueryString("unid_ricetta_operazione").ToString,
                                                AgroKey_EncoderDecoder,
                                                Server)
        Else
            Qs_Unid_Ricetta_Operazione = ""
        End If
        If Not IsNothing(Request.QueryString("unid_operazione")) Then
            Qs_Unid_Operazione = Stringa_Decodifica(Request.QueryString("unid_operazione").ToString,
                                                AgroKey_EncoderDecoder,
                                                Server)
        Else
            Qs_Unid_Operazione = ""
        End If
        If Not IsNothing(Request.QueryString("operazione_ricetta")) Then
            Qs_Operazione_Ricetta = Stringa_Decodifica(Request.QueryString("operazione_ricetta").ToString,
                                                AgroKey_EncoderDecoder,
                                                Server)
        Else
            Qs_Operazione_Ricetta = ""
        End If
        If Not IsNothing(Request.QueryString("r")) Then
            Qs_Ricetta_Cod = Stringa_Decodifica(Request.QueryString("r").ToString,
                                                AgroKey_EncoderDecoder,
                                                Server)
        Else
            Qs_Ricetta_Cod = "0"
        End If

        LeggiImpostazioni()

        DtCultivar.Columns.Add("veg_cod", GetType(Integer))
        DtCultivar.Columns.Add("cul_cod", GetType(Integer))
        DtCultivar.Columns.Add("cul_des", GetType(String))

        DtFinalita.Columns.Add("veg_cod", GetType(Integer))
        DtFinalita.Columns.Add("grfi_cod", GetType(Integer))
        DtFinalita.Columns.Add("grfi_des", GetType(String))

        DtDpi.Columns.Add("veg_cod", GetType(Integer))
        DtDpi.Columns.Add("dpi_cod", GetType(String))
        DtDpi.Columns.Add("dpi_des", GetType(String))

        If Not IsPostBack Then

            VerificaPermessi()

            Imposta_TipoSemina_DaOpzioneUtente()

            Session("MovimentiDettagliDDT") = Nothing
            ImageInfo0.Visible = False
            LabelInfo0.Visible = False

            CreaImpostazioniColonneSemine()
            ImpostazioniColonneSemine()
            caricaControlli()


            LabelInfo1.Text = Resources.AgronicaAgenda_2010.NelCasoDiUtilizzoDelMagazzinoOccorreSelezi
            LabelInfo2.Text = Resources.AgronicaAgenda_2010.AssicurarsiSeSiUtilizzaIlMagazzinoCheCiSia
            LabelInfo3.Text = Resources.AgronicaAgenda_2010.GliImpiantiPerEssereVisibiliDevonoAvereUna
            LabelInfo4.Text = Resources.AgronicaAgenda_2010.NelCasoDiSeminaTrapiantoInTerreniNudiIlNom

            If objParametriAgenda.Lav_Cod = LAVCOD_SOVESCIO Then
                LabelInfo4.Text = "NB: Per il Sovescio i dati sull'impianto come varietà, finalità, distanze/fila, germinabilità ect. NON vengono aggiornati!"
            End If
            LabelInfo5.Text = Resources.AgronicaAgenda_2010.SePerISementiÈStataImpostataLaDosePerEttar


            If objParametriAgenda.Tipo_Operazione = TipiEnumerativi.enum_TipoOperazioneDB.Lettura Or objParametriAgenda.Tipo_Operazione = TipiEnumerativi.enum_TipoOperazioneDB.Modifica Then

            End If


        Else

            If Not IsNothing(Session("MovimentiDettagliDDT")) Then
                ImageInfo0.Visible = True
                LabelInfo0.Visible = True
            Else
                ImageInfo0.Visible = False
                LabelInfo0.Visible = False
            End If
            'liste precaricate per combo e per tabelle impianti e giacenze
            'ListaSpecie = Session("ListaSpecie")
            'ListaVarieta = Session("ListaVarieta")
            'ListaFinalita = Session("ListaFinalita")
            'ListaDisciplinari = Session("ListaDisciplinari")
            Dt_Impianti = Session("Dt_Impianti")
            Dt_Magazzino = Session("Dt_Magazzino")
        End If

        disabilitaControlli()

        If objParametriAgenda.TargetOperazione = enum_Tipo_Operazione_Agenda_Target.Reale Then
            objParametriAgenda_TargetOperazione = "Enum_TargetOperazione.Reale"
            'AggiornaVisibilitaColonneSemineImpianti()
        Else
            objParametriAgenda_TargetOperazione = "Enum_TargetOperazione.Planning"
        End If

    End Sub


#Region "Metodi Eseguiti nel Load"


    Private Sub verificoCredenzialiDiAccesso()
        '----- Verifico che l'utente sia autenticato
        If Session("ASG_Utente_Username") = "" Then
            Response.Redirect("~/Custom500.aspx")
        End If
    End Sub


    Private Sub inizializzoObjParametri()
        objParametri_Server = New AgronicaCoreDataProvider.AgronicaCoreParametri(Session("ASG_objParametri_Server"))
        objParametri_Utenti = New AgronicaCoreDataProvider.AgronicaCoreParametri(Session("ASG_objParametri_Utenti"))
    End Sub


    Private Sub inizializzoParametriAgenda()
        objParametriAgenda = New ParametriAgenda
        Id_Agenda_Old = objParametriAgenda.Id_Agenda
        objParametriAgenda.OperazioneMulticentro = True
    End Sub


    Private Sub inizializzoParametriPagina()


        If Not IsNothing(Session("PERMETTI_OPERAZIONI_SU_IMPIANTI_BLOCCATI")) Then
            If Session("PERMETTI_OPERAZIONI_SU_IMPIANTI_BLOCCATI") = True Then
                PERMETTI_OPERAZIONI_SU_IMPIANTI_BLOCCATI = True
            End If
        End If

        inizializzoParametriAgenda()

        Dim objOperazioniLeggi As New AgronicaCoreMetaSchemaDAL.Operazioni_R
        Dim Lav_Des As String = objOperazioniLeggi.LavorazioneDes_from_LavorazioneCod(objParametriAgenda.Lav_Cod, objParametri_Server)
        objParametriAgenda.Lav_Des = Lav_Des
        Master_Operazione.Property_Lbl_Titolo.Text = Lav_Des

        Select Case CInt(objParametriAgenda.Lav_Cod)

            Case LAVCOD_SEMINA, LAVCOD_TRAPIANTO, LAVCOD_SOVESCIO, LAVCOD_SOD_SEDDING
                objParametriAgenda.Cau_Mov = CStr(enum_Agenda_Causali.LAVORAZIONE)

            Case Else
                Throw New NotImplementedException

        End Select





    End Sub


    Private Sub VerificaPermessi()
        Dim UtenteAbilitato_Lettura As Boolean = False
        Dim UtenteAbilitato_Modifica As Boolean = False
        Dim objUtility As New AgronicaCoreModello.Utility_Operazioni

        objUtility.Verifica_Permessi_OperazioniAgenda_X_PagineAgronicaAgenda(objParametri_Server, objParametri_Utenti, UtenteAbilitato_Lettura, UtenteAbilitato_Modifica)

        Session("UtenteAbilitato_Lettura") = UtenteAbilitato_Lettura

        If UtenteAbilitato_Lettura = False Then
            Response.Redirect("~/classi/Agro_Pages/AccessoNonConsentito.aspx")
            Exit Sub
        End If

        Session("UtenteAbilitato_Modifica") = UtenteAbilitato_Modifica

        If (Not (objParametriAgenda.Tipo_Operazione = enum_TipoOperazioneDB.Lettura)) AndAlso UtenteAbilitato_Modifica = False Then
            Response.Redirect("~/classi/Agro_Pages/AccessoNonConsentito.aspx")
            Exit Sub
        End If

    End Sub


    Private Sub LeggiImpostazioni()

        Dim ObjUtenti As New AgronicaCoreUtentiDAL.Utenti_Impostazioni_FiltroMono_R
        Dim Dt_Impostazioni As DataTable = ObjUtenti.Leggi(enum_Impostazioni_Utenti.UTENTE_COD_UTILIZZA_SUP_APP_AGENDA,
                                          objParametriAgenda.Lav_Cod,
                                          enumSelezioneVariabile.Selezione_TabellaCompleta,
                                          "", "", objParametri_Utenti)

        If Not IsNothing(Dt_Impostazioni) AndAlso Dt_Impostazioni.Rows.Count > 0 Then
            ViewState("Utilizza_SupApp") = True
        Else
            ViewState("Utilizza_SupApp") = False
        End If

    End Sub

    Private Sub Imposta_TipoSemina_DaOpzioneUtente()

        DivOpzioni.Attributes.Add("style", "display:none")

        Select Case objParametriAgenda.Tipo_Operazione

            Case TipiEnumerativi.enum_TipoOperazioneDB.Scrittura

                If objParametriAgenda.Lav_Cod = LAVCOD_SOVESCIO Then

                    RBL_Tipo_Semina.SelectedValue = 30

                Else

                    'RBL_Tipo_Semina.SelectedValue = 30

                    Dim Tipo_Semina_Val As String
                    Dim ObjUtentiI As New AgronicaCoreUtentiDAL.Utenti_Impostazioni_Read
                    Tipo_Semina_Val = ObjUtentiI.Impostazione_Valore_From_Impostazione_Cod_Utente_Poi_SuperUser(
                                                                    enum_Impostazioni_Utenti.UTENTE_COD_SEMINA_TIPO,
                                                                     objParametri_Utenti)
                    If IsNumeric(Tipo_Semina_Val) Then

                        Select Case Tipo_Semina_Val
                            Case enum_SEMINA_TIPO.Frazionamento_Appezzamenti_perLotto_e_Semina_sui_Singoli_Vincolo
                                RBL_Tipo_Semina.SelectedValue = 50
                            Case enum_SEMINA_TIPO.Semina_e_Modifica_Appezzamenti_Vincolo
                                RBL_Tipo_Semina.SelectedValue = 40
                            Case enum_SEMINA_TIPO.Solo_Semina_Vincolo
                                RBL_Tipo_Semina.SelectedValue = 30
                            Case Else
                                RBL_Tipo_Semina.SelectedValue = Tipo_Semina_Val
                                DivOpzioni.Attributes.Remove("style")
                        End Select

                        If RBL_Tipo_Semina.SelectedValue = enum_SEMINA_TIPO.Frazionamento_Appezzamenti_perLotto_e_Semina_sui_Singoli_Default Then
                            Master_Operazione.Property_ImgBtn_DDT.Visible = False
                            Master_Operazione.Property_ImgBtn_DDT_Cancella.Visible = False
                        End If

                    Else

                        RBL_Tipo_Semina.SelectedValue = 30
                        DivOpzioni.Attributes.Remove("style")

                    End If

                End If

            Case Else

                'If objParametriAgenda.Lav_Cod = LAVCOD_SOVESCIO Or objParametriAgenda.Tipo_Operazione = enum_TipoOperazioneDB.Lettura Or objParametriAgenda.Tipo_Operazione = enum_TipoOperazioneDB.Modifica Then
                RBL_Tipo_Semina.SelectedValue = 30
                'End If
                'RBL_Tipo_Semina.Items(2).Attributes.Add("style", "display:none")

        End Select

        'Salvo il tipo per usarlo lato javascript
        'hf_UTENTE_COD_SEMINA_TIPO.Value = Tipo_Semina

    End Sub

    Private Sub caricaControlli()



        Select Case objParametriAgenda.Tipo_Operazione

            Case TipiEnumerativi.enum_TipoOperazioneDB.Scrittura
                rbl_Specie.SelectedValue = "0"
                'CheckBoxTerrenoNudo.Checked = False
                'in scrittura carico le due tabelle senza bisogno di avere le due liste di valori
                caricaListePerCombo()

                If objParametriAgenda.TargetOperazione = enum_Tipo_Operazione_Agenda_Target.Reale Then
                    caricaDtImpianti()
                Else
                    caricaDtPlanning()
                End If

                caricaDtGiacenze()
                caricaTabelle(False)

            Case TipiEnumerativi.enum_TipoOperazioneDB.Modifica, TipiEnumerativi.enum_TipoOperazioneDB.Lettura

                rbl_Specie.SelectedValue = "0"
                'CheckBoxTerrenoNudo.Checked = False

                'pemporaneamente in lettura e modifica visualizzo tutte le giacenze, per non
                'rischiare di nascondere ed eliminare il dato, dato che se salvassi
                'un sato con giacenza nulla questo in lettura o modifica non
                'sarebbe inserito nella tabella e quindi perso al salvataggio
                Me.CheckBoxGiacenzePositive.Checked = False


                'in modifica e lettura prima di caricare le tabelle leggo dall'operazione agenda i valori 
                'da inserire nelle liste dei valori e che serviranno per imostare i check e i valori nelle tabelle
                'e nei parametri agenda come fabbricato, data, specie
                CaricaListeValori_e_LeggiParametri_DaAgenda()

                caricaListePerCombo()

                If objParametriAgenda.TargetOperazione = enum_Tipo_Operazione_Agenda_Target.Reale Then
                    caricaDtImpianti(True)
                Else
                    caricaDtPlanning()
                End If

                caricaDtGiacenze()

                'Genero le due tabelle come in una nuova operazione
                caricaTabelle(False)

                If objParametriAgenda.Fabbricato <> "0" Then

                    ChekkoRighe_Tabella_Magazzino()

                End If

                Master_Operazione.CaricaCostiAccessori()

                Ripristina_Dati_nei_Controlli()

        End Select

    End Sub

    Private Sub caricaListePerCombo()

        'ListaSpecie = New DropDownList()
        ListaMetodoProduzione = New DropDownList
        ListaVarieta = New DropDownList()
        ListaFinalita = New DropDownList()
        ListaDisciplinari = New DropDownList()

        'AgronicaCoreUtility.CaricaListControl.MetodoProduzione(ListaMetodoProduzione, False, "", "")

        'Dim FiltroAggiuntivo As String
        'Select Case objParametriAgenda.Lav_Cod
        '    Case LAVCOD_SEMINA
        '        FiltroAggiuntivo = "  ((SpecieVegetali.Gru_Cod = '2') OR (SpecieVegetali.Gru_Cod = '3') ) "
        '    Case LAVCOD_TRAPIANTO
        '        FiltroAggiuntivo = "  ( (SpecieVegetali.Gru_Cod = '1') OR (SpecieVegetali.Gru_Cod = '3') OR (SpecieVegetali.Veg_Cod = '6') ) "
        'End Select

        ''AgronicaCoreUtility.CaricaListControl.SpecieVegetale_Optimize(ListaSpecie,
        ''      False, "", "", 0, "", True, objParametriAgenda.Veg_Cod.Split("/")(0), 0, 0, FiltroAggiuntivo, "", objParametri_Server, objParametri_Utenti)


        'Dim objSpecVeg As New AgronicaCoreMetaSchemaDAL.SpecieVegetali_R()
        'Dim DtSpecie As DataTable = objSpecVeg.SpecieVegetali_GestioneFiltroUtente_Leggi(objParametriAgenda.Veg_Cod.Split("/")(0),
        '                                                                0,
        '                                                                "",
        '                                                                "",
        '                                                                FiltroAggiuntivo,
        '                                                                "",
        '                                                                objParametri_Utenti)


        'If objParametriAgenda.Veg_Cod.Split("/")(0) = "0" Or objParametriAgenda.Veg_Cod.Split("/")(0) = "-1" Then
        '    ListaVarieta.Items.Add(New ListItem("", "-1"))
        '    ListaFinalita.Items.Add(New ListItem("", "-1"))
        '    ListaDisciplinari.Items.Add(New ListItem("", "-1"))
        '    Exit Sub
        'End If

        'AgronicaCoreUtility.CaricaListControl.CaricaCombo_VarietaColtivate_con_Visibilita_Utente(ListaVarieta,
        '      False, "", "", True, objParametriAgenda.Veg_Cod.Split("/")(0), 0, "", "", objParametri_Server, objParametri_Utenti)

        'AgronicaCoreUtility.CaricaListControl.Finalita(
        '                    ListaFinalita,
        '                    False, "", "", objParametriAgenda.Veg_Cod.Split("/")(0), 0, "", "", "", objParametri_Server)

        ''DISCIPLINARE
        'If objParametriAgenda.Veg_Cod.Split("/")(0) <> "" Then

        '    objParametri_Server.ImpostaFinestre_con_SalvataggioTemporale(CDate(objParametriAgenda.Data), CDate(objParametriAgenda.Data))
        '    Dim objCaricaCombo As New AgronicaCoreDpiBIZ.CaricaListControl
        '    objCaricaCombo.Disciplinari_Elenco(ListaDisciplinari,
        '                                     False, "", "",
        '                                    Session,
        '                                    objParametri_Server,
        '                                    objParametri_Utenti,
        '                                    CInt(0),
        '                                    CInt(objParametriAgenda.Veg_Cod),
        '                                    CInt(0),
        '                                    CInt(0),
        '                                    True,
        '                                    False,
        '                                    False,
        '                                    New AgronicaCoreGestioneRichieste.AgroWebConfig())
        '    objParametri_Server.ResettaFinestra()
        'Else
        '    ListaDisciplinari.Items.Clear()
        '    ListaDisciplinari.Items.Add(New ListItem(Resources.AgronicaAgenda_2010.NessunDisciplinare, "0"))
        'End If

        'Session("ListaSpecie") = ListaSpecie
        'Session("ListaVarieta") = ListaVarieta
        'Session("ListaFinalita") = ListaFinalita
        'Session("ListaDisciplinari") = ListaDisciplinari

    End Sub

    'Public Sub Cmb_SpecieVegetale_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)

    '    Dim indice As Integer
    '    indice = CType(CType(sender, DropDownList).Parent.Parent, GridViewRow).RowIndex
    '    Dim Veg_Cod As Integer = CType(GridViewImpianti.Rows(indice).Cells(NumColonna_Specie).Controls(1), DropDownList).SelectedValue
    '    Set_Specie(indice)

    'End Sub


    Private Sub caricaDtImpianti(Optional ByVal CaricaSalvati As Boolean = False)

        'If objParametriAgenda.Lav_Cod <> LAVCOD_SOVESCIO Then

        Dt_Impianti = InizializzaDataTablePerGridViewImpianti()

        Dim Icona_INFO As String = "<img src='../../AB_Immagini/Icone16/cI.ico' border='0'>"

        Dim filtroImpianto As String = ""
        Dim filtraSpecie As Boolean = True

        If CaricaSalvati = True Then

            For i = 0 To objParametriAgenda.Impianti.Count - 1
                filtroImpianto &= "  (Reg_Impianti.piva= '" & objParametriAgenda.Impianti(i).Piva & "'  and Reg_Impianti.sa_cod=  " & objParametriAgenda.Impianti(i).Sa_Cod & " and  Reg_Impianti.Appezza= " & objParametriAgenda.Impianti(i).Appezza & "  and Reg_Impianti.Id_Reg=  " & objParametriAgenda.Impianti(i).ID_Reg & ") OR "

            Next
            If filtroImpianto <> "" Then
                filtroImpianto = "(" & Left(filtroImpianto, filtroImpianto.Length - 3) & ") "
            End If

            'objParametriAgenda.Veg_Cod = "-1"
            filtraSpecie = False

        Else

            'lista impianti, in questa operazione serve solo per memorizzare l'impianto quandol'operazione è
            'chiamata dall'albero anagrafe e deve gestire un solo impianto. se contiene un impianto allora faccio vedere solo questo
            'perchè sicuramente arrivo dall'albero anagrave. non sono previsti altrimenti valori nella lista

            If objParametriAgenda.Impianti.Count = 1 Then
                filtroImpianto = "  Reg_Impianti.Appezza= " & objParametriAgenda.Impianti(0).Appezza & "  and Reg_Impianti.Id_Reg=  " & objParametriAgenda.Impianti(0).ID_Reg & "  "
                rbl_Specie.SelectedValue = "0"
                'CheckBoxTerrenoNudo.Checked = False
            Else
                'filtro in scrittura se ho impostato il filtro dal menu agenda
                Dim filtro As String = "|"
                If objParametriAgenda.Tipo_Operazione = enum_TipoOperazioneDB.Scrittura Then
                    If Not IsNothing(Session("Filtro")) AndAlso Session("Filtro") <> "" Then
                        'No un filtro

                        filtro = Session("Filtro")
                        filtroImpianto = filtro.Split("|")(1)
                        rbl_Specie.SelectedValue = "0"
                        'CheckBoxTerrenoNudo.Checked = False
                    End If
                End If
            End If

        End If



        'IMPIANTI CON SPECIE
        If objParametriAgenda.Veg_Cod.Split("/")(0) <> "-1" AndAlso (objParametriAgenda.Veg_Cod.Split("/")(0) <> "0" OrElse objParametriAgenda.Veg_Cod.Split("/")(1) <> "0") Then
            'If objParametriAgenda.Veg_Cod.Split("/")(0) <> "0" Then

            '----------------------------------
            'lettura particelle impianti
            Dim DtParticelle As DataTable
            Dim objImpianti As New AgronicaCoreAnagrafeDAL.Reg_Impianti_Read
            DtParticelle = objImpianti.Leggi_ParticelleImpianti_xAgenda2(False,
                                                     objParametriAgenda.Piva,
                                                     0,
                                                     objParametriAgenda.Veg_Cod.Split("/")(0),
                                                     objParametriAgenda.Cul_Cod,
                                                     objParametriAgenda.Data,
                                                     0,
                                                     0,
                                                     0,
                                                     filtroImpianto, " Cul_Des, App_Nome, Progetto ",
                                                     objParametri_Server, True, filtraSpecie)


            Dim DtParticelleTN As DataTable
            DtParticelleTN = objImpianti.Leggi_ParticelleImpianti_xAgenda2(False,
                                                     objParametriAgenda.Piva,
                                                     0,
                                                     0,
                                                     objParametriAgenda.Cul_Cod,
                                                     objParametriAgenda.Data,
                                                     0,
                                                     0,
                                                     0,
                                                     filtroImpianto, " Cul_Des, App_Nome, Progetto ",
                                                     objParametri_Server, True, filtraSpecie)
            '----------------------------------
            'lettura zone vulnerabili
            Dim DtPV As DataTable
            Dim objPV As New AgronicaCoreAnagrafeDAL.ZonexParticelle_R
            DtPV = objPV.Leggi(-17,
                                           "", "", "", 0, 0, "",
                                            AgronicaCoreParametri.enumSelezioneVariabile.Selezione_TabellaCompleta,
                                            "",
                                            "", objParametri_Server)

            '----------------------------------------
            'lettura fasce (la metto in un try catch in caso non esista la tabella nel DB PianoConcimazione_Pua e di conseguenza la vista)
            Dim DtPVF As DataTable
            Try
                Dim objPVF As New AgronicaCoreMetaSchemaDAL.ParticelleCatastali_Vulnerabili_R
                DtPVF = objPVF.Leggi("", "", "", 0, 0, "", 0,
                                                 " Fascia_Cod <>0 ",
                                                 "", objParametri_Server)
            Catch ex As Exception

            End Try

            Select Case rbl_Specie.SelectedValue
                Case "1"
                    CaricaImpiantiConSpecie(objParametriAgenda.Piva, objParametriAgenda.Sa_Cod, objParametriAgenda.Data, objParametriAgenda.Veg_Cod.Split("/")(0), Dt_Impianti, filtroImpianto, Icona_INFO, DtParticelle, DtPV, DtPVF, filtraSpecie)
                Case "2"
                    CaricaImpiantiTerreniNudi(objParametriAgenda.Piva, objParametriAgenda.Sa_Cod, objParametriAgenda.Data, Dt_Impianti, Icona_INFO, DtParticelleTN, DtPV, DtPVF, filtroImpianto)
                Case Else
                    CaricaImpiantiTerreniNudi(objParametriAgenda.Piva, objParametriAgenda.Sa_Cod, objParametriAgenda.Data, Dt_Impianti, Icona_INFO, DtParticelleTN, DtPV, DtPVF, filtroImpianto)
                    CaricaImpiantiConSpecie(objParametriAgenda.Piva, objParametriAgenda.Sa_Cod, objParametriAgenda.Data, objParametriAgenda.Veg_Cod.Split("/")(0), Dt_Impianti, filtroImpianto, Icona_INFO, DtParticelle, DtPV, DtPVF, filtraSpecie)
            End Select

        End If


        Dim Veg_Des As String = Master_Operazione.Property_ComboSpecie.Testo_Combo
        Dim Veg_Cod As String = Master_Operazione.Property_ComboSpecie.Valore_Combo.Split("/")(0)
        'For i = 0 To Dt_Impianti.Rows.Count - 1
        '    Dt_Impianti.Rows(i).Item("Veg_Des") = Veg_Des
        '    Dt_Impianti.Rows(i).Item("Veg_Cod") = Veg_Cod
        'Next
        Session("Dt_Impianti") = Dt_Impianti

        'End If

    End Sub

    Private Sub caricaDtPlanning()

        'HACK: non necessario, la struttura viene creata da: CopiaPlanning_Su_DT_Impianti
        'Dt_Impianti = InizializzaDataTablePerGridViewImpianti()

        Dim Icona_INFO As String = "<img src='../../AB_Immagini/Icone16/cI.ico' border='0'>"



        Dim DT_Campi As New DataTable
        Dim DT_Appezzamenti As New DataTable
        Dim DT_Particelle As New DataTable
        Dim DT_Intersezioni As New DataTable
        Dim DT_Appezzamenti_Eliminati As New DataTable
        Dim ElencoAppezzamenti As String

        Dim intDummy As Integer

        Dim Validita_Inizio As Date
        Dim Validita_Fine As Date
        Dim ErrMSG As String = ""
        Dim Sa_Cod As Integer = 0
        Dim Tipo_Pianificazione As Integer = 0
        Dim TuttiCentri As Boolean = True

        Dim Qs_ProgrammazioneCod As Integer
        Qs_ProgrammazioneCod = objParametriAgenda.Programmazione_Cod


        Dim objP As New AgronicaCoreAnagrafeBIZ.Programmazione_R
        objP.DT_Appezzamenti_Crea(DT_Appezzamenti)
        objP.DT_Intersezioni_Crea(DT_Intersezioni)
        objP.DT_Appezzamenti_Eliminati_Crea(DT_Appezzamenti_Eliminati)

        Dim xFiltroAggiuntivo_Programmazione_entita As String =
            "  Programmazione_entita.veg_cod = " & Master_Operazione.Property_ComboSpecie.Valore_Combo &
            "  AND " & AgronicaCoreDataProvider.UtilityProvider.Agro_SQL_SaveDate(Master_Operazione.Property_txt_DataOperazione.Text) & " BETWEEN Programmazione_entita.Validita_inizio and Programmazione_Entita.Validita_fine "

        'TODO: Capire come deve funzionare ...
        'If Master_Operazione.Property_ComboCampo.Valori_Combo.Count > 0 Then
        '    xFiltroAggiuntivo_Programmazione_entita &=
        '        " AND   cast(Programmazione_Entita.Piva as varchar(100)) + '/' +  " & _
        '        "       cast(Programmazione_Entita.Sa_cod as varchar(100)) + '/' + " & _
        '        "       cast(Programmazione_Entita.Campo_Cod as varchar(100)) " & _
        '        "in (" & _
        '        "'" & String.Join(",", ComboCampo.Valori_Combo.ToArray).Replace(",", "','") & "'" & _
        '    ")"
        'End If

        intDummy = objP.Pianificazione_Leggi(Qs_ProgrammazioneCod,
                                      "",
                                      "",
                                      objParametriAgenda.Piva,
                                      Sa_Cod,
                                      "",
                                      Validita_Inizio,
                                      Validita_Fine,
                                      Tipo_Pianificazione,
                                      DT_Appezzamenti,
                                      DT_Intersezioni,
                                      DT_Appezzamenti_Eliminati,
                                      TuttiCentri,
                                      "",
                                      AGRODATAINIZIO,
                                      "",
                                      objParametri_Server,
                                      xFiltroAggiuntivo_Programmazione_entita)



        CopiaPlanning_Su_DT_Impianti(DT_Appezzamenti)
        Session("Dt_Impianti") = Dt_Impianti

    End Sub



    Private Sub CopiaPlanning_Su_DT_Impianti(ByVal Dt As DataTable)

        Dt.Columns.Add("Interbina", GetType(String))
        Dt.Columns.Add("Germinabilita", GetType(String))
        Dt.Columns.Add("Info", GetType(String))

        'TODO: Attenzione a come vengono valorizzati...
        Dt.Columns.Add("Regolamento", GetType(Integer))
        Dt.Columns.Add("Disciplinare", GetType(String))
        Dt.Columns.Add("ProgettoCod", GetType(Integer))
        Dt.Columns.Add("P_Ha", GetType(Integer))
        Dt.Columns.Add("Mat_Cod", GetType(Integer))

        Dt.Columns.Add("Piva_Lotto", GetType(String))
        Dt.Columns.Add("Sa_Cod_Lotto", GetType(Integer))
        Dt.Columns.Add("Mat_Cod_Lotto", GetType(Integer))
        Dt.Columns.Add("Sem_Cod_Lotto", GetType(Integer))
        Dt.Columns.Add("Descrizione_Lotto", GetType(String))
        Dt.Columns.Add("Cul_Cod_Lotto", GetType(Integer))
        Dt.Columns.Add("Veg_Cod_Lotto", GetType(Integer))
        Dt.Columns.Add("Udm_Cod_Lotto", GetType(Integer))
        Dt.Columns.Add("Giacenza_Lotto", GetType(String))
        Dt.Columns.Add("Cul_Des_Lotto", GetType(String))
        Dt.Columns.Add("Udm_Sim_Lotto", GetType(String))
        Dt.Columns.Add("Qta_Lotto", GetType(String))
        Dt.Columns.Add("Lotto_Lotto", GetType(String))
        Dt.Columns.Add("Bolla_Lotto", GetType(String))
        Dt.Columns.Add("Bolla_Doc_Lotto", GetType(String))
        Dt.Columns.Add("Bolla_Des_Lotto", GetType(String))
        Dt.Columns.Add("Bolla_Qta_Lotto", GetType(String))
        Dt.Columns.Add("Bolla_Giacenza_Lotto", GetType(String))

        Dt_Impianti = Dt

    End Sub


    Private Sub CaricaImpiantiTerreniNudi(ByVal Piva As String, ByVal Sa_cod As Integer, ByVal Data_Selezionata As Date, ByVal dt As DataTable, ByVal Icona_INFO As String,
                                          ByVal DtParticelle As DataTable, ByVal DtPV As DataTable, ByVal DtPVF As DataTable, ByVal filtro As String)

        Dim esisteImpBloccato As Boolean = False

        Dim Rs As DataTable = New AgronicaCoreAnagrafeDAL.Reg_Impianti_Read().Leggi_ImpiantiConTerrenoNudo_PerSemina(Piva, Sa_cod, Data_Selezionata, False, filtro, "", objParametri_Server)

        If Not IsNothing(Rs) Then

            For Each riga As DataRow In Rs.Rows

                'Creo una nuova riga
                Dim dr As DataRow = dt.NewRow

                'Definisco i valori
                dr.Item("Piva") = riga.Item("Piva")
                dr.Item("Rag_Soc") = riga.Item("rag_soc")
                dr.Item("Sa_Cod") = CInt(riga.Item("Sa_cod"))
                dr.Item("Sa_Nome") = riga.Item("sa_nome")
                dr.Item("Appezza") = riga.Item("Appezza")
                dr.Item("Id_Reg") = riga.Item("Id_Reg")
                dr.Item("campo_cod") = riga.Item("campo_cod")
                dr.Item("campo_des") = riga.Item("campo_des")
                'dr.Item("App_Nome") = riga.Item("App_Nome") & " (" & If(riga.Item("DestinazioneTerreniNudi_Des") <> "", riga.Item("DestinazioneTerreniNudi_Des"), "Terreno Nudo") & ")"
                dr.Item("App_Nome") = riga.Item("App_Nome")  '& " (" & If(riga.Item("DestinazioneTerreniNudi_Des") <> "", riga.Item("DestinazioneTerreniNudi_Des"), "Terreno Nudo") & ")"
                dr.Item("Sup_App") = If(ViewState("Utilizza_SupApp"), riga.Item("Sup_App"), riga.Item("Sup_Imp"))
                dr.Item("Cul_Cod") = riga.Item("Cul_Cod")
                dr.Item("Veg_Cod") = 0
                dr.Item("Veg_Des_attuale") = riga.Item("veg_des") & If(riga.Item("EsisteSeminaTrapianto") = "1", " (" & Resources.AgronicaAgenda_2010.GIASEMINATO & ")", "")
                dr.Item("Veg_Des") = ""
                dr.Item("Grfi_Cod") = If(IsDBNull(riga.Item("Grfi_Cod")), 0, riga.Item("Grfi_Cod"))
                dr.Item("Regolamento") = riga.Item("Regolamento_Cod")
                dr.Item("Disciplinare") = riga.Item("Disciplinare_Cod") & "/" & riga.Item("Disciplinare_PubblicoPrivato")
                dr.Item("ProgettoCod") = riga.Item("progetto_cod")
                dr.Item("Distinta") = If(CInt(riga.Item("progetto_cod")) <> 0, riga.Item("progetto_nome"), "-----")

                'todo, verificare stringa
                dr.Item("Info") = "<a title='" & Resources.AgronicaAgenda_2010.DataImpianto & riga.Item("Validita_Inizio") & vbCrLf &
                                      Resources.AgronicaAgenda_2010.Regolamento & riga.Item("Reg_Des") & "' >" & Icona_INFO & "</a>"

                dr.Item("P_Ha") = If(IsDBNull(riga.Item("p_ha")), 0, riga.Item("p_ha"))
                dr.Item("Tra_Fila") = ""
                dr.Item("Su_Fila") = ""
                dr.Item("Interbina") = ""
                dr.Item("Germinabilita") = ""

                dr.Item("Validita_Inizio") = riga.Item("Validita_Inizio")
                dr.Item("Validita_Fine") = riga.Item("Validita_Fine")
                dr.Item("Validita_Inizio_App") = riga.Item("Validita_Inizio_App")
                dr.Item("Validita_Fine_App") = riga.Item("Validita_Fine_App")
                dr.Item("Validita_Inizio_Distinta") = riga.Item("Validita_Inizio_Distinta")
                dr.Item("Validita_Fine_Distinta") = riga.Item("Validita_Fine_Distinta")

                dr.Item("MetodoProduttivo") = riga.Item("MetodoProduttivo")
                '-----------------------------------------
                'aggiunta indicazione catasto (06/06/2018)
                Dim strCatasto As String = ""
                Dim DrParticelle() As DataRow
                Dim DrPV() As DataRow
                Dim DrPVFA() As DataRow
                Dim DrPVFB() As DataRow
                Dim p As Integer

                Dim strParticella As String
                Dim strVulnerabile As String

                If Not DtParticelle Is Nothing AndAlso DtParticelle.Rows.Count > 0 Then
                    DrParticelle = DtParticelle.Select("piva='" & riga.Item("Piva").ToString & "' and sa_cod=" & CInt(riga.Item("Sa_cod")).ToString & " and appezza=" & riga.Item("Appezza").ToString & " and id_reg=" & riga.Item("Id_Reg").ToString)
                    If Not DrParticelle Is Nothing AndAlso DrParticelle.Length > 0 Then
                        For p = 0 To DrParticelle.Length - 1
                            strParticella = DrParticelle(p).Item("prov") & "_" & DrParticelle(p).Item("com") & "_" & DrParticelle(p).Item("sezione") & "_" & DrParticelle(p).Item("foglio") & "_" & DrParticelle(p).Item("numero") & "_" & DrParticelle(p).Item("subalterno")
                            strVulnerabile = ""
                            If Not DtPV Is Nothing AndAlso DtPV.Rows.Count > 0 Then
                                DrPV = DtPV.Select("prov='" & DrParticelle(p).Item("prov").ToString & "' AND Com='" & DrParticelle(p).Item("com").ToString & "' AND Sezione='" & DrParticelle(p).Item("sezione").ToString & "' AND Foglio=" & DrParticelle(p).Item("foglio").ToString & " AND Numero=" & DrParticelle(p).Item("numero").ToString & " AND subalterno='" & DrParticelle(p).Item("subalterno").ToString & "'")
                                If Not DrPV Is Nothing AndAlso DrPV.Length > 0 Then
                                    strVulnerabile = " <b>(V)</b>"
                                End If
                            End If
                            If Not DtPVF Is Nothing AndAlso DtPVF.Rows.Count > 0 Then
                                DrPVFA = DtPVF.Select("prov='" & DrParticelle(p).Item("prov").ToString & "' AND Com='" & DrParticelle(p).Item("com").ToString & "' AND Foglio=" & DrParticelle(p).Item("foglio").ToString & " AND Fascia_Cod=1")
                                If Not DrPVFA Is Nothing AndAlso DrPVFA.Length > 0 Then
                                    strVulnerabile = " <b>(V - Fascia A)</b>"
                                End If
                                DrPVFB = DtPVF.Select("prov='" & DrParticelle(p).Item("prov").ToString & "' AND Com='" & DrParticelle(p).Item("com").ToString & "' AND Foglio=" & DrParticelle(p).Item("foglio").ToString & " AND Fascia_Cod=2")
                                If Not DrPVFB Is Nothing AndAlso DrPVFB.Length > 0 Then
                                    strVulnerabile = " <b>(V - Fascia B)</b>"
                                End If
                            End If
                            strCatasto &= strParticella & strVulnerabile & "<br>"
                        Next
                    End If
                End If
                If strCatasto <> "" Then
                    strCatasto = Left(strCatasto, strCatasto.Length - 4)
                End If

                dr.Item("catasto") = strCatasto



                'Verifico se l'appezzamento è bloccato sul palmare

                If CInt(riga.Item("Blk_Flag")) = 0 Then 'Associo alla tabella la nuova riga creata
                    dt.Rows.Add(dr) 'Se non è bloccato aggiungo la datarow al datatable degli impianti
                Else

                    If objParametriAgenda.Tipo_Operazione = enum_TipoOperazioneDB.Lettura Then
                        'se c'è impianto bloccato e sono in lettura proseguo
                        dt.Rows.Add(dr)
                    ElseIf objParametriAgenda.Tipo_Operazione = enum_TipoOperazioneDB.Modifica Then
                        'se sono in modifica e un impianto e bloccato imposto l'operazione come lettura per impedire modifiche
                        esisteImpBloccato = True
                        objParametriAgenda.Tipo_Operazione = enum_TipoOperazioneDB.Lettura
                        dt.Rows.Add(dr)

                    ElseIf objParametriAgenda.Tipo_Operazione = enum_TipoOperazioneDB.Scrittura Then
                        'se sono in scrittura non aggiungo l'impianto e mando un messaggio
                        esisteImpBloccato = True
                        If PERMETTI_OPERAZIONI_SU_IMPIANTI_BLOCCATI Then
                            dt.Rows.Add(dr)
                        End If


                    End If
                End If

            Next

        End If

        If esisteImpBloccato Then

            If PERMETTI_OPERAZIONI_SU_IMPIANTI_BLOCCATI Then
                If objParametriAgenda.Tipo_Operazione = enum_TipoOperazioneDB.Modifica Or
                   objParametriAgenda.Tipo_Operazione = enum_TipoOperazioneDB.Scrittura Then
                    Messaggi.AgroMsgBox("Alcuni impianti sono bloccati in anagrafica in quanto sincronizzati o scaricati, l'operazione di Semina/Trapianto verrà comunque registrata, ma i dati dell'impianto come la varietà, finalità, regolamento, sesto etc non verranno modificati.", Page, , Master_Operazione.Property_UpdatePanelToolBar)
                End If
            Else
                If objParametriAgenda.Tipo_Operazione = enum_TipoOperazioneDB.Modifica Then
                    Messaggi.AgroMsgBox(Resources.AgronicaAgenda_2010.AlcuniImpiantiSonoStatiScaricatiSuPalmareL, Page, , Master_Operazione.Property_UpdatePanelToolBar)
                End If
                If objParametriAgenda.Tipo_Operazione = enum_TipoOperazioneDB.Scrittura Then
                    Messaggi.AgroMsgBox(Resources.AgronicaAgenda_2010.AlcuniImpiantiSonoStatiScaricatiSuPalmareE, Page, , Master_Operazione.Property_UpdatePanelToolBar)
                End If
            End If

        End If

    End Sub


    Private Sub CaricaImpiantiConSpecie(ByVal Piva As String, ByVal Sa_cod As Integer,
                                        ByVal Data_Selezionata As Date,
                                        ByVal Veg_Cod As Integer,
                                        ByVal dt As DataTable,
                                        ByVal filtro As String,
                                        ByVal Icona_INFO As String,
                                          ByVal DtParticelle As DataTable, ByVal DtPV As DataTable, ByVal DtPVF As DataTable,
                                                Optional ByVal filtraSpecie As Boolean = True)

        Dim esisteImpBloccato As Boolean = False

        ''Impianti con la specie selezionata...
        Dim Rs As DataTable = New AgronicaCoreAnagrafeDAL.Reg_Impianti_Read().Leggi_ImpiantiConSpecieSelezionata_PerSemina(
                                                Piva, Sa_cod, Data_Selezionata, Veg_Cod, filtro, "", objParametri_Server, filtraSpecie)

        If Not IsNothing(Rs) Then

            For Each riga As DataRow In Rs.Rows

                'controllo che non sia già stata registrata un'operazione di semina su ogni impianto
                'se non lo è carico l'impianto nel datatable...
                'If Not EsisteSeminaTrapianto(Piva, Sa_cod, Rs("Appezza"), Rs("Id_Reg")) Then

                'Creo una nuova riga
                Dim dr As DataRow = dt.NewRow

                'Dim Stato_Impianto_Cod As Integer = dtp.Rows(0).Item("Stato_Impianto")
                'Dim Stato_Impianto_Des As String = dtp.Rows(0).Item("Grfi_Des")
                'Dim Cau_Progetto As Integer = dtp.Rows(0).Item("cau_progetto")

                dr.Item("Piva") = riga.Item("Piva")
                dr.Item("Rag_Soc") = riga.Item("rag_soc")
                dr.Item("Sa_Cod") = CInt(riga.Item("Sa_cod"))
                dr.Item("Sa_Nome") = riga.Item("sa_nome")
                dr.Item("Appezza") = riga.Item("Appezza")
                dr.Item("Id_Reg") = riga.Item("Id_Reg")
                dr.Item("campo_cod") = riga.Item("campo_cod")
                dr.Item("campo_des") = riga.Item("campo_des")
                dr.Item("veg_des_attuale") = riga.Item("veg_des") & If(riga.Item("EsisteSeminaTrapianto") = "1", " (" & Resources.AgronicaAgenda_2010.GIASEMINATO & ")", "")
                dr.Item("veg_des") = ""
                'dr.Item("App_Nome") = riga.Item("App_Nome") & " (" & riga.Item("Veg_Des") & ": " & riga.Item("Cul_Des") & ")" & If(riga.Item("EsisteSeminaTrapianto") = "1", Resources.AgronicaAgenda_2010.GIASEMINATO, "")
                dr.Item("App_Nome") = riga.Item("App_Nome")

                dr.Item("Sup_App") = If(ViewState("Utilizza_SupApp"), riga.Item("Sup_App"), riga.Item("Sup_Imp"))
                dr.Item("Cul_Cod") = riga.Item("Cul_Cod")
                dr.Item("Veg_Cod") = riga.Item("Veg_Cod")
                dr.Item("Grfi_Cod") = If(Not IsDBNull(riga.Item("Grfi_Cod")), riga.Item("Grfi_Cod"), 0)
                dr.Item("Distinta") = If(riga.Item("progetto_cod") <> 0, riga.Item("progetto_nome"), "-----")
                dr.Item("Regolamento") = riga.Item("Regolamento_Cod")
                dr.Item("Disciplinare") = riga.Item("Disciplinare_Cod") & "/" & riga.Item("Disciplinare_PubblicoPrivato")
                dr.Item("ProgettoCod") = riga.Item("progetto_cod")
                dr.Item("P_Ha") = If(Not IsDBNull(riga.Item("p_ha")), riga.Item("p_ha"), 0)
                dr.Item("Info") = "<a title='" & Resources.AgronicaAgenda_2010.DataImpianto & riga.Item("Validita_Inizio") & vbCrLf &
                                       Resources.AgronicaAgenda_2010.Regolamento & riga.Item("Reg_Des") & "' >" & Icona_INFO & "</a>"

                dr.Item("Tra_Fila") = riga.Item("Tra_Fila")
                dr.Item("Su_Fila") = riga.Item("Su_Fila")
                dr.Item("Interbina") = riga.Item("Interbina")
                dr.Item("Germinabilita") = riga.Item("Germinabilita")


                dr.Item("Validita_Inizio") = riga.Item("Validita_Inizio")
                dr.Item("Validita_Fine") = riga.Item("Validita_Fine")
                dr.Item("Validita_Inizio_App") = riga.Item("Validita_Inizio_App")
                dr.Item("Validita_Fine_App") = riga.Item("Validita_Fine_App")
                dr.Item("Validita_Inizio_Distinta") = riga.Item("Validita_Inizio_Distinta")
                dr.Item("Validita_Fine_Distinta") = riga.Item("Validita_Fine_Distinta")

                dr.Item("MetodoProduttivo") = riga.Item("MetodoProduttivo")

                '-----------------------------------------
                'aggiunta indicazione catasto (06/06/2018)
                Dim strCatasto As String = ""
                Dim DrParticelle() As DataRow
                Dim DrPV() As DataRow
                Dim DrPVFA() As DataRow
                Dim DrPVFB() As DataRow
                Dim p As Integer

                Dim strParticella As String
                Dim strVulnerabile As String

                If Not DtParticelle Is Nothing AndAlso DtParticelle.Rows.Count > 0 Then
                    DrParticelle = DtParticelle.Select("piva='" & riga.Item("Piva").ToString & "' and sa_cod=" & CInt(riga.Item("Sa_cod")).ToString & " and appezza=" & riga.Item("Appezza").ToString & " and id_reg=" & riga.Item("Id_Reg").ToString)
                    If Not DrParticelle Is Nothing AndAlso DrParticelle.Length > 0 Then
                        For p = 0 To DrParticelle.Length - 1
                            strParticella = DrParticelle(p).Item("prov") & "_" & DrParticelle(p).Item("com") & "_" & DrParticelle(p).Item("sezione") & "_" & DrParticelle(p).Item("foglio") & "_" & DrParticelle(p).Item("numero") & "_" & DrParticelle(p).Item("subalterno")
                            strVulnerabile = ""
                            If Not DtPV Is Nothing AndAlso DtPV.Rows.Count > 0 Then
                                DrPV = DtPV.Select("prov='" & DrParticelle(p).Item("prov").ToString & "' AND Com='" & DrParticelle(p).Item("com").ToString & "' AND Sezione='" & DrParticelle(p).Item("sezione").ToString & "' AND Foglio=" & DrParticelle(p).Item("foglio").ToString & " AND Numero=" & DrParticelle(p).Item("numero").ToString & " AND subalterno='" & DrParticelle(p).Item("subalterno").ToString & "'")
                                If Not DrPV Is Nothing AndAlso DrPV.Length > 0 Then
                                    strVulnerabile = " <b>(V)</b>"
                                End If
                            End If
                            If Not DtPVF Is Nothing AndAlso DtPVF.Rows.Count > 0 Then
                                DrPVFA = DtPVF.Select("prov='" & DrParticelle(p).Item("prov").ToString & "' AND Com='" & DrParticelle(p).Item("com").ToString & "' AND Foglio=" & DrParticelle(p).Item("foglio").ToString & " AND Fascia_Cod=1")
                                If Not DrPVFA Is Nothing AndAlso DrPVFA.Length > 0 Then
                                    strVulnerabile = " <b>(V - Fascia A)</b>"
                                End If
                                DrPVFB = DtPVF.Select("prov='" & DrParticelle(p).Item("prov").ToString & "' AND Com='" & DrParticelle(p).Item("com").ToString & "' AND Foglio=" & DrParticelle(p).Item("foglio").ToString & " AND Fascia_Cod=2")
                                If Not DrPVFB Is Nothing AndAlso DrPVFB.Length > 0 Then
                                    strVulnerabile = " <b>(V - Fascia B)</b>"
                                End If
                            End If
                            strCatasto &= strParticella & strVulnerabile & "<br>"
                        Next
                    End If
                End If
                If strCatasto <> "" Then
                    strCatasto = Left(strCatasto, strCatasto.Length - 4)
                End If

                dr.Item("catasto") = strCatasto


                '------------------------------------------------------------------------------------------
                'Verifico se l'appezzamento è bloccato sul palmare

                If CInt(riga.Item("Blk_Flag")) = 0 Then 'Associo alla tabella la nuova riga creata
                    dt.Rows.Add(dr) 'Se non è bloccato aggiungo la datarow al datatable degli impianti
                Else

                    If objParametriAgenda.Tipo_Operazione = enum_TipoOperazioneDB.Lettura Then
                        'se c'è impianto bloccato e sono in lettura proseguo
                        dt.Rows.Add(dr)
                    ElseIf objParametriAgenda.Tipo_Operazione = enum_TipoOperazioneDB.Modifica Then
                        'se sono in modifica e un impianto e bloccato imposto l'operazione come lettura per impedire modifiche
                        esisteImpBloccato = True
                        objParametriAgenda.Tipo_Operazione = enum_TipoOperazioneDB.Lettura
                        dt.Rows.Add(dr)

                    ElseIf objParametriAgenda.Tipo_Operazione = enum_TipoOperazioneDB.Scrittura Then
                        'se sono in scrittura non aggiungo l'impianto e mando un messaggio
                        esisteImpBloccato = True
                        If PERMETTI_OPERAZIONI_SU_IMPIANTI_BLOCCATI Then
                            dt.Rows.Add(dr)
                        End If


                    End If
                End If

                '------------------------------------------------------------------------------------------

            Next

        End If

        If esisteImpBloccato Then

            If PERMETTI_OPERAZIONI_SU_IMPIANTI_BLOCCATI Then
                If objParametriAgenda.Tipo_Operazione = enum_TipoOperazioneDB.Modifica Or
                   objParametriAgenda.Tipo_Operazione = enum_TipoOperazioneDB.Scrittura Then
                    Messaggi.AgroMsgBox("Alcuni impianti sono bloccati in anagrafica in quanto sincronizzati o scaricati, l'operazione di Semina/Trapianto verrà comunque registrata, ma i dati dell'impianto come la varietà, finalità, regolamento, sesto etc non verranno modificati.", Page, , Master_Operazione.Property_UpdatePanelToolBar)
                End If
            Else
                If objParametriAgenda.Tipo_Operazione = enum_TipoOperazioneDB.Modifica Then
                    Messaggi.AgroMsgBox(Resources.AgronicaAgenda_2010.AlcuniImpiantiSonoStatiScaricatiSuPalmareL, Page, , Master_Operazione.Property_UpdatePanelToolBar)
                End If
                If objParametriAgenda.Tipo_Operazione = enum_TipoOperazioneDB.Scrittura Then
                    Messaggi.AgroMsgBox(Resources.AgronicaAgenda_2010.AlcuniImpiantiSonoStatiScaricatiSuPalmareE, Page, , Master_Operazione.Property_UpdatePanelToolBar)
                End If
            End If

        End If

    End Sub


    Private Sub caricaDtGiacenze()

        Dim Dr As DataRow
        Dim Dt_new As New DataTable

        Dim Dt_TS As New DataTable
        Dim i As Integer

        Dim Filtro As String = ""

        Dt_Magazzino = New DataTable
        aggiungiColonneLotti(Dt_Magazzino)

        If objParametriAgenda.Fabbricato <> "0" Then

            Me.LabelInfo1.Visible = True
            Me.LabelInfo2.Visible = True
            ImageInfo1.Visible = True
            ImageInfo2.Visible = True

            Me.CheckBoxGiacenzePositive.Visible = True

            'Applico il filtro sulla tipologia di semente e sulla specie
            Select Case objParametriAgenda.Lav_Cod

                Case LAVCOD_SOVESCIO
                    'con il sovescio devo caricare le erbacee di tutte le specie
                    Filtro = " AND Materie_Prime.Sem_Cod IN (1,3,7) "

                Case LAVCOD_SEMINA, LAVCOD_SOD_SEDDING
                    Filtro = " AND Materie_Prime.Sem_Cod IN (1,3,7) "

                Case LAVCOD_TRAPIANTO
                    Filtro = " AND Materie_Prime.Sem_Cod IN (2,3,4,5,6,7,8,10) "

            End Select

            If objParametriAgenda.Tipo_Operazione = enum_TipoOperazioneDB.Scrittura And CheckBoxSeminaMultiSpecie.Checked = False Then
                Filtro &= " AND Materie_Prime.Veg_Cod = " & Agro_SQL_SaveNum(objParametriAgenda.Veg_Cod) & " "
            End If


            ''lista impianti, in questa operazione serve solo per memorizzare l'impianto quandol'operazione è
            ''chiamata dall'albero anagrafe e deve gestire un solo impianto. 
            ''se contiene un impianto allora faccio vedere solo le varietà specificate
            ''perchè sicuramente arrivo dall'albero anagrafe. non sono previsti altrimenti valori nella lista
            'If objParametriAgenda.Impianti.Count = 1 Then
            '    Dim culcodimp As Integer = New AgronicaCoreAnagrafeDAL.Reg_Impianti_Read().CulCod_from_PivaSaCodAppezzaIdimp(objParametriAgenda.Piva, objParametriAgenda.Impianti(0).Sa_Cod, objParametriAgenda.Impianti(0).Appezza, objParametriAgenda.Impianti(0).ID_Reg, objParametri_Server)
            '    If culcodimp <> 0 Then
            '        Dim culdes As String = New AgronicaCoreMetaSchemaDAL.Cultivar_R().CulDes_from_CulCod(culcodimp, objParametri_Server)
            '        If Not culdes.Contains("altre") And Not culdes.Contains("Altre") Then

            '            Filtro &= "AND Materie_Prime.Cul_Cod  = " & culcodimp & "  "

            '        End If
            '    End If
            'End If



            Dim objGIACENZE As New AgronicaCoreContabDAL.Giacenze_R
            Dim objStampeDal As New AgronicaCoreStampeDAL.Magazzino

            'gestisco caso magazzino esterbo o no
            If Split(objParametriAgenda.Fabbricato, "|")(2) <> objParametriAgenda.Piva Then
                Try
                    Dim MatriceIdAgendaOld(,) As String
                    Dim objRif As New AgronicaCoreContabDAL.Mov_Dettagli_Riferimenti_R
                    MatriceIdAgendaOld = objRif.MatriceChiaviAgenda_MovRiferiti(objParametriAgenda.Piva,
                                                                                  objParametriAgenda.Sa_Cod,
                                                                                  objParametriAgenda.Id_Agenda,
                                                                                  "",
                                                                                  objParametri_Server)
                    Dim Id_Agende As String = ""
                    For i = 0 To UBound(MatriceIdAgendaOld, 2)
                        If i = 0 Then
                            Id_Agende = MatriceIdAgendaOld(2, i)
                        Else
                            Id_Agende = Id_Agende & "," & MatriceIdAgendaOld(2, i)
                        End If

                    Next
                    If Id_Agende <> "" Then
                        Filtro = Filtro & " and Agenda.Id_Agenda not in (" & Id_Agende & ") "
                    End If

                Catch ex As Exception

                End Try

            Else
                'magazzino interno, uso id_agenda
                Filtro = Filtro & " and Agenda.Id_Agenda <>  " & objParametriAgenda.Id_Agenda
            End If


            Dim Giacenze As DataTable

            If Split(objParametriAgenda.Fabbricato, "|")(2) = objParametriAgenda.Piva AndAlso
                Not IsNothing(Session("MovimentiDettagliDDT")) Then
                '---------------------------------------------------------------------------------------------------
                '--------GESTIONE DELLA BOLLA COLLEGATA--------------------------------------------------------------
                '-------------------------------------------------------------------------------------------
                'caso con ddt da allegare
                Dim MovimentiDettagliDDT As List(Of Movimento_Dettaglio) = Session("MovimentiDettagliDDT")
                If MovimentiDettagliDDT.Count > 0 Then


                    Dim ids_agenda(MovimentiDettagliDDT.Count - 1) As String
                    Dim ids_movimenti(MovimentiDettagliDDT.Count - 1) As String
                    Dim ids_movimenti_dett(MovimentiDettagliDDT.Count - 1) As String
                    Dim kkk As Integer = 0
                    For Each mov As Movimento_Dettaglio In MovimentiDettagliDDT
                        ids_agenda(kkk) = mov.Id_Agenda
                        ids_movimenti(kkk) = mov.Id_Mov
                        ids_movimenti_dett(kkk) = mov.Id_Mov_Det
                        kkk = kkk + 1
                    Next

                    Giacenze = objStampeDal.SchedaGiacenzeMagazzinoSementiConBolle(objParametriAgenda.Data,
                                Split(objParametriAgenda.Fabbricato, "|")(2),
                                CInt(Split(objParametriAgenda.Fabbricato, "|")(1)),
                                CInt(Split(objParametriAgenda.Fabbricato, "|")(0)),
                                SEMENTI,
                                0,
                                0,
                                0, 0, 0, 0, LOTTO_NONDEFINITO,
                                Me.CheckBoxGiacenzePositive.Checked,
                                ids_agenda, ids_movimenti, ids_movimenti_dett,
                                Filtro,
                                objParametri_Server)



                    '---------------------------------------------------------------------------------------------------
                    '--------FINE GESTIONE DELLA BOLLA COLLEGATA--------------------------------------------------------------
                    '-------------------------------------------------------------------------------------------


                Else
                    'caso solito, ignoro ddt se è vuoto, e canello da sessione
                    Session("MovimentiDettagliDDT") = Nothing
                    ImageInfo0.Visible = False
                    LabelInfo0.Visible = False
                    'avverto
                    Messaggi.AgroMsgBox(Resources.AgronicaAgenda_2010.NessunDettaglioSuDdtÈStatoImportato, Page, ,
                                Master_Operazione.Property_UpdatePanelToolBar)
                    Giacenze = objGIACENZE.SchedaGiacenzeMagazzino(objParametriAgenda.Data,
                                    Split(objParametriAgenda.Fabbricato, "|")(2),
                                    CInt(Split(objParametriAgenda.Fabbricato, "|")(1)),
                                    CInt(Split(objParametriAgenda.Fabbricato, "|")(0)),
                                    SEMENTI,
                                    0,
                                    0,
                                    0, 0, 0, 0, LOTTO_NONDEFINITO,
                                    Me.CheckBoxGiacenzePositive.Checked,
                                    "", "", "", "", "", "", "", Filtro, "", "", "",
                                    "",
                                    objParametri_Server, objParametri_Utenti)
                End If
            Else


                'caso solito
                Giacenze = objGIACENZE.SchedaGiacenzeMagazzino(objParametriAgenda.Data,
                                Split(objParametriAgenda.Fabbricato, "|")(2),
                                CInt(Split(objParametriAgenda.Fabbricato, "|")(1)),
                                CInt(Split(objParametriAgenda.Fabbricato, "|")(0)),
                                SEMENTI,
                                0,
                                0,
                                0, 0, 0, 0, LOTTO_NONDEFINITO,
                                Me.CheckBoxGiacenzePositive.Checked,
                                "", "", "", "", "", "", "", Filtro, "", "", "",
                                "",
                                objParametri_Server, objParametri_Utenti)
            End If




            If Not Giacenze Is Nothing AndAlso Giacenze.Rows.Count > 0 Then

                For i = 0 To Giacenze.Rows.Count - 1

                    Dim Mat_Cod As Integer = Giacenze.Rows(i).Item("Mat_Cod")

                    Dim dte As DataTable = New AgronicaCoreAnagrafeDAL.Materie_Prime_R().Leggi(Split(objParametriAgenda.Fabbricato, "|")(2),
                                                                                             CInt(Split(objParametriAgenda.Fabbricato, "|")(1)),
                                                                                            SEMENTI, Mat_Cod,
                                                                                            "", 0, 0, 0, 0, 0, 0, 0, "", 0, "", False, False, "", enumSelezioneVariabile.Selezione_JoinDescrizioni, "", "", objParametri_Server)
                    If Not dte Is Nothing AndAlso dte.Rows.Count > 0 Then

                        'Creo una nuova riga
                        Dr = Dt_Magazzino.NewRow

                        'Definisco i valori
                        Dr.Item("Piva") = Giacenze.Rows(i).Item("Piva")
                        Dr.Item("Sa_Cod") = Giacenze.Rows(i).Item("Sa_Cod")
                        Dr.Item("Mat_Cod") = Giacenze.Rows(i).Item("Mat_Cod")

                        Dr.Item("Sem_Cod") = dte.Rows(0).Item("Sem_Cod")
                        Dr.Item("Regolamento") = dte.Rows(0).Item("Regolamento")

                        Dr.Item("Mat_Des") = dte.Rows(0).Item("Mat_Des")

                        If Not IsDBNull(dte.Rows(0).Item("Regolamento")) AndAlso dte.Rows(0).Item("Regolamento") = 4 Then
                            Dr.Item("Cod_Articolo") = Giacenze.Rows(i).Item("Cod_Articolo") & " - BIO"
                        Else
                            Dr.Item("Cod_Articolo") = Giacenze.Rows(i).Item("Cod_Articolo")
                        End If

                        Dr.Item("Lotto") = Giacenze.Rows(i).Item("Lotto")

                        Dr.Item("Veg_des") = dte.Rows(0).Item("Veg_Des")
                        Dr.Item("Cul_des") = dte.Rows(0).Item("Cul_Des")

                        Dr.Item("Descrizione") = dte.Rows(0).Item("Mat_Des") + String.Format(Resources.AgronicaAgenda_2010.CodX0LottoX1, Giacenze.Rows(i).Item("Cod_Articolo"), Giacenze.Rows(i).Item("Lotto"))

                        If Not IsDBNull(dte.Rows(0).Item("Regolamento")) AndAlso dte.Rows(0).Item("Regolamento") = 4 Then
                            Dr.Item("Descrizione") &= " - BIO"
                        End If

                        Dr.Item("Cul_Cod") = dte.Rows(0).Item("Cul_Cod")
                        Dr.Item("Veg_Cod") = dte.Rows(0).Item("Veg_Cod")

                        Dr.Item("Udm_Cod") = Giacenze.Rows(i).Item("Udm_Cod")
                        Dr.Item("Giacenza") = Giacenze.Rows(i).Item("Giacenza") & " " & Giacenze.Rows(i).Item("Udm_Sim")
                        Dr.Item("Udm_Sim") = Giacenze.Rows(i).Item("Udm_Sim")
                        Dr.Item("Qta") = Giacenze.Rows(i).Item("Giacenza")

                        If Split(objParametriAgenda.Fabbricato, "|")(2) = objParametriAgenda.Piva AndAlso
                            Not IsNothing(Session("MovimentiDettagliDDT")) Then
                            '---------------------------------------------------------------------------------------------------
                            '-------- GESTIONE DELLA BOLLA COLLEGATA--------------------------------------------------------------
                            '-------------------------------------------------------------------------------------------
                            'se ho i ddt allegati assegno i valori ai campi bolla
                            'md.id_agenda as id_agenda_ddt_det, 
                            'md.id_mov as id_mov_ddt_det, 
                            'md.id_Mov_Det as id_Mov_Det_ddt_det, 
                            'md.qta as qta_ddt_det , 
                            'md.Mov_Det_Des as Mov_Det_Des_ddt_det , 
                            'mov4000.Doc_Numero as Doc_Numero_ddt
                            Dr.Item("Bolla") = Giacenze.Rows(i).Item("id_agenda_ddt_det") & "|" & Giacenze.Rows(i).Item("id_mov_ddt_det") & "|" & Giacenze.Rows(i).Item("id_Mov_Det_ddt_det") & ""
                            Dr.Item("Bolla_Doc") = Giacenze.Rows(i).Item("Doc_Numero_ddt")
                            Dr.Item("Bolla_Des") = Giacenze.Rows(i).Item("Mov_Det_Des_ddt_det")
                            Dr.Item("Bolla_Qta") = Giacenze.Rows(i).Item("qta_ddt_det")
                            Dr.Item("Bolla_Giacenza") = CDbl(Dr.Item("Bolla_Qta")) - New AgronicaCoreContabDAL.Mov_Dettagli_Riferimenti_R().Qta_Usate_Di_DDT_Agganciati_A_Lavorazioni(Giacenze.Rows(i).Item("Piva"), Giacenze.Rows(i).Item("id_agenda_ddt_det"), Giacenze.Rows(i).Item("id_mov_ddt_det"), Giacenze.Rows(i).Item("id_Mov_Det_ddt_det"), objParametri_Server)
                            GridViewMagazzino.Columns(6).Visible = True
                            GridViewMagazzino.Columns(7).Visible = True
                            GridViewMagazzino.Columns(8).Visible = True
                            GridViewMagazzino.Columns(9).Visible = True

                            Dr.Item("Descrizione") = "Documento: " & Dr.Item("Bolla_Doc") & " - " & Dr.Item("Descrizione")
                            '---------------------------------------------------------------------------------------------------
                            '--------FINE GESTIONE DELLA BOLLA COLLEGATA--------------------------------------------------------------
                            '-------------------------------------------------------------------------------------------

                        Else
                            Dr.Item("Bolla") = ""
                            Dr.Item("Bolla_Doc") = ""
                            Dr.Item("Bolla_Des") = ""
                            Dr.Item("Bolla_Qta") = ""
                            Dr.Item("Bolla_Giacenza") = ""
                            GridViewMagazzino.Columns(6).Visible = False
                            GridViewMagazzino.Columns(7).Visible = False
                            GridViewMagazzino.Columns(8).Visible = False
                            GridViewMagazzino.Columns(9).Visible = False
                        End If


                        Dt_Magazzino.Rows.Add(Dr)

                    End If

                Next

            End If

            'GridViewMagazzino.Columns(0).Visible = True
            'GridViewMagazzino.Columns(1).Visible = True
            'GridViewMagazzino.Columns(2).Visible = True


        Else

            Me.LabelInfo1.Visible = False
            Me.LabelInfo2.Visible = False
            ImageInfo1.Visible = False
            ImageInfo2.Visible = False


            Me.CheckBoxGiacenzePositive.Visible = False

        End If

        Session("Dt_Magazzino") = Dt_Magazzino

    End Sub


    Private Sub disabilitaControlli()

        'DivOpzioni.Attributes.Remove("style")

        'controlo il permesso sulla specie, se non ce l'ho metto operazione in lettura
        If objParametriAgenda.Tipo_Operazione = CStr(TipiEnumerativi.enum_TipoOperazioneDB.Modifica) Then
            Try
                Dim objSpecVeg As New AgronicaCoreMetaSchemaDAL.SpecieVegetali_R
                Dim Dt As DataTable = objSpecVeg.SpecieVegetali_GestioneFiltroUtente_Leggi(objParametriAgenda.Veg_Cod.Split("/")(0),
                                                                 0,
                                                                 "",
                                                                 "",
                                                                 "",
                                                                 "",
                                                                 objParametri_Utenti)
                If Dt.Rows.Count = 0 Then
                    objParametriAgenda.Tipo_Operazione = enum_TipoOperazioneDB.Lettura
                End If
            Catch ex As Exception
                objParametriAgenda.Tipo_Operazione = enum_TipoOperazioneDB.Lettura
            End Try
        End If


        Master_Operazione.Property_GridView_Impianti.Visible = False

        'impostazioni in base operazione di creazione/modifica..
        Select Case objParametriAgenda.Tipo_Operazione

            'impostazioni in base alla lavorazione

            Case TipiEnumerativi.enum_TipoOperazioneDB.Scrittura

                Master_Operazione.Property_RBL_Salva.Visible = True
                Master_Operazione.Property_Box_Salva.Visible = True
                Master_Operazione.flag_MostraBtnSalvaCDG = True

                Master_Operazione.Property_ImgBtn_DDT.Visible = True
                Master_Operazione.Property_ImgBtn_DDT_Cancella.Visible = True

                'If Tipo_Semina = enum_SEMINA_TIPO.Con_Frazionamento_perLotto Then
                If RBL_Tipo_Semina.SelectedValue = enum_SEMINA_TIPO.Frazionamento_Appezzamenti_perLotto_e_Semina_sui_Singoli_Default Then
                    Master_Operazione.Property_ImgBtn_DDT.Visible = False
                    Master_Operazione.Property_ImgBtn_DDT_Cancella.Visible = False
                End If

                If objParametriAgenda.Impianti.Count = 1 Then
                    'se c'è un impianto allora vengo dall'albero imprese, allora lascio solo salva ed esci che mi fa tornare nell'albero imporese
                    Master_Operazione.Property_RBL_Salva.Enabled = False
                End If

                'RBL_Tipo_Semina.Items(2).Attributes.Remove("style")

            Case TipiEnumerativi.enum_TipoOperazioneDB.Modifica

                Master_Operazione.Property_Box_Salva.Enabled = True 'true per modifica
                Master_Operazione.Property_Box_Salva.Visible = True 'true per modifica
                Master_Operazione.Property_ImgBtn_Salva.Enabled = True 'true per modifica
                Master_Operazione.Property_RBL_Salva.Enabled = True
                Master_Operazione.flag_MostraBtnSalvaCDG = True
                Master_Operazione.Property_txt_Note.Enabled = True
                Master_Operazione.Property_CBL_Consigli.Enabled = True

                Master_Operazione.Property_ImgBtn_DDT.Visible = True
                Master_Operazione.Property_ImgBtn_DDT_Cancella.Visible = True

                'RBL_Tipo_Semina.Items(2).Attributes.Add("style", "display:none")


            Case TipiEnumerativi.enum_TipoOperazioneDB.Lettura

                AbilitaTutto(False)
                Master_Operazione.Property_ImgBtn_DDT.Visible = False
                Master_Operazione.Property_ImgBtn_DDT_Cancella.Visible = False

                'RBL_Tipo_Semina.Items(2).Attributes.Add("style", "display:none")

        End Select

        If objParametriAgenda.Fabbricato = "0" Then
            DivMagazzino.Visible = False
            CheckBoxSeminaMultiSpecie.Visible = False
        Else
            DivMagazzino.Visible = True
            CheckBoxSeminaMultiSpecie.Visible = True
            Select Case CInt(objParametriAgenda.Lav_Cod)
                Case LAVCOD_SEMINA, LAVCOD_SOD_SEDDING, LAVCOD_SOVESCIO
                    CheckBoxSeminaMultiSpecie.Text = "Semina Multi-Specie"
                Case LAVCOD_TRAPIANTO
                    CheckBoxSeminaMultiSpecie.Text = "Trapianto Multi-Specie"
            End Select
        End If


        If objParametriAgenda.TargetOperazione = enum_Tipo_Operazione_Agenda_Target.Planning Then

            Master_Operazione.Property_ImgBtn_DDT.Visible = False
            Master_Operazione.Property_ImgBtn_DDT_Cancella.Visible = False

        End If

        'If objParametriAgenda.Lav_Cod = LAVCOD_SOVESCIO Or objParametriAgenda.Tipo_Operazione = enum_TipoOperazioneDB.Lettura Or objParametriAgenda.Tipo_Operazione = enum_TipoOperazioneDB.Modifica Then
        '    RBL_Tipo_Semina.SelectedValue = 30
        '    DivOpzioni.Attributes.Add("style", "display:none")
        'End If

    End Sub


    Sub AbilitaTutto(ByVal abilita As Boolean)
        Master_Operazione.Property_Box_Salva.Enabled = abilita 'true per modifica
        Master_Operazione.Property_Box_Salva.Visible = abilita 'true per modifica
        Master_Operazione.Property_ImgBtn_Salva.Enabled = abilita 'true per modifica
        Master_Operazione.Property_RBL_Salva.Enabled = abilita
        Master_Operazione.Property_txt_Note.Enabled = abilita 'true per modifica
        Master_Operazione.Property_CBL_Consigli.Enabled = abilita 'true per modifica
        Master_Operazione.Property_BTN_ComboOperazione.Enabled = abilita
        Master_Operazione.Property_ComboOperazione.Enabled = abilita
        Master_Operazione.Property_ImgBtn_DDT.Enabled = abilita
        Master_Operazione.Property_ImgBtn_DDT_Cancella.Enabled = abilita
        BloccaComboJavaScript(Not abilita, Not abilita, Not abilita, Not abilita)
        GridViewMagazzino.Enabled = abilita
        GridViewImpianti.Enabled = abilita
        rbl_Specie.Enabled = abilita
        'CheckBoxTerrenoNudo.Enabled = False
    End Sub


#End Region


#Region "GestioneCombo e Filtri"

    'Protected Sub CheckBoxTerrenoNudo_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxTerrenoNudo.CheckedChanged
    '    caricaDtImpianti()
    '    caricaListeValoriDaTabelle()
    '    caricaTabelle(False)
    'End Sub

    Protected Sub GridViewMagazzino_SelectedIndexChanged(sender As Object, e As EventArgs) Handles GridViewMagazzino.SelectedIndexChanged

        Dim selezionatutti As Boolean = False


        'se ho selezionato selezione-deseleziona tutti
        'chekko o schekko tutte le righe delle giacenze 
        If sender.id = "chkSelezionaTuttiGiacenze" Then
            selezionatutti = sender.checked
            For i = 0 To GridViewMagazzino.Rows.Count - 1
                CType(GridViewMagazzino.Rows(i).Cells(NumColonna_Check).Controls(1), CheckBox).Checked = selezionatutti
            Next
        End If


        'carico i valori e rigenero le tabelle
        caricaListeValoriDaTabelle()
        caricaTabelle(False)


        'se ho selezionato selezione-deseleziona tutti
        'imposto il checkgiusto nella testa della colonna selezionatutti 
        '(si perdeva essendo rigenerate tutte le tabelle)
        If sender.id = "chkSelezionaTuttiGiacenze" Then
            CType(GridViewMagazzino.HeaderRow.Cells(NumColonna_Check).Controls(1), CheckBox).Checked = selezionatutti
        End If

        CopiaGiacenzeBolleNelleTextbox()

    End Sub

    Private Sub CopiaGiacenzeBolleNelleTextbox()
        Try

            If Not IsNothing(Session("MovimentiDettagliDDT")) Then

                'le caselle di testo della tabella giacenze, in cui impostare la qta da ridistribuire in automatico sugli impianti
                'a questo punto sono tutte con 0, essendo rigenerate le tabelle.
                'Se ho la bolla_giacenza valorizzata, ci copio questo valore, si verifica solo nel caso in cui si allegano le bolle (agribolona)
                'Non ha senso copiarci le giacenze totali dato che di solito in questo coso non si utilizzano tutte per un solo trapianto,
                'quando si importa un dettaglio bolla invece di solito si scarica tutto ed è utile copiarglielo.
                For i = 0 To GridViewMagazzino.Rows.Count - 1
                    Dim bollagiacenza As String = GridViewMagazzino.DataKeys(i).Item("Bolla_Giacenza")
                    If bollagiacenza <> "" AndAlso bollagiacenza <> "0" Then
                        CType(GridViewMagazzino.Rows(i).Cells(GridViewMagazzino.Columns.Count - 1).Controls(1), TextBox).Text = bollagiacenza
                    End If
                Next


                'Dato che il dato non è salvato occorrerebbe comunque copiarlo nella tabella impianti,
                'in questo caso occorre suddividerlo per superficie, come farebbe via javascript.
                'Di solito comunque solo agribologna che usa le bolle usa questa cosa, come detto sopra
                'non avrebbe senso copiare le giacenze totali.
                'Agribologna usa inoltre un solo impianto alla volta e inserisce tutti i dettagli bolla,
                'quindi per semplificare considero solo il caso di un solo impianto selezionato,
                'copio tutti i dettagli negli impianti selezionati
                'si usa un solo impianto
                For i = 0 To GridViewImpianti.Rows.Count - 1
                    Dim bollagiacenza As String = GridViewImpianti.DataKeys(i).Item("Bolla_Giacenza_Lotto")
                    If bollagiacenza <> "" AndAlso
                        bollagiacenza <> "0" AndAlso
                        CType(GridViewImpianti.Rows(i).Cells(GridViewImpianti.Columns.Count - 1).Controls(1), TextBox).Text = "0" Then

                        CType(GridViewImpianti.Rows(i).Cells(GridViewImpianti.Columns.Count - 1).Controls(1), TextBox).Text = bollagiacenza

                    End If
                Next

            End If



        Catch ex As Exception



        End Try
    End Sub
    Private Sub CambioCentro(sender As Object, e As EventArgs)
        'permetto cambio centro senza eliminare ddt allegati
        'Session("MovimentiDettagliDDT") = Nothing
        caricaListePerCombo()

        If objParametriAgenda.TargetOperazione = enum_Tipo_Operazione_Agenda_Target.Reale Then
            caricaDtImpianti()
        Else
            caricaDtPlanning()
        End If

        caricaDtGiacenze()
        caricaListeValoriDaTabelle()
        caricaTabelle(False)
    End Sub


    Private Sub CambioSpecie(sender As Object, e As EventArgs)
        Session("MovimentiDettagliDDT") = Nothing
        ImageInfo0.Visible = False
        LabelInfo0.Visible = False
        caricaListePerCombo()

        If objParametriAgenda.TargetOperazione = enum_Tipo_Operazione_Agenda_Target.Reale Then
            caricaDtImpianti()
        Else
            caricaDtPlanning()
        End If

        caricaDtGiacenze()
        caricaListeValoriDaTabelle()
        caricaTabelle(False)
    End Sub


    Private Sub aggiornaPerDataScadenza(sender As Object, e As EventArgs)
        'permetto il cambio data senza elimiare bolle
        'Session("MovimentiDettagliDDT") = Nothing
        caricaListePerCombo()

        If objParametriAgenda.TargetOperazione = enum_Tipo_Operazione_Agenda_Target.Reale Then
            caricaDtImpianti()
        Else
            caricaDtPlanning()
        End If

        caricaDtGiacenze()
        caricaListeValoriDaTabelle()
        caricaTabelle(False)
    End Sub

    Protected Sub CheckBoxGiacenzePositive_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxGiacenzePositive.CheckedChanged
        caricaDtGiacenze()
        caricaListeValoriDaTabelle()
        caricaTabelle(False)
    End Sub

    Private Sub CambioMagazzino(sender As Object, e As EventArgs)
        Session("MovimentiDettagliDDT") = Nothing
        ImageInfo0.Visible = False
        LabelInfo0.Visible = False
        caricaDtGiacenze()
        caricaListeValoriDaTabelle()
        caricaTabelle(False)
    End Sub


    Private Sub caricaTabelle(ByVal chekkatuttterighemagazzino As Boolean)
        inizializzoParametriAgenda()
        Genera_Tabella_Magazzino()

        If chekkatuttterighemagazzino Then
            For i = 0 To GridViewMagazzino.Rows.Count - 1
                CType(GridViewMagazzino.Rows(i).Cells(NumColonna_Check).Controls(1), CheckBox).Checked = True
            Next
        End If

        'non spostare ordine, prima devo aggiornare la visibilità delle colonne e poi
        'genero la tabella impianti, la tabella aggiornerà i valori delle sole colonne
        'visibili e lascerà i valori di default per quelle non visibili
        'FORSE, ora vediamo che fa reimpostando tutti i valori comunque

        If objParametriAgenda.TargetOperazione = enum_Tipo_Operazione_Agenda_Target.Reale Then
            'If objParametriAgenda.Lav_Cod <> LAVCOD_SOVESCIO Then
            GeneraTabellaImpianti()
            'End If
        Else
            AggiornaVisibilitaColonneSeminePlanning()
            GeneraTabellaPlanning()
        End If

    End Sub


#End Region


#Region "GridMagazzino"


    Private Sub Genera_Tabella_Magazzino()


        If objParametriAgenda.Fabbricato = "0" Then
            DivMagazzino.Visible = False
            'GridViewMagazzino.Visible = False
            CheckBoxSeminaMultiSpecie.Visible = False
        Else
            DivMagazzino.Visible = True
            CheckBoxSeminaMultiSpecie.Visible = True
            Select Case CInt(objParametriAgenda.Lav_Cod)
                Case LAVCOD_SEMINA, LAVCOD_SOD_SEDDING, LAVCOD_SOVESCIO
                    CheckBoxSeminaMultiSpecie.Text = "Semina Multi-Specie"
                Case LAVCOD_TRAPIANTO
                    CheckBoxSeminaMultiSpecie.Text = "Trapianto Multi-Specie"
            End Select
        End If


        ''Vettore di DataColumn
        Dim DtKeys(19) As String

        'Valorizzo le celle del vettore
        DtKeys(0) = "Piva"
        DtKeys(1) = "Sa_Cod"
        DtKeys(2) = "Mat_Cod"
        DtKeys(3) = "Sem_Cod"
        DtKeys(4) = "Descrizione"
        DtKeys(5) = "Cul_Cod"
        DtKeys(6) = "Udm_Cod"
        DtKeys(7) = "Giacenza"
        DtKeys(8) = "Cul_Des"
        DtKeys(9) = "Udm_Sim"
        DtKeys(10) = "Qta"

        DtKeys(11) = "Lotto"
        DtKeys(12) = "Bolla"
        DtKeys(13) = "Bolla_Doc"
        DtKeys(14) = "Bolla_Des"
        DtKeys(15) = "Bolla_Qta"
        DtKeys(16) = "Bolla_Giacenza"

        DtKeys(17) = "Veg_Cod"
        DtKeys(18) = "Veg_Des"
        DtKeys(19) = "Regolamento"

        GridViewMagazzino.DataKeyNames = DtKeys

        'ordino per cul_des
        'uso il dataview per RIordinare 
        Dim Dv As New DataView

        Dt_Magazzino.TableName = "tab_1"
        Dv.Table = Dt_Magazzino
        Dv.Sort = "Cul_Des"

        GridViewMagazzino.DataSource = Dv
        GridViewMagazzino.DataBind()

        Coloro_Tabella_Magazzino()

        ChekkoRighe_Tabella_Magazzino()
    End Sub


    Private Sub aggiungiColonneLotti(ByRef Dt As DataTable)

        Dt.Columns.Add(New DataColumn("Piva", GetType(String)))
        Dt.Columns.Add(New DataColumn("Sa_Cod", GetType(Integer)))
        Dt.Columns.Add(New DataColumn("Mat_Cod", GetType(Integer)))
        Dt.Columns.Add(New DataColumn("Sem_Cod", GetType(Integer)))
        Dt.Columns.Add(New DataColumn("Mat_Des", GetType(String)))

        Dt.Columns.Add(New DataColumn("Cod_Articolo", GetType(String)))
        Dt.Columns.Add(New DataColumn("Lotto", GetType(String)))
        Dt.Columns.Add(New DataColumn("Veg_Des", GetType(String)))
        Dt.Columns.Add(New DataColumn("Cul_Des", GetType(String)))
        Dt.Columns.Add(New DataColumn("Descrizione", GetType(String)))
        Dt.Columns.Add(New DataColumn("Cul_Cod", GetType(Integer)))
        Dt.Columns.Add(New DataColumn("Veg_Cod", GetType(Integer)))
        Dt.Columns.Add(New DataColumn("Udm_Cod", GetType(Integer)))
        Dt.Columns.Add(New DataColumn("Giacenza", GetType(String)))
        Dt.Columns.Add(New DataColumn("Regolamento", GetType(Integer)))

        Dt.Columns.Add(New DataColumn("Udm_Sim", GetType(String)))
        Dt.Columns.Add(New DataColumn("Qta", GetType(String)))

        Dt.Columns.Add(New DataColumn("Bolla", GetType(String)))
        Dt.Columns.Add(New DataColumn("Bolla_Doc", GetType(String)))
        Dt.Columns.Add(New DataColumn("Bolla_Des", GetType(String)))
        Dt.Columns.Add(New DataColumn("Bolla_Qta", GetType(String)))
        Dt.Columns.Add(New DataColumn("Bolla_Giacenza", GetType(String)))

    End Sub


    Private Sub ChekkoRighe_Tabella_Magazzino() Implements iOperazioneGUI_Semina.Ripristina_Dati_nei_Controlli_Tabella_Magazzino

        'prima schekko
        For Indice = 0 To Me.GridViewMagazzino.Rows.Count - 1
            CType(GridViewMagazzino.Rows(Indice).Cells(NumColonna_Check).Controls(1), CheckBox).Checked = False
        Next

        If Not IsNothing(ListaValoriSeminaImpianti) AndAlso Not IsNothing(GridViewMagazzino) Then

            For Indice = 0 To Me.GridViewMagazzino.Rows.Count - 1

                For i = 0 To ListaValoriSeminaImpianti.Count - 1

                    If objParametriAgenda.Fabbricato <> "0" Then
                        'magazzino

                        If ListaValoriSeminaImpianti(i).Mat_Cod = GridViewMagazzino.DataKeys(Indice).Item("Mat_Cod") AndAlso
                               ListaValoriSeminaImpianti(i).UDM_Cod = GridViewMagazzino.DataKeys(Indice).Item("Udm_Cod") AndAlso
                               ListaValoriSeminaImpianti(i).Lotto = GridViewMagazzino.DataKeys(Indice).Item("Lotto") AndAlso
                               ListaValoriSeminaImpianti(i).Bolla = GridViewMagazzino.DataKeys(Indice).Item("Bolla") Then

                            CType(GridViewMagazzino.Rows(Indice).Cells(NumColonna_Check).Controls(1), CheckBox).Checked = True

                            Try
                                If ListaValoriSeminaImpianti(i).QTA_Tot_Prodotto <> "" AndAlso IsNumeric(ListaValoriSeminaImpianti(i).QTA_Tot_Prodotto) Then
                                    CType(GridViewMagazzino.Rows(Indice).Cells(GridViewMagazzino.Columns.Count - 2).Controls(1), TextBox).Text = ListaValoriSeminaImpianti(i).QTA_Tot_Prodotto
                                End If
                            Catch ex As Exception

                            End Try


                            Exit For

                        End If

                    Else
                        'no magazzino

                        If ListaValoriSeminaImpianti(i).Pro_Cod = GridViewMagazzino.DataKeys(Indice).Item("Sem_Cod") Then

                            CType(GridViewMagazzino.Rows(Indice).Cells(NumColonna_Check).Controls(1), CheckBox).Checked = True
                            Exit For

                        End If
                    End If

                Next

            Next

        End If

    End Sub


    Private Sub Coloro_Tabella_Magazzino()

        If Not IsNothing(GridViewMagazzino) AndAlso GridViewMagazzino.Rows.Count > 0 Then

            'coloro le righe
            Dim i2 As Integer = 0
            For i2 = 0 To GridViewMagazzino.Rows.Count - 1
                If i2 Mod 2 = 0 Then
                    GridViewMagazzino.Rows(i2).BackColor = Drawing.Color.PaleGoldenrod
                End If
            Next

        End If

    End Sub


#End Region



#Region "Gestione GridViewImpianti"


    Private Function InizializzaDataTablePerGridViewImpianti() As DataTable

        Dim Dt As New DataTable("Impianti")
        aggiungiColonneSemineImpianti(Dt)
        Return Dt

    End Function


    Private Sub aggiungiColonneSemineImpianti(ByRef Dt As DataTable) Implements iOperazioneGUI_Semina.aggiungiColonneSemineImpianti

        Dt.Columns.Add("Piva", GetType(String))
        Dt.Columns.Add("Rag_Soc", GetType(String))
        Dt.Columns.Add("Sa_Cod", GetType(Integer))
        Dt.Columns.Add("Sa_nome", GetType(String))
        Dt.Columns.Add("Campo_Des", GetType(String))
        Dt.Columns.Add("Campo_Cod", GetType(Integer))
        Dt.Columns.Add("Distinta", GetType(String))
        Dt.Columns.Add("Appezza", GetType(Integer))
        Dt.Columns.Add("App_Nome", GetType(String))
        Dt.Columns.Add("Id_Reg", GetType(Integer))
        Dt.Columns.Add("Sup_App", GetType(Decimal))
        Dt.Columns.Add("Tra_Fila", GetType(String))
        Dt.Columns.Add("Su_Fila", GetType(String))
        Dt.Columns.Add("Interbina", GetType(String))
        Dt.Columns.Add("Germinabilita", GetType(String))
        Dt.Columns.Add("Info", GetType(String))
        Dt.Columns.Add("Veg_Cod", GetType(Integer))
        Dt.Columns.Add("Cul_Cod", GetType(Integer))
        Dt.Columns.Add("Grfi_Cod", GetType(Integer))
        Dt.Columns.Add("Regolamento", GetType(Integer))
        Dt.Columns.Add("Disciplinare", GetType(String))
        Dt.Columns.Add("P_Ha", GetType(Decimal))
        Dt.Columns.Add("ProgettoCod", GetType(Integer))

        Dt.Columns.Add("Piva_Lotto", GetType(String))
        Dt.Columns.Add("Sa_Cod_Lotto", GetType(Integer))
        Dt.Columns.Add("Mat_Cod_Lotto", GetType(Integer))
        Dt.Columns.Add("Sem_Cod_Lotto", GetType(Integer))
        Dt.Columns.Add("Descrizione_Lotto", GetType(String))
        Dt.Columns.Add("Cul_Cod_Lotto", GetType(Integer))
        Dt.Columns.Add("Veg_Cod_Lotto", GetType(Integer))
        Dt.Columns.Add("Udm_Cod_Lotto", GetType(Integer))
        Dt.Columns.Add("Giacenza_Lotto", GetType(String))
        Dt.Columns.Add("Cul_Des_Lotto", GetType(String))
        Dt.Columns.Add("Udm_Sim_Lotto", GetType(String))
        Dt.Columns.Add("Qta_Lotto", GetType(String))
        Dt.Columns.Add("Lotto_Lotto", GetType(String))
        Dt.Columns.Add("Bolla_Lotto", GetType(String))
        Dt.Columns.Add("Bolla_Doc_Lotto", GetType(String))
        Dt.Columns.Add("Bolla_Des_Lotto", GetType(String))
        Dt.Columns.Add("Bolla_Qta_Lotto", GetType(String))
        Dt.Columns.Add("Bolla_Giacenza_Lotto", GetType(String))

        If objParametriAgenda.TargetOperazione = enum_Tipo_Operazione_Agenda_Target.Planning Then
            Dt.Columns.Add("Operazione_Des", GetType(String))
            Dt.Columns.Add("Operazione_Cod", GetType(Integer))
            'Dt.Columns.Add("Piva_SuperUser", GetType(String))
            Dt.Columns.Add("Programmazione_Entita_Cod", GetType(Integer))
            Dt.Columns.Add("Cul_Des", GetType(String))
            Dt.Columns.Add("Cul_Des_Cliente", GetType(String))
            Dt.Columns.Add("Grfi_Des", GetType(String))
            Dt.Columns.Add("Progetto_Cod", GetType(String))
        End If

        Dt.Columns.Add("validita_inizio", GetType(Date))
        Dt.Columns.Add("validita_Fine", GetType(Date))
        Dt.Columns.Add("Validita_Inizio_App", GetType(Date))
        Dt.Columns.Add("validita_Fine_App", GetType(Date))
        Dt.Columns.Add("validita_inizio_Distinta", GetType(Date))
        Dt.Columns.Add("validita_Fine_Distinta", GetType(Date))

        Dt.Columns.Add("catasto", GetType(String))
        Dt.Columns.Add("Veg_Des_Attuale", GetType(String))
        Dt.Columns.Add("Veg_Des", GetType(String))
        Dt.Columns.Add("MetodoProduttivo", GetType(Integer))
        Dt.Columns.Add("Regolamento_Lotto", GetType(Integer))

    End Sub



    Private Function GeneraTabellaPlanning()
        Dim dt As DataTable = Dt_Impianti

        'Dim dtconlotti = InizializzaDataTablePerGridViewImpianti()
        'AggiungiLotti(dt, dtconlotti)

        Finalizza_GridViewPlanning(dt)


        Return True
    End Function


    Private Function GeneraTabellaImpianti()

        Dim dt As DataTable = Dt_Impianti

        Dim dtconlotti = InizializzaDataTablePerGridViewImpianti()
        AggiungiLotti(dt, dtconlotti)
        Finalizza_GridViewImpianti(dtconlotti)

        'per comodità, se ho un solo impianto, come quando ad esempio arrivo dall'anagrafica, seleziono già l'impianto
        If objParametriAgenda.Tipo_Operazione = enum_TipoOperazioneDB.Scrittura AndAlso Dt_Impianti.Rows.Count = 1 Then
            Try
                CType(GridViewImpianti.Rows(0).Cells(NumColonna_Check).Controls(1), CheckBox).Checked = True
            Catch ex As Exception

            End Try
        End If

        Return True

    End Function


    Private Sub AggiungiLotti(ByRef dt As DataTable, ByRef dtconlotti As DataTable)


        If objParametriAgenda.Fabbricato = "0" Then

            'If Tipo_Semina = enum_SEMINA_TIPO.Con_Frazionamento_perLotto Then
            If RBL_Tipo_Semina.SelectedValue = enum_SEMINA_TIPO.Frazionamento_Appezzamenti_perLotto_e_Semina_sui_Singoli_Default Then
                Messaggi.AgroMsgBox("Nel caso di semina/trapianto con frazionamento degli appezzamenti per lotto è necessario selezionare il magazzino dove sono presenti i lotti", Page, ,
                Master_Operazione.Property_UpdatePanelToolBar)
                Exit Sub
            End If

            'se non ho il magazzino non genero la griglia, utilizzo di default seme o piantine quando genero la tabella impianti, 
            'sono sempre rimasti quei valori

            If objParametriAgenda.Veg_Cod.Split("/")(0) = "0" Or objParametriAgenda.Veg_Cod.Split("/")(0) = "-1" Then
                'terreno nudo, così darebbe errore perchè tipoliogiesementiperspecie caricherebbe tutto!!!!!
                Dim semcod As Integer = 0
                Dim semdes As String = ""
                Select Case objParametriAgenda.Lav_Cod
                    Case LAVCOD_SOVESCIO
                        semcod = 0
                        semdes = Resources.AgronicaAgenda_2010.SelezionareUnaSpecieVegetale
                    Case LAVCOD_SEMINA, LAVCOD_SOD_SEDDING
                        semcod = 0
                        semdes = Resources.AgronicaAgenda_2010.SelezionareUnaSpecieVegetale
                    Case LAVCOD_TRAPIANTO
                        semcod = 0
                        semdes = Resources.AgronicaAgenda_2010.SelezionareUnaSpecieVegetale
                End Select
                For k = 0 To dt.Rows.Count - 1
                    dt.Rows(k).Item("Piva_Lotto") = ""
                    dt.Rows(k).Item("Sa_Cod_Lotto") = 0
                    dt.Rows(k).Item("Mat_Cod_Lotto") = 0
                    dt.Rows(k).Item("Regolamento_Lotto") = 0
                    dt.Rows(k).Item("Sem_Cod_Lotto") = semcod
                    dt.Rows(k).Item("Descrizione_Lotto") = semdes
                    dt.Rows(k).Item("Cul_Cod_Lotto") = 0
                    dt.Rows(k).Item("Veg_Cod_Lotto") = 0
                    dt.Rows(k).Item("Udm_Cod_Lotto") = 0
                    dt.Rows(k).Item("Giacenza_Lotto") = ""
                    dt.Rows(k).Item("Cul_Des_Lotto") = ""
                    dt.Rows(k).Item("Udm_Sim_Lotto") = ""
                    dt.Rows(k).Item("Qta_Lotto") = 0
                    dt.Rows(k).Item("Lotto_Lotto") = ""
                    dt.Rows(k).Item("Bolla_Lotto") = ""
                    dt.Rows(k).Item("Bolla_Des_Lotto") = ""
                    dt.Rows(k).Item("Bolla_Doc_Lotto") = ""
                    dt.Rows(k).Item("Bolla_Qta_Lotto") = ""
                    dt.Rows(k).Item("Bolla_Giacenza_Lotto") = ""
                    dtconlotti.ImportRow(dt.Rows(k))
                Next


            Else

                Dim filtro As String = ""
                Dim Dt_TS As New DataTable
                'Applico il filtro sulla tipologia di semente e sulla specie
                Select Case objParametriAgenda.Lav_Cod
                    Case LAVCOD_SOVESCIO
                        filtro = "  ( TipologieSementixSpecieVegetali.Sem_Cod IN (1,3,7) ) "
                    Case LAVCOD_SEMINA, LAVCOD_SOD_SEDDING
                        filtro = "  ( TipologieSementixSpecieVegetali.Sem_Cod IN (1,3,7) ) "
                    Case LAVCOD_TRAPIANTO
                        filtro = "  ( TipologieSementixSpecieVegetali.Sem_Cod IN (2,3,4,5,6,7,8,10) ) "
                End Select

                Dt_TS = New AgronicaCoreMetaSchemaDAL.TipologieSementixSpecie_R().Leggi(
                                            objParametriAgenda.Veg_Cod.Split("/")(0),
                                            0,
                                            AgronicaCoreDataProvider.AgronicaCoreParametri.enumSelezioneVariabile.Selezione_JoinCompleta,
                                            filtro,
                                             "", objParametri_Server)

                If Dt_TS.Rows.Count = 0 AndAlso objParametriAgenda.Lav_Cod <> LAVCOD_SOVESCIO Then
                    Messaggi.AgroMsgBox("Nessuna tipologia semente per questa specie", Page, ,
                    Master_Operazione.Property_UpdatePanelToolBar)
                    Exit Sub
                End If


                'Controllo sul numero di righe, quindi di tipo semente che ci ha nella modifica introdotta,
                'si suppone che per la semina o trapianto ci sia un solo record, per questo si è nascosta la tabella dei lotti
                'nel caso non si utilizzi il magazzino. Genero una eccezione se verifico che ci sono più record e quindi non 
                'sarebbe corretto nascondere la tabella
                If Dt_TS Is Nothing Or Dt_TS.Rows.Count > 1 Then
                    'per la patata sono erroneamente riportate due tipologie, 1 e 3, semente e tuberi, 
                    'per altre tipologie su noce ci sono righe duplicate, di solito seme e piantine da orto
                    'verrà tolto man mano il dato in più, ma filtro l'eccezione solo in questo caso

                    'dato che ordina per sem_cod prende sempre lo stesso sicuramente, quindsi se salvo posso sempre rileggere l'operazione,
                    'ma per salvataggi passati se son fatti con una tiplogia di semente differente da quella caricata di default possono 
                    'non essere reimpostati i dati


                    ''      Throw New NotImplementedException("Non c'è una sola tipologia di sementi se non utilizzo il magazzino, non è previsto")


                End If

                For k = 0 To dt.Rows.Count - 1
                    dt.Rows(k).Item("Piva_Lotto") = ""
                    dt.Rows(k).Item("Sa_Cod_Lotto") = 0
                    dt.Rows(k).Item("Mat_Cod_Lotto") = 0
                    dt.Rows(k).Item("Regolamento_Lotto") = 0
                    If objParametriAgenda.Lav_Cod <> LAVCOD_SOVESCIO Then
                        dt.Rows(k).Item("Sem_Cod_Lotto") = Dt_TS.Rows(0).Item("Sem_Cod")
                        dt.Rows(k).Item("Descrizione_Lotto") = Dt_TS.Rows(0).Item("Sem_Des")
                    Else
                        'per il sovescio non ho la tipologia semente della specie dell'impianto, quindi uso 
                        'un valore di default se non ho il magazzino
                        dt.Rows(k).Item("Sem_Cod_Lotto") = 1
                        dt.Rows(k).Item("Descrizione_Lotto") = "Semente generico per sovescio"
                    End If

                    dt.Rows(k).Item("Veg_Cod_Lotto") = 0
                    dt.Rows(k).Item("Cul_Cod_Lotto") = 0
                    dt.Rows(k).Item("Udm_Cod_Lotto") = 0
                    dt.Rows(k).Item("Giacenza_Lotto") = ""
                    dt.Rows(k).Item("Cul_Des_Lotto") = ""
                    dt.Rows(k).Item("Udm_Sim_Lotto") = ""
                    dt.Rows(k).Item("Qta_Lotto") = 0
                    dt.Rows(k).Item("Lotto_Lotto") = ""
                    dt.Rows(k).Item("Bolla_Lotto") = ""
                    dt.Rows(k).Item("Bolla_Des_Lotto") = ""
                    dt.Rows(k).Item("Bolla_Doc_Lotto") = ""
                    dt.Rows(k).Item("Bolla_Qta_Lotto") = ""
                    dt.Rows(k).Item("Bolla_Giacenza_Lotto") = ""
                    dtconlotti.ImportRow(dt.Rows(k))
                Next
            End If

        Else


            For i = 0 To dt.Rows.Count - 1
                'inizializzo i valori, per evitare di avere dei dbnull
                dt.Rows(i).Item("Piva_Lotto") = ""
                dt.Rows(i).Item("Sa_Cod_Lotto") = 0
                dt.Rows(i).Item("Mat_Cod_Lotto") = 0
                dt.Rows(i).Item("Regolamento_Lotto") = 0
                dt.Rows(i).Item("Sem_Cod_Lotto") = 0
                dt.Rows(i).Item("Descrizione_Lotto") = ""
                dt.Rows(i).Item("Veg_Cod_Lotto") = 0
                dt.Rows(i).Item("Cul_Cod_Lotto") = 0
                dt.Rows(i).Item("Udm_Cod_Lotto") = 0
                dt.Rows(i).Item("Giacenza_Lotto") = ""
                dt.Rows(i).Item("Cul_Des_Lotto") = ""
                dt.Rows(i).Item("Udm_Sim_Lotto") = ""
                dt.Rows(i).Item("Qta_Lotto") = 0
                dt.Rows(i).Item("Lotto_Lotto") = ""
                dt.Rows(i).Item("Bolla_Lotto") = ""
                dt.Rows(i).Item("Bolla_Des_Lotto") = ""
                dt.Rows(i).Item("Bolla_Doc_Lotto") = ""
                dt.Rows(i).Item("Bolla_Qta_Lotto") = ""
                dt.Rows(i).Item("Bolla_Giacenza_Lotto") = ""
                Dim copiatoalmenouno As Boolean = False
                For j = 0 To GridViewMagazzino.Rows.Count - 1
                    If CType(GridViewMagazzino.Rows(j).Cells(NumColonna_Check).Controls(1), CheckBox).Checked Then
                        CopiaLotto(dt.Rows(i), j)
                        dtconlotti.ImportRow(dt.Rows(i))
                        copiatoalmenouno = True
                    End If
                Next
                If Not copiatoalmenouno Then
                    'se non ho aggiunto l'impianto perchè non c'era nessun lotto checcato lo inserisco senza coipiare i lotti
                    dtconlotti.ImportRow(dt.Rows(i))
                End If

            Next

        End If

        'If objParametriAgenda.Veg_Cod.Split("/")(0) = "0" Or objParametriAgenda.Veg_Cod.Split("/")(0) = "-1" Then
        If objParametriAgenda.Veg_Cod.Split("/")(0) = "-1" Then

            Messaggi.AgroMsgBox(Resources.AgronicaAgenda_2010.SelezionareUnaSpecieVegetale, Page, ,
                 Master_Operazione.Property_UpdatePanelToolBar)

        End If

    End Sub


    Private Sub CopiaLotto(ByVal Dr As DataRow, ByVal Indice2 As Integer)

        'dati lotto
        Dim Piva_Lotto As String = GridViewMagazzino.DataKeys(Indice2).Item("Piva")
        Dim Sa_Cod_Lotto As Integer = GridViewMagazzino.DataKeys(Indice2).Item("Sa_Cod")
        Dim Mat_Cod_Lotto As Integer = GridViewMagazzino.DataKeys(Indice2).Item("Mat_Cod")
        Dim Sem_Cod_Lotto As Integer = GridViewMagazzino.DataKeys(Indice2).Item("Sem_Cod")
        Dim Descrizione_Lotto As String = GridViewMagazzino.DataKeys(Indice2).Item("Descrizione")
        Dim Cul_Cod_Lotto As Decimal = GridViewMagazzino.DataKeys(Indice2).Item("Cul_Cod")
        Dim Veg_Cod_Lotto As Decimal = GridViewMagazzino.DataKeys(Indice2).Item("Veg_Cod")
        Dim Veg_Des_Lotto As String = GridViewMagazzino.DataKeys(Indice2).Item("Veg_Des")
        Dim Udm_Cod_Lotto As Integer = GridViewMagazzino.DataKeys(Indice2).Item("Udm_Cod")
        Dim Giacenza_Lotto As String = GridViewMagazzino.DataKeys(Indice2).Item("Giacenza")
        Dim Cul_Des_Lotto As String = GridViewMagazzino.DataKeys(Indice2).Item("Cul_Des")
        Dim Udm_Sim_Lotto As String = GridViewMagazzino.DataKeys(Indice2).Item("Udm_Sim")
        Dim Qta_Lotto As Integer = GridViewMagazzino.DataKeys(Indice2).Item("Qta")
        Dim Lotto_Lotto As String = GridViewMagazzino.DataKeys(Indice2).Item("Lotto")
        Dim Bolla_Lotto As String = GridViewMagazzino.DataKeys(Indice2).Item("Bolla")
        Dim Bolla_Doc_Lotto As String = GridViewMagazzino.DataKeys(Indice2).Item("Bolla_Doc")
        Dim Bolla_Des_Lotto As String = GridViewMagazzino.DataKeys(Indice2).Item("Bolla_Des")
        Dim Bolla_Qta_Lotto As String = GridViewMagazzino.DataKeys(Indice2).Item("Bolla_Qta")
        Dim Bolla_Giacenza_Lotto As String = GridViewMagazzino.DataKeys(Indice2).Item("Bolla_Giacenza")
        Dim Regolamento_Lotto As Integer = GridViewMagazzino.DataKeys(Indice2).Item("Regolamento")

        Dr.Item("Piva_Lotto") = Piva_Lotto
        Dr.Item("Sa_Cod_Lotto") = Sa_Cod_Lotto
        Dr.Item("Mat_Cod_Lotto") = Mat_Cod_Lotto
        Dr.Item("Regolamento_Lotto") = Regolamento_Lotto
        Dr.Item("Sem_Cod_Lotto") = Sem_Cod_Lotto
        Dr.Item("Descrizione_Lotto") = Descrizione_Lotto
        Dr.Item("Cul_Cod_Lotto") = Cul_Cod_Lotto
        Dr.Item("Veg_Cod_Lotto") = Veg_Cod_Lotto
        If Veg_Cod_Lotto <> 0 Then
            Dr.Item("Veg_Des") = Veg_Des_Lotto
        End If
        Dr.Item("Udm_Cod_Lotto") = Udm_Cod_Lotto
        Dr.Item("Giacenza_Lotto") = Giacenza_Lotto
        Dr.Item("Cul_Des_Lotto") = Cul_Des_Lotto
        Dr.Item("Udm_Sim_Lotto") = Udm_Sim_Lotto
        Dr.Item("Qta_Lotto") = Qta_Lotto
        Dr.Item("Lotto_Lotto") = Lotto_Lotto
        Dr.Item("Bolla_Lotto") = Bolla_Lotto
        Dr.Item("Bolla_Doc_Lotto") = Bolla_Doc_Lotto
        Dr.Item("Bolla_Des_Lotto") = Bolla_Des_Lotto
        Dr.Item("Bolla_Qta_Lotto") = Bolla_Qta_Lotto
        Dr.Item("Bolla_Giacenza_Lotto") = Bolla_Giacenza_Lotto
    End Sub


    Private Sub Finalizza_GridViewPlanning(Dt As DataTable) Implements iOperazioneGUI_Semina.Finalizza_GridViewPlanning
        ''Vettore di DataColumn
        Dim DtKeys(8) As String

        'Valorizzo le celle del vettore

        DtKeys(0) = "Piva"
        DtKeys(1) = "Sa_Cod"
        DtKeys(2) = "Appezza"
        DtKeys(3) = "Id_Reg"

        DtKeys(4) = "Programmazione_Entita_Cod"

        DtKeys(5) = "ProgettoCod"

        DtKeys(6) = "Veg_Cod"
        DtKeys(7) = "Cul_Cod"

        DtKeys(8) = "Mat_Cod"

        GridViewPlanning.DataKeyNames = DtKeys

        GridViewPlanning.DataSource = Dt
        GridViewPlanning.DataBind()

        'non spostare, nasconde i controlli di alcune righe e i check quindi se si sposta non funziona più
        ColoroLaGrigliaPlanning()

        'imposto i valori di default delle righe con dati impianto
        ImpostaValoriDefaultGrigliaPlanning()

        'imposto i valori salvati nela lista
        impostoValoriTabellaSeminePlanning()



    End Sub

    Private Sub Finalizza_GridViewImpianti(Dt As DataTable) Implements iOperazioneGUI_Semina.Finalizza_GridViewImpianti

        ''Vettore di DataColumn
        Dim DtKeys(42) As String

        'Valorizzo le celle del vettore

        DtKeys(0) = "Piva"
        DtKeys(1) = "Sa_Cod"
        DtKeys(2) = "Appezza"
        DtKeys(3) = "Id_Reg"

        DtKeys(4) = "Distinta"

        DtKeys(5) = "Tra_Fila"
        DtKeys(6) = "Su_Fila"
        DtKeys(7) = "Interbina"
        DtKeys(8) = "Germinabilita"
        DtKeys(9) = "Sup_App"
        DtKeys(10) = "Cul_Cod"
        DtKeys(11) = "Grfi_Cod"
        DtKeys(12) = "Regolamento"
        DtKeys(13) = "Disciplinare"
        DtKeys(14) = "P_Ha"

        DtKeys(15) = "Piva_Lotto"
        DtKeys(16) = "Sa_Cod_Lotto"
        DtKeys(17) = "Mat_Cod_Lotto"
        DtKeys(18) = "Sem_Cod_Lotto"
        DtKeys(19) = "Cul_Cod_Lotto"
        DtKeys(20) = "Udm_Cod_Lotto"
        DtKeys(21) = "Cul_Des_Lotto"
        DtKeys(22) = "Udm_Sim_Lotto"
        DtKeys(23) = "Lotto_Lotto"
        DtKeys(24) = "Bolla_Lotto"
        'DtKeys(25) = "Bolla_Doc_Lotto"
        DtKeys(25) = "Bolla_Des_Lotto"
        DtKeys(26) = "Bolla_Qta_Lotto"
        DtKeys(27) = "Bolla_Giacenza_Lotto"

        DtKeys(28) = "ProgettoCod"
        DtKeys(29) = "Veg_Cod"
        DtKeys(30) = "Veg_Cod_Lotto"

        DtKeys(31) = "Campo_Cod"
        DtKeys(32) = "Validita_Inizio"
        DtKeys(33) = "Validita_Fine"
        DtKeys(34) = "Validita_Inizio_App"
        DtKeys(35) = "Validita_Fine_App"
        DtKeys(36) = "Validita_Inizio_Distinta"
        DtKeys(37) = "Validita_Fine_Distinta"
        DtKeys(38) = "App_Nome"
        DtKeys(39) = "Veg_Des"
        DtKeys(40) = "MetodoProduttivo"
        DtKeys(41) = "Catasto"
        DtKeys(42) = "Regolamento_Lotto"

        GridViewImpianti.DataKeyNames = DtKeys

        GridViewImpianti.DataSource = Dt
        GridViewImpianti.DataBind()

        DivImpianti.Visible = False

        If Not IsNothing(GridViewImpianti) AndAlso GridViewImpianti.Rows.Count > 0 Then

            DivImpianti.Visible = True

            'non spostare, nasconde i controlli di alcune righe e i check quindi se si sposta non funziona più
            ColoroLaGrigliaImpianti()

            'imposto i valori di default delle righe con dati impianto
            ImpostaValoriDefaultGrigliaImpianti()

            'imposto i valori salvati nela lista
            impostoValoriTabellaSemineImpianti()

        End If



    End Sub

    Private Sub ImpostaValoriDefaultGrigliaPlanning()

        For z = 0 To Me.GridViewPlanning.Rows.Count - 1
            Set_Prodotto(z)
            Set_Udm(z)
        Next
    End Sub

    Private Sub ImpostaValoriDefaultGrigliaImpianti()

        Dim SettaSupOld As Boolean = False
        Dim N_Lotti_Selezionati As Integer = 0
        For j = 0 To GridViewMagazzino.Rows.Count - 1
            If CType(GridViewMagazzino.Rows(j).Cells(NumColonna_Check).Controls(1), CheckBox).Checked Then
                N_Lotti_Selezionati += 1
            End If
        Next

        If N_Lotti_Selezionati = 1 Then
            SettaSupOld = True
        End If

        For z = 0 To Me.GridViewImpianti.Rows.Count - 1

            'Se la riga e' selezionata è quella con i dati impianto,
            'imposto i valori di default
            If GridViewImpianti.Rows(z).Cells(NumColonna_Check).Controls(1).Visible Then

                'prima usavo findcontrol, cambio per vedere di velocizzare pagina
                'If Not IsDBNull(GridViewImpianti.DataKeys(z).Item("Tra_Fila")) Then
                '    CType(GridViewImpianti.Rows(z).FindControl("TxtDistTraFila"), TextBox).Text = GridViewImpianti.DataKeys(z).Item("Tra_Fila")
                'End If
                Set_App_Nome(z)
                Set_MetodoProduttivo(z)
                Set_Distinta(z)
                Set_Tra_Fila(z)
                Set_Su_Fila(z)
                Set_Interbina(z)
                Set_Germinabilita(z)
                Set_Specie(z)
                Set_Varieta(z)
                Set_Finalita(z)
                Set_Disciplinare(z)
                Set_Superficie(z, SettaSupOld)
                Set_P_Ha(z)

            End If

            Set_Udm(z)
            Set_Qta(z)

        Next

    End Sub


    Private Sub impostoValoriTabellaSeminePlanning() Implements iOperazioneGUI_Semina.Ripristina_Dati_nei_Controlli_TabellaSeminePlanning

        'prima schekko
        For Indice = 0 To Me.GridViewPlanning.Rows.Count - 1
            If GridViewPlanning.Rows(Indice).Cells(NumColonna_Check).Controls(1).Visible Then
                CType(GridViewPlanning.Rows(Indice).Cells(NumColonna_Check).Controls(1), CheckBox).Checked = False
            End If
        Next

        If Not IsNothing(ListaValoriSeminaImpianti) AndAlso Not IsNothing(GridViewPlanning) Then

            For Indice = 0 To Me.GridViewPlanning.Rows.Count - 1

                For i = 0 To ListaValoriSeminaImpianti.Count - 1

                    'controllo l'appezzamento della riga
                    If ListaValoriSeminaImpianti(i).Programmazione_Entita_cod = GridViewPlanning.DataKeys(Indice).Item("Programmazione_Entita_Cod") Then

                        'se sono nella riga dell'impianto (la riga che ha la checkbox visibile)
                        ' metto il check,  i trafila etc
                        If GridViewPlanning.Rows(Indice).Cells(NumColonna_Check).Controls(1).Visible Then

                            CType(GridViewPlanning.Rows(Indice).Cells(NumColonna_Check).Controls(1), CheckBox).Checked = True

                            'se è la prima volta che carico la pagina non imposto i valori dell'impianto,
                            'altrimenti vado a modificare i valori preimpostati, dato che la listavalori in questo caso li ha tutti a 0
                            'invece se è la prima volta che la carico devo impostare i valri nella griglia
                            'per la qta_impostata e udm_impostata che sono stati letti solo quando si apre l'operazione
                            If IsPostBack Then
                                'Set_Distinta(Indice, ListaValoriSeminaImpianti(i).Lotto)
                                Set_Tra_Fila(Indice, ListaValoriSeminaImpianti(i).Tra_Fila)
                                Set_Su_Fila(Indice, ListaValoriSeminaImpianti(i).Su_Fila)
                                Set_Interbina(Indice, ListaValoriSeminaImpianti(i).Interbina)
                                Set_Germinabilita(Indice, ListaValoriSeminaImpianti(i).Germinabilita)
                                Set_Varieta(Indice, ListaValoriSeminaImpianti(i).Varieta)
                                Set_Finalita(Indice, ListaValoriSeminaImpianti(i).Finalita)
                                Set_Disciplinare(Indice, ListaValoriSeminaImpianti(i).Disciplinare)

                            Else
                                Set_Prodotto(Indice, ListaValoriSeminaImpianti(i).Mat_Cod)
                                Set_Qta(Indice, ListaValoriSeminaImpianti(i).QTA_Dest)
                                Set_Udm(Indice, ListaValoriSeminaImpianti(i).UDM_Cod)

                            End If

                        End If

                        'in tutte le righe imposto invece i valori della semina
                        If objParametriAgenda.Fabbricato <> "0" Then
                            'magazzino, controllo mat_cod e udm_cod
                            If ListaValoriSeminaImpianti(i).Mat_Cod = GridViewPlanning.DataKeys(Indice).Item("Mat_Cod_Lotto") AndAlso
                                   ListaValoriSeminaImpianti(i).UDM_Cod = GridViewPlanning.DataKeys(Indice).Item("Udm_Cod_Lotto") AndAlso
                                   ListaValoriSeminaImpianti(i).Lotto = GridViewPlanning.DataKeys(Indice).Item("Lotto_Lotto") AndAlso
                                   ListaValoriSeminaImpianti(i).Bolla = GridViewPlanning.DataKeys(Indice).Item("Bolla_Lotto") Then

                                Set_Qta(Indice, ListaValoriSeminaImpianti(i).QTA_Dest)
                                'if udm non corrisponde all'unico che deve essere presente nella combo eccezione, qualcosa fatto male
                                Exit For

                            End If

                        Else
                            'no magazzino, controllo solo sem_cod per corrispondenza
                            If ListaValoriSeminaImpianti(i).Pro_Cod = GridViewPlanning.DataKeys(Indice).Item("Sem_Cod_Lotto") Then
                                Set_Qta(Indice, ListaValoriSeminaImpianti(i).QTA_Dest)
                                'imposto anche udm
                                Set_Udm(Indice, ListaValoriSeminaImpianti(i).UDM_Cod)

                            End If

                        End If

                    End If

                Next

            Next
        End If
    End Sub

    Private Sub impostoValoriTabellaSemineImpianti() Implements iOperazioneGUI_Semina.Ripristina_Dati_nei_Controlli_TabellaSemineImpianti

        'prima schekko
        For Indice = 0 To Me.GridViewImpianti.Rows.Count - 1
            If GridViewImpianti.Rows(Indice).Cells(NumColonna_Check).Controls(1).Visible Then
                CType(GridViewImpianti.Rows(Indice).Cells(NumColonna_Check).Controls(1), CheckBox).Checked = False
            End If
        Next

        If Not IsNothing(ListaValoriSeminaImpianti) AndAlso Not IsNothing(GridViewImpianti) Then

            For Indice = 0 To Me.GridViewImpianti.Rows.Count - 1

                For i = 0 To ListaValoriSeminaImpianti.Count - 1

                    'controllo l'appezzamento della riga
                    If ListaValoriSeminaImpianti(i).Piva = GridViewImpianti.DataKeys(Indice).Item("Piva") AndAlso
                 ListaValoriSeminaImpianti(i).Sa_Cod = GridViewImpianti.DataKeys(Indice).Item("Sa_Cod") AndAlso
                 ListaValoriSeminaImpianti(i).Appezza = GridViewImpianti.DataKeys(Indice).Item("Appezza") AndAlso
                 ListaValoriSeminaImpianti(i).Id_Reg = GridViewImpianti.DataKeys(Indice).Item("Id_Reg") Then

                        'se sono nella riga dell'impianto (la riga che ha la checkbox visibile)
                        ' metto il check,  i trafila etc
                        If GridViewImpianti.Rows(Indice).Cells(NumColonna_Check).Controls(1).Visible Then

                            CType(GridViewImpianti.Rows(Indice).Cells(NumColonna_Check).Controls(1), CheckBox).Checked = True

                            'se è la prima volta che carico la pagina non imposto i valori dell'impianto,
                            'altrimenti vado a modificare i valori preimpostati, dato che la listavalori in questo caso li ha tutti a 0
                            'invece se è la prima volta che la carico devo impostare i valri nella griglia
                            'per la qta_impostata e udm_impostata che sono stati letti solo quando si apre l'operazione
                            If IsPostBack Then
                                Set_Tra_Fila(Indice, ListaValoriSeminaImpianti(i).Tra_Fila)
                                Set_Su_Fila(Indice, ListaValoriSeminaImpianti(i).Su_Fila)
                                Set_Interbina(Indice, ListaValoriSeminaImpianti(i).Interbina)
                                Set_Germinabilita(Indice, ListaValoriSeminaImpianti(i).Germinabilita)
                                Set_Varieta(Indice, ListaValoriSeminaImpianti(i).Varieta)
                                Set_Finalita(Indice, ListaValoriSeminaImpianti(i).Finalita)
                                Set_Disciplinare(Indice, ListaValoriSeminaImpianti(i).Disciplinare)
                            Else
                                Set_Qta(Indice, ListaValoriSeminaImpianti(i).QTA_Dest)
                                Set_Udm(Indice, ListaValoriSeminaImpianti(i).UDM_Cod)
                            End If
                        End If

                        'in tutte le righe imposto invece i valori della semina
                        If objParametriAgenda.Fabbricato <> "0" Then
                            'magazzino, controllo mat_cod e udm_cod
                            If ListaValoriSeminaImpianti(i).Mat_Cod = GridViewImpianti.DataKeys(Indice).Item("Mat_Cod_Lotto") AndAlso
                                   ListaValoriSeminaImpianti(i).UDM_Cod = GridViewImpianti.DataKeys(Indice).Item("Udm_Cod_Lotto") AndAlso
                                   ListaValoriSeminaImpianti(i).Lotto = GridViewImpianti.DataKeys(Indice).Item("Lotto_Lotto") AndAlso
                                   ListaValoriSeminaImpianti(i).Bolla = GridViewImpianti.DataKeys(Indice).Item("Bolla_Lotto") Then

                                Set_Qta(Indice, ListaValoriSeminaImpianti(i).QTA_Dest)
                                'if udm non corrisponde all'unico che deve essere presente nella combo eccezione, qualcosa fatto male
                                Exit For

                            End If

                        Else
                            'no magazzino, controllo solo sem_cod per corrispondenza
                            If ListaValoriSeminaImpianti(i).Pro_Cod = GridViewImpianti.DataKeys(Indice).Item("Sem_Cod_Lotto") Then
                                Set_Qta(Indice, ListaValoriSeminaImpianti(i).QTA_Dest)
                                'imposto anche udm
                                Set_Udm(Indice, ListaValoriSeminaImpianti(i).UDM_Cod)

                            End If

                        End If

                    End If

                Next

            Next
        End If

    End Sub

    Private Sub ColoroLaGrigliaPlanning()

    End Sub

    Private Sub ColoroLaGrigliaImpianti()

        If Not IsNothing(GridViewImpianti) AndAlso GridViewImpianti.Rows.Count > 0 Then

            Dim cambio As Boolean = True
            Dim Piva As String = GridViewImpianti.DataKeys(0).Item("Piva")
            Dim sa_cod As Integer = GridViewImpianti.DataKeys(0).Item("Sa_Cod")
            Dim appezza As Integer = GridViewImpianti.DataKeys(0).Item("Appezza")
            Dim Id_Reg As Integer = GridViewImpianti.DataKeys(0).Item("Id_Reg")

            'coloro le righe
            Dim i2 As Integer = 0
            For i2 = 1 To GridViewImpianti.Rows.Count - 1

                Dim Piva2 As String = GridViewImpianti.DataKeys(i2).Item("Piva")
                Dim sa_cod2 As Integer = GridViewImpianti.DataKeys(i2).Item("Sa_Cod")
                Dim appezza2 As Integer = GridViewImpianti.DataKeys(i2).Item("Appezza")
                Dim Id_Reg2 As Integer = GridViewImpianti.DataKeys(i2).Item("Id_Reg")

                'raggruppo per impianto e nascondo celle impianto doppie
                If Piva = Piva2 AndAlso sa_cod = sa_cod2 AndAlso appezza = appezza2 AndAlso Id_Reg = Id_Reg2 Then
                    'nascondo le celle
                    GridViewImpianti.Rows(i2).Cells(NumColonna_Rag_Soc).Text = ""
                    GridViewImpianti.Rows(i2).Cells(NumColonna_Sa_Nome).Text = ""
                    GridViewImpianti.Rows(i2).Cells(NumColonna_Campo_Des).Text = ""
                    'GridViewImpianti.Rows(i2).Cells(NumColonna_App_Nome).Text = ""

                    Select Case RBL_Tipo_Semina.SelectedValue 'Tipo_Semina
                        Case enum_SEMINA_TIPO.Frazionamento_Appezzamenti_perLotto_e_Semina_sui_Singoli_Default
                            'GridViewImpianti.Rows(i2).Cells(NumColonna_Distinta).Text = ""
                            'GridViewImpianti.Rows(i2).Cells(NumColonna_Varietà).Controls(1).Visible = False
                            'GridViewImpianti.Rows(i2).Cells(NumColonna_Finalità).Controls(1).Visible = False
                            'GridViewImpianti.Rows(i2).Cells(NumColonna_Regolamento).Controls(1).Visible = False
                            'GridViewImpianti.Rows(i2).Cells(NumColonna_Disciplinare).Controls(1).Visible = False
                            'GridViewImpianti.Rows(i2).Cells(NumColonna__Sup_App).Text = ""
                            'GridViewImpianti.Rows(i2).Cells(NumColonna_DistTraFila).Controls(1).Visible = False
                            'GridViewImpianti.Rows(i2).Cells(NumColonna_DistSuFila).Controls(1).Visible = False
                            'GridViewImpianti.Rows(i2).Cells(NumColonna_Interbina).Controls(1).Visible = False
                            'GridViewImpianti.Rows(i2).Cells(NumColonna_Fila_Binata).Controls(1).Visible = False
                            'GridViewImpianti.Rows(i2).Cells(NumColonna_Germinabilita).Controls(1).Visible = False
                            'GridViewImpianti.Rows(i2).Cells(NumColonna_p_ha).Text = ""
                        Case Else
                            GridViewImpianti.Rows(i2).Cells(NumColonna_Check).Controls(1).Visible = False
                            GridViewImpianti.Rows(i2).Cells(NumColonna_Distinta).Text = ""
                            GridViewImpianti.Rows(i2).Cells(NumColonna_Varietà).Controls(1).Visible = False
                            GridViewImpianti.Rows(i2).Cells(NumColonna_Finalità).Controls(1).Visible = False
                            GridViewImpianti.Rows(i2).Cells(NumColonna_MetodoProduttivo).Controls(1).Visible = False
                            GridViewImpianti.Rows(i2).Cells(NumColonna_Disciplinare).Controls(1).Visible = False
                            GridViewImpianti.Rows(i2).Cells(NumColonna__Sup_App).Text = ""
                            GridViewImpianti.Rows(i2).Cells(NumColonna__Sup_Semina).Text = ""
                            GridViewImpianti.Rows(i2).Cells(NumColonna_DistTraFila).Controls(1).Visible = False
                            GridViewImpianti.Rows(i2).Cells(NumColonna_DistSuFila).Controls(1).Visible = False
                            GridViewImpianti.Rows(i2).Cells(NumColonna_Interbina).Controls(1).Visible = False
                            GridViewImpianti.Rows(i2).Cells(NumColonna_Fila_Binata).Controls(1).Visible = False
                            GridViewImpianti.Rows(i2).Cells(NumColonna_Germinabilita).Controls(1).Visible = False
                            GridViewImpianti.Rows(i2).Cells(NumColonna_p_ha).Text = ""

                            GridViewImpianti.Rows(i2).Cells(NumColonna_App_Nome).Controls(1).Visible = False
                    End Select

                Else
                    cambio = Not cambio
                End If
                If Not cambio Then
                    GridViewImpianti.Rows(i2).BackColor = Drawing.Color.PaleGoldenrod
                End If

                Piva = Piva2
                sa_cod = sa_cod2
                appezza = appezza2
                Id_Reg = Id_Reg2

            Next

            'Disattivo la textbox della superficie
            'Select Case Tipo_Semina
            '    Case enum_SEMINA_TIPO.Con_Frazionamento_perLotto
            '    Case Else
            '        For i3 As Integer = 0 To GridViewImpianti.Rows.Count - 1
            '            If GridViewImpianti.Rows(i3).Cells(NumColonna__Sup_Semina).HasControls Then
            '                CType(GridViewImpianti.Rows(i3).Cells(NumColonna__Sup_Semina).Controls(1), TextBox).Enabled = False
            '            End If
            '            If GridViewImpianti.Rows(i3).Cells(NumColonna_p_ha).HasControls Then
            '                CType(GridViewImpianti.Rows(i3).Cells(NumColonna_p_ha).Controls(1), TextBox).Enabled = False
            '            End If
            '        Next
            'End Select

        End If

    End Sub


    'Private Function EsisteSeminaTrapianto(ByVal Piva As String,
    '                                       ByVal Sa_Cod As Integer,
    '                                       ByVal Appezza As Integer,
    '                                       ByVal Id_Reg As Integer) As Boolean


    '    Dim ObjAgenda As New AgronicaCoreContabDAL.Mov_Destinazioni_R
    '    Dim Dt As DataTable

    '    Dt = ObjAgenda.Leggi(CStr(Piva),
    '                         CInt(Sa_Cod),
    '                         0, 0, 0,
    '                         CInt(Appezza),
    '                         CInt(Id_Reg),
    '                         0,
    '                         AgronicaCoreDataProvider.AgronicaCoreParametri.enumSelezioneVariabile.Selezione_TabellaCompleta,
    '                         "(Lav_Cod= 2 or Lav_Cod = 71)", "", objParametri_Server)

    '    If Not Dt Is Nothing AndAlso Dt.Rows.Count > 0 Then
    '        Return True
    '    Else
    '        Return False
    '    End If

    'End Function


#End Region



    Private Sub BloccaComboJavaScript(ByVal centri As Boolean, ByVal operazioni As Boolean, ByVal specie As Boolean, ByVal magazzini As Boolean)
        'blocco via javascript pulsanti combo
        Dim script As New StringBuilder
        script.AppendLine("$(document).ready(function () { ")
        'script.AppendLine("     alert('b');")
        script.AppendLine("setTimeout('eseguiScr()',500); ")
        script.AppendLine("}); ")


        script.AppendLine("function eseguiScr(){ ")
        If centri Then
            script.AppendLine("     BloccaCombo_CentroAziendale();")
            ' script.AppendLine("     alert('a');")
        End If
        If operazioni Then
            script.AppendLine("     BloccaCombo_Operazioni();")
        End If
        If specie Then
            script.AppendLine("     BloccaCombo_Specie();")
        End If
        If magazzini Then
            script.AppendLine("     BloccaCombo_Magazzini();")
        End If

        script.AppendLine("} ")



        ScriptManager.RegisterClientScriptBlock(updateGiacenze_si_no,
                                updateGiacenze_si_no.GetType(),
                                "jQuery_{0}", script.ToString, True)


    End Sub





#Region "Salvataggio"


    Private Sub SalvaTutto(ByVal sender As Object, ByVal e As System.EventArgs) Implements iOperazioneGUI.SalvaTutto



        '--------------BLOCCO SALVATAGGIO SENZA MAGAZZINO-------------------
        If Not IsNothing(Session("UTENTE_COD_BLOCCA_SE_SENZA_MAGAZZINO")) AndAlso Session("UTENTE_COD_BLOCCA_SE_SENZA_MAGAZZINO") = True Then
            If objParametriAgenda.Fabbricato = "0" Or objParametriAgenda.Fabbricato = "" Then
                Dim MErrore As String
                MErrore = "In base alle impostazioni utente NON è possibile salvare l'operazione senza utilizzare il magazzino!"
                Messaggi.AgroMsgBox(Resources.AgronicaAgenda_2010.AttenzioneLOperazioneNonÈStataRegistrataBr & MErrore, Page, ,
                    CType(Ricerca.FindControlIterative(Page.Master, "UpdatePanelToolBar"), UpdatePanel))
                Exit Sub
            End If

        End If
        '---------------------------------------------------------------------


        Dim messaggio_errore As String = ""
        Dim Unid_Operazione As String = ""

        If SalvaOperazioneAgenda(messaggio_errore, Unid_Operazione) Then
            'premutosalva = True
            Dim TipoSalvataggio As Integer = CType(Ricerca.FindControlIterative(Page.Master, "RBL_Salva"), RadioButtonList).SelectedValue + 1
            fine_salvataggio(TipoSalvataggio, Unid_Operazione)
        Else
            'se ho lanciato il messaggio si-no per le giacenze non faccio vedere il messaggio di errore
            If (Not messaggioSiNo) Then
                Messaggi.AgroMsgBox(Resources.AgronicaAgenda_2010.AttenzioneLOperazioneNonÈStataRegistrataBr & messaggio_errore, Page, ,
                                 Master_Operazione.Property_UpdatePanelToolBar)
            End If
        End If
    End Sub


    Private Function SalvaOperazioneAgenda(ByRef messaggio_errore As String, ByRef Unid_Operazione As String) As Boolean Implements iOperazioneGUI.SalvaOperazioneAgenda

        Dim res As Boolean = False
        Dim strXMLAgenda As String

        Dim id_agende_lista As String = "0 "

        Dim ListaImpianti As New List(Of AgronicaCoreModello.ParametriAgenda_Temp.Impianto)

        Dim xGridView As GridView

        If objParametriAgenda.TargetOperazione = enum_Tipo_Operazione_Agenda_Target.Reale Then
            xGridView = GridViewImpianti
        Else
            xGridView = GridViewPlanning
        End If

        For i = 0 To xGridView.Rows.Count - 1

            'Se la riga e' selezionata ...
            If CType(xGridView.Rows(i).Cells(NumColonna_Check).Controls(1), CheckBox).Checked = True AndAlso xGridView.Rows(i).Cells(NumColonna_Check).Controls(1).Visible Then

                Dim Piva As String = xGridView.DataKeys(i).Item("Piva")
                Dim sa_cod As Integer = xGridView.DataKeys(i).Item("Sa_Cod")
                Dim appezza As Integer = xGridView.DataKeys(i).Item("Appezza")
                Dim Id_Reg As Integer = xGridView.DataKeys(i).Item("Id_Reg")
                Dim Cul_Cod As Integer = xGridView.DataKeys(i).Item("Cul_Cod")
                Dim Veg_Cod As Integer = xGridView.DataKeys(i).Item("Veg_Cod")

                Dim programmazione_entita_cod As Integer = 0

                If objParametriAgenda.TargetOperazione = enum_Tipo_Operazione_Agenda_Target.Planning Then
                    programmazione_entita_cod = xGridView.DataKeys(i).Item("Programmazione_Entita_Cod")
                End If

                Dim cul_des As String = New AgronicaCoreMetaSchemaDAL.Cultivar_R().CulDes_from_CulCod(Cul_Cod, objParametri_Server)
                Dim impianto As New AgronicaCoreModello.ParametriAgenda_Temp.Impianto
                impianto.Piva = Piva
                impianto.Sa_Cod = sa_cod
                impianto.Appezza = appezza
                impianto.ID_Reg = Id_Reg
                impianto.Programmazione_Entita_cod = programmazione_entita_cod
                impianto.Cul_Des = cul_des
                impianto.Udm_cod = Get_Udm(i)
                impianto.Mat_cod = Get_Prodotto(i)
                impianto.Qta = Get_Qta(i)
                impianto.Qta2 = Get_Superficie(i)

                impianto.Veg_Cod = Veg_Cod

                ListaImpianti.Add(impianto)


                '(25/05/2015 fede: commento per permettere di riseminare)
                '---------------------------------------
                ' Verifico se gia seminato o trapiantato
                'If objParametriAgenda.Lav_Cod = LAVCOD_SEMINA Or objParametriAgenda.Lav_Cod = LAVCOD_TRAPIANTO Then

                '    Dim dt As DataTable = New AgronicaCoreContabDAL.Mov_Destinazioni_R().Leggi(Piva, sa_cod, 0, 0, 0, appezza, Id_Reg, 0, AgronicaCoreDataProvider.AgronicaCoreParametri.enumSelezioneVariabile.Selezione_TabellaCompleta, " Agenda.Lav_Cod= " & objParametriAgenda.Lav_Cod & "", "", objParametri_Server)
                '    If dt.Rows.Count > 0 Then
                '        If objParametriAgenda.Tipo_Operazione = TipiEnumerativi.enum_TipoOperazioneDB.Modifica Then
                '            'se sono in modifica permetto il salvataggio comunque
                '        Else
                '            messaggio_errore = "L'impianto " & xGridView.Rows(i).Cells(NumColonna_App_Nome).Text & " risulta già seminato/trapiantato"
                '            Return False
                '        End If

                '    End If
                'End If

            End If

        Next

        If ListaImpianti.Count = 0 Then
            messaggio_errore = messaggio_errore & Resources.AgronicaAgenda_2010.SelezionareAlmenoUnImpiantoColturale
            Return False
        End If

        '---------------------------------------
        'Se è stato selezionato 'Tutti i centri aziendali' ma tutti gli impianti appartengono ad un solo centro, imposto il valore sull'objparametriAgenda
        If objParametriAgenda.Sa_Cod = "0" Then
            Dim listaSaCodImpianti As List(Of Integer) = (From x As AgronicaCoreModello.ParametriAgenda_Temp.Impianto In ListaImpianti Select x.Sa_Cod).Distinct().ToList()
            If listaSaCodImpianti.Count = 1 Then
                objParametriAgenda.Sa_Cod = listaSaCodImpianti.First().ToString()
            End If
        End If
        '---------------------------------------

        'se ho selezionato il magazzino verifico di aver selezionato almenno una risorsa
        If objParametriAgenda.Fabbricato <> "0" Then
            Dim AlmenoUnaRisorsa As Boolean = False
            For i = 0 To GridViewMagazzino.Rows.Count - 1
                If CType(GridViewMagazzino.Rows(i).Cells(NumColonna_Check).Controls(1), CheckBox).Checked = True Then
                    AlmenoUnaRisorsa = True
                    Exit For
                End If
            Next
            If AlmenoUnaRisorsa = False Then
                messaggio_errore = messaggio_errore & "Selezionare almeno una Risorsa di magazzino!"
                Return False
            End If
        End If



        'controlli semina Con_Frazionamento_perLotto
        Select Case RBL_Tipo_Semina.SelectedValue 'Tipo_Semina

            Case enum_SEMINA_TIPO.Frazionamento_Appezzamenti_perLotto_e_Semina_sui_Singoli_Default

                ''BLOCCO SE già movimentati (x ora poi svilupperemo frazionamentto delle operazioni già registrate)

                'Dim App_Movimentati As String = ""
                'Dim ObjDestR As New AgronicaCoreContabDAL.Mov_Destinazioni_R

                'For i = 0 To GridViewImpianti.Rows.Count - 1

                '    If CType(GridViewImpianti.Rows(i).Cells(NumColonna_Check).Controls(1), CheckBox).Checked = True Then

                '        Dim DtDest As DataTable
                '        DtDest = ObjDestR.Leggi(CStr(GridViewImpianti.DataKeys(i).Item("Piva")),
                '                         CInt(GridViewImpianti.DataKeys(i).Item("Sa_Cod")),
                '                         0, 0, 0,
                '                         CInt(GridViewImpianti.DataKeys(i).Item("Appezza")),
                '                         CInt(GridViewImpianti.DataKeys(i).Item("Id_Reg")),
                '                         0,
                '                         enumSelezioneVariabile.Selezione_JoinDescrizioni,
                '                         "",
                '                         "",
                '                         objParametri_Server)

                '        If Not IsNothing(DtDest) AndAlso DtDest.Rows.Count > 0 Then
                '            App_Movimentati &= GridViewImpianti.DataKeys(i).Item("App_Nome") & ","
                '        End If

                '    End If
                'Next

                'If App_Movimentati <> "" Then
                '    messaggio_errore = "Attenzione! Non è possibile modificare/frazionare gli appezzamenti " & Left(App_Movimentati, App_Movimentati.Length - 1) & " poichè sono già state registrate operazioni. Per procedere occorre prima eliminare tali operazioni."
                '    Return False
                'End If


                'BLOCCO SE la somma delle sup dei frazionati supera quella dell'app da frazionare

                Dim HashAppSelezionati As New Hashtable
                Dim App_CheSforano As String = ""
                Dim App_SupZero As String = ""

                For i = 0 To GridViewImpianti.Rows.Count - 1

                    If CType(GridViewImpianti.Rows(i).Cells(NumColonna_Check).Controls(1), CheckBox).Checked = True Then

                        If Not HashAppSelezionati.ContainsKey(CStr(GridViewImpianti.DataKeys(i).Item("Piva")) & "_" &
                                                   CInt(GridViewImpianti.DataKeys(i).Item("Sa_Cod")) & "_" &
                                                   CInt(GridViewImpianti.DataKeys(i).Item("Campo_Cod")) & "_" &
                                                    CInt(GridViewImpianti.DataKeys(i).Item("Appezza")) & "_" &
                                                    CInt(GridViewImpianti.DataKeys(i).Item("Id_Reg")) & "_" &
                                                    CDec(GridViewImpianti.DataKeys(i).Item("Sup_App")) & "_" &
                                                    CStr(GridViewImpianti.DataKeys(i).Item("App_Nome"))) Then

                            HashAppSelezionati.Add(CStr(GridViewImpianti.DataKeys(i).Item("Piva")) & "_" &
                                                   CInt(GridViewImpianti.DataKeys(i).Item("Sa_Cod")) & "_" &
                                                   CInt(GridViewImpianti.DataKeys(i).Item("Campo_Cod")) & "_" &
                                                    CInt(GridViewImpianti.DataKeys(i).Item("Appezza")) & "_" &
                                                    CInt(GridViewImpianti.DataKeys(i).Item("Id_Reg")) & "_" &
                                                    CDec(GridViewImpianti.DataKeys(i).Item("Sup_App")) & "_" &
                                                    CStr(GridViewImpianti.DataKeys(i).Item("App_Nome")), CDec(Get_Superficie(i)))

                            If IsNumeric(Get_Superficie(i)) AndAlso CDec(Get_Superficie(i)) <= 0 Then
                                App_SupZero &= CStr(GridViewImpianti.DataKeys(i).Item("App_Nome")) & ","
                            End If

                        Else

                            'aggiorno la sup totale
                            HashAppSelezionati(CStr(GridViewImpianti.DataKeys(i).Item("Piva")) & "_" &
                                                   CInt(GridViewImpianti.DataKeys(i).Item("Sa_Cod")) & "_" &
                                                   CInt(GridViewImpianti.DataKeys(i).Item("Campo_Cod")) & "_" &
                                                    CInt(GridViewImpianti.DataKeys(i).Item("Appezza")) & "_" &
                                                    CInt(GridViewImpianti.DataKeys(i).Item("Id_Reg")) & "_" &
                                                    CDec(GridViewImpianti.DataKeys(i).Item("Sup_App")) & "_" &
                                                    CStr(GridViewImpianti.DataKeys(i).Item("App_Nome"))) = CDec(HashAppSelezionati(CStr(GridViewImpianti.DataKeys(i).Item("Piva")) & "_" &
                                                                                                                               CInt(GridViewImpianti.DataKeys(i).Item("Sa_Cod")) & "_" &
                                                                                                                               CInt(GridViewImpianti.DataKeys(i).Item("Campo_Cod")) & "_" &
                                                                                                                                CInt(GridViewImpianti.DataKeys(i).Item("Appezza")) & "_" &
                                                                                                                                CInt(GridViewImpianti.DataKeys(i).Item("Id_Reg")) & "_" &
                                                                                                                                CDec(GridViewImpianti.DataKeys(i).Item("Sup_App")) & "_" &
                                                                                                                                CStr(GridViewImpianti.DataKeys(i).Item("App_Nome")))) + CDec(Get_Superficie(i))
                            If IsNumeric(Get_Superficie(i)) AndAlso CDec(Get_Superficie(i)) <= 0 Then
                                App_SupZero &= CStr(GridViewImpianti.DataKeys(i).Item("App_Nome")) & ","
                            End If


                        End If

                    End If
                Next

                For Each Key In HashAppSelezionati
                    If Not IsNothing(Split(Key.key, "_")(5)) AndAlso IsNumeric(Split(Key.key, "_")(5)) Then
                        If CDec(Split(Key.key, "_")(5)) < CDec(Key.value) Then
                            App_CheSforano &= Split(Key.key, "_")(6) & ","
                        End If
                    End If
                Next

                If App_CheSforano <> "" Or App_SupZero <> "" Then
                    If App_CheSforano <> "" Then
                        messaggio_errore = "Non è possibile modificare/frazionare gli appezzamenti " & Left(App_CheSforano, App_CheSforano.Length - 1) & " poichè la nuova superficie è superiore a quella attuale."
                    End If
                    If App_SupZero <> "" Then
                        messaggio_errore &= "<br>E' necessario indicare la nuova superficie (calcolata) degli appezzamenti " & Left(App_SupZero, App_SupZero.Length - 1) & "."
                    End If
                    Return False
                End If



                    'AVVISO SE presenti più di una particella che l'associazione verrà eliminata
                    'se è solo una viene mantenuta ed aggiornate le superfici

                    Dim App_ConPiuParticelle As String = ""

                If Catasto_SI_NO.Value = "0" Then

                    For i = 0 To GridViewImpianti.Rows.Count - 1

                        If CType(GridViewImpianti.Rows(i).Cells(NumColonna_Check).Controls(1), CheckBox).Checked = True Then

                            Dim StrCatasto As String = CStr(GridViewImpianti.DataKeys(i).Item("Catasto"))

                            If Not IsNothing(Split(StrCatasto, "<br>")) AndAlso Split(StrCatasto, "<br>").Length > 1 Then
                                App_ConPiuParticelle &= GridViewImpianti.DataKeys(i).Item("App_Nome") & ","
                            End If

                        End If

                    Next

                    If App_ConPiuParticelle <> "" Then
                        messaggio_errore = "Attenzione! Gli appezzamenti " & Left(App_ConPiuParticelle, App_ConPiuParticelle.Length - 1) & " hanno più particelle associate. A seguito della modifica/frazionamento sarà necessario rifare l'assegnazione."

                        ' VAnni: 28/2/2020: Non usare il resx sulla stringa "Catasto"
                        Messaggi.AgroSiNo(messaggio_errore & vbCr & Resources.AgronicaAgenda_2010.BrBIProcedereUgualmenteIB, "Catasto", Page, , updatepanelGridViewMagazzinoImpianti)
                        messaggioSiNo = True
                        Return False

                    End If

                Else
                    'se ho già cliccato  ok vado avanti

                End If



        End Select




        Try


            '-----------------------------------------------------
            '----------- CONNESSIONE E TRANSAZIONE ---------------
            Dim objDP As New AgronicaCoreDataProvider.ConnessioniTransazioni
            ConnessioniTransazioni.ApriConnessione(True, objParametri_Server)
            '-----------------------------------------------------

            Select Case RBL_Tipo_Semina.SelectedValue 'Tipo_Semina

                Case enum_SEMINA_TIPO.Frazionamento_Appezzamenti_perLotto_e_Semina_sui_Singoli_Default

                    '(06/06/2018)
                    'per ogni riga selezionata 
                    'creo un appezzamento/impianto
                    'registro la semina

                    Dim HashAppOldSelezionati As New Hashtable

                    Dim NuoviImp As New Hashtable
                    Dim DtNuoviImp As New DataTable


                    DtNuoviImp.Columns.Add("piva", GetType(String))
                    DtNuoviImp.Columns.Add("sa_cod", GetType(Integer))
                    DtNuoviImp.Columns.Add("appezza_old", GetType(Integer))
                    DtNuoviImp.Columns.Add("id_reg_old", GetType(Integer))
                    DtNuoviImp.Columns.Add("appezza_new", GetType(Integer))
                    DtNuoviImp.Columns.Add("id_reg_new", GetType(Integer))
                    DtNuoviImp.Columns.Add("sup_new", GetType(Decimal))
                    DtNuoviImp.Columns.Add("sup_perc", GetType(Decimal))



                    Dim AppezzaDaSeminare As Integer
                    Dim IdRegDaSeminare As Integer

                    Dim PrimaSemina As Boolean = False
                    Dim Id_Agenda_DaEscludere As Integer

                    For i = 0 To GridViewImpianti.Rows.Count - 1

                        PrimaSemina = False

                        If CType(GridViewImpianti.Rows(i).Cells(NumColonna_Check).Controls(1), CheckBox).Checked = True Then

                            '-------------------------------------------
                            ' il primo lo modifico

                            If Not HashAppOldSelezionati.ContainsKey(CStr(GridViewImpianti.DataKeys(i).Item("Piva")) & "_" &
                                                   CInt(GridViewImpianti.DataKeys(i).Item("Sa_Cod")) & "_" &
                                                   CInt(GridViewImpianti.DataKeys(i).Item("Campo_Cod")) & "_" &
                                                    CInt(GridViewImpianti.DataKeys(i).Item("Appezza")) & "_" &
                                                    CInt(GridViewImpianti.DataKeys(i).Item("Id_Reg")) & "_" &
                                                    CDec(GridViewImpianti.DataKeys(i).Item("Sup_App")) & "_" &
                                                    CDate(GridViewImpianti.DataKeys(i).Item("Validita_Inizio")) & "_" &
                                                    CDate(GridViewImpianti.DataKeys(i).Item("Validita_Fine")) & "_" &
                                                    CDate(GridViewImpianti.DataKeys(i).Item("Validita_Inizio_App")) & "_" &
                                                    CDate(GridViewImpianti.DataKeys(i).Item("Validita_Fine_App")) & "_" &
                                                    CDate(GridViewImpianti.DataKeys(i).Item("Validita_Inizio_Distinta")) & "_" &
                                                    CDate(GridViewImpianti.DataKeys(i).Item("Validita_Fine_Distinta")) & "_" &
                                                    CStr(GridViewImpianti.DataKeys(i).Item("App_Nome"))) Then

                                HashAppOldSelezionati.Add(CStr(GridViewImpianti.DataKeys(i).Item("Piva")) & "_" &
                                                   CInt(GridViewImpianti.DataKeys(i).Item("Sa_Cod")) & "_" &
                                                   CInt(GridViewImpianti.DataKeys(i).Item("Campo_Cod")) & "_" &
                                                    CInt(GridViewImpianti.DataKeys(i).Item("Appezza")) & "_" &
                                                    CInt(GridViewImpianti.DataKeys(i).Item("Id_Reg")) & "_" &
                                                    CDec(GridViewImpianti.DataKeys(i).Item("Sup_App")) & "_" &
                                                    CDate(GridViewImpianti.DataKeys(i).Item("Validita_Inizio")) & "_" &
                                                    CDate(GridViewImpianti.DataKeys(i).Item("Validita_Fine")) & "_" &
                                                    CDate(GridViewImpianti.DataKeys(i).Item("Validita_Inizio_App")) & "_" &
                                                    CDate(GridViewImpianti.DataKeys(i).Item("Validita_Fine_App")) & "_" &
                                                    CDate(GridViewImpianti.DataKeys(i).Item("Validita_Inizio_Distinta")) & "_" &
                                                    CDate(GridViewImpianti.DataKeys(i).Item("Validita_Fine_Distinta")) & "_" &
                                                    CStr(GridViewImpianti.DataKeys(i).Item("App_Nome")), CDec(Get_Superficie(i)))

                                'il primo selezionato lo modifico
                                Modifica_Impianto(i, messaggio_errore, "", True)

                                Dim SupPerc As Integer = 0
                                SupPerc = CDec(Get_Superficie(i)) * 100 / GridViewImpianti.DataKeys(i).Item("Sup_App")

                                Dim dr As DataRow = DtNuoviImp.NewRow
                                dr.Item("piva") = GridViewImpianti.DataKeys(i).Item("Piva")
                                dr.Item("sa_cod") = CInt(GridViewImpianti.DataKeys(i).Item("Sa_Cod"))
                                dr.Item("appezza_old") = CInt(GridViewImpianti.DataKeys(i).Item("Appezza"))
                                dr.Item("id_reg_old") = CInt(GridViewImpianti.DataKeys(i).Item("Id_Reg"))
                                dr.Item("appezza_new") = CInt(GridViewImpianti.DataKeys(i).Item("Appezza"))
                                dr.Item("id_reg_new") = CInt(GridViewImpianti.DataKeys(i).Item("Id_Reg"))
                                dr.Item("sup_new") = CDec(Get_Superficie(i))
                                dr.Item("sup_perc") = SupPerc
                                DtNuoviImp.Rows.Add(dr)

                                AppezzaDaSeminare = GridViewImpianti.DataKeys(i).Item("Appezza")
                                IdRegDaSeminare = GridViewImpianti.DataKeys(i).Item("Id_Reg")

                                PrimaSemina = True

                                '-------------------------------------------
                                ' gli altri li creo
                            Else
                                Dim AppezzaNew As Integer = 0
                                Dim IdRegNew As Integer = 0
                                Dim AssociaCatasto As Boolean = False

                                Dim strCatasto As String = CStr(GridViewImpianti.DataKeys(i).Item("Catasto"))
                                If Not IsNothing(Split(strCatasto, "<br>")) AndAlso Split(strCatasto, "<br>").Length = 1 Then
                                    AssociaCatasto = True
                                End If

                                CreaAppezzamento(CStr(GridViewImpianti.DataKeys(i).Item("Piva")), CInt(GridViewImpianti.DataKeys(i).Item("Sa_Cod")), CInt(GridViewImpianti.DataKeys(i).Item("Campo_Cod")),
                                                 CInt(GridViewImpianti.DataKeys(i).Item("Appezza")), CInt(GridViewImpianti.DataKeys(i).Item("Id_Reg")),
                                                 GridViewImpianti.DataKeys(i).Item("Validita_Inizio"), GridViewImpianti.DataKeys(i).Item("Validita_Fine"),
                                                 GridViewImpianti.DataKeys(i).Item("Validita_Inizio_App"), GridViewImpianti.DataKeys(i).Item("Validita_Fine_App"),
                                                 GridViewImpianti.DataKeys(i).Item("Validita_Inizio_Distinta"), GridViewImpianti.DataKeys(i).Item("Validita_Fine_Distinta"),
                                                 Get_App_Nome(i), Get_Superficie(i), Get_P_Ha(i),
                                                 Get_MetodoProduttivo(i), Get_Varieta(i), Get_Finalita(i), Get_Distinta(i), Get_Disciplinare(i),
                                                 AppezzaNew, IdRegNew, DtNuoviImp, GridViewImpianti.DataKeys(i).Item("Sup_App"),
                                                 AssociaCatasto)

                                AppezzaDaSeminare = AppezzaNew
                                IdRegDaSeminare = IdRegNew

                                'aggiorno la sup totale
                                HashAppOldSelezionati(CStr(GridViewImpianti.DataKeys(i).Item("Piva")) & "_" &
                                                   CInt(GridViewImpianti.DataKeys(i).Item("Sa_Cod")) & "_" &
                                                   CInt(GridViewImpianti.DataKeys(i).Item("Campo_Cod")) & "_" &
                                                    CInt(GridViewImpianti.DataKeys(i).Item("Appezza")) & "_" &
                                                    CInt(GridViewImpianti.DataKeys(i).Item("Id_Reg")) & "_" &
                                                    CDec(GridViewImpianti.DataKeys(i).Item("Sup_App")) & "_" &
                                                    CDate(GridViewImpianti.DataKeys(i).Item("Validita_Inizio")) & "_" &
                                                    CDate(GridViewImpianti.DataKeys(i).Item("Validita_Fine")) & "_" &
                                                    CDate(GridViewImpianti.DataKeys(i).Item("Validita_Inizio_App")) & "_" &
                                                    CDate(GridViewImpianti.DataKeys(i).Item("Validita_Fine_App")) & "_" &
                                                    CDate(GridViewImpianti.DataKeys(i).Item("Validita_Inizio_Distinta")) & "_" &
                                                    CDate(GridViewImpianti.DataKeys(i).Item("Validita_Fine_Distinta")) & "_" &
                                                    CStr(GridViewImpianti.DataKeys(i).Item("App_Nome"))) = CDec(HashAppOldSelezionati(CStr(GridViewImpianti.DataKeys(i).Item("Piva")) & "_" &
                                                                                                                               CInt(GridViewImpianti.DataKeys(i).Item("Sa_Cod")) & "_" &
                                                                                                                               CInt(GridViewImpianti.DataKeys(i).Item("Campo_Cod")) & "_" &
                                                                                                                                CInt(GridViewImpianti.DataKeys(i).Item("Appezza")) & "_" &
                                                                                                                                CInt(GridViewImpianti.DataKeys(i).Item("Id_Reg")) & "_" &
                                                                                                                                CDec(GridViewImpianti.DataKeys(i).Item("Sup_App")) & "_" &
                                                                                                                                CDate(GridViewImpianti.DataKeys(i).Item("Validita_Inizio")) & "_" &
                                                                                                                                CDate(GridViewImpianti.DataKeys(i).Item("Validita_Fine")) & "_" &
                                                                                                                                CDate(GridViewImpianti.DataKeys(i).Item("Validita_Inizio_App")) & "_" &
                                                                                                                                CDate(GridViewImpianti.DataKeys(i).Item("Validita_Fine_App")) & "_" &
                                                                                                                                CDate(GridViewImpianti.DataKeys(i).Item("Validita_Inizio_Distinta")) & "_" &
                                                                                                                                CDate(GridViewImpianti.DataKeys(i).Item("Validita_Fine_Distinta")) & "_" &
                                                                                                                                CStr(GridViewImpianti.DataKeys(i).Item("App_Nome")))) + CDec(Get_Superficie(i))

                            End If

                            '-------------------------------------------
                            ' registro la semina
                            Dim Agenda As Operazione_Agenda = CreaOggettoAgendaSingoloApp(i, CInt(GridViewImpianti.DataKeys(i).Item("Sa_Cod")), AppezzaDaSeminare, IdRegDaSeminare, messaggio_errore)

                            If IsNothing(Agenda) Then
                                Throw New Exception(Resources.AgronicaAgenda_2010.NonÈStatoPossibileCreareLOperazione & messaggio_errore)
                            End If

                            Dim objAgendaScrivi As New Agenda_Operazione_Helper

                            Dim Id_Agenda As Integer
                            Id_Agenda = objAgendaScrivi.Scrivi(Agenda, objParametri_Server)

                            If PrimaSemina = True Then
                                Id_Agenda_DaEscludere = Id_Agenda
                            End If

                        End If

                    Next


                    'per gli impianti selezionati
                    'se non è stata utilizzata tutta la superficie 
                    'viene generato un appezzamento terreno nudo 
                    For Each Key In HashAppOldSelezionati
                        If Not IsNothing(Split(Key.key, "_")(5)) AndAlso IsNumeric(Split(Key.key, "_")(5)) Then
                            If CDec(Split(Key.key, "_")(5)) > CDec(Key.value) Then
                                Dim AppezzaNew As Integer = 0
                                Dim IdRegNew As Integer = 0
                                Dim AppNomeNew As String = CStr(Split(Key.key, "_")(12)) & " - a"
                                CreaAppezzamento(Split(Key.key, "_")(0), CInt(Split(Key.key, "_")(1)), CInt(Split(Key.key, "_")(2)),
                                                 CInt(Split(Key.key, "_")(3)), CInt(Split(Key.key, "_")(4)),
                                                 CDate(Split(Key.key, "_")(6)), CDate(Split(Key.key, "_")(7)),
                                                 CDate(Split(Key.key, "_")(8)), CDate(Split(Key.key, "_")(9)),
                                                 CDate(Split(Key.key, "_")(10)), CDate(Split(Key.key, "_")(11)),
                                                 AppNomeNew,
                                                 CDec(Split(Key.key, "_")(5)) - CDec(Key.value),
                                                 0,
                                                 0, 0, 0, "Terreno Nudo", 0,
                                                 AppezzaNew, IdRegNew, DtNuoviImp, CDec(Split(Key.key, "_")(5)), True)
                            End If
                        End If
                    Next

                    res = True
                    'to do

                    FrazionaAgenda(HashAppOldSelezionati, DtNuoviImp, Id_Agenda_DaEscludere)


                Case Else

                    If objParametriAgenda.Sa_Cod = "0" And objParametriAgenda.TargetOperazione = enum_Tipo_Operazione_Agenda_Target.Reale Then

                        '--------------------------------------------------
                        '--------------------------------------------------
                        '--------- OPERAZIONE MULTI-CENTRO    -------------
                        '--------------------------------------------------
                        '--------------------------------------------------                

                        'apro transazione per scrittura multipla di movimenti
                        '-----------------------------------------------------
                        ' ''----------- CONNESSIONE E TRANSAZIONE ---------------


                        'creazione lista centri coinvolti nell'operazione
                        Dim ListaSacod As New List(Of Integer)
                        Dim Trovato As Boolean
                        For i = 0 To ListaImpianti.Count - 1
                            Trovato = False
                            For j = 0 To ListaSacod.Count - 1
                                If ListaImpianti(i).Sa_Cod = ListaSacod(j) Then
                                    Trovato = True
                                    Exit For
                                End If
                            Next
                            If Trovato = False Then
                                ListaSacod.Add(ListaImpianti(i).Sa_Cod)
                            End If
                        Next

                        '----------

                        'Inizio il ciclo sui centri Aziendali
                        'Ciclo per ogni centro aziendale
                        For i = 0 To ListaSacod.Count - 1

                            Dim ListaImpiantixQuestoSaCod As New List(Of AgronicaCoreModello.ParametriAgenda_Temp.Impianto)

                            'Seleziono solamente gli impianti relativi a questo centro aziendale
                            For j = 0 To ListaImpianti.Count - 1
                                If ListaImpianti(j).Sa_Cod = ListaSacod(i) Then
                                    ListaImpiantixQuestoSaCod.Add(ListaImpianti(j))
                                End If
                            Next

                            '--------------------------------------------------
                            '--------------------------------------------------
                            '--------- CREAZIONE OGGETTO DA SALVARE   ---------
                            '--------------------------------------------------
                            '--------------------------------------------------

                            'I mov dettagli riferiti sono inclusi nel movimento dettaglio dell'operazione
                            'e la tabella [Mov_Dettagli_Riferimenti] viene modificata dentro la scrittura del 
                            'movimento dettaglio

                            Dim Agenda As Operazione_Agenda = CreaOggettoAgenda(ListaImpiantixQuestoSaCod, ListaSacod(i), messaggio_errore)

                            If IsNothing(Agenda) Then
                                Throw New Exception(Resources.AgronicaAgenda_2010.NonÈStatoPossibileCreareLOperazione & messaggio_errore)
                            End If


                            '--------------------------------------------------------------------------------
                            '--------- SCRITTURA OGGETTO AGENDA  --------------------------------------------
                            '--------------------------------------------------------------------------------



                            Dim Id_Agenda As Integer

                            '1) scrittura semina
                            'I mov dettagli riferiti sono inclusi nel movimento dettaglio dell'operazione
                            'e la tabella [Mov_Dettagli_Riferimenti] viene modificata dentro la scrittura del 
                            'movimento dettaglio, anche la cancellazione dei riferimenti ai movimenti dett tecnici
                            'è inclusa nellla cancellazione dei mov dettagli

                            Dim util As New Utility_NS.Utility_Operazioni

                            If objParametriAgenda.Fabbricato <> "0" AndAlso Split(objParametriAgenda.Fabbricato, "|").Count = 3 AndAlso Split(objParametriAgenda.Fabbricato, "|")(2) <> objParametriAgenda.Piva Then

                                util.Gestisci_Magazzino_Aziendale(Agenda, Id_Agenda, objParametriAgenda, objParametri_Server)

                            Else

                                Dim objAgendaScrivi As New Agenda_Operazione_Helper

                                If objParametriAgenda.Tipo_Operazione = enum_TipoOperazioneDB.Modifica Then


                                    Dim CancellataOperazione As Boolean = False

                                    CancellataOperazione = objAgendaScrivi.Cancella(objParametriAgenda.Piva,
                                                                                     objParametriAgenda.Sa_Cod,
                                                                                     objParametriAgenda.Id_Agenda, False,
                                                                                     objParametri_Server, logCancellazione:=False)

                                End If

                                Id_Agenda = objAgendaScrivi.Scrivi(Agenda, objParametri_Server)

                            End If

                            id_agende_lista &= ", " & Id_Agenda


                            'Dim objAgendaScrivi As New Agenda_Operazione_Helper
                            'Dim Id_Agenda As Integer

                            ''1) scrittura semina
                            ''I mov dettagli riferiti sono inclusi nel movimento dettaglio dell'operazione
                            ''e la tabella [Mov_Dettagli_Riferimenti] viene modificata dentro la scrittura del 
                            ''movimento dettaglio, anche la cancellazione dei riferimenti ai movimenti dett tecnici
                            ''è inclusa nellla cancellazione dei mov dettagli
                            'Id_Agenda = objAgendaScrivi.Scrivi(Agenda, objParametri_Server)
                            'id_agende_lista &= ", " & Id_Agenda

                            ''non spostare, l'operazione agenda vecchia in modifica devo eliminarla dopo
                            'Dim util As New Utility_NS.Utility_Operazioni()
                            'util.Gestisci_Magazzino_Aziendale(Agenda, objParametriAgenda, objParametri_Server)

                            ''se la scrittura è andata a buon fine e sono in modifica
                            ''CANCELLO la vecchia operazione

                            'If Id_Agenda <> 0 And objParametriAgenda.Tipo_Operazione = enum_TipoOperazioneDB.Modifica Then
                            '    If Id_Agenda_Old > 0 Then
                            '        Dim CancellataOperazione As Boolean = False

                            '        CancellataOperazione = objAgendaScrivi.Cancella(objParametriAgenda.Piva, _
                            '                                                         objParametriAgenda.Sa_Cod, _
                            '                                                         Id_Agenda_Old, False, _
                            '                                                         objParametri_Server)


                            '        'modifico l'aggancio alla ricetta
                            '        Dim objRicetta As New AgronicaCoreContabDAL.RicettexAgenda_W
                            '        Dim ModRicetta As Boolean
                            '        ModRicetta = objRicetta.Modifica_Agenda(0, 0, _
                            '                                                Id_Agenda_Old, _
                            '                                                Id_Agenda, _
                            '                                                AGRODATAINIZIO, _
                            '                                                AGRODATAFINE, _
                            '                                                "", _
                            '                                                objParametri_Server)
                            '    End If
                            'End If

                            'objAgendaScrivi = Nothing

                            'se sono n scrittura devo agganciare la ricetta se presente
                            If Id_Agenda <> 0 And objParametriAgenda.Tipo_Operazione = enum_TipoOperazioneDB.Scrittura Then
                                ' se Session("UtilizzataRicetta") esiste e se è true allora ho utilizzato la ricetta per creare l'ìoperazione
                                If Not IsNothing(Session("UtilizzataRicetta")) AndAlso Session("UtilizzataRicetta") Then

                                    Dim ricetta_cod As String = Session("ricetta_cod")
                                    Dim Ricetta_Operazione_Cod As String = Session("Ricetta_Operazione_Cod")

                                    Dim objUtenti As New AgronicaCoreUtentiDAL.Utenti_Impostazioni_Read
                                    Dim Impostazione_RicetteXagenda As String = objUtenti.ImpostazioneValore1_from_ImpostazioneCod(enum_Impostazioni_Utenti.UTENTE_COD_DEFAULT_RiferimentoRicetteOperazioni,
                                                                            HttpContext.Current.Session("ASG_objParametri_Utenti"),
                                                                            1)
                                    If Impostazione_RicetteXagenda <> "0" Then
                                        Dim objRicetta As New AgronicaCoreContabDAL.RicettexAgenda_W
                                        If Not objRicetta.Scrivi(ricetta_cod, Ricetta_Operazione_Cod, Id_Agenda, AGRODATAINIZIO, AGRODATAFINE, objParametri_Server) Then
                                            Throw New Exception(Resources.AgronicaAgenda_2010.BNonÈRiuscitoLAggancioDellaRicettaBBr)
                                        End If
                                    End If



                                End If
                            End If

                            'objParametriAgenda.Id_Agenda = Id_Agenda


                        Next


                    Else

                        '--------------------------------------------------
                        '--------------------------------------------------
                        '--------- OPERAZIONE SINGOLO CENTRO    -----------
                        '--------------------------------------------------
                        '--------------------------------------------------

                        '--------------------------------------------------
                        '--------------------------------------------------
                        '--------- CREAZIONE OGGETTO DA SALVARE   ---------
                        '--------------------------------------------------
                        '--------------------------------------------------


                        'I mov dettagli riferiti sono inclusi nel movimento dettaglio dell'operazione
                        'e la tabella [Mov_Dettagli_Riferimenti] viene modificata dentro la scrittura del 
                        'movimento dettaglio
                        Dim Agenda As Operazione_Agenda = CreaOggettoAgenda(ListaImpianti, objParametriAgenda.Sa_Cod, messaggio_errore)
                        If IsNothing(Agenda) Then
                            Throw New Exception(Resources.AgronicaAgenda_2010.NonÈStatoPossibileCreareLOperazione & messaggio_errore)
                        End If


                        '--------------------------------------------------
                        '--------------------------------------------------
                        '---  FINE CREAZIONE OGGETTO DA SALVARE   ---------
                        '--------------------------------------------------
                        '--------------------------------------------------

                        If objParametriAgenda.TipoOperazioneAgenda = enum_Tipo_Operazione_Agenda.Ricetta Then

                            'una volta creato l'oggetto agenda posso invocare il suo metodo che mi genera l'xml
                            Dim Helper As New Agenda_Operazione_Helper
                            strXMLAgenda = Helper.GeneraXML_CAU_LAVORAZIONI(Agenda)


                        Else

                            '--------------------------------------------------------------------------------
                            '--------- SCRITTURA OGGETTO AGENDA  --------------------------------------------
                            '--------------------------------------------------------------------------------


                            Dim Id_Agenda As Integer

                            '1) scrittura semina
                            'I mov dettagli riferiti sono inclusi nel movimento dettaglio dell'operazione
                            'e la tabella [Mov_Dettagli_Riferimenti] viene modificata dentro la scrittura del 
                            'movimento dettaglio, anche la cancellazione dei riferimenti ai movimenti dett tecnici
                            'è inclusa nellla cancellazione dei mov dettagli

                            Dim util As New Utility_NS.Utility_Operazioni

                            If objParametriAgenda.Fabbricato <> "0" AndAlso Split(objParametriAgenda.Fabbricato, "|").Count = 3 AndAlso Split(objParametriAgenda.Fabbricato, "|")(2) <> objParametriAgenda.Piva Then

                                util.Gestisci_Magazzino_Aziendale(Agenda, Id_Agenda, objParametriAgenda, objParametri_Server)

                            Else

                                Dim objAgendaScrivi As New Agenda_Operazione_Helper

                                If objParametriAgenda.Tipo_Operazione = enum_TipoOperazioneDB.Modifica Then

                                    'Recupero i vecchi costi CdG prima che vengano cancellati
                                    Dim mdr_Rif As New Agenda_Movimenti_Dettagli_Riferimenti_Helper()
                                    Dim lista_mdRif As List(Of Movimento_Dettaglio_Riferimento) = mdr_Rif.LeggiRiferimentiAgenda(Agenda.Piva, 0, Agenda.Id_Agenda, 0, "", objParametri_Server)


                                    Dim CancellataOperazione As Boolean = objAgendaScrivi.Cancella(objParametriAgenda.Piva,
                                                                                                         objParametriAgenda.Sa_Cod,
                                                                                                         objParametriAgenda.Id_Agenda, False,
                                                                                                         objParametri_Server, logCancellazione:=False)

                                    'Riscrivo i vecchi costi
                                    Dim mdRif_helper As New Agenda_Movimenti_Dettagli_Riferimenti_Helper
                                    For Each mdRif As Movimento_Dettaglio_Riferimento In lista_mdRif
                                        If mdRif.Lav_Cod_Rif = LAVCOD_COSTI_CDG Then
                                            mdRif_helper.Scrivi(mdRif, objParametri_Server)
                                            Exit For
                                        End If
                                    Next

                                End If

                                Id_Agenda = objAgendaScrivi.Scrivi(Agenda, objParametri_Server)

                            End If

                            id_agende_lista &= ", " & Id_Agenda


                            'Id_Agenda = objAgendaScrivi.Scrivi(Agenda, objParametri_Server)
                            'id_agende_lista &= ", " & Id_Agenda

                            ''non spostare, l'operazione agenda vecchia in modifica devo eliminarla dopo
                            'util.Gestisci_Magazzino_Aziendale(Agenda, Id_Agenda, objParametriAgenda, objParametri_Server)

                            'se la scrittura è andata a buon fine e sono in modifica
                            'CANCELLO la vecchia operazione

                            'If Id_Agenda <> 0 And objParametriAgenda.Tipo_Operazione = enum_TipoOperazioneDB.Modifica Then
                            '    If Id_Agenda_Old > 0 Then
                            '        Dim CancellataOperazione As Boolean = False

                            '        CancellataOperazione = objAgendaScrivi.Cancella(objParametriAgenda.Piva, _
                            '                                                         objParametriAgenda.Sa_Cod, _
                            '                                                         Id_Agenda_Old, False, _
                            '                                                         objParametri_Server)


                            '        'modifico l'aggancio alla ricetta
                            '        Dim objRicetta As New AgronicaCoreContabDAL.RicettexAgenda_W
                            '        Dim ModRicetta As Boolean
                            '        ModRicetta = objRicetta.Modifica_Agenda(0, 0, _
                            '                                                Id_Agenda_Old, _
                            '                                                Id_Agenda, _
                            '                                                AGRODATAINIZIO, _
                            '                                                AGRODATAFINE, _
                            '                                                "", _
                            '                                                objParametri_Server)


                            '        'Devo Eliminare anche le altrte operazioni magazzino se presenti



                            '    End If
                            'End If

                            'objAgendaScrivi = Nothing

                            'se sono n scrittura devo agganciare la ricetta se presente
                            If Id_Agenda <> 0 And objParametriAgenda.Tipo_Operazione = enum_TipoOperazioneDB.Scrittura Then
                                ' se Session("UtilizzataRicetta") esiste e se è true allora ho utilizzato la ricetta per creare l'ìoperazione
                                If Not IsNothing(Session("UtilizzataRicetta")) AndAlso Session("UtilizzataRicetta") Then

                                    Dim ricetta_cod As String = Session("ricetta_cod")
                                    Dim Ricetta_Operazione_Cod As String = Session("Ricetta_Operazione_Cod")

                                    Dim objUtenti As New AgronicaCoreUtentiDAL.Utenti_Impostazioni_Read
                                    Dim Impostazione_RicetteXagenda As String = objUtenti.ImpostazioneValore1_from_ImpostazioneCod(enum_Impostazioni_Utenti.UTENTE_COD_DEFAULT_RiferimentoRicetteOperazioni,
                                                                            HttpContext.Current.Session("ASG_objParametri_Utenti"),
                                                                            1)
                                    If Impostazione_RicetteXagenda <> "0" Then
                                        Dim objRicetta As New AgronicaCoreContabDAL.RicettexAgenda_W
                                        If Not objRicetta.Scrivi(ricetta_cod, Ricetta_Operazione_Cod, Id_Agenda, AGRODATAINIZIO, AGRODATAFINE, objParametri_Server) Then
                                            Throw New Exception(Resources.AgronicaAgenda_2010.BNonÈRiuscitoLAggancioDellaRicettaBBr)
                                        End If
                                    End If

                                End If
                            End If

                            If CType(Ricerca.FindControlIterative(Page.Master, "RBL_Salva"), RadioButtonList).SelectedValue = 0 OrElse
                                CType(Ricerca.FindControlIterative(Page.Master, "RBL_Salva"), RadioButtonList).SelectedValue = 3 Then
                                objParametriAgenda.Id_Agenda = Id_Agenda
                            End If



                        End If

                    End If

                    res = True

                    If objParametriAgenda.TargetOperazione = enum_Tipo_Operazione_Agenda_Target.Reale And strXMLAgenda = "" Then
                        'TODO: capire questo....
                        '------------Salvo le modifiche agli impianti prima di chiudere transazione
                        '(30/08/2018 fede) nuova versione 
                        'gli appezzamenti sono modificati solo se si è scelta l'apposita impostazione
                        'con il sovescio non modifico i dati impianto
                        If RBL_Tipo_Semina.SelectedValue = enum_SEMINA_TIPO.Semina_e_Modifica_Appezzamenti_Default And
                            objParametriAgenda.Lav_Cod <> LAVCOD_SOVESCIO Then
                            Dim msg As String
                            If Not SalvaModificheImpianti(msg, id_agende_lista) Then
                                Throw New Exception(Resources.AgronicaAgenda_2010.NonÈStatoPossibileSalvareLeModificheAgliIm & msg)
                            End If
                        End If



                    End If

                    If strXMLAgenda <> "" Then

                        Dim objWebW As New AgronicaCoreVarieDAL.Web_ComunicazionePagine_W
                        Unid_Operazione = System.Guid.NewGuid.ToString
                        res = objWebW.Scrivi(Unid_Operazione, 0, enum_TipoOperazioneDB.Scrittura, 0, "", "", strXMLAgenda, "", objParametri_Server)

                    End If

            End Select





            '--------------------------------------------------------------------------
            ''=================================
            ''======== TRANSAZIONE OK =========
            'commit transazione
            ConnessioniTransazioni.ChiudiTransazione(1, objParametri_Server)

        Catch ex As Exception

            'commit transazione rollback
            ConnessioniTransazioni.ChiudiTransazione(2, objParametri_Server)

            'Throw New Exception("[ Agenda_Operazione_Helper.scrivi() ] : " & ex.Message)

            'Messaggi.AgroMsgBox("Attenzione, l'operazione non è stata registrata! <br> " & ex.Message, Page, , _
            '                   CType(Ricerca.FindControlIterative(Page.Master, "UpdatePanelToolBar"), UpdatePanel))

            messaggio_errore = ex.Message
            res = False

        Finally

            'chiudi connessione
            ConnessioniTransazioni.ChiudiConnessione(objParametri_Server)

        End Try



        Return res

    End Function




    Private Sub fine_salvataggio(ByVal TipoSalvataggio As Integer, ByVal Unid_Operazione As String) Implements iOperazioneGUI.fine_salvataggio

        Session("UtilizzataRicetta") = False

        Dim strJS As StringBuilder

        strJS = New StringBuilder
        strJS.AppendLine("$(document).ready(function () { ")

        If objParametriAgenda.TipoOperazioneAgenda = enum_Tipo_Operazione_Agenda.Ricetta Then

            strJS.AppendLine("      window.location = '../Ricette/Ricette_Edit.aspx?unid_ricetta=" & Sicurezza.Stringa_Codifica(Qs_Unid_Ricetta, CostantiPersonalizzate.AgroKey_EncoderDecoder) &
                    "&unid_operazione=" & Sicurezza.Stringa_Codifica(Unid_Operazione, CostantiPersonalizzate.AgroKey_EncoderDecoder) &
                    "&o=" & Sicurezza.Stringa_Codifica(Qs_Operazione_Ricetta, CostantiPersonalizzate.AgroKey_EncoderDecoder) &
                    "&tipo_ricetta=" & Sicurezza.Stringa_Codifica(objParametriAgenda.TipoRicetta, CostantiPersonalizzate.AgroKey_EncoderDecoder) &
                    "&r=" & Sicurezza.Stringa_Codifica(Qs_Ricetta_Cod, CostantiPersonalizzate.AgroKey_EncoderDecoder) &
                    "'; ")

            strJS.AppendLine(" });")


            ScriptManager.RegisterClientScriptBlock(Master_Operazione.Property_UpdatePanelPerScript, Master_Operazione.Property_UpdatePanelPerScript.GetType(),
                                                    String.Format("jQuery_{0}", Master_Operazione.Property_UpdatePanelPerScript.ClientID), strJS.ToString, True)

        Else


            'se ho la lista allora sono arrivato dall'anagrafe e devo tornare lì
            'Dim enumredirect As Integer = enum_PagineGiasOnline.MenuAgenda
            Dim tornaanagrafica As Boolean = False
            'If objParametriAgenda.Impianti.Count = 1 Then
            '    objParametriAgenda.TornaASitoOrigine = True
            '    tornaanagrafica = True
            '    objParametriAgenda.Svuota_DatiOperazione()
            '    Response.Redirect(CType(Master.Master, Agenda).TrovaRedirectCorretto(True, enum_PagineGiasOnline.AlberoImprese, objParametriAgenda))
            'End If


            Select Case TipoSalvataggio

                Case enum_Tipo_Salvataggio.Salva_e_Esci
                    If objParametriAgenda.SitoOrigine = Enum_SiteRedirector.GiasLan Then
                        strJS = New StringBuilder
                        strJS.AppendLine("$(document).ready(function () { ")
                        strJS.AppendLine("      window.close(); ")
                        strJS.AppendLine(" });")

                        ScriptManager.RegisterStartupScript(
                                       UpdatePanelMagazzinoImpianti,
                                       UpdatePanelMagazzinoImpianti.GetType(),
                                           String.Format("jQuery_{0}", UpdatePanelMagazzinoImpianti.ClientID), strJS.ToString, True)

                        Exit Sub
                    End If


                    Dim link As String = ""
                    Try
                        Dim sitoorigine As Enum_SiteRedirector = HttpContext.Current.Session("Sito_Origine")
                        Dim paginaOnLineRitorno As Integer = objParametriAgenda.PaginaSitoOrigine

                        If sitoorigine = Enum_SiteRedirector.Sito_GiasOnline_2010 And paginaOnLineRitorno = enum_PagineGiasOnline_2010.RegistazioneSmart Then
                            link = AgronicaCoreGestioneRichieste.RedirectGestione.IndirizzoCompleto_SitoOnline2010_PassandoDirettamenteIParametri(
                                                   Enum_SiteRedirector.Sito_AgronicaAgenda_2010,
                                                   enum_PagineGiasOnline_2010.RegistazioneSmart,
                                                   enum_PagineAgenda_2010.Menu, objParametriAgenda.Piva, "", "", 0, "")

                        ElseIf tornaanagrafica Then

                            Dim objGiasOnline As New AgronicaCoreGestioneRichieste.ParametriGiasOnline
                            objGiasOnline.Cul_Cod = objParametriAgenda.Cul_Cod
                            objGiasOnline.DataSelezionata = objParametriAgenda.Data
                            objGiasOnline.Id_Agenda = objParametriAgenda.Id_Agenda
                            objGiasOnline.Lavorazione = objParametriAgenda.Lav_Cod
                            objGiasOnline.PaginaRichiesta = enum_PagineGiasOnline.AlberoImprese
                            objGiasOnline.Piva = objParametriAgenda.Piva
                            objGiasOnline.Sa_Cod = objParametriAgenda.Sa_Cod
                            Dim specie As Integer = 0
                            If IsNumeric(objParametriAgenda.Veg_Cod.Split("/")(0)) AndAlso CInt(objParametriAgenda.Veg_Cod.Split("/")(0)) > 0 Then
                                specie = CInt(objParametriAgenda.Veg_Cod.Split("/")(0))
                            End If
                            objGiasOnline.Veg_Cod = specie

                            link = AgronicaCoreGestioneRichieste.RedirectGestione.IndirizzoCompleto_SitoOnline_PassandoDirettamente_ParametriGiasOnline(
                                           Enum_SiteRedirector.Sito_AgronicaAgenda_2010,
                                           objGiasOnline)
                        Else
                            link = CType(Master.Master, Agenda).TrovaRedirectCorretto(False, enum_PagineGiasOnline.MenuAgenda, objParametriAgenda)
                        End If

                    Catch ex As Exception
                        link = CType(Master.Master, Agenda).TrovaRedirectCorretto(False, enum_PagineGiasOnline.MenuAgenda, objParametriAgenda)
                    End Try

                    strJS = New StringBuilder
                    strJS.AppendLine("$(document).ready(function () { ")
                    strJS.AppendLine("      ChiamataParent_Id_Ageda(" & objParametriAgenda.Id_Agenda & "); ")
                    strJS.AppendLine("      window.location = '" & link & "'; ")
                    strJS.AppendLine(" });")

                    ScriptManager.RegisterStartupScript(Master_Operazione.Property_UpdatePanelPerScript, Master_Operazione.Property_UpdatePanelPerScript.GetType(),
                                                    String.Format("jQuery_{0}", Master_Operazione.Property_UpdatePanelPerScript.ClientID), strJS.ToString, True)

                    'ScriptManager.RegisterStartupScript(CType(Ricerca.FindControlIterative(Page.Master, "UpdatePanelToolBar1"), UpdatePanel), CType(Ricerca.FindControlIterative(Page.Master, "UpdatePanelToolBar1"), UpdatePanel).GetType(),
                    '                              String.Format("jQuery_{0}", CType(Ricerca.FindControlIterative(Page.Master, "UpdatePanelToolBar1"), UpdatePanel).ClientID), strJS.ToString, True)

                    Messaggi.AgroMsgBuonFine(Resources.AgronicaAgenda_2010.RegistrazioneEffettuataConSuccesso, Page, ,
                                      CType(Ricerca.FindControlIterative(Page.Master, "UpdatePanelToolBar"), UpdatePanel))

                    objParametriAgenda.Svuota_DatiOperazione()
                    'Response.Redirect(CType(Master.Master, Agenda).TrovaRedirectCorretto(False, enumredirect, objParametriAgenda))

                Case enum_Tipo_Salvataggio.Salva_e_Nuovo
                    Giacenza_SI_NO.Value = "0"
                    Catasto_SI_NO.Value = "0"
                    MateriaPrimaBio_SI_NO.Value = "0"
                    objParametriAgenda.Impianti = New List(Of AgronicaCoreModello.ParametriAgenda_Temp.Impianto)
                    objParametriAgenda.Note = New List(Of Nota)
                    objParametriAgenda.Movimenti = New List(Of Movimento)


                    Messaggi.AgroMsgBuonFine(Resources.AgronicaAgenda_2010.RegistrazioneEffettuataConSuccesso, Page, ,
                                      Master_Operazione.Property_UpdatePanelToolBar)

                    strJS = New StringBuilder
                    strJS.AppendLine("$(document).ready(function () { ")
                    'strJS.AppendLine("      alert(''); ")
                    strJS.AppendLine("      window.location = '../Operazioni/Semina_e_Trapianto_1.aspx'; ")
                    strJS.AppendLine(" });")

                    ScriptManager.RegisterClientScriptBlock(Master_Operazione.Property_UpdatePanelToolBar,
                                                        Master_Operazione.Property_UpdatePanelToolBar.GetType(),
                                                  String.Format("jQuery_{0}", Master_Operazione.Property_UpdatePanelToolBar.ClientID),
                                                  strJS.ToString, True)

                    caricaTabelle(False)

                Case enum_Tipo_Salvataggio.Salva_e_Duplica
                    Giacenza_SI_NO.Value = "0"
                    Catasto_SI_NO.Value = "0"
                    MateriaPrimaBio_SI_NO.Value = "0"
                    'elimino la lista impianti, che in questa operazione serve solo per memorizzare l'impianto quandol'operazione è
                    'chiamata dall'albero anagrafe e deve gestire un solo impianto
                    objParametriAgenda.Impianti = New List(Of AgronicaCoreModello.ParametriAgenda_Temp.Impianto)

                    'Dim strJS As New StringBuilder
                    'strJS.AppendLine("$(document).ready(function () { ")
                    'strJS.AppendLine("      BloccaSbloccaTotale(); ")
                    'strJS.AppendLine(" });")
                    'ScriptManager.RegisterStartupScript(Master_Operazione.Property_UpdatePanelToolBar1, Master_Operazione.Property_UpdatePanelToolBar1.GetType(),
                    '                              String.Format("jQuery_{0}", Master_Operazione.Property_UpdatePanelToolBar1.ClientID), strJS.ToString, True)

                    Messaggi.AgroMsgBuonFine(Resources.AgronicaAgenda_2010.RegistrazioneEffettuataConSuccesso, Page, ,
                                      Master_Operazione.Property_UpdatePanelToolBar)


                    caricaListeValoriDaTabelle()
                    caricaTabelle(False)

                Case enum_Tipo_Salvataggio.Salva_e_Vai_ai_Costi

                    Dim PaginaLink As String = "../AnalisiCostiProduzione/GestioneCosti.aspx"

                    Dim link As String = ""
                    Try
                        Dim sitoorigine As Enum_SiteRedirector = HttpContext.Current.Session("Sito_Origine")
                        Dim paginaOnLineRitorno As Integer = objParametriAgenda.PaginaSitoOrigine

                        If sitoorigine = Enum_SiteRedirector.Sito_GiasOnline_2010 And paginaOnLineRitorno = enum_PagineGiasOnline_2010.RegistazioneSmart Then
                            link = AgronicaCoreGestioneRichieste.RedirectGestione.IndirizzoCompleto_SitoOnline2010_PassandoDirettamenteIParametri(
                                                   Enum_SiteRedirector.Sito_AgronicaAgenda_2010,
                                                   enum_PagineGiasOnline_2010.RegistazioneSmart,
                                                   enum_PagineAgenda_2010.Menu, objParametriAgenda.Piva, "", "", 0, "")

                        Else
                            link = CType(Master.Master, Agenda).TrovaRedirectCorretto(False, enum_PagineGiasOnline.MenuAgenda, objParametriAgenda)
                        End If

                    Catch ex As Exception
                        link = CType(Master.Master, AgendaBootstrap).TrovaRedirectCorretto(False, enum_PagineGiasOnline.MenuAgenda, objParametriAgenda)
                    End Try

                    PaginaLink &= "?p=" & Stringa_Codifica(objParametriAgenda.Piva, AgroKey_EncoderDecoder) &
                              "&id_agenda=" & Stringa_Codifica(objParametriAgenda.Id_Agenda, AgroKey_EncoderDecoder) &
                              "&origine=" & Stringa_Codifica(link, AgroKey_EncoderDecoder) &
                              "&entrata_diretta=" & Stringa_Codifica(0, AgroKey_EncoderDecoder, Nothing)

                    strJS = New StringBuilder
                    strJS.AppendLine("$(document).ready(function () { ")
                    strJS.AppendLine("      ChiamataParent_Id_Ageda(" & objParametriAgenda.Id_Agenda & "); ")
                    strJS.AppendLine("      window.location = '" & PaginaLink & "'; ")
                    strJS.AppendLine(" });")

                    ScriptManager.RegisterStartupScript(Master_Operazione.Property_UpdatePanelPerScript, Master_Operazione.Property_UpdatePanelPerScript.GetType(),
                                                    String.Format("jQuery_{0}", Master_Operazione.Property_UpdatePanelPerScript.ClientID), strJS.ToString, True)

                    Messaggi.AgroMsgBuonFine(Resources.AgronicaAgenda_2010.RegistrazioneEffettuataConSuccesso, Page, ,
                                      CType(Ricerca.FindControlIterative(Page.Master, "UpdatePanelToolBar"), UpdatePanel))

                    objParametriAgenda.Svuota_DatiOperazione()

            End Select

        End If

    End Sub


#End Region


#Region "Creazione oggetto Agenda"


    Private Function CreaOggettoAgenda(ByVal ListaImp As List(Of AgronicaCoreModello.ParametriAgenda_Temp.Impianto), ByVal Sa_Cod As Integer, ByRef messaggio_errore As String) As Operazione_Agenda
        Dim Agenda As Operazione_Agenda

        '--------------AGENDA------------------
        If Not Crea_Agenda(ListaImp, Sa_Cod, Agenda, messaggio_errore) Then
            Return Nothing
        End If


        '--------------NOTE------------------
        If Not Crea_Agenda_Note(Agenda) Then
            Return Nothing
        End If


        '---------------MOVIMENTI COSTI ACCESSORI--------
        If Not Crea_Agenda_Movimento_CostiAccessori(Agenda) Then
            Return Nothing
        End If


        '------------- MOVIMENTO RILIEVO / TRATTAMENTO---------
        'inneschiNumTotale usato solo per trappole e massa, ignorato per conf e dis sessuale
        Select Case CInt(objParametriAgenda.Lav_Cod)

            Case LAVCOD_SEMINA, LAVCOD_TRAPIANTO, LAVCOD_SOVESCIO, LAVCOD_SOD_SEDDING
                If Not Crea_Agenda_Movimento_Semina(ListaImp, Agenda, messaggio_errore) Then
                    Return Nothing
                End If


            Case Else
                Throw New NotImplementedException
        End Select


        '-------------- MOVIMENTO SCARICO---------------------------
        'modifica per multicentro,solo se l'operazione agenda riguarda il magazzino del centro selezionato
        'If Sa_Cod = Mag_Sa_Cod AndAlso objParametriAgenda.Fabbricato <> "0" Then
        If objParametriAgenda.Fabbricato <> "0" Then
            'se è selezionato il magazzino
            'uno scarico totale per le trappole e uno pergli inneschi (solo per massa e trappole)
            'inneschiNumTotale usato solo per trappole e massa, ignorato per conf e dis sessuale
            If Not Crea_Agenda_Movimento_Scarico(Agenda, messaggio_errore) Then
                Return Nothing
            End If
        End If


        Return Agenda

    End Function


    Private Function Crea_Agenda(ByRef ListaImp As List(Of AgronicaCoreModello.ParametriAgenda_Temp.Impianto), ByRef Sa_Cod As Integer, ByRef Agenda As Operazione_Agenda, ByRef messaggio_errore As String) As Boolean

        '---------------------------------------
        ' recupero la NOTE
        'Dim strNota As String = CType(Master, Operazione).GetNota()

        '---------------------------------------
        ' recupero la DATA
        Dim Data As Date
        If objParametriAgenda.Data = AGRODATAINIZIO Then
            messaggio_errore = messaggio_errore & Resources.AgronicaAgenda_2010.IndicareUnaData
            Return False
        Else
            Data = objParametriAgenda.Data
        End If

        '---------------------------------------
        ' recupero la SPECIE
        Dim Veg_Cod As String = ""
        Dim Veg_Des As String = ""
        'If objParametriAgenda.Veg_Cod.Split("/")(0) = "-1" Or objParametriAgenda.Veg_Cod.Split("/")(0) = "0" Then
        If objParametriAgenda.Veg_Cod.Split("/")(0) = "-1" Then
            messaggio_errore = messaggio_errore & Resources.AgronicaAgenda_2010.SelezionareAlmenoUnaSpecie
            Return False
        Else
            Veg_Cod = objParametriAgenda.Veg_Cod.Split("/")(0)
            Veg_Des = Master_Operazione.Property_ComboSpecie.Testo_Combo
        End If


        '---------------------------------------
        ' recupero la OPERAZIONE
        'Dim Lav_Cod As String = ""
        Dim Lav_Des As String = ""
        If objParametriAgenda.Lav_Cod = "" Then
            messaggio_errore = messaggio_errore & Resources.AgronicaAgenda_2010.SelezionareUnOperazione
            Return False
        Else
            ''Lav_Cod = objParametriAgenda.Lav_Cod
            'Lav_Des = Master_Operazione.Property_ComboOperazione.Testo_Combo
            'objParametriAgenda.Lav_Des = Lav_Des
            Dim objOperazioni As New AgronicaCoreAnagrafeDAL.Operazioni_R
            Lav_Des = objOperazioni.Lav_Des_From_Lav_Cod(objParametriAgenda.Lav_Cod, objParametri_Server)
        End If



        Dim BaseCode As Integer
        Dim TopCode As Integer

        '------------------------------------------------
        '----- Calcolo i valori di BaseCode e TopCode
        '------------------------------------------------
        Call Calcola_BaseCode_TopCode(BaseCode,
                              TopCode,
                              Session("ASG_ProgressivoGIAS"))



        '------------------------------------------------
        '----- AGENDA
        '------------------------------------------------

        'le varietà possono cambiare da quele del'impianto, quindi le aggiungo dopo

        ' ''Dim trovato As Boolean
        ' ''Dim ListaVarieta As New List(Of String)
        ' ''Dim StrVarieta As String = ""
        ' ''For i = 0 To ListaImp.Count - 1
        ' ''    trovato = False
        ' ''    For j = 0 To ListaVarieta.Count - 1
        ' ''        If ListaVarieta(j) = ListaImp(i).Cul_Des Then
        ' ''            trovato = True
        ' ''            Exit For
        ' ''        End If
        ' ''    Next
        ' ''    If trovato = False Then
        ' ''        ListaVarieta.Add(ListaImp(i).Cul_Des)
        ' ''        StrVarieta += ", " + ListaImp(i).Cul_Des
        ' ''    End If
        ' ''Next

        ' ''StrVarieta = StrVarieta.Substring(2, (StrVarieta.Length - 2))

        Agenda = New Operazione_Agenda

        Agenda.Tipo_Operazione = objParametriAgenda.Tipo_Operazione
        'Agenda.Id_Agenda = 0
        Agenda.Id_Agenda = objParametriAgenda.Id_Agenda
        Agenda.Data = Data
        Agenda.Piva = objParametriAgenda.Piva
        Agenda.Sa_Cod = Sa_Cod
        Agenda.Lav_Cod = objParametriAgenda.Lav_Cod
        Agenda.Des_Lib = Lav_Des & " (" & Veg_Des & "  ["

        Agenda.BaseCode = BaseCode
        Agenda.TopCode = TopCode

        Return True
    End Function



    Private Function Crea_Agenda_Note(ByRef Agenda As Operazione_Agenda) As Boolean
        Dim Nota As Nota
        Dim ListaConsigli As List(Of Nota)
        ListaConsigli = CType(Master, Operazione).GetConsigli()
        If ListaConsigli.Count > 0 Then
            Agenda.Note = New List(Of Nota)
            For i = 0 To ListaConsigli.Count - 1
                Nota = New Nota
                Nota.Id_Agenda = objParametriAgenda.Id_Agenda
                Nota.Nota_Cod = ListaConsigli(i).Nota_Cod
                Agenda.Note.Add(Nota)
            Next
        End If
        Return True
    End Function


    Private Function Crea_Agenda_Movimento_CostiAccessori(ByRef Agenda As Operazione_Agenda) As Boolean
        For i = 0 To objParametriAgenda.Movimenti.Count - 1
            objParametriAgenda.Movimenti(i).Id_Agenda = objParametriAgenda.Id_Agenda
            objParametriAgenda.Movimenti(i).Sa_Cod = Agenda.Sa_Cod
            If Not IsNothing(objParametriAgenda.Movimenti(i).Movimenti_Dettagli) Then
                Dim j As Integer
                For j = 0 To objParametriAgenda.Movimenti(i).Movimenti_Dettagli.Count - 1
                    objParametriAgenda.Movimenti(i).Movimenti_Dettagli(j).Id_Agenda = objParametriAgenda.Id_Agenda
                    If objParametriAgenda.Movimenti(i).Movimenti_Dettagli(j).Sa_Cod = 0 Then
                        objParametriAgenda.Movimenti(i).Movimenti_Dettagli(j).Sa_Cod = Agenda.Sa_Cod
                    End If
                Next
            End If

            If Not IsNothing(objParametriAgenda.Movimenti(i).Movimenti_Dettagli_Tecnici) Then
                Dim j As Integer
                For j = 0 To objParametriAgenda.Movimenti(i).Movimenti_Dettagli_Tecnici.Count - 1
                    objParametriAgenda.Movimenti(i).Movimenti_Dettagli_Tecnici(j).Id_Agenda = objParametriAgenda.Id_Agenda
                    If objParametriAgenda.Movimenti(i).Movimenti_Dettagli_Tecnici(j).Sa_Cod = 0 Then
                        objParametriAgenda.Movimenti(i).Movimenti_Dettagli_Tecnici(j).Sa_Cod = Agenda.Sa_Cod
                    End If

                Next
            End If

            objParametriAgenda.Movimenti(i).Data = Agenda.Data
            Agenda.Movimenti.Add(objParametriAgenda.Movimenti(i))
        Next
        Return True
    End Function


    Private Function Crea_Agenda_Movimento_Semina(ByVal ListaImp As List(Of AgronicaCoreModello.ParametriAgenda_Temp.Impianto), ByRef Agenda As Operazione_Agenda, ByRef messaggio_errore As String) As Boolean

        Select Case CInt(objParametriAgenda.Lav_Cod)

            Case LAVCOD_SEMINA, LAVCOD_TRAPIANTO, LAVCOD_SOVESCIO, LAVCOD_SOD_SEDDING
                'continua, questo metodo deve essere chioamato solo in questi casi altrimenti genero accezione
            Case Else
                Throw New Exception
        End Select

        If ListaImp.Count = 0 Then
            messaggio_errore = messaggio_errore & Resources.AgronicaAgenda_2010.InserirePrimaLaTabella
            Return False
        End If

        Dim Movimento_OperazioneColturale As New Movimento

        Movimento_OperazioneColturale.Id_Agenda = objParametriAgenda.Id_Agenda
        Movimento_OperazioneColturale.Piva = Agenda.Piva
        Movimento_OperazioneColturale.Sa_Cod = Agenda.Sa_Cod
        Movimento_OperazioneColturale.Lav_Cod = objParametriAgenda.Lav_Cod
        Movimento_OperazioneColturale.Cau_Mov = objParametriAgenda.Cau_Mov
        Movimento_OperazioneColturale.Mov_Desc = Master_Operazione.GetNota()
        Movimento_OperazioneColturale.Data = Agenda.Data
        Movimento_OperazioneColturale.BaseCode = Agenda.BaseCode
        Movimento_OperazioneColturale.TopCode = Agenda.TopCode

        Movimento_OperazioneColturale.Extra_Int = RBL_Tipo_Semina.SelectedValue  'Tipo_Semina 'salvo il tipo raccolta, puo essere utile

        If objParametriAgenda.Fabbricato <> "0" Then

            'If objParametriAgenda.Lav_Cod = LAVCOD_SOVESCIO Then
            '    Crea_Agenda_Movimento_Sovescio_ConMagazzino(Agenda, Movimento_OperazioneColturale)
            'Else
            'quando si ha il magazzino o comunque anche senza magazzino e con la tabella dei lotti come nell'operazione dell'agenda precedente
            'End If

            If Not Crea_Agenda_Movimento_Semina_ConMagazzino(Agenda, Movimento_OperazioneColturale, messaggio_errore) Then
                Return Nothing
            End If

        Else

            Crea_Agenda_Movimento_Semina_SenzaMagazzino(ListaImp, Agenda, Movimento_OperazioneColturale)

        End If

        'Se ho almeno un movimento dettaglio nell'operazione la aggiungo all'agenda
        '(il movimento dettaglio contiene una destinazione e un mov dettaglio tecnico)
        'Se il movimento  non ha almeno un dettaglio allora genero una eccezione,
        'perchè una situazione del genere non dovrebbe mai accadere in quanto deve essere creato almeno un 
        'movimento dettaglio per ciascun centro, e tutti gli impianti selezionati nella master devono 
        'creare un movimentoi dettaglio corrispondente, anche se su centri diversi.

        If Movimento_OperazioneColturale.Movimenti_Dettagli.Count > 0 Then
            Agenda.Movimenti.Add(Movimento_OperazioneColturale)
        Else
            messaggio_errore = messaggio_errore & Resources.AgronicaAgenda_2010.ErroreNonPrevisto
            Return False
        End If



        Return True
    End Function

    Private Sub Crea_Agenda_Movimento_Semina_SenzaMagazzino(ByVal ListaImp As List(Of AgronicaCoreModello.ParametriAgenda_Temp.Impianto), ByRef Agenda As Operazione_Agenda, ByVal Movimento_OperazioneColturale As Movimento)

        If objParametriAgenda.Fabbricato <> "0" Then
            Throw New NotImplementedException
        End If

        '----------------------------------------------------------
        '----- MOVIMENTI DETTAGLI , DET.TECNICI, DESTINAZIONI -----
        '----------------------------------------------------------

        'impostazioni in base alla lavorazione
        Select Case CInt(objParametriAgenda.Lav_Cod)
            Case LAVCOD_SEMINA, LAVCOD_TRAPIANTO, LAVCOD_SOVESCIO, LAVCOD_SOD_SEDDING
            Case Else
                Throw New NotImplementedException
        End Select


        If Not IsNothing(ListaImp) AndAlso ListaImp.Count > 0 Then

            Dim righeConDati As Integer = 0 'conteggio le righe con i dati, se non ci sono allora non salvo questa operazione agenda

            Dim ListaProdottiUDM = (
                From lPu In ListaImp
                Select lPu.Mat_cod, lPu.Udm_cod
            ).Distinct.ToList

            Dim cultivarAggiunte As New List(Of String)

            For Each curProdottoUdm In ListaProdottiUDM

                '------------------------------
                '----- MOVIMENTO DETTAGLIO-----
                '------------------------------
                'Creo un unico movimento dettaglio, 
                'in questo caso sono senza magazzino quindi ho un unico movimento dettaglio che dipende dal lotto, (solo semente) e 
                'non ho più lotti e più mov dettagli come con il magazzino
                'Successivamente aggiungo tutte le righe selezionate come destinazioni a questo mov dettaglio
                Dim Movimento_Dettaglio As New Movimento_Dettaglio
                Movimento_Dettaglio.Id_Agenda = objParametriAgenda.Id_Agenda
                Movimento_Dettaglio.Piva = Agenda.Piva
                Movimento_Dettaglio.Sa_Cod = Agenda.Sa_Cod

                Movimento_Dettaglio.Elem_Cod = SEMENTI 'fisso

                'ho controllato in precedenza che ci sia almeno una riga negli impianti
                'prendo i valori dall'impianto non dal magazzino che non ho
                If objParametriAgenda.TargetOperazione = enum_Tipo_Operazione_Agenda_Target.Reale Then
                    If objParametriAgenda.Lav_Cod = LAVCOD_SOVESCIO Then
                        Movimento_Dettaglio.Pro_Cod = 1
                    Else
                        Movimento_Dettaglio.Pro_Cod = GridViewImpianti.DataKeys(0).Item("Sem_Cod_Lotto") '0 se ho magazzino, sem_cod altrimenti
                    End If
                Else
                    Movimento_Dettaglio.Pro_Cod = 0
                End If


                'TODO: non più vero: per costi accessori occorre indicare un Mat_cod
                If objParametriAgenda.TargetOperazione = enum_Tipo_Operazione_Agenda_Target.Reale Then
                    'Hack: mantengo algoritmo precedente...
                    Movimento_Dettaglio.Mat_Cod = 0 '0 se no magazzino, Mat_Cod altrimenti
                    Movimento_Dettaglio.Udm_Cod = 0
                Else
                    Movimento_Dettaglio.Mat_Cod = curProdottoUdm.Mat_cod
                    Movimento_Dettaglio.Udm_Cod = curProdottoUdm.Udm_cod
                End If


                Movimento_Dettaglio.Mov_Det_Des = "Dettagli Tecnici Materia Prima per Semina/Trapianto"
                Movimento_Dettaglio.Qta = 0 'aggiungo dopo man mano
                Movimento_Dettaglio.Contabilizzato = Movimento_Dettaglio_Contabilizzato 'verificare
                Movimento_Dettaglio.Pendente = Movimento_Dettaglio_Pendente 'verificare fisso forse
                Movimento_Dettaglio.Data = Agenda.Data
                Movimento_Dettaglio.BaseCode = Agenda.BaseCode
                Movimento_Dettaglio.TopCode = Agenda.TopCode
                Movimento_Dettaglio.Cau_Mov = objParametriAgenda.Cau_Mov
                Movimento_Dettaglio.Anno = 1900
                Movimento_Dettaglio.Lotto = ""


                'non usato finora, ma utile per la pagina
                'prendo i valori dall'impianto non dal magazzino che non ho
                If objParametriAgenda.TargetOperazione = enum_Tipo_Operazione_Agenda_Target.Reale Then
                    If objParametriAgenda.Lav_Cod <> LAVCOD_SOVESCIO Then
                        Movimento_Dettaglio.Extra_Str = GridViewImpianti.DataKeys(0).Item("Descrizione_Lotto")
                    End If
                Else
                    Movimento_Dettaglio.Extra_Str = ""
                End If



                For Each curImp In ListaImp

                    'TODO: capire questo: se ho delle righe nascoste c'è un errore, in quento presuppongo che senza magazzino ci sia un solo lotto
                    'If GridViewImpianti.Rows(i).Cells(NumColonna_Check).Controls(1).Visible = False Then
                    '    Throw New NotImplementedException
                    'End If

                    'TODO: controllo non più necessario ...?
                    'If CType(GridViewImpianti.Rows(i).Cells(NumColonna_Check).Controls(1), CheckBox).Checked = True Then
                    'se la riga appartiene a questo centro la aggiungo, altrimenti deve essere aggiunta all'operazione agenda 
                    'del centro corrispondente (se sono in multicentro)


                    If curImp.Mat_cod = curProdottoUdm.Mat_cod And curImp.Udm_cod = curProdottoUdm.Udm_cod Then

                        If Agenda.Piva = curImp.Piva AndAlso (Agenda.Sa_Cod = curImp.Sa_Cod Or objParametriAgenda.TargetOperazione = enum_Tipo_Operazione_Agenda_Target.Planning) Then

                            'imposto la prima volta l'unità di misura
                            'If righeConDati = 0 Then
                            Movimento_Dettaglio.Udm_Cod = curImp.Udm_cod 'Get_Udm(i)
                            'End If

                            'occorre impostare sempre la stessa udm
                            If Movimento_Dettaglio.Udm_Cod <> curImp.Udm_cod Then 'Get_Udm(i) 
                                Throw New NotImplementedException(Resources.AgronicaAgenda_2010.OccorreImpostareSempreLaStessaUnitàDiMisur)
                            End If

                            Dim Movimento_Destinazione As New Movimento_Destinazione
                            Movimento_Destinazione.Id_Agenda = objParametriAgenda.Id_Agenda
                            Movimento_Destinazione.Piva = curImp.Piva  'GridViewImpianti.DataKeys(i).Item("Piva")
                            Movimento_Destinazione.Sa_Cod = curImp.Sa_Cod  'GridViewImpianti.DataKeys(i).Item("Sa_Cod")
                            Movimento_Destinazione.Appezza = curImp.Appezza 'GridViewImpianti.DataKeys(i).Item("Appezza")
                            Movimento_Destinazione.Id_Destinazione = curImp.ID_Reg 'GridViewImpianti.DataKeys(i).Item("Id_Reg")
                            Movimento_Destinazione.Programmazione_Entita_Cod = curImp.Programmazione_Entita_cod
                            Movimento_Destinazione.Qta = curImp.Qta  'Get_Qta(i)
                            Movimento_Destinazione.Qta2 = curImp.Qta2
                            Movimento_Destinazione.Data = Agenda.Data

                            Movimento_Dettaglio.Qta += Movimento_Destinazione.Qta

                            'aggiungo il mov destinazione al mov dettaglio del lotto
                            Movimento_Dettaglio.Movimenti_Destinazioni.Add(Movimento_Destinazione)


                            'aggiungo qui la varietà dinamicamente 
                            'Agenda.Des_Lib = Lav_Des & " (" & Veg_Des & "  [" + StrVarieta + "])"
                            'Agenda.Des_Lib = Lav_Des & " (" & Veg_Des & "  ["
                            Dim var As String = curImp.Cul_Des 'Get_Varieta_Des(i)

                            If Not cultivarAggiunte.Contains(var) Then

                                cultivarAggiunte.Add(var)

                                If cultivarAggiunte.Count = 0 Or cultivarAggiunte.Count = 1 Then
                                    Agenda.Des_Lib = Agenda.Des_Lib & "" & var & " "
                                Else
                                    Agenda.Des_Lib = Agenda.Des_Lib & ", " & var & ""
                                End If

                            End If

                            righeConDati += 1

                        End If





                    End If 'Impianto su mat_Cod, udm_Cod


                Next 'Impianto

                'Calcolo la quota distribuzione: su quantità distribuita se presente oppure su area.
                Dim xSomma As Decimal
                If Movimento_Dettaglio.Qta <> 0 Then
                    xSomma = Movimento_Dettaglio.Qta
                Else
                    xSomma = (From xDest In Movimento_Dettaglio.Movimenti_Destinazioni Select xDest.Qta2).Sum()
                End If

                If xSomma <> 0 Then

                    For Each xQuotaDistr In Movimento_Dettaglio.Movimenti_Destinazioni
                        If xQuotaDistr.Qta <> 0 Then
                            xQuotaDistr.QuotaDistribuzione = xQuotaDistr.Qta / xSomma
                        Else
                            xQuotaDistr.QuotaDistribuzione = xQuotaDistr.Qta2 / xSomma
                        End If

                    Next
                End If

                Movimento_OperazioneColturale.Movimenti_Dettagli.Add(Movimento_Dettaglio)


            Next 'For Each curProdottoUdm In ListaProdottiUDM

            Agenda.Des_Lib = Agenda.Des_Lib & "])"
            If righeConDati = 0 Then
                Throw New NotImplementedException(Resources.AgronicaAgenda_2010.NonSonoStateTrovateRigheConDatiNegliImpian)
            End If

        End If
    End Sub


    'Caso con magazzino, ma funzionante anche senza magazzino e con griglia magazzino visibile
    'se si vuole reimpostare l'utilizzo griglia magazzino nenza magazzino per esempio nel caso 
    'di più tipi di lotti di default basta scommentare la parte che lo gestisce
    Private Function Crea_Agenda_Movimento_Semina_ConMagazzino(ByRef Agenda As Operazione_Agenda, ByVal Movimento_OperazioneColturale As Movimento, ByRef messaggio_errore As String) As Boolean

        Dim AllegataUnaBollaAlmeno As Boolean = False
        Dim bolleAllegate As Integer = 0
        '----------------------------------------------------------
        '----- MOVIMENTI DETTAGLI , DET.TECNICI, DESTINAZIONI -----
        '----------------------------------------------------------
        'per ciascuna giacenza, quindi per ciascuna riga selezionata della grid magazzino creo un movimento dettaglio
        'Per tutti gli impianti in cui viene utilizzato il prodotto aggiungo un mov destinazione

        Dim VerificaQtaZero As Boolean = False
        Dim objUtenti As New AgronicaCoreUtentiDAL.Utenti_Impostazioni_Read
        Dim BloccoSemina As String = objUtenti.ImpostazioneValore1_from_ImpostazioneCod(enum_Impostazioni_Utenti.UTENTE_COD_BLOCCA_SEMINA_SE_SENZA_QTA,
                                                HttpContext.Current.Session("ASG_objParametri_Utenti"),
                                                1)
        If BloccoSemina <> "0" Then
            VerificaQtaZero = True
        End If


        For i = 0 To GridViewMagazzino.Rows.Count - 1

            'Se la riga e' selezionata ...
            If CType(GridViewMagazzino.Rows(i).Cells(NumColonna_Check).Controls(1), CheckBox).Checked = True Then

                'verifico che per la materia prima sia selezionato almeno un impianto con qta <> 0
                If VerificaQtaZero = True Then

                    Dim ProdottoSelezionato As Boolean = False

                    For imp = 0 To GridViewImpianti.Rows.Count - 1

                        'Se la riga e' selezionata ...
                        If CType(GridViewImpianti.Rows(imp).Cells(NumColonna_Check).Controls(1), CheckBox).Checked = True AndAlso
                            GridViewImpianti.Rows(imp).Cells(NumColonna_Check).Controls(1).Visible Then
                            'se la riga appartiene a questo centro
                            If Agenda.Piva = GridViewImpianti.DataKeys(imp).Item("Piva") AndAlso Agenda.Sa_Cod = GridViewImpianti.DataKeys(imp).Item("Sa_Cod") Then

                                For j = imp To GridViewImpianti.Rows.Count - 1

                                    'all'inizio del ciclo sono nella riga con il check visibile, nei cicli successivi
                                    'sono nelle righe dello stesso impianto, con check nascosti,
                                    'se trovo un altro check visibile allora esco dal ciclo
                                    If GridViewMagazzino.DataKeys(i).Item("Mat_Cod") = GridViewImpianti.DataKeys(j).Item("Mat_Cod_Lotto") AndAlso
                                       GridViewMagazzino.DataKeys(i).Item("Udm_Cod") = GridViewImpianti.DataKeys(j).Item("Udm_Cod_Lotto") AndAlso
                                       GridViewMagazzino.DataKeys(i).Item("Lotto") = GridViewImpianti.DataKeys(j).Item("Lotto_Lotto") AndAlso
                                       IsNumeric(Get_Qta(j)) AndAlso
                                       CDbl(Get_Qta(j)) > 0 Then
                                        ProdottoSelezionato = True
                                        Exit For
                                    End If

                                    '---------------------------------
                                    imp = j 'così all'uscita risparmio dei cicli
                                    If j + 1 = GridViewImpianti.Rows.Count Then
                                        'la prossima riga è l'ultima, controlo qui per non fare eccezione nell'if successivo
                                        Exit For
                                    End If

                                    If GridViewImpianti.Rows(j + 1).Cells(NumColonna_Check).Controls(1).Visible = True Then
                                        'se la prossima riga si riferisce ad un nuovo impianto esco dal for
                                        Exit For
                                    Else
                                        'la prossima riga o è l'ultima o si riferisce allo stesso impianto,
                                        'continuo e all'inizio del ciclo inserisco la nuova riga
                                    End If
                                Next

                                If ProdottoSelezionato = True Then
                                    Exit For
                                End If

                            End If

                        End If

                    Next

                    If ProdottoSelezionato = False Then
                        Throw New Exception("ATTENZIONE! Per " & GridViewMagazzino.DataKeys(i).Item("Descrizione") & " non è stata indicata la quantità in nessun impianto!")
                    End If



                End If

                '------------------------------
                '----- MOVIMENTO DETTAGLIO-----
                '------------------------------
                Dim Movimento_Dettaglio As New Movimento_Dettaglio
                Movimento_Dettaglio.Id_Agenda = objParametriAgenda.Id_Agenda
                Movimento_Dettaglio.Piva = Agenda.Piva 'se non uso il magazzino la piva è "" e il sacod è 0
                Movimento_Dettaglio.Sa_Cod = Agenda.Sa_Cod
                Movimento_Dettaglio.Elem_Cod = SEMENTI 'fisso
                If objParametriAgenda.Fabbricato <> "0" Then
                    Movimento_Dettaglio.Pro_Cod = 0 '0 se magazzino, sem_cod altrimenti
                    Movimento_Dettaglio.Mat_Cod = GridViewMagazzino.DataKeys(i).Item("Mat_Cod") '0 se no magazzino, Mat_Cod altrimenti
                    Movimento_Dettaglio.Udm_Cod = GridViewMagazzino.DataKeys(i).Item("Udm_Cod")
                    Movimento_Dettaglio.Lotto = GridViewMagazzino.DataKeys(i).Item("Lotto")
                Else
                    '' ''Movimento_Dettaglio.Pro_Cod = GridViewMagazzino.DataKeys(i).Item("Sem_Cod") '0 se magazzino, sem_cod altrimenti
                    '' ''Movimento_Dettaglio.Mat_Cod = 0 '0 se no magazzino, Mat_Cod altrimenti
                    '' ''Movimento_Dettaglio.Udm_Cod = 0
                End If
                Movimento_Dettaglio.Mov_Det_Des = "Dettagli Tecnici Materia Prima per Semina/Trapianto"
                Movimento_Dettaglio.Qta = 0 'aggiungo dopo man mano
                Movimento_Dettaglio.Contabilizzato = Movimento_Dettaglio_Contabilizzato 'verificare
                Movimento_Dettaglio.Pendente = Movimento_Dettaglio_Pendente 'verificare fisso forse
                Movimento_Dettaglio.Data = Agenda.Data
                Movimento_Dettaglio.BaseCode = Agenda.BaseCode
                Movimento_Dettaglio.TopCode = Agenda.TopCode
                Movimento_Dettaglio.Cau_Mov = objParametriAgenda.Cau_Mov
                Movimento_Dettaglio.Anno = 1900
                'impostazioni in base alla lavorazione
                Select Case CInt(objParametriAgenda.Lav_Cod)
                    Case LAVCOD_SEMINA, LAVCOD_TRAPIANTO, LAVCOD_SOVESCIO, LAVCOD_SOD_SEDDING
                    Case Else
                        Throw New NotImplementedException
                End Select

                'non usato finore, ma utile per la pagina
                Movimento_Dettaglio.Extra_Str = GridViewMagazzino.DataKeys(i).Item("Descrizione")

                '---------------------------------------------------------------------------------------------------
                '--------GESTIONE DELLA BOLLA COLLEGATA--------------------------------------------------------------
                '-------------------------------------------------------------------------------------------
                Dim prodottoIGiaInserito As Boolean = False
                Dim Bolla As String = GridViewMagazzino.DataKeys(i).Item("Bolla")
                If Bolla <> "" Then
                    If Bolla.Split("|").Count <> 3 Then
                        Throw New Exception(Resources.AgronicaAgenda_2010.CÈUnaBollaAllegataMaIlFormatoNonÈRiconosci)
                    End If
                    If GridViewMagazzino.Columns(6).Visible = False Or
                        GridViewMagazzino.Columns(7).Visible = False Or
                        GridViewMagazzino.Columns(8).Visible = False Or
                        GridViewMagazzino.Columns(9).Visible = False Then
                        Throw New Exception(Resources.AgronicaAgenda_2010.CÈUnaBollaAllegataMaLeColonneDelleBolleNon)
                    End If
                    If IsNothing(Session("MovimentiDettagliDDT")) Then
                        Throw New Exception(Resources.AgronicaAgenda_2010.CÈUnaBollaAllegataAlMovimentoMaNonRisultan)
                    End If
                    Dim MovimentiDettagliDDT As List(Of Movimento_Dettaglio) = Session("MovimentiDettagliDDT")
                    If MovimentiDettagliDDT.Count = 0 Then
                        Throw New Exception(Resources.AgronicaAgenda_2010.CÈUnaBollaAllegataAlMovimentoMaNonRisultan1)
                    End If
                    AllegataUnaBollaAlmeno = True


                    'aggiungo il movimento dettaglio della bolla alla lista dei mov dettagli riferiti del movimento dettaglio
                    Dim mov_det_bolle = New AgronicaCoreModello.OperazioneAgenda_Temp.Agenda_Movimenti_Dettagli_Helper().Leggi(Agenda.Piva, 0, Bolla.Split("|")(0), Bolla.Split("|")(1), Bolla.Split("|")(2), objParametri_Server)
                    If mov_det_bolle.Count <> 1 Then
                        Throw New Exception(Resources.AgronicaAgenda_2010.DeveEsserciUnMovimentoDettaglioDellaBollaD & mov_det_bolle.Count & "")
                    End If

                    mov_det_bolle(0).Qta = 0
                    'la qta la assegno dopo quando scorro le righe degli impianti

                    'attenzione, anche se il mov dettaglio ha sa_cod valòorizzato,
                    'il sa_cod dell'agenda e del movimento per le bolle sono 0
                    'quindi lo modifico e metto 0,
                    'sia perchè poi non trova il caumov e il lòavcod leggendo dallagenda e dal movimento,
                    'sia perchè il riferimento ha il sa_cod_rif 0, anche se in realtà sono riferiti i dettagli
                    'e quindi dovrebbe avere tutti i campi del dettaglio bolla, ma il sa_cod è 0 come il
                    'movimento bollla e agenda bolla.
                    'controllo se prodotto gia inserito causa stesso prodotto diverse bolle, vedi sopra

                    '-------------------
                    '-----------------------
                    'Attenzione, se ho lo stesso prodotto Mat_Cod, Udm_Cod e Lotto in diverse bolle,
                    'allora devo avere un unico movimento dettaglio per il prodotto,
                    'quindi se ho un prodotto collegato usato in 3 dettagli bolle devo avere un 3 righe nelle tabelle,
                    'un solo movimento dettaglio per il movimento e per lo scarico e 3 riferimenti
                    'Quindi se ho già inserito il prodotto matcod, udm, lotto alla lista  Movimento_OperazioneColturale.Movimenti_Dettagli
                    'non devo riaggiungerlo, ma al contrario devo comunque aggiungere il movimento del dettaglio della bolla alla 
                    'lista  dei mov dettagli riferiti del movimento dettaglio
                    For Each movdetsalvato As Movimento_Dettaglio In Movimento_OperazioneColturale.Movimenti_Dettagli
                        If movdetsalvato.Mat_Cod = GridViewMagazzino.DataKeys(i).Item("Mat_Cod") AndAlso
                                     movdetsalvato.Udm_Cod = GridViewMagazzino.DataKeys(i).Item("Udm_Cod") AndAlso
                                     movdetsalvato.Lotto = GridViewMagazzino.DataKeys(i).Item("Lotto") Then
                            prodottoIGiaInserito = True
                            'lo aggiungo alla lista dei riferiti del prodotto gia inserito e non inserisco il nuovo
                            mov_det_bolle(0).Sa_Cod = 0
                            movdetsalvato.Movimenti_Dettagli_Riferiti.Add(mov_det_bolle(0))
                        End If
                    Next

                    If Not prodottoIGiaInserito Then
                        bolleAllegate += 1
                        mov_det_bolle(0).Sa_Cod = 0
                        Movimento_Dettaglio.Movimenti_Dettagli_Riferiti.Add(mov_det_bolle(0))
                    End If

                    '--------------------------------------------------------------------------------
                    '---------------------------------------------------------------------------------
                End If

                If Not prodottoIGiaInserito Then

                    Movimento_OperazioneColturale.Movimenti_Dettagli.Add(Movimento_Dettaglio)

                End If


            End If
        Next


        'ora passo tutte le righe della griglia, se è checcata e visibile controllo anche le successive fino alla prossima con check visibile,
        'per ciascuna riga ricavo il mov dettaglio del prodotto corrispondente e aggiungo la deetsinazione


        Dim sum1 As Decimal = 0
        Dim righeConDati As Integer = 0 'conteggio le righe con i dati, se non ci sono allora non salvo questa operazione agenda
        Dim AppNome As String

        Dim RegolamentoApp As Integer

        For i = 0 To GridViewImpianti.Rows.Count - 1

            Dim QtaIndicata As Boolean = False

            'Se la riga e' selezionata ...
            If CType(GridViewImpianti.Rows(i).Cells(NumColonna_Check).Controls(1), CheckBox).Checked = True AndAlso GridViewImpianti.Rows(i).Cells(NumColonna_Check).Controls(1).Visible Then

                'se la riga appartiene a questo centro
                If Agenda.Piva = GridViewImpianti.DataKeys(i).Item("Piva") AndAlso Agenda.Sa_Cod = GridViewImpianti.DataKeys(i).Item("Sa_Cod") Then

                    RegolamentoApp = Get_MetodoProduttivo(i)

                    'aggiungo qui la varietà dinamicamente 
                    'Agenda.Des_Lib = Lav_Des & " (" & Veg_Des & "  [" + StrVarieta + "])"
                    'Agenda.Des_Lib = Lav_Des & " (" & Veg_Des & "  ["
                    Dim var As String = Get_Varieta_Des(i)
                    If righeConDati = 0 Then
                        Agenda.Des_Lib = Agenda.Des_Lib & "" & var & " "
                    Else
                        Agenda.Des_Lib = Agenda.Des_Lib & ", " & var & ""
                    End If

                    AppNome = Get_App_Nome(i)

                    For j = i To GridViewImpianti.Rows.Count - 1

                        If VerificaQtaZero = True AndAlso
                                   IsNumeric(Get_Qta(j)) AndAlso
                                   CDbl(Get_Qta(j)) > 0 Then
                            QtaIndicata = True
                        End If

                        sum1 += Get_Qta(j)
                        '---------------------------------
                        'all'inizio del ciclo sono nella riga con il check visibile, nei cicli successivi
                        'sono nelle righe dello stesso impianto, con check nascosti,
                        'se trovo un altro check visibile allora esco dal ciclo
                        For Each movdet As Movimento_Dettaglio In Movimento_OperazioneColturale.Movimenti_Dettagli
                            'ricavo il mov dettaglio del lotto del magazzino corrispondente
                            If objParametriAgenda.Fabbricato <> "0" Then
                                'caso con magazzino
                                'non è gestito multimagazzino
                                If movdet.Mat_Cod = GridViewImpianti.DataKeys(j).Item("Mat_Cod_Lotto") AndAlso
                                               movdet.Udm_Cod = GridViewImpianti.DataKeys(j).Item("Udm_Cod_Lotto") AndAlso
                                               movdet.Lotto = GridViewImpianti.DataKeys(j).Item("Lotto_Lotto") Then

                                    If MateriaPrimaBio_SI_NO.Value = "0" Then

                                        '(02/07/2018) verifica materia prima bio se impianto bio
                                        Select Case RegolamentoApp

                                            Case enum_MetodoProduzione.Biologico, enum_MetodoProduzione.InConversione

                                                If GridViewImpianti.DataKeys(j).Item("Regolamento_Lotto") <> enum_Cod_Regolamento.Regolamento_bio Then

                                                    'Throw New Exception("ATTENZIONE! Poichè l'appezzamento " & AppNome & " è biologico è possibile utilizzare solo materie prime Biologiche!")
                                                    messaggio_errore = "ATTENZIONE! Poichè l'appezzamento " & AppNome & " è biologico sarebbe necessario utilizzare solo materie prime Biologiche!"

                                                    Messaggi.AgroSiNo(messaggio_errore & vbCr & Resources.AgronicaAgenda_2010.BrBIProcedereUgualmenteIB, "MateriaPrimaBio", Page, , updatepanelGridViewMagazzinoImpianti)
                                                    messaggioSiNo = True

                                                    Return False

                                                End If

                                        End Select

                                    Else
                                        'se ho già cliccato  ok vado avanti

                                    End If

                                    '---------------------------------------------------------------------------------------------------
                                    '--------GESTIONE DELLA BOLLA COLLEGATA--------------------------------------------------------------
                                    '-------------------------------------------------------------------------------------------
                                    'Attenzione, se ho lo stesso prodotto Mat_Cod, Udm_Cod e Lotto in diverse bolle,
                                    'allora devo avere un unico movimento dettaglio per il prodotto,
                                    'che deve però sommare le quantità di tutte le righe del prodotto nella tabella
                                    'e devo soprattutto avere un riferimento per ciascun dettaglio della bolla collegata,
                                    'quindi se ho un prodotto collegato usato in 3 dettagli bolle devo avere un 3 righe nelle tabelle,
                                    'un solo movimento dettaglio per il movimento e per lo scarico e 3 riferimenti
                                    Dim impiantoGiaInserito As Boolean = False
                                    If AllegataUnaBollaAlmeno Then
                                        'se ho allegato una bolla controllo
                                        'se è presente già la destinazione corrente , quindi se ho già allegato l'impianto.
                                        'Questo vuol dire che ho già passato una riga di questo impianto con questo prodotto
                                        'matcod, udm, lotto e che quindi mi trovo su una riga corrispondente ad una bolla differente
                                        '(ad un movimento dettaglio in teoria, oltre che bolla).
                                        'Quindi non aggiungo la destinazione già presente ma  la quantità di questa riga al prodotto 
                                        'L'informazione sarà mantenuta dalla tabella dei riferimenti.
                                        For Each impiant As Movimento_Destinazione In movdet.Movimenti_Destinazioni
                                            If impiant.Piva = GridViewImpianti.DataKeys(j).Item("Piva") AndAlso
                                                    impiant.Sa_Cod = GridViewImpianti.DataKeys(j).Item("Sa_Cod") AndAlso
                                                        impiant.Appezza = GridViewImpianti.DataKeys(j).Item("Appezza") AndAlso
                                                        impiant.Id_Destinazione = GridViewImpianti.DataKeys(j).Item("Id_Reg") Then
                                                impiant.Qta += Get_Qta(j)
                                                movdet.Qta += Get_Qta(j)
                                                impiantoGiaInserito = True
                                            End If
                                        Next


                                        'devo inserire il valore della qta parziale del prodotto nel 
                                        'riferimento al movimento dettaglio della bolla, infatti è l'unico posto 
                                        'dove viene inserita l'informazione sul parziale del prodotto
                                        Dim movriftraovato As Boolean = False
                                        Dim Bolla As String = GridViewImpianti.DataKeys(j).Item("Bolla_Lotto")
                                        For Each movRif As Movimento_Dettaglio In movdet.Movimenti_Dettagli_Riferiti
                                            If movRif.Id_Agenda = Bolla.Split("|")(0) AndAlso
                                                        movRif.Id_Mov = Bolla.Split("|")(1) AndAlso
                                                        movRif.Id_Mov_Det = Bolla.Split("|")(2) Then
                                                movRif.Qta += Get_Qta(j)
                                                movriftraovato = True
                                            End If
                                        Next
                                        If impiantoGiaInserito And Not (movriftraovato) Then
                                            'eccez
                                            'se ho inserito l'impianto corrente allora sono nel caso in cui ho lo stesso prodotto 
                                            'usato ma agganciato a due dettagli bolle, quindi per forza mi devo 
                                            'trovare nei suoi movimenti dettagli riferiti un movimento riferito alla bolla corrente
                                            'che devo aggiornare impostando la qta
                                        End If


                                    End If
                                    '---------------------------------------------------------------------------------------------------
                                    '--------FINE GESTIONE DELLA BOLLA COLLEGATA--------------------------------------------------------------
                                    '-------------------------------------------------------------------------------------------

                                    'se ho l'impostazione che vuole la qta obbligatoria salvo solo la destinazione con qta valorizzata
                                    Dim InserisciImpianto As Boolean = True
                                    If VerificaQtaZero = True Then
                                        If Not (IsNumeric(Get_Qta(j))) OrElse CDbl(Get_Qta(j)) <= 0 Then
                                            InserisciImpianto = False
                                        End If
                                    End If


                                    If Not impiantoGiaInserito And InserisciImpianto = True Then

                                        'creo il mov destinazione
                                        Dim Movimento_Destinazione As New Movimento_Destinazione
                                        Movimento_Destinazione.Id_Agenda = objParametriAgenda.Id_Agenda
                                        Movimento_Destinazione.Piva = GridViewImpianti.DataKeys(j).Item("Piva")
                                        Movimento_Destinazione.Sa_Cod = GridViewImpianti.DataKeys(j).Item("Sa_Cod")
                                        Movimento_Destinazione.Appezza = GridViewImpianti.DataKeys(j).Item("Appezza")
                                        Movimento_Destinazione.Id_Destinazione = GridViewImpianti.DataKeys(j).Item("Id_Reg")
                                        Movimento_Destinazione.Qta = Get_Qta(j)
                                        If IsNumeric(Get_Superficie(j)) AndAlso CDec(Get_Superficie(j)) > 0 Then
                                            Movimento_Destinazione.Qta2 = Get_Superficie(j)
                                        Else
                                            Movimento_Destinazione.Qta2 = GridViewImpianti.DataKeys(j).Item("Sup_App")
                                        End If
                                        Movimento_Destinazione.Data = Agenda.Data
                                        movdet.Qta += Movimento_Destinazione.Qta

                                        'imposto sa.cod del mov dettaglio
                                        movdet.Sa_Cod = GridViewImpianti.DataKeys(j).Item("Sa_Cod")

                                        'aggiungo il mov destinazione al mov dettaglio del lotto
                                        movdet.Movimenti_Destinazioni.Add(Movimento_Destinazione)

                                    End If


                                    righeConDati += 1

                                    'Calcolo la quota distribuzione: su quantità distribuita se presente oppure su area.
                                    Dim xSomma As Decimal
                                    If movdet.Qta <> 0 Then
                                        xSomma = movdet.Qta
                                    Else
                                        xSomma = (From xDest In movdet.Movimenti_Destinazioni Select xDest.Qta2).Sum()
                                    End If

                                    If xSomma <> 0 Then

                                        For Each xQuotaDistr In movdet.Movimenti_Destinazioni
                                            If xQuotaDistr.Qta <> 0 Then
                                                xQuotaDistr.QuotaDistribuzione = xQuotaDistr.Qta / xSomma
                                            Else
                                                xQuotaDistr.QuotaDistribuzione = xQuotaDistr.Qta2 / xSomma
                                            End If

                                        Next
                                    End If


                                End If
                            Else
                                'Non eliminare il caso senza magazzino ma con la griglia magazzino visibile, è funzionante, 
                                'se si vuole reimpostare l'utilizzo griglia magazzino nenza magazzino per esempio nel caso 
                                'di più tipi di lotti di default basta scommentare queste parti
                                Throw New NotImplementedException
                                '' ''caso senza magazzino piva="", sa_cod=0, mat_cod=0
                                ' ''If movdet.Mat_Cod = GridViewImpianti.DataKeys(j).Item("Mat_Cod_Lotto") AndAlso
                                ' ''       movdet.Pro_Cod = GridViewImpianti.DataKeys(j).Item("Sem_Cod_Lotto") Then
                                ' ''    'creo il mov destinazione
                                ' ''    Dim Movimento_Destinazione As New Movimento_Destinazione
                                ' ''    Movimento_Destinazione.Piva = GridViewImpianti.DataKeys(j).Item("Piva")
                                ' ''    Movimento_Destinazione.Sa_Cod = GridViewImpianti.DataKeys(j).Item("Sa_Cod")
                                ' ''    Movimento_Destinazione.Appezza = GridViewImpianti.DataKeys(j).Item("Appezza")
                                ' ''    Movimento_Destinazione.Id_Destinazione = GridViewImpianti.DataKeys(j).Item("Id_Reg")
                                ' ''    Movimento_Destinazione.Qta = Get_Qta(j)
                                ' ''    Movimento_Destinazione.Data = Agenda.Data

                                ' ''    movdet.Qta += Movimento_Destinazione.Qta


                                ' ''    'imposto l'udm in base alla combo (con magazzino non è necessaria in quanto prende quella del magazzino)
                                ' ''    movdet.Udm_Cod = Get_Udm(j)

                                ' ''    'aggiungo il mov destinazione al mov dettaglio del lotto
                                ' ''    movdet.Movimenti_Destinazioni.Add(Movimento_Destinazione)

                                ' ''    righeConDati += 1

                                ' ''End If
                            End If



                        Next
                        '---------------------------------
                        i = j 'così all'uscita risparmio dei cicli
                        If j + 1 = GridViewImpianti.Rows.Count Then
                            'la prossima riga è l'ultima, controlo qui per non fare eccezione nell'if successivo
                            Exit For
                        End If
                        If GridViewImpianti.Rows(j + 1).Cells(NumColonna_Check).Controls(1).Visible = True Then

                            'ultima riga impianto verifico di aver indicato la qta per almeno una materia prima
                            If VerificaQtaZero = True AndAlso QtaIndicata = False Then
                                Throw New Exception("ATTENZIONE! Per l'appezzamento " & AppNome & " non è stata indicata la quantità di nessun articolo!")
                            End If

                            'se la prossima riga si riferisce ad un nuovo impianto esco dal for
                            Exit For
                        Else
                            'la prossima riga o è l'ultima o si riferisce allo stesso impianto,
                            'continuo e all'inizio del ciclo inserisco la nuova riga
                        End If
                    Next

                End If

            End If


        Next

        Agenda.Des_Lib = Agenda.Des_Lib & "])"

        If righeConDati = 0 Then
            Throw New NotImplementedException(Resources.AgronicaAgenda_2010.NonSonoStateTrovateRigheConDatiNegliImpian)
        End If


        '---------------------------------------------------------------------------------------------------
        '--------GESTIONE DELLA BOLLA COLLEGATA-------------------------------------------------------------
        '---------------------------------------------------------------------------------------------------
        'controllo se i mov dettagli e gli elementi della lista per bolle allegate sono lo stosso numero,
        'cioè se sono state allegati tutti
        If AllegataUnaBollaAlmeno Then
            If Movimento_OperazioneColturale.Movimenti_Dettagli.Count <> bolleAllegate Then
                Throw New NotImplementedException(Resources.AgronicaAgenda_2010.IlNumeroDiDettagliAllegatiNeiRiferimentiNo)
            End If
        End If
        '---------------------------------------------------------------------------------------------------
        '--------FINE GESTIONE DELLA BOLLA COLLEGATA--------------------------------------------------------
        '---------------------------------------------------------------------------------------------------
        'verifico corrispondenze qta
        'sum1 calcolata sopra, prende i valori inseriti nella griglia
        Dim sum2Tot As Decimal = 0
        For Each movd As Movimento_Dettaglio In Movimento_OperazioneColturale.Movimenti_Dettagli
            Dim sum2 As Decimal = 0
            sum2 = movd.Qta
            sum2Tot += sum2
            Dim sum3 As Decimal = 0
            For Each md As Movimento_Destinazione In movd.Movimenti_Destinazioni
                sum3 += md.Qta
            Next
            If sum2 <> sum3 Then
                Throw New Exception(String.Format(Resources.AgronicaAgenda_2010.IncongruenzaDeiDatiContattareLAssistenzaX0, sum1, sum2, sum3))
            End If
            If Not IsNothing(movd.Movimenti_Dettagli_Riferiti) AndAlso movd.Movimenti_Dettagli_Riferiti.Count > 0 Then
                Dim sum4 As Decimal = 0
                For Each mr As Movimento_Dettaglio In movd.Movimenti_Dettagli_Riferiti
                    sum4 += mr.Qta
                Next
                If sum2 <> sum4 Then
                    Throw New Exception(String.Format(Resources.AgronicaAgenda_2010.IncongruenzaDeiDatiContattareLAssistenzaX4, sum1, sum2, sum3, sum4))
                End If
            End If
        Next
        If sum1 <> sum2Tot Then
            Throw New Exception(String.Format(Resources.AgronicaAgenda_2010.IncongruenzaDeiDatiContattareLAssistenzaX2, sum1, sum2Tot))
        End If

        Return True

    End Function

    'l'operazione di sovescio non modifica l'impianto
    'la griglia impianti utilizzata è quella delle altre operazioni
    Private Sub Crea_Agenda_Movimento_Sovescio_ConMagazzino(ByRef Agenda As Operazione_Agenda, ByVal Movimento_OperazioneColturale As Movimento)

        Dim AllegataUnaBollaAlmeno As Boolean = False
        Dim bolleAllegate As Integer = 0
        '----------------------------------------------------------
        '----- MOVIMENTI DETTAGLI , DET.TECNICI, DESTINAZIONI -----
        '----------------------------------------------------------
        'per ciascuna giacenza, quindi per ciascuna riga selezionata della grid magazzino creo un movimento dettaglio
        'Per tutti gli impianti in cui viene utilizzato il prodotto aggiungo un mov destinazione

        Dim VerificaQtaZero As Boolean = False
        Dim objUtenti As New AgronicaCoreUtentiDAL.Utenti_Impostazioni_Read
        Dim BloccoSemina As String = objUtenti.ImpostazioneValore1_from_ImpostazioneCod(enum_Impostazioni_Utenti.UTENTE_COD_BLOCCA_SEMINA_SE_SENZA_QTA,
                                                HttpContext.Current.Session("ASG_objParametri_Utenti"),
                                                1)
        If BloccoSemina <> "0" Then
            VerificaQtaZero = True
        End If

        Dim AlmenoUno As Boolean = False
        Dim Qta As Decimal = 0

        Dim ListaImpianti As New List(Of AgronicaCoreModello.ParametriAgenda_Temp.Impianto)
        ListaImpianti = CType(Master, Operazione).GetImpianti()

        If ListaImpianti.Count = 0 Then
            Throw New Exception(Resources.AgronicaAgenda_2010.SelezionareAlmenoUnImpiantoColturale)
        End If

        Dim SupTot As Decimal
        For imp = 0 To ListaImpianti.Count - 1
            SupTot += ListaImpianti(imp).Qta2
        Next

        Dim AppNome As String

        For i = 0 To GridViewMagazzino.Rows.Count - 1

            'Se la riga e' selezionata ...
            If CType(GridViewMagazzino.Rows(i).Cells(NumColonna_Check).Controls(1), CheckBox).Checked = True Then

                AlmenoUno = True

                Qta = 0

                If IsNumeric(CType(GridViewMagazzino.Rows(i).Cells(GridViewMagazzino.Columns.Count - 2).Controls(1), TextBox).Text) Then
                    Qta = CDec(CType(GridViewMagazzino.Rows(i).Cells(GridViewMagazzino.Columns.Count - 2).Controls(1), TextBox).Text)
                End If

                If VerificaQtaZero = True Then

                    If Qta = 0 Then
                        Throw New Exception("ATTENZIONE! Per " & GridViewMagazzino.DataKeys(i).Item("Descrizione") & " non è stata indicata la quantità in nessun impianto!")
                    End If

                End If


                '------------------------------
                '----- MOVIMENTO DETTAGLIO-----
                '------------------------------
                Dim Movimento_Dettaglio As New Movimento_Dettaglio
                Movimento_Dettaglio.Id_Agenda = objParametriAgenda.Id_Agenda
                Movimento_Dettaglio.Piva = Agenda.Piva 'se non uso il magazzino la piva è "" e il sacod è 0
                Movimento_Dettaglio.Sa_Cod = Agenda.Sa_Cod
                Movimento_Dettaglio.Elem_Cod = SEMENTI 'fisso
                Movimento_Dettaglio.Pro_Cod = 0 '0 se magazzino, sem_cod altrimenti
                Movimento_Dettaglio.Mat_Cod = GridViewMagazzino.DataKeys(i).Item("Mat_Cod") '0 se no magazzino, Mat_Cod altrimenti
                Movimento_Dettaglio.Udm_Cod = GridViewMagazzino.DataKeys(i).Item("Udm_Cod")
                Movimento_Dettaglio.Lotto = GridViewMagazzino.DataKeys(i).Item("Lotto")
                Movimento_Dettaglio.Mov_Det_Des = "Dettagli Tecnici Materia Prima per Semina/Trapianto"
                Movimento_Dettaglio.Qta = Qta
                Movimento_Dettaglio.Contabilizzato = Movimento_Dettaglio_Contabilizzato 'verificare
                Movimento_Dettaglio.Pendente = Movimento_Dettaglio_Pendente 'verificare fisso forse
                Movimento_Dettaglio.Data = Agenda.Data
                Movimento_Dettaglio.BaseCode = Agenda.BaseCode
                Movimento_Dettaglio.TopCode = Agenda.TopCode
                Movimento_Dettaglio.Cau_Mov = objParametriAgenda.Cau_Mov
                Movimento_Dettaglio.Anno = 1900
                'impostazioni in base alla lavorazione
                Select Case CInt(objParametriAgenda.Lav_Cod)
                    Case LAVCOD_SOVESCIO
                    Case Else
                        Throw New NotImplementedException
                End Select

                'non usato finore, ma utile per la pagina
                Movimento_Dettaglio.Extra_Str = GridViewMagazzino.DataKeys(i).Item("Descrizione")

                '---------------------------------------------------------------------------------------------------
                '--------GESTIONE DELLA BOLLA COLLEGATA--------------------------------------------------------------
                '-------------------------------------------------------------------------------------------
                Dim prodottoIGiaInserito As Boolean = False
                Dim Bolla As String = GridViewMagazzino.DataKeys(i).Item("Bolla")
                If Bolla <> "" Then
                    If Bolla.Split("|").Count <> 3 Then
                        Throw New Exception(Resources.AgronicaAgenda_2010.CÈUnaBollaAllegataMaIlFormatoNonÈRiconosci)
                    End If
                    If GridViewMagazzino.Columns(6).Visible = False Or
                        GridViewMagazzino.Columns(7).Visible = False Or
                        GridViewMagazzino.Columns(8).Visible = False Or
                        GridViewMagazzino.Columns(9).Visible = False Then
                        Throw New Exception(Resources.AgronicaAgenda_2010.CÈUnaBollaAllegataMaLeColonneDelleBolleNon)
                    End If
                    If IsNothing(Session("MovimentiDettagliDDT")) Then
                        Throw New Exception(Resources.AgronicaAgenda_2010.CÈUnaBollaAllegataAlMovimentoMaNonRisultan)
                    End If
                    Dim MovimentiDettagliDDT As List(Of Movimento_Dettaglio) = Session("MovimentiDettagliDDT")
                    If MovimentiDettagliDDT.Count = 0 Then
                        Throw New Exception(Resources.AgronicaAgenda_2010.CÈUnaBollaAllegataAlMovimentoMaNonRisultan1)
                    End If
                    AllegataUnaBollaAlmeno = True


                    'aggiungo il movimento dettaglio della bolla alla lista dei mov dettagli riferiti del movimento dettaglio
                    Dim mov_det_bolle = New AgronicaCoreModello.OperazioneAgenda_Temp.Agenda_Movimenti_Dettagli_Helper().Leggi(Agenda.Piva, 0, Bolla.Split("|")(0), Bolla.Split("|")(1), Bolla.Split("|")(2), objParametri_Server)
                    If mov_det_bolle.Count <> 1 Then
                        Throw New Exception(Resources.AgronicaAgenda_2010.DeveEsserciUnMovimentoDettaglioDellaBollaD & mov_det_bolle.Count & "")
                    End If

                    mov_det_bolle(0).Qta = Qta


                    'attenzione, anche se il mov dettaglio ha sa_cod valòorizzato,
                    'il sa_cod dell'agenda e del movimento per le bolle sono 0
                    'quindi lo modifico e metto 0,
                    'sia perchè poi non trova il caumov e il lòavcod leggendo dallagenda e dal movimento,
                    'sia perchè il riferimento ha il sa_cod_rif 0, anche se in realtà sono riferiti i dettagli
                    'e quindi dovrebbe avere tutti i campi del dettaglio bolla, ma il sa_cod è 0 come il
                    'movimento bollla e agenda bolla.
                    'controllo se prodotto gia inserito causa stesso prodotto diverse bolle, vedi sopra

                    '-------------------
                    '-----------------------
                    'Attenzione, se ho lo stesso prodotto Mat_Cod, Udm_Cod e Lotto in diverse bolle,
                    'allora devo avere un unico movimento dettaglio per il prodotto,
                    'quindi se ho un prodotto collegato usato in 3 dettagli bolle devo avere un 3 righe nelle tabelle,
                    'un solo movimento dettaglio per il movimento e per lo scarico e 3 riferimenti
                    'Quindi se ho già inserito il prodotto matcod, udm, lotto alla lista  Movimento_OperazioneColturale.Movimenti_Dettagli
                    'non devo riaggiungerlo, ma al contrario devo comunque aggiungere il movimento del dettaglio della bolla alla 
                    'lista  dei mov dettagli riferiti del movimento dettaglio
                    For Each movdetsalvato As Movimento_Dettaglio In Movimento_OperazioneColturale.Movimenti_Dettagli
                        If movdetsalvato.Mat_Cod = GridViewMagazzino.DataKeys(i).Item("Mat_Cod") AndAlso
                                     movdetsalvato.Udm_Cod = GridViewMagazzino.DataKeys(i).Item("Udm_Cod") AndAlso
                                     movdetsalvato.Lotto = GridViewMagazzino.DataKeys(i).Item("Lotto") Then
                            prodottoIGiaInserito = True
                            'lo aggiungo alla lista dei riferiti del prodotto gia inserito e non inserisco il nuovo
                            mov_det_bolle(0).Sa_Cod = 0
                            movdetsalvato.Movimenti_Dettagli_Riferiti.Add(mov_det_bolle(0))
                        End If
                    Next

                    If Not prodottoIGiaInserito Then
                        bolleAllegate += 1
                        mov_det_bolle(0).Sa_Cod = 0
                        Movimento_Dettaglio.Movimenti_Dettagli_Riferiti.Add(mov_det_bolle(0))
                    End If

                    '--------------------------------------------------------------------------------
                    '---------------------------------------------------------------------------------
                End If

                If Not prodottoIGiaInserito Then

                    Movimento_OperazioneColturale.Movimenti_Dettagli.Add(Movimento_Dettaglio)


                    For imp = 0 To ListaImpianti.Count - 1

                        If Agenda.Piva = ListaImpianti(imp).Piva AndAlso Agenda.Sa_Cod = ListaImpianti(imp).Sa_Cod Then

                            Dim var As String = ListaImpianti(imp).Cul_Des
                            If i = 0 Then
                                Agenda.Des_Lib = Agenda.Des_Lib & "" & var & " "
                            Else
                                Agenda.Des_Lib = Agenda.Des_Lib & ", " & var & ""
                            End If

                            AppNome = ListaImpianti(imp).App_Nome


                            'creo il mov destinazione
                            Dim Movimento_Destinazione As New Movimento_Destinazione
                            Movimento_Destinazione.Id_Agenda = objParametriAgenda.Id_Agenda
                            Movimento_Destinazione.Piva = ListaImpianti(imp).Piva
                            Movimento_Destinazione.Sa_Cod = ListaImpianti(imp).Sa_Cod
                            Movimento_Destinazione.Appezza = ListaImpianti(imp).Appezza
                            Movimento_Destinazione.Id_Destinazione = ListaImpianti(imp).ID_Reg

                            Movimento_Destinazione.Qta = 0
                            If Qta <> 0 Then
                                Movimento_Destinazione.Qta = Qta * ListaImpianti(imp).Qta2 / SupTot
                            End If
                            Movimento_Destinazione.Qta2 = ListaImpianti(imp).Qta2
                            Movimento_Destinazione.Data = Agenda.Data

                            Movimento_Dettaglio.Movimenti_Destinazioni.Add(Movimento_Destinazione)


                        End If



                    Next



                End If


            End If
        Next

        If AlmenoUno = False Then
            Throw New Exception("ATTENZIONE! Selezionare almeno un prodotto!")
        End If


        '---------------------------------------------------------------------------------------------------
        '--------GESTIONE DELLA BOLLA COLLEGATA-------------------------------------------------------------
        '---------------------------------------------------------------------------------------------------
        'controllo se i mov dettagli e gli elementi della lista per bolle allegate sono lo stosso numero,
        'cioè se sono state allegati tutti
        If AllegataUnaBollaAlmeno Then
            If Movimento_OperazioneColturale.Movimenti_Dettagli.Count <> bolleAllegate Then
                Throw New NotImplementedException(Resources.AgronicaAgenda_2010.IlNumeroDiDettagliAllegatiNeiRiferimentiNo)
            End If
        End If
        '---------------------------------------------------------------------------------------------------
        '--------FINE GESTIONE DELLA BOLLA COLLEGATA--------------------------------------------------------
        '---------------------------------------------------------------------------------------------------


    End Sub


    Private Function Crea_Agenda_Movimento_Scarico(ByRef Agenda As Operazione_Agenda, ByRef Messaggio_Errore As String) As Boolean


        If objParametriAgenda.Fabbricato <> "0" Then

            Dim sa_cod_magazzino As String = ""
            Dim fabbricatox_Cod As String = ""

            Dim magazzinoEsterno As Boolean = True
            If objParametriAgenda.Fabbricato <> "0" AndAlso Split(objParametriAgenda.Fabbricato, "|").Count = 3 AndAlso Split(objParametriAgenda.Fabbricato, "|")(2) <> objParametriAgenda.Piva Then
                'se magazzino è della azienda padre
                magazzinoEsterno = True
                Dim sa_cod_magazzino_predefinito_azienda As Integer
                Dim fabbricatox_Cod_magazzino_predefinito_azienda As Integer
                Dim fabbricati As New AgronicaCoreAnagrafeDAL.Fabbricati_R()
                fabbricati.Ricava_PrimoMagazzino_Impresa(Agenda.Piva, sa_cod_magazzino_predefinito_azienda, fabbricatox_Cod_magazzino_predefinito_azienda, objParametri_Server)
                sa_cod_magazzino = sa_cod_magazzino_predefinito_azienda
                fabbricatox_Cod = fabbricatox_Cod_magazzino_predefinito_azienda

            Else
                'se magazzino è quello dell'azienda
                magazzinoEsterno = False
                sa_cod_magazzino = Split(objParametriAgenda.Fabbricato, "|")(1)
                fabbricatox_Cod = Split(objParametriAgenda.Fabbricato, "|")(0)

            End If

            Dim Movimento_Scarico As New Movimento
            Movimento_Scarico.Id_Agenda = objParametriAgenda.Id_Agenda
            Movimento_Scarico.Piva = Agenda.Piva
            Movimento_Scarico.Sa_Cod = sa_cod_magazzino 'Agenda.Sa_Cod è sbagliato se ho magazzino in altro centro se multicentro è sbagliat ousare Split(objParametriAgenda.fabbricatox, "|")(1)
            Movimento_Scarico.Data = Agenda.Data
            Movimento_Scarico.Lav_Cod = objParametriAgenda.Lav_Cod
            Movimento_Scarico.Cau_Mov = CAU_SCARICO
            Movimento_Scarico.Mov_Desc = "Scarico Magazzino"
            Movimento_Scarico.BaseCode = Agenda.BaseCode
            Movimento_Scarico.TopCode = Agenda.TopCode


            '------------------------------------------------
            '----- MOVIMENTI DETTAGLI SCARICO 
            '------------------------------------------------

            'copio quelli del movimento operazione
            'If Agenda.Movimenti.Count > 1 Then
            '    Throw New NotImplementedException
            'End If

            For Each moviment As Movimento In Agenda.Movimenti

                If moviment.Cau_Mov = CAU_LAVORAZIONE Then


                    For Each movdet As Movimento_Dettaglio In moviment.Movimenti_Dettagli

                        Dim Movimento_Dettaglio_Scarico As New Movimento_Dettaglio

                        Movimento_Dettaglio_Scarico.Id_Agenda = objParametriAgenda.Id_Agenda
                        Movimento_Dettaglio_Scarico.Piva = movdet.Piva
                        Movimento_Dettaglio_Scarico.Sa_Cod = sa_cod_magazzino
                        Movimento_Dettaglio_Scarico.Elem_Cod = movdet.Elem_Cod
                        Movimento_Dettaglio_Scarico.Pro_Cod = movdet.Pro_Cod
                        Movimento_Dettaglio_Scarico.Mat_Cod = movdet.Mat_Cod
                        Movimento_Dettaglio_Scarico.Mov_Det_Des = "Utilizzo Di Materia Prima per Semina/Trapianto"
                        Movimento_Dettaglio_Scarico.Udm_Cod = movdet.Udm_Cod
                        Movimento_Dettaglio_Scarico.Qta = movdet.Qta
                        Movimento_Dettaglio_Scarico.Contabilizzato = movdet.Contabilizzato
                        Movimento_Dettaglio_Scarico.Pendente = movdet.Pendente
                        Movimento_Dettaglio_Scarico.Data = movdet.Data
                        Movimento_Dettaglio_Scarico.BaseCode = movdet.BaseCode
                        Movimento_Dettaglio_Scarico.TopCode = movdet.TopCode
                        Movimento_Dettaglio_Scarico.Cau_Mov = movdet.Cau_Mov
                        Movimento_Dettaglio_Scarico.Extra_Str = movdet.Extra_Str
                        Movimento_Dettaglio_Scarico.Anno = movdet.Anno
                        Movimento_Dettaglio_Scarico.Lotto = movdet.Lotto

                        '------------------------------------------------
                        '----- MOVIMENTI DESTINAZIONI SCARICO
                        '------------------------------------------------

                        Dim Movimento_Destinazione_Scarico_Trappola As New Movimento_Destinazione
                        Movimento_Destinazione_Scarico_Trappola.Id_Agenda = objParametriAgenda.Id_Agenda
                        Movimento_Destinazione_Scarico_Trappola.Data = Agenda.Data
                        Movimento_Destinazione_Scarico_Trappola.Piva = objParametriAgenda.Piva
                        Movimento_Destinazione_Scarico_Trappola.Sa_Cod = sa_cod_magazzino
                        Movimento_Destinazione_Scarico_Trappola.Id_Destinazione = fabbricatox_Cod
                        Movimento_Destinazione_Scarico_Trappola.Tipo = MAGAZZINO
                        Movimento_Destinazione_Scarico_Trappola.Qta = Movimento_Dettaglio_Scarico.Qta
                        Movimento_Destinazione_Scarico_Trappola.BaseCode = Agenda.BaseCode
                        Movimento_Destinazione_Scarico_Trappola.TopCode = Agenda.TopCode


                        'controllo le Giacenze 
                        'ATTEBNZIONE, dato che potrei avere il magazzino esterno, devo controllare le giacenze
                        'leggendo piva, sacod e fabbricato dalla combo, controllando quindi che ci siano le giacenze nel magazzino esterno se utilizzato,
                        'in quel caso infatti ci sarebbe uno scarico e una bolla automatica per caricare quello dell'impresa

                        'controllo giacenza 
                        'prima avevo LOTTO_NONDEFINITO= -999, ora glielo passo
                        Dim objGiacenze As New AgronicaCoreContabDAL.Movimenti_Dettagli_R
                        Dim Giacenza As Decimal = objGiacenze.Verifica_Giacenze_Con_Magazzino_Esterno(Split(objParametriAgenda.Fabbricato, "|")(2),
                                                                                Split(objParametriAgenda.Fabbricato, "|")(1),
                                                                                Split(objParametriAgenda.Fabbricato, "|")(0),
                                                                                Movimento_Dettaglio_Scarico.Elem_Cod,
                                                                                Movimento_Dettaglio_Scarico.Pro_Cod,
                                                                                Movimento_Dettaglio_Scarico.Mat_Cod,
                                                                                0, 0, Movimento_Dettaglio_Scarico.Lotto,
                                                                                0, Movimento_Dettaglio_Scarico.Udm_Cod,
                                                                                AGRODATAINIZIO,
                                                                                CDate(objParametriAgenda.Data),
                                                                                objParametriAgenda.Piva,
                                                                                objParametriAgenda.Sa_Cod,
                                                                                objParametriAgenda.Id_Agenda,
                                                                                objParametri_Server)
                        If Movimento_Dettaglio_Scarico.Qta > Giacenza Then

                            '--------------BLOCCO SALVATAGGIO SE_SUPERA_GIACENZE-------------------
                            If Not IsNothing(Session("UTENTE_COD_BLOCCA_SE_SUPERA_GIACENZE")) AndAlso Session("UTENTE_COD_BLOCCA_SE_SUPERA_GIACENZE") = True Then

                                Dim MErrore As String
                                MErrore = "In base alle impostazioni utente NON è possibile usare un prodotto con giacenza non sufficiente! "
                                Messaggi.AgroMsgBox(Resources.AgronicaAgenda_2010.AttenzioneLOperazioneNonÈStataRegistrataBr & MErrore, Page, ,
                                    CType(Ricerca.FindControlIterative(Page.Master, "UpdatePanelToolBar"), UpdatePanel))
                                Return False

                            End If
                            '---------------------------------------------------------------------

                            'controllo se ho acconsentito precedenrtemente al salvataggio senza giacenze
                            If Giacenza_SI_NO.Value = "0" Then


                                Messaggio_Errore = Messaggio_Errore & String.Format(Resources.AgronicaAgenda_2010.ATTENZIONEBrLaQuantitàDiX0BBAlBX1BÈPariABX, Movimento_Dettaglio_Scarico.Extra_Str, objParametriAgenda.Data, Giacenza, New AgronicaCoreMetaSchemaDAL.UnitaMisura_R().UdmDes_from_UdmCod(Movimento_Dettaglio_Scarico.Udm_Cod, "", objParametri_Server))


                                'se non ho acconsentito genere agrosino che mi rilancera il salvataggio via jscript
                                Messaggio_Errore = Messaggio_Errore & Resources.AgronicaAgenda_2010.BrBIProcedereSenzaGestireLeGiace1
                                'AgroSiNo
                                'Messaggi.AgroSiNo(messaggio, "Salva", Page, , UpdatePanelGridImpianti)
                                'impedisco di procedere per ora
                                ' VAnni: 28/2/2020: Non usare il resx sulla stringa "Giacenze"
                                Messaggi.AgroSiNo(Messaggio_Errore & Resources.AgronicaAgenda_2010.BrBIAltrimentiInserireUnValoreValidoOTogli, "Giacenze", Page, , updatepanelGridViewMagazzinoImpianti)
                                messaggioSiNo = True
                                Return False

                            Else
                                'se ho già cliccato  ok vado avanti

                            End If
                        End If



                        Movimento_Dettaglio_Scarico.Movimenti_Destinazioni.Add(Movimento_Destinazione_Scarico_Trappola)

                        Movimento_Scarico.Movimenti_Dettagli.Add(Movimento_Dettaglio_Scarico)

                    Next





                End If
            Next






            '-------------- aggiungo il movimento scarico all'agenda
            Agenda.Movimenti.Add(Movimento_Scarico)

            Return True

        Else
            'deve esserci sempre almeno un movimento di scarico  se viene richiamato questo metodo
            'lancio eccezione, vuol dire che qualcosa è stato pensato male
            Throw New NotImplementedException
            Return False
        End If

    End Function





    Private Function CreaOggettoAgendaSingoloApp(ByVal indice As Integer,
                                                 ByVal Sa_Cod As Integer, ByVal Appezza As Integer, ByVal Id_Reg As Integer,
                                                 ByRef messaggio_errore As String) As Operazione_Agenda

        Dim Agenda As Operazione_Agenda

        '--------------AGENDA------------------
        If Not Crea_Agenda_SingoloApp(Get_Specie_Des(indice), Sa_Cod, Agenda, messaggio_errore) Then
            Return Nothing
        End If

        '--------------NOTE------------------
        If Not Crea_Agenda_Note(Agenda) Then
            Return Nothing
        End If

        '---------------MOVIMENTI COSTI ACCESSORI--------
        If Not Crea_Agenda_Movimento_CostiAccessori(Agenda) Then
            Return Nothing
        End If


        '------------- MOVIMENTO RILIEVO / TRATTAMENTO---------
        'inneschiNumTotale usato solo per trappole e massa, ignorato per conf e dis sessuale
        Select Case CInt(objParametriAgenda.Lav_Cod)

            Case LAVCOD_SEMINA, LAVCOD_TRAPIANTO, LAVCOD_SOVESCIO, LAVCOD_SOD_SEDDING
                If Not Crea_Agenda_Movimento_Semina_SingoloApp(indice, Appezza, Id_Reg, Agenda, messaggio_errore) Then
                    Return Nothing
                End If


            Case Else
                Throw New NotImplementedException
        End Select


        '-------------- MOVIMENTO SCARICO---------------------------
        'modifica per multicentro,solo se l'operazione agenda riguarda il magazzino del centro selezionato
        'If Sa_Cod = Mag_Sa_Cod AndAlso objParametriAgenda.Fabbricato <> "0" Then
        If objParametriAgenda.Fabbricato <> "0" Then
            'se è selezionato il magazzino
            'uno scarico totale per le trappole e uno pergli inneschi (solo per massa e trappole)
            'inneschiNumTotale usato solo per trappole e massa, ignorato per conf e dis sessuale
            If Not Crea_Agenda_Movimento_Scarico(Agenda, messaggio_errore) Then
                Return Nothing
            End If
        End If


        Return Agenda

    End Function

    Private Function Crea_Agenda_SingoloApp(ByVal Veg_Des As String, ByRef Sa_Cod As Integer, ByRef Agenda As Operazione_Agenda, ByRef messaggio_errore As String) As Boolean

        '---------------------------------------
        ' recupero la DATA
        Dim Data As Date
        If objParametriAgenda.Data = AGRODATAINIZIO Then
            messaggio_errore = messaggio_errore & Resources.AgronicaAgenda_2010.IndicareUnaData
            Return False
        Else
            Data = objParametriAgenda.Data
        End If

        '---------------------------------------
        ' recupero la OPERAZIONE
        'Dim Lav_Cod As String = ""
        Dim Lav_Des As String = ""
        If objParametriAgenda.Lav_Cod = "" Then
            messaggio_errore = messaggio_errore & Resources.AgronicaAgenda_2010.SelezionareUnOperazione
            Return False
        Else
            ''Lav_Cod = objParametriAgenda.Lav_Cod
            'Lav_Des = Master_Operazione.Property_ComboOperazione.Testo_Combo
            'objParametriAgenda.Lav_Des = Lav_Des
            Dim objOperazioni As New AgronicaCoreAnagrafeDAL.Operazioni_R
            Lav_Des = objOperazioni.Lav_Des_From_Lav_Cod(objParametriAgenda.Lav_Cod, objParametri_Server)
        End If



        Dim BaseCode As Integer
        Dim TopCode As Integer

        '------------------------------------------------
        '----- Calcolo i valori di BaseCode e TopCode
        '------------------------------------------------
        Call Calcola_BaseCode_TopCode(BaseCode,
                              TopCode,
                              Session("ASG_ProgressivoGIAS"))

        Agenda = New Operazione_Agenda

        Agenda.Tipo_Operazione = objParametriAgenda.Tipo_Operazione
        Agenda.Id_Agenda = objParametriAgenda.Id_Agenda
        Agenda.Data = Data
        Agenda.Piva = objParametriAgenda.Piva
        Agenda.Sa_Cod = Sa_Cod
        Agenda.Lav_Cod = objParametriAgenda.Lav_Cod
        Agenda.Des_Lib = Lav_Des & " (" & Veg_Des & "  ["

        Agenda.BaseCode = BaseCode
        Agenda.TopCode = TopCode

        Return True
    End Function

    Private Function Crea_Agenda_Movimento_Semina_SingoloApp(ByVal indice As Integer, ByVal Appezza As Integer, ByVal Id_Reg As Integer,
                                                             ByRef Agenda As Operazione_Agenda, ByRef messaggio_errore As String) As Boolean

        Select Case CInt(objParametriAgenda.Lav_Cod)

            Case LAVCOD_SEMINA, LAVCOD_TRAPIANTO, LAVCOD_SOVESCIO, LAVCOD_SOD_SEDDING
                'continua, questo metodo deve essere chioamato solo in questi casi altrimenti genero accezione
            Case Else
                Throw New Exception
        End Select

        'If ListaImp.Count = 0 Then
        '    messaggio_errore = messaggio_errore & Resources.AgronicaAgenda_2010.InserirePrimaLaTabella
        '    Return False
        'End If

        Dim Movimento_OperazioneColturale As New Movimento

        Movimento_OperazioneColturale.Id_Agenda = objParametriAgenda.Id_Agenda
        Movimento_OperazioneColturale.Piva = Agenda.Piva
        Movimento_OperazioneColturale.Sa_Cod = Agenda.Sa_Cod
        Movimento_OperazioneColturale.Lav_Cod = objParametriAgenda.Lav_Cod
        Movimento_OperazioneColturale.Cau_Mov = objParametriAgenda.Cau_Mov
        Movimento_OperazioneColturale.Mov_Desc = Master_Operazione.GetNota()
        Movimento_OperazioneColturale.Data = Agenda.Data
        Movimento_OperazioneColturale.BaseCode = Agenda.BaseCode
        Movimento_OperazioneColturale.TopCode = Agenda.TopCode

        Movimento_OperazioneColturale.Extra_Int = RBL_Tipo_Semina.SelectedValue 'Tipo_Semina 'salvo il tipo raccolta, puo essere utile


        If objParametriAgenda.Fabbricato <> "0" Then

            If Not Crea_Agenda_Movimento_Semina_ConMagazzino_SingoloApp(indice, Appezza, Id_Reg, Agenda, Movimento_OperazioneColturale, messaggio_errore) Then
                Return Nothing
            End If

        End If

        'Se ho almeno un movimento dettaglio nell'operazione la aggiungo all'agenda
        '(il movimento dettaglio contiene una destinazione e un mov dettaglio tecnico)
        'Se il movimento  non ha almeno un dettaglio allora genero una eccezione,
        'perchè una situazione del genere non dovrebbe mai accadere in quanto deve essere creato almeno un 
        'movimento dettaglio per ciascun centro, e tutti gli impianti selezionati nella master devono 
        'creare un movimentoi dettaglio corrispondente, anche se su centri diversi.

        If Movimento_OperazioneColturale.Movimenti_Dettagli.Count > 0 Then
            Agenda.Movimenti.Add(Movimento_OperazioneColturale)
        Else
            messaggio_errore = messaggio_errore & Resources.AgronicaAgenda_2010.ErroreNonPrevisto
            Return False
        End If

        Return True

    End Function

    Private Function Crea_Agenda_Movimento_Semina_ConMagazzino_SingoloApp(ByVal indice As Integer, ByVal Appezza As Integer, ByVal Id_Reg As Integer,
                                                                     ByRef Agenda As Operazione_Agenda, ByVal Movimento_OperazioneColturale As Movimento,
                                                                     ByRef messaggio_errore As String) As Boolean

        Dim AllegataUnaBollaAlmeno As Boolean = False
        Dim bolleAllegate As Integer = 0
        '----------------------------------------------------------
        '----- MOVIMENTI DETTAGLI , DET.TECNICI, DESTINAZIONI -----
        '----------------------------------------------------------
        'per ciascuna giacenza, quindi per ciascuna riga selezionata della grid magazzino creo un movimento dettaglio
        'Per tutti gli impianti in cui viene utilizzato il prodotto aggiungo un mov destinazione

        Dim VerificaQtaZero As Boolean = False
        Dim objUtenti As New AgronicaCoreUtentiDAL.Utenti_Impostazioni_Read
        Dim BloccoSemina As String = objUtenti.ImpostazioneValore1_from_ImpostazioneCod(enum_Impostazioni_Utenti.UTENTE_COD_BLOCCA_SEMINA_SE_SENZA_QTA,
                                                HttpContext.Current.Session("ASG_objParametri_Utenti"),
                                                1)
        If BloccoSemina <> "0" Then
            VerificaQtaZero = True
        End If




        Dim RegolamentoApp As Integer = Get_MetodoProduttivo(indice)

        '------------------------------
        '----- MOVIMENTO DETTAGLIO-----
        '------------------------------
        Dim Movimento_Dettaglio As New Movimento_Dettaglio
        Movimento_Dettaglio.Id_Agenda = objParametriAgenda.Id_Agenda
        Movimento_Dettaglio.Piva = Agenda.Piva 'se non uso il magazzino la piva è "" e il sacod è 0
        Movimento_Dettaglio.Sa_Cod = Agenda.Sa_Cod
        Movimento_Dettaglio.Elem_Cod = SEMENTI 'fisso
        If objParametriAgenda.Fabbricato <> "0" Then
            Movimento_Dettaglio.Pro_Cod = 0 '0 se magazzino, sem_cod altrimenti
            Movimento_Dettaglio.Mat_Cod = GridViewImpianti.DataKeys(indice).Item("Mat_Cod_Lotto")  '0 se no magazzino, Mat_Cod altrimenti
            Movimento_Dettaglio.Udm_Cod = GridViewImpianti.DataKeys(indice).Item("Udm_Cod_Lotto")
            Movimento_Dettaglio.Lotto = GridViewImpianti.DataKeys(indice).Item("Lotto_Lotto")

        End If
        Movimento_Dettaglio.Mov_Det_Des = "Dettagli Tecnici Materia Prima per Semina/Trapianto"
        Movimento_Dettaglio.Qta = 0 'aggiungo dopo man mano
        Movimento_Dettaglio.Contabilizzato = Movimento_Dettaglio_Contabilizzato 'verificare
        Movimento_Dettaglio.Pendente = Movimento_Dettaglio_Pendente 'verificare fisso forse
        Movimento_Dettaglio.Data = Agenda.Data
        Movimento_Dettaglio.BaseCode = Agenda.BaseCode
        Movimento_Dettaglio.TopCode = Agenda.TopCode
        Movimento_Dettaglio.Cau_Mov = objParametriAgenda.Cau_Mov
        Movimento_Dettaglio.Anno = 1900
        'impostazioni in base alla lavorazione
        Select Case CInt(objParametriAgenda.Lav_Cod)
            Case LAVCOD_SEMINA, LAVCOD_TRAPIANTO, LAVCOD_SOVESCIO, LAVCOD_SOD_SEDDING
            Case Else
                Throw New NotImplementedException
        End Select

        'non usato finore, ma utile per la pagina
        Movimento_Dettaglio.Extra_Str = "" 'GridViewImpianti.DataKeys(indice).Item("Lotto_Lotto")

        Movimento_OperazioneColturale.Movimenti_Dettagli.Add(Movimento_Dettaglio)

        Dim sum1 As Decimal = 0
        Dim righeConDati As Integer = 0 'conteggio le righe con i dati, se non ci sono allora non salvo questa operazione agenda
        Dim AppNome As String


        Dim QtaIndicata As Boolean = False

        Dim var As String = Get_Varieta_Des(indice)
        If righeConDati = 0 Then
            Agenda.Des_Lib = Agenda.Des_Lib & "" & var & " "
        Else
            Agenda.Des_Lib = Agenda.Des_Lib & ", " & var & ""
        End If

        AppNome = Get_App_Nome(indice)

        If VerificaQtaZero = True Then
            If Not IsNumeric(Get_Qta(indice)) OrElse CDbl(Get_Qta(indice)) <= 0 Then
                Throw New Exception("ATTENZIONE! Per l'appezzamento " & AppNome & " non è stata indicata la quantità dell'articolo!")
            End If
        End If

        If VerificaQtaZero = True AndAlso
                           IsNumeric(Get_Qta(indice)) AndAlso
                           CDbl(Get_Qta(indice)) > 0 Then
            QtaIndicata = True

        End If

        sum1 += Get_Qta(indice)
        '---------------------------------
        'all'inizio del ciclo sono nella riga con il check visibile, nei cicli successivi
        'sono nelle righe dello stesso impianto, con check nascosti,
        'se trovo un altro check visibile allora esco dal ciclo
        For Each movdet As Movimento_Dettaglio In Movimento_OperazioneColturale.Movimenti_Dettagli
            'ricavo il mov dettaglio del lotto del magazzino corrispondente
            If objParametriAgenda.Fabbricato <> "0" Then
                'caso con magazzino
                'non è gestito multimagazzino
                If movdet.Mat_Cod = GridViewImpianti.DataKeys(indice).Item("Mat_Cod_Lotto") AndAlso
                                       movdet.Udm_Cod = GridViewImpianti.DataKeys(indice).Item("Udm_Cod_Lotto") AndAlso
                                       movdet.Lotto = GridViewImpianti.DataKeys(indice).Item("Lotto_Lotto") Then


                    If MateriaPrimaBio_SI_NO.Value = "0" Then

                        '(02/07/2018) verifica materia prima bio se impianto bio
                        Select Case RegolamentoApp

                            Case enum_MetodoProduzione.Biologico, enum_MetodoProduzione.InConversione

                                If GridViewImpianti.DataKeys(indice).Item("Regolamento_Lotto") <> enum_Cod_Regolamento.Regolamento_bio Then

                                    'Throw New Exception("ATTENZIONE! Poichè l'appezzamento " & AppNome & " è biologico è possibile utilizzare solo materie prime Biologiche!")
                                    messaggio_errore = "ATTENZIONE! Poichè l'appezzamento " & AppNome & " è biologico sarebbe necessario utilizzare solo materie prime Biologiche!"

                                    Messaggi.AgroSiNo(messaggio_errore & vbCr & Resources.AgronicaAgenda_2010.BrBIProcedereUgualmenteIB, "MateriaPrimaBio", Page, , updatepanelGridViewMagazzinoImpianti)
                                    messaggioSiNo = True

                                    Return False

                                End If

                        End Select

                    Else
                        'se ho già cliccato  ok vado avanti

                    End If


                    'se ho l'impostazione che vuole la qta obbligatoria salvo solo la destinazione con qta valorizzata
                    Dim InserisciImpianto As Boolean = True
                    If VerificaQtaZero = True Then
                        If Not (IsNumeric(Get_Qta(indice))) OrElse CDbl(Get_Qta(indice)) <= 0 Then
                            InserisciImpianto = False
                        End If
                    End If

                    If InserisciImpianto = True Then

                        'creo il mov destinazione
                        Dim Movimento_Destinazione As New Movimento_Destinazione
                        Movimento_Destinazione.Id_Agenda = objParametriAgenda.Id_Agenda
                        Movimento_Destinazione.Piva = GridViewImpianti.DataKeys(indice).Item("Piva")
                        Movimento_Destinazione.Sa_Cod = GridViewImpianti.DataKeys(indice).Item("Sa_Cod")
                        Movimento_Destinazione.Appezza = Appezza
                        Movimento_Destinazione.Id_Destinazione = Id_Reg
                        Movimento_Destinazione.Qta = Get_Qta(indice)
                        Movimento_Destinazione.Qta2 = Get_Superficie(indice)
                        Movimento_Destinazione.Data = Agenda.Data

                        movdet.Qta += Movimento_Destinazione.Qta


                        'imposto sa.cod del mov dettaglio
                        movdet.Sa_Cod = GridViewImpianti.DataKeys(indice).Item("Sa_Cod")

                        'aggiungo il mov destinazione al mov dettaglio del lotto
                        movdet.Movimenti_Destinazioni.Add(Movimento_Destinazione)

                    End If


                    righeConDati += 1

                    'Calcolo la quota distribuzione: su quantità distribuita se presente oppure su area.
                    Dim xSomma As Decimal
                    If movdet.Qta <> 0 Then
                        xSomma = movdet.Qta
                    Else
                        xSomma = (From xDest In movdet.Movimenti_Destinazioni Select xDest.Qta2).Sum()
                    End If

                    If xSomma <> 0 Then

                        For Each xQuotaDistr In movdet.Movimenti_Destinazioni
                            If xQuotaDistr.Qta <> 0 Then
                                xQuotaDistr.QuotaDistribuzione = xQuotaDistr.Qta / xSomma
                            Else
                                xQuotaDistr.QuotaDistribuzione = xQuotaDistr.Qta2 / xSomma
                            End If

                        Next
                    End If


                End If
            Else

                Throw New NotImplementedException

            End If



        Next



        Agenda.Des_Lib = Agenda.Des_Lib & "])"

        If righeConDati = 0 Then
            Throw New NotImplementedException(Resources.AgronicaAgenda_2010.NonSonoStateTrovateRigheConDatiNegliImpian)
        End If



        'verifico corrispondenze qta
        'sum1 calcolata sopra, prende i valori inseriti nella griglia
        Dim sum2Tot As Decimal = 0
        For Each movd As Movimento_Dettaglio In Movimento_OperazioneColturale.Movimenti_Dettagli
            Dim sum2 As Decimal = 0
            sum2 = movd.Qta
            sum2Tot += sum2
            Dim sum3 As Decimal = 0
            For Each md As Movimento_Destinazione In movd.Movimenti_Destinazioni
                sum3 += md.Qta
            Next
            If sum2 <> sum3 Then
                Throw New Exception(String.Format(Resources.AgronicaAgenda_2010.IncongruenzaDeiDatiContattareLAssistenzaX0, sum1, sum2, sum3))
            End If
            If Not IsNothing(movd.Movimenti_Dettagli_Riferiti) AndAlso movd.Movimenti_Dettagli_Riferiti.Count > 0 Then
                Dim sum4 As Decimal = 0
                For Each mr As Movimento_Dettaglio In movd.Movimenti_Dettagli_Riferiti
                    sum4 += mr.Qta
                Next
                If sum2 <> sum4 Then
                    Throw New Exception(String.Format(Resources.AgronicaAgenda_2010.IncongruenzaDeiDatiContattareLAssistenzaX4, sum1, sum2, sum3, sum4))
                End If
            End If
        Next
        If sum1 <> sum2Tot Then
            Throw New Exception(String.Format(Resources.AgronicaAgenda_2010.IncongruenzaDeiDatiContattareLAssistenzaX2, sum1, sum2Tot))
        End If

        Return True

    End Function



#End Region


#Region "Modifica Lettura"


    Private Sub CaricaListeValori_e_LeggiParametri_DaAgenda() Implements iOperazioneGUI_Semina.CaricaListeValori_e_LeggiParametri_DaAgenda

        Dim SeminaTrapianto_Con_Bolla As Boolean = False

        Dim Agenda As New Operazione_Agenda
        Dim objAgenda As New Agenda_Operazione_Helper

        If objParametriAgenda.TipoOperazioneAgenda = enum_Tipo_Operazione_Agenda.Ricetta Then

            Dim StringaXmlOperazione As String
            Dim objWebW As New AgronicaCoreVarieDAL.Web_ComunicazionePagine_W
            Dim objWebC As New AgronicaCoreVarieDAL.Web_ComunicazionePagine_R
            Dim DtWebC As DataTable
            DtWebC = objWebC.Leggi(Qs_Unid_Ricetta_Operazione, 0, AGRODATAINIZIO, AGRODATAFINE, enumSelezioneVariabile.Selezione_TabellaDatiMinimi, "", "", objParametri_Server)
            objWebW.Cancella(Qs_Unid_Ricetta_Operazione, 0, "", objParametri_Server)
            If DtWebC.Rows.Count > 0 Then
                StringaXmlOperazione = DtWebC.Rows(0).Item("Stringa_Parametri_Base")
                If StringaXmlOperazione <> "" Then
                    'passare solo l'operazione.
                    Dim strErr As String = ""

                    Dim BaseCode As Integer
                    Dim TopCode As Integer
                    Calcola_BaseCode_TopCode(
                        BaseCode,
                        TopCode,
                        Session("ASG_ProgressivoGIAS"))
                    Agenda = objAgenda.Genera_Agenda_Da_OperazioneRicetta(objParametriAgenda.TipoRicetta, objParametriAgenda.Data, objParametriAgenda.Piva, objParametriAgenda.Sa_Cod, BaseCode, TopCode, StringaXmlOperazione, strErr, objParametri_Server)
                End If
            End If
        Else


            Agenda = objAgenda.Leggi(objParametriAgenda.Piva,
                                                 CInt(objParametriAgenda.Sa_Cod),
                                                 CInt(objParametriAgenda.Id_Agenda),
                                                 0,
                                                 objParametri_Server)

        End If





        If Not IsNothing(Agenda) Then

            objParametriAgenda.Piva = Agenda.Piva
            objParametriAgenda.Sa_Cod = Agenda.Sa_Cod
            objParametriAgenda.Lav_Cod = Agenda.Lav_Cod

            'Imposto il fabbricato a zero, se poi c'è il magazzino verrà reimpostato più avanti,
            ' ma se non lo imposto riscio di caricare una operazione con il fabbricato preimpostato 
            'e visualizzare male il dato 
            objParametriAgenda.Fabbricato = "0"

            'NOTE
            If Not IsNothing(Agenda.Note) Then
                For i = 0 To Agenda.Note.Count - 1
                    objParametriAgenda.Note.Add(Agenda.Note(i))
                Next
            End If
            objParametriAgenda.salva()

            'MOVIMENTI
            If Not IsNothing(Agenda.Movimenti) Then

                Dim MovimentiCosti As List(Of Movimento) = New List(Of Movimento)
                objParametriAgenda.Impianti = New List(Of AgronicaCoreModello.ParametriAgenda_Temp.Impianto)

                For i = 0 To Agenda.Movimenti.Count - 1

                    'controllo corrispondeza piva con agenda
                    If Agenda.Piva <> Agenda.Movimenti(i).Piva Then
                        Throw New ApplicationException
                    End If

                    Select Case Agenda.Movimenti(i).Cau_Mov

                        Case enum_Agenda_Causali.LAVORAZIONE


                            If Agenda.Lav_Cod <> objParametriAgenda.Lav_Cod Then
                                'controllino per lo sviluppo, da togliere
                                Throw New NotImplementedException
                            End If

                            Select Case CInt(Agenda.Lav_Cod)
                                Case LAVCOD_SEMINA, LAVCOD_TRAPIANTO, LAVCOD_SOVESCIO, LAVCOD_SOD_SEDDING
                                    Dim utilbollerif As New AgronicaCoreContabDAL.Mov_Dettagli_Riferimenti_R()
                                    SeminaTrapianto_Con_Bolla = utilbollerif.Verifica_Operazione_SeminaTrapianto_Con_Bolle_Collegate(Agenda.Piva, Agenda.Sa_Cod, Agenda.Id_Agenda, Agenda.Lav_Cod, "", objParametri_Server)
                                    LeggiMovimentoAgenda_Semina(Agenda, i, SeminaTrapianto_Con_Bolla)
                                Case Else
                                    Throw New NotImplementedException
                            End Select

                        Case enum_Agenda_Causali.SCARICO

                            'MOVIMENTI_DETTAGLI
                            If Not IsNothing(Agenda.Movimenti(i).Movimenti_Dettagli) Then

                                If Agenda.Movimenti(i).Movimenti_Dettagli.Count < 1 Then

                                    'Tolgo l'eccezione, il lan salva sempre il movimento dei costi accessori anche se non ci sono dettagli
                                    'previsto se ci son cost accessori
                                    'Throw New NotImplementedException("C'è un movimento scarico senza i dettagli")
                                End If

                                For j = 0 To Agenda.Movimenti(i).Movimenti_Dettagli.Count - 1

                                    'controllo corrispondeza  piva con agenda, sa_cod potrebbe cambiare per chi ha fabbricato in altro centro
                                    If Agenda.Piva <> Agenda.Movimenti(i).Movimenti_Dettagli(j).Piva Then
                                        Throw New ApplicationException
                                    End If

                                    Select Case Agenda.Movimenti(i).Movimenti_Dettagli(j).Elem_Cod

                                        Case SEMENTI

                                            LeggiMovimentoAgenda_Scarico(Agenda, i, j)


                                        Case Else
                                            '----COSTO ACCESSORIO---------------------
                                            'è un movimento dovuto ad un costo accessorio
                                            'Throw New NotImplementedException
                                            MovimentiCosti.Add(Agenda.Movimenti(i))
                                            Exit For
                                    End Select
                                Next

                            End If


                        Case CAU_IMPUTAZIONE_PARCOMACCHINE
                            MovimentiCosti.Add(Agenda.Movimenti(i))

                        Case CAU_IMPUTAZIONE_MANODOPERA
                            MovimentiCosti.Add(Agenda.Movimenti(i))

                        Case CAU_IMPUTAZIONE_TERZISTI
                            MovimentiCosti.Add(Agenda.Movimenti(i))

                        Case CAU_IMPUTAZIONE_UTILIZZO_PRODOTTI
                            MovimentiCosti.Add(Agenda.Movimenti(i))

                        Case CAU_IMPUTAZIONE_TECNICO_RESPONSABILE
                            MovimentiCosti.Add(Agenda.Movimenti(i))

                        Case Else
                            Throw New NotImplementedException

                    End Select
                Next

                objParametriAgenda.Movimenti = MovimentiCosti

                'gestisco la selezione del fabbricato dell'azienda esterna se l'operazione era stata registrata con quella
                Dim util As New Utility_NS.Utility_Operazioni
                util.ImpostaFabbricatoDelMagazzinoEsternoSePresente(Agenda, objParametriAgenda, objParametri_Server)


            End If
        End If


        '................................
        'se tutto è andato bene ora ho i dati e devo settare le combo, text e ricostruire la tabella... 

    End Sub


    Private Sub LeggiMovimentoAgenda_Scarico(ByRef agenda As Operazione_Agenda, ByRef i As Integer, ByRef j As Integer)
        Dim Destinazione As Integer
        Dim SaCodDestinazione As Integer
        Dim tipo_Destinazione As Integer

        If Not IsNothing(agenda.Movimenti(i).Movimenti_Dettagli(j).Movimenti_Destinazioni) Then
            For x = 0 To agenda.Movimenti(i).Movimenti_Dettagli(j).Movimenti_Destinazioni.Count - 1

                If x = 0 Then
                    Destinazione = agenda.Movimenti(i).Movimenti_Dettagli(j).Movimenti_Destinazioni(x).Id_Destinazione
                    SaCodDestinazione = agenda.Movimenti(i).Movimenti_Dettagli(j).Movimenti_Destinazioni(x).Sa_Cod
                    tipo_Destinazione = agenda.Movimenti(i).Movimenti_Dettagli(j).Movimenti_Destinazioni(x).Tipo
                Else
                    If Destinazione <> agenda.Movimenti(i).Movimenti_Dettagli(j).Movimenti_Destinazioni(x).Id_Destinazione Or
                        SaCodDestinazione <> agenda.Movimenti(i).Movimenti_Dettagli(j).Movimenti_Destinazioni(x).Sa_Cod Then
                        Throw New NotImplementedException(Resources.AgronicaAgenda_2010.IMagazziniNonCoincidono)
                    End If
                End If

            Next

            'nelle operazioni salvate con genda vecchia c'è sempre il movimento magazzino anche se non è stato
            'usato il magazzino, le riconosco perchè hanno IDdestinazione=0 e tipo destinazione =0 invece che 20
            If Destinazione <> 0 AndAlso tipo_Destinazione = 20 Then

                objParametriAgenda.Fabbricato = Destinazione.ToString & "|" & SaCodDestinazione.ToString & "|" & agenda.Piva

            Else
                objParametriAgenda.Fabbricato = "0"
            End If

        End If
    End Sub


    Private Sub LeggiMovimentoAgenda_Semina(ByRef agenda As Operazione_Agenda, ByRef i As Integer, ByRef SeminaTrapianto_Con_Bolla As Boolean)

        'se sono con bolla oltre che impostare il valore bolla devo anche ricreare la lista di movimenti dettagli
        'da salvare in Session("MovimentiDettagliDDT") come nel caso di allago bolle con graffetta
        Session("MovimentiDettagliDDT") = Nothing
        ImageInfo0.Visible = False
        LabelInfo0.Visible = False
        If SeminaTrapianto_Con_Bolla Then
            Dim MovimentiDettagliDDT As New List(Of Movimento_Dettaglio)
            Session("MovimentiDettagliDDT") = MovimentiDettagliDDT
            ImageInfo0.Visible = True
            LabelInfo0.Visible = True
        End If




        ListaValoriSeminaImpianti = New List(Of ValoriImpiantoESemineImpostati)

        ''------------------------------------------
        ''----- Dichiarazione delle Variabili
        ''------------------------------------------

        'variabili univoche per ciascuna operazione agenda-trappola
        Dim Specie As Integer = 0

        objParametriAgenda.Data = agenda.Movimenti(i).Data

        Master_Operazione.SetNota(agenda.Movimenti(i).Mov_Desc)

        Dim Tipo_Semina_Salvata As enum_SEMINA_TIPO = agenda.Movimenti(i).Extra_Int  'salvo il tipo raccolta, puo essere utile


        'MOVIMENTO_DETTAGLIO TECNICO (dovrebbe essere nothing)
        If Not IsNothing(agenda.Movimenti(i).Movimenti_Dettagli_Tecnici) Then

            For j = 0 To agenda.Movimenti(i).Movimenti_Dettagli_Tecnici.Count - 1

                'controllo corrispondeza piva con agenda
                If agenda.Piva <> agenda.Movimenti(i).Piva Then
                    Throw New ApplicationException
                End If

                'non dovrebbe essercene nessuno
                Throw New NotImplementedException
            Next

        End If



        '---------------------
        '----MOVIMENTI_DETTAGLI
        '---------------------
        'uno per ciascun lotto
        If Not IsNothing(agenda.Movimenti(i).Movimenti_Dettagli) Then

            For j = 0 To agenda.Movimenti(i).Movimenti_Dettagli.Count - 1

                'controllo corrispondeza piva con agenda
                'sa cod è uguale ma potrebbe cambiare se...
                If agenda.Piva <> agenda.Movimenti(i).Movimenti_Dettagli(j).Piva Then
                    Throw New ApplicationException
                End If

                'implicito ma lo controllo dato che lo riscrivo
                If SEMENTI <> agenda.Movimenti(i).Movimenti_Dettagli(j).Elem_Cod Then
                    Throw New ApplicationException
                End If




                'lisa dei movimenti dettagli bolle per quel prodotto-udm-lotto
                Dim MovimentiDettagliDDT_ParzialeProdotto As List(Of Movimento_Dettaglio)
                'mi serve piu avanti per ripartire le qta su superficie se ho bolle con stesso prodotto
                ' Dim SuperficieTotImpiantiProdotto As Decimal = 0
                If SeminaTrapianto_Con_Bolla Then
                    '---------------------------------------------------------------------------------------------------
                    '--------GESTIONE DELLA BOLLA COLLEGATA--------------------------------------------------------------
                    '-------------------------------------------------------------------------------------------


                    'For k = 0 To agenda.Movimenti(i).Movimenti_Dettagli(j).Movimenti_Destinazioni.Count - 1
                    '    SuperficieTotImpiantiProdotto += New AgronicaCoreAnagrafeDAL.Appezzamento_Read().Superficie_from_PivaSaCodAppezza(agenda.Movimenti(i).Movimenti_Dettagli(j).Movimenti_Destinazioni(k).Piva, _
                    '                                                                                                                      agenda.Movimenti(i).Movimenti_Dettagli(j).Movimenti_Destinazioni(k).Sa_Cod, _
                    '                                                                                                                      agenda.Movimenti(i).Movimenti_Dettagli(j).Movimenti_Destinazioni(k).Appezza,
                    '                                                                                                                      objParametri_Server)
                    'Next

                    'la gestione della bolla collegata la faccio in due punti, prima della destinazione,
                    'dove creo la LISTA DEI MOVIMENTI DETTAGLI Session("MovimentiDettagliDDT") che contiene le info sul dettaglio bolla-prodotto
                    'e poi dentro la estinazione dovre creo per quella destinazione un ValoriImpiantoESemineImpostati
                    'per ciascun dettaglio bolla collegato al prodotto
                    '
                    'Quindi:
                    '
                    'se ho una sola riga di dettaglio bolle per ciascun prodotto-lotto-udm  
                    'allora non ci sono grossi problemi, dato che è suffic fare il collegamento con la bolla 
                    'riempiendo il campo ValoriImpiantoESemineImpostati.Bolla = dtmd.Rows(h).Item("Id_Agenda") & "|" & dtmd.Rows(h).Item("Id_Mov") & "|" & dtmd.Rows(h).Item("Id_Mov_Det") & ""
                    'mentre la qta del riferimento è quella del prodotto utilizzato quindi corrisponde alla qta del mov dettaglio trapianto
                    '
                    'SE invece ho più righe nei riferimenti per lo stesso prodotto-udm-lotto e quindi ho lo stesso
                    'prodotto-udm-lotto collegato a più dettagli di bolle
                    'devo leggere anche la qta del prodotto-bolla utilizzato nella tabella riferimenti e 
                    'creare n X m righe in ValoriImpiantoESemineImpostati 
                    'con n  che rappresenat tutte le righe bolle dei riferimenti con stesso prodotto 
                    'e m il numero di destinazioni
                    ' dopodiche devo ripartire
                    'le qta considerando la qta del rif e la qtatotoale e la qta per destinazione

                    '---------------------------------------------------------------------------------------------------
                    '--------LEGGO IL DETTAGLIO DEL MOVIMENTO DELLA BOLLA COLLEGATA--------------------------------------------------------------
                    '-------------------------------------------------------------------------------------------
                    'se c'è bolla collegata devo leggere il dettaglio collegato per ciascun
                    'dettaglio dell'operazione e inserire i valori id_agenda|id_mov|id_mov_det che è come
                    'viene inserito l'id bolla in Dr.Item("Bolla")=
                    'Giacenze.Rows(i).Item("id_agenda_ddt_det") & "|" & Giacenze.Rows(i).Item("id_mov_ddt_det") & "|" & Giacenze.Rows(i).Item("id_Mov_Det_ddt_det") & ""
                    'in ValoriImpiantoESemineImpostati.Bolla
                    Dim utilbollerif As New AgronicaCoreContabDAL.Mov_Dettagli_Riferimenti_R()
                    'questa funzione ritorna il movimento dettaglio della bolla collegata
                    Dim dtmd As DataTable = utilbollerif.Recupera_Dettagli_Bolle_Collegate_Al_Dettaglio_SeminaTrapianto(
                                                 agenda.Movimenti(i).Movimenti_Dettagli(j).Piva,
                                                 agenda.Movimenti(i).Movimenti_Dettagli(j).Sa_Cod,
                                                 agenda.Movimenti(i).Movimenti_Dettagli(j).Id_Agenda,
                                                 agenda.Movimenti(i).Movimenti_Dettagli(j).Id_Mov,
                                                 agenda.Movimenti(i).Movimenti_Dettagli(j).Id_Mov_Det,
                                                 agenda.Lav_Cod,
                                                 objParametri_Server)
                    If dtmd.Rows.Count = 0 Then

                        Throw New Exception(Resources.AgronicaAgenda_2010.ErroreNonPrevisto)


                    Else
                        'CREAZIONE DELLA LISTA PARZIALE DEI MOVIMENTI DETTAGLI BOLLE Del SINGOLO PRODOTTO
                        MovimentiDettagliDDT_ParzialeProdotto = New List(Of Movimento_Dettaglio)
                        For h = 0 To dtmd.Rows.Count - 1
                            'Se ho lo stesso prodotto utilizzato con lo stesso lotto e stessa udm inbolle differenti
                            'allora ho un movimento dettaglio del prodotto semina collegato a più movimenti dettagli di ddt,
                            'quindi devo aggiungere un elemento a ValoriImpiantoESemineImpostati per ciascun dettaglio ddt,
                            ' e moltiplicarli per il numero di destinazioni

                            '---------------------------------------------------------------------------------------------------
                            '---------------------------------------------------------------------------------------------------
                            '---------------------------------------------------------------------------------------------------
                            'aggiungo il movimento alla LISTA PARZIALE DEI MOVIMENTI DETTAGLI BOLLE Del SINGOLO PRODOTTO
                            Dim movdet As Movimento_Dettaglio = New Movimento_Dettaglio()
                            movdet.Piva = dtmd.Rows(h).Item("Piva")
                            movdet.Sa_Cod = dtmd.Rows(h).Item("Sa_Cod")
                            movdet.Id_Agenda = dtmd.Rows(h).Item("Id_Agenda")
                            movdet.Id_Mov = dtmd.Rows(h).Item("Id_Mov")
                            movdet.Id_Mov_Det = dtmd.Rows(h).Item("Id_Mov_Det")
                            movdet.Mat_Cod = dtmd.Rows(h).Item("Mat_Cod")
                            movdet.Udm_Cod = dtmd.Rows(h).Item("Udm_Cod")
                            'alla qta memorizzo la qta parziale utuilizzata del prodotto bolla e non la qta totale della riga bolla
                            movdet.Qta = dtmd.Rows(h).Item("Qta_Risultato")
                            ' movdet.Lotto = Lotto
                            movdet.Extra_Str = "0" 'lo uso dopo per salvare i dati parziali durante la distribuzione delle qta
                            'aggiungo il movimento e salvo la lista parziale
                            MovimentiDettagliDDT_ParzialeProdotto.Add(movdet)
                            '---------------------------------------------------------------------------------------------------
                            '---------------------------------------------------------------------------------------------------
                            '---------------------------------------------------------------------------------------------------


                        Next


                    End If

                    '---------------------------------------------------------------------------------------------------
                    '--------CREAZIONE DELLA LISTA DEI MOVIMENTI DETTAGLI Session("MovimentiDettagliDDT")-----------------------------------------------------------------------------
                    '-------------------------------------------------------------------------------------------
                    'aggiungo la lista dei movimenti dettagli bolle collegati al prodotto in questione prodotto-udm-lotto
                    ' alla lista salvata in sessione che serve per caricare la tabella giacenze
                    'Se ho un solo dettaglio per ciscun prodotto la lista ha un elemento
                    'LA LISTA VIENE USATA DALLA PAGINA PER CAPIRE SE CI SONO DDT ALLEGATI 
                    'E VIENE USATA PER SCEGLIERE SE CARICARE E VISUALIZZARE LA TABELLA DEL MAGAZZINO
                    'IN JOIN CON LE BOLLE COLLEGATE
                    Dim MovimentiDettagliDDT As List(Of Movimento_Dettaglio) = Session("MovimentiDettagliDDT")
                    ' movdet.Lotto = Lotto
                    'aggiungo il movimento e salvo la lista in sessione
                    MovimentiDettagliDDT.AddRange(MovimentiDettagliDDT_ParzialeProdotto)
                    Session("MovimentiDettagliDDT") = MovimentiDettagliDDT

                    '-------------------------------------------------------------------------------------------
                    '-------------------------------------------------------------------------------------------
                    '-------------------------------------------------------------------------------------------
                End If




                '---------------------
                'MOVIMENTO_DESTINAZIONI
                '---------------------
                'una  destinazione per ciscun appezzamento in cui è utilizzato il semente
                'ciascun semente è uitilizzato negli stessi impianti, per come è stata creata la pagina
                Dim AppezzamentoCorrente As Integer = 0
                If IsNothing(agenda.Movimenti(i).Movimenti_Dettagli(j).Movimenti_Destinazioni) Or
                                 agenda.Movimenti(i).Movimenti_Dettagli(j).Movimenti_Destinazioni.Count < 1 Then
                    'deve esistere almeno una destinazione
                    Throw New NotImplementedException(Resources.AgronicaAgenda_2010.DeveEsistereAlmenoUnaDestinazione)
                End If

                Dim idnudo As Integer = 0
                Dim idnudoold As Integer = 0
                Dim cambiatonudo As Boolean = False

                For k = 0 To agenda.Movimenti(i).Movimenti_Dettagli(j).Movimenti_Destinazioni.Count - 1

                    'leggo la specie
                    Dim objImp As New AgronicaCoreAnagrafeDAL.Reg_Impianti_Read
                    Dim Dt_Imp As New DataTable
                    Dt_Imp = objImp.Leggi(agenda.Movimenti(i).Movimenti_Dettagli(j).Movimenti_Destinazioni(k).Piva,
                                 agenda.Movimenti(i).Movimenti_Dettagli(j).Movimenti_Destinazioni(k).Sa_Cod,
                                 agenda.Movimenti(i).Movimenti_Dettagli(j).Movimenti_Destinazioni(k).Appezza,
                                 agenda.Movimenti(i).Movimenti_Dettagli(j).Movimenti_Destinazioni(k).Id_Destinazione,
                                 enumSelezioneVariabile.Selezione_JoinDescrizioni,
                                 "", "", objParametri_Server)

                    If Dt_Imp.Rows.Count > 0 Then

                        If j <> 0 AndAlso k <> 0 Then
                            'se sono dal secondo ciclo e le specie vegetali non coincidono con quella precedente allore qualcosa non va
                            If objParametriAgenda.Veg_Cod.Split("/")(0) <> CInt(Dt_Imp.Rows(0).Item("veg_cod")) Then
                                Throw New NotImplementedException(Resources.AgronicaAgenda_2010.SpecieNonCoincidonoNegliInpiantiDellOperaz)
                            End If
                        End If

                        'objParametriAgenda.Veg_Cod = CStr(CInt(Dt_Imp.Rows(0).Item("veg_cod")))

                        If k = 0 Then
                            objParametriAgenda.Veg_Cod = CInt(Dt_Imp.Rows(0).Item("veg_cod"))
                        End If

                        'per operazione su terreno nudo,
                        'se semprela solita destinazione la setto nel veg_cod, altrimenti lascio 0
                        If objParametriAgenda.Veg_Cod = 0 And CInt(Dt_Imp.Rows(0).Item("id_cod")) <> 0 Then
                            If k = 0 Then
                                idnudo = CInt(Dt_Imp.Rows(0).Item("id_cod"))
                                idnudoold = idnudo
                            Else
                                idnudo = CInt(Dt_Imp.Rows(0).Item("id_cod"))
                                If idnudoold <> idnudo Then
                                    cambiatonudo = True
                                End If
                            End If
                            If k = agenda.Movimenti(i).Movimenti_Dettagli(j).Movimenti_Destinazioni.Count - 1 Then
                                If cambiatonudo Then
                                    objParametriAgenda.Veg_Cod = "0"
                                Else
                                    objParametriAgenda.Veg_Cod = "0/" & idnudo
                                End If
                            End If
                        End If



                    Else
                        Throw New NotImplementedException(Resources.AgronicaAgenda_2010.SpecieNonPresenteNellImpianto)
                    End If

                    'If objParametriAgenda.Lav_Cod = LAVCOD_SOVESCIO Then

                    Dim objAppezzamento As New AgronicaCoreModello.ParametriAgenda_Temp.Impianto
                    objAppezzamento.Piva = agenda.Movimenti(i).Movimenti_Dettagli(j).Movimenti_Destinazioni(k).Piva
                    objAppezzamento.Sa_Cod = agenda.Movimenti(i).Movimenti_Dettagli(j).Movimenti_Destinazioni(k).Sa_Cod
                    objAppezzamento.Appezza = agenda.Movimenti(i).Movimenti_Dettagli(j).Movimenti_Destinazioni(k).Appezza
                    objAppezzamento.ID_Reg = agenda.Movimenti(i).Movimenti_Dettagli(j).Movimenti_Destinazioni(k).Id_Destinazione

                    objAppezzamento.Qta2 = agenda.Movimenti(i).Movimenti_Dettagli(j).Movimenti_Destinazioni(k).Qta2
                    objParametriAgenda.Impianti.Add(objAppezzamento)
                    objParametriAgenda.salva()

                    'End If

                    If SeminaTrapianto_Con_Bolla Then
                        '---------------------------------------------------------------------------------------------------
                        '--------GESTIONE DELLA BOLLA COLLEGATA--------------------------------------------------------------
                        '-------------------------------------------------------------------------------------------

                        '...(commento copiato da sopra)
                        'SE invece ho più righe nei riferimenti per lo stesso prodotto-udm-lotto e quindi ho lo stesso
                        'prodotto-udm-lotto collegato a più dettagli di bolle
                        'devo leggere anche la qta del prodotto-bolla utilizzato nella tabella riferimenti e 
                        'creare n X m righe in ValoriImpiantoESemineImpostati 
                        'con n  che rappresenat tutte le righe bolle dei riferimenti con stesso prodotto 
                        'e m il numero di destinazioni
                        ' dopodiche devo ripartire
                        'le qta considerando la qta del rif e la qtatotoale e la qta per destinazione
                        Dim Distribuita_Appezza_Prodotto As Decimal = 0
                        Dim Totale_Appezza_Prodotto As Decimal = agenda.Movimenti(i).Movimenti_Dettagli(j).Movimenti_Destinazioni(k).Qta
                        Dim indice As Integer = 0
                        For Each movDetProdBolla As Movimento_Dettaglio In MovimentiDettagliDDT_ParzialeProdotto
                            indice += 1

                            'leggo la qta totale del prodotto-udm-lotto, considera quindi tutte le bolle dettagli di quel prodotto
                            Dim qtaTotale_Prodotto As Decimal = agenda.Movimenti(i).Movimenti_Dettagli(j).Qta

                            'ho salvato la qta parziale prersente nella tabella dei riferimenti
                            Dim Totale_Bolla_Prodotto As Decimal = movDetProdBolla.Qta

                            'quantità finora redistribuita del prodottobolla
                            Dim Distribuita_Bolla_Prodotto As Decimal = CDbl(movDetProdBolla.Extra_Str)

                            'qta rimante
                            Dim Rimanente_Bolla_Prodotto As Decimal = Totale_Bolla_Prodotto - Distribuita_Bolla_Prodotto

                            'qta rimante appezza
                            Dim Rimanente_Appezza_Prodotto As Decimal = Totale_Appezza_Prodotto - Distribuita_Appezza_Prodotto

                            Dim Qta_Bolla_X_Appezza As Decimal = 0

                            'prima cerco di utilizzare tutte le qta bolle_prodotto fino ad esaurirle,
                            'i conti devono tornare per forza alla fine

                            If Rimanente_Bolla_Prodotto <= Rimanente_Appezza_Prodotto Then
                                'uso il rimanente della bolla, le altre bolle satureranno la qta appezza
                                Qta_Bolla_X_Appezza = Rimanente_Bolla_Prodotto
                            Else
                                'uso il rimanente dell'appezza, quindi mi rimane della bolla che sarà scaricato su altri appezza
                                Qta_Bolla_X_Appezza = Rimanente_Appezza_Prodotto
                            End If

                            'aggirono i rimanenti
                            Distribuita_Bolla_Prodotto += Qta_Bolla_X_Appezza
                            movDetProdBolla.Extra_Str = CStr(Distribuita_Bolla_Prodotto)
                            Distribuita_Appezza_Prodotto += Qta_Bolla_X_Appezza




                            '---------------------------------------------------------------------------------------------------
                            '--------IMPOSTO IL VALORE .BOLLA NELLA LISTA DEI VALORI DELLA RIGA-----------------------------------------------------------------------
                            '-------------------------------------------------------------------------------------------
                            Dim ValoriImpiantoESemineImpostati As New ValoriImpiantoESemineImpostati
                            ValoriImpiantoESemineImpostati.Piva = agenda.Movimenti(i).Movimenti_Dettagli(j).Movimenti_Destinazioni(k).Piva
                            ValoriImpiantoESemineImpostati.Sa_Cod = agenda.Movimenti(i).Movimenti_Dettagli(j).Movimenti_Destinazioni(k).Sa_Cod
                            ValoriImpiantoESemineImpostati.Appezza = agenda.Movimenti(i).Movimenti_Dettagli(j).Movimenti_Destinazioni(k).Appezza
                            ValoriImpiantoESemineImpostati.Id_Reg = agenda.Movimenti(i).Movimenti_Dettagli(j).Movimenti_Destinazioni(k).Id_Destinazione
                            ValoriImpiantoESemineImpostati.Programmazione_Entita_cod = agenda.Movimenti(i).Movimenti_Dettagli(j).Movimenti_Destinazioni(k).Programmazione_Entita_Cod

                            'ValoriImpiantoESemineImpostati.QTA_Tot = agenda.Movimenti(i).Movimenti_Dettagli(j).Qta
                            ValoriImpiantoESemineImpostati.Elem_Cod = agenda.Movimenti(i).Movimenti_Dettagli(j).Elem_Cod
                            ValoriImpiantoESemineImpostati.Mat_Cod = agenda.Movimenti(i).Movimenti_Dettagli(j).Mat_Cod
                            ValoriImpiantoESemineImpostati.Pro_Cod = agenda.Movimenti(i).Movimenti_Dettagli(j).Pro_Cod
                            ValoriImpiantoESemineImpostati.UDM_Cod = agenda.Movimenti(i).Movimenti_Dettagli(j).Udm_Cod
                            ValoriImpiantoESemineImpostati.Lotto = agenda.Movimenti(i).Movimenti_Dettagli(j).Lotto
                            'mi serve un oggettino dove tenere la destinaz+lotto+qtadelladestinazione



                            ValoriImpiantoESemineImpostati.Bolla = movDetProdBolla.Id_Agenda & "|" & movDetProdBolla.Id_Mov & "|" & movDetProdBolla.Id_Mov_Det & ""
                            'IN QUESTO MODO POSSO RICONOSCERE LA RIGA DELLA TABELLA ASSOCIATA AL DETTAGLIO BOLLA
                            'E CHECCARLE IL MAGAZZINO E RIGA IMPIANTI


                            'attenzione, la qua cambia, non è nel mov dettaglio dell'operazione, , quella si riferisce al prodotto,
                            'ma se lo stesso è usato con lotto e udm in più bolle allora la qua la trovo 
                            'nel riferimento!!!!
                            ValoriImpiantoESemineImpostati.QTA_Dest = Qta_Bolla_X_Appezza

                            'per comodità mi salvo anche il totale della qta del prodotto-lotto-udm (e bolla)
                            'anche se non è riferito alla destinazione ma solo al prodotto e quindi duplicato su diverse righe,
                            'ma mi è utile perchè posso preimpostare il valore nella box della griglia impianti
                            'se sono con le bolle il valoro lo prendo dalla tabella dei riferimenti, altrimenti
                            ' è il valore nel dettaglio dei movimenti
                            ValoriImpiantoESemineImpostati.QTA_Tot_Prodotto = movDetProdBolla.Qta

                            ListaValoriSeminaImpianti.Add(ValoriImpiantoESemineImpostati)


                        Next

                        '-------------------------------------------------------------------------------------------
                        '-------------------------------------------------------------------------------------------
                        '-------------------------------------------------------------------------------------------
                    Else


                        'mi serve un oggettino dove tenere la destinaz+lotto+qtadelladestinazione
                        Dim ValoriImpiantoESemineImpostati As New ValoriImpiantoESemineImpostati
                        ValoriImpiantoESemineImpostati.Piva = agenda.Movimenti(i).Movimenti_Dettagli(j).Movimenti_Destinazioni(k).Piva
                        ValoriImpiantoESemineImpostati.Sa_Cod = agenda.Movimenti(i).Movimenti_Dettagli(j).Movimenti_Destinazioni(k).Sa_Cod
                        ValoriImpiantoESemineImpostati.Appezza = agenda.Movimenti(i).Movimenti_Dettagli(j).Movimenti_Destinazioni(k).Appezza
                        ValoriImpiantoESemineImpostati.Id_Reg = agenda.Movimenti(i).Movimenti_Dettagli(j).Movimenti_Destinazioni(k).Id_Destinazione
                        ValoriImpiantoESemineImpostati.Programmazione_Entita_cod = agenda.Movimenti(i).Movimenti_Dettagli(j).Movimenti_Destinazioni(k).Programmazione_Entita_Cod
                        ValoriImpiantoESemineImpostati.QTA_Dest = agenda.Movimenti(i).Movimenti_Dettagli(j).Movimenti_Destinazioni(k).Qta
                        'ValoriImpiantoESemineImpostati.QTA_Tot = agenda.Movimenti(i).Movimenti_Dettagli(j).Qta
                        ValoriImpiantoESemineImpostati.Elem_Cod = agenda.Movimenti(i).Movimenti_Dettagli(j).Elem_Cod
                        ValoriImpiantoESemineImpostati.Mat_Cod = agenda.Movimenti(i).Movimenti_Dettagli(j).Mat_Cod
                        ValoriImpiantoESemineImpostati.Pro_Cod = agenda.Movimenti(i).Movimenti_Dettagli(j).Pro_Cod
                        ValoriImpiantoESemineImpostati.UDM_Cod = agenda.Movimenti(i).Movimenti_Dettagli(j).Udm_Cod
                        ValoriImpiantoESemineImpostati.Lotto = agenda.Movimenti(i).Movimenti_Dettagli(j).Lotto
                        ValoriImpiantoESemineImpostati.Bolla = ""

                        'per comodità mi salvo anche il totale della qta del prodotto-lotto-udm (e bolla)
                        'anche se non è riferito alla destinazione ma solo al prodotto e quindi duplicato su diverse righe,
                        'ma mi è utile perchè posso preimpostare il valore nella box della griglia impianti
                        'se sono con le bolle il valoro lo prendo dalla tabella dei riferimenti, altrimenti
                        ' è il valore nel dettaglio dei movimenti
                        ValoriImpiantoESemineImpostati.QTA_Tot_Prodotto = agenda.Movimenti(i).Movimenti_Dettagli(j).Qta

                        ListaValoriSeminaImpianti.Add(ValoriImpiantoESemineImpostati)

                    End If


                Next


                'HACK: vanni, 04/10/2016: escludo dopo discussione (verifico corrispondenze qta)
                'Dim sum2 As Decimal = 0
                'sum2 = agenda.Movimenti(i).Movimenti_Dettagli(j).Qta
                'Dim sum3 As Decimal = 0
                'For k = 0 To agenda.Movimenti(i).Movimenti_Dettagli(j).Movimenti_Destinazioni.Count - 1
                '    sum3 += agenda.Movimenti(i).Movimenti_Dettagli(j).Movimenti_Destinazioni(k).Qta
                'Next
                'If sum2 <> sum3 Then
                '    Throw New Exception(String.Format(Resources.AgronicaAgenda_2010.IncongruenzaDeiDatiContattareLAssistenzaX2, sum2, sum3))
                'End If



            Next

            Dim objMP As New AgronicaCoreAnagrafeDAL.Materie_Prime_R
            Dim HashSpecie As New Hashtable
            Dim VegCod As Integer
            'verifico corrispondenze qta
            Dim sum2tot As Decimal = 0
            For k = 0 To agenda.Movimenti(i).Movimenti_Dettagli.Count - 1
                sum2tot += agenda.Movimenti(i).Movimenti_Dettagli(k).Qta
            Next
            Dim sum1 As Decimal = 0

            For Each ValoriImpiantoESemineImpostati As ValoriImpiantoESemineImpostati In ListaValoriSeminaImpianti
                sum1 += ValoriImpiantoESemineImpostati.QTA_Dest
                VegCod = 0
                'verifico specie vegetali diverse
                objMP.VegCod_CulCod_from_MatCod("",
                                                ValoriImpiantoESemineImpostati.Elem_Cod,
                                                ValoriImpiantoESemineImpostati.Mat_Cod,
                                                VegCod,
                                                0,
                                                objParametri_Server)

                If Not HashSpecie.ContainsKey(VegCod) Then
                    HashSpecie.Add(VegCod, "")
                End If
            Next

            'HACK: vanni, 04/10/2016: escludo dopo discussione (verifico corrispondenze qta)
            'If sum2tot <> sum1 Then
            '    Throw New Exception(String.Format(Resources.AgronicaAgenda_2010.IncongruenzaDeiDatiContattareLAssistenzaX2, sum2tot, sum1))
            'End If

            If HashSpecie.Count > 1 Then
                CheckBoxSeminaMultiSpecie.Checked = True
            End If

            If SeminaTrapianto_Con_Bolla Then

                ImageInfo0.Visible = True
                LabelInfo0.Visible = True

                'HACK: vanni, 04/10/2016: escludo dopo discussione (verifico corrispondenze qta)
                'Dim sum4 As Decimal = 0
                'Dim MovimentiDettagliDDT As List(Of Movimento_Dettaglio) = Session("MovimentiDettagliDDT")
                'For k = 0 To MovimentiDettagliDDT.Count - 1
                '    sum4 += MovimentiDettagliDDT(k).Qta
                'Next
                'If sum1 <> sum4 Then
                '    Throw New Exception(String.Format(Resources.AgronicaAgenda_2010.IncongruenzaDeiDatiContattareLAssistenzaX0, sum1, sum2tot, sum4))
                'End If
            End If


        Else
            'ci seve essere almeno un movimento dettaglio per l'ìinstallazione di una trappola in un appezzamento
            Throw New ApplicationException
        End If



    End Sub


#End Region


    Private Sub AnnullaTutto(sender As Object, e As ImageClickEventArgs)

        If objParametriAgenda.SitoOrigine = Enum_SiteRedirector.GiasLan Then
            Dim strJS As New StringBuilder
            strJS.AppendLine("$(document).ready(function () { ")
            strJS.AppendLine("      window.close(); ")
            strJS.AppendLine(" });")

            ScriptManager.RegisterStartupScript(
                           UpdatePanelMagazzinoImpianti,
                           UpdatePanelMagazzinoImpianti.GetType(),
                               String.Format("jQuery_{0}", UpdatePanelMagazzinoImpianti.ClientID), strJS.ToString, True)

            Exit Sub
        End If


        inizializzoParametriAgenda()
        Dim tornaanagrafica As Boolean = False
        'Dim enumredirect As Integer = enum_PagineGiasOnline.MenuAgenda
        ''se ho la listaallora sono arrivato dall'anagrafe e devo tornare lì
        'If objParametriAgenda.Impianti.Count = 1 Then
        '    enumredirect = enum_PagineGiasOnline.AlberoImprese
        '    objParametriAgenda.TornaASitoOrigine = True
        '    tornaanagrafica = True
        'End If
        Session("MovimentiDettagliDDT") = Nothing
        ImageInfo0.Visible = False
        LabelInfo0.Visible = False
        objParametriAgenda.Svuota_DatiOperazione()


        Dim link As String = ""
        Try
            Dim sitoorigine As Enum_SiteRedirector = HttpContext.Current.Session("Sito_Origine")
            Dim paginaOnLineRitorno As Integer = objParametriAgenda.PaginaSitoOrigine

            If sitoorigine = Enum_SiteRedirector.Sito_GiasOnline_2010 And paginaOnLineRitorno = enum_PagineGiasOnline_2010.RegistazioneSmart Then
                link = AgronicaCoreGestioneRichieste.RedirectGestione.IndirizzoCompleto_SitoOnline2010_PassandoDirettamenteIParametri(
                                       Enum_SiteRedirector.Sito_AgronicaAgenda_2010,
                                       enum_PagineGiasOnline_2010.RegistazioneSmart,
                                       enum_PagineAgenda_2010.Menu, objParametriAgenda.Piva, "", "", 0, "")

            ElseIf tornaanagrafica Then

                Dim objGiasOnline As New AgronicaCoreGestioneRichieste.ParametriGiasOnline
                objGiasOnline.Cul_Cod = objParametriAgenda.Cul_Cod
                objGiasOnline.DataSelezionata = objParametriAgenda.Data
                objGiasOnline.Id_Agenda = objParametriAgenda.Id_Agenda
                objGiasOnline.Lavorazione = objParametriAgenda.Lav_Cod
                objGiasOnline.PaginaRichiesta = enum_PagineGiasOnline.AlberoImprese
                objGiasOnline.Piva = objParametriAgenda.Piva
                objGiasOnline.Sa_Cod = objParametriAgenda.Sa_Cod
                Dim specie As Integer = 0
                If IsNumeric(objParametriAgenda.Veg_Cod.Split("/")(0)) AndAlso CInt(objParametriAgenda.Veg_Cod.Split("/")(0)) > 0 Then
                    specie = CInt(objParametriAgenda.Veg_Cod.Split("/")(0))
                End If
                objGiasOnline.Veg_Cod = specie

                link = AgronicaCoreGestioneRichieste.RedirectGestione.IndirizzoCompleto_SitoOnline_PassandoDirettamente_ParametriGiasOnline(
                               Enum_SiteRedirector.Sito_AgronicaAgenda_2010,
                               objGiasOnline)
            ElseIf sitoorigine = Enum_SiteRedirector.GiasNG Then
                MenuBS_2017_RedirectGestione.RedirectGenerico(objParametriAgenda.Piva,
                                                                                      Enum_SiteRedirector.GiasNG,
                                                                                      objParametriAgenda.PaginaSitoOrigine,
                                                                                      link,
                                                                                      objParametri_Server)
            Else
                link = CType(Master.Master, Agenda).TrovaRedirectCorretto(False, enum_PagineGiasOnline.MenuAgenda, objParametriAgenda)
            End If

        Catch ex As Exception
            link = CType(Master.Master, Agenda).TrovaRedirectCorretto(False, enum_PagineGiasOnline.MenuAgenda, objParametriAgenda)
        End Try

        Response.Redirect(link)


    End Sub


    Private Function Modifica_Impianto(ByRef Ind As Integer,
                                       ByRef Log_Errori As String, ByVal id_agemnda_new As String,
                                       Optional ByVal ModificaSup As Boolean = False) As Boolean
        If id_agemnda_new <> "" Then
            id_agemnda_new = "(" & id_agemnda_new & ")"
        End If


        If objParametriAgenda.Lav_Cod = LAVCOD_SOVESCIO Then
            Throw New Exception("Con il sovescio non è consentita la modifica impianto")
        End If

        Try



            If PERMETTI_OPERAZIONI_SU_IMPIANTI_BLOCCATI Then
                'se lì'impianto è visibile perchè permetto le operazioni su bloccato devo impedire la modifica dell'impianto
                If New AgronicaCoreAnagrafeDAL.Appezzamento_Read().VerificaAppezzamentoBloccato(
                                    CStr(GridViewImpianti.DataKeys(Ind).Item("Piva")),
                                    CInt(GridViewImpianti.DataKeys(Ind).Item("Sa_Cod")),
                                    CInt(GridViewImpianti.DataKeys(Ind).Item("Appezza")),
                                    objParametri_Server) Then

                    Messaggi.AgroMsgBox("Alcuni impianti sono bloccati in anagrafica in quanto sincronizzati o scaricati, l'operazione di Semina/Trapianto verrà comunque registrata, ma i dati dell'impianto come la varietà, finalità, regolamento, sesto etc non verranno modificati.", Page, , Master_Operazione.Property_UpdatePanelToolBar)
                    Return True
                End If
            End If


            Dim Setup_Cod As String = "-1"

            If objParametriAgenda.Lav_Cod = LAVCOD_SEMINA Then
                Setup_Cod = "Seminato"
            Else
                Setup_Cod = "Trapiantato"
            End If

            'dati precedenti
            Dim App_Nome_OLD As String = GridViewImpianti.DataKeys(Ind).Item("App_Nome")
            Dim MetodoProduzione_OLD As Integer = GridViewImpianti.DataKeys(Ind).Item("MetodoProduzione")
            Dim Distinta_OLD As String = GridViewImpianti.DataKeys(Ind).Item("Distinta")
            Dim Cul_Cod_OLD As Integer = GridViewImpianti.DataKeys(Ind).Item("Cul_Cod")
            Dim Grfi_Cod_OLD As Integer = GridViewImpianti.DataKeys(Ind).Item("Grfi_Cod")
            Dim DisciplinareConPubPri_OLD As String = GridViewImpianti.DataKeys(Ind).Item("Disciplinare")
            Dim Dpi_Cod_OLD As String = Split(DisciplinareConPubPri_OLD, "/")(0)
            Dim DpiPP_OLD As String = Split(DisciplinareConPubPri_OLD, "/")(1)
            Dim Regolamento_Cod_OLD As Integer = GridViewImpianti.DataKeys(Ind).Item("Regolamento")

            Dim Tra_Fila_OLD As String = GridViewImpianti.DataKeys(Ind).Item("Tra_Fila")
            Dim Su_Fila_OLD As String = GridViewImpianti.DataKeys(Ind).Item("Su_Fila")
            Dim Interbina_OLD As String = GridViewImpianti.DataKeys(Ind).Item("Interbina")
            Dim Germinabilita_OLD As String = GridViewImpianti.DataKeys(Ind).Item("Germinabilita")
            Dim Sup_App As Decimal = GridViewImpianti.DataKeys(Ind).Item("Sup_App")
            Dim P_Ha_OLD As String = GridViewImpianti.DataKeys(Ind).Item("P_Ha")

            'dati nuovi
            Dim App_Nome_New As String = Get_App_Nome(Ind)
            Dim MetodoProduzione_New As Integer = Get_MetodoProduttivo(Ind)
            Dim Distinta_New As String = Get_Distinta(Ind)
            Dim Veg_Cod_New As String = Get_Specie(Ind)
            Dim Cul_Cod_New As String = Get_Varieta(Ind)
            Dim Grfi_Cod_New As String = Get_Finalita(Ind)
            Dim DisciplinareConPubPri_NEW As String = Get_Disciplinare(Ind)
            Dim Dpi_Cod_New As Integer = 0
            Dim DpiPP_New As Integer = 0
            Dim Sup_App_New As Decimal = 0
            If ModificaSup = True Then
                Sup_App_New = Get_Superficie(Ind)
            End If

            If CInt(DisciplinareConPubPri_NEW.Split("/")(0)) = 0 Then
                Dpi_Cod_New = 0
                DpiPP_New = 0
            Else
                Dpi_Cod_New = CInt(DisciplinareConPubPri_NEW.Split("/")(0))
                DpiPP_New = CInt(DisciplinareConPubPri_NEW.Split("/")(1))
            End If

            Dim Tra_Fila_New As String = Get_Tra_Fila(Ind)
            Dim Su_Fila_New As String = Get_Su_Fila(Ind)
            Dim Interbina_New As String = Get_Interbina(Ind)
            Dim Germinabilita_New As String = Get_Germinabilita(Ind)
            Dim Udm_Cod_New As String = Get_Udm(Ind)
            Dim Qta_New As String = Get_Qta(Ind)
            Dim P_Ha_New As Integer = Get_P_Ha(Ind)

            Dim objApp As New AgronicaCoreAnagrafeDAL.Appezzamento_Write
            Dim objAppCod As New AgronicaCoreAnagrafeDAL.Appezzamento_Codici_W
            Dim objAppezzaxParticelle As New AgronicaCoreAnagrafeDAL.AppezzaxParticelle_W
            Dim objAppezzaxParticellexMacrousi As New AgronicaCoreAnagrafeDAL.AppezzaxParticellexMacrousi_W
            Dim objAppezzaxParticellexMacrousixUtilizzi As New AgronicaCoreAnagrafeDAL.AppezzaxParticellexMacrousixUtilizzo_W
            Dim objImpianto As New AgronicaCoreAnagrafeDAL.Reg_Impianti_Write
            Dim objImpiantoCod As New AgronicaCoreAnagrafeDAL.Reg_Impianti_Codici_W
            Dim objImpiantoPro As New AgronicaCoreAnagrafeDAL.Impresa_Progetti_W

            'MODIFICA SUP_IMP / sup_App /sup appxpart
            'in caso di semina con frazionamento per lotto
            If ModificaSup = True Then
                If Sup_App <> Sup_App_New Then
                    objImpianto.Modifica_Parametrizzata(CStr(GridViewImpianti.DataKeys(Ind).Item("Piva")),
                                                  CInt(GridViewImpianti.DataKeys(Ind).Item("Sa_Cod")),
                                                  CInt(GridViewImpianti.DataKeys(Ind).Item("Appezza")),
                                                  CInt(GridViewImpianti.DataKeys(Ind).Item("Id_Reg")),
                                                  "sup_imp",
                                                  Sup_App_New,
                                                  "", objParametri_Server)

                    objApp.Modifica_Parametrizzata(CStr(GridViewImpianti.DataKeys(Ind).Item("Piva")),
                                                  CInt(GridViewImpianti.DataKeys(Ind).Item("Sa_Cod")),
                                                  CInt(GridViewImpianti.DataKeys(Ind).Item("Appezza")),
                                                  "sup_app",
                                                  Sup_App_New,
                                                  "", objParametri_Server)

                    Dim StrCatasto As String = CStr(GridViewImpianti.DataKeys(Ind).Item("Catasto"))

                    If Not IsNothing(Split(StrCatasto, "<br>")) Then

                        Select Case Split(StrCatasto, "<br>").Length

                            Case 1 'aggiorno
                                objAppezzaxParticelle.AggiornaSup(CStr(GridViewImpianti.DataKeys(Ind).Item("Piva")),
                                                  CInt(GridViewImpianti.DataKeys(Ind).Item("Sa_Cod")),
                                                  CInt(GridViewImpianti.DataKeys(Ind).Item("Campo_Cod")),
                                                  CInt(GridViewImpianti.DataKeys(Ind).Item("Appezza")),
                                                    "", "", "", 0, 0, "",
                                                    Sup_App_New, MetodoProduzione_New,
                                                        "", objParametri_Server)
                                'elimino perchè incoerenti
                                objAppezzaxParticellexMacrousi.Cancella(CStr(GridViewImpianti.DataKeys(Ind).Item("Piva")),
                                                  CInt(GridViewImpianti.DataKeys(Ind).Item("Sa_Cod")),
                                                  CInt(GridViewImpianti.DataKeys(Ind).Item("Appezza")),
                                                    "", "", "", 0, 0, "", "",
                                                    "", objParametri_Server)
                                objAppezzaxParticellexMacrousixUtilizzi.Cancella(CStr(GridViewImpianti.DataKeys(Ind).Item("Piva")),
                                                  CInt(GridViewImpianti.DataKeys(Ind).Item("Sa_Cod")),
                                                  CInt(GridViewImpianti.DataKeys(Ind).Item("Appezza")),
                                                    "", "", "", 0, 0, "", "", "", "",
                                                    "", objParametri_Server)
                            Case Is > 1 'elimino
                                objAppezzaxParticelle.Cancella(CStr(GridViewImpianti.DataKeys(Ind).Item("Piva")),
                                                  CInt(GridViewImpianti.DataKeys(Ind).Item("Sa_Cod")),
                                                  CInt(GridViewImpianti.DataKeys(Ind).Item("Appezza")),
                                                    "", "", "", 0, 0, "",
                                                    "", objParametri_Server)
                                objAppezzaxParticellexMacrousi.Cancella(CStr(GridViewImpianti.DataKeys(Ind).Item("Piva")),
                                                  CInt(GridViewImpianti.DataKeys(Ind).Item("Sa_Cod")),
                                                  CInt(GridViewImpianti.DataKeys(Ind).Item("Appezza")),
                                                    "", "", "", 0, 0, "", "",
                                                    "", objParametri_Server)
                                objAppezzaxParticellexMacrousixUtilizzi.Cancella(CStr(GridViewImpianti.DataKeys(Ind).Item("Piva")),
                                                  CInt(GridViewImpianti.DataKeys(Ind).Item("Sa_Cod")),
                                                  CInt(GridViewImpianti.DataKeys(Ind).Item("Appezza")),
                                                    "", "", "", 0, 0, "", "", "", "",
                                                    "", objParametri_Server)
                        End Select

                    End If


                End If
            End If

            'MODIFICA APP NOME
            If App_Nome_OLD <> App_Nome_New Then
                objApp.Modifica_Parametrizzata(CStr(GridViewImpianti.DataKeys(Ind).Item("Piva")),
                                              CInt(GridViewImpianti.DataKeys(Ind).Item("Sa_Cod")),
                                              CInt(GridViewImpianti.DataKeys(Ind).Item("Appezza")),
                                               "app_nome",
                                                  App_Nome_New,
                                                  "", objParametri_Server)
            End If

            'MODIFICA APP metodo produzione + reg impianto
            If MetodoProduzione_OLD <> MetodoProduzione_New Then
                objAppCod.Cancella(CStr(GridViewImpianti.DataKeys(Ind).Item("Piva")),
                                  CInt(GridViewImpianti.DataKeys(Ind).Item("Sa_Cod")),
                                  CInt(GridViewImpianti.DataKeys(Ind).Item("Appezza")),
                                  enum_CodiciAnagrafe.MetodoDiProduzione,
                                  "", objParametri_Server)
                objAppCod.Scrivi(CStr(GridViewImpianti.DataKeys(Ind).Item("Piva")),
                                      CInt(GridViewImpianti.DataKeys(Ind).Item("Sa_Cod")),
                                      CInt(GridViewImpianti.DataKeys(Ind).Item("Appezza")),
                                      enum_CodiciAnagrafe.MetodoDiProduzione,
                                      CStr(MetodoProduzione_New),
                                      AGRODATAINIZIO, AGRODATAFINE, objParametri_Server)

                Select Case MetodoProduzione_New
                    Case MetodoProduzione_New = enum_MetodoProduzione.Biologico,
                                                enum_MetodoProduzione.InConversione
                        objImpiantoPro.ModificaSingolo_CampoNumerico(CStr(GridViewImpianti.DataKeys(Ind).Item("Piva")),
                              CInt(GridViewImpianti.DataKeys(Ind).Item("Sa_Cod")),
                              CInt(GridViewImpianti.DataKeys(Ind).Item("Appezza")),
                              CInt(GridViewImpianti.DataKeys(Ind).Item("Id_Reg")),
                              CInt(GridViewImpianti.DataKeys(Ind).Item("ProgettoCod")),
                              "Regolamento_Cod",
                              enum_Cod_Regolamento.Regolamento_bio,
                              "", objParametri_Server)
                    Case Else
                        objImpiantoPro.ModificaSingolo_CampoNumerico(CStr(GridViewImpianti.DataKeys(Ind).Item("Piva")),
                                      CInt(GridViewImpianti.DataKeys(Ind).Item("Sa_Cod")),
                                      CInt(GridViewImpianti.DataKeys(Ind).Item("Appezza")),
                                      CInt(GridViewImpianti.DataKeys(Ind).Item("Id_Reg")),
                                      CInt(GridViewImpianti.DataKeys(Ind).Item("ProgettoCod")),
                                      "Regolamento_Cod",
                                      enum_Cod_Regolamento.Regolamento_Nessuno,
                                      "", objParametri_Server)
                End Select
            End If

            'MODIFICA DISTINTA NOME
            If Distinta_OLD <> Distinta_New Then
                objImpiantoPro.Modifica_Lotto(CStr(GridViewImpianti.DataKeys(Ind).Item("Piva")),
                                              CInt(GridViewImpianti.DataKeys(Ind).Item("Sa_Cod")),
                                              CInt(GridViewImpianti.DataKeys(Ind).Item("Appezza")),
                                              CInt(GridViewImpianti.DataKeys(Ind).Item("Id_Reg")),
                                              objParametriAgenda.Data.ToShortDateString,
                                              Distinta_New,
                                              "", objParametri_Server)
            End If

            'MODIFICA VARIETA
            If Cul_Cod_OLD <> Cul_Cod_New Then
                objImpianto.ModificaSingolo_CampoNumerico(CStr(GridViewImpianti.DataKeys(Ind).Item("Piva")),
                                              CInt(GridViewImpianti.DataKeys(Ind).Item("Sa_Cod")),
                                              CInt(GridViewImpianti.DataKeys(Ind).Item("Appezza")),
                                              CInt(GridViewImpianti.DataKeys(Ind).Item("Id_Reg")),
                                              "Cul_Cod",
                                              Cul_Cod_New,
                                              "", objParametri_Server)


                ' VAnni: 13/4/2018: 'Le operazioni di semina che vanno a cambiare gli impianti (da terreno nudo a terreno con coltura) modificano un impianto con una specie differente, quindi si perde il collegamento con il GIS, deve cambiare il tipoEntita_Cod nella tabella GIS_Entita
                Dim entitaR As New AgronicaCoreGisDAL.GIS_Entita_R
                Dim EntitaW As New AgronicaCoreGisDAL.GIS_Entita_W


                Dim entitaCodDaModificare As DataTable =
                        entitaR.Leggi(objParametri_Server.PivaSuperUser, 0, 0,
                                      CStr(GridViewImpianti.DataKeys(Ind).Item("Piva")),
                                      CInt(GridViewImpianti.DataKeys(Ind).Item("Sa_Cod")), CInt(GridViewImpianti.DataKeys(Ind).Item("Appezza")),
                                      0,
                                      CInt(GridViewImpianti.DataKeys(Ind).Item("Id_Reg")),
                                      "", "", "-1", -1, -1, "-1",
                                      0, 0, 0, 0, "CF TEC", "", Nothing, enumSelezioneVariabile.Selezione_TabellaCompleta,
                                      " Entita.TipoEntita_cod in (19,20,21,22,23) ", "", objParametri_Server, objParametri_Utenti)

                Dim iEntitaCodDaModificare As Integer = 0
                If entitaCodDaModificare.Rows.Count > 0 Then
                    iEntitaCodDaModificare = entitaCodDaModificare(0)("entita_Cod")
                End If

                If iEntitaCodDaModificare <> 0 Then

                    Dim tipoEntitaCod As Integer =
                                entitaR.TipoEntita_Cod_Leggi_Dato_Cul_Cod(Cul_Cod_New, objParametri_Server)

                    EntitaW.Modifica(objParametri_Server.PivaSuperUser, iEntitaCodDaModificare, tipoEntitaCod, "", Nothing, Nothing, Nothing, Nothing,
                                     "0", "0", "-1", -1, -1, "-1", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, "", objParametri_Server)

                End If

            End If

            'MODIFICA FINALITA
            'se ho terreni nudi o se ho una coltura su cui non sono stati fatti trattamenti/diserbi (la finalità incide)
            Dim ModificaFinalita As Boolean = True
            If Grfi_Cod_OLD <> Grfi_Cod_New Then
                If Cul_Cod_OLD <> 0 Then
                    If id_agemnda_new <> "" Then
                        If ciSonoTrattamentiDiserbi(CStr(GridViewImpianti.DataKeys(Ind).Item("Piva")),
                                            CInt(GridViewImpianti.DataKeys(Ind).Item("Sa_Cod")),
                                            CInt(GridViewImpianti.DataKeys(Ind).Item("Appezza")),
                                            CInt(GridViewImpianti.DataKeys(Ind).Item("Id_Reg")), id_agemnda_new) = True Then
                            ModificaFinalita = False
                        End If
                    End If
                End If
                If ModificaFinalita = True Then
                    objImpianto.ModificaSingolo_CampoNumerico(CStr(GridViewImpianti.DataKeys(Ind).Item("Piva")),
                                                     CInt(GridViewImpianti.DataKeys(Ind).Item("Sa_Cod")),
                                                     CInt(GridViewImpianti.DataKeys(Ind).Item("Appezza")),
                                                     CInt(GridViewImpianti.DataKeys(Ind).Item("Id_Reg")),
                                                     "Grfi_Cod",
                                                     Grfi_Cod_New,
                                                     "", objParametri_Server)
                End If
            End If


            'ELIMINAZIONE EVENTUALI DESTINAZIONI USO
            If Cul_Cod_OLD = 0 Then

                Dim strFiltroCodici As String = ""
                For i = 3000 To 3999
                    If strFiltroCodici = "" Then
                        strFiltroCodici = " id_cod=" & i & " "
                    Else
                        strFiltroCodici = strFiltroCodici & " OR id_cod = " & i & " "
                    End If
                Next
                strFiltroCodici = " (" & strFiltroCodici & ") "

                objImpiantoCod.Cancella(CStr(GridViewImpianti.DataKeys(Ind).Item("Piva")),
                                                          CInt(GridViewImpianti.DataKeys(Ind).Item("Sa_Cod")),
                                                          CInt(GridViewImpianti.DataKeys(Ind).Item("Appezza")),
                                                          CInt(GridViewImpianti.DataKeys(Ind).Item("Id_Reg")),
                                                          0,
                                                          strFiltroCodici, objParametri_Server)
            End If


            'MODIFICA DPI
            'se ho terreni nudi o se ho una coltura su cui non sono stati fatti trattamenti/diserbi (la finalità incide)
            Dim ModificaDPI As Boolean = True
            If Dpi_Cod_OLD <> Dpi_Cod_New Or DpiPP_OLD <> DpiPP_New Then

                If Cul_Cod_OLD <> 0 Then
                    If ciSonoTrattamentiDiserbi(CStr(GridViewImpianti.DataKeys(Ind).Item("Piva")),
                                            CInt(GridViewImpianti.DataKeys(Ind).Item("Sa_Cod")),
                                            CInt(GridViewImpianti.DataKeys(Ind).Item("Appezza")),
                                            CInt(GridViewImpianti.DataKeys(Ind).Item("Id_Reg")), id_agemnda_new) = True Then
                        ModificaDPI = False
                    End If
                End If
                If ModificaDPI = True Then
                    objImpiantoPro.ModificaSingolo_CampoNumerico(CStr(GridViewImpianti.DataKeys(Ind).Item("Piva")),
                                                  CInt(GridViewImpianti.DataKeys(Ind).Item("Sa_Cod")),
                                                  CInt(GridViewImpianti.DataKeys(Ind).Item("Appezza")),
                                                  CInt(GridViewImpianti.DataKeys(Ind).Item("Id_Reg")),
                                                  CInt(GridViewImpianti.DataKeys(Ind).Item("ProgettoCod")),
                                                  "Disciplinare_Cod",
                                                  Dpi_Cod_New,
                                                  "", objParametri_Server)

                    objImpiantoPro.ModificaSingolo_CampoNumerico(CStr(GridViewImpianti.DataKeys(Ind).Item("Piva")),
                                                  CInt(GridViewImpianti.DataKeys(Ind).Item("Sa_Cod")),
                                                  CInt(GridViewImpianti.DataKeys(Ind).Item("Appezza")),
                                                  CInt(GridViewImpianti.DataKeys(Ind).Item("Id_Reg")),
                                                  CInt(GridViewImpianti.DataKeys(Ind).Item("ProgettoCod")),
                                                  "Disciplinare_PubblicoPrivato",
                                                  DpiPP_New,
                                                  "", objParametri_Server)
                End If


            End If

            'MODIFICA SESTO : CANCELLO E RISCRIVO
            If Tra_Fila_New <> "" AndAlso IsNumeric(Tra_Fila_New) Then
                If Tra_Fila_OLD <> Tra_Fila_New Then
                    objImpiantoCod.Cancella(CStr(GridViewImpianti.DataKeys(Ind).Item("Piva")),
                                                      CInt(GridViewImpianti.DataKeys(Ind).Item("Sa_Cod")),
                                                      CInt(GridViewImpianti.DataKeys(Ind).Item("Appezza")),
                                                      CInt(GridViewImpianti.DataKeys(Ind).Item("Id_Reg")),
                                                      enum_CodiciAnagrafe.Impianto_TraFila_Maschio,
                                                      "", objParametri_Server)
                    objImpiantoCod.Scrivi(CStr(GridViewImpianti.DataKeys(Ind).Item("Piva")),
                                          CInt(GridViewImpianti.DataKeys(Ind).Item("Sa_Cod")),
                                          CInt(GridViewImpianti.DataKeys(Ind).Item("Appezza")),
                                          CInt(GridViewImpianti.DataKeys(Ind).Item("Id_Reg")),
                                          enum_CodiciAnagrafe.Impianto_TraFila_Maschio,
                                          CStr(Tra_Fila_New),
                                          AGRODATAINIZIO, AGRODATAFINE, objParametri_Server)
                End If
            End If


            If Su_Fila_New <> "" AndAlso IsNumeric(Su_Fila_New) Then
                If Su_Fila_OLD <> Su_Fila_New Then
                    objImpiantoCod.Cancella(CStr(GridViewImpianti.DataKeys(Ind).Item("Piva")),
                                                      CInt(GridViewImpianti.DataKeys(Ind).Item("Sa_Cod")),
                                                      CInt(GridViewImpianti.DataKeys(Ind).Item("Appezza")),
                                                      CInt(GridViewImpianti.DataKeys(Ind).Item("Id_Reg")),
                                                      enum_CodiciAnagrafe.Impianto_SuFila_Maschio,
                                                      "", objParametri_Server)
                    objImpiantoCod.Scrivi(CStr(GridViewImpianti.DataKeys(Ind).Item("Piva")),
                                          CInt(GridViewImpianti.DataKeys(Ind).Item("Sa_Cod")),
                                          CInt(GridViewImpianti.DataKeys(Ind).Item("Appezza")),
                                          CInt(GridViewImpianti.DataKeys(Ind).Item("Id_Reg")),
                                          enum_CodiciAnagrafe.Impianto_SuFila_Maschio,
                                          CStr(Su_Fila_New),
                                          AGRODATAINIZIO, AGRODATAFINE, objParametri_Server)
                End If
            End If


            If Interbina_New <> "" AndAlso IsNumeric(Interbina_New) Then
                If Interbina_OLD <> Interbina_New Then
                    objImpiantoCod.Cancella(CStr(GridViewImpianti.DataKeys(Ind).Item("Piva")),
                                                      CInt(GridViewImpianti.DataKeys(Ind).Item("Sa_Cod")),
                                                      CInt(GridViewImpianti.DataKeys(Ind).Item("Appezza")),
                                                      CInt(GridViewImpianti.DataKeys(Ind).Item("Id_Reg")),
                                                      enum_CodiciAnagrafe.Impianto_Interbina,
                                                      "", objParametri_Server)
                    objImpiantoCod.Scrivi(CStr(GridViewImpianti.DataKeys(Ind).Item("Piva")),
                                          CInt(GridViewImpianti.DataKeys(Ind).Item("Sa_Cod")),
                                          CInt(GridViewImpianti.DataKeys(Ind).Item("Appezza")),
                                          CInt(GridViewImpianti.DataKeys(Ind).Item("Id_Reg")),
                                          enum_CodiciAnagrafe.Impianto_Interbina,
                                          CStr(Interbina_New),
                                          AGRODATAINIZIO, AGRODATAFINE, objParametri_Server)
                End If
            End If


            If Germinabilita_New <> "" AndAlso IsNumeric(Germinabilita_New) Then
                If Germinabilita_OLD <> Germinabilita_New Then
                    objImpiantoCod.Cancella(CStr(GridViewImpianti.DataKeys(Ind).Item("Piva")),
                                                      CInt(GridViewImpianti.DataKeys(Ind).Item("Sa_Cod")),
                                                      CInt(GridViewImpianti.DataKeys(Ind).Item("Appezza")),
                                                      CInt(GridViewImpianti.DataKeys(Ind).Item("Id_Reg")),
                                                      enum_CodiciAnagrafe.Impianto_Germinabilita,
                                                      "", objParametri_Server)
                    objImpiantoCod.Scrivi(CStr(GridViewImpianti.DataKeys(Ind).Item("Piva")),
                                          CInt(GridViewImpianti.DataKeys(Ind).Item("Sa_Cod")),
                                          CInt(GridViewImpianti.DataKeys(Ind).Item("Appezza")),
                                          CInt(GridViewImpianti.DataKeys(Ind).Item("Id_Reg")),
                                          enum_CodiciAnagrafe.Impianto_Germinabilita,
                                          CStr(Germinabilita_New),
                                          AGRODATAINIZIO, AGRODATAFINE, objParametri_Server)
                End If
            End If

            'Se l'UDM è N.Piante modifico anche il Numero piante sulla distinta
            If Udm_Cod_New <> "" AndAlso IsNumeric(Udm_Cod_New) AndAlso Qta_New <> "" AndAlso IsNumeric(Qta_New) Then
                Select Case CInt(Udm_Cod_New)
                    Case enum_UnitaMisura.Num_Piante
                        'P_Ha_New = Int(CDbl(Qta_New) / Sup_App)
                        If P_Ha_OLD <> P_Ha_New Then
                            objImpiantoPro.ModificaSingolo_CampoNumerico(CStr(GridViewImpianti.DataKeys(Ind).Item("Piva")),
                                                                          CInt(GridViewImpianti.DataKeys(Ind).Item("Sa_Cod")),
                                                                          CInt(GridViewImpianti.DataKeys(Ind).Item("Appezza")),
                                                                          CInt(GridViewImpianti.DataKeys(Ind).Item("Id_Reg")),
                                                                          CInt(GridViewImpianti.DataKeys(Ind).Item("ProgettoCod")),
                                                                          "P_HA",
                                                                          P_Ha_New,
                                                                          "", objParametri_Server)
                        End If
                End Select
            End If



        Catch ex As Exception

            '------------------------------------------------
            'Si e' verificata una eccezione !!!!!!
            '------------------------------------------------

            'Messaggio di errore
            Dim StrDummy As String = ex.Message.ToString()

            'Se la transazione ha avuto esito negativo allora ...
            Messaggi.AgroMsgBox(Resources.AgronicaAgenda_2010.SiÈVerificatoUnErroreDuranteLaFaseDiSalvat & vbCrLf & StrDummy, Page, , Master_Operazione.Property_UpdatePanelToolBar)

            Log_Errori &= StrDummy

            Return False

        End Try

        '============================
        '===  Fine Aggiornamento  ===
        '============================

        Return True

    End Function


    Private Function SalvaModificheImpianti(ByRef Log_Errori As String, ByVal id_agemnda_new As String) As Boolean

        For i = 0 To GridViewImpianti.Rows.Count - 1

            'Se la riga e' selezionata ...
            If CType(GridViewImpianti.Rows(i).Cells(NumColonna_Check).Controls(1), CheckBox).Checked = True AndAlso GridViewImpianti.Rows(i).Cells(NumColonna_Check).Controls(1).Visible Then

                If Not Modifica_Impianto(i, Log_Errori, id_agemnda_new) Then
                    Return False
                End If

            End If
        Next
        Return True
    End Function

    Private Sub caricaListeValoriDaTabelle()
        ListaValoriSeminaImpianti = New List(Of ValoriImpiantoESemineImpostati)

        'da usare localmente
        Dim ListaValoriSeminaImpiantiMagazzino As New List(Of ValoriImpiantoESemineImpostati)

        For i = 0 To GridViewMagazzino.Rows.Count - 1
            If Not IsNothing(GridViewMagazzino) AndAlso
                CType(GridViewMagazzino.Rows(i).Cells(NumColonna_Check).Controls(1), CheckBox).Checked = True Then

                Dim ListaValoriMagazzino As New ValoriImpiantoESemineImpostati
                ListaValoriMagazzino.Mat_Cod = GridViewMagazzino.DataKeys(i).Item("Mat_Cod")
                ListaValoriMagazzino.UDM_Cod = GridViewMagazzino.DataKeys(i).Item("Udm_Cod")
                ListaValoriMagazzino.Lotto = GridViewMagazzino.DataKeys(i).Item("Lotto")
                ListaValoriMagazzino.Bolla = GridViewMagazzino.DataKeys(i).Item("Bolla")
                ListaValoriSeminaImpiantiMagazzino.Add(ListaValoriMagazzino)

            End If
        Next

        Dim xGridViewLeggere As GridView

        If objParametriAgenda.TargetOperazione = enum_Tipo_Operazione_Agenda_Target.Reale Then
            xGridViewLeggere = GridViewImpianti
        Else
            xGridViewLeggere = GridViewPlanning
        End If

        For i = 0 To xGridViewLeggere.Rows.Count - 1
            If Not IsNothing(xGridViewLeggere) AndAlso
                CType(xGridViewLeggere.Rows(i).Cells(NumColonna_Check).Controls(1), CheckBox).Checked = True AndAlso
                xGridViewLeggere.Rows(i).Cells(NumColonna_Check).Controls(1).Visible Then

                Dim Piva As String = xGridViewLeggere.DataKeys(i).Item("Piva")
                Dim Sa_Cod As Integer = xGridViewLeggere.DataKeys(i).Item("Sa_Cod")
                Dim Appezza As Integer = xGridViewLeggere.DataKeys(i).Item("Appezza")
                Dim Id_Reg As Integer = xGridViewLeggere.DataKeys(i).Item("Id_Reg")

                Dim Programmazione_Entita_cod As Integer = 0
                If objParametriAgenda.TargetOperazione = enum_Tipo_Operazione_Agenda_Target.Planning Then
                    Programmazione_Entita_cod = xGridViewLeggere.DataKeys(i).Item("Programmazione_Entita_Cod")
                End If

                'Dim App_Nome As String
                'Dim Distinta As String
                Dim Tra_Fila As String
                Dim Su_Fila As String
                Dim Interbina As String
                Dim Germinabilita As String
                Dim Specie As String
                Dim Varieta As String
                Dim Finalita As String
                Dim Disciplinare As String

                'App_Nome = Get_App_Nome(i)
                'Distinta = Get_Distinta(i)
                Tra_Fila = Get_Tra_Fila(i)
                Su_Fila = Get_Su_Fila(i)
                Interbina = Get_Interbina(i)
                Germinabilita = Get_Germinabilita(i)
                Specie = Get_Specie(i)
                Varieta = Get_Varieta(i)
                Finalita = Get_Finalita(i)
                Disciplinare = Get_Disciplinare(i)

                For j = i To xGridViewLeggere.Rows.Count - 1

                    Dim ListaValori As New ValoriImpiantoESemineImpostati

                    ListaValori.Piva = Piva
                    ListaValori.Sa_Cod = Sa_Cod
                    ListaValori.Appezza = Appezza
                    ListaValori.Id_Reg = Id_Reg
                    ListaValori.Programmazione_Entita_cod = Programmazione_Entita_cod

                    ListaValori.Tra_Fila = Tra_Fila
                    ListaValori.Su_Fila = Su_Fila
                    ListaValori.Interbina = Interbina
                    ListaValori.Germinabilita = Germinabilita
                    ListaValori.Specie = Specie
                    ListaValori.Varieta = Varieta
                    ListaValori.Finalita = Finalita
                    ListaValori.Disciplinare = Disciplinare

                    ListaValori.QTA_Dest = Get_Qta(j)

                    ListaValori.Elem_Cod = xGridViewLeggere.DataKeys(j).Item("Elem_Cod_Lotto")

                    If objParametriAgenda.Fabbricato <> "0" Then
                        ListaValori.Pro_Cod = xGridViewLeggere.DataKeys(j).Item("Pro_Cod_Lotto")
                    Else
                        ListaValori.Pro_Cod = xGridViewLeggere.DataKeys(j).Item("Sem_Cod_Lotto")
                    End If


                    ListaValori.Lotto = xGridViewLeggere.DataKeys(j).Item("Lotto_Lotto")
                    ListaValori.Bolla = xGridViewLeggere.DataKeys(j).Item("Bolla_Lotto")
                    ListaValori.Mat_Cod = xGridViewLeggere.DataKeys(j).Item("Mat_Cod_Lotto")

                    ListaValori.UDM_Cod = Get_Udm(j)

                    'aggiuingo solo se ho il lotto impostato nella riga, altrimenti la ignoro , quyindi si schecca
                    If Not (ListaValori.Pro_Cod = 0 And ListaValori.Mat_Cod = 0) Then
                        'se sono con magazzino aggiungo solo se ilk lotto è tra quelli del magazzino checcati salvati ListaValoriSeminaImpiantiMagazzino
                        If objParametriAgenda.Fabbricato <> "0" Then

                            For x = 0 To ListaValoriSeminaImpiantiMagazzino.Count - 1
                                If ListaValoriSeminaImpiantiMagazzino(x).Mat_Cod = ListaValori.Mat_Cod AndAlso
                                    ListaValoriSeminaImpiantiMagazzino(x).UDM_Cod = ListaValori.UDM_Cod AndAlso
                                    ListaValoriSeminaImpiantiMagazzino(x).Lotto = ListaValori.Lotto AndAlso
                                    ListaValoriSeminaImpiantiMagazzino(x).Bolla = ListaValori.Bolla Then
                                    ListaValoriSeminaImpianti.Add(ListaValori)
                                    Exit For
                                End If
                            Next


                        Else

                            ListaValoriSeminaImpianti.Add(ListaValori)

                        End If


                    End If


                    i = j 'così all'uscita risparmio dei cicli
                    If j + 1 = xGridViewLeggere.Rows.Count Then
                        'la prossima riga è l'ultima, controlo qui per non fare eccezione nell'if successivo
                        Exit For
                    End If
                    If xGridViewLeggere.Rows(j + 1).Cells(NumColonna_Check).Controls(1).Visible = True Then
                        'se la prossima riga si riferisce ad un nuovo impianto esco dal for
                        Exit For
                    Else
                        'la prossima riga o è l'ultima o si riferisce allo stesso impianto,
                        'continuo e all'inizio del ciclo inserisco la nuova riga
                    End If

                Next


            End If
        Next

        'se non ho impianti checcati devo comunque memorizzare i check della tabella magazzino, lo faccio copinado la struttura del magazzino
        If ListaValoriSeminaImpianti.Count = 0 Then
            ListaValoriSeminaImpianti = ListaValoriSeminaImpiantiMagazzino
        End If

        'aggiungo poi i lotti chekkati nell agrid magazzino che non sono presenti nell'impianto,
        'questo si verifica quando seleziono un nuovo lotto, devo memorizzarlo nella lista in modo che la tabella generi la nuova riga.
        'li aggiungo tutti, tanto non sono molti, non sto a vedere quelli presenti nella lista valori impianti , tanto sa gestirli
        For x = 0 To ListaValoriSeminaImpiantiMagazzino.Count - 1
            ListaValoriSeminaImpianti.Add(ListaValoriSeminaImpiantiMagazzino(x))
            'For y = 0 To ListaValoriSeminaImpianti.Count - 1

            'Next
        Next

        'a questo punto lella ListaValoriSeminaImpianti ho la somma delle righe checcate del magazzino e dell'impianto
    End Sub



#Region "Gestione ColonneSemine Impianti"

    Private Sub CreaImpostazioniColonneSemine()
        'creo
        ListaColonneSemineVisibili.Items.Clear()
        'ListaColonneSemineVisibili.Items.Add(New ListItem("Centro Az."))
        'ListaColonneSemineVisibili.Items.Add(New ListItem("Appezz."))
        'ListaColonneSemineVisibili.Items.Add(New ListItem("Distinta"))
        ListaColonneSemineVisibili.Items.Add(New ListItem(Resources.AgronicaAgenda_2010.Campo))
        ListaColonneSemineVisibili.Items.Add(New ListItem("Metodo Produzione"))
        ListaColonneSemineVisibili.Items.Add(New ListItem("Lotto Impianto"))
        'ListaColonneSemineVisibili.Items.Add(New ListItem("Varietà"))
        'ListaColonneSemineVisibili.Items.Add(New ListItem("Finalità"))
        ListaColonneSemineVisibili.Items.Add(New ListItem(Resources.AgronicaAgenda_2010.Disciplinare))
        'ListaColonneSemineVisibili.Items.Add(New ListItem("Sup. [Ha]"))
        ''ListaColonneSemineVisibili.Items.Add(New ListItem("Dist. tra Fila [m]"))
        ''ListaColonneSemineVisibili.Items.Add(New ListItem("Dist. su Fila [m]"))
        ''ListaColonneSemineVisibili.Items.Add(New ListItem("Interb. [m]"))
        ''ListaColonneSemineVisibili.Items.Add(New ListItem("Germin. [%]"))
        ''ListaColonneSemineVisibili.Items.Add(New ListItem("P / Ha"))
        ListaColonneSemineVisibili.Items.Add(New ListItem("Colonne dati Distribuzione Piante"))
        'ListaColonneSemineVisibili.Items.Add(New ListItem("Descrizione Giacenza"))
        ''ListaColonneSemineVisibili.Items.Add(New ListItem("Unità di Misura"))
        ''ListaColonneSemineVisibili.Items.Add(New ListItem("Qta / Impianto"))
        ListaColonneSemineVisibili.Items.Add(New ListItem("Quantità ed Unità di Misura"))
        ListaColonneSemineVisibili.Items.Add(New ListItem(Resources.AgronicaAgenda_2010.Catasto))

    End Sub

    Private Sub ImpostazioniColonneSemine()

        Select Case RBL_Tipo_Semina.SelectedValue 'Tipo_Semina

            Case enum_SEMINA_TIPO.Frazionamento_Appezzamenti_perLotto_e_Semina_sui_Singoli_Default

                For j = 0 To ListaColonneSemineVisibili.Items.Count - 1
                    ListaColonneSemineVisibili.Items(j).Selected = True
                Next

            Case Else

                'carico le impostazioni utente
                Dim objUtenti As New AgronicaCoreUtentiDAL.Utenti_Impostazioni_Read
                Dim Dt As DataTable
                Dt = objUtenti.Leggi_Utente_Poi_SuperUser(enum_Impostazioni_Utenti.UTENTE_COD_ColonneVisibili_TabellaImpianti_Semina,
                         1,
                         AgronicaCoreParametri.enumSelezioneVariabile.Selezione_TabellaCompleta,
                         "",
                         "",
                         objParametri_Utenti)


                Dim i As Integer
                Dim str As String = ""

                If Dt.Rows.Count > 0 Then
                    str = Dt.Rows(0).Item("Impostazione_Valore_1")
                    For i = 0 To str.Split("|").Length - 1
                        Dim valore As String = str.Split("|")(i)
                        Dim j = 0
                        For j = 0 To ListaColonneSemineVisibili.Items.Count - 1
                            If ListaColonneSemineVisibili.Items(j).Text = valore Then
                                ListaColonneSemineVisibili.Items(j).Selected = True
                            End If
                        Next
                    Next
                End If

        End Select

        Session("ListaColonneSemineVisibili") = ListaColonneSemineVisibili

        AggiornaVisibilitaColonneSemineImpianti()

    End Sub

    Private Sub AggiornaVisibilitaColonneSeminePlanning()

    End Sub


    Private Sub AggiornaVisibilitaColonneSemineImpianti()

        If Not IsNothing(Session("ListaColonneSemineVisibili")) Then
            ListaColonneSemineVisibili = Session("ListaColonneSemineVisibili")
        End If

        Dim j As Integer

        For j = 0 To ListaColonneSemineVisibili.Items.Count - 1

            Select Case ListaColonneSemineVisibili.Items(j).Text
                Case "Campo"
                    GridViewImpianti.Columns(NumColonna_Campo_Des).Visible = ListaColonneSemineVisibili.Items(j).Selected
                Case "Metodo Produzione"
                    GridViewImpianti.Columns(NumColonna_MetodoProduttivo).Visible = ListaColonneSemineVisibili.Items(j).Selected
                Case "Lotto Impianto" '"Distinta"
                    GridViewImpianti.Columns(NumColonna_Distinta).Visible = ListaColonneSemineVisibili.Items(j).Selected
                Case "Disciplinare" '8
                    GridViewImpianti.Columns(NumColonna_Disciplinare).Visible = ListaColonneSemineVisibili.Items(j).Selected
                Case "Colonne dati Distribuzione Piante" '10,11,12,14,15
                    GridViewImpianti.Columns(NumColonna_DistTraFila).Visible = ListaColonneSemineVisibili.Items(j).Selected
                    GridViewImpianti.Columns(NumColonna_DistSuFila).Visible = ListaColonneSemineVisibili.Items(j).Selected
                    GridViewImpianti.Columns(NumColonna_Germinabilita).Visible = ListaColonneSemineVisibili.Items(j).Selected
                    GridViewImpianti.Columns(NumColonna_Interbina).Visible = ListaColonneSemineVisibili.Items(j).Selected
                    GridViewImpianti.Columns(NumColonna_p_ha).Visible = ListaColonneSemineVisibili.Items(j).Selected
                Case "Quantità ed Unità di Misura" '17,18
                    If objParametriAgenda.Fabbricato <> "0" Then

                        'se ho il magazzino faccio sempre vedere i dati sulla semina
                        GridViewImpianti.Columns(NumColonna_UdmSimLotto).Visible = True


                        'ma devo fare vedere la colonna che mostra udm_sim_lotto
                        'e nascondere quella dell'unità di misura da scegliere tramite
                        'dropdownlist, infatti con il magazzino l'udm è fissa e dipende dal lotto quindi è inutile la dropdownlist

                        'nascondo sempre la colonna udm con dropdownlist
                        GridViewImpianti.Columns(NumColonna_UnitaMisura).HeaderStyle.CssClass = "displaynone"
                        GridViewImpianti.Columns(NumColonna_UnitaMisura).ItemStyle.CssClass = "displaynone"

                        'faccio sempre vedere la qta
                        GridViewImpianti.Columns(NumColonna_PianteImpianto).HeaderStyle.CssClass = ""
                        GridViewImpianti.Columns(NumColonna_PianteImpianto).ItemStyle.CssClass = ""

                    Else

                        If ListaColonneSemineVisibili.Items(j).Selected Then
                            'tolgo la classe così visualizza le colonna NumColonna_UnitaMisura e NumColonna_PianteImpianto
                            GridViewImpianti.Columns(NumColonna_UnitaMisura).HeaderStyle.CssClass = ""
                            GridViewImpianti.Columns(NumColonna_UnitaMisura).ItemStyle.CssClass = ""
                            GridViewImpianti.Columns(NumColonna_PianteImpianto).HeaderStyle.CssClass = ""
                            GridViewImpianti.Columns(NumColonna_PianteImpianto).ItemStyle.CssClass = ""
                        Else
                            'uso classe displaynone perchè così mi rimane il dato nella pagina anche se non è visibile
                            'e posso recuperarlo, altrimenti cancellerei in modifica di un'operazione il dato 
                            'se era stato salvato son le colonne visibili ed ora le ho invece non visibili
                            GridViewImpianti.Columns(NumColonna_UnitaMisura).HeaderStyle.CssClass = "displaynone"
                            GridViewImpianti.Columns(NumColonna_UnitaMisura).ItemStyle.CssClass = "displaynone"
                            GridViewImpianti.Columns(NumColonna_PianteImpianto).HeaderStyle.CssClass = "displaynone"
                            GridViewImpianti.Columns(NumColonna_PianteImpianto).ItemStyle.CssClass = "displaynone"
                        End If


                        'nascondo sempre la colonna UdmSimLotto
                        GridViewImpianti.Columns(NumColonna_UdmSimLotto).Visible = False
                    End If

                Case "Catasto"
                    GridViewImpianti.Columns(NumColonna_Catasto).Visible = ListaColonneSemineVisibili.Items(j).Selected

            End Select

        Next


        GridViewImpianti.Columns(NumColonna_App_Nome).HeaderStyle.CssClass = ""
        GridViewImpianti.Columns(NumColonna_App_Nome).ItemStyle.CssClass = ""
        GridViewImpianti.Columns(NumColonna_Distinta).HeaderStyle.CssClass = ""
        GridViewImpianti.Columns(NumColonna_Distinta).ItemStyle.CssClass = ""
        GridViewImpianti.Columns(NumColonna_Varietà).HeaderStyle.CssClass = ""
        GridViewImpianti.Columns(NumColonna_Varietà).ItemStyle.CssClass = ""
        GridViewImpianti.Columns(NumColonna_Finalità).HeaderStyle.CssClass = ""
        GridViewImpianti.Columns(NumColonna_Finalità).ItemStyle.CssClass = ""
        GridViewImpianti.Columns(NumColonna_Disciplinare).HeaderStyle.CssClass = ""
        GridViewImpianti.Columns(NumColonna_Disciplinare).ItemStyle.CssClass = ""
        GridViewImpianti.Columns(NumColonna_MetodoProduttivo).HeaderStyle.CssClass = ""
        GridViewImpianti.Columns(NumColonna_MetodoProduttivo).ItemStyle.CssClass = ""

        'GridViewImpianti.Columns(NumColonna_Specie).HeaderStyle.CssClass = "displaynone"
        'GridViewImpianti.Columns(NumColonna_Specie).ItemStyle.CssClass = "displaynone"
        GridViewImpianti.Columns(NumColonna__Sup_Semina).HeaderStyle.CssClass = "displaynone"
        GridViewImpianti.Columns(NumColonna__Sup_Semina).ItemStyle.CssClass = "displaynone"

        Select Case RBL_Tipo_Semina.SelectedValue 'Tipo_Semina

            Case enum_SEMINA_TIPO.Solo_Semina_Default
                GridViewImpianti.Columns(NumColonna_App_Nome).HeaderStyle.CssClass = "disabled"
                GridViewImpianti.Columns(NumColonna_App_Nome).ItemStyle.CssClass = "disabled"
                GridViewImpianti.Columns(NumColonna_Distinta).HeaderStyle.CssClass = "disabled"
                GridViewImpianti.Columns(NumColonna_Distinta).ItemStyle.CssClass = "disabled"
                GridViewImpianti.Columns(NumColonna_Varietà).HeaderStyle.CssClass = "disabled"
                GridViewImpianti.Columns(NumColonna_Varietà).ItemStyle.CssClass = "disabled"
                GridViewImpianti.Columns(NumColonna_Finalità).HeaderStyle.CssClass = "disabled"
                GridViewImpianti.Columns(NumColonna_Finalità).ItemStyle.CssClass = "disabled"
                GridViewImpianti.Columns(NumColonna_Disciplinare).HeaderStyle.CssClass = "disabled"
                GridViewImpianti.Columns(NumColonna_Disciplinare).ItemStyle.CssClass = "disabled"
                GridViewImpianti.Columns(NumColonna_MetodoProduttivo).HeaderStyle.CssClass = "disabled"
                GridViewImpianti.Columns(NumColonna_MetodoProduttivo).ItemStyle.CssClass = "disabled"

            Case enum_SEMINA_TIPO.Frazionamento_Appezzamenti_perLotto_e_Semina_sui_Singoli_Default
                'GridViewImpianti.Columns(NumColonna_Specie).HeaderStyle.CssClass = ""
                'GridViewImpianti.Columns(NumColonna_Specie).ItemStyle.CssClass = ""
                GridViewImpianti.Columns(NumColonna__Sup_Semina).HeaderStyle.CssClass = ""
                GridViewImpianti.Columns(NumColonna__Sup_Semina).ItemStyle.CssClass = ""

            Case Else


        End Select

    End Sub

    Protected Sub SalvaImpostazioniColonneSemine_Click(ByVal sender As Object, ByVal e As EventArgs) Handles SalvaImpostazioniColonneSemine.Click

        Dim str As String = ""
        Dim i As Integer
        For i = 0 To ListaColonneSemineVisibili.Items.Count - 1
            If ListaColonneSemineVisibili.Items(i).Selected = True Then
                str = str + ListaColonneSemineVisibili.Items(i).Text & "|"
            End If
        Next


        Dim Flag_Connessione, Flag_Transazione As Boolean

        Try

            Utility.VerificaApriTransazione(objParametri_Utenti,
                                         Flag_Connessione,
                                         Flag_Transazione)




            Dim objImpostazioniUtente As New AgronicaCoreUtentiDAL.Utenti_Impostazioni_W
            'prima cancello le vechie impostazioni
            objImpostazioniUtente.Cancella(enum_Impostazioni_Utenti.UTENTE_COD_ColonneVisibili_TabellaImpianti_Semina,
                                            "", objParametri_Utenti)

            If str.Length > 0 Then
                str = str.Substring(0, str.Length - 1)
                'salvo
                objImpostazioniUtente.Scrivi(enum_Impostazioni_Utenti.UTENTE_COD_ColonneVisibili_TabellaImpianti_Semina,
                                             str, "", "", "", AGRODATAINIZIO, AGRODATAFINE, objParametri_Utenti)
            End If



            Utility.VerificaChiudiTransazione(objParametri_Utenti,
                                  Flag_Transazione)

        Catch ex As Exception

            Utility.VerificaAnnullaTransazione(objParametri_Utenti,
                                               Flag_Transazione)
        Finally

            Utility.VerificaChiudiConnessione(objParametri_Utenti,
                                                   Flag_Connessione)

        End Try

        Session("ListaColonneSemineVisibili") = ListaColonneSemineVisibili

        AggiornaVisibilitaColonneSemineImpianti()

        caricaListeValoriDaTabelle()
        caricaTabelle(False)

    End Sub

    Protected Sub AggiornaGrigliaImpiantiSemine_Click(sender As Object, e As EventArgs) Handles AggiornaGrigliaImpiantiSemine.Click
        caricaListeValoriDaTabelle()
        caricaTabelle(False)
    End Sub


#End Region

#Region "Set e Get delle textbox e udm In base alla visibilità colonne"
    Private Function Get_Distinta(Ind As Integer) As String
        If GridViewImpianti.Columns(NumColonna_Distinta).Visible Then
            Return CType(GridViewImpianti.Rows(Ind).Cells(NumColonna_Distinta).Controls(1), TextBox).Text
        Else
            Return GridViewImpianti.DataKeys(Ind).Item("Distinta")
        End If
    End Function

    Private Function Get_App_Nome(Ind As Integer) As String
        If GridViewImpianti.Columns(NumColonna_App_Nome).Visible Then
            Return CType(GridViewImpianti.Rows(Ind).Cells(NumColonna_App_Nome).Controls(1), TextBox).Text
        Else
            Return GridViewImpianti.DataKeys(Ind).Item("App_Nome")
        End If
    End Function

    Private Function Get_Tra_Fila(Ind As Integer) As String

        Dim nCol_Tra_Fila As Integer
        Dim xGridView As GridView

        If objParametriAgenda.TargetOperazione = enum_Tipo_Operazione_Agenda_Target.Reale Then
            nCol_Tra_Fila = NumColonna_DistTraFila
            xGridView = GridViewImpianti
        Else
            nCol_Tra_Fila = NumColonna_Planning_DistTraFila
            xGridView = GridViewPlanning
        End If

        If xGridView.Columns(nCol_Tra_Fila).Visible Then
            Return CType(xGridView.Rows(Ind).Cells(nCol_Tra_Fila).Controls(1), TextBox).Text
        Else
            Return xGridView.DataKeys(Ind).Item("Tra_Fila")
        End If
    End Function

    Private Function Get_Su_Fila(Ind As Integer) As String

        Dim nCol_Su_Fila As Integer
        Dim xGridView As GridView

        If objParametriAgenda.TargetOperazione = enum_Tipo_Operazione_Agenda_Target.Reale Then
            nCol_Su_Fila = NumColonna_DistSuFila
            xGridView = GridViewImpianti
        Else
            nCol_Su_Fila = NumColonna_Planning_DistSuFila
            xGridView = GridViewPlanning
        End If

        If xGridView.Columns(nCol_Su_Fila).Visible Then
            Return CType(xGridView.Rows(Ind).Cells(nCol_Su_Fila).Controls(1), TextBox).Text
        Else
            Return xGridView.DataKeys(Ind).Item("Su_Fila")
        End If
    End Function

    Private Function Get_Interbina(Ind As Integer) As String

        Dim nCol_Interbina As Integer
        Dim xGridView As GridView

        If objParametriAgenda.TargetOperazione = enum_Tipo_Operazione_Agenda_Target.Reale Then
            nCol_Interbina = NumColonna_Interbina
            xGridView = GridViewImpianti
        Else
            nCol_Interbina = NumColonna_Planning_Interbina
            xGridView = GridViewPlanning
        End If


        If xGridView.Columns(NumColonna_Interbina).Visible Then
            Return CType(xGridView.Rows(Ind).Cells(nCol_Interbina).Controls(1), TextBox).Text
        Else
            Return xGridView.DataKeys(Ind).Item("Interbina")
        End If
    End Function

    Private Function Get_Germinabilita(Ind As Integer) As String

        Dim nCol_Germinabilita As Integer
        Dim xGridView As GridView

        If objParametriAgenda.TargetOperazione = enum_Tipo_Operazione_Agenda_Target.Reale Then
            nCol_Germinabilita = NumColonna_Germinabilita
            xGridView = GridViewImpianti
        Else
            nCol_Germinabilita = NumColonna_Planning_Germinabilita
            xGridView = GridViewPlanning
        End If


        If xGridView.Columns(nCol_Germinabilita).Visible Then
            Return CType(xGridView.Rows(Ind).Cells(nCol_Germinabilita).Controls(1), TextBox).Text
        Else
            Return xGridView.DataKeys(Ind).Item("Germinabilita")
        End If
    End Function

    Private Function Get_Specie(Ind As Integer) As String

        Dim Veg_Cod As String
        Select Case RBL_Tipo_Semina.SelectedValue 'Tipo_Semina
            Case enum_SEMINA_TIPO.Frazionamento_Appezzamenti_perLotto_e_Semina_sui_Singoli_Default
                Veg_Cod = GridViewImpianti.DataKeys(Ind).Item("Veg_Cod_Lotto")
                If Veg_Cod = "0" Then
                    Veg_Cod = GridViewImpianti.DataKeys(Ind).Item("Veg_Cod")
                End If
            Case Else
                Veg_Cod = GridViewImpianti.DataKeys(Ind).Item("Veg_Cod")
        End Select
        Return Veg_Cod
    End Function

    Private Function Get_Specie_Des(Ind As Integer) As String
        Return GridViewImpianti.DataKeys(Ind).Item("Veg_Des")
        'If GridViewImpianti.Columns(NumColonna_Specie).Visible Then
        '    Return GridViewImpianti.DataKeys(Ind).Item("Veg_Des")
        '    'Return CType(GridViewImpianti.Rows(Ind).Cells(NumColonna_Specie).Controls(1), DropDownList).SelectedItem.Text
        'Else
        '    Return New AgronicaCoreMetaSchemaDAL.SpecieVegetali_R().VegDes_from_VegCod(GridViewImpianti.DataKeys(Ind).Item("Veg_Cod"), objParametri_Server)
        'End If
    End Function

    Private Function Get_Varieta(Ind As Integer) As String
        If GridViewImpianti.Columns(NumColonna_Varietà).Visible Then
            Return CType(GridViewImpianti.Rows(Ind).Cells(NumColonna_Varietà).Controls(1), DropDownList).SelectedValue
        Else
            Return GridViewImpianti.DataKeys(Ind).Item("Cul_Cod")
        End If
    End Function

    Private Function Get_Varieta_Des(Ind As Integer) As String
        If GridViewImpianti.Columns(NumColonna_Varietà).Visible Then
            Return CType(GridViewImpianti.Rows(Ind).Cells(NumColonna_Varietà).Controls(1), DropDownList).SelectedItem.Text
        Else
            Return New AgronicaCoreMetaSchemaDAL.Cultivar_R().CulDes_from_CulCod(GridViewImpianti.DataKeys(Ind).Item("Cul_Cod"), objParametri_Server)
        End If
    End Function


    Private Function Get_Finalita(Ind As Integer) As String
        If GridViewImpianti.Columns(NumColonna_Finalità).Visible Then
            Return CType(GridViewImpianti.Rows(Ind).Cells(NumColonna_Finalità).Controls(1), DropDownList).SelectedValue
        Else
            Return GridViewImpianti.DataKeys(Ind).Item("Grfi_Cod")
        End If
    End Function

    Private Function Get_Finalita_Des(Ind As Integer) As String
        If GridViewImpianti.Columns(NumColonna_Finalità).Visible Then
            Return CType(GridViewImpianti.Rows(Ind).Cells(NumColonna_Finalità).Controls(1), DropDownList).SelectedItem.Text
        Else
            Return New AgronicaCoreMetaSchemaDAL.GruppoFinalita_R().GrfiDes_from_GrfiCod(GridViewImpianti.DataKeys(Ind).Item("Grfi_Cod"), GridViewImpianti.DataKeys(Ind).Item("Veg_Cod"), objParametri_Server)
        End If
    End Function

    Private Function Get_Disciplinare(Ind As Integer) As String
        If GridViewImpianti.Columns(NumColonna_Disciplinare).Visible Then
            Return CType(GridViewImpianti.Rows(Ind).Cells(NumColonna_Disciplinare).Controls(1), DropDownList).SelectedValue
        Else
            Return GridViewImpianti.DataKeys(Ind).Item("Disciplinare")
        End If
    End Function

    Private Function Get_Disciplinare_Des(Ind As Integer) As String
        If GridViewImpianti.Columns(NumColonna_Disciplinare).Visible Then
            Return CType(GridViewImpianti.Rows(Ind).Cells(NumColonna_Disciplinare).Controls(1), DropDownList).SelectedItem.Text
        Else
            Return GridViewImpianti.DataKeys(Ind).Item("Disciplinare")
        End If
    End Function

    Private Function Get_MetodoProduttivo(Ind As Integer) As String
        If GridViewImpianti.Columns(NumColonna_MetodoProduttivo).Visible Then
            Return CType(GridViewImpianti.Rows(Ind).Cells(NumColonna_MetodoProduttivo).Controls(1), DropDownList).SelectedValue
        Else
            Return GridViewImpianti.DataKeys(Ind).Item("MetodoProduttivo")
        End If
    End Function

    Private Function Get_MetodoProduttivo_Des(Ind As Integer) As String
        If GridViewImpianti.Columns(NumColonna_MetodoProduttivo).Visible Then
            Return CType(GridViewImpianti.Rows(Ind).Cells(NumColonna_MetodoProduttivo).Controls(1), DropDownList).SelectedItem.Text
        Else
            Return GridViewImpianti.DataKeys(Ind).Item("MetodoProduttivo")
        End If
    End Function

    Private Function Get_Prodotto(Ind As Integer) As String
        If objParametriAgenda.TargetOperazione = enum_Tipo_Operazione_Agenda_Target.Reale Then
            Return "0"
        Else

            Return CType(GridViewPlanning.Rows(Ind).Cells(NumColonna_Planning_Prodotto).Controls(1), ComboMateriePrime).Valore_Combo
        End If

    End Function

    Private Function Get_Udm(Ind As Integer) As String
        If objParametriAgenda.Fabbricato <> "0" Then
            'se ho il magazzino uso udm_cod_lotto
            'La colonna con DropDownList la vedo solo senza il magazzino, se c'è il magazzino non è necessaria
            'dato che l'udm è unica e dipende dal lotto, in questo caso uso i dati fissi 
            'GridViewImpianti.DataKeys(z).Item("Udm_Sim_Lotto") e GridViewImpianti.DataKeys(z).Item("Udm_Cod_Lotto")
            'e lo visualizzo tramite la colonna fissa Udm_Sim_Lotto
            Return CStr(GridViewImpianti.DataKeys(Ind).Item("Udm_Cod_Lotto"))
        Else
            'senza il magazzino udo il dato inserito nella DropDownList
            'il dato è sempre presente anche se la colonna non visibile perchè nascosta da javascript (displaynone)

            If objParametriAgenda.TargetOperazione = enum_Tipo_Operazione_Agenda_Target.Reale Then
                Return CType(GridViewImpianti.Rows(Ind).Cells(NumColonna_UnitaMisura).Controls(1), DropDownList).SelectedValue
            Else
                Return CType(GridViewPlanning.Rows(Ind).Cells(NumColonna_Planning_UnitaMisura).Controls(1), DropDownList).SelectedValue
            End If


        End If
        Return "0"
    End Function

    Private Function Get_Udm_Des(Ind As Integer) As String
        If objParametriAgenda.Fabbricato <> "0" Then
            Return New AgronicaCoreMetaSchemaDAL.UnitaMisura_R().UdmDes_from_UdmCod(GridViewImpianti.DataKeys(Ind).Item("Udm_Cod_Lotto"), "", objParametri_Server)
        Else
            If objParametriAgenda.TargetOperazione = enum_Tipo_Operazione_Agenda_Target.Reale Then
                Return CType(GridViewImpianti.Rows(Ind).Cells(NumColonna_UnitaMisura).Controls(1), DropDownList).Text
            Else
                Return CType(GridViewPlanning.Rows(Ind).Cells(NumColonna_Planning_UnitaMisura).Controls(1), DropDownList).Text
            End If
        End If
        Return "0"
    End Function

    Private Function Get_Superficie(Ind As Integer) As String
        'il dato è sempre presente anche se la colonna non visibile perchè nascosta da javascript (displaynone)

        Select Case RBL_Tipo_Semina.SelectedValue 'Tipo_Semina
            Case enum_SEMINA_TIPO.Frazionamento_Appezzamenti_perLotto_e_Semina_sui_Singoli_Default
                If GridViewImpianti.Columns(NumColonna__Sup_Semina).Visible Then
                    Return CType(GridViewImpianti.Rows(Ind).Cells(NumColonna__Sup_Semina).Controls(1), TextBox).Text
                Else
                    Return GridViewImpianti.DataKeys(Ind).Item("Sup_App")
                End If
            Case Else

                Dim nCol_Qta As Integer
                Dim xGridView As GridView

                If objParametriAgenda.TargetOperazione = enum_Tipo_Operazione_Agenda_Target.Reale Then
                    nCol_Qta = NumColonna__Sup_App
                    xGridView = GridViewImpianti
                Else
                    nCol_Qta = NumColonna_Planning_Sup_App
                    xGridView = GridViewPlanning
                End If

                If IsNumeric(xGridView.Rows(Ind).Cells(nCol_Qta).Text) Then
                    Return xGridView.Rows(Ind).Cells(nCol_Qta).Text
                Else
                    Return "0"
                End If
        End Select

    End Function

    Private Function Get_P_Ha(Ind As Integer) As String
        If GridViewImpianti.Columns(NumColonna_p_ha).Visible Then
            Return CType(GridViewImpianti.Rows(Ind).Cells(NumColonna_p_ha).Controls(1), TextBox).Text
        Else
            Return GridViewImpianti.DataKeys(Ind).Item("P_Ha")
        End If
    End Function

    Private Function Get_Qta(Ind As Integer) As String
        'il dato è sempre presente anche se la colonna non visibile perchè nascosta da javascript (displaynone)
        Dim nCol_Qta As Integer
        Dim xGridView As GridView

        If objParametriAgenda.TargetOperazione = enum_Tipo_Operazione_Agenda_Target.Reale Then
            nCol_Qta = NumColonna_PianteImpianto
            xGridView = GridViewImpianti
        Else
            nCol_Qta = NumColonna_Planning_PianteImpianto
            xGridView = GridViewPlanning
        End If

        If IsNumeric(CType(xGridView.Rows(Ind).Cells(nCol_Qta).Controls(1), TextBox).Text) Then
            Return CType(xGridView.Rows(Ind).Cells(nCol_Qta).Controls(1), TextBox).Text
        Else
            Return "0"
        End If
    End Function

    Private Sub Set_Distinta(ByVal z As String)
        If GridViewImpianti.Columns(NumColonna_Distinta).Visible Then
            Set_Distinta(z, GridViewImpianti.DataKeys(z).Item("Distinta"))
        End If
    End Sub
    Private Sub Set_Distinta(ByVal z As Integer, ByVal value As String)
        If value = "" Then 'or not numeric
            Dim i = 0
            'se il valore non è ammesso imposto quello di default
        End If
        If GridViewImpianti.Columns(NumColonna_Distinta).Visible Then
            Dim TextBox_Distinta As TextBox
            TextBox_Distinta = CType(GridViewImpianti.Rows(z).Cells(NumColonna_Distinta).Controls(1), TextBox)
            TextBox_Distinta.Text = value
        End If
    End Sub

    Private Sub Set_App_Nome(ByVal z As String)
        If GridViewImpianti.Columns(NumColonna_App_Nome).Visible Then
            Set_App_Nome(z, GridViewImpianti.DataKeys(z).Item("App_Nome"))
        End If
    End Sub
    Private Sub Set_App_Nome(ByVal z As Integer, ByVal value As String)
        If value = "" Then 'or not numeric
            Dim i = 0
            'se il valore non è ammesso imposto quello di default
        End If
        If GridViewImpianti.Columns(NumColonna_App_Nome).Visible Then
            Dim TextBox_App_Nome As TextBox
            TextBox_App_Nome = CType(GridViewImpianti.Rows(z).Cells(NumColonna_App_Nome).Controls(1), TextBox)
            TextBox_App_Nome.Text = value
        End If
    End Sub

    Private Sub Set_P_Ha(ByVal z As String)
        If GridViewImpianti.Columns(NumColonna_p_ha).Visible Then
            Set_P_Ha(z, GridViewImpianti.DataKeys(z).Item("P_Ha"))
        End If
    End Sub
    Private Sub Set_P_Ha(ByVal z As Integer, ByVal value As String)
        If value = "" Then 'or not numeric
            Dim i = 0
            'se il valore non è ammesso imposto quello di default
        End If
        If GridViewImpianti.Columns(NumColonna_p_ha).Visible Then
            Dim TextBox_P_Ha As TextBox
            TextBox_P_Ha = CType(GridViewImpianti.Rows(z).Cells(NumColonna_p_ha).Controls(1), TextBox)
            TextBox_P_Ha.Text = value
        End If
    End Sub

    Private Sub Set_Superficie(ByVal z As String, ByVal SettaSupOld As Boolean)
        If GridViewImpianti.Columns(NumColonna__Sup_Semina).Visible Then
            Set_Superficie(z, GridViewImpianti.DataKeys(z).Item("Sup_App"), SettaSupOld)
        End If
    End Sub
    Private Sub Set_Superficie(ByVal z As Integer, ByVal value As String, ByVal SettaSupOld As Boolean)
        If value = "" Then
            Dim i = 0
        End If
        If GridViewImpianti.Columns(NumColonna__Sup_Semina).Visible Then
            Dim TextBox_Sup_App As TextBox
            TextBox_Sup_App = CType(GridViewImpianti.Rows(z).Cells(NumColonna__Sup_Semina).Controls(1), TextBox)
            If SettaSupOld = True Then
                TextBox_Sup_App.Text = value
            Else
                TextBox_Sup_App.Text = 0
            End If
        End If
    End Sub

    Private Sub Set_Tra_Fila(ByVal z As String)
        If GridViewImpianti.Columns(NumColonna_DistTraFila).Visible Then
            Set_Tra_Fila(z, GridViewImpianti.DataKeys(z).Item("Tra_Fila"))
        End If
    End Sub
    Private Sub Set_Tra_Fila(ByVal z As Integer, ByVal value As String)
        If value = "" Then 'or not numeric
            Dim i = 0
            'se il valore non è ammesso imposto quello di default
        End If
        If GridViewImpianti.Columns(NumColonna_DistTraFila).Visible Then
            Dim TextBox_DistTraFila As TextBox
            TextBox_DistTraFila = CType(GridViewImpianti.Rows(z).Cells(NumColonna_DistTraFila).Controls(1), TextBox)
            TextBox_DistTraFila.Text = value
        End If
    End Sub

    Private Sub Set_Su_Fila(ByVal z As Integer)
        If GridViewImpianti.Columns(NumColonna_DistSuFila).Visible Then
            Set_Su_Fila(z, GridViewImpianti.DataKeys(z).Item("Su_Fila"))
        End If
    End Sub
    Private Sub Set_Su_Fila(ByVal z As Integer, ByVal value As String)
        If value = "" Then 'or not numeric
            'se il valore non è ammesso ..loop
            Dim i = 0
        End If
        If GridViewImpianti.Columns(NumColonna_DistSuFila).Visible Then
            Dim TextBox_Su_Fila As TextBox
            TextBox_Su_Fila = CType(GridViewImpianti.Rows(z).Cells(NumColonna_DistSuFila).Controls(1), TextBox)
            TextBox_Su_Fila.Text = value
        End If
    End Sub

    Private Sub Set_Interbina(ByVal z As Integer)
        If GridViewImpianti.Columns(NumColonna_Interbina).Visible Then
            Set_Interbina(z, GridViewImpianti.DataKeys(z).Item("Interbina"))
        End If
    End Sub
    Private Sub Set_Interbina(ByVal z As Integer, ByVal value As String)
        If value = "" Then 'or not numer..loop
            Dim i = 0
        End If
        If GridViewImpianti.Columns(NumColonna_Interbina).Visible Then
            Dim TextBox_Interbina As TextBox
            TextBox_Interbina = CType(GridViewImpianti.Rows(z).Cells(NumColonna_Interbina).Controls(1), TextBox)
            TextBox_Interbina.Text = value
        End If
    End Sub

    Private Sub Set_Germinabilita(ByVal z As Integer)
        If GridViewImpianti.Columns(NumColonna_Germinabilita).Visible Then
            Set_Germinabilita(z, GridViewImpianti.DataKeys(z).Item("Germinabilita"))
        End If
    End Sub
    Private Sub Set_Germinabilita(ByVal z As Integer, ByVal value As String)
        If value = "" Then 'or not numeric
            'se il valore non è ammesso..loop
            Dim i = 0
        End If
        If GridViewImpianti.Columns(NumColonna_Germinabilita).Visible Then
            Dim TextBox_Germinabilita As TextBox
            TextBox_Germinabilita = CType(GridViewImpianti.Rows(z).Cells(NumColonna_Germinabilita).Controls(1), TextBox)
            TextBox_Germinabilita.Text = value
        End If
    End Sub

    Private Sub Set_Prodotto(ByVal z As Integer)

        If GridViewPlanning.Columns(NumColonna_Planning_Prodotto).Visible Then

            Dim Combo_Prodotto As AgronicaControlli_2010.ComboMateriePrime
            Combo_Prodotto = CType(GridViewPlanning.Rows(z).Cells(NumColonna_Planning_Prodotto).Controls(1), AgronicaControlli_2010.ComboMateriePrime)

            Dim veg_cod As Integer = GridViewPlanning.DataKeys(z).Item("Veg_Cod")
            Dim cul_cod As Integer = GridViewPlanning.DataKeys(z).Item("Cul_Cod")
            Dim Elem_cod As Integer = 10
            Combo_Prodotto.CaricaComboMateriePrime(objParametriAgenda.Piva, Elem_cod, veg_cod, cul_cod, "", "", "", AGRODATAINIZIO, AGRODATAFINE)


            'script Prodotto
            ScriptManager.RegisterClientScriptBlock(UpdatePanelMagazzinoImpianti, UpdatePanelMagazzinoImpianti.GetType(),
                                             String.Format("jQuery_{0}", Combo_Prodotto.ClientID), Combo_Prodotto.GetJS(), True)

            Dim Mat_Cod As Integer = 0
            If Not IsDBNull(GridViewPlanning.DataKeys(z).Item("Mat_Cod")) AndAlso IsNumeric(GridViewPlanning.DataKeys(z).Item("Mat_Cod")) Then
                Mat_Cod = GridViewPlanning.DataKeys(z).Item("Mat_Cod")
                Combo_Prodotto.Valore_Combo = Mat_Cod
            End If


        End If
    End Sub

    Private Sub Set_Prodotto(ByVal z As Integer, ByVal Valore As String)
        CType(GridViewPlanning.Rows(z).Cells(NumColonna_Planning_Prodotto).Controls(1), AgronicaControlli_2010.ComboMateriePrime).Valore_Combo = Valore
    End Sub


    Private Sub Set_Varieta(ByVal z As Integer)

        Dim i As Integer
        Dim Veg_Cod As Integer

        Veg_Cod = GridViewImpianti.DataKeys(z).Item("Veg_Cod_Lotto")
        If Veg_Cod = 0 Then
            Veg_Cod = GridViewImpianti.DataKeys(z).Item("Veg_Cod")
        End If
        If Veg_Cod = 0 Then
            Veg_Cod = objParametriAgenda.Veg_Cod.Split("/")(0)
        End If

        If GridViewImpianti.Columns(NumColonna_Varietà).Visible And Veg_Cod > 0 Then

            Dim Combo_Varieta As DropDownList
            Combo_Varieta = CType(GridViewImpianti.Rows(z).Cells(NumColonna_Varietà).Controls(1), DropDownList)

            Dim objVarieta As New AgronicaCoreMetaSchemaDAL.Cultivar_R
            Dim DT_Cultivar As DataTable
            Dim boolTrovato As Boolean = False
            Dim CultivarDaTrovare As Integer

            CultivarDaTrovare = GridViewImpianti.DataKeys(z).Item("Cul_Cod_Lotto")
            If CultivarDaTrovare = 0 Then
                CultivarDaTrovare = GridViewImpianti.DataKeys(z).Item("Cul_Cod")
            End If
            For i = 0 To Combo_Varieta.Items.Count - 1
                If Combo_Varieta.Items(i).Value = CultivarDaTrovare Then
                    boolTrovato = True
                    Exit For
                End If
            Next

            If boolTrovato = False Then
                DT_Cultivar = objVarieta.Leggi(CultivarDaTrovare,
                                           Veg_Cod, "",
                                           enumSelezioneVariabile.Selezione_TabellaDatiMinimi,
                                            "", "", objParametri_Server)
                If Not DT_Cultivar Is Nothing AndAlso DT_Cultivar.Rows.Count > 0 Then
                    Combo_Varieta.Items.Add(New ListItem(DT_Cultivar.Rows(0).Item("cul_des"), DT_Cultivar.Rows(0).Item("cul_cod")))
                End If
            End If
            Combo_Varieta.SelectedValue = CultivarDaTrovare
        End If

    End Sub
    Private Sub Set_Varieta(ByVal z As Integer, ByVal value As String)
        If GridViewImpianti.Columns(NumColonna_Varietà).Visible Then
            CType(GridViewImpianti.Rows(z).Cells(NumColonna_Varietà).Controls(1), DropDownList).SelectedValue = value
        End If
    End Sub

    Private Sub Set_Specie(ByVal z As Integer)

        If GridViewImpianti.Columns(NumColonna_Specie).Visible Then

            Dim SpecieDaTrovare As Integer
            SpecieDaTrovare = GridViewImpianti.DataKeys(z).Item("Veg_Cod_Lotto")
            If SpecieDaTrovare = 0 Then
                SpecieDaTrovare = GridViewImpianti.DataKeys(z).Item("Veg_Cod")
            End If
            If SpecieDaTrovare = 0 Then
                SpecieDaTrovare = objParametriAgenda.Veg_Cod.Split("/")(0)
            End If

            Dim Combo_Varieta As DropDownList
            Dim Combo_Finalita As DropDownList
            Dim Combo_Dpi As DropDownList

            Combo_Varieta = CType(GridViewImpianti.Rows(z).Cells(NumColonna_Varietà).Controls(1), DropDownList)
            Combo_Finalita = CType(GridViewImpianti.Rows(z).Cells(NumColonna_Finalità).Controls(1), DropDownList)
            Combo_Dpi = CType(GridViewImpianti.Rows(z).Cells(NumColonna_Disciplinare).Controls(1), DropDownList)

            If SpecieDaTrovare > 0 Then

                Combo_Varieta.Items.Clear()
                Combo_Finalita.Items.Clear()
                Combo_Dpi.Items.Clear()

                If Not HashSpecie.ContainsKey(SpecieDaTrovare) Then

                    HashSpecie.Add(SpecieDaTrovare, "")

                    AgronicaCoreUtility.CaricaListControl.CaricaCombo_VarietaColtivate_con_Visibilita_Utente(Combo_Varieta,
                            False, "", "", True, SpecieDaTrovare, 0, "", "", objParametri_Server, objParametri_Utenti)

                    For c = 0 To Combo_Varieta.Items.Count - 1
                        Dim dr As DataRow = DtCultivar.NewRow
                        dr.Item("veg_cod") = SpecieDaTrovare
                        dr.Item("cul_cod") = Combo_Varieta.Items(c).Value
                        dr.Item("cul_des") = Combo_Varieta.Items(c).Text
                        DtCultivar.Rows.Add(dr)
                    Next

                    AgronicaCoreUtility.CaricaListControl.Finalita(
                                        Combo_Finalita,
                                        False, "", "",
                                        SpecieDaTrovare, 0, "", "", "", objParametri_Server)


                    For c = 0 To Combo_Finalita.Items.Count - 1
                        Dim dr As DataRow = DtFinalita.NewRow
                        dr.Item("veg_cod") = SpecieDaTrovare
                        dr.Item("grfi_cod") = Combo_Finalita.Items(c).Value
                        dr.Item("grfi_des") = Combo_Finalita.Items(c).Text
                        DtFinalita.Rows.Add(dr)
                    Next

                    'DISCIPLINARE

                    objParametri_Server.ImpostaFinestre_con_SalvataggioTemporale(CDate(objParametriAgenda.Data), CDate(objParametriAgenda.Data))
                    Dim objCaricaCombo As New AgronicaCoreDpiBIZ.CaricaListControl
                    objCaricaCombo.Disciplinari_Elenco(Combo_Dpi,
                                                     False, "", "",
                                                    Session,
                                                    objParametri_Server,
                                                    objParametri_Utenti,
                                                    CInt(0),
                                                    SpecieDaTrovare,
                                                    CInt(0),
                                                    CInt(0),
                                                    True,
                                                    False,
                                                    False,
                                                    New AgronicaCoreGestioneRichieste.AgroWebConfig())
                    objParametri_Server.ResettaFinestra()

                    For c = 0 To Combo_Dpi.Items.Count - 1
                        Dim dr As DataRow = DtDpi.NewRow
                        dr.Item("veg_cod") = SpecieDaTrovare
                        dr.Item("dpi_cod") = Combo_Dpi.Items(c).Value
                        dr.Item("dpi_des") = Combo_Dpi.Items(c).Text
                        DtDpi.Rows.Add(dr)
                    Next

                Else

                    Dim DrCul() As DataRow = DtCultivar.Select("veg_cod=" & SpecieDaTrovare)
                    If Not DrCul Is Nothing AndAlso DrCul.Length > 0 Then
                        For x = 0 To DrCul.Length - 1
                            Combo_Varieta.Items.Add(New ListItem(DrCul(x).Item("cul_des"), DrCul(x).Item("cul_cod")))
                        Next
                    End If
                    Dim DrFin() As DataRow = DtFinalita.Select("veg_cod=" & SpecieDaTrovare)
                    If Not DrFin Is Nothing AndAlso DrFin.Length > 0 Then
                        For x = 0 To DrFin.Length - 1
                            Combo_Finalita.Items.Add(New ListItem(DrFin(x).Item("grfi_des"), DrFin(x).Item("grfi_cod")))
                        Next
                    End If
                    Dim DrDpi() As DataRow = DtDpi.Select("veg_cod=" & SpecieDaTrovare)
                    If Not DrDpi Is Nothing AndAlso DrDpi.Length > 0 Then
                        For x = 0 To DrDpi.Length - 1
                            Combo_Dpi.Items.Add(New ListItem(DrDpi(x).Item("dpi_des"), DrDpi(x).Item("dpi_cod")))
                        Next
                    End If

                End If

            Else
                Combo_Dpi.Items.Clear()
                Combo_Dpi.Items.Add(New ListItem(Resources.AgronicaAgenda_2010.NessunDisciplinare, "0"))
            End If

        End If

    End Sub

    Private Sub Set_Finalita(ByVal z As Integer)
        If GridViewImpianti.Columns(NumColonna_Finalità).Visible Then
            Dim Combo_Finalita As DropDownList
            Combo_Finalita = CType(GridViewImpianti.Rows(z).Cells(NumColonna_Finalità).Controls(1), DropDownList)
            Combo_Finalita.SelectedValue = CInt(GridViewImpianti.DataKeys(z).Item("Grfi_Cod"))
        End If
    End Sub
    Private Sub Set_Finalita(ByVal z As Integer, ByVal value As String)
        If GridViewImpianti.Columns(NumColonna_Finalità).Visible Then
            CType(GridViewImpianti.Rows(z).Cells(NumColonna_Finalità).Controls(1), DropDownList).SelectedValue = value
        End If
    End Sub

    Private Sub Set_Disciplinare(ByVal z As Integer)
        If GridViewImpianti.Columns(NumColonna_Disciplinare).Visible Then
            Dim Combo_Disciplinare As DropDownList
            Combo_Disciplinare = CType(GridViewImpianti.Rows(z).Cells(NumColonna_Disciplinare).Controls(1), DropDownList)
            Combo_Disciplinare.SelectedValue = GridViewImpianti.DataKeys(z).Item("Disciplinare")
        End If
    End Sub
    Private Sub Set_Disciplinare(ByVal z As Integer, ByVal value As String)
        If GridViewImpianti.Columns(NumColonna_Disciplinare).Visible Then
            CType(GridViewImpianti.Rows(z).Cells(NumColonna_Disciplinare).Controls(1), DropDownList).SelectedValue = value
        End If
    End Sub

    Private Sub Set_MetodoProduttivo(ByVal z As Integer)
        If GridViewImpianti.Columns(NumColonna_MetodoProduttivo).Visible Then
            Dim Combo_MetodoProduttivo As DropDownList
            Combo_MetodoProduttivo = CType(GridViewImpianti.Rows(z).Cells(NumColonna_MetodoProduttivo).Controls(1), DropDownList)
            AgronicaCoreUtility.CaricaListControl.MetodoProduzione(Combo_MetodoProduttivo, False, "", "")
            Combo_MetodoProduttivo.SelectedValue = GridViewImpianti.DataKeys(z).Item("MetodoProduttivo")
        End If
    End Sub
    Private Sub Set_MetodoProduttivo(ByVal z As Integer, ByVal value As String)
        If GridViewImpianti.Columns(NumColonna_MetodoProduttivo).Visible Then
            CType(GridViewImpianti.Rows(z).Cells(NumColonna_MetodoProduttivo).Controls(1), DropDownList).SelectedValue = value
        End If
    End Sub

    Private Sub Set_Udm(ByVal z As Integer)
        'il dato è sempre presente anche se la colonna non visibile perchè nascosta da javascript (displaynone)

        If objParametriAgenda.Fabbricato <> "0" Then
            'Non uso più la colonna NumColonna_UnitaMisura ma quella che ha udm_sim_lotto per visualizzare la descrizione
            'mentre per il codice dell'unità di misura, che è fisso se si utilizza il magazzino uso la .DataKeys(z).Item("Udm_Sim_Lotto")
            'non è necessaria la dropdownlist dato che il dato è sempre unico
            ' ''se ho magazzino carico solo l'udm del lotto
            ' ''controllo se ho udm, se non ho ancora selezionato una giacenza udm_cod è ""
            ''If Not IsDBNull(GridViewImpianti.DataKeys(z).Item("Udm_Cod_Lotto")) AndAlso
            ''    CStr(GridViewImpianti.DataKeys(z).Item("Udm_Cod_Lotto")) <> "" Then
            ''    Combo_Udm.Items.Add(New ListItem(GridViewImpianti.DataKeys(z).Item("Udm_Sim_Lotto"), GridViewImpianti.DataKeys(z).Item("Udm_Cod_Lotto")))
            ''End If
        Else
            Dim Combo_Udm As DropDownList
            If objParametriAgenda.TargetOperazione = enum_Tipo_Operazione_Agenda_Target.Reale Then
                Combo_Udm = CType(GridViewImpianti.Rows(z).Cells(NumColonna_UnitaMisura).Controls(1), DropDownList)
            Else
                Combo_Udm = CType(GridViewPlanning.Rows(z).Cells(NumColonna_Planning_UnitaMisura).Controls(1), DropDownList)
            End If

            'se non ho magazzino carico tutte le udm nella combo
            AgronicaCoreUtility.CaricaListControl.UnitaMisuraAgendaMagazzino(
                                    Combo_Udm, True, "Non Specificata", "0", SEMENTI, CAU_CARICO, "", 0, 0, False, 0, 0, "", "", objParametri_Server, objParametri_Utenti)
            Set_Udm(z, UdmDefault)
        End If
    End Sub
    Private Sub Set_Udm(ByVal z As Integer, ByVal value As String)
        'il dato è sempre presente anche se la colonna non visibile perchè nascosta da javascript (displaynone)
        If objParametriAgenda.Fabbricato <> "0" Then
            'La colonna con DropDownList la vedo solo senza il magazzino, se c'è il magazzino non è necessaria
            'dato che l'udm è unica e dipende dal lotto, in questo caso uso i dati fissi 
            'GridViewImpianti.DataKeys(z).Item("Udm_Sim_Lotto") e GridViewImpianti.DataKeys(z).Item("Udm_Cod_Lotto")
            'e lo visualizzo tramite la colonna fissa Udm_Sim_Lotto
        Else
            If objParametriAgenda.TargetOperazione = enum_Tipo_Operazione_Agenda_Target.Reale Then
                CType(GridViewImpianti.Rows(z).Cells(NumColonna_UnitaMisura).Controls(1), DropDownList).SelectedValue = value
            Else
                CType(GridViewPlanning.Rows(z).Cells(NumColonna_Planning_UnitaMisura).Controls(1), DropDownList).SelectedValue = value
            End If

        End If

    End Sub

    Private Sub Set_Qta(ByVal z As Integer)
        Dim qtaDaProfilatore As Integer = LeggiQtaSementeDefaultDaProfilatore(z)
        If qtaDaProfilatore = 0 Then
            Set_Qta(z, QtaDefault)
        Else
            Set_Qta(z, qtaDaProfilatore)
        End If
    End Sub
    Private Sub Set_Qta(ByVal z As Integer, ByVal value As String)

        'il dato è sempre presente anche se la colonna non visibile perchè nascosta da javascript (displaynone)
        If objParametriAgenda.TargetOperazione = enum_Tipo_Operazione_Agenda_Target.Reale Then
            CType(GridViewImpianti.Rows(z).Cells(NumColonna_PianteImpianto).Controls(1), TextBox).Text = value
        Else
            CType(GridViewPlanning.Rows(z).Cells(NumColonna_Planning_PianteImpianto).Controls(1), TextBox).Text = value
        End If

    End Sub


#End Region



    Private Sub BTN_CaricoMagazzino(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim PaginaLink As String
        'Case LAVCOD_SCARICO, LAVCOD_CARICO, LAVCOD_VENDITA, LAVCOD_ACQUISTO, LAVCOD_TRASFERIMENTO
        PaginaLink = "../GestioneMagazzini/FormProdotto.aspx"

        Dim xChiave As String = ""
        Dim mode As String = ""
        Dim Carico_Scarico As String = ""
        Call Albero.ChiaveAlbero_Codifica(xChiave,
                                               enum_TipoNodo.p_PortafoglioProdotti,
                                               objParametriAgenda.Fabbricato.Split("|")(2),
                                               objParametriAgenda.Fabbricato.Split("|")(1), , , , , , , , , , , ,
                                               objParametriAgenda.Fabbricato.Split("|")(0))

        Dim Lav_Cod As Integer = LAVCOD_CARICO
        Carico_Scarico = "C"
        mode = "magazzino"


        PaginaLink = PaginaLink &
                        "?k=" & Stringa_Codifica(xChiave, AgroKey_EncoderDecoder) &
                        "&c=" & Stringa_Codifica(Carico_Scarico, AgroKey_EncoderDecoder) &
                        "&o=" & Stringa_Codifica(enum_TipoOperazioneDB.Scrittura, AgroKey_EncoderDecoder) &
                        "&orig=" & Stringa_Codifica(enum_PagineAgenda_2010.Menu, AgroKey_EncoderDecoder) &
                        "&mode=" & Stringa_Codifica(mode, AgroKey_EncoderDecoder) &
                         "&l=" & Stringa_Codifica(objParametriAgenda.Lav_Cod, AgroKey_EncoderDecoder) &
                        "&d=" & Stringa_Codifica(CStr(objParametriAgenda.Data), AgroKey_EncoderDecoder) &
                        "&s=" & Stringa_Codifica(objParametriAgenda.Sa_Cod, AgroKey_EncoderDecoder) &
                        "&a=" & Stringa_Codifica(objParametriAgenda.Id_Agenda, AgroKey_EncoderDecoder) &
                        "&Exit=True"


        Dim strJS As String
        strJS = "<script language='javascript'>" &
            "           window.open('" & PaginaLink & "' ," &
            "           'stampe'," &
            "           'height=700," &
            "           width=1000," &
            "           menubar=yes," &
            "           resizable=yes," &
            "           scrollbars=yes," &
            "           top=0,left=0');" &
            " </script> "


        ScriptManager.RegisterStartupScript(Master_Operazione.Property_UpdatePanelPerScript, Master_Operazione.Property_UpdatePanelPerScript.GetType(),
                                        String.Format("jQuery_{0}", Master_Operazione.Property_UpdatePanelPerScript.ClientID), strJS.ToString, False)


    End Sub


    Private Sub Ripristina_Dati_nei_Controlli() Implements iOperazioneGUI.Ripristina_Dati_nei_Controlli

        Dim objAgenda As New Agenda_Operazione_Helper
        Dim Agenda As Operazione_Agenda = objAgenda.Leggi(objParametriAgenda.Piva, objParametriAgenda.Sa_Cod, objParametriAgenda.Id_Agenda, 0, objParametri_Server)

        'CONTROLLO SE CI SONO DEI COSTI COLLEGATI
        For Each mdRif As Movimento_Dettaglio_Riferimento In Agenda.Agenda_Riferimenti
            If mdRif.Lav_Cod_Rif = LAVCOD_COSTI_CDG Then
                'Messaggi.AgroMsgBuonFine("NB: Esistono costi collegati a questa operazione.", Page, , CType(Ricerca.FindControlIterative(Page.Master, "UpdatePanelToolBar"), UpdatePanel))
                CType(Page.Master, Operazione).Property_hf_esistonoCostiCollegatiCDG = True
                Exit For
            End If
        Next

    End Sub


#Region "Per Ricette"

    Private Sub Btn_Conferma_Ricetta(sender As Object, e As EventArgs)


        Dim listaImpiantiSelezionati As List(Of AgronicaCoreModello.ParametriAgenda_Temp.Impianto) = Master_Operazione.GetImpianti()
        If listaImpiantiSelezionati.Count = 0 Then
            Exit Sub
        End If

        Ripristina_Dati_nei_ControlliDaRicetta()

    End Sub


    Private Sub Ripristina_Dati_nei_Controlli_xRicetta(ByVal Xml_Operazione As String) Implements iOperazioneGUI.Ripristina_Dati_nei_Controlli_xRicetta

    End Sub

    Private Function Operazione_Agenda_xRicetta(ByVal Xml_Operazione As String) As Operazione_Agenda

    End Function

    Private Sub Ripristina_Dati_nei_ControlliDaRicetta() Implements iOperazioneGUI.Ripristina_Dati_nei_ControlliDaRicetta

        Session("UtilizzataRicetta") = False


        Dim ricetta_cod As String = Session("ricetta_cod")
        Dim Ricetta_Operazione_Cod As String = Session("Ricetta_Operazione_Cod")


        Dim dt_RicetteOperazioni As DataTable = New AgronicaCoreContabDAL.Ricette_Operazioni_R().Leggi(CInt(ricetta_cod), CInt(Ricetta_Operazione_Cod), 0, 0, AGRODATAINIZIO, AGRODATAFINE, AgronicaCoreParametri.enumSelezioneVariabile.Selezione_TabellaDatiMinimi, "", "", objParametri_Server)
        Dim dt_RicetteDettagli As DataTable = New AgronicaCoreContabDAL.Ricette_Dettagli_R().Leggi(CInt(ricetta_cod), CInt(Ricetta_Operazione_Cod), 0, "", 0, 0, 0, 0, AGRODATAINIZIO, AGRODATAFINE, AgronicaCoreParametri.enumSelezioneVariabile.Selezione_TabellaDatiMinimi, "", "", objParametri_Server)
        Dim dt_RicetteDettagliTecnici As DataTable = New AgronicaCoreContabDAL.Ricette_Dett_Tecnico_R().Leggi(CInt(ricetta_cod), CInt(Ricetta_Operazione_Cod), 0, 0, 0, AGRODATAINIZIO, AGRODATAFINE, AgronicaCoreParametri.enumSelezioneVariabile.Selezione_TabellaDatiMinimi, "", "", objParametri_Server)
        'Dim dt_RicetteDestinazioni As DataTable = New AgronicaCoreContabDAL.Ricette_Destinazioni_R().Leggi(CInt(ricetta_cod), CInt(Ricetta_Operazione_Cod), 0, 0, 0, "", 0, 0, 0, AGRODATAINIZIO, AGRODATAFINE, AgronicaCoreParametri.enumSelezioneVariabile.Selezione_TabellaDatiMinimi, "", "", objParametri_Server)

        If dt_RicetteOperazioni.Rows.Count = 0 Then
            Messaggi.AgroMsgBox(Resources.AgronicaAgenda_2010.NonÈStataTrovataLaRicettaOperazione, Page, ,
                    CType(Ricerca.FindControlIterative(Page.Master, "UpdatePanelToolBar"), UpdatePanel))
            Exit Sub
        End If

        If dt_RicetteOperazioni.Rows(0).Item("Lav_Cod") <> objParametriAgenda.Lav_Cod Then
            Messaggi.AgroMsgBox(Resources.AgronicaAgenda_2010.LaRicettaHaUnaOperazioneAssociataDifferent, Page, ,
                    CType(Ricerca.FindControlIterative(Page.Master, "UpdatePanelToolBar"), UpdatePanel))
            Exit Sub
        End If


        'CONTROLLARE
        CType(Page.Master, Operazione).SetNota(dt_RicetteOperazioni.Rows(0).Item("Note"))



        'ci possono essere piu dettagli per lo stesso trattamento, 
        'sfoglio i dettagli come fossero i movimenti, per leggere i cau_mov,
        'e essere sicuro di leggere quelli del trattamento e non quelli dei costi accessori o magazzino
        If dt_RicetteDettagli.Rows.Count = 0 Then
            Messaggi.AgroMsgBox(Resources.AgronicaAgenda_2010.NonSonoStatiTrovatiDettagli, Page, ,
                    CType(Ricerca.FindControlIterative(Page.Master, "UpdatePanelToolBar"), UpdatePanel))
            Exit Sub
        End If

        'Dim Trappola_Cod As Integer = 0
        Dim Trappola_Dispenser As Integer = 0
        'Dim Trappola_UDM As Integer = 0
        Dim Trappola_QTA As Integer = 0
        'Dim Inneschi_QTA As Integer = 0
        'Dim Trappola_IDPers As Integer = 0
        Dim Trappola_Ditta As Integer = 0
        'Dim Trappola_ID As Integer = 0
        Dim Trappola_AV_COD As Integer = 0
        'Dim Trappola_AV_Sigla As String = ""

        'estraggo i cau_mov trattamenti, possono essere più di uno perchè la ricetta na un dettaglio per ciascun prodotto che contiene il cau_cov
        Dim dr_RicetteDettagliTrattamenti() As DataRow

        dr_RicetteDettagliTrattamenti = dt_RicetteDettagli.Select("Cau_Mov= '" & enum_Agenda_Causali.LAVORAZIONE & "' ")

        'Per le trappole, 
        'per ciscun impianto ho un dettaglio che incica il codice elemento, prodotto, udm, qta, uguali per tutti i dettagli, e mi interessa solo qta che incica il totale delle trapole installate in tutti gli impianti
        'per cisacun dettaglio ho una destinazione che indica l'impianto e la qta parziale (k), non mi interessa leggere
        'per ciascuh dettaglio ho k dettagli tecnic, ciscuno identifica una trappola installata nell'impianto con il suo id e codice personaliz, 
        '   ma leggo solo av_cod e ditta cod, essendo uguali per tutte le trappole basta leggere un solo det tecn.
        '   il det tecnocop ha anche il numero inneschi nella dose, ma non li leggo, ci sono solo, come altri campi, quando è generata dall'agenda
        'If dr_RicetteDettagliTrattamenti.Count > 0 Then

        '    'Trappola_Cod = CInt(dr_RicetteDettagliTrattamenti(0).Item("Elem_Cod")) '197 trappole
        '    Trappola_Dispenser = CInt(dr_RicetteDettagliTrattamenti(0).Item("Pro_Cod")) 'codice dispender per la combo
        '    'Trappola_UDM = CInt(dr_RicetteDettagliTrattamenti(0).Item("Udm_Cod")) 'udm, numero trappole
        '    Trappola_QTA = CInt(dr_RicetteDettagliTrattamenti(0).Item("Qta")) 'Numero totale trappole

        '    'MOVIMENTI_DETTAGLI_TECNICI

        '    Dim Dr_DettagliTEcRicette_Avversita() As DataRow = dt_RicetteDettagliTecnici.Select("Ricetta_Dettaglio_Cod= " & dr_RicetteDettagliTrattamenti(0).Item("Ricetta_Dettaglio_Cod") & " ")
        '    If Dr_DettagliTEcRicette_Avversita.Count > 0 Then


        '        'Inneschi_QTA = CDbl(Dr_DettagliTEcRicette_Avversita(0).Item("dose"))
        '        'Trappola_IDPers = CDbl(Dr_DettagliTEcRicette_Avversita(0).Item("Freatimetro"))
        '        Trappola_Ditta = CInt(Dr_DettagliTEcRicette_Avversita(0).Item("Ditta_Cod"))
        '        'Trappola_ID = CInt(Dr_DettagliTEcRicette_Avversita(0).Item("Trap_Num"))
        '        Trappola_AV_COD = CInt(Dr_DettagliTEcRicette_Avversita(0).Item("Av_Cod"))
        '        'Trappola_AV_Sigla = CInt(Dr_DettagliTEcRicette_Avversita(0).Item("Sigla_AV"))

        '        'Non inserisco le trappole ma faccio fare manualmente, in modo che si possa cambiare il numero e la posizione


        '        Me.Txt_NumeroTrappole.Text = Trappola_QTA

        '        cmb_Trappola.SelectedValue = Trappola_Dispenser
        '        If cmb_Trappola.SelectedValue <> Trappola_Dispenser Then
        '            cmb_Trappola.SelectedValue = "-1"
        '        End If

        '        cmb_Trappola_SelectedIndexChanged(Nothing, Nothing)

        '        cmb_Ditte.SelectedValue = Trappola_Ditta
        '        If cmb_Ditte.SelectedValue <> Trappola_Ditta Then
        '            cmb_Ditte.SelectedValue = ""
        '        End If

        '        cmb_Avversita.SelectedValue = Trappola_AV_COD
        '        If cmb_Avversita.SelectedValue <> Trappola_AV_COD Then
        '            cmb_Avversita.SelectedValue = ""
        '        End If

        '        cmb_Avversita_SelectedIndexChanged(Nothing, Nothing)




        '    End If

        'End If




        Session("UtilizzataRicetta") = True


    End Sub



#End Region


    Private Function LeggiQtaSementeDefaultDaProfilatore(z As Integer) As String

        Try

            Dim ha As Decimal = GridViewImpianti.DataKeys(z).Item("Sup_App")
            Dim varieta As Integer = GridViewImpianti.DataKeys(z).Item("Cul_Cod_Lotto")
            Dim specie As Integer = GridViewImpianti.DataKeys(z).Item("Veg_Cod_Lotto")
            If specie = 0 Then
                Return 0
            End If
            If varieta = 0 Then
                Return 0
            End If
            Dim udm As Integer = GridViewImpianti.DataKeys(z).Item("Udm_Cod_Lotto")
            If udm = 0 Then
                Return 0
            End If
            Dim dose As Decimal = 0
            Dim UDM_Res As Integer = 0

            Dim TrovataUDM As Boolean
            Dim ImpostazioneAziendale As Boolean
            Dim res As Boolean = New AgronicaCoreAnagrafeDAL.SpecieVegetali_Default_R().Leggi_Dosi_X_Ha_In_Cascata(objParametriAgenda.Piva, specie, varieta, udm, objParametriAgenda.Data, dose, TrovataUDM, ImpostazioneAziendale, "", objParametri_Server)

            If res = False Then
                Return "0"
            End If

            If TrovataUDM Then
                'se ritorna true allora ha trovato la udm specifica
                Return CStr(dose * ha)
            Else
                'se ritorna false allora la dose non ha l'udm oppure è 0, cioè non specificata, 
                'quindi per scelta considero come se la dose salvata indicasse l'unità di seme
                'quindi la calcolo la dose totale solo se l'udm della riga magazzino è unità di seme
                If udm = 93 Then
                    Return CStr(dose * ha)
                Else
                    Return "0"
                End If

            End If

        Catch ex As Exception

            Return "0"

        End Try




    End Function


    'Elimino i ddt allegati
    Private Sub ImgBtn_DDT_Cancella_Click(sender As Object, e As ImageClickEventArgs)
        Session("MovimentiDettagliDDT") = Nothing
        ImageInfo0.Visible = False
        LabelInfo0.Visible = False
        caricaDtGiacenze()
        caricaListeValoriDaTabelle()
        caricaTabelle(False)
    End Sub


    'allega ddt
    Private Sub ImgBtn_DDT_Click(sender As Object, e As ImageClickEventArgs)

        If objParametriAgenda.Lav_Cod = LAVCOD_SOVESCIO Then
            'con il sovescio non allego il ddt
            Messaggi.AgroMsgBox("Con la semina per sovescio non è possibile allegare il DDT", Page, , Master_Operazione.Property_UpdatePanelToolBar)
            Session("MovimentiDettagliDDT") = Nothing
            Exit Sub
        End If

        If objParametriAgenda.Fabbricato = "0" Or (Split(objParametriAgenda.Fabbricato, "|").Count = 3 AndAlso (Split(objParametriAgenda.Fabbricato, "|")(2) <> objParametriAgenda.Piva)) Then
            Messaggi.AgroMsgBox(Resources.AgronicaAgenda_2010.EPossibileAllegareUnDDTSoloUsandoIlMagazzi, Page, , Master_Operazione.Property_UpdatePanelToolBar)
            Exit Sub
        End If


        Dim objGiasOnline As New AgronicaCoreGestioneRichieste.ParametriGiasOnline
        objGiasOnline.Cul_Cod = objParametriAgenda.Cul_Cod
        objGiasOnline.DataSelezionata = objParametriAgenda.Data
        objGiasOnline.Id_Agenda = objParametriAgenda.Id_Agenda
        objGiasOnline.Lavorazione = LAVCOD_SEMINA
        objGiasOnline.Operazione = objParametriAgenda.Tipo_Operazione
        objGiasOnline.PaginaRichiesta = enum_PagineGiasOnline.FiltroMovContabili
        objGiasOnline.Piva = objParametriAgenda.Piva
        objGiasOnline.Sa_Cod = objParametriAgenda.Sa_Cod
        Dim specie As Integer = 0
        If IsNumeric(objParametriAgenda.Veg_Cod.Split("/")(0)) AndAlso CInt(objParametriAgenda.Veg_Cod.Split("/")(0)) > 0 Then
            specie = CInt(objParametriAgenda.Veg_Cod.Split("/")(0))
        End If
        objGiasOnline.Veg_Cod = specie
        objGiasOnline.Rag_Soc = New AgronicaCoreAnagrafeDAL.Imprese_Read().RagSoc_from_Piva(objGiasOnline.Piva, objParametri_Server)
        ' Cod_RisUm = CInt(Me.Cmb_Contatti.SelectedValue.Split("|")(0))

        Dim str As String = AgronicaCoreGestioneRichieste.RedirectGestione.IndirizzoCompleto_SitoOnline_PassandoDirettamente_ParametriGiasOnline(
                   Enum_SiteRedirector.Sito_AgronicaAgenda_2010,
                   objGiasOnline)




        Dim strJS As New StringBuilder
        strJS.AppendLine("$(document).ready(function () { ")
        strJS.AppendLine("      var listaDDT = window.open( '" & str & "','','dialogWidth:1000px; dialogHeight:700px; status:no; edge:raised; help:no; resizable:yes; '); ")
        'listaDDT è una stringa che contiene la lista dei ddt da allegare
        strJS.AppendLine(" });")

        ScriptManager.RegisterStartupScript(Master_Operazione.Property_UpdatePanelPerScript, Master_Operazione.Property_UpdatePanelPerScript.GetType(),
                                        String.Format("jQuery_{0}", Master_Operazione.Property_UpdatePanelPerScript.ClientID), strJS.ToString, True)



    End Sub



    Private Sub BottoneNascostoAllegaDDT_Click(sender As Object, e As System.EventArgs) Handles BottoneNascostoAllegaDDT.Click

        Dim MovimentiDettagliDDT As List(Of Movimento_Dettaglio)

        'modifica, permetto di aggiungere dettagli
        If Not IsNothing(Session("MovimentiDettagliDDT")) Then
            Try
                MovimentiDettagliDDT = Session("MovimentiDettagliDDT")
            Catch ex As Exception
                MovimentiDettagliDDT = New List(Of Movimento_Dettaglio)
            End Try

        Else
            MovimentiDettagliDDT = New List(Of Movimento_Dettaglio)
        End If

        Session("MovimentiDettagliDDT") = Nothing



        Dim Errore, Chiave_Oggetto, Stringa_Parametri_Base, Stringa_Parametri_Rif, Data, Username As String
        Dim Id_Riga, Tipo_Operazione, Flag_Errore As Integer


        Dim Unid As String = DDTAllegati.Value
        Try
            Dim o As New AgronicaCoreVarieDAL.Web_ComunicazionePagine_R()
            Dim Dt As DataTable
            Dt = o.Leggi(Unid, 0, AGRODATAINIZIO, AGRODATAFINE, enumSelezioneVariabile.Selezione_TabellaDatiMinimi, "", "", objParametri_Server)



            For i = 0 To Dt.Rows.Count - 1

                Unid = Dt.Rows(i).Item("Unid")
                Errore = Dt.Rows(i).Item("Errore")
                Chiave_Oggetto = Dt.Rows(i).Item("Chiave_Oggetto")
                Stringa_Parametri_Base = Dt.Rows(i).Item("Stringa_Parametri_Base")
                Stringa_Parametri_Rif = Dt.Rows(i).Item("Stringa_Parametri_Rif")
                Data = Dt.Rows(i).Item("Data")
                Username = Dt.Rows(i).Item("Username")

                Id_Riga = Dt.Rows(i).Item("Id_Riga")
                Tipo_Operazione = Dt.Rows(i).Item("Tipo_Operazione")
                Flag_Errore = Dt.Rows(i).Item("Flag_Errore")


                Try

                    If Stringa_Parametri_Base <> "" Then

                    End If

                    Dim XmlDoc As New XmlDocument
                    XmlDoc.LoadXml(Stringa_Parametri_Base)
                    Dim XML_MovimentoDettaglio As System.Xml.XmlElement
                    XML_MovimentoDettaglio = XmlDoc.SelectSingleNode("//Movimento_Dettaglio")

                    Dim Piva = CStr(XML_MovimentoDettaglio.GetAttribute("piva"))

                    Dim Sa_Cod = CInt(XML_MovimentoDettaglio.GetAttribute("sa_cod"))

                    Dim Id_Agenda = CInt(XML_MovimentoDettaglio.GetAttribute("id_agenda"))

                    Dim Id_Mov = CInt(XML_MovimentoDettaglio.GetAttribute("id_mov"))

                    Dim Id_Mov_Det = CInt(XML_MovimentoDettaglio.GetAttribute("id_mov_det"))

                    Dim Mat_Cod = CInt(XML_MovimentoDettaglio.GetAttribute("mat_cod"))

                    Dim Udm_Cod = CInt(XML_MovimentoDettaglio.GetAttribute("udm_cod"))

                    Dim Qta = CInt(XML_MovimentoDettaglio.GetAttribute("qta"))


                    Dim Lotto = CStr(XML_MovimentoDettaglio.GetAttribute("lotto"))

                    '------------------------

                    Dim x_Mov_Det_Des = XML_MovimentoDettaglio.GetAttribute("mov_det_des")

                    Dim movdet As Movimento_Dettaglio = New Movimento_Dettaglio()
                    movdet.Piva = Piva
                    movdet.Sa_Cod = Sa_Cod
                    movdet.Id_Agenda = Id_Agenda
                    movdet.Id_Mov = Id_Mov
                    movdet.Id_Mov_Det = Id_Mov_Det
                    movdet.Mat_Cod = Mat_Cod
                    movdet.Udm_Cod = Udm_Cod
                    'alla qta memorizzo la qta parziale utuilizzata del prodotto bolla e non la qta totale della riga bolla
                    ' movdet.Qta = Qta
                    movdet.Qta = 0 ' non dovùrebeb essere usata qui, ma solo quando leggo l'operazione per salvare il dato temporaneo
                    'e riassegnare le qta alle destinazioni
                    movdet.Lotto = Lotto

                    'controllo che non ci sia un ddt con lo stesso dettaglio di prodotto, cioè:
                    ' sesso: mat_cod, udm_cod, lotto
                    'Questo accade quando viene caricato un ddt stile agribologna con il LAN,
                    'in cui un prodotto con stassa udm e stesso lotto è possibile che sia presente in più righe del ddt.
                    'Anche il lan considera solo un unico dettaglio e salva il riferimento ad un unica riga del dettaglio, 
                    'probabilmente per lo stesso motivo per cui sono costretto a fare questa verifica anche io nell'agenda,
                    'Solo che il lan fa vedere le giacenze del documento considerando anche i dettagli delle altre righe doppie
                    Dim trovata As Boolean = False
                    For Each movin As Movimento_Dettaglio In MovimentiDettagliDDT
                        If movdet.Piva = movin.Piva AndAlso
                            movdet.Sa_Cod = movin.Sa_Cod AndAlso
                            movdet.Id_Agenda = movin.Id_Agenda AndAlso
                            movdet.Id_Mov = movin.Id_Mov AndAlso
                             movdet.Mat_Cod = movin.Mat_Cod AndAlso
                            movdet.Udm_Cod = movin.Udm_Cod AndAlso
                            movdet.Lotto = movin.Lotto Then
                            trovata = True
                        End If
                    Next

                    If trovata Then
                        Messaggi.AgroMsgBox(Resources.AgronicaAgenda_2010.AttenzioneSonoStateImportatePiùRigheDettag, Page, , Master_Operazione.Property_UpdatePanelToolBar)
                    Else
                        MovimentiDettagliDDT.Add(movdet)
                    End If


                Catch ex As Exception

                    Messaggi.AgroMsgBox(Resources.AgronicaAgenda_2010.ErroreDuranteLaLetturaDeiDatiRicevutiDalla, Page, , Master_Operazione.Property_UpdatePanelToolBar)

                End Try

            Next

        Catch ex As Exception

            Messaggi.AgroMsgBox(Resources.AgronicaAgenda_2010.ErroreDuranteLaLetturaDeiDatiRicevutiDalla, Page, , Master_Operazione.Property_UpdatePanelToolBar)

        Finally
            Dim o2 As New AgronicaCoreVarieDAL.Web_ComunicazionePagine_W()
            o2.CancellaTutti(Unid, objParametri_Server)
        End Try


        'se ho allegato dei ddt dettagli carico giacenze filtrandoli
        If MovimentiDettagliDDT.Count > 0 Then
            Session("MovimentiDettagliDDT") = MovimentiDettagliDDT
            ImageInfo0.Visible = True
            LabelInfo0.Visible = True
            caricaDtGiacenze()
            caricaListeValoriDaTabelle()

            'segnalo di checcare tutte le righe del magazzino
            caricaTabelle(True)

            CopiaGiacenzeBolleNelleTextbox()
        Else
            Session("MovimentiDettagliDDT") = Nothing
            ImageInfo0.Visible = False
            LabelInfo0.Visible = False
        End If

    End Sub

    Private Function ciSonoTrattamentiDiserbi(piva As String, sacod As Integer, appezza As Integer, idreg As Integer, iAgenda As String) As Boolean

        Dim o As New AgronicaCoreContabDAL.Mov_Destinazioni_R()
        Dim strFiltroLavCod As String = " Lav_Cod NOT IN (74,13,158,155,103,18) "

        Dim dt As DataTable = o.Leggi(piva, sacod, 0, 0, 0, appezza, idreg, 0, enumSelezioneVariabile.Selezione_TabellaCompleta, strFiltroLavCod, "", objParametri_Server)
        Try
            Dim drs() As DataRow = dt.Select("id_Agenda not in " & iAgenda)
            If drs.Count > 0 Then
                Return True
            End If
            Return False
        Catch ex As Exception
            Return True
        End Try

        Return False
    End Function


    Protected Sub CheckBoxSeminaMultiSpecie_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxSeminaMultiSpecie.CheckedChanged
        caricaDtGiacenze()
        caricaListeValoriDaTabelle()
        caricaTabelle(False)
    End Sub

    Protected Sub rbl_Specie_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rbl_Specie.SelectedIndexChanged

        If objParametriAgenda.TargetOperazione = enum_Tipo_Operazione_Agenda_Target.Reale Then
            caricaDtImpianti()
        Else
            caricaDtPlanning()
        End If

        caricaListeValoriDaTabelle()
        caricaTabelle(False)
    End Sub

    Private Function CreaAppezzamento(ByVal Piva As String, ByVal Sa_Cod As Integer, ByVal Campo_Cod As Integer, ByVal Appezza As Integer, ByVal Id_Reg As Integer,
                                      ByVal Validita_Inizio As Date, ByVal Validita_Fine As Date,
                                      ByVal Validita_Inizio_App As Date, ByVal Validita_Fine_App As Date,
                                      ByVal Validita_Inizio_Distinta As Date, ByVal Validita_Fine_Distinta As Date,
                                      ByVal App_Nome As String, ByVal Superficie As Decimal, ByVal P_Ha As Decimal,
                                      ByVal MetodoDiProduzione As Integer,
                                      ByVal Cul_Cod As Integer, ByVal Grfi_Cod As Integer,
                                      ByVal strProgettoNome As String, ByVal Dpi As String,
                                      ByRef AppezzaNEW As Integer, ByRef Id_RegNEW As Integer,
                                      ByRef DtNuoviImp As DataTable, ByVal SupOld As Decimal,
                                      ByVal AssociaCatasto As Boolean)


        Dim Dpi_Cod_New As Integer = 0
        Dim DpiPP_New As Integer = 0

        If CInt(Dpi.Split("/")(0)) = 0 Then
            Dpi_Cod_New = 0
            DpiPP_New = 0
        Else
            Dpi_Cod_New = CInt(Dpi.Split("/")(0))
            DpiPP_New = CInt(Dpi.Split("/")(1))
        End If

        Dim ZeroData As String = "0"
        Dim ZeroInt As Integer = 0
        Dim ZeroString As String = "0"
        Dim ZeroDouble As Double = 0
        Dim NullString As String = ""

        Dim StrCodice As String
        Dim StrCodici As String

        Dim BaseCode As Integer
        Dim TopCode As Integer

        Call Calcola_BaseCode_TopCode(BaseCode,
                                      TopCode,
                                      Session("ASG_ProgressivoGIAS"))

        '---------------------------------------------------------------------------------------
        '----- 1.  Creazione Appezzamento 
        '---------------------------------------------------------------------------------------

        Dim XmlDoc As New System.Xml.XmlDocument
        Dim XmlDatiAppezzamenti As System.Xml.XmlElement
        Dim StrAppezzamento As String
        Dim StrXmlInserisci As String


        'Genero la stringa XML
        Call XML_Appezzamento(enum_CodificaDecodifica.Codifica,
                             StrAppezzamento,
                              enum_TipoOperazioneDB.Scrittura,
                             Piva,
                             Sa_Cod,
                             ZeroInt,
                             Superficie,
                             ZeroData,
                             ZeroData,
                             ZeroDouble,
                             ZeroDouble,
                             ZeroDouble,
                             NullString,
                             ZeroDouble,
                             NullString,
                             ZeroInt,
                             NullString,
                             ZeroDouble,
                             ZeroDouble,
                             ZeroDouble,
                             ZeroDouble,
                             ZeroDouble,
                             ZeroDouble,
                             ZeroDouble,
                             ZeroDouble,
                             ZeroDouble,
                             ZeroDouble,
                             ZeroDouble,
                             ZeroDouble,
                             ZeroString,
                             ZeroInt,
                             ZeroDouble,
                             ZeroData,
                             ZeroDouble,
                             ZeroData,
                             ZeroData,
                             ZeroDouble,
                             ZeroData,
                             NullString,
                             App_Nome,
                             ZeroInt,
                             ZeroDouble,
                             NullString,
                             Campo_Cod,
                             ZeroInt,
                             ZeroData,
                             ZeroData,
                             Validita_Inizio_App,
                             Validita_Fine_App,
                             BaseCode,
                             TopCode)


        Call XML_Codice(enum_CodificaDecodifica.Codifica,
                 StrCodice,
                  enum_TipoOperazioneDB.Scrittura,
                 enum_CodiciAnagrafe.MetodoDiProduzione,
                 MetodoDiProduzione,
                 CDate("01/01/1900"),
                 CDate("31/12/2100"),
                 BaseCode,
                 TopCode,
                 "Appezzamento")

        StrCodici = StrCodici + StrCodice


        '------------------------------------------------
        '----- Costruisco la stringa XML complessiva di inserimento
        '------------------------------------------------

        'Creo il nodo "DatiAppezzamenti"
        XmlDatiAppezzamenti = XmlDoc.CreateElement("DatiAppezzamenti")

        'Inserisco il nodo "Appezzamento"
        XmlDatiAppezzamenti.InnerXml = StrAppezzamento

        'Rendo l'albero figlio del documento
        XmlDoc.AppendChild(XmlDatiAppezzamenti)

        'Seleziono il nodo Appezzamento e all'interno inserisco i nodi figli...sintassi xpath
        Dim xmlAppezzamento As System.Xml.XmlElement
        xmlAppezzamento = XmlDoc.SelectSingleNode("//Appezzamento")

        'Aaggiungo i codici creati in precedenza...
        'xmlAppezzamento.InnerXml = StrMetodoProduzione
        xmlAppezzamento.InnerXml = StrCodici

        'Estraggo la stringa XML complessiva
        StrXmlInserisci = XmlDoc.InnerXml

        'Distruggo gli oggetti
        XmlDatiAppezzamenti = Nothing
        XmlDoc = Nothing

        '------------------------------------------------
        '----- Inserisco l'APPEZZAMENTO
        '------------------------------------------------

        Dim objAppezzamentoW As New AgronicaCoreAnagrafeBIZ.Appezzamento_W
        objAppezzamentoW.Appezzamento_Scrivi(CStr(StrXmlInserisci),
                                                        "", 0, AppezzaNEW,
                                                        objParametri_Server,
                                                         objParametri_Utenti)

        'aggiungo l'intersezione con la particella se l'app precedente ne aveva una sola
        If AssociaCatasto = True Then

            Dim objAppxPart As New AgronicaCoreAnagrafeDAL.AppezzaxParticelle_R
            Dim appezzapart As New AgronicaCoreAnagrafeDAL.AppezzaxParticelle_W
            Dim dt_Part As DataTable = objAppxPart.LeggiParticelle_Da_Appezzamento(Piva, Sa_Cod, Appezza, enumSelezioneVariabile.Selezione_TabellaDatiMinimi, "", "", objParametri_Server)

            If Not IsNothing(dt_Part) AndAlso dt_Part.Rows.Count = 1 Then

                Dim SAU_Convenz_Ettari As Decimal = 0
                Dim SAU_Convenz_Are As Integer = 0
                Dim SAU_Convenz_Centiare As Integer = 0
                Dim SAU_Convers_Ettari As Decimal = 0
                Dim SAU_Convers_Are As Integer = 0
                Dim SAU_Convers_Centiare As Integer = 0
                Dim SAU_Bio_Ettari As Decimal = 0
                Dim SAU_Bio_Are As Integer = 0
                Dim SAU_Bio_Centiare As Integer = 0

                Select Case MetodoDiProduzione
                    Case enum_MetodoProduzione.Convenzionale
                        AgronicaCoreDataProvider.Conversioni.EttariAreCentiare_from_Ettari(Superficie, SAU_Convenz_Ettari, SAU_Convenz_Are, SAU_Convenz_Centiare)
                    Case enum_MetodoProduzione.InConversione
                        AgronicaCoreDataProvider.Conversioni.EttariAreCentiare_from_Ettari(Superficie, SAU_Convers_Ettari, SAU_Convers_Are, SAU_Convers_Centiare)
                    Case enum_MetodoProduzione.Biologico
                        AgronicaCoreDataProvider.Conversioni.EttariAreCentiare_from_Ettari(Superficie, SAU_Bio_Ettari, SAU_Bio_Are, SAU_Bio_Centiare)
                End Select

                appezzapart.Scrivi(
                     Piva,
                    Sa_Cod,
                    AppezzaNEW,
                        dt_Part.Rows(0).Item("prov"),
                        dt_Part.Rows(0).Item("com"),
                        dt_Part.Rows(0).Item("sezione"),
                        dt_Part.Rows(0).Item("foglio"),
                        dt_Part.Rows(0).Item("numero"),
                        dt_Part.Rows(0).Item("subalterno"),
                            Superficie,
                            SAU_Convenz_Ettari,
                            SAU_Convenz_Are,
                            SAU_Convenz_Centiare,
                            SAU_Convers_Ettari,
                            SAU_Convers_Are,
                            SAU_Convers_Centiare,
                            SAU_Bio_Ettari,
                            SAU_Bio_Are,
                            SAU_Bio_Centiare,
                                dt_Part.Rows(0).Item("validita_inizio"),
                                dt_Part.Rows(0).Item("validita_fine"),
                                objParametri_Server)

            End If
        End If


        Dim StrImpianto As String
        Dim StrXmlInserisciImpianto As String
        Dim XmlDocI As New System.Xml.XmlDocument
        Dim XmlDatiReg_Impianti As System.Xml.XmlElement

        '---------------------------------------------------------------------------------------
        '----- 2.  Creazione Impianto 
        '---------------------------------------------------------------------------------------

        'Genero la stringa XML
        Call XML_Impianto(enum_CodificaDecodifica.Codifica,
                            StrImpianto,
                             enum_TipoOperazioneDB.Scrittura,
                            Piva,
                            Sa_Cod,
                            Campo_Cod,
                            AppezzaNEW,
                            0,
                            Superficie,
                            0,
                            0,
                            0,
                            Validita_Inizio,
                            Cul_Cod,
                            0,
                            "0",
                            "0",
                            0,
                            0,
                            0,
                            "0",
                            "",
                            "0",
                            "0",
                            0,
                            0,
                            0,
                            -1,
                            -1,
                            0,
                            0,
                            0,
                            0,
                            0,
                            0,
                            "",
                            0,
                            0,
                            0,
                            0,
                            0,
                            "",
                            "0",
                            Grfi_Cod,
                            -1,
                            1,
                            0,
                            -1,
                            -1,
                            -1,
                            -1,
                            0,
                            Validita_Inizio,
                            Validita_Fine,
                            BaseCode,
                            TopCode)



        '------------------------------------------------
        '----- Costruisco la stringa XML complessiva di inserimento
        '------------------------------------------------

        'Creo il nodo "DatiReg_Impianti"
        XmlDatiReg_Impianti = XmlDocI.CreateElement("DatiReg_Impianti")

        'Inserisco il nodo "Impianto"
        XmlDatiReg_Impianti.InnerXml = StrImpianto

        'Rendo l'albero figlio del documento
        XmlDocI.AppendChild(XmlDatiReg_Impianti)

        'Estraggo la stringa XML complessiva
        StrXmlInserisciImpianto = XmlDocI.InnerXml


        '------------------------------------------------
        '----- Modifico o Inserisco l'IMPIANTO
        '------------------------------------------------

        'Creo gli oggetti COM+
        Dim objImpianto As New AgronicaCoreAnagrafeBIZ.Reg_Impianto_W
        Dim InseritoImp As Boolean = False

        'Eseguo i comandi XML
        InseritoImp = objImpianto.Reg_Impianto_Scrivi(CStr(StrXmlInserisciImpianto),
                                                    "", 0, 0, Id_RegNEW,
                                                    "", objParametri_Server)

        Dim SupPerc As Integer = 0

        If InseritoImp = True Then

            SupPerc = Superficie * 100 / SupOld

            Dim dr As DataRow = DtNuoviImp.NewRow
            dr.Item("piva") = Piva
            dr.Item("sa_cod") = Sa_Cod
            dr.Item("appezza_old") = Appezza
            dr.Item("id_reg_old") = Id_Reg
            dr.Item("appezza_new") = AppezzaNEW
            dr.Item("id_reg_new") = Id_RegNEW
            dr.Item("sup_new") = Superficie
            dr.Item("sup_perc") = SupPerc
            DtNuoviImp.Rows.Add(dr)

            'If Not NuoviImp.ContainsKey(Piva & "_" & Sa_Cod & "_" & AppezzaNEW & "_" & Id_RegNEW & "_" & Superficie.ToString & "_" & SupPerc.ToString) Then
            '    NuoviImp.Add(Piva & "_" & Sa_Cod & "_" & AppezzaNEW & "_" & Id_RegNEW & "_" & Superficie.ToString & "_" & SupPerc.ToString, "")
            'End If
        End If

        '------------------------------------------------------------
        '----- Modifico o Inserisco il PROGETTO legato all'impianto
        '------------------------------------------------------------

        Dim objProgetto As New AgronicaCoreAnagrafeBIZ.Progetto_W
        Dim StrXmlInserisciProgetto As String
        Dim StrDummyP As String

        Dim objXML As New AgronicaCoreXML.AnagrafeXML

        objXML.Xml_ProgettoPerImpianto(enum_CodificaDecodifica.Codifica,
                                       StrXmlInserisciProgetto,
                                        enum_TipoOperazioneDB.Scrittura,
                                                Piva,
                                                Sa_Cod,
                                                0,
                                                strProgettoNome,
                                                strProgettoNome,
                                                CAU_PROGETTO_PRODUZIONE,
                                                 0,
                                                0,
                                                AGRODATAINIZIO,
                                                AGRODATAFINE,
                                                "",
                                                AppezzaNEW,
                                                Id_RegNEW,
                                                0,
                                                0,
                                                0,
                                                1,
                                                Dpi_Cod_New,
                                                0,
                                                0,
                                                0,
                                                0,
                                                Validita_Inizio_Distinta,
                                                Validita_Fine_Distinta,
                                                BaseCode,
                                                TopCode, AGRODATAINIZIO,
                                                    P_Ha, DpiPP_New)

        'Eseguo i comandi XML
        StrDummyP = objProgetto.Impresa_Progetto_Scrivi(CStr(StrXmlInserisciProgetto),
                                                        "", 0, objParametri_Server)



    End Function

    Protected Sub RBL_Tipo_Semina_SelectedIndexChanged(sender As Object, e As EventArgs) Handles RBL_Tipo_Semina.SelectedIndexChanged

        If objParametriAgenda.TargetOperazione = enum_Tipo_Operazione_Agenda_Target.Reale Then
            caricaDtImpianti()
            AggiornaVisibilitaColonneSemineImpianti()
        Else
            caricaDtPlanning()
        End If

        caricaDtGiacenze()
        caricaListeValoriDaTabelle()
        caricaTabelle(False)

    End Sub

    Private Function FrazionaAgenda(ByVal HashAppOldSelezionati As Hashtable, DtNuoviImp As DataTable, ByVal Id_Agenda_DaEscludere As Integer)

        Dim ObjDestR As New AgronicaCoreContabDAL.Mov_Destinazioni_R
        Dim objDestW As New AgronicaCoreContabDAL.Mov_Destinazioni_W

        Dim BaseCode As Integer
        Dim TopCode As Integer

        '------------------------------------------------
        '----- Calcolo i valori di BaseCode e TopCode
        '------------------------------------------------
        Call Calcola_BaseCode_TopCode(BaseCode,
                              TopCode,
                              Session("ASG_ProgressivoGIAS"))

        'se l'appezzamento precedente è stato frazionato in diversi appezzamenti
        'verifico se c'erano operazioni registrate sopra
        'in tal caso le fraziono
        For Each Key In HashAppOldSelezionati

            Dim DrImp() As DataRow = DtNuoviImp.Select("piva='" & Split(Key.key, "_")(0) & "' and sa_cod=" & Split(Key.key, "_")(1) &
                                                       " and appezza_old=" & Split(Key.key, "_")(3) & " and id_reg_old=" & Split(Key.key, "_")(4))

            If Not IsNothing(DrImp) AndAlso DrImp.Length > 0 Then

                'escludo la semina nuova (Id_Agenda_DaEscludere)
                Dim DtDest As DataTable
                DtDest = ObjDestR.Leggi(CStr(Split(Key.key, "_")(0)),
                                 CInt(Split(Key.key, "_")(1)),
                                 0,
                                 0,
                                 0,
                                 CInt(Split(Key.key, "_")(3)),
                                 CInt(Split(Key.key, "_")(4)),
                                 0,
                                 enumSelezioneVariabile.Selezione_JoinDescrizioni,
                                 " Mov_Destinazioni.Id_Agenda<>" & Id_Agenda_DaEscludere,
                                 "",
                                 objParametri_Server)

                Dim NewPiva As String
                Dim NewSaCod As String
                Dim NewAppezza As String
                Dim NewIdReg As String
                Dim NewSup As String
                Dim NewPercentuale As String
                Dim NewAppNome As String

                Dim Id_Agenda As Integer
                Dim Lav_Des As String

                Dim DesLibOld As String

                Dim objAgendaR As New AgronicaCoreContabBIZ.Agenda_R
                Dim objAgendaW As New AgronicaCoreContabBIZ.Agenda_W

                Dim StringaXML As String = ""
                Dim StringaXMLNew As String = ""
                Dim StringaXMLNewTmp As String = ""

                Dim Id_Agenda_New As Integer

                Dim idAgendaProcessati As New ArrayList

                For i = 0 To DtDest.Rows.Count - 1

                    Id_Agenda = DtDest.Rows(i).Item("id_agenda")

                    If Not idAgendaProcessati.Contains(Id_Agenda) Then

                        idAgendaProcessati.Add(Id_Agenda)

                        DesLibOld = DtDest.Rows(i).Item("Des_Lib")
                        Lav_Des = DtDest.Rows(i).Item("Lav_Des")

                        StringaXML = objAgendaR.Agenda_Leggi(Split(Key.key, "_")(0), 0, Id_Agenda, 0,
                                                    True,
                                                    objParametri_Server)

                        StringaXMLNew = StringaXML

                        'Des_Lib
                        'StringaXMLNew = Replace(StringaXMLNew, DesLibOld, DesLibNew)

                        'Reimpostazione del Nuovo TipoOperazioneDB / BaseCode / TopCode
                        StringaXMLNew = Replace(StringaXMLNew, "TipoOperazioneDB=""3""", "TipoOperazioneDB =""1"" basecode = """ & BaseCode & """ topcode = """ & TopCode & """")

                        'Nota: Il trucco è di rendere negativi i codici in modi tale che il componente ne crei dei nuovi
                        'Id_Agenda, Id_Mov, Id_Mov_Det, Id_Reg_Dettaglio
                        StringaXMLNew = Replace(StringaXMLNew, "id_agenda=""", "id_agenda=""-")
                        StringaXMLNew = Replace(StringaXMLNew, "id_mov=""", "id_mov=""-")
                        StringaXMLNew = Replace(StringaXMLNew, "id_mov_det=""", "id_mov_det=""-")
                        StringaXMLNew = Replace(StringaXMLNew, "id_reg_dettaglio=""", "id_reg_dettaglio=""-")

                        For a = 0 To DrImp.Length - 1

                            StringaXMLNewTmp = StringaXMLNew

                            NewPiva = DrImp(a).Item("Piva")
                            NewSaCod = DrImp(a).Item("sa_cod")
                            NewAppezza = DrImp(a).Item("appezza_new")
                            NewIdReg = DrImp(a).Item("id_reg_new")
                            NewSup = DrImp(a).Item("sup_new")
                            NewPercentuale = DrImp(a).Item("sup_perc")
                            NewAppNome = "" 'ArrayImp(6)

                            'deslib
                            'DesLibNew = Lav_Des & " (" & NewAppNome & ")"
                            'StringaXMLNewTmp = Replace(StringaXMLNewTmp, DesLibOld, DesLibNew)

                            'destinazione
                            StringaXMLNewTmp = Replace(StringaXMLNewTmp, "appezza=""" & DrImp(a).Item("appezza_old") & """ id_destinazione=""" & DrImp(a).Item("id_reg_old") & """ tipo_destinazione=""0""", "tipo_destinazione=""0"" id_destinazione=""" & NewIdReg & """ appezza=""" & NewAppezza & """")

                            objAgendaW.Agenda_Scrivi(StringaXMLNewTmp, Id_Agenda_New, 0, 0, 0, "", objParametri_Server)

                            'modifica qta dettagli NON movimento lavorazione (in cui i dosaggi sono salvati as ETTARO)
                            Dim objDett As New AgronicaCoreContabDAL.Movimenti_Dettagli_W
                            objDett.Modifica_Quantita_DaPercentuale(DrImp(a).Item("Piva"), Id_Agenda_New, 0, 0, NewPercentuale, "", objParametri_Server)

                            'modifica qta/qta2 destinazioni
                            Dim objDest As New AgronicaCoreContabDAL.Mov_Destinazioni_W
                            'objDest.Modifica_Quantita_e_SupTrattata_DaPercentuale(Piva, Id_Agenda_New, 0, 0, NewAppezza, NewIdReg, NewPercentuale, "", objParametri_Server)
                            objDest.Modifica_Quantita_e_SupTrattata_DaPercentuale(DrImp(a).Item("Piva"), Id_Agenda_New, 0, 0, 0, 0, NewPercentuale, "", objParametri_Server)

                            'modifica qta_ril Mov_Dettaglio_Tecnico (acqua)
                            Dim objDettTec As New AgronicaCoreContabDAL.Mov_Dettaglio_Tecnico_W
                            objDettTec.Modifica_QuantitaAcqua_DaPercentuale(DrImp(a).Item("Piva"), Id_Agenda_New, NewPercentuale, "", objParametri_Server)

                        Next

                        'cancello l'operazione vecchia
                        objAgendaW.Agenda_Scrivi(StringaXML, 0, 0, 0, 0, "", objParametri_Server)

                    End If

                Next

            End If
        Next

    End Function


End Class



Public Structure ValoriImpiantoESemineImpostati

    Public Piva As String
    Public Sa_Cod As Integer
    Public Appezza As Integer
    Public Id_Reg As Integer

    Public Programmazione_Entita_cod As Integer

    Public Tra_Fila As String
    Public Su_Fila As String
    Public Interbina As String
    Public Germinabilita As String
    Public Specie As String
    Public Varieta As String
    Public Finalita As String
    Public Disciplinare As String

    Public Elem_Cod As String
    Public Pro_Cod As String
    Public Mat_Cod As String
    Public UDM_Cod As String
    Public Lotto As String
    Public Bolla As String

    Public QTA_Dest As String

    Public QTA_Tot_Prodotto As String


End Structure







