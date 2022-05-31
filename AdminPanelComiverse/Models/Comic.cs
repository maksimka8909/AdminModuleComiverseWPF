using System;
using ComicsApi.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
namespace ComicsApi.Models
{
    [Microsoft.EntityFrameworkCore.Index(nameof(IdAuthor), Name = "ID_Author")]
    [Microsoft.EntityFrameworkCore.Index(nameof(IdEditor), Name = "ID_Editor")]
    [Microsoft.EntityFrameworkCore.Index(nameof(Name), Name = "Name", IsUnique = true)]
    public partial class Comic
    {
        public Comic()
        {
            ListOfIssues = new HashSet<ListOfIssue>();
        }

        [Key]
        [Column("ID", TypeName = "int(11)")]
        public int Id { get; set; }
        [MySqlCharSet("utf8")]
        [MySqlCollation("utf8_general_ci")]
        public string Name { get; set; } = null!;
        [StringLength(10000)]
        [MySqlCharSet("utf8")]
        [MySqlCollation("utf8_general_ci")]
        public string Cover { get; set; } = null!;
        public DateTime DateOfIssue { get; set; }
        [StringLength(512)]
        [MySqlCharSet("utf8")]
        [MySqlCollation("utf8_general_ci")]
        public string? Description { get; set; }
        [Column("ID_Author", TypeName = "int(11)")]
        public int? IdAuthor { get; set; }
        [Column("ID_Editor", TypeName = "int(11)")]
        public int? IdEditor { get; set; }

        [ForeignKey(nameof(IdAuthor))]
        [InverseProperty(nameof(Author.Comics))]
        public virtual Author? IdAuthorNavigation { get; set; }
        [ForeignKey(nameof(IdEditor))]
        [InverseProperty(nameof(Editor.Comics))]
        public virtual Editor? IdEditorNavigation { get; set; }
        [InverseProperty(nameof(ListOfIssue.IdComicsNavigation))]
        public virtual ICollection<ListOfIssue> ListOfIssues { get; set; }
    }
}
