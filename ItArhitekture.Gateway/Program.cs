using AspNetCore.Proxy;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using TaskGrpcClientProto;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddProxies();

builder.Services.AddGrpc().AddJsonTranscoding();

builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowAll", policyBuilder => policyBuilder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseRouting();
app.UseCors("AllowAll");

app.MapGet("/", () => "Hi from the proxy!");

// Get object
// Forward object to the target GRPC service
// Return the response

app.MapGet("/api/tracking/{id}", async ([FromRoute]string id) =>
{

	var request = new GetTaskRequest { Id = id};
	var channel = GrpcChannel.ForAddress("http://tracking-management-grpc:9000");
	var client = new TaskGrpcService.TaskGrpcServiceClient(channel);
	var response = await client.GetTaskAsync(request);
	return response;
});

app.MapGet("/api/tracking/list/{category}", async (string category) =>
   {
   	var request = new ListTaskRequest() {Category = category};
   	var channel = GrpcChannel.ForAddress("http://tracking-management-grpc:9000");
   	var client = new TaskGrpcService.TaskGrpcServiceClient(channel);
   	var response = await client.ListTasksAsync(request);
   	return response;
   });

app.MapPost("/api/tracking", async ([FromBody]AddTaskRequest request) =>
{
	var channel = GrpcChannel.ForAddress("http://tracking-management-grpc:9000");
	var client = new TaskGrpcService.TaskGrpcServiceClient(channel);
	var response = await client.AddTaskAsync(request);
	return response;
});

app.MapPut("/api/tracking", async ([FromBody]UpdateTaskRequest request) =>
{
	var channel = GrpcChannel.ForAddress("http://tracking-management-grpc:9000");
	var client = new TaskGrpcService.TaskGrpcServiceClient(channel);
	var response = await client.UpdateTaskAsync(request);
	return response;
});

app.MapDelete("/api/tracking", async ([FromBody]DeleteTaskRequest request) =>
{
	var channel = GrpcChannel.ForAddress("http://tracking-management-grpc:9000");
	var client = new TaskGrpcService.TaskGrpcServiceClient(channel);
	var response = await client.DeleteTaskAsync(request);
	return response;
});

// Configure the proxy middleware
app.UseProxies(proxies =>
{
	proxies.Map("/api/assets", proxy => proxy.UseHttp("http://assets-management-api:9042/api/assets"));
	
	proxies.Map("/api/assets/{id}", proxy =>
		proxy.UseHttp((context, args) =>
		{
			var id = context.Request.RouteValues["id"] as string;
			var targetUrl = $"http://assets-management-api:9042/api/assets/{id}";
			return targetUrl;
		})
	);
	
	proxies.Map("/maintenance", proxy => proxy.UseHttp("http://maintenance-api:9044/api/maintenance"));
	proxies.Map("/maintenance/{id}", proxy =>
		proxy.UseHttp((context, args) =>
		{
			var id = context.Request.RouteValues["id"] as string;
			var targetUrl = $"http://maintenance-api:9044/api/maintenance/{id}";
			return targetUrl;
		})
	);
});

app.Run();