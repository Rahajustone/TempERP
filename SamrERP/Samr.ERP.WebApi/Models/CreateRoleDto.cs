using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Samr.ERP.WebApi.Models
{
    public class CreateRoleDto
    {
        public string Name { get; set; }

        public string Description { get; set; }
        public string  Category { get; set; }

    }
}
