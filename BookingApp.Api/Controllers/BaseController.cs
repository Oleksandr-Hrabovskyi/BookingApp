using System;
using System.Threading;
using System.Threading.Tasks;

using BookingApp.Contracts.Http;
using BookingApp.Domain.Exceptions;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Npgsql;

namespace BookingApp.Api.Controllers;

public class BaseController : ControllerBase
{
    private readonly ILogger _logger;

    protected BaseController(ILogger logger)
    {
        _logger = logger;
    }

    protected async Task<IActionResult> SaveExecute(Func<Task<IActionResult>> action,
        CancellationToken cancellationToken)
    {
        try
        {
            return await action();
        }
        catch (BookingException be)
        {
            _logger.LogError(be, "Booking exception raised");
            var response = new ErrorResponse
            {
                Code = be.ErrorCode,
                Message = be.Message
            };

            return ToActionResult(response);
        }
        catch (InvalidOperationException ioe) when (ioe.InnerException is NpgsqlException)
        {
            _logger.LogError(ioe, "DB exception raised");
            var response = new ErrorResponse
            {
                Code = ErrorCode.DbFailureError,
                Message = "DB failure"
            };
            return ToActionResult(response);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unhandled exception raised");
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