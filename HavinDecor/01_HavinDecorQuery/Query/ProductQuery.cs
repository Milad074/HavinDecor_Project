using System;
using System.Collections.Generic;
using System.Linq;
using _0_Framework.Application;
using _01_HavinDecorQuery.Contracts.Comment;
using _01_HavinDecorQuery.Contracts.Product;
using CommentManagement.Infrastructure.EFCore;
using DiscountManagement.Infrastructure.EFCore;
using InventoryManagement.Infrastructure.EFCore;
using Microsoft.EntityFrameworkCore;
using ShopManagement.Domain.MaterialAgg;
using ShopManagement.Domain.ProductPictureAgg;
using ShopManagement.Infrastructure.EFCore;

namespace _01_HavinDecorQuery.Query
{
    public class ProductQuery : IProductQuery
    {
        private readonly ShopContext _context;

        private readonly InventoryContext _inventoryContext;

        private readonly DiscountContext _discountContext;

        private readonly CommentContext _commentContext;

        public ProductQuery(ShopContext context, InventoryContext inventoryContext,
            DiscountContext discountContext , CommentContext commentContext)
        {
            _context = context;
            _inventoryContext = inventoryContext;
            _discountContext = discountContext;
            _commentContext = commentContext;
        }

        public ProductQueryModel GetDetails(string slug)
        {
            var inventory = _inventoryContext.Inventory
                .Select(x => new { x.ProductId, x.UnitPrice }).ToList();

            var discount = _discountContext.CustomerDiscounts
                .Where(x => x.StartDate < DateTime.Now && x.EndDate > DateTime.Now)
                .Select(x => new { x.ProductId, x.DiscountRate  , x.EndDate});

            var product = _context.Products
                .Include(x=> x.Category)
                .Include(x=> x.ProductPictures)
                .Select(x => new ProductQueryModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Category = x.Category.Name,
                    Picture = x.Picture,
                    PictureAlt = x.PictureAlt,
                    PictureTitle = x.PictureTitle,
                    Slug = x.Slug,
                    CategorySlug = x.Category.Slug,
                    Code = x.Code,
                    Description = x.Description,
                    ShortDescription = x.ShortDescription,
                    KeyWords = x.Keywords,
                    MetaDescription = x.MetaDescription,
                    ProductPictures = MapProductPicture(x.ProductPictures)
                }).FirstOrDefault(x => x.Slug == slug);


            if (product == null)
            {
                return new ProductQueryModel();
            }

            var productInventory = inventory
                .FirstOrDefault(x => x.ProductId == product.Id);

            if (productInventory != null)
            {
                var price = productInventory.UnitPrice;
                product.Price = price.ToMoney();

                

                var discountRate =
                    discount.FirstOrDefault(x => x.ProductId == product.Id);

                if (discountRate != null)
                {
                    product.EndDate = discountRate.EndDate.ToDiscountFormat();
                    int productDiscount = discountRate.DiscountRate;
                    product.DiscountRate = productDiscount;
                    product.HasDiscount = productDiscount > 0;
                    var discountAmount = Math.Round((price * productDiscount) / 100);

                    product.PriceWithDiscount = (price - discountAmount).ToMoney();
                }
            }

            product.Comments = _commentContext.Comments
                .Where(x => !x.IsCanceled)
                .Where(x => x.IsConfirmed)
                .Where(x => x.Type == CommentType.Product)
                .Where(x => x.OwnerRecordId == product.Id)
                .Select(x => new CommentQueryModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Message = x.Message
                }).OrderByDescending(x => x.Id).ToList();


            return product;
        }

        //private static List<MaterialQueryModel> MapMaterial(List<Material> Materials)
        //{
        //    return Materials.Select(x => new MaterialQueryModel
        //    {
        //        Id = x.Id,
        //        Price = x.Price,
        //        MaterialName = x.MaterialName,
        //        Panel = x.Panel,
        //        RingColor = x.RingColor
        //    }).ToList();
        //}


        public List<ProductQueryModel> GetLatestArrivals()
        {
            var inventory = _inventoryContext.Inventory
                .Select(x => new {x.ProductId, x.UnitPrice}).ToList();

            var discount = _discountContext.CustomerDiscounts
                .Where(x => x.StartDate < DateTime.Now && x.EndDate > DateTime.Now)
                .Select(x => new {x.ProductId, x.DiscountRate});

            var products = _context.Products
                .Select(x => new ProductQueryModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Category = x.Category.Name,
                    CategorySlug = x.Category.Slug,
                    Picture = x.Picture,
                    PictureAlt = x.PictureAlt,
                    PictureTitle = x.PictureTitle,
                    Slug = x.Slug
                }).AsNoTracking().OrderByDescending(x=> x.Id).Take(6).ToList();

            foreach (var product in products)
            {
                var productInventory = inventory.FirstOrDefault(x => x.ProductId == product.Id);
                if (productInventory != null)
                {
                    var price = productInventory.UnitPrice;
                    product.Price = price.ToMoney();

                    var discountRate =
                        discount.FirstOrDefault(x => x.ProductId == product.Id);
                    if (discountRate != null)
                    {
                        int productDiscount = discountRate.DiscountRate;
                        product.DiscountRate = productDiscount;
                        product.HasDiscount = productDiscount > 0;

                        var discountAmount = Math.Round((price * productDiscount) / 100);

                        product.PriceWithDiscount = (price - discountAmount).ToMoney();
                    }
                }
            }
            return products;
        }

        public List<ProductQueryModel> Search(string value)
        {
            var inventory = _inventoryContext.Inventory
                .Select(x => new { x.ProductId, x.UnitPrice }).ToList();

            var discounts = _discountContext.CustomerDiscounts
                .Where(x => x.StartDate < DateTime.Now && x.EndDate > DateTime.Now)
                .Select(x => new { x.ProductId, x.DiscountRate, x.EndDate }).ToList();

            var query = _context.Products
                .Include(x => x.Category)
                .Select(x => new ProductQueryModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Picture = x.Picture,
                    PictureAlt = x.PictureAlt,
                    PictureTitle = x.PictureTitle,
                    Category = x.Category.Name,
                    CategorySlug = x.Category.Slug,
                    Slug = x.Slug,
                    ShortDescription = x.ShortDescription
                });

            

            if (!string.IsNullOrWhiteSpace(value))
            {
                query = query
                    .Where(x => x.Name.Contains(value)
                                || x.ShortDescription.Contains(value));
            }

            var products = query.OrderByDescending(x => x.Id).ToList();

            foreach (var product in products)
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
            return products;
        }

        private static List<ProductPictureQueryModel> MapProductPicture(List<ProductPicture> productPictures)
        {
            return productPictures
                .Select(x => new ProductPictureQueryModel
                {
                    ProductId = x.Id,
                    IsRemoved = x.IsRemoved,
                    Picture = x.Picture,
                    PictureAlt = x.PictureAlt,
                    PictureTitle = x.PictureTitle
                }).Where(x => !x.IsRemoved).ToList();
        }

        public List<MaterialQueryModel> GetMaterial()
        {
            return _context.Materials.Select(x => new MaterialQueryModel
            {
                Id = x.Id,
                Price = x.Price,
                MaterialName = x.MaterialName,
                Panel = x.Panel,
                RingColor = x.RingColor
            }).ToList();
        }
    }
}
