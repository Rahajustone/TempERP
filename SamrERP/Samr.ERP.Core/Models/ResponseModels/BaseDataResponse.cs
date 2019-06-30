using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Samr.ERP.Core.Models.ErrorModels;

namespace Samr.ERP.Core.Models.ResponseModels
{
    public class BaseDataResponse<TData> : BaseDataResponse<TData, ErrorModel>
    {
        /// <summary>
        /// Creates a BaseDataResponse .
        /// </summary>
        /// <param name="data">TModel value.</param>
        /// <param name="statusCode"></param>
        /// <param name="errors">String messsages.</param>
        /// <returns>BaseDataResponse</returns>
        public BaseDataResponse(TData data, HttpStatusCode statusCode, params ErrorModel[] errors) : base(data, statusCode, errors)
        {
        }
        public BaseDataResponse(TData data, HttpStatusCode statusCode, IEnumerable<ErrorModel> errors) : base(data, statusCode, errors)
        {
        }
        public static BaseDataResponse<TData> Success(TData model) => new BaseDataResponse<TData>(model, HttpStatusCode.OK);

        public static BaseDataResponse<TData> Fail(TData model, params ErrorModel[] errors) => new BaseDataResponse<TData>(model, HttpStatusCode.BadRequest, errors);
        public static BaseDataResponse<TData> Fail(TData model, IEnumerable<ErrorModel> errors) => new BaseDataResponse<TData>(model, HttpStatusCode.BadRequest, errors);
        public static BaseDataResponse<TData> NotFound(TData model, params ErrorModel[] errors) => new BaseDataResponse<TData>(model, HttpStatusCode.NotFound, errors);
        public static BaseDataResponse<TData> NotFound(TData model) => new BaseDataResponse<TData>(model, HttpStatusCode.NotFound, new ErrorModel("Entity not found"));
        public static BaseDataResponse<TData> Unauthorized(TData model, params ErrorModel[] errors) => new BaseDataResponse<TData>(model, HttpStatusCode.Unauthorized, errors);

    }

   

    public class BaseDataResponse<TData, TMessage> : BaseResponse<TMessage>
    {
        public TData Data { get; set; }

        /// <summary>
        /// Creates a BaseDataResponse .
        /// </summary>
        /// <param name="data">TModel value.</param>
        /// <param name="statusCode"></param>
        /// <param name="errors">TMessage messsages.</param>
        /// <returns>BaseDataResponse</returns>
        public BaseDataResponse(TData data, HttpStatusCode statusCode, params TMessage[] errors) : base(statusCode, errors)
        {
            Data = data;
        }
        public BaseDataResponse(TData data, HttpStatusCode statusCode, IEnumerable<TMessage> errors) : base(statusCode, errors)
        {
            Data = data;
        }

    }





}
