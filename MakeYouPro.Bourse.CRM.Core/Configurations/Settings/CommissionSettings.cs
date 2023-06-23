using MakeYouPro.Bourse.CRM.Core.Configurations.ISettings;

namespace MakeYouPro.Bourse.CRM.Core.Configurations.Settings
{
    public class CommissionSettings : ICommissionSettings
    {
        public decimal WithdrawCommissionPercentage { get; set; } =
            decimal.Parse(Environment.GetEnvironmentVariable("WithdrawCommissionPercentage") ?? throw new InvalidDataException());

        public decimal DepositCommissionPercentage { get; set; } =
            decimal.Parse(Environment.GetEnvironmentVariable("DepositCommissionPercentage") ?? throw new InvalidDataException());
    }
}
