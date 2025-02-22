using System.Text.Json.Serialization;

namespace Core.CryptoExchangeRate.Application.Shared;




public abstract class ServiceReqContextBase
{
}

public abstract class ServiceResContextBase(ValidationError validationError)
{
    protected ServiceResContextBase() : this(new ValidationError())
    {
    }

    [JsonIgnore] public bool HasError => !string.IsNullOrEmpty(ValidationError.Code?.Trim());

    [JsonIgnore] public ValidationError ValidationError { get; set; } = validationError;

    public void Failure(ValidationError error)
    {
        ValidationError = error;
    }
}

public readonly struct ValidationError
{
    public ValidationError(string code, string message)
    {
        Code = code;
        Message = message ?? code;
        Target = null;
        Headers = null;
        HttpStatusCode = 400;
        Details = new List<ValidationError>();
    }

    public static ValidationError UnhandledValidationError(string code, string message, NotificationType notificationType = NotificationType.Toast, string trackingNumber = "")
    {
        return new ValidationError(code, message, 420, notificationType, trackingNumber);
    }

    public ValidationError(string code, string message, string target) : this(code, message) =>
        Target = target;

    public ValidationError(string code, string message, object data) : this(code, message) =>
        Data = data;

    public ValidationError(string code, string message, int httpStatusCode) : this(code, message) =>
        HttpStatusCode = httpStatusCode;

    public ValidationError(string code, string message, string target, int httpStatusCode) : this(code, message, target) =>
        HttpStatusCode = httpStatusCode;

    public ValidationError(string code, string message, string target, int httpStatusCode,
        List<ValidationError> details) : this(code, message, target, httpStatusCode) =>
        Details = details;

    public ValidationError(string code, string message, string target, int httpStatusCode,
        List<ValidationError> details, string headers) : this(code, message, target, httpStatusCode, details) =>
        Headers = headers;

    public ValidationError(string code, string message, string target, List<ValidationError> details) : this(code, message, target) =>
        Details = details;

    public ValidationError(string code, string message, List<ValidationError> details) : this(code, message) =>
        Details = details;

    public ValidationError(string code, string message, NotificationType notificationType = NotificationType.Toast) : this(code, message) =>
        DisplayMode = notificationType;

    public ValidationError(string code, string message, string target, NotificationType notificationType = NotificationType.Toast) : this(code, message, target) =>
        DisplayMode = notificationType;

    public ValidationError(string code, string message, NotificationType notificationType = NotificationType.Toast, string trackingNumber = "") : this(code, message, notificationType) =>
        TrackingNumber = trackingNumber;


    public ValidationError(string code, string message, int httpStatusCode, NotificationType notificationType = NotificationType.Toast, string trackingNumber = "") : this(code, message, notificationType)
    {
        TrackingNumber = trackingNumber;
        HttpStatusCode = httpStatusCode;
    }


    public ValidationError(string code, string message, string target, NotificationType notificationType = NotificationType.Toast, string trackingNumber = "") : this(code, message, target, notificationType) =>
        TrackingNumber = trackingNumber;

    public ValidationError(string code, string message, string target, string trackingNumber) : this(code, message, target) =>
        TrackingNumber = trackingNumber;

    public ValidationError(string code, string message, string target, string trackingNumber, string traceIdentifier) : this(code, message, target, trackingNumber) =>
        TraceIdentifier = traceIdentifier;

    public ValidationError(string code, string message, string target, int httpStatusCode, string traceIdentifier) : this(code, message, target, httpStatusCode) =>
        TraceIdentifier = traceIdentifier;

    public ValidationError(string code, string message, List<ValidationError> details, string traceIdentifier) : this(code, message, details) =>
        TraceIdentifier = traceIdentifier;

    public ValidationError(string code, string message, string target, NotificationType notificationType = NotificationType.Toast, string trackingNumber = "", string traceIdentifier = "") : this(code, message, target, notificationType, trackingNumber) =>
        TraceIdentifier = traceIdentifier;

    public ValidationError(string code, string message, NotificationType notificationType = NotificationType.Toast, string trackingNumber = "", string traceIdentifier = "") : this(code, message, notificationType, trackingNumber) =>
        TraceIdentifier = traceIdentifier;

    public ValidationError(string code, string message, string target, int httpStatusCode,
        List<ValidationError> details, string headers, string trackingNumber) : this(code, message, target, httpStatusCode, details, headers) =>
        TrackingNumber = trackingNumber;

    public ValidationError(string code, string message, string target, int httpStatusCode,
        List<ValidationError> details, string headers, string trackingNumber, string traceIdentifier) : this(code, message, target, httpStatusCode, details, headers, trackingNumber) =>
        TraceIdentifier = traceIdentifier;

    public ValidationError(string code, string message, string target, int httpStatusCode,
        List<ValidationError> details, string headers, string trackingNumber, string traceIdentifier
        , NotificationType notificationType = NotificationType.Toast, object data = null)
        : this(code, message, target, httpStatusCode, details, headers, trackingNumber, traceIdentifier)
    {
        DisplayMode = notificationType;
        Data = data;
    }

    public string Code { get; init; }
    public string Message { get; init; }
    public string Target { get; init; }
    public object Data { get; init; }
    [JsonIgnore] public int HttpStatusCode { get; init; } = 400;
    [JsonIgnore] public string Headers { get; init; }
    public List<ValidationError> Details { get; init; }
    public NotificationType DisplayMode { get; init; }
    public string TrackingNumber { get; init; }
    public string TraceIdentifier { get; init; }

    public static explicit operator ValidationError(ValidationFailure failure)
    {
        return new ValidationError(failure.ErrorCode.ToString(), failure.ErrorMessage, failure.PropertyName);
    }

    public static explicit operator ValidationError(List<ValidationFailure> validationFailures)
    {
        if (!(validationFailures?.Any() ?? false))
            return new ValidationError();

        var firstError = validationFailures.First();
        var opExplicit = new ValidationError
        (
            string.IsNullOrEmpty(firstError.ErrorCode.ToString()) ? "400" : firstError.ErrorCode.ToString(),
            firstError.ErrorMessage,
            firstError.PropertyName,
            validationFailures.Count > 1
                ? validationFailures.Select(s => (ValidationError)s).ToList()
                : new List<ValidationError>());

        return opExplicit;
    }

    public ValidationError WithTraceIdentifier(string traceIdentifier)
    {
        return new ValidationError(Code, Message, Target, HttpStatusCode, Details, Headers, TrackingNumber, traceIdentifier, DisplayMode,Data);
    }

    public enum NotificationType
    {
        Toast,
        Page,
        Inline,
        Popup
    }
}