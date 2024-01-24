using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.WEB08.PNNHAI.Core
{
    public class ImportFileTemplate
    {
        public Guid ImportFileTemplateId { set; get; }
        public string ImportFileTemplateName { set; get; }
        public string TableImport { set; get; }
        public int HeadingRowIndex { set; get; }
        public int StartImportRowIndex { set; get; }
        public ICollection<ImportColumn> ImportColumns { get; set; }

    }
}
