function changeChk(e) {
    let nameServerElem = e.sender.element.data("name_server_elem");
    let serverElem = $("#" + nameServerElem);
    serverElem.attr("checked", e.checked);
}
function caricaMagazzini() {
    //var options = Leggi_Magazzini();
    //var opt;
    //let str = "["
    //for (var i = 0, iLen = options.length; i < iLen; i++) {
    //    opt = options[i];
    //    var temp = '{"text": "' + opt.text + '", ' + '"value": "' + opt.value + '"},'
    //    str = str + temp
    //}
    //str = str.slice(0, -1) + "]"
    //console.log(str)
    //magazzini = JSON.parse(str);
    $("#ddlMagazzino").kendoDropDownList({
        filter: "contains",
        autoBind: true,
        dataTextField: "Sa_Nome",
        dataValueField: "Sa_Cod",
        dataSource: { transport: { read: Leggi_Magazzini } },
        open: kendoDropDownAdjustWidth
    }).data("kendoDropDownList");
}
function attivaDisattivaSezioniDaStampare(status) {

    for (var i = 0; i < leftSwitchesControls.length; i++) {
        if (leftSwitchesControls[i].options.enabled)
            leftSwitchesControls[i].check(status);
    }
}
function mostraData(status) {
    if (status == true) {
        $("#Data").show();
        $("#Data_Inizio").hide();
        $("#Data_Fine").hide();
    }
    if (status == false) {
        $("#Data").hide();
        $("#Data_Inizio").show();
        $("#Data_Fine").show();
    }
}
function stampaMagazzinoFalse() {
    stampaMagazzinoNew(false);
}
function stampaMagazzinoTrue() {
    stampaMagazzinoNew(true);
}
function stampaMagazzinoNew(SchedaFertilizzanti) {
    //CONTROLLA SELEZIONE
    isDdlSelected = Get_KendoDDLValue('ddlMagazzino');
    if (isDdlSelected == '-1') {
        kendo.alert("Selezionare un magazzino");
        return;
    }
    //RACCOGLIE I SETTAGGI UTENTE 
    var isStampaVeneto = false;
    if ($(cIdReportSelezionato).val() == enum_CodificaStampe.RegistroTrattamenti_Veneto)
        isStampaVeneto = true;

    var parametriServer = Prepara_Oggetto_Parametri_Stampa(false, SchedaFertilizzanti);
    if (parametriServer === null)
        return;

    //MANDA AL CONTROLLO PRE STAMPA
    Controlli_PreStampa_New(parametriServer, isStampaVeneto, true);

}
function stampaNewFalse() {
    stampaNew(false);
}
function stampaNewTrue() {
    stampaNew(true);
}
function stampaNew(StampaProva) {

    //RACCOGLIE I SETTAGGI UTENTE 
    var isStampaVeneto = false;
    if ($(cIdReportSelezionato).val() == enum_CodificaStampe.RegistroTrattamenti_Veneto)
        isStampaVeneto = true;

    var parametriServer = Prepara_Oggetto_Parametri_Stampa(StampaProva, false);
    if (parametriServer === null)
        return;
    //MANDA AL CONTROLLO PRE STAMPA
    Controlli_PreStampa_New(parametriServer, isStampaVeneto, false);
}
function Prepara_Oggetto_Parametri_Stampa(StampaProva, SchedaFertilizzanti) {
    var parametri = new Object();

    //SWITCHES SELEZIONE STAMPA (sezioni):
    parametri.check_frontespizio = getKendoSwitch("check_frontespizio");
    parametri.check_frontespizio_value = $("#check_frontespizio").attr("Value");

    parametri.check_personale = getKendoSwitch("check_personale");
    parametri.check_personale_value = $("#check_personale").attr("Value");

    parametri.check_dati_catastali = getKendoSwitch("check_dati_catastali");
    parametri.check_dati_catastali_value = $("#check_dati_catastali").attr("Value");

    parametri.check_semine = getKendoSwitch("check_semine");
    parametri.check_semine_value = $("#check_semine").attr("Value");

    parametri.check_Fertilizzazioni = getKendoSwitch("check_Fertilizzazioni");
    parametri.check_Fertilizzazioni_value = $("#check_Fertilizzazioni").attr("Value");

    parametri.check_trattamenti = getKendoSwitch("check_trattamenti");
    parametri.check_trattamenti_value = $("#check_trattamenti").attr("Value");

    parametri.check_fitoregolatori = getKendoSwitch("check_fitoregolatori");
    parametri.check_fitoregolatori_value = $("#check_fitoregolatori").attr("Value");

    parametri.check_fasi_fenologiche = getKendoSwitch("check_fasi_fenologiche");
    parametri.check_fasi_fenologiche_value = $("#check_fasi_fenologiche").attr("Value");

    parametri.check_trappole = getKendoSwitch("check_trappole");
    parametri.check_trappole_value = $("#check_trappole").attr("Value");

    parametri.check_ril_avver_trappole = getKendoSwitch("check_ril_avver_trappole");
    parametri.check_ril_avver_trappole_value = $("#check_ril_avver_trappole").attr("Value");

    parametri.check_ril_avver_campo = getKendoSwitch("check_ril_avver_campo");
    parametri.check_ril_avver_campo_value = $("#check_ril_avver_campo").attr("Value");

    parametri.check_irrigazione = getKendoSwitch("check_irrigazione");
    parametri.check_irrigazione_value = $("#check_irrigazione").attr("Value");

    parametri.check_operazioni_colturali = getKendoSwitch("check_operazioni_colturali");
    parametri.check_operazioni_colturali_value = $("#check_operazioni_colturali").attr("Value");

    parametri.check_ind_maturita = getKendoSwitch("check_ind_maturita");
    parametri.check_ind_maturita_value = $("#check_ind_maturita").attr("Value");

    parametri.check_rilievo_prod = getKendoSwitch("check_rilievo_prod");
    parametri.check_rilievo_prod_value = $("#check_rilievo_prod").attr("Value");

    parametri.check_piogge = getKendoSwitch("check_piogge");
    parametri.check_piogge_value = $("#check_piogge").attr("Value");

    parametri.check_informazioni = getKendoSwitch("check_informazioni");
    parametri.check_informazioni_value = $("#check_informazioni").attr("Value");

    parametri.check_manutenzione = getKendoSwitch("check_manutenzione");
    parametri.check_manutenzione_value = $("#check_manutenzione").attr("Value");

    parametri.check_visite_ispettive = getKendoSwitch("check_visite_ispettive");
    parametri.check_visite_ispettive_value = $("#check_visite_ispettive").attr("Value");

    parametri.check_trattamenti_post_raccolta = getKendoSwitch("check_trattamenti_post_raccolta");
    parametri.check_trattamenti_post_raccolta_value = $("#check_trattamenti_post_raccolta").attr("Value");

    parametri.check_verifiche_conf = getKendoSwitch("check_verifiche_conf");
    parametri.checkAvversitaQta = getKendoSwitch("chkAvversitaQta");
    parametri.checkTutteRaccolte = getKendoSwitch("chkTutteRaccolte");
    parametri.checkQtaQtaRaccolte = getKendoSwitch("chkQtaQtaRaccolte");
    parametri.checkDataUltimaRaccolta = getKendoSwitch("chkDataUltimaRaccolta");
    parametri.checkDataUltimaRaccolta = getKendoSwitch("chkDataUltimaRaccolta");
    parametri.checkLogoRegione = getKendoSwitch("checkLogoRegione");

    //SWITCHES OPZIONI STAMPA:
    if ($(cId_GlobalGapDiv).val() == "visible") {
        parametri.checkGlobalGap = getKendoSwitch("check_globalgap");
        if (!kendo.parseInt($("#txt_tempo_rientro").val())) {
            kendo.alert("Inerire valore corretto in Tempo Rientro");
            return null;
        }
        parametri.tempoRientro = $("#txt_tempo_rientro").val();
    }
    parametri.checkPrioritaColturePrecedenti = getKendoSwitch("chkPrioritaColturePrecedenti");
    parametri.checkVisualizzaTipologieVarietali = getKendoSwitch("chkVisualizzaTipologieVarietali");
    parametri.checkVisualizzaCapitolatoPrivato = getKendoSwitch("chkVisualizzaCapitolatoPrivato");
    parametri.checkVisualizzaFinalita = getKendoSwitch("chkVisualizzaFinalita");
    parametri.checkRaggruppaXCampo = getKendoSwitch("chkRaggruppaXCampo");
    parametri.checkVisualizzaAcquaHa = getKendoSwitch("chkVisualizzaAcquaHa");
    parametri.checkStampaAnnoImpiantoPluriennali = getKendoSwitch("chkStampaAnnoImpiantoPluriennali");
    parametri.checkMostraValoriSignificativiNeiRilievi = getKendoSwitch("chkMostraValoriSignificativiNeiRilievi");
    parametri.checkSuperfici = getKendoSwitch("checkSuperfici");
    parametri.checkData = getKendoSwitch("checkData");
    parametri.checkStampaProva = StampaProva;
    parametri.checkLogoRegione = getKendoSwitch("checkLogoRegione");
    parametri.checkStampeMagazzino = SchedaFertilizzanti;
    parametri.reportSelezionato = $(cIdReportSelezionato).val();

    parametri.checkMostraFirmaODC = getKendoSwitch("checkStampaODC");
    parametri.checkMostraDataDiStampa = getKendoSwitch("checkMostraDataStampa");
    //parametri.checkVisualizzaLotto = getKendoSwitch("checkVisualizzaLotto") == null ? false : getKendoSwitch("checkVisualizzaLotto");

    //ALTRI CONTROLLI:
    parametri.ddlRegioni = Get_KendoDDLValue("ddlRegioni");
    parametri.ddlArrotondamento = Get_KendoDDLValue("ddlArrotondamento");
    parametri.ddlMagazzino = Get_KendoDDLValue("ddlMagazzino");

    //MAGAZZINI
    parametri.piva = $(cIds_piva).val();


    //IMPIANTI FILTRATI
    parametri.ddlCentroAziendale = Get_KendoDDLValue("ddlCentroAziendale");
    parametri.ddlSpecieVegetale = Get_KendoDDLValue("ddlSpecieVegetale");
    parametri.UtilizzaImpiantiFiltrati = $(cId_VisualizzaImpiantiFiltrati).val();

    // SEZIONI VUOTE
    parametri.ElencoSezioniVuote = elencoSezioniVuote;

    // Parametri per stampa Veneto
    parametri.schedaVeneto = "";
    if ($(cIdReportSelezionato).val() == enum_CodificaStampe.RegistroTrattamenti_Veneto) {
        parametri.schedaVeneto = Get_KendoDDLValue("ddlSchede");
        if (parametri.schedaVeneto == "") {
            kendo.alert("Selezionare una scheda");
            return null;
        }
    }
    if (parametri.checkData == true) {
        parametri.txtValiditaInizio = KendoDate("txtValiditaInizio").value();
        parametri.TxtValiditaFine = KendoDate("txtValiditaFine").value();
        //controllo date
        var diff = Math.round(parametri.txtValiditaInizio - parametri.TxtValiditaFine);
        if (diff == 0) {
            kendo.alert('date incongruenti'); //stessa data
            return null;
        }
        if (diff > 0) {
            kendo.alert('date incongruenti'); //la data iniziale è temporalmente maggiore della data finale
            return null;
        }
    }
    else
        parametri.txtStampaGiorno = KendoDate("txtStampaGiorno").value();

    var parametriEscaped = kendoEscapeOggetto(parametri);
    var parametriServer = " { objParams: '" + parametriEscaped + "' }";

    return parametriServer;
}
function SelezionaDeselezionaFito() {

    var switchTrattamenti = $("#check_trattamenti").data("kendoSwitch");
    var switchFitoregolatori = $("#check_fitoregolatori").data("kendoSwitch");
    var checked = switchTrattamenti.check();
    if (!checked) {
        switchFitoregolatori.check(false);
        switchFitoregolatori.enable(false);
    }
    else {
        switchFitoregolatori.check(true);
        switchFitoregolatori.enable(true);
    }

}
function SelezionaTutte(report) {
    var currentReport = parseInt(report);
    switch (currentReport) {

        //case enum_CodificaStampe.Registro_Trattamenti_Massivo:
        //case enum_CodificaStampe.Registro_Fertilizzazioni_Massivo:
        //case enum_CodificaStampe.Registro_Fertilizzazioni:
        //case enum_CodificaStampe.RegistroTrattamenti_Semplificata:
        //case enum_CodificaStampe.RegistroTrattamenti_Veneto:
        //case enum_CodificaStampe.RegistroTrattamentiVeneto_StdCondizionalita:
        //case enum_CodificaStampe.SchedaCampagna_Multi_Lombardia:
        //case enum_CodificaStampe.RegistroAziendaleUnico:
        //    {
        //        $("#divCentroAziendale").hide();
        //        $("divSpecieVegetale").hide();
        //    }
        //    break;
        case enum_CodificaStampe.SchedaInterventiAgronomici:

            for (let i = 0; i < leftSwitchesControls.length - 1; i++) {
                if (leftSwitchesControls[i].element[0].value == ElencoReportChiave.FRONTESPIZIO ||
                    leftSwitchesControls[i].element[0].value == ElencoReportChiave.FERTILIZZAZIONI ||
                    leftSwitchesControls[i].element[0].value == ElencoReportChiave.TRATTAMENTI ||
                    leftSwitchesControls[i].element[0].value == ElencoReportChiave.RIL_AVVERS_NELLE_TRAPP ||
                    leftSwitchesControls[i].element[0].value == ElencoReportChiave.RIL_AVVERS_IN_CAMPO ||
                    leftSwitchesControls[i].element[0].value == ElencoReportChiave.IRRIGAZIONE ||
                    leftSwitchesControls[i].element[0].value == ElencoReportChiave.ALTRE_OPER_COLTURALI ||
                    leftSwitchesControls[i].element[0].value == ElencoReportChiave.RIL_PROD_E_DATA_RACC ||
                    leftSwitchesControls[i].element[0].value == ElencoReportChiave.PIOGGE ||
                    leftSwitchesControls[i].element[0].value == ElencoReportChiave.VISITE_ISPETTIVE)
                    leftSwitchesControls[i].check(true);
            }
            break;

        case enum_CodificaStampe.SchedaCampagna_2078_Semplificata:
        case enum_CodificaStampe.SchedaRegistrazione_Semplificata:
        case enum_CodificaStampe.SchedaCampagna_Multicentro: {
            for (let i = 0; i < leftSwitchesControls.length - 1; i++) {
                if (leftSwitchesControls[i].options.enabled)
                    leftSwitchesControls[i].check(true);
                //deseleziono la sezione fasi fenologiche e patentino
                if (leftSwitchesControls[i].element[0].value == ElencoReportChiave.FASI_FENOLOGICHE ||
                    leftSwitchesControls[i].element[0].value == ElencoReportChiave.PERSONALE_CON_PATENTINO)
                    leftSwitchesControls[i].check(false);
                //deseleziono tratt post raccolta
                if (leftSwitchesControls[i].element[0].value == ElencoReportChiave.TRATTAMENTI_POSTRACCOLTA)
                    leftSwitchesControls[i].check(false);
            }
        }
            break;

        case enum_CodificaStampe.RegistroTrattamenti_Semplificata:
        case enum_CodificaStampe.Registro_Trattamenti_Massivo:
            for (let i = 0; i < leftSwitchesControls.length - 1; i++) {
                if (leftSwitchesControls[i].element[0].value == ElencoReportChiave.FRONTESPIZIO ||
                    leftSwitchesControls[i].element[0].value == ElencoReportChiave.PERSONALE_CON_PATENTINO ||
                    leftSwitchesControls[i].element[0].value == ElencoReportChiave.DATI_CATASTALI ||
                    leftSwitchesControls[i].element[0].value == ElencoReportChiave.TRATTAMENTI ||
                    leftSwitchesControls[i].element[0].value == ElencoReportChiave.INFORMAZ_E_DICHIAR)
                    leftSwitchesControls[i].check(true);
            }
            break;

        case enum_CodificaStampe.RegistroTrattamentiVeneto_StdCondizionalita:
            for (let i = 0; i < leftSwitchesControls.length - 1; i++) {
                if (leftSwitchesControls[i].element[0].value == ElencoReportChiave.FRONTESPIZIO ||
                    leftSwitchesControls[i].element[0].value == ElencoReportChiave.MANUTEN_MACCHINARI ||
                    leftSwitchesControls[i].element[0].value == ElencoReportChiave.PERSONALE_CON_PATENTINO ||
                    leftSwitchesControls[i].element[0].value == ElencoReportChiave.TRATTAMENTI ||
                    leftSwitchesControls[i].element[0].value == ElencoReportChiave.TRATTAMENTI_POSTRACCOLTA)
                    leftSwitchesControls[i].check(true);
            }
            break;

        case enum_CodificaStampe.SchedaCampagna_Multi_Lombardia:
            for (let i = 0; i < leftSwitchesControls.length - 1; i++) {
                if (leftSwitchesControls[i].element[0].value == ElencoReportChiave.FRONTESPIZIO ||
                    leftSwitchesControls[i].element[0].value == ElencoReportChiave.DATI_CATASTALI ||
                    leftSwitchesControls[i].element[0].value == ElencoReportChiave.FERTILIZZAZIONI ||
                    leftSwitchesControls[i].element[0].value == ElencoReportChiave.TRATTAMENTI ||
                    leftSwitchesControls[i].element[0].value == ElencoReportChiave.IRRIGAZIONE ||
                    leftSwitchesControls[i].element[0].value == ElencoReportChiave.ALTRE_OPER_COLTURALI ||
                    leftSwitchesControls[i].element[0].value == ElencoReportChiave.PIOGGE)
                    leftSwitchesControls[i].check(true);
            }
            break;
        case enum_CodificaStampe.RegistroAziendaleUnico:
            for (let i = 0; i < leftSwitchesControls.length - 1; i++) {
                if (leftSwitchesControls[i].options.enabled)
                    leftSwitchesControls[i].check(true);
            }
            break;

        case enum_CodificaStampe.Registro_Fertilizzazioni:
        case enum_CodificaStampe.Registro_Fertilizzazioni_Massivo:
            for (let i = 0; i < leftSwitchesControls.length - 1; i++) {
                if (leftSwitchesControls[i].element[0].value == ElencoReportChiave.FRONTESPIZIO ||
                    leftSwitchesControls[i].element[0].value == ElencoReportChiave.DATI_CATASTALI ||
                    leftSwitchesControls[i].element[0].value == ElencoReportChiave.FERTILIZZAZIONI ||
                    leftSwitchesControls[i].element[0].value == ElencoReportChiave.INFORMAZ_E_DICHIAR)
                    leftSwitchesControls[i].check(true);
            }
            break;

        case enum_CodificaStampe.RegistroTrattamenti_Veneto:
            for (let i = 0; i < leftSwitchesControls.length - 1; i++) {
                if (leftSwitchesControls[i].element[0].value == ElencoReportChiave.FRONTESPIZIO)
                    leftSwitchesControls[i].check(true);
            }
            break;


        case enum_CodificaStampe.SchedaCampagna_ConserveItalia:
        case enum_CodificaStampe.SchedaColturale_Biologico:
            for (let i = 0; i < leftSwitchesControls.length - 1; i++) {
                if (leftSwitchesControls[i].element[0].value == ElencoReportChiave.PERSONALE_CON_PATENTINO)
                    leftSwitchesControls[i].check(false);
                else
                    leftSwitchesControls[i].check(true);

                if (leftSwitchesControls[i].element[0].value == ElencoReportChiave.TRATTAMENTI_POSTRACCOLTA)
                    leftSwitchesControls[i].check(false);

                if (currentReport == enum_CodificaStampe.SchedaCampagna_ConserveItalia && leftSwitchesControls[i].element[0].value == ElencoReportChiave.INFORMAZ_E_DICHIAR)
                    leftSwitchesControls[i].check(false);
                if (currentReport == enum_CodificaStampe.SchedaCampagna_ConserveItalia && leftSwitchesControls[i].element[0].value == ElencoReportChiave.VISITE_ISPETTIVE)
                    leftSwitchesControls[i].check(false);
            }
            break;

        case enum_CodificaStampe.Eurep_Gap_Semplificata:
        case enum_CodificaStampe.SchedaCampagna_Pizzoli:
        case enum_CodificaStampe.Eurep_Gap_Multicentro:
            for (let i = 0; i < leftSwitchesControls.length - 1; i++) {
                if (leftSwitchesControls[i].element[0].value == ElencoReportChiave.INFORMAZ_E_DICHIAR ||
                    leftSwitchesControls[i].element[0].value == ElencoReportChiave.MANUTEN_MACCHINARI)
                    leftSwitchesControls[i].check(false);
                else
                    leftSwitchesControls[i].check(true);

                ////deseleziono tratt post raccolta
                if (leftSwitchesControls[i].element[0].value == ElencoReportChiave.TRATTAMENTI_POSTRACCOLTA)
                    leftSwitchesControls[i].check(false);
            }
            break;

        default:
            {
                for (let i = 0; i < leftSwitchesControls.length - 1; i++) {
                    if (leftSwitchesControls[i].options.enabled)
                        leftSwitchesControls[i].check(true);
                }
            }
    }

    for (let i = 0; i < leftSwitchesControls.length - 1; i++) {
        if (leftSwitchesControls[i].element[0].value == ElencoReportChiave.VERIFICHE_CONFORMITA)
            leftSwitchesControls[i].check(false);
    }

    if (currentReport != enum_CodificaStampe.RegistroTrattamenti_Veneto) {
        var check = false;
        for (let i = 0; i < leftSwitchesControls.length - 1; i++) {
            if (leftSwitchesControls[i].element[0].value == ElencoReportChiave.TRATTAMENTI) {
                var checked = leftSwitchesControls[i].check();
            }
            if (checked)
                if (leftSwitchesControls[i].element[0].value == ElencoReportChiave.FITOFARMACI)
                    leftSwitchesControls[i].check(true);
        }
    }
    if (currentReport == enum_CodificaStampe.SchedaCampagna_Multicentro ||
        currentReport == enum_CodificaStampe.RegistroAziendaleUnico ||
        currentReport == enum_CodificaStampe.Eurep_Gap_Multicentro) {
        for (let i = 0; i < centralSwitchesControls.length - 1; i++) {
            if (centralSwitchesControls[i].element[0].name == "chkAvversitaQta") {
                setKendoSwitchVisible(centralSwitchesControls[i].element[0].name, true);
                document.getElementById("lblAvversitaQta").style.display = 'inline';
            }
        }
    }
}
function VisualizzaSezionixRegTrattVeneto() {
    document.getElementById("sezioniNonVeneto").style.display = "none";
    document.getElementById("sezioniVeneto").style.display = "inline";
}
function Inizializza_Ddl_Centri_Aziendali() {
    $("#ddlCentroAziendale").kendoDropDownList({
        filter: "contains",
        autoBind: true,
        dataTextField: "Sa_Nome",
        dataValueField: "Sa_Cod",
        dataSource: { transport: { read: Leggi_Centri_Aziendali } },
        open: kendoDropDownAdjustWidth
    }).data("kendoDropDownList");

}
function Inizializza_Ddl_Specie_Vegetali() {
    $("#ddlSpecieVegetale").kendoDropDownList({
        filter: "contains",
        autoBind: true,
        dataTextField: "Sv_Nome",
        dataValueField: "Sv_Cod",
        dataSource: { transport: { read: Leggi_Specie_Vegetali } },
        open: kendoDropDownAdjustWidth
    }).data("kendoDropDownList");
}
function aggiungiSezioneVuote() {
    let str = ""
    for (let i = 0; i < sezioniVuote.length; i++) {
        var checked = sezioniVuote[i].check();
        if (checked) {
            str += sezioniVuote[i].element[0].value + '|';
        }
    }
    elencoSezioniVuote = str.slice(0, -1);
    $("#SezioniVuoteWindow").data("kendoWindow").close();

}
function annullaSezioniVuote() {
    for (let i = 0; i < sezioniVuote.length; i++) {
        var checked = sezioniVuote[i].check();
        if (checked)
            sezioniVuote[i].check(false);
    }
    if (elencoSezioniVuote != "")
        elencoSezioniVuote = "";
    $("#SezioniVuoteWindow").data("kendoWindow").close();
}
function DatiGlobalGap() {
    var dati = document.getElementById('txt_revisione').value;
    if (dati != "")
        SalvaDatiGlobalGap(dati);
}
function ChiudiGlobalGap() {
    $("#globalGapWindow").data("kendoWindow").close();
}
function ChiudiColture() {
    $("#tabellaRotazione").data("kendoWindow").close();
}
function DatiColture() {
    //inserire il salvataggio

    $("#tabellaRotazione").data("kendoWindow").close();
}
//utility
function nascondiVecchiControlli(status) {

    //controlli    
    var elencoReportWindow = document.getElementById("hide");

    if (status == true) {

        elencoReportWindow.style.display = 'none';

    }
    if (status == false) {

        elencoReportWindow.style.display = 'inline';
    }
}

