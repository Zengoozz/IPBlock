using API.Repositories;
using API.Services;
using IPGeolocation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton(sp => new IPGeolocationAPI("61aa6245355a4bb897fbb6eed56c1457"));
builder.Services.AddSingleton<IPAddressLookupService>(); 
builder.Services.AddSingleton<BlockedCountriesRepository>();
builder.Services.AddSingleton<ApiResponsesRepository>();


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
