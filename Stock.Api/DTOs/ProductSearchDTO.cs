namespace Stock.Api.DTOs
{
    public class ProductSearchDTO
    {
        public string Name { get; set; }
        public string CostPrice { get; set; }
        public string SalePrice { get; set; }
        public ActionDto Condition { get; set; }
    }
}