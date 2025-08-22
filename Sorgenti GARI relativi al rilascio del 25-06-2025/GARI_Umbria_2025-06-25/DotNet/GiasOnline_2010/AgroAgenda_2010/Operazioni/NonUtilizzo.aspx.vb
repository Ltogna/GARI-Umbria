Imports AgronicaCoreModello.ParametriAgenda_Temp
Imports AgronicaCoreDataProvider.TipiEnumerativi
Imports AgronicaCoreDataProvider
Imports AgronicaCoreDataProvider.CostantiPersonalizzate
Imports AgronicaCoreDataProvider.UtilityProvider
Imports AgronicaCoreUtility
Imports AgronicaCoreModello.OperazioneAgenda_Temp
Imports AgronicaCoreDataProvider.AgronicaCoreParametri
Imports System.Drawing

Public Class NonUtilizzo
    Inherits System.Web.UI.Page

    Public Master_Operazione As Operazione

    Private Shared Function GetDoorkey() As String
        Dim Doorkey As String = "Y4h8u3B5w2"
        Return Doorkey
    End Function

    Protected Overrides Sub InitializeCulture()
        MyBase.InitializeCulture()
        Lingua.Gias_InizializzaCultura_DaSession()

    End Sub



    Dim objParametri_Server, objParametri_Utenti As AgronicaCoreParametri
    Dim objParametriAgenda As ParametriAgenda
    Dim Id_Agenda_Old As Integer

    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Master_Operazione = CType(Page.Master, Operazione)

        'AddHandler Master_Operazione.Property_BTN_ChangeData.Click, AddressOf Me.aggiornaPerDataScadenza
        AddHandler Master_Operazione.Property_ImgBtn_AnnullaTutto.Click, AddressOf Me.AnnullaTutto
        AddHandler Master_Operazione.Property_BTN_CentroAziendale.Click, AddressOf Me.CambioCentro
        AddHandler Master_Operazione.Property_ImgBtn_Salva.Click, AddressOf Me.SalvaTutto
        AddHandler Master_Operazione.Property_BTN_ChangeData.Click, AddressOf Me.cambioData
        AddHandler CType(Page.Master, MasterPage).PreRender, AddressOf Me.MasterUnload

       

    End Sub


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        inizializzoObjParametri()
        inizializzoParametriPagina()

        If Not Page.IsPostBack Then
            caricaControlli()
            disabilitaControlli()
            cambioData()
        End If
    End Sub


    Private Sub cambioData()

        helperUI.VerificaNonUtilizzo(2, objParametriAgenda.Lav_Cod, objParametriAgenda.Piva, lErroriSegnalazioni, objParametriAgenda.Data, objParametri_Server, objParametri_Utenti)


    End Sub

    Private Sub Ready1()

        Dim strJS As New StringBuilder
        strJS.AppendLine("$(document).ready(function () { ")
        strJS.AppendLine("     $('#chkSelezionaTuttiCentri').click(function (){ ")
        strJS.AppendLine("         SelezionaDeselezionaTuttiCentri();")
        strJS.AppendLine("      });")
        strJS.AppendLine("     $('#chkSelezionaTuttiRilievi').click(function (){ ")
        strJS.AppendLine("         SelezionaDeselezionaTuttiRilievi();")
        strJS.AppendLine("       });")
        strJS.AppendLine("     $('.ChkSelezionaRilievo').click(function (){ ")
        strJS.AppendLine("         ChkSelezionaRilievo_Click();")
        strJS.AppendLine("     });")


        strJS.AppendLine("  $.datepicker.regional['it'];")



        strJS.AppendLine(" });")
        ScriptManager.RegisterStartupScript(Master_Operazione.Property_UpdatePanelPerScript, Master_Operazione.Property_UpdatePanelPerScript.GetType(),
                                      String.Format("jQuery_{0}", Master_Operazione.Property_UpdatePanelPerScript.ClientID), strJS.ToString, True)


    End Sub


    Private Sub inizializzoParametriAgenda()
        objParametriAgenda = New ParametriAgenda
        'objParametriAgenda.Leggi()
        Id_Agenda_Old = objParametriAgenda.Id_Agenda
        objParametriAgenda.OperazioneMulticentro = True
    End Sub


    Private Sub inizializzoParametriPagina()

        inizializzoParametriAgenda()
    End Sub


    Private Sub inizializzoObjParametri()
        '---
        objParametri_Server = New AgronicaCoreDataProvider.AgronicaCoreParametri(Session("ASG_objParametri_Server"))
        objParametri_Utenti = New AgronicaCoreDataProvider.AgronicaCoreParametri(Session("ASG_objParametri_Utenti"))
        '---
    End Sub

    Private Sub AnnullaTutto(ByVal sender As Object, ByVal e As System.EventArgs)
        RipristinaSessione()
        Dim objParametriAgenda As New ParametriAgenda
        Dim link As String = ""
        Try
            Dim sitoorigine As Enum_SiteRedirector = HttpContext.Current.Session("Sito_Origine")
            Dim paginaOnLineRitorno As Integer = objParametriAgenda.PaginaSitoOrigine

            If sitoorigine = Enum_SiteRedirector.Sito_GiasOnline_2010 And paginaOnLineRitorno = enum_PagineGiasOnline_2010.RegistazioneSmart Then
                link = AgronicaCoreGestioneRichieste.RedirectGestione.IndirizzoCompleto_SitoOnline2010_PassandoDirettamenteIParametri( _
                                       Enum_SiteRedirector.Sito_AgronicaAgenda_2010, _
                                       enum_PagineGiasOnline_2010.RegistazioneSmart, _
                                       enum_PagineAgenda_2010.Menu, objParametriAgenda.Piva, "", "", 0, "")

            Else
                link = CType(Master.Master, Agenda).TrovaRedirectCorretto(False, enum_PagineGiasOnline.MenuAgenda, objParametriAgenda)
            End If

        Catch ex As Exception
            link = CType(Master.Master, Agenda).TrovaRedirectCorretto(False, enum_PagineGiasOnline.MenuAgenda, objParametriAgenda)
        End Try

        Response.Redirect(link)
    End Sub


    Private Sub RipristinaSessione()

        ripristinaObjParametriAgenda()
    End Sub


    Private Sub ripristinaObjParametriAgenda()
        objParametriAgenda.Svuota_DatiOperazione()
        objParametriAgenda.OperazioneMulticentro = True
    End Sub

    Private Sub CambioCentro(sender As Object, e As EventArgs)

    End Sub


    Private Sub MasterUnload(sender As Object, e As EventArgs)

    End Sub


    Public Sub AbilitaTutto(ByVal abilita As Boolean)
        Me.sfondoverdeCentri.Visible = abilita
        Me.IMGB_InserisciCentri.Enabled = abilita
        Me.IMGB_InserisciCentri.Visible = abilita
        Me.IMGB_RimuoviCentri.Enabled = abilita
        Me.IMGB_RimuoviCentri.Visible = abilita
    End Sub


    Private Sub disabilitaControlli()


        Master_Operazione.Property_Div_ProvenienzaRisorse.Visible = False
        Master_Operazione.Property_Div_Specie.Visible = False
        Master_Operazione.Property_GridView_Impianti.Visible = False        
        'Master_Operazione.Property_CBL_Consigli.Visible = False        

        Me.sfondoverdeCentri.Visible = True



        'impostazioni in base operazione di creazione/modifica..
        Select Case objParametriAgenda.Tipo_Operazione

            'impostazioni in base alla lavorazione


            Case TipiEnumerativi.enum_TipoOperazioneDB.Scrittura

                Master_Operazione.Property_RBL_Salva.Visible = True
                Master_Operazione.Property_Box_Salva.Visible = True



                Me.sfondoverdeCentri.Visible = True

                AbilitaStep1(False)

            Case TipiEnumerativi.enum_TipoOperazioneDB.Modifica

                AbilitaTutto(False)

                Master_Operazione.Property_Box_Salva.Enabled = True 'true per modifica
                Master_Operazione.Property_Box_Salva.Visible = True 'true per modifica
                Master_Operazione.Property_ImgBtn_Salva.Enabled = True 'true per modifica
                Master_Operazione.Property_RBL_Salva.Enabled = False



                'Master_Operazione.Property_CBL_Consigli.Enabled = True

                GridView_NonUtilizzo.Visible = True
                GridView_NonUtilizzo.Enabled = True


            Case TipiEnumerativi.enum_TipoOperazioneDB.Lettura

                AbilitaTutto(False)
                'Master_Operazione.Property_CBL_Consigli.Enabled = False
                GridView_NonUtilizzo.Visible = True

        End Select




    End Sub


    Private Sub AbilitaStep1(ByVal abilita As Boolean)

        'da visualizzare quando si preme
        Me.IMGB_RimuoviCentri.Visible = abilita

        'da nascondere quando si preme
        Me.LabelInserisciCentri.Visible = Not abilita
        Me.IMGB_InserisciCentri.Visible = Not abilita



    End Sub


