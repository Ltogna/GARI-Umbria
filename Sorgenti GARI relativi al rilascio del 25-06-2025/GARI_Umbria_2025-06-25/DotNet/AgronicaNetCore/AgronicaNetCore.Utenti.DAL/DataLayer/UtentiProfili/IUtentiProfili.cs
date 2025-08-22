using AgronicaNetCore.Base.Models;
using System.Data;

namespace AgronicaNetCore.Utenti.DAL.DataLayer.UtentiProfili
{
    public interface IUtentiProfili { 
        Task<DataTable?> ReadAsync(AgronicaCoreParametri objParametriUser, int idServizio = 0);
        Task<bool> VisibilitaTotaleGiasOnline(AgronicaCoreParametri objParametriUser);
    }
}
