using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RPGOnline.API.Helpers;
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

builder.Services.Configure<BlobStorageConf>(configuration.GetSection("BlobStorage"));

builder.Services.AddScoped<IApplicationDbContext, RPGOnlineDbContext>();
builder.Services.AddScoped<IUser, UserService>();
builder.Services.AddScoped<IFriendship, FriendshipService>();
builder.Services.AddScoped<IPost, PostService>();
builder.Services.AddScoped<IAccount, AccountService>();
builder.Services.AddScoped<IMessage, MessageService>();
builder.Services.AddScoped<IRace, RaceService>();
builder.Services.AddScoped<IItem, ItemService>();
builder.Services.AddScoped<ISpell, SpellService>();
builder.Services.AddScoped<IProfession, ProfessionService>();
builder.Services.AddScoped<ICharacter, CharacterService>();


builder.Services.AddInfrastructure().AddApplication();
    

builder.Services.AddDbContext<RPGOnlineDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DBRPGOnline")));


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(opt =>
{
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.FromMinutes(60),
        ValidIssuer = configuration["JWT:ValidIssuer"],
        ValidAudience = configuration["JWT:ValidAudience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
    };
    opt.SaveToken = true;

    opt.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
            {
                context.Response.Headers.Add("Token-expired", "true");
            }
            return Task.CompletedTask;
        },
        OnMessageReceived = context => {

            if (context.Request.Cookies.ContainsKey("AccessToken"))
            {
                context.Token = context.Request.Cookies["AccessToken"];
            }

            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization(options =>
{

});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


app.UseDeveloperExceptionPage();
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Fajerbol"));


app.UseCors("AllowSetOrigins");

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
