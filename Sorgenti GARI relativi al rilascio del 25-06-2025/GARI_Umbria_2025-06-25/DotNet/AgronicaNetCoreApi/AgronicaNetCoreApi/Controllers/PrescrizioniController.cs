using Newtonsoft.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using AgronicaCoreUtilityStd;
using AgronicaCoreVarieBizSTD;
using AgronicaNetCore.Base.Utility;
using AgronicaDataProvider6.Models;
using AgronicaCoreDTOStd.InData.Zoo;
using AgronicaNetCore.Zoo.BIZ.Services.Prescrizioni;
using AgronicaCoreModelsSTD.attivita;
using AgronicaNetCore.Zoo.DAL.DataLayer.Prescrizioni;
using OutData.Zoo;
using Microsoft.Extensions.Localization;
using AgronicaNetCoreApi.Resources;
using InData.Zoo;
using System.Data;
using System.ComponentModel.DataAnnotations;

namespace AgronicaNetCoreApi.Controllers
{
    [Authorize]
    [Route("prescrizioni")]
    [ApiController]
    public class PrescrizioniController : BaseController
    {
        private readonly IPrescrizioni _prescrizioniDal;
        private readonly IPrescrizionIService _prescrizioniBiz;

        public PrescrizioniController(IServiceProvider provider, IOptions<SecuritySettings> securitySettings, IStringLocalizer<Messages> localizer) : base(provider, securitySettings, localizer)
        {
            _prescrizioniDal = provider.GetRequiredService<IPrescrizioni>();
            _prescrizioniBiz = provider.GetRequiredService<IPrescrizionIService>();
        }

