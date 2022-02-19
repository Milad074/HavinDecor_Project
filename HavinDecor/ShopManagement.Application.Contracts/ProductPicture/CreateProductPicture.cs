using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using _0_Framework.Application;
using Microsoft.AspNetCore.Http;
using ShopManagement.Application.Contracts.Product;

namespace ShopManagement.Application.Contracts.ProductPicture
{
    public class CreateProductPicture
    {
        public long ProductId { get;  set; }


        [FileExtensionLimitation(new string[]{".jpeg" , ".jpg" , ".png"} , ErrorMessage = ValidationMessages.InvalidFileFormat)]
        [MaxFileSize(2*1024*1024 , ErrorMessage = ValidationMessages.MaxFileSize)]
        public IFormFile Picture { get;  set; }


        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public string PictureAlt { get;  set; }


        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public string PictureTitle { get;  set; }


        [Range(1,10000 , ErrorMessage = ValidationMessages.IsRequired)]
        public List<ProductViewModel> Products { get; set; }

    }
}
