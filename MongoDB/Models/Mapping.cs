using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDB.Models
{
    [BsonIgnoreExtraElements]
    public class Mapping
    {
        public string RoomId { get; set; }
        public string NatbuildNo { get; set; }
        public string RealEstateUnitNo { get; set; }
    }
}
