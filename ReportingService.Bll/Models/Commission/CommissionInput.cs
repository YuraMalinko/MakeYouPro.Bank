﻿namespace ReportingService.Bll.Models.Commission
{
    public class CommissionInput
    {
        public int Id { get; set; }

        public decimal Amount { get; set; }

        public int OperationDay { get; set; }

        public int OperationMonth { get; set; }

        public int OperationYear { get; set; }

        public TimeOnly OperationTime { get; set; }

        public int TransactionId { get; set; }
    }
}
