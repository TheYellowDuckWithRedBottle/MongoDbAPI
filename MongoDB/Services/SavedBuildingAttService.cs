using MongoDB.Driver;
using MongoDB.Extension;
using MongoDB.Models;
using MongoDB.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDB.Services
{
    public class SavedBuildingAttService
    {
        private readonly IMongoCollection<Attributes> _Attribute;
        public SavedBuildingAttService()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("png");
            _Attribute = database.GetCollection<Attributes>("Attributes");
        }
        //https://github.com/TheYellowDuckWithRedBottle/MongoDbAPI.git
        public List<Attributes> Get() => _Attribute.Find(attribute => true).ToList();
        public Attributes Get(string name)
        {
            Attributes attributes = null;
            if (name != "")
            {
                attributes = _Attribute.Find<Attributes>(attributes => attributes.name == name).FirstOrDefault();
            }
            return attributes;
        }
        public long Update(string name,Attributes attributes)
        {
            try
            {
                FilterDefinition<Attributes> filter = Builders<Attributes>.Filter.Eq("name", name);
                var list = new List<UpdateDefinition<Attributes>>();
                var properties = typeof(Attributes).GetProperties();
                foreach (var item in properties)
                {
                    //if (item.Name.ToLower() == "name") continue;
                    var value = item.GetValue(attributes);
                    list.Add(Builders<Attributes>.Update.Set(item.Name, value));
                }
              
                var updateFilter = Builders<Attributes>.Update.Combine(list);
                return _Attribute.UpdateOne(filter, updateFilter).MatchedCount;

            }
          catch(Exception ex)
            {
                throw ex;
            }
            
        }
        public Attributes Create(Attributes  attribute)
        {
            _Attribute.InsertOne(attribute);
            return attribute;
        }
        public void Remove(string name) =>
            _Attribute.DeleteOne(attribute => attribute.name == name);
            
    }

        
    
}
