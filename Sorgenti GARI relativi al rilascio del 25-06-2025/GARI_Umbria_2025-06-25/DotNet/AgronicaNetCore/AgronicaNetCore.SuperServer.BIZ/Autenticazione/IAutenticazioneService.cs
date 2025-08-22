using AgronicaCoreDTOStd.InData.Provisioning;
using AgronicaNetCore.Base.Models;
using AgronicaNetCore.SuperServer.DAL.Autenticazione;
using System.Data;

namespace AgronicaNetCore.SuperServer.BIZ.Autenticazione
{
    public interface IAutenticazioneService
    {
        Task<bool> AggiornaUtentiTokenJWTAsync(string idToken, AggiornaUtentiTokenJWTCampi campi, AgronicaCoreParametri objParametriSuperServer);

        Task<CreaTokenJWT_In> RefreshTokenAsync(RefreshTokenJWT_In refreshTokenIN, AgronicaCoreParametri objParametriSuperServer);
        [Obsolete("Usare la versione asincrona del metodo")]
        DataTable LeggiObjParametri(string IdToken, AgronicaCoreParametri objParametriSuperServer);
        Task<DataTable> LeggiObjParametriAsync(string IdToken, AgronicaCoreParametri objParametriSuperServer);
    }
}