function kReadValorizzazioneGrid_rows(options) {

    let data = $('#hdKendoTabellaRotazioneValore').val();
    let jSonParsed_Kendo = JSON.parse(data);
    options.success(jSonParsed_Kendo.kendo_rows);
}
function kReadValorizzazioneGridElRe_rows(options) {

    let data = $('#hdKendoTabellaElencoReport').val();
    let jSonParsed_Kendo = JSON.parse(data);
    options.success(jSonParsed_Kendo.kendo_rows);
}
function onEditNumeratoriTipi(e) {

    e.container.find("input[name='Coltura1']").attr('maxlength', '10');
    e.container.find("input[name='Coltura2']").attr('maxlength', '10');
    e.container.find("input[name='Coltura3']").attr('maxlength', '10');
    e.container.find("input[name='Coltura4']").attr('maxlength', '10');



}
function SubmitCulturePrecedenti(options) {
    var grid = $("#divKendoRotazione").data("kendoGrid");
    var currentData = grid.dataSource.data();

   
    

    // Non ci sono errori, procedo con aggiornamenti
    var updatedRecords = [];
    var newRecords = [];
    var deletedRecords = [];

    // modificate / inserite
    for (let i = 0; i < currentData.length; i++) {

        if (currentData[i].isNew()) {
            newRecords.push(currentData[i].toJSON());
        } else if (currentData[i].dirty) {
            updatedRecords.push(currentData[i].toJSON());
        }
    }

    // cancellate
    for (let i = 0; i < grid.dataSource._destroyed.length; i++) {

        deletedRecords.push(grid.dataSource._destroyed[i].toJSON());
    }

    if (newRecords.length > 0 || updatedRecords.length > 0 || deletedRecords.length > 0) {

        var righeInserite = kendoEscapeOggetto(newRecords);
        var righeModificate = kendoEscapeOggetto(updatedRecords);
        var righeCancellate = kendoEscapeOggetto(deletedRecords);

        var allOk = false;

        var objParametri = new Object();
        objParametri.RigheInserite = righeInserite;
        objParametri.RigheModificate = righeModificate;
        objParametri.RigheCancellate = righeCancellate;

        var paramEscaped = kendoEscapeOggetto(objParametri);
        var param = "{paramString: '" + paramEscaped + "'}";

      
        ajaxAgronicaSync(indirizzohttp + "/SalvaColturePrecedenti",
            param, false,
            function (risposta) {
                allOk = true;
                //grid.dataSource._destroyed = [];
                //grid.dataSource.read();
                //grid.refresh();
                Leggi_ColturePrecedenti();
                MessaggioTuttoOK_Bootstrap("Salvataggio effettuato correttamente", "DIV_Messaggipopup");
            }, function (risposta) {
                var errori = risposta.RispostaStringa + ' ' + risposta.Errore;
                MessaggioErrore_Bootstrap(errori, "DIV_Messaggi");
            });

        if (!allOk) {
            erroreSubmitGriglia(grid);
        }
        
        
    }
}
function Cultura_DropDownEditor(container) {
    if (ElencoCulture == null || ElencoCulture == undefined)
        ElencoCulture = CaricaComboSpecieVegetaliDestinazioneUso();

    creaDropDownEditor(container, "Coltura1", "CodColtura1", ElencoCulture, changeCulture);

}
function Cultura_DropDownEditor2(container) {
    if (ElencoCulture == null || ElencoCulture == undefined)
        ElencoCulture = CaricaComboSpecieVegetaliDestinazioneUso();

    let ElendoCodici = [];
    for (var x = 0; x < ElencoCulture.length; x++) {
        ElendoCodici.push({ "CodColtura2": ElencoCulture[x]["CodColtura1"], "Coltura2": ElencoCulture[x]["Coltura1"] });
    }

    creaDropDownEditor(container, "Coltura2", "CodColtura2", ElendoCodici, changeCulture2);

}
function Cultura_DropDownEditor3(container) {
    if (ElencoCulture == null || ElencoCulture == undefined)
        ElencoCulture = CaricaComboSpecieVegetaliDestinazioneUso();

    let ElendoCodici = [];
    for (var x = 0; x < ElencoCulture.length; x++) {
        ElendoCodici.push({ "CodColtura3": ElencoCulture[x]["CodColtura1"], "Coltura3": ElencoCulture[x]["Coltura1"] });
    }

    creaDropDownEditor(container, "Coltura3", "CodColtura3", ElendoCodici, changeCulture3);

}
function Cultura_DropDownEditor4(container) {
    if (ElencoCulture == null || ElencoCulture == undefined)
        ElencoCulture = CaricaComboSpecieVegetaliDestinazioneUso();

    let ElendoCodici = [];
    for (var x = 0; x < ElencoCulture.length; x++) {
        ElendoCodici.push({ "CodColtura4": ElencoCulture[x]["CodColtura1"], "Coltura4": ElencoCulture[x]["Coltura1"] });
    }

    creaDropDownEditor(container, "Coltura4", "CodColtura4", ElendoCodici, changeCulture4);

}


