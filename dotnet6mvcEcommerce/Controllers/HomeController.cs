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
            Student student = new Student() { Id = 1, Name = "Quan程式設計" };
            ViewData["Name"] = "ViewData---Quan程式設計";
            ViewBag.name = "ViewBag--Quan程式設計";
            return View(student);
            //return Json(student);
        }

        public IActionResult Privacy()
        {
            //可以寫入文本文件、資料庫、控制台
            _logger.LogError("嚴重錯誤");
            _logger.LogWarning("警告錯誤");
            _logger.LogInformation("在HomeController的Privacy方法內");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
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