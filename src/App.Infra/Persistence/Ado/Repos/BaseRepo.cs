using Microsoft.Extensions.Configuration;
using Npgsql;
using NpgsqlTypes;

namespace App.Infra.Persistence.Ado.Repos;
public abstract class BaseRepo(IConfiguration config)
{

    private readonly string _connectionString = 
        config.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Missing connection string 'DefaultConnection'");

    private static void _SetParameters(NpgsqlCommand cmd, (string Name, object? Value, NpgsqlDbType Type)[] parameters)
    {
      foreach(var (Name, Value, Type) in parameters)
      {
          var param = cmd.Parameters.Add(Name, Type);
          param.Value = Value ?? DBNull.Value;
      }
    }
    
    protected async Task<T?> ExecuteSingleReaderAsync<T>(string sql, (string Name, object? Value, NpgsqlDbType Type)[] parameters, Func<NpgsqlDataReader, T?> mapperFunc, CancellationToken ct)
    {
      await using var conn = new NpgsqlConnection(_connectionString);
      await using var cmd = new NpgsqlCommand(sql, conn);
      cmd.CommandTimeout = 30;
      _SetParameters(cmd, parameters);
      await conn.OpenAsync(ct);
      await using var reader = await cmd.ExecuteReaderAsync(ct);
      if(!await reader.ReadAsync(ct)) return default;
      return mapperFunc(reader);
    }

    protected async Task<IReadOnlyList<T>> ExecuteReaderAsync<T>(string sql, (string Name, object? Value, NpgsqlDbType Type)[] parameters, Func<NpgsqlDataReader, T> mapperFunc, CancellationToken ct)
    {
      await using var conn = new NpgsqlConnection(_connectionString);
      await using var cmd = new NpgsqlCommand(sql, conn);
      cmd.CommandTimeout = 30;
      _SetParameters(cmd, parameters);
      await conn.OpenAsync(ct);
      await using var reader = await cmd.ExecuteReaderAsync(ct);
      var list = new List<T>();
      while(await reader.ReadAsync(ct))
      {
        list.Add(mapperFunc(reader));
      }
      return list;
    }

    protected async Task<int> ExecuteNonQueryAsync(string sql, (string Name, object? Value, NpgsqlDbType Type)[] parameters, CancellationToken ct)
    {
      await using var conn = new NpgsqlConnection(_connectionString);
      await using var cmd = new NpgsqlCommand(sql, conn);
      cmd.CommandTimeout = 30;
      _SetParameters(cmd, parameters);
      await conn.OpenAsync(ct);
      int affectedRows = await cmd.ExecuteNonQueryAsync(ct);
      return affectedRows;
    }

    protected async Task<T?> ExecuteScalarAsync<T>(string sql, (string Name, object? Value, NpgsqlDbType Type)[] parameters, CancellationToken ct)
    {
      await using var conn = new NpgsqlConnection(_connectionString);
      await using var cmd = new NpgsqlCommand(sql, conn);
      cmd.CommandTimeout = 30;
      _SetParameters(cmd, parameters);
      await conn.OpenAsync(ct);
      var result = await cmd.ExecuteScalarAsync(ct);
      return (result is null || result is DBNull) ? default : (T)Convert.ChangeType(result, typeof(T));
    }
}
