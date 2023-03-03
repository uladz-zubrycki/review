namespace RateLimiter.Interfaces;

public interface IUserData
{
	public string Resource { get; set; }
	public DateTime Date { get; set; }
}