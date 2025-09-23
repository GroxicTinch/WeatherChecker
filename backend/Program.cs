var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddHttpClient();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("VueFrontendPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:5173")  // Your Vue dev server
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseRouting();
app.UseCors("VueFrontendPolicy");
app.UseAuthorization();

app.MapControllers();

app.Run();