using ErrorOr;
using StatisticsApi.Models;

namespace StatisticsApi.Services;

public class StatisticsRepository(ILogger<StatisticsRepository> logger) : IStatisticsRepository
{
    private readonly List<EndpointModel> _statisticsCollection = new();

    public ErrorOr<EndpointModel> GetLastCalledEndpoint()
    {
        try
        {
            var result = _statisticsCollection.MaxBy(e => e.LastCalled);

            if (result == null)
            {
                return Error.NotFound();
            }
        
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while getting last called endpoint.");
            return Error.Unexpected(); 
        }   
            
    }

    public ErrorOr<EndpointModel> GetMostCalledEndpoint()
    {
        try
        {
            var result = _statisticsCollection.MaxBy(e => e.Calls);

            if (result == null)
            {
                return Error.NotFound();
            }

            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while getting most called endpoint");
            return Error.Unexpected(); 
        }
    }
    
    
    public ErrorOr<List<EndpointModel>> GetCallsPerEndpoint()
    {
        try
        {
            return _statisticsCollection.ToList();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while getting calls per endpoint");
            return Error.Unexpected();
        }
    }
    public ErrorOr<EndpointModel> UpdateCalledEndpoint(EndpointModel? endpoint)
    {
        try
        {
            if (endpoint == null || string.IsNullOrEmpty(endpoint.Endpoint))
            {
                return Error.Validation();
            }

            var existingEndpoint = _statisticsCollection.FirstOrDefault(e => e.Endpoint == endpoint.Endpoint);
            if (existingEndpoint != null)
            {
                existingEndpoint.Calls++;
                existingEndpoint.LastCalled = DateTime.UtcNow;
            }
            else
            {
                endpoint.Id = Guid.NewGuid();
                endpoint.Calls++;
                endpoint.LastCalled = DateTime.UtcNow;    
                _statisticsCollection.Add(endpoint);
            }

            return existingEndpoint ?? endpoint;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while updating called endpoint");
            return Error.Unexpected();
        }
    }
}