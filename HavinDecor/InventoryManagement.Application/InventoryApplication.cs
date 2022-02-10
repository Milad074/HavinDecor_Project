using System;
using System.Collections.Generic;
using _0_Framework.Application;
using InventoryManagement.Application.Contracts.Inventory;
using InventoryManagement.Domain.InventoryAgg;

namespace InventoryManagement.Application
{
    public class InventoryApplication :IInventoryApplication
    {
        private readonly IInventoryRepository _inventoryRepository;

        public InventoryApplication(IInventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }

        public OperationResult Create(CreateInventory command)
        {
            var operation = new OperationResult();

            if (_inventoryRepository.Exists(x => x.ProductId == command.ProductId))
            {
                return operation.Failed(ApplicationMessage.DuplicatedRecord);
            }

            var inventory = new Inventory(command.ProductId, command.UnitPrice);

            _inventoryRepository.Create(inventory);
            _inventoryRepository.SaveChanges();

            return operation.Succedded();
        }

        public OperationResult Edit(EditInventory command)
        {
            var operation = new OperationResult();

            var inventory = _inventoryRepository.Get(command.Id);

            if (inventory == null)
            {
                return operation.Failed(ApplicationMessage.RecordNotFound);
            }

            if (_inventoryRepository.Exists(x => x.ProductId == command.ProductId && x.Id != command.Id))
            {
                return operation.Failed(ApplicationMessage.DuplicatedRecord);
            }

            inventory.Edit(command.ProductId, command.UnitPrice);

            _inventoryRepository.SaveChanges();

            return operation.Succedded();
        }

        public OperationResult Increase(IncreaseInventory command)
        {
            var operation = new OperationResult();

            var increaseInventory = _inventoryRepository.Get(command.InventoryId);

            if (increaseInventory == null)
            {
                return operation.Failed(ApplicationMessage.RecordNotFound);
            }

            const long operatorId = 1;

            increaseInventory.Increase(command.Count , operatorId , command.Description);

            _inventoryRepository.SaveChanges();

            return operation.Succedded();
        }

        public OperationResult Reduce(List<ReduceInventory> command)
        {
            var operation = new OperationResult();
            const long operatorId = 1;

            foreach (var item in command)
            {
                var reduceInventory = _inventoryRepository.GetByProduct(item.ProductId);
                reduceInventory.Reduce(item.Count , operatorId , item.Description , item.OrderId);
            }

            _inventoryRepository.SaveChanges();

            return operation.Succedded();
        }

        public OperationResult Reduce(ReduceInventory command)
        {
            var operation = new OperationResult();

            var reduceInventory = _inventoryRepository.Get(command.InventoryId);

            if (reduceInventory == null)
            {
                return operation.Failed(ApplicationMessage.RecordNotFound);
            }

            const long operatorId = 1;

            reduceInventory.Reduce(command.Count, operatorId, command.Description , 0);

            _inventoryRepository.SaveChanges();

            return operation.Succedded();
        }

        public EditInventory GetDetails(long id)
        {
            return _inventoryRepository.GetDetails(id);
        }

        public List<InventoryViewModel> Search(InventorySearchModel searchModel)
        {
            return _inventoryRepository.Search(searchModel);
        }

        public List<InventoryOperationViewModel> GetOperationLog(long inventoryId)
        {
            return _inventoryRepository.GetOperationLog(inventoryId);
        }
    }
}