function changeCulture(e) {

    var dataItem = e.sender.dataItem();
    var grid = $("#divKendoRotazione").data("kendoGrid");
    var model = grid.dataItem(this.element.closest("tr"));
    model.CodColtura1 = dataItem.CodColtura1;
    model.Coltura1 = dataItem.Coltura1;
    model.dirty = true;
}
function changeCulture2(e) {

    var dataItem = e.sender.dataItem();
    var grid = $("#divKendoRotazione").data("kendoGrid");
    var model = grid.dataItem(this.element.closest("tr"));
    model.CodColtura2 = dataItem.CodColtura2;
    model.Coltura2 = dataItem.Coltura2;
    model.dirty = true;
}
function changeCulture3(e) {

    var dataItem = e.sender.dataItem();
    var grid = $("#divKendoRotazione").data("kendoGrid");
    var model = grid.dataItem(this.element.closest("tr"));
    model.CodColtura3 = dataItem.CodColtura3;
    model.Coltura3 = dataItem.Coltura3;
    model.dirty = true;
}
function changeCulture4(e) {

    var dataItem = e.sender.dataItem();
    var grid = $("#divKendoRotazione").data("kendoGrid");
    var model = grid.dataItem(this.element.closest("tr"));
    model.CodColtura4 = dataItem.CodColtura4;
    model.Coltura4 = dataItem.Coltura4;
    model.dirty = true;
}




