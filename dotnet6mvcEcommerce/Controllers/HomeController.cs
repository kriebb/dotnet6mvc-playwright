using dotnet6mvcEcommerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace dotnet6mvcEcommerce.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index(int? id)
        {
            Student student = new Student() { Id = 1, Name = "StudentAdmin" };
            ViewData["Name"] = "ViewData---StudentAdmin";
            ViewBag.name = "ViewBag--StudentAdmin";
            return View(student);
                    }

        public IActionResult Privacy()
        {
            _logger.LogError("Privacy Error");
            _logger.LogWarning("Privacy Warning");
            _logger.LogInformation("HomeController Privacy Action");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [Route("error")]        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}