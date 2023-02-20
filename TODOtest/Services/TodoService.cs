using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TODOTEST.Services
{

    public class TodoService : ITodoService
    {
        private readonly ITodoRepository _todoRepository;

        public TodoService(ITodoRepository todoRepository)
        {
            _todoRepository = todoRepository;
        }

        public async Task<IEnumerable<TodoDto>> GetTodosAsync(string status = null)
        {
            var todos = await _todoRepository.GetTodosAsync(status);
            return todos.Select(t => new TodoDto(t));
        }

        public async Task<TodoDto> GetTodoByIdAsync(int id)
        {
            var todo = await _todoRepository.GetTodoByIdAsync(id);
            return new TodoDto(todo
    
}
