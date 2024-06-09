using Microsoft.EntityFrameworkCore;
using PG_Api.Context;
using PG_Api.Models.Entity;

namespace PG_Api.Repositories;
public class FoodRepository (DbPgContext _context)
{
    public async Task<IEnumerable<Food>> GetAll() { 
        var food = await _context.Foods.ToListAsync();
        return food;
    }
}
