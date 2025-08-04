using System.Data;

namespace DMT.Infrastructure.Data;

public interface IDbConnectionFactory
{
  IDbConnection CreateConnection();
  Task<IDbConnection> CreateConnectionAsync();
}
