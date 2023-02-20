using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TODOTEST.Models;

namespace TODOTEST.Repositories
{

    public class TodoRepository : ITodoRepository
    {
        private readonly DataContext _context;

        public TodoRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Todo> GetTodoByIdAsync(int id)
        {
            return await _context.Todos.FindAsync(id);
        }

        public async Task CreateAsync(Todo todo)
        {
            await _dbContext.Todos.AddAsync(todo);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Todo todo)
        {
            _dbContext.Todos.Update(todo);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var todo = await _dbContext.Todos.FindAsync(id);
            if (todo != null)
            {
                _dbContext.Todos.Remove(todo);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
