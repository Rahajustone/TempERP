using System;
using System.Collections.Generic;
using System.Text;
using Samr.ERP.Core.Models.ErrorModels;

namespace Samr.ERP.Core.Models.ResponseModels
{
    public class BaseResponse<TModel>:BaseResponse<TModel,ErrorModel>
    {
        /// <summary>
        /// Creates a BaseResponse .
        /// </summary>
        /// <param name="model">TModel value.</param>
        /// <param name="success">IsSuccesed.</param>
        /// <param name="errors">String messsages.</param>
        /// <returns>BaseResponse</returns>
        public BaseResponse(TModel model, bool success, params ErrorModel[] errors) : base(model,success, errors)
        {
        }
        public BaseResponse(TModel model, bool success, IEnumerable<ErrorModel> errors) : base(model, success, errors)
        {
        }
    }
    public class BaseResponse<TModel,TMessage>
    {
        public TModel Model { get; set; }
        public bool Success { get; protected set; }
        public IEnumerable<TMessage> Errors { get; protected set; }

        /// <summary>
        /// Creates a BaseResponse .
        /// </summary>
        /// <param name="model">TModel value.</param>
        /// <param name="success">IsSuccesed.</param>
        /// <param name="errors">TMessage messsages.</param>
        /// <returns>BaseResponse</returns>
        public BaseResponse(TModel model, bool success, params TMessage[] errors)
        {
            Model = model;
            Success = success;
            Errors = errors;
        }
        public BaseResponse(TModel model, bool success,IEnumerable<TMessage> errors)
        {
            Model = model;
            Success = success;
            Errors = errors;
        }

    }
}
