using Microsoft.Extensions.Configuration;
using SmartVault.Program.BusinessObjects;
using SmartVault.Program.QueryLayer;
using System;
using System.IO;
using System.Linq;

namespace SmartVault.Program
{
    partial class Program
    {
        static void Main(string[] args)
        {
            var accountId = "1";
            if (args.Length > 0)
            {
                accountId = args[0];
            }

            WriteEveryThirdFileToFile(accountId);
            GetAllFileSizes();
        }

        private static void GetAllFileSizes()
        {
            var dal = new FileDAL();
            var total = dal.GetTotalFileSize();
            Console.WriteLine(total);
        }

        private static void WriteEveryThirdFileToFile(string accountId)
        {
            var dal = new FileDAL();
            var resultingDocuments = dal.GetThirdDocumentsWithProperty(accountId);
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();

            using var file = File.CreateText(configuration["CopyDataDestinationFileName"] ?? "CopyDataDestinationFileName");
            var content = string.Join(Environment.NewLine, resultingDocuments);
            file.Write(content);
        }
    }
}