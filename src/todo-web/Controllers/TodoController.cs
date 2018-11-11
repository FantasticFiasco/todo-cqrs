using System;
using System.Collections.Generic;
using System.Linq;
using Cqrs;
using Microsoft.AspNetCore.Mvc;
using Todo.ReadModel;
using Todo.Web.Controllers.DataTransferObjects;

namespace Todo.Web.Controllers
{
    [ApiController]
    [Route("todos")]
    public class TodoController : ControllerBase
    {
        private readonly MessageDispatcher messageDispatcher;
        private readonly ITodoList todoList;

        public TodoController(MessageDispatcher messageDispatcher, ITodoList todoList)
        {
            this.messageDispatcher = messageDispatcher;
            this.todoList = todoList;
        }

        /// <summary>
        /// Adds a new todo.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(TodoResponse), 201)]
        public IActionResult Create([FromBody] AddTodoRequest body)
        {
            var command = new AddTodo(Guid.NewGuid(), body.Title);

            messageDispatcher.SendCommand(command);

            return Created(
                $"/todos/{command.Id}",
                new TodoResponse
                {
                    Id = command.Id,
                    Title = command.Title
                });
        }

        /// <summary>
        /// Gets all todos.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TodoResponse>), 200)]
        public IActionResult GetAll()
        {
            var todoItems = todoList.GetAll();

            return Ok(todoItems.Select(ToResponse));
        }

        /// <summary>
        /// Gets todo.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TodoResponse), 200)]
        public IActionResult Get(Guid id)
        {
            var todoItem = todoList.Get(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return Ok(ToResponse(todoItem));
        }

        /// <summary>
        /// Renames a todo.
        /// </summary>
        [HttpPost("{id}/title")]
        [ProducesResponseType(204)]
        public IActionResult Rename(Guid id, [FromBody] RenameTodoRequest body)
        {
            var todoItem = todoList.Get(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            messageDispatcher.SendCommand(new RenameTodo(id, body.NewTitle));

            return NoContent();
        }

        /// <summary>
        /// Marks a todo as completed or incompleted.
        /// </summary>
        [HttpPost("{id}/completed")]
        [ProducesResponseType(204)]
        public IActionResult Rename(Guid id, [FromBody] CompletedTodoRequest body)
        {
            var todoItem = todoList.Get(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            if (body.IsCompleted)
            {
                messageDispatcher.SendCommand(new CompleteTodo(id));
            }
            else
            {
                messageDispatcher.SendCommand(new IncompleteTodo(id));
            }

            return NoContent();
        }

        /// <summary>
        /// Remove a todo.
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        public IActionResult Delete(Guid id)
        {
            var command = new RemoveTodo(id);

            messageDispatcher.SendCommand(command);

            return NoContent();
        }

        private static TodoResponse ToResponse(TodoItem todoItem) =>
            new TodoResponse
            {
                Id = todoItem.Id,
                Title = todoItem.Title,
                IsCompleted = todoItem.IsCompleted
            };
    }
}
