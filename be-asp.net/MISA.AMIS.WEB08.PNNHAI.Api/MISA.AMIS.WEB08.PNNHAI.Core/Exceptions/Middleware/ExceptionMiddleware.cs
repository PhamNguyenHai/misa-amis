using Microsoft.AspNetCore.Http;
using MISA.AMIS.WEB08.PNNHAI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.WEB08.PNNHAI.Core
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }
        /// <summary>
        /// Hàm thực hiện kiểm soát lỗi
        /// </summary>
        /// <param name="context">http context </param>
        /// <param name="exception">exception trả về</param>
        /// <returns></returns>
        /// Author: PNNHai
        /// Date
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            Console.WriteLine(exception);
            context.Response.ContentType = "application/json";
            switch (exception)
            {
                case NotFoundException:
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    await context.Response.WriteAsync(
                        text: new BaseNotifyException()
                        {
                            ErrorCode = ((NotFoundException)exception).ErrorCode,
                            UserMessage = exception.Message,
                            DevMessage = exception.Message,
                            TraceId = context.TraceIdentifier,
                            MoreInfo = exception.HelpLink,
                        }.ToString() ?? ""      // ToString là chuyển sang json (hàm được override bên base)
                    );
                    break;

                case ValidateException:
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsync(
                        text: new BaseNotifyException()
                        {
                            ErrorCode = ((ValidateException)exception).ErrorCode,
                            UserMessage = exception.Message,
                            DevMessage = exception.Message,
                            TraceId = context.TraceIdentifier,
                            MoreInfo = exception.HelpLink,
                        }.ToString() ?? ""
                    );
                    break;

                default:
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    await context.Response.WriteAsync(
                        text: new BaseNotifyException()
                        {
                            ErrorCode = context.Response.StatusCode,
                            UserMessage = $"{Core.Resources.AppResource.SystemError}",
                            DevMessage = exception.Message,
                            TraceId = context.TraceIdentifier,
                            MoreInfo = exception.HelpLink,
                        }.ToString() ?? "");
                    break;
            }
        }
    }
}