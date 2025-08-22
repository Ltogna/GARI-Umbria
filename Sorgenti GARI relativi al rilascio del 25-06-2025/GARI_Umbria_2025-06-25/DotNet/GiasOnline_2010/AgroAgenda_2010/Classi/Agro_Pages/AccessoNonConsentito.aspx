<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/AgendaBootstrap.Master" CodeBehind="AccessoNonConsentito.aspx.vb" Inherits="AgroAgenda_2010.AccessoNonConsentito" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentScript" runat="server"></asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <p>
        <asp:Localize Text="<%$ Resources: AgronicaAgenda_2010, Attenzione %>" runat="server">Attenzione!</asp:Localize></p>
    <p>
        <asp:Localize Text="<%$ Resources: AgronicaAgenda_2010, MancanzaPermessiPagina %>" runat="server">Non si hanno i permessi per accedere a questa pagina/funzione</asp:Localize></p>
</asp:Content>
