using System.Collections.Generic;

namespace _01_HavinDecorQuery.Contracts.ArticleCategory
{
    public interface IArticleCategoryQuery
    {
        List<ArticleCategoryQueryModel> GetArticleCategories();
        ArticleCategoryQueryModel GetArticleCategoryBySlug(string slug);
    }
}
