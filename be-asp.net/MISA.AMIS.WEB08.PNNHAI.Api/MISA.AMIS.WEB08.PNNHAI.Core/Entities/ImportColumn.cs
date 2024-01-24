using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.WEB08.PNNHAI.Core
{
    public class ImportColumn
    {
        public Guid ImportColumnId { set; get; }
        public string ColumnTitle { set; get; }
        public int ColumnIndex { set; get; }
        public bool? IsRequired { set; get; }
        public string ColumnInsert { set; get; }
        public ColumnImportDataType ColumnDataType { set; get; }
        public string? ReferenceObjectName { set; get; }
        public Guid ImportFileTemplateId { set; get; }
    }
}
