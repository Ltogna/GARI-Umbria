using InData.Agenda;
using System.Transactions;
using AgronicaNetCore.Base.Models;
using Microsoft.Extensions.DependencyInjection;
using AgronicaNetCore.Operazione.DAL.DataLayer.Movimenti_Zoo;
using Microsoft.Extensions.Localization;
using AgronicaNetCore.Operazione.BIZ.Resources;

namespace AgronicaNetCore.Operazione.BIZ.Services.Movimenti_Zoo
{
    public class MovZooService : BaseServiceOperazioneBIZ //, IMovZooService
    {
        private readonly IMovimenti_Zoo _movZooDal;

        public MovZooService(IServiceProvider provider, IStringLocalizer<Messages> localizer) : base(provider, localizer)
        {
            _movZooDal = provider.GetRequiredService<IMovimenti_Zoo>();
        }

        public async Task<bool> ScriviModificaAsync(WriteMovimentiZoo dtoMovZoo, AgronicaCoreParametri objP)
        {
            try
            {
                bool isNew = !await _movZooDal.ExistAsync(dtoMovZoo.Piva, dtoMovZoo.Id_Agenda, dtoMovZoo.Id_Mov, objP);

                if (isNew)
                    await _movZooDal.CreateAsync(dtoMovZoo, objP);
                else
                    await _movZooDal.UpdateAsync(dtoMovZoo, objP);

                return true;
            }
            catch (Exception ex)
            {
                LogError(ex.Message, objP, ex);
                throw;
            }
        }

        public async Task<bool> EliminaAsync(WriteMovimentiZoo dtoMovZoo, AgronicaCoreParametri objP)
        {
            try
            {
                return await _movZooDal.DeleteAsync(dtoMovZoo.Piva, dtoMovZoo.Id_Agenda, dtoMovZoo.Id_Mov, objP);
            }
            catch (Exception ex)
            {
                LogError(ex.Message, objP, ex);
                throw;
            }
        }

        public async Task<bool> EliminaAsync(List<WriteMovimentiZoo> MovZoo, AgronicaCoreParametri objP)
        {
            using (TransactionScope ts = new(TransactionScopeOption.RequiresNew, new TimeSpan(0, 10, 0)))
            {
                try
                {
                    foreach (var dto in MovZoo)
                    {
                        await _movZooDal.DeleteAsync(dto.Piva, dto.Id_Agenda, dto.Id_Mov, objP);
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
