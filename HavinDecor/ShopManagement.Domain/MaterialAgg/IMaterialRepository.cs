using System.Collections.Generic;
using _0_Framework.Domain;
using ShopManagement.Application.Contracts.Material;

namespace ShopManagement.Domain.MaterialAgg
{
    public interface IMaterialRepository : IRepository<long , Material>
    {
        EditMaterial GetDetails(long id);
        List<MaterialViewModel> ViewModel();
    }
}