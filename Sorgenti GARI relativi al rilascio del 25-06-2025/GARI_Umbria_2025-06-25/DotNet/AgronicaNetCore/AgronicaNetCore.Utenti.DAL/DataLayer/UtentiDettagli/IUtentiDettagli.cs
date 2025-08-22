using AgronicaNetCore.Base.Models;
using System.Data;

namespace AgronicaNetCore.Utenti.DAL.DataLayer.UtentiDettagli
{
    public interface IUtentiDettagli
    {
        Task<DataTable> LeggiAsync(int idServizio, AgronicaCoreParametri objParametriUtenti);
    }
}
