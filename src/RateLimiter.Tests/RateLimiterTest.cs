using NUnit.Framework;
using RateLimiter.Interfaces;
using RateLimiter.Models;
using RateLimiter.Rules;
using RateLimiter.Services;

namespace RateLimiter.Tests;

[TestFixture]
public class RateLimiterTest
{
	[Test]
	[TestCase("", "/api/user/registration")]
	[TestCase("user-token", "")]
	public void AllowsEmptyParameters(string userToken, string resource)
	{
		var store = new DatabaseStore();

		var rule = new CheckUserRequestsCountPerSecondRule();
		var result = rule.CheckRule(store, userToken, resource);

		Assert.NotNull(store);
		Assert.NotNull(rule);
		Assert.True(result);
	}

	[Test]
	[TestCase("user-token", "/api/user/registration")]
	[TestCase("user-token", "/api/products")]
	public void AllowsNullStore(string userToken, string resource)
	{
		var rule = new CheckUserRequestsCountPerSecondRule();
		var result = rule.CheckRule(null, userToken, resource);

		Assert.NotNull(rule);
		Assert.True(result);
	}

	[Test]
	[TestCase("user-token", "/api/user/registration")]
	[TestCase("user-token", "/api/products")]
	public void Allows(string userToken, string resource)
	{
		var store = new DatabaseStore();
		store.Add(userToken, new List<UserDataModel>()
		{
			new() {Resource = resource, Date = DateTime.Now.AddSeconds(-15)},
			new() {Resource = resource, Date = DateTime.Now.AddSeconds(-23)},
			new() {Resource = resource, Date = DateTime.Now.AddSeconds(-45)},
			new() {Resource = resource, Date = DateTime.Now.AddSeconds(-59)}
		});

		var rule = new CheckUserRequestsCountPerSecondRule();
		var result = rule.CheckRule(store, userToken, resource);

		Assert.NotNull(store);
		Assert.NotNull(rule);
		Assert.True(result);
	}

	[Test]
	[TestCase("user-token", "/api/user/registration")]
	public void DoesNotAllow(string userToken, string resource)
	{
		var store = new DatabaseStore();
		store.Add(userToken, new List<UserDataModel>()
		{
			new() {Resource = resource, Date = DateTime.Now.AddSeconds(-15)},
			new() {Resource = resource, Date = DateTime.Now.AddSeconds(-23)},
			new() {Resource = resource, Date = DateTime.Now.AddSeconds(-45)},
			new() {Resource = resource, Date = DateTime.Now.AddSeconds(-20)},
			new() {Resource = resource, Date = DateTime.Now.AddSeconds(-42)},
			new() {Resource = resource, Date = DateTime.Now.AddSeconds(-42)}
		});

		var rule = new CheckUserRequestsCountPerSecondRule();
		var result = rule.CheckRule(store, userToken, resource);

		Assert.NotNull(store);
		Assert.NotNull(rule);
		Assert.False(result);
	}

	[Test]
	[TestCase("user-token", "/api/user/transactions")]
	[TestCase("user-token", "/api/products")]
	public void CheckLimitsAllows(string userToken, string resource)
	{
		var service = new ResourceLimitService();

		var result = service.CheckLimits(new List<IRule>() {new CheckUserRequestsCountPerSecondRule()},
			userToken, resource);

		Assert.NotNull(service);
		Assert.True(result);
	}
}