
using System.Data;
using AgronicaNetCore.Base.Models;
using InData.ProfilazioneMacchina;

namespace AgronicaNetCore.ProfilazioneMacchine.BIZ.Services;

public interface IProfilazioneMacchineService
{

    public Task<DataTable> LeggiAsync(int idProfilazione, AgronicaCoreParametri objParams);
    public Task<bool> UpdateCaratteristicheMacchinaAsync(CaratteristicheMacchina_In body, AgronicaCoreParametri objParams);
}