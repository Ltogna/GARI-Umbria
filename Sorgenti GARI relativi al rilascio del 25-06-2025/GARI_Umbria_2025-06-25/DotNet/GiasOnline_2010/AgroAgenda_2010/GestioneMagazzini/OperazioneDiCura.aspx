<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/Agenda.Master"
    CodeBehind="OperazioneDiCura.aspx.vb" Inherits="AgroAgenda_2010.OperazioneDiCura" %>

<%@ Register Assembly="AgronicaControlli_2010" Namespace="AgronicaControlli_2010"
    TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentAgendaHead" runat="server">
    <!--include per l'albero-->
    <script type="text/javascript" src="<%= ResolveClientUrl("~/Scripts/jquery.cookie.js?" & Application("GiasVersioneCorrente").ToString) %>"></script>
    <script type="text/javascript" src="<%= ResolveClientUrl("~/Scripts/jquery.hotkeys.js?" & Application("GiasVersioneCorrente").ToString) %>"></script>
    <script type="text/javascript" src="<%= ResolveClientUrl("~/Scripts/jquery.jstree.js?" & Application("GiasVersioneCorrente").ToString) %>"></script>
    <script type="text/javascript" src="<%= ResolveClientUrl("~/Scripts/min/jquery.cookie.min.js?" & Application("GiasVersioneCorrente").ToString) %>"></script>
    <script type="text/javascript" src="<%= ResolveClientUrl("~/Scripts/min/jquery.hotkeys.min.js?" & Application("GiasVersioneCorrente").ToString) %>"></script>
    <script type="text/javascript" src="<%= ResolveClientUrl("~/Scripts/min/jquery.jstree.min.js?" & Application("GiasVersioneCorrente").ToString) %>"></script>
    <script type="text/javascript" src="../Scripts/jquery-ui.combobox.js?<% =Application("GiasVersioneCorrente")%>"></script>
    <script type="text/javascript" src="<%= ResolveClientUrl("~/Scripts/jquery.ui.timepicker.js?" & Application("GiasVersioneCorrente").ToString) %>"> </script>
    <link href="../Styles/jquery.ui.timepicker.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .bordomenu
        {
            border-bottom-color: #444444;
            border-bottom-width: 1px;
            border-bottom-style: solid;
        }
    </style>
    <script type="text/javascript">

        $(document).ready(function () {

        });


        function DoPostBack_Combo($_combo, valoreOpt) {
            //postBack CentroAziendale
            if ($_combo.attr("id").endsWith("ComboCentroAziendale")) {
                $("#<%=BTN_ComboCentroAziendale.ClientID %>").click();
                $('#WaitFrame').show();
                return;
            }

            //postBack Magazzini
            if ($_combo.attr("id").endsWith("ComboMagazzini")) {
                $("#<%=BTN_ComboMagazzini.ClientID %>").click();
                $('#WaitFrame').show();
                return;
            }

            //postBack CentroAziendale
            if ($_combo.attr("id").endsWith("ComboSpecie")) {
                $("#<%=BTN_ComboSpecie.ClientID %>").click();
                $('#WaitFrame').show();
                return;
            }

            //postBack CentroAziendale
            if ($_combo.attr("id").endsWith("ComboCentroDestinazioneCure")) {
                $("#<%=BTN_ComboCentroDestinazioneCure.ClientID %>").click();
                $('#WaitFrame').show();
                return;
            }

            //postBack Magazzini
            if ($_combo.attr("id").endsWith("ComboMagazziniDestinazioneCure")) {
                $("#<%=BTN_ComboMagazziniDestinazioneCure.ClientID %>").click();
                $('#WaitFrame').show();
                return;
            }

            //postBack Categoria
            if ($_combo.attr("id").endsWith("cmb_Categoria")) {
                $("#<%=BTN_cmb_Categoria.ClientID %>").click();
                $('#WaitFrame').show();
                return;
            }


        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentAgendaContenuti" runat="server">
    <asp:UpdatePanel ID="UpdatePanelPerScript" runat="server">
        <ContentTemplate>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="box" style="margin: 10px;">
        <div style="width: 100%;">
            <asp:UpdatePanel ID="UpdatePanelOperazione" runat="server" UpdateMode="Always">
                <ContentTemplate>
                    <!-- Riga 1 -->
                    <div id="Div1" runat="server">
                        <div style="width: 100%; margin-top: 5px;">
                            <asp:Label runat="server" ID="Label6" BackColor="#EAF4FD">Prodotto da Curare</asp:Label>
                        </div>
                        <div class="box">
                            <div id="DivRicerca" runat="server">
                                <div class="box33" style="float: left; min-width: 350px">
                                    <div class="descrizione" style="width: 80px">
                                        <asp:Label ID="lblCentroAziendale" runat="server" meta:resourcekey="lblCentroAziendaleResource1">Centro Aziendale</asp:Label>
                                    </div>
                                    <div class="valoriinput">
                                        <cc1:ComboCentroAziendale ID="ComboCentroAziendale" runat="server" meta:resourcekey="ComboCentroAziendaleResource1" />
                                        <asp:Button ID="BTN_ComboCentroAziendale" runat="server" Text="Button" Style="display: none" />
                                    </div>
                                </div>
                                <div class="box33" style="float: left; min-width: 350px">
                                    <div class="descrizione" style="width: 80px">
                                        <asp:Label ID="lblProvenienzaRisorse" runat="server">Magazzino</asp:Label>
                                    </div>
                                    <div class="valoriinput">
                                        <cc1:ComboMagazzini ID="ComboMagazzini" runat="server" Style="width: 250px" Fabbricato_Cod="0"
                                            Flag_CodCentroFabbricato="True" Flag_GestioneMagazziniImpresaPadre="False" meta:resourcekey="ComboMagazziniResource1"
                                            Sa_Cod="0" TipoMagazzino="20" />
                                        <asp:Button ID="BTN_ComboMagazzini" runat="server" Text="Button" Style="display: none" />
                                    </div>
                                </div>
                                <div class="box33" style="float: left; min-width: 350px">
                                    <div class="descrizione" style="width: 80px">
                                        <asp:Label ID="Label26" runat="server">Specie</asp:Label>
                                    </div>
                                    <div class="valoriinput">
                                        <cc1:ComboSpecie ID="ComboSpecie" runat="server" meta:resourcekey="ComboSpecieResource1" />
                                        <asp:Button ID="BTN_ComboSpecie" runat="server" Text="Button" Style="display: none" />
                                    </div>
                                </div>
                                <div class="clear">
                                </div>
                                <div class="box33" style="min-width: 350px">
                                    <div class="descrizione" style="width: 80px">
                                        <asp:Label ID="lblCategoria" runat="server">Categoria Prodotto</asp:Label>
                                    </div>
                                    <div class="valoriinput">
                                        <asp:DropDownList ID="cmb_Categoria" runat="server" Width="250px" CssClass="txtUI"
                                            AutoPostBack="false">
                                        </asp:DropDownList>
                                        <asp:Button ID="BTN_cmb_Categoria" runat="server" Text="Button" Style="display: none" />
                                    </div>
                                </div>
                                <div class="" style="float: left; width: 200px;">
                                    <div class="descrizione" style="width: 80px">
                                        <asp:Label ID="Label1" runat="server">Giacenza al giorno:</asp:Label></div>
                                    <div class="valoriinput" style="width: 95px">
                                        <asp:TextBox ID="txt_DataOperazioneAlGiorno" Style="width: 90px" runat="server" CssClass="txtUI datepicker"></asp:TextBox>
                                        <asp:Button ID="BTN_ChangeData_AlGiorno" runat="server" Text="Button" Style="display: none" />
                                    </div>
                                </div>
                                <div class="box33" style="float: left; min-width: 350px">
                                    <div class="descrizione" style="width: 80px">
                                        <asp:Label ID="Label5" runat="server"> </asp:Label>
                                    </div>
                                    <div class="valoriinput">
                                        <div id="Div_CheckBoxGiacenze0">
                                            <asp:CheckBox ID="CheckBoxGiacenze0" runat="server" Checked="true" Text="Visualizza le Giacenze 0" />
                                        </div>
                                    </div>
                                </div>
                                <div class="clear">
                                </div>
                                <!-- Riga 3 -->
                                <div class="box33" style="float: left; min-width: 350px">
                                    <div class="descrizione" style="width: 80px">
                                        <asp:Label ID="LabelProdotto" runat="server">Nome Prodotto</asp:Label>
                                    </div>
                                    <div class="valoriinput">
                                        <asp:TextBox ID="TxtProdotto" CssClass="txtUI" runat="server" Width="100%"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="box33" style="float: left; min-width: 350px">
                                    <div class="descrizione" style="width: 80px">
                                        <asp:Label ID="Label4" runat="server">Codice Prodotto</asp:Label>
                                    </div>
                                    <div class="valoriinput">
                                        <asp:TextBox ID="TxtCodProdotto" CssClass="txtUI" runat="server" Width="100%"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="box33" style="float: left; min-width: 350px">
                                    <div class="descrizione" style="width: 80px">
                                        <asp:Label ID="Label2" runat="server">Lotto</asp:Label>
                                    </div>
                                    <div class="valoriinput">
                                        <asp:TextBox ID="TxtLotto" CssClass="txtUI" runat="server" Width="100%"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clear">
                                </div>
                                <div class="box33" style="float: left; min-width: 350px">
                                    <div class="descrizione" style="width: 80px">
                                        <asp:Label ID="Label3" runat="server">Codice Lotto Interno</asp:Label>
                                    </div>
                                    <div class="valoriinput">
                                        <asp:TextBox ID="TxtCodLottoInterno" CssClass="txtUI" runat="server" Width="100%"></asp:TextBox>
                                    </div>
                                </div>
                                <div id="DivCercaGiacenze" class="box33" style="float: left; min-width: 350px">
                                    <div class="descrizione" style="width: 80px">
                                        <asp:Label ID="Label7" runat="server"> Cerca</asp:Label>
                                    </div>
                                    <div class="valoriinput" style="width: 50px">
                                        <asp:ImageButton ID="ImgBtn_Cerca_Giacenza" CssClass="btn_per_load" runat="server"
                                            ImageUrl="../AB_Immagini/icone32/Trova2.ico" Style="width: 42px" />
                                    </div>
                                </div>
                            </div>
                            <div class="clear">
                            </div>
                            <asp:GridView ID="GridView_Giacenze" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                Width="100%" CellPadding="5" CssClass="ui-widget-content" Caption="Prodotti Nel Magazzino"
                                meta:resourcekey="GridView_DosiResource1">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="ChkSelezionaMovimento" runat="server" CssClass="ChkSelezionaMovimento" />
                                        </ItemTemplate>
                                        <HeaderStyle Width="20px" />
                                        <ItemStyle Width="20px" />
                                        <FooterStyle Width="20px" />
                                        <ControlStyle Width="20px" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Piva" HeaderText="Piva" HtmlEncode="False">
                                        <ItemStyle CssClass="displaynone" />
                                        <HeaderStyle CssClass="displaynone" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Sa_Cod" HeaderText="Sa_Cod" HtmlEncode="False">
                                        <ItemStyle CssClass="displaynone" />
                                        <HeaderStyle CssClass="displaynone" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Sa_Nome" HeaderText="Centro" HtmlEncode="False"></asp:BoundField>
                                    <asp:BoundField DataField="Fabbricato_Cod" HeaderText="Fabbricato_Cod" HtmlEncode="False">
                                        <ItemStyle CssClass="displaynone" />
                                        <HeaderStyle CssClass="displaynone" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Fabbricato_Des" HeaderText="Magazzino" HtmlEncode="False">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Cat_Cod" HeaderText="Cat_Cod" HtmlEncode="False">
                                        <ItemStyle CssClass="displaynone" />
                                        <HeaderStyle CssClass="displaynone" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Cat_Des" HeaderText="Categoria" HtmlEncode="False"></asp:BoundField>
                                    <asp:BoundField DataField="Pro_Cod" HeaderText="Codice Prodotto" HtmlEncode="False">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Pro_Des" HeaderText="Prodotto" HtmlEncode="False"></asp:BoundField>
                                    <asp:BoundField DataField="Mat_Cod" HeaderText="Mat_Cod" HtmlEncode="False">
                                        <ItemStyle CssClass="displaynone" />
                                        <HeaderStyle CssClass="displaynone" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Lotto_Int" HeaderText="Lotto Int." HtmlEncode="False">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Lotto_Acc" HeaderText="Lotto Acc." HtmlEncode="False">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Cod_Progetto" HeaderText="Cod_Progetto" HtmlEncode="False">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Param_Des" HeaderText="Parametro Qualitativo" HtmlEncode="False">
                                        <ItemStyle CssClass="displaynone" />
                                        <HeaderStyle CssClass="displaynone" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Cal_Cod" HeaderText="Cal_Cod" HtmlEncode="False">
                                        <ItemStyle CssClass="displaynone" />
                                        <HeaderStyle CssClass="displaynone" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Cal_Des" HeaderText="Qualità" HtmlEncode="False">
                                        <ItemStyle CssClass="displaynone" />
                                        <HeaderStyle CssClass="displaynone" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Udm_Cod" HeaderText="Udm_Cod" HtmlEncode="False">
                                        <ItemStyle CssClass="displaynone" />
                                        <HeaderStyle CssClass="displaynone" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Udm_Des" HeaderText="Unità di Misura" HtmlEncode="False">
                                        <%--                                    <ItemStyle CssClass="displaynone" />
                                        <HeaderStyle CssClass="displaynone" />--%>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Qta_Mag" HeaderText="Qta" HtmlEncode="False">
                                        <%--                                      <ItemStyle CssClass="displaynone" />
                                        <HeaderStyle CssClass="displaynone" />--%>
                                    </asp:BoundField>
                                </Columns>
                                <HeaderStyle CssClass="ui-widget-header" />
                                <PagerStyle CssClass="ui-widget-header" HorizontalAlign="Center" />
                                <RowStyle CssClass="rigaImpianti" />
                            </asp:GridView>
                        </div>
                        <div class="clear">
                        </div>
                    </div>
                    <div class="clear">
                    </div>
                    <!-- Riga 2 -->
                    <div id="Div2" runat="server" visible="true">
                        <div style="width: 100%; margin-top: 5px;">
                            <asp:Label runat="server" ID="Label9" BackColor="#EAF4FD">Infornature</asp:Label>
                        </div>
                        <div class="box">
                            <%--                            <div class="box33" style="min-width: 350px">
                                <div class="descrizione" style="width: 80px">
                                    <asp:Label ID="Label20" runat="server">UDS</asp:Label>
                                </div>
                                <div class="valoriinput">
                                    <asp:DropDownList ID="ComboUDS" CssClass="txtUI" Style="width: 250px" runat="server"
                                        AutoPostBack="true" />
                                </div>
                            </div>--%>
                            <div id="OpzioniInfornature" runat="server">
                                <div class="box33" style="min-width: 350px">
                                    <div class="descrizione" style="width: 80px">
                                        <asp:Label ID="lbl55" runat="server">Centro Di Cura</asp:Label>
                                    </div>
                                    <div class="valoriinput">
                                        <asp:DropDownList ID="ComboCentroCura" CssClass="txtUI" Style="width: 250px" runat="server"
                                            AutoPostBack="true" />
                                    </div>
                                </div>
                                <div class="box33" style="min-width: 350px" runat="server" id="ProvenienzaRisorse">
                                    <div class="descrizione" style="width: 80px">
                                        <asp:Label ID="lbl44" runat="server">Essiccatoio</asp:Label>
                                    </div>
                                    <div class="valoriinput">
                                        <asp:DropDownList ID="ComboEssiccatoio" CssClass="txtUI" Style="width: 250px" runat="server" />
                                    </div>
                                </div>
                                <div class="box33" style="min-width: 350px">
                                    <div class="descrizione" style="width: 80px">
                                        <asp:Label ID="Label22" runat="server">Prodotto In Essiccatoio</asp:Label>
                                    </div>
                                    <div class="valoriinput">
                                        <asp:DropDownList ID="ComboProdottoInLavorazione" runat="server" Style="width: 250px"
                                            CssClass="txtUI" AutoPostBack="True">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="clear">
                                </div>
                                <div class="" style="float: left; width: 170px;">
                                    <div class="descrizione" style="width: 76px">
                                        <asp:Label ID="Label145" runat="server">Cassoni Usati</asp:Label></div>
                                    <div class="valoriinput" style="width: 76px">
                                        <asp:TextBox ID="TextBoxNCassoni" CssClass="txtUI" Text="0" runat="server" Width="70px"></asp:TextBox>
                                    </div>
                                </div>
                                <div style="float: left; width: 200px;">
                                    <div class="descrizione" style="width: 90px">
                                        <asp:Label ID="Label10" runat="server">Data Inizio Cura</asp:Label></div>
                                    <div class="valoriinput" style="width: 90px">
                                        <asp:TextBox ID="TextBoxDataInizioCura" Style="width: 90px" runat="server" CssClass="txtUI datepicker "></asp:TextBox>
                                    </div>
                                </div>
                                <div style="float: left; width: 190px;">
                                    <div class="descrizione" style="width: 90px">
                                        <asp:Label ID="Label13" runat="server">Ora Inizio Cura</asp:Label></div>
                                    <div class="valoriinput" style="width: 62px">
                                        <asp:TextBox ID="OraInizioCura" CssClass="txtUI
                    datepicker" runat="server" Style="width: 50px">00:00</asp:TextBox>
                                    </div>
                                </div>
                                <div class="" style="float: left; width: 170px;">
                                    <div class="descrizione" style="width: 76px">
                                        <asp:Label ID="Label23" runat="server">Qta Prodotto</asp:Label></div>
                                    <div class="valoriinput" style="width: 76px">
                                        <asp:TextBox ID="TextBoxQta" CssClass="txtUI" Text="" runat="server" Width="70px"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="" style="float: left; width: 170px;">
                                    <div class="descrizione" style="width: 76px">
                                        <asp:Label ID="Label16" runat="server">Inserisci Infornatura</asp:Label></div>
                                    <div class="valoriinput" style="width: 76px">
                                        <asp:ImageButton ID="ImgBtn_Inserisci_Infornatura" CssClass="btn_per_load" runat="server"
                                            ImageUrl="../AB_Immagini/icone32/FrecciaDN.ico" Style="width: 42px" />
                                    </div>
                                </div>
                                <div class="clear">
                                </div>
                            </div>
                            <div>
                                <asp:GridView ID="GridViewInfornature" runat="server" AutoGenerateColumns="False"
                                    Width="100%" CellPadding="5" CssClass="ui-widget-content" Caption="Infornature">
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="ChkSelezionaMovimento" runat="server" CssClass="ChkSelezionaMovimento" />
                                            </ItemTemplate>
                                            <HeaderStyle Width="20px" />
                                            <ItemStyle Width="20px" />
                                            <FooterStyle Width="20px" />
                                            <ControlStyle Width="20px" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Id_Infornatura" HeaderText="Codice Lotto Infornatura"
                                            HtmlEncode="False"></asp:BoundField>
                                        <asp:BoundField DataField="Sa_Cod" HeaderText="Sa_Cod" HtmlEncode="False">
                                            <ItemStyle CssClass="displaynone" />
                                            <HeaderStyle CssClass="displaynone" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Sa_nome" HeaderText="Centro di Cura" SortExpression="Centro">
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Fabbricato_Cod" HeaderText="Fabbricato_Cod" SortExpression="Fabbricato_Cod">
                                            <ItemStyle CssClass="displaynone" />
                                            <HeaderStyle CssClass="displaynone" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Fabbricato_Des" HeaderText="Essiccatoio" SortExpression="Essiccatoio">
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Data_Inizio_Cura" HeaderText="Data Inizio Cura" SortExpression="Data_Inizio_Cura">
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Ora_Inizio_Cura" HeaderText="Ora" SortExpression="Ora_Inizio_Cura">
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Cassoni" HeaderText="Cassoni" SortExpression="Cassoni">
                                        </asp:BoundField>
                                        <%--Dati Magazzino--%>
                                        <asp:BoundField DataField="Magazzino_Sa_Cod" HeaderText="Sa_Cod" HtmlEncode="False">
                                            <ItemStyle CssClass="displaynone" />
                                            <HeaderStyle CssClass="displaynone" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Magazzino_Sa_Nome" HeaderText="Centro Provenienza" HtmlEncode="False">
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Magazzino_Fabbricato_Cod" HeaderText="Fabbricato_Cod"
                                            HtmlEncode="False">
                                            <ItemStyle CssClass="displaynone" />
                                            <HeaderStyle CssClass="displaynone" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Magazzino_Fabbricato_Des" HeaderText="Magazzino Provenienza"
                                            HtmlEncode="False"></asp:BoundField>
                                        <asp:BoundField DataField="Magazzino_Cat_Cod" HeaderText="Cat_Cod Provenienza" HtmlEncode="False">
                                            <ItemStyle CssClass="displaynone" />
                                            <HeaderStyle CssClass="displaynone" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Magazzino_Cat_Des" HeaderText="Categoria Provenienza"
                                            HtmlEncode="False">
                                            <ItemStyle CssClass="displaynone" />
                                            <HeaderStyle CssClass="displaynone" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Magazzino_Pro_Cod" HeaderText="Codice Prodotto Provenienza"
                                            HtmlEncode="False">
                                            <ItemStyle CssClass="displaynone" />
                                            <HeaderStyle CssClass="displaynone" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Magazzino_Pro_Des" HeaderText="Prodotto Da Curare" HtmlEncode="False">
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Magazzino_Mat_Cod" HeaderText="Mat_Cod Da Curare" HtmlEncode="False">
                                            <ItemStyle CssClass="displaynone" />
                                            <HeaderStyle CssClass="displaynone" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Magazzino_Lotto_Int" HeaderText="Lotto Int. Da Curare"
                                            HtmlEncode="False"></asp:BoundField>
                                        <asp:BoundField DataField="Magazzino_Lotto_Acc" HeaderText="Lotto Acc. Da Curare"
                                            HtmlEncode="False"></asp:BoundField>
                                        <asp:BoundField DataField="Magazzino_Cod_Progetto" HeaderText="Cod_Progetto Da Curare"
                                            HtmlEncode="False"></asp:BoundField>
                                        <asp:BoundField DataField="Magazzino_Param_Des" HeaderText="Parametro Qualitativo Da Curare"
                                            HtmlEncode="False">
                                            <ItemStyle CssClass="displaynone" />
                                            <HeaderStyle CssClass="displaynone" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Magazzino_Cal_Cod" HeaderText="Cal_Cod Da Curare" HtmlEncode="False">
                                            <ItemStyle CssClass="displaynone" />
                                            <HeaderStyle CssClass="displaynone" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Magazzino_Cal_Des" HeaderText="Qualità Da Curare" HtmlEncode="False">
                                            <ItemStyle CssClass="displaynone" />
                                            <HeaderStyle CssClass="displaynone" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Magazzino_Udm_Cod" HeaderText="Udm_Cod Da Curare" HtmlEncode="False">
                                            <ItemStyle CssClass="displaynone" />
                                            <HeaderStyle CssClass="displaynone" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Magazzino_Udm_Des" HeaderText="Unità di Misura" HtmlEncode="False">
                                            <%--                                            <ItemStyle CssClass="displaynone" />
                                            <HeaderStyle CssClass="displaynone" />--%>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Magazzino_Qta" HeaderText="Qta" SortExpression="Qta Da Curare"
                                            HtmlEncode="False">
                                            <%--                                            <ItemStyle CssClass="displaynone" />
                                            <HeaderStyle CssClass="displaynone" />--%>
                                        </asp:BoundField>
                                        <asp:ButtonField ItemStyle-HorizontalAlign="Center" Text="&lt;img src='../AB_Immagini/icone16/Gomma16.ico' border='0'&gt; "
                                            HeaderText="Cancella" CommandName="Cancella">
                                            <ItemStyle CssClass="btn_per_load" />
                                        </asp:ButtonField>
                                    </Columns>
                                    <HeaderStyle CssClass="ui-widget-header" />
                                    <PagerStyle CssClass="ui-widget-header" HorizontalAlign="Center" />
                                </asp:GridView>
                            </div>
                        </div>
                        <div class="clear">
                        </div>
                    </div>
                    <div class="clear">
                    </div>
                    <!-- Riga 3 -->
                    <div id="Div3" runat="server" visible="true">
                        <div style="width: 100%; margin-top: 5px;">
                            <asp:Label runat="server" ID="Label15" BackColor="#EAF4FD">Sfornature</asp:Label>
                        </div>
                        <div class="box">
                            <div id="OpzioniSfornature" runat="server">
                                <div class="box33" style="float: left; min-width: 350px">
                                    <div class="descrizione" style="width: 80px">
                                        <asp:Label ID="Label20" runat="server">Centro Aziendale</asp:Label>
                                    </div>
                                    <div class="valoriinput">
                                        <asp:DropDownList ID="ComboCentroDestinazioneCure" runat="server" CssClass="txtUI">
                                        </asp:DropDownList>
                                        <asp:Button ID="BTN_ComboCentroDestinazioneCure" runat="server" Text="Button" Style="display: none" />
                                    </div>
                                </div>
                                <div class="box33" style="float: left; min-width: 350px">
                                    <div class="descrizione" style="width: 80px">
                                        <asp:Label ID="Label21" runat="server">Magazzino</asp:Label>
                                    </div>
                                    <div class="valoriinput">
                                        <asp:DropDownList ID="ComboMagazziniDestinazioneCure" runat="server" Style="width: 250px">
                                        </asp:DropDownList>
                                        <asp:Button ID="BTN_ComboMagazziniDestinazioneCure" runat="server" Text="Button"
                                            Style="display: none" />
                                    </div>
                                </div>
                                <div class="clear">
                                </div>
                                <div class="box33" style="min-width: 350px">
                                    <div class="descrizione" style="width: 80px">
                                        <asp:Label ID="Label8" runat="server">Prodotto Lavorato</asp:Label>
                                    </div>
                                    <div class="valoriinput">
                                        <asp:DropDownList ID="ComboProdottoLavorato" runat="server" Style="width: 250px"
                                            CssClass="txtUI" AutoPostBack="True">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="" style="float: left; width: 170px;">
                                    <div class="descrizione" style="width: 76px">
                                        <asp:Label ID="Label12" runat="server">Cassoni Sfornati</asp:Label></div>
                                    <div class="valoriinput" style="width: 76px">
                                        <asp:TextBox ID="TextBoxCassoniSfornati" CssClass="txtUI" Text="0" runat="server"
                                            Width="70px"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="clear">
                                </div>
                                <div style="float: left; width: 200px;">
                                    <div class="descrizione" style="width: 90px">
                                        <asp:Label ID="Label18" runat="server">Data Fine Cura</asp:Label></div>
                                    <div class="valoriinput" style="width: 90px">
                                        <asp:TextBox ID="TextBoxDataFineCura" Style="width: 90px" runat="server" CssClass="txtUI datepicker "></asp:TextBox>
                                    </div>
                                </div>
                                <div style="float: left; width: 180px;">
                                    <div class="descrizione" style="width: 90px">
                                        <asp:Label ID="Label19" runat="server">Ora Fine Cura</asp:Label></div>
                                    <div class="valoriinput" style="width: 62px">
                                        <asp:TextBox ID="OraFineCura" CssClass="txtUI datepicke" runat="server" Style="width: 50px">00:00</asp:TextBox>
                                    </div>
                                </div>
                                <div class="" id="DivPivaProv" style="float: left; width: 270px;">
                                    <div class="descrizione" style="width: 76px">
                                        <asp:Label ID="Label27" runat="server">Prefisso Lotto</asp:Label></div>
                                    <div class="valoriinput" style="width: 76px">
                                        <asp:TextBox ID="TextBoxPivaProv" CssClass="txtUI" Text="" runat="server" 
                                            Width="183px"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="" style="float: left; width: 170px;">
                                    <div class="descrizione" style="width: 76px">
                                        <asp:Label ID="Label14" runat="server">N. Lotti</asp:Label></div>
                                    <div class="valoriinput" style="width: 76px">
                                        <asp:TextBox ID="TextBoxNColli" CssClass="txtUI" Text="0" runat="server" Width="70px"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="" style="float: left; width: 170px;">
                                    <div class="descrizione" style="width: 76px">
                                        <asp:Label ID="Label17" runat="server">Inizio Numeraz.</asp:Label></div>
                                    <div class="valoriinput" style="width: 76px">
                                        <asp:TextBox ID="TextBoxInizioNum" CssClass="txtUI" Text="1" runat="server" Width="70px"></asp:TextBox>
                                    </div>
                                </div>
                                <%--                              <div class="" style="float: left; width: 170px;">
                                        <div class="valoriinput" style="width: 160px">
                                            <asp:CheckBox ID="CheckBoxVerificaProgressivo" Text="Verifica Progressivo Collo/Lotto"
                                                Checked="True" runat="server" AutoPostBack="true"></asp:CheckBox></div>
                                    </div>--%>
                                <div class="" style="float: left; width: 170px;">
                                    <div class="descrizione" style="width: 76px">
                                        <asp:Label ID="Label11" runat="server">Inserisci Sfornatura</asp:Label></div>
                                    <div class="valoriinput" style="width: 76px">
                                        <asp:ImageButton ID="ImgBtn_Inserisci_Sfornatura" CssClass="btn_per_load" runat="server"
                                            ImageUrl="../AB_Immagini/icone32/FrecciaDN.ico" Style="width: 42px" />
                                    </div>
                                </div>
                                <div class="clear">
                                </div>
                            </div>
                            <div>
                                <asp:GridView ID="GridViewColli" runat="server" AutoGenerateColumns="False" Width="100%"
                                    CellPadding="5" CssClass="ui-widget-content" Caption="Dati Cura">
                                    <Columns>
                                        <asp:BoundField DataField="Id_Infornatura" HeaderText="Codice Infornatura" HtmlEncode="False">
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Cassoni" HeaderText="Cassoni Sfornati" SortExpression="Cassoni">
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Data_Fine_Cura" HeaderText="Data Fine Cura" SortExpression="Data_Fine_Cura">
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Ora_Fine_Cura" HeaderText="Ora" SortExpression="Ora_Fine_Cura">
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Magazzino_Sa_Cod" HeaderText="Magazzino_Sa_Cod" HtmlEncode="False">
                                            <ItemStyle CssClass="displaynone" />
                                            <HeaderStyle CssClass="displaynone" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Magazzino_Sa_Nome" HeaderText="Centro Destinazione" SortExpression="Magazzino_Sa_Nome">
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Magazzino_Fabbricato_Cod" HeaderText="Magazzino_Fabbricato_Cod"
                                            SortExpression="Magazzino_Fabbricato_Cod">
                                            <ItemStyle CssClass="displaynone" />
                                            <HeaderStyle CssClass="displaynone" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Magazzino_Fabbricato_Des" HeaderText="Magazzino Destinazione"
                                            SortExpression="Essiccatoio"></asp:BoundField>
                                        <asp:TemplateField HeaderText="Lotto" SortExpression="Collo">
                                            <ItemTemplate>
                                                <asp:TextBox ID="Txt_Collo" runat="server" CssClass="ColloRiga txtUI" ToolTip="Collo"
                                                    Text='0' meta:resourcekey="Txt_DoseResource1" />
                                            </ItemTemplate>
                                            <ControlStyle Width="190px" />
                                            <HeaderStyle CssClass="IntestazCollo" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Peso" SortExpression="Peso">
                                            <ItemTemplate>
                                                <asp:TextBox ID="Txt_Peso" runat="server" CssClass="PesoRiga txtUI" ToolTip="Peso"
                                                    Text='0' meta:resourcekey="Txt_DoseResource1" />
                                            </ItemTemplate>
                                            <ControlStyle Width="80px" />
                                            <HeaderStyle CssClass="IntestazPeso" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Id_Lotto" HeaderText="Id Interno" SortExpression="Id_Lotto">
                                        </asp:BoundField>
                                        <asp:ButtonField ItemStyle-HorizontalAlign="Center" Text="&lt;img src='../AB_Immagini/icone16/Gomma16.ico' border='0'&gt; "
                                            HeaderText="Cancella" CommandName="Cancella">
                                            <ItemStyle CssClass="btn_per_load" />
                                        </asp:ButtonField>
                                    </Columns>
                                    <HeaderStyle CssClass="ui-widget-header" />
                                    <PagerStyle CssClass="ui-widget-header" HorizontalAlign="Center" />
                                </asp:GridView>
                            </div>
                        </div>
                        <div class="clear">
                        </div>
                    </div>
                    <div id="Div4" runat="server" visible="true">
                        <div style="width: 100%; margin-top: 5px;">
                            <asp:Label runat="server" ID="Label24" BackColor="#EAF4FD">Salvataggio</asp:Label>
                        </div>
                        <div class="box">
                            <div class="" style="float: left; width: 200px;">
                                <div class="descrizione" style="width: 110px">
                                    <asp:Label ID="Lbl_SalvaMagazzino" runat="server" Width="100px">
                                            Salva l'operazione</asp:Label>
                                </div>
                                <div class="valoriinput" style="width: 49px">
                                    <asp:ImageButton ID="ImgBtnSalvaTutto" runat="server" Width="32px" ImageUrl="../AB_Immagini/Icone32/dischetto.ico">
                                    </asp:ImageButton>
                                </div>
                            </div>
                            <div class="" id="DivRitorno" runat="server" visible="false" style="float: left;
                                width: 550px;">
                                <div class="descrizione" style="width: 190px">
                                    <asp:Label ID="Label25" runat="server">Modalità di Trasferimento nell'azienda di origine della raccolta</asp:Label>
                                </div>
                                <div class="">
                                    <asp:DropDownList ID="ComboTipoTrasferimento" Enabled="false" Style="width: 250px"
                                        runat="server">
                                        <asp:ListItem Text="Nessuno" Value="0" Selected="False"></asp:ListItem>
                                        <asp:ListItem Text="Carico/scarico Diretto" Value="1" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Conferimento" Value="2" Selected="False"></asp:ListItem>
                                        <asp:ListItem Text="Scarico e DDT" Value="3" Selected="False"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="" style="float: left; width: 200px;">
                                <div class="descrizione" style="width: 110px">
                                    <asp:Label ID="Label28" runat="server" Width="100px">
                                            Rintraccia</asp:Label>
                                </div>
                                <div class="valoriinput" style="width: 49px">
                                    <asp:ImageButton ID="ImageButtonRintraccia" runat="server" Width="32px" ImageUrl="../AB_Immagini/Icone32/lente.ico">
                                    </asp:ImageButton>
                                </div>
                            </div>
                            <div class="clear">
                            </div>
                        </div>
                        <div class="clear">
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
