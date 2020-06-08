using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SPAapi
{
  public class ValidateModelAttribute : ActionFilterAttribute
  {
    public override void OnActionExecuting(ActionExecutingContext context)
    {
      if (!context.ModelState.IsValid)
      {
        context.Result = new ValidationFailedResult(context.ModelState);
      }
    }
  }
  public class ValidationError
  {
    //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Field { get; }
    public string Message { get; }
    public ValidationError(string field, string message)
    {
      Field = field != string.Empty ? field : null;
      Message = message;
    }
  }
  public class ApiResultFilterAttribute : ActionFilterAttribute
  {
    public override void OnActionExecuting(ActionExecutingContext context)
    {
      base.OnActionExecuting(context);
    }
    public override void OnResultExecuting(ResultExecutingContext context)
    {
      if (context.Result is ValidationFailedResult)
      {
        var objectResult = context.Result as ObjectResult;
        context.Result = objectResult;
      }
      else
      {
        var objectResult = context.Result as ObjectResult;
        context.Result = new OkObjectResult(new BaseResultModel(code: 200, result: objectResult.Value));
      }
    }
  }
  public class BaseResultModel
  {
    public BaseResultModel(int? code = null, string message = null,
        object result = null, ReturnStatus returnStatus = ReturnStatus.Success)
    {
      this.Code = code;
      this.Result = result;
      this.Message = message;
      this.ReturnStatus = returnStatus;
    }
    public int? Code { get; set; }

    public string Message { get; set; }

    public object Result { get; set; }

    public ReturnStatus ReturnStatus { get; set; }
  }
  public class ValidationFailedResultModel : BaseResultModel
  {
    public ValidationFailedResultModel(ModelStateDictionary modelState)
    {
      Code = 422;
      Message = "参数不合法";
      Result = modelState.Keys
                  .SelectMany(key => modelState[key].Errors.Select(x => new ValidationError(key, x.ErrorMessage)))
                  .ToList();
      ReturnStatus = ReturnStatus.Fail;
    }
  }
  public class ValidationFailedResult : ObjectResult
  {

    public ValidationFailedResult(ModelStateDictionary modelState)
          : base(new ValidationFailedResultModel(modelState))
    {
      StatusCode = StatusCodes.Status422UnprocessableEntity;
    }
  }


  public enum ReturnStatus
  {
    Success = 1,
    Fail = 0,
    ConfirmIsContinue = 2,
    Error = 3
  }
}
