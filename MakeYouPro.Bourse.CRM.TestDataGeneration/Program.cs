// See https://aka.ms/new-console-template for more information
using MakeYouPro.Bourse.CRM.Auth.Dal.Models;
using MakeYouPro.Bourse.CRM.Core.Enums;
using MakeYouPro.Bourse.CRM.Dal.Models;
using MakeYouPro.Bourse.CRM.TestDataGeneration;
using MakeYouPro.Bourse.CRM.TestDataGeneration.FakerGenerators;
using MakeYouPro.Bourse.CRM.TestDataGeneration.TablesGenerator;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Diagnostics;

string connectionString = TableGenerator.GetConnectionString();
string connectAuthString = TableAuthDBGenerator.GetConnectionString();
using (SqlConnection connection =
           new SqlConnection(connectionString))
{

    var dataGenerator = new FakerGenerator();
    var leads = new List<LeadEntity>();
    var accounts = new List<AccountEntity>();

    connection.Open();

    for (int i = 0; i < 40; i += 12)
    {
        leads = dataGenerator.GenerateLeads(100000, LeadRoleEnum.StandartLead);
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

        using (SqlConnection connectAuthDB = new SqlConnection(connectAuthString))
        {
            var users = new List<UserEntity>();
            var usersGenerator = new AuthFakerGenerator();
            users = usersGenerator.GenerateUsers(leads);

            connectAuthDB.Open();

            DataTable usersTable = TableAuthDBGenerator.MakeUsersTable(users);

            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connectAuthDB))
            {
                bulkCopy.DestinationTableName = "dbo.Users";

                try
                {
                    bulkCopy.WriteToServer(usersTable);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

    }
}

