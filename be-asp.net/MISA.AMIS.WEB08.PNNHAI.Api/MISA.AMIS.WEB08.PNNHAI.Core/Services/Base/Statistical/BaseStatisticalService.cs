using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.WEB08.PNNHAI.Core
{
    public abstract class BaseStatisticalService : IBaseStatisticalService
    {
        protected readonly IBaseStatisticalRepository _baseStatisticalRepository;

        protected BaseStatisticalService(IBaseStatisticalRepository baseStatisticalRepository)
        {
            _baseStatisticalRepository = baseStatisticalRepository;
        }

        /// <summary>
        /// Hàm thực hiện thống kê dữ liệu theo tên thuộc tính
        /// </summary>
        /// <param name="propertyKey">Thuộc tính cần thống kê theo</param>
        /// <returns>Danh sách dữ liệu thống kê</returns>
        public async Task<IEnumerable<StatisticalDto>> StatisticalByPropertyKey(string propertyKey)
        {
            var result = await _baseStatisticalRepository.GetStatisticalByPropertyKeyAsync(propertyKey);
            return result;
        }
    }
}
