using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Data;
using Classes;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Configuración de Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Calasancio",
        Description = "Calasancio",
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

// Configuración de Entity Framework y base de datos
builder.Services.AddDbContext<DataContext>(opt =>
{
    if (builder.Environment.IsDevelopment())
    {
        string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        opt.UseSqlServer(connectionString);
    }
    else
    {
        opt.UseInMemoryDatabase("DataContext");
    }
});

// Configuración de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "MyAllowSpecificOrigins",
                      policy =>
                      {
                          policy.AllowAnyHeader()
                                .AllowAnyOrigin()
                                .AllowAnyMethod();
                      });
});

// Configuración de Stripe

var app = builder.Build();

// Configurar el pipeline de solicitudes HTTP
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("MyAllowSpecificOrigins");

app.MapControllers();

app.Run();
