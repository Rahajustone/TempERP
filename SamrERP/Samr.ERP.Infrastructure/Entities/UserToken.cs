using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Samr.ERP.Infrastructure.Entities
{
    public class ActiveUserToken
    {
        [Key]
        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }
        public User User { get; set; }
        public string Token { get; set; }
    }
}
