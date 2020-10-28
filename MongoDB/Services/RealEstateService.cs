using MongoDB.Driver;
using MongoDB.Extension;
using MongoDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDB.Services
{
    public class RealEstateService
    {
        private readonly IMongoCollection<Building> _Building;
        public RealEstateService()
        {

            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("SuQianDB");
            _Building = database.GetCollection<Building>("RealEstate");
        }
        /// <summary>
        /// 查找所有的不动产
        /// </summary>
        /// <returns>不动产列表</returns>
        public List<Building> GetAll() =>
             _Building.Find(building => true).ToList();

        public List<Building> FindList(string[] field = null)
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
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 根据查询参数获取某个建筑物
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Building GetOneRealEstate(QueryParameter query)
        {       
            FilterDefinition<Building> filter;//查询表达式
            var filterBuilder = Builders<Building>.Filter;//查询构建器
             filter = filterBuilder.Eq("NatbuildNo", query.NatbuildNo) & filterBuilder.Eq("RoomId",query.RoomId);
            var building = _Building.Find(filter).FirstOrDefault();
            return building;
        }
       
        public PaginatedList<Building> GetBuildings(QueryParameter parameter)
        {
            var query = _Building.AsQueryable().OrderBy(x => x.HouseHoldeID);
            var count = query.Count();

            var buildings = query
                .Skip(parameter.PageIndex * parameter.PageIndex)
                .Take(parameter.PageSize)

                .ToList();
            return new PaginatedList<Building>(parameter.PageIndex, parameter.PageSize, count, buildings);

        }
        public Building Get(string HouseholdID)
        {
            return _Building.Find<Building>(building => building.HouseHoldeID == HouseholdID || building.EstateUnitNo == HouseholdID).FirstOrDefault();
        }

        /// <summary>
        /// 新建一个建筑物
        /// </summary>
        /// <param name="building"></param>
        /// <returns></returns>
        public Building Create(Building building)
        {
            _Building.InsertOne(building);
            return building;
        }
        /// <summary>
        ///上传多个信息
        /// </summary>
        /// <param name="buildings"></param>
        public void CreateMany(IEnumerable<Building> buildings)
        {
            _Building.InsertMany(buildings);
        }
        public void Update(string EstateUnitNo, Building building) =>
            _Building.ReplaceOne(building => building.EstateUnitNo == EstateUnitNo, building);
        public void Remove(Building building) =>
            _Building.DeleteOne(building => building.EstateUnitNo == building.EstateUnitNo);
        public void Remove(string EstateUnitNo) =>
            _Building.DeleteOne(building => building.EstateUnitNo == EstateUnitNo);

    }
}

