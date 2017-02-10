namespace _1CIntegration
{
    public class Sklad
    {
        public int Id { get; set; }
        public string VendorCode { get; set; }
        public string Name { get; set; }
        public BasicUnit Unit { get; set; }
        public Price Price { get; set; }
        public Store Store { get; set; }

    }

    public class BasicUnit
    {
        public int Unit { get; set; }
        public int Сoefficient { get; set; }
    }

    public class Price
    {
        public decimal View { get; set; }
        public int TypePriceId { get; set; }
    }

    public class Store
    {
        public string StoreId { get; set; }
        public int CountInStore { get; set; }
    }
}