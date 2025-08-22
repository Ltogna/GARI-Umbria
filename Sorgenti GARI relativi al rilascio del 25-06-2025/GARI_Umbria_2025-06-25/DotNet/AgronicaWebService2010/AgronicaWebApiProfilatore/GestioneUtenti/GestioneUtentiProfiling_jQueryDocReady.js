
//DOCUMENT READY
$(document).ready(function () {

    $.logThis("DocReady: INIZIO");

    //inizializzazione della pagina la prima volta che viene caricata


    $(".panelArea").show();


    $(".kendoCalendar").kendoDatePicker({
        footer: "#: kendo.toString(data, 'd')#",  //Template per il footer
        max: new Date(2100, 11, 31)
    });

    //creaKendoMultiselect("multiselLicenza", { read: RiempiLicenze, data: { Id_Servizio: 0 } }, "Servizio_Des", "Id_Servizio", null, null, null, LicenzeChange);
    //creaKendoMultiselect("multiselLicenza_Estensione", { read: RiempiLicenzeEstensione, data: { Id_Servizio: 0 } }, "Servizio_Des", "Id_Servizio", null, null, null, LicenzeEstensioneChange);

    
    ConfiguraGrigliaUtentiProfiling("tab_griglia_utenti_profiling", true);


    //fine controlli

    $('#dialogSessioneScaduta').on('show.bs.modal', function (event) {
        impostaRedirectStart();
    });



    $("#btn_ricerca_Profiling").click(
        function () {
            ConfiguraGrigliaUtentiProfiling("tab_griglia_utenti_profiling", false);
        });



    $.logThis("DocReady: FINE");

});
