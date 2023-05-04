using ErciyesSozluk.Common.Infrastructure.Exceptions;
using ErciyesSozluk.Common.Infrastructure.Results;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace ErciyesSozluk.Api.WebApi.Infrastructure.Extensions
{
    public static class ApplicationBuilderExtension
    {
        //includeExceptionDetails geriye butun detayların donup donmeyecegini tutar
        //useDefaultHandlingResponse, metoda verilecek olan metodun mu yoksa default metodun mu calisacagini tutar
        //handleException disaridan verilen metottur
        public static IApplicationBuilder ConfigureExceptionHandling(this IApplicationBuilder app,
            bool includeExceptionDetails = false,
            bool useDefaultHandlingResponse = true,
            Func<HttpContext, Exception, Task> handleException = null)
        {
            app.UseExceptionHandler(options =>
            {
                options.Run(context =>
                {
                    //exception detaylarini tutar
                    var exceptionObject = context.Features.Get<IExceptionHandlerFeature>();

                    //metoda bir metod verilmemis, ancak metoda kendi verecegi metodu kullanacagini soylemisse hata firlatir
                    if (!useDefaultHandlingResponse && handleException == null)
                    {
                        throw new ArgumentNullException($"{nameof(handleException)} cannot be null when {nameof(useDefaultHandlingResponse)} is false");
                    }

                    if (!useDefaultHandlingResponse && handleException != null)
                    {
                        return handleException(context, exceptionObject.Error);
                    }

                    return DefaultHandleException(context, exceptionObject.Error, includeExceptionDetails);
                });
            });

            return app;
        }

        private static async Task DefaultHandleException(HttpContext context, Exception exception, bool includeExceptionDetails)
        {
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            string message = "Internal server error occured!";

            if (exception is UnauthorizedAccessException)
            {
                statusCode = HttpStatusCode.Unauthorized;
            }

            //ornegin bu tur hata meydana gelirse spesifik bir tur geri dondurulur
            if (exception is DatabaseValidationException)
            {
                statusCode = HttpStatusCode.BadRequest;
                var validationResponse = new ValidationResponseModel(exception.Message);
                await WriteResponse(context, statusCode, validationResponse);
                return;
            }

            var res = new
            {
                HttpStatusCode = (int)statusCode,
                Detail = includeExceptionDetails ? exception.ToString() : message
            };

            await WriteResponse(context, statusCode, res);
        }

        private static async Task WriteResponse(HttpContext context, HttpStatusCode statusCode, object responseObj)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            await context.Response.WriteAsJsonAsync(responseObj);
        }
    }
}
