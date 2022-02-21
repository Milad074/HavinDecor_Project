using System.Collections.Generic;
using System.Linq;
using _0_Framework.Application;
using _0_Framework.Infrastructure;
using Microsoft.EntityFrameworkCore;
using ShopManagement.Application.Contracts.Material;
using ShopManagement.Domain.MaterialAgg;

namespace ShopManagement.Infrastructure.EFCore.Repository
{
    public class MaterialRepository :RepositoryBase<long , Material> , IMaterialRepository
    {
        private readonly ShopContext _context;

        public MaterialRepository(ShopContext context) : base(context)
        {
            _context = context;
        }

        public EditMaterial GetDetails(long id)
        {
            return _context.Materials.Select(x => new EditMaterial 
            {
                Id = x.Id,
                Price = x.Price,
                MaterialName = x.MaterialName,
                Panel = x.Panel,
                RingColor = x.RingColor
            }).FirstOrDefault(x => x.Id == id);
        }

        public List<MaterialViewModel> ViewModel()
        {
            return _context.Materials
                .Select(x=> new MaterialViewModel
                {
                    Id = x.Id,
                    Price = x.Price,
                    MaterialName = x.MaterialName,
                    Panel = x.Panel,
                    CreationDate = x.CreationDate.ToFarsi(),
                    RingColor = x.RingColor
                }).OrderByDescending(x=> x.Id)
                .ToList();
        }
    }
}
