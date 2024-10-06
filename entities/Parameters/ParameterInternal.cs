using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace entities
{
    public class ParameterInternal : Base
    {

        //[Display(Name = nameof(Name), ResourceType = typeof(ParameterInternal))]

        [Column(TypeName = "varchar(255)")]
        public string? Name { get; set; }

        //[Display(Name = nameof(Value), ResourceType = typeof(ParameterInternal))]

        [Column(TypeName = "varchar(1000)")]
        public string? Value { get; set; }

    }
}
