﻿namespace MakeYouPro.Bourse.CRM.Core.Clients.TransactionService.Models
{
    public class DepositRequest
    {
        public int AccountId { get; set; }

        public decimal Amount { get; set; }
    }
}
