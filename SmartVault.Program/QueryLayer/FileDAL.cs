using Dapper;
using SmartVault.Program.BusinessObjects;
using System;
using System.Collections.Generic;
using System.IO;

namespace SmartVault.Program.QueryLayer
{
    public class FileDAL
    {
        private readonly DbManager manager;

        public FileDAL()
        {
            manager = new DbManager();
        }

        public long GetTotalFileSize()
        {
            using var conn = manager.GetConnection();

            long totalSize = 0;
            var filePaths = conn.Query<Document>("SELECT FilePath FROM Document GROUP BY 'FilePath'");
            foreach (var file in filePaths)
            {
                totalSize += new FileInfo(file.FilePath).Length;
            }

            return totalSize;
        }

        public List<string> GetThirdDocumentsWithProperty(string accountId, string property = "Smith Property")
        {
            var result = new List<string>();
            using var conn = manager.GetConnection();

            var documents = conn.Query<Document>($"SELECT * FROM Document WHERE AccountId = {accountId}");

            var control = 1;
            foreach (var doc in documents)
            {
                if (control % 3 != 0)
                {
                    control++;
                    continue;
                }

                var fileContent = File.ReadAllText(doc.FilePath);
                if (fileContent.Contains(property, StringComparison.InvariantCultureIgnoreCase)) 
                {
                    result.Add(fileContent);
                }
                control = 1;
            }
            return result;
        }
    }
}
