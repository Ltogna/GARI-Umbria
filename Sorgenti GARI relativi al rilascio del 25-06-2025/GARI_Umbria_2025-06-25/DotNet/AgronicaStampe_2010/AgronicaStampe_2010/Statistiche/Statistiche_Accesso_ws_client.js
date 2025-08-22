var indirizzohttp = "./Statistiche_Accesso.aspx";

function CaricaRicercaRapida() {
    return new Promise((resolve, reject) => {
        ajaxAgronica(indirizzohttp + "/caricaAziende",
            {},
            function (risposta) {
                let result = JSON.parse(risposta.RispostaStringa);
                resolve(result);
            }, function (risposta) { reject(); gestioneErrore(risposta.RispostaStringa) });
    })
}
function getDataGrafici() {
    let v_inizio = get_data("DataValiditaInizio");
    let v_fine = get_data("DataValiditaFine");
    let parametri = {
        "v_inizio": v_inizio == null ? '' : v_inizio,
        "v_fine": v_fine == null ? '' : v_fine,
        "piva": Get_KendoDDLValue("ddRicercaRapida"),
        "tipoFiltro": getTipoFiltro()
    }
    return new Promise((resolve, reject) => {
        ajaxAgronica(indirizzohttp + "/getDataGrafici",
            JSON.stringify(parametri),
            function (risposta) {
                let result = JSON.parse(risposta.RispostaStringa);
                resolve(result);
            }, function (risposta) { reject(); gestioneErrore(risposta.RispostaStringa) });
    })
}

function getElencoSinteticoMovimenti() {
    let v_inizio = get_data("DataValiditaInizio");
    let v_fine = get_data("DataValiditaFine");
    let parametri = {
        "v_inizio": v_inizio == null ? '' : v_inizio,
        "v_fine": v_fine == null ? '' : v_fine,
        "piva": Get_KendoDDLValue("ddRicercaRapida"),
        "tipoFiltro": getTipoFiltro()
    }

    return new Promise((resolve, reject) => {
        ajaxAgronica(indirizzohttp + "/getElencoSinteticoMovimenti",
            JSON.stringify(parametri),
            function (risposta) {
                if (risposta.RispostaStringa != '') {
                    let risp = JSON.parse(risposta.RispostaStringa);
                    let fileDati = risp.File;
                    let nomeFile = risp.NomeFile;
                    let estensione = risp.Estensione;

                    SaveAndOpenFileByteArray(nomeFile, fileDati, estensione);
                }
            }, function (risposta) {
                reject();
                gestioneErrore(risposta.RispostaStringa)
            });
    })

}

function getReportDettagli() {
    let v_inizio = get_data("DataValiditaInizio");
    let v_fine = get_data("DataValiditaFine");
    let parametri = {
        "v_inizio": v_inizio == null ? '' : v_inizio,
        "v_fine": v_fine == null ? '' : v_fine,
        "piva": Get_KendoDDLValue("ddRicercaRapida"),
        "tipoFiltro": getTipoFiltro()
    }
    return new Promise((resolve, reject) => {
        ajaxAgronica(indirizzohttp + "/getReportDettagliServiziPerAzienda",
            JSON.stringify(parametri),
            function (risposta) {
                if (risposta.RispostaStringa != '') {
                    let risp = JSON.parse(risposta.RispostaStringa);
                    let fileDati = risp.File;
                    let nomeFile = risp.NomeFile;
                    let estensione = risp.Estensione;

                    SaveAndOpenFileByteArray(nomeFile, fileDati, estensione);
                }
                resolve();
            }, function (risposta) {
                reject();
                gestioneErrore(risposta.RispostaStringa)
            });
    })
}

function getElencoSinteticoDettagliOperazioni() {
    let v_inizio = get_data("DataValiditaInizio");
    let v_fine = get_data("DataValiditaFine");
    let parametri = {
        "v_inizio": v_inizio == null ? '' : v_inizio,
        "v_fine": v_fine == null ? '' : v_fine,
        "piva": Get_KendoDDLValue("ddRicercaRapida"),
        "tipoFiltro": getTipoFiltro()
    }
    return new Promise((resolve, reject) => {
        ajaxAgronica(indirizzohttp + "/getElencoSinteticoMovimentiConDettagli",
            JSON.stringify(parametri),
            function (risposta) {
                if (risposta.RispostaStringa != '') {
                    let risp = JSON.parse(risposta.RispostaStringa);
                    let fileDati = risp.File;
                    let nomeFile = risp.NomeFile;
                    let estensione = risp.Estensione;

                    SaveAndOpenFileByteArray(nomeFile, fileDati, estensione);
                }
            }, function (risposta) {
                reject();
                gestioneErrore(risposta.RispostaStringa)
            });
    })
}
