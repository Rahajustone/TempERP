using System;
using System.Collections.Generic;
using System.Text;
using Samr.ERP.Infrastructure.Interfaces;

namespace Samr.ERP.Infrastructure.Entities
{
    public class RefreshToken:BaseObjects.BaseObject,ICreatable,IActivable
    {
        public string Token { get; set; }
        public DateTime Expires { get; set; }
        public Guid UserId { get; set; }
        public bool Active => DateTime.UtcNow <= Expires;
        public string RemoteIpAddress { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
    }
}
