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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            List<Product> productsList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return View(productsList);
        }

        public IActionResult Upsert(int? Id)
        {
            ProductVM productsVM = new()
            {
                CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem { Text = u.Name, Value = u.Id.ToString()}),
                Product = new Product()
            };

            if (Id == null || Id == 0)
            {
				return View(productsVM);
			}
            else
            {
                productsVM.Product = _unitOfWork.Product.Get(u => u.Id == Id);
                return View(productsVM);
            }
        }

        [HttpPost]
        public IActionResult Upsert(ProductVM ProductVm, IFormFile? file)
        {
            if (ModelState.IsValid)
			{
				string wwwRootPath = _webHostEnvironment.WebRootPath;
				if (file is not null)
				{
					string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
					string productPath = Path.Combine(wwwRootPath, @"images/product");

					if (!string.IsNullOrEmpty(ProductVm.Product.ImageUrl))
					{
						var oldImagePath = Path.Combine(wwwRootPath, ProductVm.Product.ImageUrl.TrimStart('/'));

						if (System.IO.File.Exists(oldImagePath))
						{
							System.IO.File.Delete(oldImagePath);
						}
					}

					using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
					{
						file.CopyTo(fileStream);
					}
					ProductVm.Product.ImageUrl = @"/images/product/" + fileName;
				}

                if (ProductVm.Product.Id == 0)
                {
					_unitOfWork.Product.Add(ProductVm.Product);
				}
                else
                {
                    _unitOfWork.Product.Update(ProductVm.Product);
                }
				
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
        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> productsList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return Json(new { data = productsList});
        } 
        #endregion
    }
}
