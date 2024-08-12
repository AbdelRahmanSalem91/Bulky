using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private IUnitOfWork _unitOfWork;
        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            List<Product> categories = _unitOfWork.Product.GetAll().ToList();
            return View(categories);
        }

        public IActionResult Create()
        {
            ProductVM productsVM = new()
            {
                CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem { Text = u.Name, Value = u.Id.ToString()}),
                Product = new Product()
            };

            return View(productsVM);
        }

        [HttpPost]
        public IActionResult Create(ProductVM ProductVm)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Add(ProductVm.Product);
                _unitOfWork.Save();
                TempData["success"] = "Product Created Successfully";
                return RedirectToAction("Index");
            }
            else
            {
                ProductVm.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem { Text = u.Name, Value = u.Id.ToString() });
				return View(ProductVm);
			}
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Product? Product = _unitOfWork.Product.Get(c => c.Id == id);

            if (Product == null)
            {
                return NotFound();
            }

            return View(Product);
        }

        [HttpPost]
        public IActionResult Edit(Product Product)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Update(Product);
                _unitOfWork.Save();
                TempData["success"] = "Product Updated Successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Product? Product = _unitOfWork.Product.Get(c => c.Id == id);

            if (Product == null)
            {
                return NotFound();
            }

            return View(Product);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            Product? Product = _unitOfWork.Product.Get(cat => cat.Id == id);
            if (Product == null)
            {
                return NotFound();
            }

            _unitOfWork.Product.Remove(Product);
            _unitOfWork.Save();
            TempData["success"] = "Product Deleted Successfully";
            return RedirectToAction("Index");
        }
    }
}
