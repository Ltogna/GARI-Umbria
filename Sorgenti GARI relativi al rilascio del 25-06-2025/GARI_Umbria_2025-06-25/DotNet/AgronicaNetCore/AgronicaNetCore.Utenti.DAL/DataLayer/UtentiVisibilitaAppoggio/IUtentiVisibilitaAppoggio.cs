using AgronicaNetCore.Base.Models;
using System.Data;

namespace AgronicaNetCore.Utenti.DAL.DataLayer.UtentiVisibilitaAppoggio
{
    public interface IUtentiVisibilitaAppoggio
    {
        Task<DataTable?> ReadAsync(int entity, AgronicaCoreParametri objParametriUser, string piva = "");

        Task<DataTable?> ReadVisibilitaCentriAsync(string? piva, AgronicaCoreParametri objParametriUser);
    }
}
