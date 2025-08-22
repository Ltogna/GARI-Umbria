<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/Operazione.master"
    ValidateRequest="false" EnableEventValidation="false" CodeBehind="Semina_e_Trapianto_1.aspx.vb"
    Inherits="AgroAgenda_2010.Semina_e_Trapianto_1" meta:resourcekey="PageResource1" %>

<%@ Register Assembly="AgronicaControlli_2010" Namespace="AgronicaControlli_2010"
    TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentOperazioniHeader" runat="server">
    <style type="text/css">
        .classComboFertilizzanti .ui-autocomplete-input {
            min-width: 525px;
            width: 90%;
        }

        .disabled {
            pointer-events: none;
        }
    </style>
    <script type="text/javascript">
        function addUnidDDT(unidDDT) {
            $('#<%= DDTAllegati.ClientID %>').val(unidDDT);
            $('#<%= BottoneNascostoAllegaDDT.ClientID %>').click();
        }

        var Enum_TargetOperazione = {
            Reale: { value: 1, name: "Reale", code: 1 },
            Planning: { value: 2, name: "Planning", code: 2 }
        };

        var objParametriAgenda_TargetOperazione = <%=objParametriAgenda_TargetOperazione %>;

        var GridView_ClientID = "";
        var GridView_Class_ChkSel = "";

        if (objParametriAgenda_TargetOperazione == Enum_TargetOperazione.Reale) {
            GridView_ClientID = "#<%= GridViewImpianti.ClientID %>";
            GridView_Class_ChkSel = ".ChkSelezionaImp";
        } else {
            GridView_ClientID = "#<%= GridViewPlanning.ClientID %>";
            GridView_Class_ChkSel = ".ChkSelezionaImpPlanning";
        }

        /////////////////////////////////////////////////
        //per distribuzione semi in base superfici impianti
        function CalcolaSupSelezionate_Semina_Con_Frazionamento_perLotto(mat_cod,lotto) {
            var sup = 0.00;

            $(GridView_ClientID).find(GridView_Class_ChkSel).each(function () {
                if ($(this).children('input').is(':checked')) {

                    if (($($(this).parent().parent().find('.mat_cod_lotto')).text() === mat_cod) && ($($(this).parent().parent().find('.lotto_lotto')).text() === lotto))  {

                        $(this).parent().parent().find('.Sup_App').each(function () {
                            sup += parseFloat($(this).html().replace(',', '.'));
                        });

                    }
                }
            });

            return sup;
        }

        function DistribuisciQtaSeme_Semina_Con_Frazionamento_perLotto(pippo) {

            if ($(pippo).val() !== '') {
                var Qta = $(pippo).val();
                var mat_cod = $($(pippo).parent().parent().find('.mat_cod')).text();
                var lotto = $($(pippo).parent().parent().find('.lotto')).text();
                var sup = CalcolaSupSelezionate_Semina_Con_Frazionamento_perLotto(mat_cod,lotto);

                var id_check = $(pippo).parent().parent().find('.ChkSeleziona').children().attr('id');

                var numero_riga;
                var conta = 0;

                $('#<%=GridViewMagazzino.ClientID %>').find('.ChkSeleziona').children('input:checked').each(function () {

                    if ($(this).attr('id') == id_check)
                        numero_riga = conta;
                    else
                        conta++;
                });

                var somma = 0;

                $(GridView_ClientID).find(GridView_Class_ChkSel).each(function () {

                    var mat_cod_questo = $($(this).parent().parent().find('.mat_cod_lotto')).text();
                    var lotto_questo = $($(this).parent().parent().find('.lotto_lotto')).text();


                    if ((mat_cod === mat_cod_questo) && (lotto === lotto_questo)) {

                        if ($(this).children('input')[0].checked === true) {

                            var sup_questo = $(this).parent().parent().find('.Sup_App').html().replace(',', '.');

                            var moltiplica = (Qta / sup * sup_questo)
                            if (moltiplica !== roundNumber(moltiplica, 0)) {
                                moltiplica = roundNumber((moltiplica + 0.5), 0);
                            }

                            if (moltiplica < 1)
                                moltiplica = 1;

                            if (Qta < somma + moltiplica)
                                moltiplica = Qta - somma;

                            var txt = $(this).parent().parent().find('.TxtPianteImpianto');
                            $(txt).val(moltiplica + "");
                            somma += moltiplica;

                            TxtPianteImpianto_KeyUp(txt);
                            //$(txt).onkeyup();

                        } else {

                            var txt = $(this).parent().parent().find('.TxtPianteImpianto');
                            $(txt).val('0');
                        }


                    }

                });

            }
        }

        /////////////////////////////////////////////////
        //per distribuzione semi in base superfici impianti
        function CalcolaSupSelezionate_Semina_Standard() {
            var sup = 0.00;
            var num = 0.00;

            $(GridView_ClientID).find(GridView_Class_ChkSel).each(function () {
                if ($(this).children('input').is(':checked')) {
                    num = num + 1;
                    $(this).parent().parent().find('.Sup_App').each(function () {
                        sup = sup + parseFloat($(this).html().replace(',', '.'));
                    });
                }
            });
            return sup;
        }


        function DistribuisciQtaSeme_Semina_Standard(pippo) {

            if ($(pippo).val() != '') {
                var Qta = $(pippo).val();
                var sup = CalcolaSupSelezionate_Semina_Standard();

                var id_check = $(pippo).parent().parent().find('.ChkSeleziona').children().attr('id');

                var numero_riga;
                var conta = 0;

                $('#<%=GridViewMagazzino.ClientID %>').find('.ChkSeleziona').children('input:checked').each(function () {

                    if ($(this).attr('id') == id_check) {
                        numero_riga = conta;
                    }
                    else {
                        conta++;
                    }
                });

                var somma = 0;

                $(GridView_ClientID).find(GridView_Class_ChkSel).children('input:checked').each(function () {
                    var sup_questo;
                    sup_questo = $(this).parent().parent().parent().find('.Sup_App').html().replace(',', '.');
                    var id_text_partenza = $(this).parent().parent().parent().find('.TxtPianteImpianto').attr('id');

                    var i;
                    var trovato = false;
                    var conta_decrescente = numero_riga;

                    $(GridView_ClientID).find('.TxtPianteImpianto').each(function () {
                        if ($(this).attr('id') == id_text_partenza)
                            trovato = true;

                        if (trovato == true)
                            conta_decrescente = conta_decrescente - 1;

                        if (conta_decrescente == -1 && trovato == true) {
                            trovato = false;
                            var moltiplica = (Qta / sup * sup_questo)

                            if (moltiplica !== roundNumber(moltiplica, 0)) {
                                moltiplica = roundNumber((moltiplica + 0.5), 0);
                            }

                            if (moltiplica < 1) {
                                moltiplica = 1;
                            }

                            if (Qta < somma + moltiplica) {
                                moltiplica = Qta - somma;
                            }

                            $(this).val(moltiplica + "");
                            somma += moltiplica;
                        }
                    });
                });
            }
        }

        /////////////////////////////////////////////////
        /////////////////////////////////////////////////


        function DistribuisciQtaSeme(pippo) {

            var TipoSemina = "40";
            var listItems = $('#<%=RBL_Tipo_Semina.ClientID%>').find("input");
            for (var i = 0; i < listItems.length; i++) {
                if (listItems[i].checked) {
                    TipoSemina = listItems[i].value;
                }
            }

            if ((TipoSemina === "40") || (TipoSemina === "30")) {//standard o solo semina
                DistribuisciQtaSeme_Semina_Standard(pippo)
            } else if (TipoSemina === "50") {//Con Frazionameno per lotto
                DistribuisciQtaSeme_Semina_Con_Frazionamento_perLotto(pippo)
            }
        }


        /////////////////////////////////////////////////
        /////////////////////////////////////////////////


        function colora_varieta() {
            var precedente = "";

            $('.classe_varieta').each(function () {
                var questo;
                questo = $(this).html();
                if (questo != precedente) {
                    precedente = questo;
                    $(this).parent().css("border-top", "4px solid #555");
                }
            });
        }

        -

            function CopiaValoriColonnaCmbUdm(pippo) {
                $(pippo).parent().parent().parent().find(".CmbUdm").val($(pippo).val());
            }

        function SelezionaDeselezionaTuttiImp() {
            if ($('#chkSelezionaTuttiImp').is(':checked')) {

                //seleziono tutto
                $('.ChkSelezionaImp').each(function () {
                    $(this).children('input').attr('checked', 'checked');
                    ChkSelezionaImp_Click($(this).children('input'));
                });
            }
            else {

                //seleziono tutto
                $('.ChkSelezionaImp').each(function () {
                    $(this).children('input').removeAttr('checked');
                    ChkSelezionaImp_Click($(this).children('input'));
                });
            }
        }

        function SelezionaDeselezionaTuttiPlanning() {
            if ($('#chkSelezionaTuttiImpPlanning').is(':checked')) {

                //seleziono tutto
                $('.ChkSelezionaImpPlanning').each(function () {
                    $(this).children('input').attr('checked', 'checked');
                    ChkSelezionaImp_Click($(this).children('input'));
                });
            }
            else {

                //seleziono tutto
                $('.ChkSelezionaImpPlanning').each(function () {
                    $(this).children('input').removeAttr('checked');
                    ChkSelezionaImp_Click($(this).children('input'));
                });
            }
        }




        /////////////////////////////////////////////////////////
        ////////CALCOLO COSTI ACCESSORI AUTOMATICO///////////////

        function ChkSelezionaImp_Click(Oggetto) {
            var SupTot = 0.00;
            //sel la checkbox è chekkata
            $(GridView_Class_ChkSel).children('input:checked').each(function () {
                var app = ValoreSuperficie_ImpSemina($(this));
                SupTot += parseFloat(app);
            });
            CalcolaCostiAccessori(SupTot);
        }

        function ValoreSuperficie_ImpSemina(Oggetto) {
            return Oggetto.parent().parent().parent().children('.Sup_App').html().replace(',', '.');
        }


        function CalcolaCostiAccessori(sup) {
            var flag = $(".CostiAperti").children().is(':checked');
            if (flag == true) {
                if (sup > 0) {
                    aggiornaCostiSuServer(sup);
                    $('.GridViewCostiAccessoriVisibili').find('.UdmCosti').each(function () {
                        var udm = $(this).val();
                        if (udm == 1) {
                            InserisciQtaCosti($(this), sup)
                            var CostoUnitario = ValoreCostoUnitario($(this));
                            if (CostoUnitario != 0) {
                                var tot = CostoUnitario * sup
                                InserisciCosto($(this), tot);
                            }
                        }
                        else if (udm == 2) {

                            var minuti = Number($(this).parent().parent().children('.ore').html());
                            minuti = minuti * 60;
                            minuti = minuti + Number($(this).parent().parent().children('.minuti').html());
                            minuti = minuti * sup;

                            var ore = Math.floor(minuti / 60);
                            var resto = minuti - (ore * 60);
                            // InserisciQtaCosti
                            var OreDecimal = ore.toString() + "," + (Math.floor(resto * 100 / 60)).toString()
                            $(this).parent().parent().find('.QtaCosti').val(OreDecimal);

                            var CostoUnitario = ValoreCostoUnitario($(this));
                            if (CostoUnitario != 0) {
                                var tot = CostoUnitario * Number(OreDecimal.replace(",", "."));
                                InserisciCosto($(this), tot);
                            }
                        };
                    });
                };
            };
        }

        function InserisciQtaCosti(Oggetto, valore) {
            var sup = roundNumber(valore, 4) + "";
            sup = sup.replace(".", ",");
            $(Oggetto).parent().parent().find('.QtaCosti').val(sup);
        }

        function InserisciCosto(Oggetto, valore) {
            var tot = roundNumber(valore, 4) + "";
            tot = tot.replace(".", ",");
            $(Oggetto).parent().parent().find('.Costo').html(tot);
        }

        function ValoreCostoUnitario(Oggetto) {
            return $(Oggetto).parent().parent().children('.CostoUnitario').html().replace(',', '.');
        }

        function ValoreUDMCostoUnitario(Oggetto) {
            return $(Oggetto).parent().parent().children('.UdmCosti').val();
        }

        function aggiornaCostiSuServer(sup) {
            var Attesa;
            $.ajax({
                type: "POST",
                url: "trattamenti_2.aspx/Update_Sup_Costi",
                data: "{ sup: '" + sup + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {

                },
                error: function (xhr, ajaxOptions, thrownError) {
                    alert(xhr.status);
                    alert(thrownError);
                }
            });
        }

        /////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////


        //per la Gestione dell'Eliminazione 
        function DoPostBack_Combo_Slave($_combo, valoreOpt) {
        }

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
            if (key == 'Giacenze') {
                $("#<%=Giacenza_SI_NO.ClientID %>").val("OK");
                $("#<%=Master_Operazione.Property_ImgBtn_Salva.ClientID %>").click();
            }
            if (key == 'Catasto') {
                $("#<%=Catasto_SI_NO.ClientID %>").val("OK");
                $("#<%=Master_Operazione.Property_ImgBtn_Salva.ClientID %>").click();
            }
            if (key == 'MateriaPrimaBio') {
                $("#<%=MateriaPrimaBio_SI_NO.ClientID %>").val("OK");
                $("#<%=Master_Operazione.Property_ImgBtn_Salva.ClientID %>").click();
            }
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
        }


        function PulisciGrigliaAv_GrAv() {
        }

        $(document).ready(function () {
            setTimeout(function () { $("#tabs").tabs("option", "active", 1); }, 500); // delays 1.5 sec

        });

        //Grilli:Da Fabrizio (14/06/2018):
        //Se cambio le piantine totali, tengo fermo le p/ha e ricalcolo la superficie
        //Se cambio le p/ha, tengo fermo le piantine totali e ricalcolo la superficie
        //Se cambio la superficie, tengo fermo le piantine totali e ricalcolo le p/ha

        function TxtSupApp_KeyUp(obj) {

            var TxtSupApp = $(obj);
            var SupApp = $(TxtSupApp).val().replace(",", ".");

            var Txtpha = $(obj).parent().parent().find(".Txtpha");
            var pha = $(Txtpha).val().replace(",", ".");

            var TxtPianteImpianto = $(obj).parent().parent().find(".TxtPianteImpianto");
            var PianteImpianto = $(TxtPianteImpianto).val().replace(",", ".");

            if ($.isNumeric(SupApp) && $.isNumeric(PianteImpianto) && SupApp !== "0" && PianteImpianto !== "0")
                Txtpha.val((PianteImpianto / SupApp).toString().replace(".", ","));
        }

        function Txtpha_KeyUp(obj) {
            var TxtSupApp = $(obj).parent().parent().find(".TxtSupApp");
            var SupApp = $(TxtSupApp).val().replace(",", ".");

            var Txtpha = $(obj);
            var pha = $(Txtpha).val().replace(",", ".");

            var TxtPianteImpianto = $(obj).parent().parent().find(".TxtPianteImpianto");
            var PianteImpianto = $(TxtPianteImpianto).val().replace(",", ".");

            if ($.isNumeric(PianteImpianto) && $.isNumeric(pha) && pha !== "0" && PianteImpianto !== "0")
                TxtSupApp.val((PianteImpianto / pha).toString().replace(".", ","));

        }

        function TxtPianteImpianto_KeyUp(obj) {
            var TxtSupApp = $(obj).parent().parent().find(".TxtSupApp");;
            var SupApp = $(TxtSupApp).val().replace(",", ".");

            var Txtpha = $(obj).parent().parent().find(".Txtpha");
            var pha = $(Txtpha).val().replace(",", ".");

            var TxtPianteImpianto = $(obj);
            var PianteImpianto = $(TxtPianteImpianto).val().replace(",", ".");;

            if ($.isNumeric(pha) && $.isNumeric(PianteImpianto) && pha !== "0" && PianteImpianto !== "0")
                TxtSupApp.val((PianteImpianto / pha).toString().replace(".", ","));

        }



    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlacexTRATTAMENTI" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentOperazioniContenuti" runat="server">
    <asp:HiddenField ID="hf_UTENTE_COD_SEMINA_TIPO" runat="server" />

    <!-- hidden per si no  -->
    <asp:UpdatePanel ID="updateGiacenze_si_no" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="Giacenza_SI_NO" runat="server" Value="0" />
            <asp:HiddenField ID="Catasto_SI_NO" runat="server" Value="0" />
            <asp:HiddenField ID="MateriaPrimaBio_SI_NO" runat="server" Value="0" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="UpdatePanelMagazzinoImpianti" runat="server">
        <ContentTemplate>
            <asp:UpdatePanel ID="updatepanelGridViewMagazzinoImpianti" runat="server">
                <ContentTemplate>
                    <asp:Button ID="BottoneNascostoAllegaDDT" runat="server" Text="BottoneNascostoAllegaDDT"
                        Style="display: none" meta:resourcekey="BottoneNascostoAllegaDDTResource1" />
                    <asp:HiddenField ID="DDTAllegati" runat="server" />

                    <div class="box" >

                        <div style="width: 100%;">

                            <!-- Riga 1 -->
                            <div id="DivOpzioni" runat="server">

                                <div style="width: 100%; margin-top: 5px;">
                                    <asp:Label runat="server" ID="Label2" BackColor="#EAF4FD">Opzioni</asp:Label>
                                </div>

                                <div class="box" style="margin-top: 5px;">
                                    <div>
                                        <asp:RadioButtonList ID="RBL_Tipo_Semina" runat="server" AutoPostBack="true" >
                                            <asp:ListItem Text="Registra Solo Semina/Trapianto" Value="30" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Registra Semina/Trapianto ed Aggiorna l'anagrafica dell'Appezzamento/Impianto (App. Nome, Metodo Produttivo, Disciplinare, Nome Esercizio, Varietà, Finalità, N.Piante)" Value="40"></asp:ListItem>
                                            <asp:ListItem Text="Fraziona gli Appezzamenti in base ai Lotti delle Materie Prime e Registra le Semine/Trapianti sui nuovi Appezzamenti/Impianti" Value="50"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                </div>

                                <div class="clear">
                                </div>

                            </div>

                            <!-- Riga 2 -->
                            <div id="DivMagazzino" runat="server">

                                <div style="width: 100%; margin-top: 5px;">
                                    <asp:Label runat="server" ID="Label3" BackColor="#EAF4FD">Giacenze Magazzino</asp:Label>
                                </div>

                                <div class="box">
                                    <div class="" style="float: left; margin-top: 5px;">
                                        <asp:Image ID="ImageInfo0" runat="server" ImageUrl="~/AB_Immagini/icone32/Clip32.ico"
                                            Style="width: 14px" meta:resourcekey="ImageInfo0Resource1" />
                                        <asp:Label ID="LabelInfo0" runat="server" Text="Ci sono dei DDT allegati" meta:resourcekey="LabelInfo0Resource1"></asp:Label>
                                        <br />
                                        <asp:Image ID="ImageInfo1" runat="server" ImageUrl="~/AB_Immagini/Icone16/Esclamativo16.ico"
                                            Style="width: 14px" meta:resourcekey="ImageInfo1Resource1" />
                                        <asp:Label ID="LabelInfo1" runat="server" Text="Label" meta:resourcekey="LabelInfo1Resource1"></asp:Label>
                                        <br />
                                        <asp:Image ID="ImageInfo2" runat="server" ImageUrl="~/AB_Immagini/Icone16/Esclamativo16.ico"
                                            Style="width: 14px" meta:resourcekey="ImageInfo2Resource1" />
                                        <asp:Label ID="LabelInfo2" runat="server" Text="Label" meta:resourcekey="LabelInfo2Resource1"></asp:Label>
                                        <br />
                                        <asp:CheckBox ID="CheckBoxGiacenzePositive" Checked="True" CssClass="btn_per_load"
                                            runat="server" AutoPostBack="True" Text="Nascondi le giacenze 0" meta:resourcekey="CheckBoxGiacenzePositiveResource1" />
                                        <br />
                                        <asp:CheckBox ID="CheckBoxSeminaMultiSpecie" Checked="false" CssClass="btn_per_load"
                                            runat="server" AutoPostBack="True" Text="Multi-Specie" />
                                    </div>

                                    <div class="clear">
                                    </div>

                                    <div style="float: left; margin-top: 5px;">
                                        <asp:GridView ID="GridViewMagazzino" runat="server" AutoGenerateColumns="False" CellPadding="5"
                                            CssClass="ui-widget-content">
                                            <Columns>
                                                <asp:TemplateField meta:resourcekey="TemplateFieldResource1">
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="chkSelezionaTuttiGiacenze" runat="server" AutoPostBack="True" CssClass="chkSelezionaTuttiGiacenze btn_per_load"
                                                            OnCheckedChanged="GridViewMagazzino_SelectedIndexChanged" meta:resourcekey="chkSelezionaTuttiGiacenzeResource1" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="ChkSeleziona" runat="server" AutoPostBack="True" CssClass="ChkSeleziona btn_per_load"
                                                            OnCheckedChanged="GridViewMagazzino_SelectedIndexChanged" meta:resourcekey="ChkSelezionaResource1" />
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="20px" />
                                                    <ItemStyle Width="20px" />
                                                    <FooterStyle Width="20px" />
                                                    <ControlStyle Width="20px" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Veg_des" HeaderText="Specie Articolo" SortExpression="Veg_des"
                                                    meta:resourcekey="BoundFieldResource1"></asp:BoundField>
                                                <asp:BoundField DataField="Cul_Des" HeaderText="Varietà" meta:resourcekey="BoundFieldResource2">
                                                    <ItemStyle CssClass="classe_varieta" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Cod_Articolo" HeaderText="Codice Articolo" SortExpression="Cod_Articolo"
                                                    meta:resourcekey="BoundFieldResource3"></asp:BoundField>
                                                <asp:BoundField DataField="Lotto" HeaderText="Lotto di Accettazione" SortExpression="Lotto"
                                                    meta:resourcekey="BoundFieldResource4">                                                    <ItemStyle CssClass="lotto" />

                                                </asp:BoundField>
                                                <asp:BoundField DataField="Giacenza" HeaderText="Giacenza Rilevata" SortExpression="Giacenza"
                                                    meta:resourcekey="BoundFieldResource5"></asp:BoundField>
                                                <asp:BoundField DataField="Bolla_Doc" HeaderText="Numero Documento" Visible="False"
                                                    SortExpression="Bolla_Doc" meta:resourcekey="BoundFieldResource6"></asp:BoundField>
                                                <asp:BoundField DataField="Bolla_Des" HeaderText="Descrizione Articolo" Visible="False"
                                                    SortExpression="Bolla_Des" meta:resourcekey="BoundFieldResource7"></asp:BoundField>
                                                <asp:BoundField DataField="Bolla_Qta" HeaderText="Qta nel Documento" Visible="False"
                                                    SortExpression="Bolla_Qta" meta:resourcekey="BoundFieldResource8"></asp:BoundField>
                                                <asp:BoundField DataField="Bolla_Giacenza" HeaderText="Giacenza Documento" Visible="False"
                                                    SortExpression="Bolla_Giacenza" meta:resourcekey="BoundFieldResource9"></asp:BoundField>
                                                <asp:TemplateField HeaderText="Qta." SortExpression="Qta" meta:resourcekey="TemplateFieldResource2">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TxtQtaGiacenza" runat="server" CssClass="TxtQtaGiacenza" Text='0'
                                                            ToolTip="Inserire la quantità da utilizzare, verrà ripatita sugli impianti in base alle superfici. Il dato che fa fede è sempre quello inserito sugli impianti."
                                                            meta:resourcekey="TxtQtaGiacenzaResource1" />
                                                    </ItemTemplate>
                                                    <ControlStyle Width="40px" />
                                                    <HeaderStyle Width="35px" />
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="Mat_Cod" HeaderText="Mat_Cod" HeaderStyle-CssClass="displaynone">
                                                    <ItemStyle CssClass="mat_cod displaynone" />
                                                </asp:BoundField>

                                            </Columns>
                                            <HeaderStyle CssClass="ui-widget-header" />
                                            <PagerStyle CssClass="ui-widget-header" HorizontalAlign="Center" />
                                        </asp:GridView>
                                    </div>

                                    <div class="clear">
                                    </div>

                                </div>

                            </div>

                            <!-- Riga 3 -->
                            <div id="DivImpianti" runat="server">

                                <div style="width: 100%; margin-top: 5px;">
                                    <asp:Label runat="server" ID="Label4" BackColor="#EAF4FD">Impianti</asp:Label>
                                </div>

                                <div class="box">

                                    <div class="" style="float: left; margin-top: 5px;">
                                        <asp:RadioButtonList ID="rbl_Specie" runat="server" AutoPostBack="True" CssClass="btn_per_load">
                                            <asp:ListItem Text="Visualizza gli Impianti della Specie Selezionata ed i Terreni Nudi" Value="0" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Visualizza Solo gli Impianti della Specie Selezionata" Value="1" Selected="False"></asp:ListItem>
                                            <asp:ListItem Text="Visualizza Solo i Terreni Nudi" Value="2" Selected="False"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>

                                    <div class="clear">
                                    </div>

                                    <div class="" style="float: left; font-size: 12px; width: 100%;">
                                        <asp:Image ID="ImageInfo3" runat="server" ImageUrl="~/AB_Immagini/Icone16/Esclamativo16.ico"
                                            Style="width: 14px" meta:resourcekey="ImageInfo3Resource1" />
                                        <asp:Label ID="LabelInfo3" runat="server" Text="Label" meta:resourcekey="LabelInfo3Resource1"></asp:Label>
                                        <br />
                                        <asp:Image ID="ImageInfo4" runat="server" ImageUrl="~/AB_Immagini/Icone16/Esclamativo16.ico"
                                            Style="width: 14px" meta:resourcekey="ImageInfo4Resource1" />
                                        <asp:Label ID="LabelInfo4" runat="server" Text="Label" meta:resourcekey="LabelInfo4Resource1"></asp:Label>
                                        <br />
                                        <asp:Image ID="ImageInfo5" runat="server" ImageUrl="~/AB_Immagini/Icone16/Esclamativo16.ico"
                                            Style="width: 14px" meta:resourcekey="ImageInfo5Resource1" />
                                        <asp:Label ID="LabelInfo5" runat="server" Text="Label" meta:resourcekey="LabelInfo5Resource1"></asp:Label>
                                        <br />
                                    </div>

                                    <div class="clear">
                                    </div>

                                    <div class="" style="overflow-x: scroll; ">

                                        <asp:Button ID="AggiornaGrigliaImpiantiSemine" runat="server" Style="display: none;"
                                            meta:resourcekey="AggiornaGrigliaImpiantiSemineResource1" />

                                        <asp:GridView ID="GridViewImpianti" runat="server" AutoGenerateColumns="False" CellPadding="5" Width="100%"
                                            CssClass="ui-widget-content" Caption="Selezionare gli Impianti: <img src='../AB_Immagini/icone16/plus.png' alt='Aggiungi Colonne' id='btn_Impostazioni_ColonneSemine' style='margin-left:10px' />"
                                            meta:resourcekey="GridViewImpiantiResource1">
                                            <Columns>
                                                <asp:TemplateField meta:resourcekey="TemplateFieldResource3">
                                                    <HeaderTemplate>
                                                        <input type="checkbox" id="chkSelezionaTuttiImp" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="ChkSeleziona" runat="server" CssClass="ChkSelezionaImp" meta:resourcekey="ChkSelezionaResource2" />
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="20px" />
                                                    <ItemStyle Width="20px" />
                                                    <FooterStyle Width="20px" />
                                                    <ControlStyle Width="20px" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Rag_Soc" HeaderText="Ragione Sociale" SortExpression="Rag_Soc"
                                                    Visible="False" meta:resourcekey="BoundFieldResource10"></asp:BoundField>
                                                <asp:BoundField DataField="Sa_Nome" HeaderText="Centro Az." SortExpression="Sa_Nome"
                                                    meta:resourcekey="BoundFieldResource11"></asp:BoundField>
                                                <asp:BoundField DataField="Campo_Des" HeaderText="Campo" SortExpression="Campo_Des"></asp:BoundField>

                                                <asp:TemplateField HeaderText="App." SortExpression="App_Nome">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TxtAppNome" runat="server" />
                                                    </ItemTemplate>
                                                    <ControlStyle Width="150px" />
                                                    <HeaderStyle Width="90px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Metodo Produzione" SortExpression="MetodoProduttivo">
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="CmbMetodoProduttivo" runat="server" CssClass="CmbMetodoProduttivo" />
                                                    </ItemTemplate>
                                                    <ControlStyle Width="150px" />
                                                    <HeaderStyle Width="90px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Lotto Impianto" SortExpression="Distinta">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TxtDistinta" runat="server" />
                                                    </ItemTemplate>
                                                    <ControlStyle Width="150px" />
                                                    <HeaderStyle Width="90px" />
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="Veg_Des_Attuale" HeaderText="Specie Attuale" SortExpression="Veg_Des_Attuale"></asp:BoundField>
                                                <asp:BoundField DataField="Veg_Des" HeaderText="Specie Articolo" SortExpression="Veg_Des"></asp:BoundField>

                                                <asp:TemplateField HeaderText="Varietà" SortExpression="Varietà" meta:resourcekey="TemplateFieldResource4">
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="CmbVarieta" runat="server" CssClass="CmbVarieta" meta:resourcekey="CmbVarietaResource1" />
                                                    </ItemTemplate>
                                                    <ControlStyle Width="150px" />
                                                    <HeaderStyle Width="90px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Finalità" SortExpression="Finalità" meta:resourcekey="TemplateFieldResource5">
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="CmbFinalita" runat="server" CssClass="CmbFinalita" meta:resourcekey="CmbFinalitaResource1" />
                                                    </ItemTemplate>
                                                    <ControlStyle Width="150px" />
                                                    <HeaderStyle Width="90px" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Disciplinare" SortExpression="Disciplinare" meta:resourcekey="TemplateFieldResource7">
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="CmbDisciplinare" runat="server" CssClass="CmbDisciplinare"
                                                            meta:resourcekey="CmbDisciplinareResource1" />
                                                    </ItemTemplate>
                                                    <ControlStyle Width="150px" />
                                                    <HeaderStyle Width="90px" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Sup_App" HeaderText="Sup. App. [Ha]" SortExpression="Sup_App"
                                                    meta:resourcekey="BoundFieldResource14">
                                                    <ItemStyle CssClass="Sup_App" />
                                                </asp:BoundField>

                                                <asp:TemplateField HeaderText="Sup.Calcolata [Ha]">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TxtSup" runat="server" CssClass="TxtSupApp" onkeyup="TxtSupApp_KeyUp(this);" BackColor="pink" />
                                                    </ItemTemplate>
                                                    <ControlStyle Width="40px" />
                                                    <HeaderStyle Width="50px" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Dist. tra Fila [m]" SortExpression="DistTraFila" meta:resourcekey="TemplateFieldResource8">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TxtDistTraFila" runat="server" CssClass="TxtDistTraFila" meta:resourcekey="TxtDistTraFilaResource1" />
                                                    </ItemTemplate>
                                                    <ControlStyle Width="40px" />
                                                    <HeaderStyle Width="50px" CssClass="displaynone" />
                                                    <ItemStyle CssClass="displaynone" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Dist. su Fila [m]" SortExpression="DistSuFila" meta:resourcekey="TemplateFieldResource9">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TxtDistSuFila" runat="server" CssClass="TxtDistSuFila" meta:resourcekey="TxtDistSuFilaResource1" />
                                                    </ItemTemplate>
                                                    <ControlStyle Width="40px" />
                                                    <HeaderStyle Width="50px" CssClass="displaynone" />
                                                    <ItemStyle CssClass="displaynone" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Interb. [m]" SortExpression="Interbina" meta:resourcekey="TemplateFieldResource10">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TxtInterbina" runat="server" CssClass="TxtInterbina" ToolTip="Inserire la distanza di Interbina "
                                                            meta:resourcekey="TxtInterbinaResource1" />
                                                    </ItemTemplate>
                                                    <ControlStyle Width="40px" />
                                                    <HeaderStyle Width="50px" CssClass="displaynone" />
                                                    <ItemStyle CssClass="displaynone" />

                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Fila Binata" SortExpression="Fila_Binata" Visible="False"
                                                    meta:resourcekey="TemplateFieldResource11">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="ChkInterbina" Checked="True" runat="server" Enabled="False" CssClass="ChkInterbina"
                                                            meta:resourcekey="ChkInterbinaResource1" />
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="20px" />
                                                    <ItemStyle Width="20px" />
                                                    <FooterStyle Width="20px" />
                                                    <ControlStyle Width="20px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Germin. [%]" SortExpression="Germinabilita" meta:resourcekey="TemplateFieldResource12">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TxtGerminabilita" runat="server" CssClass="TxtGerminabilita" ToolTip="Inserire la germinabilità [%]"
                                                            meta:resourcekey="TxtGerminabilitaResource1" />
                                                    </ItemTemplate>
                                                    <ControlStyle Width="40px" />
                                                    <HeaderStyle Width="50px" CssClass="displaynone" />
                                                    <ItemStyle CssClass="displaynone" />
                                                </asp:TemplateField>


                                                <asp:TemplateField HeaderText="Densità (Piante/Ha o Semi/Ha)" SortExpression="p_ha">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="Txt_p_ha" runat="server" CssClass="Txtpha" onkeyup="Txtpha_KeyUp(this);" BackColor=" #C6E99C " />
                                                    </ItemTemplate>
                                                    <ControlStyle Width="40px" />
                                                </asp:TemplateField>


                                                <asp:BoundField DataField="Descrizione_Lotto" HeaderText="Descriz. Giacenza" SortExpression="Descrizione_Lotto"
                                                    meta:resourcekey="BoundFieldResource16" />
                                                <asp:TemplateField HeaderText="Unità di Misura" SortExpression="UnitaMisura" meta:resourcekey="TemplateFieldResource13">
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="CmbUdm" runat="server" CssClass="CmbUdm" meta:resourcekey="CmbUdmResource1" />
                                                    </ItemTemplate>
                                                    <ControlStyle Width="80px" />
                                                    <HeaderStyle Width="90px" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Udm_Sim_Lotto" HeaderText="Unità di Misura" SortExpression="Udm_Sim_Lotto"
                                                    meta:resourcekey="BoundFieldResource17"></asp:BoundField>
                                                <asp:TemplateField HeaderText="Qta / Imp." SortExpression="PianteImpianto" meta:resourcekey="TemplateFieldResource14">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TxtPianteImpianto" runat="server" CssClass="TxtPianteImpianto" Text='0' onkeyup="TxtPianteImpianto_KeyUp(this);" meta:resourcekey="TxtPianteImpiantoResource1" BackColor="#C6E99C" />
                                                    </ItemTemplate>
                                                    <ControlStyle Width="40px" />
                                                    <HeaderStyle Width="35px" />
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="Catasto" HeaderText="Catasto" SortExpression="Catasto" HtmlEncode="false"></asp:BoundField>

                                                <asp:BoundField DataField="Mat_Cod_Lotto" HeaderText="Mat_Cod_Lotto">
                                                    <ItemStyle CssClass="mat_cod_lotto displaynone" />
                                                    <HeaderStyle CssClass="displaynone" />
                                                </asp:BoundField>

                                                <asp:BoundField DataField="Lotto_Lotto" HeaderText="Lotto_Lotto">
                                                    <ItemStyle CssClass="lotto_lotto displaynone" />
                                                    <HeaderStyle CssClass="displaynone" />
                                                </asp:BoundField>

                                            </Columns>
                                            <HeaderStyle CssClass="ui-widget-header" />
                                            <PagerStyle CssClass="ui-widget-header" HorizontalAlign="Center" />
                                        </asp:GridView>

                                        <asp:Button ID="AggiornaGrigliaPlanningSemine" runat="server" Style="display: none;" />

                                        <asp:GridView ID="GridViewPlanning" runat="server" AutoGenerateColumns="False" CellPadding="5"
                                            CssClass="ui-widget-content" Caption="Selezionare da Pianificazione: <img src='../AB_Immagini/icone16/plus.png' alt='Aggiungi Colonne' id='btn_Impostazioni_ColonneSemine_Planning' style='margin-left:10px' />">
                                            <Columns>
                                                <asp:TemplateField meta:resourcekey="TemplateFieldResource3">
                                                    <HeaderTemplate>
                                                        <input type="checkbox" id="chkSelezionaTuttiImpPlanning" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="ChkSelezionaPlanning" runat="server" CssClass="ChkSelezionaImpPlanning" meta:resourcekey="ChkSelezionaResource2" />
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="20px" />
                                                    <ItemStyle Width="20px" />
                                                    <FooterStyle Width="20px" />
                                                    <ControlStyle Width="20px" />
                                                </asp:TemplateField>

                                                <%-- da planning operazione master --%>
                                                <asp:BoundField DataField="Operazione_Des" HeaderText="Operazione" />
                                                <asp:BoundField DataField="Operazione_Cod" HeaderText="Operazione_Cod">
                                                    <ItemStyle CssClass="displaynone" />
                                                    <HeaderStyle CssClass="displaynone" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Piva" HeaderText="Piva">
                                                    <ItemStyle CssClass="displaynone" />
                                                    <HeaderStyle CssClass="displaynone" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Sa_Cod" HeaderText="Sa_Cod">
                                                    <ItemStyle CssClass="displaynone" />
                                                    <HeaderStyle CssClass="displaynone" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Campo_Cod" HeaderText="Campo_Cod">
                                                    <ItemStyle CssClass="displaynone" />
                                                    <HeaderStyle CssClass="displaynone" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Appezza" HeaderText="Appezza">
                                                    <ItemStyle CssClass="displaynone" />
                                                    <HeaderStyle CssClass="displaynone" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Id_Reg" HeaderText="Id_Reg">
                                                    <ItemStyle CssClass="displaynone" />
                                                    <HeaderStyle CssClass="displaynone" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Progetto_Cod" HeaderText="Progetto_Cod">
                                                    <ItemStyle CssClass="displaynone" />
                                                    <HeaderStyle CssClass="displaynone" />
                                                </asp:BoundField>
                                                <%--<asp:BoundField DataField="Piva_SuperUser" HeaderText="Piva_SuperUser">
                                            <ItemStyle CssClass="displaynone" />
                                            <HeaderStyle CssClass="displaynone" />
                                        </asp:BoundField>--%>
                                                <asp:BoundField DataField="Programmazione_Entita_Cod" HeaderText="Programmazione_Entita_Cod">
                                                    <ItemStyle CssClass="displaynone" />
                                                    <HeaderStyle CssClass="displaynone" />
                                                </asp:BoundField>

                                                <asp:BoundField DataField="Sa_nome" HeaderText="Centro" />
                                                <asp:BoundField DataField="Campo_Des" HeaderText="Campo" />
                                                <asp:BoundField DataField="App_nome" HeaderText="App." />


                                                <%-- Da impianti in semina  --%>
                                                <asp:BoundField DataField="Cul_Des" HeaderText="Varieta'" />
                                                <asp:BoundField DataField="Cul_Des_Cliente" HeaderText="Varieta' AGEA" />


                                                <asp:BoundField DataField="Grfi_Des" HeaderText="Finalita'" />



                                                <%--<asp:TemplateField HeaderText="Regolamento" SortExpression="Regolamento" Visible="False"
                                            meta:resourcekey="TemplateFieldResource6">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="CmbRegolamento" runat="server" CssClass="CmbRegolamento" meta:resourcekey="CmbRegolamentoResource1" />
                                            </ItemTemplate>
                                            <ControlStyle Width="150px" />
                                            <HeaderStyle Width="90px" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Disciplinare" SortExpression="Disciplinare" meta:resourcekey="TemplateFieldResource7">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="CmbDisciplinare" runat="server" CssClass="CmbDisciplinare"
                                                    meta:resourcekey="CmbDisciplinareResource1" />
                                            </ItemTemplate>
                                            <ControlStyle Width="150px" />
                                            <HeaderStyle Width="90px" />
                                        </asp:TemplateField>
                                                --%>


                                                <asp:BoundField DataField="Sup_App" HeaderText="Sup. [Ha]" SortExpression="Sup_App"
                                                    meta:resourcekey="BoundFieldResource14">
                                                    <ItemStyle CssClass="Sup_App" />
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="Dist. tra Fila [m]" SortExpression="DistTraFila" meta:resourcekey="TemplateFieldResource8">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TxtDistTraFila" runat="server" CssClass="TxtDistTraFila" meta:resourcekey="TxtDistTraFilaResource1" />
                                                    </ItemTemplate>
                                                    <ControlStyle Width="40px" />
                                                    <ItemStyle CssClass="displaynone" />
                                                    <HeaderStyle CssClass="displaynone" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Dist. su Fila [m]" SortExpression="DistSuFila" meta:resourcekey="TemplateFieldResource9">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TxtDistSuFila" runat="server" CssClass="TxtDistSuFila" meta:resourcekey="TxtDistSuFilaResource1" />
                                                    </ItemTemplate>
                                                    <ControlStyle Width="40px" />
                                                    <ItemStyle CssClass="displaynone" />
                                                    <HeaderStyle CssClass="displaynone" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Interb. [m]" SortExpression="Interbina" meta:resourcekey="TemplateFieldResource10">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TxtInterbina" runat="server" CssClass="TxtInterbina" ToolTip="Inserire la distanza di Interbina "
                                                            meta:resourcekey="TxtInterbinaResource1" />
                                                    </ItemTemplate>
                                                    <ControlStyle Width="40px" />
                                                    <ItemStyle CssClass="displaynone" />
                                                    <HeaderStyle CssClass="displaynone" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Fila Binata" SortExpression="Fila_Binata" Visible="False"
                                                    meta:resourcekey="TemplateFieldResource11">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="ChkInterbina" Checked="True" runat="server" Enabled="False" CssClass="ChkInterbina"
                                                            meta:resourcekey="ChkInterbinaResource1" />
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="20px" />
                                                    <ItemStyle Width="20px" />
                                                    <FooterStyle Width="20px" />
                                                    <ControlStyle Width="20px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Germin. [%]" SortExpression="Germinabilita" meta:resourcekey="TemplateFieldResource12">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TxtGerminabilita" runat="server" CssClass="TxtGerminabilita" ToolTip="Inserire la germinabilità [%]"
                                                            meta:resourcekey="TxtGerminabilitaResource1" />
                                                    </ItemTemplate>
                                                    <ControlStyle Width="40px" />
                                                    <ItemStyle CssClass="displaynone" />
                                                    <HeaderStyle CssClass="displaynone" />
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="p_ha" HeaderText="P / Ha" SortExpression="p_ha" meta:resourcekey="BoundFieldResource15"></asp:BoundField>




                                                <asp:BoundField DataField="Veg_Cod" HeaderText="Veg_Cod">
                                                    <ItemStyle CssClass="displaynone" />
                                                    <HeaderStyle CssClass="displaynone" />
                                                </asp:BoundField>

                                                <asp:BoundField DataField="Cul_Cod" HeaderText="Cul_Cod">
                                                    <ItemStyle CssClass="displaynone" />
                                                    <HeaderStyle CssClass="displaynone" />
                                                </asp:BoundField>

                                                <asp:TemplateField HeaderText="Prodotto" SortExpression="Prodotto">
                                                    <ItemTemplate>
                                                        <cc1:ComboMateriePrime ID="CmbProdotto" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="Mat_Cod" HeaderText="Mat_Cod">
                                                    <ItemStyle CssClass="mat_cod_lotto displaynone" />
                                                    <HeaderStyle CssClass="displaynone" />
                                                </asp:BoundField>

                                                <asp:BoundField DataField="ProgettoCod" HeaderText="ProgettoCod">
                                                    <ItemStyle CssClass="displaynone" />
                                                    <HeaderStyle CssClass="displaynone" />
                                                </asp:BoundField>

                                                <asp:TemplateField HeaderText="Unità di Misura" SortExpression="UnitaMisura" meta:resourcekey="TemplateFieldResource13">
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="CmbUdm" runat="server" CssClass="CmbUdm" meta:resourcekey="CmbUdmResource1" />
                                                    </ItemTemplate>
                                                    <ControlStyle Width="80px" />
                                                    <HeaderStyle Width="90px" />
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="Udm_Sim_Lotto" HeaderText="Unità di Misura" SortExpression="Udm_Sim_Lotto"
                                                    meta:resourcekey="BoundFieldResource17"></asp:BoundField>

                                                <asp:TemplateField HeaderText="Qta / Imp." SortExpression="PianteImpianto" meta:resourcekey="TemplateFieldResource14">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TxtPianteImpianto" runat="server" CssClass="TxtPianteImpianto" Text='0'
                                                            meta:resourcekey="TxtPianteImpiantoResource1" />
                                                    </ItemTemplate>
                                                    <ControlStyle Width="40px" />
                                                    <HeaderStyle Width="35px" />
                                                </asp:TemplateField>

                                                              <asp:BoundField DataField="Lotto_Lotto" HeaderText="Lotto_Lotto">
                                                    <ItemStyle CssClass="lotto_lotto displaynone" />
                                                    <HeaderStyle CssClass="displaynone" />
                                                </asp:BoundField>

                                            </Columns>
                                            <HeaderStyle CssClass="ui-widget-header" />
                                            <PagerStyle CssClass="ui-widget-header" HorizontalAlign="Center" />
                                        </asp:GridView>

                                    </div>

                                </div>

                            </div>
                            
                        </div>

                    </div>
                  
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="SalvaImpostazioniColonneSemine" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="AggiornaGrigliaImpiantiSemine" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script type="text/javascript">
        function SalvaImpostazioniColonneSemine() {
            $('#<%=SalvaImpostazioniColonneSemine.ClientID %>').click();
        };



    </script>
    <asp:UpdatePanel ID="UpdatePanelImpostazioniColonneSemine" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Button ID="AggiornaImpostazioniColonneSemine" runat="server" Style="display: none;"
                meta:resourcekey="AggiornaImpostazioniColonneSemineResource1" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <!--Dialog per l'eliminazione -->
    <div id="dialogImpostazioniColonneSemine" title="Visualizzazione ColonneSemine">
        <asp:UpdatePanel ID="UpdatePanelVisualizzazioneColonneSemine" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:UpdateProgress ID="UpdateProgressVisualizzazioneColonneSemine" runat="server"
                    DisplayAfter="50" AssociatedUpdatePanelID="UpdatePanelVisualizzazioneColonneSemine">
                    <ProgressTemplate>
                        <div class="LoadPanel">
                            <div class="loading-indicator-bars">
                            </div>
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <div>
                    <!--- Bottoni Salvattaggio e Annulla -->
                    <asp:Button ID="SalvaImpostazioniColonneSemine" runat="server" Style="display: none;"
                        meta:resourcekey="SalvaImpostazioniColonneSemineResource1" />
                    <asp:CheckBoxList ID="ListaColonneSemineVisibili" runat="server" meta:resourcekey="ListaColonneSemineVisibiliResource1">
                    </asp:CheckBoxList>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="SalvaImpostazioniColonneSemine" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>
