using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;
using MicroCRM.Models;

namespace MicroCRM.Models
{
    [Table("Projets", Schema = "dbo")]
    public class ProjectModel
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ProjectId")]        
        public int ProjectId { get; set; }
        [Column("ProjectName")]
        [Required]
        public string ProjectName { get; set; }

        [Column("ProjectDescription")]
        [Required]
        public string ProjectDescription { get; set; }
        [Column("ProjectLocaton")]
        public string ProjectLocation { get; set; }
        [Column("ProjectLoceationCity")]
        public string ProjectLocationCity { get; set; }
        [Column("ProjectLoceationRegion")]
        public string Region { get; set; }

        public bool IsDeleted { get; set; }
        public bool IsArchived { get; set; }

        [ForeignKey("ClientModel")]
        public Guid ClientID { get; set; }

        [ForeignKey("IdentityUser")]
        [Column("Owner")]
        public Guid Id { get; set; }
    }
}
