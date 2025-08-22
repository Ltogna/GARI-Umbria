<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/AgendaBootstrap.Master" CodeBehind="Prodotto_Edit.aspx.vb" Inherits="AgroAgenda_2010.Prodotto_Edit" %>

<%@ Import Namespace="AgronicaCoreDataProvider.TipiEnumerativi" %>
<%@ MasterType VirtualPath="~/Master/AgendaBootstrap.Master" %>
<%@ Register TagPrefix="ucProd" TagName="Prodotto_Edit_UC" Src="~/Anagrafica/UserControl/Prodotto_Edit_UC.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div data-toggle="validator" role="form" class="row ">

        <div class="row rowTopHeaderButton">
            <% If winProdotto Then %><br /><% End If %>
            <div class="col-lg-12 col-md-12 col-sm-12 text-right ">
                <%If (permessi.getPermesso(enum_Security_Attivita.Angrafica_Prodotti).Scrittura = True) And (XTipoOperazione <> enum_TipoOperazioneDB.Lettura) Then%>

                <div id="prodotto_Edit_UC_Salva" class="btn btn-success xi-btn-primary" onclick="prodotto_Edit_UC_ValidaxSubmit(false,false);">
                    <%-- i18N Salva da tradurre--%>
                    <i class="fa fa-floppy-o"></i>Salva
                </div>
                <div id="prodotto_Edit_UC_SalvaEsci" class="btn btn-success xi-btn-primary" onclick="prodotto_Edit_UC_ValidaxSubmit(false, true);">
                    <%-- i18N Salva ed Esci da tradurre--%>
                    <i class="fa fa-floppy-o"></i>Salva ed Esci
                </div>
                <div id="prodotto_Edit_UC_SalvaNuovo" class="btn btn-success xi-btn-primary" onclick="prodotto_Edit_UC_ValidaxSubmit(true, false);">
                    <%-- i18N Salva e Nuovo da tradurre--%>
                    <i class="fa fa-floppy-o"></i>Salva e Nuovo
                </div>
                <div id="prodotto_Edit_UC_Annulla" class="btn btn-danger" onclick="prodotto_Edit_UC_Annulla();" >
                    <%-- i18N Annulla da tradurre--%>
                    <i class="fa fa-undo"></i>Annulla
                </div>

                <asp:HiddenField ID="tipo_salva" runat="server" />
                <asp:ImageButton ID="ImgBtn_AnnullaTutto" ImageUrl="~/AB_Immagini/icone32/esci.bmp"
                        runat="server" Style="float: right; display:none" />
            </div>
            <% End If%>
        </div>

         <div id="tb_prodotti" class="row">
            <ucProd:Prodotto_Edit_UC id="prodottoUC" runat="server" />
        </div>

    </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScript" runat="server">

    <input type="hidden" id="hfPaginaRedirect" runat="server" />
    <input type="hidden" id="hf_UtenteAbilitatoLettura" runat="server" />
    <input type="hidden" id="hf_UtenteAbilitatoScrittura" runat="server" />
    <input type="hidden" id="hf_TipoOperazione" runat="server" />
    <input type="hidden" id="hf_Opzioni_Materie_Prime" runat="server" />
    <input type="hidden" id="hf_Piva" runat="server" />
    <input type="hidden" id="hf_Mat_Cod" runat="server" />
    <input type="hidden" id="hf_Elem_Cod" runat="server" />
    <input type="hidden" id="hf_Pro_Cod" runat="server" />
    <input type="hidden" id="hf_Prodotto_Des" runat="server" />
    <input type="hidden" id="hf_xProprietario" runat="server" />
    <input type="hidden" id="hf_Sa_Cod" runat="server" />
    <input type="hidden" id="hf_Ragione_Sociale" runat="server" />       
    <input type="hidden" id="hf_Piva_Corrente" runat="server" />
    <input type="hidden" id="hf_DatiImmagineCaricata" runat="server" />
    <input type="hidden" id="hf_Is_Alias" runat="server" />
    <input type="hidden" id="hdSuperUserAccGerarchia" runat="server" />
    <input type="hidden" id="hfGruppoMerceControllo" runat="server" />

    <script type="text/javascript" src="<%= AgronicaControlli_2010.Minify.LinkJsMin("Prodotto_Edit_jQueryDocReady.js") %>"></script>
    <script type="text/javascript" src="<%= AgronicaControlli_2010.Minify.LinkJsMin(ResolveClientUrl("~/ScriptsGestionali/leggitabelle_ws_client.js")) %>"></script>
    <script type="text/javascript" src="<%= AgronicaControlli_2010.Minify.LinkJsMin(ResolveClientUrl("~/ScriptsGestionali/leggiArrayCostanti.js")) %>"></script>
    <script type="text/javascript" src="<%= AgronicaControlli_2010.Minify.LinkJsMin(ResolveClientUrl("~/ScriptsGestionali/funzioni_comuni.js")) %>"></script>

    <script>
        var Controls = {
            xPiva: "#<%=hf_Piva.ClientID %>",
            xTipoOperazione: "#<%=hf_TipoOperazione.ClientID %>",
            xOpzioni: "#<%=hf_Opzioni_Materie_Prime.ClientID %>",
            xMat_Cod: "#<%=hf_Mat_Cod.ClientID %>",
            xElem_Cod: "#<%=hf_Elem_Cod.ClientID %>",
            xPro_Cod: "#<%=hf_Pro_Cod.ClientID %>",
            xProdotto_Des: "#<%=hf_Prodotto_Des.ClientID %>",
            xProprietario: "#<%=hf_xProprietario.ClientID %>",
            xRagioneSociale: "#<%=hf_Ragione_Sociale.ClientID %>",
            xPaginaRedirect: "#<%=hfPaginaRedirect.ClientID %>",
            xPiva_Corrente: "#<%=hf_Piva_Corrente.ClientID %>"
        };
        var cIdPiva = "#<%=hf_Piva.ClientID %>";
        var xSa_Cod = "#<%=hf_Sa_Cod.ClientID %>";
        var winProdotto = <%= If(winProdotto, "true", "false") %>;
        var xDatiImmagineCaricata = "#<%=hf_DatiImmagineCaricata.ClientID %>";
        var xIs_Alias = "#<%=hf_Is_Alias.ClientID %>";
        var cIdSuperUserAccGerarchia = "#<%= hdSuperUserAccGerarchia.ClientID %>";
        var cIdGruppoMerceControllo = "#<%= hfGruppoMerceControllo.ClientID %>";
    </script>


</asp:Content>
