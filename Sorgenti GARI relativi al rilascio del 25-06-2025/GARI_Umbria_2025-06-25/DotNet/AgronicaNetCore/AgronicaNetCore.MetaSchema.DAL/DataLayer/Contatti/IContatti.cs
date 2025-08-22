using System.Data;
using AgronicaNetCore.Base.Models;

namespace AgronicaNetCore.MetaSchema.DAL.DataLayer.Contatti;

public interface IContatti
{
    public Task<DataTable> LeggiAsync(string visibilityFilter, AgronicaCoreParametri objParams);

    public Task<DataTable> LeggiFromCodRisUmAsync(string codRisUm, AgronicaCoreParametri objParams);
}