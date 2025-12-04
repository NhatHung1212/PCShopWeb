using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopWeb.Data;
using ShopWeb.Models;

namespace ShopWeb.Controllers;

public class HomeController : BaseController
{
    public HomeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager) 
        : base(context, userManager)
    {
    }
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
