using PG_Api.Models.Dtos;

namespace PG_Api.Services
{
    public interface IAutorizacionService
    {
        Task<AutorizacionResponse> returnToken(AutorizacionRequest autorizacion);
    }
}
