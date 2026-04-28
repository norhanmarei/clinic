using Microsoft.Extensions.Configuration;
using Npgsql;

namespace App.Infra.Persistence.Ado.Repos;
public abstract class BaseRepo(IConfiguration config)
{

    private readonly string _connectionString = 
        config.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Missing connection string 'DefaultConnection'");
    private static void _SetParameters(NpgsqlCommand cmd, (string Name, object? Value)[] parameters)
    {
      foreach(var (Name, Value) in parameters)
      {
          cmd.Parameters.AddWithValue(Name, Value ?? DBNull.Value);
      }
    }
    protected async Task<T?> ExecuteReaderAsync<T>(string sql, (string Name, object? Value)[] parameters, Func<NpgsqlDataReader, T?> mapperFunc, CancellationToken ct)
    {
      await using var conn = new NpgsqlConnection(_connectionString);
      await using var cmd = new NpgsqlCommand(sql, conn);
      _SetParameters(cmd, parameters);
      await conn.OpenAsync(ct);
      await using var reader = await cmd.ExecuteReaderAsync(ct);
      if(!await reader.ReadAsync(ct)) return default;
      return mapperFunc(reader);
    }
}
