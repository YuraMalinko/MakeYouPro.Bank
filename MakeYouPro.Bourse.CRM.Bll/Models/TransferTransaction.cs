namespace MakeYouPro.Bourse.CRM.Bll.Models
{
    public class TransferTransaction
    {
        public AccountTransfer AccountSource { get; set; }

        public AccountTransfer AccountDestination { get; set; }

        public decimal Amount { get; set; }
    }
}
