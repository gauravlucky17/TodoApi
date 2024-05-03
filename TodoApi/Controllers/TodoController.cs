using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly TodoContext _todoContext;
        public TodoController(TodoContext todoContext)
        {
            _todoContext = todoContext;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodo()
        {
            if (_todoContext.Items == null)
            {
                return NotFound();
            }
            return await _todoContext.Items.ToListAsync();
        }
        [HttpGet("id")]
        public async Task<ActionResult<TodoItem>> GetTodo(int id)
        {
            if (_todoContext.Items == null)
            {
                return NotFound();
            }
            var todo= await _todoContext.Items.FindAsync(id);
            if(todo == null)
            {
                return NotFound();
            }
            return todo;
        }
        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodo(TodoItem todo)
        {
            _todoContext.Items.Add(todo);
            await _todoContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTodo), new {id=todo.Id},todo);
        }

        [HttpPut]
        public async Task<IActionResult> PutTodo(int  id, TodoItem todo )
        {
            if(id!=todo.Id)
            {
                return BadRequest();
            }
            _todoContext.Entry(todo).State = EntityState.Modified;
            try
            {
                await _todoContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {

            }
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(int id)
        {
            if(_todoContext.Items==null)
            {
                return NotFound();
                //when no object is found
            }
            var todoItem = await _todoContext.Items.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            _todoContext.Items.Remove(todoItem);
            await _todoContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
