using Dapper;
using MISA.AMIS.WEB08.PNNHAI.Core;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.WEB08.PNNHAI.Infrastructure
{
    public class ExcelImportTemplateSettingRepository : IExcelImportTemplateSettingRepository
    {
        #region Fields
        protected readonly IUnitOfWork _uow;
        #endregion

        #region Constructor
        public ExcelImportTemplateSettingRepository(IUnitOfWork uow)
        {
            _uow = uow;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Hàm thực hiện đọc dữ liệu để lấy ra cấu hình cho việc đọc file excel
        /// </summary>
        /// <param name="workingObjectTable">Bảng thực hiện import (trong csdl)</param>
        /// <returns>Dữ liệu để cấu hình cho việc đọc file excel</returns>
        /// Author: PNNHai
        /// Date: 
        public async Task<ImportFileTemplate> GetExcelImportTemplateSettingAsync(string workingObjectTable)
        {
            string storedProcedureName = $"Proc_importfiletemplate_GetSettingTemplate";

            var param = new DynamicParameters();
            param.Add("i_ImportColumn", workingObjectTable);


            var multipleResults = await _uow.Connection.QueryMultipleAsync(storedProcedureName, param,
                commandType: CommandType.StoredProcedure, transaction: _uow.Transaction);

            var importFileTemplate = await multipleResults.ReadFirstOrDefaultAsync<ImportFileTemplate>();
            if (importFileTemplate != null)
            {
                importFileTemplate.ImportColumns = (List<ImportColumn>)await (multipleResults.ReadAsync<ImportColumn>());
                return importFileTemplate;
            }
            else
            {
                throw new ValidateException(Core.Resources.AppResource.NotImplementWithObjError);
            }
        } 
        #endregion
    }
}
