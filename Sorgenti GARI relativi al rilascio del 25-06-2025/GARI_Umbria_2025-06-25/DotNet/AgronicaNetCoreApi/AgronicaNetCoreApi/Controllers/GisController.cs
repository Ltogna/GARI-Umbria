using AgronicaCoreUtilityStd;
using AgronicaCoreVarieBizSTD;
using AgronicaDataProvider6.Models;
using AgronicaNetCore.Base.Utility;
using AgronicaNetCore.Gis.BIZ.Services.Gis;
using AgronicaNetCoreApi.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Data;
using System.Security.Claims;

namespace AgronicaNetCoreApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class GisController : BaseController
    {
        private readonly IGisService _gisService;

        public GisController(IServiceProvider provider, IOptions<SecuritySettings> securitySettings, IStringLocalizer<Messages> localizer) : base(provider, securitySettings, localizer)
        {
            _gisService = provider.GetRequiredService<IGisService>();
        }

        [HttpGet]
        [Route(nameof(GetGISProcessingAlgorithmsCleaningAlgorithm))]
        [ProducesResponseType(typeof(RispostaStandard), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetGISProcessingAlgorithmsCleaningAlgorithm()
        {
            RispostaStandard resp = new();
            try
            {
                var auth = await _identityService.IsAuthorizedAsync(HttpContext.User.Identity as ClaimsIdentity, Request.Headers);
                if (!auth.status) return Unauthorized();
                var objParametriServer = ExtractObjParametriServerAndSetCulture(auth);

                DataTable GIS_ProcessingAlgorithms_Cleaning_Algorithm = await _gisService.LeggiGIS_ProcessingAlgorithms_Cleaning_AlgorithmAsync(objParametriServer);

                resp.RispostaStringa = JsonConvert.SerializeObject(GIS_ProcessingAlgorithms_Cleaning_Algorithm, Formatting.Indented);
                resp.RispostaOK = true;

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
