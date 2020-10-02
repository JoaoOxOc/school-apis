using Entities.Base;
using LoggingService.Models;
using LoggingService.Services.UnitOfWork;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Utils.Enums;

namespace LoggingService.Repositories
{
    public class LogRepository : ILogRepository
    {
        private readonly DatabaseContext context = null;

        public LogRepository(IOptions<Configurations> configs)
        {
            this.context = new DatabaseContext(configs);
        }

        public async Task<IEnumerable<Log>> Get()
        {
            try
            {
                return await context.Logs.Find(_ => true).ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public IEnumerable<Log> GetFiltered(string searchText, int? activityType, string companyId, int? logType, DateTime? startDate, DateTime? endDate, int? offset, int? limit, string sort, string sortDirection)
        {
            try
            {
                searchText = searchText == null ? string.Empty : searchText.Trim();

                IQueryable<Log> query = context.Logs.AsQueryable();
                 
                 query = query.Where(x => x.Username.Contains(searchText) || x.Message.Contains(searchText));

                if (companyId != null && companyId.Length > 0)
                {
                    query = query.Where(x => x.CompanyId == companyId);
                }

                if (logType != null)
                {
                    query = query.Where(x => x.Type == logType.Value);
                }

                if (activityType != null)
                {
                    query = query.Where(x => x.ActivityType == activityType.Value);
                }

                if (startDate != null)
                {
                    query = query.Where(x => x.Date >= startDate.Value.Date);
                }

                if (endDate != null)
                {
                    query = query.Where(x => x.Date < endDate.Value.Date.AddDays(1));
                }

                //SortDefinition<Log> sortFilter;
                
                switch(sort)
                {
                    case "username":
                        query = sortDirection == "asc" ? query.OrderBy(x => x.Username) : query.OrderByDescending(x => x.Username);
                    break;
                    case "id":
                        query = sortDirection == "asc" ? query.OrderBy(x => x.Id) : query.OrderByDescending(x => x.Id);
                        break;
                    case "type":
                        query = sortDirection == "asc" ? query.OrderBy(x => x.Type) : query.OrderByDescending(x => x.Type);
                        break;
                    default:
                        query = sortDirection == "asc" ? query.OrderBy(x => x.Date) : query.OrderByDescending(x => x.Date);
                    break;
                }

                
                
                if (offset.HasValue && limit.HasValue)
                {
                    query = query.Skip((offset.Value - 1) * limit.Value).Take(limit.Value);
                }

                List<Log> logsList = query.ToList();

                return logsList;

            }
            catch(Exception ex)
            {
                throw ex;
            }
        }


        public int Count(string searchText, int? activityType, string companyId, int? logType, DateTime? startDate, DateTime? endDate, int? offset, int? limit, string sort)
        {
            try
            {
                searchText = searchText == null ? string.Empty : searchText.Trim();

                IQueryable<Log> query = context.Logs.AsQueryable();

                query = query.Where(x => x.Username.Contains(searchText) || x.Message.Contains(searchText));

                if (companyId != null)
                {
                    query = query.Where(x => x.CompanyId == companyId);
                }

                if (logType != null)
                {
                    query = query.Where(x => x.Type == logType.Value);
                }

                if (activityType != null)
                {
                    query = query.Where(x => x.ActivityType == activityType.Value);
                }

                if (startDate != null)
                {
                    query = query.Where(x => x.Date >= startDate.Value.Date);
                }

                if (endDate != null)
                {
                    query = query.Where(x => x.Date < endDate.Value.Date.AddDays(1));
                }
                
                List<Log> logsList = query.ToList();

                return logsList.Count();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Log> GetById(object id)
        {
            var filter = Builders<Log>.Filter.Eq("Id", id);

            try
            {
                return await context.Logs.Find(filter).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task Add(Log entity)
        {
            try
            {
                await this.context.Logs.InsertOneAsync(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> Delete(string id)
        {
            try
            {
                DeleteResult actionResult = await this.context.Logs.DeleteOneAsync(Builders<Log>.Filter.Eq("Id", id));

                return actionResult.IsAcknowledged && actionResult.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
}
