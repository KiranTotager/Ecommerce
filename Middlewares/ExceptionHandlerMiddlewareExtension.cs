using ECommerce.CustomExceptions;
using ECommerce.DTO;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using System.Text.Json;

namespace ECommerce.Middlewares
{
    public static class ExceptionHandlerMiddlewareExtension
    {
        public static IApplicationBuilder UseExceptionHandlerMiddleware(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(error =>
            {
                error.Run(async context =>
                {
                    context.Response.ContentType = "application/json";
                    var ExceptionHanlderFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (ExceptionHanlderFeature != null)
                    {
                        int StatusCode = (int)HttpStatusCode.InternalServerError;
                        string Message = "Internal Server Error";
                        switch (ExceptionHanlderFeature.Error)
                        {
                            case NotFoundException notfound:
                                StatusCode = (int)HttpStatusCode.NotFound;
                                Message = notfound.Message;
                                break;
                            case AlreadyExistException alreadyExistException:
                                StatusCode = (int)HttpStatusCode.Conflict;
                                Message = alreadyExistException.Message;
                                break;
                             case UnauthorizedAccessException unauthorizedAccessException:
                                StatusCode=(int)HttpStatusCode.Unauthorized;
                                Message = unauthorizedAccessException.Message;
                                break;
                            case SecurityTokenException securityTokenException:
                                StatusCode= (int)HttpStatusCode.Unauthorized;
                                Message = securityTokenException.Message;
                                break; 
                            case CreationException creationException:
                                StatusCode = (int)HttpStatusCode.BadRequest;
                                Message = creationException.Message;
                                break;
                            case EmailNotConfirmedException emailNotConfirmedException:
                                StatusCode = (int)HttpStatusCode.Forbidden;
                                Message = emailNotConfirmedException.Message;
                                break;
                            case DeletionException deletionException:
                                StatusCode = (int)HttpStatusCode.Conflict;
                                Message = deletionException.Message;
                                break;
                        }
                        context.Response.StatusCode = StatusCode;
                        await context.Response.WriteAsync(JsonSerializer.Serialize(new ResponseBase<Object>(StatusCode, Message)));
                       
                    }

                });
            });
            return app;

        }
    }
}
