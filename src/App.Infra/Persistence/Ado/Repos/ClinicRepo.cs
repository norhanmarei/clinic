using Microsoft.Extensions.Configuration;
using App.Domain.Entities;
using App.Application.Interfaces.Repos;
namespace App.Infra.Persistence.Ado.Repos
{
  public class ClinicRepo(IConfiguration config) : BaseRepo(config), IClinicRepo
  {

    public async Task<Clinic?> GetByNameAsync(string name, CancellationToken token = default)
    {
      const string sql = "SELECT id, name, timezone, start_time, end_time, is_active, created_at, updated_at FROM clinics WHERE name=@name";
      var parameters = new (string Name, object? Value)[] 
      {
        ("@name", name),
      };  
      return await ExecuteReaderAsync<Clinic?>(sql, parameters, Mappers.Mappers.ToClinic, token);
    }
  }
}
