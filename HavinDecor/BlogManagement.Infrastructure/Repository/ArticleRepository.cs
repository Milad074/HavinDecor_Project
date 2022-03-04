using System;
using System.Collections.Generic;
using System.Linq;
using _0_Framework.Application;
using _0_Framework.Infrastructure;
using BlogManagement.Application.Contracts.Article;
using BlogManagement.Domain.ArticleAgg;
using Microsoft.EntityFrameworkCore;

namespace BlogManagement.Infrastructure.EFCore.Repository
{
    public class ArticleRepository:RepositoryBase<long , Article> , IArticleRepository
    {
        private readonly BlogContext _context;

        public ArticleRepository(BlogContext context) : base(context)
        {
            _context = context;
        }

        public EditArticle GetDetails(long id)
        {
            return _context.Articles.Select(x => new EditArticle
            {
                Id = x.Id,
                CategoryId = x.CategoryId,
                Title = x.Title,
                PictureAlt = x.PictureAlt,
                PictureTitle = x.PictureTitle,
                ShortDescription = x.ShortDescription,
                Description = x.Description,
                Slug = x.Slug,
                MetaDescription = x.MetaDescription,
                Keywords = x.Keywords,
                CanonicalAddress = x.CanonicalAddress,
                PublishingDate = x.PublishingDate.ToFarsi(),
            }).FirstOrDefault(x => x.Id == id);
        }

        public Article GetArticleWithCategory(long id)
        {
            return _context.Articles.Include(x => x.Category)
                .FirstOrDefault(x => x.Id == id);
        }

        public List<ArticleViewModel> Search(ArticleSearchModel searchModel)
        {
            var query = _context.Articles
                .Select(x => new ArticleViewModel
                {
                    Id = x.Id,
                    ArticleCategory = x.Category.Name,
                    PublishingDate = x.PublishingDate.ToFarsi(),
                    ShortDescription = x.ShortDescription.Substring(0 , Math.Min(x.ShortDescription.Length , 40)) + "...",
                    Title = x.Title,
                    Picture = x.Picture,
                    CategoryId = x.CategoryId
                });
            if (!string.IsNullOrWhiteSpace(searchModel.Title))
            {
                query = query.Where(x => x.Title.Contains(searchModel.Title));
            }

            if (searchModel.CategoryId !=0)
            {
                query = query.Where(x => x.CategoryId == searchModel.CategoryId);
            }

            return query.OrderByDescending(x => x.Id).ToList();
        }
    }
}
