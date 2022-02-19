using System;
using System.Collections.Generic;
using System.Linq;
using _0_Framework.Application;
using _01_HavinDecorQuery.Contracts.Product;
using _01_HavinDecorQuery.Contracts.productCategory;
using DiscountManagement.Infrastructure.EFCore;
using InventoryManagement.Infrastructure.EFCore;
using Microsoft.EntityFrameworkCore;
using ShopManagement.Domain.ProductAgg;
using ShopManagement.Infrastructure.EFCore;

namespace _01_HavinDecorQuery.Query
{
    public class ProductCategoryQuery:IProductCategoryQuery
    {
        private readonly ShopContext _context;

        private readonly InventoryContext _inventoryContext;

        private readonly DiscountContext _discountContext;

        public ProductCategoryQuery(ShopContext context, InventoryContext inventoryContext, DiscountContext discountContext)
        {
            _context = context;
            _inventoryContext = inventoryContext;
            _discountContext = discountContext;
        }

        public List<ProductCategoryQueryModel> GetProductCategories()
        {
            return _context.ProductCategories
                .Where(x=> x.ParentId == null)
                .Select(x=> new ProductCategoryQueryModel
            {
                Id = x.Id,
                Name = x.Name,
                //Picture = x.Picture,
                PictureAlt = x.PictureAlt,
                PictureTitle = x.PictureTitle,
                Slug = x.Slug
            }).AsNoTracking().ToList();

        }

        public List<ProductCategoryQueryModel> GetProductCategoriesWithProducts()
        {
            var inventory = _inventoryContext.Inventory
                .Select(x => new {x.ProductId, x.UnitPrice}).ToList();

            var discounts = _discountContext.CustomerDiscounts
                .Where(x=> x.StartDate < DateTime.Now && x.EndDate > DateTime.Now)
                .Select(x => new {x.ProductId, x.DiscountRate}).ToList();

            var categories = _context.ProductCategories
                .Include(x => x.Products)
                .ThenInclude(x => x.Category)
                .Where(x => x.ParentId == null)
                .Select(x => new ProductCategoryQueryModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Products = MapProduct(x.Products)
                }).AsNoTracking().ToList();

            foreach (var category in categories)
            {
                foreach (var product in category.Products)
                {
                    var productInventory = inventory.FirstOrDefault(x => x.ProductId == product.Id);
                    if (productInventory != null)
                    {
                        var price = productInventory.UnitPrice;
                        product.Price = price.ToMoney();

                        var discount =
                            discounts.FirstOrDefault(x => x.ProductId == product.Id);
                        if (discount != null)
                        {
                            int productDiscount = discount.DiscountRate;
                            product.DiscountRate = productDiscount;
                            product.HasDiscount = productDiscount > 0;

                            var discountAmount = Math.Round((price * productDiscount) / 100);

                            product.PriceWithDiscount = (price - discountAmount).ToMoney();
                        }
                    }
                }
            }


            return categories;
        }

        public ProductCategoryQueryModel GetProductCategoryWithProductsBy(string slug)
        {
            var inventory = _inventoryContext.Inventory
                .Select(x => new { x.ProductId, x.UnitPrice }).ToList();

            var discounts = _discountContext.CustomerDiscounts
                .Where(x => x.StartDate < DateTime.Now && x.EndDate > DateTime.Now)
                .Select(x => new { x.ProductId, x.DiscountRate , x.EndDate }).ToList();

            var category = _context.ProductCategories
                .Include(x => x.Products)
                .ThenInclude(x => x.Category)
                .Where(x=> x.ParentId == null)
                .Select(x => new ProductCategoryQueryModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    MetaDescription = x.MetaDescription,
                    KeyWords = x.Keywords,
                    Slug = x.Slug,
                    Products = MapProduct(x.Products)
                }).AsNoTracking().FirstOrDefault(x => x.Slug == slug);
            
                foreach (var product in category.Products)
                {
                    var productInventory = inventory.FirstOrDefault(x => x.ProductId == product.Id);
                    if (productInventory != null)
                    {
                        var price = productInventory.UnitPrice;
                        product.Price = price.ToMoney();

                        var discount =
                            discounts.FirstOrDefault(x => x.ProductId == product.Id);
                        if (discount != null)
                        {
                            int productDiscount = discount.DiscountRate;
                            product.DiscountRate = productDiscount;
                            product.EndDate = discount.EndDate.ToDiscountFormat();
                            product.HasDiscount = productDiscount > 0;

                            var discountAmount = Math.Round((price * productDiscount) / 100);

                            product.PriceWithDiscount = (price - discountAmount).ToMoney();
                        }
                    }
                }
                return category;
        }

        private static List<ProductQueryModel> MapProduct(List<Product> products)
        {
            return products.Select(product => new ProductQueryModel
            {
                Id = product.Id,
                Name = product.Name,
                Picture = product.Picture,
                PictureAlt = product.PictureAlt,
                PictureTitle = product.PictureTitle,
                Category = product.Category.Name,
                Slug = product.Slug
            }).ToList();
        }
    }
}