        [HttpPost("GetPrescrizioni")]
        [ProducesResponseType(typeof(RispostaStandard), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(RispostaStandard), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(RispostaStandard), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> LeggiPrescrizioni([FromBody] LeggiPrescrizioni body)
        {
            var resp = new RispostaStandard();

            try
            {
                var auth = await _identityService.IsAuthorizedAsync(HttpContext.User.Identity as ClaimsIdentity, Request.Headers);
                if (!auth.status) return Unauthorized();

                var objParametriServer = UtilityAgronica.convertStringtoOBJparametri(auth.objP.objP_server, _securitySettings);
                var objParametriUtenti = UtilityAgronica.convertStringtoOBJparametri(auth.objP.objP_utenti, _securitySettings);
                var result = await _prescrizioniDal.ReadPrescrizioniAsync(body, objParametriServer, objParametriUtenti);

                resp.RispostaStringa = JsonConvert.SerializeObject(result, Formatting.Indented);
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

        [HttpPost("GetRighePrescrizione")]
        [ProducesResponseType(typeof(RispostaStandard), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(RispostaStandard), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(RispostaStandard), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> LeggiRighePrescrizione([FromQuery] int Ricetta_Cod)
        {
            var resp = new RispostaStandard();

            try
            {
                var auth = await _identityService.IsAuthorizedAsync(HttpContext.User.Identity as ClaimsIdentity, Request.Headers);
                if (!auth.status) return Unauthorized();

                var objParams = ExtractObjParametriServerAndSetCulture(auth);

                if (Ricetta_Cod == 0)
                {
                    resp.RispostaOK = false;
                    resp.Errore = "Ricetta_Cod non può essere 0.";
                    return BadRequest(resp);
                }

                var result = await _prescrizioniDal.ReadRighePrescrizioneAsync(Ricetta_Cod, objParams);

                resp.RispostaStringa = JsonConvert.SerializeObject(result, Formatting.Indented);
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

        [HttpPost("GetDestinazioniProdottiPres")]
        [ProducesResponseType(typeof(RispostaStandard), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(RispostaStandard), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(RispostaStandard), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> LeggiDestinazioniProdottiPres([FromQuery] int Ricetta_Cod, [FromQuery] int Riga_Cod)
        {
            var resp = new RispostaStandard();

            try
            {
                var auth = await _identityService.IsAuthorizedAsync(HttpContext.User.Identity as ClaimsIdentity, Request.Headers);
                if (!auth.status) return Unauthorized();

                var objParams = ExtractObjParametriServerAndSetCulture(auth);

                if (Ricetta_Cod == 0)
                {
                    resp.RispostaOK = false;
                    resp.Errore = "Ricetta_Cod non può essere 0.";
                    return BadRequest(resp);
                }

                if (Riga_Cod == 0)
                {
                    resp.RispostaOK = false;
                    resp.Errore = "Riga_Cod non può essere 0.";
                    return BadRequest(resp);
                }

                var result = await _prescrizioniDal.ReadDestinazioniProdottiPresAsync(Ricetta_Cod, Riga_Cod, objParams);

                resp.RispostaStringa = JsonConvert.SerializeObject(result, Formatting.Indented);
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
        
        [HttpPost("GetTempiSospensioneRigaPrescrizione")]
        [ProducesResponseType(typeof(RispostaStandard), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(RispostaStandard), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(RispostaStandard), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> LeggiTempiSospensioneRigaPres([FromQuery] int Ricetta_Cod, [FromQuery] int Riga_Cod)
        {
            var resp = new RispostaStandard();

            try
            {
                var auth = await _identityService.IsAuthorizedAsync(HttpContext.User.Identity as ClaimsIdentity, Request.Headers);
                if (!auth.status) return Unauthorized();

                var objParams = ExtractObjParametriServerAndSetCulture(auth);

                if (Ricetta_Cod == 0)
                {
                    resp.RispostaOK = false;
                    resp.Errore = "Ricetta_Cod non può essere 0.";
                    return BadRequest(resp);
                }

                if (Riga_Cod == 0)
                {
                    resp.RispostaOK = false;
                    resp.Errore = "Riga_Cod non può essere 0.";
                    return BadRequest(resp);
                }

                var result = await _prescrizioniDal.ReadTempiSospensioneRigaPresAsync(Ricetta_Cod, Riga_Cod, objParams);

                resp.RispostaStringa = JsonConvert.SerializeObject(result, Formatting.Indented);
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

        [HttpPost(nameof(GetAttivitaFromPrescrizione))]
        [ProducesResponseType(typeof(RispostaStandard<Attivita>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(RispostaStandard), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(RispostaStandard), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(RispostaStandard), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAttivitaFromPrescrizione([FromQuery] int Ricetta_Cod, [FromQuery] int Ricetta_Agenda_Cod)
        {
            var resp = new RispostaStandard<SomministrazioneDaPrescrizione>();

            try
            {
                var auth = await _identityService.IsAuthorizedAsync(HttpContext.User.Identity as ClaimsIdentity, Request.Headers);
                if (!auth.status) return Unauthorized();

                var objParams = ExtractObjParametriServerAndSetCulture(auth);

                if (Ricetta_Cod == 0)
                {
                    resp.RispostaOK = false;
                    resp.Errore = "Ricetta_Cod non può essere 0.";
                    return BadRequest(resp);
                }
                
                var result = await _prescrizioniBiz.GetAttivitaFromRicetteZoo(Ricetta_Cod, Ricetta_Agenda_Cod, objParams);

                if (result == null)
                {
                    resp.RispostaOK = false;
                    resp.Errore = "Nessuna attivita creata.";
                    return NotFound(resp);
                }

                resp.RispostaStringa = result;
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

        [HttpPost(nameof(UpdatePrescription))]
        [ProducesResponseType(typeof(RispostaStandard), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(RispostaStandard), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(RispostaStandard), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(RispostaStandard), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdatePrescription([FromBody] UpdatePrescrizione updatePrescrizione)
        {
            var resp = new RispostaStandard<bool>();

            try
            {
                var auth = await _identityService.IsAuthorizedAsync(HttpContext.User.Identity as ClaimsIdentity, Request.Headers);
                if (!auth.status) return Unauthorized();

                if (updatePrescrizione.IdRicetta == 0)
                {
                    resp.RispostaOK = false;
                    resp.Errore = "IdRicetta non può essere 0.";
                    return BadRequest(resp);
                }

                if (updatePrescrizione.IdAgenda == 0)
                {
                    resp.RispostaOK = false;
                    resp.Errore = "IdAgenda non può essere 0.";
                    return BadRequest(resp);
                }
                
                if (updatePrescrizione.IdMov == 0)
                {
                    resp.RispostaOK = false;
                    resp.Errore = "IdMov non può essere 0.";
                    return BadRequest(resp);
                }
                
                if (updatePrescrizione.IdDettaglio == 0)
                {
                    resp.RispostaOK = false;
                    resp.Errore = "IdDettaglio non può essere 0.";
                    return BadRequest(resp);
                }

                var objP_Server = UtilityAgronica.convertStringtoOBJparametri(auth.objP.objP_server, _securitySettings);
                var objP_Utenti = UtilityAgronica.convertStringtoOBJparametri(auth.objP.objP_utenti, _securitySettings);
                string result = await _prescrizioniBiz.UpdatePrescrizioneAsync(updatePrescrizione, objP_Server, objP_Utenti);

                if (result != "")
                {
                    resp.RispostaOK = false;
                    resp.Errore = "Errore durante aggiornamento. " + result;
                    return NotFound(resp);
                }

                resp.RispostaStringa = true;
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

        [HttpPost(nameof(GetProtocolliInCorso))]
        [ProducesResponseType(typeof(RispostaStandard<List<ProtocolloInCorso>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(RispostaStandard), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(RispostaStandard), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(RispostaStandard), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(RispostaStandard), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetProtocolliInCorso([FromQuery] string Piva, [FromQuery] int Sa_Cod, [FromQuery] int Sta_Num, [FromQuery] DateTime Data)
        {
            var resp = new RispostaStandard<List<ProtocolloInCorso>>();

            try
            {
                var auth = await _identityService.IsAuthorizedAsync(HttpContext.User.Identity as ClaimsIdentity, Request.Headers);
                if (!auth.status) return Unauthorized();

                if (string.IsNullOrEmpty(Piva))
                {
                    resp.RispostaOK = false;
                    resp.Errore = $"Il parametro {nameof(Piva)} non può essere vuoto.";
                    return BadRequest(resp);
                }

                if (Sa_Cod == 0)
                {
                    resp.RispostaOK = false;
                    resp.Errore = $"Il parametro {nameof(Sa_Cod)} non può essere vuoto.";
                    return BadRequest(resp);
                }

                if (Sta_Num == 0)
                {
                    resp.RispostaOK = false;
                    resp.Errore = $"Il parametro {nameof(Sta_Num)} non può essere vuoto.";
                    return BadRequest(resp);
                }

                if (Data == default)
                {
                    resp.RispostaOK = false;
                    resp.Errore = $"Il parametro {nameof(Data)} non può essere vuoto.";
                    return BadRequest(resp);
                }

                var objP_Server = UtilityAgronica.convertStringtoOBJparametri(auth.objP.objP_server, _securitySettings);
                var objP_Utenti = UtilityAgronica.convertStringtoOBJparametri(auth.objP.objP_utenti, _securitySettings);

                var result = await _prescrizioniBiz.GetProtocolliInCorso(Piva, Sa_Cod, Sta_Num, Data, objP_Server, objP_Utenti);

                resp.RispostaStringa = result;
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
