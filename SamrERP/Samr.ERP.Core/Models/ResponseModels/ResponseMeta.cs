using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Samr.ERP.Core.Models.ResponseModels
{
    public class ResponseMeta<TMessage>
    {
        public ResponseMeta(bool success, IEnumerable<TMessage> errors)
        {
            Errors = errors?.ToList() ?? new List<TMessage>();
            Success = success;
            StatusCode = (int)(success ? HttpStatusCode.OK : HttpStatusCode.BadRequest);

        }
        public int StatusCode { get; protected set; }
        public bool Success { get; set; }
        public IList<TMessage> Errors { get; set; }

    }
}