using System.ComponentModel.DataAnnotations.Schema;

namespace la_mia_pizzeria_static.Models
{
    [Table("Category")]
    public class Category
    {
            public long Id { get; set; }
            public string Nome { get; set; }
            public List<Pizza> Pizze { get; set; }
    }
}