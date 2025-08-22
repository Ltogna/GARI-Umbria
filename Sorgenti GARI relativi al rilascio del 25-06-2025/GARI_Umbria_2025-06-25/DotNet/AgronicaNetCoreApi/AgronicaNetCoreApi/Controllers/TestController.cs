using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using AgronicaNetCore.Anagrafe.BIZ.Services.Imprese;
using Microsoft.Extensions.Options;
using AgronicaDataProvider6.Models;
using AgronicaNetCoreApi.Controllers;
using AgronicaCoreDTOStd.InData.Utility;
using AgronicaNetCore.Gis.BIZ.Services.Gis;
using Microsoft.Extensions.Localization;
using AgronicaNetCoreApi.Resources;
using AgronicaNetCore.Operazione.BIZ.Services.ActivityImport.Mapper;
using AgronicaCoreDTOStd.InData.ActivityImport;


namespace AgronicaCoreApiNet6.Controllers
{
    [ApiController]
    [Authorize]
    public class TestController : BaseController
    {
        private readonly IImpresaService _impresaService;
        private readonly IGisService _gisService;
        private readonly ILogger<TestController> _logger;
        private readonly IActivityImportMapper<ActivityImportData> _test;

        public TestController(ILogger<TestController> logger, IServiceProvider provider, IOptions<SecuritySettings> securitySettings, IStringLocalizer<Messages> localizer) : base(provider, securitySettings, localizer) 
        {
            _logger = logger;
            _impresaService = provider.GetRequiredService<IImpresaService>();
            _gisService = provider.GetRequiredService<IGisService>();
            _test = provider.GetRequiredService<IActivityImportMapper<ActivityImportData>>();
        }

        [HttpGet]
        [Route("Impresa")]
        public async Task<IActionResult> LeggiImpresa(string partitaiva)
        {
            try
            {
                var auth = await _identityService.IsAuthorizedAsync(HttpContext.User.Identity as ClaimsIdentity, Request.Headers);
                if (!auth.status) return Unauthorized();

                var objParametriServer = ExtractObjParametriServerAndSetCulture(auth);

                var impresa = await _impresaService.LeggiImpresaAsync(objParametriServer,partitaiva);

                if (impresa == null) return NotFound("Impresa non trovata.");

                return Ok(impresa);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }

        [HttpGet]
        [Route("ImpresaDT")]
        public async Task<IActionResult> LeggiImpressa2(string partitaIva)
        {
            try
            {
                var auth = await _identityService.IsAuthorizedAsync(HttpContext.User.Identity as ClaimsIdentity, Request.Headers);
                if (!auth.status) return Unauthorized();

                var objParametriServer = ExtractObjParametriServerAndSetCulture(auth);

                var impresa = await _impresaService.Leggi2Async(objParametriServer,partitaIva);

                if (impresa == null) return NotFound("Impresa non trovata.");

                return Ok(impresa);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }


        [HttpPost]
        [Route("TestClaudolaIN")]
        public async Task<IActionResult> LeggiImpresaClausolaIN(Test request)
        {
            try
            {
                var auth = await _identityService.IsAuthorizedAsync(HttpContext.User.Identity as ClaimsIdentity, Request.Headers);
                if (!auth.status) return Unauthorized();

                var objParametriServer = ExtractObjParametriServerAndSetCulture(auth);

                var impresa = await _impresaService.TestClausolaINAsync(request.elencoPiva, request.elencoVegCod, request.varieta, objParametriServer);
                if (impresa == null) return NotFound("Impresa non trovata.");

                return Ok(impresa);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }

        [HttpPost]
        [Route("TestImport")]
        [AllowAnonymous]
        public async Task<IActionResult> TestImport(ActivityImportData data)
        {
            try
            {
                var auth = await _identityService.IsAuthorizedAsync(HttpContext.User.Identity as ClaimsIdentity, Request.Headers);
                if (!auth.status) return Unauthorized();

                var objParametriServer = ExtractObjParametriServerAndSetCulture(auth);

                _test.SetBase(data);
                var att = await _test.MapActivity(objParametriServer);

                return Ok(att);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }
    }
}
