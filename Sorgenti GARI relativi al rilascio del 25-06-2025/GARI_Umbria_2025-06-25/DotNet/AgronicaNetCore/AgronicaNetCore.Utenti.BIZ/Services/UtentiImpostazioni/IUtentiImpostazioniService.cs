using AgronicaCoreDTOStd.OutData;
using AgronicaNetCore.Base.Models;
using System.Data;

namespace AgronicaNetCore.Utenti.BIZ.Services.UtentiImpostazioni
{
    public interface IUtentiImpostazioniService
    {
        Task<DataTable?> Read_User_Then_SuperUserAsync(int cod, AgronicaCoreParametri objParametriUser);
        Task<DataTable?> ReadAsync(int cod, int User1_SuperUser2, AgronicaCoreParametri objParametriUser);
        Task<DataInizioEFine> LeggiAnnataAgrariaAsync(AgronicaCoreParametri objParametriUser);
    }
}
