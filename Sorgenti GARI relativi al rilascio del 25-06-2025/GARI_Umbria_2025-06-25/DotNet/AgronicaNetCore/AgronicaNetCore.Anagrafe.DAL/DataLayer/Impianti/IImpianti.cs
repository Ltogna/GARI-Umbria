using AgronicaCoreDTOStd.InData.Anagrafica;
using AgronicaCoreModelsSTD.metaschema;
using AgronicaCoreModelsSTD.metaschema.utilizzi;
using AgronicaNetCore.Base.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgronicaNetCore.Anagrafe.DAL.DataLayer.Impianti
{
    public interface IImpianti
    {
        Task<DataTable?> GetPianoColturalePerConfrontoCatastoAsync(string partitaIva, DateTime dataInizio, DateTime dataFine, AgronicaCoreParametri objParametri, bool origine = true, bool ShowCatasto = false, bool showVarieta = false);
        Task<DataSet> GetExistsContributiACAAsync(AgronicaCoreParametri objParametri);


        /// <summary>
        /// Legge i dati relativi all'impianto e i relativi esercizi associati.
        /// </summary>
        Task<DataTable?> LeggiAsync(AgronicaCoreParametri objParametri, string piva = "", int saCod = 0, int appezza = 0, int idReg = 0, int progCod = -1);

        /// <summary>
        /// Legge i dati relativi all'impianto e ai suoi codici.
        /// </summary>
        Task<DataTable?> LeggiConCodiciAsync(AgronicaCoreParametri objParametri, string piva = "", int saCod = 0, int appezza = 0, int idReg = 0, int culCod = 0, int progCod = -1, int idCod = 0, string valCod = "");
        /// <summary>
        /// Mette in left join le tabelle Reg_Impianti, Cultivar, SpecieVegetali e GruppoVegetale.
        /// </summary>
        Task<DataTable?> LeggiInfoVarietaAsync(string piva, int saCod, int appezza, int idReg, AgronicaCoreParametri objParametri);
    }
}
