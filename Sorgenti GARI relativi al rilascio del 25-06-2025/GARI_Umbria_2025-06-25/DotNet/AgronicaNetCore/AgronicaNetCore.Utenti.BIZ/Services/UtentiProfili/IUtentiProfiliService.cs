using AgronicaNetCore.Base.Models;

namespace AgronicaNetCore.Utenti.BIZ.Services.UtentiProfili
{
    public interface IUtentiProfiliService { 
        Task<string?> ReadAsync(AgronicaCoreParametri objParametriUser, int idServizio = 0);
    }
}
