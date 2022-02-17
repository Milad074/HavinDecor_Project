using _01_HavinDecorQuery.Contracts.productCategory;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.ViewComponents
{
    public class MainMenuViewComponent : ViewComponent
    {
        private readonly IProductCategoryQuery _productCategoryQuery;

        public MainMenuViewComponent(IProductCategoryQuery productCategoryQuery)
        {
            _productCategoryQuery = productCategoryQuery;
        }

        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
