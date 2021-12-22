using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using _0_Framework.Infrastructure;
using ShopManagement.Application.Contracts.ProductCategory;
using ShopManagement.Domain.ProductCategoryAgg;

namespace ShopManagement.Infrastructure.EFCore.Repository
{
    public class ProductCategoryRepository : RepositoryBase<long , ProductCategory> , IProductCategoryRepository
    {
        private readonly ShopContext _context;

        public ProductCategoryRepository(ShopContext context) :base(context)
        {
            _context = context;
        }

        public EditProductCategory GetDetails(long id)
        {
            return _context.ProductCategories.Select(pc => new EditProductCategory()
            {
                Id = pc.Id,
                Name = pc.Name,
                Description = pc.Description,
                Picture = pc.Picture,
                PictureAlt = pc.PictureAlt,
                PictureTitle = pc.PictureTitle,
                Keywords = pc.Keywords,
                MetaDescription = pc.MetaDescription,
                Slug = pc.Slug
            }).FirstOrDefault(pc => pc.Id == id);
        }

        public List<ProductCategoryViewModel> Search(ProductCategorySearchModel searchModel)
        {
            var query = _context.ProductCategories.Select(pc => new ProductCategoryViewModel()
            {
                Id = pc.Id,
                Name = pc.Name,
                Picture = pc.Picture,
                CreationDate = pc.CreationDate.ToString()
            });

            if (!string.IsNullOrWhiteSpace(searchModel.Name))
            {
                query = query.Where(pc => pc.Name.Contains(searchModel.Name));
            }

            return query.OrderByDescending(pc => pc.Id).ToList();
        }

        
    }
}
