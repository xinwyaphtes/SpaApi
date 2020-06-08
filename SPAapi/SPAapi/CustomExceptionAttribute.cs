using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SPAapi
{
  public class CustomExceptionResultModel : BaseResultModel
  {
    public CustomExceptionResultModel(int? code, Exception exception)
    {
      Code = code;
      Message = exception.InnerException != null ?
          exception.InnerException.Message :
          exception.Message;
      Result = exception.Message;
      ReturnStatus = ReturnStatus.Error;
    }
  }
  public class CustomExceptionResult : ObjectResult
  {
    public CustomExceptionResult(int? code, Exception exception)
            : base(new CustomExceptionResultModel(code, exception))
    {
      StatusCode = code;
    }
  }
  public class CustomExceptionAttribute : IExceptionFilter
  {
    private readonly Microsoft.Extensions.Logging.ILogger<CustomExceptionAttribute> _logger;
    public CustomExceptionAttribute(Microsoft.Extensions.Logging.ILogger<CustomExceptionAttribute> logger)
    {
      _logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
      HttpStatusCode status = HttpStatusCode.InternalServerError;

      //处理各种异常
      _logger.LogInformation(1000, "ttt");

      context.ExceptionHandled = true;
      context.Result = new CustomExceptionResult((int)status, context.Exception);
    }
  }
}