#Region "Caricamento"

    Private Sub caricaControlli()

        


        Select Case objParametriAgenda.Tipo_Operazione

            Case TipiEnumerativi.enum_TipoOperazioneDB.Scrittura

            Case TipiEnumerativi.enum_TipoOperazioneDB.Modifica, TipiEnumerativi.enum_TipoOperazioneDB.Lettura
                RipristinaControlliDaAgenda()
                Master_Operazione.CaricaCostiAccessori()


        End Select
    End Sub

#End Region

#Region "Sequenza Caricamento operazione da DB"


    Private Sub RipristinaControlliDaAgenda()


        Dim objAgenda As New Agenda_Operazione_Helper
        Dim Agenda As New Operazione_Agenda
        Agenda = objAgenda.Leggi(objParametriAgenda.Piva, _
                                     CInt(objParametriAgenda.Sa_Cod), _
                                     CInt(objParametriAgenda.Id_Agenda), _
                                     0, _
                                     objParametri_Server)


        If Not IsNothing(Agenda) Then

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

            'MOVIMENTI
            If Not IsNothing(Agenda.Movimenti) Then

                Dim MovimentiCosti As List(Of Movimento) = New List(Of Movimento)

                Dim letto As Boolean = False
                For i = 0 To Agenda.Movimenti.Count - 1

                    'controllo corrispondeza piva con agenda
                    If Agenda.Piva <> Agenda.Movimenti(i).Piva Then
                        Throw New ApplicationException
                    End If

                    Select Case Agenda.Movimenti(i).Cau_Mov

                        Case enum_Agenda_Causali.RILIEVO_CAMPO, enum_Agenda_Causali.TRATTAMENTO
                            If letto Then
                                Throw New NotImplementedException
                            End If
                            letto = True

                            If Agenda.Lav_Cod <> objParametriAgenda.Lav_Cod Then
                                'controllino per lo sviluppo, da togliere
                                Throw New NotImplementedException
                            End If

                         
                            LeggiMovimentoAgenda_NonUtilizzo(Agenda, i)
                         



                        Case Else
                            Throw New NotImplementedException

                    End Select
                Next

                objParametriAgenda.Movimenti = MovimentiCosti

            End If
        End If




    End Sub

    Private Sub LeggiMovimentoAgenda_NonUtilizzo(ByRef agenda As Operazione_Agenda, ByRef i As Integer)

        Dim Piva As String = agenda.Piva
        Dim Sacod As Integer = agenda.Sa_Cod
        Dim Data As Date = agenda.Movimenti(i).Data
        If agenda.Data <> agenda.Movimenti(i).Data Then
            Throw New NotImplementedException
        End If


        Dim dt As DataTable = InizializzaDataTableNU()
        '----------------------------------------------------------------------
        '----------------Carico i dati.................................--------
        '----------------------------------------------------------------------

        Dim row As DataRow


        row = dt.NewRow
        row.Item("PIVA") = Piva
        row.Item("Rag_Soc") = (New AgronicaCoreAnagrafeDAL.Imprese_Read().RagSoc_from_Piva(Piva, objParametri_Server))
        row.Item("Sa_Cod") = Sacod
        row.Item("Sa_nome") = (New AgronicaCoreAnagrafeDAL.CentriAziendali_Read()).SaNome_from_SaCod(Piva, Sacod, objParametri_Server)        

        dt.Rows.Add(row)

        finalizzaDataTableNU(dt, GridView_NonUtilizzo)

    End Sub

    Private Sub finalizzaDataTableNU(Dt As DataTable, GW As GridView)

        ' ''Vettore di DataColumn

        Dim DtKeys(2) As String

        DtKeys(0) = "PIVA"
        DtKeys(1) = "Sa_Cod"
        DtKeys(2) = "id_agenda"

        GridView_NonUtilizzo.DataKeyNames = DtKeys

        GW.DataSource = Dt
        GW.DataBind()



    End Sub

    Private Function InizializzaDataTableNU() As DataTable
        Dim Dt As New DataTable("Piogge")

        Dt.Columns.Add("piva", GetType(String))
        Dt.Columns.Add("Rag_Soc", GetType(String))
        Dt.Columns.Add("sa_cod", GetType(Integer))
        Dt.Columns.Add("Sa_Nome", GetType(String))
        Dt.Columns.Add("id_Agenda", GetType(String))


        Return Dt
    End Function


