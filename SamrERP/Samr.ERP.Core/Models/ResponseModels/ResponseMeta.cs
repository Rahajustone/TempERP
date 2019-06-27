using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Samr.ERP.Core.Models.ResponseModels
{
    public class ResponseMeta<TMessage>
    {
        public ResponseMeta(HttpStatusCode statusCode, IEnumerable<TMessage> errors)
        {
            Errors = errors?.ToList() ?? new List<TMessage>();
            Success = statusCode == HttpStatusCode.OK;
            StatusCode = statusCode;
        }
        public bool Success { get; }
        public HttpStatusCode StatusCode { get; }
        public IList<TMessage> Errors { get; set; }

    }
}