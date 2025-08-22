using AgronicaCoreModelsSTD.attivita.dettagli;
using AgronicaNetCore.Base.Models;
using InData.Zoo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgronicaNetCore.Operazione.BIZ.Services.Agenda
{
    public interface IMagazzinoService
    {
        public Task<List<DettaglioRegistroSomministrazioni>> Leggi_Giacenze_Farmaci(
            LeggiGiacenzaFarmaci paramsFarmaci,
            AgronicaCoreModelsSTD.attivita.Attivita.Tipo_Attivita tipoAttivita,
            AgronicaCoreModelsSTD.attivita.Attivita.Stati statoAttivita,
            bool escludiGiacenzeZero,
            List<string> codiciAIC,
            AgronicaCoreParametri objParametriSuperServer, 
            AgronicaCoreParametri objParametriServer, 
            AgronicaCoreParametri objParametriUtenti);
    }
}
