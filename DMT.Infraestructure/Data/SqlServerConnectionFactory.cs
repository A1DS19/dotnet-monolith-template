using System.Data;
using Microsoft.Data.SqlClient;

namespace DMT.Infrastructure.Data;

public class SqlServerConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public SqlServerConnectionFactory(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
    }

    public IDbConnection CreateConnection()
    {
        return new SqlConnection(_connectionString);
    }

    public async Task<IDbConnection> CreateConnectionAsync()
    {
        var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        return connection;
    }
}
