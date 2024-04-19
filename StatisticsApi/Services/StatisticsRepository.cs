using StatisticsApi.Models;

namespace StatisticsApi.Services;

public class StatisticsRepository(ILogger<StatisticsRepository> logger) : IStatisticsRepository
{
    private readonly List<EndpointModel> _statisticsCollection = new();

    public Task<EndpointModel> GetLastCalledEndpoint()
    {
        try
        {
            return Task.FromResult(_statisticsCollection.MaxBy(e => e.LastCalled));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while getting last called endpoint");
            return Task.FromResult<EndpointModel>(null);
        }
    }

    public Task<EndpointModel> GetMostCalledEndpoint()
    {
        try
        {
            return Task.FromResult(_statisticsCollection.MaxBy(e => e.Calls));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while getting most called endpoint");
            return Task.FromResult<EndpointModel>(null); // Return null as per the original logic
        }
    }
    
    
    public Task<List<EndpointModel>> GetCallsPerEndpoint()
    {
        try
        {
            return Task.FromResult(_statisticsCollection.ToList());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while getting calls per endpoint");
            return Task.FromResult(new List<EndpointModel>()); // Return an empty list instead of null
        }
    }
    public Task<EndpointModel> UpdateCalledEndpoint(EndpointModel endpoint)
    {
        if (endpoint == null || string.IsNullOrEmpty(endpoint.Endpoint))
        {
            throw new ArgumentNullException(nameof(endpoint));
        }

        var existingEndpoint = _statisticsCollection.FirstOrDefault(e => e.Endpoint == endpoint.Endpoint);
        if (existingEndpoint != null)
        {
            existingEndpoint.Calls++;
            existingEndpoint.LastCalled = DateTime.UtcNow;
        }
        else
        {
            _statisticsCollection.Add(endpoint);
        }

        return Task.FromResult(existingEndpoint ?? endpoint);
    
    }
}