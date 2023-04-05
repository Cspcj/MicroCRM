using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicroCRM.Models
{
    [Table("Clients", Schema = "dbo")]
    public class ClientModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ClientId")]
        public Guid ClientID { get; set; }

        [Column("ClientName")]
        [Required]
        public string ClientName { get; set; }

        [Column("ClientCompany")]
        public string? ClientCompany { get; set; }

        [Column("ClientEmail")]
        [Required]
        public string ClientEmail { get; set; }

        [Column("ClientPhoneNumber")]
        [Required]
        //[MinLength(10, "Phone number minimum length is 10 chars")]
        public string ClientPhoneNumber { get; set; }
        
        [Column("ClientAdress")]
        public string? ClientAdress { get; set; }
        
        [Column("ClientOtherInformation")]
        public string? ClientOtherInformation { get; set; }

        [ForeignKey ("IdentityUser")]
        [Column("Owner")]
        public Guid Id { get; set; }    
    }
}
