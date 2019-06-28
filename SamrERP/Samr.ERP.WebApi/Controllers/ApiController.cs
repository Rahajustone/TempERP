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
        protected new BaseResponse Response(BaseResponse responseModel)
        {
            ResponseMetaHandler(responseModel.Meta);

            return responseModel;
        }
        protected new BaseDataResponse<T> Response<T>(BaseDataResponse<T> dataResponseModel)
        {
            ResponseMetaHandler(dataResponseModel.Meta);

            return dataResponseModel;

        }

        private void ResponseMetaHandler(ResponseMeta<ErrorModel> responseMeta)
        {
            HttpContext.Response.StatusCode =  (int) responseMeta.StatusCode;

            if (!responseMeta.Success) AddModelStateErrors(responseMeta);
        }

        protected void AddModelStateErrors(ResponseMeta<ErrorModel> responseMeta)
        {
            if (responseMeta.Errors == null)
            {
                responseMeta.Errors = new List<ErrorModel>();
            }

            foreach (var error in ModelState.Values)
            {
                foreach (var item in error.Errors)
                {
                    responseMeta.Errors.Add(new ErrorModel()
                    {
                        Description = item.ErrorMessage
                    });
                }
            }
        }
    }
}