using AspNetCore.Proxy;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddProxies();

builder.Services.AddGrpc().AddJsonTranscoding();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.MapGet("/", () => "Hi from the proxy!");

// Configure the proxy middleware
app.UseProxies(proxies =>
{
	proxies.Map("/api/assets", proxy => proxy.UseHttp("http://assets-management-api:9042/api/assets"));
	
	proxies.Map("/maintenance", proxy => proxy.UseHttp("http://maintenance-api:9044/maintenance"));
	
});

app.Run();