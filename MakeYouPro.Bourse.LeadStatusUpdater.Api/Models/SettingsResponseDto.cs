namespace MakeYouPro.Bourse.LeadStatusUpdater.Api.Models
{
    public class SettingsResponseDto
    {
        public int PeriodOfTransactionsInDays { get; set; }
        public int CountOfTransactions { get; set; }
        public int PeriodOfFreshMoneyInDays { get; set; }
        public int CountOfFreshMoneyInRUB { get; set; }
        public int PeriodOfBirthdayVIPInDays { get; set; }
    }
}
