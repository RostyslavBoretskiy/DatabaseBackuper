using DatabaseBackuper.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseBackuper.Services
{
    public class MsSqlBackupService : IMsSqlBackupService
    {
        private readonly string connectionString;
        private readonly string backupFolderFullPath;
        private readonly string[] systemDatabaseNames = { "master", "tempdb", "model", "msdb" };

        public MsSqlBackupService(string connectionString, string backupFolderFullPath)
        {
            this.connectionString = connectionString;
            this.backupFolderFullPath = backupFolderFullPath;
        }

        public async Task BackupAllUserDatabasesAsync()
        {
            foreach (string databaseName in getAllUserDatabases())
                await BackupDatabaseAsync(databaseName);
        }
        public async Task BackupDatabaseAsync(string databaseName)
        {
            string filePath = buildBackupPathWithFilename(databaseName);

            using (var connection = new SqlConnection(connectionString))
            {
                var query = string.Format("BACKUP DATABASE [{0}] TO DISK='{1}'", databaseName, filePath);

                using (var command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        private IEnumerable<string> getAllUserDatabases()
        {
            var databases = new List<string>();

            DataTable databasesTable;

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                databasesTable = connection.GetSchema("Databases");

                connection.Close();
            }

            foreach (DataRow row in databasesTable.Rows)
            {
                string databaseName = row["database_name"].ToString();

                if (systemDatabaseNames.Contains(databaseName))
                    continue;

                databases.Add(databaseName);
            }

            return databases;
        }
        private string buildBackupPathWithFilename(string databaseName)
        {
            string filename = string.Format("{0}-{1}.bak", databaseName, DateTime.Now.ToString("yyyy-MM-dd"));

            return Path.Combine(backupFolderFullPath, filename);
        }
    }
}
