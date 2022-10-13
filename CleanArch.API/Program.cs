using Microsoft.EntityFrameworkCore;
using RPGOnline.API.Data;
using RPGOnline.Application;
using RPGOnline.Application.Common.Interfaces;
using RPGOnline.Infrastructure;
using RPGOnline.Infrastructure.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

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

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

//app.UseAuthorization();

app.MapControllers();

app.Run();
