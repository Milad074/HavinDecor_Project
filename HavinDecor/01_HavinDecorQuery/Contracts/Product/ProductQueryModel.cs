﻿using System.Collections.Generic;

namespace _01_HavinDecorQuery.Contracts.Product
{
    public class ProductQueryModel
    {
        public long Id { get; set; }

        public string Picture { get; set; }
        public string PictureAlt { get; set; }
        public string PictureTitle { get; set; }
        public string Price { get; set; }
        public string PriceWithDiscount { get; set; }
        public int DiscountRate { get; set; }
        public bool HasDiscount { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Slug { get; set; }
        public string EndDate { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string CategorySlug { get; set; }
        public string Material { get;  set; }
        public string Code { get;  set; }
        public string KeyWords { get; set; }
        public string MetaDescription { get; set; }

        public List<ProductPictureQueryModel> ProductPictures { get; set; }
    }
}