#End Region


#Region "Salvataggio"

    Private Sub SalvaTutto(ByVal sender As Object, ByVal e As System.EventArgs)

        If GridView_NonUtilizzo.Rows.Count = 0 Then
            Inserisci_Centri()
        End If

        Dim messaggio_errore As String = ""
        If SalvaOperazioneAgenda(messaggio_errore) Then
            'premutosalva = True
            gestisciTipoSalvataggio()
        Else
            Messaggi.AgroMsgBox(Resources.AgronicaAgenda_2010.NonÈStatoPossibileCreareLOperazione & messaggio_errore, Page, , _
                  Master_Operazione.Property_UpdatePanelToolBar)
        End If
    End Sub


    Private Function SalvaOperazioneAgenda(ByRef messaggio_errore As String) As Boolean
        Dim res As Boolean = False




        Try


            '-----------------------------------------------------
            '----------- CONNESSIONE E TRANSAZIONE ---------------
            Dim objDP As New AgronicaCoreDataProvider.ConnessioniTransazioni
            ConnessioniTransazioni.ApriConnessione(True, objParametri_Server)
            '-----------------------------------------------------

            Dim Piva As String = 0
            Dim Sacod As Integer = 0
            Dim Data As String = "0"
            Dim Ora As String = "0"
            Dim Minuti As String = "0"
            Dim Pioggia As String = "0"
            Dim Note As String = "0"
            Dim TMin As String = "0"
            Dim TMax As String = "0"
            Dim Umidita As String = "0"
            Dim RigheSelezionate As Integer = 0

            For i = 0 To GridView_NonUtilizzo.Rows.Count - 1


                If CType(GridView_NonUtilizzo.Rows(i).FindControl("ChkSelezionaRilievo"), CheckBox).Checked = True Then


                    Piva = CStr(GridView_NonUtilizzo.DataKeys(i).Item("PIVA"))
                    Sacod = CInt(GridView_NonUtilizzo.DataKeys(i).Item("Sa_Cod"))
                    Data = Master_Operazione.Property_txt_DataOperazione.Text
                    Note = Master_Operazione.Property_txt_Note.Text

                    If objParametriAgenda.Lav_Cod = LAVCOD_FERTILIZZAZIONI_DICHIARAZIONE_NON_UTILIZZO Then
                        objParametriAgenda.Cau_Mov = "2100"
                    Else
                        objParametriAgenda.Cau_Mov = "2050"
                    End If




                    '-------------------------------------------------------------------------------
                    '-------Salvataggio di una operazione agenda per riga--------------------
                    '-------------------------------------------------------------------------------
                    Dim Agenda As Operazione_Agenda = CreaOggettoAgenda(Piva, Sacod, Data, Ora, Minuti, Pioggia, Note, TMin, TMax, Umidita, messaggio_errore)
                    If IsNothing(Agenda) Then
                        Throw New Exception(Resources.AgronicaAgenda_2010.NonÈStatoPossibileCreareLOperazione)
                    End If


                    'se  esiste l'operaznione di rilievo piogge fatta lo stesso giorno fermo l'operazione e visualizzo errore
                    If objParametriAgenda.Tipo_Operazione = enum_TipoOperazioneDB.Scrittura Then
                        Dim filtro As String = " Validita_Inizio =  " & Agro_SQL_SaveDate(CDate(Data)) & "  "
                        Dim dt As DataTable = New AgronicaCoreContabDAL.Agenda_R().Leggi(Piva, Sacod, 0, Agenda.Lav_Cod, enumSelezioneVariabile.Selezione_TabellaCompleta, filtro, "", objParametri_Server)
                        If dt.Rows.Count > 0 Then

                            messaggio_errore &= String.Format(Resources.AgronicaAgenda_2010.EsistonoGiàX0RilieviPioggeInDataX1PerIlCen, dt.Rows.Count, Data, (New AgronicaCoreAnagrafeDAL.CentriAziendali_Read()).SaNome_from_SaCod(Piva, Sacod, objParametri_Server))
                            messaggio_errore &= String.Format(Resources.AgronicaAgenda_2010.DeselezionareLaRigaX0OEliminareIlPrecedent, (i + 1))
                            Throw New Exception(messaggio_errore)
                        End If
                    End If

                    'In modifica escludo l'id agenda corrente che naturalmente avrà la stessa data
                    If objParametriAgenda.Tipo_Operazione = enum_TipoOperazioneDB.Modifica Then
                        Dim filtro As String = " Validita_Inizio =  " & Agro_SQL_SaveDate(CDate(Data)) & " AND not Agenda.Id_Agenda = " & Agro_SQL_SaveNum(objParametriAgenda.Id_Agenda) & "  "
                        Dim dt As DataTable = New AgronicaCoreContabDAL.Agenda_R().Leggi(Piva, Sacod, 0, Agenda.Lav_Cod, enumSelezioneVariabile.Selezione_TabellaCompleta, filtro, "", objParametri_Server)
                        If dt.Rows.Count > 0 Then


                            messaggio_errore &= String.Format(Resources.AgronicaAgenda_2010.EsistonoGiàX0RilieviPioggeInDataX1PerIlCen, dt.Rows.Count, Data, (New AgronicaCoreAnagrafeDAL.CentriAziendali_Read()).SaNome_from_SaCod(Piva, Sacod, objParametri_Server))
                            messaggio_errore &= String.Format(Resources.AgronicaAgenda_2010.DeselezionareLaRigaX0OEliminareIlPrecedent, (i + 1))
                            Throw New Exception(messaggio_errore)

                        End If
                    End If


                    Dim objAgendaScrivi As New Agenda_Operazione_Helper
                    Dim Id_Agenda As Integer

                    If objParametriAgenda.Tipo_Operazione = enum_TipoOperazioneDB.Modifica Then

                        Dim CancellataOperazione As Boolean = False
                        CancellataOperazione = objAgendaScrivi.Cancella(objParametriAgenda.Piva,
                                                                         objParametriAgenda.Sa_Cod,
                                                                         objParametriAgenda.Id_Agenda, False,
                                                                         objParametri_Server, logCancellazione:=False)
                    End If

                    Id_Agenda = objAgendaScrivi.Scrivi(Agenda, objParametri_Server)




                    RigheSelezionate += 1



                End If

            Next


            ''chiudi connessione e commit transazione
            'ConnessioniTransazioni.ChiudiTransazione(1, objParametri_Server)
            'ConnessioniTransazioni.ChiudiConnessione(objParametri_Server)

            If RigheSelezionate = 0 Then
                messaggio_errore &= Resources.AgronicaAgenda_2010.NonÈStatoSelezionatoNessunRilievo
                Throw New Exception(messaggio_errore)
            End If







            res = True


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



    Private Sub gestisciTipoSalvataggio()
        Session("UtilizzataRicetta") = False
        Dim TipoSalvataggio As String = Master_Operazione.Property_RBL_Salva.SelectedValue + 1
        Select Case TipoSalvataggio
            Case enum_Tipo_Salvataggio.Salva_e_Esci
                Dim strJS As New StringBuilder
                strJS.AppendLine("$(document).ready(function () { ")


                Dim link As String = ""
                Try
                    Dim sitoorigine As Enum_SiteRedirector = HttpContext.Current.Session("Sito_Origine")
                    Dim paginaOnLineRitorno As Integer = objParametriAgenda.PaginaSitoOrigine

                    If sitoorigine = Enum_SiteRedirector.Sito_GiasOnline_2010 And paginaOnLineRitorno = enum_PagineGiasOnline_2010.RegistazioneSmart Then
                        link = AgronicaCoreGestioneRichieste.RedirectGestione.IndirizzoCompleto_SitoOnline2010_PassandoDirettamenteIParametri( _
                                               Enum_SiteRedirector.Sito_AgronicaAgenda_2010, _
                                               enum_PagineGiasOnline_2010.RegistazioneSmart, _
                                               enum_PagineAgenda_2010.Menu, objParametriAgenda.Piva, "", "", 0, "")

                    Else
                        link = CType(Master.Master, Agenda).TrovaRedirectCorretto(False, enum_PagineGiasOnline.MenuAgenda, objParametriAgenda)
                    End If

                Catch ex As Exception
                    link = CType(Master.Master, Agenda).TrovaRedirectCorretto(False, enum_PagineGiasOnline.MenuAgenda, objParametriAgenda)
                End Try


                strJS.AppendLine("      ChiamataParent_Id_Ageda(" & objParametriAgenda.Id_Agenda & "); ")
                strJS.AppendLine("      window.location = '" & link & "'; ")



                strJS.AppendLine(" });")
                ScriptManager.RegisterStartupScript(CType(Ricerca.FindControlIterative(Page.Master, "UpdatePanelToolBar1"), UpdatePanel), CType(Ricerca.FindControlIterative(Page.Master, "UpdatePanelToolBar1"), UpdatePanel).GetType(),
                                              String.Format("jQuery_{0}", CType(Ricerca.FindControlIterative(Page.Master, "UpdatePanelToolBar1"), UpdatePanel).ClientID), strJS.ToString, True)

                Messaggi.AgroMsgBuonFine(Resources.AgronicaAgenda_2010.RegistrazioneEffettuataConSuccesso, Page, , _
                                  CType(Ricerca.FindControlIterative(Page.Master, "UpdatePanelToolBar"), UpdatePanel))

                RipristinaSessione()
                'Response.Redirect(CType(Master.Master, Agenda).TrovaRedirectCorretto(False, enum_PagineGiasOnline.MenuAgenda, objParametriAgenda))



            Case enum_Tipo_Salvataggio.Salva_e_Nuovo


                objParametriAgenda.Impianti = New List(Of AgronicaCoreModello.ParametriAgenda_Temp.Impianto)
                objParametriAgenda.Note = New List(Of Nota)
                objParametriAgenda.Movimenti = New List(Of Movimento)

                Rimuovi_Centri()



                Messaggi.AgroMsgBuonFine(Resources.AgronicaAgenda_2010.RegistrazioneEffettuataConSuccesso, Page, , _
                                  Master_Operazione.Property_UpdatePanelToolBar)

                Dim strJS As New StringBuilder
                strJS.AppendLine("$(document).ready(function () { ")
                'strJS.AppendLine("      alert(''); ")
                strJS.AppendLine("      window.location = '../Operazioni/NonUtilizzo.aspx'; ")
                strJS.AppendLine(" });")

                ScriptManager.RegisterClientScriptBlock(Master_Operazione.Property_UpdatePanelToolBar, _
                                                    Master_Operazione.Property_UpdatePanelToolBar.GetType(),
                                              String.Format("jQuery_{0}", Master_Operazione.Property_UpdatePanelToolBar.ClientID), _
                                              strJS.ToString, True)






            Case enum_Tipo_Salvataggio.Salva_e_Duplica



                Messaggi.AgroMsgBuonFine(Resources.AgronicaAgenda_2010.RegistrazioneEffettuataConSuccesso, Page, , _
                                  Master_Operazione.Property_UpdatePanelToolBar)





        End Select
    End Sub


