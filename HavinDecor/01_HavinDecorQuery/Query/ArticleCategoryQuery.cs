using System.Collections.Generic;
using System.Linq;
using _0_Framework.Application;
using _01_HavinDecorQuery.Contracts.Article;
using _01_HavinDecorQuery.Contracts.ArticleCategory;
using BlogManagement.Domain.ArticleAgg;
using BlogManagement.Infrastructure.EFCore;
using Microsoft.EntityFrameworkCore;

namespace _01_HavinDecorQuery.Query
{
    public class ArticleCategoryQuery : IArticleCategoryQuery
    {
        private readonly BlogContext _blogContext;

        public ArticleCategoryQuery(BlogContext blogContext)
        {
            _blogContext = blogContext;
        }

        public List<ArticleCategoryQueryModel> GetArticleCategories()
        {
            return _blogContext.ArticleCategories
                .Include(x => x.Articles)
                .Select(x => new ArticleCategoryQueryModel
                {
                    Name = x.Name,
                    ArticleCount = x.Articles.Count,
                    Picture = x.Picture,
                    PictureAlt = x.PictureAlt,
                    PictureTitle = x.PictureTitle,
                    Slug = x.Slug
                }).ToList();
        }

        public ArticleCategoryQueryModel GetArticleCategoryBySlug(string slug)
        {
            var articleCategory = _blogContext.ArticleCategories
                .Select(x => new ArticleCategoryQueryModel
                {
                    Name = x.Name,
                    Picture = x.Picture,
                    PictureAlt = x.PictureAlt,
                    PictureTitle = x.PictureTitle,
                    Slug = x.Slug,
                    Keywords = x.Keywords,
                    Articles = MapArticle(x.Articles)
                }).FirstOrDefault(x => x.Slug == slug);

            if (articleCategory != null)
            {
                articleCategory.KeywordList = articleCategory.Keywords.Split(",").ToList();
            }
            return articleCategory;
        }

        private static List<ArticleQueryModel> MapArticle(List<Article> articles)
        {
            return articles.Select(x => new ArticleQueryModel
            {
                Id = x.Id,
                Title = x.Title,
                ShortDescription = x.ShortDescription,
                Picture = x.Picture,
                PictureAlt = x.PictureAlt,
                PictureTitle = x.PictureTitle,
                Slug = x.Slug,
                PublishingDate = x.PublishingDate.ToFarsi()
            }).ToList();
        }
    }
}
