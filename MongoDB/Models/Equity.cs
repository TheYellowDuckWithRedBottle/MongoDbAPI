using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDB.Models
{
    [BsonIgnoreExtraElements]
    public class Equity
    {
        
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonProperty("ID")]
        public string Id { get; set; }
        [JsonProperty("不动产单元号")]
        public string RealEstateNo { get; set; }
        [JsonProperty("房间号")]
        public string HouseHoldID { get; set; }
        [JsonProperty("单元号")]
        public string UnitId  { get; set; }
        [JsonProperty("坐落")]
        public string Location { get; set; }
        [JsonProperty("规划用途")]
        public string PlanUsage        { get; set; }
        [JsonProperty("所在层")]
        public string Layer        { get; set; }
        [JsonProperty("名义层")]
        public string NameLayer        { get; set; }
        [JsonProperty("房屋类型")]
        public string PropertyType        { get; set; }
        [JsonProperty("房屋性质")]
        public string RoomProperty        { get; set; }
        [JsonProperty("是否登记")]
        public string IsRegister { get; set; }
        [JsonProperty("不动产证号")]
        public string TitleNo        { get; set; }
        [JsonProperty("权利类型")]
        public string PowerType        { get; set; }
        [JsonProperty("建筑面积")]
        public double BuildingArea        { get; set; }
        [JsonProperty("登记类型")]
        public string RegisterType        { get; set; }
        [JsonProperty("权利人")]
        public string Obligee        { get; set; }
        [JsonProperty("是否预告")]
        public string IsForecast        { get; set; }
        [JsonProperty("面积")]
        public string Area        { get; set; }
        [JsonProperty("类型")]
        public string RegisterType1        { get; set; }
        [JsonProperty("预告证明号")]
        public string ForestRealEstateNo { get; set; }

        [JsonProperty("预告权利人 ")]
        public string ForestPowerPerson        { get; set; }
        [JsonProperty("是否抵押")]
        public string IsMortgage        { get; set; }
        [JsonProperty("抵押方式")]
        public string MortgageWay        { get; set; }

        [JsonProperty("债权履行期限")]
        public string deadline        { get; set; }
        [JsonProperty("抵押金额/万元")]
        public double? pawnAmount        { get; set; }
        [JsonProperty("抵押权人")]
        public string mortgagee        { get; set; }
        [JsonProperty("抵押人")]
        public string mortgager        { get; set; }
        [JsonProperty("是否查封")]
        public string IsSealed        { get; set; }

    }
}
