using RateLimiter.Interfaces;
using System.Collections.Concurrent;

namespace RateLimiter.Services;

public class DatabaseStore : IDatabaseStore
{
    private static readonly ConcurrentDictionary<string, IEnumerable<IUserData>> Store = new();
    public static readonly DatabaseStore Instance = new();

    public void Update(
        string userToken,
        IEnumerable<IUserData> value,
        IEnumerable<IUserData> comparisonValue)
    {
        Store.TryUpdate(userToken, value, comparisonValue);
    }

    public void Add(string userToken, IEnumerable<IUserData> value)
    {
        Store.TryAdd(userToken, value);
    }

    public IEnumerable<IUserData> Get(string userToken)
    {
        Store.TryGetValue(userToken, out var value);
        return value;
    }
}