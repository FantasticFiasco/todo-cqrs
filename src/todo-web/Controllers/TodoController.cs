using System;
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
        public IActionResult Create(TodoRequest request)
        {
            var command = new AddTodo(Guid.NewGuid(), request.Title);

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
        [ProducesResponseType(typeof(TodoResponse[]), 200)]
        public IActionResult GetAll()
        {
            var todoItems = todoList.GetAll();

            return Ok(todoItems.Select(ToResponse));
        }

        /// <summary>
        /// Gets todo.
        /// </summary>
        /// <param name="id">The id</param>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TodoResponse), 200)]
        public IActionResult Get(Guid id)
        {
            var todoItem = todoList.Get(id);

            return Ok(ToResponse(todoItem));
        }

        /// <summary>
        /// Remove a todo.
        /// </summary>
        /// <param name="id">The id</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        public IActionResult Delete(Guid id)
        {
            var command = new RemoveTodo(Guid.NewGuid());

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
