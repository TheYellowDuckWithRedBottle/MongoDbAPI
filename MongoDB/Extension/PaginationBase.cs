using MongoDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDB.Extension
{
    public class PaginationBase
    {
        private int _pageSize = 5;
        public int PageIndex { get; set; } = 0;
        private int MaxPageSize { get; set; }=20;

        public int PageSize
        {
            get => _pageSize;//读_pagesize
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;//写入的size不能大于最大的100
        }
        public string OrderBy { get; set; } = nameof(Building.HouseHoldeID);

        public PaginationBase Clone()
        {
            PaginationBase paginationBase = new PaginationBase();
            paginationBase._pageSize = this._pageSize;
            paginationBase.PageIndex = this.PageIndex;
            paginationBase.MaxPageSize = this.PageIndex;
            return paginationBase;
        }
    }
}
