using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AspNetCore.Proxy;
using Grpc.Net.Client;
using ItArhitekture.Gateway;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
	{
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = false,
			ValidateAudience = false,
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true,
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your_secret_key_here_that_is_longer_than_256_bits"))
		};
	});

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.UseRouting();

app.UseAuthentication(); // Make sure this is placed after UseRouting and before UseAuthorization
app.UseAuthorization();

app.UseMiddleware<StatisticsMiddleware>();

// Add token authorization
app.MapGet("/", () => "Hi from the proxy!");

app.MapGet("/generate-token", () =>
{
	var tokenHandler = new JwtSecurityTokenHandler();
	var key = Encoding.ASCII.GetBytes("your_secret_key_here_that_is_longer_than_256_bits");
	var tokenDescriptor = new SecurityTokenDescriptor
	{
		Subject = new ClaimsIdentity(
		[
			new Claim(ClaimTypes.Name, "user")
		]),
		Expires = DateTime.UtcNow.AddDays(7),
		SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
	};
	var token = tokenHandler.CreateToken(tokenDescriptor);
	var tokenString = tokenHandler.WriteToken(token);

	return Results.Ok(new { Token = tokenString });
});

// Get object
// Forward object to the target GRPC service
// Return the response

app.MapGet("/api/tracking/{id}", async ([FromRoute] string id) =>
{
	var request = new GetTaskRequest { Id = id };
	var channel = GrpcChannel.ForAddress("http://tracking-management-grpc:9000");
	var client = new TaskGrpcService.TaskGrpcServiceClient(channel);
	var response = await client.GetTaskAsync(request);
	return response;
}).RequireAuthorization();

app.MapGet("/api/tracking/list/{category}", async (string category) =>
   {
   	var request = new ListTaskRequest() { Category = category };
   	var channel = GrpcChannel.ForAddress("http://tracking-management-grpc:9000");
   	var client = new TaskGrpcService.TaskGrpcServiceClient(channel);
   	var response = await client.ListTasksAsync(request);
   	return response;
   }).RequireAuthorization();

app.MapPost("/api/tracking", async ([FromBody] AddTaskRequest request) =>
{
	var channel = GrpcChannel.ForAddress("http://tracking-management-grpc:9000");
	var client = new TaskGrpcService.TaskGrpcServiceClient(channel);
	var response = await client.AddTaskAsync(request);
	return response;
}).RequireAuthorization();

app.MapPut("/api/tracking", async ([FromBody] UpdateTaskRequest request) =>
{
	var channel = GrpcChannel.ForAddress("http://tracking-management-grpc:9000");
	var client = new TaskGrpcService.TaskGrpcServiceClient(channel);
	var response = await client.UpdateTaskAsync(request);
	return response;
}).RequireAuthorization();

app.MapDelete("/api/tracking", async ([FromBody] DeleteTaskRequest request) =>
{
	var channel = GrpcChannel.ForAddress("http://tracking-management-grpc:9000");
	var client = new TaskGrpcService.TaskGrpcServiceClient(channel);
	var response = await client.DeleteTaskAsync(request);
	return response;
}).RequireAuthorization();

// Configure the proxy middleware
app.UseProxies(proxies =>
{
	proxies.Map("/api/assets", proxy => proxy
		.UseHttp(async (context, args) =>
		{
			// Check if the user is authenticated
			if (context.User.Identity == null || !context.User.Identity.IsAuthenticated)
			{
				context.Response.StatusCode = StatusCodes.Status401Unauthorized;
				await context.Response.WriteAsync("Unauthorized");
				return string.Empty; // End the request here
			}

			// If authenticated, proceed to forward the request
			var targetUrl = "http://assets-management-api:9042/api/assets";
			return targetUrl;
		}));

	proxies.Map("/api/assets/{id}", proxy =>
		proxy.UseHttp(async (context, args) =>
		{
			if (context.User.Identity == null || !context.User.Identity.IsAuthenticated)
			{
				context.Response.StatusCode = StatusCodes.Status401Unauthorized;
				await context.Response.WriteAsync("Unauthorized");
				return string.Empty;
			}

			var id = context.Request.RouteValues["id"] as string;
			var targetUrl = $"http://assets-management-api:9042/api/assets/{id}";
			return targetUrl;
		})
	);

	proxies.Map("/maintenance", proxy => proxy.UseHttp(async (context, args) =>
	{
		if (context.User.Identity == null || !context.User.Identity.IsAuthenticated)
		{
			context.Response.StatusCode = StatusCodes.Status401Unauthorized;
			await context.Response.WriteAsync("Unauthorized");
			return string.Empty;
		}

		return "http://maintenance-api:9044/api/maintenance";
	}));

	proxies.Map("/maintenance/{id}", proxy =>
		proxy.UseHttp(async (context, args) =>
		{
			if (context.User.Identity == null || !context.User.Identity.IsAuthenticated)
			{
				context.Response.StatusCode = StatusCodes.Status401Unauthorized;
				await context.Response.WriteAsync("Unauthorized");
				return string.Empty;
			}

			var id = context.Request.RouteValues["id"] as string;
			var targetUrl = $"http://maintenance-api:9044/api/maintenance/{id}";
			return targetUrl;
		})
	);
});

app.Run();