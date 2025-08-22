using AgronicaNetCore.Base.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgronicaNetCore.MetaSchema.DAL.DataLayer.Farmaci
{
    public interface IFarmaci
    {
        Task<DataTable> LeggiFarmaciAsync(int Farm_Cod, string Aic, AgronicaCoreParametri objParams);

        Task<DataTable> LeggiFarmaciListAICAsync(int Farm_Cod, List<string> Aic, List<string> FamigliaAic, AgronicaCoreParametri objParams);
    }
}
