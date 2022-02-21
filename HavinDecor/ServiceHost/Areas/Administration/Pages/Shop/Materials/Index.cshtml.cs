using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShopManagement.Application.Contracts.Material;
using ShopManagement.Application.Contracts.ProductCategory;

namespace ServiceHost.Areas.Administration.Pages.Shop.Materials
{
    public class IndexModel : PageModel
    {
        public List<MaterialViewModel> Materials;

        private readonly IMaterialApplication _materialApplication;

        public IndexModel(IMaterialApplication materialApplication)
        {
            _materialApplication = materialApplication;
        }

        public void OnGet()
        {
            Materials = _materialApplication.ViewModel();
        }

        public IActionResult OnGetCreate()
        {
            return Partial("./Create",new CreateMaterial());
        }

        public JsonResult OnPostCreate(CreateMaterial command)
        {
            var result = _materialApplication.Create(command);

            return new JsonResult(result);
        }

        public IActionResult OnGetEdit(long id)
        {
            var material = _materialApplication.GetDetails(id);

            return Partial("./Edit", material);
        }

        public JsonResult OnPostEdit(EditMaterial command)
        {
            if (ModelState.IsValid)
            {
                
            }

            var result = _materialApplication.Edit(command);

            return new JsonResult(result);
        }
    }
}
