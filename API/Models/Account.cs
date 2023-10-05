using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Model
{
    public class Account
    {
        [Key]
        public string NIK { get; set; }
        public string Password { get; set; }
        public string OTP { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsUsed { get; set; } = false;
        public Employee Employee { get; set; }
        public Profiling Profiling { get; set; }
    }
}
