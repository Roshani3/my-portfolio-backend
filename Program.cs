using Portfolio.Backend.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

builder.Services.AddScoped<IEmailService, EmailService>();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
// Enable CORS
app.UseCors("AllowAngular");

app.UseAuthorization();
app.MapControllers();

app.Run();