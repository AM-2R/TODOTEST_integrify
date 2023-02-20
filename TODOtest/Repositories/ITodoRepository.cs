using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TODOTEST.Models;

namespace TODOTEST.Repositories
{
    public interface ITodoRepository
    {
        Task<Todo> GetTodoByIdAsync(int id);
        Task<List<Todo>> GetTodosByUserIdAsync(int userId);
        Task<List<Todo>> GetTodosByUserIdAndStatusAsync(int userId, TodoStatus status);
        Task CreateTodoAsync(Todo todo);
        Task UpdateTodoAsync(Todo todo);
        Task DeleteTodoAsync(Todo todo);
    }
}
