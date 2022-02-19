using System.Collections.Generic;
using System.Linq;
using _0_Framework.Application;
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
                    ShortDescription = p.ShortDescription,
                    Description = p.Description,
                    CategoryId = p.CategoryId,
                    Material = p.Material,
                    Pieces = p.Pieces,
                    Area = p.Area,
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
                Picture = p.Picture,
                CreationDate = p.CreationDate.ToFarsi()
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

        public List<ProductViewModel> GetProducts()
        {
            return _context.Products.Select(x => new ProductViewModel
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();
        }

        public Product GetProductWithCategory(long id)
        {
            return _context.Products.Include(x => x.Category)
                .FirstOrDefault(x => x.Id == id);
        }
    }
}
