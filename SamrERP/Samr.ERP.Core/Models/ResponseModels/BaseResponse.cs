using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Samr.ERP.Core.Models.ErrorModels;

namespace Samr.ERP.Core.Models.ResponseModels
{
    public class BaseResponse<TData> : BaseResponse<TData, ErrorModel>
    {
        /// <summary>
        /// Creates a BaseResponse .
        /// </summary>
        /// <param name="data">TModel value.</param>
        /// <param name="success">IsSuccesed.</param>
        /// <param name="statusCode"></param>
        /// <param name="errors">String messsages.</param>
        /// <returns>BaseResponse</returns>
        public BaseResponse(TData data, HttpStatusCode statusCode, params ErrorModel[] errors) : base(data, statusCode, errors)
        {
        }
        public BaseResponse(TData data, HttpStatusCode statusCode, IEnumerable<ErrorModel> errors) : base(data, statusCode, errors)
        {
        }
        public static BaseResponse<TData> Success(TData model) => new BaseResponse<TData>(model, HttpStatusCode.OK);

        public static BaseResponse<TData> Fail(TData model, params ErrorModel[] errors) => new BaseResponse<TData>(model, HttpStatusCode.BadRequest, errors);
        public static BaseResponse<TData> NotFound(TData model, params ErrorModel[] errors) => new BaseResponse<TData>(model, HttpStatusCode.NotFound, errors);
        public static BaseResponse<TData> NotFound(TData model) => new BaseResponse<TData>(model, HttpStatusCode.NotFound, new ErrorModel("Entity not found"));
        public static BaseResponse<TData> Unauthorized(TData model, params ErrorModel[] errors) => new BaseResponse<TData>(model, HttpStatusCode.Unauthorized, errors);

    }
    public class BaseResponse<TData, TMessage>
    {
        public ResponseMeta<TMessage> Meta;
        public TData Data { get; set; }

        /// <summary>
        /// Creates a BaseResponse .
        /// </summary>
        /// <param name="data">TModel value.</param>
        /// <param name="statusCode"></param>
        /// <param name="errors">TMessage messsages.</param>
        /// <returns>BaseResponse</returns>
        public BaseResponse(TData data, HttpStatusCode statusCode, params TMessage[] errors)
        {
            Meta = new ResponseMeta<TMessage>(statusCode, errors);
            Data = data;
        }
        public BaseResponse(TData data, HttpStatusCode statusCode, IEnumerable<TMessage> errors)
        {
            Meta = new ResponseMeta<TMessage>(statusCode, errors);
            Data = data;

        }

    }
}
