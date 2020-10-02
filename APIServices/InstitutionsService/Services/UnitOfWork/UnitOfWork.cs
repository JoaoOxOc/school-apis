using Entities.Base;
using Entities.Institution;
using Entities.Institution.ViewModel;
using InstitutionsService.Repositories;
using Utils.Interfaces;

namespace InstitutionsService.Services.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DatabaseContext dbContext;
        //private IGenericRepository<V_Dashboard> _dashboardRepository;
        private InstitutionsRepository _institutionRepository;
        private IGenericRepository<InstitutionAddress> _institutionAddressRepository;
        private IGenericRepository<Setting> _settingsRepository;
        private IGenericRepository<InstitutionAddressVM> _institutionAddressVMRepository;

        //public IGenericRepository<V_Dashboard> DashboardRepository
        //{
        //    get
        //    {
        //        return _dashboardRepository = _dashboardRepository ?? new GenericRepository<V_Dashboard>(dbContext);
        //    }
        //}

        public InstitutionsRepository InstitutionRepository
        {
            get
            {
                return _institutionRepository = _institutionRepository ?? new InstitutionsRepository(dbContext);
            }
        }
        public IGenericRepository<InstitutionAddress> InstitutionAddressRepository
        {
            get
            {
                return _institutionAddressRepository = _institutionAddressRepository ?? new GenericRepository<InstitutionAddress>(dbContext);
            }
        }


        public IGenericRepository<Setting> SettingsRepository
        {
            get
            {
                return _settingsRepository = _settingsRepository ?? new GenericRepository<Setting>(dbContext);
            }
        }


        public IGenericRepository<InstitutionAddressVM> InstitutionAddressVMRepository
        {
            get
            {
                return _institutionAddressVMRepository = _institutionAddressVMRepository ?? new GenericRepository<InstitutionAddressVM>(dbContext);
            }
        }


        public UnitOfWork(DatabaseContext context)
        {
            dbContext = context;
        }

        public void Save()
        {
            dbContext.SaveChanges();
        }

        public void Dispose()
        {
            dbContext.Dispose();
        }
    }
}
