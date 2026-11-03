using System.Data;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using StackExchange.Exceptional;

namespace Portfolio.Shared.Repositories;

public class BaseRepository
{
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly string? _connectionString;

    public BaseRepository(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
    {
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
        _connectionString = configuration.GetConnectionString("umbracoDbDSN");
    }

    private DynamicParameters BuildDapperParameters(IEnumerable<SqlParameter>? sqlParams)
    {
        var parameters = new DynamicParameters();

        if (sqlParams != null)
        {
            foreach (var p in sqlParams)
            {
                parameters.Add(p.ParameterName, p.Value);
            }
        }
        
        return parameters;
    }


    public async Task<T?> ExecuteScalarAsync<T>(string spName, IReadOnlyCollection<SqlParameter>? sqlParams = null, string? connectionString = null)
    {
        var cs = connectionString ?? _connectionString;

        try
        {
            await using var conn = new SqlConnection(cs);
            var parameters = BuildDapperParameters(sqlParams);

            return await conn.ExecuteScalarAsync<T>(
                spName,
                parameters,
                commandType: CommandType.StoredProcedure);
        }
        catch (Exception ex)
        {
            await ex.LogAsync(_httpContextAccessor.HttpContext);
            throw;
        }
    }


    public async Task<IEnumerable<T>> GetDataListAsync<T>(
        string spName,
        IReadOnlyCollection<SqlParameter>? sqlParams,
        string? connectionString = null)
        where T : new()
    {
        var cs = connectionString ?? _connectionString;
        try
        {
            await using var conn = new SqlConnection(cs);
            var parameters = BuildDapperParameters(sqlParams);

            return await conn.QueryAsync<T>(
                spName,
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        catch (Exception ex)
        {
            ex.Log(_httpContextAccessor.HttpContext);
            throw;
        }
    }

    public async Task<(List<T0>, List<T1>)> GetDataSetAsync<T0, T1>(
        string spName,
        IReadOnlyCollection<SqlParameter>? sqlParams, string? connectionString = null)
        where T0 : new()
        where T1 : new()
    {
        var cs = connectionString ?? _connectionString;

        try
        {
            await using var conn = new SqlConnection(cs);
            var parameters = BuildDapperParameters(sqlParams);

            using var multi = await conn.QueryMultipleAsync(
                spName,
                parameters,
                commandType: CommandType.StoredProcedure);

            var result0 = (await multi.ReadAsync<T0>()).ToList();
            var result1 = (await multi.ReadAsync<T1>()).ToList();

            return (result0, result1);
        }
        catch (Exception ex)
        {
            ex.Log(_httpContextAccessor.HttpContext);
            throw;
        }
    }


    public async Task<T?> GetDataSingleAsync<T>(
        string spName,
        IReadOnlyCollection<SqlParameter>? sqlParams,
        string? connectionString = null)
        where T : new()
    {
        var cs = connectionString ?? _connectionString;

        try
        {
            await using var conn = new SqlConnection(cs);
            var parameters = BuildDapperParameters(sqlParams);

            return await conn.QuerySingleOrDefaultAsync<T>(
                spName,
                parameters,
                commandType: CommandType.StoredProcedure);

        }
        catch (Exception ex)
        {
            ex.Log(_httpContextAccessor.HttpContext);
            throw;
        }
    }

    public async Task<int> ExecuteNonQueryAsync(
        string spName,
        IReadOnlyCollection<SqlParameter> sqlParams,
        string? connectionString = null)

    {
        var cs = connectionString ?? _connectionString;
        try
        {
            await using var conn = new SqlConnection(cs);
            var parameters = BuildDapperParameters(sqlParams);

            return await conn.ExecuteAsync(
                spName,
                parameters,
                commandType: CommandType.StoredProcedure);

        }
        catch (Exception ex)
        {
            ex.Log(_httpContextAccessor.HttpContext);
            throw;
        }
    }
}