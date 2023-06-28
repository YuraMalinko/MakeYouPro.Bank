namespace ReportingService.DAL.ModelsDAL.Commissions
{
    public class CommissionPerDayInput
    {
        public string Type { get; set; }

        public int OperationDay { get; set; }

        public int OperationMonth { get; set; }

        public int OperationYear { get; set; }

        public decimal AmountCommission { get; set; }
    }
}
