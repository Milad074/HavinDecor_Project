using System.ComponentModel;
using _0_Framework.Application;
using System.ComponentModel.DataAnnotations;

namespace ShopManagement.Application.Contracts.Material
{
    public class CreateMaterial
    {
        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        [DisplayName("جنس پارچه")]
        public string MaterialName { get; set; }

        [Range(1 , 999999999 , ErrorMessage = ValidationMessages.IsRequired)]
        public double Price { get; set; }

        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        [DisplayName("سمت پنل")]
        public string Panel { get; set; }

        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        [DisplayName("رنگ حلقه پانچ")]
        public string RingColor  { get; set; }
    }
}
