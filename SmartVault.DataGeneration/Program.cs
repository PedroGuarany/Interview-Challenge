using Dapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SmartVault.Library;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Xml.Serialization;

namespace SmartVault.DataGeneration
{
    partial class Program
    {

        static void Main(string[] args)
        {
            var insertDocument = $"INSERT INTO Document (Id, Name, FilePath, Length, AccountId, CreatedOn) VALUES(@1,@2,@3,@4,@5, @6);";
            var insertUser = $"INSERT INTO User (Id, FirstName, LastName, DateOfBirth, AccountId, Username, Password, CreatedOn) VALUES(@1,@2,@3,@4,@5,@6,@7,@8)";
            var insertAccount = $"INSERT INTO Account (Id, Name, CreatedOn) VALUES(@1,@2,@3)";
            var start = DateTime.UtcNow;
            var documentPaths = new List<string> { new FileInfo("TestDoc.txt").FullName, new FileInfo("TestDoc2.txt").FullName, new FileInfo("TestDoc3.txt").FullName };
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();

            SQLiteConnection.CreateFile(configuration["DatabaseFileName"]);
            File.WriteAllText("TestDoc.txt", $"This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}This is my test document{Environment.NewLine}");
            File.WriteAllText("TestDoc2.txt", $"This is my test that has a Smith Property");
            File.WriteAllText("TestDoc3.txt", $"This is my test that has a Another Property");


            using (var connection = new SQLiteConnection(string.Format(configuration?["ConnectionStrings:DefaultConnection"] ?? "", configuration?["DatabaseFileName"])))
            {
                var files = Directory.GetFiles(@"..\..\..\..\BusinessObjectSchema");

                connection.Open();
                var transaction = connection.BeginTransaction();
                for (int i = 0; i < files.Length; i++)
                {
                    var serializer = new XmlSerializer(typeof(BusinessObject));
                    var businessObject = serializer.Deserialize(new StreamReader(files[i])) as BusinessObject;
                    
                    var script = businessObject?.Script.Replace(");", ",CreatedOn DATETIME);");
                    connection.Execute(script);

                }
                var documentNumber = 0;
                for (int i = 0; i < 100; i++)
                {

                    var randomDayIterator = RandomDay().GetEnumerator();
                    randomDayIterator.MoveNext();

                    var now = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
                    var userParameters = new DynamicParameters();
                    userParameters.Add("@1", $"{i}");
                    userParameters.Add("@2", $"FName{i}");
                    userParameters.Add("@3", $"LName{i}");
                    userParameters.Add("@4", $"{randomDayIterator.Current:yyyy-MM-dd}");
                    userParameters.Add("@5", $"{i}");
                    userParameters.Add("@6", $"UserName-{i}");
                    userParameters.Add("@7", $"e10adc3949ba59abbe56e057f20f883e");
                    userParameters.Add("@8", $"{now}");
                    connection.Execute(insertUser, userParameters);

                    var accountParameters = new DynamicParameters();
                    accountParameters.Add("@1", $"{i}");
                    accountParameters.Add("@2", $"Account{i}");
                    accountParameters.Add("@3", $"{now}");
                    connection.Execute(insertAccount, accountParameters);

                    for (int d = 0; d < 10000; d++, documentNumber++)
                    {
                        var documentPath = documentPaths[d % 3];
                        var parameters = new DynamicParameters();
                        parameters.Add("@1", $"{documentNumber}");
                        parameters.Add("@2", $"Document{i}-{d}.txt");
                        parameters.Add("@3", $"{documentPath}");
                        parameters.Add("@4", $"{new FileInfo(documentPath).Length}");
                        parameters.Add("@5", $"{i}");
                        parameters.Add("@6", $"{now}");
                        connection.Execute(insertDocument, parameters);
                    }

                }
                transaction.Commit();
                var accountData = connection.Query("SELECT COUNT(*) FROM Account;");
                Console.WriteLine($"AccountCount: {JsonConvert.SerializeObject(accountData)}");
                var documentData = connection.Query("SELECT COUNT(*) FROM Document;");
                Console.WriteLine($"DocumentCount: {JsonConvert.SerializeObject(documentData)}");
                var userData = connection.Query("SELECT COUNT(*) FROM User;");
                Console.WriteLine($"UserCount: {JsonConvert.SerializeObject(userData)}");

                Console.WriteLine("Total: " + (DateTime.UtcNow - start));
            }
        }

        static IEnumerable<DateTime> RandomDay()
        {
            DateTime start = new DateTime(1985, 1, 1);
            Random gen = new Random();
            int range = (DateTime.Today - start).Days;
            while (true)
                yield return start.AddDays(gen.Next(range));
        }
    }
}