function kendoCulture_inizializza(divKendo, keys) {

    let jSonParsed_Kendo = JSON.parse(keys);
    var campiKendoModel = jSonParsed_Kendo.kendo_model;

    //let columns = jSonParsed_Kendo.kendo_columns;
    var funzioniCRUD = { funzioneRead: kReadValorizzazioneGrid_rows };

    if (ElencoCulture == null || ElencoCulture == undefined) {
        ElencoCulture = CaricaComboSpecieVegetaliDestinazioneUso();
        ElencoCulture.push({ "CodColtura1": "", "Coltura1": ""});
        for (var RO = 0; RO < jSonParsed_Kendo.kendo_rows.length; RO++) {
            var TrovatoElemento = false;
            for (var x = 0; x < ElencoCulture.length; x++) {
                if (ElencoCulture[x]["Coltura1"] === jSonParsed_Kendo.kendo_rows[RO]["Coltura1"]) {
                    TrovatoElemento = true;
                }
            }
            if (!TrovatoElemento) {
                ElencoCulture.push({ "CodColtura1": jSonParsed_Kendo.kendo_rows[RO]["Coltura1"], "Coltura1": jSonParsed_Kendo.kendo_rows[RO]["Coltura1"] });
            }
            TrovatoElemento = false;
            for (var x = 0; x < ElencoCulture.length; x++) {
                if (ElencoCulture[x]["Coltura1"] === jSonParsed_Kendo.kendo_rows[RO]["Coltura2"]) {
                    TrovatoElemento = true;
                }
            }
            if (!TrovatoElemento) {
                ElencoCulture.push({ "CodColtura1": jSonParsed_Kendo.kendo_rows[RO]["Coltura2"], "Coltura1": jSonParsed_Kendo.kendo_rows[RO]["Coltura2"] });
            }
            TrovatoElemento = false;

            for (var x = 0; x < ElencoCulture.length; x++) {
                if (ElencoCulture[x]["Coltura1"] === jSonParsed_Kendo.kendo_rows[RO]["Coltura3"]) {
                    TrovatoElemento = true;
                }
            }
            if (!TrovatoElemento) {
                ElencoCulture.push({ "CodColtura1": jSonParsed_Kendo.kendo_rows[RO]["Coltura3"], "Coltura1": jSonParsed_Kendo.kendo_rows[RO]["Coltura3"] });
            }
            TrovatoElemento = false;
            for (var x = 0; x < ElencoCulture.length; x++) {
                if (ElencoCulture[x]["Coltura1"] === jSonParsed_Kendo.kendo_rows[RO]["Coltura4"]) {
                    TrovatoElemento = true;
                }
            }
            if (!TrovatoElemento) {
                ElencoCulture.push({ "CodColtura1": jSonParsed_Kendo.kendo_rows[RO]["Coltura4"], "Coltura1": jSonParsed_Kendo.kendo_rows[RO]["Coltura4"] });
            }
        }
        ElencoCulture.sort(function (a, b) {
            if (a.Coltura1 > b.Coltura1) return 1;
            else if (a.Coltura1 < b.Coltura1) return -1;
            return 0;
        }); //contollare il sort kiwano
    }

    var colonneKendoGrid = [
        { field: 'Sa_Nome', title: 'Centro Aziendale', width: 150 },
        { field: 'Campo_Des', title: 'Campo', width: 150 },
        { field: 'App_Nome', title: 'App.', width: 150 },
        { field: 'Veg_Des', title: 'Specie', width: 150 },
        { field: 'Cul_Des', title: 'Varietà', width: 150 },
        { field: 'Dest_Uso', title: 'Dest. Uso', width: 150 },
        { field: 'Sup_Imp', title: 'Sup.Imp. [ha]', width: 150 },
        { field: 'Validita', title: 'Validità Impianto', width: 150 },
        { field: "Coltura1", title: "Coltura Precedente", editor: Cultura_DropDownEditor, width: 150 },
        { field: "Coltura2", title: "Coltura Precedente 2", editor: Cultura_DropDownEditor2, width: 150 },
        { field: "Coltura3", title: "Coltura Precedente 3", editor: Cultura_DropDownEditor3, width: 150 },
        { field: "Coltura4", title: "Coltura Precedente 4", editor: Cultura_DropDownEditor4, width: 150 }
    ];

    var colCustKendoGrid = [
        {
            command: [
                {
                    iconClass: "fa fa-pencil fa-xs", className: "blockModifica", name: "edit", text: { edit: "", update: "Conf.", cancel: "Ann." }
                }
            ],
            title: "Operazioni", width: "150px"
        }
    ];



    var parametriPerLettura = [];
    var parametriDataSource = { pagesize: 50 };
    var parametriKendoGrid = {
        editable: {
            mode: "inline"
        },
        columnMenu: false,
        impostaColonneKendoGridDaCookie: false,
        excel: false,
        pdf: false,
        sortable: true,
        groupable: false,
        reorderable: false,
        salvaRipristinaPersonalizzazioni: false,
        scrollable: true,
        selectable: "row",
        pageable: { pageSizes: [5, 10, 20, 50, 100, "all"], buttonCount: 3 },
        filterable: false,
        btnEliminaTuttiFiltri: false,
        colonneCustomKendoGrid: colCustKendoGrid,

    };
    var funzioniPrimaDopoEventi = {
        funzioneDaChiamareDopoDataBound: function (e) {
            //Se in mobile mostro solo la colonna unica
            //mostraColonnaUnicaSeInMobile(e, 1);
            //autoFitSeMobile(e);

            //Accorcio l'altezza delle righe
            //riduciAltezzaRighe(e, 1);

            //    if (keys != undefined) {
            //        var arrKeys = keys.split(",");
            //        var grid = $("#" + divKendo).data("kendoGrid");
            //        var data = grid.dataSource.data();
            //        for (var i = 0; i < arrKeys.length; i++) {
            //            for (var j = 0; j < data.length; j++) {
            //                if (data[j].chiave == arrKeys[i]) {
            //                    var rowUid = data[j].uid;
            //                    var row = grid.table.find("[data-uid=" + rowUid + "]");
            //                    grid.select(row);
            //                }
            //            }
            //        }
            //    }
            //    nascondiBottoniProdotto();
        },
        funzioneDaChiamareDopoEdit: null //onEditNumeratoriTipi
    };
    var mostraRigheCancellate = true;
    var colonneDisabilitateSoloInModifica = [];
    var funzioniCRUD = {
        funzioneRead: kReadValorizzazioneGrid_rows,
        funzioneSubmit: { funzione: SubmitCulturePrecedenti, flagInsert: false, flagUpdate: true, flagDelete: false },
        UtenteAbilitatoInserimentoModifica: true

    };

    KendoOperazioni = creaKendoGrid(divKendo, // rappresenta l'ID del div a cui si associa la griglia
        funzioniCRUD,  //funzioni js da chiamare per read, insert, update, delete
        "KeyReg", // chiave riga
        campiKendoModel, // campi modello
        colonneKendoGrid, // colonne da mostrare
        parametriPerLettura, // parametri da passare alla lettura
        parametriDataSource, // parametri data source { chiave - valore}
        parametriKendoGrid,   // parametri griglia [{ chiave - valore}]
        funzioniPrimaDopoEventi, // funzioni da chiamare all'inizio e alla fine dei vari eventi
        mostraRigheCancellate, // se true le righe cancellate vengono mostrate barrate e viene gestita funzione custom cancellazione
        colonneDisabilitateSoloInModifica // colonne non modificabili in modifica["colA", "colB", ...]
    );



}


