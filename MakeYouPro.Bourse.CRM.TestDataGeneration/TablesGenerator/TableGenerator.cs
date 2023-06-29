using EntityFrameworkCore.EncryptColumn.Interfaces;
using EntityFrameworkCore.EncryptColumn.Util;
using MakeYouPro.Bourse.CRM.Dal.Models;
using System.Data;

namespace MakeYouPro.Bourse.CRM.TestDataGeneration.TablesGenerator
{
    internal static class TableGenerator
    {

        private static readonly IEncryptionProvider _provider =
            new GenerateEncryptionProvider(Environment.GetEnvironmentVariable("EncryptKey"));

        internal static DataTable MakeLeadTable(IEnumerable<LeadEntity> leads)
        {
            DataTable leadsTable = new DataTable("Leads");
            DataColumn leadId = new DataColumn();
            leadId.DataType = Type.GetType("System.Int32");
            leadId.ColumnName = "Id";
            leadId.AutoIncrement = false;
            leadId.AllowDBNull = false;
            leadsTable.Columns.Add(leadId);

            DataColumn role = new DataColumn();
            role.DataType = Type.GetType("System.Int32");
            role.ColumnName = "Role";
            role.AllowDBNull = false;
            leadsTable.Columns.Add(role);

            DataColumn status = new DataColumn();
            status.DataType = Type.GetType("System.Int32");
            status.ColumnName = "Status";
            status.AllowDBNull = false;
            leadsTable.Columns.Add(status);

            DataColumn dateCreate = new DataColumn();
            dateCreate.DataType = Type.GetType("System.DateTime");
            dateCreate.ColumnName = "DateCreate";
            dateCreate.AllowDBNull = false;
            leadsTable.Columns.Add(dateCreate);

            DataColumn name = new DataColumn();
            name.DataType = Type.GetType("System.String");
            name.ColumnName = "Name";
            name.AllowDBNull = false;
            leadsTable.Columns.Add(name);

            DataColumn middleName = new DataColumn();
            middleName.DataType = Type.GetType("System.String");
            middleName.ColumnName = "MiddleName";
            middleName.AllowDBNull = true;
            leadsTable.Columns.Add(middleName);

            DataColumn surname = new DataColumn();
            surname.DataType = Type.GetType("System.String");
            surname.ColumnName = "Surname";
            surname.AllowDBNull = false;
            leadsTable.Columns.Add(surname);

            DataColumn birthday = new DataColumn();
            birthday.DataType = typeof(DateTime);
            birthday.ColumnName = "Birthday";
            birthday.AllowDBNull = false;
            leadsTable.Columns.Add(birthday);

            DataColumn phoneNumber = new DataColumn();
            phoneNumber.DataType = typeof(string);
            phoneNumber.ColumnName = "PhoneNumber";
            phoneNumber.AllowDBNull = false;
            leadsTable.Columns.Add(phoneNumber);

            DataColumn email = new DataColumn();
            email.DataType = Type.GetType("System.String");
            email.ColumnName = "Email";
            email.AllowDBNull = false;
            leadsTable.Columns.Add(email);

            DataColumn citizenship = new DataColumn();
            citizenship.DataType = Type.GetType("System.String");
            citizenship.ColumnName = "Citizenship";
            citizenship.AllowDBNull = false;
            leadsTable.Columns.Add(citizenship);

            DataColumn passportNumber = new DataColumn();
            //passportNumber.DataType = Type.GetType("System.String");
            passportNumber.DataType = typeof(string);
            passportNumber.ColumnName = "PassportNumber";
            passportNumber.AllowDBNull = false;
            leadsTable.Columns.Add(passportNumber);

            DataColumn registration = new DataColumn();
            registration.DataType = Type.GetType("System.String");
            registration.ColumnName = "Registration";
            registration.AllowDBNull = false;
            leadsTable.Columns.Add(registration);

            DataColumn comment = new DataColumn();
            comment.DataType = Type.GetType("System.String");
            comment.ColumnName = "Comment";
            comment.AllowDBNull = true;
            leadsTable.Columns.Add(comment);

            DataRow row = leadsTable.NewRow();
            foreach (var lead in leads)
            {
                row = leadsTable.NewRow();
                row["Id"] = lead.Id;
                row["Role"] = (int)lead.Role;
                row["Status"] = (int)lead.Status;
                row["Id"] = lead.Id;
                row["DateCreate"] = lead.DateCreate;
                row["Name"] = lead.Name;
                row["MiddleName"] = lead.MiddleName;
                row["Surname"] = lead.Surname;
                row["Birthday"] = lead.Birthday.ToDateTime(TimeOnly.MinValue);
                row["PhoneNumber"] = lead.PhoneNumber;
                row["Email"] = lead.Email;
                row["Citizenship"] = lead.Citizenship;
                row["PassportNumber"] = _provider.Encrypt(lead.PassportNumber);
                row["Registration"] = _provider.Encrypt(lead.Registration);
                row["Comment"] = lead.Comment;
                leadsTable.Rows.Add(row);
            }
            leadsTable.AcceptChanges();

            return leadsTable;
        }

        internal static DataTable MakeAccountTable(IEnumerable<AccountEntity> accounts)
        {
            DataTable accountsTable = new DataTable("Accounts");
            DataColumn accountId = new DataColumn();
            accountId.DataType = Type.GetType("System.Int32");
            accountId.ColumnName = "Id";
            accountId.AutoIncrement = true;
            accountId.AllowDBNull = false;
            accountsTable.Columns.Add(accountId);

            DataColumn leadId = new DataColumn();
            leadId.DataType = Type.GetType("System.Int32");
            leadId.ColumnName = "LeadId";
            leadId.AllowDBNull = false;
            accountsTable.Columns.Add(leadId);

            DataColumn dateCreate = new DataColumn();
            dateCreate.DataType = Type.GetType("System.DateTime");
            dateCreate.ColumnName = "DateCreate";
            dateCreate.AllowDBNull = false;
            accountsTable.Columns.Add(dateCreate);

            DataColumn currency = new DataColumn();
            currency.DataType = Type.GetType("System.String");
            currency.ColumnName = "Currency";
            currency.AllowDBNull = false;
            accountsTable.Columns.Add(currency);

            DataColumn status = new DataColumn();
            status.DataType = Type.GetType("System.Int32");
            status.ColumnName = "Status";
            status.AllowDBNull = false;
            accountsTable.Columns.Add(status);

            DataColumn comment = new DataColumn();
            comment.DataType = Type.GetType("System.String");
            comment.ColumnName = "Comment";
            comment.AllowDBNull = true;
            accountsTable.Columns.Add(comment);

            DataRow row = accountsTable.NewRow();
            foreach (var account in accounts)
            {
                row = accountsTable.NewRow();
                row["Id"] = account.Id;
                row["LeadId"] = account.LeadId;
                row["DateCreate"] = account.DateCreate;
                row["Currency"] = account.Currency;
                row["Status"] = account.Status;
                row["Comment"] = account.Comment;
                accountsTable.Rows.Add(row);
            }
            accountsTable.AcceptChanges();

            return accountsTable;
        }

        internal static string GetConnectionString()
        {
            return Environment.GetEnvironmentVariable("CrmBourseDB");
            //return Environment.GetEnvironmentVariable("CrmBourseLocalDB");
        }
    }
}