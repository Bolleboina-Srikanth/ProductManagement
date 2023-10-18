using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using ProductDetails.Entity;
using ProductDetails.Models;
using ProductDetails.Services;
using System;
using System.Linq;

namespace ProductDetails.Controllers
{
    public class ProductController : Controller
    {

        private readonly ProductService _productService;

        // ProductController constructor
        // Initializes a new instance of the ProductController class.
        // This constructor is used to inject the ProductService dependency.
        // ProductController depends on ProductService to perform various operations on products.
        // <param name="productService">The ProductService instance to be injected.</param>
        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        // GET: ProductController
        public ActionResult Index(string searchString, string sortColumn, string sortDirection, int page=1)
        {
            try
            {
                // Retrieve a list of products from the ProductService
                var productmodel = _productService.GetAllProducts();
                // Applying search filter if searchString is provided

                // Sorting logic
                if (!string.IsNullOrEmpty(sortColumn))
                {
                    switch (sortColumn)
                    {
                        case "Code":
                            productmodel = sortDirection == "asc"
                                ? productmodel.OrderBy(s => s.Code)
                                : productmodel.OrderByDescending(s => s.Code);
                            break;
                        case "Name":
                            productmodel = sortDirection == "asc"
                                ? productmodel.OrderBy(s => s.Name)
                                : productmodel.OrderByDescending(s => s.Name);
                            break;
                            // Add cases for other columns you want to sort by
                    }
                }




                if (!String.IsNullOrEmpty(searchString))
                {
                    productmodel = productmodel.Where(s => s.Code.Contains(searchString)
                                           || s.Name.Contains(searchString)
                                           || s.Description.Contains(searchString)
                                           || s.ExpiryDate.ToString().Contains(searchString)  // Convert non-string property to string
                                           || s.Category.Contains(searchString)
                                           || s.Image.Contains(searchString)
                                           || s.Status.Contains(searchString)
                                           || s.Creationdate.ToString().Contains(searchString)
                                           );
                }
                // Convert ProductEntity objects to ProductModel objects
                var result = productmodel.Select(ProductEntity => new ProductModel
                {
                    ProductId = ProductEntity.ProductId,
                    Code = ProductEntity.Code,
                    Name = ProductEntity.Name,
                    Description = ProductEntity.Description,
                    ExpiryDate = ProductEntity.ExpiryDate,
                    Category = ProductEntity.Category,
                    Image = ProductEntity.Image,
                    Status = ProductEntity.Status,
                    CreationDate = ProductEntity.Creationdate
                });

                //Defining pagination settings
                const int pageSize = 3;
                //Validating and adjust the page number
                if (page < 1)
                {
                    page = 1;
                }
                // Get the total record count
                int recsCount = result.Count();

                // Create a pager object for pagination
                var pager = new Pager(recsCount, page, pageSize);

                // Calculate the number of records to skip
                int recSkip = (page - 1) * pageSize;

                // Extract the data for the current page
                var data = result.Skip(recSkip).Take(pager.PageSize).ToList();

                // Store the pager in ViewBag for use in the view
                this.ViewBag.Pager = pager;



                // Return the view with paginated data
                return View(data);
            }
            catch (Exception ex)
            {

                // Handling exceptions here (e.g., log, show error message)
                ViewBag.ErrorMessage = "An error occurred while processing your request.";
                return View();
                //  throw ex;

            }
        }

        // GET: ProductController/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ProductController/Create
        /// <summary>
        /// Displays the view for creating or editing a product.
        /// </summary>
        /// <param name="id">The ID of the product to edit (0 for create).</param>
        public ActionResult CreateOrEdit(int id)
        {
            // Retrieve the product for deletion
            // Implement the GetProductById method in ProductService
            var product = _productService.GetProductById(id).SingleOrDefault();

            return View(product);
           // return View();
        }

        // POST: ProductController/Create
        /// <summary>
        /// Handles the HTTP POST request for creating a new product.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductModel productModel)
        {
            try
            {
                // Create a new product using the provided model
                _productService.CreateProduct(productModel);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while processing your request.";
                return View();
            }
        
 
        }

        // GET: ProductController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductModel productModel)
        {
                    _productService.UpdateProductWithStoredProcedure(
                        productModel.ProductId,
                        productModel.Code,
                        productModel.Name,
                        productModel.Description,
                        productModel.ExpiryDate,
                        productModel.Category,
                        productModel.Image,
                        productModel.Status,
                        productModel.CreationDate);

              
                return View(productModel);
        }




        /// <summary>
        /// Displays the view for deleting a product based on the provided ID.
        /// </summary>
        /// <param name="id">The ID of the product to delete.</param>
        // GET: ProductController/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                // Retrieve the product for deletion
                // Implement the GetProductById method in ProductService
                var product = _productService.GetProductById(id).SingleOrDefault();

                return View(product);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while processing your request.";
                return View();
            }
        }





        /// <summary>
        /// Handles the HTTP POST request for deleting a product.
        /// </summary>
        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(ProductModel productModel)
        {
            try
            {
               var productId= productModel.ProductId;
                // Delete the product using the provided model
                _productService.DeleteProductWithStoredProcedure(productId);
                return RedirectToAction("Index");
            }
            catch(Exception ex)
              {
                ViewBag.ErrorMessage = "An error occurred while processing your request.";
                return View();
            }
        }
    }
}









