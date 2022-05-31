using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ComicsApi.Models
{
    [Table("User")]
    [Microsoft.EntityFrameworkCore.Index(nameof(Login), Name = "Login", IsUnique = true)]
    public partial class User
    {

        [Key]
        [Column("ID", TypeName = "int(11)")]
        public int Id { get; set; }

        [StringLength(40)]
        [MySqlCharSet("utf8")]
        [MySqlCollation("utf8_general_ci")]
        public string Login { get; set; } = null!;

        [StringLength(255)]
        [MySqlCharSet("utf8")]
        [MySqlCollation("utf8_general_ci")]
        public string Password { get; set; } = null!;

        [StringLength(40)]
        [MySqlCharSet("utf8")]
        [MySqlCollation("utf8_general_ci")]
        public string Name { get; set; } = null!;

        [StringLength(10000)]
        [MySqlCharSet("utf8")]
        [MySqlCollation("utf8_general_ci")]
        public string Photo { get; set; } = null!;

        [Column(TypeName = "datetime")] public DateTime LastLog { get; set; }

        [StringLength(100)]
        [MySqlCharSet("utf8")]
        [MySqlCollation("utf8_general_ci")]
        public string Email { get; set; } = null!;

        [Required] public bool? Access { get; set; }
        [Required] public bool? Role { get; set; }
    }
}
