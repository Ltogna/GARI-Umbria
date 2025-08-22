using AgronicaCoreUtilityStd;
using AgronicaCoreVarieBizSTD;
using AgronicaNetCore.Base.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using AgronicaNetCore.FiltroRicerca.BIZ.Services;
using Microsoft.Extensions.Options;
using AgronicaDataProvider6.Models;
using Microsoft.Extensions.Localization;
using AgronicaNetCoreApi.Resources;

namespace AgronicaNetCoreApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class FiltroRicercaController : BaseController
    {
        private readonly IFiltroRicercaService _filtroRicercaService;

        public FiltroRicercaController(IServiceProvider provider, IOptions<SecuritySettings> securitySettings, IStringLocalizer<Messages> localizer) : base(provider, securitySettings, localizer)
        {
            _filtroRicercaService = provider.GetRequiredService<IFiltroRicercaService>();
        }

        [HttpPost]
        [Route(nameof(GetResult))]
        [ProducesResponseType(typeof(RispostaStandard), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetResult([FromBody] AgronicaCoreDTOStd.InData.FiltroRicerca.CriteriRicerca_IN criteriRicerca_IN)
        {
            RispostaStandard resp = new();
            try
            {
                var auth = await _identityService.IsAuthorizedAsync(HttpContext.User.Identity as ClaimsIdentity, Request.Headers);
                if (!auth.status) return Unauthorized();
                var objP_Server = ExtractObjParametriServerAndSetCulture(auth);

                var result = await _filtroRicercaService.GetResultAsync(criteriRicerca_IN, objP_Server, UtilityAgronica.convertStringtoOBJparametri(auth.objP.objP_utenti, _securitySettings), UtilityAgronica.convertStringtoOBJparametri(auth.objP.objP_super_server, _securitySettings)) ?? throw new Exception("Nessun dato recuperato");
                resp.RispostaStringa = JsonConvert.SerializeObject(result, Formatting.Indented);
                resp.RispostaOK = true;

                result.kendoColumns.Clear();

                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();

                return Ok(resp);
            }
            catch (Exception ex)
            {
                resp.RispostaOK = false;
                resp.Errore = Gestione_Eccezioni.MessaggioCompletoDataEccezione(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, resp);
            }

        }
    }
}
