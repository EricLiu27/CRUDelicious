using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CRUDelicious.Models;

namespace CRUDelicious.Controllers;

public class DishController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private MyContext _context;

    public DishController(ILogger<HomeController> logger, MyContext context)
    {
        _logger = logger;
        _context = context;
    }
    // This is where you can create a dish
    [HttpGet("dishes/new")]
    public ViewResult NewDish()
    {
        return View();
    }
    // processing the creating of the dish and redirects to the view all dishes on the home page
    [HttpPost("dishes/create")]
    public IActionResult CreateDish(Dish newDish)
    {
        if (!ModelState.IsValid)
        {
            return View("NewDish");
        }
        _context.Add(newDish);
        _context.SaveChanges();
        return RedirectToAction("Index", "Home");
    }
    // This is the view one page where you can click the link on the home page to get here. 
    [HttpGet("dishes/{dishId}")]
    public IActionResult ViewDish(int dishId)
    {
        Dish? SingleDish = _context.Dishes.FirstOrDefault(d => d.DishId == dishId);
        if (SingleDish == null)
        {
            return RedirectToAction("Index", "Home");
        }
        return View(SingleDish);
    }
    //This is the edit page where you can click the button on the view one page. 
    [HttpGet("dishes/{dishId}/edit")]
    public IActionResult EditDish(int dishId)
    {
        Dish? Edited = _context.Dishes.FirstOrDefault(d => d.DishId == dishId);
        if (Edited == null)
        {
            return RedirectToAction("Index", "Home");
        }
        return View(Edited);
    }

    [HttpPost("dishes/{dishId}/update")]
    public IActionResult UpdateDish(int dishId, Dish editedDish)
    {
        Dish? Updated = _context.Dishes.FirstOrDefault(d => d.DishId == dishId);
        if (!ModelState.IsValid || Updated == null)
        {
            return View("EditDish", Updated);
        }
        Updated.Name = editedDish.Name;
        Updated.Chef = editedDish.Chef;
        Updated.Tastiness = editedDish.Tastiness;
        Updated.Calories = editedDish.Calories;
        Updated.Description = editedDish.Description;
        Updated.UpdatedAt = DateTime.Now;

        _context.SaveChanges();

        return RedirectToAction("ViewDish", new { dishId = dishId });
    }



    //This is the delete page that is clicked on the view one page and where it gets processed and redireted back to the view all home page. 
    [HttpPost("dishes/{dishId}/delete")]
    public IActionResult DeleteDish(int dishId)
    {
        Dish? Deleted = _context.Dishes.SingleOrDefault(d => d.DishId == dishId);
        if (Deleted != null)
        {
            _context.Remove(Deleted);
            _context.SaveChanges();
        }
        return RedirectToAction("Index", "Home");
    }




    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
