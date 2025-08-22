<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/Operazione.master"
    CodeBehind="NonUtilizzo.aspx.vb" Inherits="AgroAgenda_2010.NonUtilizzo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentOperazioniHeader" runat="server">
    <style type="text/css">
        .classComboFertilizzanti .ui-autocomplete-input
        {
            min-width: 525px;
            width: 90%;
        }
    </style>
    <script type="text/javascript">



        function UDMDoseCambiata() {

        }

        function CopiaValoriColonnaTipoIrrig(pippo) {
        }

        function CopiaValoriColonna(pippo) {

        }

        function getUDM() {

        }


        function DoseCambiata(pippo) {

        }

        function AggiornaDose(pippo) {

        }

        function OreCambiata(pippo) {

        }

        function PortataCambiata(pippo) {

        }

        function QtaTotCambiata(pippo) {

        }


        function DoPostBack_ControlliSiNo(key) {
        }

        function Info() {

        }

        function AcquaTotChecked() {
        }

        function Abilita_Disabilita_ACQUA() {
        }

        function DoseHAChecked() {
        }

        function QtaTOTChecked() {
        }

        function Abilita_Disabilita_DOSI() {
        }


        /////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////


        ///////////////////////////////////
        ////////RICAVA INSERISCI///////////
        ///////////////////////////////////

        function SupTrattata() {

        }

        function InserisciSupTrattata(valore) {

        }

        function SupTotale() {

        }

        function InserisciSupTotale(valore) {

        }

        function AcquaTot() {
        }

        function InserisciAcquaTot(valore) {
        }

        function AcquaHA() {
        }

        function InserisciAcquaHA(valore) {
        }

        function DoseHA() {
        }

        function InserisciDoseHA(valore) {
        }

        function DoseHL() {
        }

        function InserisciDoseHL(valore) {
        }

        function TotHA() {
        }

        function InserisciTotHA(valore) {
        }

        function TotHL() {
        }

        function InserisciTotHL(valore) {
        }

        function AggiornaACQUA() {
        }


        function AggiornaDOSI() {
        }

        function AcquaTot_Keyup() {
        }
        function AcquaHA_Keyup() {
        }

        function AggiornaDopo_SupTrattata() {
            //Aggiorno i costi accessori
            CalcolaCostiAccessori();
        }


        function PulisciGrigliaAv_GrAv() {
        }

        $(document).ready(function () {

        });

        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlacexTRATTAMENTI" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentOperazioniContenuti" runat="server">
    <div>
        <b>
            <asp:Label ID="LabelInserisciCentri" runat="server" Text=" Inserisci i Centri da Rilevare "
                meta:resourcekey="LabelInserisciCentriResource1"></asp:Label>
        </b>
        <asp:UpdatePanel ID="upSeg" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
            <ContentTemplate>
                <div id="dSegnalazioni" style="padding: 10px">
                    <b>
                        <asp:Label ID="lErroriSegnalazioni" runat="server" Text="" ForeColor="Red"></asp:Label></b>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="BTN_ChangeData" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <div style="font-size: 10px; width: 100%" class="sfondoverde" id="sfondoverdeCentri"
        runat="server" align="center">
        <div style="width: 100px; height: 40px;">
            <div class="clear">
                <br />
                <asp:ImageButton ID="IMGB_InserisciCentri" runat="server" Height="23px" ImageUrl="~/AB_Immagini/Icone32/FrecciaDN.ico"
                    Width="32px" meta:resourcekey="IMGB_InserisciCentriResource1" />
                <asp:ImageButton ID="IMGB_RimuoviCentri" runat="server" Height="23px" ImageUrl="~/AB_Immagini/Icone32/FrecciaUP.ico"
                    Width="32px" meta:resourcekey="IMGB_RimuoviCentriResource1" />
                <br />
            </div>
        </div>
    </div>
    <div>
        <asp:GridView ID="GridView_NonUtilizzo" runat="server" AutoGenerateColumns="False"
            Width="90%" CellPadding="5" CssClass="ui-widget-content" Caption="Dichiarazioni di non utilizzo"
            meta:resourcekey="GridView_PioggeResource1">
            <Columns>
                <asp:TemplateField meta:resourcekey="TemplateFieldResource8">
                    <HeaderTemplate>
                        <input type="checkbox" id="chkSelezionaTuttiRilievi" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="ChkSelezionaRilievo" Checked="True" runat="server" CssClass="ChkSelezionaRilievo"
                            meta:resourcekey="ChkSelezionaRilievoResource1" />
                    </ItemTemplate>
                    <HeaderStyle Width="20px" />
                    <ItemStyle Width="20px" />
                    <FooterStyle Width="20px" />
                    <ControlStyle Width="20px" />
                </asp:TemplateField>
                <asp:BoundField DataField="piva" HeaderText="piva" SortExpression="piva" Visible="False">
                </asp:BoundField>
                <asp:BoundField DataField="Rag_Soc" HeaderText="Ragione Sociale" SortExpression="Rag_Soc"
                    Visible="False"></asp:BoundField>
                <asp:BoundField DataField="sa_cod" HeaderText="sa_cod" SortExpression="sa_cod" Visible="False">
                </asp:BoundField>
                <asp:BoundField DataField="Sa_Nome" HeaderText="Centro Aziendale" SortExpression="Sa_Nome">
                </asp:BoundField>
            </Columns>
            <HeaderStyle CssClass="ui-widget-header" />
            <PagerStyle CssClass="ui-widget-header" HorizontalAlign="Center" />
        </asp:GridView>
        <br />
    </div>
</asp:Content>
