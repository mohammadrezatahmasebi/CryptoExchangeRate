namespace Infra.ExchangeRatesApi.Base
{
    internal class ApiCallLogModel
    {
        public string Timestamp { get; set; }
        public string Method { get; set; }
        public string Endpoint { get; set; }
        public int StatusCode { get; set; }
        public int Duration { get; set; }
        public string? Error { get; set; }
        public string? UserId { get; set; }//TODO
        public string? RequestBody { get; set; }
        public string? ResponseBody { get; set; }
        public string? TrackCode { get; set; }
        public ApiCallLogModel(string timestamp, string method,
            string endpoint, int statusCode,
            int duration, string? error, 
            string? userId, string? requestBody,
            string? responseBody, string? trackCode)
        {
            Timestamp = timestamp;
            Method = method;
            Endpoint = endpoint;
            StatusCode = statusCode;
            Duration = duration;
            Error = error;
            UserId = userId;
            RequestBody = requestBody;
            ResponseBody = responseBody;
            TrackCode = trackCode;
        }
    }

}
