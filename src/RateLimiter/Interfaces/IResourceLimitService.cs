namespace RateLimiter.Interfaces;

public interface IResourceLimitService
{
	public bool CheckLimits(IEnumerable<IRule> rules, string userToken, string resource);
}