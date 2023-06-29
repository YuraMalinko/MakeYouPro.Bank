using MakeYouPro.Bourse.CRM.Auth.Dal.Models;
using System.Data;

namespace MakeYouPro.Bourse.CRM.TestDataGeneration.TablesGenerator
{
    internal static class TableAuthDBGenerator
    {
        internal static string GetConnectionString()
        {
            return Environment.GetEnvironmentVariable("AuthCrmBourseDB");
        }

        internal static DataTable MakeUsersTable(IEnumerable<UserEntity> users)
        {
            DataTable usersTable = new DataTable("Users");

            DataColumn userId = new DataColumn();
            userId.DataType = Type.GetType("System.Int32");
            userId.ColumnName = "Id";
            userId.AutoIncrement = true;
            userId.AllowDBNull = false;
            usersTable.Columns.Add(userId);

            DataColumn email = new DataColumn();
            email.DataType = Type.GetType("System.String");
            email.ColumnName = "Email";
            email.AllowDBNull = false;
            usersTable.Columns.Add(email);

            DataColumn password = new DataColumn();
            password.DataType = Type.GetType("System.String");
            password.ColumnName = "Password";
            password.AllowDBNull = false;
            usersTable.Columns.Add(password);

            DataColumn role = new DataColumn();
            role.DataType = Type.GetType("System.Int32");
            role.ColumnName = "Role";
            role.AllowDBNull = false;
            usersTable.Columns.Add(role);

            DataColumn status = new DataColumn();
            status.DataType = Type.GetType("System.Int32");
            status.ColumnName = "Status";
            status.AllowDBNull = false;
            usersTable.Columns.Add(status);

            DataRow row = usersTable.NewRow();
            foreach (var u in users)
            {
                row = usersTable.NewRow();
                row["Id"] = u.Id;
                row["Email"] = u.Email;
                row["Password"] = u.Password;
                row["Role"] = u.Role;
                row["Status"] = u.Status;
                usersTable.Rows.Add(row);
            }
            usersTable.AcceptChanges();

            return usersTable;
        }
    }
}
