using BenchmarkDotNet.Attributes;
using Microsoft.EntityFrameworkCore;
using MyBoardss.Entities;

namespace MyBoards.Benchmark;

[MemoryDiagnoser]
public class TrackingBenchmark
{
    private readonly string _connectionString;
    
    public TrackingBenchmark(string connectionString)
    {
        _connectionString = connectionString;
    }
    
    [Benchmark]
    public int WithTracking()
    {
        var optionsBuilder = new DbContextOptionsBuilder<MyBoardsContext>()
            .UseMySql(
                _connectionString,
                ServerVersion.AutoDetect(_connectionString)
            );
        using var _dbContext = new MyBoardsContext(optionsBuilder.Options);
        
        
        var comments = _dbContext.Comments.ToList();
        return comments.Count;
    }
    
    [Benchmark]
    public int WithNoTracking()
    {
        var optionsBuilder = new DbContextOptionsBuilder<MyBoardsContext>()
            .UseMySql(
                _connectionString,
                ServerVersion.AutoDetect(_connectionString)
            );
        using var _dbContext = new MyBoardsContext(optionsBuilder.Options);
        
        var comments = _dbContext.Comments
            .AsNoTracking()
            .ToList();
        
        return comments.Count;
    }
    
    
}