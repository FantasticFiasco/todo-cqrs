using System;
using System.ComponentModel.DataAnnotations;

namespace Todo.Web.Controllers.DataTransferObjects
{
    public class TodoResponse
    {
        /// <summary>
        /// the id
        /// </summary>
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        /// the title
        /// </summary>
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// whether todo is completed
        /// </summary>
        [Required]
        public bool IsCompleted { get; set; }
    }
}
