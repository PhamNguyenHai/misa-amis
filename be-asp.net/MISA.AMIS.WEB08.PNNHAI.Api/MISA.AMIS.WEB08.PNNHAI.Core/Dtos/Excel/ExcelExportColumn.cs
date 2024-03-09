using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.WEB08.PNNHAI.Core
{
    public class ExcelExportColumn
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string ColumnKey { get; set; } = string.Empty;

        public double? Width { get; set; }

        public FormatType? FormatType { get; set; }
        public TextAlign? Align { get; set; }
    }
}
