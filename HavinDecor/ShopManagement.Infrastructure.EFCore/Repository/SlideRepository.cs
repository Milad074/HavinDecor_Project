using System.Collections.Generic;
using System.Linq;
using _0_Framework.Infrastructure;
using Microsoft.EntityFrameworkCore;
using ShopManagement.Application.Contracts.Slide;
using ShopManagement.Domain.SlideAgg;

namespace ShopManagement.Infrastructure.EFCore.Repository
{
    public class SlideRepository :RepositoryBase<long , Slide> , ISlideRepository
    {
        private readonly ShopContext _context;

        public SlideRepository(ShopContext context) :base(context)
        {
            _context = context;
        }

        public EditSlide GetDetails(long id)
        {
            return _context.Slides.Select(x => new EditSlide
            {
                Id = x.Id,
                Picture = x.Picture,
                PictureAlt = x.PictureAlt,
                PictureTitle = x.PictureTitle,
                Heading = x.Heading,
                Title = x.Title,
                Text = x.Text,
                BtnText = x.BtnText,
                BtnColor = x.BtnColor
            }).FirstOrDefault(x => x.Id == id);
        }

        public List<SlideViewModel> Search(SlideSearchModel searchModel)
        {
            var query = _context.Slides.Select(x => new SlideViewModel
            {
                Id = x.Id,
                CreationDate = x.CreationDate.ToString(),
                Heading = x.Heading,
                Picture = x.Picture,
                Title = x.Title,
                IsRemoved = x.IsRemoved
            });

            if (!string.IsNullOrWhiteSpace(searchModel.Heading))
            {
                query = query.Where(x => x.Heading.Contains(searchModel.Heading));
            }

            if (!string.IsNullOrWhiteSpace(searchModel.Title))
            {
                query = query.Where(x => x.Title.Contains(searchModel.Title));
            }

            return query.OrderByDescending(x => x.Id).ToList();
        }
    }
}
