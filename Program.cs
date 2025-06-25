
using System.Reflection;

using Microsoft.OpenApi.Models;
using Swagger_Demo.Examples;
using Swagger_Demo.Models;
using Swagger_Demo.Models.Exceptions;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});


builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
// הגדרות Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("Users", new OpenApiInfo
    {
        Title = "Users API",
        Version = "v1"
    });

    // מאפשר תיעוד עם תגיות ודוגמאות
    c.EnableAnnotations();     
    c.ExampleFilters();         
});


builder.Services.AddSwaggerExamplesFromAssemblyOf<UserResponseExample>();
builder.Services.AddSwaggerExamplesFromAssemblyOf<BadRequestErrorResponseExample>();
builder.Services.AddSwaggerExamplesFromAssemblyOf<NotFoundErrorResponseExample>();


builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseCors();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
// Middleware של CORS
app.UseCors();

// Middleware של Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/Users/swagger.json", "Users API");
});
app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (Exception ex)
    {
        var response = context.Response;
        response.ContentType = "application/json";

        int statusCode = ex switch
        {
            ApiException apiEx => apiEx.StatusCode,
            _ => 500
        };

        var error = new ErrorResponse
        {
            StatusCode = statusCode,
            Message = ex.Message,
            Details = ex.InnerException?.Message
        };

        response.StatusCode = statusCode;
        await response.WriteAsJsonAsync(error);
    }
});

// Middleware של HTTPS
app.UseHttpsRedirection();

// Middleware של הרשאות
app.UseAuthorization();

// Middleware של טיפול בשגיאות
app.UseExceptionHandler("/error"); 

// ניתוב לקונטרולרים
app.MapControllers();

app.Run();
