using System.Collections.Generic;
using System.Linq;
using _0_Framework.Domain;

namespace InventoryManagement.Domain.InventoryAgg
{
    public class Inventory :EntityBase
    {
        public long ProductId { get; private set; }

        public double UnitPrice { get; private set; }

        public bool InStoke { get; private set; }


        public List<InventoryOperation> Operations { get; private set; }

        public Inventory(long productId, double unitPrice)
        {
            ProductId = productId;
            UnitPrice = unitPrice;
            InStoke = false;
        }

        public void Edit(long productId, double unitPrice)
        {
            ProductId = productId;
            UnitPrice = unitPrice;
        }

        public long CalculateCurrentCount()
        {
            var plus = Operations.Where(x => x.Operation).Sum(x => x.Count);

            var minus = Operations.Where(x => !x.Operation).Sum(x => x.Count);

            return plus - minus;
        }

        public void Increase(long count , long operatorId , string description)
        {
            var currentCount = CalculateCurrentCount() + count;

            var operation = new InventoryOperation(true, count,
                operatorId, description, 0, currentCount, Id);

            Operations.Add(operation);

            InStoke = currentCount > 0; 
        }

        public void Reduce(long count , long operatorId , string description , long orderId)
        {
            var currentCount = CalculateCurrentCount() - count;

            var operation = new InventoryOperation(false, count,
                operatorId, description, orderId, currentCount, Id);

            Operations.Add(operation);

            InStoke = currentCount > 0; 
        }

    }
}
