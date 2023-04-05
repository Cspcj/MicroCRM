using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicroCRM.Models
{
    public class TaskModel
    {
        [Key]
        public Guid TaskId { get; set; }
        [Required]
        public string Text { get; set; }
        [Required]
        [ForeignKey("Projects")]
        public int ProjectId { get; set; }
        [Required]
        public Guid UserId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsCompleted { get; set; }
    }
}
