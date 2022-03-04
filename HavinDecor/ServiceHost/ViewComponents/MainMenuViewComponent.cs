using _01_HavinDecorQuery;
using _01_HavinDecorQuery.Contracts.ArticleCategory;
using _01_HavinDecorQuery.Contracts.productCategory;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.ViewComponents
{
    public class MainMenuViewComponent : ViewComponent
    {
        private readonly IProductCategoryQuery _productCategoryQuery;
        private readonly IArticleCategoryQuery _articleCategoryQuery;

        public MainMenuViewComponent(IProductCategoryQuery productCategoryQuery, IArticleCategoryQuery articleCategoryQuery)
        {
            _productCategoryQuery = productCategoryQuery;
            _articleCategoryQuery = articleCategoryQuery;
        }

        public IViewComponentResult Invoke()
        {
            var result = new MenuModel
            {
                ProductCategories = _productCategoryQuery.GetProductCategoriesWithChild(),
                ArticleCategories = _articleCategoryQuery.GetArticleCategories()
            };

            return View(result);
        }
    }
}
