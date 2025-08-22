using AgronicaCoreAPI.Resources;
using AgronicaCoreModelsSTD.exceptions;
using AgronicaCoreVarieBizSTD;
using AgronicaCoreWebServiceSTD;
using AgronicaDataProvider6.Models;
using InData.WidgetManager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace AgronicaCoreAPI.Controllers
{
    public class DSSController : BaseController
    {
        public DSSController(IServiceProvider provider, IConfiguration config, IHttpClientFactory httpClientFactory, IOptions<SecuritySettings> securitySettings, IStringLocalizer<Messages> localizer) : base(provider, config, httpClientFactory, securitySettings, localizer)
        {

        }
        [HttpPost]
        [Route("WidgetManager")]
        public ObjectResult PostWidgetManager([FromBody] WidgetManagerRequest widgetManagerRequest, [FromHeader] string Authorization)
        {

            if (!isAuthorized(Authorization)) return StatusCode(StatusCodes.Status401Unauthorized, null);
            RispostaStandard Result = new RispostaStandard();
            widgetManagerResponse responseWidgetManager = new widgetManagerResponse();

            try
            {
                var request = new CoreWS_WidgetManager(objP_super_server,
                                                           objP_server,
                                                           objP_utenti,
                                                           widgetManagerRequest);

                //recupero il token ------- Start
                string token = "";
                APICallsBasic requestImpostazione = new APICallsBasic(objP_super_server, objP_server, objP_utenti);
                var resultImpostazione = new CoreWSController(hc, coreWSBaseURL, bearerToken, Request).LeggiImpostazioni(requestImpostazione);

                var LeggiImpostazione = new List<LeggiImpostazione>();
                LeggiImpostazione = JsonConvert.DeserializeObject<List<LeggiImpostazione>>(resultImpostazione.RispostaStringa);
                foreach (var item in LeggiImpostazione)
                {
                    if (item.Impostazione_Cod == 860)
                    {
                        if (item.Impostazione_Valore_1.ToString().Length > 0)
                        {
                            var ImpostazioneToken = new ImpostazioneToken();
                            ImpostazioneToken = JsonConvert.DeserializeObject<ImpostazioneToken>(item.Impostazione_Valore_1.ToString());
                            if (ImpostazioneToken.Token != "")
                            {
                                token = ImpostazioneToken.Token;
                            }
                        }
                        break;
                    }
                }

                if (string.IsNullOrEmpty(token))
                {
                    Result.RispostaOK = false;
                    return StatusCode(StatusCodes.Status400BadRequest, responseWidgetManager);
                }
                //recupero il token ------- End
         
                String urlServizio = coreWSBaseURL.Replace("AgronicaCoreWS", "AgronicaAgenda", StringComparison.CurrentCultureIgnoreCase) + "?token=" + token + "&username=" + user + "&rDir=D3GD0GC9GC6GBEGD9GD3GD5GD7GC2G161GA0";
       
                //creo widgetManager  -- Start
                string widgetManagerStr = "{ \"message\": \"OK\", \"statusCode\": 0, \"token\": null, \"dettaglioEsito\": { \"dettaglioRisposta\": [ { \"descrizione\": \"apri direttamente GIAS in una nuova scheda\", \"descrizioneAggiuntiva\": \"senza ripetere il login accedi all’applicazione completa\", \"idWidget\": \"SSOByPassGias\", \"nascosto\": false, \"tipoRender\": \"nuovaScheda\", \"titolo\": \"ACCEDI A GIAS\", \"urlImmagine\": \"\", \"urlServizio\": \"\" }, { \"descrizione\": \"riepilogo delle previsioni dei modelli di difesa\", \"descrizioneAggiuntiva\": \"\", \"idWidget\": \"DSSAgronica1\", \"nascosto\": false, \"tipoRender\": \"iframe\", \"titolo\": \"INDICATORI DSS DIFESA\", \"urlImmagine\": \"\", \"urlServizio\": \"\" } ], \"tipoRisposta\": null, \"titoloWidget\": \"widget di prova\" }, \"errori\": \"\" }";

                responseWidgetManager = JsonConvert.DeserializeObject<widgetManagerResponse>(widgetManagerStr);
                foreach (var item in responseWidgetManager.dettaglioEsito.dettaglioRisposta) 
                {
                    item.urlServizio = urlServizio;
                }

                //creo widgetManager  -- End            
            }
            catch (CoreWSNotAuthenticatedException coreWsNotAuthenticatedException)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, responseWidgetManager);
            }
            catch (Exception ex)
            {
                responseWidgetManager.message = "NO";
                //responseWidgetManager.Errore = Gestione_Eccezioni.MessaggioCompletoDataEccezione(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, responseWidgetManager);
            }

               return StatusCode(StatusCodes.Status200OK, responseWidgetManager);
        }
    }
}
