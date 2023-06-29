// See https://aka.ms/new-console-template for more information
using MakeYouPro.Bourse.CRM.Core.Enums;
using MakeYouPro.Bourse.CRM.Dal.Models;
using MakeYouPro.Bourse.CRM.TestDataGeneration;
using Microsoft.Data.SqlClient;
using System.Data;

string connectionString = TableGenerator.GetConnectionString();
using (SqlConnection connection =
           new SqlConnection(connectionString))
{
    var dataGenerator = new FakerGenerator();
    var leads = new List<LeadEntity>();
    var accounts = new List<AccountEntity>();

    connection.Open();

    for (int i = 0; i < 4000000; i += 120000)
    {
        leads = dataGenerator.GenerateLeads(100000, LeadRoleEnum.StandardLead);
        leads.AddRange(dataGenerator.GenerateLeads(20000, LeadRoleEnum.VipLead));
        accounts = dataGenerator.GenerateAccountsForLeads(leads);

        DataTable leadsTable = TableGenerator.MakeLeadTable(leads);

        using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
        {
            bulkCopy.DestinationTableName =
                "dbo.Leads";

            try
            {
                bulkCopy.WriteToServer(leadsTable);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        DataTable accountsTable = TableGenerator.MakeAccountTable(accounts);

        using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
        {
            bulkCopy.DestinationTableName =
                "dbo.Accounts";

            try
            {
                bulkCopy.WriteToServer(accountsTable);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}

