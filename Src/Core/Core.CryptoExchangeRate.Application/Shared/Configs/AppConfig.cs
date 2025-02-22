
using Core.CryptoExchangeRate.Application.Shared.Models;

namespace Core.CryptoExchangeRate.Application.Shared.Configs;

public class AppConfig
{
    public VehicleCoreConfig VehicleCoreConfig { get; set; }
}

public sealed class VehicleCoreConfig : ApiConfig
{
    public string InquiryByNationalCodeUrl { get; set; }
}



public class ApiConfig
{
    public string ApiKey { get; set; }
    public string GetTokenUrl { get; set; }
    public string BaseUrl { get; set; }
    public Dictionary<string, string> ErrorCode { get; set; } = [];
    public int Timeout { get; set; }
    public List<ErrorMapping> ErrorMappings { get; set; } = [];
}