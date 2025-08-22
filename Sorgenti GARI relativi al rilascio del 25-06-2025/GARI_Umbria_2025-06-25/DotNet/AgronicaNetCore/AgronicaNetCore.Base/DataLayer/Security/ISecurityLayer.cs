using AgronicaNetCore.Base.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgronicaNetCore.Base.DataLayer.Security
{
    public interface ISecurityLayer
    {
        public Task<DataTable> LeggiConnessioniAsync(AgronicaCoreParametri objParams);

        [Obsolete("Usare la versione asincrona del metodo")]
        public DataTable LeggiConnessioni(AgronicaCoreParametri objParams);

        public Task<DataTable> LeggiConfigurazioneSitiAsync(string chiave, AgronicaCoreParametri objParams);
        public Task<string> LeggiConfigurazioneSitiScalareAsync(string chiave, AgronicaCoreParametri objParams_Server, AgronicaCoreParametri objParams_Super_Server);

        [Obsolete("Usare la versione asincrona del metodo")]
        public DataTable LeggiConfigurazioneSiti(string chiave, AgronicaCoreParametri objParams);
    }
}
