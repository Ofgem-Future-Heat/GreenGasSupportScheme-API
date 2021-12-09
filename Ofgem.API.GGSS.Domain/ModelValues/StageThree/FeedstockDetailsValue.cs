namespace Ofgem.API.GGSS.Domain.ModelValues.StageThree 
{
    public class FeedstockDetailsValue
    {
        public string Status { get; set; } = "CannotStartYet";
        public string FeedstockPlan { get; set; }
        public string FeedstockSupplierName { get; set; }
        public DocumentValue FeedStockSupplyDocument { get; set; } = new DocumentValue();
    }
}