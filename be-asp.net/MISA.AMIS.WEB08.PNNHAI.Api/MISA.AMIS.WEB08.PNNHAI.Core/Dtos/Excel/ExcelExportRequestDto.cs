using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.WEB08.PNNHAI.Core
{
    public class ExcelExportRequestDto
    {
        [Required]
        public IEnumerable<ExcelExportColumn> Columns { get; set; }
        public string? Title { get; set; }
        public string? Sheet { get; set; }
        public ExportType ExportType { get; set; }
        public string? SearchString { get; set; }
        public IEnumerable<FilterColumn>? FilterColumns { get; set; }
        public IEnumerable<Guid>? Ids { get; set; }

        public ExcelExportRequestDto()
        {
            FilterColumns = new List<FilterColumn>();
            Ids = new List<Guid>();
        }
    }
}