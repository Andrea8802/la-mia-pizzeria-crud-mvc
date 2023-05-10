using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace la_mia_pizzeria_static.Models
{
    [Table("ingredient")]
    public class Ingredient
    {
        [Key]
        public int Id { get; set; }

        public string Nome { get; set; }

        public List<Pizza> Pizze { get; set; }
    }
}
