using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RPGOnline.API.Middlewares;
using RPGOnline.Application;
using RPGOnline.Application.Common.Interfaces;
using RPGOnline.Application.Interfaces;
using RPGOnline.Infrastructure;
using RPGOnline.Infrastructure.Models;
using RPGOnline.Infrastructure.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
configuration.AddUserSecrets("aaa");
// Add services to the container.

//builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//Enable CORS
builder.Services.AddCors(c =>
            {
                c.AddPolicy("AllowSetOrigins", options => {
                    options.WithOrigins("http://localhost:3000");
                    options.SetIsOriginAllowed(host => true);
                    options.AllowAnyHeader();
                    options.AllowAnyMethod();
                    options.AllowCredentials();
                });
            });

builder.Services.AddControllers();

builder.Services.AddScoped<IApplicationDbContext, RPGOnlineDbContext>();
builder.Services.AddScoped<IUser, UserService>();
builder.Services.AddScoped<IPost, PostService>();
builder.Services.AddScoped<IAccount, AccountService>();
builder.Services.AddScoped<IMessage, MessageService>();
builder.Services.AddScoped<IRace, RaceService>();
builder.Services.AddScoped<IItem, ItemService>();


builder.Services.AddInfrastructure().AddApplication();
    

builder.Services.AddDbContext<RPGOnlineDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DBRPGOnline")));

//builder.Services.AddDbContext<ApplicationDbContext>();


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    //options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(opt =>
{
    opt.SaveToken = true;

    opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.FromMinutes(10),
        ValidIssuer = configuration["JWT:ValidIssuer"],
        ValidAudience = configuration["JWT:ValidAudience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
    };

    opt.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
            {
                context.Response.Headers.Add("Token-expired", "true");
            }
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.


//app.UseGreatErrorHandling();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Fajerbol"));
    app.UseSwaggerUI();
}

app.UseCors("AllowSetOrigins");

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
