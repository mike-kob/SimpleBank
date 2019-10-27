using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank_server.Models
{
    [Table("User", Schema = "dbo")]
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
        [Required]
        public DateTime DateBirth { get; set; }
        [Required]
        public DateTime Created { get; set; }
        //not sure if this property is necessary
        [Required]
        public int NumOfCards { get; set; }
    }
}
