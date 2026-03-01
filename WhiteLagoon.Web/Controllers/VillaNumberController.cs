using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;
using WhiteLagoon.Web.ViewModels;

namespace WhiteLagoon.Web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public VillaNumberController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var villaNumbers = _unitOfWork.VillaNumber.GetAll(includeProperties: "Villa");
            return View(villaNumbers);
        }
        public IActionResult Create()
        {
            VillaNumberVm villaNumberVm = new()
            {
                VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
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

            bool isNumberUnique = _unitOfWork.VillaNumber.GetAll().Any(u => u.Villa_Number == obj.VillaNumber.Villa_Number);

            if (ModelState.IsValid && !isNumberUnique)
            {
                _unitOfWork.VillaNumber.Add(obj.VillaNumber);
                _unitOfWork.Save();

                TempData["success"] = "Villa Number created successfully";
                return RedirectToAction("Index");
            }

            if (isNumberUnique)
            {
                TempData["error"] = "Villa Number already exists. Please choose a unique number.";
            }

            obj.VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
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
                VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                VillaNumber = _unitOfWork.VillaNumber.Get(u => u.Villa_Number == villaNumberId)
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
                _unitOfWork.VillaNumber.Update(villaNumberVm.VillaNumber);
                _unitOfWork.Save();

                TempData["success"] = "Villa Number updated successfully";
                return RedirectToAction("Index");
            }

            villaNumberVm.VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
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
                VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                VillaNumber = _unitOfWork.VillaNumber.Get(u => u.Villa_Number == villaNumberId)
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
            VillaNumber? objFromDb = _unitOfWork.VillaNumber.Get(u => u.Villa_Number == villaNumberVm.VillaNumber.Villa_Number);

            if (objFromDb is not null)
            {
                _unitOfWork.VillaNumber.Remove(objFromDb);
                _unitOfWork.Save();

                TempData["success"] = "Villa number deleted successfully";
                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = "Error deleting villa number";
            return View();
        }
    }
}
