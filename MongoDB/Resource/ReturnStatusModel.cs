using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDB.Resource
{
    public class ReturnStatusModel
    {
        public string Status { get; set; }
        public string RoomId { get; set; }
        public string BuildingId { get; set; }
    }
    public class ListStatus
    {
        public string Status { get; set; }
        public List<ReturnStatusModel> RoomId { get; set; }
    }
}
