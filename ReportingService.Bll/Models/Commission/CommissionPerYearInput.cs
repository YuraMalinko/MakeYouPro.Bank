namespace ReportingService.DAL.ModelsDAL.Commissions
{
    public class CommissionPerYearInput
    {
        public string Type { get; set; }

        public int OperationYear { get; set; }

        public decimal AmountCommission { get; set; }
    }
}
