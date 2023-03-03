namespace RateLimiter.Interfaces;

public interface IRule
{
	public bool CheckRule(IDatabaseStore database, string userToken, string resource);
}