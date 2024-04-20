using System.Text;
using Newtonsoft.Json;

namespace ItArhitekture.Gateway;

public class StatisticsMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<StatisticsMiddleware> _logger;
    private readonly IHttpClientFactory _httpClientFactory;

    public StatisticsMiddleware(RequestDelegate next, ILogger<StatisticsMiddleware> logger, IHttpClientFactory httpClientFactory)
    {
        _next = next;
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var endpoint = context.Request.Path.Value;
        var httpClient = _httpClientFactory.CreateClient();

        _logger.LogInformation($"Endpoint: {endpoint}, Path: {context.Request.Path}");
        
        var jsonPayload = JsonConvert.SerializeObject(endpoint);

        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

        try
        {
            var response = await httpClient.PostAsync("http://statistics-service:8080/endpoint", content);
            _logger.LogInformation($"Response StatusCode: {response.StatusCode}, Content: {response.Content}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error posting to statistics service: {ex.Message}");
        }

        await _next(context);
    }

}