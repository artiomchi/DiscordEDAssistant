using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace FlexLabs.DiscordEDAssistant.Repositories.EFCore.Base
{
    public abstract class RepositoryBase : IDisposable
    {
        protected EDAssistantDataContext DataContext { get; private set; }
        public RepositoryBase(EDAssistantDataContext context)
        {
            DataContext = context;
        }

        public void Dispose()
        {
            DataContext?.Dispose();
            DataContext = null;
            _sqlConnection?.Dispose();
            _sqlConnection = null;
        }

        private SqlConnection _sqlConnection = null;
        private SqlConnection SqlConnection => _sqlConnection ?? (_sqlConnection = new SqlConnection(EDAssistantDataContext.ConnectionString));

        public DateTime GetDate() => DateTime.UtcNow;

        public void SetLongTimeout() => DataContext.Database.SetCommandTimeout(EDAssistantDataContext.LongTimeoutMs);

        protected long ConvertID(ulong id)
        {
            if (id <= long.MaxValue)
                return Convert.ToInt64(id);

            return -Convert.ToInt64(id - long.MaxValue);
        }

        protected ulong ConvertID(long id)
        {
            if (id >= 0)
                return Convert.ToUInt64(id);

            return Convert.ToUInt64(-id) + long.MaxValue;
        }

        protected async Task BulkUploadEntitiesAsync<TEntity>(string tableName, IEnumerable<TEntity> entities)
        {
            using (var conn = new SqlConnection(EDAssistantDataContext.ConnectionString))
            using (var copy = new SqlBulkCopy(conn, SqlBulkCopyOptions.TableLock | SqlBulkCopyOptions.UseInternalTransaction, null)
            {
                BulkCopyTimeout = EDAssistantDataContext.LongTimeoutMs / 1000,
                DestinationTableName = tableName,
                EnableStreaming = true,
                BatchSize = 10000,
            })
            {
                var source = entities.AsDataReader();
                for (int i = 0; i < source.FieldCount; i++)
                {
                    var columnName = source.GetName(i);
                    copy.ColumnMappings.Add(new SqlBulkCopyColumnMapping(columnName, columnName));
                }

                if (conn.State != ConnectionState.Open)
                    await conn.OpenAsync();
                await copy.WriteToServerAsync(source);
            }
        }

    }
}
