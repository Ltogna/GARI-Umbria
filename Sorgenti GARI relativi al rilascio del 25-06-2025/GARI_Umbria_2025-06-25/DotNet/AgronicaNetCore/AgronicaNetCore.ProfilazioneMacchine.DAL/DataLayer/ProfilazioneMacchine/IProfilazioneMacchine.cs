using System.Data;
using AgronicaNetCore.Base.Models;

namespace AgronicaNetCore.ProfilazioneMacchine.DAL.DataLayer.ProfilazioneMacchine;

public interface IProfilazioneMacchine
{
    public Task<DataTable> LeggiAsync(int idProfiloDati, int macCod, int macCarCod, string filtroAggiuntivo, string orderBy,
        AgronicaCoreParametri objParams);

    public Task<bool> CancellaAsync(int idProfiloDati, int macCod, int macCarCod, string filtroAggiuntivo,
        AgronicaCoreParametri objParams);

    public Task<bool> ScriviAsync(int idProfilazione, int macCod, int macCarCod, string valore, DateTime dtInizio, DateTime dtFine,
        AgronicaCoreParametri objParams);
}