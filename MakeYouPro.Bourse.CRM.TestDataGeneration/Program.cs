// See https://aka.ms/new-console-template for more information
using MakeYouPro.Bourse.CRM.Core.Enums;
using MakeYouPro.Bourse.CRM.Dal;
using MakeYouPro.Bourse.CRM.TestDataGeneration;

Console.WriteLine("Hello, World!");

var dbConteext = new CRMContext(Environment.GetEnvironmentVariable("EncryptKey"));
var dataGenerator = new FakerGenerator();

for (int i = 0; i < 4000000; i += 12000)
{
    var leads = dataGenerator.GenerateLeads(10000, LeadRoleEnum.StandardLead);
    leads.AddRange(dataGenerator.GenerateLeads(2000, LeadRoleEnum.VipLead));
    dbConteext.AddRange(leads);
    dbConteext.SaveChanges();

    var accounts = dataGenerator.GenerateAccountsForLeads(leads);
    dbConteext.AddRange(accounts);
    dbConteext.SaveChanges();
}