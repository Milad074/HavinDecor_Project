using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Security.Cryptography.X509Certificates;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShopManagement.Application.Contracts.Product;
using ShopManagement.Application.Contracts.ProductCategory;

namespace ServiceHost.Areas.Administration.Pages.Shop.Products
{
    public class IndexModel : PageModel
    {
        [TempData] public string Message { get; set; }


        public ProductSearchModel SearchModel;

        public List<ProductViewModel> Products;

        public SelectList ProductCategories;

        public List<ProductCategoryViewModel> SubGroups;

        private readonly IProductApplication _productApplication;

        private readonly IProductCategoryApplication _productCategoryApplication;

        public IndexModel(IProductApplication productApplication , IProductCategoryApplication productCategoryApplication)
        {
            _productApplication = productApplication;

            _productCategoryApplication = productCategoryApplication;
        }


        public void OnGet(ProductSearchModel searchModel)
        {
            ProductCategories = new SelectList(_productCategoryApplication.GetProductCategories(), "Id", "Name");

            Products = _productApplication.Search(searchModel);
        }

        public IActionResult OnGetCreate()
        {
            var command = new CreateProduct()
            {
                Categories = _productCategoryApplication.GetProductCategories(),
                //SubCategories = _productCategoryApplication.GetSubCategories(Convert.ToInt64(SubGroups.First().Id))
            };
            

            return Partial("./Create", command);
        }

        public JsonResult OnPostCreate(CreateProduct command)
        {
            var result = _productApplication.Create(command);

            return new JsonResult(result);
        }

        public IActionResult OnGetEdit(long id)
        {
            var product = _productApplication.GetDetails(id);
            product.Categories = _productCategoryApplication.GetProductCategories();

            return Partial("./Edit", product);
        }

        public JsonResult OnPostEdit(EditProduct command)
        {
            if (ModelState.IsValid)
            {
                
            }
            var result = _productApplication.Edit(command);

            return new JsonResult(result);
        }

        //public IActionResult ONGetSubGroups(int id)
        //{
        //    List<ProductCategoryViewModel> list = new List<ProductCategoryViewModel>()
        //    {
        //        new ProductCategoryViewModel(){Name = "",ParentId = 0}
        //    };
        //    list.AddRange(_productCategoryApplication.GetSubCategories(id));

        //    return Partial("./Create", new SelectList(list, "Value", "Text"));
        //}
    }
}
