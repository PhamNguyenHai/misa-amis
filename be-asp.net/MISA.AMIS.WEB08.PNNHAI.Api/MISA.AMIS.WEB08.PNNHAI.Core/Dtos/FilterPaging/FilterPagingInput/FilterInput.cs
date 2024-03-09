using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.WEB08.PNNHAI.Core
{
    public class FilterInput
    {
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SearchString { get; set; }
        public string? SortColumn { get; set; }
        public bool? IsSortDesc { get; set; }
        public IEnumerable<FilterColumn>? FilterColumns { get; set; }

        public FilterInput() 
        {
            FilterColumns = new List<FilterColumn>();
        }
    }
}
