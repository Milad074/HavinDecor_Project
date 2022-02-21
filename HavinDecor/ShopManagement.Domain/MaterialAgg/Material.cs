using _0_Framework.Domain;

namespace ShopManagement.Domain.MaterialAgg
{
    public class Material : EntityBase
    {
        public string MaterialName { get; private set; }

        public double Price { get; private set; }

        public string Panel { get; private set; }

        public string RingColor { get; private set; }

        public Material(string materialName, double price, string panel , string ringColor)
        {
            MaterialName = materialName;
            Price = price;
            Panel = panel;
            RingColor = ringColor;
        }

        public void Edit(string materialName, double price, string panel, string ringColor)
        {
            MaterialName = materialName;
            Price = price;
            Panel = panel;
            RingColor = ringColor;

        }
    }
}
