using System.Diagnostics;
using System.Text.Json;
using Core.CryptoExchangeRate.Application.Shared.Models;

namespace EndPoint.CryptoExchangeRate.Api.GlobalExceptionHandler;

public interface IGlobalException
{
    Task InvokeAsync(HttpContext context);

}

public class GlobalException : IGlobalException
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
        string text = JsonSerializer.Serialize<ErrorResponse>(new ()
        {
        
            ErrorMessage = ex.ErrorMessage ?? ex.StackTrace.ToString(),
        });
        return context.Response.WriteAsync(text);
    }   
}