using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.WEB08.PNNHAI.Core
{
    public class FilterResult<TModel> : BaseFilterResult
    {
        public IEnumerable<TModel>? Data { get; set; }
        public FilterResult(int currentPage, int pageSize, int totalPage, int totalRecords, IEnumerable<TModel> data)
            : base(currentPage, pageSize, totalPage, totalRecords)
        {
            Data = new List<TModel>();
            Data = data;
        }
    }
}
