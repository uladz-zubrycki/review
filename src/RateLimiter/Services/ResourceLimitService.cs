using RateLimiter.Interfaces;

namespace RateLimiter.Services;

public class ResourceLimitService : IResourceLimitService
{
	public bool CheckLimits(IEnumerable<IRule> rules, string userToken, string resource)
	{
		if (rules.All(rule => rule.CheckRule(DatabaseStore.Instance, userToken, resource)))
		{
			return true;
		}

		return false;
	}
}