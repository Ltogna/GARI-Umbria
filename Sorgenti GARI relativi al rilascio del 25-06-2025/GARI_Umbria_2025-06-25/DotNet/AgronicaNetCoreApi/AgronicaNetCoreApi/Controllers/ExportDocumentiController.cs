using AgronicaCoreModelsSTD.Utility;
using AgronicaCoreUtilityStd;
using AgronicaCoreVarieBizSTD;
using AgronicaDataProvider6.Models;
using AgronicaNetCore.Base.Utility;
using AgronicaNetCore.Documentale.BIZ.Services;
using AgronicaNetCoreApi.Resources;
using InData.DataExchange;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OutData.DataExchange;
using System.Security.Claims;


namespace AgronicaNetCoreApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class ExportDocumentiController : BaseController
    {
        private readonly AgronicaNetCore.Documentale.BIZ.Services.IExportDocumentiService _ExportDocumentiService;

        public ExportDocumentiController(IServiceProvider provider, IOptions<SecuritySettings> securitySettings, IStringLocalizer<Messages> localizer) : base(provider, securitySettings, localizer)
        {
            _ExportDocumentiService = provider.GetRequiredService<IExportDocumentiService>();
        }

        [HttpPost]
        [Route(nameof(LeggiDocPortaleSocio))]
        [ProducesResponseType(typeof(Api_Response), StatusCodes.Status200OK)]
        public async Task<IActionResult> LeggiDocPortaleSocio([FromBody] InData.DataExchange.ExportDocumenti_In ExportDocumenti_In)
        {
            Api_Response resp = new();
            try
            {
                var auth = await _identityService.IsAuthorizedAsync(HttpContext.User.Identity as ClaimsIdentity, Request.Headers);
                if (!auth.status) return Unauthorized();
                var objP_Server = ExtractObjParametriServerAndSetCulture(auth);

                if (string.IsNullOrEmpty(ExportDocumenti_In.CUAA))
                    throw new Exception("Indicare un CUAA.");

                List<int> listaAmmissibiliTest_ToRemove = new() { 76, 77, 78 };
                //Tipologie create su ambiente di collaudo ZANI, per i test
                //76  Tipologia Test 1
                //77  Tipologia Test 2
                //78  Tipologia Test 3

                if (!listaAmmissibiliTest_ToRemove.Contains(ExportDocumenti_In.Id_Tipologia))
                    throw new Exception("Id_Tipologia non gestito in fase di test. Utilizzare una delle Tipologie fornite nella documentazione.");

                var result = await _ExportDocumentiService.GetDocumentiExportAsync(ExportDocumenti_In, objP_Server) ?? throw new Exception("Nessun dato recuperato");

                resp.dati = result;
                if (string.IsNullOrEmpty(result))
                    resp.message = "Nessun documento recuperato";
                else
                    resp.message = Api_Response_Message_Type.Ok;

                return Ok(resp);
            }
            catch (Exception ex)
            {
                resp.errore = ex.Message;
                resp.message = "";
                return StatusCode(StatusCodes.Status500InternalServerError, resp);
            }
        }
    }
}
