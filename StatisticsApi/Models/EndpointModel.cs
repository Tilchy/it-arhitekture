namespace StatisticsApi.Models;

public class EndpointModel
{
	public Guid? Id { get; set; }
	public string? Endpoint { get; set; }
	public int Calls { get; set; }
	public DateTime LastCalled { get; set; }
}