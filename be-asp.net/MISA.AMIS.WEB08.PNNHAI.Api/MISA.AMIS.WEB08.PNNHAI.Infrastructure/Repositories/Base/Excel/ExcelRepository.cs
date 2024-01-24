using AutoMapper;
using Microsoft.AspNetCore.Http;
using MISA.AMIS.WEB08.PNNHAI.Core;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LicenseContext = OfficeOpenXml.LicenseContext;


namespace MISA.AMIS.WEB08.PNNHAI.Infrastructure
{
    public abstract class ExcelRepository<TRespondDto, TEntity, TModel> : ExcelExportRepository<TEntity, TModel>, IExcelRepository<TRespondDto>
        where TRespondDto : class, new()
    {

        public ExcelRepository(IReadOnlyRepository<TEntity, TModel> readOnlyRepository, IMapper mapper) : base(readOnlyRepository, mapper)
        {
        }
    }
}
