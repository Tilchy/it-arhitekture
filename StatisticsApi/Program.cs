using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using StatisticsApi.Models;
using StatisticsApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();
builder.Services.AddHealthChecks();

builder.Services.AddSingleton<IStatisticsServiceHandler, StatisticsServiceHandler>();
builder.Services.AddSingleton<IStatisticsRepository, StatisticsRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("last-called-endpoint", (IStatisticsServiceHandler serviceHandler) =>
{
    try
    {
        var result = serviceHandler.GetLastCalledEndpoint();
        
        if (result.IsError)
        {
            return Results.BadRequest(result.Errors);
        }

        return Results.Ok(result.Value);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
})
.WithName("GetLastCalledEndpoint")
.WithOpenApi();


app.MapGet("most-called-endpoint", (IStatisticsServiceHandler statisticsService) =>
    {
        try
        {
            var result = statisticsService.GetMostCalledEndpoint();

            if (result.IsError)
            {
                return Results.BadRequest(result.Errors);
            }

            return Results.Ok(result.Value);
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    })
    .WithName("GetMostCalledEndpoint")
    .WithOpenApi();

app.MapGet("calls-per-endpoint", (IStatisticsServiceHandler statisticsService) =>
    {
        try
        {
            var result = statisticsService.GetCallsPerEndpoint();

            if (result.IsError)
            {
                return Results.BadRequest(result.Errors);
            }

            return Results.Ok(result.Value);
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }

    })
    .WithName("GetCallsPerEndpoint")
    .WithOpenApi();


app.MapPost("/endpoint", (IStatisticsServiceHandler statisticsService, [FromBody] string endpoint) =>
    {
        try
        {
            if (string.IsNullOrWhiteSpace(endpoint))
            {
                return Results.BadRequest(Error.Validation());
            }
            
            var endpointModel = new EndpointModel
            {
                Endpoint = endpoint
            };

            var result = statisticsService.UpdateCalledEndpoint(endpointModel);

            if (result.IsError)
            {
                return Results.BadRequest(result.Errors);
            }
            return Results.Ok(result.Value);
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    })
    .WithName("PostEndpointStatisticToDatabase")
    .WithOpenApi();


app.Run();