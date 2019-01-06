using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReadModel;

namespace Reads
{
    [ApiController]
    public class ReadsController : ControllerBase
    {
        private readonly ITodoList readModel;
        private readonly ILogger<ReadsController> logger;

        public ReadsController(ITodoList readModel, ILogger<ReadsController> logger)
        {
            this.readModel = readModel;
            this.logger = logger;
        }

        [HttpGet("todo-items")]
        public async Task<IActionResult> GetAll()
        {
            var todoItems = await readModel.GetAllAsync();

            logger.LogInformation("Get {count} todo items", todoItems.Length);

            return Ok(todoItems);
        }

        [HttpGet("todo-items/{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var todoItem = await readModel.GetAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            logger.LogInformation("Get {item}", todoItem);

            return Ok(todoItem);
        }
    }
}