function kendoElencoReport_inizializza(divKendo, keys) {
    let jSonParsed_Kendo = JSON.parse(keys);
    var campiKendoModel = jSonParsed_Kendo.kendo_model;
    var colonneKendoGrid = [
        { field: 'Piva', title: 'Piva Aziendale', width: 150 },
        { field: 'Documento_Cod', title: 'Documento', width: 150 },
        { field: 'SottoCartella', title: 'SottoCartella.', width: 150 },
        { field: 'NomeFile', title: 'NomeFile', width: 150 },
        { field: 'Inizio', title: 'Inizio', width: 150 },
        { field: 'Fine', title: 'Fine', width: 150 }
    ];

    var templateVisualizzaProdotto = "<span class='fa fa-info fa-2x info_elem' title='Scarica file pdf' onclick=ScaricaFile(this.closest('tr'),this.closest('.k-grid'),0)></span>";
    var colCustKendoGrid = [
        {
            command: [
                {
                    template: templateVisualizzaProdotto
                }
            ],
            title: "Dowload", width: "150px"
        }
    ];



    var parametriPerLettura = [];
    var parametriDataSource = { pagesize: 50 };
    var parametriKendoGrid = {
        editable: {
            mode: "inline"
        },
        columnMenu: false,
        impostaColonneKendoGridDaCookie: false,
        excel: false,
        pdf: false,
        sortable: true,
        groupable: false,
        reorderable: false,
        salvaRipristinaPersonalizzazioni: false,
        scrollable: true,
        selectable: "row",
        pageable: { pageSizes: [5, 10, 20, 50, 100, "all"], buttonCount: 3 },
        filterable: false,
        btnEliminaTuttiFiltri: false,
        colonneCustomKendoGrid: colCustKendoGrid,

    };
    var funzioniPrimaDopoEventi = {
        funzioneDaChiamareDopoDataBound: function (e) {
        },
        funzioneDaChiamareDopoEdit: null //onEditNumeratoriTipi
    };
    var mostraRigheCancellate = true;
    var colonneDisabilitateSoloInModifica = [];
    var funzioniCRUD = {
        funzioneRead: kReadValorizzazioneGridElRe_rows,
        UtenteAbilitatoInserimentoModifica: true

    };

    KendoOperazioni = creaKendoGrid(divKendo, // rappresenta l'ID del div a cui si associa la griglia
        funzioniCRUD,  //funzioni js da chiamare per read, insert, update, delete
        "PIva", // chiave riga
        campiKendoModel, // campi modello
        colonneKendoGrid, // colonne da mostrare
        parametriPerLettura, // parametri da passare alla lettura
        parametriDataSource, // parametri data source { chiave - valore}
        parametriKendoGrid,   // parametri griglia [{ chiave - valore}]
        funzioniPrimaDopoEventi, // funzioni da chiamare all'inizio e alla fine dei vari eventi
        mostraRigheCancellate, // se true le righe cancellate vengono mostrate barrate e viene gestita funzione custom cancellazione
        colonneDisabilitateSoloInModifica // colonne non modificabili in modifica["colA", "colB", ...]
    );

}