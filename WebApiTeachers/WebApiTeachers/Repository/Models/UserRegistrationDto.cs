using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace WebApiTeachers.Repository.Models
{
    [Table("UserRegistrationDto")]
    public partial class UserRegistrationDto
    {
        [Key]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
        public string PhoneNumber { get; set; }
    }
}