#End Region




#Region "Creazione oggetto Agenda per salvataggio"


    Private Function CreaOggettoAgenda(Piva As String, Sacod As Integer, Data As String, Ora As String, Minuti As String, Pioggia As String, Note As String, TMin As String, TMax As String, Umidita As String, messaggio_errore As String) As Operazione_Agenda
        Dim Agenda As Operazione_Agenda

        '--------------AGENDA------------------
        If Not Crea_Agenda(Data, Piva, Sacod, Agenda, messaggio_errore) Then
            Return Nothing
        End If


        '--------------NOTE------------------
        If Not Crea_Agenda_Note(Agenda) Then
            Return Nothing
        End If





        '------------- MOVIMENTO RILIEVO / TRATTAMENTO---------
        'inneschiNumTotale usato solo per trappole e massa, ignorato per conf e dis sessuale
        If Not Crea_Agenda_Movimento_NonUtilizzo(Agenda, Piva, Sacod, Data, Note, messaggio_errore) Then
            Return Nothing
        End If


        Return Agenda

    End Function


    Private Function Crea_Agenda(ByRef Data As String, ByRef PIVA As String, ByRef Sa_Cod As Integer, ByRef Agenda As Operazione_Agenda, ByRef messaggio_errore As String) As Boolean

        '---------------------------------------
        ' recupero la OPERAZIONE
        'Dim Lav_Cod As String = ""
        Dim Lav_Des As String = ""
        If objParametriAgenda.Lav_Cod = "" Then
            messaggio_errore = messaggio_errore & Resources.AgronicaAgenda_2010.SelezionareUnOperazione
            Return False
        Else
            'Lav_Cod = objParametriAgenda.Lav_Cod
            Lav_Des = Master_Operazione.Property_ComboOperazione.Testo_Combo
            objParametriAgenda.Lav_Des = Lav_Des
        End If

        Dim BaseCode As Integer
        Dim TopCode As Integer

        '------------------------------------------------
        '----- Calcolo i valori di BaseCode e TopCode
        '------------------------------------------------
        Call Calcola_BaseCode_TopCode(BaseCode, _
                              TopCode, _
                              Session("ASG_ProgressivoGIAS"))


        '------------------------------------------------
        '----- AGENDA
        '------------------------------------------------
        Agenda = New Operazione_Agenda

        Agenda.Tipo_Operazione = objParametriAgenda.Tipo_Operazione
        Select Case Agenda.Tipo_Operazione
            Case TipiEnumerativi.enum_TipoOperazioneDB.Scrittura
                Agenda.Id_Agenda = 0
            Case Else
                Agenda.Id_Agenda = objParametriAgenda.Id_Agenda
        End Select
        'Agenda.Id_Agenda = 0

        Agenda.Data = CDate(Data)
        Agenda.Piva = objParametriAgenda.Piva
        Agenda.Sa_Cod = Sa_Cod
        Agenda.Lav_Cod = objParametriAgenda.Lav_Cod
        Agenda.Des_Lib = Lav_Des '& " (Centro: " & New AgronicaCoreAnagrafeDAL.CentriAziendali_Read().SaNome_from_SaCod(PIVA, Sa_Cod, objParametri_Server) & ")"

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
                Select Case Agenda.Tipo_Operazione
                    Case TipiEnumerativi.enum_TipoOperazioneDB.Scrittura
                        Nota.Id_Agenda = 0
                    Case Else
                        Nota.Id_Agenda = objParametriAgenda.Id_Agenda
                End Select
                Nota.Nota_Cod = ListaConsigli(i).Nota_Cod
                Agenda.Note.Add(Nota)
            Next
        End If
        Return True
    End Function


    Private Function Crea_Agenda_Movimento_NonUtilizzo(ByRef Agenda As Operazione_Agenda, Piva As String, Sacod As Integer, Data As String, Note As String, messaggio_errore As String) As Boolean


        Dim Movimento_OperazioneColturale As New Movimento

        Select Case Agenda.Tipo_Operazione
            Case TipiEnumerativi.enum_TipoOperazioneDB.Scrittura
                Movimento_OperazioneColturale.Id_Agenda = 0
            Case Else
                Movimento_OperazioneColturale.Id_Agenda = objParametriAgenda.Id_Agenda
        End Select
        Movimento_OperazioneColturale.Piva = Agenda.Piva
        Movimento_OperazioneColturale.Sa_Cod = Agenda.Sa_Cod
        Movimento_OperazioneColturale.Lav_Cod = objParametriAgenda.Lav_Cod
        Movimento_OperazioneColturale.Cau_Mov = objParametriAgenda.Cau_Mov
        Movimento_OperazioneColturale.Mov_Desc = Note
        Movimento_OperazioneColturale.Data = Agenda.Data



        '----------------------------------------------------------
        '----------------------------------------------------------
        '----------------------------------------------------------
        'attenzione, modifica nfinchè non vengono utilizzate le ore e i minuti,
        'se sono in modifica riscrivo il vallore precedernte, dato che ora salvo sempre 23:00 essendo te textbox nascoste
        'se si vuole riutilizzare le ore e minuti basta visualuizzarle di nuovo nell'aspx, e togliere il blocco seguente e dovrebbero funzionare correttamente
        Try
            If objParametriAgenda.Tipo_Operazione = TipiEnumerativi.enum_TipoOperazioneDB.Modifica Then
                Dim mov As List(Of Movimento) = New Agenda_Movimenti_Helper().Leggi(Agenda.Piva, Agenda.Sa_Cod, objParametriAgenda.Id_Agenda, objParametri_Server)
                For kk = 0 To mov.Count - 1
                    If mov(kk).Cau_Mov = enum_Agenda_Causali.RILIEVO_CAMPO Then
                        Movimento_OperazioneColturale.Ora = mov(kk).Ora
                    End If
                Next

            End If
        Catch

        End Try
        '----------------------------------------------------------
        '----------------------------------------------------------
        '----------------------------------------------------------


        Movimento_OperazioneColturale.BaseCode = Agenda.BaseCode
        Movimento_OperazioneColturale.TopCode = Agenda.TopCode



        '------------------------------
        '----- MOVIMENTO DET TECNICO --
        '------------------------------
        Dim Movimento_Dettaglio_Tecnico As New Movimento_Dettaglio_Tecnico

        Select Case Agenda.Tipo_Operazione
            Case TipiEnumerativi.enum_TipoOperazioneDB.Scrittura
                Movimento_Dettaglio_Tecnico.Id_Agenda = 0
            Case Else
                Movimento_Dettaglio_Tecnico.Id_Agenda = objParametriAgenda.Id_Agenda
        End Select

        Movimento_Dettaglio_Tecnico.Piva = Agenda.Piva
        Movimento_Dettaglio_Tecnico.Sa_Cod = Agenda.Sa_Cod
        Movimento_Dettaglio_Tecnico.Data = AGRODATAINIZIO
        Movimento_Dettaglio_Tecnico.BaseCode = Agenda.BaseCode
        Movimento_Dettaglio_Tecnico.TopCode = Agenda.TopCode

        Movimento_OperazioneColturale.Movimenti_Dettagli_Tecnici.Add(Movimento_Dettaglio_Tecnico)

        Agenda.Movimenti.Add(Movimento_OperazioneColturale)

        Return True
    End Function


