using ApiVentasLoteria.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => 
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

builder.Services.AddScoped<SeguridadData>();
builder.Services.AddScoped<ConfiguracionData>();
builder.Services.AddScoped<JuegosData>();
builder.Services.AddScoped<LogData>();


builder.Services.AddCors(
    options => {
        options.AddPolicy(name: "API",
        builder =>
        {
            builder.WithHeaders("*");
            builder.WithOrigins("*");
            builder.WithMethods("*");
        });
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("API");

app.MapControllers();

app.Run();
