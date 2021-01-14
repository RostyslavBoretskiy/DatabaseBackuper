using System.Threading.Tasks;

namespace DatabaseBackuper.Core
{
    public interface IBackupService
    {
        public Task BackupDatabaseAsync(string databaseName);
    }
}
