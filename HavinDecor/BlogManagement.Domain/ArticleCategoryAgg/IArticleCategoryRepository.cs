using System.Collections.Generic;
using _0_Framework.Domain;
using BlogManagement.Application.Contracts.ArticleCategory;

namespace BlogManagement.Domain.ArticleCategoryAgg
{
    public interface IArticleCategoryRepository :IRepository<long , ArticleCategory>
    {
        EditArticleCategory GetDetails(long id);

        string GetSlugById(long id);

        List<ArticleCategoryViewModel> GerArticleCategories();

        List<ArticleCategoryViewModel> Search(ArticleCategorySearchModel searchModel);
    }
}
