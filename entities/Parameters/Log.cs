using System.ComponentModel.DataAnnotations.Schema;

namespace entities
{
    public class Log : Base
    {

        [Column(TypeName = "varchar(255)")]
        public string? Process { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string? Type { get; set; }

        [Column(TypeName = "int")]
        public int? EventId { get; set; }

        [Column(TypeName = "text")]
        public string? Message { get; set; }

        [Column(TypeName = "text")]
        public string? Error { get; set; }

        [NotMapped]
        public DateTime? LoggedDateStart { get; set; }

        [NotMapped]
        public DateTime? LoggedDateEnd { get; set; }

    }
}
