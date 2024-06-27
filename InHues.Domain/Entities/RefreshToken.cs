using InHues.Domain.BaseModels;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InHues.Domain.Entities
{
    public class RefreshToken
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Token { get; set; }
        public string JwtId { get; set; }
        public DateTime Expirydate { get; set; }
        public bool IsUsed { get; set; }
        public bool IsInvalidated { get; set; }
        public string CustomerId { get; set; }
        public bool IsEnabled { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
    }
}
