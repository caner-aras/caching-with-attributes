using caching_with_attributes_api;
using caching_with_attributes_api.Caching;
using Castle.DynamicProxy;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Setup Interception
builder.Services.AddSingleton(new ProxyGenerator());
builder.Services.AddScoped<IInterceptor, CacheInterceptor>();

// Register with the AddProxiedScope method.  When a class needs an 
// IPersonRepository injected, then DynamicProxy will create a proxy
// object by adding a caching layer on top of the real 
// PersonRepository.  That proxy object will be injected into 
// the class that needs an IPersonRepository injected into it.
builder.Services.AddProxiedScoped<IWeatherService, WeatherService>();

// Other registrations
builder.Services.AddScoped<AnotherSerivce>();

// This was already in this method because we're doing MVC
builder.Services.AddControllersWithViews();

builder.Services.AddControllers();
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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
