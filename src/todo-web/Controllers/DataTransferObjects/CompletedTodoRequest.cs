using System.ComponentModel.DataAnnotations;

namespace Todo.Web.Controllers.DataTransferObjects
{
    public class CompletedTodoRequest
    {
        /// <summary>
        /// whether todo is completed
        /// </summary>
        [Required]
        public bool IsCompleted { get; set; }
    }
}
