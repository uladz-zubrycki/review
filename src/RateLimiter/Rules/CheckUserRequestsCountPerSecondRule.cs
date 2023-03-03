using RateLimiter.Constants;
using RateLimiter.Interfaces;
using RateLimiter.Models;

namespace RateLimiter.Rules;

public class CheckUserRequestsCountPerSecondRule : IRule
{
	public bool CheckRule(IDatabaseStore database, string userToken, string resource)
	{
		if (database == null ||
		    string.IsNullOrEmpty(userToken) ||
		    string.IsNullOrEmpty(resource))
		{
			return true;
		}

		var now = DateTime.Now;
		var storeRecords = database.Get(userToken);
		var newRecord = new List<UserDataModel>
		{
			new()
			{
				Resource = resource,
				Date = now
			}
		};

		if (storeRecords == null)
		{
			database.Add(userToken, newRecord);
			return true;
		}

		var tryCount = storeRecords.Count(
			x => x.Date >= now.AddSeconds(-RuleConstants.TimeSeconds) &&
			     x.Resource == resource);

		var newRecords = storeRecords.Concat(newRecord);
		database.Update(userToken, newRecords, storeRecords);

		return tryCount < RuleConstants.TryCount;
	}
}