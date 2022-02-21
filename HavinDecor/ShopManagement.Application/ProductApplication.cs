using System.Collections.Generic;
using _0_Framework.Application;
using ShopManagement.Application.Contracts.Product;
using ShopManagement.Domain.ProductAgg;
using ShopManagement.Domain.ProductCategoryAgg;

namespace ShopManagement.Application
{
    public class ProductApplication : IProductApplication
    {
        private readonly IFileUploader _fileUploader;
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly IProductRepository _productRepository;

        public ProductApplication(IFileUploader fileUploader,
            IProductCategoryRepository productCategoryRepository,
            IProductRepository productRepository)
        {
            _fileUploader = fileUploader;
            _productCategoryRepository = productCategoryRepository;
            _productRepository = productRepository;
        }

        public OperationResult Create(CreateProduct command)
        {
            var operation = new OperationResult();

            if (_productRepository.Exists(p=> p.Name == command.Name))
            {
                return operation.Failed(ApplicationMessage.DuplicatedRecord);
            }

            var categorySlug = _productCategoryRepository.GetSlugById(command.CategoryId);

            var path = $"{categorySlug}/{command.Slug}";

            var fileName = _fileUploader.Upload(command.Picture, path);

            var product = new Product(command.Name, command.Code, command.ShortDescription,
                 command.Description, fileName, command.PictureAlt,
                command.PictureTitle, command.CategoryId, command.Slug,
                 command.Keywords, command.MetaDescription);

            _productRepository.Create(product);

            _productRepository.SaveChanges();

            return operation.Succedded();
        }

        public OperationResult Edit(EditProduct command)
        {
            var operation = new OperationResult();

            var product = _productRepository.GetProductWithCategory(command.Id);

            if (product == null)
            {
                return operation.Failed(ApplicationMessage.RecordNotFound);
            }

            if (_productRepository.Exists(p=> p.Name == command.Name && p.Id != command.Id))
            {
                return operation.Failed(ApplicationMessage.DuplicatedRecord);
            }
            

            var path = $"{product.Category.Slug}/{command.Slug}";
            var fileName = _fileUploader.Upload(command.Picture, path);

            product.Edit(command.Name, command.Code, command.ShortDescription, command.Description,
                 fileName, command.PictureAlt, command.PictureTitle, command.CategoryId,
                 command.Slug, command.Keywords, command.MetaDescription);

            _productRepository.SaveChanges();

            return operation.Succedded();
        }

        public EditProduct GetDetails(long id)
        {
            return _productRepository.GetDetails(id);
        }

        public List<ProductViewModel> Search(ProductSearchModel searchModel)
        {
            return _productRepository.Search(searchModel);
        }

        public List<ProductViewModel> GetProducts()
        {
            return _productRepository.GetProducts();
        }
    }
}
