
////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////     GRIGLIE    ///////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function ConfiguraGrigliaUtentiProfiling(IDControllo, bRead) {

    funzioneSubmitDaUsare = { funzione: SubmitUtenti, flagInsert: true, flagUpdate: false, flagDelete: true };

    //var Abilitazione = "True";
    //var AbilitazioneBoolean = true;

    var funzioniCRUD = {
        funzioneRead: Ricerca_Utenti_Profiling,
        funzioneSubmit: funzioneSubmitDaUsare,
        UtenteAbilitatoInserimentoModifica: false,
        UtenteAbilitatoCancellazione: false,
        omettiPulsantiSalva: true,
        checkBoxFunction: false
    };
    var idModel = "CodiceTransazioneComm";
    var campiKendoModel = {
        RagioneSociale: { editable: false, type: "string" },
        Cognome: { editable: false, type: "string" },
        Nome: { editable: false, type: "string" },
        Via: { editable: false, type: "string" },
        NumeroCivico: { editable: false, type: "number" },
        Citta: { editable: false, type: "string" },
        Provincia: { editable: false, type: "string" },
        Cap: { editable: false, type: "string" },
        Tel: { editable: false, type: "string" },
        Fax: { editable: false, type: "string" },
        Email: { editable: false, type: "string" },
        Piva: { editable: false, type: "string" },
        CodiceTransazioneComm: { editable: false, type: "string" },
        CodiceSdi: { editable: false, type: "string" },
        NLicenze: { editable: false, type: "number" },
        CodiceCoupon: { editable: false, type: "string" },
        ModalitaPagamento: { editable: false, type: "string" },
        StatoPagamento: { editable: false, type: "string" },
        PEC: { editable: false, type: "string" }

    };

    var colonneKendoGrid = [
        { field: "RagioneSociale", title: "Ragione Sociale" },
        { field: "Cognome", title: "Cognome" },
        { field: "Nome", title: "Nome" },
        { field: "Via", title: "Via" },
        { field: "NumeroCivico", title: "Numero Civico" },
        { field: "Citta", title: "Città" },
        { field: "Provincia", title: "Provincia" },
        { field: "Cap", title: "Cap" },
        { field: "Tel", title: "Tel" },
        { field: "Fax", title: "Fax" },
        { field: "Email", title: "Email" },
        { field: "Piva", title: "P.IVA" },
        { field: "CodiceTransazioneComm", title: "CodiceTransazioneComm" },
        { field: "CodiceSdi", title: "CodiceSdi" },
        { field: "NLicenze", title: "N.Licenze" },
        { field: "CodiceCoupon", title: "Codice Coupon" },
        { field: "ModalitaPagamento", title: "Modalità Pagamento" },
        { field: "StatoPagamento", title: "Stato Pagamento" },
        { field: "PEC", title: "PEC" }

    ];

    var colCustKendoGrid = [];
    var parametriPerLettura = [bRead];
    var parametriDataSource = {};
    var colonneDisabilitateSoloInModifica = [];

    colCustKendoGrid = [
        {
            command: [
                {//modificaElemento
                    template: "<span class='fa fa-2x fa-pencil-square-o edit_elem' title='" + "Modifica" + "' onclick=rinnovaLicenza(this.closest('tr'),this.closest('.k-grid'))></span>"
                }
            ]
            , title: "Azioni", width: "100px"
        }

    ];


    var parametriKendoGrid = {
        //columnMenu: false,
        // editable: { mode: "inline" },
        selectable: true,
        colonneCustomKendoGrid: colCustKendoGrid,
        pageable: { pageSizes: [5, 10, 20, 50, 100, "all"] }
    }; // { salvaRipristinaPersonalizzazioni: { url: pathCoreWS }};

    var funzioniPrimaDopoEventi = { funzioneDaChiamareDopoEdit: EventiGridEdit, funzioneDaChiamareDopoDataBound: kendo_Operazioni_onDataBoundedRighe_UtentiProfiling };

    var funzioniPrimaDopoDatabound = {};
    var mostraRigheCancellate = false;


    creaKendoGrid(IDControllo, // rappresenta l'ID del div a cui si associa la griglia
        funzioniCRUD,  //funzioni js da chiamare per read, insert, update, delete
        idModel, // chiave riga 
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

function kendo_Operazioni_onDataBoundedRighe_UtentiProfiling(e) {


    //coloraRigheUtenti("#tab_griglia_utenti", e);

}

function SubmitUtenti(options) {

    let errMess = "";
    let found = false;
    bDatiNecessariInseriti = true;
    var updatedRecords = [];
    var newRecords = [];
    var deletedRecords = [];
    var grid = $("#tab_griglia_utenti").data("kendoGrid");

    var currentData = grid.dataSource.data();

    //Controllo Correttezza Dati

    currentData = grid.dataSource.data();

    for (let i = 0; i < currentData.length; i++) {


        if (currentData[i].dirty) {
            updatedRecords.push(currentData[i].toJSON());
        }

    }


    for (var i = 0; i < grid.dataSource._destroyed.length; i++) {
        deletedRecords.push(grid.dataSource._destroyed[i].toJSON());
    }

    if (newRecords.length > 0 || updatedRecords.length > 0 || deletedRecords.length > 0) {

        //Variabili globali
        righeInseriteGrid_Licenze = kendoEscapeOggetto(newRecords);
        righeModificateGrid_Licenze = kendoEscapeOggetto(updatedRecords);
        righeCancellateGrid_Licenze = kendoEscapeOggetto(deletedRecords);


    }

}



function SelezionaLicenza(e) {
    var checked = this.checked;
    var row = $(this).parents("tr");
    var grid = KendoGrid("tab_griglia_utenti_profiling");
    var dataItem = grid.dataItem(row);
    dataItem.Selected = checked;
    if (checked) {
        if (!row.hasClass("k-state-selected")) row.addClass("k-state-selected");
    } else {
        if (row.hasClass("k-state-selected")) row.removeClass("k-state-selected");
    }
}

//modificaElemento
function rinnovaLicenza(tr_elem, grid_elem) {

    var datiGriglia = $(grid_elem).data('kendoGrid');
    var datiRiga = datiGriglia.dataItem(tr_elem);

    var Username = datiRiga.UserName;

    var param = ""
    param = kendo.stringify({ 'Username': Username });

    window.location = "./GestioneUtentiProfiling.aspx?param=" + param;

    ////apriFormDialog('Scad_CreaModificaItem.aspx?scadstr=' + param);
}





function EventiGridEdit(e) {

    var fieldName = e.container.find("input").attr("name");
    var gridId = e.sender.element[0].id;
    var grid = $("#" + gridId).data("kendoGrid");

    grid.closeCell();

}

function Azione_Indietro() {
    window.location = "../Menu/Menu.aspx";
}



