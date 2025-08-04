using System.Data;
using DMT.Infrastructure.Data;

namespace DMT.Infrastructure.Repositories;

public abstract class BaseRepository
{
    protected readonly IDbConnectionFactory _connectionFactory;

    protected BaseRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
    }

    protected async Task<TResult> ExecuteAsync<TResult>(Func<IDbConnection, Task<TResult>> operation)
    {
        using var connection = _connectionFactory.CreateConnection();
        return await operation(connection);
    }

    protected async Task ExecuteAsync(Func<IDbConnection, Task> operation)
    {
        using var connection = _connectionFactory.CreateConnection();
        await operation(connection);
    }
}
