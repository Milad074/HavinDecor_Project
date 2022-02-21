using System.Collections.Generic;
using _0_Framework.Application;

namespace ShopManagement.Application.Contracts.Material
{
    public interface IMaterialApplication
    {
        OperationResult Create(CreateMaterial command);

        OperationResult Edit(EditMaterial command);
        EditMaterial GetDetails(long id);
        List<MaterialViewModel> ViewModel();
    }
}
