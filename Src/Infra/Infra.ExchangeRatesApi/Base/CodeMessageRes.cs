namespace Infra.ExchangeRatesApi.Base;

public class CodeMessageRes
{
    public int Code { get; set; }
    public string ErrorMessage { get; set; }
    public object Message { get; set; }
    public object Messages { get; set; }
}