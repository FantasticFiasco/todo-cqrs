using System.ComponentModel.DataAnnotations;

namespace Todo.Web.Controllers.DataTransferObjects
{
    public class RenameTodoRequest
    {
        /// <summary>
        /// the title
        /// </summary>
        [Required]
        public string NewTitle { get; set; }
    }
}
