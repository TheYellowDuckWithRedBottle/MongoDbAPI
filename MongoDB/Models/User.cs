using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDB.Models
{
   
    public class User
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string role { get; set; }
    }
}
