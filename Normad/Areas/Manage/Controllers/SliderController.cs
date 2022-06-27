using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Normad.DAL;
using Normad.Extentions;
using Normad.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Normad.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class SliderController : Controller
    {
        private readonly AppDbContext _sql;
        private readonly IWebHostEnvironment _env;

        public SliderController(AppDbContext sql,IWebHostEnvironment env)
        {
            _sql = sql;
            _env = env;
        }
        public IActionResult Index()
        {
            List<Slider> sliders = _sql.Sliders.ToList();
            return View(sliders);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Slider slider)
        {
            if (!ModelState.IsValid) return View();
            if (slider == null) return NotFound();
            Slider slider1 = new Slider
            {
                PersonName=slider.PersonName,
                Level=slider.Level
            };
            if (slider.ImageFile != null)
            {
                if (!slider.ImageFile.IsImage())
                {
                    ModelState.AddModelError("ImageFile","Sekilin formati duzgun deyil!");
                    return View();
                }
                if (!slider.ImageFile.IsSizeOk(5))
                {
                    ModelState.AddModelError("ImageFile","Sekil 5 mb-dan boyuk ola bilmez!");
                    return View();
                }
                slider1.Image = slider.ImageFile.SaveImage(_env.WebRootPath, "assets/images");
            }
            
            await _sql.Sliders.AddAsync(slider1);
            await _sql.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public IActionResult Edit(int? id)
        {
            Slider slider = _sql.Sliders.Find(id);
            return View(slider);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, Slider slider)
        {
            if (!ModelState.IsValid) return View();
            if (slider == null) return NotFound();
            Slider sliderIn = _sql.Sliders.Find(id);
            if (sliderIn == null) return NotFound();
            if (slider.ImageFile != null)
            {
                if (!slider.ImageFile.IsImage())
                {
                    ModelState.AddModelError("ImageFile", "Sekilin formati duzgun deyil!");
                    return View();
                }
                if (!slider.ImageFile.IsSizeOk(5))
                {
                    ModelState.AddModelError("ImageFile", "Sekil 5 mb-dan boyuk ola bilmez!");
                    return View();
                }
                Helpers.Helper.Delete(_env.WebRootPath, "assets/images", sliderIn.Image);
                sliderIn.Image = slider.ImageFile.SaveImage(_env.WebRootPath, "assets/images");
            }
            sliderIn.PersonName = slider.PersonName;
            sliderIn.Level = slider.Level;
            await _sql.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int? id)
        {
            Slider slider = await _sql.Sliders.FindAsync(id);
            if (slider == null) return View();
            Helpers.Helper.Delete(_env.WebRootPath,"assets/images",slider.Image);
            _sql.Sliders.Remove(slider);
            await _sql.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
