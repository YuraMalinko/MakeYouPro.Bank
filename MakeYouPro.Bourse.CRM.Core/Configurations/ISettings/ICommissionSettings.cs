namespace MakeYouPro.Bourse.CRM.Core.Configurations.ISettings
{
    public interface ICommissionSettings
    {
        decimal WithdrawCommissionPercentage { get; }

        decimal DepositCommissionPercentage { get; }

        decimal TransferTransactionCommissionPercentage { get; }

        decimal ExtraTransferTransactionCommissionPercentage { get; }
    }
}
