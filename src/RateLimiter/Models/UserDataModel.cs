using RateLimiter.Interfaces;

namespace RateLimiter.Models;

public class UserDataModel : IUserData
{
	public string Resource { get; set; }
	public DateTime Date { get; set; }
}