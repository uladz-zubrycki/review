namespace RateLimiter.Interfaces;

public interface IDatabaseStore
{
	public void Add(string userToken, IEnumerable<IUserData> value);

	public void Update(
		string userToken,
		IEnumerable<IUserData> value,
		IEnumerable<IUserData> comparisonValue);

	public IEnumerable<IUserData> Get(string userToken);
}