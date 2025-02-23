using System.Diagnostics;
using System.Text.Json;
using Core.CryptoExchangeRate.Application.Shared.Models;
using Core.CryptoExchangeRate.Domain.Framework;

namespace EndPoint.CryptoExchangeRate.Api.GlobalExceptions
{
    public class GlobalException : IGlobaException
    {
        private readonly RequestDelegate _next;

        public GlobalException(RequestDelegate next) => this._next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await this._next(context);
            }
            catch (ErrorExceptions ex)
            {
                await GlobalException.HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, ErrorExceptions ex)
        {
            context.Response.ContentType = "application/json";
            Process currentProcess = Process.GetCurrentProcess();
            int id = currentProcess.Id;
            string processName = currentProcess.ProcessName;
            context.Response.StatusCode = true ? (true ? (true ? (true ? (true ? (ex.StatusCode == 0 ? 500 : (int) ex.StatusCode) : 404) : 508) : 400) : 404) : 401;
            string text = JsonSerializer.Serialize<ErrorResponse>(new ErrorResponse()
            {
                ErrorCode = "500",
                ErrorMessage = ex.ErrorMessage,
                StatusCode = (int)ex.StatusCode
            });
            return context.Response.WriteAsync(text);
        }
    }
}