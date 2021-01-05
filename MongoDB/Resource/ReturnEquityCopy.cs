using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDB.Resource
{
    public class ReturnEquityCopy
    {
        [JsonProperty("不动产单元号")]
        public string RealEstateNo { get; set; }
       
        [JsonProperty(PropertyName = "不动产证明号")]
        public string TitleNo { get; set; }
        [JsonProperty("预告权利人")]
        public string Obligee { get; set; }
        [JsonProperty("坐落")]
        public string Location { get; set; }
        [JsonProperty("所在层")]
        public string Layer { get; set; }
        [JsonProperty("面积")]
        public string BuildingArea { get; set; }
        [JsonProperty("规划用途")]
        public string PlanUsage { get; set; }

        [JsonProperty("房屋性质")]
        public string RoomProperty { get; set; }
        [JsonProperty("权利类型")]
        public string PowerType { get; set; }
        [JsonProperty("是否抵押")]
        public string IsMortgage { get; set; }
        [JsonProperty("是否查封")]
        public string IsSealed { get; set; }

    }
}
