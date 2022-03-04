using System.Collections.Generic;
using _01_HavinDecorQuery.Contracts.ArticleCategory;
using _01_HavinDecorQuery.Contracts.productCategory;

namespace _01_HavinDecorQuery
{
    public class MenuModel
    {
        public List<ArticleCategoryQueryModel> ArticleCategories { get; set; }
        public List<ProductCategoryQueryModel> ProductCategories { get; set; }

    }
}
