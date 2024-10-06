using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace entities
{
    public class Base
    {
        [Key]
        [Column(TypeName = "char(36)")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Column(TypeName = "varchar(255)")]
        public string? RecordUser { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? RecordDate { get; set; }
    }
}
