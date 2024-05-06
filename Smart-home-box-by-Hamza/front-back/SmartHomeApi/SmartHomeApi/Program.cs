using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using SmartHomeApi.Data;
using SmartHomeApi.Helpers;

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", false)
    .Build();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<MyDBContext>(options => options.UseSqlServer(config.GetConnectionString("db1")));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<WhatsAppSender>();
builder.Services.AddTransient<SmsSender>();
builder.Services.AddTransient<EmailSender>();
builder.Services.AddTransient<AuthService>();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "SmartHomeApi",
        Description = ""
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();

app.UseCors(options => options
    .SetIsOriginAllowed(_ => true) // Omogućava pristup sa svih izvora
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials() // Dozvoljava korištenje kredencijala u CORS zahtjevima
);

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
