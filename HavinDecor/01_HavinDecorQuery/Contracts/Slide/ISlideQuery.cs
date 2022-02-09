using System.Collections.Generic;

namespace _01_HavinDecorQuery.Contracts.Slide
{
    public interface ISlideQuery
    {
        List<SlideQueryModel> GetSlides();
    }
}
