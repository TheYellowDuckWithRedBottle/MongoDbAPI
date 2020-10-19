using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDB.Resource
{
    public class BuildingResource
    {

        [JsonProperty("不动产单元号")]
        public string EstateUnitNo { get; set; } = null;
        [JsonProperty("幢号")]
        public string NatbuildNo { get; set; } = null;
        [JsonProperty("层号")]
        public int FloLayerId { get; set; } = 0;

        [JsonProperty("户号")]
        public int CoverId { get; set; } = 0;
        [JsonProperty("房屋面积")]
        public double AreaBuilding { get; set; } = 0;

        [JsonProperty("套内面积")]
        public double AreaInterior { get; set; } = 0;

        [JsonProperty("分摊面积")]
        public double AreaShared { get; set; } = 0;
        [JsonProperty("土地权利性质")]
        public string ProperLand { get; set; } = null;
        [JsonProperty("建筑类型")]
        public string TypeBuilding { get; set; } = null;
        [JsonProperty("房屋坐落")]
        public string LocationRoom { get; set; } = null;

    }
}
