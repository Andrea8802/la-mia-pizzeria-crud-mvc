using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;

namespace la_mia_pizzeria_static.Models
{
    public class PizzaFormModel
    {
        public Pizza Pizza { get; set; }
        public List<Category>? Categories { get; set; }
        public List<SelectListItem>? Ingredient { get; set; }
        public List<string> SelectedIngredients { get; set; }
    }
}
