using AppScheduler.Data;
using AppScheduler.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDBContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("AppSchedulerDB"));
});
builder.Services.AddScoped<IAppointmentService, AppointmentService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AppScheduler", policy =>
    {
        policy.WithOrigins("http://localhost:4000")
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

var app = builder.Build();


app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("AppScheduler");

app.MapControllers();

app.Run();
