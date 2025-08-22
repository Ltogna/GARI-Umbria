using AgronicaCoreDTOStd.InData.Metaschema;
using AgronicaNetCore.Base.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgronicaNetCore.MetaSchema.DAL.DataLayer.UnitaMisura
{
    public interface IUnitaMisura
    {
        Task<DataTable> LeggiAsync(int udmCod, int udmCodAux, string filtroAggiuntivo, string orderBy, AgronicaCoreParametri objParams);
        Task<DataTable> LeggiAsyncxProtocolli(AgronicaCoreParametri objParametri);
    }
}
