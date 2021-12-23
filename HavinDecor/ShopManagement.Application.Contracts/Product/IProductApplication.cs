using System.Collections.Generic;
using _0_Framework.Application;

namespace ShopManagement.Application.Contracts.Product
{
    public interface IProductApplication
    {
        OperationResult Create(CreateProduct command);

        OperationResult Edit(EditProduct command);

        OperationResult IsStock(long id);

        OperationResult NOtInStock(long id);

        EditProduct GetDetails(long id);

        List<ProductViewModel> Search(ProductSearchModel searchModel);
    }
}
