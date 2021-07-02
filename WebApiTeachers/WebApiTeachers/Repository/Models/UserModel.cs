using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace WebApiTeachers.Repository.Models
{
    [Table("userModels")]
    public partial class UserModel
    {
        [Key]
        [Column("ID")]
        public string Id { get; set; }
        [Column("OFFSET")]
        public int Offset { get; set; }
    }
}
