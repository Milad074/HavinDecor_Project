using System.Collections.Generic;
using System.Linq;
using _0_Framework.Application;
using _0_Framework.Infrastructure;
using InventoryManagement.Application.Contracts.Inventory;
using InventoryManagement.Domain.InventoryAgg;
using ShopManagement.Infrastructure.EFCore;

namespace InventoryManagement.Infrastructure.EFCore.Repository
{
    public class InventoryRepository  : RepositoryBase<long , Inventory> , IInventoryRepository
    {
        private readonly InventoryContext _inventoryContext;

        private readonly ShopContext _shopContext;

        public InventoryRepository(InventoryContext inventoryContext , ShopContext shopContext) :base(inventoryContext)
        {
            _inventoryContext = inventoryContext;
            _shopContext = shopContext;
        }

        public EditInventory GetDetails(long id)
        {
            return _inventoryContext.Inventory
                .Select(x => new EditInventory
            {
                Id = x.Id,
                ProductId = x.ProductId,
                UnitPrice = x.UnitPrice
            }).FirstOrDefault(x => x.Id == id);
        }

        public Inventory GetByProduct(long productId)
        {
            return _inventoryContext.Inventory.FirstOrDefault(x => x.ProductId == productId);
        }

        public List<InventoryViewModel> Search(InventorySearchModel searchModel)
        {
            var products = _shopContext.Products.Select(x => new
            {
                x.Id,
                x.Name
            }).ToList();

            var query = _inventoryContext.Inventory
                .Select(x => new InventoryViewModel
            {
                    Id = x.Id,
                    ProductId = x.ProductId,
                    UnitPrice = x.UnitPrice,
                    CurrentCount = x.CalculateCurrentCount(),
                    InStoke = x.InStoke,
                    CreationDate = x.CreationDate.ToFarsi()
            });

            if (searchModel.ProductId > 0)
            {
                query = query.Where(x => x.ProductId == searchModel.ProductId);
            }

            if (searchModel.InStoke)
            {
                query = query.Where(x => !x.InStoke);
            }

            var inventory = query.OrderByDescending(x => x.Id).ToList();

            inventory.ForEach(item=>
                item.Product = products.FirstOrDefault(x=> x.Id == item.ProductId)?.Name);

            return inventory;
        }

        public List<InventoryOperationViewModel> GetOperationLog(long inventoryId)
        {
            var inventory = _inventoryContext.Inventory
                .FirstOrDefault(x => x.Id == inventoryId);

            return inventory.Operations.Select(x => new InventoryOperationViewModel
            {
                Id = x.Id,
                Count = x.Count,
                Description = x.Description,
                Operation = x.Operation,
                OrderId = x.OrderId,
                OperationDate = x.OperationDate.ToFarsi(),
                Operator = "مدیر سیستم",
                OperatorId = x.OperatorId,
                CurrentCount = x.CurrentCount
            }).OrderByDescending(x=> x.Id).ToList();
        }
    }
}
