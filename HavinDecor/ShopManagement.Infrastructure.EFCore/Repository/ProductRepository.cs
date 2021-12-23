using System.Collections.Generic;
using System.Linq;
using _0_Framework.Infrastructure;
using Microsoft.EntityFrameworkCore;
using ShopManagement.Application.Contracts.Product;
using ShopManagement.Domain.ProductAgg;

namespace ShopManagement.Infrastructure.EFCore.Repository
{
    public class ProductRepository : RepositoryBase<long , Product> , IProductRepository
    {
        private readonly ShopContext _context;

        public ProductRepository(ShopContext context) : base(context)
        {
            _context = context;
        }

        public EditProduct GetDetails(long id)
        {
            return _context.Products.Select(p => new EditProduct
                {
                    Id = p.Id,
                    Name = p.Name,
                    Code = p.Code,
                    UnitPrice = p.UnitPrice,
                    ShortDescription = p.ShortDescription,
                    Description = p.Description,
                    CategoryId = p.CategoryId,
                    Material = p.Material,
                    Pieces = p.Pieces,
                    Area = p.Area,
                    Picture = p.Picture,
                    PictureAlt = p.PictureAlt,
                    PictureTitle = p.PictureTitle,
                    Slug = p.Slug,
                    Keywords = p.Keywords,
                    MetaDescription = p.MetaDescription
                })
                .FirstOrDefault(p => p.Id == id);
        }

        public List<ProductViewModel> Search(ProductSearchModel searchModel)
        {
            var query = _context.Products
                .Include(p=> p.Category)
                .Select(p => new ProductViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Category = p.Category.Name,
                Code = p.Code,
                CategoryId = p.CategoryId,
                UnitPrice = p.UnitPrice,
                IsInStock = p.IsInStock,
                Picture = p.Picture,
                CreationDate = p.CreationDate.ToString()
            });

            if (!string.IsNullOrWhiteSpace(searchModel.Name))
            {
                query = query.Where(p => p.Name.Contains(searchModel.Name));
            }

            if (!string.IsNullOrWhiteSpace(searchModel.Code))
            {
                query = query.Where(p => p.Code.Contains(searchModel.Code));
            }

            if (searchModel.CategoryId != 0)
            {
                query = query.Where(p => p.CategoryId == searchModel.CategoryId);
            }

            return query.OrderByDescending(p => p.Id).ToList();
        }
    }
}
