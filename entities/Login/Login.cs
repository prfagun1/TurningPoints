using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace entities
{
    public class Login : Base
    {
        [Display(Name = "Name", ResourceType = typeof(localization.Locales.Resources))]
        [Required(ErrorMessageResourceType = typeof(localization.Locales.Resources), ErrorMessageResourceName = "NameRequired")]

        [Column(TypeName = "varchar(255)")]
        public string Name { get; set; }


        [Column(TypeName = "datetime")]
        public DateTime BirthDate { get; set; }


        [Column(TypeName = "varchar(255)")]
        [EmailAddress(ErrorMessageResourceType = typeof(localization.Locales.Resources), ErrorMessageResourceName = "EmailValid")]
        public string Email { get; set; }


        [Column(TypeName = "varchar(255)")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Column(TypeName = "tinyint(1)")]
        public bool Blocked { get; set; }

        [Column(TypeName = "tinyint(2)")]
        public Enumerators.Permission Permission { get; set; }

        [Column(TypeName = "int")]
        public int InvalidLoginCount { get; set; }

        [Column(TypeName = "varchar(255)")]
        public string Token { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime TokenExpiration { get; set; }

    }
}
