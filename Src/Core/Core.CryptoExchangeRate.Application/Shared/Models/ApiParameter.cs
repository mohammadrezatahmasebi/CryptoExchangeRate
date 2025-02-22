using Core.CryptoExchangeRate.Application.Shared.Configs;

namespace Core.CryptoExchangeRate.Application.Shared.Models;

public class ApiParameter
{
    public ApiConfig ApiConfig { get; set; }
    public string ApiUrl { get; set; }
    public HttpMethod HttpMethod { get; set; } = null;
    public HttpContent HttpContent { get; set; } = null;
    public Dictionary<string, string> HttpHeader { get; set; } = [];
    public Dictionary<string,string[] > QueryParams { get; set; } = [];
    public bool CheckErrorInSuccessStatus { get; set; } = false;
    public int SuccessCode { get; set; } = 0;
    public bool UseCache { get; set; } = false;
    
}