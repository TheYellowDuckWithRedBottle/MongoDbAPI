using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDB.Extension
{
    public class PaginatedList<T> :List<T> where T:class
    {
        public PaginationBase PaginationBase { get; }
        public int TotalItemsCount { get; set; }
        public bool HasPrevious => PaginationBase.PageIndex > 0;
        public bool HasNext => PaginationBase.PageIndex < PageCount - 1;
        public int PageCount => TotalItemsCount / PaginationBase.PageSize + (TotalItemsCount % PaginationBase.PageSize != 0 ? 1 : 0);
        public PaginatedList(int pageIndex,int pageSize,int totalItemCount,IEnumerable<T> data)
        {
            PaginationBase = new PaginationBase { PageSize = pageSize, PageIndex = pageIndex };
            TotalItemsCount = totalItemCount;
            AddRange(data);
        }
    }
}
