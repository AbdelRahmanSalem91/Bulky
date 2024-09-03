using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CompanyController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            List<Company> companysList = _unitOfWork.Company.GetAll().ToList();
            return View(companysList);
        }

        public IActionResult Upsert(int? Id)
        {
            if (Id == null || Id == 0)
            {
				return View(new Company());
			}
            else
            {
                Company company = _unitOfWork.Company.Get(u => u.Id == Id);
                return View(company);
            }
        }

        [HttpPost]
        public IActionResult Upsert(Company company)
        {
            if (ModelState.IsValid)
			{
                if (company.Id == 0)
                {
					_unitOfWork.Company.Add(company);
				}
                else
                {
                    _unitOfWork.Company.Update(company);
                }
				
                _unitOfWork.Save();
                TempData["success"] = "Company Created Successfully";
                return RedirectToAction("Index");
            }
            else
            {
				return View(company);
			}
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Company? company = _unitOfWork.Company.Get(c => c.Id == id);

            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            Company? company = _unitOfWork.Company.Get(cat => cat.Id == id);
            if (company == null)
            {
                return NotFound();
            }

            _unitOfWork.Company.Remove(company);
            _unitOfWork.Save();
            TempData["success"] = "Company Deleted Successfully";
            return RedirectToAction("Index");
        }
    }
}
