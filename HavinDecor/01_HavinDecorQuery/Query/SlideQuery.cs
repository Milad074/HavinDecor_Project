using System.Collections.Generic;
using System.Linq;
using _01_HavinDecorQuery.Contracts.Slide;
using ShopManagement.Infrastructure.EFCore;

namespace _01_HavinDecorQuery.Query
{
    public class SlideQuery : ISlideQuery
    {
        private readonly ShopContext _shopContext;

        public SlideQuery(ShopContext shopContext)
        {
            _shopContext = shopContext;
        }

        public List<SlideQueryModel> GetSlides()
        {
            return _shopContext.Slides
                .Where(x => x.IsRemoved == false)
                .Select(x => new SlideQueryModel
                {
                    Picture = x.Picture,
                    PictureAlt = x.PictureAlt,
                    PictureTitle = x.PictureTitle,
                    Heading = x.Heading,
                    Link = x.Link,
                    Title = x.Title,
                    Text = x.Text,
                    BtnText = x.BtnText,
                    BtnColor = x.BtnColor
                }).ToList();
        }
    }
}
