using System;
using System.ComponentModel.DataAnnotations;

namespace Samr.ERP.Core.ViewModels.Position
{
    public class GetAllListPositionViewModel
    {
        public Infrastructure.Entities.Position Position { get; set; }
        public Infrastructure.Entities.Employee Employee { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
}
