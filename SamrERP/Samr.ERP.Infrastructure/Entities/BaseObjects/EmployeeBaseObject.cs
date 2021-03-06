﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Samr.ERP.Infrastructure.Interfaces;

namespace Samr.ERP.Infrastructure.Entities.BaseObjects
{
    public abstract class EmployeeBaseObject : CreatableByUserBaseObject, ICreatable, IActivable
    {
        [Required]
        [StringLength(32)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(32)]
        public string LastName { get; set; }

        [StringLength(32)]
        public string MiddleName { get; set; }

        [Required]
        public Guid PositionId { get; set; }
        public Position Position { get; set; }
        
        [StringLength(32)]
        public string ImageName { get; set; }

        [Required]
        public Guid GenderId { get; set; }
        [ForeignKey(nameof(GenderId))]
        public Gender Gender { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [StringLength(9)]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime HireDate { get; set; }

        [StringLength(256, ErrorMessage = "Description length must be not more 256")]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [StringLength(128)]
        public string Email { get; set; }

        [StringLength(256)]
        public string FactualAddress { get; set; }
 
        public DateTime? LockDate { get; set; }

        public Guid? EmployeeLockReasonId { get; set; }
        [ForeignKey(nameof(EmployeeLockReasonId))]
        public EmployeeLockReason EmployeeLockReason { get; set; }
       
        public Guid? LockUserId { get; set; }
        [ForeignKey(nameof(LockUserId))]
        public User LockUser { get; set; }

        [StringLength(32)]
        public string PassportNumber { get; set; }

        [StringLength(64)]
        public string PassportIssuer { get; set; }

        public DateTime? PassportIssueDate { get; set; }

        public Guid? NationalityId { get; set; }
        [ForeignKey(nameof(NationalityId))]
        public Nationality Nationality { get; set; }

        [StringLength(256)]
        public string PassportAddress { get; set; }
        
        public DateTime CreatedAt { get; set; }

        public bool IsActive { get; set; }

        public Guid? UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        public string PhotoPath { get; set; }
        public string PassportScanPath { get; set; }

        public string FullName()
        {
            var fullName = $"{LastName} {FirstName}";
            return !string.IsNullOrEmpty(MiddleName) ? $"{fullName} {MiddleName}" : $"{fullName}";
        }
    }
}