using AgronicaCoreModelsSTD.metaschema;
using AgronicaNetCore.Base.Base;
using AgronicaNetCore.Base.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgronicaNetCore.Anagrafe.BIZ.Services.Impianti
{
    public interface IImpiantiService
    {
        Task<List<Contribute>?> GetContributiACAAsync(AgronicaCoreParametri objParametri);
    }
}
