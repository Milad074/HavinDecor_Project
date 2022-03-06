using System;
using System.Collections.Generic;
using System.Linq;
using _0_Framework.Application;
using _01_HavinDecorQuery.Contracts.Article;
using _01_HavinDecorQuery.Contracts.Comment;
using BlogManagement.Infrastructure.EFCore;
using CommentManagement.Infrastructure.EFCore;
using Microsoft.EntityFrameworkCore;

namespace _01_HavinDecorQuery.Query
{
    public class ArticleQuery : IArticleQuery
    {
        private readonly BlogContext _blogContext;
        private readonly CommentContext _commentContext;

        public ArticleQuery(BlogContext blogContext, CommentContext commentContext)
        {
            _blogContext = blogContext;
            _commentContext = commentContext;
        }

        public List<ArticleQueryModel> LatestArticles()
        {
            return _blogContext.Articles.Include(x => x.Category)
                .Where(x => x.PublishingDate <= DateTime.Now)
                .Select(x => new ArticleQueryModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    ShortDescription = x.ShortDescription,
                    Picture = x.Picture,
                    PictureAlt = x.PictureAlt,
                    PictureTitle = x.PictureTitle,
                    PublishingDate = x.PublishingDate.ToFarsi(),
                    Slug = x.Slug,
                }).OrderByDescending(x => x.Id).Take(6).ToList();
        }

        public ArticleQueryModel GetArticleDetails(string slug)
        {
            var article = _blogContext.Articles.Include(x => x.Category)
                 .Where(x => x.PublishingDate <= DateTime.Now)
                 .Select(x => new ArticleQueryModel
                 {
                     Id = x.Id,
                     Title = x.Title,
                     ShortDescription = x.ShortDescription,
                     Description = x.Description,
                     Picture = x.Picture,
                     PictureAlt = x.PictureAlt,
                     PictureTitle = x.PictureTitle,
                     PublishingDate = x.PublishingDate.ToFarsi(),
                     Slug = x.Slug,
                     Keywords = x.Keywords,
                     MetaDescription = x.MetaDescription,
                     CanonicalAddress = x.CanonicalAddress,
                     CategoryId = x.CategoryId,
                     CategoryName = x.Category.Name,
                     CategorySlug = x.Category.Slug
                 }).FirstOrDefault(x => x.Slug == slug);

            if (article != null)
            {
                article.KeywordList = article.Keywords.Split(",").ToList();
            }

            var comments = _commentContext.Comments
                   .Where(x => !x.IsCanceled)
                   .Where(x => x.IsConfirmed)
                   .Where(x => x.Type == CommentType.Article)
                   .Where(x => x.OwnerRecordId == article.Id)
                   .Select(x => new CommentQueryModel
                   {
                       Id = x.Id,
                       Name = x.Name,
                       Message = x.Message,
                       CreationDate = x.CreationDate.ToFarsi(),
                       ParentId = x.ParentId
                   }).OrderBy(x => x.Id).ToList();

            foreach (var comment in comments)
            {
                if (comment.ParentId > 0)
                {
                    comment.ParentName = comments.FirstOrDefault(x => x.Id == comment.ParentId)?.Name;
                }
            }

            article.Comments = comments;
            return article;
        }
    }
}
