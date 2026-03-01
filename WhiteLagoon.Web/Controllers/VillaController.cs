using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly IVillaRepository _villaRepo;

        public VillaController(IVillaRepository villaRepo)
        {
            _villaRepo = villaRepo;
        }

        public IActionResult Index()
        {
            var villas = _villaRepo.GetAll();
            return View(villas);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Villa obj)
        {
            if (obj.Name == obj.Description)
            {
                ModelState.AddModelError("Description", "The Description cannot exactly match the Name.");
            }

            if (ModelState.IsValid)
            {
                _villaRepo.Add(obj);
                _villaRepo.Save();

                TempData["success"] = "Villa created successfully";
                return RedirectToAction("Index");
            }

            return View();
        }

        public IActionResult Edit(int? Id)
        {
            Villa obj = _villaRepo.Get(u => u.Id == Id);

            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        [HttpPost]
        public IActionResult Edit(Villa obj)
        {
            if (obj.Name == obj.Description)
            {
                ModelState.AddModelError("Description", "The Description cannot exactly match the Name.");
            }
            if (ModelState.IsValid)
            {
                _villaRepo.Update(obj);
                _villaRepo.Save();

                TempData["success"] = "Villa updated successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Delete(int? Id)
        {
            Villa obj = _villaRepo.Get(u => u.Id == Id);

            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        [HttpPost]
        public IActionResult Delete(Villa obj)
        {
            Villa objFromDb = _villaRepo.Get(u => u.Id == obj.Id);

            if (objFromDb is not null)
            {
                _villaRepo.Remove(objFromDb);
                _villaRepo.Save();

                TempData["success"] = "Villa deleted successfully";
                return RedirectToAction("Index");
            }

            TempData["error"] = "Error deleting villa";
            return View();
        }
    }
}
