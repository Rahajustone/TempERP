using System;
using System.Collections.Generic;
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
        /// <param name="errors">String messsages.</param>
        /// <returns>BaseResponse</returns>
        public BaseResponse(TData data, bool success, params ErrorModel[] errors) : base(data, success, errors)
        {
        }
        public BaseResponse(TData data, bool success, IEnumerable<ErrorModel> errors) : base(data, success, errors)
        {
        }
        public static BaseResponse<TData> Success(TData model, params ErrorModel[] errors) => new BaseResponse<TData>(model,true,errors);

        public static BaseResponse<TData> Fail(TData model, params ErrorModel[] errors) => new BaseResponse<TData>(model, false, errors);
      
    }
    public class BaseResponse<TData, TMessage>
    {
        public ResponseMeta<TMessage> Meta;
        public TData Data { get; set; }

        /// <summary>
        /// Creates a BaseResponse .
        /// </summary>
        /// <param name="data">TModel value.</param>
        /// <param name="success">IsSuccesed.</param>
        /// <param name="errors">TMessage messsages.</param>
        /// <returns>BaseResponse</returns>
        public BaseResponse(TData data, bool success, params TMessage[] errors)
        {
            Meta = new ResponseMeta<TMessage>(success, errors);
            Data = data;
        }
        public BaseResponse(TData data, bool success, IEnumerable<TMessage> errors)
        {
            Meta = new ResponseMeta<TMessage>(success, errors);
            Data = data;

        }

    }
}
