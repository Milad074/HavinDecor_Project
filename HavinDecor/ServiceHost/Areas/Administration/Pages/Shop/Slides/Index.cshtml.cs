using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShopManagement.Application.Contracts.Slide;

namespace ServiceHost.Areas.Administration.Pages.Shop.Slides
{
    public class IndexModel : PageModel
    {
        [TempData] public string Message { get; set; }

        public SlideSearchModel SearchModel;
        public List<SlideViewModel> Slides;


        private readonly ISlideApplication _slideApplication;

        public IndexModel(ISlideApplication slideApplication)
        {
            _slideApplication = slideApplication;
        }

        public void OnGet(SlideSearchModel searchModel)
        {
            Slides = _slideApplication.Search(searchModel);
        }

        public IActionResult OnGetCreate()
        {
            return Partial("./Create", new CreateSlide());
        }

        public JsonResult OnPostCreate(CreateSlide command)
        {
            var result = _slideApplication.Create(command);

            return new JsonResult(result);
        }

        public IActionResult OnGetEdit(long id)
        {
            var slide = _slideApplication.GetDetails(id);

            return Partial("./Edit", slide);
        }

        public JsonResult OnPostEdit(EditSlide command)
        {
            if (ModelState.IsValid)
            {
                
            }

            var result = _slideApplication.Edit(command);

            return new JsonResult(result);
        }

        public IActionResult OnGetRemove(long id)
        {
            var result = _slideApplication.Remove(id);

            if (result.IsSuccedded)
            {
                return RedirectToPage("./Index");
            }

            Message = result.Message;
            return RedirectToPage("./Index");

        }

        public IActionResult OnGetRestore(long id)
        {
            var result = _slideApplication.Restore(id);


            if (result.IsSuccedded)
            {
                return RedirectToPage("./Index");
            }

            Message = result.Message;
            return RedirectToPage("./Index");
        }
    }
}
