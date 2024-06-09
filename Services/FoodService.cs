using PG_Api.Models.Entity;
using PG_Api.Repositories;

namespace PG_Api.Services;
public class FoodService (FoodRepository _repository)
{
    public async Task<IEnumerable<Food>> GetAll () => 
        await _repository.GetAll ();
}
