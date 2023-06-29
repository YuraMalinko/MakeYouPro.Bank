using Aspose.Cells.Utility;
using MakeYouPro.Bourse.CRM.Auth.Dal.Models;
using System.Text.Json;
using Workbook = Aspose.Cells.Workbook;
using Worksheet = Aspose.Cells.Worksheet;

namespace MakeYouPro.Bourse.CRM.TestDataGeneration.Services
{
    public class WhriterToFile
    {
        private readonly string _filePath;
        private readonly int maxCountObject;

        public WhriterToFile()
        {
            _filePath = @$"{Environment.GetEnvironmentVariable("LogFiles")}\Users\";
            maxCountObject = 12000;
        }

        public void WhriteUsers(List<UserEntity> users)
        {
            for (int i = 0; i < users.Count(); i += maxCountObject)
            {
                string filename = $"{_filePath}Users with {users[i].Id} index.xlsx";
                var whriterr = new StreamWriter(filename);
                whriterr.Close();
                
                using (var writer = new Workbook(filename))
                {
                    Worksheet sheet = writer.Worksheets[0];


                    if (i + maxCountObject >= users.Count())
                    {
                        var writeUsers = users.GetRange(i, users.Count() - i).ToArray();
                        string serialiseForFile = JsonSerializer.Serialize(writeUsers);
                        JsonLayoutOptions options = new JsonLayoutOptions();
                        options.ArrayAsTable = true;
                        JsonUtility.ImportData(serialiseForFile, sheet.Cells, 0, 0, options);
                        writer.Save(filename);
                    }
                    else
                    {
                        
                        var writeUsers = users.GetRange(i,maxCountObject).ToArray();
                        string serialiseForFile = JsonSerializer.Serialize(writeUsers);
                        JsonLayoutOptions options = new JsonLayoutOptions();
                        options.ArrayAsTable = true;
                        JsonUtility.ImportData(serialiseForFile, sheet.Cells, 0, 0, options);
                        writer.Save(filename);
                    }
                }
            }
        }
    }
}
