using StatisticsApi.Models;

namespace StatisticsApi.Services;

public class StatisticsServiceHandler(
	IStatisticsRepository statisticsRepository,
	ILogger<StatisticsServiceHandler> logger)
{
	public  Task<EndpointModel> GetLastCalledEndpoint()
	{
		try
		{
			return statisticsRepository.GetLastCalledEndpoint();
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Error while getting last called endpoint");
			return null!;
		}

	}

	public Task<EndpointModel> GetMostCalledEndpoint()
	{
		try
		{
			return statisticsRepository.GetMostCalledEndpoint();
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Error while getting most called endpoint");
			return null!;
		}
	}
	public  Task<List<EndpointModel>> GetCallsPerEndpoint()
	{
		try
		{
			return  statisticsRepository.GetCallsPerEndpoint();
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Error while getting calls per endpoint");
			return null!;
		}
	}
	public Task<EndpointModel> UpdateCalledEndpoint(EndpointModel? endpoint)
	{
		if (endpoint == null || string.IsNullOrEmpty(endpoint.Endpoint))
		{
			logger.LogError("Endpoint is null or name is null or empty");
			return null!;
		}
		try
		{
			return  statisticsRepository.UpdateCalledEndpoint(endpoint); ;
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Error while updating called endpoint");
			return null!;
		}
	}
}