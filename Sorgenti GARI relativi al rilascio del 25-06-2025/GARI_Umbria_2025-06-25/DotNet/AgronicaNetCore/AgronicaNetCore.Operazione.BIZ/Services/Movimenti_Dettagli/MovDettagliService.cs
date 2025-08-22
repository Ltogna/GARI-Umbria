using InData.Agenda;
using System.Transactions;
using AgronicaNetCore.Base.Models;
using Microsoft.Extensions.DependencyInjection;
using AgronicaNetCore.Operazione.DAL.DataLayer.Movimenti_Dettagli;
using AgronicaNetCore.UtilityDB.DAL.DataLayer.Agro_Sequenze;
using Microsoft.Extensions.Localization;
using AgronicaNetCore.Operazione.BIZ.Resources;
using InData.Anagrafica;
using AgronicaNetCore.Base.Constants;
using AgronicaNetCore.Operazione.BIZ.Services.Mov_Destinazioni;

namespace AgronicaNetCore.Operazione.BIZ.Services.Movimenti_Dettagli
{
    public class MovDettagliService : BaseServiceOperazioneBIZ, IMovDettagliService
    {
        private readonly IMovimenti_Dettagli _movDettagliDal;
        private readonly IMovDestinazioniService _movDestinazioni;
        private readonly IAgro_Sequence _sequenceDal;

        public MovDettagliService(IServiceProvider provider, IStringLocalizer<Messages> localizer) : base(provider, localizer)
        {
            _movDettagliDal = provider.GetRequiredService<IMovimenti_Dettagli>();
            _sequenceDal = provider.GetRequiredService<IAgro_Sequence>();
            _movDestinazioni = provider.GetRequiredService<IMovDestinazioniService>();
        }


        public async Task ScriviMovimentoDettaglioAsync(Movimento_Dettaglio movDett, AgronicaCoreParametri objServer, bool documentoPrevisionale = false, bool usaDataModifica = false)
        {
            // TODO aggiorna materie prime
            int idMovDet = movDett.Id_Mov_Det;
            if (idMovDet <= 0)
            {
                idMovDet = await _sequenceDal.NuovoId_TabellaAsync("Movimenti_Dettagli", movDett.BaseCode, movDett.TopCode, objServer);
            }
            movDett.Id_Mov_Det = idMovDet;

            if (movDett.Data > DateTime.Now && !documentoPrevisionale)
                movDett.Contabilizzato = -Math.Abs(movDett.Contabilizzato);
            movDett.Data_Modifica = usaDataModifica ? movDett.Data_Modifica : new DateTime(1900,1,2);

            await _movDettagliDal.CreateAsync(movDett.ToWriteMovDettagli(), objServer);
        
            // TODO write dettagli riferiti
            // TODO write dettagli riferimenti
            // TODO write dettagli tecnici
            // TODO write dettagli tecnici extra
            // TODO write dettagli conferimento
            // TODO write dettagli destinazione

            if (movDett.Movimenti_Destinazioni.Any())
            {
                foreach (var dest in movDett.Movimenti_Destinazioni)
                {
                    dest.Id_Agenda = movDett.Id_Agenda;
                    dest.Id_Mov = movDett.Id_Mov;
                    dest.Id_Mov_Det = movDett.Id_Mov_Det;
                    await _movDestinazioni.ScriviMovimentoDestinazioneAsync(dest, objServer);
                }
            }
        }

        public async Task<int> ScriviModificaAsync(WriteMovDettagli dtoMovDettagli, AgronicaCoreParametri objP)
        {
            try
            {
                bool isNew = false;

                if (dtoMovDettagli.Id_Mov_Det == 0)
                {
                    dtoMovDettagli.Id_Mov_Det = await _sequenceDal.NuovoId_TabellaAsync("movimenti_dettagli", 0, 2000000000, objP);
                    isNew = true;
                }
                else
                    isNew = !await _movDettagliDal.ExistAsync(dtoMovDettagli.Id_Mov_Det, objP);

                if (isNew)
                    await _movDettagliDal.CreateAsync(dtoMovDettagli, objP);
                else
                    await _movDettagliDal.UpdateAsync(dtoMovDettagli, objP);

                return dtoMovDettagli.Id_Mov_Det;
            }
            catch (Exception ex)
            {
                LogError(ex.Message, objP, ex);
                throw;
            }
        }

        public async Task<bool> EliminaAsync(WriteMovDettagli dtoMovDettagli, AgronicaCoreParametri objP)
        {
            try
            {
                return await _movDettagliDal.DeleteAsync(dtoMovDettagli.Piva, dtoMovDettagli.Id_Agenda, dtoMovDettagli.Id_Mov, dtoMovDettagli.Id_Mov_Det, objP);
            }
            catch (Exception ex)
            {
                LogError(ex.Message, objP, ex);
                throw;
            }
        }

        public async Task<bool> EliminaAsync(List<WriteMovDettagli> MovDettagli, AgronicaCoreParametri objP)
        {
            using (TransactionScope ts = new(TransactionScopeOption.RequiresNew, new TimeSpan(0, 10, 0)))
            {
                try
                {
                    foreach (var dto in MovDettagli)
                    {
                        await _movDettagliDal.DeleteAsync(dto.Piva, dto.Id_Agenda, dto.Id_Mov, dto.Id_Mov_Det, objP);
                    }
                    ts.Complete();
                }
                catch (Exception ex)
                {
                    LogError(ex.Message, objP, ex);
                    throw;
                }
                finally
                {
                    ts.Dispose();
                }
            }

            return true;
        }
    }
}
