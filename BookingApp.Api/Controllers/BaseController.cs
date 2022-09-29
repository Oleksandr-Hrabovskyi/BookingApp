using System;
using System.Threading;
using System.Threading.Tasks;

using BookingApp.Contracts.Http;
using BookingApp.Domain.Exceptions;

using Microsoft.AspNetCore.Mvc;

using Npgsql;

namespace BookingApp.Api.Controllers;

public class BaseController : ControllerBase
{
    protected async Task<IActionResult> SaveExecute(Func<Task<IActionResult>> action,
        CancellationToken cancellationToken)
    {
        try
        {
            return await action();
        }
        catch (BookingException be)
        {
            var response = new ErrorResponse
            {
                Code = be.ErrorCode,
                Message = be.Message
            };

            return ToActionResult(response);
        }
        catch (InvalidOperationException ioe) when (ioe.InnerException is NpgsqlException)
        {
            var response = new ErrorResponse
            {
                Code = ErrorCode.DbFailureError,
                Message = "DB failure"
            };
            return ToActionResult(response);
        }
        catch (Exception)
        {
            var response = new ErrorResponse
            {
                Code = ErrorCode.InternalServerError,
                Message = "Unhandled error"
            };
            return ToActionResult(response);
        }
    }
    protected IActionResult ToActionResult(ErrorResponse errorResponse)
    {
        return StatusCode((int)errorResponse.Code / 100, errorResponse);
    }
}