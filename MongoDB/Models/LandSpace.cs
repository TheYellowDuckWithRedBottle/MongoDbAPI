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
    public class LandSpace
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonProperty("ID")]
        public string Id { get; set; }

        [JsonProperty("标识码")]
        public string BSM  { get; set; }
        [JsonProperty("要素代码")]
        public string YSDM { get; set; }
        [JsonProperty("图斑编号")]
        public string TBBH        { get; set; }
        [JsonProperty("自然资源分类编码")]
        public string ZRZYFLBM        { get; set; }
        [JsonProperty("自然资源分类名称")]
        public string ZRZYFLMC        { get; set; }
        [JsonProperty("权属性质")]
        public string QSXZ     { get; set; }
        [JsonProperty("权属单位代码")]
        public string QSDWDM        { get; set; }
        [JsonProperty("权属单位名称")]
        public string QSDWMC        { get; set; }
        [JsonProperty("坐落单位名称")]
        public string ZLDWMC        { get; set; }
        [JsonProperty("描述说明")]
        public string MSSM        { get; set; }
        [JsonProperty("海岛名称")]
        public string HDMC        { get; set; }
        [JsonProperty("数据年份")]
        public double SJNF { get; set; }
        [JsonProperty("备注")]
        public string BZ { get; set; }
        [JsonProperty("SHAPE_Length")]
        public double SHAPE_Length { get; set; }
        [JsonProperty("SHAPE_Area")]
        public double SHAPE_Area { get; set; }

        [JsonProperty("经度")]
        public double longitude{ get; set; }

        [JsonProperty("纬度")]
        public double latitude{ get; set; }

    }
}
