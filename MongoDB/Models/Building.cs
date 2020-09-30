using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;

namespace MongoDB.Models
{

    [BsonIgnoreExtraElements]
    public class Building
    {   
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
       [JsonProperty("ID")]
        public string Id { get; set; }
        [JsonProperty("户ID")]

        public string HouseholdID { get; set; }
        [JsonProperty("原户编号")]
        public string OrgID { get; set; }
        [JsonProperty("不动产单元号")]
        public string EstateUnitNo { get; set; }
        [JsonProperty("户号ID")]
        public string HouseHoldeID { get; set; }
        [JsonProperty("自然幢号")]

        public string NatbuildNo { get; set; }
        [JsonProperty("逻辑幢号")]
        public string LogicBuildNo { get; set; }
        [JsonProperty("户号")]
        public int CoverId { get; set; }
        [JsonProperty("单元号")]
        public int UnitId { get; set; }
        [JsonProperty("实际层号")]
        public double FloLayerId { get; set; }
        [JsonProperty("名义层号")]
        public double NameId { get; set; }
        [JsonProperty("房间号")]
        public string RoomId { get; set;}
        [JsonProperty("单元名称")]
        public string UnitName { get; set; }
        [JsonProperty("房屋用途")]
        public string HouseUsage { get; set; }
        [JsonProperty("房屋权利类型")]
        public string PowerType { get; set; }
        [JsonProperty("房屋权利性质")]
        public string RomPowCharacter { get; set; }
        [JsonProperty("户型结构")]
        public string RomTyeStru { get; set; }
        [JsonProperty("户型")]
        public string CoverType { get; set; }
        [JsonProperty("预测建筑面积(㎡)")]
        public double PreBuilArea { get; set; }
        [JsonProperty("预测套内建筑面积(㎡)")]
        public double PreInterArea { get; set; }
        [JsonProperty("预测分摊建筑面积(㎡)")]
        public double PreSharedArea { get; set; }
        [JsonProperty("预测地下建筑面积(㎡)")]
        public string PreUndgrondArea { get; set; }
        [JsonProperty("预测其他建筑面积(㎡)")]
        public string PreOtherArea { get; set; }
        [JsonProperty("实测建筑面积(㎡)")]
        public double AreaBuilding { get; set; }
        [JsonProperty("实测套内建筑面积(㎡)")]

        public double AreaInterior { get; set; }
        [JsonProperty("实测分摊建筑面积(㎡)")]

        public double AreaShared { get; set; }
        [JsonProperty("实测地下建筑面积(㎡)")]
        public string AreaRealUnderground { get; set; }
        [JsonProperty("实测其他建筑面积(㎡)")]
        public string AreaRealOther { get; set; }
        [JsonProperty("取得方式")]

        public string MethodGet { get; set; }
        [JsonProperty("土地期限")]
        public string LandTenure { get; set; }
        [JsonProperty("土地起始日期")]
        public string LandStartDate { get; set; }
        [JsonProperty("土地终止日期")]
        public string LandEndDate { get; set; }
        [JsonProperty("土地用途")]
        public string UsageLand { get; set; }
        [JsonProperty("权利归属")]
        public string PowerBelong { get; set; }
        [JsonProperty("独用土地面积")]
        public string ExclluLand { get; set; }
        [JsonProperty("分摊土地面积")]
        public double AppLandArea { get; set; }
        [JsonProperty("房间坐落")]
        public string LocationRoom { get; set; }
        [JsonProperty("测绘状态")]
        public string MappingStatus { get; set; }
        [JsonProperty("附加说明")]
        public string AttachRemark { get; set; }
        [JsonProperty("是否可登记")]
        public bool IsRegister { get; set; }
        [JsonProperty("建筑类型")]
        public string TypeBuilding { get; set; }
        [JsonProperty("土地权利性质")]
        public string ProperLand { get; set; }
        public string GetValue(string name)
        {
            return Convert.ToString(this.GetType().GetProperty(name).GetValue(this, null));
        }

    }
}
