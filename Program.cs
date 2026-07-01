using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory; // 1. Imported the Caching namespace

var builder = WebApplication.CreateBuilder(args);

// 2. Add Memory Cache to the Dependency Injection container
builder.Services.AddMemoryCache(); 

var app = builder.Build();

// Mock Data Source 
var mockPostDatabase = new List<Post>
{
    new(Guid.NewGuid(), "user123", "Hello world! This is my first post.", DateTime.UtcNow.AddMinutes(-50)),
    new(Guid.NewGuid(), "user456", "Just drinking some coffee...", DateTime.UtcNow.AddMinutes(-45)),
    new(Guid.NewGuid(), "user123", "System design assignments are fun.", DateTime.UtcNow.AddMinutes(-30)),
    new(Guid.NewGuid(), "user789", "Is anyone using .NET 8/9 here?", DateTime.UtcNow.AddMinutes(-15)),
    new(Guid.NewGuid(), "user123", "Adding a caching layer next!", DateTime.UtcNow.AddMinutes(-5))
};

// 3. Inject IMemoryCache into the endpoint
app.MapGet("/feed/{userId}", ([FromRoute] string userId, IMemoryCache cache) =>
{
    // Define a unique cache key for this specific user
    string cacheKey = $"feed_{userId}";

    // STEP A: Try to get the feed from the cache (CACHE HIT)
    if (cache.TryGetValue(cacheKey, out List<Post>? cachedFeed))
    {
        return Results.Ok(new { 
            UserId = userId, 
            Source = "Cache (In-Memory Hit! ⚡)", // Notice the source change
            Count = cachedFeed!.Count, 
            Data = cachedFeed 
        });
    }

    // STEP B: If not in cache (CACHE MISS), hit the database
    Thread.Sleep(200); // Simulated 200ms DB latency

    var userFeed = mockPostDatabase
        .Where(p => p.UserId == userId)
        .OrderByDescending(p => p.CreatedAt)
        .ToList();

    // STEP C: Save the database result into the cache for next time
    // 4. Set a Time-To-Live (TTL) of 30 seconds
    var cacheOptions = new MemoryCacheEntryOptions()
        .SetAbsoluteExpiration(TimeSpan.FromSeconds(30));

    cache.Set(cacheKey, userFeed, cacheOptions);

    return Results.Ok(new { 
        UserId = userId, 
        Source = "Database (Cache Miss 🐢)", 
        Count = userFeed.Count, 
        Data = userFeed 
    });
});

app.Run();

public record Post(Guid Id, string UserId, string Content, DateTime CreatedAt);