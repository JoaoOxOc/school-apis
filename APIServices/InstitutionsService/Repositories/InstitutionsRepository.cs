using Entities.Institution;
using Entities.Institution.ViewModel;
using InstitutionsService.Services.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Utils.Enums;
using Utils.Extensions;

namespace InstitutionsService.Repositories
{
    public class InstitutionsRepository : GenericRepository<Institution>
    {
        internal new DatabaseContext context;

        public InstitutionsRepository(DatabaseContext context) : base(context)
        {
            this.context = context;
        }

        public List<InstitutionAddressVM> getInstitutionsWithAddresses(int? pageNum = null,
            int? size = null, Expression<Func<Institution, bool>> filter = null,
            string orderBy = null,
            SortDirection sortDirection = SortDirection.Ascending,
            string includeProperties = "", bool noTracking = false)
        {
            var institutionsQuery = (from inst in context.Institution select inst);
            if (filter != null)
            {
                institutionsQuery = institutionsQuery.Where(filter);
            }
            if (!string.IsNullOrEmpty(orderBy))
            {
                institutionsQuery = institutionsQuery.OrderBy(orderBy, sortDirection);
            }
            else
            {
                institutionsQuery = institutionsQuery.OrderBy("Id", SortDirection.Ascending);
            }
            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                institutionsQuery = institutionsQuery.Include(includeProperty.Trim());
            }

            if (pageNum.HasValue && size.HasValue)
            {
                institutionsQuery = institutionsQuery.Skip((pageNum.Value - 1) * size.Value).Take(size.Value);
            }
            var institutionsWithAddressesQuery = institutionsQuery.Join(context.InstitutionAddress,
                inst => inst.Id,
                instAddresses => instAddresses.InstitutionId,
                (inst, instAddresses) => new InstitutionAddressVM
                                  {
                                      InstitutionId = inst.Id,
                                      AddressId = instAddresses.Id,
                                      InstitutionName = inst.Name,
                                      StreetAddress = instAddresses.StreetAddress,
                                      City = instAddresses.City,
                                      PostalCode = instAddresses.PostalCode,
                                      Province = instAddresses.Province,
                                      Region = instAddresses.Region,
                                      Country = instAddresses.Country,
                                      Latitude = instAddresses.Latitude,
                                      Longitude = instAddresses.Longitude
                                  });


            if (noTracking)
                return institutionsWithAddressesQuery.AsNoTracking().ToList();
            else
                return institutionsWithAddressesQuery.ToList();
        }
    }
}
