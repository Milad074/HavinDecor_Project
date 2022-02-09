using System.Collections.Generic;

namespace _01_HavinDecorQuery.Contracts.productCategory
{
    public interface IProductCategoryQuery
    {
        List<ProductCategoryQueryModel> GetProductCategories();
    }
}