using System.Net;
using Core.CryptoExchangeRate.Application.Framework.Queries;
using Core.CryptoExchangeRate.Domain.Framework;
using MediatR;
using Serilog;

namespace Core.CryptoExchangeRate.Application.Framework.Behavior
{
    public class UnhandledExceptionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IQuery<TResponse>
        where TResponse : Result
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            try
            {
                return await next();
            }
            catch (Exception error)
            {
                var errorDetail = HandleException(error);
                Log.Error(error, "Error in request {RequestType}: {Error}", typeof(TRequest).Name, errorDetail);

                throw new ErrorExceptions()
                {
                    Message = error.Message,
                    ErrorCode = 500,
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }


        public string HandleException(Exception error)
        {
            string errorMessage = $"{error.Message}-{error.InnerException?.Message}";
            switch (error.GetType().Name)
            {
                case nameof(InvalidOperationException):
                    errorMessage = HandleInvalidOperationException((error as InvalidOperationException)!);
                    break;
                case nameof(NullReferenceException):
                    errorMessage = HandleNullReferenceException((error as NullReferenceException)!);
                    break;


                case nameof(HttpRequestException):
                    errorMessage = HandleHttpRequestException((error as HttpRequestException)!);
                    break;
            }

            return errorMessage;
        }


        private string HandleInvalidOperationException(InvalidOperationException error)
        {
            if (error.Message == "Sequence contains more than one element.")
                return "SequenceMultiElementExceptionError";
            return $"InvalidOperationExceptionError-{CreateErrorDetail(error)}";
        }

        private string HandleNullReferenceException(NullReferenceException error)
        {
            return $"NullReferenceExceptionError-{CreateErrorDetail(error)}";
        }

        private string HandleHttpRequestException(HttpRequestException error)
        {
            return $"HttpRequestError-{CreateErrorDetail(error)}";
        }

        private string CreateErrorDetail(Exception exception)
        {
            return $"{exception.Message}. {exception.InnerException?.Message}. " +
                   $"{exception.Source}. {exception.TargetSite?.Name}. {exception.StackTrace}";
        }
    }
}