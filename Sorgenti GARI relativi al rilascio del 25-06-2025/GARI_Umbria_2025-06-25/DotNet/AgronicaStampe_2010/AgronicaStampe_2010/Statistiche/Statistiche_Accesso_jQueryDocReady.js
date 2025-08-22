$(document).ready(function () {

    $.logThis("DocReady: INIZIO");
    docReady();


});

function docReady() {
    caricaRicercaRapida();
    iniziallizaTooltips();
    iniziallizaKendoDate();
    iniziallizaOnClickEvents();
    $(".btn").unbind('hover');
}