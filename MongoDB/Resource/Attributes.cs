using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDB.Resource
{
    public class Attributes
    {
        //[BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
     
        public string Id { get; set; }
        public string name;
        public string cname { get; set; }
        public bool isShow { get; set; }
    }
}
