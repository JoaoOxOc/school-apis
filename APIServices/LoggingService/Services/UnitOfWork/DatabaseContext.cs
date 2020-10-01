using Entities.Base;
using LoggingService.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoggingService.Services.UnitOfWork
{
    public class DatabaseContext
    {
        private readonly IMongoDatabase _database = null;

        public DatabaseContext(IOptions<Configurations> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            if (client != null)
            {
                _database = client.GetDatabase(settings.Value.Database);
            }
        }

        public IMongoCollection<Log> Logs
        {
            get
            {
                return _database.GetCollection<Log>("Log");
            }
        }

    }
}
