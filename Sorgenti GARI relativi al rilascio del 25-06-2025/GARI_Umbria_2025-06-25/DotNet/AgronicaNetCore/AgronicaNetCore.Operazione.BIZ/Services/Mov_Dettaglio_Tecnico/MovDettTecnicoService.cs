using InData.Agenda;
using AgronicaNetCore.Base.Models;
using AgronicaNetCore.Operazione.DAL.DataLayer.Mov_Dettaglio_Tecnico;
using Microsoft.Extensions.DependencyInjection;
using AgronicaNetCore.UtilityDB.DAL.DataLayer.Agro_Sequenze;
using Microsoft.Extensions.Localization;
using AgronicaNetCore.Operazione.BIZ.Resources;

namespace AgronicaNetCore.Operazione.BIZ.Services.Mov_Dettaglio_Tecnico
{
    public class MovDettTecnicoService : BaseServiceOperazioneBIZ   //, IMovDettTecnicoService
    {
        private readonly IMov_Dettaglio_Tecnico _movDettTecDal;
        private readonly IAgro_Sequence _sequenceDal;

        public MovDettTecnicoService(IServiceProvider provider, IStringLocalizer<Messages> localizer) : base(provider, localizer)
        {
            _movDettTecDal = provider.GetRequiredService<IMov_Dettaglio_Tecnico>();
            _sequenceDal = provider.GetRequiredService<IAgro_Sequence>();
        }

        public async Task<int> ScriviModificaAsync(WriteMovDettTecnico dtoMovDettTec, AgronicaCoreParametri objP)
        {
            try
            {
                bool isNew = false;

                if (dtoMovDettTec.Id_Mov_Det == 0)
                {
                    dtoMovDettTec.Id_Mov_Det = await _sequenceDal.NuovoId_TabellaAsync("movimenti_dettagli_tecnici", 0, 2000000000, objP);
                    isNew = true;
                }
                else
                    isNew = !await _movDettTecDal.ExistAsync(dtoMovDettTec.Piva, dtoMovDettTec.Id_Agenda, dtoMovDettTec.Id_Mov, dtoMovDettTec.Id_Mov_Det, dtoMovDettTec.Id_Reg_Dettaglio, objP);

                if (isNew)
                    await _movDettTecDal.CreateAsync(dtoMovDettTec, objP);
                else
                    await _movDettTecDal.UpdateAsync(dtoMovDettTec, objP);

                return dtoMovDettTec.Id_Reg_Dettaglio;
            }
            catch (Exception ex)
            {
                LogError(ex.Message, objP, ex);
                throw;
            }
        }

        public async Task<bool> EliminaAsync(WriteMovDettTecnico dtoMovDettTec, AgronicaCoreParametri objP)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> EliminaAsync(List<WriteMovDettTecnico> MovDettTec, AgronicaCoreParametri objP)
        {
            throw new NotImplementedException();
        }
    }
}
