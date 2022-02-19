using System.Collections.Generic;
using _0_Framework.Application;
using ShopManagement.Application.Contracts.ProductPicture;
using ShopManagement.Domain.ProductAgg;
using ShopManagement.Domain.ProductPictureAgg;

namespace ShopManagement.Application
{
    public class ProductPictureApplication :IProductPictureApplication
    {
        private readonly IFileUploader _fileUploader;
        private readonly IProductRepository _productRepository;
        private readonly IProductPictureRepository _productPictureRepository;

        public ProductPictureApplication(IFileUploader fileUploader, IProductRepository productRepository, IProductPictureRepository productPictureRepository)
        {
            _fileUploader = fileUploader;
            _productRepository = productRepository;
            _productPictureRepository = productPictureRepository;
        }

        public OperationResult Create(CreateProductPicture command)
        {
            var operation = new OperationResult();

            //if (_productPictureRepository.Exists
            //(x=> x.Picture == command.Picture 
            //  && x.ProductId == command.ProductId))
            //{
            //    return operation.Failed(ApplicationMessage.DuplicatedRecord);
            //}

            var product = _productRepository.GetProductWithCategory(command.ProductId);
            var path = $"{product.Category.Slug}/{product.Slug}";
            var fileName = _fileUploader.Upload(command.Picture, path);

            var productPicture = new ProductPicture(command.ProductId, fileName, command.PictureAlt,
                command.PictureTitle);

            _productPictureRepository.Create(productPicture);

            _productPictureRepository.SaveChanges();

            return operation.Succedded();
        }

        public OperationResult Edit(EditProductPicture command)
        {
            var operation = new OperationResult();

            var productPicture = _productPictureRepository.GetProductPictureWithProductAndCategory(command.Id);

            if (productPicture == null)
            {
                return operation.Failed(ApplicationMessage.RecordNotFound);
            }

            //if (_productPictureRepository.Exists(x=> x.Picture == command.Picture && x.ProductId == x.ProductId && x.Id != command.Id))
            //{
            //    return operation.Failed(ApplicationMessage.DuplicatedRecord);
            //}

            var path = $"{productPicture.Product.Category.Slug}/{productPicture.Product.Slug}";
            var fileName = _fileUploader.Upload(command.Picture, path);

            productPicture.Edit(command.ProductId , fileName , command.PictureAlt , command.PictureTitle);

            _productPictureRepository.SaveChanges();

            return operation.Succedded();
        }

        public OperationResult Remove(long id)
        {
            var operation = new OperationResult();

            var productPicture = _productPictureRepository.Get(id);

            if (productPicture == null)
            {
                return operation.Failed(ApplicationMessage.RecordNotFound);
            }

            productPicture.Remove();

            _productPictureRepository.SaveChanges();

            return operation.Succedded();
        }

        public OperationResult Restore(long id)
        {
            var operation = new OperationResult();

            var productPicture = _productPictureRepository.Get(id);

            if (productPicture == null)
            {
                return operation.Failed(ApplicationMessage.RecordNotFound);
            }

            productPicture.Restore();

            _productPictureRepository.SaveChanges();

            return operation.Succedded();
        }

        public EditProductPicture GetDetails(long id)
        {
            return _productPictureRepository.GetDetails(id);
        }

        public List<ProductPictureViewModel> Search(ProductPictureSearchModel searchModel)
        {
            return _productPictureRepository.Search(searchModel);
        }
    }
}
