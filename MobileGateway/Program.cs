using AspNetCore.Proxy;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddProxies();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}


app.Use(async (context, next) => 
{
	if (context.Request.Method != HttpMethods.Get)
	{
		context.Response.StatusCode= StatusCodes.Status405MethodNotAllowed;
	}
	else
	{
		await next();
	}
});

app.UseProxies(proxies =>
{
	proxies.Map("/api/assets", proxy => proxy.UseHttp("http://assets-management-api:9042/api/assets"));	
    
	proxies.Map("/maintenance", proxy => proxy.UseHttp("http://maintenance-api:9044/maintenance"));
});


app.Run();
