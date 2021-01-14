using System.Threading.Tasks;

namespace DatabaseBackuper.Core
{
    public interface IMsSqlBackupService : IBackupService
    {
        public Task BackupAllUserDatabasesAsync();
    }
}
