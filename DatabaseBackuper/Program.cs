using DatabaseBackuper.Services;
using System;
using System.Configuration;
using System.Threading.Tasks;

namespace DatabaseBackuper
{
    class Program
    {
        public const string ConnectionString = "testConnectionString";
        public const string BackupFolder = "backupFolder";

        static async Task Main(string[] args)
        {
            try
            {
                var connectionString = ConfigurationManager.ConnectionStrings[ConnectionString].ConnectionString;
                var backupFolder = ConfigurationManager.AppSettings[BackupFolder];

                var msSqlDbBackupService = new MsSqlBackupService(connectionString, backupFolder);

                await msSqlDbBackupService.BackupAllUserDatabasesAsync();
            }
            catch
            {
                Console.WriteLine("Backup failed");
            }
            Console.WriteLine("Backup successed");
        }
    }
}
