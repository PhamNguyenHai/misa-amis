using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.WEB08.PNNHAI.Core
{
    public class ExcelImportRespondedDto
    {
        public List<string> Errors { set; get; }

        public ExcelImportRespondedDto()
        {
            Errors = new List<string>();
        }
    }
}
