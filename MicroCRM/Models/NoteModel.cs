using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicroCRM.Models
{
    public class NoteModel
    {
        [Key]
        public Guid NoteId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Note { get; set; }
        [Required]
        [ForeignKey("Projects")]
        public int ProjectId { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
