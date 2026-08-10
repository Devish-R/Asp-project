using Amazon;
using Amazon.S3;
using EmployeeManagement.Data;
using EmployeeManagement.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString)
    ));

builder.Services.AddSingleton<IAmazonS3>(sp =>
{
    var region =
        builder.Configuration["AWS:Region"];

    return new AmazonS3Client(
        RegionEndpoint.GetBySystemName(region)
    );
});

builder.Services.AddScoped<S3Service>();

builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapControllers();

app.Run();