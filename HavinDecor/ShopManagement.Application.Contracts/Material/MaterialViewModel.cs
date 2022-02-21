namespace ShopManagement.Application.Contracts.Material
{
    public class MaterialViewModel
    {
        public long Id { get; set; }
        public string MaterialName { get; set; }

        public double Price { get; set; }

        public string Panel { get; set; }

        public string RingColor { get; set; }
        public string CreationDate { get; set; }
    }
}