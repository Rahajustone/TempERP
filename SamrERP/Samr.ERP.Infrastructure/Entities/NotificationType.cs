using Samr.ERP.Infrastructure.Entities.BaseObjects;
using Samr.ERP.Infrastructure.Interfaces;

namespace Samr.ERP.Infrastructure.Entities
{
    public class NotificationType : BaseObject
    {
        public string Name { get; set; }
        public string Message { get; set; }
    }
}