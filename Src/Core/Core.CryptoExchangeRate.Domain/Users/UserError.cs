using System.Net;
using Core.CryptoExchangeRate.Domain.Framework;
using Core.CryptoExchangeRate.Domain.TranslatorMessage;

namespace Core.CryptoExchangeRate.Domain.Users;

public static class UserError
{
    public static Error NotFound = new(HttpStatusCode.NotFound, TranslateValues.EXCEPTION_RECORD_NOT_FOUND);
    public static Error Duplicate = new(HttpStatusCode.Conflict, TranslateValues.EXCEPTION_DULICATE_RECORDS);
    public static Error PasswordIsNotCorrect = new(HttpStatusCode.Unauthorized, TranslateValues.EXCEPTION_PASSWORD_NOTCORRECT);
}