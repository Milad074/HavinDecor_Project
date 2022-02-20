using System.Collections.Generic;

namespace _01_HavinDecorQuery.Contracts.Product
{
    public interface IProductQuery
    {
        List<ProductQueryModel> GetLatestArrivals();
        List<ProductQueryModel> Search(string value);
        ProductQueryModel GetDetails(string slug);
    }
}
