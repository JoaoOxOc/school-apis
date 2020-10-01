using LoggingService.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LoggingService.Repositories
{
    public interface ILogRepository
    {
        Task<IEnumerable<Log>> Get();

        IEnumerable<Log> GetFiltered(string searchText, int? activityType, string companyId, int? logType, DateTime? startDate, DateTime? endDate, int? offset, int? limit, string sort, string sortDirection);
        
        int Count(string searchText, int? activityType, string companyId, int? logType, DateTime? startDate, DateTime? endDate, int? offset, int? limit, string sort);

        Task<Log> GetById(object id);

        Task Add(Log entity);

        Task<bool> Delete(string id);
    }
}
