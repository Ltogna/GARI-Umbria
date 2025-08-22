

//////////////////////////////////////////////////////////
// Griglia Imputazione
//////////////////////////////////////////////////////////

var indirizzohttp = "./GestioneUtentiProfiling.aspx";

function EmptyRead(options) { }
function EmptySubmit(options) { }

function Ricerca_Utenti_Profiling(options, parametriPerLettura) {

    //var filtro_datainizio = "0";
    
    
    var filtro_datainizio = $('input[name$="txtDataInizio"]').val();
    if (filtro_datainizio == "")
        filtro_datainizio = Date()

    var d = new Date(filtro_datainizio).toISOString();

    //alert("{ filtro_data_inizio: '" + filtro_datainizio + "'}");
    ajaxAgronicaSync(indirizzohttp + "/Ricerca_Utenti_Profiling",
        "{ filtro_data_inizio: '" + d + "'}"
        ,
        false,
        function (risposta) {
            risp = JSON.parse(risposta.RispostaStringa);
            options.success(risp);
        }, null);


}



