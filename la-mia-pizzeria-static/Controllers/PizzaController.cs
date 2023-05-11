using la_mia_pizzeria_static.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace la_mia_pizzeria_static.Controllers
{
    public class PizzaController : Controller
    {
        public ICustomLogger Logger;

        public PizzaController(ICustomLogger logger)
        {
            Logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            using (PizzaContext db = new PizzaContext())
            {
                List<Pizza> pizze = db.Pizza.OrderBy(pizza => pizza.Id).ToList<Pizza>();

                if (pizze.Count == 0)
                    return View("Error", "Non ci sono pizze!");

                return View(pizze);
            }

        }

        [HttpGet]
        public IActionResult Details(long id)
        {
            using (PizzaContext db = new PizzaContext())
            {
                Pizza pizza = db.Pizza.Where(pizza => pizza.Id == id).Include(pizza => pizza.Category).Include(pizza => pizza.Ingredients).FirstOrDefault();

                if (pizza == null)
                    return View("Error", "Nessuna pizza trovata con questo ID!");

                return View("Details", pizza);
            }
        }

        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            using (PizzaContext db = new PizzaContext())
            {
                List<Category> categories = db.Categories.ToList();
                PizzaFormModel model = new PizzaFormModel();
                List<Ingredient> ingredients = db.Ingredients.ToList();

                List<SelectListItem> listIngredients = new List<SelectListItem>();


                foreach(Ingredient ingredient in ingredients)
                {
                    listIngredients.Add(new SelectListItem()
                        { Text=ingredient.Nome, Value=ingredient.Id.ToString() });
                }


                model.Pizza = new Pizza();
                model.Categories = categories;
                model.Ingredient = listIngredients;

                return View("Create", model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Create(PizzaFormModel data)
        {
            if (!ModelState.IsValid)
            {
                using(PizzaContext db = new PizzaContext())
                {
                    List<Category> categories = db.Categories.ToList();
                    data.Categories = categories;

                    List<Ingredient> ingredients = db.Ingredients.ToList();
                    List<SelectListItem> listIngredients = new List<SelectListItem>();
                    data.Pizza = new Pizza();

                    foreach (Ingredient ingredient in ingredients)
                    {
                        listIngredients.Add(new SelectListItem()
                        { Text = ingredient.Nome, Value = ingredient.Id.ToString() });
                    }

                    data.Ingredient = listIngredients;

                    return View(data);
                }
            }

            using (PizzaContext db = new PizzaContext())
            {
                Pizza pizza = new Pizza();
                pizza.Nome = data.Pizza.Nome;
                pizza.Descrizione = data.Pizza.Descrizione;
                pizza.Prezzo = data.Pizza.Prezzo;
                pizza.Img = data.Pizza.Img;

                pizza.CategoryId = data.Pizza.CategoryId;

                pizza.Ingredients = new List<Ingredient>();

                if(data.SelectedIngredients != null)
                {
                    foreach(string selectedIngredientId in data.SelectedIngredients)
                    {
                        int ingredientId = int.Parse(selectedIngredientId);
                        Ingredient ingredient = db.Ingredients.Where(ing => ing.Id == ingredientId).FirstOrDefault();
                        pizza.Ingredients.Add(ingredient);
                    }
                }

                db.Pizza.Add(pizza);
                db.SaveChanges();

                Logger.WriteLog($"Elemento '{pizza.Nome}' creato!");


                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        [Authorize]
        public IActionResult Update(long id)
        {
            using (PizzaContext db = new PizzaContext())
            {
                Pizza pizza = db.Pizza.Where(pizza => pizza.Id == id).FirstOrDefault();

                if (pizza == null)
                    return NotFound();

                else
                {
                    List<Category> categories = db.Categories.ToList();

                    PizzaFormModel model = new PizzaFormModel();
                    List<Ingredient> ingredients = db.Ingredients.ToList();
                    List<SelectListItem> listIngredients = new List<SelectListItem>();
                    pizza.Ingredients = new List<Ingredient>();

                    foreach(Ingredient ingredient in ingredients)
                    {
                        listIngredients.Add(new SelectListItem()
                        {
                            Text = ingredient.Nome,
                            Value = ingredient.Id.ToString(),
                            Selected = pizza.Ingredients.Any(pizza => pizza.Id == ingredient.Id)
                        });
                    }

                    model.Pizza = pizza;
                    model.Categories = categories;
                    model.Ingredient = listIngredients;

                    return View(model);
                }
            }
             
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]

        public IActionResult Update(long id, PizzaFormModel data)
        {
            if (!ModelState.IsValid)
            {
                using(PizzaContext db = new PizzaContext())
                {
                    Pizza pizza = db.Pizza.Where(pizza => pizza.Id == id).FirstOrDefault();

                    List<Category> categories = db.Categories.ToList();
                    List<Ingredient> ingredients = db.Ingredients.ToList();
                    List<SelectListItem> listIngredients = new List<SelectListItem>();

                    pizza.Ingredients = new List<Ingredient>();

                    foreach (Ingredient ingredient in ingredients)
                    {
                        listIngredients.Add(new SelectListItem()
                        {
                            Text = ingredient.Nome,
                            Value = ingredient.Id.ToString(),
                            Selected = pizza.Ingredients.Any(pizza => pizza.Id == ingredient.Id)
                        });
                    }

                    data.Categories = categories;
                    data.Ingredient = listIngredients;
                    data.Pizza = pizza;
                    return View("Update", data);
                }
            }

            using(PizzaContext db = new PizzaContext())
            {
                Pizza pizza = db.Pizza.Where(pizza => pizza.Id == id).Include(pizza => pizza.Ingredients).FirstOrDefault();

                if (pizza != null)
                {
                    pizza.Nome = data.Pizza.Nome;
                    pizza.Descrizione = data.Pizza.Descrizione;
                    pizza.Prezzo = data.Pizza.Prezzo;
                    pizza.CategoryId = data.Pizza.CategoryId;

                    if(data.SelectedIngredients != null)
                    {
                        foreach(string selectedIngredientId in data.SelectedIngredients)
                        {
                            int ingredientId = int.Parse(selectedIngredientId);
                            Ingredient ingredient = db.Ingredients.Where(ing => ing.Id == ingredientId).FirstOrDefault();
                            pizza.Ingredients.Add(ingredient);
                        }
                    }

                    db.SaveChanges();

                    Logger.WriteLog($"Elemento '{pizza.Nome}' modificato!");

                    return RedirectToAction("Index");
                }
                else
                {
                    return NotFound();
                }


            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]

        public IActionResult Delete(long id)
        {
            using (PizzaContext db = new PizzaContext())
            {
                Pizza pizza = db.Pizza.Where(pizza => pizza.Id == id).FirstOrDefault();

                if (pizza != null)
                {
                    db.Pizza.Remove(pizza);

                    db.SaveChanges();

                    Logger.WriteLog($"Elemento '{pizza.Nome}' eliminato!");

                    return RedirectToAction("Index");
                }
                else
                {
                    return NotFound();
                }
            }
        }
    }
}

   
