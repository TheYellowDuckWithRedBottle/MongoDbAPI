using Microsoft.Extensions.Configuration;
using MongoDB.Common;
using MongoDB.Driver;
using MongoDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDB.Services
{
    public class EquityService
    {
        private readonly IMongoCollection<Equity> _Equity;
        public EquityService(IConfiguration configuration)
        {
           var ConnectString= configuration.GetConnectionString("DefaultConnection");
           var DBName = configuration.GetConnectionString("DatabaseName");
           var client = new MongoClient(ConnectString);
           var database = client.GetDatabase(DBName);
           _Equity = database.GetCollection<Equity>("Equity");
        }

        public List<Equity> Get() =>
             _Equity.Find(equity => true).ToList();
        public Equity GetOne(QueryParameter query)
        {
            Equity equity = null;
            if (query.EstateUnitNo != null)
            {
                equity = _Equity.Find<Equity>(equity => equity.RealEstateNo == query.EstateUnitNo).FirstOrDefault();
            }
            return equity;
        }
    }
}
