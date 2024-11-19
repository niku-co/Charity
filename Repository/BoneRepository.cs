using Dapper;
using NikuAPI.Entities;
using NikuAPI.IRepository;
using System.Data.SqlClient;

namespace NikuAPI.Repository;
public class BoneRepository : IBoneRepository
{
    private readonly IConfiguration _configuration;
    public BoneRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<Bone> GetBoneById(string nationalCode)
    {
        var sql = @"select BoneCode, DependantsNumber, FullName from Bones where BoneCode = @nationalCode ";
        var connectionString = _configuration.GetConnectionString("DefaultConnection");
        using var connection = new SqlConnection(connectionString);
        var result = await connection.QuerySingleOrDefaultAsync<Bone>(sql, new { nationalCode = nationalCode });
        
        return result;
    }

    public async Task<string> UpdateBone(string nationalCode)
    {
        var sql = @"UPDATE Bones SET DependantsNumber = DependantsNumber - 1 WHERE BoneCode = @nationalCode 
                    ; Exec GetCurrentDateTime";
        var connectionString = _configuration.GetConnectionString("DefaultConnection");
        using var connection = new SqlConnection(connectionString);
        var result = await connection.QuerySingleOrDefaultAsync<string>(sql, new { nationalCode = nationalCode });
        return result;
    }
}
