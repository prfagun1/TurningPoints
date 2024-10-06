using System.ComponentModel.DataAnnotations.Schema;

namespace entities
{
    public class Chapter : Base
    {
        public Guid StoryId { get; set; }


        [Column(TypeName = "varchar(255)")]
        public string? Titulo { get; set; }

        [Column(TypeName = "varchar(255)")]
        public string? Title { get; set; }


        [Column(TypeName = "char(36)")]
        public Guid? Choice01 { get; set; }

        [Column(TypeName = "char(36)")]
        public Guid? Choice02 { get; set; }

        [Column(TypeName = "char(36)")]
        public Guid? Choice03 { get; set; }

        [Column(TypeName = "char(36)")]
        public Guid? Choice04 { get; set; }


        [Column(TypeName = "tinyint(1)")]
        public bool? IsEnd { get; set; }

        [Column(TypeName = "tinyint(1)")]
        public bool? IsStart { get; set; }

        [Column(TypeName = "MEDIUMTEXT")]
        public string? Content { get; set; }

        [Column(TypeName = "MEDIUMTEXT")]
        public string? Conteudo { get; set; }

        public byte[]? Badge { get; set; }
        public virtual Story? Story { get; set; }
    }
}


/*
 como fazer o vinculo de um capitulo com o ooutro

como definir os caminhos






*/