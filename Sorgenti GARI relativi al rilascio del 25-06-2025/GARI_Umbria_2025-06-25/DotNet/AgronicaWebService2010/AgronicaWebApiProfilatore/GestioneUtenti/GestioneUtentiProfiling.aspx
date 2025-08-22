<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="GestioneUtentiProfiling.aspx.vb" Inherits="AgronicaWebApiProfilatore.GestioneUtentiProfiling" MasterPageFile="~/Master/Profilatore_Bootstrap.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
<style type="text/css">
    .errorClass {
        border-color:#D41E1A;
        border-width: 1px;
        border-style: dotted;
        background-color: Yellow;
    }
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">   
   
    <div id="panelArea" class="panel-group searchArea">
      
       <div class="row">
        <div class="col-lg-12 col-md-12 col-sm-12">

            <div class="jumbotron" style="padding-bottom: 0;">
                <div class="container" style="width: 100%;">
                    <div class="container_tabUtenti" style="padding: 0; /*margin-bottom: 70px*/">
                       
                        <div class="panel-group Indice">

                        
                            
                            <%--RICERCA--%>
                       <div class="row">    
 



                               <div class="col-lg-3 col-md-3 col-sm-12" id="id_data">
			                        <div class="form-horizontal">
                                       <div class="input-group">
                                            <label class="input-group-addon lbl_required" id="lblDataScadenza" for="txtDataInizio">
                                                <asp:Localize runat="server">A Data</asp:Localize>:
                                            </label>
                                            <input ID="txtDataInizio" name="txtDataInizio" class="kendoCalendar" style="width: 100%;" MaxLength="10" />                            
                                        </div>
                                    </div>				       
                               </div>


                               <div class="col-lg-2 col-md-2 col-sm-12">                            
                                    <div class="btn btn-success" id="btn_ricerca_Profiling">
                                    <span class="fa fa-search lampeggiante"></span><span class="lampeggiante">Ricerca
                                        <%-- <asp:Localize Text="<%$ Resources: AgronicaAgenda_2010, SalvaEdEsci %>" runat="server">Salva ed Esci</asp:Localize>--%>
                                        </span>
                                   </div>
                               </div>         

                        </div>
                             
                       

                            
                            <%-- ESTENSIONE--%>
                       <div class="row">    
 
                          
                           <%-- GRIGLIA--%>
                            <div class="row">
                                <div class="col-lg-12 col-md-12 col-sm-12" id="id_riga_utenti">
                                    <!--Griglia-->
                                    <div style="overflow: auto; margin-top: 10px; margin-bottom: 70px;">
                                        <div id="tab_griglia_utenti_profiling"></div>
                                    </div>
                                </div>

                            </div>
                        </div>

                    </div>

                </div>

            </div>

          </div>
        
       </div> 
  
   </div> 

   <div id="dialogConferma"></div>

   <%-- <input type="hidden" id="hdId_Area" runat="server" />
    <input type="hidden" id="hdId_Indice" runat="server" />
    <input type="hidden" id="hdRiservato" runat="server" />
    <input type="hidden" id="hdPiva" runat="server" />
    <input type="hidden" id="hdPiva_Codificata" runat="server" />--%>
   <%-- <input type="hidden" id="hdKendo_risultatiLettura" runat="server" />
    <input type="hidden" id="hdPaginaRedirect" runat="server" />--%>


    

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentScript" runat="server">
    
<%--    <script type="text/javascript" src="<%= AgronicaControlli_2010.Minify.LinkJsMin(ResolveClientUrl("~/ScriptsGestionali/funzioni_comuni.js")) %>"></script>
    <script type="text/javascript" src="<%= AgronicaControlli_2010.Minify.LinkJsMin(ResolveClientUrl("~/ScriptsGestionali/leggitabelle_ws_client.js")) %>"></script>
--%>
    <script type="text/javascript" src="<%= AgronicaControlli_2010.Minify.LinkJsMin("GestioneUtentiProfiling.js") %>"></script>
    <script type="text/javascript" src="<%= AgronicaControlli_2010.Minify.LinkJsMin("GestioneUtentiProfiling_globali.js") %>"></script>
    <script type="text/javascript" src="<%= AgronicaControlli_2010.Minify.LinkJsMin("GestioneUtentiProfiling_jQueryDocReady.js") %>"></script>
    <script type="text/javascript" src="<%= AgronicaControlli_2010.Minify.LinkJsMin("GestioneUtentiProfiling_ws_client.js") %>"></script>

    <script type="text/javascript">
        <%--var objP_server = '<%=objparametri_server_string %>';
        var cIdPiva = "#<%=hdPiva.ClientID() %>";
        var cPiva_Codificata = "#<%=hdPiva_Codificata.ClientID() %>";
        var cId_Area = "#<%=hdId_Area.ClientID() %>";
        var cId_Indice = "#<%=hdId_Indice.ClientID() %>";
        var cRiservato = "#<%=hdRiservato.ClientID() %>";--%>
       <%-- var cKendo_risultatiLettura = "#<%=hdKendo_risultatiLettura.ClientID() %>";
        var paginaRedirect = "#<%=hdPaginaRedirect.ClientID() %>";--%>

</script>

</asp:Content>