using AgronicaNetCore.Base.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgronicaNetCore.Anagrafe.DAL.DataLayer.Impresa
{
    public interface IImpresa
    {
        Task<DataTable?> LeggiAsync(string partitaIva, AgronicaCoreParametri objParamertri);

        Task<DataTable> LeggiPadriAsync(DataTable dtImpreseVisibili, AgronicaCoreParametri objParametri);

        Task<DataTable> LeggiImpreseAsync(string piva, DataTable dtImpreseVisibili, AgronicaCoreParametri objParametriServer);
        Task<DataTable> LeggiClausolaInAsync(List<string> elencoPiva, List<int> elencoVegCod, int varieta, AgronicaCoreParametri objParametri);

        Task<string> PivaFromCuaaAsync(string cuaa, AgronicaCoreParametri objParametri);
    }
}
