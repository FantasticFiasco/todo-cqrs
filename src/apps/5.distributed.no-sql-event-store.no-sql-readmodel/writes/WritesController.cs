using Cqrs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Todo;

namespace Writes
{
    [ApiController]
    public class WritesController : ControllerBase
    {
        private readonly ICommandRelay commandRelay;
        private readonly ILogger<WritesController> logger;

        public WritesController(ICommandRelay commandRelay, ILogger<WritesController> logger)
        {
            this.commandRelay = commandRelay;
            this.logger = logger;
        }

        [HttpPost("add")]
        public IActionResult Add(AddTodo command)
        {
            try
            {
                return SendCommand(command);
            }
            catch (TodoAlreadyExistsException e)
            {
                return BadRequest(e);
            }
        }

        [HttpPost("rename")]
        public IActionResult Rename(RenameTodo command)
        {
            try
            {
                return SendCommand(command);
            }
            catch (TodoDoesNotExistException e)
            {
                return BadRequest(e);
            }
        }

        [HttpPost("complete")]
        public IActionResult Complete(CompleteTodo command)
        {
            try
            {
                return SendCommand(command);
            }
            catch (TodoDoesNotExistException e)
            {
                return BadRequest(e);
            }
        }

        [HttpPost("incomplete")]
        public IActionResult Incomplete(IncompleteTodo command)
        {
            try
            {
                return SendCommand(command);
            }
            catch (TodoDoesNotExistException e)
            {
                return BadRequest(e);
            }
        }

        [HttpPost("remove")]
        public IActionResult Remove(RemoveTodo command)
        {
            try
            {
                return SendCommand(command);
            }
            catch (TodoDoesNotExistException e)
            {
                return BadRequest(e);
            }
        }

        private IActionResult SendCommand<T>(T command)
        {
            if (!ModelState.IsValid)
            {
                logger.LogInformation("Not sending command due to bad request.");

                return BadRequest(ModelState);
            }

            logger.LogInformation("Send {command} to command relay", command);

            commandRelay.SendCommand(command);

            return Ok();
        }
    }
}
