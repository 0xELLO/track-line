using System.Diagnostics;
using System.Net;
using App.Public.DTO.v1.Helpers;
using Base.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;

namespace WebApp.ApiControllers.Helpers;

public class ErrorBuilder
{
    public RestApiErrorResponse ErrorResponse(string traceId, IEnumerable<IdentityError> apiErrors)
    {
        //_logger.LogWarning("Email is already registered {} ", registrationData.Email);
        
        var error = new RestApiErrorResponse
        {
            Type = ApiErrorType.BadRequestType,
            Status = (int) HttpStatusCode.BadRequest,
            TraceId = traceId,
            Errors = new Dictionary<string, string>()
        };
        
        foreach (var apiError in apiErrors)
        {
            error.Errors.Add(apiError.Code, apiError.Description);
        }
        
        return error;
    } 
    
}

