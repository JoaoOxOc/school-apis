using Entities.Base;
using Entities.Institution;
using Entities.Institution.ViewModel;
using InstitutionsService.Repositories;
using Utils.Interfaces;

namespace InstitutionsService.Services.UnitOfWork
{
    public interface IUnitOfWork
    {
        //IGenericRepository<V_Dashboard> DashboardRepository { get; }
        InstitutionsRepository InstitutionRepository { get; }
        IGenericRepository<InstitutionAddress> InstitutionAddressRepository { get; }
        IGenericRepository<Setting> SettingsRepository { get; }

        IGenericRepository<InstitutionAddressVM> InstitutionAddressVMRepository { get; }

        void Save();
    }
}
