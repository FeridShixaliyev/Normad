using Microsoft.AspNetCore.Mvc;
using Normad.DAL;
using Normad.Models;
using System.Collections.Generic;
using System.Linq;

namespace Normad.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _sql;

        public HomeController(AppDbContext sql)
        {
            _sql = sql;
        }
        public IActionResult Index()
        {
            List<Slider> sliders = _sql.Sliders.ToList();
            return View(sliders);
        }
    }
}
