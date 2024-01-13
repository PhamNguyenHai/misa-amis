using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.WEB08.PNNHAI.Core
{
    public interface IHasCode
    {
        /// <summary>
        /// Để get code của entity
        /// </summary>
        /// <returns>code của entity</returns>
        string GetCode();

    }
}
