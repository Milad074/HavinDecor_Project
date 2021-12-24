﻿using System.Collections.Generic;
using _0_Framework.Application;
using ShopManagement.Application.Contracts.Product;
using ShopManagement.Domain.ProductAgg;

namespace ShopManagement.Application
{
    public class ProductApplication : IProductApplication
    {
        private readonly IProductRepository _productRepository;

        public ProductApplication(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public OperationResult Create(CreateProduct command)
        {
            var operation = new OperationResult();

            if (_productRepository.Exists(p=> p.Name == command.Name))
            {
                return operation.Failed(ApplicationMessage.DuplicatedRecord);
            }

            var product = new Product(command.Name, command.Code, command.UnitPrice, command.Material, command.Pieces,
                command.Area, command.ShortDescription, command.Description, command.Picture, command.PictureAlt,
                command.PictureTitle, command.CategoryId, command.Slug, command.Keywords, command.MetaDescription);

            _productRepository.Create(product);

            _productRepository.SaveChanges();

            return operation.Succedded();
        }

        public OperationResult Edit(EditProduct command)
        {
            var operation = new OperationResult();

            var product = _productRepository.Get(command.Id);

            if (product == null)
            {
                return operation.Failed(ApplicationMessage.RecordNotFound);
            }

            if (_productRepository.Exists(p=> p.Name == command.Name && p.Id != command.Id))
            {
                return operation.Failed(ApplicationMessage.DuplicatedRecord);
            }

            product.Edit(command.Name, command.Code, command.UnitPrice, command.Material, command.Pieces,
                command.Area, command.ShortDescription, command.Description, command.Picture, command.PictureAlt,
                command.PictureTitle, command.CategoryId, command.Slug, command.Keywords, command.MetaDescription);

            _productRepository.SaveChanges();

            return operation.Succedded();
        }

        public OperationResult IsStock(long id)
        {
            var operation = new OperationResult();

            var product = _productRepository.Get(id);

            if (product == null)
            {
                return operation.Failed(ApplicationMessage.RecordNotFound);
            }

            product.InStock();

            _productRepository.SaveChanges();

            return operation.Succedded();
        }

        public OperationResult NOtInStock(long id)
        {
            var operation = new OperationResult();

            var product = _productRepository.Get(id);

            if (product == null)
            {
                return operation.Failed(ApplicationMessage.RecordNotFound);
            }

            product.NotInStock();

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