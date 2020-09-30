
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDB.Models
{
    public class Tile
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string x { get; set; }
        public string y { get; set; }
        public string z { get; set; }
           
        public byte[] img { get; set; }
    }
}
