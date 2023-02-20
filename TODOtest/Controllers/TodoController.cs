using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TODOTEST.Models;

using TODOTEST.Services;

[RoutePrefix("api/v1/todos")]

namespace TODOTEST.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/todos")]
    public class TodoController : ControllerBase
    {
        private readonly ITodoService _todoService;

        public TodoController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoDto>>> GetTodos([FromQuery] TodoStatus? status)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var todos = await _todoService.GetTodosByUserIdAsync(userId, status);
            var todoDtos = todos.Select(todo => new TodoDto(todo));
            return Ok(todoDtos);
        }

        [HttpPost]
        public async Task<ActionResult<TodoDto>> CreateTodo([FromBody] TodoCreateDto todoCreateDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var todo = await _todoService.CreateTodoAsync(userId, todoCreateDto);
            var todoDto = new TodoDto(todo);
            return CreatedAtAction(nameof(GetTodoById), new { id = todo.Id }, todoDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TodoDto>> GetTodoById([FromRoute] int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var todo = await _todoService.GetTodoByIdAsync(userId, id);
            if (todo == null)
            {
                return NotFound();
            }
            var todoDto = new TodoDto(todo);
            return Ok(todoDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TodoDto>> UpdateTodo([FromRoute] int id, [FromBody] TodoUpdateDto todoUpdateDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var todo = await _todoService.UpdateTodoAsync(userId, id, todoUpdateDto);
            if (todo == null)
            {
                return NotFound();
            }
            var todoDto = new TodoDto(todo);
            return Ok(todoDto);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTodo([FromRoute] int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var success = await _todoService.DeleteTodoAsync(userId, id);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }
    }


}
