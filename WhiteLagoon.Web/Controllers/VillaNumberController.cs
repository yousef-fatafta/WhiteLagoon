using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;
using WhiteLagoon.Web.ViewModels;

namespace WhiteLagoon.Web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly ApplicationDbContext _db;

        public VillaNumberController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var villaNumbers = _db.VillaNumbers.Include(u=>u.Villa).ToList();
            return View(villaNumbers);
        }
        public IActionResult Create()
        {
            VillaNumberVm villaNumberVm = new()
            {
                VillaList = _db.Villas.ToList().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                })
            };

            return View(villaNumberVm);
        }

        [HttpPost]
        public IActionResult Create(VillaNumberVm obj)
        {

            bool isNumberUnique = !_db.VillaNumbers.Any(u => u.Villa_Number == obj.VillaNumber.Villa_Number);

            if (ModelState.IsValid && !isNumberUnique)
            {
                _db.VillaNumbers.Add(obj.VillaNumber);
                _db.SaveChanges();

                TempData["success"] = "Villa Number created successfully";
                return RedirectToAction("Index");
            }

            if (isNumberUnique)
            {
                TempData["error"] = "Villa Number already exists. Please choose a unique number.";
            }

            obj.VillaList = _db.Villas.ToList().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });
            return View(obj);
        }

        public IActionResult Edit(int villaNumberId)
        {
            VillaNumberVm villaNumberVm = new()
            {
                VillaList = _db.Villas.ToList().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                VillaNumber = _db.VillaNumbers.FirstOrDefault(u => u.Villa_Number == villaNumberId)
            };

            if (villaNumberVm.VillaNumber == null)
            {
                return NotFound();
            }

            return View(villaNumberVm);
        }

        [HttpPost]
        public IActionResult Edit(VillaNumberVm villaNumberVm)
        {
            if (ModelState.IsValid)
            {
                _db.VillaNumbers.Update(villaNumberVm.VillaNumber);
                _db.SaveChanges();

                TempData["success"] = "Villa Number updated successfully";
                return RedirectToAction("Index");
            }

            villaNumberVm.VillaList = _db.Villas.ToList().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });
            return View(villaNumberVm);
        }

        public IActionResult Delete(int villaNumberId)
        {
            VillaNumberVm villaNumberVm = new()
            {
                VillaList = _db.Villas.ToList().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                VillaNumber = _db.VillaNumbers.FirstOrDefault(u => u.Villa_Number == villaNumberId)
            };

            if (villaNumberVm.VillaNumber == null)
            {
                return NotFound();
            }

            return View(villaNumberVm);
        }

        [HttpPost]
        public IActionResult Delete(VillaNumberVm villaNumberVm)
        {
            VillaNumber? objFromDb = _db.VillaNumbers.FirstOrDefault(u => u.Villa_Number == villaNumberVm.VillaNumber.Villa_Number);

            if (objFromDb is not null)
            {
                _db.VillaNumbers.Remove(objFromDb);
                _db.SaveChanges();

                TempData["success"] = "Villa number deleted successfully";
                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = "Error deleting villa number";
            return View();
        }
    }
}
