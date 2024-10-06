using System.ComponentModel.DataAnnotations.Schema;

namespace entities
{
    public class Story : Base
    {
        [Column(TypeName = "varchar(255)")]
        public string Title { get; set; }

        [Column(TypeName = "varchar(255)")]
        public string Titulo { get; set; }

        [Column(TypeName = "varchar(5000)")]
        public string Description { get; set; }

        [Column(TypeName = "varchar(5000)")]
        public string Descricao { get; set; }

        [Column(TypeName = "varchar(1000)")]
        public string Tags { get; set; }

        [Column(TypeName = "int")]
        public int RecommendedAge { get; set; }

        public byte[]? Image { get; set; }
        public virtual ICollection<Chapter>? Chapter { get; set; }
    }
}
