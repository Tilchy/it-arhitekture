var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}


app.MapGet("statistics/last-called-endpoint", async (IStatisticsServiceHandler statisticsService) =>
{
    try
    {
        var result = await statisticsService.GetLastCalledEndpointAsync();

        if (result == null)
        {
            return Results.NotFound();
        }

        return Results.Ok(result);

    }catch(Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
})
.WithName("GetLastCalledEndpoint")
.WithOpenApi();

app.MapGet("statistics/most-called-endpoint", async (IStatisticsServiceHandler statisticsService) =>
{
    try
    {
        var result = await statisticsService.GetMostCalledEndpointAsnyc();

        if (result == null)
        {
            return Results.NotFound();
        }

        return Results.Ok(result);

    }catch(Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
})
.WithName("GetMostCalledEndpoint")
.WithOpenApi();

app.MapGet("statistics/calls-per-endpoint", async (IStatisticsServiceHandler statisticsService) =>
{
    try
    {
        var result = await statisticsService.GetCallsPerEndpointAsync();

        if (result == null)
        {
            return Results.NotFound();
        }

        return Results.Ok(result);

    }catch(Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }

})
.WithName("GetCallsPerEndpoint")
.WithOpenApi();

app.MapPost("statistics/endpoint", async (IStatisticsServiceHandler statisticsService, EndpointDto endpoint) =>
{
    try
    {
        var endpointModel = new EndpointModel
        {
            Endpoint = endpoint.Endpoint,
            LastCalled = endpoint.LastCalled
        };

        var result = await statisticsService.UpdateCalledEndpointAsync(endpointModel);

        if (result == null)
        {
            return Results.NotFound();
        }
        return Results.Ok(result);

    }catch(Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
})
.WithName("PostEndpointStatisticToDatabase")
.WithOpenApi();

app.Run();
