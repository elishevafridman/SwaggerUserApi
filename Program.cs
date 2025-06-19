using Microsoft.AspNetCore.Diagnostics;
using Swagger_Demo.Models;
using Swashbuckle.AspNetCore.Annotations;

var builder = WebApplication.CreateBuilder(args);

// שירותים ל־API וקונטרולרים
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// 🟦 Swagger עם תגיות
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("Users", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Users API",
        Version = "v1"
    });

    c.EnableAnnotations(); // מאפשר להשתמש ב־[SwaggerOperation]
});

// 🟦 CORS – פתוח לכל מקור
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// 🟦 טיפול חריגות גלובלי
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.ContentType = "application/json";
        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

        var response = new ErrorResponse
        {
            StatusCode = 500,
            Message = "An unexpected error occurred",
#if DEBUG
            Details = exception?.Message
#endif
        };

        context.Response.StatusCode = 500;
        await context.Response.WriteAsJsonAsync(response);
    });
});

// 🟦 הפעלת CORS
app.UseCors();

// 🟦 Swagger רק בפיתוח
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI(c =>
    {
        // קישור למסמך שנוצר עבור קבוצת Users
        c.SwaggerEndpoint("/swagger/Users/swagger.json", "Users API");
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
