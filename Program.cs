using FleetManagement.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// =============================
// ADD SERVICES
// =============================

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Bỏ lỗi vòng lặp navigation
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;

        // 🔥 Đồng bộ camelCase cho frontend JS
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

var app = builder.Build();

// =============================
// MIDDLEWARE
// =============================

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Load index.html tự động khi vào /
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseCors("AllowAll");

app.MapControllers();

app.Run();