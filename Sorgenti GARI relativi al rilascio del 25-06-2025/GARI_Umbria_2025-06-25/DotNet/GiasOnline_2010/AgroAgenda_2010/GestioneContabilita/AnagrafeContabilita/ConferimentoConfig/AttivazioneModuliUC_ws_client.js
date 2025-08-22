function Leggi_StatoAttivazioni() {
    var result = null;
    var param = "{ piva: '" + piva + "' }";
    ajaxAgronicaSync(indirizzohttp_Lotto_AssegnazioneUC + "/LeggiStatoAttivazioni",
        param,
        false,
        function (risposta) {
            risp = JSON.parse(risposta.RispostaStringa);
            result = risp;
        }, null);
    return result;
}
 
function Salva_AttivazioneModuli() {
    var trasformazioniVegetali = $("#chkTrasformazioniVegetali").data("kendoSwitch").check();
    var trasformazioniAnimali = $("#chkTrasformazioniAnimali").data("kendoSwitch").check();
    var param = "{ piva: '" + piva + "', trasformazioniVegetali: " + trasformazioniVegetali + ", trasformazioniAnimali: " + trasformazioniAnimali + " }";
    ajaxAgronicaSync(indirizzohttp_Lotto_AssegnazioneUC + "/SalvaAttivazioneModuli",
        param,
        false,
        function (risposta) {
            risp = JSON.parse(risposta.RispostaStringa);
            statoAttivazione = risp;
            MessaggioTuttoOK_Bootstrap("Salvataggio effettuato correttamente", "DIV_Messaggi");
            ImpostaSwitches();
        }, null);
}

function Conferma_Annulla_AttivazioneModuli() {
    Conferimento_Attivazione_ModuliUC_DocReady();
}