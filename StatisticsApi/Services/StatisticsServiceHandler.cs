using ErrorOr;
using StatisticsApi.Models;

namespace StatisticsApi.Services;

public class StatisticsServiceHandler: IStatisticsServiceHandler
{
	private readonly IStatisticsRepository _statisticsRepository;
	private readonly ILogger<StatisticsServiceHandler> _logger;
	
	public StatisticsServiceHandler(IStatisticsRepository statisticsRepository, ILogger<StatisticsServiceHandler> logger)
	{
		_statisticsRepository = statisticsRepository;
		_logger = logger;
	}
	
	public ErrorOr<EndpointModel> GetLastCalledEndpoint()
	{
		try
		{
			return _statisticsRepository.GetLastCalledEndpoint();
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error while getting last called endpoint");
			return Error.Unexpected();
		}

	}

	public ErrorOr<EndpointModel> GetMostCalledEndpoint()
	{
		try
		{
			return _statisticsRepository.GetMostCalledEndpoint();
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error while getting most called endpoint");
			return Error.Unexpected();
		}
	}
	public  ErrorOr<List<EndpointModel>> GetCallsPerEndpoint()
	{
		try
		{
			return  _statisticsRepository.GetCallsPerEndpoint();
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error while getting calls per endpoint");
			return Error.Unexpected();
		}
	}
	public ErrorOr<EndpointModel> UpdateCalledEndpoint(EndpointModel? endpoint)
	{
		if (endpoint == null || string.IsNullOrEmpty(endpoint.Endpoint))
		{
			_logger.LogError("Endpoint is null or name is null or empty");
			return Error.Validation();
		}
		try
		{
			return  _statisticsRepository.UpdateCalledEndpoint(endpoint); ;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error while updating called endpoint");
			return Error.Unexpected();
		}
	}
}