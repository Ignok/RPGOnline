using Microsoft.EntityFrameworkCore;
using RPGOnline.Application;
using RPGOnline.Application.Common.Interfaces;
using RPGOnline.Application.Interfaces;
using RPGOnline.Infrastructure;
using RPGOnline.Infrastructure.Models;
using RPGOnline.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//Enable CORS
builder.Services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });

builder.Services.AddControllers();

builder.Services.AddScoped<IApplicationDbContext, RPGOnlineDbContext>();
builder.Services.AddScoped<IUser, UserService>();
builder.Services.AddScoped<IAccount, AccountService>();


builder.Services.AddInfrastructure().AddApplication();
    

builder.Services.AddDbContext<RPGOnlineDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DBRPGOnline")));

//builder.Services.AddDbContext<ApplicationDbContext>();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Fajerbol"));
    app.UseSwaggerUI();
}

app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

//app.UseAuthorization();

app.MapControllers();

app.Run();
