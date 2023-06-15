// See https://aka.ms/new-console-template for more information
using MakeYouPro.Bourse.CRM.Core.Enums;
using MakeYouPro.Bourse.CRM.Dal;
using MakeYouPro.Bourse.CRM.TestDataGeneration;

Console.WriteLine("Hello, World!");

var dataGenerator = new FakerGenerator();

var encryptKey = Environment.GetEnvironmentVariable("EncryptKey");
for (int i = 0; i < 1420000; i += 12000)
{
    var dbContext = new CRMContext(encryptKey);
    using (dbContext)
    {
        var leads = dataGenerator.GenerateLeads(10000, LeadRoleEnum.StandardLead);
        leads.AddRange(dataGenerator.GenerateLeads(2000, LeadRoleEnum.VipLead));
        dbContext.AddRange(leads);
        dbContext.SaveChanges();

        var accounts = dataGenerator.GenerateAccountsForLeads(leads);
        dbContext.AddRange(accounts);
        dbContext.SaveChanges();
    }
}