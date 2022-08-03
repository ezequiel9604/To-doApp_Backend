
using AutoMapper;
using System.Text;
using Domain.Repositories;
using Infrastructure.Mapper;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// setting up dbms
builder.Services.AddDbContext<TodoAppDbContext>(opts => opts.UseSqlServer(
    builder.Configuration.GetConnectionString("mainConnection")));


// setting up authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opts => opts.TokenValidationParameters = new TokenValidationParameters
    {
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            builder.Configuration.GetSection("AppSettings:TokenKey").Value)),
        ValidateIssuerSigningKey = true,
        ValidateIssuer = false,
        ValidateAudience = false
    });


// setting up repository scope
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUTaskRepository, UTaskRepository>();
builder.Services.AddScoped<IMailRepository, MailRepository>();
builder.Services.AddScoped<IChangeOfPasswordRepository, ChangeOfPasswordRepository>();


// setting up cors
builder.Services.AddCors(c => c.AddPolicy("corsapp", builder =>
{
    builder.WithOrigins("*").AllowAnyHeader().AllowAnyMethod();
}));


// setting up automapper
var mapperConfig = new MapperConfiguration(cfg =>
{
    cfg.AddProfile(new AutoMapperProfileConfiguration());
});

builder.Services.AddSingleton(mapperConfig.CreateMapper());


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("corsapp");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
