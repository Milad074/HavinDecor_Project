using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using _0_Framework.Application;
using ShopManagement.Application.Contracts.ProductCategory;

namespace ShopManagement.Application.Contracts.Product
{
    public class CreateProduct
    {
        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public string Name { get;  set; }

        public string Code { get;  set; }


        [Range(1 , 100000000 , ErrorMessage = ValidationMessages.IsRequired)]
        public double UnitPrice { get;  set; }


        public string Material { get;  set; }


        public string Pieces { get;  set; }


        public string Area { get;  set; }


        public string ShortDescription { get;  set; }

        public string Description { get;  set; }

        public string Picture { get;  set; }

        public string PictureAlt { get;  set; }

        public string PictureTitle { get;  set; }

        [Range(1 , 100000 , ErrorMessage = ValidationMessages.IsRequired)]
        public long CategoryId { get;  set; }

        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public string Slug { get;  set; }

        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public string Keywords { get;  set; }

        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public string MetaDescription { get;  set; }

        public List<ProductCategoryViewModel> Categories { get; set; }
    }
}
