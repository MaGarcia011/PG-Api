using Microsoft.IdentityModel.Tokens;
using PG_Api.Context;
using PG_Api.Models.Dtos;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;

namespace PG_Api.Services;

public class AutorizacionService : IAutorizacionService
{
    private readonly DbPgContext _dbPgContext;
    private readonly IConfiguration _configuration;

    public AutorizacionService(DbPgContext context, IConfiguration configuration)
    {
        _dbPgContext = context;
        _configuration = configuration;
    }

    private string generateToken(string iduser)
    {
        //Rescatamos la key del token
        var key = _configuration.GetValue<string>("JwtSettings:key");
        var keyBytes = Encoding.ASCII.GetBytes(key); //Convertimos la llave en array

        //Informacion del usuario para el token y anadimos el id
        var claims = new ClaimsIdentity(); 
        claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, iduser));

        //credencial para el token
        var credencialToken = new SigningCredentials(
            new SymmetricSecurityKey(keyBytes),
            SecurityAlgorithms.HmacSha256Signature
            );

        //Descripcion
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = claims,
            Expires = DateTime.UtcNow.AddMinutes(1), //tiempo universal que dura 1 minuto
            SigningCredentials = credencialToken
        };

        //Controladores de JWT
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);

        //Generar el token
        string realtoken = tokenHandler.WriteToken(tokenConfig);

        return realtoken;
    }

    //Es la implementacion de la intefaz,nos ayuda a devolver, recibe los datos de peticion
    public async Task<AutorizacionResponse> returnToken(AutorizacionRequest autorizacion)
    {
        //Cadena tipo SQL de Entity, es como un select con la condicion
        var userFound = _dbPgContext.Users.FirstOrDefault(x =>
            x.Name == autorizacion.UserName &&
            x.Pwd == autorizacion.Pwd);

        if (userFound is null) {
            return await Task.FromResult<AutorizacionResponse>(null);
        }

        //Si los valores no son null entonces crea el token pasando el id en string 
        string tokencreated = generateToken(userFound.IdUser.ToString());

        //Se puede devolver el objeto, pero así es más rapido para no hacer tantas inyecciones de dependencias
        return new AutorizacionResponse() { Token = tokencreated, Result = true, Message = "Creado con éxito, tiene 1 minuto"};
    }


}
