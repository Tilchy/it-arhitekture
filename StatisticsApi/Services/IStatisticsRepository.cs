using StatisticsApi.Models;
using ErrorOr;
namespace StatisticsApi.Services;

public interface IStatisticsRepository
{
	ErrorOr<EndpointModel> GetLastCalledEndpoint();
	ErrorOr<EndpointModel> GetMostCalledEndpoint();
	ErrorOr<List<EndpointModel>> GetCallsPerEndpoint();
	ErrorOr<EndpointModel> UpdateCalledEndpoint(EndpointModel? endpoint);
}