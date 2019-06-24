using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Samr.ERP.Core.Models.ErrorModels;
using Samr.ERP.Core.Models.ResponseModels;
using Swashbuckle.AspNetCore.Swagger;

namespace Samr.ERP.WebApi.Controllers
{

    public abstract class ApiController : ControllerBase
    {
        protected new BaseResponse<T> Response<T>(BaseResponse<T> responseModel)
        {
            return responseModel.Meta.Success ? responseModel : BadRequest(responseModel);
        }

        protected BaseResponse<T> BadRequest<T>(BaseResponse<T> responseModel)
        {
            AddModelStateErrors(responseModel);
            HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            return responseModel;
        }

        protected void AddModelStateErrors<T>(BaseResponse<T> baseResponse)
        {
            if (baseResponse.Meta.Errors == null)
            {
                baseResponse.Meta.Errors = new List<ErrorModel>();
            }

            foreach (var error in ModelState.Values)
            {
                foreach (var item in error.Errors)
                {
                    baseResponse.Meta.Errors.Add(new ErrorModel()
                    {
                        Description = item.ErrorMessage
                    });
                }
            }
        }
    }
}