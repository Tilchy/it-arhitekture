using StatisticsApi.Models;

namespace StatisticsApi.Services;

public interface IStatisticsRepository
{
	Task<EndpointModel> GetLastCalledEndpoint();
	Task<EndpointModel> GetMostCalledEndpoint();
	Task<List<EndpointModel>> GetCallsPerEndpoint();
	Task<EndpointModel> UpdateCalledEndpoint(EndpointModel endpoint);
}