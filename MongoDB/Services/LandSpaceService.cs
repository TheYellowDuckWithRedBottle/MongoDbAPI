using MongoDB.Driver;
using MongoDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDB.Services
{
    public class LandSpaceService
    {
        private readonly IMongoCollection<LandSpace> _LandSpace;
        public LandSpaceService()
        {

            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("SuQianDB");
            _LandSpace = database.GetCollection<LandSpace>("landSpace");
        }
        public List<LandSpace> GetAll() =>
             _LandSpace.Find(item => true).ToList();

    }
}
