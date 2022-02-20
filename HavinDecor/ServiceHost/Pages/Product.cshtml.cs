using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _01_HavinDecorQuery.Contracts.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Pages
{
    public class ProductModel : PageModel
    {
        public ProductQueryModel Product;

        private readonly IProductQuery _productQuery;

        public ProductModel(IProductQuery productQuery)
        {
            _productQuery = productQuery;
        }

        public void OnGet(string id)
        {
            Product = _productQuery.GetDetails(id);
        }
    }
}
