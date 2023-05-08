using la_mia_pizzeria_static.Models;
using Microsoft.AspNetCore.Mvc;

namespace la_mia_pizzeria_static.Controllers
{
    public class PizzaController : Controller
    {

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

        public IActionResult Details(long id)
        {
            using (PizzaContext db = new PizzaContext())
            {
                Pizza pizza = db.Pizza.Where(pizza => pizza.Id == id).FirstOrDefault();

                if (pizza == null)
                    return View("Error", "Nessuna pizza trovata con questo ID!");

                return View(pizza);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Pizza data)
        {
            if(!ModelState.IsValid)
                return View(data);

            using (PizzaContext db = new PizzaContext())
            {
                Pizza newPizza = new Pizza();
                newPizza.Nome = data.Nome;
                newPizza.Descrizione = data.Descrizione;
                newPizza.Prezzo = data.Prezzo;
                newPizza.Img = data.Img;

                db.Pizza.Add(newPizza);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

    }
}

   
