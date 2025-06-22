using Microsoft.OpenApi.Models;
using Swagger_Demo.Examples;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

// הוספת קונטרולרים
builder.Services.AddControllers();

// הגדרות Swagger
builder.Services.AddSwaggerGen(c =>
{
    // קבוצה בשם "Users"
    c.SwaggerDoc("Users", new OpenApiInfo
    {
        Title = "Users API",
        Version = "v1"
    });

    // מאפשר תיעוד עם תגיות ודוגמאות
    c.EnableAnnotations();       // מאפשר שימוש ב-[SwaggerOperation]
    c.ExampleFilters();          // מאפשר דוגמאות לקלט/פלט
});

// הוספת הדוגמאות מתוך הקבצים שלך
builder.Services.AddSwaggerExamplesFromAssemblyOf<UserResponseExample>();
builder.Services.AddSwaggerExamplesFromAssemblyOf<ErrorResponseExample>();

// הוספת CORS אם צריך
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

// Middleware של CORS
app.UseCors();

// Middleware של Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/Users/swagger.json", "Users API");
});

// Middleware של HTTPS
app.UseHttpsRedirection();

// Middleware של הרשאות
app.UseAuthorization();

// Middleware של טיפול בשגיאות
app.UseExceptionHandler("/error"); // אם יש לך ErrorController, או אפשרות ל־Middleware פנימי

// ניתוב לקונטרולרים
app.MapControllers();

app.Run();
