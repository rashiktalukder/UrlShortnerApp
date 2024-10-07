using Microsoft.EntityFrameworkCore;
using UrlShortner.API.Application.Services;
using UrlShortner.API.Domain.Interfaces;
using UrlShortner.API.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseMySQL(builder.Configuration.GetConnectionString("UrlShortenerDbConnStr")));
builder.Services.AddScoped<IUrlShortnerRepository,UrlShortnerService>();
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
