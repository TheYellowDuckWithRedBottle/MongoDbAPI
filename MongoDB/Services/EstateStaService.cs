using BooksApi.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MongoDB.Extension;
using MongoDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDB.Services
{
    public class EstateStaService
    {
        private readonly IMongoCollection<Building> _Building;
        public EstateStaService(IConfiguration configuration)
        {

           var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("png");
            _Building = database.GetCollection<Building>("OfficeBuilding");
        }

        public List<Building> Get() =>
             _Building.Find(building => true).ToList();
        public Building Get(QueryParameter query)
        {
            Building building = null;
            if (query.EstateUnitNo!=null)
            {
                building= _Building.Find<Building>(building => building.EstateUnitNo == query.EstateUnitNo).FirstOrDefault();
            }

            building= _Building.Find<Building>(building => building.HouseHoldeID == query.HouseHoldeID).FirstOrDefault();
            return building;
        }
       public List<Building> FindList (string[] field=null)
        {
            try
            {
                var fieldList = new List<ProjectionDefinition<Building>>();
                foreach (var item in field)
                {
                    fieldList.Add(Builders<Building>.Projection.Include(item.ToString()));
                }
                var projection = Builders<Building>.Projection.Combine(fieldList);
                fieldList?.Clear();

                return _Building.Find(build => true).Project<Building>(projection).ToList();
            } 
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public  PaginatedList<Building> GetBuildings(QueryParameter parameter)
        {
            var query = _Building.AsQueryable().OrderBy(x => x.HouseHoldeID);
            var count = query.Count();
                      
            var buildings= query
                .Skip(parameter.PageIndex * parameter.PageIndex)
                .Take(parameter.PageSize)
                
                .ToList();
            return new PaginatedList<Building>(parameter.PageIndex, parameter.PageSize, count,buildings);
            
        }
        public Building Get(string HouseholdID)
        {
           return  _Building.Find<Building>(building => building.HouseHoldeID == HouseholdID||building.EstateUnitNo==HouseholdID).FirstOrDefault();
        }
           
       
        public Building Create(Building building)   
        {
            _Building.InsertOne(building);
            return building;
        }
        public void Update(string EstateUnitNo, Building building) =>
            _Building.ReplaceOne(building => building.EstateUnitNo == EstateUnitNo, building);
        public void Remove(Building building) =>
            _Building.DeleteOne(building => building.EstateUnitNo == building.EstateUnitNo);
        public void Remove(string EstateUnitNo) =>
            _Building.DeleteOne(building => building.EstateUnitNo == EstateUnitNo);

    }
}
