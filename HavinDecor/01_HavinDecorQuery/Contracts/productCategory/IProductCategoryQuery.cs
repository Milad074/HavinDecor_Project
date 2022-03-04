using System.Collections.Generic;

namespace _01_HavinDecorQuery.Contracts.productCategory
{
    public interface IProductCategoryQuery
    {
        List<ProductCategoryQueryModel> GetProductCategories();

        List<ProductCategoryQueryModel> GetProductCategoriesWithProducts();

        ProductCategoryQueryModel GetProductCategoryWithProductsBy(string slug);

        List<ProductCategoryQueryModel> GetProductCategoriesWithChild();

    }
}