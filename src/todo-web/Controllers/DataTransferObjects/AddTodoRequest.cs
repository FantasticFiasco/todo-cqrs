using System.ComponentModel.DataAnnotations;

namespace Todo.Web.Controllers.DataTransferObjects
{
    public class AddTodoRequest
    {
        /// <summary>
        /// the title
        /// </summary>
        [Required]
        public string Title { get; set; }
    }
}
