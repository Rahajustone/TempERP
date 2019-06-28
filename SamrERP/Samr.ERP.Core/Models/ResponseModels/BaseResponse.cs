using System.Collections.Generic;
using System.Net;
using Samr.ERP.Core.Models.ErrorModels;

namespace Samr.ERP.Core.Models.ResponseModels
{

    public class BaseResponse : BaseResponse<ErrorModel>
    {
        public BaseResponse(HttpStatusCode statusCode, params ErrorModel[] errors) : base(statusCode, errors)
        {
        }
        public BaseResponse(HttpStatusCode statusCode, IEnumerable<ErrorModel> errors) : base(statusCode, errors)
        {
        }
        public static BaseResponse Success() => new BaseResponse(HttpStatusCode.OK);
        public static BaseResponse Fail(params ErrorModel[] errors) => new BaseResponse(HttpStatusCode.BadRequest, errors);
        public static BaseResponse Fail(IEnumerable<ErrorModel> errors) => new BaseResponse(HttpStatusCode.BadRequest, errors);
        public static BaseResponse NotFound(params ErrorModel[] errors) => new BaseResponse(HttpStatusCode.NotFound, errors);
        public static BaseResponse NotFound() => new BaseResponse(HttpStatusCode.NotFound, new ErrorModel("Entity not found"));
        public static BaseResponse Unauthorized(params ErrorModel[] errors) => new BaseResponse(HttpStatusCode.Unauthorized, errors);
    }

    public class BaseResponse<TMessage>
    {
        public ResponseMeta<TMessage> Meta;

        /// <summary>
        /// Creates a BaseResponse .
        /// </summary>
        /// <param>TModel value.</param>
        /// <param name="statusCode"></param>
        /// <param name="errors">TMessage messsages.</param>
        /// <returns>BaseResponse</returns>
        public BaseResponse(HttpStatusCode statusCode, params TMessage[] errors)
        {
            Meta = new ResponseMeta<TMessage>(statusCode, errors);
        }
        public BaseResponse(HttpStatusCode statusCode, IEnumerable<TMessage> errors)
        {
            Meta = new ResponseMeta<TMessage>(statusCode, errors);
        }

    }

}