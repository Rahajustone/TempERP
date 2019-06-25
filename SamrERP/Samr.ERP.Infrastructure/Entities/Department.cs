﻿using System;

namespace Samr.ERP.Infrastructure.Entities
{
    public class Department
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}