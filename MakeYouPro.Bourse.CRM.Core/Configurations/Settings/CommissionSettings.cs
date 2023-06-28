using MakeYouPro.Bourse.CRM.Core.Configurations.ISettings;

namespace MakeYouPro.Bourse.CRM.Core.Configurations.Settings
{
    public class CommissionSettings : ICommissionSettings
    {
        public decimal WithdrawCommissionPercentage { get; } =
            decimal.Parse(Environment.GetEnvironmentVariable("WithdrawCommissionPercentage") ?? throw new InvalidDataException());

        public decimal DepositCommissionPercentage { get; } =
            decimal.Parse(Environment.GetEnvironmentVariable("DepositCommissionPercentage") ?? throw new InvalidDataException());

        public decimal TransferTransactionCommissionPercentage { get; } =
            decimal.Parse(Environment.GetEnvironmentVariable("TransferTransactionCommissionPercentage") ?? throw new InvalidDataException());

        public decimal ExtraTransferTransactionCommissionPercentage { get; } =
            decimal.Parse(Environment.GetEnvironmentVariable("ExtraTransferTransactionCommissionPercentage") ?? throw new InvalidDataException());
    }
}
