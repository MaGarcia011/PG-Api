using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PG_Api.Context;
using PG_Api.Repositories;
using PG_Api.Services;
using System.Linq.Expressions;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Declaración de la cadena para usarla en contexto
builder.Services.AddDbContext<DbPgContext>(opt => {
    opt.UseSqlServer(builder.Configuration.GetConnectionString("stringSql"));
});

//Para que service pueda ser usada en todo el proyect // Se puede poner en una carpeta
builder.Services.AddScoped<IAutorizacionService, AutorizacionService>();
builder.Services.AddScoped<FoodService>();
builder.Services.AddScoped<FoodRepository>();

//Configuración de un JWT para usarlo en el proyecto siempre se usan para ASPNET
var key = builder.Configuration.GetValue<string>("JwtSettings:key");
var keyBytes = Encoding.ASCII.GetBytes(key);
builder.Services.AddAuthentication(config =>
{
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(config => { 
    config.RequireHttpsMetadata = false;
    config.SaveToken = true;
    config.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
        ValidateIssuer = false, // no necesitamos saber quien hace peticion porque ya pedimos credenciales
        ValidateAudience = false, //no necesitamos el donde solicita el usuario ademas es local
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero //No se debe desviar con el tiempo de vida
    };
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
