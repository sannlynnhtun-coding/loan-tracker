namespace LoanTracker.Domain.Models;

public class ResponseModel
{
	public bool IsSuccess { get; set; }
	public string Message { get; set; }
	public object? Data { get; set; }
}