#End Region

    Private Sub Rimuovi_Centri()

        AbilitaStep1(False)
    End Sub


#Region "Eventi pulsanti"

    Protected Sub IMGB_InserisciCentri_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles IMGB_InserisciCentri.Click
        Inserisci_Centri()
    End Sub

    Private Sub Inserisci_Centri()
        CaricaGrigliaCentri()
        AbilitaStep1(True)
    End Sub

    Private Sub CaricaGrigliaCentri()
        CaricaGrigliaCentriPerManuale()
    End Sub

    Private Sub CaricaGrigliaCentriPerManuale()
        Dim Dt As New DataTable("Piogge")

        Dt.Columns.Add("PIVA", GetType(String))
        Dt.Columns.Add("Rag_Soc", GetType(String))
        Dt.Columns.Add("Sa_Cod", GetType(Integer))
        Dt.Columns.Add("Sa_nome", GetType(String))

        Dim i As Integer = 0
        Dim Piva As String = objParametriAgenda.Piva
        Dim Sa_Cod As Integer = CInt(Master_Operazione.Property_ComboCentroAziendale.Valore_Combo)
        Dim DTCentri As DataTable = New AgronicaCoreAnagrafeDAL.CentriAziendali_Read().Leggi(Piva, Sa_Cod, AgronicaCoreDataProvider.AgronicaCoreParametri.enumSelezioneVariabile.Selezione_TabellaCompleta, "", " Sa_Nome ", objParametri_Server)
        For i = 0 To DTCentri.Rows.Count - 1
            Dim row As DataRow = Dt.NewRow
            row = Dt.NewRow
            row.Item("PIVA") = DTCentri.Rows(i).Item("PIVA")
            row.Item("Rag_Soc") = New AgronicaCoreAnagrafeDAL.Imprese_Read().RagSoc_from_Piva(DTCentri.Rows(i).Item("PIVA"), objParametri_Server)
            row.Item("Sa_Cod") = DTCentri.Rows(i).Item("Sa_Cod")
            row.Item("Sa_nome") = DTCentri.Rows(i).Item("Sa_nome")

            Dt.Rows.Add(row)
        Next

        Dim DtKeys(1) As String

        DtKeys(0) = "PIVA"
        DtKeys(1) = "Sa_Cod"

        GridView_NonUtilizzo.DataKeyNames = DtKeys

        GridView_NonUtilizzo.DataSource = Dt
        GridView_NonUtilizzo.DataBind()

        'coloro le righe
        Dim i2 As Integer = 0
        For i2 = 0 To Me.GridView_NonUtilizzo.Rows.Count - 1
            If i2 Mod 2 = 0 Then
                Me.GridView_NonUtilizzo.Rows(i2).BackColor = Drawing.Color.Beige
            End If
        Next



    End Sub
#End Region

End Class